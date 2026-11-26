const fs=require('node:fs');
const path=require('node:path');
const {root,json,files}=require('./run.cjs');
const metrics=json('metrics.json');
const held=metrics.holdout;
const keys=['A','B','C'];
const rows=keys.map(key=>{
  const c=held.conditions[key];
  return '| '+[key,c.confirmedF2+'/'+c.valid,c.newF2,c.propagatedF2,c.amplifiedF2,c.factualReviewed,c.factualWithIncompleteReview,c.unresolved].join(' | ')+' |';
});
const cohorts=['stress','archived','fresh'].map(key=>'| '+[key,...keys.map(condition=>{
  const value=held.cohort[key][condition];
  return value.confirmedF2+'/'+value.valid+' ('+value.newF2+' new)';
})].join(' | ')+' |');
const reviews={};
for(const file of files('reviews').filter(file=>file.endsWith('/attempt.json'))) {
  const attempt=json(file);
  const key=file.split('/').slice(1,3).join('/')+' pass '+attempt.pass;
  const summary=reviews[key]??={attempted:0,strictParserRejected:0,processFailures:0};
  summary.attempted++;
  if(!attempt.valid) summary.strictParserRejected++;
  if(attempt.exit!==0) summary.processFailures++;
}
const failures=Object.values(reviews).reduce((sum,value)=>sum+value.processFailures,0);
const out=[
  '# Factual fidelity and attribution','',
  'Counts are distinct holdout Articles containing at least one finding of that type. Categories can overlap. Amplification is reported separately from claims independently introduced or newly distorted by Compose. Incomplete required review and unresolved semantic findings are separate outcomes. Neither is a fidelity pass.','',
  '| Condition | Confirmed F2 | New Compose F2 | Propagated F2 | Amplified F2 | At least one usable blind factual review | Incomplete required review | Unresolved finding |',
  '|---|---:|---:|---:|---:|---:|---:|---:|',...rows,'',
  '| Holdout cohort | A | B | C |','|---|---:|---:|---:|',...cohorts,'',
  '## Regression cases','',
  '- **Wolf.** Each of the nine Articles refers to eland calves and bulls where the source refers to Yellowstone elk (wapiti). The same substitution appears in the input Narrative. The wording wolvendoding is also ambiguous in isolation. Its use in the hunting paragraph did not establish a second actor reversal. [Audit](source-audit.json)',
  '- **Digital.** Each of the nine Articles carries the alternating-sprint framing. The source describes two weeks within each fortnightly sprint, with research and testing in both. The Narrative already misstates the cadence. Explicit references to different whole sprints sharpen that wording, but the underlying fortnight interval is already assigned by the input. [Source](fixtures/digital/source.txt)',
  '- **Webb.** NASA charts cooling during deployment and into alignment. The Narrative describes a sequential progression. Explicit wording in A and C sharpened a plausible sequential reading into alignment following weeks of cooling. The ledger records the outputs where a material timeline discrepancy was confirmed. [NASA](https://science.nasa.gov/mission/webb/deployment/)',
  '- **EIA.** The fresh production Narrative for this historical subject omitted the absolute quantities and units. The archived source retains them, but the historical quadrillion translation was therefore not directly tested in Compose. The 32%-versus-7% denominator contrast was present. [Narrative](fixtures/eia/narrative.json)',
  '- **FAA.** The wrong surface label was preserved. A repetition 1 and B repetition 3 introduced two-shape headlines even though their bodies listed the three shapes. The source distinguishes three shapes and two categories and the Narrative describes two categories. This is a new material Compose count error in both conditions. [A](outputs/holdout/faa-1-A/result.json) · [B](outputs/holdout/faa-3-B/result.json)',
  '- **Fresh SOFAR.** C repetition 2 widened the scope in the headline to whale calls traveling across the ocean. NOAA supports distant whale-call detection and, separately, possible whole-basin propagation of some low-frequency sounds. The title attributes the latter scope to whale calls specifically. [C](outputs/holdout/sofar-channel-2-C/result.json) · [NOAA](https://oceanservice.noaa.gov/facts/sofar.html)','',
  '## Review evidence','',
  'The [claim ledger](factual-findings.csv) links exact output excerpts to reviewer evidence and Narrative wording. [Independent raw requests and responses](reviews/factual/) retain favorable and unfavorable assessments. [Source-checked determinations](source-audit.json) include supporting evidence and origin, and [supplemental checks](source-checks.md) and [claim rules](source-review-rules.json) explain dispositions for source-coverage and interpretation gaps. Disputed and unresolved labels remain in the raw independent reviews. Primary-source review superseded where the source settled the issue.','',
  'Primary-source confirmation of a few material issues cannot establish release safety. Review coverage is limited by short source digests, output excerpt validation and adjudication uncertainty. The sample already shows factual grounds to reject both candidates in addition to their editorial results.',''
];
fs.writeFileSync(path.join(root,'factual-fidelity-analysis.md'),out.join('\n'));
fs.writeFileSync(path.join(root,'review-operations.json'),JSON.stringify({processFailures:failures,stages:reviews},null,2)+'\n');
