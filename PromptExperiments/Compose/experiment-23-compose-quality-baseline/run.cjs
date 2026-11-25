const fs = require('node:fs');
const path = require('node:path');
const { createHash } = require('node:crypto');
const { execFile, execFileSync } = require('node:child_process');
const root = __dirname;
const repo = path.resolve(root, '../../..');
const at = file => path.resolve(root, file);
const read = file => fs.readFileSync(at(file), 'utf8').replace(/^\uFEFF/, '');
const json = file => JSON.parse(read(file));
const save = (file, value) => fs.writeFileSync(at(file), value, { flag: 'wx' });
const write = (file, value) => save(file, JSON.stringify(value, null, 2) + '\n');
const sha = file => createHash('sha256').update(fs.readFileSync(at(file))).digest('hex');
const exists = file => fs.existsSync(at(file));
const mkdir = file => fs.mkdirSync(at(file), { recursive: true });
const model = 'gpt-6-luna';
const e19 = '../../Narrative/experiment-19-real-world-semantic-validation';
const e20 = '../../Narrative/experiment-20-fidelity-judge-repair-loop';
const src = '../../../src';
const app = at(src + '/Nueyon.Compose.Host.Console/bin/Debug/net10.0/Nueyon.Compose.Host.Console.dll');
const cases = {
  acid: ['rich'], airmf: ['constrained'], coral: ['constrained'], digital: ['rich', '166274757454'],
  fusion: ['constrained'], gps: ['rich'], grace: ['rich'], highways: ['rich'],
  labor: ['constrained'], python: ['constrained'], webb: ['constrained', '0d1f75b51c'], wolf: ['rich', '2ff42e2fa4']
};
const subjects = Object.keys(cases);
const tasks = [1, 2, 3].flatMap(rep => (rep === 2 ? [...subjects].reverse() : subjects).flatMap((subject, i) =>
  ((i + rep) % 2 ? ['Article', 'LinkedInPost'] : ['LinkedInPost', 'Article']).map(format => ({
    id: subject + '-' + rep, subject, rep, format, cohort: cases[subject][0],
    folder: format === 'Article' ? 'article' : 'linkedin'
  }))));
const out = task => 'outputs/' + task.folder + '/' + task.id;
const fixture = task => 'fixtures/' + task.subject;
const input = task => fixture(task) + '/' + task.folder + '.json';
const reviewDir = (t, type, pass) => 'reviews/' + type + '/' + t.folder + '/' + t.id + '-' + pass;
function freeze() {
  const manifest = ['subject,richness,archived_source,archived_upstream,archived_narrative'];
  for (const subject of subjects) {
    const [cohort, id] = cases[subject];
    const base = id ? e20 + '/cases/archived/' + id : e19 + '/cases/' + subject;
    const source = base + '/source.txt';
    const upstream = base + (id ? '/input.json' : '/cli-input.json');
    const narrative = id ? base + '/initial.json' : e19 + '/narrative-runs/' + subject + '-1/result.json';
    const dir = fixture({subject});
    mkdir(dir);
    for (const [from, to] of [[source,'source.txt'],[upstream,'upstream.json'],[narrative,'narrative.json']])
      save(dir + '/' + to, fs.readFileSync(at(from)));
    for (const task of tasks.filter(task => task.subject === subject && task.rep === 1))
      write(input(task), {narrative:{content:json(narrative).content},composition:{format:task.format}});
    manifest.push([subject,cohort,source,upstream,narrative].join(','));
  }
  for (const folder of ['article','linkedin']) mkdir('outputs/' + folder);
  for (const type of ['editorial','factual']) mkdir('reviews/' + type);
  save('dataset-manifest.csv', manifest.join('\n') + '\n');
  write('plan.json', tasks);
  write('model-configuration.json', {
    sourceCommit: execFileSync('git',['rev-parse','HEAD'],{cwd:repo,encoding:'utf8'}).trim(),
    generationModel:model, reviewModel:model, resolvedBuild:null, dotnet:'10.0.404', node:process.version, copilot:'1.0.116',
    generation:'Production OpenAI Chat SDK via Microsoft Agent Framework, appsettings.json and machine configuration, provider defaults',
    review:'Unattended GitHub Copilot CLI, no tools or custom instructions'
  });
  const files = ['run.cjs','protocol.md','editorial.prompt.md','factual.prompt.md','dataset-manifest.csv','source-provenance.md',
    'plan.json','model-configuration.json', ...subjects.flatMap(s =>
      ['source.txt','upstream.json','narrative.json','article.json','linkedin.json'].map(n => 'fixtures/'+s+'/'+n)),
    ...['system','article.user','linkedin.user'].map(s => src+'/Nueyon.Compose.Application/Prompts/Compose/Compose.'+s+'.prompt.md'),
    src+'/Nueyon.Compose.Host.Console/appsettings.json'];
  write('freeze.json',{time:new Date().toISOString(),hashes:Object.fromEntries(files.map(file=>[file,sha(file)]))});
}
function verify() {
  for(const [file, expected] of Object.entries(json('freeze.json').hashes))
    if(sha(file)!==expected) throw Error('Frozen file changed: '+file);
}
function call(bin,args,cwd) {
  const start = new Date().toISOString();
  const begin = Date.now();
  return new Promise(resolve => execFile(bin,args,{cwd,timeout:240000,maxBuffer:8*1024*1024,windowsHide:true,encoding:'buffer'},
    (err,stdout,stderr)=>resolve({stdout,stderr,start,seconds:(Date.now()-begin)/1000,
      exit:err ? err.code ?? null : 0,signal:err?.signal??null,killed:err?.killed??false})));
}
async function archive(dir, entry, launch, inspect) {
  if(exists(dir+'/attempt.json')) return json(dir+'/attempt.json');
  mkdir(dir);
  write(dir+'/started.json',entry);
  const {stdout,stderr,...meta}=await launch();
  save(dir+'/stdout.log',stdout);
  save(dir+'/stderr.log',stderr);
  const record={...entry,...meta,...inspect(meta)};
  write(dir+'/attempt.json',record);
  console.log(dir,record.valid?'valid':record.failure??'invalid',record.seconds);
  return record;
}
function generate(task) {
  const dir=out(task);
  const result=dir+'/result.json';
  return archive(dir,{...task,inputSha256:sha(input(task))},
    ()=>call('dotnet',[app,'run','compose','--input',at(input(task)),'--output',at(result)],repo),
    ({exit})=>{
      let valid=false;
      try {valid=exit===0 && !!json(result).content.trim();} catch {}
      const error=read(dir+'/stderr.log');
      const failure=valid?null:error.match(/HTTP \d{3}|model_not_found|insufficient_quota|API key|response content is empty/i)?.[0]??'unclassified';
      return {valid,failure,outputSha256:exists(result)?sha(result):null};
    });
}
function invokeReview(prompt) {
  const loader=process.env.EXP23_COPILOT_LOADER;
  if(!loader) throw Error('Set EXP23_COPILOT_LOADER');
  return call(process.execPath,[loader,'--model',model,'--silent','--no-custom-instructions','--disable-builtin-mcps',
    '--available-tools','experiment23_no_tools','-p',prompt],require('node:os').tmpdir());
}
function review(task,type,pass) {
  const dir=reviewDir(task,type,pass);
  if(exists(dir+'/attempt.json')) return json(dir+'/attempt.json');
  const content=json(out(task)+'/result.json').content;
  const upstream=json(fixture(task)+'/upstream.json');
  const payload=type==='editorial'?{format:task.format,audience:upstream.selectedIdea?.Audience??upstream.selectedIdea?.audience,content}:
    {content,narrative:json(fixture(task)+'/narrative.json').content,upstream,source:read(fixture(task)+'/source.txt')};
  const prompt=read(type+'.prompt.md').trim()+'\n\n'+JSON.stringify(payload);
  mkdir(dir);
  save(dir+'/request.txt',prompt);
  return archive(dir,{...task,type,pass,requestSha256:sha(dir+'/request.txt')},()=>invokeReview(prompt),({exit})=>{
    let valid=false;
    try {
      const fence=String.fromCharCode(96).repeat(3);
      const raw=read(dir+'/stdout.log').trim();
      const result=JSON.parse(raw.startsWith(fence)?raw.slice(raw.indexOf('\n')+1,raw.lastIndexOf(fence)):raw);
      write(dir+'/parsed.json',result);
      valid=exit===0 && (type==='editorial'?result.scores.length===6&&result.scores.every(s=>Number.isInteger(s)&&s>=1&&s<=5)&&Array.isArray(result.defects):
        Array.isArray(result.claims)&&result.claims.length>0&&result.claims.every(c=>['F0','F1','F2','U'].includes(c.label)&&c.excerpt&&content.includes(c.excerpt)));
    } catch {}
    return {valid,responseSha256:sha(dir+'/stdout.log')};
  });
}
async function batches(items,fn,stop) {
  for(let i=0;i<items.length;i+=4) {
    const group=await Promise.all(items.slice(i,i+4).map(fn));
    if(stop?.(group)) break;
  }
}
async function main() {
  const stage=process.argv[2];
  if(stage==='freeze') return freeze();
  verify();
  if(stage==='verify') return;
  if(stage==='generate') return batches(tasks,generate,g=>g.length===4&&g.every(r=>!r.valid&&r.failure===g[0].failure&&
    /api key|model_not_found|insufficient_quota|HTTP (401|403|404|429)/i.test(r.failure)));
  if(stage==='probe') {
    const result=await invokeReview('Reply only READY.');
    console.log(result.stdout.toString(),result.stderr.toString(),result.exit);
    return;
  }
  const valid=tasks.filter(t=>exists(out(t)+'/attempt.json')&&json(out(t)+'/attempt.json').valid);
  if(['editorial','factual'].includes(stage)) return batches(valid,t=>review(t,stage,1));
  if(stage==='second') return batches(valid.filter(t=>{
    const dir=reviewDir(t,'factual',1);
    return t.rep===2||!exists(dir+'/attempt.json')||!json(dir+'/attempt.json').valid||
      json(dir+'/parsed.json').claims.some(c=>['F2','U'].includes(c.label));
  }),t=>review(t,'factual',2));
  throw Error('Unknown stage: '+stage);
}
if(require.main===module)main().catch(error=>{console.error(error);process.exitCode=1;});
module.exports={at,json,read,sha,exists,out,input,reviewDir,verify,tasks,subjects};
