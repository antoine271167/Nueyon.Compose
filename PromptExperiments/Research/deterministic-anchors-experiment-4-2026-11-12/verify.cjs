const fs=require('fs'),path=require('path'),crypto=require('crypto');const out=__dirname,root=path.resolve(out,'../../..');
const read=p=>JSON.parse(fs.readFileSync(path.join(out,p),'utf8')),hash=b=>crypto.createHash('sha256').update(b).digest('hex');
const save=(p,v)=>fs.writeFileSync(path.join(out,p),JSON.stringify(v,null,2));
const equal=(a,b)=>JSON.stringify(a)===JSON.stringify(b);
const normalize=v=>Array.isArray(v)?v.map(normalize):v&&typeof v==='object'?Object.fromEntries(Object.entries(v).map(([k,w])=>[k[0].toLowerCase()+k.slice(1),normalize(w)])):v;
const freeze=read('freeze.json'),files=Object.entries(freeze.files).map(([p,h])=>({path:p,ok:hash(fs.readFileSync(path.join(root,p)))===h}));save('verification-freeze.json',files);
const protectedFiles=read('protected-before.json');save('verification-protected-files.json',Object.entries(protectedFiles).map(([p,h])=>({path:p,ok:hash(fs.readFileSync(path.join(root,p)))===h})));
const checks=[],usage=[],processed=[];
for(const call of read('plan.json')){
 const prefix='calls/'+call.id;if(!fs.existsSync(path.join(out,prefix,'request.json')))continue;
 const request=read(prefix+'/request.json'),raw=read(prefix+'/response-raw.json'),input=normalize(read(`cases/${call.pair}/${call.arm==='map'?'bad':call.variant}/input.json`));
 const user=JSON.parse(request.messages.find(m=>m.role==='user').content),system=request.messages.filter(m=>m.role==='system'||m.role==='developer').map(m=>m.content).join('');
 let check={id:call.id,model:request.model==='gpt-6-luna',maxOutput:request.max_completion_tokens===6000||request.max_tokens===6000,prompt:system===fs.readFileSync(path.join(out,'prompts',call.arm==='map'?'mapper.md':'judge.md'),'utf8'),source:user.originalSource===input.input.content,idea:equal(user.selectedIdea,input.selectedIdea.idea),noLabels:!('labels' in user)&&!('audit' in user)};
 if(call.arm==='map'){
  check.noResearch=!('research' in user)&&!('synthesisInput' in user);check.fragments=equal(user.sourceFragments,read(`fragments/${call.pair}.json`).fragments);
 }else{
  check.research=equal(user.research,normalize(read(`cases/${call.pair}/${call.variant}/research-result.json`)));
  check.filter=user.synthesisInput===fs.readFileSync(path.join(out,prefix,'synthesis-filter-output.md'),'utf8');
  check.map=call.arm==='with'?equal(user.evidenceMap,read(`calls/${call.pair}-map/map.json`)):!('evidenceMap' in user);
 }
 checks.push(check);let u=raw.usage||{};usage.push({...call,status:read(prefix+'/http-metadata.json').status,responseModel:raw.model,input:u.prompt_tokens??0,output:u.completion_tokens??0,total:u.total_tokens??0,rawUsage:u});
 const content=raw.choices?.[0]?.message?.content;let verdict;try{verdict=JSON.parse(content);}catch{}
 processed.push({...call,finishReason:raw.choices?.[0]?.finish_reason,parserValid:fs.existsSync(path.join(out,prefix,'verdict.json')),content:verdict??content,anchors:verdict?.blockingIssues?.map(x=>({quote:x?.sourceQuote,exact:typeof x?.sourceQuote==='string'&&x.sourceQuote.length>0&&input.input.content.includes(x.sourceQuote)}))});
}
save('verification-requests.json',checks);save('usage.json',usage);save('responses-processed.json',processed);
const total=rows=>rows.reduce((a,r)=>({calls:a.calls+1,input:a.input+r.input,output:a.output+r.output,total:a.total+r.total}),{calls:0,input:0,output:0,total:0});
const without=total(usage.filter(x=>x.arm==='without')),withJudges=total(usage.filter(x=>x.arm==='with')),maps=total(usage.filter(x=>x.arm==='map')),withAll=total(usage.filter(x=>x.arm!=='without'));
const summary={all:total(usage),without,withJudges,maps,withAll,extraTokensPercent:(withAll.total/without.total-1)*100,requestChecksPass:checks.every(x=>Object.entries(x).every(([k,v])=>k==='id'||v===true)),freezePass:files.every(f=>f.ok)};
save('cost-summary.json',summary);console.log(JSON.stringify(summary,null,2));
const paired=[];
for(const pair of ['intro','005','010'])for(const variant of ['bad','correct']){
 const a=read(`calls/${pair}-${variant}-without/request.json`),b=read(`calls/${pair}-${variant}-with/request.json`);
 const am=a.messages.find(m=>m.role==='user'),bm=b.messages.find(m=>m.role==='user');
 const bp=JSON.parse(bm.content);delete bp.evidenceMap;bm.content=JSON.stringify(bp);am.content=JSON.stringify(JSON.parse(am.content));
 paired.push({pair,variant,identicalExceptEvidenceMap:equal(a,b)});
}
save('verification-paired-requests.json',paired);
const old=path.join(out,'../evidence-map-experiment-3-2026-11-12'),fixtureChecks=[];
for(const pair of ['intro','005','010'])for(const variant of ['bad','correct'])for(const name of ['input.json','research-result.json']){
 const p=`cases/${pair}/${variant}/${name}`;fixtureChecks.push({path:p,byteIdenticalToExperiment3:fs.readFileSync(path.join(out,p)).equals(fs.readFileSync(path.join(old,p)))});
}
save('verification-reused-fixtures.json',fixtureChecks);
if(!summary.requestChecksPass||!summary.freezePass)process.exitCode=1;
