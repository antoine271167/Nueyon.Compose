// Offline only: historical data are never rewritten or counted as new live results.
const fs=require('fs'),path=require('path'),crypto=require('crypto');const out=__dirname,old=path.join(out,'../evidence-map-experiment-3-2026-11-12');
const read=(d,p)=>JSON.parse(fs.readFileSync(path.join(d,p),'utf8')),sha=s=>crypto.createHash('sha256').update(Buffer.from(s,'utf8')).digest('hex');
const historical=[];
for(const id of ['intro','005','010','garden','kiln','catalogue']){
 const input=read(old,`cases/${id}/bad/input.json`),source=(input.Input||input.input).Content??input.input.content,map=read(old,`calls/${id}-map/map-raw.json`);
 const anchors=map.items.flatMap(i=>i.sourceQuotes.map(quote=>({item:i.id,quote,exact:source.includes(quote)})));
 historical.push({id,reusedFromExperiment:3,allExact:anchors.every(a=>a.exact),anchors});
}
const current=[];
for(const id of ['intro','005','010']){
 const source=read(out,`cases/${id}/bad/input.json`).Input.Content,catalog=read(out,`fragments/${id}.json`),raw=read(out,`calls/${id}-map/map-raw.json`),hydrated=read(out,`calls/${id}-map/map.json`);
 const catalogExact=catalog.sourceSha256Utf8===sha(source)&&catalog.fragments.map(f=>f.text).join('')===source&&catalog.fragments.every(f=>source.slice(f.start,f.start+f.length)===f.text);
 const entries=raw.items.map(i=>{const h=hydrated.items.find(h=>h.id===i.id);return{id:i.id,allKnown:i.sourceFragmentIds.every(fid=>catalog.fragments.some(f=>f.id===fid)),unique:new Set(i.sourceFragmentIds).size===i.sourceFragmentIds.length,quotesExact:h.sourceQuotes.every((q,n)=>q===catalog.fragments.find(f=>f.id===i.sourceFragmentIds[n]).text&&source.includes(q))};});
 current.push({id,catalogExact,items:raw.items.length,anchorReferences:raw.items.reduce((n,i)=>n+i.sourceFragmentIds.length,0),entries});
}
const result={method:'Ordinal exact matching; no trim, case folding, newline conversion, Markdown stripping or Unicode normalization. Historical quotations reused, current ID maps new.',historical,current};
fs.writeFileSync(path.join(out,'verification-anchor-replay.json'),JSON.stringify(result,null,2));
console.log(JSON.stringify({historicalValidMaps:historical.filter(x=>x.allExact).length,historicalMaps:historical.length,newMaps:current.length,newItems:current.reduce((n,x)=>n+x.items,0),newAnchorReferences:current.reduce((n,x)=>n+x.anchorReferences,0),newValid:current.every(x=>x.catalogExact&&x.entries.every(e=>e.allKnown&&e.unique&&e.quotesExact))},null,2));
