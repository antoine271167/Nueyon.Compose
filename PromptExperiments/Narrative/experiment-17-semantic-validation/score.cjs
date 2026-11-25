const fs = require('node:fs');
const path = require('node:path');
const {parse} = require('./run.cjs');
const {read,rows,saveCsv} = require('./inventory.cjs');
const local = f => path.join(__dirname,f);
const manifest = rows(local('dataset-manifest.csv'));
const labels = new Map([...read(local('ground-truth.md')).matchAll(/^\| (\w+) \| (F[012]) \|/gm)].map(m=>[m[1],m[2]]));
const review = process.argv.includes('--inspect') ? {} : JSON.parse(read(local('finding-review.json')));
const normalized = s => s.replaceAll('\r\n','\n');
const output = [];
const findings = [];
for (const row of manifest) {
  if(!labels.has(row.id)) throw Error('Missing ground truth '+row.id);
  for(const condition of ['S','S+Source']) {
    const dir = local(path.join('runs',row.id,condition));
    if(!fs.existsSync(path.join(dir,'attempt.json'))) {
      if(process.argv.includes('--inspect')) continue;
      throw Error('Missing attempt '+dir);
    }
    const label = labels.get(row.id);
    const attempt = JSON.parse(read(path.join(dir,'attempt.json')));
    const result = attempt.valid ? parse(read(path.join(dir,'response.txt'))) : {status:'INVALID',findings:[]};
    const material = label==='F2';
    const status = result.status;
    const confusion = status==='INVALID' ? 'INVALID' : status==='FAIL' ? material ? 'TP' : 'FP' : material ? 'FN' : 'TN';
    const narrative = JSON.parse(read(row.output)).content;
    const evidence = Object.values(JSON.parse(read(row.idea))).filter(v=>typeof v==='string')
      .concat(JSON.parse(read(row.synthesis)).content);
    if(condition==='S+Source') evidence.push(read(row.source));
    let matched = false;
    let actionable = false;
    result.findings.forEach((f,i)=>{
      const key = [row.id,condition,i+1].join('/');
      const truth = review[key];
      const narrativeExact = narrative.includes(f.narrativeExcerpt);
      const evidenceExact = !!f.evidenceExcerpt && evidence.some(s=>s.includes(f.evidenceExcerpt));
      const evidenceNewlineEquivalent = !!f.evidenceExcerpt && evidence.some(s=>normalized(s).includes(normalized(f.evidenceExcerpt)));
      if(process.argv.includes('--inspect')) {
        console.log(JSON.stringify({key,status,label,narrativeExact,evidenceExact,evidenceNewlineEquivalent,...f}));
        return;
      }
      if(!truth) throw Error('Missing finding review '+key);
      if ((truth.match==='F2' && !material) || (truth.match==='F1' && label!=='F1')) throw Error('Inconsistent finding match '+key);
      const exact = narrativeExact && evidenceExact;
      const action = exact && truth.supported && truth.corrective;
      matched ||= truth.match==='F2' && f.severity==='material';
      actionable ||= truth.match==='F2' && f.severity==='material' && action;
      findings.push({id:row.id,condition,index:i+1,category:f.category,severity:f.severity,match:truth.match,narrativeExact,evidenceExact,evidenceNewlineEquivalent,supported:truth.supported,corrective:truth.corrective,actionable:action,note:truth.note});
    });
    output.push({id:row.id,split:row.split,subject:row.subject,condition,label,status,confusion,findings:result.findings.length,matchedMaterial:matched,actionableMaterial:actionable,seconds:attempt.seconds});
  }
}
if(process.argv.includes('--inspect')) process.exit();
if(Object.keys(review).length!==findings.length) throw Error('Finding review keys do not match findings');
saveCsv('scores.csv',output);
saveCsv('finding-scores.csv',findings);
const ratio = (numerator,denominator) => ({numerator,denominator,value:denominator ? numerator/denominator : null});
const metrics = {};
for(const split of ['development','evaluation']) {
  metrics[split]={};
  for(const condition of ['S','S+Source']) {
    const subset = output.filter(r=>r.condition===condition && r.split===split);
    const positive = subset.filter(r=>r.label==='F2');
    const scoped = findings.filter(f=>f.condition===condition && subset.some(r=>r.id===f.id));
    const confirmed = scoped.filter(r=>r.match==='F2' && r.severity==='material');
    const count = key => subset.filter(r=>r.confusion===key).length;
    const [TP,FP,TN,FN,invalid] = ['TP','FP','TN','FN','INVALID'].map(count);
    const materialFindings = scoped.filter(f=>f.severity==='material');
    metrics[split][condition]={
      cases:subset.length,subjects:new Set(subset.map(r=>r.subject)).size,
      independentPositiveSubjects:[...new Set(positive.map(r=>r.subject))],
      confusion:{TP,FP,TN,FN,invalid},
      coverage:ratio(subset.length-invalid,subset.length),
      precision:ratio(TP,TP+FP),recall:ratio(TP,TP+FN),
      falsePositiveRate:ratio(FP,FP+TN),falseNegativeRate:ratio(FN,TP+FN),
      reviewRate:ratio(subset.filter(r=>r.status==='REVIEW').length,subset.length-invalid),
      materialFlagOrReviewRate:ratio(TP+FP+subset.filter(r=>r.status==='REVIEW').length,subset.length-invalid),
      findingPrecision:ratio(confirmed.length,materialFindings.length),
      findingRecall:ratio(positive.filter(r=>r.matchedMaterial).length,positive.length),
      actionableMaterialRecall:ratio(positive.filter(r=>r.actionableMaterial).length,positive.length),
      actionableEvidence:ratio(confirmed.filter(r=>r.actionable).length,confirmed.length),
      actionableAmongAllFindings:ratio(scoped.filter(r=>r.actionable).length,scoped.length),
      findings:scoped.length
    };
  }
}
fs.writeFileSync(local('metrics.json'),JSON.stringify(metrics,null,2)+'\n');

