import pathlib,json,hashlib,subprocess,datetime,sys,concurrent.futures,threading,random
R=pathlib.Path(__file__).resolve().parent; repo=R.parents[2]
phase=sys.argv[1];lock=threading.Lock();ledger=R/'verification/call-ledger.jsonl'
def now(): return datetime.datetime.now(datetime.timezone.utc).isoformat()
def log(obj):
 with ledger.open('a',encoding='utf-8') as f:f.write(json.dumps(dict(utc=now(),**obj))+'\n')
for p,h in json.loads((R/'verification/frozen.json').read_text()).items():assert hashlib.sha256((R/p).read_bytes()).hexdigest()==h,p
assert subprocess.check_output(['git','rev-parse','HEAD'],cwd=repo,text=True).strip()=='f894d2f161e034aa514a7ce138eb2ba278b3cffe'
if phase!='offline':assert (R/'verification/offline.ok').exists()
jobs=[]
for split in (['tuning','holdout'] if phase=='offline' else ['tuning'] if phase=='repeat' else [phase]):
 for c in json.loads((R/split/'cases.json').read_text()):
  if phase=='repeat' and c['id'] not in ['005-b','010-b','v4-005-original','v4-010-original']:continue
  for condition in ['baseline','v2','source','editorial']:jobs.append((split,c['id'],condition))
random.Random(20261111).shuffle(jobs)
def run(job):
 split,id,cond=job;key=f'{phase}/{cond}/{id}';prefix=R/'runs'/phase/f'{cond}-{id}';prefix.parent.mkdir(parents=True,exist_ok=True)
 args=['dotnet',str(R/'harness/bin/Debug/net10.0/ValidatorAcceptance.dll'),'offline' if phase=='offline' else 'live',str(R/'prompts'/cond),str(R/split/'cases.json'),id,str(prefix)]
 with lock:
  if phase!='offline':
   entries=[json.loads(s) for s in ledger.read_text().splitlines()] if ledger.exists() else []
   reserved=[e for e in entries if e['event']=='reserved']
   if any(e['key']==key for e in reserved):return 0
   assert len(reserved)<112,'judge budget exhausted'
   log(dict(event='reserved',key=key,args=args,alias='gpt-5-mini'))
 started=now()
 with prefix.with_suffix('.log').open('w',encoding='utf-8') as f:
  try:result=subprocess.run(args,cwd=repo,stdout=f,stderr=subprocess.STDOUT,timeout=115);code=result.returncode
  except subprocess.TimeoutExpired:code=124
 with lock:
  if phase!='offline':log(dict(event='completed',key=key,exit=code,started=started,responseSaved=pathlib.Path(str(prefix)+'.verdict.json').exists()))
  print(key,code,flush=True)
 return code
with concurrent.futures.ThreadPoolExecutor(max_workers=3) as pool:codes=list(pool.map(run,jobs))
if phase=='offline' and all(c==0 for c in codes):(R/'verification/offline.ok').write_text('96 combinations x 8 contract probes passed\n')
(R/'verification/state.json').write_text(json.dumps(dict(phase=phase,completed=all(c==0 for c in codes),maxPaidCalls=176,judgeCap=112,productionCap=64)))
sys.exit(0 if all(c==0 for c in codes) else 1)
