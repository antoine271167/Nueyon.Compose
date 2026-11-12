import pathlib,json,statistics,re,hashlib
O=pathlib.Path(__file__).parent
def read(p):return json.loads((O/p).read_text(encoding='utf-8-sig'))
def text(p):return (O/p).read_bytes().decode('utf-8-sig')
def save(p,x):(O/p).write_text(json.dumps(x,ensure_ascii=False,indent=2),encoding='utf-8')
a=read('assessments-unblinded.json');u=read('usage.json');labels=read('labels.json');result=read('results.json')
audit=[]
for x in a:
 case=x['lane'].split('-')[0]; ls={l['id']:l for l in x['labels']}
 available=all(ls[l[0]][s] in ['present','partial'] for l in labels[case] for s in l[2].split(',') if s in ['Research','projection'])
 audit.append({'lane':x['lane'],'rawResearchPass':x['researchPass'],'rawUsablePublication':x['usablePublication'],'strictDerivedResearchPass':x['derivedResearchPass'],'strictDerivedUsable':x['derivedUsable'],'researchProjectionLabelAvailabilityOnly':available,'styleSum':x['styleSum'],'scoreContractValid':x['contractValid']})
qs=[q for x in a for q in x['quoteVerification']]
placeholder=sum(1 for x in a for i in x['issues'] if i.get('outputQuote','').strip() in ['—','-',''])
metrics={'primary':result,'availabilityOnly':{arm:sum(x['researchProjectionLabelAvailabilityOnly'] for x in audit if x['lane'].endswith(arm)) for arm in ['A','B']},'quoteChecks':{'issues':len(qs),'literalSourceQuotes':sum(q['sourceExact'] for q in qs),'literalOutputQuotesIncludingPlaceholders':sum(q['outputExact'] for q in qs),'placeholderOutputQuotes':placeholder},'rawVsStrict':audit,'conservativeFinalSensitivity':{'A':0,'B':0,'reason':'A harbor-r1, the sole strict pass, omits fictional/synthetic status and the later audio repair; both are explicit source boundaries, and comparable omissions are treated as material elsewhere. Root audit is unblinded, not an independent replacement verdict.'},'notes':[
 'The evaluator sometimes labels Research a fail for omissions first arising in Synthesis. Availability-only is a location-specific diagnostic, not a post-hoc promotion criterion.',
 'The raw evaluator calls 005-r2 A and B usable, but gives both style sum 4; the pre-frozen threshold is 5. The strict derived score correctly excludes both.',
 'Harbor-r1 A and Harbor-r2 A both omit the later audio repair: evaluator treats the former as minor, the latter as material. A harbor-r1 also omits fictional status, while archive-r1 A is rejected for an equivalent omission.',
 'Archive rights label itself says 760 openbare scans, although source establishes completed rights clearance, not necessarily public downloadability. Frozen label remains unchanged; original source takes precedence. vrijgegeven may refer to rights release and is therefore ambiguous; exclude this diagnosis as sole decisive evidence.',
 'Archive productivity/log labels may be overly broad for the selected Idea. Treat their exclusion as a sensitivity; the keyboard-accessibility omission is central, repeated in all archive arms, and remains.',
 'No evaluator repairs/retries. Invalid literal quotes are not valid anchors. Manually verified examples are in critical-audit.md.']}
for arm in ['A','B']:
 g=[x for x in u if x.get('arm')==arm]
 vals=[sum(x['usage']['total_tokens'] for x in g if x['case']==case and x['repeat']==rep) for case in ['005','harbor','archive'] for rep in [1,2]]
 metrics.setdefault('descriptiveTokenVariation',{})[arm]={'perPipeline':vals,'mean':statistics.mean(vals),'sampleSdAcrossMixedCases':statistics.stdev(vals),'min':min(vals),'max':max(vals),'note':'Mixed-case SD is descriptive, not pure stochastic repeat variance.'}
counts={}
for x in u:
 for k,v in x['usage'].items():
  if isinstance(v,int):counts[k]=counts.get(k,0)+v
save('audit-summary.json',metrics);save('usage-top-level-totals.json',counts)
print(json.dumps({'availabilityOnly':metrics['availabilityOnly'],'quoteChecks':metrics['quoteChecks'],'totals':counts,'tokenSavings':1-result['B']['tokens']['total_tokens']/result['A']['tokens']['total_tokens'],'weightedSavings':1-(130079+4*30511)/(186423+4*33888),'researchSavings':1-26398/85592},indent=2))
