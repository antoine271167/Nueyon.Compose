const fs = require('node:fs');
const {createHash} = require('node:crypto');
const {verify} = require('./run.cjs');
process.chdir(__dirname);
verify();
const load = p=>JSON.parse(fs.readFileSync(p,'utf8'));
const cases = load('dataset.json');
const truth = load('ground-truth.json');
const conditions = ['baseline','deterministic','hybrid'];
const rows = [];
const inspect = process.argv[2]==='inspect';
const matches = inspect?{}:load('matches.json');
const present = p=>fs.existsSync(p)?load(p):null;
const confirmed = f=>f.status==='confirmed'||f.verdict==='CONTRADICTED';
const risk = f=>f.status==='uncertain'||f.verdict==='INSUFFICIENT_EVIDENCE';
const scopes = new Map();
for(const test of cases){
 const expected = truth[test.id];
 for(const condition of conditions){
  for(const attempt of test.repeat&&condition!=='deterministic'?[1,2]:[1]){
   const slot = [condition,test.id,attempt].join('/');
   const result = present(slot+'/outcome.json');
   if(!result) {
    if(inspect) continue;
    throw Error('Missing planned outcome '+slot);
   }
   const blockers = result.findings.filter(confirmed).length;
   const reviewed = result.findings.filter(risk).length;
   if(inspect){
    if(blockers||reviewed||expected.label==='F2'){
     console.log(slot,result.status,expected.label,JSON.stringify(result.findings.filter(f=>confirmed(f)||risk(f))));
    }
    continue;
   }
   const annotated = matches[slot]??[];
   if(annotated.some(i=>!result.findings[i]||!(confirmed(result.findings[i])||risk(result.findings[i]))||expected.label!=='F2'))
    throw Error('Invalid issue match '+slot);
   const matchedBlockers = annotated.filter(i=>confirmed(result.findings[i])).length;
   const matchedRisks = annotated.filter(i=>risk(result.findings[i])).length;
   const meta = present(slot+'/attempt.json');
   const deterministic = present('deterministic/'+test.id+'/1/outcome.json');
   const target = expected.label==='F2' ? expected.findings.find(f=>f.severity==='F2'||f.severity===undefined) : null;
   rows.push({condition,id:test.id,subject:test.subject,split:test.split,cohort:test.cohort,attempt,
    label:expected.label,category:target?.category??'',status:result.status,blockers,matchedBlockers,matchedRisks,
    targetInCandidates:Boolean(target&&deterministic?.candidates.some(c=>c.includes(target.narrativeExcerpt))),
    candidates:deterministic?.candidates.length??0,modelCalls:Number(Boolean(meta)),seconds:meta?.seconds??0,
    deterministicMs:result.milliseconds??0,failure:meta?.failure??'',started:meta?.started??''});
  }
 }
}
if(inspect) process.exit(0);

function summary(group){
 const positives = group.filter(row=>row.label==='F2');
 const acceptable = group.filter(row=>row.label!=='F2');
 const count = (ar,p)=>ar.filter(p).length;
 const sum = (key)=>group.reduce((total,row)=>total+row[key],0);
 const tp = count(positives,r=>r.matchedBlockers);
 const fp = count(acceptable,r=>r.status==='FAIL');
 const riskOnly = count(positives,r=>r.matchedRisks&&!r.matchedBlockers);
 const fraction = (x,y)=>y?x/y:null;
 const review = count(group,r=>r.status==='REVIEW');
 const technical = count(group,r=>r.status==='TECHNICAL_FAILURE');
 return {briefs:group.length,subjects:new Set(group.map(r=>r.subject)).size,positives:positives.length,acceptable:acceptable.length,
  tp,fp,tn:acceptable.length-fp,fn:positives.length-tp,
  blockerCount:sum('blockers'),matchedBlockers:sum('matchedBlockers'),riskOnly,
  recall:fraction(tp,positives.length),blockerPrecision:fraction(sum('matchedBlockers'),sum('blockers')),
  briefPrecision:fraction(tp,count(group,r=>r.status==='FAIL')),
  unmatchedFailOnF2:count(positives,r=>r.status==='FAIL'&&!r.matchedBlockers),
  unrelatedReviewOnF2:count(positives,r=>r.status==='REVIEW'&&!r.matchedRisks),
  triageRecall:fraction(tp+riskOnly,positives.length),silentEscape:count(positives,r=>r.status==='PASS'),
  silentEscapeRate:fraction(count(positives,r=>r.status==='PASS'),positives.length),
  falseFailRate:fraction(fp,acceptable.length),review,reviewRate:fraction(review,group.length),
  acceptableReview:count(acceptable,r=>r.status==='REVIEW'),technical,technicalRate:fraction(technical,group.length),
  evidenceBackedDetectionRate:fraction(tp,positives.length),
  targetInCandidates:count(positives,r=>r.targetInCandidates),briefsWithCandidates:count(group,r=>r.candidates),
  modelCalls:sum('modelCalls'),callsPerBrief:fraction(sum('modelCalls'),group.length),seconds:sum('seconds'),
  meanCallSeconds:fraction(sum('seconds'),sum('modelCalls')),deterministicMs:sum('deterministicMs'),
  invalidJson:count(group,r=>r.failure.startsWith('invalid-json')),invalidContract:count(group,r=>r.failure.startsWith('invalid-contract')),
  transport:count(group,r=>r.failure==='transport')};
}
const primary=rows.filter(r=>r.attempt===1&&r.split==='evaluation');
scopes.set('evaluation',primary);
for(const field of ['cohort','category','subject']){
 for(const value of new Set(primary.map(r=>r[field]).filter(Boolean))){
  scopes.set(field+'/'+value,primary.filter(r=>r[field]===value));
 }
}
scopes.set('development',rows.filter(r=>r.split==='development'));
scopes.set('naturallyOccurring',primary.filter(r=>r.cohort!=='new-mutation'));
scopes.set('repeat2',rows.filter(r=>r.attempt===2));
const metrics=Object.fromEntries([...scopes].map(([key,group])=>[key,Object.fromEntries(conditions.map(c=>[c,summary(group.filter(r=>r.condition===c))]))]));
metrics.repeatAgreement={};
for(const condition of ['baseline','hybrid']){
 const repeat=rows.filter(r=>r.attempt===2&&r.condition===condition);
 const paired=repeat.map(r=>[r,primary.find(x=>x.condition===condition&&x.id===r.id)]);
 metrics.repeatAgreement[condition]={paired:paired.length,sameStatus:paired.filter(([a,b])=>a.status===b.status).length,
  sameConfirmedDetection:paired.filter(([a,b])=>Boolean(a.matchedBlockers)===Boolean(b.matchedBlockers)).length};
}
const reviews=[];
for(let i=1;i<=10;i++){
 const records=present('runs/reviews/'+i+'/result.json')??[];
 for(const row of records){
  const test=cases.find(t=>createHash('sha256').update('fidelity-review/'+t.id).digest('hex').slice(0,12)===row.id);
  const total=row.editorial.reduce((sum,grade,j)=>sum+grade*[25,25,25,15,10][j],0);
  reviews.push({...row,caseId:test.id,cohort:test.cohort,reference:truth[test.id].label,
   total,passes:total>=350&&row.editorial.slice(0,3).every(grade=>grade>=3)});
 }
}
metrics.independentReview={
 reviewed:reviews.length,agreement:reviews.filter(r=>r.fidelity===r.reference).length,
 challengeF2:reviews.filter(r=>r.cohort==='new-mutation'&&r.fidelity==='F2').length,
 naturalF2:reviews.filter(r=>r.cohort==='new-natural'&&r.fidelity==='F2').length,
 naturalEditorialPass:reviews.filter(r=>r.cohort==='new-natural'&&r.passes).length,
 naturalEditorialReviewed:reviews.filter(r=>r.cohort==='new-natural').length};
const active=rows.filter(r=>r.split==='evaluation'&&r.started);
const generation=cases.filter(t=>t.cohort==='new-natural').map(t=>load(t.initial.replace(/natural-(\d+)\.json$/,'generation-$1.json')));
const reviewAttempts=Array.from({length:10},(_,i)=>load('runs/reviews/'+(i+1)+'/attempt.json'));
metrics.operational={
 evaluationWallSeconds:(Math.max(...active.map(r=>Date.parse(r.started)+r.seconds*1000))-Math.min(...active.map(r=>Date.parse(r.started))))/1000,
 narrativePreparation:{calls:generation.length,aggregateSeconds:generation.reduce((sum,r)=>sum+r.seconds,0)},
 independentReview:{calls:reviewAttempts.length,aggregateSeconds:reviewAttempts.reduce((sum,r)=>sum+r.seconds,0)},
 tokenUsage:null,
 monetaryCost:null};
const csv=x=>'"'+String(x).replaceAll('"','""')+'"';
fs.writeFileSync('scores.csv',Object.keys(rows[0]).join(',')+'\n'+rows.map(r=>Object.values(r).map(csv).join(',')).join('\n')+'\n');
fs.writeFileSync('metrics.json',JSON.stringify(metrics,null,2)+'\n');
fs.writeFileSync('runs/editorial-review.json',JSON.stringify(reviews,null,2)+'\n');
