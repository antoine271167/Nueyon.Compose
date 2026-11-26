const fs=require('node:fs');
const path=require('node:path');
const {createRequire}=require('node:module');
const {compileFunction}=require('node:vm');
const runner=path.join(__dirname,'runner.cjs');
const stage=process.argv[2];
const phase=process.argv[3]??'holdout';
if(!['narrative','article','linkedin'].includes(stage))throw Error('Choose a review stage');
const supplement=String.raw`
verify();
const metrics=json(stage==='narrative'?'evidence-preservation-metrics.json':'article-metrics.json');
const missing=metrics.rows.filter(r=>r.stage===stage&&r.phase===phase&&r.generated&&!r.factualReviewed);
return pool(missing,async ({subject:s,c,id,rep})=>{
  const n=narrative(s,c)+'/result.json';
  const out=stage==='narrative'?n:'outputs/'+(stage==='linkedin'?'linkedin-neutrality':phase)+'/'+s+'-'+rep+'-'+c+'/result.json';
  const body=content(out), input=upstream(s), items=json(inventory(s)+'/verified.json').items;
  const payload={body,upstream:input,source:source(s),items,...(stage==='narrative'?{}:{narrative:content(n)})};
  const prompt=factualPrompt+' Use the exact property name "excerpt" for every factual claim. Copy short exact contiguous excerpts verbatim including case and punctuation. Use only the inventory IDs supplied and assign each once. Return compact JSON with no prose outside it.';
  const dest='reviews/'+stage+'-factual/'+phase+'/'+id+'-coverage';
  return review(dest,prompt,payload,x=>factualValid(x,body,items),{outputHash:hash(out),supplementalCoverage:true});
},6);
`;
const execute=compileFunction(fs.readFileSync(runner,'utf8')+'\n'+supplement,
  ['require','__dirname','module','stage','phase'],{filename:runner});
execute(createRequire(runner),__dirname,{},stage,phase).catch(error=>{
  console.error(error);
  process.exitCode=1;
});
