const fs=require('fs'),p=require('path'),crypto=require('crypto');const out=__dirname,root=p.resolve(out,'../../..');
const read=x=>JSON.parse(fs.readFileSync(x,'utf8').replace(/^\uFEFF/,'')),write=(x,v)=>fs.writeFileSync(p.join(out,x),JSON.stringify(v,null,2)),hash=x=>crypto.createHash('sha256').update(fs.readFileSync(x)).digest('hex');
let ledger=[],checks=[],processed=[];
for(let c of read(p.join(out,'case-manifest.json'))){let dir=p.join(out,'calls',c.id);if(!fs.existsSync(p.join(dir,'response-raw.json')))continue;
 let response=read(p.join(dir,'response-raw.json')),req=read(p.join(dir,'request.json')),inp=read(p.join(out,'cases',c.id,'input.json')),res=read(p.join(out,'cases',c.id,'research-result.json'));
 let raw=response.choices?.[0]?.message?.content,verdict;try{verdict=JSON.parse(raw)}catch{}
 let payload=JSON.parse(req.messages.find(m=>m.role==='user').content),filter=fs.readFileSync(p.join(dir,'synthesis-filter-output.md'),'utf8');
 let issues=(verdict?.blockingIssues||[]).map(i=>({...i,exactSourceAnchor:inp.Input.Content.includes(i.sourceQuote),anchorInSelectedIdea:JSON.stringify(inp.SelectedIdea).includes(i.sourceQuote)}));
 processed.push({...c,accepted:verdict?.accepted,productionParserPassed:fs.existsSync(p.join(dir,'verdict.json')),issues});
 fs.writeFileSync(p.join(dir,'response-processed.json'),JSON.stringify({verdict,issues,productionParserPassed:fs.existsSync(p.join(dir,'verdict.json'))},null,2));
 ledger.push({call:ledger.length+1,id:c.id,group:c.group,model:response.model,status:read(p.join(dir,'http-metadata.json')).status,usage:response.usage,productionParserPassed:fs.existsSync(p.join(dir,'verdict.json'))});
 checks.push({id:c.id,sourceMatches:payload.source===inp.Input.Content,ideaMatches:JSON.stringify(payload.selectedIdea)===JSON.stringify(Object.fromEntries(Object.entries(inp.SelectedIdea.Idea).map(([k,v])=>[k[0].toLowerCase()+k.slice(1),v]))),researchMatches:JSON.stringify(payload.research)===JSON.stringify({content:res.Content??res.content}),filterMatches:payload.synthesisInput===filter,promptMatches:req.messages[0].content===fs.readFileSync(p.join(out,'prompts/Research/ResearchValidation.system.prompt.md'),'utf8')});
}
write('usage.json',{maxCalls:12,attempts:fs.readdirSync(p.join(out,'calls')).filter(x=>fs.existsSync(p.join(out,'calls',x,'attempt-started.json'))).length,calls:ledger,totals:ledger.reduce((s,c)=>{for(let k of ['prompt_tokens','completion_tokens','total_tokens'])s[k]=(s[k]||0)+(c.usage?.[k]||0);return s},{})});write('verification-requests.json',checks);write('results.json',processed);
let before=read(p.join(out,'before-hashes.json'));write('verification-protected-files.json',{checked:Object.keys(before).length,changed:Object.entries(before).filter(([f,h])=>!fs.existsSync(p.join(root,f))||hash(p.join(root,f))!==h).map(([f])=>f)});
console.log(JSON.stringify(processed,null,2));

