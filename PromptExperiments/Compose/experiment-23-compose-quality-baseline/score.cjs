const fs=require('node:fs');
const assert=require('node:assert/strict');
const {at,json,sha,exists,out,input,reviewDir,verify,tasks,subjects}=require('./run.cjs');
const {readReview}=require('./review-records.cjs');
const mean=a=>a.length?a.reduce((sum,x)=>sum+x,0)/a.length:null;
const median=a=>{const s=[...a].sort((x,y)=>x-y);return s.length?(s[Math.floor(s.length/2)]+s[Math.floor((s.length-1)/2)])/2:null;};
const tally=keys=>Object.groupBy(keys,key=>key);
const counts=keys=>Object.fromEntries(Object.entries(tally(keys)).map(([k,v])=>[k,v.length]));
const rate=(n,d)=>({n,d,percent:d?100*n/d:null});
const weights={article:[20,25,20,15,10,10],linkedin:[25,20,20,15,15,5]};
const key=t=>t.folder+'/'+t.id;
const available=f=>exists(f)?json(f):null;
function get(t) {
  const dir=out(t);
  const generation=available(dir+'/attempt.json');
  if(generation){
    assert.equal(generation.inputSha256,sha(input(t)));
    if(generation.outputSha256)assert.equal(generation.outputSha256,sha(dir+'/result.json'));
    if(generation.valid)assert.ok(json(dir+'/result.json').content.trim());
  }
  for(const [type,pass] of [['editorial',1],['factual',1],['factual',2]]){
    const d=reviewDir(t,type,pass);
    const a=available(d+'/attempt.json');
    if(!a)continue;
    assert.equal(a.requestSha256,sha(d+'/request.txt'));
    assert.equal(a.responseSha256,sha(d+'/stdout.log'));
  }
  const editorial=readReview(t,'editorial',1);
  const primary=readReview(t,'factual',1);
  const secondary=readReview(t,'factual',2);
  const scores=editorial?.usable?editorial.data.scores:null;
  const weighted=scores?scores.reduce((n,s,i)=>n+s*weights[t.folder][i],0)/100:null;
  return {...t,generation,editorial,primary,secondary,scores,weighted,
    accepted:scores?weighted>=3.75&&scores.every(s=>s>=3):null};
}
function edit(rows) {
  const es=rows.filter(r=>r.scores);
  return {reviewed:es.length,mean:mean(es.map(r=>r.weighted)),median:median(es.map(r=>r.weighted)),
    distribution:counts(es.map(r=>r.weighted)),acceptance:rate(es.filter(r=>r.accepted).length,es.length),
    acceptanceOfGenerated:rate(es.filter(r=>r.accepted).length,rows.filter(r=>r.generation?.valid).length),
    criterionMeans:Array.from({length:6},(_,i)=>mean(es.map(r=>r.scores[i]))),
    criticalWeakness:Array.from({length:6},(_,i)=>rate(es.filter(r=>r.scores[i]<3).length,es.length)),
    patterns:counts(es.flatMap(r=>[...new Set(r.editorial.data.defects)])),
    outputs:es.map(r=>({id:r.id,scores:r.scores,weighted:r.weighted,accepted:r.accepted}))};
}
function summarize(rows, findings) {
  const g=rows.filter(r=>r.generation);
  const valid=g.filter(r=>r.generation.valid);
  const ids=new Set(rows.map(key));
  const f=findings.filter(finding=>ids.has(finding.output));
  const confirmed=f.filter(finding=>finding.label==='F2');
  const affected=new Set(confirmed.map(finding=>finding.output));
  const first=valid.filter(r=>r.primary?.usable);
  const fallback=valid.filter(r=>!r.primary?.usable&&r.secondary?.usable);
  const covered=[...first,...fallback];
  const claims=covered.flatMap(r=>(r.primary?.usable?r.primary:r.secondary).data.claims);
  const dual=valid.filter(r=>r.primary?.usable&&r.secondary?.usable);
  const mismatch=dual.filter(r=>['F2','U'].some(label=>
    r.primary.data.claims.some(c=>c.label===label)!==r.secondary.data.claims.some(c=>c.label===label)));
  const subjectsBy=tally(rows.map(r=>r.subject));
  const profiles=Object.fromEntries(Object.keys(subjectsBy).map(subject=>{
    const series=rows.filter(r=>r.subject===subject);
    const scores=series.filter(r=>r.scores).map(r=>r.weighted);
    return [subject,{...edit(series),valid:series.filter(r=>r.generation?.valid).length,
      range:scores.length>1?Math.max(...scores)-Math.min(...scores):null,
      confirmedF2Claims:confirmed.filter(c=>c.output.split('/')[1].startsWith(subject+'-')).length,
      reviewedF2Outputs:series.filter(r=>affected.has(key(r))).length}];
  }));
  const paired=covered.filter(r=>r.scores);
  return {planned:rows.length,attempted:g.length,valid:valid.length,generationFailures:counts(g.filter(r=>!r.generation.valid).map(r=>r.generation.failure)),
    generationSeconds:{sum:g.reduce((sum,r)=>sum+r.generation.seconds,0),mean:mean(g.map(r=>r.generation.seconds)),
      median:median(g.map(r=>r.generation.seconds))},contentWords:mean(valid.map(r=>json(out(r)+'/result.json').content.trim().split(/\s+/).length)),
    editorial:edit(rows),
    reviews:Object.fromEntries(['editorial','primary','secondary'].map(k=>{
      const entries=valid.map(r=>r[k]).filter(Boolean);
      return [k,{attempted:entries.length,usable:entries.filter(r=>r.usable).length,
        strictParserValid:entries.filter(r=>r.valid).length,meanSeconds:mean(entries.map(r=>r.seconds))}];
    })),
    factual:{firstUsable:first.length,fallback:fallback.length,claimCount:claims.length,
      reviewerLabels:{F0:0,F1:0,F2:0,U:0,...counts(claims.map(c=>c.label))},
      confirmedF2Count:confirmed.length,adjudicatedUnresolved:f.filter(c=>c.label==='U').length,
      confirmedF2Outputs:rate(covered.filter(r=>affected.has(key(r))).length,covered.length),
      confirmedF2AcrossGenerated:rate(valid.filter(r=>affected.has(key(r))).length,valid.length),
      confirmedF2Category:counts(confirmed.map(c=>c.category)),confirmedF2Origin:counts(confirmed.map(c=>c.origin)),
      dualReviews:dual.length,outputLevelFlagDisagreement:mismatch.map(r=>r.id),
      outsideNarrative:claims.filter(c=>c.label==='F0'&&c.narrativeRelation==='absent').length,
      eitherF2OrEditorialFailure:rate(paired.filter(r=>affected.has(key(r))||!r.accepted).length,paired.length)},
    bySubject:profiles,
    byRichness:Object.fromEntries(['rich','constrained'].map(cohort=>[cohort,edit(rows.filter(r=>r.cohort===cohort))])),
    seriesWithEditorialFailure:rate(Object.values(profiles).filter(p=>p.reviewed>p.acceptance.n).length,
      Object.values(profiles).filter(p=>p.reviewed>0).length),
    seriesWithConfirmedF2:Object.values(profiles).filter(p=>p.confirmedF2Claims>0).length};
}
function main(){
  verify();
  assert.equal(tasks.length,72);
  assert.equal(new Set(tasks.map(key)).size,72);
  const rows=tasks.map(get);
  const findings=available('reviews/factual/adjudications.json')??[];
  for(const f of findings){
    const row=rows.find(r=>key(r)===f.output);
    assert.ok(row,f.output);
    assert.ok(json(out(row)+'/result.json').content.includes(f.excerpt),f.output+' '+f.excerpt);
    if(f.label==='F2')assert.ok(f.evidence&&f.explanation&&f.origin&&f.materiality,f.output);
  }
  const metrics={subjects:subjects.length,formats:Object.fromEntries(['article','linkedin'].map(folder=>
    [folder,summarize(rows.filter(r=>r.folder===folder),findings)]))};
  const editorial=Object.fromEntries(Object.entries(metrics.formats).map(([format,summary])=>{
    const e=summary.editorial;
    const coverage=e.reviewed>=30&&Object.values(summary.bySubject).every(p=>p.reviewed>0);
    return [format,coverage?(e.acceptanceOfGenerated.percent>=80&&e.criticalWeakness.every(w=>w.percent<20)?'GO':'NO-GO'):'INSUFFICIENT EVIDENCE'];
  }));
  const decision={editorial,unattendedPublication:'NO-GO',
    unattendedReason:'Reliable working verification and withholding were not demonstrated.'};
  const detailed=rows.map(({generation,editorial,primary,secondary,...r})=>({...r,
    generation:generation?.valid??null,primary:primary?.usable??null,secondary:secondary?.usable??null}));
  for(const [file,val] of [['metrics.json',metrics],['decision.json',decision],['output-manifest.json',detailed]])
    fs.writeFileSync(at(file),JSON.stringify(val,null,2)+'\n');
}
if(require.main===module)main();
module.exports={get,key};
