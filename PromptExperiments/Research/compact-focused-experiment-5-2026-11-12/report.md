# Experiment 5 — compacte Research en gerichte semantische Judge

12 november 2026. **B is kansrijk voor een onafhankelijke vervolgproef; C is NO-GO. Geen promotie of activering.**

De compacte Research-prompt levert in deze kleine proef zes van zes kwalitatief geslaagde resultaten, tegenover vier van zes voor productie (vijf bij een gunstiger lezing van Intro). B gebruikt **74,6% minder generatietokens**. De gerichte Judge accepteert alle zes B-resultaten, voegt **128,6% tokens** toe en levert geen extra geslaagd resultaat. Bij historische fouten mist hij alle drie vooraf gedefinieerde hoofddoelen. Eén historisch resultaat wordt gedeeltelijk verbeterd, maar blijft materieel onvolledig en wordt toch geaccepteerd.

Dit is een verkennende vergelijking van Research-informatie en de echte filterprojectie. Geen bewijs van algemene betrouwbaarheid, statistische non-inferioriteit, productiegeschiktheid of kwaliteit van een uiteindelijke publicatie.

## Uitgangspunt en eerdere proeven

Repository `C:\Workspaces\Nueyon.Compose`, origin `antoine271167/Nueyon.Compose`. Schone beginstatus, lokale en met `git ls-remote` gecontroleerde remote HEAD beide `f7179cadde90c4fc15b2b1b42a35f45a9bde2b84`. De gekoppelde conversatie is gelezen. Actuele ResearchAgent, ResearchSemanticJudge, ResearchResult, prompt-rendering, SynthesisResearchFilter, defaultopties en relevante tests zijn onderzocht, evenals baseline en experimentrapporten 1–4.

De baseline gebruikte drie productie-Research-generaties en vond de bekende Intro-, 005- en 010-problemen. Experiment 1 vond met de Judge 0/3 hoofddoelen; de compacte generatie werd door de kalibratiepoort niet uitgevoerd. Experiment 2 vond 1/3 doelen, met bovendien ongeldige bronankers. Experiment 3 vond met Evidence Mapper 2/3 doelen maar had map-/Judge-ankerproblemen. In de matched ablatietest van experiment 4 vond zonder-map 2/3 en met-map 0/3, tegenover 44.273 en 117.222 tokens. Dat waren vooral beoordelingen van bevroren outputs, geen A/B-generatorvergelijking. Deze cijfers zijn historische context, geen nieuwe metingen of gelijkwaardige gerandomiseerde vergelijkingen met experiment 5.

Daarom is hier de generatievraag zelfstandig getest. Geen Evidence Mapper, geen extra broninventariscontract, geen wijziging van productiegedrag.

## Vooraf vastgelegde opzet

`protocol.md`, `plan.json`, `labels.json`, prompts, fixtures, runner, 101 beschermde bronbestanden en gebruikte runtime-DLLs zijn vóór de eerste livecall gehasht: **161 bevroren bestanden**. Freeze is achteraf intact. De kandidaat-Research-prompts zijn ongewijzigd overgenomen uit de nooit gepromoveerde compacte kandidaat van experiment 1; A is een kopie van de actuele productieprompts. De nieuwe Judge focust generiek op clause-level Facts/Unknowns-tegenstrijdigheden, materiële beperkingen/tegenvoorbeelden en de werkelijke projectie. Geen casenamen, bekende antwoorden of broncitaten in de instructies.

| Arm | Werking |
|---|---|
| A | Productie-Research-prompt, zonder Judge |
| B | Compacte Research-prompt, zonder Judge |
| C | Exact B-output, daarna gerichte Judge en bij geldige afwijzing maximaal één repair en één herbeoordeling |

B en C delen dezelfde eerste generatie. Daardoor meet C de toegevoegde controle op exact dezelfde tekst, zonder toevallige verschillen in een derde generatie. Standalone kosten van C bevatten wel de B-generatie. Werkelijk in het experiment wordt deze slechts één keer betaald. A/B-volgorde wisselt per case. Eerst alle twaalf generaties, daarna judges; geen tuning op tussentijdse uitslagen.

Dezelfde bron en alle vijf Selected Idea-velden per arm. Productie-ResearchAgent zonder geïnjecteerde Judge voert A/B uit. Voor de experimentele repair wordt de productie-user-rendering en ResearchResult-parser hergebruikt via reflectie, in een nieuwe sessie met oorspronkelijke bron/Idea, vorige Research en diagnoses. Judge gebruikt het bestaande ResearchSemanticJudge-contract en zijn strenge parser. De echte SynthesisResearchFilter maakt alle projecties. Geen vervangende filter of verruimde verdictparser.

Alle nieuw toegevoegde instructies zijn format-agnostisch; geen LinkedIn-/Article-eisen voor Research. Overige workflowagents zijn niet uitgevoerd of gewijzigd. Bestaande Narrative.user.prompt.md bevat nog de Article/LinkedIn-compatibiliteitsregel; die is een bestaande grensinconsistentie, geen door deze proef opgeloste eigenschap van de volledige keten. Brondata met formatverwijzingen blijft brondata, geen publicatie-opdracht aan Research.

Historisch: intro/005/010 gebruiken dezelfde bevroren bron/Idea-inputs als de baseline. Intro is een gereconstrueerde brief, geen originele ruwe auteursbron. Daarnaast drie nieuwe synthetische bronnen: reservoir, translation, inspection. Deze toetsen nieuwe domeinen, met deels onware Idea.Evidence om bronautoriteit te bewaken. Ze zijn nieuw en ongebruikt, maar korter en duidelijker dan de historische rapporten en geen onafhankelijke menselijke labels. Dezelfde assistent schreef en beoordeelde ze, ongeblindeerd. Semantische foutfamilies zijn verwant aan de bekende problemen; geen bewijs tegen alle vormen van overfitting.

Rubric vooraf: brontrouw/scope, essentiële dekking, epistemische consistentie en bruikbaarheid echte projectie, elk 0–2. Een materiële fout/omissie faalt, ook als andere dimensies voldoende zijn. Kleine niet-materiële tekortkomingen mogen score 1 krijgen. Equivalent bewijs telt; niet elke bronzin is verplicht. Structuur, exacte citaten en parsergeldigheid worden apart van inhoud beoordeeld. `generation-assessments.json` bevat alle scores, componentdekking, locaties, motivering en onzekerheid.

## A/B/C: kwaliteit van nieuwe Research-generatie

| Case | A | B | C | Belangrijkste observatie |
|---|---|---|---|---|
| Intro | Fail, middelmatige zekerheid | Pass | Pass | A bewaart de concrete-expression-Fact, maar verklaart het verband tussen ambitie en productkeuze te breed onbekend. B behoudt de bronbetekenis en beperkt onzekerheid tot onbewezen effect/levering. |
| 005 | Fail | Pass | Pass | A mist Idea/Research-variatie geheel; datums verdwijnen met Development sequence. B behoudt variatie, datums/backendgrens en Synthesis-spanning in Facts/Unknowns. |
| 010 | Pass | Pass | Pass | Beide bewaren nu Compose-feitfout versus eerdere company-first-keten met hiërarchie 2/2, naast de observatiegrenzen. |
| Reservoir | Pass | Pass | Pass | Beide bewaren changed demand én threshold, West-tegenproef met aparte pomp en beperkte controlewaarde, ontbrekende heranalyse en geplande sensor. |
| Translation | Pass | Pass | Pass | Beide scheiden toegeschreven betekenis van onbewezen begrip/belonging, en feitelijke tijdsfout van wordingproblemen, met feedback- en samplinggrenzen. |
| Inspection | Pass | Pass | Pass | Beide bewaren Larch/Birch-tegencontrast, Hazel, convenience sample, confounding en ongeteste aanbeveling. |
| **Totaal** | **4/6; sensitiviteit 5/6** | **6/6** | **6/6** | **C bevat dezelfde Research als B; geen extra succes.** |

Intro-A verdient geen absolute claim: de laatste Unknown zegt dat de bron geen causal relationship tussen de ambitie en de keuze van Compose vestigt; Development sequence noemt de relationship UNKNOWN. Daartegenover geeft §11 expliciet een rationale: “because it gives the broader vision a concrete expression”. Onder de vooraf vastgelegde rubric ontkent de blanket-kwalificatie te veel. Bij een uitsluitend empirische interpretatie van causal relationship is een pass verdedigbaar. Daarom ook **A=5/6** doorgerekend; de conclusie over B en de kosten verandert niet. De oorspronkelijke historische Intro-fout is explicieter dan deze nieuwe A-variant en wordt afzonderlijk beoordeeld.

B is niet foutloos. Translation-B voegt in de verwijderde Development sequence een niet expliciet gedateerde volgorde toe (“it then supplied”, feedback “during the pilot”); Inspection-B suggereert met “then” een aanbevelingsvolgorde. Dit zijn kleine bronprecisieproblemen, scores 1, zonder materieel labelverlies en buiten de Synthesis-projectie. A is op die chronologie soms zorgvuldiger. Reservoir-A formuleert één Unknown minder precies dan het broninterval, terwijl dezelfde Unknown en Fact 7 dat interval wel noemen. Translation-A is overmatig voorzichtig over de reden van herzieningen. Deze verschillen zijn niet achteraf tot nieuwe harde faalcriteria gemaakt.

Alle twaalf initiële Research-antwoorden en de ene repair hebben de vijf exacte headings en zijn met de productieparser geldig. De echte filter bewaart Facts/Unknowns conform code. In 005-A staan de datums uitsluitend buiten de projectie: technisch correct filtergedrag maar inhoudelijk onvolledige overdracht. De ontbrekende upstreamvariatie ontbreekt al vóór filtering. Geen filterbug vastgesteld. Bruikbaarheid betreft informatiebeschikbaarheid, geen gerealiseerde Synthesis-/Narrative-/Compose-kwaliteit.

## Judge afzonderlijk van Research-generatie

Op de zes geslaagde B-resultaten: zes geldige acceptaties, geen overrejectie waargenomen, geen repairs. Omdat hier geen materieel fout B-resultaat is ontstaan, kan deze set de gevoeligheid of false-acceptance-rate niet bepalen. Acceptatie is geen onafhankelijk bewijs dat de rubric klopt.

Zes afzonderlijke historische controles, uit experiment 4 bytegelijk hergebruikt:

| Case | Defecte variant | Correcte variant | Vooraf bepaald hoofddoel |
|---|---|---|---|
| Intro | Accept | Accept | Te brede Unknown 4 gemist |
| 005 | Reject, geldig | Accept | Upstreamvariatie gemist; andere materiële lacune wel gevonden |
| 010 | Accept | Accept | Ontbrekende Compose-feitfout met behouden hiërarchie gemist |

**Hoofddoeldetectie 0/3; enige materiële afwijzing 1/3; correcte controles 3/3 geaccepteerd.** Alle zes verdicts zijn parsergeldig. Dit onderscheid voorkomt dat een willekeurige reject als juiste targetdetectie telt.

005-blocker 1 benoemt de ontbrekende Synthesis-spanning terecht, met een exact en inhoudelijk passend bronanker. Blocker 2 vraagt de scores expliciet subjectief te noemen. Dat is brongetrouw en nuttig, maar als zelfstandige blocker betwistbaar: de bestaande Facts schrijven de scores al aan de bron toe en claimen geen geautomatiseerde meting. De eerste blocker draagt de afwijzing; de tweede krijgt geen afzonderlijk bewezen detectiesucces. Beide citaten zijn letterlijke substrings. Geen caselabels of historische diagnoses aan de Judge gevoerd.

Ten opzichte van experiment 4 zonder map is de hoofddoeldetectie hier lager, maar dat is **geen** herhaalde matched vergelijking van judgeprompts: andere prompt, één sample per document. Geen bewezen regressiekans of oorzaak afleiden. Wel voldoet deze kandidaat niet aan haar eigen vooraf vastgelegde poort.

## Herstel: gedeeltelijk beter, nog niet geslaagd

Alleen historisch 005 werd geldig afgewezen en dus één keer hersteld. Intro/010 kregen geen repair omdat de Judge ze accepteerde. Natuurlijke B-repairs: nul. De reserve is niet gebruikt voor geforceerde fouten, extra controles of tuning.

De repair voegt de Synthesis-spanning toe in Fact 4, kwalificeert subjectieve scores in Fact 8 en brengt datums/backendgrens naar Fact 12. De genoemde diagnoses zijn dus opgelost. Uitkomsten, rollback, kwalificaties en het feit dat geen geïsoleerd promptcausaal effect is bewezen blijven behouden. Compressie van 30 naar 12 Facts verwijdert onder meer exacte modelnaam en instructiedetails; geen nieuwe **materiële** regressie onder de referentierubric gevonden.

**Idea/Research-variatie ontbreekt nog steeds volledig.** De materiële dekkings- en projectiescore blijft daarom 0, ondanks gedeeltelijke componentverbetering. De Judge accepteert de repair vervolgens. Strikt kwalitatief herstelresultaat: **0/1**, niet 1/1. Het toont dat een bruikbare deelcorrectie kan lukken zonder dat het volledige resultaat aan de rubric voldoet. Het toont niet dat de repair de ongediagnosticeerde upstreamvariatie niet zou kunnen herstellen als die wel correct werd aangewezen.

## Werkelijk verbruik en kosten per geslaagd resultaat

| Onderdeel | Calls | Inputtokens | Outputtokens | Totaal |
|---|---:|---:|---:|---:|
| A generatie | 6 | 67.707 | 10.953 | **78.660** |
| B generatie | 6 | 13.432 | 6.529 | **19.961** |
| Judge op B | 6 | 23.232 | 2.428 | **25.660** |
| C standalone: B + Judge | 12 | 36.664 | 8.957 | **45.621** |
| Historische kalibratie | 6 | 37.282 | 6.363 | **43.645** |
| Historische repair + herbeoordeling | 2 | 11.206 | 2.573 | **13.779** |
| **Werkelijk experiment, zonder dubbelgeteld B** | **26** | **152.859** | **28.846** | **181.705** |

De C-rij is een vergelijkingsoptelling, geen extra uitgave bovenop B en de zes judges. Historische kalibratie/reparaties zijn onderzoekskosten; niet als natuurlijke C-overhead of B/C-successen meegerekend. Inclusief die onderzoekskosten is hier geen extra kwalitatief geslaagd resultaat gekocht.

| Arm | Successen | Tokens per succes, I+O | Gewogen eenheden per succes, I+4O |
|---|---:|---:|---:|
| A, strikte Intro-lezing | 4 | 19.665 | 27.880 |
| A, gunstiger Intro-lezing | 5 | 15.732 | 22.304 |
| B | 6 | **3.327** | **6.591** |
| C | 6 | **7.604** | **12.082** |

I+4O is een vooraf vastgelegde gevoeligheidsweging voor duurdere output, **geen geldtarief**. B bespaart 74,6% op generatietokens en 64,5% onder I+4O; alle zes cases gebruiken minder tokens met B. C kost 128,6% meer tokens dan B, zonder extra succes; onder I+4O eveneens duurder per succes. C is wel goedkoper dan A, maar B domineert C in deze gemeten vergelijking.

Geen verifieerbaar GPT-6 Luna-factuurtarief beschikbaar in configuratie of providerresponses; dus geen fictieve dollars/euro's. De geldvraag is alleen conditioneel beantwoord. Voor tarieven per miljoen is de formule `(I-cached)*p_input + cached*p_cached + O*p_output`, gedeeld door een miljoen, plus eventueel aantoonbare cache-write-opslag. De providercharging van cache_write moet daarvoor eerst worden bevestigd. `usage.json` bewaart alle oorspronkelijke velden. A had 13.730 cached tokens; B en C nul. Zelfs als deze A-cache volledig gratis is, heeft A meer betaalbare input én output dan B, ook per succes bij de gunstiger A-lezing. Dit ondersteunt een sterke kostenindicatie, geen vastgestelde factuurprijs. Cache-write-telling: A 53.959, B 11.334, C 34.548; geen onbekende opslag verondersteld.

Reasoning is inbegrepen in output: 17.597 tokens voor de hele run, niet opnieuw opgeteld. Cache-write 136.971 en cached 13.730 zijn details van input, geen extra tokenaantallen boven total. Alle 26 HTTP 200, model gpt-6-luna, finish_reason stop. Nul truncaties, technische fouten of parserfouten. Geen andere live modelcalls in deze uitvoering.

## Budget, integriteit en offline validatie

Hard maximum **42**, opgebouwd als 12 generaties + 6 natuurlijke judges + 6 historische judges + maximaal 12 natuurlijke repair/rejudge + maximaal 6 historische bad repair/rejudge. Daarmee passen alle vooraf geplande varianten en maximale repairs; geen criteria onderweg versoberd. **26 gebruikt, 16 ongebruikt**. SDK RetryPolicy(0), één HTTP-request per exclusieve attemptmarker en een globale exclusieve runmarker. Geen heruitvoering of reset. Maximaal 5.000 outputtokens per call, hard worst-case 210.000; werkelijk 28.846. Maximaal 96.000 UTF-8 requestbytes per call, gecontroleerd vóór verzenden; dit is een transportbudget, geen exacte tokenvoorspelling. Geen geldplafond geclaimd waar een betrouwbaar tarief ontbreekt.

- Solutionbuild geslaagd: nul waarschuwingen/fouten. **300 offline tests geslaagd**, 238 Application, 25 Infrastructure, 37 Console; nul mislukt/overgeslagen. Domain bevat geen discoverable tests. Logs bewaard.
- Runnerbuild en offline controles voor productie-Researchparser, vijf headings/filter en Judgeparser geslaagd, inclusief null blocker, dubbele properties en extra velden. Geen nieuwe productietests nodig voor geïsoleerde prompts/data.
- Alle **26 antwoorden offline gereplayd** met de actuele productieparser; alle opgeslagen projecties opnieuw gelijk aan de echte filter. Geen modelclient/netwerk nodig voor replay.
- Alle requests geverifieerd op model, outputcap, bron, vijf Idea-velden, juiste systemprompt, Research en daadwerkelijke filterprojectie waar van toepassing. Alle checks slagen. Geen labels of beoordelingsnotities in modelinput.
- Alle **161 freezehashes intact**, alle **101 beschermde bronbestanden bytegelijk**, uitsluitend deze nieuwe experimentmap in gitstatus. `git diff --check` schoon. Geen configuratie-, prompt-, ResearchAgent-, filter- of EnableResearchJudge-wijziging. Default-disabled test slaagt; experimentele judges zijn direct in de geïsoleerde runner aangemaakt, geen hostactivering.
- Geen Evidence Mapper, retrieval, GitHub Copilot CLI, commit of push. API-key en authheaders niet gelogd.

## Besluit en concreet advies

**B voldoet aan de vooraf vastgelegde verkennende GO-poort:** 6/6 en 3/3 holdouts, minstens evenveel successen als A, geen materiële regressie tegenover geslaagd A en meer dan 20% lagere kostenproxies, ook per succes. Dat is uitsluitend een GO voor een onafhankelijke vervolgproef. Promotie blijft ongeautoriseerd en inhoudelijk te vroeg: één generatie per cel, bekende historische doelen, korte eigen holdouts, ongeblindeerde eigen beoordeling en onzekere tariefprijs.

**C is NO-GO:** 0/3 hoofddoelen, 0 natuurlijke rescues, geen extra successen, geen volledig geslaagde repair en hogere kosten per succes. Correcte acceptaties en gedeeltelijke herstelwinst compenseren deze poortfouten niet. De proef bewijst niet dat iedere Judge zinloos is, maar rechtvaardigt deze kandidaat niet als standaardstap.

Behoud productie nu ongewijzigd en laat als volgende stap een onafhankelijke beoordelaar de vooraf gedefinieerde materiële labels valideren, vooral de nieuwe Intro-A-ambiguïteit. Vergelijk daarna A/B zonder Judge op nieuwe langere echte bronnen, met dezelfde generieke kandidaat, geblindeerde beoordeling en herhaalde/interleaved generaties binnen een nieuw budget. Meet dan ook gevolgen voor Synthesis en uiteindelijke inhoud met een aparte formatgrens. Verifieer eerst het providertarief als een geldbesluit nodig is. Geen verdere Judge-tuning op deze drie historische missers zonder nieuwe onafhankelijke evaluatieset. Voor iedere promotie of activering is afzonderlijke toestemming nodig.

## Artefacten

Alle bestanden staan onder deze experimentmap: `protocol.md`, `plan.json`, `labels.json`, `freeze.json`, `protected-before.json`, `head.txt`; `prompts/`, `cases/`, `calibration/`; `runner/` en voorbereidings-/samenvattingsscripts; `calls/<id>/` met exclusieve markers, exacte requests, ruwe responses, HTTP-metadata, Research/verdicts, echte filteroutput en repaircontext; `usage.json`, `results.json`, `generation-assessments.json`, `judge-repair-assessments.json`; `verification-requests.json`, `verification-offline-replay.json`, `verification-integrity.json`; build-, test- en runlogs. Geen bestaande experimentbestanden aangepast. **Deze run niet opnieuw uitvoeren; run.lock en markers behouden.**
