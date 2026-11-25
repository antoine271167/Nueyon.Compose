const fs=require('node:fs');
const path=require('node:path');
const {createHash}=require('node:crypto');
const at=file=>path.join(__dirname,file);
const read=file=>fs.readFileSync(at(file),'utf8');
const obj=file=>JSON.parse(read(file));
const exists=file=>fs.existsSync(at(file));
const sha=file=>createHash('sha256').update(fs.readFileSync(at(file))).digest('hex');
const truth=obj('ground-truth.json');
const supplement=obj('supplementary-plan.json');
const planned=read('narrative-plan.csv').trim().split(/\r?\n/).slice(1).map(line=>line.split(',')[0]);
function csv(file,rows){
 const keys=[...new Set(rows.flatMap(Object.keys))];
 const cell=value=>'"'+String(value??'').replaceAll('"','""')+'"';
 fs.writeFileSync(at(file),keys.join(',')+'\n'+rows.map(row=>keys.map(k=>cell(row[k])).join(',')).join('\n')+'\n');
}
const narrative=planned.flatMap(id=>[1,2].map(rep=>'narrative-runs/'+id+'/attempt-'+rep+'/attempt.json').filter(exists).map(obj));
const collect=(cohort,folder)=>cohort.flatMap(row=>[1,2].map(rep=>{
 const dir=folder+'/'+row.id+'/'+rep;
 const file=dir+'/attempt.json';
 if(!exists(file))return {id:row.id,rep,subject:row.subject,label:row.label,status:null,error:'missing',dir};
 const meta=obj(file);
 for(const [f,hash] of [['input.json',meta.inputSha256],['prompt.txt',meta.promptSha256],['response.txt',meta.responseSha256]])if(sha(dir+'/'+f)!==hash)throw Error('Archive hash mismatch '+dir+'/'+f);
 return {...meta,subject:row.subject,label:row.label,dir};
}));
const primary=collect(truth,'validator-runs');
const regression=collect(supplement,'supplementary-runs');
for(const cohort of [primary,regression]) {
 for(let i=0;i<cohort.length;i+=2){
  const [a,b]=cohort.slice(i,i+2);
  if(a.inputSha256&&b.inputSha256&&(a.inputSha256!==b.inputSha256||a.promptSha256!==b.promptSha256))throw Error('Repeat input differs '+a.id);
 }
}
const flags=row=>row.status==='FAIL'||row.status==='REVIEW';
const count=(rows,p)=>rows.filter(p).length;
const rate=(a,b)=>b?a/b:null;
const positive=row=>row.label==='F2';
const fin=exists('finding-review.json')?obj('finding-review.json'):{};
function findings(rows){
 return rows.filter(row=>row.status).flatMap(row=>{
  const input=obj(row.dir+'/input.json');
  return obj(row.dir+'/parsed.json').findings.map((finding,index)=>{
   const name=row.id+'/'+row.rep+'/'+(index+1);
   const quote=input.narrative.content.includes(finding.narrativeExcerpt);
   const exactEvidence=!!finding.evidenceExcerpt&&[input.source,input.synthesis.content,...Object.values(input.selectedIdea)].filter(x=>typeof x==='string').some(x=>x.includes(finding.evidenceExcerpt));
   const adjudication=fin[name];
   return {name,id:row.id,rep:row.rep,label:row.label,...finding,quote,exactEvidence,...adjudication,
    actionable:quote&&exactEvidence&&adjudication?.relevant===true&&adjudication.explained===true&&adjudication.typed===true&&adjudication.corrected===true};
  });
 });
}
function metrics(rows){
 const valid=rows.filter(row=>row.status);
 const pos=valid.filter(positive);
 const neg=valid.filter(row=>!positive(row));
 const tp=count(pos,r=>r.status==='FAIL'),fp=count(neg,r=>r.status==='FAIL');
 const escapes=pos.filter(r=>r.status==='PASS');
 const pairs=[];
 const changes=[];
 const bothPass=[];
 for(let i=0;i<rows.length;i+=2){
  const [a,b]=rows.slice(i,i+2);
  if(a.status&&b.status){
   pairs.push(a.status===b.status);
   if(positive(a)&&flags(a)!==flags(b))changes.push(a.id);
   if(positive(a)&&a.status==='PASS'&&b.status==='PASS')bothPass.push(a.id);
  }
 }
 const latency=rows.filter(row=>typeof row.seconds==='number').map(row=>row.seconds).sort((a,b)=>a-b);
 const pct=p=>latency.length?latency[Math.ceil(p*latency.length)-1]:null;
 const checked=findings(rows);
 const material=checked.filter(f=>f.severity==='material');
 const workload=Object.fromEntries(['PASS','REVIEW','FAIL',null].map(s=>{const n=count(rows,r=>r.status===s);return [s??'INVALID_OR_MISSING',{count:n,rate:rate(n,rows.length)}];}));
 return {planned:rows.length,executed:count(rows,r=>r.error!=='missing'),valid:valid.length,validRate:rate(valid.length,rows.length),validF2Attempts:pos.length,validAcceptableAttempts:neg.length,
  errors:Object.fromEntries(['invalid-json','invalid-contract','transport','missing'].map(kind=>[kind,count(rows,r=>r.error===kind)])),
  invalidJsonRate:rate(count(rows,r=>r.error==='invalid-json'),rows.length),
  definitive:{tp,fp,tn:neg.length-fp,fn:pos.length-tp,precision:rate(tp,tp+fp),recall:rate(tp,pos.length),specificity:rate(neg.length-fp,neg.length)},
  triage:{tp:count(pos,flags),fp:count(neg,flags),tn:count(neg,r=>!flags(r)),fn:escapes.length,sensitivity:rate(count(pos,flags),pos.length),specificity:rate(count(neg,r=>!flags(r)),neg.length),silentEscapes:escapes.length,silentEscapeRate:rate(escapes.length,pos.length),escapeIds:escapes.map(r=>r.id+'/'+r.rep)},
  acceptable:{failRate:rate(fp,neg.length),flagRate:rate(count(neg,flags),neg.length),reviewAttempts:count(neg,r=>r.status==='REVIEW'),reviewOutputs:new Set(neg.filter(r=>r.status==='REVIEW').map(r=>r.id)).size,rejectedOutputs:new Set(neg.filter(r=>r.status==='FAIL').map(r=>r.id)).size},
  stability:{plannedPairs:rows.length/2,validPairs:pairs.length,agreements:pairs.filter(Boolean).length,agreement:rate(pairs.filter(Boolean).length,pairs.length),materialDisagreements:changes,repeatedSilentEscapeIds:bothPass},
  evidence:{totalFindings:checked.length,adjudicated:count(checked,f=>typeof f.relevant==='boolean'),actionableFindings:count(checked,f=>f.actionable),materialFindings:material.length,actionableMaterial:count(material,f=>f.actionable),actionableMaterialRate:rate(count(material,f=>f.actionable),material.length)},
  latencySeconds:{mean:rate(latency.reduce((a,b)=>a+b,0),latency.length),min:latency[0]??null,p50:pct(.5),p90:pct(.9),p95:pct(.95),max:latency.at(-1)??null},workload};
}
if(process.argv.includes('--inspect')){
 for(const f of findings([...primary,...regression]))console.log(JSON.stringify(f));
 process.exit(0);
}
if(findings([...primary,...regression]).some(f=>typeof f.relevant!=='boolean'))throw Error('Missing finding review');
csv('narrative-run-manifest.csv',narrative);
csv('validator-run-manifest.csv',primary.map(({label,...row})=>row));
csv('scores.csv',primary.map(({dir,...row})=>({...row,flag:flags(row)})));
csv('finding-scores.csv',findings([...primary,...regression]));
csv('supplementary-scores.csv',regression);
const results={subjects:new Set(truth.map(row=>row.subject)).size,narratives:truth.length,F0:count(truth,r=>r.label==='F0'),F1:count(truth,r=>r.label==='F1'),F2:count(truth,positive),F2Subjects:new Set(truth.filter(positive).map(r=>r.subject)).size,narrativeAttempts:narrative.length,primary:metrics(primary),supplementary:metrics(regression)};
fs.writeFileSync(at('metrics.json'),JSON.stringify(results,null,2)+'\n');
console.log(JSON.stringify(results,null,2));

