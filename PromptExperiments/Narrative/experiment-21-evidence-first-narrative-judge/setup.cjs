const fs=require('node:fs');
const path=require('node:path');
const crypto=require('node:crypto');
process.chdir(__dirname);
const old='../experiment-20-fidelity-judge-repair-loop';
const read=p=>fs.readFileSync(p,'utf8').replace(/^\uFEFF/,'');
const load=p=>JSON.parse(read(p));
const save=(p,data)=>fs.writeFileSync(p,JSON.stringify(data,null,2)+'\n',{flag:'wx'});
const config=load('model-configuration.json');
const cases=[];
const truth={};
const add=(id,subject,split,cohort,input,source,initial,label,finding)=>{
  cases.push({id,subject,split,cohort,input,source,initial});
  truth[id]={label,findings:finding?[finding]:[]};
};
const findings=load(old+'/archived-material-review.json');
for(const t of load(old+'/dataset.json').filter(t=>t.initial)){
  const finding=findings[t.id];
  if(finding?.category==='quantitative_meaning') finding.category='numerical_scale';
  const split=['4632ab6ce2','2336788e5a'].includes(t.id)?'development':'evaluation';
  add(t.id,t.subject,split,t.cohort,...[t.input,t.source,t.initial].map(p=>old+'/'+p),t.prior,finding);
}
const mutations={
  bennu:['actor_object','About 20 minutes after releasing the capsule above Earth’s atmosphere, the spacecraft fired its engines to divert away from Earth.','About 20 minutes after its release above Earth’s atmosphere, the capsule fired its engines to divert away from Earth.','The engine firing is attributed to the capsule instead of the spacecraft.'],
  earthquake:['numerical_scale','measured amplitude increases tenfold, while energy release increases 32-fold','measured amplitude increases 32-fold, while energy release increases tenfold','The measured amplitude and energy multipliers are swapped.'],
  coral:['scope_certainty','corals have been known to recover when stress-caused bleaching is not severe','corals always recover when stress-caused bleaching is not severe','Documented possibility is changed into a guarantee.']
};
for(const [subject,[category,from,to,explanation]] of Object.entries(mutations)){
  const d='evaluation/'+subject;
  const source=d+'/source.txt';
  const original=d+'/original.json';
  const challenge=d+'/challenge.json';
  const input=d+'/input.json';
  const content=load(original).content;
  if(!content.includes(from)) throw Error('Missing mutation anchor '+subject);
  save(challenge,{content:content.replace(from,to)});
  add(subject+'-control',subject,'evaluation','new-production',input,source,original,'F0');
  add(subject+'-challenge',subject,'evaluation','new-mutation',input,source,challenge,'F2',{category,narrativeExcerpt:to,evidenceExcerpt:read(source).trim(),explanation});
}
for(const t of cases){
 const narrative=load(t.initial);
 const content=narrative.content??narrative.Content;
 if(typeof content!=='string'||!content.trim()) throw Error('Missing Narrative '+t.id);
 for(const f of truth[t.id].findings)
  if(!content.includes(f.narrativeExcerpt)||!read(t.source).includes(f.evidenceExcerpt)) throw Error('Nonverbatim anchor '+t.id);
}
save('ground-truth.json',truth);
save('dataset.json',cases);
fs.mkdirSync('development',{recursive:true});
fs.writeFileSync('development/cases.md','# Development set\n\nWolf 4632ab6ce2 and Webb 2336788e5a informed the design. Other variants remain held out at brief level. The same source facts recur in evaluation.\n');
fs.writeFileSync('dataset-manifest.csv', ['id,subject,split,cohort,input,source,initial',...cases.map(t=>Object.values(t).join(','))].join('\n')+'\n',{flag:'wx'});
const prod='../../../src/Nueyon.Compose.Application/Prompts/Narrative';
const files=[
 'dataset.json','dataset-manifest.csv','ground-truth.json','model-configuration.json','setup.cjs','run.cjs',
 'protocol.md','hypothesis.md','claim-extraction.prompt.md','evidence-comparison.prompt.md','judge-schema.json','source-provenance.md','development/cases.md',
 old+'/judge.prompt.md',old+'/judge-schema.json',old+'/run.cjs',old+'/editorial-scores.csv',
 '../../../src/Nueyon.Compose.Host.Console/appsettings.json',
 ...fs.readdirSync(prod).map(name=>prod+'/'+name),...cases.flatMap(t=>[t.input,t.source,t.initial])
];
save('freeze.json',Object.fromEntries([...new Set(files)].map(f=>[f,crypto.createHash('sha256').update(fs.readFileSync(f)).digest('hex')])));

