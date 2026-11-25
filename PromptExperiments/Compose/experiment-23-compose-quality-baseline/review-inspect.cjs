const {tasks,exists,out,json,reviewDir}=require('./run.cjs');
const {readReview}=require('./review-records.cjs');
const mode=process.argv[2]??'progress';
for(const folder of ['article','linkedin']){
  const rows=tasks.filter(t=>t.folder===folder);
  if(mode==='progress'){
    const formatted=rows.map(t=>({t,attempt:exists(out(t)+'/attempt.json')?json(out(t)+'/attempt.json'):null}))
      .filter(r=>r.attempt?.valid);
    console.log(folder,'generated',formatted.length,'review [attempted,usable]',Object.fromEntries(
      [['editorial',1],['factual',1],['factual',2]].map(([kind,pass])=>{
        const seen=formatted.map(({t})=>readReview(t,kind,pass)).filter(Boolean);
        return [kind+pass,[seen.length,seen.filter(r=>r.usable).length]];
      })));
    continue;
  }
  for(const t of rows){
    if(!exists(out(t)+'/result.json'))continue;
    if(mode==='flags'){
      for(const pass of [1,2]){
        const r=readReview(t,'factual',pass);
        if(!r?.data)continue;
        for(const c of r.data.claims.filter(c=>['F2','U'].includes(c.label)))
          console.log(JSON.stringify({output:folder+'/'+t.id,pass,...c}));
      }
    } else {
      const r=readReview(t,'editorial',1);
      if(r?.usable)console.log(JSON.stringify({output:folder+'/'+t.id,...r.data}));
    }
  }
}
