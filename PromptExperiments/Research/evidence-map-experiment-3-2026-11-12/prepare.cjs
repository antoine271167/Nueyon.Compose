const fs=require('fs'),p=require('path'),crypto=require('crypto'),cp=require('child_process');
const out=__dirname,root=p.resolve(out,'../../..'),old=p.join(out,'../judge-calibration-experiment-2-2026-11-12');
const hash=f=>crypto.createHash('sha256').update(fs.readFileSync(f)).digest('hex');
const read=f=>JSON.parse(fs.readFileSync(f,'utf8').replace(/^\uFEFF/,''));
const write=(f,v)=>{fs.mkdirSync(p.dirname(f),{recursive:true});fs.writeFileSync(f,JSON.stringify(v,null,2));};
if(fs.existsSync(p.join(out,'freeze.json')))throw Error('Already frozen');
let pairs=[];
function historical(id,requirements){
 const input=read(p.join(old,'cases','baseline-'+id,'input.json'));
 for(const [dest,src] of [['bad','baseline-'+id],['correct',id==='005'?'control-005-complete':'control-'+id]]){
  const dir=p.join(out,'cases',id,dest);fs.mkdirSync(dir,{recursive:true});
  fs.copyFileSync(p.join(old,'cases',src,'input.json'),p.join(dir,'input.json'));
  fs.copyFileSync(p.join(old,'cases',src,'research-result.json'),p.join(dir,'research-result.json'));
 }
 pairs.push({id,group:'historical',requirements,inputSource:'Experiment 2 unchanged',target:requirements[0].meaning});
}
const req=(id,quote,meaning)=>({id,sourceQuote:quote,meaning});
historical('intro',[
 req('meaning','Nuëyon.Compose is the first step because it gives the broader vision a concrete expression.','Explicit concrete-expression meaning; Unknown 4 in baseline denies this, distinct from unmeasured reader effect.'),
 req('stage','Nuëyon is at an early stage.','Early stage and broader portfolio under development; no proven outcomes, launch dates or existing portfolio implied.'),
 req('audience',"Nuëyon's intended audience is other AI-first companies:",'Company identity and intended other AI-first-company audience remain primary.'),
 req('roadmap','The roadmap is best understood as a sequence of development priorities rather than a fixed release schedule.','Incremental planned priorities, not delivery commitments.')]);
historical('005',[
 req('variation','Idea and Research outputs vary between runs.','Upstream outputs varied despite matched prompts; not an isolated causal Narrative intervention.'),
 req('tension','That internal tension may have reinforced the technical angle.','Baseline Synthesis contains internal company/product vs orchestration tension; possible contribution only.'),
 req('decision','**Decision: Do not promote.**','Candidate worse on both stage means, all candidate hierarchy/restraint failures; restored production, bounded to this test.'),
 req('backend','the model provider may change the backend behind an alias.','Dates differed and backend alias may drift; small sample cannot prove general performance.')]);
historical('010',[
 req('contrast','Selected Idea and earlier chain remain company-first; the artifact’s score gives downstream hierarchy 2/2.','009 baseline 001 Compose factual overclaim despite intact earlier hierarchy and 2/2; not a Compose-first hierarchy failure.'),
 req('missing','We can localize drift only from Synthesis onward for those chains;','007 has no Idea/Research extracts; earliest observable does not establish true first cause.'),
 req('multiple','The first visible editorial hierarchy break varies by run.','Idea, Synthesis and Narrative observed departures; Narrative recurrent but no universal root cause.'),
 req('dimensions','Factual and editorial failures are distinct.','Technical truth and editorial primacy separate; qualitative engagement evidence not consistent measured comparison.')]);
function hold(id,source,title,evidence,requirements,goodFacts,goodUnknowns,badFacts,badUnknowns,discard='None.'){
 const input={Input:{Content:source},SelectedIdea:{Idea:{Title:title,Description:title,Audience:'Reviewers',Rationale:'Preserve observed conclusions and their limits.',Evidence:evidence}}};
 const research=(facts,unknowns)=>({Content:`### Facts\n\n${facts}\n\n### Interpretations\n\n${discard}\n\n### Unknowns\n\n${unknowns}\n\n### Development sequence\n\nNone.\n\n### Editorial relevance\n\nNone.`});
 for(const variant of ['bad','correct']){write(p.join(out,'cases',id,variant,'input.json'),input);write(p.join(out,'cases',id,variant,'research-result.json'),variant==='bad'?research(badFacts,badUnknowns):research(goodFacts,goodUnknowns));}
 pairs.push({id,group:'new-holdout',requirements,target:requirements[0].meaning});
}
hold('garden',
 'The garden board calls the seed library its first practical expression of the neighbourhood-sharing plan. This is the board’s stated meaning, not a measured result. The library is a pilot; expansion is undecided. No visitor survey or sharing-outcome study has been conducted. Opening hours are not yet fixed.',
 'A sharing ambition takes a first practical form','The library has already increased neighbourhood sharing.',[
 req('meaning','The garden board calls the seed library its first practical expression of the neighbourhood-sharing plan.','Preserve attributed meaning; baseline Unknown denies it; Idea evidence invents an increase and must not become evidence.'),
 req('limits','No visitor survey or sharing-outcome study has been conducted.','No measured outcomes; pilot/undecided expansion and opening hours are genuine uncertainty.')],
 '- The board describes the seed library as its first practical expression of the sharing plan.\n- It is a pilot and expansion is undecided.',
 '- No survey or outcome study establishes visitor effects or increased sharing; opening hours are unfixed.',
 '- The board describes the seed library as its first practical expression of the sharing plan.\n- It is a pilot and expansion is undecided.',
 '- The source does not establish that the library expresses the sharing plan or increases sharing. Opening hours are unfixed.');
hold('kiln',
 'Kiln log: week A used controller X and recorded 12 cracked tiles in 100. Week B used controller Y and recorded 5 in 100. Clay batches and drying humidity differed between weeks. A same-controller X check also recorded 5 in 100 after a batch change. The team retained Y for a longer trial; it did not conclude that Y caused the reduction. The log contains no long-term durability measurements.',
 'Lower cracking merits another trial, not a causal victory','Y reduced cracks from 12 to 5.',[
 req('confounder','Clay batches and drying humidity differed between weeks.','Specific upstream material/humidity confounders missing in bad downstream projection.'),
 req('negative','A same-controller X check also recorded 5 in 100 after a batch change.','Negative control counters controller-exclusive causal story; preserved only in discarded section of bad variant.'),
 req('decision','it did not conclude that Y caused the reduction.','Observed counts and continued trial retained without causal claim; durability unmeasured.')],
 '- X recorded 12/100 cracked tiles in A; Y recorded 5/100 in B. Clay batches and humidity differed.\n- An unchanged-X check recorded 5/100 after a batch change. Y was retained for a longer trial without a causal conclusion.',
 '- Controller causation and long-term durability remain unestablished.',
 '- X recorded 12/100 cracked tiles in A; Y recorded 5/100 in B. Y was retained for a longer trial.',
 '- The cause of the difference and long-term durability remain unknown.',
 'Clay batches and humidity differed; the same-X check recorded 5/100 after a batch change.');
hold('catalogue',
 'Catalogue audit: batch North preserved the intended shelf order but its printed labels gave wrong years. Batch South printed the years correctly but reversed shelf order. Batch West had both dimensions correct. The audit found no single earliest cause across batches; a missing intake log prevents attribution for North. It recommended checking dates and shelf order separately. No reader-comprehension tests were run.',
 'Two audit dimensions need separate checks','Wrong dates prove shelf-order failure.',[
 req('contrast','batch North preserved the intended shelf order but its printed labels gave wrong years.','North date error with correct order must survive; bad variant collapses dimensions and omits North.'),
 req('reverse','Batch South printed the years correctly but reversed shelf order.','Reverse contrast and West correct-both are controls; no single dimension explains all failures.'),
 req('limits','a missing intake log prevents attribution for North.','Missing observation limits causal attribution; no comprehension evidence.')],
 '- North had wrong years with correct shelf order. South had correct years with reversed order. West had both correct.\n- Audit recommended separate checks; no single earliest cause was found, and missing North intake data prevents attribution.',
 '- Causes and reader comprehension are not established.',
 '- South had correct years with reversed shelf order. West had both correct.\n- Audit recommended separate checks; no single earliest cause was found, and missing North intake data prevents attribution.',
 '- Causes and reader comprehension are not established.');
for(const pair of pairs){const source=read(p.join(out,'cases',pair.id,'bad/input.json')).Input.Content;for(const r of pair.requirements)if(!source.includes(r.sourceQuote))throw Error('Invalid reference anchor '+pair.id+' '+r.id);}
write(p.join(out,'labels.json'),pairs);
let protectedFiles={};function walk(dir,collector){for(const e of fs.readdirSync(dir,{withFileTypes:true})){if(['bin','obj','.vs'].includes(e.name))continue;let f=p.join(dir,e.name);if(e.isDirectory())walk(f,collector);else collector[p.relative(root,f).replaceAll('\\','/')]=hash(f);}}
walk(p.join(root,'src'),protectedFiles);write(p.join(out,'before-hashes.json'),protectedFiles);
fs.mkdirSync(p.join(out,'runner'),{recursive:true});fs.copyFileSync(p.join(old,'runner/Runner.csproj'),p.join(out,'runner/Runner.csproj'));
let frozenFiles={};walk(out,frozenFiles);
write(p.join(out,'freeze.json'),{frozenAt:new Date().toISOString(),head:cp.execFileSync('git',['rev-parse','HEAD'],{cwd:root,encoding:'utf8'}).trim(),model:'gpt-6-luna',maxCalls:18,retries:0,pairs:pairs.map(x=>({id:x.id,group:x.group})),files:frozenFiles});
console.log('Frozen: 6 maps + 12 judges, all reference anchors exact.');
