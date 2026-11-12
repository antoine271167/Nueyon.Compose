# Research-baseline — GPT-6 Luna, 12 november 2026

**Uitkomst:** technisch geslaagd, inhoudelijk bruikbaar vertrekpunt met belangrijke dekkingslacunes; geen bewijs van algemene Research-betrouwbaarheid. Exact drie Research-modelcalls, één per bevroren testcase. Geen andere agents uitgevoerd en geen productieaanpassing.

## Opzet en integriteit

Repo: C:\Workspaces\Nueyon.Compose; origin https://github.com/antoine271167/Nueyon.Compose.git; lokale HEAD 4a96d9d9e6dc0df4790c1e047c3e3507c4b49dfa. Beginstatus schoon. Geen fetch uitgevoerd; dit rapporteert de actuele lokale checkout, niet een geverifieerde actuele GitHub HEAD.

De laatste commit migreert Research.system.prompt.txt en Research.user.prompt.txt naar .prompt.md met 0 toegevoegde/verwijderde inhoudsregels (100% rename). Load/render-paden, embedded resources en host-copyregels gebruiken .md. De runner is opnieuw gebouwd, gebruikt expliciet de huidige diskpromptmap via PromptDirectory.Use en roept de productie-ResearchAgent aan. De werkelijk verstuurde systemtekst matcht het diskbestand in alle drie requests. De usertekst wordt door de productie-CreateUserMessage/PromptLoader gerenderd; het volledige request met beide berichten en JSON-schema staat per case in request.json. Promptkopieën staan in prompts/.

Idea-selecties komen bytegetrouw uit PromptExperiments/IdeaGenerator/luna-simplified-comparison-2026-11-12/{intro,005,010}-selected.json. Bijbehorende bronnen komen uit de daar gedocumenteerde legacy-simplified-acceptance-2026-11-12/{case}-input.json. Intro is de eerder bevroren reconstructed brief, geen oorspronkelijke onbewerkte auteursbron. 010 gebruikt de definitieve geaccepteerde reparatie: description noemt alleen Idea, Synthesis en Narrative als eerste hiërarchieafwijkingen; evidence onderscheidt de Compose-feitelijke fout en de behouden hiërarchie expliciet.

Een minimale runner staat uitsluitend in deze experimentmap. Hij gebruikt dezelfde OpenAI Chat-client/MAF-route, ResearchAgent, ResearchResult-parser en de echte SynthesisResearchFilter.ExtractFactsAndUnknowns. Afwijkingen voor meetbaarheid: HTTP-bodycapture, RetryPolicy(0), maximaal één HTTP-request per case, marker tegen heruitvoering. Geen externe retrieval/tools. Geen eigen generatie-, judge- of reparatielus. Geen GitHub Copilot CLI. Build: 0 fouten, 0 waarschuwingen.

172 gevolgde bestanden vóór/na gehasht: geen wijziging. Git meldt alleen de nieuwe experimentmap. Geen commits of pushes. verification-repository.json en before-hashes.json ondersteunen dit.

## Resultaten en concrete evidence

| Case | Brontrouw, causaliteit en scope | Dekking / voornaamste beperking |
|---|---|---|
| Intro | Bedrijfsambitie, beoogde AI-first doelgroep en vroege productstatus behouden. Facts 2–7 blijven ambitie; Facts 9–11 onderscheiden Compose van het portfolio in ontwikkeling. Unknowns 1–3,5 voorkomen claims over tractie, beschikbaarheid en gegarandeerde levering. | 23 facts, geen interpretaties. Unknown 4 verwerpt het “tangible” maken van de ambitie te breed. |
| 005 | Vergelijkingsuitkomsten blijven gerapporteerde observaties. Facts 16–21 bewaren scores en gemiddelden; Fact 30 beperkt ze tot deze steekproef. Unknowns onderscheiden onbekende oorzaak, generaliseerbaarheid, significantie en mogelijk gewijzigde modelbackend. Geen bewezen promptcausaliteit geclaimd. | 30 facts, geen interpretaties. Essentiële upstreamvariatie uit bron ontbreekt; beperkingen van causale attributie zijn daardoor onvolledig. |
| 010 | Facts 3–5 behouden ontbrekende Idea/Research-extracten voor 007; Facts 6–18 lokaliseren verschillende waargenomen startpunten. Fact 19 en Unknowns wijzen universele oorzaak en interne modelcausaliteit af. Geen Narrative-first verbeterprioriteit of rendement verzonnen. | 21 facts, geen interpretaties. Compose-tegenvoorbeeld uit hersteld idee ontbreekt volledig; onderscheid feitelijke fout versus hiërarchie wordt niet expliciet overgedragen. |

### Intro: conservatisme kan expliciet bronbetekenis afzwakken

Research Unknown 4: “The source does not establish that juxtaposing the broad company ambition with Compose makes the ambition tangible; this is an editorial assessment in the Selected Idea.”

De bron zegt in §11: “Nuëyon.Compose is the first step because it gives the broader vision a concrete expression.” §5 en §14 noemen Compose eveneens de eerste concrete uitdrukking/stap. Research behoudt die status in Fact 13. Niet bewezen is een effect op lezers, maar de bron biedt wél steun voor het concrete product als uitdrukking van de ambitie. Het gehele redactionele verband als onbekend afwijzen is dus te breed. Dit is een kwalificatieprobleem, geen verzonnen bedrijfsfeit. Unknown 4 bereikt Synthesis ongewijzigd.

Volledigheid is passend selectief: geen reden om elke portfolio-optie voor dit idee te herhalen. Onbekende operationalisering van “AI-only” had de relevante Facts 6–7 wel explicieter kunnen begrenzen. De bron zegt dat de precieze operationele betekenis nog niet is gedefinieerd.

### 005: sterke uitkomstregistratie, ontbrekende attributiebeperking

Bron, Limitations: “Idea and Research outputs vary between runs.” Ook baseline- en kandidaatruns zijn niet gelijktijdig geproduceerd. Research noemt wel mogelijke backendverandering in Unknowns, maar upstreamvariatie ontbreekt zowel in Facts als Unknowns; de datums staan alleen in Development sequence en verdwijnen bij filtering.

Hierdoor kan een downstreamlezer Fact 9 (“other prompt files and workflow inputs remained identical”) ten onrechte lezen alsof ook de interne stage-inputs gelijk waren. Die fact is een getrouwe weergave van de bron, maar behoeft voor deze causale invalshoek de expliciete upstreambeperking ernaast. Lagere scores bewijzen niet dat uitsluitend de gewijzigde Narrative-regel de verschillen veroorzaakte. Research doet die causale claim zelf niet.

Facts zijn meestal brongetrouw maar niet steeds strikt atomair: bijvoorbeeld Fact 7 bundelt meerdere instructies en Fact 27 meerdere afwijsredenen. Geen duidelijk nieuw semantisch verband vastgesteld, wel afwijking van het eigen minimale-propositiecontract. De titelmetadata in Fact 1 is minder relevant dan de weggelaten confounder.

### 010: correct gelokaliseerd, belangrijke negatieve controle ontbreekt

Bron, Experiment 009 / Baseline 001: eerste gerapporteerde feitelijke fout bij Compose (“repeatable process that reliably converts”), terwijl “Selected Idea and earlier chain remain company-first” en de downstreamhiërarchie 2/2 scoort. Dit staat ook expliciet in de herstelde Idea-evidence.

Research herhaalt de oorspronkelijke fout niet: Compose wordt nergens een eerste hiërarchiebreuk genoemd. Maar het bewaart evenmin de positieve tegenproef die uitlegt waarom Compose hier anders moet worden geclassificeerd. Daardoor is de voor het herstel cruciale scheiding tussen feitelijke en redactionele afwijking onvolledig in de kennisbasis. De bron beveelt verder aan de werkelijk ontvangen Synthesis-input te vergelijken vóór wijziging van Narrative; Research draagt dat niet over als bestaande aanbeveling of als onbewezen interventiewerking. Er wordt wel terecht geen effect van een ingreep geclaimd.

Actor- en zekerheidsscope blijven in de drie samples overwegend intact: geen nieuwe klanten, stakeholders of bewezen marktuitkomsten; evaluatiebevindingen worden veelal toegeschreven aan het rapport. Bronrapporten zijn hier de bewijsgrens; hun onderliggende experimenten zijn niet opnieuw onafhankelijk gevalideerd.

## Filter, headings en redactionele relevantie

Alle drie antwoorden gebruiken exact de vijf vereiste headings, in de juiste volgorde. verification-filter.json vergelijkt de echte filteruitvoer met de complete Facts- en Unknowns-tekst: 3/3 match. Geen weggevallen fact of unknown door parsing/headingvariatie waargenomen.

| Case | Volledige Content (tekens) | Filteruitvoer (tekens) |
|---|---:|---:|
| Intro | 3723 | 2463 |
| 005 | 5288 | 4204 |
| 010 | 5059 | 3359 |

De verwijderde secties zijn conform ontwerp Interpretations, Development sequence en Editorial relevance. Alle Interpretations zijn “None”. Editorial relevance blijft een bronindex, zonder nieuwe strategie/impactclaims, maar die index bereikt Synthesis niet. Vooral 005 illustreert semantisch verlies bij een technisch correcte filter: de precieze vergelijkingsdatums staan alleen in de verwijderde sequence. De upstreamvariatie en 010-tegenproef zijn al vóór het filter afwezig; dit zijn Research-dekkingsfouten, geen filterbugs.

Statisch blijft de filter kwetsbaar: ordinale substringzoekopdrachten op vaste headings, geen structurele headingvalidatie en geen fail-closed gedrag wanneer Facts/Unknowns ontbreken. Dat risico trad in deze drie samples niet op. Geen afwijkende-headingcall of Synthesis-call uitgevoerd.

## Promptspanning en geprioriteerde verbeterhypothesen

1. **P1 — Bewaar cruciale bewijsgrenzen en tegenvoorbeelden in Facts/Unknowns.** Test later een generieke verplichting om causale confounders, relevante negatieve controles en expliciete onderscheidingen uit bron/Idea te dekken. Acceptatievoorbeelden: 005 upstreamvariatie en ongelijke meetdatums; 010 Compose-feitelijke fout versus behouden hiërarchie. Verander geen oorzaak in een bewezen effect.
2. **P1 — Harmoniseer het interpretatiecontract.** System §3 staat niet-expliciete maar conservatief afgeleide conclusies/relaties toe (“can reasonably be derived from source Facts”; voorbeeld: “The two changes appear related”). User STEP 5 en OUTPUT STRUCTURE staan alleen semantische compressie toe, zonder nieuwe relatie. Dit is een werkelijke instructiespanning. Alle drie samples kozen “None”; geen overtredende interpretatie waargenomen en geen causale koppeling tussen spanning en fouten bewezen. Kies bij een latere kandidaat één definitie; maak onderscheid tussen expliciet bronverband, compressie en echte inferentie.
3. **P2 — Kwalificeer Unknowns nauwkeuriger.** Onderscheid “bron stelt X expliciet” van “effect van X op lezers is niet bewezen”. Intro Unknown 4 is het concrete regressievoorbeeld; niet elke redactionele samenvatting is ontbrekend bewijs.
4. **P2 — Maak de overdracht controleerbaar.** Onderzoek later headingvalidatie/gestructureerde secties en een controle dat essentiële beperkingen niet uitsluitend in weggefilterde views staan. Deze baseline rechtvaardigt geen bredere handoff zonder bewijs: de huidige filter werkte technisch 3/3 correct.
5. **P3 — Minder redundantie, betere atomiciteit.** Selecteer relevante bewijsproposities vóór metadata en herhaling; splits samengestelde facts waar nodig. Bewaar bronankers om volledige dekking per belangrijke Idea-claim toetsbaar te maken.

Dit zijn hypothesen voor een afzonderlijke vervolgproef, geen doorgevoerde wijzigingen of bewezen verbeteringen. Eén sample per bron, dezelfde assistent als evaluator, niet geblindeerd; geen statistische vergelijking, herhaalbaarheidstest of downstreamkwaliteit aangetoond.

## Werkelijke calls, kosten en grenzen

| Case | Research HTTP/modelcalls | Inputtokens | Outputtokens | Totaal |
|---|---:|---:|---:|---:|
| Intro | 1 | 12496 | 1849 | 14345 |
| 005 | 1 | 12559 | 2059 | 14618 |
| 010 | 1 | 13425 | 2343 | 15768 |
| Totaal | **3** | **38480** | **6251** | **44731** |

Provider-responsemodel is telkens gpt-6-luna; drie succesvolle HTTP-responses. RetryPolicy=0 en de capture-handler tellen daadwerkelijk één request per case. Geen metadata-/prijsmodelcalls, geen overige modelgeneraties. Cache- en reasoningdetails staan in usage.json en de ruwe responses; geen aparte optelling bovenop total_tokens. Factuurbedrag en tarief ontbreken in de providerresponses en zijn niet vastgesteld; daarom geen fictieve euro/dollarraming. Er was geen vooraf vastgestelde geldlimiet, wel een harde calllimiet van drie, volledig benut. Geen technische blokkades tijdens uitvoering.

## Lokale artefacten

Root: C:\Workspaces\Nueyon.Compose\PromptExperiments\Research\luna-baseline-2026-11-12

Per intro/, 005/, 010/:
- source-input.json en source.md: exact bevroren bronbestand en uitgelezen brontekst.
- idea-input.json: exact bevroren geselecteerd eindidee; research-input.json: typed agentinput.
- request.json: exact HTTP-requestbody, inclusief gerenderde prompts en outputschema; geen API-key/header opgeslagen.
- response-raw.json: ruwe provider-JSON, inclusief message.content, model en usage.
- research-result.json en research-content.md: daadwerkelijk geparste ResearchResult en Content.
- synthesis-filter-output.md: daadwerkelijk door de productie-filter teruggegeven tekst, zonder Synthesis uit te voeren.
- attempt-started.json, metadata.json, http-metadata.json: tijd, limiet, requesttelling, HTTP-status en responseheaders.

Overig: prompts/, runner/, usage.json, verification-filter.json, verification-repository.json, before-hashes.json, head.txt, status-before.txt. De runner weigert bestaande attempts te herhalen; niet opnieuw starten voor meer live calls binnen deze opdracht.
