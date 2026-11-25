const fs=require('node:fs');
const path=require('node:path');
const assert=require('node:assert/strict');
const {execFileSync}=require('node:child_process');
const {at,json,read,sha,exists,out,reviewDir,verify,tasks}=require('./run.cjs');
const {readReview}=require('./review-records.cjs');
const repo=at('../../..');
const base=json('model-configuration.json').sourceCommit;
const list=dir=>fs.readdirSync(at(dir),{recursive:true,withFileTypes:true}).filter(e=>e.isFile())
  .map(e=>path.relative(at('.'),path.join(e.parentPath,e.name)).replaceAll('\\','/'));
const generated=[];
const archived=[];
verify();
for(const t of tasks){
  const dir=out(t);
  const a=json(dir+'/attempt.json');
  assert.equal(a.id,t.id);
  assert.equal(a.format,t.format);
  assert.equal(exists(dir+'/result.json'),a.valid);
  archived.push(dir);
  if(!a.valid)continue;
  generated.push(a.outputSha256);
  for(const kind of ['editorial','factual']){
    const d=reviewDir(t,kind,1);
    assert.equal(json(d+'/attempt.json').id,t.id);
    archived.push(d);
  }
  assert.ok(readReview(t,'editorial',1).usable);
  const first=readReview(t,'factual',1);
  const second=readReview(t,'factual',2);
  if(t.rep===2||!first.usable||first.data.claims.some(c=>['F2','U'].includes(c.label)))assert.ok(second,t.id);
  if(second)archived.push(reviewDir(t,'factual',2));
  assert.ok(first.usable||second?.usable,t.id);
}
assert.equal(generated.length,new Set(generated).size,'Duplicate valid body artifacts');
assert.equal(archived.length,new Set(archived).size,'Duplicate attempt directories');
const paths=[...list('outputs'),...list('reviews')];
assert.equal(paths.filter(f=>f.endsWith('/attempt.json')).length,archived.length);
assert.equal(paths.filter(f=>f.endsWith('/started.json')).length,archived.length);
for(const dir of archived){
  for(const file of ['attempt.json','started.json','stdout.log','stderr.log'])assert.ok(exists(dir+'/'+file),dir+'/'+file);
}
for(const file of list('.').filter(f=>f.endsWith('.md'))){
  for(const [,target] of read(file).matchAll(/\]\(([^)\s]+)\)/g)){
    if(/^(https?:|#)/.test(target))continue;
    const p=path.resolve(at(file), '..',target.split('#')[0]);
    assert.ok(fs.existsSync(p),file+' -> '+target);
  }
}
const findings=json('reviews/factual/adjudications.json').filter(f=>f.label==='F2');
for(const f of findings){
  const source=read(f.source);
  for(const excerpt of f.evidence.split(/(?<=deployed\.) /))assert.ok(source.includes(excerpt),f.output+' source');
  const subject=f.output.split('/')[1].replace(/-\d$/,'');
  assert.ok(json('fixtures/'+subject+'/narrative.json').content.includes(f.narrative),f.output+' narrative');
}
const and=['src','PromptExperiments/Narrative'];
execFileSync('git',['diff','--exit-code',base,'--',...and],{cwd:repo,stdio:'pipe'});
const all=list('.').filter(f=>f!=='artifact-hashes.json').sort();
const fingerprints=Object.fromEntries(all.map(file=>[file,sha(file)]));
const ledger=at('artifact-hashes.json');
if(fs.existsSync(ledger))assert.deepEqual(json('artifact-hashes.json'),fingerprints);
else fs.writeFileSync(ledger,JSON.stringify(fingerprints,null,2)+'\n');
console.log('Verified',tasks.length,'generation slots,',generated.length,'distinct outputs,',archived.length,'completed attempts,',findings.length,'material findings,',all.length,'asset hashes');
