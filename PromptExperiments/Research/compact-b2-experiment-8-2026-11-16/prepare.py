import pathlib,json,hashlib,subprocess,shutil
r=pathlib.Path.cwd(); e=r/'PromptExperiments/Research/compact-b2-experiment-8-2026-11-16'
def write(p,t):
 p=e/p;p.parent.mkdir(parents=True,exist_ok=True);p.write_text(t,encoding='utf-8')
def js(p,o):write(p,json.dumps(o,ensure_ascii=False,indent=2))
old=r/'PromptExperiments/Research/production-validation-experiment-6-2026-11-12'
for arm in ['A','B','B2']:
 for kind in ['system','user']:
  src=r/f'src/Nueyon.Compose.Application/Prompts/Research/Research.{kind}.prompt.md' if arm=='A' else old/f'prompts/B/Research/Research.{kind}.prompt.md'
  dest=e/f'prompts/{arm}/Research/Research.{kind}.prompt.md';dest.parent.mkdir(parents=True,exist_ok=True);shutil.copyfile(src,dest)
b=(e/'prompts/B/Research/Research.system.prompt.md').read_text()
b=b.replace('Preserve actors, scope, attribution and certainty.', 'Preserve actors, scope, attribution and certainty. For each Idea-relevant quality or outcome, retain material contrasts: successful and failed checks, exceptions and unresolved defects. A passing check does not subsume a failing check of another aspect. For comparisons, preserve the concrete attribution limits: run-to-run variation, allocation, conditions, dates and changed measurement methods when relevant; do not replace them with a generic caution.')
b=b.replace('Use source evidence only.', 'Use source evidence only. Preserve precise technical terms and definitions; do not translate an ambiguous term into a different technical event. Distinguish lack of evidence from evidence of absence. Do not infer event order from document order or status descriptions; give chronology only where explicitly supported.')
b=b.replace('Identify precise relevant gaps or unsupported parts of the Idea.', 'Identify precise relevant gaps or unsupported parts of the Idea. Do not negate known evidence with blanket Unknowns or convert an unverified implication into a negative fact.')
write('prompts/B2/Research/Research.system.prompt.md',b)
cases={
'water':('Een geslaagde kalibratie maakt een waterpilot nog niet betrouwbaar', 'Vergelijk meetkwaliteit, storingen en besluitgrenzen van een sensorpilot zonder de waargenomen daling causaal toe te schrijven.', '''# Larch water telemetry review — synthetic dossier, 30 October 2026
This is a newly constructed evaluation source, not a real utility study. The review concerns twelve pressure loggers at two branches; it is not a drinking-water safety certification. The team calls the pilot encouraging. That is its assessment, not an independently established result.
## Recorded outcomes
The legacy fortnight recorded 46 low-pressure alerts in 1,400 logger-hours; the pilot fortnight recorded 29 in 1,120 logger-hours. The counts are alerts, not confirmed leaks or affected households. A presentation had reported 25 pilot alerts; the final ledger restored four suppressed duplicates of timestamps that were actually distinct device events. Use 29. The report does not estimate a rate-adjusted treatment effect.
The legacy alert threshold was 1.8 bar for 30 seconds; the pilot used 1.6 bar for 60 seconds. Old traces were not reprocessed. A planned night flushing operation occurred during the legacy fortnight only. Branches were selected for ease of access, not randomly; rainfall also differed. No unaffected comparison branch was instrumented. A lower alert count cannot isolate the sensor upgrade's effect.
## Checks and defects
All twelve loggers passed the static reference-gauge calibration at 2.0 bar. Four nevertheless failed the overnight packet-delivery check, losing 17% of scheduled messages in that check. The pressure readings that arrived were not shown to be inaccurate. The delivery fault remained open at review; a proposed antenna move had not been tested. A battery replacement was completed on two different devices, but the dossier provides no date linking that work to the delivery check. Do not treat the battery status as a repair of the packet fault.
A bench check recorded three brownouts: transient supply-voltage drops causing device restarts, not a regional power outage. A capacitor swap and firmware update were both made before a repeat bench check with no brownouts. Their separate contributions were not identified; the bench result does not certify field uptime.
## Repeats and decision
Three replays of the same stored trace emitted 29, 31 and 28 alerts; a nondeterministic deduplication tie-breaker was enabled. The team did not select one replay as the true count. These are replay counts and do not replace the final field ledger of 29. Repeat variability and changed thresholds limit attribution.
The committee authorized a six-week paired measurement study with unchanged thresholds. It did not authorize network-wide deployment. The budget of EUR 18,000 covers logger rental and field visits, excludes staff overtime and does not establish savings. No safety, leak-prevention or annual reliability benefit was measured. Training attendance was 10 of 12 technicians, but attendance was not a competence test.
'''),
'language':('Correcte woorden zijn nog geen toegankelijke dienstverlening', 'Onderzoek een meertalig formulier via vertaaljuistheid, taaktoegang, meetgrenzen en een beperkt vervolgadvies.', '''# Vale civic-language form review — synthetic audit note
This newly constructed source describes a fictional benefits form. It contains no real resident data. The title used by management, an inclusive launch, states an ambition rather than a measured inclusion outcome. No general accessibility certification was commissioned.
## Sample and linguistic checks
Reviewers sampled 80 of 640 message strings: 40 Spanish and 40 Polish. All 80 retained the glossary's approved entitlement terms. Six Spanish strings nevertheless reversed the meaning of the deadline exception. The Polish sample had no deadline reversal. Terminology conformity and semantic correctness were separate checks; neither result certifies all 640 strings. A second reviewer checked the six reversals and agreed. The remaining strings did not receive that second semantic review.
## Access and task results
An automated colour-contrast scan passed the three tested page templates. Keyboard-only testing could not activate the document-upload button on the second template. The upload defect was still open. Screen-reader testing announced the upload label correctly on that template, but did not test keyboard activation. A visually correct label is not evidence that a keyboard user can upload a document. Management approved a keyboard fix proposal; no deployed fix is recorded.
Task completion in the old English-only service was 33 of 50 moderated sessions. The multilingual form recorded 42 of 50 unmoderated sessions. Completion formerly required a final submitted application; the new dashboard counted reaching the review screen. Participants were recruited from different channels and neither assigned randomly nor matched for prior experience. The dashboard's apparent improvement is not a comparable completion-effect estimate.
## Processing and reproducibility
A preprocessing step called case folding normalised letter case; it did not merge benefit cases or applications. Three runs on the same 80 strings flagged six, five and six semantic issues with a sampling-based checker. A human reference review identified the six deadline reversals. Automated run counts do not establish three different ground truths or a stable checker. The checker version was held fixed, but its sample order varied.
## Status and decision boundary
The appendix says legal approval covers the Spanish consent paragraph only. It gives no approval date or temporal relationship to the sample review. Polish consent approval is not documented. Translation availability does not itself establish legal approval. The coordinator authorised a corrected-form usability study, conditional on a verified upload fix, and withheld general release. The study budget includes translation fees but excludes staff time; no cost saving is demonstrated.
A separate satisfaction poll received 18 responses from 90 invitations. Sixteen respondents said wording was clearer. Nonresponse and self-selection prevent treating that fraction as population satisfaction. The poll did not measure successful submission or uptake of benefits. Management's inclusive-launch phrase should not be repeated as a finding.
'''),
'heat':('Warmteopslag toont proefwinst met duidelijke grenzen', 'Behoud prestaties én mislukte controles, onderscheiden technische processen en beperkingen van een energievergelijking.', '''# Oriel thermal-store trial — synthetic engineering memorandum
This is a fictional engineering dossier newly constructed for prompt evaluation. It records a two-shift factory trial of a phase-change thermal store. It is not a safety approval or annual energy model. The supplier's phrase dependable low-carbon heat is a marketing interpretation, not a measured emissions conclusion.
## Performance and measurement
The final metered record shows 410 kWh of useful heat from 500 kWh electrical input over the trial boundary. A draft listed 450 kWh useful heat; 40 kWh was duplicated during meter-file concatenation and removed. The 410/500 ratio concerns this boundary and period, not seasonal efficiency. The legacy boiler's displayed 76% used fuel energy on a lower-heating-value basis, while the electrical trial used meter input and included pump demand. No common-boundary reanalysis exists.
Trial shifts processed lighter products and ambient temperature was 8 degrees higher than in the boiler reference shifts. Operators selected the trial shifts to avoid peak production. There was no random assignment or matched control. The record cannot isolate a storage-technology effect or quantify avoided emissions; grid-carbon intensity was not measured.
## Successful and failed controls
All four vessels passed the specified pressure-hold test. Two nevertheless failed the insulation surface-temperature check at the access hatch. The hot surfaces remained unresolved at sign-off, and a proposed shield was not installed. Passing the pressure test does not demonstrate acceptable touch temperature. An interlock test passed for all four vessels; it did not test the hot-surface problem.
Three thermal trips occurred during initial charging. Here a thermal trip is a protective over-temperature shutdown, not a production journey or heat-delivery cycle. A controller setting and sensor mounting were changed together before a repeat charge with zero trips. The record cannot separate the two actions' contributions. Zero trips in one repeat does not establish long-term reliability.
## Variability, sequencing and decision
Three identical-input simulation runs estimated usable discharge at 402, 418 and 407 kWh; the solver used stochastic initialisation. Those simulations are not three additional metered field trials and do not supersede the measured 410 kWh. Solver variation is an attribution limit for interpreting simulation improvements.
The finance appendix reports a EUR 9,200 rental allowance. It gives no date or sequence relative to testing. The allowance excludes installation, downtime and maintenance, so no payback claim is supported. Procurement approved only a longer instrumented trial conditional on a verified hatch remedy; no purchase or factory-wide roll-out was approved.
An operator survey had seven responses from twelve operators, five preferring the quieter charging shift. Noise levels were not instrumented and trial workloads differed. The supplier's emissions phrase is not validated by this preference. The record reports that a sensor serial number was corrected in the asset register; it does not say the physical sensor was replaced.
''')}
labels={}
# Each exact quoted sentence is a prospectively mandatory material unit; context is explicitly separate.
anchors={
'water':[
('ledger','kern','The legacy fortnight recorded 46 low-pressure alerts in 1,400 logger-hours; the pilot fortnight recorded 29 in 1,120 logger-hours.'),
('method','kern','The legacy alert threshold was 1.8 bar for 30 seconds; the pilot used 1.6 bar for 60 seconds.'),
('conditions','belangrijk','A planned night flushing operation occurred during the legacy fortnight only.'),
('allocation','belangrijk','Branches were selected for ease of access, not randomly; rainfall also differed.'),
('contrast','kern','All twelve loggers passed the static reference-gauge calibration at 2.0 bar. Four nevertheless failed the overnight packet-delivery check, losing 17% of scheduled messages in that check.'),
('open','kern','The delivery fault remained open at review; a proposed antenna move had not been tested.'),
('chronology','belangrijk','A battery replacement was completed on two different devices, but the dossier provides no date linking that work to the delivery check.'),
('term','kern','A bench check recorded three brownouts: transient supply-voltage drops causing device restarts, not a regional power outage.'),
('attribution','kern','A capacitor swap and firmware update were both made before a repeat bench check with no brownouts. Their separate contributions were not identified; the bench result does not certify field uptime.'),
('variation','kern','Three replays of the same stored trace emitted 29, 31 and 28 alerts; a nondeterministic deduplication tie-breaker was enabled.'),
('decision','kern','The committee authorized a six-week paired measurement study with unchanged thresholds. It did not authorize network-wide deployment.'),
('cost','belangrijk','The budget of EUR 18,000 covers logger rental and field visits, excludes staff overtime and does not establish savings.')],
'language':[
('sample','kern','Reviewers sampled 80 of 640 message strings: 40 Spanish and 40 Polish.'),
('contrast','kern',"All 80 retained the glossary's approved entitlement terms. Six Spanish strings nevertheless reversed the meaning of the deadline exception. The Polish sample had no deadline reversal."),
('review','belangrijk','A second reviewer checked the six reversals and agreed. The remaining strings did not receive that second semantic review.'),
('access','kern','An automated colour-contrast scan passed the three tested page templates. Keyboard-only testing could not activate the document-upload button on the second template.'),
('open','kern','The upload defect was still open. Screen-reader testing announced the upload label correctly on that template, but did not test keyboard activation.'),
('outcome','kern','Task completion in the old English-only service was 33 of 50 moderated sessions. The multilingual form recorded 42 of 50 unmoderated sessions.'),
('method','kern','Completion formerly required a final submitted application; the new dashboard counted reaching the review screen.'),
('allocation','belangrijk','Participants were recruited from different channels and neither assigned randomly nor matched for prior experience.'),
('term','kern','A preprocessing step called case folding normalised letter case; it did not merge benefit cases or applications.'),
('variation','kern','Three runs on the same 80 strings flagged six, five and six semantic issues with a sampling-based checker. A human reference review identified the six deadline reversals.'),
('legal','kern','The appendix says legal approval covers the Spanish consent paragraph only. It gives no approval date or temporal relationship to the sample review. Polish consent approval is not documented.'),
('decision','kern','The coordinator authorised a corrected-form usability study, conditional on a verified upload fix, and withheld general release.')],
'heat':[
('ledger','kern','The final metered record shows 410 kWh of useful heat from 500 kWh electrical input over the trial boundary.'),
('method','kern',"The legacy boiler's displayed 76% used fuel energy on a lower-heating-value basis, while the electrical trial used meter input and included pump demand. No common-boundary reanalysis exists."),
('conditions','belangrijk','Trial shifts processed lighter products and ambient temperature was 8 degrees higher than in the boiler reference shifts.'),
('allocation','belangrijk','Operators selected the trial shifts to avoid peak production. There was no random assignment or matched control.'),
('contrast','kern','All four vessels passed the specified pressure-hold test. Two nevertheless failed the insulation surface-temperature check at the access hatch.'),
('open','kern','The hot surfaces remained unresolved at sign-off, and a proposed shield was not installed.'),
('interlock','belangrijk','An interlock test passed for all four vessels; it did not test the hot-surface problem.'),
('term','kern','Three thermal trips occurred during initial charging. Here a thermal trip is a protective over-temperature shutdown, not a production journey or heat-delivery cycle.'),
('attribution','kern',"A controller setting and sensor mounting were changed together before a repeat charge with zero trips. The record cannot separate the two actions' contributions."),
('variation','kern','Three identical-input simulation runs estimated usable discharge at 402, 418 and 407 kWh; the solver used stochastic initialisation.'),
('cost','belangrijk','The finance appendix reports a EUR 9,200 rental allowance. It gives no date or sequence relative to testing. The allowance excludes installation, downtime and maintenance, so no payback claim is supported.'),
('decision','kern','Procurement approved only a longer instrumented trial conditional on a verified hatch remedy; no purchase or factory-wide roll-out was approved.')]
}
for c,(title,desc,source) in cases.items():
 idea=dict(Title=title,Description=desc,Audience='Professionals die operationele proefresultaten en besluitvorming beoordelen.',Rationale='Een positieve deelcontrole of ruwe vergelijking is onvoldoende voor een brede kwaliteitsclaim.',Evidence='Verifieer de gerapporteerde uitkomsten, geslaagde en mislukte controles, technische definities, variatie, attributiegrenzen en het beperkte besluit tegen de bron.')
 write(f'cases/{c}/source.md',source);js(f'cases/{c}/input.json',{'Input':{'Content':source},'SelectedIdea':{'Idea':idea}})
 labels[c]=[dict(id=i,materiality=m,sourceQuote=q) for i,m,q in anchors[c]]
 for l in labels[c]:assert l['sourceQuote'] in source
js('labels.json',labels)
schedule=[]
for rep in range(1,4):
 for ci,c in enumerate(cases):
  arms=['A','B','B2'];shift=(rep-1+ci)%3;arms=arms[shift:]+arms[:shift]
  for arm in arms:schedule.append(dict(id=f'{c}-r{rep}-{arm}',case=c,arm=arm))
js('plan.json',dict(model='gpt-6-luna',maxCalls=27,maxOutputTokens=5000,maxRequestBytes=120000,retries=0,schedule=schedule))
write('rubric.md','''# Vooraf vastgelegde rubric
Per output: brontrouw, dekking, nuances/contrasten, attributie, epistemische precisie, Facts/Unknowns-consistentie en feitelijke bruikbaarheid van de echte projectie. Elk 0 (materieel onveilig/onvolledig), 1 (concreet gebrek, voorwaardelijk), 2 (voldoende). Geen compenserende som. Voldoende = alle dimensies 2, geen ontbrekend/deels materieel label; context is niet verplicht. Kernomissie geeft dekking/bruikbaarheid 0; belangrijk gemis geeft 1. Een gedeeltelijke formulering telt alleen als correct als claim en benodigde grens beide behouden zijn. Bekijk volledige Research EN projectie; alleen projectie telt als downstreambewijs. Geen uitvoer van Synthesis.
Traceability: per bevroren label correct/deels/ontbreekt/ongefundeerd, exacte bronquote en feitelijke outputregels. Geen verzonnen afwezigheidscitaten. Daarnaast output-naar-broncontrole voor verzinsels, brede Unknowns, verkeerde termen, ongefundeerde chronologie en tegenstrijdigheid. Context zoals enquêtes/training is alleen verplicht bij een claim die erop steunt. Herhaalbaarheid: successen per case/arm en terugkerende/variabele defecten over drie repeats.
GO B2 voor onafhankelijke vervolgvalidatie, niet promotie: minstens 8/9 voldoende, per case minstens 2/3; minstens evenveel successen als A en B; geen herhaalde kernfout of materiële regressie tegenover voldoende A; minstens 20% minder I+O en I+4O dan A, ook per voldoende output. Anders NO-GO. Research afgerond alleen als deze poort slaagt EN onafhankelijke beoordeling/natuurlijke bronnen voldoende steun geven; deze synthetische zelfbeoordeling alleen rechtvaardigt geen afronding. Nul successen: kosten per succes ongedefinieerd, geen delen door alle generaties. Labels worden niet na uitkomsten aangepast. Sensitiviteit: rapporteer kern-only succes naast strikte volledige labels, zonder de GO-poort te veranderen.
''')
write('protocol.md','''# Experiment 8 — prospectief Research-only A/B/B2
16 november 2026. Nieuw geïsoleerd experiment. Huidige productie A, bytegelijke compacte B uit Experiment 5/6, één generieke B2: alleen systeemtekst uitgebreid voor contrasten, concrete attributiebeperkingen, technische termen, epistemische grenzen en expliciete chronologie. B2-userprompt blijft B. Geen bron-specifieke regels, Judge, Evidence Mapper, nieuwe agent, retrieval of Copilot CLI. Echte ResearchAgent zonder judge en echte SynthesisResearchFilter; geen andere agents uitvoeren. Idea staat vooraf vast.
Drie nieuwe synthetische dossiers (watertelemetrie, meertalige dienstverlening, thermische opslag), drie repeats per arm = 27 calls. Geen echte veldstudies; auteur van bronnen, labels, kandidaat en beoordeling is dezelfde Work-assistent. Foutfamilies zijn geïnformeerd door Experiment 7; geen onafhankelijke holdout of echte blindering. Beoordeling gebeurt in Work, geen extra beoordelingscalls. Geen statistische non-inferioriteit claimen. Alle labels, rubric, prompts, cases, budget, schema, runner en DLLs krijgen hashes vóór calls. Hashcontrole vóór elke call; exclusieve globale lock/attemptmarkers; retries 0; maximaal één HTTP per poging, geen vervanging of reparatie. 27 requests, 5000 outputtokens elk (135000 worst-case); 120000 UTF-8 requestbytes per call, geen exacte inputtoken-equivalentie. Geen betrouwbaar Luna-factuurtarief: geen fictief geldplafond; werkelijke usage en conditionele kostenformule rapporteren. I+4O is uitsluitend sensitiviteit. Redenering niet dubbel tellen; cachevelden bewaren.
Interleaved cyclische armvolgorde over cases/repeats. Zelfde bron en vijf Idea-velden, nieuwe sessie/client per call, alleen promptarm verschilt; geen seed/temperatuurwijziging. Technische fouten tellen als onvoldoende en krijgen geen retry; budget niet hergebruiken. Logging bewaart requests zonder authheaders, ruwe responses, output en echte projectie. Na afloop offline parser/filterreplay, requestcontrole, freeze en beschermde bestanden bytecontrole. Geen productiecode/config/prompts of eerdere experimenten wijzigen, geen commit/push/promotie. Menselijke labelvalidatie is hier niet beschikbaar: beperking expliciet melden.
Context gelezen: Experiment 5 report/protocol, Experiment 6 report/protocol, Experiment 7 report/protocol/traceability. Experiment 7 corrigeert downstreamverlies dat ten onrechte aan Research was toegeschreven; huidige proef beoordeelt uitsluitend de Research-informatie en daadwerkelijke projectie.
''')
# Retain proven capture/replay mechanics, remove all other-agent code paths before compiling.
s=(old/'runner/Program.cs').read_text()
for line in ['using Nueyon.Compose.Application.Agents.Narrative;\n','using Nueyon.Compose.Application.Agents.Compose;\n']:s=s.replace(line,'')
a=s.index('  object result=stage switch{');z=s.index('  if(JsonSerializer.Serialize(result',a)
s=s[:a]+'''  if(stage!="Research")throw new Exception("Research only");
  object result=await new ResearchAgent(client.AsAIAgent("offline"),NullLogger<ResearchAgent>.Instance).ExecuteAsync(context,JsonSerializer.Deserialize<ResearchInput>(input,json)!);
'''+s[z:]
a=s.index('if(mode!="run"');z=s.index('async Task<T> Call<T>',a)
s=s[:a]+'''if(mode!="run")throw new Exception("run/replay only");
using var runLock=new FileStream(Path.Combine(output,"run.lock"),FileMode.CreateNew,FileAccess.Write,FileShare.None);
using var secrets=JsonDocument.Parse(File.ReadAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),"Microsoft/UserSecrets/01058711-e286-4f69-9558-dc678f32599d/secrets.json")));
var sr=secrets.RootElement;var key=sr.TryGetProperty("OpenAI:ApiKey",out var flat)?flat.GetString():sr.GetProperty("OpenAI").GetProperty("ApiKey").GetString();
using var plan=JsonDocument.Parse(File.ReadAllText(Path.Combine(output,"plan.json")));
foreach(var lane in plan.RootElement.GetProperty("schedule").EnumerateArray()){
 var id=lane.GetProperty("id").GetString()!;var caseId=lane.GetProperty("case").GetString()!;var arm=lane.GetProperty("arm").GetString()!;
 using var promptScope=PromptDirectory.Use(Path.Combine(output,"prompts",arm));
 var input=Load<ResearchInput>($"cases/{caseId}/input.json");
 try{
  var r=await Call<ResearchResult>(id,"Research",input,ResearchAgent.GetSystemInstructions(),async(ai)=>await new ResearchAgent(ai,NullLogger<ResearchAgent>.Instance).ExecuteAsync(Context(),input));
  CheckResearch(r);
  File.WriteAllText(Path.Combine(output,"calls",id+"-Research","projection.md"),SynthesisResearchFilter.ExtractFactsAndUnknowns(r.Content));
 }catch(BudgetException){throw;}catch(Exception ex){Save(output,id+"-failure.json",new{id,error=ex.ToString()});Console.WriteLine(id+": failed; no retry");}
}
VerifyFreeze();Console.WriteLine("Completed; total attempts="+Directory.GetFiles(Path.Combine(output,"calls"),"attempt-started.json",SearchOption.AllDirectories).Length);
'''+s[z:]
s=s.replace('all>=54','all>=27').replace('Global 54-attempt cap','Global 27-attempt cap').replace('6000','5000').replace('180000','120000')
write('runner/Program.cs',s)
shutil.copyfile(old/'runner/Runner.csproj',e/'runner/Runner.csproj') if (old/'runner/Runner.csproj').exists() else shutil.copyfile(next((old/'runner').glob('*.csproj')),e/'runner/Runner.csproj')
write('head.txt',subprocess.check_output(['git','rev-parse','HEAD'],text=True).strip()+'\n')
tracked=subprocess.check_output(['git','ls-files','-z']).decode().split('\0')
js('protected-before.json',{p:hashlib.sha256((r/p).read_bytes()).hexdigest() for p in tracked if p and (r/p).is_file()})
print('Prepared',len(schedule),'calls;',len(labels),'new cases;',sum(map(len,labels.values())),'material labels')
