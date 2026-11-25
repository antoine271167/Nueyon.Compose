const fs=require('fs'),p=require('path');
const d=__dirname;
const rows=fs.readFileSync(p.join(d,'run-manifest.csv'),'utf8').trim().split('\n').slice(1).map(l=>l.split(',')).filter(r=>!process.argv[2]||r[2]===process.argv[2]).sort((a,b)=>a[1].localeCompare(b[1]));
for(const [,id,subject] of rows){const f=p.join(d,'runs',id,'result.json');if(fs.existsSync(f)) console.log('\n'+id+' | '+subject+'\n'+JSON.parse(fs.readFileSync(f)).content);}

