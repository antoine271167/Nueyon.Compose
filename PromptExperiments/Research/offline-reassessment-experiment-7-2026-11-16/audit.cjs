// Offline only: built-in filesystem/crypto/process APIs. No service clients.
const fs=require('fs'),path=require('path'),crypto=require('crypto'),cp=require('child_process');
const root=path.resolve(__dirname,'../../..'), exp=path.join(root,'PromptExperiments/Research/production-validation-experiment-6-2026-11-12');
const read=p=>fs.readFileSync(p,'utf8').replace(/^\uFEFF/,'');
const hash=p=>crypto.createHash('sha256').update(fs.readFileSync(p)).digest('hex');
const walk=d=>fs.readdirSync(d,{withFileTypes:true}).flatMap(x=>x.isDirectory()?walk(path.join(d,x.name)):[path.join(d,x.name)]);
const out=(n,x)=>fs.writeFileSync(path.join(__dirname,n),typeof x==='string'?x:JSON.stringify(x,null,2)+'\n');
const protectedFiles=[...walk(path.join(root,'src')).filter(p=>!/[\\/](bin|obj|\.vs)[\\/]/.test(p)),...walk(path.join(root,'PromptExperiments')).filter(p=>!p.startsWith(__dirname+path.sep)&&!/[\\/](bin|obj|\.vs)[\\/]/.test(p))];
if(!fs.existsSync(path.join(__dirname,'protected-before.json'))) out('protected-before.json',Object.fromEntries(protectedFiles.map(p=>[path.relative(root,p),hash(p)])));
const rules={
 '005':[
 ['outcome','kern','Candidate not promoted; production Narrative user prompt restored byte-for-byte to baseline.',/not to promote|not promoted/i],
 ['scores','kern','Candidate mean 4.00/10',/4\.00|6\.00|3\.33|5\.67/i],
 ['gates','kern','All three candidate Narrative runs and all three candidate Compose runs failed this gate.',/criterion C|criterion D|hierarchy criterion|restraint criterion|failed.*orchestration|failed.*restraint|unsupported.*framing/i],
 ['scope','kern','All other prompt files and workflow inputs remained identical.',/changed only|only prompt change|only.*Narrative|other prompts.*matched|other prompt files/i],
 ['variation','belangrijk','Idea and Research outputs vary between runs.',/Idea and Research outputs vary|upstream Idea and Research|run-to-run variation/i],
 ['dates','belangrijk','The baseline runs were produced on 2026-11-03; candidate runs were produced on 2026-11-04.',/2026-11-03|Nov\. 3|Nov\. 4|Nov.*4|2026-11-04/i],
 ['backend','belangrijk','the model provider may change the backend behind an alias.',/backend/i],
 ['tension','context','That internal tension may have reinforced the technical angle.',/tension|competing emphasis|central to the product direction/i],
 ['limits','kern','Three runs per condition show consistency in these samples, not statistical significance.',/statistical significance|statistically significant/i],
 ['restore','kern','The production prompt was restored from the saved byte snapshot.',/restored byte-for-byte|restored.*baseline/i]
 ],
 harbor:[
 ['synthetic','kern','This dossier is a synthetic operational source for an evaluation, not a real transport study.',/fictional|synthetic|synthetisch/i],
 ['service','kern','the electric group completed 2,310 of 2,400 scheduled departures, or 96.3%.',/2,310|2\.310|96\.3|96,3/],
 ['duplicates','kern','30 entries were duplicate departure IDs and were removed on 9 July.',/duplicate|dubbele/i],
 ['substitution','kern','The summary completion rate is a service-delivery measure, not a measure of exclusive electric operation.',/substitut|exclusive electric|dieselbus|niet als vertrek|service-delivery/i],
 ['punctuality','kern','There is no reprocessing of the old GPS records under a common threshold.',/punctuality|punctualiteit|threshold|drempel/i],
 ['routes','kern','The diesel comparison vehicles were not randomly assigned.',/non-random|not random|niet willekeurig|relatively flat|relatief vlak/i],
 ['diesel-control','belangrijk','the hill route completed 1,170 of 1,200 departures before the demonstration and 1,188 of 1,200 during it.',/1,170|1\.170|1,188|1\.188/],
 ['cost','kern','Finance has not produced a like-for-like whole-cost calculation.',/8,400|8\.400|13,700|13\.700|whole-cost|totale.kosten/i],
 ['carbon','kern','Neither is a certified whole-life emissions estimate.',/emissions|emissie|net-carbon/i],
 ['audio','kern','two had an audio-stop announcement failure during inspection.',/audio/i],
 ['repair','kern','The maintenance team restored the two audio units on 2 July, after the trial ended.',/2 July|2 juli|2 juli|2 July/i],
 ['winter','kern','There were no freezing days and no winter heating loads.',/winter|freezing|vries/i],
 ['decision','kern','the board authorized preparation of a winter-assessment specification within the existing planning budget.',/specification|specificatie/i],
 ['feedback','context','These are volunteered comments from selected sessions, not 148 independently sampled citizens and not a representative satisfaction score.',/148|representative.*satisfaction|representatieve.*tevredenheid/i],
 ['meaning','context','That phrase is the operator\'s intended meaning, not a measured finding about emissions, public trust or service quality.',/visible first step|intended meaning|beoogde betekenis/i],
 ['maintenance','context','The count difference cannot by itself establish greater reliability.',/maintenance|onderhoud|reliability|betrouwbaarheid/i]
 ],
 archive:[
 ['output','kern','It created 900 catalogue records: 300 from each collection.',/900/],
 ['dimensions','kern','Cedar and Elm failed different checks;',/Cedar|Elm|Rowan/i],
 ['sampling','kern','Boxes with fragile bindings were excluded.',/fragile|fragiele|non-random|not a random|rather than.*random/i],
 ['dashboard','belangrijk','This is a reporting-rule correction, not an improvement to the underlying records.',/dashboard|reporting-rule/i],
 ['recheck','kern','Reviewers rechecked those seventeen corrected entries, not all 180 reviewed entries and not the remaining 720 catalogue records.',/17|seventeen|720|recheck/i],
 ['rights','kern','Rights clearance is complete for 760 of the 900 scans.',/760|140/],
 ['forecast','kern','that is a forecast without a committed release date.',/committed release|release date|forecast/i],
 ['keyboard','belangrijk','A keyboard-only review still found that the language switch could not be reached on two record-page templates.',/keyboard|language switch|contrast|toetsenbord/i],
 ['decision','kern','The committee approved designing a deeper random audit stratified by handwriting, print quality and collection.',/stratified|stratifi|audit/i],
 ['productivity','context','One cannot infer that the tool caused a 37.5% labor saving',/37\.5|24 minutes|15 minutes|225|48 recorded|labor saving/i],
 ['logs','context','The visible increase is not a comparable measure of adoption',/1,800|2,700|logging|search traffic|searches/i],
 ['role','context','That is a factual role error, not just a stylistic preference',/captain|owner|role error/i],
 ['meaning','context','an expression of our commitment to making local history approachable',/commitment|belonging|institution|access mission/i],
 ['praise-error','context','One message praised the interface while identifying a wrong vessel name.',/praised|praise|feedback|wrong vessel name/i]
 ]};
let rows=[], usage=[], checks=[];
const headings=['### Facts','### Interpretations','### Unknowns','### Development sequence','### Editorial relevance'];
function section(t,h){let a=t.indexOf(h);if(a<0)return null;let start=a+h.length;let ends=headings.filter(x=>x!==h).map(x=>t.indexOf(x,start)).filter(x=>x>=0);return t.slice(start,ends.length?Math.min(...ends):t.length).replace(/^[\r\n]+/,'').trimEnd();}
for(const c of Object.keys(rules)){
 let source=read(path.join(exp,'cases',c,'source.md')), inp=JSON.parse(read(path.join(exp,'cases',c,'input.json')));
 out('selected-idea-'+c+'.json',inp.SelectedIdea);
 for(const [id,material,quote,re] of rules[c]){
  let ix=source.indexOf(quote);if(ix<0)throw Error('Invalid source quote '+c+'/'+id);
  let sl=source.slice(0,ix).split('\n').length;
  for(const r of [1,2])for(const arm of ['A','B']){
   let run=`${c}-r${r}-${arm}-Research`,dir=path.join(exp,'calls',run), content=read(path.join(dir,'content.md')),projection=read(path.join(dir,'projection.md'));
   const hits=t=>t.split(/\r?\n/).flatMap((s,i)=>re.test(s)?[{line:i+1,text:s}]:[]);
   let raw=hits(content),proj=hits(projection), status=proj.length?'correct':raw.length?'deels':'ontbreekt',note='Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.';
   if(c==='005'&&(id==='variation'||(id==='dates'&&arm==='A'))&&r===1){status='ontbreekt';note='Concrete beperking ontbreekt; algemene causale terughoudendheid en backendonzekerheid zijn wel aanwezig.';}
   if(c==='harbor'&&id==='maintenance'&&arm==='A'&&r===2){status='ontbreekt';raw=[];proj=[];note='De onderhoudsregistervergelijking ontbreekt; maintenance bij audioreparatie is geen equivalent bewijs.';}
   if(c==='harbor'&&id==='routes'&&arm==='B'&&r===1){status='deels';note='Niet-random routes en dieseltegencontrast aanwezig; vakanties/omleiding ontbreken.';}
   if(c==='archive'&&id==='meaning'){status=proj.some(x=>/commitment/i.test(x.text))?'deels':status;note='Effecten op gebruikers zijn niet hetzelfde als het expliciet toegeschreven institutionele motief; geen kernvereiste.';}
   if(c==='archive'&&id==='keyboard'){status='ontbreekt';note='Geen contrast/keyboarddefect in volledige Research of projectie. Belangrijk voor brede toegankelijkheid; sensitiviteit als harde eis apart rapporteren.';}
   rows.push({case:c,id,material,sourceFile:`../production-validation-experiment-6-2026-11-12/cases/${c}/source.md`,sourceLine:sl,sourceQuote:quote,sourceParagraph:source.split(/\r?\n/)[sl-1],run,status,raw,projection:proj,note});
  }
 }
}
for(const d of fs.readdirSync(path.join(exp,'calls')).filter(x=>/-Research$/.test(x)).sort()){
 let dir=path.join(exp,'calls',d),raw=JSON.parse(read(path.join(dir,'response-raw.json'))),parsed=JSON.parse(raw.choices[0].message.content),content=read(path.join(dir,'content.md')),projection=read(path.join(dir,'projection.md'));
 let rendered=['### Facts','### Unknowns'].map(h=>[h,section(content,h)]).filter(x=>x[1]!==null).map(([h,s])=>h+'\n\n'+s.replace(/\r\n/g,'\n')).join('\n\n');
 let input=JSON.parse(read(path.join(dir,'input.json'))),c=d.split('-')[0],fixed=JSON.parse(read(path.join(exp,'cases',c,'input.json'))); usage.push({run:d,...raw.usage});
 let sourceMatch=input.input.content===fixed.Input.Content,ideaMatch=Object.entries(fixed.SelectedIdea.Idea).every(([k,v])=>input.selectedIdea.idea[k.toLowerCase()]===v);
 if(!sourceMatch||!ideaMatch)throw Error('Input mismatch '+d);
 checks.push({run:d,sourceMatch,allFiveIdeaFieldsMatch:ideaMatch,rawContentMatches:parsed.content.replace(/\r\n/g,'\n').trimEnd()===content.replace(/\r\n/g,'\n').trimEnd(),projectionMatchesFilterAlgorithm:rendered.trimEnd()===projection.replace(/\r\n/g,'\n').trimEnd(),headings:headings.every(h=>content.includes(h)),finish:raw.choices[0].finish_reason,inputKeys:Object.keys(input)});
}
out('traceability.json',rows);out('usage-from-responses.json',usage);out('offline-checks.json',checks);
let md='# Bron-naar-Research traceability matrix\n\nAlle citaten hieronder zijn exacte substrings, automatisch tegen de oorspronkelijke bestanden gecontroleerd. L = één-gebaseerd regelnummer. Research-ankers tonen de volledige oorspronkelijke regel. Geen anker betekent geen citaat, geen verzonnen leegtebewijs. De regex in audit.cjs helpt ankers verzamelen; semantische classificatie en materialiteit staan afzonderlijk vermeld. Bestandspaden zijn relatief aan Experiment 7.\n\n';
for(const c of Object.keys(rules))for(const [id,material] of rules[c]){
 const rr=rows.filter(x=>x.case===c&&x.id===id),x=rr[0];md+=`## ${c} / ${id} — ${material}\n\nBron [L${x.sourceLine}](${x.sourceFile}#L${x.sourceLine}): “${x.sourceQuote}”\n\nVolledige oorspronkelijke bronregel (context en overige componenten):\n\n> ${x.sourceParagraph}\n\n| Run | Classificatie | Research / projectie |\n|---|---|---|\n`;
 for(const row of rr)md+=`| ${row.run} | ${row.status} | ${row.raw.length?'ruw L'+row.raw.map(x=>x.line).join(', '):'geen anker in ruw'}; ${row.projection.length?'projectie L'+row.projection.map(x=>x.line).join(', '):'geen anker in projectie'} |\n`;
 for(const row of rr){md+=`\n### ${row.run}\n\n${row.note}\n\n`;for(const [type,hits] of [['content.md',row.raw],['projection.md',row.projection]]){md+=`[${type}](../production-validation-experiment-6-2026-11-12/calls/${row.run}/${type})\n\n`;md+=hits.length?hits.map(h=>`- L${h.line}: ${h.text}`).join('\n')+'\n':'Geen passend bewijsanker; volledige tekst is beslissend.\n';}}
}
out('traceability.md',md);
const promptIdentity={};for(const f of ['Research.system.prompt.md','Research.user.prompt.md'])promptIdentity[f]={currentEqualsFrozenA:hash(path.join(root,'src/Nueyon.Compose.Application/Prompts/Research',f))===hash(path.join(exp,'prompts/A/Research',f)),compactEqualsExperiment5:hash(path.join(exp,'prompts/B/Research',f))===hash(path.join(root,'PromptExperiments/Research/compact-focused-experiment-5-2026-11-12/prompts/B/Research',f))};out('prompt-identity.json',promptIdentity);
let totals={};for(const arm of ['A','B']){let u=usage.filter(x=>x.run.includes('-'+arm+'-'));totals[arm]={runs:u.length,input:u.reduce((s,x)=>s+x.prompt_tokens,0),output:u.reduce((s,x)=>s+x.completion_tokens,0),total:u.reduce((s,x)=>s+x.total_tokens,0),cached:u.reduce((s,x)=>s+(x.prompt_tokens_details?.cached_tokens||0),0),reasoning:u.reduce((s,x)=>s+(x.completion_tokens_details?.reasoning_tokens||0),0)};}
totals.savingTotalPercent=100*(1-totals.B.total/totals.A.total);totals.savingWeightedI4OPercent=100*(1-(totals.B.input+4*totals.B.output)/(totals.A.input+4*totals.A.output));out('usage-summary.json',totals);
const before=JSON.parse(read(path.join(__dirname,'protected-before.json'))),changed=Object.entries(before).filter(([p,h])=>!fs.existsSync(path.join(root,p))||hash(path.join(root,p))!==h).map(([p])=>p);
out('integrity.json',{protectedCount:Object.keys(before).length,changed,head:cp.execFileSync('git',['rev-parse','HEAD'],{cwd:root,encoding:'utf8'}).trim(),status:cp.execFileSync('git',['status','--short'],{cwd:root,encoding:'utf8'}),matrixRows:rows.length,sourceQuotesValidated:rows.length,checksPassed:checks.every(x=>x.rawContentMatches&&x.projectionMatchesFilterAlgorithm&&x.headings&&x.finish==='stop')});
console.log(JSON.stringify({totals,checks,changed,rows:rows.length},null,2));

