import pathlib,json,hashlib,collections,math,subprocess
R=pathlib.Path(__file__).resolve().parent;repo=R.parents[2]
frozen=json.loads((R/'verification/frozen.json').read_text())
src=json.loads((R/'verification/src-hashes.json').read_text())
ledger=[json.loads(s) for s in (R/'verification/call-ledger.jsonl').read_text().splitlines()]
reserved=[e for e in ledger if e['event']=='reserved'];done=[e for e in ledger if e['event']=='completed']
scores=json.loads((R/'verification/scores.json').read_text())
family={}
for comparator in ['baseline','v2']:
 groups=collections.defaultdict(list)
 for s in scores:
  if s['condition']!='split' or s['phase']=='repeat':continue
  b=next(x for x in scores if x['phase']==s['phase'] and x['condition']==comparator and x['id']==s['id'])
  id=s['id'];fam='005' if '005' in id else '010' if '010' in id else id.rsplit('-',1)[0]
  groups[fam].append((s['accepted']==s['expected'],b['accepted']==b['expected']))
 wins=losses=0;details={}
 for fam,pairs in groups.items():
  advantage=sum(int(a)-int(b) for a,b in pairs);wins+=advantage>0;losses+=advantage<0
  details[fam]=dict(netCorrectAdvantage=advantage)
 n=wins+losses
 family[comparator]=dict(wins=wins,losses=losses,ties=len(groups)-n,oneSidedSignP=sum(math.comb(n,k) for k in range(wins,n+1))/2**n if n else 1,details=details)
models=collections.Counter();usage=collections.Counter()
for p in (R/'runs').rglob('*.metadata.json'):
 m=json.loads(p.read_text());models[m['modelId']]+=1
 usage.update({k:v for k,v in (m.get('usage') or {}).items() if isinstance(v,int)})
audit=dict(head=subprocess.check_output(['git','rev-parse','HEAD'],cwd=repo,text=True).strip(),reserved=len(reserved),completed=len(done),failures=[e for e in done if e['exit']!=0],duplicateSlots=len(reserved)-len({e['key'] for e in reserved}),remainingTotal=176-len(reserved),remainingJudge=112-len(reserved),conditionalProductionUnused=64,models=models,usage=usage,familyComparisons=family,frozenChanged=[p for p,h in frozen.items() if hashlib.sha256((R/p).read_bytes()).hexdigest()!=h],srcChanged=[p for p,h in src.items() if hashlib.sha256((repo/p).read_bytes()).hexdigest()!=h],trackedDiff=subprocess.check_output(['git','diff','--stat','HEAD'],cwd=repo,text=True))
(R/'verification/final-integrity.json').write_text(json.dumps(audit,indent=2),encoding='utf-8')
print(json.dumps(audit,indent=2))
