const fs=require('node:fs');
process.chdir(__dirname);
const load=p=>JSON.parse(fs.readFileSync(p,'utf8').replace(/^\uFEFF/,''));
const m=load('metrics.json');
const a=load('adjudication.json');
const truth=load('ground-truth.json');
const tasks=load('dataset.json').filter(t=>t.split==='evaluation');
const positives=tasks.filter(t=>truth[t.id].label==='F2');
const pct=v=>v===null?'undefined':(100*v).toFixed(1)+'%';
const c=m.candidate,b=m.baseline;
const count=(v,n)=>v+'/'+n;
const technical=c.statusCounts.technical_failure;
const calls=Object.values(m.calls).map(r=>r.evaluation);
const detectionCalls=calls[0].calls+calls[1].calls;
const fullBase='827fd03462b583694074611be7055cda16f061d8';
const phA={matchedTriage:c.triageRecall>=.9,acceptableRevise:c.falsePositiveReviseRate<=.1,
 groundedBlockers:c.supportedBlockers===c.blockerCount,criticalCategoryCoverage:!Object.values(m.categories).some(x=>x.targets>0&&x.extracted===0),
 credibleBreadth:m.positiveSubjects>=10&&m.controlBriefs>=10&&Object.values(m.categories).every(x=>x.targets>0),failClosedTechnical:tasks.every(t=>{
  const o=load('judge-runs/'+t.id+'/outcome.json');
  return o.status==='GO'?o.progression==='eligible':o.progression==='withheld';
 })};
if(phA.matchedTriage&&phA.acceptableRevise) throw Error('This report expects an empirically failed gate');
const decision={decision:'NO-GO',baseCommit:fullBase,phaseA:{status:'FAIL',...phA,
 noRecurringEntityTemporalEscape:!positives.some(t=>c.silentEscapes.includes(t.id)&&['entity_identity','temporal_relationship'].includes(truth[t.id].findings[0].category)), developmentWolfSilentEscape:true,
 breadthStatus:'INSUFFICIENT EVIDENCE for a positive generalization decision'},phaseB:{executed:false,reason:'Phase A failed'},
 editorialRegression:'not measured because no repairs ran',recommendedAction:'Correct source-term preservation and use resolvable evidence spans, then run a newly frozen independent detection study before testing repairs.'};
fs.writeFileSync('decision.json',JSON.stringify(decision,null,2)+'\n');
const det=(label,x)=>'| '+label+' | '+count(x.confirmed.length,m.positiveBriefs)+' | '+pct(x.materialPrecision)+' ('+count(x.supportedBlockers,x.blockerCount)+') | '+pct(x.triageRecall)+' | '+pct(x.silentEscapeRate)+' | '+pct(x.falsePositiveReviseRate)+' | '+pct(x.uncertainRate)+' | '+x.statusCounts.technical_failure+' |';
const stages=Object.entries(m.calls).map(([name,r])=>{
 const e=r.evaluation;
 return '| '+name+' | '+e.calls+' | '+e.valid+' | '+e.invalidJson+' | '+e.invalidContract+' | '+e.transport+' | '+e.meanSeconds.toFixed(1)+' | '+(e.aiCredits?.toFixed(3)??'Unavailable')+' ('+count(e.metered,e.calls)+') |';
}).join('\n');
const cat=Object.entries(m.categories).map(([name,x])=>'| '+name+' | '+x.extracted+' | '+x.targets+' | '+x.confirmed+' |').join('\n');
const dev=['2336788e5a','4632ab6ce2'].map(id=>id+' '+load('judge-runs/'+id+'/outcome.json').status).join(', ');
const exact=positives.map(t=>{
 const f=truth[t.id].findings[0],r=a[t.id];
 const base=fs.existsSync('judge-runs/'+t.id+'/experiment20/result.json')?load('judge-runs/'+t.id+'/experiment20/result.json').verdict:'technical failure';
 const result=load('judge-runs/'+t.id+'/outcome.json').status;
 return '### '+t.subject+' — '+t.id+'\n\n- Narrative excerpt: “'+f.narrativeExcerpt+'”\n- Archived evidence: “'+f.evidenceExcerpt+'”\n- Source review: '+f.explanation+'\n- Evidence-first '+result+' ('+r.candidateMatch+'). Experiment 20 '+base+' ('+r.baselineMatch+'). '+r.note;
}).join('\n\n');
fs.writeFileSync('failure-decomposition.md','# Material error trace\n\n'+positives.map(t=>{
 const r=a[t.id];
 return '## '+t.subject+' — '+t.id+'\n\n- Target assertion included: '+(r.extracted??'unassessable')+'.\n- Decisive evidence found: '+(r.evidenceLocated??'unassessable')+'.\n- Normalization: '+r.normalization+'\n- Comparison: '+r.comparison+'\n- Aggregation or technical gate: '+r.aggregation;
}).join('\n\n')+'\n');
fs.writeFileSync('error-analysis.md','# Evidence review\n\n'+exact+'\n\n## Acceptable briefs\n\n'+tasks.filter(t=>truth[t.id].label!=='F2').map(t=>'- '+t.id+'. '+a[t.id].note).join('\n')+'\n\n## Assessment independence\n\nThe operator checked archived target labels against the original source anchors, examined every blocker and disputed finding, and reviewed new mutation and control briefs before acceptance scoring. Six fresh review sessions on the three new subjects used the full inputs without the experimental verdict or label. These used the same GPT-6 Luna model, with previous editorial scores retained for exactly matched historical inputs. The independent reference tracks the known central errors; full-brief claim recall and general evidence alignment accuracy cannot be inferred from target-only review. Exact quote validation establishes lexical provenance, while semantic relevance and accuracy were adjudicated for the decision-bearing findings. No claims were added to the frozen truth after results arrived.\n');
fs.mkdirSync('final-outputs',{recursive:true});
fs.writeFileSync('final-outputs/phase-a-dispositions.csv','id,evidence_first,automatic_progression\n'+tasks.map(t=>{const o=load('judge-runs/'+t.id+'/outcome.json');return [t.id,o.status,o.progression].join(',');}).join('\n')+'\n');
fs.writeFileSync('report.md',[
 '# Experiment 21 — Evidence-First Narrative Judge',
 '**NO-GO.** Evidence-first matched '+c.confirmed.length+' of '+m.positiveBriefs+' material errors as supported contradictions and '+c.unresolved.length+' as relevant unresolved risks. Triage was '+pct(c.triageRecall)+' against the 90% gate. The study stopped before repair.',
 '## Dataset and confidence',
 'The frozen evaluation contains '+m.evaluationBriefs+' complete Narratives on '+m.evaluationSubjects+' source subjects. There are six historical F2 briefs, four historical acceptable briefs, three current-production controls and three operator mutations on those same three new sources. The nine F2 briefs span '+m.positiveSubjects+' subjects. Only FAA, EIA and Digital were historical subject holdouts; the evaluated Wolf and Webb samples share source facts with development. Development outcomes were '+dev+'.',
 'The purposive corpus has dependent pairs and repeated subjects. Each error changes observed recall by 11.1 percentage points. The positive-subject and acceptable-brief counts fall short of the frozen breadth floor and there is no F2 causality target. There is insufficient evidence for generalization, with a demonstrated failure of the observed detection gate.',
 '## Paired Phase A',
 '| Condition | Material recall | Blocker precision | Triage recall | Silent escape | Acceptable REVISE | UNCERTAIN | Technical blocks |\n| --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: |\n'+det('Evidence-first',c)+'\n'+det('Experiment 20',b),
 'The comparator was rerun in fresh GPT-6 Luna sessions on exactly the same documents and briefs, using the archived prompt, schema and decoder. Original Experiment 20 aggregate results were 2/8 known errors detected and repaired. The paired results above are the relevant comparison. Evidence-first yielded one additional historical detection (Digital) and lost four detections the comparator made (Wolf, FAA, EIA and Bennu). Six of seven acceptable briefs were withheld by the candidate. A technical hold or an unrelated uncertainty blocks progression, but earns no factual detection credit.',
 'The accepted extraction captured '+m.targetClaimCoverage.covered+'/'+m.targetClaimCoverage.total+' target assertions. Correct source anchors appeared in '+m.targetEvidenceAlignment.correct+'/'+m.targetEvidenceAlignment.assessable+' assessable target traces, including raw outputs later rejected on other quotations. See the decomposition for stages that dropped a relationship or erased a term during normalization.',
 '| Category | Valid extracted claims | Reference F2 targets | Confirmed |\n| --- | ---: | ---: | ---: |\n'+cat,
 'The exact results for Wolf, Webb, EIA, FAA and Digital are in [error analysis](error-analysis.md). [Failure decomposition](failure-decomposition.md) traces extraction, location, normalization, comparison and aggregation separately for every F2.',
 '## Editorial and factual outcomes',
 'The initial reference set has '+tasks.filter(t=>truth[t.id].label==='F0').length+' F0, '+tasks.filter(t=>truth[t.id].label==='F1').length+' F1 and '+m.positiveBriefs+' F2 briefs. The full-brief Experiment 15 rubric yielded '+m.editorial.accepted+'/'+m.editorial.scored+' editorial passes, with a mean of '+m.editorial.mean.toFixed(2)+'/5. The Judge only assessed briefs. Repair, editorial change, post-repair fidelity and introduction of new errors through repair were therefore unmeasured.',
 '## Runtime, cost and unattended handling',
 'The production console generated three new originals in '+m.generation.totalSeconds.toFixed(1)+' summed seconds and reported no token or monetary usage in these run archives. The candidate spent '+detectionCalls+' evaluation calls across extraction and comparison. Aggregation was local with no model call. Repair and reverification used zero calls.',
 '| Stage | Calls | Valid | Invalid JSON | Invalid contract | Transport failure | Mean elapsed s | AI credits and metered coverage |\n| --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: |\n'+stages,
 'Per-call model latency includes CLI startup. The sequential candidate stages incurred '+(calls[0].totalSeconds+calls[1].totalSeconds).toFixed(1)+' summed seconds and '+(calls[0].aiCredits+calls[1].aiCredits).toFixed(3)+' credits across evaluation, compared with '+calls[2].totalSeconds.toFixed(1)+' seconds and '+calls[2].aiCredits.toFixed(3)+' credits for the comparator. AI credits come from matched Copilot session shutdown records and are separate from wall time and any currency costs. Six independent review calls are archived separately with usage.',
 'Evidence-first marked '+c.statusCounts.GO+' eligible, withheld '+c.statusCounts.REVISE+' for asserted contradictions, '+c.statusCounts.UNCERTAIN+' for uncertainty and '+technical+' for technical faults. Source and Narrative substring checks caught model quotation errors. Deterministic validation checks cover duplicate keys, empty extraction, missing assessments, invalid quotes, missing evidence, material uncertainty and contradictions. GO remains unsafe when normalization hides a discrepancy or a relationship goes unextracted.',
 '## Decision and next action',
 'Keep Narrative outside unattended publication. For a subsequent detection experiment, preserve claim surface wording and referents alongside normalization, ground evidence with mechanically resolvable spans, and test those changes on a new frozen subject-disjoint corpus before considering repair.',
 'Delivery commit SHA is recoverable with the command in [REPRODUCE.md](REPRODUCE.md).'
].join('\n\n')+'\n');

