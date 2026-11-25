const fs = require('node:fs');
const path = require('node:path');
const {createHash} = require('node:crypto');
const {execFile, execFileSync} = require('node:child_process');

const root = __dirname;
const repo = path.resolve(root, '../../..');
const app = path.join(repo, 'src/Nueyon.Compose.Host.Console/bin/Debug/net10.0/Nueyon.Compose.Host.Console.dll');
const production = '../../../src/Nueyon.Compose.Application/Prompts/Narrative';
const settings = '../../../src/Nueyon.Compose.Host.Console/appsettings.json';
const at = name => path.join(root, name);
const read = name => fs.readFileSync(at(name), 'utf8').replace(/^\uFEFF/, '');
const load = name => JSON.parse(read(name));
const exists = name => fs.existsSync(at(name));
function save(name, value) {
  fs.mkdirSync(path.dirname(at(name)), {recursive:true});
  fs.writeFileSync(at(name), value, {flag:'wx'});
}
const json = (name, value) => save(name, JSON.stringify(value, null, 2) + '\n');
const sha = name => createHash('sha256').update(fs.readFileSync(at(name))).digest('hex');
const csv = (headers, rows) => [headers, ...rows.map(row => headers.map(key => row[key] ?? ''))]
  .map(row => row.map(cell => '"' + String(cell).replaceAll('"', '""') + '"').join(',')).join('\n') + '\n';
const categories = Object.keys(load('judge-schema.json').properties.checks.properties);
const config = load('model-configuration.json');
const stages = ['baseline','judge-runs','repair-runs','final-outputs','reviews'];

function prepare() {
  const archived = read('archived-index.csv').trim().split(/\r?\n/).slice(1).map(line => {
    const [id,subject,cohort,prior,input,source,initial,idea,synthesis] = line.slice(1,-1).split('","');
    const dir = 'cases/archived/' + id;
    const sourceFile = name => path.resolve(repo, name.replaceAll('\\','/'));
    const data = name => JSON.parse(fs.readFileSync(sourceFile(name), 'utf8').replace(/^\uFEFF/, ''));
    const contract = input ? data(input) : {selectedIdea:data(idea), synthesis:data(synthesis)};
    json(dir+'/input.json', contract);
    save(dir+'/source.txt', fs.readFileSync(sourceFile(source)));
    save(dir+'/initial.json', fs.readFileSync(sourceFile(initial)));
    return {id,subject,cohort,prior,input:dir+'/input.json',source:dir+'/source.txt',initial:dir+'/initial.json'};
  });
  const handoffs = load('new-handoffs.json');
  for (const [subject,[title,description,evidence,limits]] of Object.entries(handoffs)) {
    json('cases/'+subject+'/input.json', {
      selectedIdea:{title,description,audience:'Curious general readers',rationale:description,evidence},
      synthesis:{content:'### Central insight\n'+description+'\n\n### Evidence\n'+evidence+'\n\n### Gaps and uncertainty\n'+limits}
    });
  }
  const subjects = Object.keys(handoffs);
  const fresh = [1,2,3].flatMap(repetition => subjects.map(subject => ({
    id:subject+'-'+repetition,subject,cohort:'new',repetition,input:'cases/'+subject+'/input.json',source:'cases/'+subject+'/source.txt'
  })));
  const tasks = [...archived,...fresh];
  if (tasks.length !== 36 || new Set(tasks.map(task=>task.id)).size !== 36) throw Error('Unexpected corpus size');
  json('dataset.json',tasks);
  save('dataset-manifest.csv',csv(['id','cohort','subject','repetition','prior','input','source','initial'],tasks));
  const files = [
    'run.cjs','dataset.json','dataset-manifest.csv','archived-index.csv','new-handoffs.json','source-provenance.md',
    'protocol.md','hypothesis.md','judge.prompt.md','judge-schema.json','repair.prompt.md','review.prompt.md',
    'model-configuration.json',settings,
    ...fs.readdirSync(at(production)).map(name => production+'/'+name),
    ...tasks.flatMap(task => [task.source,task.input,task.initial].filter(Boolean))
  ];
  json('freeze.json',Object.fromEntries([...new Set(files)].map(name => [name,sha(name)])));
}

function verify() {
  for (const [file,expected] of Object.entries(load('freeze.json'))) {
    if (sha(file) !== expected) throw Error('Frozen file changed: '+file);
  }
}

const nonblank = value => typeof value === 'string' && value.trim().length > 0;
function keys(value, expected) {
  if (!value || Object.keys(value).sort().join('|') !== expected.slice().sort().join('|')) throw Error('Unexpected properties');
}
function parse(text) {
  const body = text.trim().replace(/^```(?:json)?\s*([\s\S]*?)\s*```$/i,'$1');
  const value = JSON.parse(body);
  // Repeated keys survive JSON.parse only as a single value. Count direct member tokens per object.
  const stack = [];
  const tokens = body.matchAll(/("(?:[^"\\]|\\.)*")(\s*:)?|([{}\[\]])/g);
  for (const [,string,colon,bracket] of tokens) {
    if (bracket === '{' || bracket === '[') stack.push(new Set());
    if (bracket === '}' || bracket === ']') stack.pop();
    if (colon) {
      const key = JSON.parse(string);
      if (stack.at(-1).has(key)) throw Error('Duplicate JSON key');
      stack.at(-1).add(key);
    }
  }
  return value;
}
function narrative(text, strict) {
  const value = parse(text);
  if (strict) keys(value,['content']);
  const content = value.content ?? value.Content;
  if (!nonblank(content)) throw Error('Missing Narrative content');
  return {content};
}
function excerpts(finding, input) {
  const evidence = [input.source, ...Object.values(input.selectedIdea), input.synthesis.content ?? input.synthesis.Content];
  return nonblank(finding.narrativeExcerpt) && input.narrative.content.includes(finding.narrativeExcerpt)
    && nonblank(finding.evidenceExcerpt) && evidence.some(text => typeof text === 'string' && text.includes(finding.evidenceExcerpt));
}
function judge(text,input) {
  const value = parse(text);
  keys(value,['checks','verdict','blockers']);
  keys(value.checks,categories);
  if (!Object.values(value.checks).every(nonblank) || !Array.isArray(value.blockers) || value.blockers.length > 3) throw Error('Invalid audit or blockers');
  for (const item of value.blockers) {
    keys(item,['category','status','narrativeExcerpt','evidenceExcerpt','problem','requiredCorrection']);
    if (!categories.includes(item.category) || !excerpts(item,input) || !nonblank(item.problem)) throw Error('Invalid blocker or excerpt');
    const valid = item.status === 'confirmed' ? nonblank(item.requiredCorrection) : item.status === 'uncertain' && item.requiredCorrection === null;
    if (!valid) throw Error('Invalid blocker status or correction');
  }
  const confirmed = value.blockers.some(item => item.status === 'confirmed');
  const expected = confirmed ? 'REVISE' : value.blockers.length ? 'UNCERTAIN' : 'GO';
  if (value.verdict !== expected) throw Error('Inconsistent verdict');
  return value;
}
function review(text,input) {
  const value = parse(text);
  keys(value,['fidelity','findings','editorial','rationale']);
  if (!nonblank(value.rationale) || !Array.isArray(value.editorial) || value.editorial.length !== 5 ||
      !value.editorial.every(n=>Number.isInteger(n)&&n>=1&&n<=5) || !Array.isArray(value.findings)) throw Error('Invalid review');
  for (const item of value.findings) {
    keys(item,['severity','category','narrativeExcerpt','evidenceExcerpt','explanation']);
    if (!['F1','F2'].includes(item.severity) || ![...categories,'other'].includes(item.category) ||
        !nonblank(item.explanation) || !excerpts(item,input)) throw Error('Invalid finding or excerpt');
  }
  const expected = value.findings.some(item=>item.severity==='F2') ? 'F2' : value.findings.length ? 'F1' : 'F0';
  if (value.fidelity !== expected) throw Error('Inconsistent grade');
  return value;
}

function launch(bin,args,dir) {
  const started = new Date().toISOString();
  const begin = Date.now();
  fs.mkdirSync(at(dir),{recursive:true});
  console.log('Start',dir,started);
  return new Promise(resolve=>execFile(bin,args,{cwd:repo,windowsHide:true,timeout:config.timeoutSeconds*1000,maxBuffer:4*1024*1024},
    (err,stdout,stderr) => {
      save(dir+'/stdout.log',stdout);
      save(dir+'/stderr.log',stderr);
      resolve({started,seconds:(Date.now()-begin)/1000,exitCode:err?.code??(err?null:0),signal:err?.signal??null,stdout});
    }));
}
async function attempt(dir,kind,bin,args,decode) {
  if (exists(dir+'/attempt.json')) return exists(dir+'/result.json') ? load(dir+'/result.json') : null;
  if (exists(dir+'/stdout.log')) throw Error('Interrupted archive: '+dir);
  const {stdout,...meta}=await launch(bin,args,dir);
  let result=null;
  let error=null;
  if (meta.exitCode!==0) error='transport';
  else {
    try { result=decode(stdout); }
    catch (failure) { error=(failure instanceof SyntaxError?'invalid-json: ':'invalid-contract: ')+failure.message; }
  }
  if (result) json(dir+'/result.json',result);
  json(dir+'/attempt.json',{kind,...meta,error});
  console.log('Complete',dir,meta.seconds,error??'valid');
  return result;
}
let loader;
function copilot(dir,kind,prompt,input,decode) {
  if (!exists(dir+'/request.json')) json(dir+'/request.json',input);
  const message=prompt+'\n\n'+JSON.stringify(input);
  if (!exists(dir+'/prompt.txt')) save(dir+'/prompt.txt',message);
  const args=[loader,'--model',config.model,'--silent','--no-custom-instructions','--no-ask-user','--disable-builtin-mcps','--available-tools','experiment20_no_tools','-p',message];
  return attempt(dir,kind,process.execPath,args,text=>decode(text,input));
}
function evidence(task,brief) {
  return {...load(task.input),source:read(task.source),narrative:brief};
}
function baseline(task) {
  const dir='baseline/'+task.id;
  if (task.initial) {
    if (!exists(dir+'/result.json')) save(dir+'/result.json',fs.readFileSync(at(task.initial)));
    return narrative(read(dir+'/result.json'),false);
  }
  return attempt(dir,'narrative','dotnet',[app,'run','narrative','--input',at(task.input),'--output',at(dir+'/native.json')],
    ()=>narrative(read(dir+'/native.json'),false));
}
async function loop(task) {
  const final='final-outputs/'+task.id;
  if (exists(final+'/status.json')) return;
  let current=await baseline(task);
  let repairs=0;
  let status='technical_failure';
  if (current) for (let round=0;round<=2;round++) {
    const input=evidence(task,current);
    const verdict=await copilot('judge-runs/'+task.id+'/'+round,'judge',read('judge.prompt.md')+'\n'+read('judge-schema.json'),input,judge);
    if (!verdict) break;
    if (verdict.verdict==='GO') { status=['accepted_without_repair','accepted_after_one','accepted_after_two'][repairs]; break; }
    if (verdict.verdict==='UNCERTAIN') { status='uncertain'; break; }
    if (round===2) { status='remaining_blockers'; break; }
    const blockers=verdict.blockers.filter(item=>item.status==='confirmed');
    const next=await copilot('repair-runs/'+task.id+'/'+(round+1),'repair',read('repair.prompt.md'),
      {...input,blockers},text=>narrative(text,true));
    if (!next) break;
    current=next;
    repairs++;
  }
  if (current) json(final+'/result.json',current);
  json(final+'/status.json',{id:task.id,status,repairs});
}
async function assess(task) {
  const versions=[];
  if (exists('baseline/'+task.id+'/result.json')) versions.push(narrative(read('baseline/'+task.id+'/result.json'),false));
  for (const round of [1,2]) {
    const file='repair-runs/'+task.id+'/'+round+'/result.json';
    if (exists(file)) versions.push(load(file));
  }
  for (const [version,brief] of versions.entries()) {
    const input=evidence(task,brief);
    const dir='reviews/'+task.id+'/'+version;
    const primary=await copilot(dir+'/first','review',read('review.prompt.md'),input,review);
    const initialJudge='judge-runs/'+task.id+'/'+version+'/result.json';
    const hadFinding=exists(initialJudge)&&load(initialJudge).verdict!=='GO';
    const weighted=primary?.editorial.reduce((sum,n,i)=>sum+n*[25,25,25,15,10][i],0);
    if (version || !primary || primary.fidelity==='F2' || hadFinding || (weighted>=335&&weighted<=365) ||
        primary.editorial.slice(0,3).includes(2)) {
      await copilot(dir+'/second','review',read('review.prompt.md')+' Independently recheck entity translations and event relationships, and verify exact quotations.',input,review);
    }
  }
}
async function batch(tasks,fn) {
  for (let i=0;i<tasks.length;i+=config.concurrency) await Promise.all(tasks.slice(i,i+config.concurrency).map(fn));
}
async function main() {
  const stage=process.argv[2];
  if (stage==='prepare') return prepare();
  verify();
  if (stage==='verify') return;
  loader=process.env.EXP20_COPILOT_LOADER;
  if (!loader || !fs.existsSync(loader)) throw Error('Set EXP20_COPILOT_LOADER to the authenticated Copilot npm-loader.js');
  const tasks=load('dataset.json');
  if (stage==='loop') return batch(tasks,loop);
  if (stage==='review') return batch(tasks,assess);
  throw Error('Unknown stage');
}
if (require.main===module) main().catch(error=>{console.error(error);process.exitCode=1;});
module.exports={load,read,exists,save,json,csv,sha,at,stages,verify};
