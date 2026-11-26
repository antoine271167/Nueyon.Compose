const fs=require('node:fs');
const path=require('node:path');
process.chdir(__dirname);
const read=f=>JSON.parse(fs.readFileSync(f,'utf8'));
const decisions={};
const files=fs.readdirSync('reviews/adjudication',{recursive:true,withFileTypes:true})
  .filter(f=>f.name==='parsed.json').map(f=>path.join(f.parentPath,f.name));
for(const file of files){
  for(const {key,...decision} of read(file))
    decisions[key]={...decision,independentReview:file.replaceAll('\\','/')};
}
const overrides=fs.existsSync('operator-adjudications.json')?read('operator-adjudications.json'):[];
const flags=read('factual-flags.json');
for(const {subject,phase,stage,condition,contains,exact,decision,origin,reason,source} of overrides){
  const matches=flags.filter(f=>f.subject===subject&&(!phase||f.phase===phase)&&(!stage||f.stage===stage)&&
    (!condition||f.condition===condition)&&(!contains||f.excerpt.toLowerCase().includes(contains.toLowerCase()))&&(!exact||f.excerpt===exact));
  if(matches.length===0)throw Error('Operator adjudication did not match a flag: '+subject+' '+(exact??contains));
  for(const f of matches)decisions[f.key]={...decisions[f.key],decision,origin,reason,source,operator:true};
}
fs.writeFileSync('adjudications.json',JSON.stringify(decisions,null,2)+'\n');
