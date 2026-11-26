const fs = require('node:fs');
const {root,json,write,has,hash,output,reviewDir,verify} = require('./run.cjs');
const path = require('node:path');
const weights = [.2,.25,.2,.15,.1,.1];
const keys = ['opening','development','readability','focus','ending','voice'];
const tags = ['repetition','generic_opening','generic_ending','report_voice','thin_substance'];
const mean = values => values.length ? values.reduce((sum,value)=>sum+value,0)/values.length : null;
const pct = (values,p) => values.length ? [...values].sort((a,b)=>a-b)[Math.ceil(p*values.length)-1] : null;
const median = values => values.length ? (pct(values,.5)+[...values].sort((a,b)=>a-b)[Math.floor(values.length/2)])/2 : null;
const variance = values => values.length>1 ? mean(values.map(value=>(value-mean(values))**2)) : null;
const save = (file,text) => fs.writeFileSync(path.join(root,file),text);
const collect = task => {
  const dir = output(task);
  const generation = has(dir+'/attempt.json')?json(dir+'/attempt.json'):null;
  const content = generation?.valid?json(dir+'/result.json').content:null;
  if (content && generation.outputHash!==hash(dir+'/result.json')) throw Error('Invalid output hash '+dir);
  const review = (kind,pass) => {
    const folder = reviewDir(task,kind,pass);
    if(!has(folder+'/attempt.json')) return null;
    const record = json(folder+'/attempt.json');
    if(record.requestHash!==hash(folder+'/request.txt')||record.responseHash!==hash(folder+'/stdout.log')) throw Error('Invalid review hash '+folder);
    if(record.exit!==0||!has(folder+'/parsed.json')) return null;
    const parsed=json(folder+'/parsed.json');
    if(kind==='editorial') {
      const named=['opening_interest','development_substance','readability_flow','focus_coherence','ending_impact','naturalness_originality'];
      const scores=Array.isArray(parsed.scores)?parsed.scores:named.map(key=>parsed.scores?.[key]);
      return scores.length===6&&scores.every(value=>Number.isInteger(value)&&value>=1&&value<=5)&&Array.isArray(parsed.defects)?{...parsed,scores}:null;
    }
    const claims=parsed.claims?.map(claim=>{
      const excerpt=content.includes(claim.excerpt)?claim.excerpt:claim.excerpt?.trim().replace(/^["“]|["”]$/g,'');
      return {...claim,excerpt,label:claim.label?.match(/^(F[012]|U)(?![A-Za-z0-9])/)?.[0],
        narrativeRelation:claim.narrativeRelation?.match(/^(supports|differs|absent|ambiguous)\b/)?.[0],
        origin:claim.origin?.match(/^(none|propagated|new_distortion|amplified|independently_introduced|unknown)\b/)?.[0]};
    });
    return claims?.length&&claims.every(claim=>claim.label&&claim.excerpt&&content.includes(claim.excerpt))?{claims}:null;
  };
  const editorial = review('editorial',1);
  const first = review('factual',1);
  const second = review('factual',2);
  const score = editorial ? Math.round(100 * editorial.scores.reduce((sum,value,index)=>sum+value*weights[index],0))/100 : null;
  const pass = editorial ? score>=3.75&&editorial.scores.every(value=>value>=3) : false;
  const wordCount = content?.match(/[\p{L}\p{N}]+(?:['’.-][\p{L}\p{N}]+)*/gu)?.length??null;
  const findings = [first,second].flatMap((review,index)=>review?.claims.map(claim=>({...claim,pass:index+1}))??[])
    .filter(claim=>claim.label!=='F0'||claim.narrativeRelation!=='supports');
  const needsSecond=task.rep===2||!first||first.claims.some(claim=>['F2','U'].includes(claim.label)||claim.narrativeRelation!=='supports');
  return {...task,generation,content,editorial,first,second,findings,needsSecond,score,pass,wordCount,dir};
};
const normalize = value => value.normalize('NFKC').toLowerCase().replace(/[^\p{L}\p{N}]+/gu,' ').trim();
const overlap = (a,b) => {
  const left=normalize(a),right=normalize(b);
  return left.includes(right)||right.includes(left);
};
const originGroup = claim => ['new_distortion','independently_introduced'].includes(claim.origin)?'new':claim.origin;
function reconcile(row,decisions,stressDecisions) {
  const material=row.findings.filter(claim=>claim.label==='F2');
  const unresolved=row.findings.filter(claim=>claim.label==='U');
  const id=row.phase+'/'+row.subject+'-'+row.rep+'-'+row.condition;
  const audited=(stressDecisions[id]??[]).map(claim=>({...claim,pass:'source-audit'}));
  const sourceChecks=has('source-review-rules.json')?json('source-review-rules.json'):[];
  const dismissed=claim=>sourceChecks.some(rule=>row.subject===rule.subject&&
    rule.labels.includes(claim.label)&&new RegExp(rule.explanationPattern,'i').test(claim.explanation??'')&&
    new RegExp(rule.excerptPattern,'i').test(claim.excerpt));
  const confirmed=[...audited];
  const auditedIssue=claim=>audited.some(result=>overlap(claim.excerpt,result.excerpt)||
    (row.subject==='digital'&&/sprint|periode|week/.test(claim.excerpt)&&/during|within|tested|test|alternat|afwissel|week|cadence/i.test(claim.explanation))||
    (row.subject==='webb'&&/afkoel|koelde|spiegeluitlijn/.test(claim.excerpt)&&claim.category==='chronology')||
    (row.subject==='wolf'&&/eland/.test(claim.excerpt)&&claim.category==='entity'));
  const disputed=[];
  for (const claim of material.filter(claim=>!auditedIssue(claim)&&!dismissed(claim))) {
    const match=material.find(other=>!dismissed(other)&&other.pass!==claim.pass&&originGroup(other)===originGroup(claim)&&overlap(other.excerpt,claim.excerpt));
    if(match) {
      if(claim.pass===1) confirmed.push(claim);
    } else disputed.push(claim);
  }
  const decision=decisions[id]??null;
  if(decision) {
    for(const index of decision.confirm??[]) confirmed.push(disputed[index]);
    if((decision.confirm??[]).some(index=>!disputed[index])) throw Error('Adjudication refers outside flagged records: '+id);
  }
  const unresolvedMaterial=disputed.length>0&&!decision;
  const unresolvedEvidence=unresolved.some(claim=>!auditedIssue(claim)&&!dismissed(claim))&&!decision?.resolveU;
  return {...row,findings:[...row.findings,...audited],id,confirmed,disputed,unresolved:unresolved.filter(claim=>!auditedIssue(claim)&&!dismissed(claim)),decision,
    incomplete:row.needsSecond?!row.second||!row.first:!row.first,
    unresolvedMaterial,unresolvedEvidence};
}
const cases = values => values.filter(row=>row.score!==null);
const grouped = (values,key) => Object.groupBy(values,row=>row[key]);
function summary(rows) {
  const valid=rows.filter(row=>row.generation?.valid);
  const reviewed=cases(rows);
  const scores=reviewed.map(row=>row.score);
  const subjectGroups=Object.values(grouped(reviewed,'subject'));
  const countOrigin=origin=>rows.filter(row=>row.confirmed.some(claim=>originGroup(claim)===origin)).length;
  return {planned:rows.length,attempted:rows.filter(row=>row.generation).length,valid:valid.length,editorialReviewed:reviewed.length,
    acceptanceCount:reviewed.filter(row=>row.pass).length,acceptance:rows.length?reviewed.filter(row=>row.pass).length/rows.length:null,
    mean:mean(scores),median:median(scores),distribution:Object.fromEntries([1,2,3,4].map(lower=>[lower,scores.filter(score=>score>=lower&&(lower===4?score<=5:score<lower+1)).length])),
    criteria:Object.fromEntries(keys.map((key,index)=>[key,mean(reviewed.map(row=>row.editorial.scores[index]))])),
    withinSubjectVariance:mean(subjectGroups.map(group=>variance(group.map(row=>row.score))).filter(value=>value!==null)),
    words:mean(valid.map(row=>row.wordCount)), defects:Object.fromEntries(tags.map(tag=>[tag,reviewed.filter(row=>row.editorial.defects.includes(tag)).length])),
    confirmedF2:rows.filter(row=>row.confirmed.length>0).length,newF2:countOrigin('new'),propagatedF2:countOrigin('propagated'),
    amplifiedF2:countOrigin('amplified'),factualReviewed:valid.filter(row=>row.first||row.second).length,factualWithIncompleteReview:valid.filter(row=>row.incomplete).length,
    unresolved:valid.filter(row=>row.unresolvedMaterial||row.unresolvedEvidence).length,
    medianSeconds:median(valid.map(row=>row.generation.seconds)),p90Seconds:pct(valid.map(row=>row.generation.seconds),.9)};
}
function bootstrap(deltas) {
  if(!deltas.length) return null;
  let state=24024;
  const draws=[];
  for(let i=0;i<10000;i++) {
    const sample=deltas.map(()=>{
      state=(Math.imul(state,1664525)+1013904223)>>>0;
      return deltas[Math.floor(state/4294967296*deltas.length)];
    });
    draws.push(mean(sample));
  }
  return [pct(draws,.025),pct(draws,.975)];
}
const byCondition = rows => Object.fromEntries(Object.entries(grouped(rows,'condition')).map(([key,items])=>[key,summary(items)]));
function compare(rows,condition) {
  const baselines=rows.filter(row=>row.condition==='A');
  const paired=baselines.map(a=>[a,rows.find(b=>b.condition===condition&&b.subject===a.subject&&b.rep===a.rep)]);
  const subjects=Object.entries(Object.groupBy(paired,pair=>pair[0].subject)).map(([subject,pairs])=>{
    const scorePairs=pairs.filter(pair=>pair.every(row=>row.score!==null));
    return {subject,pairs:pairs.map(([a,b])=>({rep:a.rep,a:a.score,b:b.score,aPass:a.pass,bPass:b.pass})),
      meanDelta:mean(scorePairs.map(([a,b])=>b.score-a.score)),acceptanceDelta:mean(pairs.map(([a,b])=>Number(b.pass)-Number(a.pass)))};
  });
  return {subjects,meanInterval:bootstrap(subjects.map(row=>row.meanDelta).filter(value=>value!==null)),
    acceptanceInterval:bootstrap(subjects.map(row=>row.acceptanceDelta)),
    leaveOneOut:subjects.map(row=>({omitted:row.subject,
      meanDelta:mean(subjects.filter(item=>item!==row).map(item=>item.meanDelta).filter(value=>value!==null)),
      acceptanceDelta:mean(subjects.filter(item=>item!==row).map(item=>item.acceptanceDelta))}))};
}
const table = rows=>Object.fromEntries(Object.entries(grouped(rows,'phase')).map(([phase,items])=>[phase,{
  conditions:byCondition(items),
  cohort:Object.fromEntries(Object.entries(grouped(items,'cohort')).map(([key,group])=>[key,byCondition(group)])),
  richness:Object.fromEntries(Object.entries(grouped(items,'richness')).map(([key,group])=>[key,byCondition(group)])),
  paired:Object.fromEntries(['B','C'].map(condition=>[condition,compare(items,condition)]))
}]));
function csv(file,header,rows) {
  const quote = value => '"'+String(value??'').replaceAll('"','""')+'"';
  save(file,header.join(',')+'\n'+rows.map(row=>row.map(quote).join(',')).join('\n')+'\n');
}
verify();
const command=process.argv[2];
const adjudications=has('adjudications.json')?json('adjudications.json'):{};
const stress=has('source-audit.json')?json('source-audit.json'):{}; const rows=json('plan.json').map(collect).map(row=>reconcile(row,adjudications,stress));
const metrics=table(rows);
if(command==='unreviewed') { console.log(rows.filter(row=>row.generation?.valid&&!row.first&&!row.second).map(row=>row.dir).join('\n')); } else if(command==='issues') {
  const phase=process.argv[3];
  console.log(JSON.stringify(rows.filter(row=>row.phase===phase&&row.generation?.valid&&
    (row.disputed.length||row.unresolved.length||row.incomplete)).map(({id,disputed,unresolved,incomplete})=>({id,disputed,unresolved,incomplete})),null,2));
} else if(command==='select') {
  const development=metrics.development.conditions;
  if(Object.values(development).some(row=>row.attempted!==row.planned)) throw Error('Development is incomplete.');
  const rank=['B','C'].sort((a,b)=>{
    const x=development[a],y=development[b];
    return x.newF2+x.amplifiedF2-y.newF2-y.amplifiedF2 ||
      x.unresolved+x.factualWithIncompleteReview+x.planned-x.editorialReviewed-y.unresolved-y.factualWithIncompleteReview-y.planned+y.editorialReviewed ||
      y.acceptance-x.acceptance || y.mean-x.mean || a.localeCompare(b);
  });
  write('development-selection.json',{at:new Date().toISOString(),selected:rank[0],rank,development,
    protocolHash:hash('protocol.md'),freezeHash:hash('freeze.json'),developmentAdjudications:adjudications});
} else {
  save('metrics.json',JSON.stringify(metrics,null,2)+'\n');
  csv('editorial-scores.csv',['phase','subject','rep','condition','blind_id','valid',...keys,'weighted','accepted','words','defects'],
    rows.map(row=>[row.phase,row.subject,row.rep,row.condition,row.blindId,row.generation?.valid,...(row.editorial?.scores??Array(6).fill('')),
      row.score,row.pass,row.wordCount,row.editorial?.defects.join(' ')]));
  csv('factual-findings.csv',['phase','subject','rep','condition','blind_id','review_pass','label','category','excerpt','source_evidence',
    'narrative','narrative_relation','origin','explanation','materiality','confirmed','sources'],
    rows.flatMap(row=>row.findings.map(claim=>[row.phase,row.subject,row.rep,row.condition,row.blindId,claim.pass,claim.label,
      claim.category,claim.excerpt,claim.evidence,claim.narrative,claim.narrativeRelation,claim.origin,claim.explanation,
      claim.materiality,row.confirmed.includes(claim),claim.sources?.join(' ')])));
}






