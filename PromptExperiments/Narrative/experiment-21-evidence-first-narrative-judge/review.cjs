const fs=require('node:fs');
const path=require('node:path');
const {execFile}=require('node:child_process');
process.chdir(__dirname);
const read=p=>fs.readFileSync(p,'utf8').replace(/^\uFEFF/,'');
const prompt=read('../experiment-20-fidelity-judge-repair-loop/review.prompt.md');
const tasks=JSON.parse(read('dataset.json')).filter(t=>t.cohort.startsWith('new-'));
async function review(t){
 const dir=path.resolve('evaluation/reviews',t.id);
 fs.mkdirSync(dir,{recursive:true});
 const write=(name,text)=>fs.writeFileSync(path.join(dir,name),text,{flag:'wx'});
 const input={...JSON.parse(read(t.input)),source:read(t.source),narrative:JSON.parse(read(t.initial))};
 const message=prompt+'\n\n'+JSON.stringify(input);
 write('prompt.txt',message);
 const started=new Date().toISOString(),begin=Date.now();
 const args=[process.env.EXP21_COPILOT_LOADER,'--model','gpt-6-luna','--silent','--no-custom-instructions','--no-ask-user','--disable-builtin-mcps','--available-tools','experiment20_no_tools','--usage-output-file',path.join(dir,'usage.json'),'-p',message];
 console.log('Start independent review',t.id);
 const res=await new Promise(resolve=>execFile(process.execPath,args,{cwd:path.resolve('../../..'),windowsHide:true,timeout:180000,maxBuffer:4*1024*1024},(e,stdout,stderr)=>resolve({e,stdout,stderr})));
 write('stdout.log',res.stdout);
 write('stderr.log',res.stderr);
 let failure=res.e?'transport':null;
 if(!failure)try{
  const v=JSON.parse(res.stdout.trim().replace(/^[\x60]{3}(?:json)?\s*([\s\S]*?)\s*[\x60]{3}$/i,'$1'));
  if(!Array.isArray(v.editorial)||v.editorial.length!==5||!v.editorial.every(n=>Number.isInteger(n)&&n>=1&&n<=5)||!['F0','F1','F2'].includes(v.fidelity)||!Array.isArray(v.findings)) throw Error('Invalid review');
  write('result.json',JSON.stringify(v,null,2)+'\n');
 }catch(e){failure=e.message;}
 write('attempt.json',JSON.stringify({started,seconds:(Date.now()-begin)/1000,failure},null,2)+'\n');
 console.log('Complete independent review',t.id,failure??'valid');
}
(async()=>{for(let i=0;i<tasks.length;i+=2) await Promise.all(tasks.slice(i,i+2).map(review));})().catch(e=>{console.error(e);process.exitCode=1;});

