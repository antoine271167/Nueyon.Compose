const fs = require('node:fs');
const path = require('node:path');
const {spawn} = require('node:child_process');
const {read,hash,rows} = require('./inventory.cjs');
const local = f => path.join(__dirname,f);
const manifest = rows(local('dataset-manifest.csv'));
const loader = process.env.EXPERIMENT17_COPILOT_LOADER;
if (!loader || !fs.existsSync(loader)) throw Error('Set EXPERIMENT17_COPILOT_LOADER to Copilot npm-loader.js');
function verify() {
  for (const filename of ['dataset-freeze.json','validator-freeze.json']) {
    for (const [f,h] of Object.entries(JSON.parse(read(local(filename))))) {
      if(hash(local(f)) !== h) throw Error('Freeze mismatch: ' + f);
    }
  }
  for (const row of manifest) {
    for(const key of ['output','idea','synthesis','source'])
      if(hash(row[key]) !== row[key+'Sha256']) throw Error('Input hash mismatch: '+row[key]);
  }
}
const fields = ['category','severity','narrativeExcerpt','evidenceExcerpt','explanation','findingType','suggestedCorrection'];
const categories = ['entity_identity','actor_object','temporal_relationship','causality','numerical_scale','denominator','scope','certainty'];
function parse(raw) {
  const result = JSON.parse(raw);
  if (!['PASS','FAIL','REVIEW'].includes(result.status) || !Array.isArray(result.findings)) throw Error('Invalid top-level schema');
  for (const f of result.findings) {
    if (!f || fields.some(k => typeof f[k] !== 'string' || (!f[k].trim() && k !== 'evidenceExcerpt')) ||
        !categories.includes(f.category) || !['material','potential'].includes(f.severity) ||
        !['contradiction','unsupported_assertion','ambiguity'].includes(f.findingType)) throw Error('Invalid finding schema');
  }
  const expected = result.findings.some(f=>f.severity==='material') ? 'FAIL' : result.findings.length ? 'REVIEW' : 'PASS';
  if (result.status !== expected) throw Error('Inconsistent status and findings');
  return result;
}
async function run(row, condition) {
  const dir = local(path.join('runs',row.id,condition));
  if(fs.existsSync(dir)) throw Error('Attempt already exists: '+dir);
  fs.mkdirSync(dir,{recursive:true});
  const input = {selectedIdea:JSON.parse(read(row.idea)),synthesis:JSON.parse(read(row.synthesis)),narrative:JSON.parse(read(row.output))};
  if(condition==='S+Source') input.source = read(row.source);
  const prompt = read(local('validator.prompt.md')).trim()+'\n\n'+JSON.stringify(input);
  const started = Date.now();
  const stdout = fs.createWriteStream(path.join(dir,'response.txt'));
  const stderr = fs.createWriteStream(path.join(dir,'stderr.log'));
  const args = [loader,'--model','gpt-6-luna','--silent','--no-custom-instructions','--disable-builtin-mcps','--available-tools','experiment17_no_tools','-p',prompt];
  const child = spawn(process.execPath,args,{windowsHide:true,stdio:['ignore','pipe','pipe']});
  child.stdout.pipe(stdout);
  child.stderr.pipe(stderr);
  const timer = setTimeout(()=>child.kill(),180000);
  const closed = stream => new Promise(resolve=>stream.once('finish',resolve));
  const completed = await Promise.all([
    new Promise(resolve=>{
      child.once('error',e=>resolve({error:e.message}));
      child.once('close',(code,signal)=>resolve({code,signal}));
    }),closed(stdout),closed(stderr)
  ]);
  clearTimeout(timer);
  const meta = {...completed[0],seconds:(Date.now()-started)/1000};
  try {
    if (meta.code !== 0) throw Error('CLI failed');
    const result = parse(read(path.join(dir,'response.txt')));
    meta.valid = true;
    meta.status = result.status;
  } catch(e) {
    meta.valid = false;
    meta.validationError = e.message;
  }
  fs.writeFileSync(path.join(dir,'attempt.json'),JSON.stringify(meta,null,2)+'\n');
  console.log(row.id,condition,meta.status ?? meta.validationError,meta.seconds);
}
async function main() {
  verify();
  const split = process.argv[2];
  if (!['development','evaluation'].includes(split)) throw Error('Specify development or evaluation');
  const cases = manifest.filter(r=>r.split===split);
  let next=0;
  const worker = async () => {
    while(next<cases.length) {
      const i=next++;
      const conditions = i%2 ? ['S+Source','S'] : ['S','S+Source'];
      for(const condition of conditions) await run(cases[i],condition);
    }
  };
  await Promise.all(Array.from({length:4},worker));
  verify();
}
if(require.main===module) main().catch(e=>{console.error(e);process.exitCode=1;});
module.exports = {parse};

