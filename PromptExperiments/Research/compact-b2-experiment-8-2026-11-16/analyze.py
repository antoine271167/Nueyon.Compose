import pathlib,json,re,hashlib,subprocess
r=pathlib.Path.cwd(); e=r/'PromptExperiments/Research/compact-b2-experiment-8-2026-11-16'
def load(p):return json.loads((e/p).read_text(encoding='utf-8-sig'))
def save(p,o):(e/p).write_text(json.dumps(o,ensure_ascii=False,indent=2),encoding='utf-8')
labels=load('labels.json');plan=load('plan.json')
patterns={
'water':dict(ledger=r'46|1,?400|1,?120|29',method=r'1\.8|1\.6|30 seconds|60 seconds|reprocess',conditions=r'flush',allocation=r'rainfall|random|access,|ease of access|chosen for access',contrast=r'calibrat|packet.delivery|17%',open=r'fault.*open|delivery.*open|antenna',chronology=r'batter|date link',term=r'brownout|voltage|device restart|power outage',attribution=r'capacitor|firmware|contributions|field uptime',variation=r'replay|tie.break|29, 31',decision=r'six.week|network.wide|unchanged thresholds',cost=r'18,?000|overtime|rental|savings'),
'language':dict(sample=r'80|640|40 Spanish|40 Polish|40 Spaans|40 Pools',contrast=r'glossary|entitlement|deadline|omkering|reversal|terminolog',review=r'second.*review|tweede.*review|overige strings|remaining strings',access=r'contrast|keyboard|toetsenbord|upload',open=r'open|screen.reader|schermlezer|label|activation|activering',outcome=r'33|42|moderated|gemodereerd',method=r'review screen|submitted application|controlescherm|ingediende aanvraag',allocation=r'recruit|random|matched|wervings|willekeurig|gematcht',term=r'folding|letter case|letters|merge|samen',variation=r'six, five|six, five,|zes, vijf|checker|human reference|menselijke referentie',legal=r'consent|legal approval|goedkeuring|toestemming',decision=r'coordinator|coördinator|usability|general release|algemene uitrol'),
'heat':dict(ledger=r'410|500',method=r'76%|lower.heating|pump|boundary reanalysis',conditions=r'lighter|8 degrees',allocation=r'peak production|random|matched',contrast=r'pressure.hold|surface.temperature|hatch',open=r'unresolved|shield|not installed',interlock=r'interlock',term=r'thermal trip|over.temperature|shutdown',attribution=r'controller|sensor.mount|contributions|zero trips|zero.trip',variation=r'402|418|407|stochastic|simulation',cost=r'9,?200|rental allowance|installation|downtime|maintenance|payback',decision=r'Procurement|purchase|roll.out|longer instrumented')}
# Semantic decisions entered by Work after reading all 27 full outputs and their sources.
# Regex only selects verbatim evidence rows; it does not decide semantic correctness.
defects=[
 dict(run='water-r1-B',section='Development sequence',needle='The committee then authorized',category='chronologie',reason='then koppelt het besluit aan de voorafgaande benchgebeurtenis zonder expliciete bronvolgorde.',quote='The committee authorized a six-week paired measurement study with unchanged thresholds. It did not authorize network-wide deployment.'),
 dict(run='water-r2-B',section='Development sequence',needle='After reviewing the pilot',category='chronologie',reason='Voegt expliciet reviewmoment vóór commissieautorisatie toe; het dossier vermeldt dat moment niet. Kleine, betwistbare procesinferentie; geen kernfout.',quote='The committee authorized a six-week paired measurement study with unchanged thresholds. It did not authorize network-wide deployment.'),
 dict(run='water-r3-B',section='Development sequence',needle='The recorded legacy fortnight preceded',category='chronologie',reason='Expliciete kalenderorde staat niet in de bron. legacy suggereert eerder, maar bewijst de gerapporteerde periodevolgorde niet. Kleine betwistbare inferentie; ook After review voegt volgorde toe.',quote='The legacy fortnight recorded 46 low-pressure alerts in 1,400 logger-hours; the pilot fortnight recorded 29 in 1,120 logger-hours.'),
 dict(run='heat-r2-B',section='Development sequence',needle='Procurement subsequently approved',category='chronologie',reason='subsequently plaatst het besluit na de herhaaltest; bron geeft geen relatieve datering van besluit tegenover tests.',quote='Procurement approved only a longer instrumented trial conditional on a verified hatch remedy; no purchase or factory-wide roll-out was approved.'),
 dict(run='heat-r3-B',section='Development sequence',needle='Procurement subsequently approved',category='chronologie',reason='subsequently koppelt besluit aan sign-off, zonder expliciete relatieve datering in bron.',quote='Procurement approved only a longer instrumented trial conditional on a verified hatch remedy; no purchase or factory-wide roll-out was approved.'),
 dict(run='water-r1-B2',section='Development sequence',needle='The committee then authorized',category='chronologie',reason='then maakt commissieautorisatie later dan reviewstatus; bron noemt beide zonder expliciete volgorde.',quote='The committee authorized a six-week paired measurement study with unchanged thresholds. It did not authorize network-wide deployment.'),
 dict(run='language-r2-B2',section='Development sequence',needle='a fix proposal as approved but not deployed',category='epistemische precisie',reason='not deployed is sterker dan no deployed fix is recorded. Facts 5 en Unknowns bewaren de bronbeperking wel correct.',quote='Management approved a keyboard fix proposal; no deployed fix is recorded.')]
for d in defects:
 content=(e/f"calls/{d['run']}-Research/content.md").read_text();lines=content.splitlines();hits=[(i+1,l) for i,l in enumerate(lines) if d['needle'] in l];assert len(hits)==1,d
 d['outputLine'],d['outputQuote']=hits[0];d['materiality']='klein';d['inProjection']=d['needle'] in (e/f"calls/{d['run']}-Research/projection.md").read_text();assert not d['inProjection'];assert d['quote'] in (e/f"cases/{d['run'].split('-')[0]}/source.md").read_text()
# Known important omission, not a source-fidelity fabrication.
defects.append(dict(run='heat-r3-A',section='Facts/Unknowns',category='omissie',reason='Het volledige belangrijke kostenlabel ontbreekt in volledige Research én projectie: EUR 9.200, geen datering, installatie/downtime/onderhoud uitgesloten, geen paybackclaim.',quote=labels['heat'][10]['sourceQuote'],outputQuote=None,outputLine=None,materiality='belangrijk',inProjection=False))
save('precision-audit.json',defects)
rows=[];ass=[]
for lane in plan['schedule']:
 id=lane['id'];case=lane['case'];d=e/f'calls/{id}-Research';raw=(d/'content.md').read_text();proj=(d/'projection.md').read_text();source=(e/f'cases/{case}/source.md').read_text()
 for lab in labels[case]:
  status='ontbreekt' if id=='heat-r3-A' and lab['id']=='cost' else 'correct'
  pat=patterns[case][lab['id']]
  def hits(text):return [dict(line=i+1,quote=l) for i,l in enumerate(text.splitlines()) if re.search(pat,l,re.I)]
  rh=hits(raw);ph=hits(proj)
  if status=='ontbreekt':assert not rh and not ph
  else:assert rh and ph,(id,lab['id'])
  # For traceability, retain complete matching lines, not keyword counts as quality scores.
  rows.append(dict(run=id,case=case,label=lab['id'],materiality=lab['materiality'],sourceQuote=lab['sourceQuote'],sourceLine=source[:source.index(lab['sourceQuote'])].count('\n')+1,status=status,rawEvidence=rh,projectionEvidence=ph,note='Semantisch beoordeeld door Work; ankers zijn vindhulp, geen automatische kwaliteitsmeting.'))
 scores=dict(brontrouw=2,dekking=2,nuances_contrasten=2,attributie=2,epistemische_precisie=2,consistentie=2,bruikbaarheid_projectie=2)
 ds=[x for x in defects if x['run']==id]
 if ds:
  if id=='heat-r3-A':scores.update(dekking=1,nuances_contrasten=1,bruikbaarheid_projectie=1)
  else:scores.update(brontrouw=1,epistemische_precisie=1)
 ass.append(dict(id=id,case=case,arm=lane['arm'],scores=scores,sufficient=all(x==2 for x in scores.values()),coreOnly=True,reason='; '.join(x['reason'] for x in ds) if ds else 'Alle materiële labels en hun grenzen behouden; geen concrete ongefundeerde toevoeging aangetroffen.'))
save('traceability.json',rows);save('assessments.json',ass)
md=['# Bron-naar-Research-matrix Experiment 8','', '36 vooraf bevroren labels × 9 outputs per bron = 324 beoordelingen. De Work-assistent heeft alle volledige Research-outputs en bronnen gelezen; semantische classificaties zijn handmatige AI-beoordelingen, geen onafhankelijke menselijke labels. Regex selecteert alleen vindbare citaten en beslist geen kwaliteit. Alle geciteerde regels zijn exact uit de artefacten; afwezigheid heeft geen verzonnen outputcitaat. Volledige ankers ook in traceability.json.','']
for row in rows:
 id=row['run'];c=row['case'];md += [f"## {id} / {row['label']} — {row['materiality']} — {row['status']}",'',f"Bron [L{row['sourceLine']}](cases/{c}/source.md):",'', '> '+row['sourceQuote'],'']
 for field,file in [('rawEvidence','content.md'),('projectionEvidence','projection.md')]:
  md += [f'[{file}](calls/{id}-Research/{file})','']
  if not row[field]:md+=['Ontbreekt; geen equivalent bewijs gevonden in de volledige secties.','']
  else:
   for h in row[field]:md += [f"- L{h['line']}: {h['quote']}"]
   md+=['']
(e/'traceability.md').write_text('\n'.join(md),encoding='utf-8')
usage=load('usage.json');summ=[]
for arm in ['A','B','B2']:
 uu=[x for x in usage if x['arm']==arm];aa=[x for x in ass if x['arm']==arm];I=sum(x['usage']['prompt_tokens'] for x in uu);O=sum(x['usage']['completion_tokens'] for x in uu);n=sum(x['sufficient'] for x in aa)
 summ.append(dict(arm=arm,calls=len(uu),sufficient=n,conditional=9-n,coreOnly=sum(x['coreOnly'] for x in aa),input=I,output=O,total=I+O,weighted=I+4*O,tokensPerSufficient=(I+O)/n if n else None,weightedPerSufficient=(I+4*O)/n if n else None,cached=sum(x['usage']['prompt_tokens_details']['cached_tokens'] for x in uu),cacheWrite=sum(x['usage']['prompt_tokens_details'].get('cache_write_tokens',0) for x in uu),reasoning=sum(x['usage']['completion_tokens_details']['reasoning_tokens'] for x in uu)))
save('results.json',summ)
print(json.dumps(summ,indent=2));print('matrix',len(rows),'correct',sum(x['status']=='correct' for x in rows),'missing',sum(x['status']=='ontbreekt' for x in rows))
