# Idea Generator Experiment 003

## Opzet

Kandidaat 001 behoudt de systemprompt byte-identiek en voegt op twee plekken regels toe aan de Idea-userprompt. De regels scheiden het hoofdwerk van begeleidende tekst, toetsen claims tegen specifieke passages en vragen om één concrete lezersbelofte binnen het redactionele doel. De SourceContent-placeholder gebruikt PromptRunner-syntaxis. Alleen de ongewijzigde bron en de bronmarkers gaan naar het model. Het manifestdoel is alleen gebruikt voor beoordeling.

De batch loopt in manifestvolgorde, hoogstens één PromptRunner-proces per bron. Binnen dat proces mag de productievalidatielus maximaal drie modelpogingen doen. Bij een ernstige brontrouwfout, uitvoeringsfout of onverwachte wijziging stopt de batch. Elke volgende batch of promptiteratie vereist toestemming.

## Uitvoering en vergelijkbaarheid

Experiment 001 gebruikte vrije tekst zonder productievalidatie. De nieuwe Idea-modus van PromptRunner laadt de Markdown met de productie-SourceContextLoader, gebruikt dezelfde OpenAI-chatclient en IdeaResponse-schemageneratie, en hergebruikt IdeaValidationLoopEvaluator, IdeaValidator en LoopAgent met MaxIterations 3. De runner deserialiseert het eindantwoord hoofdletterongevoelig en kiest index 0 zoals de productie-workflow. De structurele validator toetst geen inhoudelijke brontrouw. De runner rendert de losse promptbestanden, terwijl de productieagent ingebedde prompts laadt. De runner legt antwoordtekst en SDK-tokengebruik per validatiepoging vast. De provider geeft geen bedrag per aanroep terug.

Baseline-002 draaide de volledige Compose-pipeline en haalde 1 van 15 op de gecombineerde toets. De eerste batch van Experiment 003 is een Idea-screening zonder Research, Synthesis en Narrative. De uitkomsten zijn beperkt vergelijkbaar. De vijf Idea-scores, twee inhoudelijke gates en ernstige brontrouwfouten zijn beoordeelbaar. De Narrative-check en de acceptatiebeslissing blijven open.

Modelalias gpt-5-mini, OpenAI Chat, standaard samplinginstellingen. Zie preflight.json voor commit, SDK, bibliotheken en SHA-256. Onder runs staan de bevroren invoerkopieën, gerenderde prompts en artefacten van uitgevoerde runs.

## Uitkomst eerste batch

**Vroegtijdig gestopt na 2 van 5 geplande runs.** Beide PromptRunner-processen eindigden met code 0 en hadden één modelpoging. De tweede run bevatte een ernstige inhoudelijke brontrouwfout. De overige drie bronnen zijn alleen voorbereid en gerenderd. Er is geen volgende run of iteratie gestart.

| Bron | Geselecteerd idee in het kort | Brontrouw | Doel | Focus | Invalshoek | Aantrekkingskracht | Totaal | Tokens | Kostenindicatie USD |
| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: |
| [Productstrategie](runs/01-strategy-r1/evaluation.json) | Nuëyon als AI-first bedrijf met Compose als eerste concrete stap | 1 | 1 | 2 | 1 | 1 | 6 | 11.113 | 0,00651 |
| [Ontwikkelverslag](runs/02-development-r1/evaluation.json) | Een generieke regel voor redactionele intentie kan uitvoer minder trouw maken | 0 | 2 | 2 | 2 | 2 | 8 | 11.190 | 0,00669 |
| Productdocumentatie | Open | | | | | | | | |
| Diagnostische analyse | Open | | | | | | | | |
| Historische rede | Open | | | | | | | | |

Beide ideeën missen de Idea-drempels. De strategie verbreedt het publiek. Het ontwikkelverslag gebruikt de lagere kandidaatscores als basis voor een algemene uitspraak over betrouwbaarheid en suggereert causale verslechtering. De bron rapporteert drie runs per conditie, zonder statistische significantie, en noemt runvariatie. Zie de evaluatiebestanden voor bronregels en motivering per criterium. Eén beoordelaar scoorde deze screening.

Totaal 22.303 tokens en circa USD 0,01319. De schatting gebruikt de SDK-telling en het openbare standaardtarief per miljoen tokens van USD 0,25 voor ongecachete invoer, USD 0,025 voor gecachete invoer en USD 2,00 voor uitvoer. [OpenAI gpt-5-mini](https://developers.openai.com/api/docs/models/gpt-5-mini).

## Verificatie

De PromptRunner-build slaagde met nul waarschuwingen en fouten. De systemprompt-hash komt ook overeen met het ingebedde Baseline-002-resource. De vijf invoerkopieën en gerenderde prompts komen overeen met de preflight-hashes. Voor beide runs zijn alle vijf velden van de opgeslagen selectie gelijk aan de eerste Idea in de ruwe eindrespons. De artefactscan vond geen lokale API-sleutel of sleutels in het verwachte tokenformaat. Git bevestigde dezelfde HEAD en geen wijzigingen onder productiebroncode of productieprompts. De enige codewijzigingen zitten in PromptRunner. Geen commit of push.
