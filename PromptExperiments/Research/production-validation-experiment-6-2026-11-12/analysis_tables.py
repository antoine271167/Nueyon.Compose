import pathlib,json,collections,statistics
O=pathlib.Path(__file__).parent
def read(p):return json.loads((O/p).read_text(encoding='utf-8-sig'))
a=read('assessments-unblinded.json');u=read('usage.json');lines=[]
lines+=['## Geblindeerde scores na ontsleuteling','', '| Cel | Research-pass | Bruikbare publicatie | Stijl /6 | Brontrouw eind /2 |','|---|---:|---:|---:|---:|']
for x in sorted(a,key=lambda x:x['lane']):lines.append(f"| {x['lane']} | {x['derivedResearchPass']} | {x['derivedUsable']} | {x['styleSum']} | {x['scores']['finalFidelity']} |")
lines+=['','## Stagekosten','', '| Arm / stage | Calls | Input | Output | Totaal |','|---|---:|---:|---:|---:|']
for arm in ['A','B','evaluation']:
 for stage in ['Research','Synthesis','Narrative','Compose','evaluation']:
  g=[v for v in u if v.get('arm','evaluation')==arm and v['stage']==stage]
  if g:lines.append(f"| {arm} / {stage} | {len(g)} | {sum(v['usage']['prompt_tokens'] for v in g):,} | {sum(v['usage']['completion_tokens'] for v in g):,} | {sum(v['usage']['total_tokens'] for v in g):,} |")
lines+=['','## Herhalingen en variantie','', '| Case / arm | Research pass /2 | Eind pass /2 | Ketentokens r1/r2 | Stijl r1/r2 |','|---|---:|---:|---|---|']
for case in ['005','harbor','archive']:
 for arm in ['A','B']:
  group=sorted([x for x in a if x['lane'].startswith(case+'-') and x['arm']==arm],key=lambda x:x['lane'])
  t=[sum(x['usage']['total_tokens'] for x in u if x.get('case')==case and x.get('repeat')==rep and x.get('arm')==arm) for rep in [1,2]]
  lines.append(f"| {case} / {arm} | {sum(x['derivedResearchPass'] for x in group)} | {sum(x['derivedUsable'] for x in group)} | {t[0]:,} / {t[1]:,} | {' / '.join(str(x['styleSum']) for x in group)} |")
lines+=['','## Scores per dimensie (gemiddelden, geen intervalschattingen)','', '| Dimensie | A | B |','|---|---:|---:|']
for metric in a[0]['scores']:
 lines.append(f"| {metric} | {statistics.mean(x['scores'][metric] for x in a if x['arm']=='A'):.2f} | {statistics.mean(x['scores'][metric] for x in a if x['arm']=='B'):.2f} |")
lines+=['','## Bronlabels per cel en stage','', 'P=present, ~=partial, –=absent, X=contradicted. Volgorde Research / projectie / Synthesis / Narrative / Compose.','']
status={'present':'P','partial':'~','absent':'–','contradicted':'X'}
for x in sorted(a,key=lambda x:x['lane']):
 lines+=['### '+x['lane'],'']
 for l in x['labels']:lines.append('- '+l['id']+': '+ ' / '.join(status.get(l[s],'?') for s in ['Research','projection','Synthesis','Narrative','Compose'])+' — '+l.get('reason',''))
 lines+=['']
(O/'analysis-tables.md').write_text('\n'.join(lines),encoding='utf-8')
print('\n'.join(lines[:65]))
