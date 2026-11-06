const fs=require('fs'),path=require('path'),crypto=require('crypto'),cp=require('child_process');
const root=__dirname,repo=path.resolve(root,'../../..'),base=path.resolve(root,'../acceptance-v1');
const json=file=>JSON.parse(fs.readFileSync(file,'utf8').replace(/^\uFEFF/,''));
const hash=b=>crypto.createHash('sha256').update(b).digest('hex');
const git=(...args)=>cp.execFileSync('git',args,{cwd:repo,encoding:'utf8'}).trim();
const manifest=json(path.join(base,'test-manifest.json'));
const preflight=json(path.join(root,'preflight.json'));
const rows=manifest.runOrder.map(run=>{
 const dir=path.join(root,'runs',run.id),names=fs.existsSync(dir)?fs.readdirSync(dir).filter(n=>/^attempt-\d+$/.test(n)):[];
 const attempts=names.map(name=>{const file=path.join(dir,name,'execution.json');return {name,...(fs.existsSync(file)?json(file):{})};});
 const success=attempts.find(a=>a.ideaCaptured&&a.narrativeCaptured);
 if(!success)return {...run,attempts};
 const a=path.join(dir,success.name), f=path.join(dir,'evaluation.json'), ev=fs.existsSync(f)?json(f):null;
 const idea=json(path.join(a,'ideas-0.json')), selected=json(path.join(a,'selected-idea.json')).Idea;
 const narrative=json(path.join(a,'narrative.json')).Content;
 const runtime=json(path.join(a,'runtime.json'));
 const input=path.join(a,'work/data/input'), files=fs.readdirSync(input);
 const source=manifest.sources.find(s=>s.id===run.sourceId);
 const configValid=preflight.runtime.configuration.every(c=>hash(fs.readFileSync(path.join(a,'work',c.file)))===c.sha256);
 const log=fs.readFileSync(path.join(a,'console.log'),'utf8');
 return {...run,attempts,artifactPath:path.relative(root,a).replaceAll('\\','/'),exitCode:success.exitCode,
 verified:{completeIdea:Object.keys(idea).sort().join(',')==='Audience,Description,Evidence,Rationale,Title'&&Object.values(idea).every(v=>typeof v==='string'&&v.length>0),
 selectedMatches:JSON.stringify(idea)===JSON.stringify(selected),narrativeCaptured:typeof narrative==='string'&&narrative.length>0,
 sourceMatches:files.length===1&&files[0]===path.basename(source.file)&&hash(fs.readFileSync(path.join(input,files[0])))===source.sha256,
 configurationMatches:configValid&&JSON.stringify(runtime.runtime)===JSON.stringify(preflight.runtime),
 logHasExpectedStages:['IdeaAgent','ResearchAgent','SynthesizerAgent','NarrativeAgent'].every(stage=>log.includes('Agent '+stage+' invocation completed'))},
 evaluation:ev?{scores:ev.ideaQuality.scores,total:ev.ideaQuality.total,ideaPassed:ev.ideaQuality.passed,narrativePassed:ev.downstreamViability.passed,severe:ev.severeFidelityError,passed:ev.passed}:null};
});
const summary=items=>({planned:items.length,completed:items.filter(r=>r.artifactPath).length,evaluated:items.filter(r=>r.evaluation).length,ideaPassed:items.filter(r=>r.evaluation?.ideaPassed).length,narrativePassed:items.filter(r=>r.evaluation?.narrativePassed).length,passed:items.filter(r=>r.evaluation?.passed).length,severeRuns:items.filter(r=>r.evaluation?.severe).length});
const overall=summary(rows), bySource=Object.fromEntries(manifest.sources.map(s=>[s.id,summary(rows.filter(r=>r.sourceId===s.id))]));
const status=git('status','--short','--branch');
const sourceStatus=git('status','--porcelain','--','src','PromptExperiments/IdeaGenerator/acceptance-v1','PromptExperiments/IdeaGenerator/experiment-001');
const final={overall,bySource,acceptance:overall.evaluated!==15?'onbeslist':overall.passed>=14&&overall.severeRuns===0?'geslaagd':'afgewezen',rows,repository:{status,head:git('rev-parse','HEAD'),originMain:git('rev-parse','origin/main'),trackedProductionAndReferenceStatus:sourceStatus}};
fs.writeFileSync(path.join(root,'summary.json'),JSON.stringify(final,null,2));
console.log(JSON.stringify({overall,bySource,acceptance:final.acceptance,status,verificationProblems:rows.filter(r=>r.verified&&Object.values(r.verified).some(v=>!v)).map(r=>r.id)}));
