const fs=require('node:fs');
const {createHash}=require('node:crypto');
process.chdir(__dirname);
const read=f=>JSON.parse(fs.readFileSync(f,'utf8'));
const write=(f,x)=>fs.writeFileSync(f,JSON.stringify(x,null,2)+'\n');
const hash=f=>createHash('sha256').update(fs.readFileSync(f)).digest('hex');
const original=read('freeze.json');
if(fs.existsSync('freeze.initial.json'))throw Error('Supplement already applied');
const inventoryFreeze=read('inventory-freeze.json');
const supplements=read('source-supplements.json');
for(const [subject,record] of Object.entries(supplements)){
  const folder='evidence-inventories/'+subject;
  const before=folder+'/verified.json';
  if(hash(before)!==inventoryFreeze.hashes[before])throw Error('Inventory already changed');
  fs.copyFileSync(before,folder+'/verified-initial.json',fs.constants.COPYFILE_EXCL);
  const draft=read(folder+'/draft/parsed.json');
  const selected=draft.items.filter(i=>i.useful&&!record.excluded?.includes(i.id));
  if(selected.length===0)throw Error(subject+' lacks verified evidence');
  write(before,{...read(before),items:selected,operatorSourceAudit:record});
  inventoryFreeze.hashes[before]=hash(before);
}
fs.copyFileSync('freeze.json','freeze.initial.json',fs.constants.COPYFILE_EXCL);
fs.copyFileSync('inventory-freeze.json','inventory-freeze.initial.json',fs.constants.COPYFILE_EXCL);
write('inventory-freeze.json',{...inventoryFreeze,revisedAt:new Date().toISOString()});
fs.copyFileSync('runner.cjs','runner.initial.cjs',fs.constants.COPYFILE_EXCL);
const runner=fs.readFileSync('runner.cjs','utf8');
const old="const source = s => read(fixture(s,'source.txt'));";
const next="const source = s => {\n  const supplements=json('source-supplements.json');\n  return read(fixture(s,'source.txt'))+(supplements[s]?'\\n\\nAuthoritative source verification by operator: '+JSON.stringify(supplements[s]):'');\n};";
if(!runner.includes(old))throw Error('Unexpected runner source function');
fs.writeFileSync('runner.cjs',runner.replace(old,next));
for(const f of ['runner.cjs','runner.initial.cjs','freeze.initial.json','inventory-freeze.initial.json','supplement.cjs','source-supplements.json'])
  original.hashes[f]=hash(f);
for(const subject of Object.keys(supplements)){
  const f='evidence-inventories/'+subject+'/verified-initial.json';
  original.hashes[f]=hash(f);
}
write('freeze.json',{time:original.time,revisionAt:new Date().toISOString(),reason:'Reviewer-only primary-source corroboration before any holdout generation. No generation inputs, prompts, gates or plan changed.',hashes:original.hashes});
if(fs.existsSync('candidate-freeze.json')){
  const prior=read('candidate-freeze.json');
  fs.copyFileSync('candidate-freeze.json','candidate-freeze.initial.json',fs.constants.COPYFILE_EXCL);
  for(const f of ['freeze.json','inventory-freeze.json','candidate-freeze.initial.json'])prior.hashes[f]=hash(f);
  write('candidate-freeze.json',prior);
}
