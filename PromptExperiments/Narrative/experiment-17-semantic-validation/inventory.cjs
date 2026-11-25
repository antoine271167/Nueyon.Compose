const fs = require('node:fs');
const path = require('node:path');
const crypto = require('node:crypto');
const root = 'PromptExperiments/Narrative';
const names = fs.readdirSync(root);
const exp = n => path.join(root, names.find(s => s.startsWith('experiment-' + n + '-') && !s.includes('semantic-validation')));
const read = f => fs.readFileSync(f, 'utf8').replace(/^\uFEFF/, '');
const hash = f => crypto.createHash('sha256').update(fs.readFileSync(f)).digest('hex');
const csv = f => read(f).trim().split(/\r?\n/).map(l => [...l.matchAll(/(?:^|,)(?:"((?:[^"]|"")*)"|([^,]*))/g)].map(m => m[1]?.replace(/""/g, '"') ?? m[2]));
const rows = f => { const [h,...r] = csv(f); return r.map(a => Object.fromEntries(h.map((k,i) => [k,a[i]]))); };
const saveCsv = (f,r) => fs.writeFileSync(path.join(__dirname,f), [Object.keys(r[0]),...r.map(Object.values)].map(a => a.map(v => '"' + String(v ?? '').replaceAll('"','""') + '"').join(',')).join('\n')+'\n');
const scores = new Map(rows(path.join(exp(15),'scores.csv')).map(r=>[r.id,r.fidelity]));
const old = rows(path.join(exp(15),'manifest.csv')).map(({id,experiment,variant,subject,repeat,output,idea,synthesis,source}) => ({id,experiment,variant,subject,repeat,output,idea,synthesis,source,previous:scores.get(id)}));
const recent = rows(path.join(exp(16),'scores.csv')).map(({id,subject,arm,repeat,fidelity})=>{
  const dir = path.join(exp(16),'cases',subject);
  return {id,experiment:'16',variant:arm,subject,repeat,output:path.join(exp(16),'runs',id,'result.json'),idea:path.join(dir,'idea.json'),synthesis:path.join(dir,fs.readdirSync(dir).find(f=>f.startsWith('synthesis-'))),source:path.join(dir,'source.md'),previous:fidelity};
});
const inventory = [...old,...recent].map(r=>{
  for(const k of ['output','idea','synthesis','source']) r[k]=r[k].replaceAll('\\','/');
  return {...r,outputSha256:hash(r.output),ideaSha256:hash(r.idea),synthesisSha256:hash(r.synthesis),sourceSha256:hash(r.source)};
});
const selectors = [
  ['wolf','F2','13'],['wolf','F0','16'],['webb','F2','14'],['webb','F0','13'],['webb','F1','16'],
  ['faa','F2','16'],['faa','F0','16'],['eia','F2','16'],['eia','F0','16'],
  ['nist','F1','16'],['nist','F0','16'],['suffrage','F1','16'],['suffrage','F0','16'],
  ['digital','F1','16'],['digital','F0','13'],['lowell','F1','16'],['lowell','F0','14'],
  ['library','F1','13'],['library','F0','14'],['cooling','F1','14'],['cooling','F0','13']
];
const seen = new Map();
for (const r of inventory) {
  r.duplicateOf = seen.get(r.outputSha256) ?? '';
  if (!r.duplicateOf) seen.set(r.outputSha256,r.id);
}
const selected = selectors.map(([subject,previous,experiment])=>{
  const match = inventory.find(r=>r.subject===subject && r.previous===previous && r.experiment===experiment && !r.duplicateOf)
    ?? inventory.find(r=>r.subject===subject && r.previous===previous && !r.duplicateOf);
  if (!match) throw Error(subject + previous);
  return {...match,split:['wolf','webb'].includes(subject)?'development':'evaluation'};
});
if (require.main === module) {
  saveCsv('inventory.csv',inventory);
  saveCsv('dataset-manifest.csv',selected);
  for (const r of selected) console.log(r.id,r.subject,r.previous,r.experiment);
}
module.exports = {read,hash,rows,saveCsv};
