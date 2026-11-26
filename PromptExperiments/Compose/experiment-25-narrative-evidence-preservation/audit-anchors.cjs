const fs=require('node:fs');
process.chdir(__dirname);
const read=f=>JSON.parse(fs.readFileSync(f,'utf8'));
const plan=read('plan.json');
const regressions=read('regression-anchor-excerpts.json');
const verified=[];
const source={
 wolf:'NPS Wolf Ecology, Changes in Their Prey: early-winter elk calves and bull elk proportions reversed. NPS Elk identifies the Yellowstone species as wapiti (Cervus canadensis). ANW identifies Dutch eland as Alces alces. https://home.nps.gov/yell/learn/nature/wolf.htm | https://www.nps.gov/yell/learn/nature/elk.htm | https://anw.ivdnt.org/article/eland',
 webb:'NASA Webb launch media kit: cooling begins during deployment following sunshield deployment. NASA Deployment puts completion of telescope-structure deployment in week two. https://science.nasa.gov/wp-content/uploads/2024/08/webbmediakit.pdf | https://science.nasa.gov/mission/webb/deployment/',
 eia:'EIA 2024 table: 42.4 of 605.6 quadrillion Btu. US quadrillion here is 10^15; Dutch quadriljoen is 10^24. https://www.eia.gov/tools/faqs/faq.php?id=527&t=4 | https://www.vlaanderen.be/team-taaladvies/taaladviezen/verhoudingen-bij-maten-en-gewichten'
};
function mark(r,excerpt,origin,synthesisStatement,narrativeStatement,reason){
  if(!excerpt||!read(r.file).content.includes(excerpt))throw Error('Missing exact operator excerpt: '+r.file);
  const stages={narratives:'narrative',articles:'article',linkedin:'linkedin'};
  verified.push({id:r.id,subject:r.subject,phase:r.phase,stage:stages[r.stage],condition:r.c,rep:r.rep,
    excerpt,sourceEvidence:source[r.subject],synthesisStatement,narrativeStatement,origin,decision:'F2',materiality:'material',reason});
}
const wolfPattern=/\belanden?\b|elandkalv/i;
const wolfImpact=/effecten.*(?:op elanden)|gevolgen.*(?:voor elanden)/i;
const wolfSynthesis='The composition of wolf prey changed over time: the proportion of early-winter prey made up by elk calves and bull elk reversed between 1995–2000 and 2001–2007. The source links such changes to prey vulnerability and availability.';
for(const r of regressions.filter(x=>x.subject==='wolf')){
  if(r.stage==='narratives'&&r.c==='C')continue;
  const statement=r.excerpts.find(x=>wolfImpact.test(x))??r.excerpts.find(x=>wolfPattern.test(x));
  if(!statement)continue;
  const n=regressions.find(x=>x.subject==='wolf'&&x.stage==='narratives'&&x.c===r.c);
  const narrativeStatement=n.excerpts.find(x=>wolfImpact.test(x))??n.excerpts.find(x=>/elk calves and bull elk/i.test(x));
  mark(r,statement,r.stage==='narratives'?'narrative':r.c==='C'?'compose':'propagated',wolfSynthesis,narrativeStatement,
    'Dutch eland is a different species. This changes the actor in a substantive ecological relationship.');
  if(r.stage==='articles'&&r.c==='B'){
    const changed=r.excerpts.find(x=>/elandkalv/i.test(x));
    const original=n.excerpts.find(x=>/elk-kalveren/i.test(x));
    if(changed&&original&&changed!==statement)mark(r,changed,'amplification',wolfSynthesis,original,
      'The handoff keeps elk in the prey-age comparison but uses eland for the related population impact. The Article extends that inconsistent translation into the prey-age comparison.');
  }
}
const webb=regressions.find(r=>r.subject==='webb'&&r.stage==='narratives'&&r.c==='A');
const timing='Daarna volgden gedurende de ingebruikname afkoeling, spiegeluitlijning en kalibratie.';
mark(webb,timing,'narrative','In the first month, the telescope and instruments cooled for several weeks. Mirror alignment took place in months two through four, followed by instrument-mode calibration in months five and six.',
  timing,'The antecedent of Daarna is the second-week mirror deployment milestone. The ordering implies cooling followed that milestone although it began during deployment.');
const eia=regressions.find(r=>r.subject==='eia'&&r.stage==='articles'&&r.c==='C'&&r.rep===1);
mark(eia,eia.excerpts.find(x=>/quadriljoen Btu/i.test(x)),'compose',
  'Across all energy production, renewables were 42.4 out of 605.6 quadrillion Btu, reported as 7%.',
  'Across all energy production, renewables were 42.4 out of 605.6 quadrillion Btu, reported as 7%.',
  'The Dutch magnitude word denotes a quantity one billion times larger. The percentage could remain identical because both numerator and denominator are mistranslated.');
fs.writeFileSync('operator-anchor-findings.json',JSON.stringify(verified,null,2)+'\n');
