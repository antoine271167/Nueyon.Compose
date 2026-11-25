const fs=require('node:fs');
const path=require('node:path');
const os=require('node:os');
const {createHash}=require('node:crypto');
process.chdir(__dirname);
const sha=v=>createHash('sha256').update(v).digest('hex');
const calls=new Map();
for(const root of ['claim-extractions','evidence-alignments','judge-runs']){
 for(const id of fs.readdirSync(root)){
  const d=root+'/'+id+(root==='judge-runs'?'/experiment20':'');
  if(fs.existsSync(d+'/attempt.json')){
   const m=JSON.parse(fs.readFileSync(d+'/attempt.json','utf8'));
   calls.set(m.promptSha256,{path:d,started:m.started});
  }
 }
}
const first=Date.parse([...calls.values()].sort((a,b)=>a.started.localeCompare(b.started))[0].started);
const state=path.join(os.homedir(),'.copilot','session-state');
const output=[];
for(const id of fs.readdirSync(state)){
 const file=path.join(state,id,'events.jsonl');
 if(!fs.existsSync(file)||fs.statSync(file).mtimeMs<first)continue;
 let events;
 try{events=fs.readFileSync(file,'utf8').trim().split('\n').map(JSON.parse);}catch{continue;}
 const user=events.find(e=>e.type==='user.message')?.data;
 if(!user||typeof user.content!=='string')continue;
 const matched=calls.get(sha(user.content));
 if(!matched)continue;
 const usage=events.findLast(e=>e.type==='session.shutdown')?.data;
 if(!usage)continue;
 output.push({...matched,sessionId:id,totalNanoAiu:usage.totalNanoAiu,tokenDetails:usage.tokenDetails,totalApiDurationMs:usage.totalApiDurationMs});
}
fs.writeFileSync('usage-ledger.json',JSON.stringify(output.sort((a,b)=>a.path.localeCompare(b.path)),null,2)+'\n',{flag:'wx'});
console.log('Usage matched',output.length,'of',calls.size);

