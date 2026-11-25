const fs=require('node:fs');
const path=require('node:path');
const {execFile}=require('node:child_process');
const {createHash}=require('node:crypto');
process.chdir(__dirname);
const read=p=>fs.readFileSync(p,'utf8');
const load=p=>JSON.parse(read(p));
const write=(p,s)=>fs.writeFileSync(p,s,{flag:'wx'});
const save=(p,v)=>write(p,JSON.stringify(v,null,2)+'\n');
const loader=process.env.EXP22_COPILOT_LOADER;
const template=read('independent-review.prompt.md');
const cases=load('dataset.json').filter(t=>t.cohort.startsWith('new-'));
const opaque=id=>createHash('sha256').update('fidelity-review/'+id).digest('hex').slice(0,12);
cases.sort((a,b)=>opaque(a.id).localeCompare(opaque(b.id)));
const batches=[];
for(let i=0;i<cases.length;i+=4) batches.push(cases.slice(i,i+4));
async function run(batch,n){
 const dir='runs/reviews/'+(n+1);
 if(fs.existsSync(dir+'/attempt.json')) return;
 fs.mkdirSync(dir,{recursive:true});
 const request=batch.map(t=>({id:opaque(t.id),source:read(t.source).trim(),
  selectedIdea:load(t.input).selectedIdea,narrative:load(t.initial).content}));
 const prompt=template+'\n\n'+JSON.stringify(request);
 write(dir+'/prompt.txt',prompt);
 save(dir+'/request.json',request);
 const start=Date.now();
 const args=[loader,'--model','gpt-6-luna','--silent','--no-custom-instructions','--no-ask-user','--disable-builtin-mcps','--available-tools','experiment20_no_tools','-p',prompt];
 const {err,stdout,stderr}=await new Promise(resolve=>execFile(process.execPath,args,
  {cwd:path.resolve('../../..'),timeout:150000,windowsHide:true},(err,stdout,stderr)=>resolve({err,stdout,stderr})));
 write(dir+'/stdout.log',stdout);
 write(dir+'/stderr.log',stderr);
 let failure=err?'transport':null;
 if(!failure) try{
  const body=stdout.trim().replace(/^```(?:json)?\s*([\s\S]*?)\s*```$/i,'$1');
  const result=JSON.parse(body);
  if(!Array.isArray(result)||result.length!==request.length||new Set(result.map(row=>row.id)).size!==request.length) throw Error('Unpaired rows');
  for(const row of result){
   const fixture=request.find(t=>t.id===row.id);
   if(!fixture||!['F0','F1','F2'].includes(row.fidelity)||row.editorial?.length!==5||
    !row.editorial.every(x=>Number.isInteger(x)&&x>=1&&x<=5)||typeof row.reason!=='string'||
    (row.fidelity==='F2'&&(!row.narrativeExcerpt||!fixture.narrative.includes(row.narrativeExcerpt)))) throw Error('Bad review row');
  }
  save(dir+'/result.json',result);
 }catch(e){failure=e.message;}
 save(dir+'/attempt.json',{seconds:(Date.now()-start)/1000,failure});
 console.log('Review',n+1,failure??'valid');
}
(async()=>{
 if(!loader||!fs.existsSync(loader)) throw Error('Set EXP22_COPILOT_LOADER');
 for(let i=0;i<batches.length;i+=3) await Promise.all(batches.slice(i,i+3).map((b,j)=>run(b,i+j)));
})().catch(e=>{console.error(e);process.exitCode=1;});
