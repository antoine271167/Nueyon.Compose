const fs=require('fs'),path=require('path'),crypto=require('crypto');const out=__dirname,root=path.resolve(out,'../../..');
const hash=p=>crypto.createHash('sha256').update(fs.readFileSync(p)).digest('hex');
if(fs.existsSync(path.join(out,'freeze.json')))throw Error('Freeze already exists');
const files={};function add(p){files[path.relative(root,p).replaceAll('\\','/')]=hash(p);}
function walk(p){for(const e of fs.readdirSync(p,{withFileTypes:true})){if(['bin','obj'].includes(e.name))continue;const f=path.join(p,e.name);if(e.isDirectory())walk(f);else add(f);}}
for(const d of ['cases','fragments','prompts','runner'])walk(path.join(out,d));
for(const f of ['protocol.md','labels.json','plan.json','prepare.cjs','make-runner.cjs','freeze.cjs','protected-before.json'])add(path.join(out,f));
for(const f of ['src/Nueyon.Compose.Application/Agents/Research/ResearchSemanticJudge.cs','src/Nueyon.Compose.Application.Tests/Agents/ResearchJudgeTests.cs'])add(path.join(root,f));
fs.writeFileSync(path.join(out,'freeze.json'),JSON.stringify({createdUtc:new Date().toISOString(),head:'094b2e539f7b625c0481d38f04d4105815a568dd',model:'gpt-6-luna',plannedCalls:15,absoluteCeiling:18,retries:0,files},null,2));
console.log('Frozen '+Object.keys(files).length+' files before live calls.');
