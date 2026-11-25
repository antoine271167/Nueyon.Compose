const fs=require('node:fs');
const path=require('node:path');
const Module=require('node:module');
const {createHash}=require('node:crypto');
const {execFile}=require('node:child_process');
process.chdir(__dirname);
const old=path.resolve('../experiment-20-fidelity-judge-repair-loop/run.cjs');
const read=p=>fs.readFileSync(p,'utf8').replace(/^\uFEFF/,'');
const load=p=>JSON.parse(read(p));
const sha=buffer=>createHash('sha256').update(buffer).digest('hex');
const prior=new Module(old,module);
prior.filename=old;
prior._compile(read(old)+'\nmodule.exports={parse,judge};',old);
const {parse,judge}=prior.exports;
const cfg=load('model-configuration.json');
const schemas=load('judge-schema.json').$defs;
const mkdir=p=>fs.mkdirSync(p,{recursive:true});
function put(p,body){mkdir(path.dirname(p));fs.writeFileSync(p,body,{flag:'wx'});}
const save=(p,value)=>put(p,JSON.stringify(value,null,2)+'\n');
function verify(){
 for(const [p,digest] of Object.entries(load('freeze.json')))
  if(sha(fs.readFileSync(p))!==digest) throw Error('Frozen byte mismatch '+p);
}
function check(condition,message){if(!condition) throw Error(message);}
function validate(s,v){
 if(s.anyOf){check(s.anyOf.some(alt=>{try{validate(alt,v);return true;}catch{return false;}}),'Invalid alternative');return;}
 if(s.enum){check(s.enum.includes(v),'Invalid enum');return;}
 if(s.type==='null'){check(v===null,'Expected null');return;}
 if(s.type==='string'){check(typeof v==='string'&&v.trim(),'Expected nonblank string');return;}
 if(s.type==='boolean'){check(typeof v==='boolean','Expected boolean');return;}
 if(s.type==='array'){
  check(Array.isArray(v)&&v.length>=(s.minItems??0),'Invalid array');
  v.forEach(x=>validate(s.items,x));return;
 }
 check(v&&typeof v==='object'&&!Array.isArray(v),'Expected object');
 check(Object.keys(v).sort().join('|')===s.required.slice().sort().join('|'),'Unexpected property set');
 for(const [k,sub] of Object.entries(s.properties)) validate(sub,v[k]);
}
function extract(raw,narrative){
 const v=parse(raw);validate(schemas.extraction,v);
 check(new Set(v.claims.map(c=>c.claimId)).size===v.claims.length,'Repeated claim ID');
 for(const c of v.claims) check(narrative.content.includes(c.narrativeExcerpt),'Nonverbatim Narrative excerpt');
 return v;
}
function compare(raw,input){
 const v=parse(raw);validate(schemas.comparison,v);
 const expected=new Set(input.claims.map(c=>c.claimId));
 for(const a of v.assessments){
  check(expected.delete(a.claimId),'Unexpected or repeated claim assessment');
  check(a.sourceExcerpt===null||input.source.includes(a.sourceExcerpt),'Nonverbatim source excerpt');
  check(a.verdict==='INSUFFICIENT_EVIDENCE'||(a.sourceExcerpt&&a.sourceMeaning),'Decisive assessment lacks source grounding');
  check(Boolean(a.correction)===(a.material&&a.verdict==='CONTRADICTED'),'Invalid correction');
 }
 check(!expected.size,'Missing claim assessments');
 for(const q of v.coverageQuestions) check(input.narrative.content.includes(q.narrativeExcerpt),'Nonverbatim coverage question');
 return v;
}
function aggregate(extraction,comparison){
 const {claims}=extraction;
 const {assessments,coverageQuestions}=comparison;
 const blockers=assessments.filter(a=>a.material&&a.verdict==='CONTRADICTED');
 const unresolved=[...assessments.filter(a=>a.material&&a.verdict==='INSUFFICIENT_EVIDENCE'),...coverageQuestions];
 const verdict=blockers.length?'REVISE':unresolved.length?'UNCERTAIN':'GO';
 const result={claims,assessments,coverageQuestions,verdict,blockers,unresolved};
 validate(schemas.aggregate,result);
 return result;
}
let loader=process.env.EXP21_COPILOT_LOADER;
async function call(dir,prompt,input,decode){
 const meta=dir+'/attempt.json';
 if(fs.existsSync(meta)) return fs.existsSync(dir+'/result.json')?load(dir+'/result.json'):null;
 const msg=prompt+'\n\n'+JSON.stringify(input);
 put(dir+'/prompt.txt',msg);
 save(dir+'/request.json',input);
 const args=[loader,'--model',cfg.model,'--silent','--no-custom-instructions','--no-ask-user','--disable-builtin-mcps','--available-tools','experiment20_no_tools','-p',msg];
 const started=new Date().toISOString();
 const begin=Date.now();
 console.log('Start',dir);
 const {error,stdout,stderr}=await new Promise(resolve=>execFile(process.execPath,args,{
  cwd:path.resolve('../../..'),windowsHide:true,timeout:cfg.timeoutSeconds*1000,maxBuffer:4*1024*1024
 },(error,stdout,stderr)=>resolve({error,stdout,stderr})));
 put(dir+'/stdout.log',stdout);
 put(dir+'/stderr.log',stderr);
 let result=null;
 let failure=error?'transport':null;
 if(!error) try{result=decode(stdout,input);}catch(e){failure=(e instanceof SyntaxError?'invalid-json: ':'invalid-contract: ')+e.message;}
 if(result) save(dir+'/result.json',result);
 save(meta,{started,seconds:(Date.now()-begin)/1000,exitCode:error?.code??(error?null:0),signal:error?.signal??null,failure,promptSha256:sha(msg)});
 console.log('Complete',dir,failure??'valid');
 return result;
}
const prev='../experiment-20-fidelity-judge-repair-loop/';
async function task(t){
 const n=load(t.initial);
 const narrative={content:n.content??n.Content};
 const input={...load(t.input),source:read(t.source),narrative};
 const dir='judge-runs/'+t.id;
 if(!fs.existsSync(dir+'/outcome.json')){
  const extraction=await call('claim-extractions/'+t.id,read('claim-extraction.prompt.md'),{narrative},out=>extract(out,narrative));
  const comparison=extraction&&await call('evidence-alignments/'+t.id,read('evidence-comparison.prompt.md'),{...input,...extraction},compare);
  const result=comparison&&aggregate(extraction,comparison);
  if(result) save(dir+'/result.json',result);
  save(dir+'/outcome.json',{status:result?.verdict??'technical_failure',progression:result?.verdict==='GO'?'eligible':'withheld'});
 }
 if(t.split==='evaluation') await call(dir+'/experiment20',read(prev+'judge.prompt.md')+'\n'+read(prev+'judge-schema.json'),input,judge);
}
async function main(){
 verify();
 if(process.argv[2]==='verify') return;
 check(loader&&fs.existsSync(loader),'Set EXP21_COPILOT_LOADER to installed npm-loader.js');
 const split=process.argv[2];
 check(['development','evaluation'].includes(split),'Specify development or evaluation');
 const tasks=load('dataset.json').filter(t=>t.split===split);
 for(let i=0;i<tasks.length;i+=cfg.concurrency) await Promise.all(tasks.slice(i,i+cfg.concurrency).map(task));
 verify();
}
main().catch(e=>{console.error(e);process.exitCode=1;});

