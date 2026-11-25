// Schedule independent second reviews with the frozen runner and normalized label decoding.
// No model payload, sampling, client, or archive format is changed.
const fs=require('node:fs');
const path=require('node:path');
const vm=require('node:vm');
const {createRequire}=require('node:module');
const api=require('./run.cjs');
const {readReview}=require('./review-records.cjs');
const filename=path.join(__dirname,'run.cjs');
const runner={exports:{}};
const source=fs.readFileSync(filename,'utf8');
vm.runInThisContext('(function(require,module,__dirname){'+source+'\nmodule.exports.review=review;\n})',
  {filename})(createRequire(filename),runner,__dirname);
const {review}=runner.exports;
async function main() {
  api.verify();
  const mode=process.argv[2]??'second';
  const end=process.argv.includes('--final');
  const tasks=api.tasks.filter(t=>api.exists(api.out(t)+'/attempt.json')&&api.json(api.out(t)+'/attempt.json').valid);
  const candidates=tasks.filter(t=>{
    const first=readReview(t,'factual',1);
    return t.rep===2||(first&&(!first.usable||first.data.claims.some(c=>['F2','U'].includes(c.label))))||(!first&&end);
  });
  const pass=mode==='second'?2:1;
  const kind=mode==='second'?'factual':mode;
  const selected=mode==='second'?candidates:tasks.slice(-24).reverse();
  const queue=selected.filter(t=>!api.exists(api.reviewDir(t,kind,pass)+'/started.json'));
  const width=mode==='second'?6:4;
  await Promise.all(Array.from({length:width},async()=>{
    while(queue.length) {
      const t=queue.shift();
      if(!api.exists(api.reviewDir(t,kind,pass)+'/started.json')) await review(t,kind,pass);
    }
  }));
}
main().catch(e=>{console.error(e);process.exitCode=1;});
