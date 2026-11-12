import pathlib,json,hashlib,re,collections,datetime
O=pathlib.Path(__file__).parent;R=O.parents[2]
def txt(p):return p.read_bytes().decode('utf-8-sig')
def read(p):return json.loads(txt(p))
def low(v):return {k.lower():low(x) for k,x in v.items()} if isinstance(v,dict) else ([low(x) for x in v] if isinstance(v,list) else v)
def save(p,v):(O/p).write_text(json.dumps(v,ensure_ascii=False,indent=2),encoding='utf-8')
def sha(p):return hashlib.sha256(p.read_bytes()).hexdigest()
freeze=read(O/'freeze.json');protected=read(O/'protected-before.json')
save('verification-integrity.json',{'freezeFiles':len(freeze['files']),'freezeMismatches':[p for p,h in freeze['files'].items() if not (R/p).exists() or sha(R/p)!=h],'protectedFiles':len(protected),'protectedMismatches':[p for p,h in protected.items() if not (R/p).exists() or sha(R/p)!=h]})
requestchecks=[];usage=[];totals=collections.defaultdict(lambda:collections.Counter())
for d in sorted((O/'calls').iterdir()):
 if not d.is_dir() or not (d/'request.json').exists():continue
 mark=read(d/'attempt-started.json');req=read(d/'request.json');raw=read(d/'response-raw.json') if (d/'response-raw.json').exists() else {};u=raw.get('usage',{})
 stage=mark['stage'];item={'id':d.name,'stage':stage,'usage':u,'status':read(d/'http-metadata.json') if (d/'http-metadata.json').exists() else None,'model':raw.get('model'),'finish':raw.get('choices',[{}])[0].get('finish_reason')}
 if stage!='evaluation':
  lane=mark['id'];arm=lane[-1];case=lane.split('-')[0];repeat=int(lane.split('-')[1][1:]);item.update(arm=arm,case=case,repeat=repeat)
 else:arm='evaluation'
 usage.append(item)
 for k in ['prompt_tokens','completion_tokens','total_tokens']:totals[arm][k]+=u.get(k,0)
 totals[arm]['calls']+=1
 msgs=req['messages'];sys=msgs[0]['content'];usr=msgs[-1]['content'];check={'id':d.name,'model':req.get('model')=='gpt-6-luna','maxOutput':req.get('max_completion_tokens',req.get('max_tokens'))==6000,'requestBytes':(d/'request.json').stat().st_size,'singleHttp':item['status'] and item['status']['httpRequests']==1,'responseModel':item['model']=='gpt-6-luna','finishStop':item['finish']=='stop'}
 if stage=='evaluation':
  check['responseSchema']=req.get('response_format',{}).get('type')=='json_object'
  packet=read(O/'blind/packets'/(mark['id']+'.json'))
  check['systemMatch']=sys==txt(O/'evaluation-system.md')
  check['packetMatch']=json.loads(usr)==packet
  check['noIdentityFields']=all(set(x)=={'code','Research','Synthesis','Narrative','Compose','projection'} for x in packet['outputs'])
 else:
  rf=req.get('response_format',{});schema=rf.get('json_schema',{})
  check['responseSchema']=rf.get('type')=='json_schema' and schema.get('name')==stage+'Result' and schema.get('schema',{}).get('properties')=={'content':{'type':'string'}} and schema.get('schema',{}).get('required')==['content'] and schema.get('schema',{}).get('additionalProperties') is False
  inp=read(d/'input.json');base=read(O/'cases'/case/'input.json');idea=base['SelectedIdea']['Idea']
  check['systemMatch']=sys==txt(d/'system.md')
  check['frozenSystemMatch']=sys==txt(O/'prompts'/arm/stage/(stage+'.system.prompt.md'))
  promptname={'Research':'Research/Research.user.prompt.md','Synthesis':'Synthesis/Synthesis.user.prompt.md','Narrative':'Narrative/Narrative.user.prompt.md','Compose':'Compose/Compose.linkedin.user.prompt.md'}[stage]
  values={'Idea'+k:v for k,v in idea.items()}
  if stage=='Research':values['SourceContent']=base['Input']['Content']
  if stage=='Synthesis':values['ResearchContent']=inp['research']['content']
  if stage=='Narrative':values['SynthesisContent']=inp['synthesis']['content']
  if stage=='Compose':values={'NarrativeContent':inp['narrative']['content'],'Format':'LinkedInPost'}
  template=txt(O/'prompts'/arm/promptname)
  tokens=set(re.findall(r'\{\{([A-Za-z][A-Za-z0-9]*)\}\}',template));check['renderTokens']=sorted(tokens)
  # Research source token name is discovered from the unchanged owning template.
  if stage=='Research':
   for t in tokens-set(values):values[t]=base['Input']['Content']
  rendered=re.sub(r'\{\{([A-Za-z][A-Za-z0-9]*)\}\}',lambda m:values[m[1]],template)
  check['productionRenderMatch']=usr==rendered
  check['sourceIdeaMatch']=low(inp)==low(base) if stage=='Research' else (low(inp['selectedIdea'])==low(idea) if stage in ['Synthesis','Narrative'] else True)
  if stage=='Synthesis':check['handoffMatch']=inp['research']['content']==txt(O/'calls'/f'{lane}-Research'/'projection.md')
  if stage=='Narrative':check['handoffMatch']=inp['synthesis']['content']==read(O/'calls'/f'{lane}-Synthesis'/'result.json')['content']
  if stage=='Compose':check['handoffMatch']=inp['narrative']['content']==read(O/'calls'/f'{lane}-Narrative'/'result.json')['content'];check['linkedInFormat']=inp['composition']['format']==1
 requestchecks.append(check)
save('usage.json',usage);save('cost-summary.json',dict(totals));save('verification-requests.json',requestchecks)
if (O/'blind/key.json').exists():
 key=read(O/'blind/key.json');assessments=[];evalvalid=[]
 for d in sorted((O/'calls').glob('*-evaluation')):
  if not (d/'evaluation.json').exists():continue
  ev=read(d/'evaluation.json');packet=read(O/'blind/packets'/(d.name[:-11]+'.json'))
  codes=[x['code'] for x in packet['outputs']];valid=set(codes)==set(x['code'] for x in ev['outputs'])
  for out in ev['outputs']:
   lane=key[out['code']];out['lane']=lane;out['arm']=lane[-1]
   labels={x['id']:x for x in out['labels']};stages=['Research','projection','Synthesis','Narrative','Compose']
   statuses={'present','partial','absent','contradicted'}
   labelvalid=set(labels)=={x[0] for x in packet['labels']} and all(l.get(s) in statuses for l in labels.values() for s in stages)
   scorevalid=set(out['scores'])=={'sourceFidelity','essentialCoverage','epistemicConsistency','projectionIntegrity','downstreamRetention','hook','readability','engagement','finalFidelity'} and all(type(v)==int and 0<=v<=2 for v in out['scores'].values())
   quotes=[]
   neutral=next(x for x in packet['outputs'] if x['code']==out['code'])
   for issue in out['issues']:
    sq=issue.get('sourceQuote','');oq=issue.get('outputQuote','');st=issue.get('stage','')
    quotes.append({'stage':st,'sourceExact':bool(sq) and sq in packet['originalSource'],'outputExact':bool(oq) and (oq in neutral.get(st,'') or any(oq in neutral[s] for s in stages))})
   scores=out['scores'];requiredResearch=all(labels.get(l[0],{}).get(s) in ['present','partial'] for l in packet['labels'] for s in l[2].split(',') if s in ['Research','projection'])
   requiredFinal=all(labels.get(l[0],{}).get(s) in ['present','partial'] for l in packet['labels'] for s in l[2].split(',') if s=='Compose')
   materialResearch=any(i['severity']=='material' and i['stage'] in ['Research','projection'] for i in out['issues'])
   materialFinal=any(i['severity']=='material' and i['stage']=='Compose' for i in out['issues'])
   style=sum(scores[s] for s in ['hook','readability','engagement'])
   research=valid and labelvalid and scorevalid and out['researchPass'] and requiredResearch and not materialResearch and all(scores[s]>=1 for s in ['sourceFidelity','essentialCoverage','epistemicConsistency','projectionIntegrity'])
   usable=valid and labelvalid and scorevalid and out['usablePublication'] and requiredFinal and scores['finalFidelity']==2 and style>=5 and all(scores[s]>=1 for s in ['hook','readability','engagement']) and not materialFinal
   out.update(contractValid=valid and labelvalid and scorevalid,quoteVerification=quotes,derivedResearchPass=research,derivedUsable=usable,styleSum=style)
   assessments.append(out)
  evalvalid.append({'packet':d.name,'validCodes':valid})
 save('assessments-unblinded.json',assessments);save('verification-evaluations.json',evalvalid)
 result={}
 for arm in ['A','B']:
  group=[x for x in assessments if x['arm']==arm];n=sum(bool(x['derivedUsable']) for x in group);t=totals[arm];result[arm]={'researchPass':sum(bool(x['derivedResearchPass']) for x in group),'usable':n,'assessed':len(group),'meanStyle':sum(x['styleSum'] for x in group)/len(group) if group else None,'tokens':dict(t),'tokensPerUsable':t['total_tokens']/n if n else None,'weightedPerUsable':(t['prompt_tokens']+4*t['completion_tokens'])/n if n else None,'note':'null cost per usable means zero successes, undefined/infinite'}
 save('results.json',result)
 print(json.dumps(result,indent=2))
print('Requests',len(requestchecks),'false checks',[(x['id'],k) for x in requestchecks for k,v in x.items() if v is False])
