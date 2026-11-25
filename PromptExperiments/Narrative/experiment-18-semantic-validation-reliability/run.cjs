const fs = require('node:fs');
const path = require('node:path');
const { createHash } = require('node:crypto');
const { execFile } = require('node:child_process');
const { parse } = require('../experiment-17-semantic-validation/run.cjs');
const local = name => path.join(__dirname, name);
const read = name => fs.readFileSync(local(name), 'utf8');
const obj = name => JSON.parse(read(name));
const save = (name, data) => fs.writeFileSync(local(name), JSON.stringify(data, null, 2) + '\n');
const hash = name => createHash('sha256').update(fs.readFileSync(local(name))).digest('hex');
const frozen = ['protocol.md','validator.prompt.md','inputs.json','sources.json','ground-truth.json','dataset-manifest.csv','model-configuration.json','source-provenance.md','run.cjs'];
const loader = process.env.EXPERIMENT17_COPILOT_LOADER;
const ground = obj('ground-truth.json');
const sources = obj('sources.json');
const inputs = obj('inputs.json');
const conditions = ['S','S+Source'];
function verify() {
  if (hash('validator.prompt.md') !== hash('../experiment-17-semantic-validation/validator.prompt.md')) throw Error('Validator differs from Experiment 17');
  for (const file of frozen) if (obj('freeze.json')[file] !== hash(file)) throw Error('Freeze mismatch: '+file);
  if (ground.length !== 48 || Object.keys(sources).length !== 12 || ground.filter(x => x.label === 'F2').length !== 24) throw Error('Invalid cohort');
  for (const item of ground) {
    if (!inputs[item.id].narrative.content.includes(item.narrativeExcerpt) || !sources[item.subject].source.includes(item.sourceExcerpt)) throw Error('Excerpt mismatch: '+item.id);
  }
}
async function attempt(task) {
  const {item, condition, repetition, seq} = task;
  const dir = 'runs/'+item.id+'/'+condition+'/'+repetition;
  if (fs.existsSync(local(dir))) {
    if (!fs.existsSync(local(dir+'/attempt.json'))) throw Error('Interrupted attempt requires explicit accounting: '+dir);
    return obj(dir+'/attempt.json');
  }
  fs.mkdirSync(local(dir), {recursive:true});
  const input = {...inputs[item.id]};
  if (condition === 'S+Source') input.source = sources[item.subject].source;
  save(dir+'/input.json', input);
  const prompt = read('validator.prompt.md').trim()+'\n\n'+JSON.stringify(input);
  const args = [loader,'--model','gpt-6-luna','--silent','--no-custom-instructions','--disable-builtin-mcps','--available-tools','experiment17_no_tools','-p',prompt];
  const startedAt = new Date().toISOString();
  const start = Date.now();
  const meta = await new Promise(resolve => execFile(process.execPath,args,{windowsHide:true,timeout:180000,maxBuffer:1048576,encoding:'buffer'},(error,stdout,stderr) => {
    fs.writeFileSync(local(dir+'/response.txt'),stdout);
    fs.writeFileSync(local(dir+'/stderr.log'),stderr);
    const meta = {seq,id:item.id,condition,repetition,startedAt,seconds:(Date.now()-start)/1000,exitCode:error?.code??0,valid:false};
    try {
      if (error) throw Error('Transport exited: '+(error.code??error.signal??'unknown'));
      meta.status = parse(stdout.toString('utf8')).status;
      meta.valid = true;
    } catch (failure) {meta.validationError = failure.message;}
    save(dir+'/attempt.json',meta);
    resolve(meta);
  }));
  console.log(seq,item.id,condition,repetition,meta.status??meta.validationError,meta.seconds);
  return meta;
}
async function main() {
  if (process.argv.includes('--freeze')) {
    if (fs.existsSync(local('freeze.json')) || fs.existsSync(local('runs'))) throw Error('Already frozen or started');
    save('freeze.json',Object.fromEntries(frozen.map(file=>[file,hash(file)])));
    verify();
    console.log('Frozen 12 subjects, 48 examples, 192 planned attempts.');
    return;
  }
  verify();
  let seq = 0;
  let bad = 0;
  for (const repetition of [1,2]) {
    const cohort = repetition === 1 ? ground : [...ground].reverse();
    const tasks = cohort.flatMap(item=>{
      const reverse = (Number(item.id.slice(1))+repetition)%2;
      return (reverse?[...conditions].reverse():conditions).map(condition=>({item,condition,repetition,seq:++seq}));
    });
    for (let i=0; i<tasks.length; i+=8) {
      if (bad>=8) throw Error('Stopped after eight or more transport failures');
      await Promise.all(Array.from({length:4},async (_,lane)=>{
        for (const task of tasks.slice(i+2*lane,i+2*lane+2)) {
          const row = await attempt(task);
          if (row.exitCode !== 0) bad++;
        }
      }));
    }
  }
  verify();
}
if (require.main===module) main().catch(error=>{console.error(error);process.exitCode=1;});
module.exports = {verify};

