# Research Judge + compacte Research — 12 november 2026

**NO-GO voor activering en promptpromotie.** De onafhankelijke judge herkent 0/3 vooraf benoemde baselineproblemen. De code voor beoordeling en maximaal één reparatie is geïmplementeerd en met mocks getest, maar de judge staat standaard uit. De compacte prompts zijn uitsluitend een geïsoleerde kandidaat. Geen Copilot CLI, commit of push.

## Scope en vooraf vastgelegde grenzen

Werkdirectory C:\Workspaces\Nueyon.Compose; begin-HEAD `23c7d2b881523ae3cb2af6e2ebf75ac916eb351b`. Beginstatus schoon. Actuele IdeaSemanticJudge, IdeaValidationLoopEvaluator, bijbehorende tests, ResearchAgent/prompts, StoryWorkflow, SynthesisResearchFilter en alle drie baselinebronnen/outputs onderzocht. De expliciete opdracht bevatte de benodigde vervolgcontext; geen aanvullende historische claims uit de gecachete chat gebruikt.

protocol.md is vóór de eerste call geschreven. Maximum 12 HTTP-requests, geen SDK-retries, geen heruitvoering. GPT-6 Luna conform huidige hostconfiguratie. Eerst drie bevroren baselines en drie minimaal gecorrigeerde controles beoordelen. Daarna uitsluitend bij geslaagde kalibratie drie compacte generaties en drie kandidaatbeoordelingen. De runner bewaakt een globale markerlimiet van 12 en één request per poging; bestaande pogingen weigert hij. Candidate vereist een afzonderlijke calibration-approved.md, die vanwege NO-GO niet is aangemaakt.

“Offline kalibratie” betekent hier beoordeling van bevroren lokale data zonder retrieval, Research-generatie of andere workflowagents. De judgebeoordelingen zijn wel echte modelcalls. Ze zijn allemaal hieronder verantwoord. Geen volledige pipeline, Synthesis, live reparatie of externe informatiebron aangeroepen.

## Judge-ontwerp en kalibratie

Vijf generieke dimensies: source fidelity, evidence coverage (confounders/tegenvoorbeelden), epistemic accuracy, selected-Idea relevance, downstream integrity. De input bevat originele source, alle geselecteerde Idea-velden, ResearchResult en exact de productie-filteruitvoer. Alle zes requests zijn hierop gecontroleerd; verification-requests.json rapporteert per veld en onderdeel een match. Systemtekst komt exact uit de bevroren promptkopie.

JSON-contract: accepted plus maximaal vijf blockingIssues met category, researchLocation, problem, sourceQuote, constraint. Strenge parser weigert onbekende velden, dubbele properties, ontbrekende velden, ongeldige categorieën, inconsistente acceptatie en citaten die niet letterlijk in de originele bron voorkomen. Dit controleert aanwezigheid van bronankers, niet de semantische juistheid van een diagnose of de afwezigheid van nieuwe feiten in vrije tekst. Die laatste eis staat in de rubric en is inhoudelijk beoordeeld. Technische/verkeerde judgeantwoorden stoppen direct en vormen geen reparatiebewijs.

De judge krijgt geen caselabels, bekende fouten of verwachte antwoorden. De rubric is niet na deze resultaten aangepast. Positieve controles wijzigen uitsluitend het bekende kwalificatieprobleem of voegen de bekende ontbrekende beperkingen/tegenproef toe; alle oorspronkelijke Facts blijven behouden. Dit zijn gerichte controles, geen onafhankelijk gecertificeerde foutloze documenten.

| Case | Baselineverdict | Bekend probleem herkend? | Controleverdict | Beoordeling diagnose |
|---|---|---|---|---|
| intro | pass | Nee: te brede Unknown 4 gemist | pass | Correctie onderscheidt bronuitdrukking van onbewezen lezereffect. Geen correcte Facts afgewezen. |
| 005 | fail | Nee: upstreamvariatie gemist | fail | Beide keren andere, brononderbouwde lacune: spanning in oorspronkelijke Synthesis ontbreekt. Geen claim dat correcte Fact 9 onwaar is. |
| 010 | pass | Nee: Compose-tegenvoorbeeld ontbreekt | pass | Geen onderscheid tussen onvolledige baseline en aangevulde controle. Geen correcte Facts afgewezen. |

005 sourceQuote verwijst exact naar de alinea waarin baseline-Synthesis orchestration “central to the product direction” noemt en de bron slechts zegt dat dit de technische invalshoek mogelijk versterkte. Die diagnose is verdedigbaar; niet als valse afwijzing van correcte feiten tellen. De gerichte 005-controle verhelpt die extra lacune niet, dus haar afwijzing bewijst op zichzelf geen false positive. De kalibratie faalt al doordat de drie vooraf gespecificeerde kritieke issues alle worden gemist. Target-recall in deze kleine steekproef 0/3; geen algemene precisie/recall of betrouwbare false-positive-rate afleidbaar.

Concrete bronankers voor verwachte issues:
- Intro §11: “Nuëyon.Compose is the first step because it gives the broader vision a concrete expression.” Bronuitdrukking is wél gevestigd; effect op lezers niet. Baseline Unknown 4 ontkent te veel en bereikt Synthesis.
- 005 Limitations: “Idea and Research outputs vary between runs.” Identieke workflowinputs betekenen niet identieke tussenresultaten. Vergelijkingsdatums zijn verschillend en verdwijnen uit de baseline bij filtering van Development sequence.
- 010 Experiment 009 / Baseline 001: “Selected Idea and earlier chain remain company-first”; downstreamhiërarchie 2/2, feitelijke overclaim pas bij Compose. De tegenproef moet feitelijke afwijking onderscheiden van hiërarchieverlies.

## Compacte kandidaat en vergelijking

Kandidaatbestanden staan in prompts/Research/. Eén Interpretations-definitie: semantische compressie van geïdentificeerde Facts zonder nieuwe betekenis, relatie of zekerheid. Positieve onderzoeksinstructies richten zich op bronbewijs, relevante tegenvoorbeelden/confounders en precieze onzekerheid; essentieel bewijs moet in Facts/Unknowns staan. Geen formatkeuzes voor pre-Compose agents toegevoegd.

ResearchResult, vijf headings en SynthesisResearchFilter zijn ongewijzigd. De bestaande productie-Researchprompts matchen bytegetrouw de baselinekopieën (verification-prompts.json). Kandidaatprompts zijn niet naar productie gekopieerd. Door de mislukte kalibratie zijn fase 3-generaties en kandidaatbeoordelingen conform protocol niet gestart. Er is dus **geen gemeten kandidaatkwaliteit, geen kandidaat-outputtokenvergelijking en geen bewezen tokenbesparing**.

De sjablonen bevatten samen 2.384 tekens tegenover 49.075 baseline-tekens (ongeveer 95,1% minder sjabloontekst, exclusief ingevulde bron/Idea). Dit is geen token- of kwaliteitsmeting. prompt-size.json bevat aantallen per bestand.

Beoordeling bevroren baseline door dezelfde assistent, niet geblindeerd, geen onafhankelijke menselijke labels. Schaal 0 materieel probleem; 1 beperking; 2 adequaat voor deze case. Brontrouw en onzekerheid apart om correcte Facts niet als onwaar te scoren wegens een omissie.

| Baseline | Fidelity | Coverage | Uncertainty | Downstream usability | Motivering |
|---|---:|---:|---:|---:|---|
| intro | 1 | 2 | 0 | 0 | Facts grotendeels trouw; Unknown 4 verzwakt expliciete bronbetekenis in overdracht. |
| 005 | 2 | 0 | 1 | 0 | Waarnemingen/scores trouw; relevante upstreamvariatie en datums ontbreken downstream. Extra Synthesis-spanning eveneens niet gedekt. |
| 010 | 2 | 0 | 2 | 0 | Lokalisatie terughoudend; essentiële negatieve controle en onderscheid ontbreken. |
| Compacte kandidaat | n.v.t. | n.v.t. | n.v.t. | n.v.t. | Niet gegenereerd na kalibratie-NO-GO. |

Deze scores zijn diagnosehulpmiddelen, geen onafhankelijke validatie of objectieve totaalscore. Geen downstreammodel uitgevoerd; usability betreft informatieoverdracht door de filter, niet gerealiseerde redactionele kwaliteit.

## Integratie en tests

ResearchAgent accepteert optioneel een aparte ResearchSemanticJudge, met hetzelfde scheidingsprincipe als Idea: onafhankelijke modelcall met gestructureerde diagnose, originele bron blijft leidend. Implementatie gebruikt een expliciete begrensde lus in ResearchAgent in plaats van Idea's LoopAgent-wrapper: maximaal twee Research-generaties en twee beoordelingen, verse sessies en lokale toestand per uitvoering. Geen extra framework-iteratie mogelijk. Pass retourneert Research en geeft toegang tot bestaande Synthesis-handoff. Fail levert één gerichte reparatie met originele bron/Idea, vorige Research en JSON-diagnoses; die wordt opnieuw beoordeeld. Tweede afwijzing geeft ResearchValidationException met attemptCount, Structural/Semantic en beknopte blockers zonder sourceQuote. Bij structurele fout volgt maximaal één schema-/headingreparatie; geen judgecall voor onbruikbare Research.

Exact de vijf bestaande headings worden gevalideerd vóór beoordeling wanneer de gate aanstaat; de filter zelf is niet gewijzigd. Ontbrekende/duplicaat-headings geven structurele feedback. Cancellation en judge-transport/JSON/ankerfouten worden niet als inhoudelijke afwijzing herprobeerd.

Infrastructure configureert dit met `OpenAI:EnableResearchJudge` (standaard false), hetzelfde ingestelde model voor generator/judge, aparte AIAgent-instanties. Vanwege NO-GO **niet activeren voor normaal gebruik**. Aanroepen met de bestaande constructor blijven ongekeurd zoals vóór deze wijziging. Dit is een geteste experimentele integratie, geen werkende productiegarantie voor inhoudelijke detectie.

StoryWorkflowResultExtractor gaf voorheen alleen Idea-executorfouten door; Research-fouten werden een algemene “zonder Research result”-fout. Research wordt nu eveneens met behoud van type doorgegeven. Integratietests bevestigen dat geen Synthesis/Narrative/Compose start bij definitieve Research-afwijzing.

23 nieuwe Application-testcases met IChatClient/AsAIAgent: pass, gerichte repair/pass, repair/fail, origineel/Idea/filterinput, frisse judgecontext, calls begrensd, malformed/duplicate/unknown JSON, ongeldige diagnoses/bronankers, blockerlimiet, structurele reparatie, headings, cancellation, transportfout, gedeelde agentuitvoering en echte MAF-workflowhandoff. Drie Infrastructure-testcases dekken default uit en serviceconstructie met beide instellingen. CLI-promptsettest aangepast van 12 naar 13 resources en expliciete aanwezigheid van judgeprompt gecontroleerd.

Definitieve validatie: `dotnet build src/Nueyon.Compose.slnx` geslaagd, 0 waarschuwingen/fouten. `dotnet test src/Nueyon.Compose.slnx --no-build`: **299 geslaagd** (Application 237, Infrastructure 25, Console 37), 0 mislukt. Domain-project bevat geen discoverable tests. Eerste volledige run vond een verouderde CLI-resourcecount (12 versus 13), vervolgens hersteld en volledige suite opnieuw groen. `git diff --check` schoon. Geen live modelcalls tijdens tests.

## Alle modelcalls en kosten

| Call | Conditie | Case | Inputtokens | Outputtokens | Totaal |
|---:|---|---|---:|---:|---:|
| 1 | baseline judge | intro | 5633 | 1359 | 6992 |
| 2 | baseline judge | 005 | 6128 | 2168 | 8296 |
| 3 | baseline judge | 010 | 6872 | 1469 | 8341 |
| 4 | control judge | intro | 5645 | 1032 | 6677 |
| 5 | control judge | 005 | 6276 | 2559 | 8835 |
| 6 | control judge | 010 | 7052 | 2089 | 9141 |
| **Totaal** | **6/12 benut** | | **37606** | **10676** | **48282** |

Elke call exact één HTTP-request, status 200, provider-responsemodel gpt-6-luna. Alle tokens zijn judgeoverhead; er zijn geen nieuwe Research-tokens. Baseline uit eerder experiment: 38.480 input + 6.251 output = 44.731 tokens voor drie Research-calls; niet opnieuw uitgevoerd en niet bij bovenstaande nieuwe calls opgeteld. Reasoning/cached details staan in de ruwe providerresponses; niet nogmaals bij total opgeteld. Betrouwbaar tarief/factuurbedrag ontbreekt; daarom geen verzonnen geldschatting. Geen onnodige prijscalls.

Per conditie/case: attempt-started.json, request.json, response-raw.json, http-metadata.json, geparseerd verdict.json, research-result.json en echte synthesis-filter-output.md. API-key/headers niet opgeslagen. usage.json is het callledger. Protocol, bevroren prompts, runner, verificaties en dit rapport staan in deze nieuwe experimentmap. Bevroren baselinebestanden niet gewijzigd; geen kandidaat naar productie gepromoveerd.

## Gerichte vervolgstappen

1. Maak een volgende, afzonderlijke judgeversie die eerst een brongeankerde coverage-map maakt van belangrijke Idea-proposities, relevante tegenvoorbeelden en causale beperkingen, en die vervolgens vergelijkt met de werkelijke Facts/Unknowns. Test een bewering-voor-bewering Unknown-contradictiecheck. Dit zijn hypothesen, geen bewezen reparaties; gebruik generieke instructies zonder casespecifieke antwoorden.
2. Breid controles uit met onafhankelijke labels, correcte relevante facts en geïsoleerde contrastparen. De huidige 005-controle is bewust slechts doelgericht gecorrigeerd en blijkt nog een verdedigbare lacune te hebben. Scheid tuningset van nieuwe holdouts; verander deze runmarkers niet en voer deze calls niet opnieuw uit.
3. Leg voor die nieuwe proef opnieuw een klein budget vast. Pas na geslaagde kalibratie dezelfde drie compacte Research-inputparen uitvoeren en fidelity, coverage, uncertainty, filtered usability en echte tokens vergelijken. Daarna pas beslissen over promotie en een afzonderlijke end-to-end proef. Geen bewijs dat een judge met deze rubric al betrouwbaar kwaliteitsbewaking levert.

## Bestaande formatgrens: resterende beperking

De nieuwe Research-kandidaat en judge krijgen geen outputformat en sturen niet op platform, lengte, hooks of publicatiestijl. Audit van de overige actuele pre-Compose-prompts laat echter een bestaande inconsistentie zien: Narrative.user.prompt.md bevat nog “The story direction must work for both Article and LinkedIn formats.” Dit bestond vóór deze wijziging en is in deze Research-proef niet gewijzigd om geen andere workflowvariabele mee te veranderen. Er is daarom geen bewijs of claim dat de volledige bestaande pre-Compose-keten nu overal vrij van formatverwijzingen is. Een aparte gerichte Narrative-boundarywijziging blijft nodig als strikte afwezigheid van die verwijzingen vereist is. Bronvermeldingen van een format mogen als brondata behouden blijven; ze geven Research geen formatontwerptaak.
