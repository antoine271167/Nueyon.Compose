const fs = require('node:fs');
const path = require('node:path');
const Module = require('node:module');
const {createHash} = require('node:crypto');
const {execFile} = require('node:child_process');
const {performance} = require('node:perf_hooks');
process.chdir(__dirname);
const old = '../experiment-20-fidelity-judge-repair-loop/';
const read = p => fs.readFileSync(p,'utf8').replace(/^\uFEFF/,'');
const load = p => JSON.parse(read(p));
const hash = p => createHash('sha256').update(fs.readFileSync(p)).digest('hex');
function put(p,body){
 fs.mkdirSync(path.dirname(p),{recursive:true});
 fs.writeFileSync(p,body,{flag:'wx'});
}
const save = (p,v) => put(p,JSON.stringify(v,null,2)+'\n');
const comparator = path.resolve(old+'run.cjs');
const prior = new Module(comparator,module);
prior.filename = comparator;
prior._compile(read(comparator)+'\nmodule.exports={parse,judge};',comparator);
const {parse,judge} = prior.exports;
const cfg = load('model-configuration.json');
const terms = load('controlled-terms.json');

function ratio(text){
 const formats = {en:'out of',nl:'van de'};
 for(const [language,joiner] of Object.entries(formats)){
  const decimal = language==='en'?'\\.':',';
  const number = '(\\d+(?:'+decimal+'\\d+)?)';
  const expression = new RegExp('(?<![\\d.,])'+number+' '+joiner+' '+number+
   ' ('+Object.keys(terms.scales[language]).join('|')+') ([A-Za-z][A-Za-z0-9]{0,11})\\b','gi');
  const matches = [...text.matchAll(expression)];
  if(matches.length!==1) continue;
  const [quote,a,b,scale,unit] = matches[0];
  return {quote,unit,coefficients:[a,b].map(x=>Number(x.replace(',','.'))),
   exponent:terms.scales[language][scale.toLowerCase()]};
 }
 return null;
}

function discover(content,invariant){
 const patterns = invariant.anchors.split('|').map(anchor=>new RegExp(anchor,'iu'));
 return content.split(/\r?\n/).map(line=>({line,score:patterns.filter(p=>p.test(line)).length}))
  .filter(row=>row.score).sort((a,b)=>b.score-a.score).slice(0,3).map(row=>row.line);
}

function mechanical(content,invariant){
 const started = performance.now();
 const candidates = discover(content,invariant);
 const sourceRatio = invariant.category==='numerical_scale' && ratio(invariant.source);
 const findings = [];
 if(sourceRatio){
  for(const candidate of candidates){
   const result = ratio(candidate);
   if(result?.unit===sourceRatio.unit && result.coefficients.every((v,i)=>v===sourceRatio.coefficients[i])
     && result.exponent!==sourceRatio.exponent){
    findings.push({mechanism:'deterministic numerical checking',verdict:'CONTRADICTED',
     narrativeExcerpt:result.quote,evidenceExcerpt:sourceRatio.quote,
     relation:'Same explicit unit and ratio coefficients; the named scale changes both magnitudes.',
     correction:'Express the original magnitude (10^'+sourceRatio.exponent+' multiplier).'});
   }
  }
 }
 return {status:findings.length?'FAIL':'REVIEW',findings,candidates,
  unresolved:findings.length?null:candidates.length?'Relation needs semantic interpretation':'No candidate found',
  milliseconds:performance.now()-started};
}

function semantic(raw,request){
 const value = parse(raw);
 const nonblank = x=>typeof x==='string'&&Boolean(x.trim());
 const {verdict,narrativeExcerpt,relation,correction} = value;
 if(Object.keys(value).sort().join('|')!=='correction|narrativeExcerpt|relation|verdict' ||
 !['SUPPORTED','CONTRADICTED','INSUFFICIENT_EVIDENCE'].includes(verdict) ||
 !nonblank(relation) ||
 !(verdict==='SUPPORTED'&&narrativeExcerpt===null || nonblank(narrativeExcerpt)&&request.candidates.some(x=>x.includes(narrativeExcerpt))) ||
 !(verdict==='CONTRADICTED'?nonblank(correction):correction===null)) throw Error('Invalid focused response contract');
 return value;
}

async function call(dir,template,request,decode){
 if(fs.existsSync(dir+'/attempt.json')) return fs.existsSync(dir+'/result.json')?load(dir+'/result.json'):null;
 const prompt = template+'\n\n'+JSON.stringify(request);
 save(dir+'/request.json',request);
 put(dir+'/prompt.txt',prompt);
 const loader=process.env.EXP22_COPILOT_LOADER;
 const args=[loader,'--model',cfg.model,'--silent','--no-custom-instructions','--no-ask-user','--disable-builtin-mcps','--available-tools','experiment20_no_tools','-p',prompt];
 const started = Date.now();
 const {err,stdout,stderr} = await new Promise(resolve=>execFile(process.execPath,args,
  {cwd:path.resolve('../../..'),windowsHide:true,timeout:cfg.timeoutSeconds*1000,maxBuffer:4*1024*1024},
  (err,stdout,stderr)=>resolve({err,stdout,stderr})));
 put(dir+'/stdout.log',stdout);
 put(dir+'/stderr.log',stderr);
 let failure = err?'transport':null;
 let result = null;
 if(!err) try{result=decode(stdout,request);}catch(e){failure=(e instanceof SyntaxError?'invalid-json: ':'invalid-contract: ')+e.message;}
 if(result) save(dir+'/result.json',result);
 save(dir+'/attempt.json',{started:new Date(started).toISOString(),seconds:(Date.now()-started)/1000,exitCode:err?.code??(err?null:0),failure});
 console.log(dir,failure??'valid');
 return result;
}

const status = {GO:'PASS',REVISE:'FAIL',UNCERTAIN:'REVIEW',SUPPORTED:'PASS',CONTRADICTED:'FAIL',INSUFFICIENT_EVIDENCE:'REVIEW'};
async function run(test,attempt,condition){
 const content = load(test.initial).content ?? load(test.initial).Content;
 const source = read(test.source);
 const dir = condition+'/'+test.id+'/'+attempt;
 if(fs.existsSync(dir+'/outcome.json')) return;
 if(condition==='baseline'){
  const input={...load(test.input),source,narrative:{content}};
  const result=await call(dir,read(old+'judge.prompt.md')+'\n'+read(old+'judge-schema.json'),input,judge);
  save(dir+'/outcome.json',{status:status[result?.verdict]??'TECHNICAL_FAILURE',
   findings:result?.blockers??[]});
  return;
 }
 const invariant=load('invariants.json')[test.subject];
 if(!invariant || !source.includes(invariant.source)) throw Error('Ungrounded invariant '+test.id);
 const deterministic = mechanical(content,invariant);
 if(condition==='deterministic' || deterministic.status==='FAIL' || !deterministic.candidates.length){
  save(dir+'/outcome.json',deterministic);
  return;
 }
 const request={invariant,candidates:deterministic.candidates,
  terminology:test.subject==='wolf'?terms.identity:[]};
 const result=await call(dir,read('semantic-comparison.prompt.md'),request,semantic);
 save(dir+'/outcome.json',{...deterministic,status:status[result?.verdict]??'TECHNICAL_FAILURE',
  findings:result?[{...result,evidenceExcerpt:invariant.source,mechanism:'focused semantic comparison'}]:[],
  unresolved:result?.verdict==='INSUFFICIENT_EVIDENCE'?result.relation:null});
}

function verify(){
 for(const [p,digest] of Object.entries(load('freeze.json'))){
  if(hash(p)!==digest) throw Error('Frozen byte mismatch '+p);
 }
}

async function main(){
 const phase=process.argv[2];
 if(phase==='verify') return verify();
 if(phase!=='development') verify();
 if(!fs.existsSync(process.env.EXP22_COPILOT_LOADER??'')) throw Error('Set EXP22_COPILOT_LOADER to Copilot npm-loader.js');
 const tests=load('dataset.json').filter(t=>t.split===phase);
 if(!tests.length) throw Error('No cases for phase '+phase);
 const jobs=tests.flatMap(test=>[
  [test,1,'deterministic'],[test,1,'baseline'],[test,1,'hybrid'],
  ...(test.repeat?[[test,2,'baseline'],[test,2,'hybrid']]:[])
 ]);
 for(let i=0;i<jobs.length;i+=cfg.concurrency) await Promise.all(jobs.slice(i,i+cfg.concurrency).map(job=>run(...job)));
 if(phase==='evaluation') verify();
}
if(require.main===module) main().catch(e=>{console.error(e);process.exitCode=1;});
module.exports={mechanical,verify};
