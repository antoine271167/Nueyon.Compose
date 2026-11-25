const fs=require('node:fs');
const {createHash}=require('node:crypto');
const {execFileSync}=require('node:child_process');
process.chdir(__dirname);
const read=p=>fs.readFileSync(p,'utf8').replace(/^\uFEFF/,'');
const load=p=>JSON.parse(read(p));
const save=(p,v)=>fs.writeFileSync(p,JSON.stringify(v,null,2)+'\n');
const prior='../experiment-21-evidence-first-narrative-judge/';
const archive=load(prior+'dataset.json');
const oldTruth=load(prior+'ground-truth.json');
const subjects={
 wolf:['entity_identity','The identified elk in the prey composition are North American elk (wapiti), with calves and adult males as separate classes.','elk|eland|wapiti|prooi'],
 webb:['temporal_relationship','Cooling was already changing during sunshield deployment; later continuation is compatible.','afkoel|cool|uitvou|deploy'],
 faa:['entity_identity','The cylinder denotes the nominal wrong surface hot spot category.','cilinder|cylinder|oppervlak|verkeerd'],
 eia:['numerical_scale','The two coefficients apply to total energy production in the same unit and scale.','Btu|energy production|energieproductie'],
 digital:['temporal_relationship','The alternation happened within a two-week sprint; testing and research occurred both weeks.','sprint|weken|onderzoek|beta']
};
const invariantRefs={wolf:'4632ab6ce2',webb:'2336788e5a',faa:'a027be447f28',eia:'0f2553c79399',digital:'166274757454'};
const invariants={};
for(const [subject,[category,relation,anchors]] of Object.entries(subjects)){
 invariants[subject]={subject,category,relation,anchors,source:oldTruth[invariantRefs[subject]].findings[0].evidenceExcerpt};
}
const cases=[];
const truth={};
function add(row,label,findings,origin){
 const brief=load(row.initial);
 const content=brief.content??brief.Content;
 if(typeof content!=='string'||!content.includes('Story Progression')) throw Error('Incomplete '+row.id);
 if(!read(row.source).includes(invariants[row.subject].source)) throw Error('Missing invariant '+row.id);
 for(const finding of findings){
  if(!content.includes(finding.narrativeExcerpt)) throw Error('Missing target '+row.id);
  if(!read(row.source).includes(finding.evidenceExcerpt)) throw Error('Missing provenance '+row.id);
 }
 cases.push(row);
 truth[row.id]={label,findings,origin};
}
for(const test of archive.filter(t=>Object.hasOwn(subjects,t.subject)&&t.cohort.startsWith('historical'))){
 const split=['wolf','webb'].includes(test.subject)?'development':'evaluation';
 const result=oldTruth[test.id];
 add({...test,split,repeat:split==='evaluation'&&result.label==='F2'},result.label,result.findings,'Experiment 21 archived evidence adjudication');
}

const changes={
 ice:[[/less dense/g,'denser']],
 leap:[[/UTC is based on TAI and is adjusted by leap seconds/g,'TAI is adjusted by leap seconds to match UTC'],[/UTC is based on TAI, but leap-second adjustments distinguish the two/g,'TAI is adjusted by leap seconds to match UTC'],[/UTC’s basis in TAI/g,'TAI’s adjustment toward UTC']],
 voyager:[[/Voyager 2/g,'Voyager 1']],
 fdic:[[/per depositor, per FDIC-insured bank, and per ownership category/g,'per account'],[/these three units together frame/g,'this unit frames'],[/the three units to which that amount applies: depositor, FDIC-insured bank, and ownership category/g,'that the amount applies separately to each account'],[/all three units together/g,'each account separately']],
 homestead:[[/continuous residence for five years was required before title could be received/g,'title was received before the five years of residence began'],[/five years of continuous residence before title could be received/g,'title was received before the five years of residence began'],[/made continuous residence a requirement along the path from claim to title/g,'made continuous residence a requirement beginning after title']],
 wright:[[/Orville/g,'Wilbur']],
 sqlite:[[/readers do not block writers, and writers do not block readers/g,'readers must wait until writing has finished'],[/can proceed concurrently rather than waiting on one another/g,'must proceed in sequence'],[/reading and writing can proceed concurrently/g,'readers wait until writes finish']],
 http:[[/must not send content/g,'must send content'],[/with response content omitted/g,'with response content included'],[/absence of response content/g,'presence of response content']],
 json:[[/UTF-8/g,'UTF-16']],
 uuid:[[/48-bit/g,'84-bit']]
};
const provenance=['# Source provenance','','New excerpts were retrieved from the listed public sources and preserved verbatim as contiguous spans with line breaks normalized. The short excerpts are the experimental evidence boundary. The prior reports and immutable archive paths document the historical sources.',''];
for(const [subject,url,source,category,mutation,anchors,title] of load('sources.json')){
 invariants[subject]={subject,source,category,relation:title,anchors};
 const dir='fixtures/'+subject;
 provenance.push('- ['+subject+']('+url+') · '+dir+'/source.txt');
 for(let i=1;i<=3;i++){
  add({id:subject+'-natural-'+i,subject,split:'evaluation',cohort:'new-natural',input:dir+'/input.json',source:dir+'/source.txt',initial:dir+'/natural-'+i+'.json',repeat:i===1},
   'F0',[],'Pre-run source comparison; see editorial-impact.md for review limitations');
 }
 let content=load(dir+'/natural-1.json').content;
 for(const [pattern,replacement] of changes[subject]) content=content.replace(pattern,replacement);
 const target=changes[subject][0][1];
 if(!content.includes(target)||content===load(dir+'/natural-1.json').content) throw Error('Mutation failed '+subject);
 save(dir+'/challenge.json',{content});
 add({id:subject+'-challenge',subject,split:'evaluation',cohort:'new-mutation',input:dir+'/input.json',source:dir+'/source.txt',initial:dir+'/challenge.json',repeat:true},
  'F2',[{category,narrativeExcerpt:target,evidenceExcerpt:source,explanation:mutation}],'Operator mutation checked against source before comparisons');
}
save('dataset.json',cases);
save('ground-truth.json',truth);
save('invariants.json',invariants);
fs.writeFileSync('source-provenance.md',provenance.join('\n')+'\n');
fs.writeFileSync('dataset-manifest.csv',['id,subject,split,cohort,input,source,initial,repeat',...cases.map(t=>[t.id,t.subject,t.split,t.cohort,t.input,t.source,t.initial,t.repeat].join(','))].join('\n')+'\n');
const config=load('model-configuration.json');
config.sourceCommit=execFileSync('git',['rev-parse','HEAD'],{encoding:'utf8'}).trim();
config.dotnet='10.0.404';
config.node=process.version;
config.copilot='1.0.116';
save('model-configuration.json',config);
const frozen=[
 'prepare.cjs','generate.cjs','run.cjs','dataset.json','dataset-manifest.csv','ground-truth.json','invariants.json','sources.json','source-provenance.md','protocol.md','hypothesis.md',
 'architecture.md','controlled-terms.json','invariants-schema.json','deterministic-rules.md','semantic-comparison.prompt.md','model-configuration.json',
 '../experiment-20-fidelity-judge-repair-loop/run.cjs','../experiment-20-fidelity-judge-repair-loop/judge.prompt.md','../experiment-20-fidelity-judge-repair-loop/judge-schema.json',
 ...['system','user'].map(type=>'../../../src/Nueyon.Compose.Application/Prompts/Narrative/Narrative.'+type+'.prompt.md'),
 ...cases.flatMap(t=>[t.input,t.source,t.initial])
];
save('freeze.json',Object.fromEntries([...new Set(frozen)].sort().map(p=>[p,createHash('sha256').update(fs.readFileSync(p)).digest('hex')])));
console.log(cases.length+' cases frozen');
