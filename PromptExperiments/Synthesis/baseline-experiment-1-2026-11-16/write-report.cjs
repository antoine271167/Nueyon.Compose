const fs=require('fs'),p=require('path');const e='PromptExperiments/Synthesis/baseline-experiment-1-2026-11-16',read=f=>JSON.parse(fs.readFileSync(p.join(e,f),'utf8')),freeze=read('freeze.json'),scores=read('assessments.json').runs,results=read('results.json'),usage=read('usage.json');const line=(f,pat)=>{const a=fs.readFileSync(p.join(e,f),'utf8').split(/\r?\n/);const i=a.findIndex(x=>pat.test(x));return {line:i+1,text:a[i]}};const actor=line('calls/digital-r1-Synthesis/content.md',/Nuëyons/),originalActor=line('cases/digital/source.md',/I’m the Product Manager/),contrast=line('cases/lowell/projection.md',/technological gains/),sourceContrast=line('cases/lowell/source.md',/Technological advances/);let report=`# Synthesis Experiment 1 — Baseline

16 november 2026. **Baseline: verbeterpunten; geen GO.** Acht van twaalf outputs zijn volledig voldoende onder de vooraf bevroren rubric. Eén digitale-dienstoutput bevat een verkeerde actor in de Narrative-instructie; alle drie Lowell-outputs laten het vooraf vereiste productie/werkgelegenheidscontrast weg. De centrale verhalen blijven verder herkenbaar en de outputs bevatten bruikbare inhoud. Geen bewijs dat de hele Synthesis-agent onbruikbaar is, wel onvoldoende voor acceptatie onder dit plan.

Alleen de bestaande SynthesizerAgent is live uitgevoerd: vier vaste cases × drie herhalingen = **12 requests met gpt-6-luna**. Geen Idea-, Research-, Narrative- of Compose-generatie, Judge, Evidence Mapper, extra evaluatiemodel, retries of repairs. Geen promptoptimalisatie, productieaanpassing, promotie, cleanup, Copilot CLI, commit of push.

## Actuele productie en historie

Lokale repo C:\\Workspaces\\Nueyon.Compose; HEAD ${freeze.head}, origin antoine271167/Nueyon.Compose. Beginstatus schoon. Remote niet opnieuw geverifieerd; conclusies betreffen deze lokale versie. De gekoppelde chat is gelezen als context.

Geïnspecteerd: domeincontracten SynthesisInput(Idea SelectedIdea, ResearchForSynthesis Research), SynthesisResult(string Content), CLI-contract, SynthesizerAgent, productie-system/userprompt, PromptLoader/PromptDirectory, Infrastructure-registratie, OpenAiAgentFactory/AgentFactory en StoryWorkflow. StoryWorkflow regels 183–184 voert ExtractFactsAndUnknowns uit vóór Synthesis. De echte agent rendert de vijf Idea-velden plus ResearchContent en configureert de bestaande JSON-schemaresponse SynthesisResult; de echte parser controleert de niet-lege Content. Geen eigen vervangende generatieprompt of parser.

Het bestaande filter ontvangt volledige Research en geeft alleen Facts en Unknowns door; Interpretations, Development sequence en Editorial relevance vallen weg. Geen oorspronkelijke brontekst of beoordelingslabels toegevoegd aan modelrequests. Het model kent dus geen aanvullende bron die het selectieverlies later zou kunnen herstellen.

Eerdere lessen, vóór deze calls geraadpleegd: [Research Experiment 6](../../Research/production-validation-experiment-6-2026-11-12/report.md) signaleert reeds Nuëyon-specifieke Synthesis-guidance en een vergelijkbare verkeerde instructie in Archive B-r1; plausibel productierisico, geen bewezen causaliteit. [Experiment 7](../../Research/offline-reassessment-experiment-7-2026-11-16/report.md) corrigeert toeschrijving van bus-audioverlies: Research en projectie bevatten die feiten al. Daarom beoordelen we deze agent los met vaste inputs. De historische samenvatting van Narrative Experiment 011 meldt ongeldige runs door stale embedded prompts en geen afgeronde geldige 3+3-conclusie. Die oude bestanden zijn niet in de huidige boom gevonden; dit is indirect historisch bewijs, geen opnieuw gevalideerde proef. De aanvullend na generatie gelezen samenvatting van Experiment 010 meldt variabele eerste drift over meerdere stages en het verschil tussen brontrouw en vlakke inhoud. Ze verandert geen prospectieve criteria of scores.

## Vaste gevallen en inputintegriteit

| Case | Selected Idea | Gekozen opgeslagen Research | Waarom representatief |
|---|---|---|---|
| Webb | Uitgevouwen is nog niet klaar: de lange weg naar Webbs eerste wetenschap | webb-r1-B2-Research/result.json | Ontwerp, risico en verschillende ingebruiknamefasen |
| Wolf | Een wolf is geen losse schakel: Yellowstone als levend netwerk | wolf-r1-B2-Research/result.json | Sociale relaties, veranderende prooi, multicausaliteit |
| Digital | Snel digitaal leveren begon hier met keuzes, niet met een formulier | digital-r1-B2-Research/result.json | Praktijkervaring, gebruikersvolgorde, concrete planning, gerapporteerde KPI’s |
| Lowell | Lowell vond een nieuw gebruik voor zijn verleden, maar ontsnapte niet aan verandering | lowell-r1-B2-Research/result.json | Industrieel verval, mensen, hergebruik, herstel en terugval |

Alle vier komen uit [Research Experiment 9](../../Research/editorial-acceptance-experiment-9-2026-11-16/report.md). Vooraf uniform **de eerste succesvolle B2-herhaling (r1)** geselecteerd, destijds alle vier voldoende. Geen selectie op de nieuwe Synthesis-uitkomsten. Het zijn alle vier beschikbare natuurlijke gevallen, geen nieuwe onafhankelijke holdout.

[verification-source-identity.json](verification-source-identity.json) bevestigt per case byte-identiteit van opgeslagen Research-resultaat én Research-input; alle vijf Idea-velden en originele broninhoud exact gelijk; productieprojectie exact gelijk aan de opgeslagen Experiment-9-projectie. JSON-propertynamen van SynthesisInput volgen het productiecontract, inhoud is niet aangepast. De oorspronkelijke bron blijft in cases/<case>/source.md voor audit, niet voor de modelcall.

Natuurlijke bronversies: NASA Webb Deployment, NPS Wolf Ecology, de Ministry of Justice-praktijkblog uit 2018 en het NPS-Lowell-boekperspectief uit 1992. Bronherkomst/HTML/extractie zijn opgeslagen in Experiment 9; geen nieuwe webversie gedownload. We toetsen trouw aan deze vaste teksten, geen actuele natuur-, stads- of beleidsclaims.

## Prospectieve freeze, configuratie en budget

[protocol.md](protocol.md) en [plan.json](plan.json) lagen vóór calls vast. Freeze: **${freeze.createdUtc}**, ${Object.keys(freeze.files).length} SHA256-hashes in [freeze.json](freeze.json): bestaande gevolgde repo, bronartefacten, alle inputs, criteria, plan, gekopieerde prompts, runner en gebouwde runtime. [verification-requests.json](verification-requests.json) bevestigt iedere call na freeze. Analyse-/rapportagescripts zijn deels later toegevoegd; ze worden niet als prospectief bevroren analysecode voorgesteld.

Productieprompts bytegelijk gekopieerd; disk prompt scope gebruikt. Bij voorbereiding zijn ook de daadwerkelijk ingebedde system/user-resources met de kopieën vergeleken. Bij alle twaalf verstuurde requests zijn de systeemtekst en volledige productie-userprompt-rendering exact gecontroleerd. Zo steunt promptidentiteit niet alleen op bestandsnamen of een oude build.

Zoals de productiefabriek: officiële OpenAI ChatClient → IChatClient → AsAIAgent → echte SynthesizerAgent. Alleen experimentele transportbegrenzing toegevoegd: RetryPolicy(0), één HTTP-request per poging, maximaal 5.000 completiontokens inclusief reasoning, maximaal 120.000 UTF-8 requestbytes en 300 seconden timeout. Verse client/session per call; geen temperatuur-, seed- of reasoningoverride. Geen wijzigingen aan productieconfiguratie. Productiemodel staat ook op gpt-6-luna. Beoordelingscriteria kwamen niet in de requests.

Vooraf hard maximum: 12 attempts, 60.000 outputtokens totaal. Alle twaalf gebruikt; HTTP 200, finish_reason stop, nul truncatie-, parser- of contractfouten. Geen extra attempts of vervanging. De bytecap is geen exacte inputtokencap; geen geverifieerd geldtarief en dus geen verzonnen eurokosten.

## Vooraf vastgelegde beoordeling

Vijf dimensies 0–2: **BT** brontrouw; **F** focus Selected Idea; **BN** behoud essentiële feiten/betekenisnuances; **IW** inhoudelijke inzichten/contrasten/details; **NG** bruikbaarheid als grondstof voor Narrative. 2 voldoende, 1 bruikbaar met concreet relevant gebrek, 0 materieel misleidend/onbruikbaar op die dimensie. Geen compenserende totaalscore.

GO vereiste minstens 11/12 volledig voldoende, minstens 2/3 per case, geen materiële verzonnen/misleidende claim en geen herhaalde essentiële omissie. Geen academische volledigheid, LinkedIn-hook, verplichte lengte of eindpostkwaliteit. Narrative/Compose zijn niet uitgevoerd of gescoord. Beschikbare concrete details hoeven niet allemaal geselecteerd te worden.

| Run | BT | F | BN | IW | NG | Oordeel |
${scores.map(x=>'| '+x.id+' | '+x.scores.join(' | ')+' | '+(x.pass?'voldoende':'verbeterpunt')+' |').join('\n')}

**Webb 3/3; Wolf 3/3; Digital 2/3; Lowell 0/3. Totaal 8/12.** Dit zijn beschrijvende assistentscores, geen objectieve of statistisch gevalideerde succespercentages.

## Concrete bevindingen en attributie

### 1. Verkeerde actor in Digital-r1 — nieuw bij Synthesis

Output [digital-r1](calls/digital-r1-Synthesis/content.md), regel ${actor.line}:

> ${actor.text}

Oorspronkelijke bron, regel ${originalActor.line}:

> ${originalActor.text}

De Research Facts 1–2 en Selected Idea betreffen OPG/POA, niet Nuëyon. Nuëyon staat nergens in deze vaste bron, Idea of Research; het verschijnt pas in de Synthesis-guidance. De overige Evidence-feiten zijn grotendeels juist. De fout is materieel voor een productiehandoff: Narrative krijgt expliciet een opdracht met een verkeerde organisatie. Daarom BT 0 en F/NG 1; dit betekent niet dat de hele tekst waardeloos is. Er is geen downstreamtekst geproduceerd, dus werkelijke verspreiding naar een post is onbekend.

De bestaande userprompt bevat onder meer “Describing: What Nuëyon believes…” en “explicitly state when the story should explain Nuëyon's own product direction”. Dat is een concrete kandidaatverklaring en sluit aan bij Experiment 6. Deze baseline zonder promptvariant bewijst niet dat die regels de fout veroorzaakten. De andere twee digitale herhalingen bevatten de actorinvoeging niet; ook de andere cases blijven bij hun bronactoren.

### 2. Lowell: herhaalde verlies van een betekenisvol contrast bij selectie

Vaste Research én echte projectie, regel ${contrast.line}:

> ${contrast.text}

Oorspronkelijke bron, regel ${sourceContrast.line}:

> ${sourceContrast.text}

Geen van de drie Synthesis-outputs behoudt de relatie **outputgroei ondanks afnemend werk**. R1 comprimeert tot aantallen banen en hardship/uncertainty; r2 begint pas bij de naoorlogse sluitingen; r3 noemt 8.000 banen en algemene menselijke gevolgen. Zie [coverage-audit.md](coverage-audit.md), onderwerp output-versus-jobs per run. Dit contrast was vóór calls als essentieel vastgelegd. Daarom BN/NG 1 in alle drie.

Dit is **geen Research- of filteromissie**: de precieze relatie is al in Fact 1 doorgegeven. De hergebruik/terugvalboodschap blijft wel goed: alle drie bewaren burgerinitiatief, meer dan honderd hergebruikte gebouwen, late-1980s terugval en het historische perspectief. Ze verzinnen geen blijvend herstel of actuele Wang-status. De bijdrage van universiteit/Wang is in r1 concreter dan in r2/r3, waar die vooral in begrenzing/secondary terechtkomt.

De relevantie van het vroege productie/werkcontrast voor deze smalle hergebruik-Idea is een redactioneel oordeel en bespreekbaar. Onder een mildere rubric waarin dit context is, zouden de drie Lowell-runs voldoende kunnen zijn: **11/12**. De primaire vooraf bevroren score blijft **8/12**; de materiële actorinvoeging verhindert ook in die sensitiviteit GO. Geen nieuwe poort achteraf.

### 3. Veel inhoudelijke selectie is juist

- **Webb:** alle drie behouden het onderscheid tussen volledig uitgevouwen in week twee en routinewetenschap na zes maanden, met afkoeling, uitlijning en kalibratie; concrete risico’s >50 stappen/>300 faalpunten. Geen verzonnen storing of reddingsactie. Ground control staat alleen in r2; gedetailleerde heater/ijs- en spiegelbeelden worden niet geselecteerd. Dat maakt de grondstof soberder, maar niet onvoldoende voor deze centrale tegenstelling.
- **Wolf:** alle drie bewaren sociaal jagen, voedsel voor andere soorten, veranderende prooipatronen en de grenzen van één-oorzaakverklaringen (andere predatoren, beheer, weer). R1 bevat de omgekeerde kalf/stierverhouding; r3 blijft algemener maar concreet met raven/eksters/coyotes/beren. Geen bewezen ecosysteembrede cascade of onjuist bison-jachtverhaal. Niet alle populatie-, ziekte- of terugkeercijfers zijn nodig.
- **Digital:** alle drie behouden twaalf weken, eerst aanvragers/alpha en later caseworkers, en het afwisselende onderzoek/verwerkingsritme. R1/r3 noemen het stoppen van nieuwe inzendingen expliciet; r2 comprimeert dit tot afwisselende weken. KPI’s blijven gerapporteerde aanvragen/tevredenheid/completion, geen uitgekeerde refunds of bewezen effect van de aanpak. Assisted digital zonder papier blijft aanwezig, maar wordt secundair geplaatst (r2 in Gaps). Dat is een prioriteitsrisico bij deze Idea, geen aparte harde fail zolang de betekenis beschikbaar blijft.
- **Lowell:** concrete gebouwen en economische terugval leveren een bruikbare inhoudelijke tegenstelling. Alle drie abstraheren menselijke ervaringen; paardenbloembladeren, hard brood en de letterlijk teruggestuurde werknemer verdwijnen. Onder dit plan waardevol verbeterpotentieel, geen verplicht historisch detail op zichzelf.

### 4. Kleine precisieproblemen blijven zichtbaar

Webb r2/r3 formuleren afkoeling te strak als “daarna”; r1 spreekt van maanden cooling na deployment maar Evidence 3 zegt correct continued. De bron beschrijft voortgezette/overlappende afkoeling. Geen gewijzigd hoofdpunt (uitvouwen ≠ routinewetenschap), wel een te schematische fasering. Digital-r1 zegt “twaalf weken later” zonder vastgesteld beginpunt, terwijl Research juist een onbekend startpunt noemt. Digital-r3 noemt “outdated-contact-details” waar de bron alleen details zegt. Dit zijn kleine ongefundeerde preciseringen, geen reden om correcte kernfeiten tot verzonnen uitkomsten te verheffen. Ze staan afzonderlijk in de traceability-audit.

## Filter/projectie versus Synthesis

Alle vier projecties zijn opnieuw met de echte deterministische filter gemaakt en exact met Experiment 9 vergeleken. Alle hier vooraf essentiële feiten staan in Facts/Unknowns; **geen van de beslissende gebreken ontstaat door filtering**. Interpretations, Development sequence en Editorial relevance zijn wel verwijderd, zoals productie voorschrijft. Hun duiding wordt dus niet aan Synthesis getoond, maar de feitelijke basis en relevante Unknowns blijven beschikbaar.

De audit [traceability.json](traceability.json) / [.md](traceability.md) bevat 90 gehele outputparagrafen met exacte outputregels, Research/projectieregels en oorspronkelijke bronpassages voor positieve claims. [coverage-audit.json](coverage-audit.json) / [.md](coverage-audit.md) volgt de geselecteerde kern- en contextonderwerpen door volledige Research → projectie → Synthesis, inclusief omissies. Een lege zoektreffer is geen zelfstandig semantisch oordeel; de volledige teksten zijn gelezen. De bron is leidend, niet automatisch elk historisch label of elk gegenereerd citaatanker.

## Verbruik en verificatie

| Case | Calls | Inputtokens | Outputtokens | Totaal |
${Object.entries(results.byCase).map(([c,x])=>'| '+c+' | '+x.calls+' | '+x.input.toLocaleString('nl-NL')+' | '+x.output.toLocaleString('nl-NL')+' | '+x.total.toLocaleString('nl-NL')+' |').join('\n')}
| **Totaal** | **12** | **65.073** | **16.771** | **81.844** |

Output omvat **10.826 reasoningtokens**; gemelde gecachete input **47.897**. Geen vergelijking met een kandidaat of geldprijs. [usage.json](usage.json) bewaart alle ruwe usage-objecten. Calls bevatten ongewijzigde request.json, response-raw.json, input.json, system.md, result.json, content.md, attemptmarker en HTTP-metadata. Geen API-sleutels of authheaders opgeslagen.

- Productie-hostbuild geslaagd: 0 warnings/errors. Geïsoleerde runnerbuild geslaagd.
- Zes relevante bestaande tests (SynthesisResearchFilterTests en SynthesizerAgentBoundaryTests) geslaagd; geen nieuwe tests of volledige regressieclaim voor ongewijzigde productie.
- [verification-offline-replay.json](verification-offline-replay.json): alle twaalf antwoorden opnieuw door echte SynthesizerAgent/parser, exact hetzelfde resultaat, geen netwerk. Dit bevestigt parsereproduceerbaarheid, niet modeldeterminisme.
- [verification-requests.json](verification-requests.json): twaalf juiste modellen, systeemteksten, productie-user-renderings, vaste inputs, schema, limieten, één HTTP 200 en finish_reason stop; alle controles waar.
- [verification-integrity.json](verification-integrity.json): alle ${Object.keys(freeze.files).length} freezehashes intact. Git toont alleen de nieuwe map PromptExperiments/Synthesis/. Geen bestaande productie- of experimentbestanden gewijzigd.

## Grenzen en kleinste volgende stap

Vier Engelse institutionele bronnen, één vaste succesvolle B2-Research-output per case en drie Synthesis-herhalingen testen beperkte variatie. Geen nieuwe Research-samples, onafhankelijke holdouts, andere modellen, bronvalidatie, menselijke blindbeoordeling of werkelijke Narrative-bruikbaarheid gemeten. Work-assistent kent cases, rubric en outputs; scores zijn ongeblindeerd. De 8/12 is geen algemene kwaliteitskans. De brand-contaminatie is concreet; de hardheid van het Lowell-selectiecriterium blijft een expliciete beoordelingskeuze.

**Kleinste zinvolle vervolgproef:** één afzonderlijke kandidaat die uitsluitend de Nuëyon/Compose-specifieke beschrijvingsinstructies in Synthesis onderwerpneutraal maakt, vergeleken met deze ongewijzigde baseline op dezelfde vaste inputs. Laat daarbij het Lowell-contrastcriterium expliciet reviewen vóór een nieuwe freeze; niet achteraf aanpassen. Zo toetsen we eerst de duidelijkste grensfout zonder filter, Research of downstreamagents te veranderen. Deze proef is een voorstel, niet uitgevoerd; onderwerpneutraliteit lost selectieproblemen niet vanzelf op. Geen promotie op basis van deze baseline.

## Reproduceren zonder nieuwe calls

Zie [REPLAY.md](REPLAY.md). Run.lock en attemptmarkers behouden. Run-modus niet opnieuw uitvoeren, freeze niet herschrijven en prepare.cjs niet na freeze uitvoeren. De opgeslagen gebouwde runner en ruwe responses zijn voldoende voor offline replay.
`;fs.writeFileSync(p.join(e,'report.md'),report);
fs.writeFileSync(p.join(e,'REPLAY.md'),`# Offline replay\n\nVanuit C:\\Workspaces\\Nueyon.Compose:\n\n\`\`\`powershell\ndotnet run --no-build --project PromptExperiments/Synthesis/baseline-experiment-1-2026-11-16/runner -- replay\nnode PromptExperiments/Synthesis/baseline-experiment-1-2026-11-16/verify.cjs\n\`\`\`\n\nReplay gebruikt uitsluitend opgeslagen response-raw.json via een offline IChatClient en echte SynthesizerAgent/parser; geen credentials of netwerk. De freeze wordt eerst gecontroleerd. Resultaat moet 12 exacte replays zijn. verify controleert SHA256, requestrendering, inputs en usage. Bij een rebuild kunnen runtimehashes verschillen; bewaar de oorspronkelijke freeze en binaries en onderzoek het verschil, overschrijf de freeze niet.\n\nRun.lock/attemptmarkers nooit resetten. Live run niet opnieuw starten. Prepare/freeze zijn historische voorbereidende scripts, geen replaycommando’s.\n`);
fs.writeFileSync(p.join(e,'verification-build-tests.json'),JSON.stringify({hostBuild:{project:'src/Nueyon.Compose.Host.Console/Nueyon.Compose.Host.Console.csproj',passed:true,warnings:0,errors:0},runnerBuild:{passed:true},existingTests:{project:'src/Nueyon.Compose.Application.Tests/Nueyon.Compose.Application.Tests.csproj',filter:'FullyQualifiedName~SynthesisResearchFilterTests|FullyQualifiedName~SynthesizerAgentBoundaryTests',passed:6,failed:0,skipped:0},scope:'Observed tool executions; no new modelcalls or new tests'},null,2));
console.log('Wrote Dutch report');
