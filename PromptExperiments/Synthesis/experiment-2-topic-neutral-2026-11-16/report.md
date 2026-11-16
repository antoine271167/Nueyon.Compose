# Synthesis Experiment 2 — onderwerpneutralisatie

16 november 2026. **NO-GO volgens de vooraf bevroren poort.** B verwijdert de geobserveerde Nuëyon-merkcontaminatie (A 2/12, B 0/12), maar is slechts 10/12 volledig voldoende waar minstens 11/12 vereist was. A scoort 9/12. De actorverbetering compenseert geen andere gebreken. Geen productiepromotie.

## Proef en inputs

Vier vaste cases × A/B × drie herhalingen = **24 echte SynthesizerAgent-calls met gpt-6-luna**. Alle 24 geldig, HTTP 200/finish stop, nul truncaties, parserfouten, retries of vervangingscalls. Geen Idea-, Research-, Narrative-, Compose-, Judge-, Evidence Mapper- of evaluatiemodelcalls; geen downstreamscores. Work-assistent beoordeelt ongeblindeerd, zonder onafhankelijke mens. Narrative-grondstof beoordeeld, geen feitelijke Narrative-resultaten.

Repo C:\Workspaces\Nueyon.Compose, HEAD **663908a0712d089389bc9743c7a8a9ed46b9abbf**, origin antoine271167/Nueyon.Compose. Beginstatus schoon; lokale versie onderzocht, geen fetch/push. Eerst gelezen: Experiment 1-protocol, rapport, alle twaalf volledige outputs en scores, ruwe responses/usage, vier volledige Researchs, projecties, Ideas, vaste bronnen en huidige productieprompts/agent/filter/promptloader. De huidige productieprompts zijn bytegelijk aan Experiment 1.

Alle casebestanden zijn bytegelijk gekopieerd uit [Experiment 1](../baseline-experiment-1-2026-11-16/report.md), inclusief exact dezelfde opgeslagen Research B2-r1 uit Research Experiment 9. Alleen vijf Selected Idea-velden en Facts/Unknowns-projectie gaan naar het model. Geen bronaanvulling, rubric of labels in requests. De echte deterministische filter bevestigt dezelfde projecties. Alle upstreamagents blijven onaangeroerd en formatagnostisch.

## Exacte wijziging en rationale

[Volledige precieze diff](prompt.diff), [11 exacte bytevervangingen](replacements.json). Eén interventie over beide promptbestanden:

- System: merkidentiteit verwijderen; agent→orchestrator-voorbeeld, superioriteitsvergelijking en onzekerheidsvoorbeeld generaliseren (vier vervangingen).
- User: drie Compose/orchestration/naming-voorbeelden generaliseren; vier expliciete Nuëyon-beschrijvings- en richtingsinstructies vervangen door gedocumenteerde actoren (zeven vervangingen).

Voorbeeld: “What Nuëyon believes, is building, intends to offer, or plans to explore.” wordt “What the documented actors believe, are doing, intend to offer, or plan to explore.” De opdracht over “Nuëyon's own product direction” verwijst in B naar “the documented actors' own direction”. De systeemidentiteit wordt “You are the Synthesizer Agent.”

Causaliteits-, bewijs-, selectiviteits-, onzekerheids- en outputcontractregels blijven inhoudelijk gelijk. Algemene negatieve product/workflow/architectuurvoorbeelden blijven waar ze geen specifieke actor opleggen; dit beperkt onnodige herschrijving. Narrative en Compose blijven ketenrollen, geen eindformaat toegevoegd. [Bytecontrole](verification-source-identity.json) bevestigt uitsluitend de elf opgegeven vervangingen, met alle overige bytes/whitespace behouden. Geen compacte herschrijving of tweede kandidaat.

## Freeze, rubric en poort

[protocol.md](protocol.md), [plan.json](plan.json), prompts/inputs/runner/runtime bevroren op **2026-11-16T21:35:52.443Z**, vóór alle calls. SHA256-freeze: **2335 bestanden**, inclusief eerdere experimenten en gevolgde repo. Audit/assessment/reportcode is nadien toegevoegd, geen prospectieve analysecodeclaim.

Vijf dimensies 0–2: BT brontrouw/actoridentiteit; F Idea-focus; BN essentiële feiten/nuances; IW interessante concrete grondstof; NG Narrative-grondstof. 2 voldoende, 1 bruikbaar met concreet gebrek, 0 materieel misleidend/onbruikbaar. Alle vijf 2 vereist voor voldoende, geen compenserende totaalscore. Alle secties inclusief guidance tellen. Geen academische volledigheid, hook, lengte of verplichte feitenquota.

B GO: twaalf geldige calls, nul materiële contaminatie/verzinsels, minstens 11/12 volledig voldoende, minstens 2/3 per case, geen herhaalde essentiële omissie per case, niet minder voldoendes dan A totaal/per case en geen ongecompenseerde herhaalde gepaarde dimensieregressie. Voor actorcorrectie moet A contaminatie tonen en B nul. De merkvoorwaarde wordt gehaald, de kwaliteitspoort niet.

**Lowell vooraf redactioneel beslist:** outputgroei ondanks banenverlies is interessante context, geen verplichte essentie voor deze specifieke hergebruik/terugval-Idea. Het vroege contrast uit 1895–1918 bewijst geen hergebruik of late-1980s terugval; automatisch eisen voegt een tweede productiviteitsverhaal toe. De kern werkt via neergang → tastbaar hergebruik → nieuwe kwetsbaarheid. Alle zes Lowell-outputs voldoende; geen bewaart het vroege contrast. Deze proef heeft prospectief een andere rubric op dit punt dan Experiment 1; oude scores/bestanden blijven ongewijzigd. Historische 8/12 is geen gelijkwaardige comparator voor de nieuwe A/B.

Volledige individuele scoretabel: [scores.md](scores.md); expliciete poorttoets: [gate-evaluation.json](gate-evaluation.json).

## Resultaten

| Case | A voldoende | B voldoende |
|---|---:|---:|
| Webb | 2/3 | 2/3 |
| Wolf | 2/3 | 2/3 |
| Digital | 2/3 | 3/3 |
| Lowell | 3/3 | 3/3 |
| Totaal | **9/12** | **10/12** |

Drie gepaarde B-winsten (Webb-r1, Wolf-r3, Digital-r2), twee B-verliezen (Wolf-r1, Webb-r3), zeven paren beide voldoende. [results.json](results.json) bewaart alle dimensiedelta's. Dit zijn beschrijvende assistentscores, geen statistisch gevalideerde kwaliteitskansen.

## Gebreken en attributie

**Webb-A-r1 — BT0/F1/BN1/IW2/NG1.** Guidance regel 10: “Beschrijf Nuëyons onderwerp als Webbs eigen ingebruiknameproces”. Dit legt een niet-gedocumenteerde merkrelatie op. Nuëyon ontbreekt in bron, Idea, volledige Research en projectie. Daarnaast ontbreekt formaat→vouwen (Fact 1), terwijl de afmetingen expliciet secundair worden gezet. Hoofdcontrast tweede week versus zes maanden blijft goed. Zie [output](calls/webb-r1-A-Synthesis/content.md).

**Digital-A-r2 — BT0/F1/BN2/IW2/NG1.** Guidance regel 12: “Explain this as Nuëyon’s account of one product’s direction and working choices”. Bron en Research Facts 2/4/6/7 gaan over het OPG/POA-team. De andere evidence is bruikbaar, maar de expliciete eigenaar in de handoff is verkeerd. Zie [output](calls/digital-r2-A-Synthesis/content.md).

**Wolf-A-r3 — BT2/F1/BN1/IW1/NG1.** “Pack cooperation ... are not needed to establish this story.” Sociale samenwerking wordt gedeprioriteerd; voedselrelaties staan slechts als algemene optionele verwijzing zonder concreet soortvoorbeeld. De netwerk-Idea vernauwt tot veranderende predatie/elk. Facts 3/6 staan volledig in Research én projectie. Goede multicausale grens behouden. Zie [output](calls/wolf-r3-A-Synthesis/content.md).

**Wolf-B-r1 — BT1/F2/BN2/IW2/NG1.** Evidence zegt “elandkadaver” en “effecten op elanden”; guidance herhaalt dit. Vaste bron en Research Facts 6/9 zeggen elk. Dit verandert de prooisoort in de Nederlandse vertaling. Het netwerkverhaal blijft bruikbaar, maar vereist correctie; lokale fout, geen BT0 voor de hele output. Zie [output](calls/wolf-r1-B-Synthesis/content.md).

Terminologie na calls beperkt geverifieerd: Yellowstone onderscheidt elk (Cervus canadensis) van moose (Alces alces); Nederlandse eland verwijst naar de tweede soort. [NPS Elk](https://www.nps.gov/yell/learn/nature/elk.htm) bespreekt expliciet de verwarring tussen de Amerikaanse en Europese woordbetekenis; [NPS Moose](https://www.nps.gov/yell/learn/nature/moose.htm) identificeert de andere soort. Deze check voegt geen actuele ecologische feiten of input aan de proef toe. De brontrouw-/actoridentiteitsrubric was al bevroren. Geen merkvermelding is dus geen bewijs dat alle identiteitsfouten opgelost zijn.

**Webb-B-r3 — BT2/F2/BN1/IW2/NG1.** Guidance zet “De exacte afmetingen en het vouwontwerp voor de raket” expliciet op de achtergrond; formaat→vouwen ontbreekt geheel. Dit was vooraf vereist en wordt gelijk beoordeeld als A-r1. Facts 1/2 waren in volledige Research én projectie beschikbaar. Uitvouwen versus wetenschap, maanden en mogelijke risico's blijven juist. Zie [output](calls/webb-r3-B-Synthesis/content.md).

**Geen beslissend gebrek ontstaat bij Research of filter.** Juiste actoren/soort en de weggelaten essenties staan al in de vaste Facts/projectie. Nuëyon verschijnt pas in Synthesis-guidance en staat wel in A-prompts. De proef ondersteunt de gezamenlijke neutralisatie als verklaring voor minder merkcontaminatie, maar isoleert geen enkele regel als oorzaak. Andere B-gebreken zijn waargenomen verschillen, geen bewijs dat neutralisatie die causaal veroorzaakte.

## Betekenisbehoud en concrete grondstof

- Webb: alle zes bewaren tweede-week-ontplooiing versus routinewetenschap na zes maanden en afkoeling/uitlijning/kalibratie. A-r2 bewaart heaters tegen ijs en vervormde sterbeelden; B-r1/r2 grondbesturing/risico's. Geen verzonnen storing. Enkele Nederlandse outputs maken cooling met “daarna” te scherp opeenvolgend; bron beschrijft overlap/voortzetting. Voor beide armen kleine precisienotitie, geen gewijzigde kernclaim.
- Wolf: alle zes bewaren veranderende prooipatronen en andere predatoren/weer/beheer. Vijf bewaren sociale relaties en concrete aaseters. Geen bisonmisclaim of bewezen cascade. A-r3 mist de netwerkhelft; B-r1 bevat soortvertaling.
- Digital: alle zes bewaren twaalf weken, aanvragers vóór caseworkers, de concrete tweeweekse sprint en causaliteitsgrens. Assisted digital zonder papier soms alleen in Gaps, maar beschikbaar zonder verzonnen hulpvorm. KPI's mogen optioneel/weggelaten zijn; geen uitgekeerde refunds of bewezen causale verbetering. A-r3 “twaalf weken later” verankert onbekend beginpunt te strak; A-r2 “12-week deadline” verwart gerapporteerde duur iets met deadline. B-r2 “alternating fortnightly weeks” is onhandig, maar Evidence specificeert correct twee verschillende weken. Afzonderlijk genoteerd, kleiner dan verkeerde eigenaar.
- Lowell: alle zes bewaren neergang/menselijke schaal, samenwerking, meer dan honderd hergebruikte gebouwen, opleving én late-1980s terugval plus 1992-grens. Onderwijs/technologie blijven context/begrenzing, geen verplicht tweede verhaal. Menselijke recollections vaak geabstraheerd; geen verplicht brood/paardenbloemdetail. A-r2 zet oudere worker recollections naast postwar closures; klein temporeel precisierisico, geen expliciete nieuwe datum.

[assessments.json](assessments.json) bevat alle scores, exacte foutpassages/regels, feitenverwijzingen en kleine notities. [traceability.json](traceability.json) koppelt alle **95 Evidence-proposities** aan projectieregels en oorspronkelijke bronpassages. [audit-inventory.md](audit-inventory.md) bewaart volledige outputs, projecties en bronnen met regelnummers. Alle secties zijn gelezen; omissies en guidancefouten apart beoordeeld. De mapping is geen garantie van foutloze parafrases of onafhankelijk oordeel.

## Usage en verificatie

| Arm | Calls | Inputtokens | Outputtokens incl. reasoning | Totaal |
|---|---:|---:|---:|---:|
| A | 12 | 65073 | 19849 | 84922 |
| B | 12 | 64581 | 19958 | 84539 |
| Totaal | 24 | 129654 | 39807 | 169461 |

27503 reasoningtokens; 112519 gecachete inputtokens. B 492 minder inputtokens, maar 109 meer outputtokens. Geen kostenvoordeel of europrijs claimen. Hard maximum 24 attempts × 5000 completiontokens = 120000 outputtokens; maximaal 120000 requestbytes/call, 300 seconden timeout, één HTTP/call, retries nul, verse client/session. Geen seed/temperatuur/reasoningoverride. Geen exact inputtoken- of geldplafond voorgespiegeld.

- Geïsoleerde runnerbuild en productie-hostbuild geslaagd, 0 warnings/errors. Zes bestaande Synthesis/filtertests geslaagd.
- Embedded productieprompts exact gelijk aan A; echte filterprojecties vóór freeze exact gelijk aan vaste inputs.
- [verification-requests.json](verification-requests.json): 24 juiste system/user-renderings, schema, model, input, caps, HTTP 200, finish stop en start na freeze. [verification-pairs.json](verification-pairs.json): twaalf bytegelijke A/B-inputparen, zelfde schema/model/budget, exacte schedule, nul failures.
- [verification-offline-replay.json](verification-offline-replay.json): 24/24 ruwe responses opnieuw door echte SynthesizerAgent/parser, exact hetzelfde resultaat, zonder netwerk. Geen modeldeterminismeclaim.
- [verification-integrity.json](verification-integrity.json): alle 2335 freezehashes intact. [verification-source-identity.json](verification-source-identity.json): baseline/cases/A bytegelijk, alleen elf opgegeven B-wijzigingen.
- Raw requests/responses, usage, werkelijke content en resultaten per call opgeslagen zonder API-sleutel/authheaders. Lock/attemptmarkers behouden. Productie en eerdere experimenten onveranderd. Geen cleanup, Copilot CLI, commit of push. [REPLAY.md](REPLAY.md) beschrijft offline reproductie.

## Besluit en kleine vervolgstap

**NO-GO: B 10/12 < vereiste 11/12.** Merkcontaminatie verbetert hier van 2/12 naar 0/12, maar “opgelost zonder andere regressies” is niet aangetoond. Per-case passcounts verslechteren niet; twee gepaarde outputs wel. Geen post-hoc versoepeling van de poort.

Kleinste volgende stap: onafhankelijke blinde herbeoordeling van de vijf gemarkeerde outputs met hun bronpassages, onder dezelfde bevroren rubric, vooral soortvertaling en de vereiste vouwreden. Dit kan met opgeslagen artefacten zonder nieuwe calls of promptwijziging. B blijft geïsoleerd kandidaat. Pas daarna eventueel een nieuwe vooraf vastgelegde proef op onafhankelijke niet-productcases. Geen automatische productieactie.

Vier bekende institutionele bronnen, één vaste succesvolle Research per case, drie repeats en ongeblindeerde assistentscores dragen geen algemene betrouwbaarheids- of statistische causaliteitsclaim. Geen onafhankelijke holdout, menselijk oordeel of werkelijke downstreambruikbaarheid gemeten.
