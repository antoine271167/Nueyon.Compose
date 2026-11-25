const fs=require('fs'),p=require('path'),crypto=require('crypto'),cp=require('child_process');
const d=__dirname, repo=p.resolve(d,'../../..');
const sha=f=>crypto.createHash('sha256').update(fs.readFileSync(p.join(d,f))).digest('hex');
const frozen=fs.readFileSync(p.join(d,'freeze.sha256'),'utf8').trim().split('\n');
function verify(){for(const row of frozen){const [hash,f]=row.split('  ');if(sha(f)!==hash)throw Error('Frozen file changed: '+f);}}
function valid(f){try{return typeof JSON.parse(fs.readFileSync(f)).content==='string'&&JSON.parse(fs.readFileSync(f)).content.trim().length>0;}catch{return false;}}
verify();
for(const line of fs.readFileSync(p.join(d,'run-manifest.csv'),'utf8').trim().split('\n').slice(1)){
 const [slot,id,subject,, ,arm]=line.split(',');
 const dir=p.join(d,'runs',id); fs.mkdirSync(dir,{recursive:true});
 if(valid(p.join(dir,'result.json'))&&fs.existsSync(p.join(dir,'accepted.json')))continue;
 for(let retry=0;retry<=1;retry++){
  const stem=p.join(dir,'attempt-'+(retry+1));
  if(fs.existsSync(stem+'.json'))throw Error('Existing attempt needs inspection '+id);
  const output=stem+'.result.json', start=new Date();
  console.log('Narrative slot '+slot+'/48 '+subject+' '+arm+' attempt '+(retry+1));
  const r=cp.spawnSync('dotnet',[p.join(repo,'src/Nueyon.Compose.Host.Console/bin/Debug/net10.0/Nueyon.Compose.Host.Console.dll'),'run','narrative','--input',p.join(d,'cases',subject,'cli-input.json'),'--output',output,'--prompts',p.join(d,arm)],{cwd:repo,encoding:'utf8',timeout:240000});
  const accepted=r.status===0&&valid(output);
  const meta={slot:Number(slot),id,subject,arm,model:'gpt-6-luna',attempt:retry+1,started:start.toISOString(),elapsedMs:Date.now()-start,exitCode:r.status,signal:r.signal,error:r.error?.code??null,accepted};
  fs.writeFileSync(stem+'.json',JSON.stringify(meta,null,2)+'\n');
  fs.writeFileSync(stem+'.stdout.log',r.stdout??'');
  fs.writeFileSync(stem+'.stderr.log',r.stderr??'');
  if(accepted){fs.copyFileSync(output,p.join(dir,'result.json'));fs.writeFileSync(p.join(dir,'accepted.json'),JSON.stringify(meta,null,2)+'\n');break;}
  const details=r.stderr??'';
  if(retry===1||!(r.error?.code==='ETIMEDOUT'||/HTTP (408|429|5\d\d)|response content|malformed|connection/i.test(details)||r.status===0))throw Error('Stopped at '+id+' '+details);
 }
}
verify();
console.log('All 48 slots have results; frozen hashes verified.');

