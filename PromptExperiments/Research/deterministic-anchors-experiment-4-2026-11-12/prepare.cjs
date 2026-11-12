const fs=require('fs'),path=require('path'),crypto=require('crypto'),cp=require('child_process');
const out=__dirname,root=path.resolve(out,'../../..'),old=path.join(out,'../evidence-map-experiment-3-2026-11-12');
const hash=b=>crypto.createHash('sha256').update(b).digest('hex');
const save=(p,v)=>{fs.mkdirSync(path.dirname(p),{recursive:true});fs.writeFileSync(p,typeof v==='string'?v:JSON.stringify(v,null,2));};
if(fs.existsSync(path.join(out,'freeze.json')))throw Error('Already prepared');
for(const id of ['intro','005','010'])for(const variant of ['bad','correct'])for(const file of ['input.json','research-result.json']){
 const rel=`cases/${id}/${variant}/${file}`;save(path.join(out,rel),fs.readFileSync(path.join(old,rel),'utf8'));
}
const labels=JSON.parse(fs.readFileSync(path.join(old,'labels.json'))).filter(x=>x.group==='historical');
save(path.join(out,'labels.json'),labels);
// Exact UTF-16 offsets; line endings and trailing whitespace belong to fragments.
for(const id of ['intro','005','010']){
 const input=JSON.parse(fs.readFileSync(path.join(out,`cases/${id}/bad/input.json`))),source=(input.input||input.Input).content??input.Input.Content;
 const fragments=[];let start=0,index=1;
 for(const text of source.match(/[^\n]*\n|[^\n]+$/g)||[]){fragments.push({id:`F${String(index++).padStart(4,'0')}`,start,length:text.length,text});start+=text.length;}
 if(fragments.map(x=>x.text).join('')!==source)throw Error('Fragment roundtrip failed');
 save(path.join(out,`fragments/${id}.json`),{sourceSha256Utf8:hash(Buffer.from(source,'utf8')),offsetUnit:'UTF-16 code units',fragments});
}
save(path.join(out,'prompts/judge.md'),fs.readFileSync(path.join(old,'prompts/judge.md'),'utf8'));
let mapper=fs.readFileSync(path.join(old,'prompts/mapper.md'),'utf8');
mapper=mapper.replace('sourceQuotes (one or more nonempty exact contiguous substrings copied from originalSource, without normalization or ellipses)','sourceFragmentIds (one or more IDs selected from sourceFragments; never generate quotations or offsets)');
mapper+='\nsourceFragments are deterministic unmodified line spans from originalSource, including their original line endings. Select every fragment required for scope and attribution. The software resolves IDs to original text without normalization. Unknown or duplicate IDs invalidate the map. Fragment selection establishes provenance only, never semantic support. No Research is supplied.\n';
save(path.join(out,'prompts/mapper.md'),mapper);
const plan=[];for(const id of ['intro','005','010'])plan.push({id:`${id}-map`,pair:id,variant:'map',arm:'map'});
let n=0;for(const id of ['intro','005','010'])for(const variant of ['bad','correct']){
 const arms=n++%2===0?['without','with']:['with','without'];for(const arm of arms)plan.push({id:`${id}-${variant}-${arm}`,pair:id,variant,arm});
}
save(path.join(out,'plan.json'),plan);
const protectedFiles={};function walk(dir){for(const e of fs.readdirSync(dir,{withFileTypes:true})){if(['bin','obj','.vs'].includes(e.name))continue;let p=path.join(dir,e.name);if(e.isDirectory())walk(p);else protectedFiles[path.relative(root,p).replaceAll('\\','/')]=hash(fs.readFileSync(p));}}walk(path.join(root,'src'));
save(path.join(out,'protected-before.json'),protectedFiles);
console.log('Prepared exact historical cases, fragment catalogs, prompts, labels and 15-call plan. Freeze only after offline preflight.');
