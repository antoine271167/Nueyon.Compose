const fs=require('fs'),path=require('path'),crypto=require('crypto'),cp=require('child_process');
const out=__dirname,root=process.cwd(),read=p=>JSON.parse(fs.readFileSync(p)),hash=p=>crypto.createHash('sha256').update(fs.readFileSync(p)).digest('hex');
const ledger=[],checks=[];
const freeze=read(path.join(out,'freeze.json'));for(const [p,h] of Object.entries(freeze.files))if(hash(path.join(root,p))!==h)throw Error('Freeze mismatch '+p);
for(const id of fs.readdirSync(path.join(out,'calls'))){const dir=path.join(out,'calls',id),rp=path.join(dir,'response-raw.json');if(!fs.existsSync(rp))continue;
 const raw=read(rp),req=read(path.join(dir,'request.json')),usage=raw.usage;
 ledger.push({id,model:raw.model,status:read(path.join(dir,'http-metadata.json')).status,finishReason:raw.choices?.[0]?.finish_reason,input:usage.prompt_tokens,cached:usage.prompt_tokens_details?.cached_tokens||0,cacheWrite:usage.prompt_tokens_details?.cache_write_tokens||0,output:usage.completion_tokens,reasoning:usage.completion_tokens_details?.reasoning_tokens||0,total:usage.total_tokens,usage});
 const caseId=id.split('-')[0],input=read(path.join(out,'cases',caseId,'input.json')),user=req.messages.filter(m=>m.role==='user').map(m=>m.content).join('\n'),sys=req.messages.filter(m=>m.role==='system'||m.role==='developer').map(m=>m.content).join('\n');
 const judge=fs.existsSync(path.join(dir,'judge-input.json'));
 let match={id,model:req.model==='gpt-6-luna',cap:(req.max_completion_tokens||req.max_tokens)===5000,requestBytes:Buffer.byteLength(fs.readFileSync(path.join(dir,'request.json'))),source:true,idea:true,prompt:true,projection:true};
 if(judge){const p=JSON.parse(user),r=read(path.join(dir,'judge-research.json'));match.source=p.source===input.Input.Content;match.idea=Object.keys(input.SelectedIdea.Idea).every(k=>p.selectedIdea[k[0].toLowerCase()+k.slice(1)]===input.SelectedIdea.Idea[k]);match.prompt=sys===fs.readFileSync(path.join(out,'prompts/judge.md'),'utf8');match.projection=p.synthesisInput===fs.readFileSync(path.join(dir,'synthesis-filter-output.md'),'utf8');match.research=p.research.content===r.content;
 }else{const arm=id.endsWith('-A')?'A':'B';match.source=user.includes(input.Input.Content);match.idea=Object.values(input.SelectedIdea.Idea).every(v=>user.includes(v));match.prompt=sys===fs.readFileSync(path.join(out,'prompts',arm,'Research/Research.system.prompt.md'),'utf8');}
 checks.push(match);
}
ledger.sort((a,b)=>read(path.join(out,'calls',a.id,'attempt-started.json')).started.localeCompare(read(path.join(out,'calls',b.id,'attempt-started.json')).started));
const total=ledger.reduce((a,v)=>{for(const k of ['input','cached','cacheWrite','output','reasoning','total'])a[k]=(a[k]||0)+v[k];return a;},{});
fs.writeFileSync(path.join(out,'usage.json'),JSON.stringify({calls:ledger.length,total,ledger},null,2));
fs.writeFileSync(path.join(out,'verification-requests.json'),JSON.stringify(checks,null,2));
const protectedBefore=read(path.join(out,'protected-before.json')),changed=Object.keys(protectedBefore).filter(p=>hash(path.join(root,p))!==protectedBefore[p]);
fs.writeFileSync(path.join(out,'verification-integrity.json'),JSON.stringify({frozenFiles:Object.keys(freeze.files).length,freezeMatches:true,protectedFiles:Object.keys(protectedBefore).length,changed,gitStatus:cp.execFileSync('git',['status','--short'],{encoding:'utf8'}),calls:ledger.length,hardMax:42,allHttpSingle:ledger.every(v=>read(path.join(out,'calls',v.id,'http-metadata.json')).httpRequests===1)},null,2));
console.log(JSON.stringify({calls:ledger.length,total,failedRequestChecks:checks.filter(c=>Object.values(c).includes(false)),changed},null,2));
