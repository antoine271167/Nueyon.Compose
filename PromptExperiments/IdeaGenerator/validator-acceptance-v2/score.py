import pathlib,json,math,sys
R=pathlib.Path(__file__).resolve().parent
def interval(k,n):
 if not n:return None
 z=1.96; d=1+z*z/n; c=(k/n+z*z/(2*n))/d; w=z*math.sqrt((k/n*(1-k/n)+z*z/(4*n))/n)/d
 return [c-w,c+w]
rows=[]; diagnoses=[]
for phase in ['tuning','holdout','repeat']:
 cases=json.loads((R/('tuning' if phase=='repeat' else phase)/'cases.json').read_text())
 for cond in ['baseline','candidate1','candidate2']:
  for c in cases:
   p=R/'runs'/phase/f'{cond}-{c["id"]}.verdict.json'
   if not p.exists():continue
   try:v=json.loads(p.read_text()); accepted=v['accepted']
   except Exception:continue
   classification=('TN' if accepted else 'FP') if c['expectedAccepted'] else ('FN' if accepted else 'TP')
   rows.append(dict(phase=phase,condition=cond,id=c['id'],expectedAccepted=c['expectedAccepted'],accepted=accepted,classification=classification))
   for i,b in enumerate(v['blockingIssues']):diagnoses.append(dict(phase=phase,condition=cond,id=c['id'],index=i+1,**b))
stats=[]
for phase in ['tuning','holdout','repeat','unique']:
 for cond in ['baseline','candidate1','candidate2']:
  rs=[r for r in rows if r['condition']==cond and (r['phase'] in ['tuning','holdout'] if phase=='unique' else r['phase']==phase)]
  count={k:sum(r['classification']==k for r in rs) for k in ['TP','FP','TN','FN']}
  tp,fp,tn,fn=[count[k] for k in ['TP','FP','TN','FN']]
  stats.append(dict(phase=phase,condition=cond,n=len(rs),**count,precision=tp/(tp+fp) if tp+fp else None,recall=tp/(tp+fn) if tp+fn else None,recallWilson95=interval(tp,tp+fn)))
agreement=[]
for r in rows:
 if r['phase']=='repeat':
  first=next(x for x in rows if x['phase']=='tuning' and x['condition']==r['condition'] and x['id']==r['id'])
  agreement.append(dict(condition=r['condition'],id=r['id'],first=first['accepted'],repeat=r['accepted'],agrees=first['accepted']==r['accepted']))
metadata=[]
for p in (R/'runs').rglob('*.metadata.json'): metadata.append(json.loads(p.read_text()))
usage={k:sum((m.get('usage') or {}).get(k,0) or 0 for m in metadata) for k in ['inputTokenCount','cachedInputTokenCount','outputTokenCount','reasoningTokenCount','totalTokenCount']}
out=dict(stats=stats,rows=rows,agreement=agreement,models=sorted(set(m['modelId'] for m in metadata)),calls=len(metadata),usage=usage)
(R/'verification/scores.json').write_text(json.dumps(out,indent=2),encoding='utf-8')
(R/'verification/blockers.json').write_text(json.dumps(diagnoses,indent=2,ensure_ascii=False),encoding='utf-8')
print(json.dumps(dict(stats=stats,calls=len(metadata),usage=usage,disagreements=[a for a in agreement if not a['agrees']]),indent=2))
