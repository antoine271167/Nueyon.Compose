const fs=require('node:fs');
process.chdir(__dirname);
const read=f=>JSON.parse(fs.readFileSync(f,'utf8'));
const write=(f,s)=>fs.writeFileSync(f,s.trim()+'\n');
const n=read('evidence-preservation-metrics.json'), a=read('article-metrics.json');
const ops=read('operations.json'), decision=read('decision.json');
const dataset=read('dataset.json');
const group=['A','B','C'];
const num=(v,d=2)=>Number.isFinite(v)?v.toFixed(d):'unavailable';
const pct=v=>Number.isFinite(v)?num(100*v,1)+'%':'unavailable';
const pp=v=>Number.isFinite(v)?num(100*v,1)+' pp':'unavailable';
const sign=(v,f=num)=>Number.isFinite(v)?(v>0?'+':'')+f(v):'unavailable';
const ci=(x,f=num)=>x.interval?'['+x.interval.map(v=>sign(v,f)).join(', ')+']':'unavailable';
const table=(head,rows)=>'| '+head.join(' | ')+' |\n| '+head.map(()=> '---').join(' | ')+' |\n'+rows.map(r=>'| '+r.join(' | ')+' |').join('\n');
const span=(v,stage)=>v.editorial+'/'+v.generated+' editorial, '+v.factual+'/'+v.generated+' factual';
const counts=read('evidence-item-outcomes.json');
const aHold=a.holdout;
const nHold=n.holdout;
const links=a.linkedin.holdout;
const pair=a.paired.B;
const preserve=n.paired.B.preservation;
const narrativeTable=table(['Treatment','Usable factual reviews','Preserved','Distinct developments','Words mean / median','Editorial score','Redundancy','Peripheral'],
  group.map(c=>{const h=nHold[c];return [c,h.factual+'/'+h.generated,pct(h.preservation),num(h.developments),
    num(h.words,1)+' / '+num(h.medianWords,1),num(h.score),num(h.redundancy),num(h.peripheral)];}));
const articleTable=table(['Treatment','Accepted / scored','Overall','Development','Opening','Readability','Focus','Ending','Voice','Mean words','Evidence from handoff'],
  group.map(c=>{const h=aHold[c];return [c,h.accepted+'/'+h.editorial+' ('+pct(h.acceptance)+')',num(h.score),
    ...[1,0,2,3,4,5].map(i=>num(h.criteria[i])),num(h.words,1),num(h.realized)];}));
const pairTable=table(['Measure','B − A','95% subject bootstrap','Paired subjects','C − A','95% subject bootstrap','Paired subjects'],
  Object.entries({acceptance:'Acceptance',score:'Editorial score',development:'Development',opening:'Opening',ending:'Ending',
    readability:'Readability',focus:'Focus',words:'Words',realized:'Useful handoff items used'}).map(([k,label])=>{
      const f=k==='acceptance'?pp:num;
      const b=a.paired.B[k],c=a.paired.C[k];
      return [label,sign(b.mean,f),ci(b,f),Object.keys(b.deltas).length,sign(c.mean,f),ci(c,f),Object.keys(c.deltas).length];
  }));
const development=table(['Treatment','Narrative items preserved','Article accepted','Article mean','Article development','Usable factual Article reviews'],
  group.map(c=>[c,pct(n.development[c].preservation),a.development[c].accepted+'/'+a.development[c].editorial,num(a.development[c].score),
    num(a.development[c].criteria[1]),a.development[c].factual+'/'+a.development[c].generated]));
const stratum=table(['Stratum','Treatment','Subjects','Narrative preservation','Article accepted / scored','Article development','Article words','Handoff items used'],
  ['rich','constrained'].flatMap(stratum=>group.map(c=>{
    const h=aHold[c][stratum], nh=nHold[c][stratum];
    return [stratum,c,nh.planned,pct(nh.preservation),h.accepted+'/'+h.editorial,num(h.criteria[1]),num(h.words,1),num(h.realized)];
  })));
const nMeasure=counts.filter(x=>x.phase==='holdout');
const bySubject=table(['Subject','Cohort','Stratum','Inventory items','A preserved','B preserved','C preserved','B − A Article development'],
  dataset.filter(r=>r.phase==='holdout').map(({subject,cohort,richness})=>{
    const items=nMeasure.filter(r=>r.subject===subject);
    return [subject,cohort,richness,items.length,...group.map(c=>items.some(i=>i.narrative[c]!==null)?items.filter(i=>i.narrative[c]==='preserved').length:'unavailable'),
      sign(pair.development.deltas[subject])];
  }));
function categories(c,status){
  const selected=nMeasure.filter(x=>x.narrative[c]===status);
  return selected.length?selected.map(x=>x.subject+'/'+x.id+' ('+x.kind+')').join(', '):'none captured in usable reviews';
}
write('narrative-analysis.md',
'# Narrative editorial and evidence results\n\n'+narrativeTable+
'\n\nThe mean paired A-to-B preservation difference was '+sign(preserve.mean,pp)+' with a 95% subject-grouped bootstrap interval of '+ci(preserve,pp)+
' on '+Object.keys(preserve.deltas).length+' paired subjects. Preservation uses audited relevant items, with each available subject weighted equally.'+
'\n\n'+bySubject+
'\n\nA omissions by evidence item and type include '+categories('A','omitted')+'. B omissions include '+categories('B','omitted')+
'. A distortions recorded by the inventory reviewers include '+categories('A','distorted')+'. B distortions include '+categories('B','distorted')+
'. The complete inventory status, wording, per-condition review and Article result links are in [evidence item outcomes](evidence-item-outcomes.json).'+
'\n\nThe scores concern structure and editorial usefulness before Compose. The semantic fidelity analysis examines material errors and their origins separately.');
write('article-analysis.md',
'# Article editorial quality and realized evidence\n\n'+articleTable+'\n\n'+pairTable+
'\n\nCoverage per treatment was '+group.map(c=>c+' '+span(aHold[c])).join(', ')+
'. The realized-evidence column counts useful inventory items present in both the Narrative and the finished Article. This requires a usable factual review for both stages. Opening, development, focus, ending and voice come from a separate blinded review. The [metrics](article-metrics.json) retain exact paragraph steps, endpoint tags, omitted handoff items, repetition and defect counts.'+
'\n\n## Source richness\n\n'+stratum+
'\n\nThe strata are purposive and the paired intervals resample subjects, keeping their three Article runs together. Repeated generations from the same handoff share an upstream unit.');
const linkedTable=table(['Treatment','Valid','Accepted','Mean','Substance','Mean words','Usable factual reviews'],
  ['A','B'].map(c=>{const h=links[c];return [c,h.generated+'/'+h.planned,h.accepted+'/'+h.editorial,num(h.score),num(h.criteria[2]),num(h.words,1),h.factual];}));
write('format-neutrality-analysis.md',
'# Format neutrality\n\n'+linkedTable+
'\n\nThe LinkedIn check uses one independent generation per condition on Digital, Webb, DART and Git sparse checkout. These are the same A and B Narratives used for Articles. The production LinkedIn prompt and schema are frozen. The LinkedIn rubric examines substance in criterion 3. Its four subject pairs provide a limited transfer check.'+
'\n\nThe handoff remains structured as plain Narrative content. Neither the added paragraph nor its template imposes a target channel, writing length, finished prose, hook, hashtags or call to action. Source errors can transfer to either format with the same handoff, as the independent factual review documents.');
const opTable=table(['Stage','Started','Valid raw','Invalid raw','Interrupted','Process seconds mean / median'],
  Object.entries(ops).map(([stage,r])=>[stage,r.started,r.valid,r.invalid,r.openSlots.length,num(r.meanSeconds,1)+' / '+num(r.medianSeconds,1)]));
write('operational-cost.md',
'# Operational cost and reliability\n\n'+opTable+
'\n\nNarrative and Compose are separate production CLI processes. The outputs folder counts development Article, holdout Article and LinkedIn. C references are operator-generated files and use no production model slot. The six recovered input casing rejections are accounted for separately and stopped before provider invocation. The named raw status counts include review JSON mismatches that deterministic normalization could recover. Raw failures and review-only secondary calls remain in the archive.'+
'\n\nThe holdout Narrative median CLI times were '+num(decision.checks.narrativeLatency.measured.A,1)+' seconds for A and '+
num(decision.checks.narrativeLatency.measured.B,1)+' seconds for B. Process time includes application startup and provider latency. Billed cost, tokens and provider build were unavailable. The preflight GPT-6 Luna Narrative and Copilot calls were outside the measured plan.'+
'\n\nThe solution built successfully and 303 discovered tests passed. The Domain.Tests project contained no discovered tests. See [build](build.log) and [tests](tests.log).');
write('results-summary.md','# Development and holdout tables\n\n'+development+'\n\n'+narrativeTable+'\n\n'+articleTable+'\n\n'+pairTable+'\n\n'+stratum+'\n\n'+linkedTable);
