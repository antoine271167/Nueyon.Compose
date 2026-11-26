const fs=require('node:fs');
const path=require('node:path');
const {createHash}=require('node:crypto');
const {execFileSync}=require('node:child_process');
process.chdir(__dirname);
const read=f=>JSON.parse(fs.readFileSync(f,'utf8'));
const hash=f=>createHash('sha256').update(fs.readFileSync(f)).digest('hex');
const files=dir=>fs.readdirSync(dir,{recursive:true,withFileTypes:true}).filter(e=>e.isFile()).map(e=>path.join(e.parentPath,e.name).replaceAll('\\','/'));
const command=process.argv[2]??'verify';
const manifest='handoff-hashes.json';
const current=Object.fromEntries(files('narratives').filter(f=>f.endsWith('/result.json')).sort().map(f=>[f,hash(f)]));
if(command==='snapshot'){
  if(Object.keys(current).length!==54)throw Error('Only snapshot after all 54 artifacts exist');
  fs.writeFileSync(manifest,JSON.stringify({time:new Date().toISOString(),hashes:current},null,2)+'\n',{flag:'wx'});
}else{
  execFileSync(process.execPath,['runner.cjs','verify'],{stdio:'inherit'});
  const initial=read('freeze.initial.json');
  for(const [f,h] of Object.entries(initial.hashes)){
    const original=f==='runner.cjs'?'runner.initial.cjs':f;
    if(hash(original)!==h)throw Error('Original freeze mismatch: '+original);
  }
  const first=read('inventory-freeze.initial.json');
  for(const [f,h] of Object.entries(first.hashes)){
    const initialF=f.replace('verified.json','verified-initial.json');
    const original=fs.existsSync(initialF)?initialF:f;
    if(hash(original)!==h)throw Error('Original inventory mismatch: '+original);
  }
  for(const [f,h] of Object.entries(read(manifest).hashes))
    if(current[f]!==h)throw Error('Narrative hash mismatch '+f);
  for(const f of files('prompts/production')){
    const relative=f.slice('prompts/production/'.length);
    const actual='../../../src/Nueyon.Compose.Application/Prompts/'+relative;
    if(hash(f)!==hash(actual))throw Error('Production prompt differed from baseline '+relative);
  }
  for(const s of read('dataset.json')){
    const folder='narratives/C-reference/'+s.subject;
    const trace=read(folder+'/trace.json');
    const input=read('fixtures/'+s.subject+'/upstream.json');
    const verified=read('evidence-inventories/'+s.subject+'/verified.json').items;
    if(JSON.stringify(trace.items)!==JSON.stringify(verified))throw Error('C used an arbitrary inventory '+s.subject);
    if(trace.items.some(i=>!Object.values(input.synthesis).some(value=>typeof value==='string'&&value.includes(i.synthesisExcerpt))))
      throw Error('C used content outside Synthesis '+s.subject);
    const angle=input.selectedIdea.title??input.selectedIdea.Title;
    const distinct=[...new Set(trace.items.map(i=>i.synthesisExcerpt))];
    const expected='### Selected Story\n'+angle+'\n\n### Story Progression\n'+distinct.map((x,i)=>
      '- **'+(i===distinct.length-1?'Endpoint':i===0?'Context':'Development')+':** '+x).join('\n')+
      '\n\n### Critical Constraints\n- Preserve the scope and qualifications of the developments.';
    if(expected!==read(folder+'/result.json').content)throw Error('C asserted additional unsupported content '+s.subject);
  }
  execFileSync(process.execPath,['analyze.cjs'],{stdio:'inherit'});
  console.log('Frozen data, original snapshots, Narrative versions, Compose inputs and review linkage verified.');
}
