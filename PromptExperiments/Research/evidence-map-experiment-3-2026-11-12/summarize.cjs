const fs=require('fs'),p=require('path'),crypto=require('crypto');
const out=__dirname,root=p.resolve(out,'../../..');
const read=f=>JSON.parse(fs.readFileSync(f,'utf8').replace(/^\uFEFF/,'')),write=(f,v)=>fs.writeFileSync(f,JSON.stringify(v,null,2));
const freeze=read(p.join(out,'freeze.json')),hash=f=>crypto.createHash('sha256').update(fs.readFileSync(f)).digest('hex');
const rows=[],checks=[];
for(const {id} of freeze.pairs)for(const variant of ['map','bad','correct']){
 const dir=p.join(out,'calls',id+'-'+variant);if(!fs.existsSync(p.join(dir,'response-raw.json')))continue;
 const response=read(p.join(dir,'response-raw.json')),request=read(p.join(dir,'request.json'));
 const input=read(p.join(out,'cases',id,variant==='map'?'bad':variant,'input.json'));
 const user=JSON.parse(request.messages.find(m=>m.role==='user').content);
 const mapper=variant==='map';
 let c={id:id+'-'+variant,source:user.originalSource===input.Input.Content,idea:JSON.stringify(user.selectedIdea)===JSON.stringify(Object.fromEntries(Object.entries(input.SelectedIdea.Idea).map(([k,v])=>[k[0].toLowerCase()+k.slice(1),v]))),model:request.model==='gpt-6-luna',prompt:request.messages.find(m=>m.role==='system').content===fs.readFileSync(p.join(out,'prompts',mapper?'mapper.md':'judge.md'),'utf8')};
 if(mapper){c.sourceOnly=Object.keys(user).sort().join(',')==='originalSource,selectedIdea';}
 else{
  const research=read(p.join(out,'cases',id,variant,'research-result.json'));
  c.research=user.research.content===(research.content??research.Content);
  c.realFilter=user.synthesisInput===fs.readFileSync(p.join(dir,'synthesis-filter-output.md'),'utf8');
  c.map=JSON.stringify(user.evidenceMap)===JSON.stringify(read(p.join(out,'calls',id+'-map/map-raw.json')));
 }
 checks.push(c);
 const rawText=response.choices?.[0]?.message?.content;
 if(rawText)fs.writeFileSync(p.join(dir,'response-text.json'),rawText);
 const usage=response.usage??{};
 const row={id:id+'-'+variant,stage:mapper?'mapper':'judge',model:response.model,status:read(p.join(dir,'http-metadata.json')).status,inputTokens:usage.prompt_tokens??0,outputTokens:usage.completion_tokens??0,totalTokens:usage.total_tokens??0,usage};
 if(fs.existsSync(p.join(dir,'verdict.json')))row.verdict=read(p.join(dir,'verdict.json'));
 if(fs.existsSync(p.join(dir,'error.txt')))row.error=fs.readFileSync(p.join(dir,'error.txt'),'utf8');
 if(!mapper&&rawText){try{row.rawVerdict=JSON.parse(rawText);row.anchors=row.rawVerdict.blockingIssues.map(i=>({quote:i.sourceQuote,exact:input.Input.Content.includes(i.sourceQuote)}));}catch{row.parseError=true;}}
 rows.push(row);
}
write(p.join(out,'usage.json'),{attempts:fs.readdirSync(p.join(out,'calls')).filter(d=>fs.existsSync(p.join(out,'calls',d,'attempt-started.json'))).length,rows,totals:rows.reduce((a,r)=>({inputTokens:a.inputTokens+r.inputTokens,outputTokens:a.outputTokens+r.outputTokens,totalTokens:a.totalTokens+r.totalTokens}),{inputTokens:0,outputTokens:0,totalTokens:0})});
write(p.join(out,'verification-requests.json'),checks);
let protectedDiff=[];for(const [f,h] of Object.entries(read(p.join(out,'before-hashes.json'))))if(hash(p.join(root,f))!==h)protectedDiff.push(f);
write(p.join(out,'verification-protected-files.json'),{checked:Object.keys(read(p.join(out,'before-hashes.json'))).length,changed:protectedDiff});
let frozenDiff=[];for(const [f,h] of Object.entries(freeze.files))if(hash(p.join(root,f))!==h)frozenDiff.push(f);
write(p.join(out,'verification-freeze.json'),{checked:Object.keys(freeze.files).length,changed:frozenDiff});
console.log(JSON.stringify({count:rows.length,totals:read(p.join(out,'usage.json')).totals,failedChecks:checks.filter(c=>Object.values(c).includes(false)),protectedDiff,frozenDiff},null,2));
