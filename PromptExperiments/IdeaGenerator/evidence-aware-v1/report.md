# Evidence-Aware Idea Generation Experiment v1

**11 november 2026 — NO-GO voor B en C.** Deze proef toont geen duidelijke verbetering boven de bestaande productie-Idea Generator. Op tien bronfamilies slagen **A 3/10, B 3/10 en C 2/10**; op de acht nieuwe holdouts respectievelijk3/8,3/8 en2/8. B heeft drie andere successen dan A, maar verliest ook drie gevallen waarop A slaagt. C levert geen robuuste extra succesvolle correctie van het geselecteerde eerste idee. De productie-Idea Agent, prompts en JSON-contracten zijn ongewijzigd. **Geen commit, push of productiepromotie.**

Het herbruikbare consoleharnas en zijn tests zijn wel voltooid en blijven geïsoleerd beschikbaar in [src/Nueyon.Compose.Experiments](../../../src/Nueyon.Compose.Experiments/) en [src/Nueyon.Compose.Experiments.Tests](../../../src/Nueyon.Compose.Experiments.Tests/), toegevoegd aan de bestaande solution. Alle C#-, project-, test- en solution-edits zijn via GitHub Copilot CLI uitgevoerd.

## Opzet en herkomst

Start op de toen actuele GitHub HEAD **e9f302ada8052635094bfa3b5297e7b76a93cdea**; `git fetch origin` bevestigde gelijkheid met origin/main en de werkboom was aanvankelijk schoon. Eerdere v4-, validator-acceptance-v1/v2- en specialist-judges-v1-rapporten en de huidige MAF/IChatClient/Idea-infrastructuur zijn geïnspecteerd. De twee moeilijke regressiebronnen zijn de **exacte v4-contentstrings** van005 en010, inclusief wrappers. Acht nieuwe, synthetische bronfamilies zijn keramiekreparatie, koor, wetland, bakkerij, textielcoöperatie, observatorium, boomgaard en vertaalpilot.

Er is één B-prompt en één reviewprompt opgesteld, vóór de nieuwe holdoutbronnen. Geen betaalde tuning, selectie uit promptkandidaten of aanpassingen na resultaten. De twee oude bronnen zijn ontwikkel-/regressiemateriaal; de acht nieuwe bronnen zijn apart gemarkeerde holdouts. Vier vooraf gekozen families (005,010,keramiek,koor) krijgen één extra replicaat. Dat zijn herhalingen, **geen vier onafhankelijke extra families**. Alleen de eigen bron bereikt elke modelcontext; labels, eerdere resultaten en andere cases worden niet meegestuurd.

[Protocol](protocol.md), [rubric](rubric.md), [cases](cases.json), [vooraf vastgelegde bron-/doelgrenzen](labels/boundaries.json), [inspectie](inspection.md). **187 bestanden** — protocol, labels, bronnen, prompts, schema’s, scripts, broncode en uitvoerbare bibliotheken — zijn vóór de eerste live call gehasht. De freeze is van21:51:59UTC; de eerste reservering van21:52:01UTC. Alle hashes en HEAD zijn na afloop gelijk gebleven. [Freeze](verification/frozen.json), [integriteit](verification/integrity.json).

## Wat is daadwerkelijk uitgevoerd?

| Conditie | Uitvoering |
|---|---|
| A | Ongewijzigde productie-IdeaAgent, productieschema, structural validator, bestaande semantic judge en MAF-herhaallus; maximaal drie generaties met bijbehorende beoordelingen |
| B | Eén nieuwe evidence-aware MAF-generatie; experimentele ideeën plus expliciete claim→sourceEvidence→inference/constraints-records voor alle vijf velden van het eerste idee |
| C | Eén afzonderlijke review/herzieningscall in een nieuwe context, op de **exact bewaarde B-draft** plus oorspronkelijke bron; volledig herzien experimenteel resultaat en reviewrecords |

Alle gebruiken dezelfde bestaande OpenAI Chat Completions/IChatClient-adapter, configuratieroute en credentials, alias **gpt-5-mini**, zonder temperatuur- of seedoverride. Iedere respons meldt **gpt-5-mini-2025-08-07**. De recorder legt voor alle condities dezelfde limiet van6.000 outputtokens op; SDK-retries staan op0. A behoudt dus de productieprompts, het contract en de workflow, met deze gedeelde experimentele tokenlimiet. B/C vragen een apart JSON-schema; een strikte lokale validator weigert onbekende/dubbele/ontbrekende velden, lege waarden, verkeerde cardinaliteit, niet-onderbouwde veldnamen en niet-letterlijke broncitaten. Een matching quote bewijst nog geen inhoudelijke rechtvaardiging.

C wordt volgens het bevroren protocol zonder call geblokkeerd als B niet door de outputvalidatie komt. Daarom test deze proef **niet** of een review een ongeldige B-draft alsnog zou kunnen redden. C is ook geen onafhankelijke nieuwe generatie. B en C zijn gepaard en afhankelijk; hun kosten delen dezelfde B-call. A heeft zijn bestaande judge; B niet. De vergelijking betreft deze volledige pakketten, niet een geïsoleerd causaal effect van één promptzin of een gelijk-callbudgetonderzoek.

Alle 54 opgeslagen aanvragen zijn gecontroleerd op exacte systemprompt, oorspronkelijke bron, schema-aanwezigheid,6.000-tokeninstelling, ontbrekende temperatuuroverride en — bij C — de exacte B-draft. [Transportcontrole](verification/transport-audit.json). Er zijn geen Research/Narrative/Compose-eindproducten gegenereerd.

## Beoordeling

Vijf criteria0–2: brontrouw, redactionele intentie, focus, onderscheidende invalshoek en aantrekkingskracht voor de bedoelde lezer. Slagen vereist **brontrouw2, intentie2 en totaal≥8/10**, plus geldige uitvoer. Alle vijf velden van het geselecteerde eerste idee zijn tegen de volledige bron gelezen. De bestaande judgeverdicts gelden niet als evaluatiescore.

De beoordeling is uitgevoerd door dezelfde **Work-assistent** die protocol en nieuwe bronnen opstelde, met expliciete bronvergelijking en herleidbare toelichting. Er is **geen onafhankelijke menselijke beoordelaar en geen onafhankelijke blindering**. Er zijn geen extra scoring-modelcalls of nieuwe AI-judges gebruikt. Deze scores zijn controleerbare redactionele oordelen, geen onafhankelijk bevestigde waarheid of schatting van productiereliability.

[Alle42 ratings met vijfdelige vectors en geselecteerde ideeën](scores/scored.json), [CSV](scores/scores.csv), [uitgebreide bronvergelijkingen, citaten, onzekerheden en reviewbeoordeling](scores/review-notes.md). Een ongeldige/geblokkeerde uitvoer krijgt voor de aflevermeting administratief0; de ruwe inhoudelijke score blijft apart bewaard waar een idee zichtbaar is. Een lege of geblokkeerde uitvoer krijgt **geen inhoudelijke raw-score**. Dat onderscheid voorkomt dat ontbrekende ideeën als aangetoonde bronfouten worden voorgesteld.

| Primaire bronfamilie | A | B | C |
|---|---|---|---|
|005 |7, bronfout |Ongeldig; raw3 |Geblokkeerd |
|010 |9, bronfout |Ongeldig; raw5 |Geblokkeerd |
|Keramiek |8, doel afgezwakt |**8, geslaagd** |**8, geslaagd** |
|Koor |5, bron/doel fout |**8, geslaagd** |**8, geslaagd** |
|Wetland |9, bronfout¹ |Ongeldig; raw9 |Geblokkeerd |
|Bakkerij |7, bron/doel fout |**9, geslaagd** |Ongeldig; raw9 |
|Textielcoöperatie |**8, geslaagd** |6, bronambiguïteit/kwaliteit |Ongeldig; raw6 |
|Observatorium |**8, geslaagd** |5, verkeerd doel |Ongeldig; raw5 |
|Vertaalpilot |5, bron/doel fout |4, bron/doel fout |4, bron/doel fout |
|Boomgaard |**8, geslaagd** |Ongeldig; raw8 |Geblokkeerd |

¹Wetland-A is een grensgeval: het centrale telmethodenverhaal klopt, maar rationale verzint een gebruikelijke lezersreactie en voegt niet-onderbouwde volledigheidseisen toe. Als dat als onbelangrijk wordt beschouwd, slaagt A 4/10 in plaats van3/10. Ook de ambiguïteit “membership review” in B-textiel en twee herhalingsscores hebben lenientere lezingen. Geen daarvan verandert het NO-GO. A 005’s aanvankelijk voorgestelde appeal2 is bij rubriccontrole naar1 gecorrigeerd: een causaal opgeblazen headline voldoet niet aan de bevroren eis voor appeal2. Geen label, rubric, prompt of beslisgrens is aangepast.

De vier herhalingen slagen bij alle condities0/4 onder de strikte lezing. A blijft in005 causaal overclaimen en in010 het grootste interventie-effect veronderstellen. B 005 levert nu wel geldig JSON, maar beveelt een ongeteste specifieke guardwijziging als meest directe oplossing aan; C laat dit ongewijzigd. B 010 en B-keramiek geven in de herhaling een lege zichtbare respons: alle6.000 geregistreerde outputtokens zijn reasoning, finishReason=length. C is daar geblokkeerd. Bij koor vernauwt C één eerste-idee-rationale, maar introduceert een onduidelijke toeschrijving van de entry-windowbeslissing aan feedback. Een ruimere lezing kan B/C-koor en A-keramiek in de herhaling laten slagen; dit levert geen robuuste C-winst of oplossing van005/010 op.

## Wat zeggen de brononderbouwing en self-review?

In de primaire B-uitvoer matchen **41/50 groundingcitaten exact**; inhoudelijk zijn35 principale claim/onderbouwing-relaties volledig ondersteund,7 gedeeltelijk en8 onvoldoende. Dit is een beperkte redactionele audit per hoofdpropositie/veld, geen uitputtende zinsgewijze feitencontrole. Vier B-outputs falen quotevalidatie. Wetland betreft alleen een weggelaten paragraafscheiding; boomgaard en de oorspronkelijke rapporten bevatten samengestelde of gewijzigde citaten. Deze typen zijn apart vastgelegd, niet allemaal als verzonnen broninhoud geteld. [Quote-audit](verification/quote-audit.json), [supportgrades en toelichting](scores/review-notes.md).

Ook met perfecte quotes gaat het inhoudelijk mis. Observatorium-B citeert correct dat de update over bezoek en weer gaat en **geen technische review** is, maar maakt er toch een procurement/performance-audit van. Vertaalpilot-B erkent in constraints dat de inhoud van feedback onbekend is, terwijl de rationale toch “readers who reported confusion” stelt. Bij005/010 begrenzen de constraints soms de conclusie, zonder de causale of interventieclaim in het idee zelf te veranderen.

In de tien zichtbare primaire ideeën heeft A **2 causale attributie-overclaims en 2 interventie-overclaims**; B heeft er **1 en 3**, inclusief de bewaarde ongeldige drafts. C heeft **0 en 1 in zes werkelijk gereviewde drafts**. Dit zijn aantallen ideeën met een flag, geen onafhankelijke aantallen zinnen; een idee kan beide flags krijgen. C’s lagere aantallen zijn geen aangetoonde verbetering, omdat vier B-fouten C niet bereikten. In de herhalingen heeft A1 causale en1 interventie-overclaim in vier ideeën; B en C elk0 en1 in slechts twee zichtbare ideeën. Lege/geblokkeerde uitvoer is geen bewijs van afwezigheid van overclaimrisico. De per-veldredenen en flags staan in de ratings en bronvergelijkingen.

Onder een diagnostische lezing die de quotecontractfouten negeert, slagen **B 5/10 tegenover A 3/10** op ruwe semantiek. Dat is een interessant ontwikkelsignaal, geen voldoende bewijs: de oorspronkelijke moeilijke gevallen blijven fout, B heeft nieuwe kritieke doelmissers, en5/8 semantische holdoutsuccessen haalt de7/8-gate niet. Het oordeel berust dus niet alleen op strenge whitespacecontrole.

C krijgt in de primaire reeks zes geldige B-drafts te zien. Alle 30 herziene groundingcitaten zijn exact, maar dat percentage is **selectief**: de vier ongeldige B-drafts bereikten C niet. C behoudt dezelfde drie ruwe semantische successen als B binnen die zes gevallen; twee daarvan blijven geldig afgeleverd. De andere drie eerste-idee-fouten worden niet opgelost. Bakkerij/observatorium plaatsen grounding op het verkeerde JSON-niveau; textiel heeft een niet-letterlijk reviewcitaat. Een hoger quotepercentage is hier geen bewijs van betere redactionele beslissingen.

Over alle **acht reviewcalls** zijn er **nul robuuste volledige eerste-idee-gatereparaties**, één gerichte maar onvolledige aanscherping in koor-replicaat 2, enkele zinnige correcties van niet-geselecteerde alternatieven, en ook overstrenge diagnoses over niet-letterlijk genoemde functietitels of extra volledigheid. De score van het geselecteerde eerste idee mag niet worden afgeleid uit een verbetering van alternatief 2 of 3. [Reviews/diffs](verification/review-diffs.json).

De ruwe primaire gemiddelde score voor invalshoek+aantrekkingskracht is bij A en B gelijk: **2,9/4**. B’s ruwe totaalgemiddelde is6,5 versus7,4 bij A. Er zijn dus aantrekkelijke, bruikbare B-ideeën en ook vlakke checklist-/briefvoorstellen; er is geen aangetoonde combinatie van betrouwbaardere brontrouw, beter doelbehoud én structureel betere redactionele kwaliteit. De aflevergemiddelden met0 voor uitval staan apart in [summary.json](scores/summary.json) en mogen niet als zuivere stijlscores worden gelezen.

## Calls, tokens en latency

Vooraf plafond **112 GPT-5-mini-calls**:14 gematchte sets×(A≤6+B1+C1). Werkelijk **54/112 gereserveerd en beantwoord**, geen transportfouten, ontbrekende responsen, SDK-herpogingen of vervangende calls. De overige58 slots zijn niet herverdeeld. Twee lege antwoorden door de outputlimiet tellen volledig mee. De oorzaak-/interventieflags inspecteren zichtbare eerste ideeën; lage aantallen bij C bewijzen niets wanneer C geblokkeerd is.

| Gemeten onderdeel, alle14 sets | Calls | Inputtokens | Cached subset | Outputtokens | Reasoning subset | Gemiddelde tijd |
|---|---:|---:|---:|---:|---:|---|
|A, inclusief judge/herstel |32 |132.476 |61.312 |53.842 |33.152 |45,2s per set |
|B |14 |23.483 |7.552 |67.351 |47.712 |59,3s per set |
|C, alleen extra review |8 |21.022 |0 |29.484 |16.320 |44,8s per review |
|C-pipeline, B plus review² |22 |44.505 |7.552 |96.835 |64.032 |103,4s per daadwerkelijk gereviewde set |

²Dit is geen extra gefactureerde B-reeks: B wordt gedeeld tussen de B- en C-condities. Werkelijk totaal is32+14+8=54. Inclusief zes vroeg geblokkeerde C-sets is de gemiddelde B+C-tijd85,0s; dat gemiddelde verhult uitval, daarom staat ook103,4s voor de acht werkelijk gereviewde sets vermeld.

Totaal **176.981 inputtokens** (68.864 cached) en **150.677 outputtokens** (97.184 reasoning). Cached en reasoning zijn subsets, niet extra bij te tellen. Opgetelde API-latency30,3minuten; kosten zijn niet naar geld omgerekend en niet met een factuur vergeleken. Copilot-implementatiecalls vallen buiten deze experimentele modelledger en zijn apart gelogd. [Ledger](verification/call-ledger.jsonl), [usage](verification/usage.json), [alle callmetrics](verification/calls.json), [statistiek](verification/statistics.json), [commandolog](verification/command-log.jsonl).

## Verificatie, grenzen en beslissing

Copilot CLI 1.0.109 is geverifieerd; Claude Haiku 4.5 was beschikbaar en eerst gebruikt. Twee onvolledige skeletversies zijn bij code-inspectie afgewezen vóór live calls. Na onbeschikbare Sonnet-ID’s heeft Copilot auto de werkende implementatie en daadwerkelijke tests gebouwd. Transcripties en de afgewezen eerste versiebeoordeling zijn bewaard; het oude certificaat staat onder verification/superseded en geldt niet als bewijs. De definitieve solution-build heeft **0 waarschuwingen/0 fouten**, alle **241 tests** slagen (230 bestaande+11 harnastests), Domain had geen vindbare tests. Offline probes voeren de echte MAF-lus met fake clients uit en controleren validator, budget, malformed outputs en hervatting. [Build](verification/build.log), [tests](verification/test.log), [offline](verification/offline.log).

42/42 cellen hebben een definitieve status; geen incomplete reserveringen. De ledger heeft54 unieke, aaneengesloten reserveringen, alle met aanvraag/antwoord/metadata. Een echte hervattingsprobe op een eerder gefaalde B-cel geeft skipped en laat de ledger54→54. De187 frozen hashes, HEAD en oude productiebronbestanden zijn gelijk; alleen solutionregistratie en twee nieuwe projecten zijn toegevoegd. [Resumecontrole](verification/resume-existing.json), [integriteit](verification/integrity.json).

**B en C halen de vooraf vastgelegde GO-gates niet.** Beide missen de minimumsuccessen, herhaalde005/010-gate en aantoonbare winst boven A. B/C hebben bovendien een nieuwe verkeerde verhaalrichting bij een observatoriumbron waarop A slaagt. C haalt de correctiegates niet. [Machineleesbare gates](scores/gates.json).

Dit is een kleine, synthetische, niet-onafhankelijk beoordeelde screening. De nieuwe bronnen bevatten vrij expliciete intentie-/bewijsgrenzen. Oude bronnen zijn eerder bekeken; prompt-, schema- en lengteverschillen vormen samen de interventie. Een vaste outputcap kan vooral B/C raken; twee lege B-antwoorden illustreren dat. Geen populatiebetrouwbaarheid, geïsoleerde causale promptwinst of downstream-publicatiekwaliteit is aangetoond. De proef verwerpt **deze kandidaten onder deze instellingen**, niet elke mogelijke evidence-aware of self-reviewarchitectuur.

**Implementatieaanbeveling:** behoud het harnas als experimentele infrastructuur; neem nu geen B/C-logica over in productie. Een eventuele volgende proef kan eerst op ontwikkelbronnen een korter source-span-/quoteprotocol, expliciete nesting van C’s volledige revised-object en effectieve backend-schemahandhaving verifiëren. Richt review expliciet op de geselecteerde eerste claim en bewijs→conclusie-relatie, en test of goede voorbeelden onterecht worden versmald. Daarna pas opnieuw bevriezen met werkelijk nieuwe, minder expliciet gelabelde bronnen en een onafhankelijke geblindeerde beoordeling. Daarvoor zijn in deze sessie geen nieuwe prompts getuned of calls besteed.

## Hergebruik en hervatting

Vanaf repository-root: bouw de solution; voer de offline-opdracht uit zoals in [README](README.md). Deze bestaande map is verbruikt/bewaard: **reset nooit ledger of started-markers**. `run.ps1 all` slaat afgeronde cellen over; gestart-zonder-final vereist inspectie en wordt niet automatisch opnieuw verstuurd. Het harnas controleert freeze/HEAD en houdt een exclusieve runlock vast. Een nieuwe proef gebruikt een nieuwe artefactroot met cases.json, prompts, baseline snapshots, expliciet budget en nieuwe freeze; het huidige harnasplafond blijft112.

De evaluatie is reproduceerbaar met de vastgelegde oordelen en scripts, zonder nieuwe modelcalls:

```powershell
./PromptExperiments/IdeaGenerator/evidence-aware-v1/verification/make-ratings.ps1
./PromptExperiments/IdeaGenerator/evidence-aware-v1/score.ps1
./PromptExperiments/IdeaGenerator/evidence-aware-v1/verification/analyze.ps1
./PromptExperiments/IdeaGenerator/evidence-aware-v1/verification/summarize.ps1
./PromptExperiments/IdeaGenerator/evidence-aware-v1/audit.ps1
```

Dit reconstrueert de opgeslagen beoordeling, geen onafhankelijke herbeoordeling. Een verse modelreproductie vereist een nieuwe runroot/budget; modelantwoorden zijn niet bit-identiek gegarandeerd. [Eindmanifest](verification/final-manifest.json) legt de opgeleverde artefacten vast.
