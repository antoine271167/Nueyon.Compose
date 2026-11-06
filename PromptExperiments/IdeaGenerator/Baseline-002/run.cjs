const fs=require('node:fs'), path=require('node:path'), cp=require('node:child_process'), crypto=require('node:crypto');
const root=__dirname, repo=path.resolve(root,'../../..'), base=path.resolve(root,'../acceptance-v1'), build=path.join(root,'build');
const manifest=JSON.parse(fs.readFileSync(path.join(base,'test-manifest.json'),'utf8'));
const dbg=process.env.BASELINE_DBG;
const sha=b=>crypto.createHash('sha256').update(b).digest('hex');
const secretFile=path.join(process.env.APPDATA,'Microsoft/UserSecrets/01058711-e286-4f69-9558-dc678f32599d/secrets.json');
const secrets=fs.existsSync(secretFile)?JSON.parse(fs.readFileSync(secretFile,'utf8').replace(/^\uFEFF/,'')):{};
const host=path.join(repo,'src/Nueyon.Compose.Host.Console');
const envName=process.env.ASPNETCORE_ENVIRONMENT??'Production';
const configFiles=['appsettings.json','appsettings.'+envName+'.json'].filter(f=>fs.existsSync(path.join(host,f)));
const configs=configFiles.map(f=>JSON.parse(fs.readFileSync(path.join(host,f),'utf8')));
const keys=[secrets['OpenAI:ApiKey'],secrets.OpenAI?.ApiKey,process.env.OpenAI__ApiKey,process.env.OPENAI_API_KEY,...configs.map(c=>c.OpenAI?.ApiKey)].filter(Boolean);
const scrub=s=>keys.reduce((out,key)=>out.replaceAll(key,'[REDACTED]'),s).replace(/\bsk-[A-Za-z0-9_-]{12,}/g,'[REDACTED]');
const write=(p,v)=>fs.writeFileSync(p,scrub(JSON.stringify(v,null,2)));
const git=(...args)=>cp.execFileSync('git',args,{cwd:repo,encoding:'utf8'}).trim();
const model=secrets['OpenAI:Model']??secrets.OpenAI?.Model??process.env.OpenAI__Model??configs.map(c=>c.OpenAI?.Model).filter(Boolean).at(-1);
if(model!=='gpt-5-mini'||!keys.length)throw Error('Model or configured authentication preflight failed');
const sources=manifest.sources.map(s=>{const f=path.join(base,s.file), b=fs.readFileSync(f); if(b.length!==s.bytes||sha(b)!==s.sha256)throw Error(s.id);return {id:s.id,file:s.file,bytes:b.length,sha256:sha(b)};});
const diff=git('diff',manifest.referenceRevision,'HEAD','--','src');
if(diff || git('status','--porcelain','--','src',path.relative(repo,base)))throw Error('Production or acceptance files differ');
const resources=JSON.parse(fs.readFileSync(path.join(root,'resources.json'),'utf8').replace(/^\uFEFF/,''));
if(resources.length!==11||resources.some(r=>!r.Match))throw Error('Embedded resources mismatch');
const runtime={commit:git('rev-parse','HEAD'),originMain:git('rev-parse','origin/main'),referenceRevision:manifest.referenceRevision,productionDiffAgainstReference:false,provider:'OpenAI',modelAlias:model,environment:envName,os:require('node:os').version(),sdk:cp.execFileSync('dotnet',['--version'],{encoding:'utf8'}).trim(),configuration:configFiles.map(f=>({file:f,sha256:sha(fs.readFileSync(path.join(host,f)))})),libraries:Object.keys(JSON.parse(fs.readFileSync(path.join(build,'Nueyon.Compose.Host.Console.deps.json'))).libraries).filter(x=>/OpenAI|Microsoft.Agents/.test(x)),resources};
write(path.join(root,'preflight.json'),{runtime,sources});
const workflow=path.join(repo,'src/Nueyon.Compose.Application/Workflows/StoryWorkflow.cs');
async function run(spec){
const source=sources.find(s=>s.id===spec.sourceId), dir=path.join(root,'runs',spec.id), attempt=path.join(dir,'attempt-'+(fs.existsSync(dir)?fs.readdirSync(dir).length+1:1));
const cwd=path.join(attempt,'work'), input=path.join(cwd,'data/input');
fs.mkdirSync(input,{recursive:true});
fs.copyFileSync(path.join(base,source.file),path.join(input,path.basename(source.file)));
for(const file of configFiles)fs.copyFileSync(path.join(host,file),path.join(cwd,file));
const meta={...spec,source,runtime,debugger:path.basename(dbg),startedAt:new Date().toISOString()};
write(path.join(attempt,'runtime.json'),meta);
const child=cp.spawn(dbg,process.env.BASELINE_DBG_MI?['--interpreter=vscode']:['--interpreter=vscode'],{windowsHide:true});
let buf=Buffer.alloc(0), seq=0, log='', events=[], pending=new Map(), processing=Promise.resolve(), finish;
const done=new Promise(resolve=>finish=resolve);
const timer=setTimeout(()=>{events.push('Timeout'); finish();},420000);
const send=m=>{const bytes=Buffer.from(JSON.stringify(m));child.stdin.write('Content-Length: '+bytes.length+'\r\n\r\n');child.stdin.write(bytes);};
const req=(command,args={})=>new Promise((resolve,reject)=>{const n=++seq;pending.set(n,{resolve,reject});send({seq:n,type:'request',command,arguments:args});});
const save=(name,data)=>write(path.join(attempt,name+'.json'),data);
const checkpoint=setInterval(()=>fs.writeFileSync(path.join(attempt,'console.log'),scrub(log)),10000);
let idea,narrative,exitCode;
async function stopped(e){
const stack=(await req('stackTrace',{threadId:e.threadId,startFrame:0,levels:1})).stackFrames[0];
const scopes=(await req('scopes',{frameId:stack.id})).scopes;
async function vars(ref){return (await req('variables',{variablesReference:ref})).variables;}
const locals=(await Promise.all(scopes.map(s=>vars(s.variablesReference)))).flat();
async function variable(expr){return req('evaluate',{expression:expr,frameId:stack.id,context:'watch'});}
function val(v){const s=v.value??v.result;if(s.startsWith('"')&&s.endsWith('"')){try{return JSON.parse(s);}catch{return s.slice(1,-1);}}throw Error('Unexpected debugger string representation');}
if(stack.line===134){
 const arr=locals.find(v=>v.name==='ideas');
 const item=arr&&(await vars(arr.variablesReference)).find(v=>v.name==='[0]');
 const fields=item?await vars(item.variablesReference):[];
 const captured={};for(const name of ['Title','Description','Audience','Rationale','Evidence'])captured[name]=val(fields.find(f=>f.name===name)??await variable('ideas[0].'+name));
 idea=captured;
 save('ideas-0',idea);
}else if(stack.line===219){const obj=locals.find(v=>v.name==='narrative');const fields=obj?await vars(obj.variablesReference):[];narrative=val(fields.find(f=>f.name==='Content')??await variable('narrative.Content'));save('narrative',{Content:narrative});}
else if(stack.line===135){const selected=await variable('ideas[0]');if(selected.variablesReference&&idea)save('selected-idea',{Idea:idea});}
else throw Error('Unexpected breakpoint line '+stack.line);
 await req('continue',{threadId:e.threadId});
}
function handle(m){
 if(m.type==='response'){const p=pending.get(m.request_seq);if(p){pending.delete(m.request_seq);m.success?p.resolve(m.body):p.reject(Error(m.command+': '+m.message));}return;}
 if(m.type==='request'){events.push('Adapter request '+m.command);send({seq:++seq,type:'response',request_seq:m.seq,command:m.command,success:false,message:'Unsupported client request'});return;}
 if(m.event==='output'){if(!['telemetry'].includes(m.body.category))log+=m.body.output;return;}
 if(m.event==='exited')exitCode=m.body.exitCode;
 if(m.event==='terminated'){finish();return;}
 if(m.event==='initialized')processing=processing.then(async()=>{const b=await req('setBreakpoints',{source:{path:workflow},breakpoints:[{line:134},{line:219}]});save('breakpoints',b);await req('configurationDone');}).catch(e=>{events.push(e.message);finish();});
 if(m.event==='stopped')processing=processing.then(()=>stopped(m.body)).catch(e=>{events.push(e.message);finish();});
}
child.stdout.on('data',b=>{buf=Buffer.concat([buf,b]);for(;;){const end=buf.indexOf('\r\n\r\n');if(end<0)return;const n=Number(/Content-Length:\s*(\d+)/i.exec(buf.subarray(0,end).toString())?.[1]);if(!n||buf.length<end+4+n)return;handle(JSON.parse(buf.subarray(end+4,end+4+n)));buf=buf.subarray(end+4+n);}});
child.stderr.on('data',b=>{log+=b.toString();});
child.on('error',e=>{events.push(e.message);finish();});
child.on('exit',()=>finish());
(async()=>{await req('initialize',{adapterID:'coreclr',pathFormat:'path',linesStartAt1:true,columnsStartAt1:true});await req('launch',{name:'Baseline 002',type:'coreclr',request:'launch',program:path.join(build,'Nueyon.Compose.Host.Console.exe'),cwd,console:'internalConsole',justMyCode:true});})().catch(e=>{events.push(e.message);finish();});
await done;clearTimeout(timer);clearInterval(checkpoint);
if(child.exitCode===null)child.kill();
if(idea)save('selected-idea',{Idea:idea,selection:'StoryWorkflow.cs:134'});
fs.writeFileSync(path.join(attempt,'console.log'),scrub(log));
save('execution',{exitCode,events,ideaCaptured:!!idea,narrativeCaptured:!!narrative,finishedAt:new Date().toISOString()});
console.log(spec.id,JSON.stringify({exitCode,events,idea:!!idea,narrative:!!narrative}));
return {idea,narrative,exitCode,events,log};
}
(async()=>{for(const spec of manifest.runOrder){const dir=path.join(root,'runs',spec.id);if(fs.existsSync(dir)&&fs.readdirSync(dir).some(a=>fs.existsSync(path.join(dir,a,'ideas-0.json'))&&fs.existsSync(path.join(dir,a,'narrative.json'))))continue; const r=await run(spec);if(r.events.length||(!r.idea&&!r.narrative&&/401|429|403|HttpRequestException|connection/i.test(r.log)))break;if(process.argv.includes('--one'))break;}})().catch(e=>{console.error(scrub(e.message));process.exitCode=1;});

