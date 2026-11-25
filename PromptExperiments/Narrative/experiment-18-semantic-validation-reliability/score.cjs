const fs = require('node:fs');
const path = require('node:path');
const local = name => path.join(__dirname,name);
const obj = name => JSON.parse(fs.readFileSync(local(name),'utf8'));
const save = (name,data) => fs.writeFileSync(local(name),JSON.stringify(data,null,2)+'\n');
const text = (name,data) => fs.writeFileSync(local(name),data+'\n');
const ground = obj('ground-truth.json');
const conditions = ['S','S+Source'];
const statuses = ['PASS','REVIEW','FAIL'];
const ratio = (numerator,denominator) => ({numerator,denominator,value:denominator?numerator/denominator:null});
const csv = (name,rows) => {
  if (!rows.length) return;
  const keys = Object.keys(rows[0]);
  text(name,[keys.join(','),...rows.map(row=>keys.map(key=>'"'+String(row[key]??'').replaceAll('"','""')+'"').join(','))].join('\n'));
};
const leaves = value => typeof value==='string'?[value]:value&&typeof value==='object'?Object.values(value).flatMap(leaves):[];
const attempts = [];
const findings = [];
for (const item of ground) for (const condition of conditions) for (const repetition of [1,2]) {
  const dir = 'runs/'+item.id+'/'+condition+'/'+repetition+'/';
  if (!fs.existsSync(local(dir+'attempt.json'))) continue;
  const meta = obj(dir+'attempt.json');
  const input = obj(dir+'input.json');
  attempts.push({...item,...meta,dir});
  if (!meta.valid) continue;
  const result = obj(dir+'response.txt');
  const evidence = leaves([input.selectedIdea,input.synthesis,input.source]);
  result.findings.forEach((finding,index)=>findings.push({
    key:[item.id,condition,repetition,index+1].join('/'),id:item.id,condition,repetition,label:item.label,
    narrativeQuote:leaves(input.narrative).some(q=>q.includes(finding.narrativeExcerpt)),
    evidenceQuote:!!finding.evidenceExcerpt&&evidence.some(q=>q.includes(finding.evidenceExcerpt)),
    ...finding
  }));
}
if (process.argv.includes('--inspect')) {
  console.log(JSON.stringify({attempts:attempts.length,findings},null,2));
  process.exit(0);
}
const reviews = obj('finding-review.json');
for (const f of findings) {
  if (!Object.hasOwn(reviews,f.key)) throw Error('Missing adjudication '+f.key);
  Object.assign(f,reviews[f.key]);
  f.actionable = f.confirmed && f.narrativeQuote && f.evidenceQuote && f.relevant && f.explained && f.typed && f.corrected;
}
if (Object.keys(reviews).length!==findings.length) throw Error('Unexpected review records');
function summarize(rows) {
  const valid = rows.filter(a=>a.valid);
  const positives = valid.filter(a=>a.label==='F2');
  const acceptable = valid.filter(a=>a.label!=='F2');
  const count = (cohort,status)=>cohort.filter(a=>a.status===status).length;
  const tp = count(positives,'FAIL');
  const fp = count(acceptable,'FAIL');
  const flags = positives.length-count(positives,'PASS');
  const confirmed = findings.filter(f=>f.confirmed&&rows.some(a=>a.id===f.id&&a.condition===f.condition&&a.repetition===f.repetition));
  return {
    attempted:rows.length,valid:valid.length,
    definitivePrecision:ratio(tp,tp+fp),definitiveRecall:ratio(tp,positives.length),
    definitiveFalsePositiveRate:ratio(fp,acceptable.length),definitiveFalseNegativeRate:ratio(positives.length-tp,positives.length),
    triageSensitivity:ratio(flags,positives.length),triageSpecificity:ratio(count(acceptable,'PASS'),acceptable.length),
    falsePositiveFlagRate:ratio(acceptable.length-count(acceptable,'PASS'),acceptable.length),
    silentEscapeRate:ratio(count(positives,'PASS'),positives.length),
    flaggedWorkload:ratio(valid.length-count(valid,'PASS'),valid.length),
    workload:Object.fromEntries(statuses.map(status=>[status,ratio(count(valid,status),valid.length)])),
    acceptableReview:ratio(count(acceptable,'REVIEW'),acceptable.length),
    supportedBlocks:ratio(positives.filter(a=>a.status==='FAIL'&&confirmed.some(f=>f.id===a.id&&f.condition===a.condition&&f.repetition===a.repetition&&f.actionable)).length,valid.length),
    confirmedDefinitiveRecall:ratio(positives.filter(a=>a.status==='FAIL'&&confirmed.some(f=>f.id===a.id&&f.condition===a.condition&&f.repetition===a.repetition&&f.severity==='material')).length,positives.length),
    confirmedTriageRecall:ratio(positives.filter(a=>a.status!=='PASS'&&confirmed.some(f=>f.id===a.id&&f.condition===a.condition&&f.repetition===a.repetition)).length,positives.length),
    actionableEvidence:ratio(confirmed.filter(f=>f.actionable).length,confirmed.length),
    unsupportedFindings:findings.filter(f=>!f.actionable&&rows.some(a=>a.id===f.id&&a.condition===f.condition&&a.repetition===f.repetition)).length
  };
}
const stability = {};
const categories = [...new Set(ground.map(x=>x.category))];
const metrics = {subjects:new Set(ground.map(x=>x.subject)).size,examples:ground.length,planned:192,attempted:attempts.length,valid:attempts.filter(a=>a.valid).length,conditions:{}};
for (const condition of conditions) {
  const rows=attempts.filter(a=>a.condition===condition);
  const pairs=ground.map(item=>{
    const statuses=[1,2].map(rep=>rows.find(a=>a.id===item.id&&a.repetition===rep)?.status??'INVALID');
    return {id:item.id,subject:item.subject,category:item.category,label:item.label,statuses};
  });
  const complete=pairs.filter(p=>!p.statuses.includes('INVALID'));
  const material=complete.filter(p=>p.label==='F2');
  const mixed=material.filter(p=>p.statuses.filter(s=>s==='PASS').length===1);
  const escapes=material.filter(p=>p.statuses.includes('PASS'));
  const recurring=categories.filter(cat=>escapes.some(p=>p.category===cat&&p.statuses.every(s=>s==='PASS'))||new Set(escapes.filter(p=>p.category===cat).map(p=>p.subject)).size>=2);
  const systematicInconsistency=categories.filter(cat=>new Set(mixed.filter(p=>p.category===cat).map(p=>p.subject)).size>=2);
  stability[condition]={
    agreement:ratio(complete.filter(p=>p.statuses[0]===p.statuses[1]).length,complete.length),
    materialDisagreement:ratio(material.filter(p=>p.statuses[0]!==p.statuses[1]).length,material.length),
    materialPassFlagSwitch:ratio(mixed.length,material.length),recurring,systematicInconsistency,
    transitions:Object.fromEntries(statuses.flatMap(a=>statuses.map(b=>[a+'→'+b,complete.filter(p=>p.statuses[0]===a&&p.statuses[1]===b).length]))),
    pairs
  };
  metrics.conditions[condition]={...summarize(rows),
    byCategory:Object.fromEntries(categories.map(c=>[c,summarize(rows.filter(a=>a.category===c))])),
    byRepetition:Object.fromEntries([1,2].map(r=>[r,summarize(rows.filter(a=>a.repetition===r))])),
    stability:stability[condition]};
}
const gates={};
for(const [condition,m] of Object.entries(metrics.conditions)) {
  gates[condition]={
    sensitivity:m.triageSensitivity.value>=.95,escapePattern:!m.stability.recurring.length,
    falseFlags:m.falsePositiveFlagRate.value!==null&&m.falsePositiveFlagRate.value<=.15,
    precision:m.definitivePrecision.value>=.90,evidence:m.actionableEvidence.value>=.90,
    stability:m.stability.agreement.value>=.90&&!m.stability.systematicInconsistency.length,
    coverage:metrics.subjects>=12&&ground.filter(x=>x.label==='F2').length>=24&&ground.filter(x=>x.label!=='F2').length>=24,
    complete:metrics.valid===192
  };
}
const passed=Object.keys(gates).filter(c=>Object.values(gates[c]).every(Boolean));
const potential=Object.keys(gates).filter(c=>metrics.conditions[c].triageSensitivity.value>=.90&&gates[c].falseFlags&&gates[c].precision&&gates[c].evidence&&gates[c].escapePattern);
const decision=passed.length?'GO':potential.length?'CONDITIONAL GO':'NO-GO';
save('metrics.json',metrics);
save('decision.json',{decision,gates,qualifyingConditions:passed,conditionalConditions:potential});
csv('run-manifest.csv',attempts.sort((a,b)=>a.seq-b.seq).map(({seq,id,condition,repetition,startedAt,seconds,exitCode,valid,status,dir,validationError})=>({seq,id,condition,repetition,startedAt,seconds,exitCode,valid,status,dir,validationError})));
csv('scores.csv',attempts.map(a=>{
  const related=findings.filter(f=>f.id===a.id&&f.condition===a.condition&&f.repetition===a.repetition);
  return {id:a.id,condition:a.condition,repetition:a.repetition,subject:a.subject,category:a.category,label:a.label,status:a.status??'INVALID',definitive:a.valid&&a.status==='FAIL',flagged:a.valid&&a.status!=='PASS',findings:related.length,confirmed:related.filter(f=>f.confirmed).length,actionable:related.filter(f=>f.actionable).length};
}));
csv('finding-scores.csv',findings);
console.log(decision,metrics.attempted+' attempts',metrics.valid+' valid');

