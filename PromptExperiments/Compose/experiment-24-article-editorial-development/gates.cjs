const fs=require('node:fs');
const path=require('node:path');
const {root,json,hash,verify}=require('./run.cjs');
verify();
const metrics=json('metrics.json');
const selection=json('development-selection.json');
if(selection.freezeHash!==hash('freeze.json')||selection.protocolHash!==hash('protocol.md')) throw Error('Selection does not match freeze.');
const held=metrics.holdout.conditions;
const baseline=held.A;
const criteria=['readability','focus'];
const rows=fs.readFileSync(path.join(root,'editorial-scores.csv'),'utf8').trim().split('\n').slice(1)
  .map(line=>[...line.matchAll(/"((?:[^"]|"")*)"(?:,|$)/g)].map(match=>match[1].replaceAll('""','"')));
const tags=[...new Set(rows.filter(row=>row[0]==='holdout').flatMap(row=>row.at(-1).split(' ')).filter(Boolean))];
const decisions={};
for(const condition of ['B','C']) {
  const candidate=held[condition];
  const paired=metrics.holdout.paired[condition];
  const delta={acceptance:candidate.acceptance-baseline.acceptance,mean:candidate.mean-baseline.mean,
    ...Object.fromEntries(Object.keys(candidate.criteria).map(key=>[key,candidate.criteria[key]-baseline.criteria[key]]))};
  const novelDefects=tags.flatMap(tag=>{
    const flagged=key=>rows.filter(row=>row[0]==='holdout'&&row[3]===key&&row.at(-1).split(' ').includes(tag));
    const observed=flagged(condition);
    const difference=observed.length/candidate.editorialReviewed-flagged('A').length/baseline.editorialReviewed;
    return difference>=.1&&new Set(observed.map(row=>row[1])).size>=3?[{tag,difference}]:[];
  });
  const editorial={
    acceptance:delta.acceptance>=.20,weightedMean:delta.mean>=.30,development:delta.development>=.40,ending:delta.ending>=.40,
    readabilityAndFocus:criteria.every(key=>delta[key]>-.20),
    breadth:paired.leaveOneOut.every(row=>row.meanDelta>0&&row.acceptanceDelta>0),
    uncertainty:paired.meanInterval?.[0]>0&&paired.acceptanceInterval?.[0]>0,
    recurringDefects:novelDefects.length===0
  };
  const factual={noNewF2:candidate.newF2===0,noAdditionalAmplification:candidate.amplifiedF2<=baseline.amplifiedF2,
    fullCoverage:candidate.factualWithIncompleteReview===0&&candidate.unresolved===0&&baseline.factualWithIncompleteReview===0&&baseline.unresolved===0};
  const operational={validity:candidate.attempted===candidate.planned&&candidate.valid>=baseline.valid-1,
    latency:candidate.medianSeconds<=1.25*baseline.medianSeconds&&candidate.p90Seconds<=1.50*baseline.p90Seconds};
  const reproducibility={selected:selection.selected===condition,editorialCoverage:candidate.editorialReviewed===candidate.valid&&baseline.editorialReviewed===baseline.valid};
  const negative=Object.values(editorial).some(value=>!value)||!factual.noNewF2||!factual.noAdditionalAmplification||!operational.validity;
  const clear=[editorial,factual,operational,reproducibility].every(group=>Object.values(group).every(Boolean)); const label=negative?'NO-GO':clear?'GO':reproducibility.editorialCoverage?'CONDITIONAL GO':'INSUFFICIENT EVIDENCE';
  decisions[condition]={label,delta,editorial,factual,operational,reproducibility,novelDefects};
}
fs.writeFileSync(path.join(root,'decision.json'),JSON.stringify({selected:selection.selected,
  overall:decisions[selection.selected].label,promoted:false,decisions,freezeHash:hash('freeze.json')},null,2)+'\n');

