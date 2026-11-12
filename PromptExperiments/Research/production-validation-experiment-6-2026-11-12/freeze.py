import pathlib,json,hashlib,datetime
O=pathlib.Path(__file__).parent;R=O.parents[2]
if (O/'freeze.json').exists():raise Exception('Already frozen')
files={}
for p in O.rglob('*'):
 if p.is_file() and 'obj' not in p.parts and 'bin' not in p.parts:files[p.relative_to(R).as_posix()]=hashlib.sha256(p.read_bytes()).hexdigest()
for p in json.loads((O/'protected-before.json').read_text()):files[p]=hashlib.sha256((R/p).read_bytes()).hexdigest()
for p in (O/'runner/bin/Debug/net10.0').glob('*.dll'):files[p.relative_to(R).as_posix()]=hashlib.sha256(p.read_bytes()).hexdigest()
(O/'freeze.json').write_text(json.dumps({'frozenAt':datetime.datetime.now(datetime.timezone.utc).isoformat(),'maxCalls':54,'retries':0,'files':files},indent=2),encoding='utf-8')
print('Frozen',len(files),'files')
