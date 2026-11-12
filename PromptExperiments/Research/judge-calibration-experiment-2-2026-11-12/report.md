# Experiment 2 — Research Judge calibration, 12 november 2026

**NO-GO. Geen promotie; EnableResearchJudge blijft uit.** De nieuwe generieke source-first rubric detecteert 1/3 bekende doelproblemen. Twee baselineantwoorden falen bovendien de bestaande bronankercontrole. De vijf kleine synthetische holdouts slagen, maar compenseren de historische missers niet. Geen Research-generatorprompt, filter, contract, integratie of workflowgedrag gewijzigd. Geen Copilot CLI, commit of push.

## Uitgangspunt en uitvoering

Actuele repository gelezen: HEAD `99a5f78` (`sip`), beginstatus schoon. Eerder compact-judge-2026-11-12/report.md, protocol, runner, bevroren bron/Idea-paren, Research en controles gelezen; ResearchSemanticJudge, SynthesisResearchFilter, huidige rubric en default/configuratie geïnspecteerd. Het eerdere experiment vond 0/3 doelproblemen. Deze opdracht bood voldoende vervolgcontext; geen aanvullende historische chat of geheugenbron gebruikt.

Vooraf protocol.md, case-manifest.json en freeze.json vastgelegd. Eén kandidaat, twaalf vooraf toegewezen calls, geen adaptieve promptwijziging, retries of heruitvoering. Iedere call gebruikt dezelfde productie-ResearchSemanticJudge, JSON-schema en strenge parser; de echte SynthesisResearchFilter maakt de downstreamprojectie. Geen Research-generatie, reparatie, model-Synthesis of externe retrieval uitgevoerd. De rubric is uitsluitend een geïsoleerd experimentbestand onder prompts/Research/ResearchValidation.system.prompt.md.

Procedure in de kandidaat: eerst essentiële bronproposities voor alle Idea-velden inventariseren, inclusief expliciete betekenis, causaliteitsgrenzen en discriminerende tegenvoorbeelden; daarna per item Research Facts/Unknowns en werkelijke filteroutput vergelijken; vervolgens iedere Unknown-clausule op te ruime ontkenning controleren; tenslotte alleen materiële, exact brongeankerde diagnoses retourneren. De inventory is interne modelinstructie, geen extra contractveld en geen zichtbaar/auditbaar tussenproduct. Dit experiment bewijst niet dat het model die interne procedure volledig uitvoert.

De prompt bevat geen casenamen, verwachte antwoorden, Nuëyon-specifieke voorbeelden of bekende broncitaten. Holdouts zijn vóór de calls geschreven in andere domeinen, met twee correcte en drie defecte documenten. Het museum-contrastpaar toetst bewust hetzelfde abstracte semantische onderscheid als intro; dit is domeintransfer, geen volledig onafhankelijk fouttype. Vijf documenten vertegenwoordigen drie korte bronscenario's, geen vijf onafhankelijke bronnen. Auteurschap, labeling en beoordeling door dezelfde assistent, niet geblindeerd of onafhankelijk menselijk gevalideerd.

## Brongeankerde referentiebeoordeling

Deze referentiepunten zijn beoordelingsdata, nooit aan de judge meegegeven als labels:

- Intro: de bron zegt “Nuëyon.Compose is the first step because it gives the broader vision a concrete expression.” Facts 9/13 ondersteunen die relatie. Baseline Unknown 4 ontkent juist dat Compose de ambitie concreet maakt. Een onzeker lezereffect is iets anders dan de expliciete bronbetekenis. De bestaande controle beperkt Unknown 4 terecht tot lezereffect.
- 005: “Idea and Research outputs vary between runs.” De gerapporteerde negatieve uitkomsten en identieke prompts/workflowinputs blijven juiste feiten, maar betekenen geen identieke tussenresultaten of geïsoleerd causaal Narrative-effect. Baseline Facts/Unknowns missen die variatie; een algemeen onbekende oorzaak vervangt de concrete beperking niet. Datums en mogelijke backendwijziging zijn bijkomende beperkingen. De oude gerichte controle voegt variatie/datums toe, maar mist nog de bronbeschreven spanning in baseline-Synthesis. De extra controle voegt die spanning toe met alleen mogelijke invloed, en versmalt de blanket Unknown over een algemene les. Alle oorspronkelijke Facts blijven behouden.
- 010: de bron vermeldt bij Experiment 009 Baseline 001 een feitelijke Compose-fout (“repeatable process that reliably converts”), terwijl “Selected Idea and earlier chain remain company-first; the artifact’s score gives downstream hierarchy 2/2.” Dit onderscheid ontbreekt in de baselineprojectie. De controle bewaart het expliciet. Het is geen bewijs voor een Compose-first hiërarchiebreuk of voor interne causaliteit.

## Tuningset: zeven calls

| Input | Ruw oordeel | Productieparser | Doel en inhoudelijke beoordeling |
|---|---|---|---|
| baseline intro | reject | fout | Unknown 4 gemist. Alleen Unknown 3 aangevallen; citaat verandert Nuëyon in `NuByon` en is dus niet letterlijk. |
| baseline 005 | reject | fout | Upstreamvariatie gemist. Alleen een blanket Unknown over de algemene les aangevallen, met een citaat uit de **Selected Idea**, niet de originele bron. Ongeldige bronautoriteit. |
| baseline 010 | reject | geldig | Doelprobleem correct: ontbrekende Compose-feitfout met behouden hiërarchie en score 2/2. Exact bronanker en terughoudende repairgrens. |
| controle intro | reject | geldig | Unknown 4 niet opnieuw afgewezen, maar Unknown 3 wel. Overstrenge materialiteitsbeoordeling; zie hieronder. Geen bewijs van een geslaagde positieve controle. |
| oude controle 005 | reject | geldig | Verdedigbare resterende Synthesis-lacune correct en letterlijk geankerd; gerepareerde upstreamvariatie niet opnieuw afgewezen. Geen correcte Fact als onwaar bestempeld. |
| controle 010 | accept | geldig | Correct contrast geaccepteerd. |
| aangevulde controle 005 | accept | geldig | Variatie, causaliteitsgrenzen en Synthesis-spanning geaccepteerd. |

De intro-controlediagnose heeft een letterlijk anker, maar de bronstatus “under development” staat al in Fact 10 en de bron geeft geen precieze implementatiestatus of bewezen effectiviteit van de bredere portfolio. Unknown 3 kan preciezer, maar de judge toont niet overtuigend aan dat dit hier een materiële ontkenning is die afwijzing vereist. Daarom beoordelen wij deze conservatief als overrejectie/ambigue blocker, niet als bewezen juiste afwijzing. Correcte Facts worden niet letterlijk onwaar genoemd; de positieve controle faalt niettemin. Ook bij een gunstiger interpretatie blijft het experiment NO-GO door twee gemiste doelen en twee ongeldige bronankers.

Targetdetectie **1/3**, tegenover 0/3 in de vorige afzonderlijke run; dit is geen statistisch bewezen verbetering. Dezelfde bevroren outputs zijn gebruikt, maar er is geen herhaalde, geïnterleavede vergelijking tussen beide judges. 5/7 tuningantwoorden passeren de technische parser, 2/7 niet. Een ruw reject geldt nooit automatisch als correcte detectie.

## Holdouts: vijf calls, afzonderlijk gerapporteerd

| Case | Verwacht | Uitkomst | Diagnosekwaliteit |
|---|---|---|---|
| Museum correct | accept | accept | Expliciet toegeschreven betekenis en onbekend leereffect correct naast elkaar behouden. |
| Museum ontkenning | reject | reject | Exact citaat van museumkarakterisering; Unknown ontkent die ten onrechte. Geen verzonnen effect. |
| Warehouse confounder | reject | reject | Concrete pakketmixvariatie en vergelijking bij dezelfde layout ontbreken uit echte filteroutput, hoewel aanwezig in Interpretations; algemene causaliteitsdisclaimer is onvoldoende. Geen causaliteitsbewijs geclaimd. |
| Archive correct | accept | accept | Ordening en transcriptiefouten apart gehouden; beide contrastgevallen behouden. |
| Archive omissie | reject | reject | Cedar heeft transcriptiefouten ondanks juiste ordening; ontbrekende tegenproef exact aangewezen, zonder de foutdimensies te verwisselen. |

Alle vijf geldig JSON met geldige productieparser; beide correcte documenten geaccepteerd, alle drie ingevoegde fouten brongetrouw gedetecteerd. Geen ernstige regressie waargenomen in deze kleine, eenvoudige set. Dat bewijst geen betrouwbare generalisatie naar langere, complexere bronnen: juist daar falen intro/005. Geen algemene precisie, false-positive-rate of repeatability-claim.

## Callledger en artefacten

| Call | Input | Inputtokens | Outputtokens | Totaal |
|---:|---|---:|---:|---:|
| 1 | baseline intro | 6019 | 865 | 6884 |
| 2 | baseline 005 | 6514 | 2389 | 8903 |
| 3 | baseline 010 | 7258 | 1188 | 8446 |
| 4 | controle intro | 6031 | 1000 | 7031 |
| 5 | oude controle 005 | 6662 | 2134 | 8796 |
| 6 | controle 010 | 7438 | 680 | 8118 |
| 7 | aangevulde controle 005 | 6764 | 713 | 7477 |
| 8 | museum correct | 1246 | 198 | 1444 |
| 9 | museum ontkenning | 1270 | 328 | 1598 |
| 10 | warehouse confounder | 1265 | 580 | 1845 |
| 11 | archive correct | 1251 | 81 | 1332 |
| 12 | archive omissie | 1225 | 265 | 1490 |
| **Totaal** | **12/12** | **52943** | **10421** | **63364** |

Alle twaalf HTTP 200, responsemodel gpt-6-luna, elk exact één request; SDK RetryPolicy(0) plus handlerlimiet 1. Reasoningtokens zijn al onderdeel van outputtokens, niet opnieuw optellen. Geen betrouwbare geldprijs vastgesteld. Ook ongeldige antwoorden tellen volledig mee. Geen calls buiten dit ledger. De resterende vooraf vastgelegde controles/holdouts zijn na de vroege NO-GO afgerond om transfer en fouttype te meten; geen foutantwoord opnieuw opgevraagd.

cases/ bevat bron/Idea en Research per input; calls/ bevat attempt-started.json, request.json, response-raw.json, http-metadata.json, echte synthesis-filter-output.md en response-processed.json. Succesvolle productieparsing levert verdict.json; mislukte levert error.txt. response-processed.json bewaart ook ongeldige ruwe diagnoses met afzonderlijke ankercontrole en mag niet als geaccepteerd productieverdict worden behandeld. results.json en usage.json bundelen inhoud en usage. Geen API-key of authheaders vastgelegd.

verification-requests.json controleert voor alle twaalf bron, vijf Idea-velden, Research, werkelijke filtertekst en prompt tegen de bevroren inputbestanden. Alle velden matchen; casing volgt de bestaande camelCase-serialisatie. freeze.json bevat kandidaat-SHA256, begin-HEAD, budget en vooraf bepaalde volgorde/labels. prepare.cjs leest de letterlijke holdoutfixtures uit prepare.py (Python is hier niet geïnstalleerd); geen model wordt door voorbereiding of samenvatting aangeroepen. Runner weigert een experimentherstart zodra een poging bestaat.

## Offline validatie en scope

- `dotnet build src/Nueyon.Compose.slnx`: geslaagd, 0 waarschuwingen, 0 fouten.
- `dotnet test src/Nueyon.Compose.slnx --no-build`: 299 geslaagd (Application 237, Infrastructure 25, Console 37), 0 mislukt/overgeslagen; Domain bevat geen discoverable tests.
- Runner afzonderlijk gebouwd zonder fouten/waarschuwingen. Geen nieuwe tests toegevoegd voor uitsluitend experimentele prompt-/datawijzigingen; bestaande contract-, filter-, default- en workflowtests zijn groen.
- verification-protected-files.json: alle 103 bronbestanden buiten bin/obj/.vs bytegelijk aan begin. Geen productieprompt of technische wijziging. Git toont uitsluitend deze nieuwe experimentmap; `git diff --check` schoon.
- EnableResearchJudge heeft default false, de bijbehorende test slaagt. Hostappsettings bevat geen override; gecontroleerde relevante user-secret- en omgevingssleutels bevatten evenmin een override. Niets geactiveerd.

## Concrete volgende stap

Niet promoveren. Eerst onafhankelijk laten vastleggen welke Unknowns werkelijk blokkeren en een expliciete broncoverage-map voor lange bronnen annoteren. Een volgende afzonderlijke proef kan vervolgens een generieke, zichtbaar controleerbare source-only evidence-map als evaluatieartefact testen, met een daaropvolgende vergelijking van Facts/Unknowns/filteroutput. Dit is een onbewezen onderzoekshypothese; het vereist een nieuw vooraf vastgesteld budget en mogelijk een afzonderlijk experimenteel contract, geen stilzwijgende wijziging aan productie. Houd Selected Idea-proposities strikt gescheiden van bewijs en beoordeel volledigheid los van de aandacht voor Unknowns. Behoud deze runmarkers en voer deze twaalf calls niet opnieuw uit. Dit experiment geeft geen rechtvaardiging om de Research-generator, Synthesis-filter of overige workflow aan te passen.
