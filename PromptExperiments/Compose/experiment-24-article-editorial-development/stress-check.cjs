const {json,write,output}=require('./run.cjs');
const tasks=json('plan.json').filter(task=>task.phase==='holdout');
const ledger={};
function add(task,claim) {
  const article=json(output(task)+'/result.json').content;
  const narrative=json('fixtures/'+task.subject+'/narrative.json').content;
  if(!article.includes(claim.excerpt)||!narrative.includes(claim.narrative)) throw Error('Evidence mismatch '+output(task));
  ledger[output(task).replace('outputs/','')]=[claim];
}
for(const task of tasks) {
  const article=json(output(task)+'/result.json').content;
  if(task.subject==='wolf') {
    add(task,{label:'F2',category:'entity',excerpt:'elandkalveren en volwassen mannelijke elanden',
      evidence:'in early winter, elk calves comprised 50% of wolf prey, and bull elk comprised 25%',
      narrative:'elandkalveren en volwassen mannelijke elanden',narrativeRelation:'supports',origin:'propagated',
      explanation:'The original concerns Yellowstone elk (wapiti), not the Dutch eland (moose).',
      materiality:'The central prey example identifies the wrong animal.',
      sources:['fixtures/wolf/source.txt','https://www.nps.gov/yell/learn/nature/elk.htm','https://anw.ivdnt.org/article/eland']});
  }
  if(task.subject==='digital') {
    const excerpt=article.match(/Tijdens de private beta wisselden tweewekelijkse sprints[^.]+\./)?.[0];
    add(task,{label:'F2',category:'chronology',excerpt,
      evidence:'splitting our fortnightly sprints in two: testing, researching and accepting applications in the first week',
      narrative:'Tijdens de private beta wisselden tweewekelijkse sprints af tussen aanvragen aannemen en tijd besteden aan testen, onderzoek en verwerking.',
      narrativeRelation:'supports',origin:'propagated',
      explanation:'The Article describes alternating fortnightly sprints or states that sprints alternated between activities. The original describes two weeks within each sprint and testing and research in both weeks.',
      materiality:'Changes the work cadence and obscures overlap in the activities.',
      sources:['fixtures/digital/source.txt','https://mojdigital.blog.gov.uk/2018/03/13/how-to-build-and-release-a-digital-by-default-service-in-12-weeks/']});
  }
  if(task.subject==='faa'&&/twee vormen/i.test(article.split('\n')[0])) {
    add(task,{label:'F2',category:'quantity',excerpt:article.split('\n')[0],
      evidence:'three shapes with two distinct meanings',narrative:'twee soorten hot spots',
      narrativeRelation:'differs',origin:'new_distortion',
      explanation:'The headline changes the count from two categories to two shapes. The three shapes are circle, ellipse and cylinder.',
      materiality:'The number of different shapes is a central distinction in the article.',
      sources:['fixtures/faa/source.txt','https://www.faa.gov/newsroom/hot-spot-standardized-symbology']});
  }
  if(task.subject==='sofar-channel'&&article.split('\n')[0].includes('Across the Ocean')) {
    add(task,{label:'F2',category:'scope',excerpt:article.split('\n')[0],
      evidence:'In some instances, low-frequency sounds can be heard across entire ocean basins.',
      narrative:'in some instances, it can travel across entire ocean basins, helping explain how distant calls can be detected.',
      narrativeRelation:'differs',origin:'new_distortion',
      explanation:'The heading claims cross-ocean transmission for whale calls. The source establishes ocean-basin scale for some low-frequency sound and separately gives distant whale calls as a detection example. It reports whale communication over hundreds and sometimes thousands of miles, without establishing the cross-ocean claim.',
      materiality:'Introduces a specific range claim into the title.',
      sources:['https://oceanservice.noaa.gov/facts/sofar.html']});
  }
  if(task.subject==='webb') {
    const excerpt=article.match(/(?:Daarna volgden weken|Daarna koelde|Eerst volgden weken|De telescoop koelde daarna|De telescoop moest nog weken afkoelen, waarna)[^.]+\./)?.[0];
    if(!excerpt) continue;
    const amplified=/afkoelen, waarna/.test(excerpt);
    add(task,{label:'F2',category:'chronology',excerpt,
      evidence:'NASA places cooldown during sunshield deployment and reports that cooldown effectively ended during the months of optical alignment.',
      narrative:'Daarna volgden weken van afkoeling, maanden van spiegeluitlijning en vervolgens instrumentkalibratie.',
      narrativeRelation:amplified?'differs':'supports',origin:amplified?'amplified':'propagated',
      explanation:amplified?'The Article explicitly places the alignment after weeks of cooling, heightening the sequential reading of the Narrative. NASA documents overlap.':'The Article follows the sequential framing of the Narrative although cooling and deployment or alignment overlap.',
      materiality:'Misstates how commissioning steps fit together in an Article organized around that chronology.',
      sources:['https://science.nasa.gov/mission/webb/deployment/','https://science.nasa.gov/mission/webb/faqs-full/']});
  }
}
require('node:fs').writeFileSync(require('node:path').join(__dirname,'source-audit.json'),JSON.stringify(ledger,null,2)+'\n');

