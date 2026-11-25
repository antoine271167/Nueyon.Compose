const fs=require('node:fs');
process.chdir(__dirname);
const metrics=JSON.parse(fs.readFileSync('metrics.json','utf8'));
const {baseline:a,deterministic:b,hybrid:c}=metrics.evaluation;
const natural=metrics.naturallyOccurring;
const mutation=metrics['cohort/new-mutation'];
const reviews=metrics.independentReview;
const op=metrics.operational;
const counts=(n,d)=>n+'/'+d;
const sec=x=>x.toFixed(2)+' s';
const labels=[['A · Experiment 20',a],['B · Mechanical only',b],['C · Hybrid',c]];
const conditions=['baseline','deterministic','hybrid'];
const line=(label,m)=>'| '+[label,counts(m.tp,m.positives),m.riskOnly,m.blockerCount?counts(m.matchedBlockers,m.blockerCount):'—',m.fp,m.review,m.technical,m.silentEscape].join(' | ')+' |';
const summaries=[['Evaluation',metrics.evaluation],['Historical regressions',metrics['cohort/historical-regression']],['New mutations',mutation]];
const regressionRows=[
 ['Wolf','development',['2ff42e2fa4','1ae0c9278f','4632ab6ce2']],
 ['Webb','development',['0d1f75b51c','2336788e5a']],
 ['EIA','evaluation',['0f2553c79399']],
 ['FAA','evaluation',['a027be447f28']],
 ['Digital','evaluation',['166274757454']]
];
const match=JSON.parse(fs.readFileSync('matches.json','utf8'));
const summary=(condition,ids)=>ids.map(id=>{
 const slot=condition+'/'+id+'/1';
 const outcome=JSON.parse(fs.readFileSync(slot+'/outcome.json','utf8'));
 const hit=match[slot]?.length;
 return outcome.status+(hit?(outcome.status==='REVIEW'?' (matched risk)':' (matched)'):'');
}).join(', ');
const report=[
 '# Experiment 22 — Semantic Fidelity Guard','',
 '**NO-GO. Architectural recommendation C.** Keep the guard experimental. Test source-grounded verification of final article claims after Compose before considering unattended release.','',
 'The hybrid confirmed '+counts(c.tp,c.positives)+' held-out material discrepancies. The unchanged Experiment 20 Judge confirmed '+counts(a.tp,a.positives)+' and raised '+a.riskOnly+' additional relevant unresolved flag. Both confirmed '+counts(mutation.hybrid.tp,mutation.hybrid.positives)+' deliberate mutations. The hybrid missed or misidentified '+(natural.hybrid.positives-natural.hybrid.tp)+' of '+natural.hybrid.positives+' naturally occurring held-out errors.','',
 '## Corpus and evidence','',
 'The corpus contains 49 complete briefs from 15 subjects. Five Wolf and Webb development cases are excluded from primary metrics. The 44 held-out briefs span 13 subjects and contain 13 reference F2 cases and 31 acceptable briefs. The 30 unmodified production outputs on ten new subjects contained zero confirmed natural F2s. Naturally occurring positive evidence is limited to the three familiar archival subjects. All ten authored mutations were separately confirmed by a blinded model review. Archived reference labels are inherited from the previous source reviews.','',
 'New production briefs used one short authoritative excerpt and an operator-prepared handoff per subject. Subject diversity is stronger than the claim-density and positive-case diversity. Source relationships and anchor expressions were supplied manually. No full-brief material-claim census was collected. [Dataset](dataset-manifest.csv) · [Sources](source-provenance.md) · [Freeze ledger](freeze.json)','',
 '## Paired first attempts','',
 '| Condition | Confirmed F2 | Additional matched risks | Confirmed blocker precision | False FAIL on acceptable | REVIEW | Technical failure | Silent F2 escapes |',
 '|---|---:|---:|---:|---:|---:|---:|---:|',
 ...labels.map(([label,m])=>line(label,m)),'',
 '| Cohort | A confirmed | A additional matched risks | B confirmed | C confirmed | C additional matched risks |',
 '|---|---:|---:|---:|---:|---:|',
 ...summaries.map(([name,m])=>'| '+[name,counts(m.baseline.tp,m.baseline.positives),m.baseline.riskOnly,counts(m.deterministic.tp,m.deterministic.positives),counts(m.hybrid.tp,m.hybrid.positives),m.hybrid.riskOnly].join(' | ')+' |'),'',
 '| Reference category | A confirmed | C confirmed |',
 '|---|---:|---:|',
 ...Object.entries(metrics).filter(([key])=>key.startsWith('category/')).map(([key,m])=>'| '+[key.split('/')[1].replaceAll('_',' '),counts(m.baseline.tp,m.baseline.positives),counts(m.hybrid.tp,m.hybrid.positives)].join(' | ')+' |'),'',
 'B found no material discrepancy on the evaluation corpus. Every B outcome was REVIEW. The EIA scale fingerprint fails when the line also includes the distinct electricity ratio. Isolating the EIA ratio demonstrated a mechanical block, but an editorial prohibition using the same literal pattern falsely blocked in the supplementary probe. Entity and chronology routing supplied candidates and had no safe standalone block. [Mechanical probes](runs/fault-check.json)','',
 'The hybrid routed '+counts(c.targetInCandidates,c.positives)+' exact held-out error spans into the candidate set. Candidate routing and evidence quoting were insufficient to establish correct interpretation. All matched hybrid blockers came from the focused model. [Coverage](coverage-analysis.md) · [Case data](scores.csv)','',
 '## Historical regressions','',
 '| Subject | Split | A · Experiment 20 | B · Mechanical | C · Hybrid |',
 '|---|---|---|---|---|---|',
 ...regressionRows.map(([subject,split,ids])=>'| '+[subject,split,...conditions.map(x=>summary(x,ids))].join(' | ')+' |'),'',
 'Unmatched FAIL indicates a different discrepancy. In EIA, the hybrid conflated the energy and electricity comparisons; the baseline correctly identified the Dutch scale on its first attempt. In FAA, the baseline flagged the relevant category uncertainty and the hybrid accepted the changed name. Digital was the hybrid’s archival gain on the first attempt; that result disappeared on repeat. Both Webb chronology errors escaped both model conditions in development. The wolf results were obtained with the intended referents explicitly supplied.','',
 '## Editorial freedom and operating cost','',
 'No brief or upstream content was rewritten. The blinded rubric review found '+counts(reviews.naturalEditorialPass,reviews.naturalEditorialReviewed)+' new naturally generated briefs at or above the Experiment 15 editorial gate. Compact source handoffs often produced repetitive progression and thin endings. The hybrid withheld '+(c.fp+c.acceptableReview)+' of '+c.acceptable+' acceptable briefs in primary evaluation. See the case analysis for the contested Digital finding and evidence-limited risk flags. [Editorial review](editorial-impact.md) · [False positives](false-positive-analysis.md)','',
 'A and C each used one call per brief. Summed first-attempt model-call time was '+sec(a.seconds)+' for A and '+sec(c.seconds)+' for C. B’s mechanical processing totaled '+b.deterministicMs.toFixed(2)+' ms. The paired evaluation used 134 calls including repeats and took '+sec(op.evaluationWallSeconds)+' wall-clock. There was one invalid baseline repeat, which was held as a technical failure. All hybrid calls passed the response contract. No token, credit or price measurement was captured. [Operations](operational-cost.md)','',
 '## Decision and next action','',
 'The 90% matched-detection-or-risk gate was missed and the hybrid did not improve paired triage over the Experiment 20 Judge. The deterministic prototype demonstrated a false contradiction and had no confirmed live detection. Chronology escapes and repeated-answer inconsistency remain. The natural positives are too sparse to generalize. Scope, stability and operational value therefore do not justify adding a Narrative integration.','',
 'Evaluate a final-content release check on archived or separately generated articles. Preserve a source-backed evidence map through the pipeline, then compare the final article’s material factual claims with source evidence. Measure final-stage errors and article-level editorial quality together. Keep publication withheld when material evidence remains unresolved or verification fails. Advance toward unattended release only after this final gate is shown to be accurate and stable on diverse complete articles.','',
 '[Decision gates](decision.json) · [Metrics](metrics.json) · [Reproduction](REPRODUCE.md)',''
];
fs.writeFileSync('report.md',report.join('\n'));
