const fs=require('fs'),p=require('path'),crypto=require('crypto');
const out=__dirname,root=p.resolve(out,'../../..'),file=p.join(out,'freeze.json');
if(fs.existsSync(p.join(out,'calls')))throw Error('Cannot refreeze after attempts');
fs.copyFileSync(file,p.join(out,'freeze-preflight.json'),fs.constants.COPYFILE_EXCL);
const freeze=JSON.parse(fs.readFileSync(file));
freeze.preflightNote='Runner array syntax corrected and unused local removed after offline build failure; zero calls. Protocol, rubric, fixtures and labels unchanged. Final freeze before first model call.';
freeze.frozenAt=new Date().toISOString();
freeze.files['PromptExperiments/Research/evidence-map-experiment-3-2026-11-12/runner/Program.cs']=crypto.createHash('sha256').update(fs.readFileSync(p.join(out,'runner/Program.cs'))).digest('hex');
fs.writeFileSync(file,JSON.stringify(freeze,null,2));
