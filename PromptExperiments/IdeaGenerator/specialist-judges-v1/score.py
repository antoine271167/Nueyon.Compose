import pathlib,json,collections
R=pathlib.Path(__file__).resolve().parent
rows=[];blockers=[];usage=collections.defaultdict(lambda:collections.Counter())
for phase in ['tuning','holdout','repeat']:
 cases=json.loads((R/('tuning' if phase=='repeat' else phase)/'cases.json').read_text())
 for c in cases:
  decisions={}
  for cond in ['baseline','v2','source','editorial']:
   p=R/'runs'/phase/f'{cond}-{c["id"]}.verdict.json'
   if not p.exists():continue
   v=json.loads(p.read_text());decisions[cond]=v['accepted']
   expected=c['expectedAccepted']
   if cond=='source' and c.get('remit')=='editorial':expected=True
   if cond=='editorial' and c.get('remit')=='source':expected=True
   rows.append(dict(phase=phase,condition=cond,id=c['id'],expected=expected,accepted=v['accepted'],score='TN' if expected and v['accepted'] else 'FP' if expected else 'FN' if v['accepted'] else 'TP'))
   for i,b in enumerate(v['blockingIssues']):blockers.append(dict(key=f'{phase}/{cond}/{c["id"]}/{i}',**b))
   m=json.loads(p.with_name(p.name.replace('.verdict.json','.metadata.json')).read_text());u=m.get('usage') or {}
   usage[cond].update({k:v for k,v in u.items() if isinstance(v,int)})
  if 'source' in decisions and 'editorial' in decisions:
   a=decisions['source'] and decisions['editorial'];e=c['expectedAccepted']
   rows.append(dict(phase=phase,condition='split',id=c['id'],expected=e,accepted=a,score='TN' if e and a else 'FP' if e else 'FN' if a else 'TP'))
summary={}
for phase in ['tuning','holdout','repeat','unique']:
 for cond in ['baseline','v2','source','editorial','split']:
  sub=[r for r in rows if r['condition']==cond and (r['phase']!='repeat' if phase=='unique' else r['phase']==phase)]
  n=collections.Counter(r['score'] for r in sub)
  summary[phase+'/'+cond]={k:n[k] for k in ['TP','FP','TN','FN']}
  summary[phase+'/'+cond].update(recall=n['TP']/(n['TP']+n['FN']) if n['TP']+n['FN'] else None,precision=n['TP']/(n['TP']+n['FP']) if n['TP']+n['FP'] else None)
stability={}
for cond in ['baseline','v2','source','editorial','split']:
 repeats=[r for r in rows if r['phase']=='repeat' and r['condition']==cond];same=0
 for r in repeats:
  original=next(x for x in rows if x['phase']=='tuning' and x['condition']==cond and x['id']==r['id']);same+=r['accepted']==original['accepted']
 stability[cond]=dict(agree=same,n=len(repeats))
for name,obj in [('scores',rows),('summary',summary),('blockers',blockers),('usage',usage),('stability',stability)]:
 (R/f'verification/{name}.json').write_text(json.dumps(obj,indent=2),encoding='utf-8')
print(json.dumps(dict(summary=summary,usage=usage,stability=stability),indent=2))
