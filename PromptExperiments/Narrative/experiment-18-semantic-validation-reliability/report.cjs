const fs=require('node:fs');
const path=require('node:path');
const put=(name,data)=>fs.writeFileSync(path.join(__dirname,name),data.join('\n')+'\n');
const read=name=>JSON.parse(fs.readFileSync(path.join(__dirname,name),'utf8'));
const ground=read('ground-truth.json');
const metrics=read('metrics.json');
const decision=read('decision.json');
const audit=read('finding-review.json');
const names=Object.keys(metrics.conditions);
const outcome=(r)=>r.value===null?'undefined':r.numerator+'/'+r.denominator+' ('+(100*r.value).toFixed(1)+'%)';
const results=names.map(n=>metrics.conditions[n]);
const status=(id,name,r)=>{
  const file=path.join(__dirname,'runs',id,name,String(r),'attempt.json');
  return fs.existsSync(file)?JSON.parse(fs.readFileSync(file,'utf8')).status??'INVALID':'MISSING';
};
const consistency=['# Repeatability','','Comparisons are within the same fixture and condition. Repetitions do not add independent subjects.',''];
for(const [name,m] of Object.entries(metrics.conditions)){
  consistency.push('## '+name,'','- Exact agreement '+outcome(m.stability.agreement),
    '- F2 status disagreement '+outcome(m.stability.materialDisagreement),
    '- F2 PASS/flag switches '+outcome(m.stability.materialPassFlagSwitch),
    '- Recurring escape categories '+(m.stability.recurring.join(', ')||'none'),
    '- Systematic inconsistency categories '+(m.stability.systematicInconsistency.join(', ')||'none'),
    '- Nonzero transitions '+Object.entries(m.stability.transitions).filter(([,n])=>n).map(([k,n])=>k+' '+n).join(', '),'',
    ...Object.entries(m.byRepetition).map(([r,v])=>'- Repetition '+r+' manual REVIEW '+outcome(v.workload.REVIEW)+', FAIL '+outcome(v.workload.FAIL)+', total flags '+outcome(v.flaggedWorkload)),'',
    ...m.stability.pairs.filter(p=>p.statuses[0]!==p.statuses[1]).map(p=>'- '+p.id+' '+p.subject+' ('+p.label+') '+p.statuses.join(' → ')),'');
}
put('stability-analysis.md',consistency);
const errors=['# Escapes and false flags','','Outcomes are listed in repetition order. A PASS has no finding, so the stated interpretation is inferential.',''];
for(const item of ground){
  const outcomes=names.map(n=>[1,2].map(r=>status(item.id,n,r)));
  const escape=item.label==='F2'&&outcomes.flat().includes('PASS');
  const falseFlag=item.label!=='F2'&&outcomes.flat().some(s=>['REVIEW','FAIL'].includes(s));
  if(!escape&&!falseFlag) continue;
  errors.push('## '+item.id+' '+item.subject+' / '+item.category,'',
    '- Narrative: '+item.narrativeExcerpt,
    '- Source: '+item.sourceExcerpt.replaceAll('\n',' / '),
    '- S '+outcomes[0].join(' / ')+' and S+Source '+outcomes[1].join(' / ')+'.',
    '- '+item.explanation);
  if(escape){
    const m=outcomes.map(values=>values.filter(s=>s==='PASS').length);
    errors.push('- Correction direction: '+item.expectedCorrection,
      '- Evidence: '+(item.id==='N28'?'S supports challenging the unsupported percentage but omits the 1.70 odds ratio. Source supplies that estimate.':'Both conditions supplied the governing fact in the handoff. The source condition also supplied the original wording.'),
      '- Pattern: '+(m.some(x=>x===2)?'The miss repeated within a condition.':'The miss depended on repetition, evidence condition, or both.')+' '+(item.category==='identity'?'The model likely missed a referent change.':item.category==='temporal'?'The model likely misread sequence or continuation.':item.category==='actor'?'The model likely missed a change in participant roles.':item.category==='causality'?'The model likely underweighted the difference between what the evidence establishes and what the brief asserts.':'The model likely missed a quantitative denominator, magnitude, or unit discrepancy.'));
  } else {
    for(const [key,review] of Object.entries(audit).filter(([key])=>key.startsWith(item.id+'/'))) {
      const [,condition,repetition,num]=key.split('/');
      const p=read('runs/'+item.id+'/'+condition+'/'+repetition+'/response.txt').findings[Number(num)-1];
      errors.push('- '+key+' '+p.findingType+': '+p.explanation+' '+review.note);
    }
    errors.push('- Adjudication: PASS is supported by the full handoff, so escalation to REVIEW would also be unnecessary.');
  }
  errors.push('');
}
const invalid = [];
for (const item of ground) for (const condition of names) for (const r of [1,2]) {
  const file = 'runs/'+item.id+'/'+condition+'/'+r+'/attempt.json';
  if (!fs.existsSync(path.join(__dirname,file))) invalid.push('- '+item.id+' '+condition+' '+r+' missing call');
  else {
    const m = read(file);
    if (!m.valid) invalid.push('- '+item.id+' '+condition+' repetition '+r+' ('+item.label+') '+m.validationError+'. Raw response and stderr are archived.');
  }
}
if (invalid.length) errors.push('## Invalid or missing attempts','',...invalid,'','Invalid calls are excluded from valid-attempt rates and prevent GO. Transport terminations remain unclassified. N28/S/1 identified the unsupported percentage but paired REVIEW with material severity, which violates the archived schema.','');
put('error-analysis.md',errors);
const fields={
  'Definitive precision':'definitivePrecision','Definitive recall':'definitiveRecall','Definitive false-positive rate':'definitiveFalsePositiveRate',
  'Definitive false-negative rate':'definitiveFalseNegativeRate','Triage sensitivity':'triageSensitivity','Triage specificity':'triageSpecificity',
  'False-positive flag rate':'falsePositiveFlagRate','Silent-escape rate':'silentEscapeRate','Total flagged workload':'flaggedWorkload',
  'Acceptable briefs sent for manual review':'acceptableReview','Evidence-backed blocks':'supportedBlocks','Finding-confirmed FAIL recall':'confirmedDefinitiveRecall','Finding-confirmed triage recall':'confirmedTriageRecall','Actionable evidence among confirmed findings':'actionableEvidence'};
const lines=['# Experiment 18 — Semantic Validation Reliability','',
  '**'+decision.decision+'**. '+(decision.qualifyingConditions.length?'Qualifying condition: '+decision.qualifyingConditions.join(', ')+'.':'See decision.json for the frozen gate outcomes.'),'',
  'The primary set contains '+metrics.examples+' Dutch synthetic briefs on '+metrics.subjects+' independent subjects, with 24 matched F2 mutations and 24 acceptable controls. GPT-6 Luna completed '+metrics.valid+' valid responses from '+metrics.attempted+' attempts. The validator prompt was byte-identical to Experiment 17.','',
  '| Metric | S | S+Source |','|---|---:|---:|',
  ...Object.entries(fields).map(([label,k])=>'| '+label+' | '+results.map(m=>outcome(m[k])).join(' | ')+' |'),
  ...['PASS','REVIEW','FAIL'].map(s=>'| '+s+' workload | '+results.map(m=>outcome(m.workload[s])).join(' | ')+' |'),
  '| Repeat agreement | '+results.map(m=>outcome(m.stability.agreement)).join(' | ')+' |','',
  'The denominator is valid attempts in the relevant class or set. The attempts share evidence within subjects and fixtures. The approximate 95% one-sided zero-event upper bound is 22.1% across 12 independent subjects, even under binomial random-sampling assumptions. This purposive cohort does not satisfy those assumptions. The per-attempt percentages are descriptive.','',
  'All planned F2 attempts per condition: '+results.map((m,i)=>names[i]+' '+m.triageSensitivity.numerator+'/48 flagged if all invalid or missing positive attempts are counted as unflagged for sensitivity analysis').join('; ')+'. Invalid responses are not observed PASSes.','',
  '## Hypotheses','',
  '- H1. REVIEW added '+results.map((m,i)=>names[i]+' '+(m.triageSensitivity.numerator-m.definitiveRecall.numerator)+' F2 detections').join(', ')+'. '+(results.some(m=>m.triageSensitivity.numerator>m.definitiveRecall.numerator)?'Observed triage sensitivity improved where REVIEW surfaced a material issue.':'REVIEW produced no extra F2 detection in this cohort.'),
  '- H2. S and S+Source had '+results.map(m=>m.unsupportedFindings).join(' versus ')+' non-actionable or unconfirmed findings, '+results.map(m=>m.falsePositiveFlagRate.numerator).join(' versus ')+' acceptable flags, and '+results.map(m=>m.silentEscapeRate.numerator).join(' versus ')+' escapes. '+(results[1].unsupportedFindings<results[0].unsupportedFindings&&results[1].falsePositiveFlagRate.numerator<results[0].falsePositiveFlagRate.numerator&&results[1].silentEscapeRate.numerator<=results[0].silentEscapeRate.numerator?'The paired observations support the direction of H2.':'The joint H2 directional criterion was not satisfied.'),
  '- H3. F2 detection by category (S / S+Source): '+Object.keys(results[0].byCategory).map(k=>k+' '+results.map(m=>outcome(m.byCategory[k].triageSensitivity)).join(' / ')).join('; ')+'. See error-analysis.md for misses and stability-analysis.md for cross-run disagreement.',
  '- H4. Manual REVIEW took '+results.map(m=>outcome(m.workload.REVIEW)).join(' / ')+' of valid attempts. Combined FAIL and REVIEW also includes automatic blocks. A fair operational evaluation must measure adjudication time and actual Narrative error prevalence, because the deliberate 50% F2 fixture mix changes both workload and precision.','',
  '## Interpretation','',
  'The insomnia prevalence mutation (N28) received '+[status('N28','S',1),status('N28','S',2)].join(' / ')+' with S and '+[status('N28','S+Source',1),status('N28','S+Source',2)].join(' / ')+' with source. Source supplied the odds ratio that the handoff omitted. The OSIRIS-REx ignition mutation (N42) received '+[status('N42','S',1),status('N42','S',2)].join(' / ')+' with S and '+[status('N42','S+Source',1),status('N42','S+Source',2)].join(' / ')+' with source. The successful source attempt quoted the actual ignition timing. The handoff only stated when the spacecraft diverted.','',
  'Experiment 17 had three held-out F2 cases, one attempt per condition and archived briefs that had already served earlier Narrative work. The independent cases here address source overlap and add actor, causality and terminology contrasts. The paired mini-briefs still contain a single focal assertion, Synthesis usually foregrounds the fact, and source excerpts are short and mostly redundant. This tests difficult semantic distinctions in isolation with limited distraction. The fixture author assessed ground truth before any validator call and also performed the finding review. An independent second reviewer and naturally generated multi-claim briefs would broaden the evidence.','',
  'See error-analysis.md for the exact case excerpts and adjudications. For a rollout, REVIEW needs an actual subsequent evidence check. '+(decision.decision==='GO'?'Evaluate the smallest follow-up as a separate task: shadow validation in Narrative with source, evidence-linked FAIL blocks and a human REVIEW queue before any automatic routing. Measure natural prevalence and adjudication first.':'Keep production unchanged. Closely examine any false flags, recurring categories and unstable pairs before a further blinded test on longer naturally generated briefs.')];
put('report.md',lines);

