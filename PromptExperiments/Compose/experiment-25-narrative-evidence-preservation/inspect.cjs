const fs=require('node:fs');
process.chdir(__dirname);
const read=f=>JSON.parse(fs.readFileSync(f,'utf8'));
const m=read('evidence-preservation-metrics.json');
const a=read('article-metrics.json');
const plan=read('plan.json');
const records=[];
for(const row of read('dataset.json')){
  const {subject,phase,richness,cohort}=row;
  const inv=read('evidence-inventories/'+subject+'/verified.json').items;
  for(const item of inv){
    const lookup=(stage,c)=>{
      const rows=(stage==='narrative'?m.rows:a.rows).filter(r=>r.subject===subject&&r.c===c&&r.stage===stage&&r.factualReviewed);
      return rows.length?rows.map(r=>r.preserved.includes(item.id)?'preserved':r.distorted.includes(item.id)?'distorted':'omitted'):null;
    };
    records.push({subject,phase,richness,cohort,id:item.id,claim:item.claim,kind:item.kind,
      narrative:Object.fromEntries(['A','B','C'].map(c=>[c,lookup('narrative',c)?.[0]??null])),
      articles:Object.fromEntries(['A','B','C'].map(c=>[c,lookup('article',c)]))});
  }
}
fs.writeFileSync('evidence-item-outcomes.json',JSON.stringify(records,null,2)+'\n');
for(const c of ['A','B','C']){
  console.log(c,m.holdout[c],a.holdout[c]);
  const omitted=records.filter(r=>r.phase==='holdout'&&r.narrative[c]==='omitted');
  console.log('Omitted from Narrative:',omitted.map(r=>r.subject+'/'+r.id).join(', '));
}
const historical=new Set(['wolf','webb','digital','eia','faa']);
const matches={
  wolf:/elk|eland|wapiti|kalf|prooi|stier/i,
  webb:/cool|koel|uitvouw|deploy|alignment|afstell|inregel/i,
  digital:/sprint|week|pauz|suspend/i,
  eia:/percent|%|kwh|btu|trillion|biljoen|quadrillion|biljard/i,
  faa:/shape|vorm|ellipse|ellips|cirkel|circle|cylind|cilind|wrong surface|ground movement/i
};
const anchors=[];
for(const [stage,tasks] of Object.entries(plan)){
  for(const t of tasks.filter(r=>historical.has(r.subject))){
    const f=stage==='narratives'?'narratives/'+(t.c==='C'?'C-reference':t.c)+'/'+t.subject+'/result.json':
      'outputs/'+(stage==='linkedin'?'linkedin-neutrality':t.phase)+'/'+t.subject+'-'+t.rep+'-'+t.c+'/result.json';
    if(!fs.existsSync(f))continue;
    const body=read(f).content;
    anchors.push({stage,...t,file:f,excerpts:body.split(/\n+|(?<=[.!?])\s+(?=[\p{Lu}])/u).filter(s=>matches[t.subject].test(s))});
  }
}
fs.writeFileSync('regression-anchor-excerpts.json',JSON.stringify(anchors,null,2)+'\n');
