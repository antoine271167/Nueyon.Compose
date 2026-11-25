const fs = require('node:fs');
const path = require('node:path');
const {execFile} = require('node:child_process');
const root = path.resolve(__dirname, '../../..');
const save = (p,v) => fs.writeFileSync(p, JSON.stringify(v,null,2)+'\n');
process.chdir(__dirname);
const jobs = [];
for(const [id,url,quote,category,mutation,anchors,title] of require('./sources.json')){
 const dir = 'fixtures/'+id;
 fs.mkdirSync(dir,{recursive:true});
 fs.writeFileSync(dir+'/source.txt',quote+'\n');
 save(dir+'/input.json',{selectedIdea:{title,description:title,audience:'Curious general readers',rationale:title,evidence:quote},synthesis:{content:'### Central insight\n'+title+'\n### Source evidence\n'+quote}});
 for(let i=1;i<=3;i++) jobs.push({dir,i});
}
async function run({dir,i}){
 const output=dir+'/natural-'+i+'.json';
 if(fs.existsSync(output)) return;
 const start=Date.now();
 await new Promise(resolve=>execFile('dotnet',[root+'/src/Nueyon.Compose.Host.Console/bin/Debug/net10.0/Nueyon.Compose.Host.Console.dll','run','narrative','--input',path.resolve(dir+'/input.json'),'--output',path.resolve(output)],{cwd:root,timeout:180000},(err,stdout,stderr)=>{
  save(dir+'/generation-'+i+'.json',{exit:err?.code??0,seconds:(Date.now()-start)/1000,stdout,stderr});
  console.log(output,err?.code??'complete');
  resolve();
 }));
}
(async()=>{for(let i=0;i<jobs.length;i+=4){await Promise.all(jobs.slice(i,i+4).map(run)); if(i===0&&!fs.existsSync(jobs[0].dir+'/natural-1.json')) throw Error('Narrative preflight failed');}})().catch(e=>{console.error(e);process.exitCode=1;});
