const fs=require('node:fs');
process.chdir(__dirname);
const read=p=>fs.readFileSync(p,'utf8').replace(/^\uFEFF/,'');
const load=p=>JSON.parse(read(p));
const optional=p=>fs.existsSync(p)?load(p):null;
const save=(p,v)=>fs.writeFileSync(p,JSON.stringify(v,null,2)+'\n');
const tasks=load('dataset.json').filter(t=>t.split==='evaluation');
const truth=load('ground-truth.json');
const reviewed=load('adjudication.json');
const precision=(n,d)=>d?n/d:null;
const csv=(headers,rows)=>[headers,...rows].map(row=>row.map(c=>'"'+String(c??'').replaceAll('"','""')+'"').join(',')).join('\n')+'\n';
const ledger=optional('usage-ledger.json')??[];
const oldFS='../experiment-20-fidelity-judge-repair-loop/editorial-scores.csv';
const oldEditorial=new Map(read(oldFS).trim().split(/\r?\n/).slice(1).map(l=>l.slice(1,-1).split('","')).filter(r=>r[3]==='0').map(r=>[r[0],r.slice(4,9).map(Number)]));
const rows=tasks.map(t=>{
 const id=t.id;
 const reference=truth[id];
 const a=reviewed[id];
 if(!a)throw Error('Missing source review for '+id);
 const v=optional('judge-runs/'+id+'/result.json');
 const candidate=load('judge-runs/'+id+'/outcome.json').status;
 const baseline=optional('judge-runs/'+id+'/experiment20/result.json');
 const claims=optional('claim-extractions/'+id+'/result.json')?.claims??[];
 const secondReview=optional('evaluation/reviews/'+id+'/result.json');
 const editorial=oldEditorial.get(id)??secondReview?.editorial??null;
 return {id,subject:t.subject,cohort:t.cohort,label:reference.label,category:reference.findings[0]?.category,
   candidate,baseline:baseline?.verdict??'technical_failure',a,v,claims,editorial,reviewLabel:secondReview?.fidelity};
});
const positives=rows.filter(r=>r.label==='F2');
const controls=rows.filter(r=>r.label!=='F2');
const stages={extraction:'claim-extractions/',comparison:'evidence-alignments/',baseline:'judge-runs/'};
const calls={};
for(const [key,dir] of Object.entries(stages)){
 const all=fs.readdirSync(dir).map(id=>{
  const p=dir+id+(key==='baseline'?'/experiment20':'');
  const a=optional(p+'/attempt.json');
  return a&&{...a,p,usage:ledger.find(x=>x.path===p)};
 }).filter(Boolean);
 const summarize=rs=>{
  const metered=rs.filter(x=>Number.isFinite(x.usage?.totalNanoAiu));
  return {calls:rs.length,valid:rs.filter(x=>!x.failure).length,transport:rs.filter(x=>x.failure==='transport').length,invalidJson:rs.filter(x=>x.failure?.startsWith('invalid-json')).length,invalidContract:rs.filter(x=>x.failure?.startsWith('invalid-contract')).length,
   totalSeconds:rs.reduce((s,x)=>s+x.seconds,0),meanSeconds:precision(rs.reduce((s,x)=>s+x.seconds,0),rs.length),
   metered:metered.length,aiCredits:metered.length?metered.reduce((s,x)=>s+x.usage.totalNanoAiu,0)/1e9:null};
 };
 calls[key]={evaluation:summarize(all.filter(x=>tasks.some(t=>x.p.split('/')[1]===t.id))),development:summarize(all.filter(x=>!tasks.some(t=>x.p.split('/')[1]===t.id)))};
}
function detection(arm){
 const f=arm==='candidate'?'candidateMatch':'baselineMatch';
 const matched=positives.filter(r=>r.a[f]==='confirmed');
 const unresolved=positives.filter(r=>r.a[f]==='unresolved');
 const revise=rows.filter(r=>r[arm]==='REVISE');
 const fp=controls.filter(r=>r[arm]==='REVISE');
 const blockerCount=rows.reduce((s,r)=>s+(arm==='candidate'?(r.v?.blockers.length??0):(optional('judge-runs/'+r.id+'/experiment20/result.json')?.blockers.filter(b=>b.status==='confirmed').length??0)),0);
 const grounded=rows.reduce((s,r)=>s+(arm==='candidate'?r.a.supportedBlockers:r.a.supportedBaselineBlockers),0);
 return {confirmed:matched.map(r=>r.id),unresolved:unresolved.map(r=>r.id),materialRecall:precision(matched.length,positives.length),
  briefPrecision:precision(matched.length,revise.length),blockerCount,supportedBlockers:grounded,materialPrecision:precision(grounded,blockerCount),
  triageRecall:precision(matched.length+unresolved.length,positives.length),silentEscapes:positives.filter(r=>r[arm]==='GO').map(r=>r.id),
  silentEscapeRate:precision(positives.filter(r=>r[arm]==='GO').length,positives.length),falsePositiveRevise:fp.map(r=>r.id),falsePositiveReviseRate:precision(fp.length,controls.length),
  uncertain:rows.filter(r=>r[arm]==='UNCERTAIN').length,uncertainRate:precision(rows.filter(r=>r[arm]==='UNCERTAIN').length,rows.length),
  statusCounts:Object.fromEntries(['GO','REVISE','UNCERTAIN','technical_failure'].map(s=>[s,rows.filter(r=>r[arm]===s).length]))};
}
const weights=[.25,.25,.25,.15,.10];
const scored=rows.filter(r=>r.editorial).map(r=>{
 const weighted=r.editorial.reduce((s,n,i)=>s+n*weights[i],0);
 return {...r,weighted,accepted:weighted>=3.5&&r.editorial.slice(0,3).every(n=>n>=3)};
});
const generation=tasks.filter(t=>t.cohort==='new-production').map(t=>load('evaluation/'+t.subject+'/generation.json'));
const candidate=detection('candidate');
const baseline=detection('baseline');
const cats=Object.keys(load('judge-schema.json').$defs.extraction.properties.claims.items.properties).length;
const categories=load('judge-schema.json').$defs.extraction.properties.claims.items.properties.category.enum;
const align=positives.filter(r=>typeof r.a.evidenceLocated==='boolean');
const metrics={evaluationBriefs:rows.length,evaluationSubjects:new Set(rows.map(r=>r.subject)).size,positiveBriefs:positives.length,
 positiveSubjects:new Set(positives.map(r=>r.subject)).size,controlBriefs:controls.length,candidate,baseline,
 targetClaimCoverage:{covered:positives.filter(r=>r.a.extracted===true).length,total:positives.length},
 targetEvidenceAlignment:{correct:align.filter(r=>r.a.evidenceLocated).length,assessable:align.length},
 categories:Object.fromEntries(categories.map(c=>[c,{extracted:rows.reduce((s,r)=>s+r.claims.filter(x=>x.category===c).length,0),targets:positives.filter(r=>r.category===c).length,confirmed:positives.filter(r=>r.category===c&&r.a.candidateMatch==='confirmed').length}])),
 calls,generation:{calls:generation.length,totalSeconds:generation.reduce((s,g)=>s+g.seconds,0),cost:null},aggregation:{modelCalls:0},repair:{executed:false,reverificationCalls:0},
 editorial:{scored:scored.length,accepted:scored.filter(r=>r.accepted).length,mean:precision(scored.reduce((s,r)=>s+r.weighted,0),scored.length)}};
save('metrics.json',metrics);
fs.writeFileSync('fidelity-scores.csv',csv(['id','subject','reference','secondReview','candidate','candidateMatch','experiment20','experiment20Match','sourceReview'],
 rows.map(r=>[r.id,r.subject,r.label,r.reviewLabel,r.candidate,r.a.candidateMatch,r.baseline,r.a.baselineMatch,r.a.note])));
fs.writeFileSync('editorial-scores.csv',csv(['id','direction','development','usability','resolution','details','weighted','accepted','basis'],
 scored.map(r=>[r.id,...r.editorial,r.weighted.toFixed(2),r.accepted,r.cohort.startsWith('new-')?'fresh independent Luna session':'Experiment 20 version 0 exact input'])));
console.log(JSON.stringify(metrics,null,2));

