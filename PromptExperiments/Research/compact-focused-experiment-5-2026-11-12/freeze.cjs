const fs=require('fs'),path=require('path'),crypto=require('crypto');
const root=process.cwd(),out=__dirname, hash=f=>crypto.createHash('sha256').update(fs.readFileSync(f)).digest('hex');
if(fs.existsSync(path.join(out,'freeze.json')))throw Error('Already frozen');
const files={};
function walk(dir){for(const f of fs.readdirSync(dir,{withFileTypes:true})){if(['obj','bin'].includes(f.name))continue;const p=path.join(dir,f.name);if(f.isDirectory())walk(p);else files[path.relative(root,p).replaceAll('\\','/')]=hash(p);}}
walk(out);
for(const f of Object.keys(JSON.parse(fs.readFileSync(path.join(out,'protected-before.json')))))files[f]=hash(path.join(root,f));
// Runtime binaries were built offline before freeze; attest exactly the assemblies used.
for(const f of fs.readdirSync(path.join(out,'runner/bin/Debug/net10.0'))){if(f.endsWith('.dll')){const p=path.join(out,'runner/bin/Debug/net10.0',f);files[path.relative(root,p).replaceAll('\\','/')]=hash(p);}}
fs.writeFileSync(path.join(out,'freeze.json'),JSON.stringify({frozenAt:new Date().toISOString(),maxCalls:42,retries:0,files},null,2));
console.log('Frozen '+Object.keys(files).length+' files, including production sources and runtime DLLs');
