import pathlib,json,hashlib,subprocess,datetime
R=pathlib.Path(__file__).resolve().parent; repo=R.parents[2]
def sha(p):return hashlib.sha256(p.read_bytes()).hexdigest()
frozen=json.loads((R/'verification/frozen.json').read_text())
changed=[p for p,h in frozen.items() if sha(R/p)!=h]
before=json.loads((R/'verification/production-before.json').read_text())
productionChanged=[p for p,h in before.items() if sha(repo/p)!=h]
entries=[json.loads(s) for s in (R/'verification/call-ledger.jsonl').read_text().splitlines()]
reserved=[e for e in entries if e['event']=='reserved']; complete=[e for e in entries if e['event']=='completed']
assert len(reserved)==len(set(e['key'] for e in reserved))==108
assert len(complete)==108 and all(e['exit']==0 and e['responseSaved'] for e in complete)
assert not changed and not productionChanged
assert subprocess.check_output(['git','diff','--name-only'],cwd=repo,text=True)==''
assert subprocess.check_output(['git','diff','--cached','--name-only'],cwd=repo,text=True)==''
selection=json.loads((R/'verification/selection-hash.json').read_text(encoding='utf-8-sig'))
assert sha(R/'selection.md').upper()==selection['Hash']
firstHoldout=min(e['utc'] for e in reserved if e['key'].startswith('holdout/'))
lastTuning=max(e['utc'] for e in complete if e['key'].startswith('tuning/'))
selectionTime=datetime.datetime.fromtimestamp((R/'selection.md').stat().st_mtime,datetime.timezone.utc).isoformat()
assert lastTuning<selectionTime<firstHoldout
counts={}
for e in reserved: counts[e['key'].split('/')[0]]=counts.get(e['key'].split('/')[0],0)+1
data=dict(utc=datetime.datetime.now(datetime.timezone.utc).isoformat(),frozenCount=len(frozen),frozenChanged=changed,productionFiles=len(before),productionChanged=productionChanged,reserved=len(reserved),completed=len(complete),phaseCounts=counts,lastTuning=lastTuning,selectionTime=selectionTime,firstHoldout=firstHoldout,trackedDiffEmpty=True,stagedDiffEmpty=True,head=subprocess.check_output(['git','rev-parse','HEAD'],cwd=repo,text=True).strip(),status=subprocess.check_output(['git','status','--short'],cwd=repo,text=True))
(R/'verification/final-integrity.json').write_text(json.dumps(data,indent=2),encoding='utf-8')
manifest={str(p.relative_to(R)):sha(p) for p in R.rglob('*') if p.is_file() and not any(x in p.parts for x in ['bin','obj']) and p.name!='final-manifest.json'}
(R/'verification/final-manifest.json').write_text(json.dumps(manifest,indent=2),encoding='utf-8')
print(json.dumps(data,indent=2))
