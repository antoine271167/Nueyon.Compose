const fs=require('node:fs');
process.chdir(__dirname);
const read=p=>fs.readFileSync(p,'utf8').replace(/^\uFEFF/,'');
const load=p=>JSON.parse(read(p));
const get=p=>fs.existsSync(p)?load(p):null;
const raw=p=>{try{return JSON.parse(read(p).trim().replace(/^[\x60]{3}(?:json)?\s*([\s\S]*?)\s*[\x60]{3}$/i,'$1'));}catch{return null;}};
const truth=load('ground-truth.json');
for(const t of load('dataset.json')){
 const id=t.id;
 const status=get('judge-runs/'+id+'/outcome.json')?.status;
 if(!status)continue;
 const old=get('judge-runs/'+id+'/experiment20/result.json');
 const v=get('judge-runs/'+id+'/result.json');
 const e=get('claim-extractions/'+id+'/result.json')??raw('claim-extractions/'+id+'/stdout.log');
 const a=get('evidence-alignments/'+id+'/result.json')??raw('evidence-alignments/'+id+'/stdout.log');
 const n=load(t.initial);
 const content=n.content??n.Content;
 const f=truth[id].findings[0];
 const source=read(t.source);
 console.log('\n'+id,t.subject,truth[id].label,status,old?.verdict??'--');
 if(!v){
  for(const c of e?.claims??[])if(!content.includes(c.narrativeExcerpt))console.log('Bad Narrative:',c.claimId,c.narrativeExcerpt);
  for(const c of a?.assessments??[])if(c.sourceExcerpt&&!source.includes(c.sourceExcerpt))console.log('Bad source:',c.claimId,c.sourceExcerpt);
 }
 if(f)console.log('Truth:',f.narrativeExcerpt,'=>',f.evidenceExcerpt);
 for(const c of e?.claims??[]){
  const compare=a?.assessments?.find(b=>b.claimId===c.claimId);
  if(process.argv[2]==='all'||compare?.material&&compare.verdict!=='SUPPORTED'||f&&(c.category===f.category||c.narrativeExcerpt.includes(f.narrativeExcerpt)||f.narrativeExcerpt.includes(c.narrativeExcerpt))){
   console.log(c.claimId,c.category,c.narrativeExcerpt,'|',c.narrativeMeaning);
   if(compare)console.log(' ',compare.verdict,compare.material,compare.sourceExcerpt,'|',compare.sourceMeaning,'|',compare.explanation);
  }
 }
 for(const q of a?.coverageQuestions??[])console.log('Coverage:',q);
 for(const b of old?.blockers??[])console.log('E20:',b.status,b.narrativeExcerpt,'|',b.evidenceExcerpt,'|',b.problem);
}

