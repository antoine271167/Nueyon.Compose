const fs=require('fs'),path=require('path'),crypto=require('crypto');
const root=process.cwd(),e=path.join(root,'PromptExperiments/Research/compact-b2-experiment-8-2026-11-16');
const files=JSON.parse(fs.readFileSync(path.join(e,'protected-before.json')));
function walk(p){for(const d of fs.readdirSync(p,{withFileTypes:true})){const f=path.join(p,d.name);if(d.isDirectory())walk(f);else if(!['freeze.json'].includes(d.name))files[path.relative(root,f).replaceAll('\\','/')]=crypto.createHash('sha256').update(fs.readFileSync(f)).digest('hex');}}
walk(e);fs.writeFileSync(path.join(e,'freeze.json'),JSON.stringify({frozenAt:new Date().toISOString(),files},null,2));console.log('Frozen files:',Object.keys(files).length);
