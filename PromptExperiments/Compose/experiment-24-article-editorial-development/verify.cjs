const {execFileSync}=require('node:child_process');
const fs=require('node:fs');
const path=require('node:path');
const {root,json,hash,has,files,read,verify,output,reviewDir}=require('./run.cjs');
verify();
const plan=json('plan.json');
const conditions={A:'A-production',B:'B-supported-development',C:'C-writing-strategy'};
const selection=json('development-selection.json');
const freeze=json('freeze.json');
const ids=new Set(plan.map(task=>task.blindId));
if(ids.size!==plan.length) throw Error('Duplicate blinding IDs.');
for (const task of plan) {
  const dir=output(task);
  const attempt=json(dir+'/attempt.json');
  const start=json(dir+'/started.json');
  const fixture='fixtures/'+task.subject+'/';
  const article=json(fixture+'article.json');
  const narrative=json(fixture+'narrative.json');
  if(article.composition.format!=='Article'||article.narrative.content!==narrative.content) throw Error('Invalid fixture '+fixture);
  if(attempt.promptHash!==hash('prompts/'+conditions[task.condition]+'/Compose.article.user.prompt.md')||
      attempt.narrativeHash!==hash(fixture+'narrative.json')||attempt.inputHash!==hash(fixture+'article.json')||
      attempt.model!==json('model-configuration.json').generationModel||attempt.condition!==task.condition||
      attempt.blindId!==task.blindId||start.blindId!==task.blindId||attempt.started<freeze.at||
      (task.phase==='holdout'&&attempt.started<selection.at)) throw Error('Invalid generation provenance '+dir);
  if(attempt.valid&&attempt.outputHash!==hash(dir+'/result.json')) throw Error('Invalid generation bytes '+dir);
  for(const [kind,pass] of [['editorial',1],['factual',1],['factual',2]]) {
    const review=reviewDir(task,kind,pass);
    if(!has(review+'/attempt.json')) {
      if(attempt.valid&&pass===1) throw Error('Missing review invocation '+review);
      continue;
    }
    const record=json(review+'/attempt.json');
    const request=read(review+'/request.txt');
    const rubric=read(kind+'.prompt.md').trim();
    if(!request.startsWith(rubric+'\n\n')) throw Error('Review instructions changed '+review);
    const data=JSON.parse(request.slice(rubric.length+2));
    const expected=kind==='editorial'?['audience','content','format']:['content','narrative','source','upstream'];
    if(!Object.keys(data).every(key=>expected.includes(key))||data.content!==json(dir+'/result.json').content||
      record.blindId!==task.blindId||record.requestHash!==hash(review+'/request.txt')||
      record.responseHash!==hash(review+'/stdout.log')) throw Error('Blinding or review link failure '+review);
  }
}
const examples=[
  ['holdout/hope-diamond-mail-2-A','The postage cost $2.44, and registration cost $142.85.'],
  ['holdout/highways-1-B','The 1944 Act authorized designation of 40,000 miles for a national interstate network.'],
  ['holdout/stonewall-park-2-B','The events took place around the inn, the park, and the surrounding area.']
];
for (const [id,excerpt] of examples) {
  if(!json('outputs/'+id+'/result.json').content.includes(excerpt)) throw Error('Report excerpt mismatch '+id);
}
const started=files('outputs').filter(file=>file.endsWith('/started.json'));
if(started.length!==plan.length) throw Error('Unexpected generation starts.');
for (const file of files('reviews').filter(file=>file.endsWith('/started.json'))) {
  if(!has(file.replace('started.json','attempt.json'))) throw Error('Incomplete review attempt '+file);
}
const existing=execFileSync('git',['diff','--name-only','HEAD','--','src'],{cwd:root,encoding:'utf8'}).trim();
if(existing) throw Error('Unexpected tracked production changes '+existing);
const blob=execFileSync('git',['show','HEAD:src/Nueyon.Compose.Application/Prompts/Compose/Compose.article.user.prompt.md'],{cwd:root});
if(!blob.equals(fs.readFileSync(path.join(root,'prompts/A-production/Compose.article.user.prompt.md')))) throw Error('Baseline blob mismatch.');
console.log('Verified '+plan.length+' generation slots, frozen bytes, prospective selection, review blinding and production boundaries.');

