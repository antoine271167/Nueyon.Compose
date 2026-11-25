const fs=require('node:fs');
const path=require('node:path');
const Module=require('node:module');
const runner=path.join(__dirname,'run.cjs');
const variant=new Module(runner,module);
variant.filename=runner;
variant._compile(fs.readFileSync(runner,'utf8')+'\nmodule.exports={mechanical,semantic};',runner);
const {mechanical,semantic}=variant.exports;
const source={category:'numerical_scale',anchors:'Btu',source:'42.4 out of 605.6 quadrillion Btu'};
const examples=[
 ['bare scale relation','42,4 van de 605,6 quadriljoen Btu','FAIL'],
 ['converted scale name','42,4 van de 605,6 biljard Btu','REVIEW'],
 ['omission','The source compares two quantities.','REVIEW'],
 ['other unit','42,4 van de 605,6 quadriljoen joule','REVIEW'],
 ['two ratio clauses','10,0 van de 30,7 biljoen kWh; 42,4 van de 605,6 quadriljoen Btu','FAIL'],
 ['editorial prohibition','Do not say 42,4 van de 605,6 quadriljoen Btu.','REVIEW']
];
const results=examples.map(([name,content,expected])=>{
 const {status:actual,findings}=mechanical(content,source);
 return {name,content,expected,actual,agreement:expected===actual,findings};
});
const request={candidates:['Readers and writers can proceed concurrently.']};
const invalid=[
 ['malformed','{'],
 ['missing correction',JSON.stringify({verdict:'CONTRADICTED',narrativeExcerpt:request.candidates[0],relation:'reason',correction:null})],
 ['invented quote',JSON.stringify({verdict:'CONTRADICTED',narrativeExcerpt:'quote not in input',relation:'reason',correction:'fix'})],
 ['invalid status',JSON.stringify({verdict:'GO',narrativeExcerpt:null,relation:'reason',correction:null})]
].map(([name,body])=>{
 try{semantic(body,request);return {name,rejected:false};}
 catch(e){return {name,rejected:true,reason:e.message};}
});
console.log(JSON.stringify({source,results,invalid},null,2));
