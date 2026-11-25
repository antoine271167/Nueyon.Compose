const {json,exists,out,reviewDir}=require('./run.cjs');
const labels=['F0','F1','F2','U'];
const baseLabel=value=>typeof value==='string'?value.match(/^(F[012]|U)(?![A-Za-z0-9])/)?.[0]:undefined;
const clip=value=>typeof value==='string'?value.trim().replace(/^["“]|["”]$/g,''):'';
const criteria={
  article:['opening_interest','development_substance','readability_flow','focus_coherence','ending_impact','naturalness_originality'],
  linkedin:['hook_immediate_interest','readability_pacing','substance_insight','focus_progression','ending_engagement','natural_credible_voice']
};
function readReview(task,type,pass) {
  const dir=reviewDir(task,type,pass);
  if(!exists(dir+'/attempt.json')) return null;
  const attempt=json(dir+'/attempt.json');
  const data=exists(dir+'/parsed.json')?json(dir+'/parsed.json'):null;
  if(attempt.exit!==0||!data) return {...attempt,usable:false};
  if(type==='editorial'){
    const scores=Array.isArray(data.scores)?data.scores:criteria[task.folder].map(key=>data.scores?.[key]);
    const usable=scores.length===6&&scores.every(s=>Number.isInteger(s)&&s>=1&&s<=5)&&Array.isArray(data.defects);
    return {...attempt,usable,data:{...data,scores}};
  }
  const content=json(out(task)+'/result.json').content;
  const valid=Array.isArray(data.claims)&&data.claims.length>0;
  const claims=valid?data.claims.map(claim=>({
    ...claim,label:baseLabel(claim.label)??null,exactExcerpt:content.includes(claim.excerpt)?claim.excerpt:
      content.includes(clip(claim.excerpt))?clip(claim.excerpt):null
  })):[];
  return {...attempt,usable:valid&&claims.every(c=>labels.includes(c.label)&&c.exactExcerpt),data:{claims}};
}
module.exports={readReview};
