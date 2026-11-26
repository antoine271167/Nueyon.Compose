const fs = require('node:fs');
const path = require('node:path');
const os = require('node:os');
const { createHash, randomUUID } = require('node:crypto');
const { execFile, execFileSync } = require('node:child_process');

const root = __dirname;
const repo = path.resolve(root, '../../..');
const resolve = file => path.resolve(root, file);
const has = file => fs.existsSync(resolve(file));
const read = file => fs.readFileSync(resolve(file), 'utf8').replace(/^\uFEFF/, '');
const json = file => JSON.parse(read(file));
const create = (file, data) => {
  fs.mkdirSync(path.dirname(resolve(file)), { recursive: true });
  fs.writeFileSync(resolve(file), data, { flag: 'wx' });
};
const write = (file, data) => create(file, JSON.stringify(data, null, 2) + '\n');
const copy = (from, to) => create(to, fs.readFileSync(resolve(from)));
const hash = file => createHash('sha256').update(fs.readFileSync(resolve(file))).digest('hex');
const files = dir => fs.readdirSync(resolve(dir), { recursive: true, withFileTypes: true })
  .filter(entry => entry.isFile()).map(entry => path.relative(root, path.join(entry.parentPath, entry.name)).replaceAll('\\', '/'));
const previous = '../experiment-23-compose-quality-baseline';
const sourcePrompts = '../../../src/Nueyon.Compose.Application/Prompts';
const prompt = 'Compose/Compose.article.user.prompt.md';
const app = resolve('../../../src/Nueyon.Compose.Host.Console/bin/Debug/net10.0/Nueyon.Compose.Host.Console.dll');
const model = 'gpt-6-luna';
const conditions = { A: 'A-production', B: 'B-supported-development', C: 'C-writing-strategy' };
const development = ['acid','airmf','fusion','gps','labor','grace'];
const archived = ['coral','digital','highways','python','webb','wolf'];
const supplements = ['eia','faa'];
const stress = ['digital','webb','wolf', ...supplements];
const fixture = (subject, file) => 'fixtures/' + subject + '/' + file;
const variant = condition => 'prompts/' + conditions[condition] + '/Compose.article.user.prompt.md';
const preflight = file => '../../../src/Nueyon.Compose.Host.Console/' + file;

function prepare() {
  const checkout = fs.readFileSync(resolve(sourcePrompts + '/' + prompt));
  const baseline = execFileSync('git', ['show','HEAD:src/Nueyon.Compose.Application/Prompts/' + prompt], {cwd:repo});
  const base = baseline; if (checkout.toString().replaceAll('\r\n','\n') !== baseline.toString().replaceAll('\r\n','\n')) throw Error('The working Article prompt differs semantically from committed bytes.');
  const additions = {
    B: 'Open with the most interesting concrete detail or distinction the Narrative supports. Give each paragraph a different job: develop the central idea through specific evidence, explain a distinction already present, or make an explicitly established connection clear. Vary sentence and paragraph rhythm. Once a point is made, move to the next supported development. Let the final relevant detail or supported endpoint carry the ending. Use only the material available, even when that makes for a short article.',
    C: 'Before drafting, privately choose the central thread and the strongest concrete material that serves it. Decide which detail can open the piece and what the reader needs to learn next. Order the selected material into distinct steps, using only facts, distinctions and relationships already established in the Narrative. Choose where the piece can naturally stop once that thread is clear. Write continuous, natural prose in the form that fits the material. Return only the article, with no plan or checklist.'
  };
  const baselineText = base.toString('utf8');
  const marker = '## ARTICLE QUALITY';
  if (!baselineText.includes(marker)) throw Error('Missing insertion point.');
  create(variant('A'), base);
  for (const condition of ['B','C']) {
    const newline = baselineText.includes('\r\n') ? '\r\n' : '\n';
    create(variant(condition), baselineText.replace(marker, marker + newline + newline + additions[condition]));
  }
  for (const type of ['editorial','factual']) copy(previous+'/'+type+'.prompt.md',type+'.prompt.md');
  const historic = Object.fromEntries(read(previous+'/dataset-manifest.csv').trim().split(/\r?\n/).slice(1)
    .map(line => line.split(',').slice(0,2)));
  const dataset = [];
  for (const subject of [...development,...archived]) {
    for (const file of ['source.txt','upstream.json','narrative.json','article.json'])
      copy(previous+'/fixtures/'+subject+'/'+file, fixture(subject,file));
    dataset.push({subject,phase:development.includes(subject)?'development':'holdout',cohort:stress.includes(subject)?'stress':'archived',richness:historic[subject],method:'Experiment 23 byte-for-byte'});
  }
  for (const subject of supplements) {
    const source = '../../Narrative/experiment-16-semantic-fidelity/cases/'+subject+'/';
    copy(source+'source.md', fixture(subject,'source.txt'));
    copy(source+'cli-input.json', fixture(subject,'upstream.json'));
    dataset.push({subject,phase:'holdout',cohort:'stress',richness:'constrained',method:'Production Narrative from archived Experiment 16 upstream'});
  }
  for (const [subject,richness,domain,urls,evidence,angle] of json('fresh-sources.json')) {
    create(fixture(subject,'source.txt'), urls+'\n\nEvidence summary (paraphrased):\n'+evidence+'\n');
    write(fixture(subject,'upstream.json'), {
      selectedIdea:{title:subject.replaceAll('-',' '),description:angle,audience:'Curious general readers',rationale:angle,evidence},
      synthesis:{content:'Central thread\n'+angle+'\n\nEvidence\n'+evidence}
    });
    dataset.push({subject,phase:'holdout',cohort:'fresh',richness,domain,method:'Production Narrative from source-backed hand-prepared Idea and Synthesis'});
  }
  write('dataset.json',dataset);
  const columns = ['subject','phase','cohort','richness','domain','method'];
  create('dataset-manifest.csv',columns.join(',')+'\n'+dataset.map(row=>columns.map(key=>row[key]??'').join(',')).join('\n')+'\n');
  write('model-configuration.json',{
    sourceCommit:execFileSync('git',['rev-parse','HEAD'],{cwd:repo,encoding:'utf8'}).trim(),generationModel:model,reviewModel:model,
    runtime:'Production CLI using OpenAI and Microsoft Agent Framework; appsettings and machine credentials; provider defaults',
    resolvedModelBuild:null, tokenUsageAvailable:false, costAvailable:false,
    dotnet:'10.0.404',node:process.version,reviewRuntime:'GitHub Copilot noninteractive, tools and custom instructions disabled'
  });
}
function call(binary,args,cwd) {
  const started = new Date().toISOString();
  const beginning = Date.now();
  return new Promise(result => execFile(binary,args,{cwd,timeout:240000,windowsHide:true,maxBuffer:8*1024*1024},
    (error,stdout,stderr)=>result({started,seconds:(Date.now()-beginning)/1000,exit:error ? error.code??null : 0,signal:error?.signal??null,stdout,stderr})));
}
async function archive(dir,entry,invoke,inspect) {
  if (has(dir+'/started.json')) {
    if (!has(dir+'/attempt.json')) throw Error('Unresolved prior attempt '+dir);
    return json(dir+'/attempt.json');
  }
  write(dir+'/started.json',entry);
  const {stdout,stderr,...operational} = await invoke();
  create(dir+'/stdout.log',stdout);
  create(dir+'/stderr.log',stderr);
  const record = {...entry,...operational,...inspect(operational.exit,stdout)};
  write(dir+'/attempt.json',record);
  console.log(dir,record.valid?'valid':'invalid',record.seconds);
  return record;
}
async function pool(items,worker,width) {
  let index = 0;
  await Promise.all(Array.from({length:width},async()=>{
    while(index < items.length) await worker(items[index++]);
  }));
}
async function narratives() {
  await pool(json('dataset.json').filter(row=>row.method!=='Experiment 23 byte-for-byte'),async ({subject})=>{
    const dir='preparation/'+subject;
    const result=dir+'/result.json';
    const input=fixture(subject,'upstream.json');
    const attempt=await archive(dir,{subject,model,inputHash:hash(input)},
      ()=>call('dotnet',[app,'run','narrative','--input',resolve(input),'--output',resolve(result)],repo),
      exit=>({valid:exit===0&&has(result)&&typeof json(result).content==='string'&&!!json(result).content.trim()}));
    if (!attempt.valid) return;
    if (!has(fixture(subject,'narrative.json'))) copy(result,fixture(subject,'narrative.json'));
    if (!has(fixture(subject,'article.json'))) write(fixture(subject,'article.json'),{narrative:{content:json(result).content},composition:{format:'Article'}});
  },4);
}
function freeze() {
  const dataset=json('dataset.json');
  const tasks=[];
  for(const phase of ['development','holdout']) {
    const rows=dataset.filter(row=>row.phase===phase);
    for(let rep=1;rep<=(phase==='development'?2:3);rep++) {
      const order=rep===2?[...rows].reverse():rows;
      order.forEach((row,index)=>{
        ['A','B','C'].map((_,offset)=>['A','B','C'][(offset+index+rep-1)%3]).forEach(condition=>{
          tasks.push({...row,rep,condition,blindId:randomUUID()});
        });
      });
    }
  }
  write('plan.json',tasks);
  const freezeFiles=['run.cjs','protocol.md','hypothesis.md','fresh-sources.json','dataset.json','dataset-manifest.csv','model-configuration.json',
    'plan.json','editorial.prompt.md','factual.prompt.md',...files('fixtures'),...Object.keys(conditions).map(variant),
    ...files(sourcePrompts),preflight('appsettings.json')];
  for(const {subject} of dataset) if (!has(fixture(subject,'article.json'))) throw Error('Missing Article fixture '+subject);
  write('freeze.json',{at:new Date().toISOString(),hashes:Object.fromEntries(freezeFiles.map(file=>[file,hash(file)]))});
}
function verify() {
  for(const [file,expected] of Object.entries(json('freeze.json').hashes)) {
    if(hash(file)!==expected) throw Error('Frozen bytes changed: '+file);
  }
  const tasks=json('plan.json');
  if(tasks.some(t=>tasks.some(other=>other.subject===t.subject&&other.phase!==t.phase))) throw Error('Subject leakage.');
}
const output = task => 'outputs/'+task.phase+'/'+task.subject+'-'+task.rep+'-'+task.condition;
const reviewDir = (task,type,pass) => 'reviews/'+type+'/'+task.phase+'/'+task.blindId+'-'+pass;
async function generate(phase) {
  if(phase==='holdout'&&!has('development-selection.json')) throw Error('Freeze development selection first.');
  const temp=fs.mkdtempSync(path.join(os.tmpdir(),'exp24-prompts-'));
  for(const condition of Object.keys(conditions)) {
    const dest=path.join(temp,condition);
    fs.cpSync(resolve(sourcePrompts),dest,{recursive:true});
    fs.copyFileSync(resolve(variant(condition)),path.join(dest,prompt));
  }
  await pool(json('plan.json').filter(task=>task.phase===phase),async task=>{
    const dir=output(task);
    const input=fixture(task.subject,'article.json');
    const result=dir+'/result.json';
    await archive(dir,{...task,model,runtime:'model-configuration.json',promptHash:hash(variant(task.condition)),narrativeHash:hash(fixture(task.subject,'narrative.json')),inputHash:hash(input)},
      ()=>call('dotnet',[app,'run','compose','--input',resolve(input),'--output',resolve(result),'--prompts',path.join(temp,task.condition)],repo),
      (exit)=>({valid:exit===0&&has(result)&&typeof json(result).content==='string'&&!!json(result).content.trim(),outputHash:has(result)?hash(result):null}));
  },6);
  fs.rmSync(temp,{recursive:true});
}
function parseReview(stdout) {
  const raw=stdout.trim();
  return JSON.parse(raw.startsWith(String.fromCharCode(96).repeat(3))?raw.slice(raw.indexOf('\n')+1,raw.lastIndexOf(String.fromCharCode(96).repeat(3))):raw);
}
async function review(phase,type,pass) {
  const loader=process.env.EXP24_COPILOT_LOADER;
  if(!loader) throw Error('Set EXP24_COPILOT_LOADER.');
  const items=json('plan.json').filter(task=>task.phase===phase&&has(output(task)+'/attempt.json')&&json(output(task)+'/attempt.json').valid);
  await pool(items,async task=>{
    const primary=reviewDir(task,type,1);
    if(pass===2&&task.rep!==2&&has(primary+'/attempt.json')&&json(primary+'/attempt.json').valid&&
        !json(primary+'/parsed.json').claims.some(claim=>['F2','U'].includes(claim.label)||claim.narrativeRelation!=='supports')) return;
    const dir=reviewDir(task,type,pass);
    const content=json(output(task)+'/result.json').content;
    const upstream=json(fixture(task.subject,'upstream.json'));
    const payload=type==='editorial'?{format:'Article',audience:upstream.selectedIdea?.Audience??upstream.selectedIdea?.audience,content}:
      {content,narrative:json(fixture(task.subject,'narrative.json')).content,upstream,source:read(fixture(task.subject,'source.txt'))};
    const request=read(type+'.prompt.md').trim()+'\n\n'+JSON.stringify(payload);
    if(!has(dir+'/request.txt')) create(dir+'/request.txt',request);
    await archive(dir,{blindId:task.blindId,type,pass,model,requestHash:hash(dir+'/request.txt')},
      ()=>call(process.execPath,[loader,'--model',model,'--silent','--no-custom-instructions','--disable-builtin-mcps','--available-tools','experiment24_no_tools','-p',request],os.tmpdir()),
      (exit,stdout)=>{
        let valid=false;
        try {
          const parsed=parseReview(stdout);
          write(dir+'/parsed.json',parsed);
          valid=exit===0&&(type==='editorial'?parsed.scores.length===6&&parsed.scores.every(s=>Number.isInteger(s)&&s>=1&&s<=5)&&Array.isArray(parsed.defects):
            parsed.claims.length>0&&parsed.claims.every(claim=>['F0','F1','F2','U'].includes(claim.label)&&claim.excerpt&&content.includes(claim.excerpt)));
        } catch {}
        return {valid,responseHash:hash(dir+'/stdout.log')};
      });
  },10);
}
async function main() {
  const [command,phase,type,pass]=process.argv.slice(2);
  if(command==='prepare') return prepare();
  if(command==='narratives') return narratives();
  if(command==='freeze') return freeze();
  verify();
  if(command==='generate') return generate(phase);
  if(command==='review') return review(phase,type,Number(pass??1));
}
module.exports={root,read,json,write,has,hash,files,output,reviewDir,verify};
if(require.main===module) main().catch(error=>{console.error(error.message);process.exitCode=1;});

