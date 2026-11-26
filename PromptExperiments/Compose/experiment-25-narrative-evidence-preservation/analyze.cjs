const fs=require('node:fs');
const path=require('node:path');
const {createHash}=require('node:crypto');
process.chdir(__dirname);
const has=f=>fs.existsSync(f);
const read=f=>JSON.parse(fs.readFileSync(f,'utf8'));
const hash=f=>createHash('sha256').update(fs.readFileSync(f)).digest('hex');
const write=(f,x)=>fs.writeFileSync(f,JSON.stringify(x,null,2)+'\n');
const mean=values=>values.length?values.reduce((a,b)=>a+b,0)/values.length:null;
const median=values=>values.length?[...values].sort((a,b)=>a-b)[Math.floor((values.length-1)/2)]/2+[...values].sort((a,b)=>a-b)[Math.floor(values.length/2)]/2:null;
const fmt=n=>n===null||n===undefined?'unavailable':n.toFixed(2);
const plan=read('plan.json');
const rows=[];
const findings=[];
const csv=(f,fields,data)=>fs.writeFileSync(f,fields.join(',')+'\n'+data.map(r=>fields.map(k=>'"'+String(r[k]??'').replaceAll('"','""')+'"').join(',')).join('\n')+'\n');
const weights={article:[.2,.25,.2,.15,.1,.1],narrative:[.2,.25,.25,.15,.1,.05],linkedin:[.2,.15,.25,.15,.15,.1]};
const editorialKeys={
  article:['opening_interest','development_substance','readability_flow','focus_coherence','ending_impact','naturalness_originality'],
  linkedin:['hook_immediate_interest','readability_pacing','substance_insight','focus_progression','ending_engagement','natural_credible_voice']
};
const adjudications=has('adjudications.json')?read('adjudications.json'):{};
const preservationChecks=has('preservation-adjudications.json')?read('preservation-adjudications.json'):{};
const anchorFindings=has('operator-anchor-findings.json')?read('operator-anchor-findings.json'):[];
const claimKey=(r,c)=>r.id+'|'+c.excerpt;
function verifiedPreservation(r,stage,fact){
  if(!fact)return null;
  const revised={...fact};
  const status=['preserved','distorted','omitted'];
  const changes={...(stage==='narrative'?preservationChecks[r.subject+':'+r.c]?.changes:{})};
  if(stage==='article'&&anchorFindings.some(f=>f.id===r.id)){
    if(r.subject==='wolf'){
      changes['multiple-influences-on-predation']='distorted';
      if(anchorFindings.some(f=>f.id===r.id&&f.origin==='amplification')||r.c!=='B')changes['prey-composition-shift']='distorted';
    }
    if(r.subject==='eia')changes['renewable-all-energy-production-share-2024']='distorted';
  }
  for(const key of status)revised[key]=fact[key].filter(id=>!Object.hasOwn(changes,id));
  for(const [id,value] of Object.entries(changes)){
    if(!status.some(k=>fact[k].includes(id)))throw Error('Unknown item in preservation adjudication: '+id);
    revised[value].push(id);
  }
  return revised;
}
function review(dir,out,{flagsOnly=false}={}){
  if(!has(dir+'/attempt.json'))return null;
  const attempt=read(dir+'/attempt.json');
  if(attempt.outputHash!==hash(out)||attempt.requestHash!==hash(dir+'/request.txt'))throw Error('Review linkage differs: '+dir);
  const parsed=dir+'/parsed.json';
  if(!has(parsed))return null;
  const data=read(parsed);
  if(attempt.exit!==0)return null;
  if(!Array.isArray(data.claims)){
    const keys=editorialKeys[dir.split('/')[1].replace('-editorial','')];
    if(!Array.isArray(data.scores)&&keys)data.scores=keys.map(key=>data.scores?.[key]);
    return Array.isArray(data.scores)&&data.scores.length===6&&data.scores.every(v=>Number.isInteger(v)&&v>=1&&v<=5)?data:null;
  }
  const subject=Object.keys(plan).flatMap(k=>plan[k]).find(r=>dir.split('/').at(-1).replace(/-(?:second|coverage)$/,'')===r.id)?.subject;
  const ids=read('evidence-inventories/'+subject+'/verified.json').items.map(i=>i.id);
  const body=read(out).content;
  for(const claim of data.claims){
    claim.excerpt??=claim.bodyExcerpt;
    if(typeof claim.excerpt==='string'&&!body.includes(claim.excerpt))
      claim.excerpt=claim.excerpt.trim().replace(/^["“]|["”]$/g,'');
    if(typeof claim.label==='string')claim.label=claim.label.match(/^(F[012]|U)(?![A-Za-z0-9])/)?.[0];
  }
  const stages=['preserved','distorted','omitted'];
  const claimFits=c=>typeof c.excerpt==='string'&&c.excerpt.length>0&&body.includes(c.excerpt)&&['F0','F1','F2','U'].includes(c.label);
  if(flagsOnly)return {...data,claims:data.claims.filter(claimFits)};
  const sound=data.claims.length>0&&data.claims.every(claimFits)&&
    stages.every(k=>Array.isArray(data[k]))&&stages.flatMap(k=>data[k]).sort().join('|')===ids.sort().join('|');
  return sound?data:null;
}
for(const [stage,entries] of [['narrative',plan.narratives],['article',plan.articles],['linkedin',plan.linkedin]]){
  for(const r of entries){
    const {subject:s,c,phase,rep,id}=r;
    const n='narratives/'+({A:'A',B:'B',C:'C-reference'}[c])+'/'+s+'/result.json';
    const out=stage==='narrative'?n:'outputs/'+(stage==='article'?phase:'linkedin-neutrality')+'/'+s+'-'+rep+'-'+c+'/result.json';
    const attemptFile=path.dirname(out)+'/attempt.json';
    const generated=has(out)&&(stage==='narrative'&&c==='C'||has(attemptFile)&&read(attemptFile).valid);
    const row={...r,stage,generated};
    rows.push(row);
    if(!generated)continue;
    const body=read(out).content;
    row.words=body.match(/[\p{L}\p{N}]+(?:['’.-][\p{L}\p{N}]+)*/gu)?.length??0;
    if(stage==='narrative'){
      if(c==='C'){
        const trace=read(path.dirname(out)+'/trace.json');
        if(trace.outputHash!==hash(out)||trace.inputHash!==hash('fixtures/'+s+'/upstream.json')||
          trace.items.some(i=>!body.includes(i.synthesisExcerpt)))throw Error('C provenance mismatch '+s);
      }else {
        const original='fixtures/'+s+'/upstream.json';
        const canonical='fixtures/'+s+'/cli-input.json';
        if(has(canonical)){
          const old=read(original), mapped=read(canonical);
          const expected=Object.fromEntries(Object.entries(old.selectedIdea).map(([k,v])=>[k[0].toLowerCase()+k.slice(1),v]));
          if(JSON.stringify({...old,selectedIdea:expected})!==JSON.stringify(mapped))throw Error('Casing recovery changed meaning '+s);
        }
        if(read(attemptFile).inputHash!==hash(has(canonical)?canonical:original))throw Error('Unequal Narrative input '+s);
      }
    }else{
      const input=path.dirname(out)+'/input.json';
      const payload=read(input);
      if(read(attemptFile).inputHash!==hash(input)||payload.narrative.content!==read(n).content||
        Object.keys(payload).sort().join()!=='composition,narrative'||Object.keys(payload.narrative).join()!=='content'||
        payload.composition.format!==(stage==='article'?'Article':'LinkedInPost'))throw Error('Compose input differs '+out);
    }
    const edit=review('reviews/'+stage+'-editorial/'+phase+'/'+id,out);
    if(edit){
      row.scores=edit.scores;
      row.score=edit.scores.reduce((sum,v,i)=>sum+v*weights[stage][i],0);
      row.accepted=row.score>=3.75-1e-9&&row.scores.every(v=>v>=3);
      row.defects=edit.defects;
      row.usability=edit.usability;
    }
    const dir='reviews/'+stage+'-factual/'+phase+'/'+id;
    const primary=review(dir,out);
    const second=review(dir+'-second',out);
    const coverage=review(dir+'-coverage',out);
    const originalFact=primary??second??coverage;
    const fact=verifiedPreservation(r,stage,originalFact);
    const flags=new Map();
    for(const assessment of ['', '-second', '-coverage'].map(suffix=>review(dir+suffix,out,{flagsOnly:true})).filter(x=>Array.isArray(x?.claims))){
      for(const cl of assessment.claims.filter(cl=>['F2','U'].includes(cl.label))){
        const k=claimKey(r,cl);
        if(!flags.has(k))flags.set(k,{...cl,reviewLabels:[]});
        flags.get(k).reviewLabels.push(cl.label);
      }
    }
    for(const [key,cl] of flags)
      findings.push({...cl,id,subject:s,condition:c,stage,phase,reviewLabels:cl.reviewLabels.join('/'),decision:adjudications[key]?.decision??'pending',decisionReason:adjudications[key]?.reason,decisionOrigin:adjudications[key]?.origin,key});
    if(!fact)continue;
    row.factualReviewed=true;
    const items=read('evidence-inventories/'+s+'/verified.json').items;
    row.factualClaims=fact.claims.length;
    row.inventory=items.length;
    row.independentPreserved=originalFact.preserved;
    row.independentPreservation=items.length?originalFact.preserved.length/items.length:null;
    row.preserved=fact.preserved;
    row.preservation=items.length?fact.preserved.length/items.length:null;
    row.distorted=fact.distorted;
    row.omitted=fact.omitted;
    row.developments=fact.developments;
    row.explainedFacts=fact.explainedFacts;
    row.paragraphSteps=fact.paragraphSteps;
    row.endpointAddsMeaning=fact.endpointAddsMeaning;
    row.redundancy=fact.redundancy;
    row.peripheral=fact.peripheral;
    row.unsupported=fact.unsupported;
    row.formatNeutral=fact.formatNeutral;
    if(stage!=='narrative'){
      const nr=plan.narratives.find(v=>v.subject===s&&v.c===c);
      const nd='reviews/narrative-factual/'+phase+'/'+nr.id;
      const nf=verifiedPreservation(r,'narrative',review(nd,n)??review(nd+'-second',n)??review(nd+'-coverage',n));
      if(nf) {
        row.realizedFromNarrative=fact.preserved.filter(i=>nf.preserved.includes(i)).length;
        row.usefulNarrativeDetailsOmitted=nf.preserved.filter(i=>!fact.preserved.includes(i)).length;
      }
    }
  }
}
if(anchorFindings.length){
  for(const evidence of anchorFindings){
    const key=claimKey(evidence,evidence);
    const matched=findings.find(f=>f.key===key);
    const decision={decision:evidence.decision,decisionOrigin:evidence.origin,decisionReason:evidence.reason,
      sourceEvidence:evidence.sourceEvidence,synthesisStatement:evidence.synthesisStatement,narrativeStatement:evidence.narrativeStatement,
      operatorVerified:true};
    if(matched)Object.assign(matched,decision);
    else findings.push({...evidence,key,...decision,reviewLabels:'operator source check'});
  }
}
function summarize(group){
  const ok=group.filter(r=>r.generated);
  const rated=ok.filter(r=>r.scores);
  const factual=ok.filter(r=>r.factualReviewed);
  const numberMean=(group,key)=>mean(group.map(r=>r[key]).filter(Number.isFinite));
  return {planned:group.length,generated:ok.length,editorial:rated.length,factual:factual.length,
    accepted:rated.filter(r=>r.accepted).length,acceptance:rated.length?rated.filter(r=>r.accepted).length/rated.length:null,
    score:numberMean(rated,'score'),criteria:Array.from({length:6},(_,i)=>mean(rated.map(r=>r.scores[i]))),
    words:numberMean(ok,'words'),medianWords:median(ok.map(r=>r.words)),inventory:numberMean(factual,'inventory'),
    preservation:numberMean(factual,'preservation'),independentPreservation:numberMean(factual,'independentPreservation'),preservedCount:mean(factual.map(r=>r.preserved.length)),
    developments:numberMean(factual,'developments'),explainedFacts:numberMean(factual,'explainedFacts'),
    realized:numberMean(factual,'realizedFromNarrative'),omittedNarrative:numberMean(factual,'usefulNarrativeDetailsOmitted'),
    endpointRate:mean(factual.map(r=>r.endpointAddsMeaning).filter(v=>typeof v==='boolean').map(Number)),
    redundancy:mean(factual.map(r=>r.redundancy?.length).filter(Number.isFinite)),
    peripheral:mean(factual.map(r=>r.peripheral?.length).filter(Number.isFinite)),
    defects:Object.fromEntries([...new Set(rated.flatMap(r=>r.defects??[]))].map(d=>[d,rated.filter(r=>r.defects?.includes(d)).length]))};
}
function paired(stage,metric,c){
  const hold=rows.filter(r=>r.stage===stage&&r.phase==='holdout');
  const deltas={};
  for(const s of new Set(hold.map(r=>r.subject))){
    const per=c=>hold.filter(r=>r.subject===s&&r.c===c).map(metric).filter(Number.isFinite);
    const a=per('A'),b=per(c);
    const reps=stage==='article'?3:1;
    if(a.length===reps&&b.length===reps)deltas[s]=mean(b)-mean(a);
  }
  const vals=Object.values(deltas);
  if(!vals.length)return {mean:null,interval:null,deltas};
  let seed=25;
  const sample=()=>{seed=(Math.imul(seed,1664525)+1013904223)>>>0;return vals[Math.floor(seed/2**32*vals.length)];};
  const boot=Array.from({length:10000},()=>mean(vals.map(sample))).sort((a,b)=>a-b);
  return {mean:mean(vals),interval:[boot[250],boot[9749]],deltas};
}
const metrics={};
for(const stage of ['narrative','article','linkedin']){
  metrics[stage]={};
  for(const phase of ['development','holdout']){
    const all=rows.filter(r=>r.stage===stage&&r.phase===phase);
    if(!all.length)continue;
    metrics[stage][phase]={};
    for(const c of stage==='linkedin'?['A','B']:['A','B','C']){
      const group=all.filter(r=>r.c===c);
      metrics[stage][phase][c]={...summarize(group),rich:summarize(group.filter(r=>r.richness==='rich')),
        constrained:summarize(group.filter(r=>r.richness==='constrained'))};
    }
  }
  metrics[stage].paired={};
  for(const c of stage==='linkedin'?['B']:['B','C'])
    metrics[stage].paired[c]=Object.fromEntries(Object.entries({score:r=>r.score,
      acceptance:r=>r.scores?Number(r.accepted):null,preservation:r=>r.preservation,independentPreservation:r=>r.independentPreservation,
      opening:r=>r.scores?.[0],development:r=>r.scores?.[1],readability:r=>r.scores?.[2],focus:r=>r.scores?.[3],ending:r=>r.scores?.[4],voice:r=>r.scores?.[5],
      words:r=>r.words,realized:r=>r.realizedFromNarrative,developments:r=>r.developments
    }).map(([k,v])=>[k,paired(stage,v,c)]));
}
write('evidence-preservation-metrics.json',{...metrics.narrative,rows:rows.filter(r=>r.stage==='narrative')});
write('article-metrics.json',{...metrics.article,linkedin:metrics.linkedin,rows:rows.filter(r=>r.stage!=='narrative')});
write('factual-flags.json',findings);
csv('factual-findings.csv',['id','subject','condition','stage','phase','excerpt','sourceEvidence','synthesisStatement','narrativeStatement','origin','explanation','materiality','reviewLabels','decision','decisionOrigin','decisionReason'],findings);
const rated=rows.filter(r=>r.scores).map(r=>({...r,...Object.fromEntries(r.scores.map((v,i)=>['score'+(i+1),v]))}));
csv('editorial-scores.csv',['id','subject','stage','phase','c','rep','score1','score2','score3','score4','score5','score6','score','accepted','words','defects'],rated);

function walk(dir){
  if(!has(dir))return [];
  return fs.readdirSync(dir,{recursive:true,withFileTypes:true}).filter(f=>f.isFile()).map(f=>path.join(f.parentPath,f.name));
}
const operations={};
for(const dir of ['evidence-inventories','narratives','outputs','reviews','recovery']){
  const files=walk(dir);
  const attempts=files.filter(f=>path.basename(f)==='attempt.json').map(read);
  operations[dir]={started:files.filter(f=>path.basename(f)==='started.json').length,completed:attempts.length,
    valid:attempts.filter(a=>a.valid).length,invalid:attempts.filter(a=>!a.valid).length,
    meanSeconds:mean(attempts.map(a=>a.seconds)),medianSeconds:median(attempts.map(a=>a.seconds)),totalProcessSeconds:attempts.reduce((s,a)=>s+a.seconds,0),
    invalidSlots:files.filter(f=>path.basename(f)==='attempt.json'&&!read(f).valid),
    openSlots:files.filter(f=>path.basename(f)==='started.json'&&!has(path.join(path.dirname(f),'attempt.json')))};
}
write('operations.json',operations);
