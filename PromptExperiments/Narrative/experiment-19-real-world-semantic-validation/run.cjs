const fs=require('node:fs');
const path=require('node:path');
const {createHash}=require('node:crypto');
const {execFile}=require('node:child_process');
const root=__dirname;
const repo=path.resolve(root,'../../..');
const at=name=>path.resolve(root,name);
const read=name=>fs.readFileSync(at(name),'utf8');
const obj=name=>JSON.parse(read(name));
const save=(name,data)=>fs.writeFileSync(at(name),data,{flag:'wx'});
const json=(name,data)=>save(name,JSON.stringify(data,null,2)+'\n');
const digest=data=>createHash('sha256').update(data).digest('hex');
const sha=name=>digest(fs.readFileSync(at(name)));
const subjects=fs.readdirSync(at('cases')).sort();
const model='gpt-6-luna';
const prior='../experiment-18-semantic-validation-reliability/validator.prompt.md';
const prompts='../../../src/Nueyon.Compose.Application/Prompts';
const settings='../../../src/Nueyon.Compose.Host.Console/appsettings.json';
const app=path.resolve(repo,'src/Nueyon.Compose.Host.Console/bin/Debug/net10.0/Nueyon.Compose.Host.Console.dll');
const promptTree=()=>fs.readdirSync(at(prompts),{recursive:true,withFileTypes:true}).filter(entry=>entry.isFile()).map(entry=>path.relative(root,path.join(entry.parentPath,entry.name)).replaceAll('\\','/')).sort();
const fileHashes=files=>Object.fromEntries(files.map(file=>[file,sha(file)]));
const cases=subjects.flatMap(id=>['source.txt','cli-input.json'].map(file=>'cases/'+id+'/'+file));
const ordered=rep=>rep===2?[...subjects].reverse():subjects;
const tasks=[1,2,3].flatMap(rep=>ordered(rep).map(subject=>({id:subject+'-'+rep,subject,rep})));
const narrativeResult=id=>'narrative-runs/'+id+'/result.json';
function verify(file){
 for(const [name,hash] of Object.entries(obj(file))) if(sha(name)!==hash) throw Error('Hash changed: '+name);
}
function prepare(){
 if(subjects.length!==12||sha(prior)!==sha('validator.prompt.md'))throw Error('Corpus or validator mismatch');
 const config={model,dotnet:'10.0.404',node:process.version,copilot:'1.0.116',sourceCommit:'3c5f9cfd2c14701faad191911649d73148e9c6d3',promptSource:'Production CLI default',generationOptions:'Application defaults',validatorOptions:'Experiment 18 options unchanged'};
 json('model-configuration.json',config);
 save('narrative-plan.csv','id,subject,repetition,inputSha256\n'+tasks.map(t=>[t.id,t.subject,t.rep,sha('cases/'+t.subject+'/cli-input.json')].join(',')).join('\n')+'\n');
 json('freeze.json',fileHashes([...cases,...promptTree(),settings,prior,'validator.prompt.md','protocol.md','source-provenance.md','model-configuration.json','narrative-plan.csv','run.cjs']));
}
async function launch(bin,args,dir,timeout){
 const started=new Date().toISOString();
 const begin=Date.now();
 return await new Promise(resolve=>execFile(bin,args,{cwd:repo,windowsHide:true,timeout,maxBuffer:4*1024*1024,encoding:'buffer'},(err,out,stderr)=>{
   save(dir+'/stdout.log',out);
   save(dir+'/stderr.log',stderr);
   resolve({started,seconds:(Date.now()-begin)/1000,exitCode:err?.code??(err?null:0),signal:err?.signal??null});
 }));
}
async function narrative(task){
 const home='narrative-runs/'+task.id;
 if(fs.existsSync(at(narrativeResult(task.id))))return;
 if(fs.existsSync(at(home)))throw Error('Unresolved archive '+home);
 const input='cases/'+task.subject+'/cli-input.json';
 for(const attempt of [1,2]){
   const dir=home+'/attempt-'+attempt;
   fs.mkdirSync(at(dir),{recursive:true});
   const output=dir+'/result.json';
   console.log('Narrative start',task.id,attempt);
   const meta=await launch('dotnet',[app,'run','narrative','--input',at(input),'--output',at(output)],dir,240000);
   let usable=false;
   try { const {content}=obj(output);usable=typeof content==='string'&&!!content.trim(); } catch {}
   const valid=meta.exitCode===0&&usable;
   json(dir+'/attempt.json',{...task,...meta,model,attempt,valid,inputSha256:sha(input),outputSha256:usable?sha(output):null});
   console.log('Narrative complete',task.id,attempt,valid,meta.seconds);
   if(valid){fs.copyFileSync(at(output),at(narrativeResult(task.id)),fs.constants.COPYFILE_EXCL);return;}
   if(attempt===2||!(meta.signal||meta.exitCode===0||/HTTP (408|429|5\d\d)|response content|malformed|connection/i.test(read(dir+'/stderr.log'))))throw Error('Narrative failure: '+dir);
 }
}
function seal(){
 const labels=obj('ground-truth.json');
 if(labels.length!==tasks.length||new Set(labels.map(row=>row.id)).size!==tasks.length)throw Error('Incomplete ground truth');
 for(const task of tasks) {
  const row=labels.find(row=>row.id===task.id);
  if(!['F0','F1','F2'].includes(row?.label))throw Error('Missing label '+task.id);
  const content=obj(narrativeResult(task.id)).content;
  const evidence=[read('cases/'+task.subject+'/source.txt'),...Object.values(obj('cases/'+task.subject+'/cli-input.json').selectedIdea),obj('cases/'+task.subject+'/cli-input.json').synthesis.content];
  if(!Array.isArray(row.findings)||(row.label!=='F0'&&!row.findings.length))throw Error('Missing findings '+task.id);
  for(const f of row.findings)if(!content.includes(f.narrativeExcerpt)||!evidence.some(e=>e.includes(f.evidenceExcerpt))||!f.category||!f.explanation||!f.correction)throw Error('Invalid finding '+task.id);
 }
 const supplement=fs.existsSync(at('supplementary-plan.json'))?['supplementary-plan.json',...obj('supplementary-plan.json').flatMap(t=>[t.input,t.output,t.source])]:[];
 if(labels.filter(row=>row.label==='F2').length<8&&!supplement.length)throw Error('Supplement required');
 json('ground-truth-freeze.json',fileHashes(['ground-truth.json',...tasks.map(t=>narrativeResult(t.id)),...new Set(supplement)]));
}
async function validate(task,rep,loader,parse){
 const supplementary=!!task.input;
 const home=(supplementary?'supplementary-runs/':'validator-runs/')+task.id+'/'+rep;
 if(fs.existsSync(at(home))) {
  if(!fs.existsSync(at(home+'/attempt.json')))throw Error('Unresolved validator archive '+home);
  return;
 }
 const input={...obj(task.input??'cases/'+task.subject+'/cli-input.json'),narrative:obj(task.output??narrativeResult(task.id)),source:read(task.source??'cases/'+task.subject+'/source.txt')};
 fs.mkdirSync(at(home),{recursive:true});
 json(home+'/input.json',input);
 const prompt=read('validator.prompt.md').trim()+'\n\n'+JSON.stringify(input);
 save(home+'/prompt.txt',prompt);
 const args=[loader,'--model',model,'--silent','--no-custom-instructions','--disable-builtin-mcps','--available-tools','experiment17_no_tools','-p',prompt];
 console.log('Validator start',task.id,rep);
 const meta=await launch(process.execPath,args,home,180000);
 fs.copyFileSync(at(home+'/stdout.log'),at(home+'/response.txt'),fs.constants.COPYFILE_EXCL);
 let status=null,error=null;
 if(meta.exitCode!==0)error='transport';
 else try{
  const response=parse(read(home+'/response.txt'));
  json(home+'/parsed.json',response);
  status=response.status;
 }catch(failure){error=failure instanceof SyntaxError?'invalid-json':'invalid-contract';}
 json(home+'/attempt.json',{id:task.id,rep,model,...meta,status,error,inputSha256:sha(home+'/input.json'),promptSha256:digest(prompt),responseSha256:sha(home+'/response.txt')});
 console.log('Validator complete',task.id,rep,status??error,meta.seconds);
}
async function batches(items,width,fn){
 for(let i=0;i<items.length;i+=width){
  const batch=await Promise.allSettled(items.slice(i,i+width).map(fn));
  const failed=batch.find(result=>result.status==='rejected');
  if(failed)throw failed.reason;
 }
}
async function main(){
 const stage=process.argv[2];
 if(stage==='freeze')return prepare();
 verify('freeze.json');
 if(stage==='verify'){if(fs.existsSync(at('ground-truth-freeze.json')))verify('ground-truth-freeze.json');return;}
 if(stage==='narrative')return await batches(tasks,4,narrative);
 if(stage==='seal')return seal();
 if(stage!=='validator')throw Error('Unknown stage');
 verify('ground-truth-freeze.json');
 const loader=process.env.EXPERIMENT17_COPILOT_LOADER;
 const {parse}=require('../experiment-17-semantic-validation/run.cjs');
 for(const rep of [1,2])await batches(rep===1?tasks:[...tasks].reverse(),4,task=>validate(task,rep,loader,parse));
 if(fs.existsSync(at('supplementary-plan.json')))for(const rep of [1,2])await batches(obj('supplementary-plan.json'),4,task=>validate(task,rep,loader,parse));
}
main().catch(error=>{console.error(error);process.exitCode=1;});

