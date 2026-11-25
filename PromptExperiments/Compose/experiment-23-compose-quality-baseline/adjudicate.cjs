const fs=require('node:fs');
const {at,read,json,tasks,exists,out}=require('./run.cjs');
const {readReview}=require('./review-records.cjs');
const findings=[];
const refs={
  digital:'https://mojdigital.blog.gov.uk/2018/03/13/how-to-build-and-release-a-digital-by-default-service-in-12-weeks/',
  wolf:'https://www.nps.gov/yell/learn/nature/wolves.htm',
  webb:'https://science.nasa.gov/mission/webb/deployment/',
  acid:'https://www.epa.gov/acidrain/acid-rain-program'
};
const exact=(text,regex)=>text.match(regex)?.[0];
function add(t,fields){
  const match=fields.excerpt;
  if(!match)throw Error('Missing output excerpt '+t.folder+'/'+t.id);
  findings.push({output:t.folder+'/'+t.id,source:'fixtures/'+t.subject+'/source.txt',
    reference:refs[t.subject]??null,...fields});
}
for(const t of tasks){
  if(!exists(out(t)+'/result.json'))continue;
  const content=json(out(t)+'/result.json').content;
  const narrative=json('fixtures/'+t.subject+'/narrative.json').content;
  const source=read('fixtures/'+t.subject+'/source.txt');
  if(t.subject==='wolf'){
    add(t,{label:'F2',category:'entity',origin:'propagated',
      excerpt:exact(content,/elandkalveren en volwassen mannelijke elanden/),
      evidence:exact(source,/elk calves comprised 50% of wolf prey, and bull elk comprised 25%/),
      narrative:exact(narrative,/elandkalveren en volwassen mannelijke elanden/),
      explanation:'North American elk is wapiti (Cervus canadensis). Dutch eland means moose (Alces alces). Compose carried the wrong species into the central historical prey comparison.',
      materiality:'High. The prey species in the central wildlife observation changes.'});
  }
  if(t.subject==='digital'){
    add(t,{label:'F2',category:'chronology',origin:'propagated',
      excerpt:exact(content,/Tijdens de private beta wisselden tweewekelijkse sprints[^.]*\./),
      evidence:exact(source,/splitting our fortnightly sprints in two:[^.]*\./),
      narrative:exact(narrative,/Tijdens de private beta wisselden tweewekelijkse sprints[^.]*\./),
      explanation:'The source split each sprint into two weeks and kept testing and research in both. The draft puts the alternation at the level of fortnightly sprints. Nearby sentences in some versions also imply that intake and testing did not overlap.',
      materiality:'High. This changes the operating rhythm the story sets out to explain.'});
  }
  if(t.subject==='webb'){
    const excerpt=exact(content,/De telescoop koelde daarna wekenlang af[.;]/) ??
      exact(content,/Daarna volgden weken van afkoeling[^.]*\./) ??
      exact(content,/Tijdens de ingebruikname van zes maanden koelde Webb eerst wekenlang af\. Daarna duurden de spiegeluitlijning maanden/);
    if(excerpt)add(t,{label:'F2',category:'chronology',origin:'propagated',excerpt,
      evidence:exact(source,/Note the major changes in the area marked as days 3-10 when the sunshield was deployed\./)
        +' '+exact(source,/Cooldown effectively ended and the cryocooler started running at its lowest temperature/),
      narrative:exact(narrative,/In de tweede week waren de spiegels[^.]*\. Daarna volgden weken van afkoeling[^.]*\./),
      explanation:'NASA describes cooldown during deployment and into optics work. The Article 1/2 and LinkedIn 3 wording places cooling after deployment; Article 3 constructs successive cooldown and alignment stages. Either reading removes a documented overlap from a story about commissioning chronology.',
      materiality:'Material. The distinction between deployment and science readiness remains, but the relationship between stages changes.'});
  }
  if(t.folder==='linkedin'&&t.id==='acid-1'){
    add(t,{label:'F2',category:'technical',origin:'new_distortion',excerpt:'a permitted total that could be traded',
      evidence:exact(source,/The sulfur dioxide mechanism applies an aggregate cap and allows trading; one allowance covers one ton\./),
      narrative:exact(narrative,/under an aggregate cap with trading/),
      explanation:'The relative clause makes the aggregate emissions limit itself tradable. The EPA framework trades unit allowances beneath a cap. The Narrative contains both concepts but does not attach tradability to the cap.',
      materiality:'Material. This changes how the central policy mechanism operates.'});
  }
}
const resolutions={
  coral:{test:/record|satellite/i,label:'F0',evidence:'NOAA calls this the largest event to date, identifies the former record, and describes the satellite-based product.'},
  labor:{test:/brows|job listings/i,label:'F0',evidence:'BLS gives browsing postings without further action as a passive job search method.'},
  python:{test:/thread.safe|separate responsibilities/i,label:'F0',evidence:'The official guide treats runtime support declaration and implementation thread safety as separate tasks.'},
  acid:{test:/Title IV|allowances traded among/i,label:'F0',evidence:'EPA identifies Title IV as establishment authority and documents allowance trading among regulated sources.'},
  fusion:{test:/delivered|significant|facility|grid/i,label:'F0',evidence:'DOE links target delivery to fusion output and qualifies its scientific significance. The target-energy comparison excludes facility accounting and does not measure grid electricity.'},
  highways:{test:/not a single act completing/i,label:'F0',evidence:'The archive distinguishes legislation authorizing network construction from completed construction.'}
};
for(const t of tasks){
  if(!exists(out(t)+'/result.json'))continue;
  const key=t.folder+'/'+t.id;
  const seen=new Set();
  for(const pass of [1,2]){
    const review=readReview(t,'factual',pass);
    for(const c of review?.data?.claims??[]){
      if(!['F2','U'].includes(c.label)||!c.exactExcerpt||seen.has(c.exactExcerpt))continue;
      seen.add(c.exactExcerpt);
      const confirmed=findings.find(f=>f.output===key&&f.label==='F2'&&
        (f.excerpt.includes(c.exactExcerpt)||c.exactExcerpt.includes(f.excerpt)||
         (t.subject==='digital'&&/ene periode/.test(c.exactExcerpt))||
         (t.subject==='wolf'&&/eland/.test(c.exactExcerpt))));
      if(confirmed)continue;
      const resolution=resolutions[t.subject];
      let label='U';
      let explanation='The available evidence or phrasing leaves the reviewer’s flagged inference unresolved.';
      if(resolution?.test.test(c.exactExcerpt)){
        label=resolution.label;
        explanation=resolution.evidence;
      }
      if(t.subject==='digital'&&/twaalf weken/.test(c.exactExcerpt)){
        label='F0';explanation='The original author reports a public launch in twelve weeks.';
      }
      if(t.subject==='digital'&&/het stelt op zichzelf niet vast/.test(c.exactExcerpt)){
        label='F0';explanation='The source reports the team’s causal interpretation, but the piece only says this account does not itself establish causation.';
      }
      add(t,{label,category:c.category,origin:label==='U'?'unknown':'none',excerpt:c.exactExcerpt,
        evidence:c.evidence||explanation,narrative:c.narrative||null,explanation,reviewPass:pass,reviewLabel:c.label});
    }
  }
}
fs.writeFileSync(at('reviews/factual/adjudications.json'),JSON.stringify(findings,null,2)+'\n');
