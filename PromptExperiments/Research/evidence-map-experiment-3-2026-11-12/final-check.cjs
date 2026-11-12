const fs=require('fs'),p=require('path'),crypto=require('crypto');const out=__dirname,root=p.resolve(out,'../../..');
const read=f=>JSON.parse(fs.readFileSync(f,'utf8').replace(/^\uFEFF/,'')),hash=f=>crypto.createHash('sha256').update(fs.readFileSync(f)).digest('hex');
const usage=read(p.join(out,'usage.json')),checks=read(p.join(out,'verification-requests.json')),results=read(p.join(out,'results.json'));
if(usage.attempts!==18||usage.rows.length!==18||checks.length!==18||checks.some(c=>Object.values(c).includes(false)))throw Error('Ledger/request mismatch');
if(usage.rows.some(r=>r.model!=='gpt-6-luna'||r.status!==200))throw Error('Unexpected provider/model');
if(read(p.join(out,'verification-freeze.json')).changed.length)throw Error('Freeze changed');
const old=p.join(out,'../judge-calibration-experiment-2-2026-11-12');
const matched=['baseline-intro','baseline-005','baseline-010','control-intro','control-010','control-005-complete'];
const oldRows=read(p.join(old,'usage.json')).calls.filter(r=>matched.includes(r.id));
const oldCost=oldRows.reduce((a,r)=>a+r.usage.total_tokens,0);
if(oldCost!==results.experiment2MatchedHistorical.totalTokens)throw Error('Comparison cost mismatch');
let fixtures=[];for(const id of ['intro','005','010'])for(const variant of ['bad','correct']){
 const oldId=variant==='bad'?'baseline-'+id:id==='005'?'control-005-complete':'control-'+id;
 for(const name of ['input.json','research-result.json']){
  const same=hash(p.join(out,'cases',id,variant,name))===hash(p.join(old,'cases',oldId,name));fixtures.push({id,variant,name,byteEqualToExperiment2:same});if(!same)throw Error('Historical fixture changed');
 }
}
let runtime=read(p.join(out,'runtime-snapshot/manifest.json')).map(r=>({file:r.File,snapshotSha256:r.Sha256.toLowerCase(),runnerCopyStillMatches:hash(p.join(out,'runner/bin/Debug/net10.0',r.File))===r.Sha256.toLowerCase()}));
if(runtime.some(r=>!r.runnerCopyStillMatches))throw Error('Runner dependency changed after snapshot');
fs.writeFileSync(p.join(out,'verification-final.json'),JSON.stringify({checkedAt:new Date().toISOString(),calls:18,maxCalls:18,retries:0,matchedOldCost:oldCost,fixtures,runtime,decision:results.decision},null,2));
let manifest={};function walk(dir){for(const e of fs.readdirSync(dir,{withFileTypes:true})){if(['bin','obj'].includes(e.name))continue;const f=p.join(dir,e.name);if(e.isDirectory())walk(f);else if(e.name!=='artifact-manifest.json')manifest[p.relative(out,f).replaceAll('\\','/')]=hash(f);}}walk(out);
fs.writeFileSync(p.join(out,'artifact-manifest.json'),JSON.stringify(manifest,null,2));
console.log('Final checks passed: 18 calls, frozen files unchanged, 12 historical fixture files byte-identical, runner dependencies stable, cost comparison verified.');
