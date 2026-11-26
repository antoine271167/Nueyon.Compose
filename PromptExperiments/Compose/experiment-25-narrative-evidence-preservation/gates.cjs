const fs=require('node:fs');
const path=require('node:path');
process.chdir(__dirname);
const read=f=>JSON.parse(fs.readFileSync(f,'utf8'));
const n=read('evidence-preservation-metrics.json');
const a=read('article-metrics.json');
const ops=read('operations.json');
const flags=read('factual-flags.json').filter(f=>f.phase==='holdout');
const nominations=fs.existsSync('operator-gates.json')?read('operator-gates.json'):{};
const gate=(measured,required,pass)=>({measured,required,pass});
const difference=(dataset,key)=>dataset.paired.B[key].mean;
const checks={
  preservation:gate(difference(n,'preservation'),0.20,difference(n,'preservation')!==null&&difference(n,'preservation')>=.20),
  distinctSupportedDevelopments:gate(difference(n,'developments'),'positive',
    difference(n,'developments')!==null&&difference(n,'developments')>0),
  acceptance:gate(difference(a,'acceptance'),.20,difference(a,'acceptance')!==null&&difference(a,'acceptance')>=.20),
  editorial:gate(difference(a,'score'),.30,difference(a,'score')!==null&&difference(a,'score')>=.30),
  development:gate(difference(a,'development'),.40,difference(a,'development')!==null&&difference(a,'development')>=.40)
};
for(const key of ['readability','focus','ending']){
  const value=difference(a,key);
  checks[key]=gate(value,-.15,value!==null&&value>=-.15);
}
for(const key of ['acceptance','development']){
  const value=a.paired.B[key].interval?.[0]??null;
  checks[key+'Interval']=gate(value,'greater than zero',value!==null&&value>0);
}
const subjects=Object.values(a.paired.B.development.deltas).filter(x=>x>0).length;
checks.developmentSubjects=gate(subjects,4,subjects>=4);
const neutral=n.rows.filter(r=>r.phase==='holdout'&&r.c==='B'&&r.formatNeutral===true).length;
checks.neutralStructure=gate({independentFactualFlagsNeutral:neutral,operatorConfirmed:nominations.formatNeutrality?.pass??null},
  'All handoffs channel agnostic',neutral===12||nominations.formatNeutrality?.pass===true);
const reviewCounts=[a,n].map(x=>Object.values(x.holdout).map(r=>({generated:r.generated,editorial:r.editorial,factual:r.factual})));
const unresolved=flags.filter(f=>['U','pending'].includes(f.decision)).length;
const newNarrativeF2=flags.filter(f=>f.condition==='B'&&f.stage==='narrative'&&f.decision==='F2'&&f.decisionOrigin==='narrative').length;
const factualCoverage=reviewCounts.every(group=>group.every(r=>r.factual===r.generated));
const editorialCoverage=reviewCounts.every(group=>group.every(r=>r.editorial===r.generated));
const materialByOutput=Object.fromEntries(['A','B','C'].map(c=>[c,Object.fromEntries(['narrative','article','linkedin'].map(stage=>[stage,
  Object.fromEntries(['upstream','narrative','propagated','compose','amplification'].map(origin=>[origin,
    new Set(flags.filter(f=>f.condition===c&&f.stage===stage&&f.decision==='F2'&&f.decisionOrigin===origin).map(f=>f.id)).size]))]))]));
const latency=c=>{
  const list=n.rows.filter(r=>r.phase==='holdout'&&r.c===c&&r.generated).map(r=>read(path.join('narratives',c,r.subject,'attempt.json')).seconds).sort((a,b)=>a-b);
  return list.length?(list[Math.floor((list.length-1)/2)]+list[Math.floor(list.length/2)])/2:null;
};
checks.narrativeLatency=gate({A:latency('A'),B:latency('B')},'B median no more than 150% of A',latency('B')<=1.5*latency('A'));
checks.newNarrativeF2=gate(newNarrativeF2,0,newNarrativeF2===0&&factualCoverage&&unresolved===0);
checks.newComposeF2=gate({A:materialByOutput.A.article.compose,B:materialByOutput.B.article.compose},'B no more than A',materialByOutput.B.article.compose<=materialByOutput.A.article.compose&&factualCoverage&&unresolved===0);
checks.amplification=gate({A:materialByOutput.A.article.amplification,B:materialByOutput.B.article.amplification},'B no more than A',materialByOutput.B.article.amplification<=materialByOutput.A.article.amplification&&factualCoverage&&unresolved===0);
checks.reviewCoverage=gate({reviewCounts,unresolved,editorialCoverage,factualCoverage},'Complete decision-relevant evidence',editorialCoverage&&factualCoverage&&unresolved===0);
const completeArticles=['A','B','C'].every(c=>a.holdout[c].generated===36&&a.holdout[c].editorial===36);
const primary=['acceptance','editorial','development'];
const bestAcceptance=(a.holdout.B.accepted+36-a.holdout.B.editorial-a.holdout.A.accepted)/36;
const robustFailure=completeArticles&&primary.some(k=>!checks[k].pass)||bestAcceptance<.20;
const safetyFailure=newNarrativeF2>0||materialByOutput.B.article.compose>materialByOutput.A.article.compose&&factualCoverage&&unresolved===0;
const operatorKeys=['centralThread','redundancy','peripheralDetail','realizedEvidence','formatNeutrality','reliability','independentCoverage','exactFreeze'];
const convincing=completeArticles&&Object.values(checks).every(result=>result.pass)&&operatorKeys.every(key=>nominations[key]?.pass===true);
const status=robustFailure||safetyFailure?'NO-GO':convincing?'GO':
  completeArticles&&factualCoverage&&unresolved===0&&(checks.preservation.pass||checks.editorial.pass)?'CONDITIONAL GO':'INSUFFICIENT EVIDENCE';
const result={status,promoted:false,diagnosticC:true,checks,materialByOutput,unresolved,operations:ops,
  bestAcceptanceGainIncludingMissing:bestAcceptance,operatorGates:nominations,
  basis:completeArticles&&robustFailure?'The complete blinded Article cohort failed a frozen quality threshold.':robustFailure?'The Article quality threshold fails even under a best-case bound for missing reviews.':safetyFailure?'A mandatory material-fidelity gate failed.':convincing?'All measured and operator-verified promotion requirements passed.':'Verified coverage or conclusive passing evidence required for promotion is incomplete.'};
fs.writeFileSync('decision.json',JSON.stringify(result,null,2)+'\n');
