# Experiment 3 — expliciete Evidence Mapper + Research Judge

12 november 2026. **NO-GO voor productie-integratie, promotie en activering.**

De tweestapsaanpak detecteert **2/3 historische doelproblemen**, tegenover 1/3 in Experiment 2. Alle drie historische correcte controles worden geaccepteerd. Dat is bemoedigend binnen deze kleine steekproef, maar onvoldoende: intro blijft een false negative ondanks een inhoudelijk juiste map, drie maps bevatten ongeldige exacte bronankers en één nieuwe holdout-beoordeling wordt door de productieparser terecht geweigerd. De matched historische vergelijking kost inclusief maps **81.063 versus 46.859 tokens (+73,0%)**, bij negen versus zes calls. Geen statistisch bewijs van algemene verbetering of bewezen waarde voor productie.

## Onderzoek, scope en reproduceerbaarheid

Lokale repository `C:\Workspaces\Nueyon.Compose`, origin `https://github.com/antoine271167/Nueyon.Compose.git`, begin-HEAD `0dc334bf3441f91e29b005751900b3dc05907dcf`; eerste git-status schoon. Actuele ResearchSemanticJudge, ResearchAgent, SynthesisResearchFilter, serviceconfiguratie, ResearchJudgeTests, ResearchAgentEvidenceTests en filtertests onderzocht, evenals protocollen, rapporten, runner en fixtures van compact-judge-2026-11-12 en judge-calibration-experiment-2-2026-11-12. De gerefereerde chat is gelezen voor vervolgcontext; beoordeling en uitvoering steunen op lokale code en artefacten.

De echte filter behoudt Facts en Unknowns en verwijdert de andere secties. De bestaande agent kan bij ingeschakelde judge maximaal twee generaties/beoordelingen uitvoeren; hier is die workflow niet gestart. De productiejudge valideert JSON-contract, consistentie van accepted/blockingIssues, maximaal vijf blockers, categorieën, verplichte velden en letterlijke broncitaten. Dat bewijst geen semantische juistheid of materialiteit.

Alle eigen wijzigingen staan uitsluitend in deze nieuwe experimentmap. Geen productie-Researchprompt, filter, agentworkflow of EnableResearchJudge gewijzigd door deze uitvoering. Geen Research-generatie, reparatie, Synthesis-modelcall, retrieval, Copilot CLI, commit of push. Ook bij GO zou eerst toestemming voor integratie nodig zijn; wegens NO-GO wordt geen integratie voorgesteld.

## Vooraf vastgelegde proef

Vóór de eerste live call zijn protocol.md, prompts/mapper.md, prompts/judge.md, labels.json, twaalf testdocumenten, runner en budget gehasht in freeze.json. Een offline compilatiefout in de runner is vóór de eerste modelcall hersteld; freeze-preflight.json bewaart de eerste freeze en de definitieve freeze beschrijft de correctie. Protocol, rubric en labels zijn daarbij niet aangepast. De finale freeze is intact volgens verification-freeze.json.

Hard budget: **18 GPT-6 Luna HTTP-requests**, nul retries, nul heruitvoeringen. Zes maps uit zes bron/Idea-paren, daarna twaalf judges. Alle maps waren gereed en map-audit.json was opgeslagen voordat de judgefase startte. De maps zijn niet hersteld of inhoudelijk bijgestuurd. Een map wordt ongewijzigd hergebruikt voor twee Research-varianten van hetzelfde bron/Idea-paar. De zes maps zijn dus geen twaalf onafhankelijke waarnemingen.

Historische baselines en correcte controles zijn bytegelijk gekopieerd uit Experiment 2. Voor 005 is de aangevulde correcte controle gebruikt; de oude gedeeltelijke controle is bewust niet opgenomen. Nieuwe holdouts: garden (toegeschreven betekenis versus gemeten effect), kiln (concrete confounders/negatieve controle buiten de filterprojectie), catalogue (feitelijke datumfout versus correcte ordening en het omgekeerde). Elk heeft een correcte en defecte variant, vooraf gelabeld.

De nieuwe bronnen zijn onafhankelijk van de historische teksten en waren niet eerder door een model beoordeeld. Ze toetsen verwante abstracte fouttypen, geen volledig nieuwe taxonomie. Dezelfde assistent heeft ze geschreven, gelabeld en beoordeeld, ongeblindeerd en zonder onafhankelijke menselijke validatie. Dat beperkt de kracht van de claims over transfer en de betrouwbaarheid van de labels.

Mapper krijgt uitsluitend originalSource en selectedIdea, zonder Research, filteroutput, verwachte labels of diagnoses. Idea.Evidence blijft een afzonderlijke te controleren claim, nooit bronautoriteit. De drie nieuwe Ideas bevatten bewust onbewezen/verkeerde Evidence-beweringen. Judge ontvangt originele bron, alle Idea-velden, ongewijzigde ruwe evidence map, Research en de **werkelijke productie-filteroutput**. Een lokale wrapper voegt de map als afzonderlijk user-payloadveld toe aan de normale productiejudge-input. De bestaande ResearchSemanticJudge verwerkt en valideert het modelantwoord zelf; er is geen soepelere experimentele verdictparser. Labels en de menselijke mapaudit gaan niet mee.

De prompts bevatten generieke regels en geen casenamen, bekende citaten of verwachte diagnoses. De rubric verlangt materiële verschillen die toegestane conclusies veranderen; equivalente of verspreid aanwezige informatie mag volstaan. Onzekerheid over gemeten effecten of precieze implementatie mag naast bekende bronbetekenis of brede ontwikkelstatus blijven bestaan.

## Afzonderlijke beoordeling van de evidence maps

De zes maps bevatten samen **47 entries**. Elke entry is beoordeeld op bronsteun, scope, materialiteit en letterlijke ankers; twintig vooraf gelabelde coveragecomponenten zijn apart vergeleken. **19/20 volledig gedekt, één gedeeltelijk**: bij 010 ontbreekt de secundaire kwalificatie over niet-uniform gemeten engagement. De primaire scheiding tussen feitelijke fout en hiërarchie is wel aanwezig. Die secundaire nuance is voor deze Selected Idea perifeer; zij wordt niet achteraf als nieuw historisch doelprobleem opgevoerd.

Correctie op een telzin in de oorspronkelijke pre-judge audit: de samenvatting zegt abusievelijk 21/22. De gedetailleerde audit bevat 19 volledig en één gedeeltelijk gedekte component, totaal twintig. De oorspronkelijke audit is bewaard; results.json vermeldt de correctie. Dit is een verslagfout, geen wijziging van labels of resultaten.

| Map | Inhoudelijke hoofdbevinding | Exacte ankers |
|---|---|---|
| intro | S2 bewaart de expliciete concrete-expression-relatie; S4 de vroege status; S1 de company/audience-scope | Alle substrings geldig |
| 005 | S9 benoemt variabele Idea/Research-outputs; S6 de Synthesis-spanning als mogelijke bijdrage; scores/besluit/beperkingen behouden | Alle substrings geldig |
| 010 | P7 bewaart Compose-feitfout met eerdere company-first-keten en hiërarchie 2/2; geen Compose-first-hiërarchiebreuk | P8 verwijdert Markdown-vetmarkeringen uit één citaat: ongeldig |
| garden | Toegeschreven betekenis en ongemeten sharing-effect gescheiden; verkeerde Idea Evidence expliciet unsupported | Alle substrings geldig |
| kiln | Batch/vochtigheid en same-X-tegenproef aanwezig, zonder causale conclusie | S1 verandert `week A` in `Week A`: ongeldig |
| catalogue | North/South/West, twee onafhankelijke dimensies en ontbrekend intake-log behouden; verkeerde Idea Evidence unsupported | S1 verandert `batch North` in `Batch North`: ongeldig |

**Slechts 3/6 maps voldoen volledig aan letterlijke bronankers.** Niet normaliseren of stilzwijgend repareren: kleine typografische veranderingen zijn bij dit contract echte validatiefouten.

Ook geldige substrings zijn niet automatisch voldoende bewijs. Intro S7 noemt verschillende portfolio-gebieden maar citeert alleen algemene zinnen over verkende richtingen en beschikbaarheid, zonder de specifieke namen exact te ankeren. De namen staan wel in de bron: dit is onvolledige bewijsvoering, geen verzonnen portfolio. Intro S9 noemt een verbod een negativeControl en catalogue S7 noemt afwezige tests een negativeControl. Beide zijn werkelijk bronondersteunde grenzen, maar geen uitgevoerde empirische negatieve controles. Die taxonomie is onnauwkeurig en maakt de map minder helder controleerbaar.

Kiln itemiseert de causale claim uit Idea.Evidence niet afzonderlijk in ideaChecks; de hoofdmap beperkt die claim wel consequent. Geen materiële bronuitvinding gevonden bij deze eigen inhoudelijke beoordeling. Deze positieve beoordeling is evenmin onfeilbaar. map-audit.json bewaart per entry en referentiecomponent de onderbouwing; automatische substringcontrole en menselijke semantische beoordeling zijn gescheiden.

## Historische uitkomsten: mapfout versus judgefout

| Case | Verwacht | Judge | Inhoudelijk resultaat |
|---|---|---|---|
| intro baseline | reject | accept | Doelprobleem gemist |
| intro correct | accept | accept | Correct geaccepteerd; eerdere overrejectie van Unknown 3 niet herhaald |
| 005 baseline | reject | reject, geldig | Upstreamvariatie én Synthesis-spanning correct benoemd |
| 005 correct | accept | accept | Bestaande aangevulde controle geaccepteerd |
| 010 baseline | reject | reject, geldig | Ontbrekende feitfout met behouden hiërarchie correct benoemd |
| 010 correct | accept | accept | Correct contrast geaccepteerd |

**Intro is een judge-false-negative.** De bron beschrijft Compose expliciet als concrete uitdrukking van de bredere visie. Baseline Unknown 4 ontkent dat het naast elkaar zetten van ambitie en Compose de ambitie concreet maakt, als louter redactionele Idea-beoordeling. Facts 9/13, map S2 en de oorspronkelijke bron waren beschikbaar. De judge accepteert toch. Een minder expliciet contrast in de map kan een onderzoekshypothese zijn, maar er is geen bewijs dat de hoofdclaim ontbrak of dat dit de verborgen reden voor acceptatie was.

**005 is een bruikbare inhoudelijke detectie.** Beide issues zijn exact geankerd, materieel voor causale terughoudendheid en laten de waargenomen slechtere resultaten intact. De judge stelt niet dat upstreamvariatie of Synthesis-spanning de uitkomsten bewezen veroorzaakte.

**010 is eveneens bruikbaar.** Map P7 is inhoudelijk goed en letterlijk verankerd. De judge gebruikt die geldige bronzin en bewaart het onderscheid tussen feitelijke Compose-fout en behouden company-first-hiërarchie. Een afzonderlijke P8-mapankerfout wordt niet overgenomen; dat maakt de map als geheel niet alsnog geldig. ResearchLocation noemt ook Development sequence, maar de diagnose betreft terecht de omissie in Facts/projectie.

Resultaat: **2/3 historische doelen**, **3/3 correcte historische controles**. Geen bewijs voor algemene nauwkeurigheid of foutpercentages buiten deze set.

## Nieuwe holdouts en negatieve controles

| Paar | Defecte variant | Correcte variant | Beoordeling |
|---|---|---|---|
| garden | Geldige afwijzing | Accept | Toegeschreven betekenis correct onderscheiden van onbewezen effect; onware Idea Evidence niet als bewijs gebruikt |
| kiln | Geldige afwijzing | Accept | Concrete confounders en same-X-tegenproef ontbreken downstream; aanwezigheid in Interpretations redt dit niet |
| catalogue | Ruw reject, productieparser weigert | Accept | Ruwe contrastdiagnose is inhoudelijk relevant, maar twee ongeldige broncitaten; geen geslaagde detectie |

Kiln wijst de twee materiële ontbrekende beperkingen goed aan met een exact citaat. De aanvullende wens om de expliciete no-causal-conclusion-zin te herhalen is mogelijk te streng, omdat de bestaande Unknown al zegt dat de oorzaak onbekend is. Dat is geen aparte detectie en rechtvaardigt op zichzelf geen blocker; de concrete omissies dragen de afwijzing.

Catalogue laat een gedeelde kwetsbaarheid zien. De mapper maakt van `batch North` het niet-letterlijke `Batch North`; de judge gebruikt diezelfde ongeldige hoofdletter in beide sourceQuotes. Dit is verenigbaar met foutoverdracht vanuit de map, maar zonder ingreep/ablatierun is niet bewezen dat de judge uit de map heeft gekopieerd. De oorspronkelijke bron was beschikbaar en de judge controleerde de ankers onvoldoende. Daarom: **mapper-ankerfout én judge-ankerfout**.

Daarbij valt het eerste catalogue-issue rechtstreeks selectedIdea.evidence aan. Die claim is onjuist, maar een fout in de Idea is op zichzelf geen Research-blocker als Research haar niet overneemt. De tweede ruwe diagnose wijst wél de materiële North-omissie in Research aan. De correcte variant met dezelfde Idea wordt geaccepteerd, wat deze onjuiste extra blocker niet rechtvaardigt. Alle correcte controles zijn dus geaccepteerd, maar de diagnoselogica is niet overal even zuiver.

Holdouts: **2/3 technisch geldige, inhoudelijk juiste defectdetecties; 3/3 correcte controles geaccepteerd**. Niet als 6/6 of 3/3 detectie rapporteren. Daarmee is het vooraf vereiste ontbreken van ernstige holdoutregressies niet aangetoond.

## Vergelijking met Experiment 2 en kosten

| Maatstaf | Experiment 2 | Experiment 3 |
|---|---:|---:|
| Historische doelproblemen correct gedetecteerd | 1/3 | 2/3 |
| Gelijke drie correcte historische controles geaccepteerd | 2/3 | 3/3 |
| Geldige historische verdicts, matched zes | 4/6 | 6/6 |
| Holdouts, geldige defectdetecties | 3/3 | 2/3 |
| Holdouts, correcte controles geaccepteerd | 2/2 | 3/3 |
| Nieuwe expliciet controleerbare maps | geen | 6; 3 volledig exact geankerd |
| Hele run, calls | 12 | 18 |
| Hele run, inputtokens | 52.943 | 71.567 |
| Hele run, outputtokens | 10.421 | 25.470 |
| Hele run, totaal tokens | 63.364 | 97.037 |

Holdouts verschillen tussen experimenten; hun cijfers zijn geen matched bewijs van verslechtering of verbetering. Historische vergelijking gebruikt dezelfde zes Research-documenten en bronnen/Ideas; oude gedeeltelijke 005-controle uit Experiment 2 is uitgesloten. Er is niet herhaald of geïnterleaved en naast de map is ook de generieke judgeprompt veranderd. Het verschil kan dus niet causaal uitsluitend aan de map worden toegeschreven.

Matched historische kosten:

- Experiment 2: zes judges, 40.024 input + 6.835 output = **46.859 tokens**.
- Experiment 3 judges alleen: zes calls, 48.744 input + 10.515 output = **59.259 tokens**.
- Drie historische maps: drie calls, 12.860 input + 8.944 output = **21.804 tokens**.
- Historisch totaal: negen calls, **81.063 tokens**, **+34.204 / +73,0%**, +50% calls.

Die extra kosten omvatten zowel mapgeneratie als het opnieuw aanbieden van de map aan beide judges. Hergebruik van één map voor twee beoordelingen is werkelijk toegepast. Voor één los Research-resultaat zijn twee calls nodig; bij veel beoordelingen met onveranderde bron/Idea kan de generatiekost verder worden gedeeld, maar de map blijft inputtokens kosten en gedeelde mapfouten blijven risico. Geen live productieprijs of latentieverbetering bewezen.

Alle zes maps kosten samen 26.779 tokens; de twaalf judges 70.258. Nieuwe holdoutfase inclusief maps: 15.974 tokens. De hele run gebruikt 33.673 tokens meer dan Experiment 2 (+53,1%), maar met een andere set en meer calls. Reasoningtokens zitten al in outputtokens en worden niet dubbel geteld; cached/cache-write-details staan ongewijzigd in usage.json. Geen betrouwbare geldprijs vastgesteld of verzonnen.

### Volledig callledger

Werkelijke volgorde: eerst intro/005/010/garden/kiln/catalogue-map, daarna per paar bad en correct. Hieronder zijn de kosten per paar gegroepeerd, niet als uitvoeringstijdlijn.

| Call-id | Inputtokens | Outputtokens | Totaal |
|---|---:|---:|---:|
| intro-map | 3986 | 2702 | 6688 |
| intro-bad | 7454 | 1147 | 8601 |
| intro-correct | 7466 | 2607 | 10073 |
| 005-map | 3969 | 3328 | 7297 |
| 005-bad | 8078 | 2497 | 10575 |
| 005-correct | 8328 | 1129 | 9457 |
| 010-map | 4905 | 2914 | 7819 |
| 010-bad | 8619 | 1333 | 9952 |
| 010-correct | 8799 | 1802 | 10601 |
| garden-map | 605 | 943 | 1548 |
| garden-bad | 1294 | 379 | 1673 |
| garden-correct | 1286 | 343 | 1629 |
| kiln-map | 634 | 945 | 1579 |
| kiln-bad | 1330 | 594 | 1924 |
| kiln-correct | 1382 | 175 | 1557 |
| catalogue-map | 610 | 1238 | 1848 |
| catalogue-bad | 1403 | 1159 | 2562 |
| catalogue-correct | 1419 | 235 | 1654 |
| **Totaal, 18/18** | **71567** | **25470** | **97037** |

Alle achttien responses: HTTP 200 en provider-model gpt-6-luna. Iedere poging precies één vastgelegd request, RetryPolicy(0) en handlerlimiet één; globale limiet achttien. Ongeldige maps/verdicts tellen volledig mee. Geen leftovers gebruikt voor tuning. Mislukte catalogue-judge niet opnieuw uitgevoerd.

## Offline verificatie en gelijktijdige externe wijzigingen

- Solutionbuild geslaagd, nul waarschuwingen/fouten. Alle **299 tests geslaagd**: Application 237, Infrastructure 25, Console 37; nul mislukt/overgeslagen. Domain heeft geen discoverable tests. Vanwege externe codewijzigingen is de oplossing na de live run opnieuw gebouwd/getest; build.log en tests.log bewaren de eindcontrole.
- Runnerbuild en offline zelfcontrole geslaagd: accept, dubbele JSON-properties, extra properties en fout bronanker; mappercontrole weigert ongeldig citaat. Geen netwerkclient in de offline replay.
- Alle twaalf antwoorden opnieuw met de actuele productieparser offline afgespeeld: exact dezelfde parse-uitkomsten en verdicts; alle twaalf bewaarde filterprojecties opnieuw met echte SynthesisResearchFilter vergeleken en gelijk. verification-offline-replay.json.
- verification-requests.json: alle achttien requests matchen bron, vijf Idea-velden, juiste prompt/model; mapper ontvangt geen Research; alle judgecalls matchen Research, opgeslagen echte filteroutput en dezelfde ongewijzigde map. Geen labels of mapaudit toegevoegd.
- Finale freeze-bestanden ongewijzigd. Opgeslagen bronankerchecks maken geen semantische bewijsclaim. Bronquotes in diagnoses zijn separaat beoordeeld op relevantie/materialiteit.
- EnableResearchJudge heeft default false; bestaande defaulttest slaagt. Appsettings heeft geen override, evenmin de gecontroleerde relevante user-secretvelden of OpenAI__EnableResearchJudge-omgevingsvariabele. Deze run heeft niets geactiveerd.

**Integriteitsafwijking buiten deze uitvoering:** tussen eerste inspectie en einde werden drie productiecodebestanden extern gewijzigd: ResearchAgent.cs, ResearchSemanticJudge.cs en ResearchValidationException.cs. Alle eigen schrijfacties zijn beperkt tot deze experimentmap. De wijzigingen zijn bewaard in external-working-tree-changes.diff en niet teruggedraaid. Het betreft grotendeels formattering; daarnaast is de null-check voor een blocker uit ResearchSemanticJudge verwijderd. Dat laatste kan foutafhandeling van een null-issue veranderen en mag niet als louter opmaak worden afgedaan. Geen van de twaalf responses bevat null-issues; de twaalf replay-uitkomsten zijn gelijk.

De 103 protected hashes zijn bij de finale freeze vastgelegd, na een deel van die externe edits. De eindcontrole vindt één afwijking ten opzichte van die freeze: ResearchAgent.cs. De git-diff tegenover begin-HEAD toont alle drie bestanden. Daarom is een algemene claim “alle productiecode bytegelijk aan de allereerste inspectie” onjuist. De productieprompts/filter/configuratie zijn ongewijzigd, en er is geen bron voor causale conclusies over invloed van die externe opmaakwijzigingen. runtime-snapshot/ bewaart de runnerkopieën van Application/Domain/Infrastructure met SHA256 en bestandsdatum; live uitvoering gebruikte de lokale runnerassemblies. De achteraf gemaakte snapshot is geen voorafgaande cryptografische runtime-attestatie. De exact opgeslagen requests/responses en offline replay zijn het controleerbare gedragsbewijs. Deze beperking is een extra reden geen productiegeschiktheid te claimen.

## Besluit en concrete vervolgstappen

**NO-GO.** Historisch criterium faalt op intro, mapankercriterium faalt in drie maps en holdoutcriterium faalt op catalogue. De acceptatie van alle correcte controles compenseert die fouten niet. De tweestapsaanpak verbetert hier de zichtbaarheid van foutlocaties, maar verdient de hogere kosten nog niet als betrouwbare kwaliteitsbewaker. Dit is evenmin bewijs dat iedere expliciete maparchitectuur onmogelijk werkt.

1. Laat de referentielabels voor te brede Unknowns en verplichte materiële coverage onafhankelijk beoordelen. Vooral intro vraagt een scherp onderbouwd onderscheid tussen expliciete bronbetekenis en onzeker lezereffect; deze run lost dat niet op.
2. Onderzoek in een nieuw geïsoleerd protocol bronankers via exacte spans/offsets of gevalideerde extractie. Geen stilzwijgende normalisatie of modelherstel in deze afgeronde run. Houd semantische steun en materialiteit als afzonderlijke controles: een geldige substring is onvoldoende.
3. Test een generieke clause-level vergelijking van Unknowns met expliciete bronbetekenissen en beperkingen, met nieuwe contrastgevallen en passende onzekerheid als positieve controle. Neem Idea-fouten afzonderlijk op; maak ze geen Research-blocker als Research ze niet overneemt.
4. Als verder onderzoek wordt geautoriseerd: gebruik een matched vergelijking met dezelfde judgeprompt mét/zonder map, onafhankelijke nieuwe labels en herhalingen binnen een nieuw vooraf vastgelegd budget. Daarmee kan de toegevoegde waarde van de map worden onderscheiden van de promptwijziging. Deze 18 calls niet herhalen of resetten.

Geen productie-integratie of activering uitgevoerd; geen commit/push. Artefacten: protocol.md/freeze.json/labels.json en prompts; cases/; calls/ met attempts, requests, ruwe responses, HTTP-metadata, maps/ankerchecks of verdict/error en echte filteroutput; map-audit.json; results.json/usage.json; verification-*.json; build.log/tests.log; runner en offline-verification. Keys en authheaders zijn niet opgeslagen.
