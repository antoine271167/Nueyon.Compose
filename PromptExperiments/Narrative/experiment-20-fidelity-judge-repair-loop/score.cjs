const fs = require('node:fs');
const path = require('node:path');
const {load,read,exists,save,json,csv,sha,at,stages,verify}=require('./run.cjs');
const tasks=load('dataset.json');
const known=load('archived-material-review.json');
const adjudications=exists('adjudications.json')?load('adjudications.json'):{};
const matches=exists('blocker-matches.json')?load('blocker-matches.json'):{};
const weights=[25,25,25,15,10];
const divide=(a,b)=>b?a/b:null;
const average=values=>divide(values.reduce((a,b)=>a+b,0),values.length);
const accepted=status=>status.startsWith('accepted_');
function distribution(values) {
  return Object.fromEntries([...new Set(values)].sort().map(key=>[key,values.filter(value=>value===key).length]));
}
function percentile(values,p) {
  if (!values.length) return null;
  return [...values].sort((a,b)=>a-b)[Math.ceil(p*values.length)-1];
}
function latency(values) {
  return {count:values.length,mean:average(values),median:percentile(values,.5),p95:percentile(values,.95),max:percentile(values,1)};
}
function result(file) { return exists(file)?load(file):null; }
function narrative(file) {
  const value=result(file);
  return value?.content??value?.Content??null;
}
function assessment(task,version,brief) {
  const dir='reviews/'+task.id+'/'+version;
  const first=result(dir+'/first/result.json');
  const second=result(dir+'/second/result.json');
  const edit=adjudications[task.id+'/'+version];
  let findings=edit?.findings??first?.findings??second?.findings;
  const initialFinding=version===0?known[task.id]:null;
  if (initialFinding) findings=[initialFinding,...(findings??[]).filter(item=>
    item.category!==initialFinding.category && !item.narrativeExcerpt.includes(initialFinding.narrativeExcerpt) &&
    !initialFinding.narrativeExcerpt.includes(item.narrativeExcerpt))];
  const evidence=load(task.input);
  const texts=[read(task.source),...Object.values(evidence.selectedIdea),evidence.synthesis.content??evidence.synthesis.Content];
  for (const finding of findings??[]) {
    if (!brief.includes(finding.narrativeExcerpt)||!texts.some(text=>text.includes(finding.evidenceExcerpt))) throw Error('Invalid adjudicated excerpt '+task.id+'/'+version);
  }
  const fidelity=findings?findings.some(item=>item.severity==='F2')?'F2':findings.length?'F1':'F0':null;
  const editorial=edit?.editorial??first?.editorial??second?.editorial;
  const weighted=editorial?editorial.reduce((sum,n,i)=>sum+n*weights[i],0)/100:null;
  const pass=editorial?weighted>=3.5&&editorial.slice(0,3).every(n=>n>=3):null;
  return {version,fidelity,findings,editorial,weighted,pass,first:first?.fidelity,second:second?.fidelity,
    rationale:edit?.rationale??first?.rationale??second?.rationale??null};
}
function collect() {
  return tasks.map(task=>{
    const loop=result('final-outputs/'+task.id+'/status.json');
    if (!loop) throw Error('Pair has no terminal status '+task.id);
    const versions=[];
    for (const [version,file] of ['baseline/'+task.id+'/result.json',...[1,2].map(i=>'repair-runs/'+task.id+'/'+i+'/result.json')].entries()){
      const brief=narrative(file);
      if (brief) versions.push(assessment(task,version,brief));
    }
    const verdicts=[0,1,2].map(i=>result('judge-runs/'+task.id+'/'+i+'/result.json'));
    const initial=versions[0];
    const last=versions.at(-1);
    const initialF2=initial?.findings?.filter(f=>f.severity==='F2')??[];
    const confirmedMatch=matches[task.id]?.confirmed??false;
    const uncertainMatch=matches[task.id]?.uncertain??false;
    const newF2=versions.slice(1).flatMap(version=>version.findings?.filter(f=>f.severity==='F2' &&
      !(initial?.findings??[]).some(old=>old.severity==='F2'&&old.category===f.category))??[]);
    const introduced=adjudications[task.id+'/new-f2']?.findings??newF2;
    const pathFor=(type,index)=>type==='baseline'?'baseline/'+task.id+'/attempt.json':type+'/'+task.id+'/'+index+'/attempt.json';
    const calls=['baseline','judge-runs','repair-runs'].flatMap(type=>(type==='baseline'?[0]:type==='judge-runs'?[0,1,2]:[1,2])
      .map(index=>({type,index,file:pathFor(type,index),...result(pathFor(type,index))})).filter(call=>call.kind));
    return {...task,loop,versions,initial,last,initialF2,verdicts,confirmedMatch,uncertainMatch,introduced,calls};
  });
}
function metrics(rows) {
  const f2=rows.filter(row=>row.initial?.fidelity==='F2');
  const acceptable=rows.filter(row=>['F0','F1'].includes(row.initial?.fidelity));
  const acceptedRows=rows.filter(row=>accepted(row.loop.status));
  const confirmed=f2.filter(row=>row.confirmedMatch);
  const knownRows=f2.filter(row=>known[row.id]);
  const flagged=rows.filter(row=>row.verdicts[0]?.verdict==='REVISE');
  const falsePositive=acceptable.filter(row=>row.verdicts[0]?.verdict==='REVISE');
  const repaired=f2.filter(row=>['F0','F1'].includes(row.last?.fidelity));
  const pairs=rows.filter(row=>row.initial?.weighted!=null&&row.last?.weighted!=null);
  const operational=rows.flatMap(row=>row.calls);
  return {
    subjects:new Set(rows.map(row=>row.subject)).size, count:rows.length,
    initial:distribution(rows.map(row=>row.initial?.fidelity??'unscored')),
    final:distribution(rows.map(row=>row.last?.fidelity??'unscored')),
    detection:{truePositive:confirmed.length,revise:flagged.length,precision:divide(confirmed.length,flagged.length),
      recall:divide(confirmed.length,f2.length),triageRecall:divide(f2.filter(row=>row.confirmedMatch||row.uncertainMatch).length,f2.length),
      knownDetected:knownRows.filter(row=>row.confirmedMatch).length,knownTriage:divide(knownRows.filter(row=>row.confirmedMatch||row.uncertainMatch).length,knownRows.length),
      knownTotal:knownRows.length,falsePositive:falsePositive.length,falsePositiveRate:divide(falsePositive.length,acceptable.length),
      uncertainty:rows.filter(row=>row.verdicts[0]?.verdict==='UNCERTAIN').length,
      uncertaintyRate:divide(rows.filter(row=>row.verdicts[0]?.verdict==='UNCERTAIN').length,rows.length),
      unmatchedRevise:flagged.filter(row=>!row.confirmedMatch).map(row=>row.id),
      silentEscapes:f2.filter(row=>row.verdicts[0]?.verdict==='GO').map(row=>row.id),
      unmatchedF2:f2.filter(row=>!row.confirmedMatch&&!row.uncertainMatch).map(row=>row.id)},
    repair:{corrected:repaired.map(row=>row.id),correctedRate:divide(repaired.length,f2.length),
      correctedAfterOne:repaired.filter(row=>row.last.version===1).length,
      correctedAfterTwo:repaired.filter(row=>row.last.version===2).length,
      improvedSubjects:[...new Set(repaired.map(row=>row.subject))],
      remainingF2:rows.filter(row=>row.last?.fidelity==='F2').map(row=>row.id),
      introducedF2:rows.filter(row=>row.introduced.length).map(row=>row.id),
      unnecessaryRewrites:falsePositive.filter(row=>row.last?.version>0).map(row=>row.id)},
    editorial:{pairs:pairs.length,before:average(pairs.map(row=>row.initial.weighted)),after:average(pairs.map(row=>row.last.weighted)),
      meanChange:average(pairs.map(row=>row.last.weighted-row.initial.weighted)),
      beforeAcceptable:divide(pairs.filter(row=>row.initial.pass).length,pairs.length),
      afterAcceptable:divide(pairs.filter(row=>row.last.pass).length,pairs.length),
      acceptedPass:divide(acceptedRows.filter(row=>row.last?.pass).length,acceptedRows.length),
      directionChange:average(pairs.map(row=>row.last.editorial[0]-row.initial.editorial[0])),
      developmentChange:average(pairs.map(row=>row.last.editorial[1]-row.initial.editorial[1]))},
    operations:{calls:operational.length,byKind:distribution(operational.map(call=>call.kind)),
      meanJudgeCalls:divide(operational.filter(call=>call.kind==='judge').length,rows.length),
      meanRepairCalls:divide(operational.filter(call=>call.kind==='repair').length,rows.length),
      outcomes:distribution(rows.map(row=>row.loop.status)),
      failures:operational.filter(call=>call.error).map(call=>({file:call.file,error:call.error})),
      latencyByKind:Object.fromEntries(['narrative','judge','repair'].map(kind=>[kind,latency(operational.filter(call=>call.kind===kind).map(call=>call.seconds))])),
      addedLatency:latency(rows.map(row=>row.calls.filter(call=>call.kind!=='narrative').reduce((total,call)=>total+call.seconds,0)))}
  };
}
function main() {
  verify();
  const rows=collect();
  const all=metrics(rows);
  const cohorts=Object.fromEntries([...new Set(rows.map(row=>row.cohort))].map(cohort=>[cohort,metrics(rows.filter(row=>row.cohort===cohort))]));
  const grades=rows.flatMap(row=>row.versions.map(version=>({
    id:row.id,cohort:row.cohort,subject:row.subject,...version,findings:JSON.stringify(version.findings??[]),
    direction:version.editorial?.[0],development:version.editorial?.[1],usability:version.editorial?.[2],
    resolution:version.editorial?.[3],details:version.editorial?.[4]
  })));
  const reviewCalls=fs.readdirSync(at('reviews'),{recursive:true}).filter(file=>file.endsWith('attempt.json'))
    .map(file=>({file:'reviews/'+file.replaceAll('\\','/'),...load('reviews/'+file)}));
  const calls=[...rows.flatMap(row=>row.calls.map(call=>({id:row.id,cohort:row.cohort,subject:row.subject,...call}))),...reviewCalls];
  const silentCategories=distribution(rows.filter(row=>row.verdicts[0]?.verdict==='GO').flatMap(row=>[...new Set(row.initialF2.map(f=>f.category))]));
  const gates={
    knownTriage:all.detection.knownTriage>=.9,
    repair:all.repair.correctedRate>=.8,
    noNewF2:all.repair.introducedF2.length===0,
    falsePositive:all.detection.falsePositiveRate!=null&&all.detection.falsePositiveRate<=.1,
    acceptedEditorial:all.editorial.acceptedPass>=.9,
    editorialMean:all.editorial.meanChange!=null&&all.editorial.meanChange>=-.15,
    noRecurringSilentCategory:Object.values(silentCategories).every(count=>count<2),
    directionDevelopment:all.editorial.directionChange>=-.15&&all.editorial.developmentChange>=-.15,
    artifactsAndExecution:rows.every(row=>row.initial?.fidelity&&row.last?.fidelity&&row.initial?.weighted!=null&&row.last?.weighted!=null)&&!all.operations.failures.length,
    benefitOnTwoSubjects:all.repair.improvedSubjects.length>=2,
    coverage:all.subjects>=12&&all.count>=36&&all.detection.knownTotal>=8&&(all.initial.F0??0)+(all.initial.F1??0)>=20&&cohorts.new.subjects>=8
  };
  const reason=gates.noRecurringSilentCategory&&gates.noNewF2&&gates.falsePositive&&gates.acceptedEditorial&&
    gates.editorialMean&&gates.directionDevelopment&&all.repair.corrected.length?
    Object.values(gates).every(Boolean)?'GO':'CONDITIONAL GO':'NO-GO';
  save('fidelity-scores.csv',csv(['id','cohort','subject','version','fidelity','first','second','findings'],grades));
  save('editorial-scores.csv',csv(['id','cohort','subject','version','direction','development','usability','resolution','details','weighted','pass','rationale'],grades));
  save('run-manifest.csv',csv(['id','cohort','subject','type','index','kind','file','started','seconds','exitCode','signal','error','sha256'],
    calls.map(call=>({...call,sha256:sha(call.file)}))));
  json('metrics.json',{all,cohorts,silentCategories,reviewCalls:{count:reviewCalls.length,failures:reviewCalls.filter(call=>call.error)},totalModelInvocations:calls.length});
  json('decision.json',{decision:reason,gates,silentCategories,action:reason==='GO'?'Separate production integration decision':'Keep the Judge experimental'});
  json('paired-results.json',rows.map(({id,subject,cohort,loop,initial,last,confirmedMatch,uncertainMatch,introduced})=>({id,subject,cohort,loop,initial,last,confirmedMatch,uncertainMatch,introduced})));
}
if (require.main===module) main();
module.exports={collect,metrics};
