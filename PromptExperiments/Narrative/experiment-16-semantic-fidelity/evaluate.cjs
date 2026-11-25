const fs=require('fs'),p=require('path'),crypto=require('crypto');
const d=__dirname;
const get=f=>fs.readFileSync(p.join(d,f),'utf8');
const read=f=>JSON.parse(get(f));
const write=(f,v)=>fs.writeFileSync(p.join(d,f),v);
for(const row of get('freeze.sha256').trim().split('\n')){
 const [hash,file]=row.split('  ');
 if(crypto.createHash('sha256').update(fs.readFileSync(p.join(d,file))).digest('hex')!==hash) throw Error('Frozen file changed '+file);
}
const assessments=new Map(), findings=[];
for(const [id,review] of Object.entries(read('blind-assessments.json'))) assessments.set(id,review);
const rows=get('run-manifest.csv').trim().split('\n').slice(1).map(line=>{
 const [slot,id,subject,set,repeat,arm]=line.split(',');
 const [marks,note,issues=[],format=false]=assessments.get(id)??[];
 if(!/^[1-5]{5}$/.test(marks)||!note) throw Error('Missing assessment '+id);
 const text=read('runs/'+id+'/result.json').content;
 const meta=read('runs/'+id+'/accepted.json');
 if(meta.exitCode!==0||!meta.accepted||meta.id!==id||meta.arm!==arm||meta.subject!==subject||meta.slot!==Number(slot)||meta.model!=='gpt-6-luna'||!text.trim()) throw Error('Invalid result '+id);
 if(get('runs/'+id+'/result.json')!==get('runs/'+id+'/attempt-'+meta.attempt+'.result.json'))throw Error('Original payload mismatch '+id);
 const source=get('cases/'+subject+'/source.md');
 for(const [level,category,origin,excerpt,sourceExcerpt,explanation] of issues){
  if(!['F1','F2'].includes(level)||!excerpt||!text.includes(excerpt)||!sourceExcerpt||!source.includes(sourceExcerpt)||!explanation) throw Error('Invalid finding excerpt '+id+' '+excerpt);
  findings.push({id,subject,set,arm,level,category,origin,excerpt,sourceExcerpt,explanation});
 }
 const ratings=[...marks].map(Number), weighted=ratings.reduce((t,x,i)=>t+x*[25,25,25,15,10][i],0);
 const fidelity=issues.some(i=>i[0]==='F2')?'F2':issues.length?'F1':'F0';
 return {id,subject,set,repeat:Number(repeat),arm,ratings,weighted,fidelity,editorialPass:weighted>=350&&ratings.slice(0,3).every(x=>x>=3),format,note};
});
if(rows.length!==48||assessments.size!==48||new Set(rows.map(x=>x.id)).size!==48)throw Error('Invalid coverage');
const summarize=group=>{
 const n=group.length;
 return {n,F0:group.filter(r=>r.fidelity==='F0').length,F1:group.filter(r=>r.fidelity==='F1').length,F2:group.filter(r=>r.fidelity==='F2').length,editorialPass:group.filter(r=>r.editorialPass).length,mean:group.reduce((a,r)=>a+r.weighted,0)/(n*100),coreMeans:[0,1,2].map(i=>group.reduce((a,r)=>a+r.ratings[i],0)/n),coreBelow3:[0,1,2].map(i=>group.filter(r=>r.ratings[i]<3).length)};
};
const groups={};
for(const group of ['all','regression','holdout',...new Set(rows.map(r=>r.subject))])
 groups[group]=Object.fromEntries(['A','B'].map(a=>[a,summarize(rows.filter(x=>x.arm===a&&(group==='all'||x.subject===group||x.set===group)))]));
const {A,B}=groups.all;
const newRecurring=[...new Set(findings.filter(r=>r.arm==='B').map(r=>r.category))].filter(c=>!findings.some(r=>r.arm==='A'&&r.category===c)&&new Set(findings.filter(r=>r.arm==='B'&&r.category===c).map(r=>r.id)).size>=2);
const gates={regressionZero:groups.regression.B.F2===0,holdoutZero:groups.holdout.B.F2===0,editorialAcceptance:B.editorialPass>=20,editorialMean:B.mean+0.15000001>=A.mean,coreCriteria:B.coreMeans.every((v,i)=>v+0.25000001>=A.coreMeans[i]&&B.coreBelow3[i]<=A.coreBelow3[i]+2),regressionImprovement:groups.regression.A.F2-groups.regression.B.F2>=2,noNewRecurringCategory:newRecurring.length===0,coverage:A.n===24&&B.n===24&&Object.entries(groups).every(([k,g])=>k==='all'||['regression','holdout'].includes(k)?true:g.A.n===3&&g.B.n===3),formatNeutrality:rows.filter(r=>r.arm==='B').every(r=>!r.format)};
const decision=Object.values(gates).every(Boolean)?'GO':'NO-GO';
const csv=(headers,items)=>headers.join(',')+'\n'+items.map(r=>r.map(v=>'"'+String(v).replaceAll('"','""')+'"').join(',')).join('\n')+'\n';
write('scores.csv',csv(['id','subject','set','arm','repeat','fidelity','editorialPass','formatViolation'],rows.map(r=>[r.id,r.subject,r.set,r.arm,r.repeat,r.fidelity,r.editorialPass,r.format])));
write('editorial-scores.csv',csv(['id','subject','arm','direction','development','usability','resolution','evidence','weighted','editorialPass','rationale'],rows.map(r=>[r.id,r.subject,r.arm,...r.ratings,(r.weighted/100).toFixed(2),r.editorialPass,r.note])));
write('fidelity-findings.md','# Fidelity findings\n\n'+findings.map(r=>'## '+r.id+' '+r.subject+' '+r.arm+' '+r.level+'\n\n'+r.category+' ('+r.origin+'). '+r.explanation+'\n\n- Narrative: “'+r.excerpt+'”\n- [Source](cases/'+r.subject+'/source.md): “'+r.sourceExcerpt+'”').join('\n\n')+'\n');
write('decision.json',JSON.stringify({decision,gates,newRecurring,groups},null,2)+'\n');
console.log(JSON.stringify({decision,gates,groups},null,2));

