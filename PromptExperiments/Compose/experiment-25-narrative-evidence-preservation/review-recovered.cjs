const fs=require('node:fs');
const path=require('node:path');
const {createRequire}=require('node:module');
const {compileFunction}=require('node:vm');

// Reuse the frozen evaluator without reopening any Compose slots.
const runner=path.join(__dirname,'runner.cjs');
const execute=compileFunction(fs.readFileSync(runner,'utf8')+
  "\nverify(); verify('candidate-freeze.json'); return assess('holdout','narrative');",
  ['require','__dirname','module'],{filename:runner});
execute(createRequire(runner),__dirname,{}).catch(error=>{
  console.error(error);
  process.exitCode=1;
});
