const fs=require('node:fs');
const path=require('node:path');
const {createHash}=require('node:crypto');
process.chdir(__dirname);
const read=f=>JSON.parse(fs.readFileSync(f,'utf8'));
const write=(f,x)=>fs.writeFileSync(f,JSON.stringify(x,null,2)+'\n');
const hash=f=>createHash('sha256').update(fs.readFileSync(f)).digest('hex');
const subjects=['digital','webb','wolf'];
const recovery='recovery/input-case-rejections';
if(fs.existsSync(recovery))throw Error('Recovery already applied');
const entries=[];
for(const subject of subjects){
  for(const c of ['A','B']){
    const dir='narratives/'+c+'/'+subject;
    const attempt=read(dir+'/attempt.json');
    const error=fs.readFileSync(dir+'/stderr.log','utf8');
    if(attempt.exit!==2||attempt.valid||fs.existsSync(dir+'/result.json')||
      !error.includes("The JSON property 'Title' could not be mapped"))
      throw Error('Attempt was not a pre-model casing failure: '+dir);
    entries.push({subject,c,dir,target:recovery+'/'+c+'/'+subject});
  }
}
const additional=[];
for(const subject of subjects){
  const base='fixtures/'+subject+'/upstream.json';
  const orig=read(base);
  const idea=Object.fromEntries(Object.entries(orig.selectedIdea).map(([k,v])=>[k[0].toLowerCase()+k.slice(1),v]));
  const file=base.replace('upstream.json','cli-input.json');
  write(file,{...orig,selectedIdea:idea});
  additional.push(file);
}
for(const {dir,target} of entries){
  const root=path.resolve('.');
  const source=path.resolve(dir);
  const dest=path.resolve(target);
  if(!source.startsWith(path.join(root,'narratives')+path.sep)||!dest.startsWith(path.join(root,'recovery')+path.sep))
    throw Error('Recovery move must stay inside its case directories');
  fs.mkdirSync(path.dirname(dest),{recursive:true});
  fs.renameSync(source,dest);
}
fs.copyFileSync('runner.cjs','runner.pre-recovery.cjs',fs.constants.COPYFILE_EXCL);
fs.copyFileSync('freeze.json','freeze.pre-recovery.json',fs.constants.COPYFILE_EXCL);
fs.copyFileSync('candidate-freeze.json','candidate-freeze.pre-recovery.json',fs.constants.COPYFILE_EXCL);
let runner=fs.readFileSync('runner.cjs','utf8');
const input="const dir=narrative(s,c), input=fixture(s,'upstream.json'), result=dir+'/result.json';";
if(!runner.includes(input))throw Error('Expected frozen runner input line');
runner=runner.replace(input,"const dir=narrative(s,c), result=dir+'/result.json';\n      const normalized=fixture(s,'cli-input.json');\n      const input=c!=='C'&&has(normalized)?normalized:fixture(s,'upstream.json');");
runner=runner.replace("if(command==='inventories')return inventories();","if(command==='inventories')return inventories();\n  if(command==='recover-narratives')return buildNarratives('holdout');");
fs.writeFileSync('runner.cjs',runner);
const frozen=read('freeze.json');
for(const f of [...additional,'runner.cjs','runner.pre-recovery.cjs','freeze.pre-recovery.json','candidate-freeze.pre-recovery.json','recover-case.cjs'])
  frozen.hashes[f]=hash(f);
write('freeze.json',{...frozen,recoveryAt:new Date().toISOString(),recoveryReason:'Current CLI rejects the archived PascalCase Idea field names before reaching the model. Preserve all original values, original inputs and failed attempts. One casing-only pre-model recovery for paired A and B.',hashes:frozen.hashes});
const candidate=read('candidate-freeze.json');
candidate.hashes['freeze.json']=hash('freeze.json');
write('candidate-freeze.json',candidate);
write('recovery/input-case-recovery.json',{at:new Date().toISOString(),subjects,rejectedAttempts:entries.length,
  reachedProvider:0,mappedInputPaths:additional,originalInputsRetained:true});
