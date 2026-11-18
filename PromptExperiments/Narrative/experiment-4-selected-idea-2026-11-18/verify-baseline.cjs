const fs=require('fs'), assert=require('node:assert/strict'), path=require('path');
const root='PromptExperiments/Narrative/';
const x=root+'experiment-4-selected-idea-2026-11-18';
const dirs=['baseline-experiment-1-2026-11-18','experiment-2-topic-neutral-2026-11-18','experiment-3-compact-2026-11-18'].map(d=>root+d);
const equal=(a,b)=>assert(fs.readFileSync(a).equals(fs.readFileSync(b)), a+' != '+b);
const fixtures=fs.readdirSync(x+'/cases',{recursive:true,withFileTypes:true}).filter(d=>d.isFile());
assert.equal(fixtures.length,20);
for(const f of fixtures){
 const current=path.join(f.parentPath,f.name);
 const relative=path.relative(x,current);
 for(const dir of dirs) equal(current,path.join(dir,relative));
}
const production='src/Nueyon.Compose.Application/Prompts/';
for(const file of fs.readdirSync(x+'/prompts/A/Narrative')){
 equal(x+'/prompts/A/Narrative/'+file,production+'Narrative/'+file);
 equal(x+'/prompts/A/Narrative/'+file,dirs[2]+'/prompts/A/Narrative/'+file);
 equal(x+'/prompts/B/Narrative/'+file,dirs[2]+'/prompts/B/Narrative/'+file);
}
equal(x+'/prompts/B/Narrative/Narrative.user.prompt.md',x+'/prompts/C/Narrative/Narrative.user.prompt.md');
for(const file of fs.readdirSync(x+'/prompts/Compose')) equal(x+'/prompts/Compose/'+file,production+'Compose/'+file);
console.log('20 fixture files byte-identical to each of Experiments 1–3. A, B and Compose prompt mappings and unchanged C user prompt verified.');
