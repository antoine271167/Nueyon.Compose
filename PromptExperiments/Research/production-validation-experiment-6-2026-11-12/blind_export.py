import pathlib,json,secrets,random,hashlib
O=pathlib.Path(__file__).parent
def read(p):return json.loads((O/p).read_text(encoding='utf-8-sig'))
def save(p,v):
 p=O/p;p.parent.mkdir(parents=True,exist_ok=True);p.write_text(json.dumps(v,ensure_ascii=False,indent=2),encoding='utf-8')
if (O/'blind/key.json').exists():raise Exception('Already exported')
key={};packet_manifest={}
for case in ['005','harbor','archive']:
 for rep in [1,2]:
  outputs=[]
  for arm in ['A','B']:
   lane=f'{case}-r{rep}-{arm}';code=secrets.token_hex(5);key[code]=lane
   item={'code':code}
   for stage in ['Research','Synthesis','Narrative','Compose']:
    p=pathlib.Path('calls')/f'{lane}-{stage}'/'result.json'
    item[stage]=read(p)['content'] if (O/p).exists() else '[MISSING STAGE: FAILED]'
   pp=O/'calls'/f'{lane}-Research'/'projection.md';item['projection']=pp.read_text(encoding='utf-8-sig') if pp.exists() else '[MISSING]'
   outputs.append(item)
  random.SystemRandom().shuffle(outputs)
  inp=read(pathlib.Path('cases')/case/'input.json')
  packet={'originalSource':inp['Input']['Content'],'selectedIdea':inp['SelectedIdea']['Idea'],'rubric':(O/'rubric.md').read_text(encoding='utf-8').split('GO voor')[0],'labels':read('labels.json')[case],'outputs':outputs}
  pid=secrets.token_hex(6);save(pathlib.Path('blind/packets')/(pid+'.json'),packet)
  packet_manifest[pid]={'case':case,'repeat':rep,'codes':[x['code'] for x in outputs]}
save('blind/key.json',key);save('blind/packet-manifest.json',packet_manifest)
save('blind/freeze.json',{p.name:hashlib.sha256(p.read_bytes()).hexdigest() for p in (O/'blind/packets').glob('*.json')})
print('Exported six blinded evaluation packets; key separate')
