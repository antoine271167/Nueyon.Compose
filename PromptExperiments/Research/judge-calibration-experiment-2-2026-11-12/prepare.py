import pathlib, json, hashlib, shutil, subprocess
root=pathlib.Path(__file__).resolve().parents[3]
out=pathlib.Path(__file__).resolve().parent
old=root/'PromptExperiments/Research/compact-judge-2026-11-12'
base=root/'PromptExperiments/Research/luna-baseline-2026-11-12'
def write(p,x):
 p.parent.mkdir(parents=True,exist_ok=True); p.write_text(json.dumps(x,ensure_ascii=False,indent=2),encoding='utf-8')
cases=[]
def add(id,group,inp,res,expect,target):
 d=out/'cases'/id
 write(d/'input.json',inp); write(d/'research-result.json',res)
 cases.append(dict(id=id,group=group,expected=expect,target=target))
for condition in ['baseline','control']:
 for id in ['intro','005','010']:
  inp=json.loads((base/id/'research-input.json').read_text(encoding='utf-8-sig'))
  res=json.loads((old/(condition+'-judge')/id/'research-result.json').read_text(encoding='utf-8-sig'))
  add(condition+'-'+id,'tuning',inp,res, 'reject' if condition=='baseline' else ('qualified' if id=='005' else 'accept'), {'intro':'Unknown 4 denies explicit concrete expression; distinguish reader effect','005':'missing upstream Idea/Research variation' if condition=='baseline' else 'upstream variation repaired; Synthesis tension still omitted','010':'missing Compose factual counterexample with preserved hierarchy'}[id])
inp=json.loads((base/'005/research-input.json').read_text(encoding='utf-8-sig'))
res=json.loads((old/'control-judge/005/research-result.json').read_text(encoding='utf-8-sig'))
k=next(iter(res))
res[k]=res[k].replace('### Interpretations','- The source notes internal tension in baseline Synthesis: top-level company identity and Compose coexist with a later description calling orchestration central to the product direction. It may have reinforced the technical angle; this is not established causation.\n\n### Interpretations').replace('The source does not state an explicit general lesson beyond the reported experiment result and decision.','The source concludes that the candidate was insufficient in this test; its broader effectiveness remains unestablished.')
add('control-005-complete','tuning',inp,res,'accept','all known material causal limits and Synthesis tension retained')
def hold(id,source,title,facts,unknowns,expect,target,discard='None.'):
 inp={'Input':{'Content':source},'SelectedIdea':{'Idea':{'Title':title,'Description':title,'Audience':'Reviewers of this report','Rationale':'Preserve the reported result and its evidence limits.','Evidence':'Verify against the source.'}}}
 content=f'### Facts\n\n{facts}\n\n### Interpretations\n\n{discard}\n\n### Unknowns\n\n{unknowns}\n\n### Development sequence\n\nNone.\n\n### Editorial relevance\n\nNone.'
 add(id,'holdout',inp,{'Content':content},expect,target)
s='The museum calls the Lantern Room its first public expression of a wider education plan. The room opened for a two-week trial. No visitor-learning outcomes were measured. The remaining education plan is a proposal, not an operating program.'
hold('holdout-museum-correct',s,'The trial room gives the proposed education plan a public expression, without demonstrated learning benefits.','- The museum describes the Lantern Room as the first public expression of its wider education plan.\n- The room opened for a two-week trial.\n- The rest of the plan remains proposed.','- Visitor-learning outcomes were not measured; benefits remain unestablished.','accept','attributed meaning is valid; outcome uncertainty is precise')
hold('holdout-museum-denial',s,'The trial room gives the proposed education plan a public expression, without demonstrated learning benefits.','- The museum describes the Lantern Room as the first public expression of its wider education plan.\n- The room opened for a two-week trial.\n- The rest of the plan remains proposed.','- Whether the trial room expresses the wider education plan is not established by the source.\n- Visitor-learning outcomes were not measured.','reject','Unknown denies explicitly attributed source relationship')
s='A warehouse compared two packing layouts. Layout P had 12 damaged parcels in 200 shipments; layout Q had 7 in 200. The two weeks used different parcel-size mixes. The report does not isolate the layout effect. During a separate check with the same layout, damaged-parcel counts also differed across parcel-size mixes. Staff training hours were not recorded.'
hold('holdout-warehouse-confounder',s,'The lower recorded damage count under layout Q does not establish a causal layout benefit.','- Layout P had 12 damaged parcels in 200 shipments; layout Q had 7 in 200.','- The report does not isolate the layout effect.\n- Staff training hours were not recorded.','reject','specific parcel-size confounder and supporting same-layout comparison absent downstream', '- The two weeks had different parcel-size mixes; the same-layout check also varied across mixes.')
s='The archive audit evaluated transcription accuracy and document ordering separately. Batch Cedar had correct ordering but contained two mistranscribed dates. Batch Birch had accurate dates but incorrect ordering. Batch Elm passed both checks. The audit did not identify the cause of either error type. These three batches do not establish an overall error rate.'
f='- Cedar had correct ordering and two mistranscribed dates.\n- Birch had accurate dates and incorrect ordering.\n- Elm passed both checks.'
u='- The cause of either error type is not identified.\n- Three batches do not establish an overall error rate.'
hold('holdout-archive-correct',s,'Ordering and transcription failures are distinct in the reviewed archive batches.',f,u,'accept','both discriminating counterexamples and limits retained')
hold('holdout-archive-omission',s,'Ordering and transcription failures are distinct in the reviewed archive batches.','- Birch had accurate dates and incorrect ordering.\n- Elm passed both checks.',u,'reject','Cedar transcription error despite correct order missing; generic distinct-dimensions claim insufficient')
write(out/'case-manifest.json',cases)
protected=[p for p in (root/'src').rglob('*') if p.is_file() and 'bin' not in p.parts and 'obj' not in p.parts]
write(out/'before-hashes.json',{str(p.relative_to(root)):hashlib.sha256(p.read_bytes()).hexdigest() for p in protected})
write(out/'freeze.json',{'head':subprocess.check_output(['git','rev-parse','HEAD'],cwd=root,text=True).strip(),'maxCalls':12,'retries':0,'promptSha256':hashlib.sha256((out/'prompts/Research/ResearchValidation.system.prompt.md').read_bytes()).hexdigest(),'cases':cases})
(out/'runner').mkdir(exist_ok=True)
shutil.copyfile(old/'runner/Runner.csproj',out/'runner/Runner.csproj')
