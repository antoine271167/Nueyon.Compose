# Experiment 8 — compacte generieke Research-kandidaat B2

16 november 2026. **B2: NO-GO onder de vooraf bevroren poort. Research kan nog niet worden afgerond.** B2 levert 7/9 strikt voldoende outputs; vereist waren minstens 8/9 én minstens zoveel als A (8/9). B levert 4/9. B2 gebruikt 78,47% minder totale tokens dan A en 75,39% minder tokens per strikt voldoende output. Deze NO-GO betekent niet dat B2 materiële bronfeiten mist: alle 108 B2-labelbeoordelingen zijn correct in de echte Facts/Unknowns-projectie. Twee kleine precisiefouten buiten die projectie verhinderen een volledig voldoende score. Geen productiepromotie of wijziging uitgevoerd.

## Reikwijdte en context

Uitsluitend Research, met vaste Selected Ideas. Idea is niet opnieuw uitgevoerd of beoordeeld; Synthesis, Narrative en Compose zijn niet uitgevoerd of inhoudelijk beoordeeld. Feitelijke bruikbaarheid voor Synthesis betekent hier of de daadwerkelijke Facts/Unknowns-projectie de benodigde evidence bevat, niet of een downstreamagent die goed gebruikt. Geen Judge, Evidence Mapper, nieuwe agentimplementatie, subagent, retrieval of Copilot CLI. De offline traceability-matrix is een audit van opgeslagen tekst en geen generatiestap of modelinput.

Repository `C:\Workspaces\Nueyon.Compose`, origin `https://github.com/antoine271167/Nueyon.Compose.git`, lokale HEAD `c4a5ba8a6d966307d7107e5e36b29d5551b5b235`. Schone start. Geen fetch nodig voor de lokaal vastgelegde productieprompt; geen claim dat remote intussen ongewijzigd is. Gekoppelde chat gelezen, maar conclusies steunen op repositoryartefacten. Alle nieuwe bestanden staan uitsluitend in deze experimentmap. Niets gecommit, gepusht of gepromoveerd.

Gelezen context: Experiment 5 report/protocol, Experiment 6 report/protocol en Experiment 7 report/protocol/traceability. Experiment 5's B-resultaat was veelbelovend maar ongeblindeerd en zonder repeats. Experiment 6's eindpublicatiefouten zijn geen zuivere Research-meting. Experiment 7 toonde dat audioverlies in de eindtekst niet door Research veroorzaakt was; tegelijk misten A en B keyboardinformatie en soms concrete attributiebeperkingen. De huidige proef toetst alleen Research en zijn echte projectie en neemt oude absolute successcores niet als referentielabels over.

## Vooraf vastgelegd ontwerp

[protocol.md](protocol.md), [rubric.md](rubric.md), [labels.json](labels.json) en [plan.json](plan.json) zijn vóór calls vastgelegd. [freeze.json](freeze.json) is gemaakt op 16 november 2026, 21:09:57 CET, en bevat 1.592 SHA-256-hashes: 1.508 beschermde gevolgde repositorybestanden plus de voorbereidende experimentbestanden, runner en runtimebestanden. Controle vóór iedere call; eindcontrole zonder afwijkingen. Analyse, scores en rapport zijn achteraf geschreven, niet als vooraf bevroren analysecode voorgesteld.

- A: bytegelijke huidige productie-system- en userprompt.
- B: beide prompts bytegelijk aan compacte B uit Experiment 5 én 6.
- B2: B met één generieke systeemuitbreiding voor materiële contrasten, concrete attributiegrenzen, precieze termen, bewijsafwezigheid tegenover afwezigheidsbewijs en alleen ondersteunde chronologie. De userprompt blijft bytegelijk B. Geen bronnaam, antwoordlabel of bron-specifieke regel in B2. Promptomvang system+user: A 7.219 woorden, B 306, B2 420.

[verification-prompt-identity.json](verification-prompt-identity.json) controleert die identiteiten. Prompts staan onder [prompts/](prompts/). B2 is niet na outputs aangepast.

| Nieuwe bron | Woorden | Vaste Idea / gevarieerde evidence |
|---|---:|---|
| [water](cases/water/source.md) | 433 | Kalibratie versus pakketverlies; alarmdrempels, flushing/regen/selectie, brownouts, replayvariatie en beperkt besluit |
| [language](cases/language/source.md) | 454 | Terminologie versus betekenis, contrast/schermlezer versus keyboard, verschillende completiondefinities, case folding, juridische status |
| [heat](cases/heat/source.md) | 461 | Gemeten warmte versus verschillende energiebasis, pressure-hold/interlock versus heet oppervlak, thermal trips, simulatievariatie en kostenbegrenzing |

Alle drie bronnen zijn **nieuw synthetisch**, geen echte veldrapporten. De vijf Selected-Idea-velden staan vast in iedere `cases/<case>/input.json` en zijn per arm/repeat gelijk. De onderwerpen verschillen, maar de dossiers hebben bewust verwante foutfamilies en expliciete waarschuwingszinnen. Ze zijn korter en duidelijker gestructureerd dan de nieuwe Experiment-6-dossiers. Dit beperkt de moeilijkheid en generaliseerbaarheid; het is geen onafhankelijke natuurlijke benchmark. De bronnen, labels, B2 en beoordeling zijn van dezelfde Work-assistent. Er is geen onafhankelijke menselijke validatie en geen echte blindering.

Drie bronnen × drie herhalingen × A/B/B2 = **27 geplande en uitgevoerde calls**, cyclisch afwisselende armvolgorde. Maximaal 27 HTTP-attempts, retries 0, geen repairs/vervangingen, 5.000 outputtokens per call inclusief reasoning (135.000 worst-case), 120.000 UTF-8 requestbytes per call. Bytecap is geen exact inputtokenplafond. Geen geverifieerd Luna-geldtarief beschikbaar, dus geen fictief eurobudget. Iedere call een verse sessie/client, hetzelfde `gpt-6-luna`, geen seed- of temperatuurwijziging. Werkelijke requests en responses zijn bewaard.

De geïsoleerde runner roept de echte `ResearchAgent` zonder judge aan. De productieparser verwerkt de structured ResearchResult-response; `SynthesisResearchFilter.ExtractFactsAndUnknowns` maakt de opgeslagen projectie. Geen gereconstrueerde benadering van dat filter. Labels/rubric komen niet in requests. [verification-requests.json](verification-requests.json) controleert exacte systeemtekst, exact via het productietemplate gerenderde bron plus vijf Idea-velden, model, outputcap en HTTP-resultaat.

## Beoordeling en traceability

Zeven dimensies, elk 0/1/2: brontrouw, dekking, nuances/contrasten, attributie, epistemische precisie, consistentie en feitelijke bruikbaarheid van de projectie. Volledig voldoende vereist overal 2; geen compenserend totaalcijfer. Kernomissie geeft 0, belangrijk gemis 1. Een klein concreet precisiegebrek kan 1 geven zonder materiële onbruikbaarheid. De volledige Research telt voor brontrouw/precisie; de echte projectie telt voor downstreamdekking. Daarmee worden kleine fouten buiten de projectie zichtbaar zonder ze een Synthesis-fout te noemen.

[traceability.md](traceability.md) en [traceability.json](traceability.json) bevatten 324 bron/run-labelbeoordelingen met exacte broncitaten, regelnummers en volledige oorspronkelijke outputregels. 323 correct, één ontbreekt; geen gedeeltelijke of ongefundeerde materiële labelcomponent aangetroffen. De regex verzamelt alleen vindankers, beoordeelt geen semantiek. De Work-assistent heeft de drie bronnen en alle 27 volledige Research-outputs gelezen. [precision-audit.json](precision-audit.json) bewaart aanvullende output-naar-brongebreken. Citaten zijn exact op aanwezigheid gecontroleerd; een omissie krijgt geen verzonnen outputcitaat.

| Case | A strikt voldoende | B strikt voldoende | B2 strikt voldoende | Kern-only sensitiviteit A/B/B2 |
|---|---:|---:|---:|---|
| water | 3/3 | 0/3 | 2/3 | 3/3, 3/3, 3/3 |
| language | 3/3 | 3/3 | 2/3 | 3/3, 3/3, 3/3 |
| heat | 2/3 | 1/3 | 3/3 | 3/3, 3/3, 3/3 |
| Totaal | **8/9** | **4/9** | **7/9** | **9/9 voor elke arm** |

De uitkomsten zijn sterk rubricgevoelig: B's lagere strikte score komt geheel door kleine chronologieclaims, niet door verlies van materiële bronlabels. Als alle kleine buiten-projectiegebreken worden geaccepteerd, wordt de descriptieve uitkomst A 8/9, B 9/9, B2 9/9. Dat is een aanvullende gevoeligheidslezing, geen achteraf gewijzigde GO-poort. Kern-only negeert ook het belangrijke kostenlabel en komt bij alle armen op 9/9. Deze lezingen tonen waarom 4/9 niet als vier feitelijk bruikbare B-projecties of algemene betrouwbaarheid mag worden uitgelegd.

## Concrete bronbevindingen en omissies

**Contrasten overleven bij alle armen in alle repeats.** Waterbron: “All twelve loggers passed the static reference-gauge calibration at 2.0 bar. Four nevertheless failed the overnight packet-delivery check, losing 17% of scheduled messages in that check.” Alle negen wateroutputs/projecties behouden de succesvolle kalibratie én verliescheck met open fout. Taalbron: “An automated colour-contrast scan passed the three tested page templates. Keyboard-only testing could not activate the document-upload button on the second template.” Alle negen taaloutputs bewaren dit contrast, plus de correct uitgesproken schermlezerlabelcheck zonder keyboardbewijs. Warmtebron bewaart overal pressure-hold/interlock-pass tegenover twee gefaalde hatchchecks en het nog niet geïnstalleerde schild. Er is dus op deze bronnen geen aantoonbaar B2-dekkingsvoordeel tegenover B.

**Attributie en precieze termen overleven eveneens.** Alle armen bewaren alarmuren en gewijzigde drempels; completion met submitted application tegenover review screen en gemodereerd tegenover ongemodereerd; boiler lower-heating-value tegenover elektrisch meterinput inclusief pomp. Concrete selectie/omstandigheden en alle drie repeatwaarden blijven behouden. Brownouts blijven spanningsdips/device restarts, case folding lettercase-normalisatie, thermal trips beschermende overtemperatuuruitschakelingen. Geen herhaling vertaalt die laatste naar ritten. Facts/Unknowns bewaren bekende negatieve controles en grenzen; geen materiële contradictie of blanket-Unknown aangetroffen. Dat is assistentbeoordeling, geen bewijs dat elke zin foutloos is.

**Werkelijk belangrijk gat: heat-r3-A.** De bron zegt: “The finance appendix reports a EUR 9,200 rental allowance. It gives no date or sequence relative to testing. The allowance excludes installation, downtime and maintenance, so no payback claim is supported.” Deze volledige component ontbreekt zowel in [Research](calls/heat-r3-A-Research/content.md) als in [projectie](calls/heat-r3-A-Research/projection.md). A-r1/r2 en alle B/B2-outputs behouden haar. Dit is generatieverlies, niet filterverlies. Omdat A-r3 geen paybackclaim doet en de kern van de vaste Idea intact blijft, scoren dekking, nuance en bruikbaarheid 1, niet 0. Productie is dus evenmin herhaalbaar volledig.

**Kleine B2-gebreken buiten de projectie:**

- water-r1-B2, Development sequence: “The committee then authorized a six-week paired study with unchanged thresholds, not network-wide deployment.” De bron vermeldt de autorisatie, maar geen expliciete volgorde tegenover de reviewstatus. “then” voegt die toe. Facts 9 bevat het juiste beperkte besluit. De andere twee B2-wateroutputs doen deze toevoeging niet.
- language-r2-B2, Development sequence: “a fix proposal as approved but not deployed”. De bron zegt “no deployed fix is recorded”, wat minder sterk is. Facts 5 en Unknowns behouden die registratiegrens wel correct. De andere twee B2-taaloutputs houden die precisie intact.

B heeft vergelijkbare kleine chronologieproblemen in water-r1 (“then”), water-r2 (“After reviewing the pilot”), water-r3 (“legacy fortnight preceded” en “After review”) en heat-r2/r3 (“subsequently”). Zie de volledige oorspronkelijke regels in de precisie-audit. Vooral “legacy” en een review vóór een commissiebesluit zijn verdedigbare dagelijkse inferenties; onder de vooraf strikte regel alleen expliciet ondersteunde chronologie krijgen ze toch 1. Ze worden niet opgeblazen tot materiële kernfouten. A voegt bij taal-r1 “later” toe aan de tweede review; een tweede reviewer die de geïdentificeerde zes reversals bevestigt veronderstelt wel de eerdere identificatie, dus dit is inhoudelijk gedragen. Gewone lijstvolgorde is niet als chronologieclaim behandeld.

**Selectiviteit die geen falen is:** water B-r2/r3 laten training attendance/competentie weg; heat A-r1/r2/r3 laten de asset-registercorrectie weg; heat B-r3 laat die correctie en de operatorpoll weg. Die zijn vooraf context, niet verplicht voor deze Ideas zolang de output geen competentie-, vervangings- of populatieclaim doet. Geen van die outputs maakt zo'n claim. A-r3 laat bovendien supplier-marketing/poll weg; de projectie bewaart wel de niet-gemeten grid-carbon- en emissiegrens. Geen nieuwe harde labels achteraf toegevoegd.

## Scores per output

BT / DK / NU / AT / EP / CO / BR = brontrouw / dekking / nuances / attributie / epistemische precisie / consistentie / bruikbaarheid projectie. Twee is voldoende, één voorwaardelijk. Nul komt hier niet voor.

| Run | BT | DK | NU | AT | EP | CO | BR | Strikt voldoende |
|---|---:|---:|---:|---:|---:|---:|---:|---|
| water-r1-A | 2 | 2 | 2 | 2 | 2 | 2 | 2 | ja |
| water-r1-B | 1 | 2 | 2 | 2 | 1 | 2 | 2 | voorwaardelijk |
| water-r1-B2 | 1 | 2 | 2 | 2 | 1 | 2 | 2 | voorwaardelijk |
| language-r1-B | 2 | 2 | 2 | 2 | 2 | 2 | 2 | ja |
| language-r1-B2 | 2 | 2 | 2 | 2 | 2 | 2 | 2 | ja |
| language-r1-A | 2 | 2 | 2 | 2 | 2 | 2 | 2 | ja |
| heat-r1-B2 | 2 | 2 | 2 | 2 | 2 | 2 | 2 | ja |
| heat-r1-A | 2 | 2 | 2 | 2 | 2 | 2 | 2 | ja |
| heat-r1-B | 2 | 2 | 2 | 2 | 2 | 2 | 2 | ja |
| water-r2-B | 1 | 2 | 2 | 2 | 1 | 2 | 2 | voorwaardelijk |
| water-r2-B2 | 2 | 2 | 2 | 2 | 2 | 2 | 2 | ja |
| water-r2-A | 2 | 2 | 2 | 2 | 2 | 2 | 2 | ja |
| language-r2-B2 | 1 | 2 | 2 | 2 | 1 | 2 | 2 | voorwaardelijk |
| language-r2-A | 2 | 2 | 2 | 2 | 2 | 2 | 2 | ja |
| language-r2-B | 2 | 2 | 2 | 2 | 2 | 2 | 2 | ja |
| heat-r2-A | 2 | 2 | 2 | 2 | 2 | 2 | 2 | ja |
| heat-r2-B | 1 | 2 | 2 | 2 | 1 | 2 | 2 | voorwaardelijk |
| heat-r2-B2 | 2 | 2 | 2 | 2 | 2 | 2 | 2 | ja |
| water-r3-B2 | 2 | 2 | 2 | 2 | 2 | 2 | 2 | ja |
| water-r3-A | 2 | 2 | 2 | 2 | 2 | 2 | 2 | ja |
| water-r3-B | 1 | 2 | 2 | 2 | 1 | 2 | 2 | voorwaardelijk |
| language-r3-A | 2 | 2 | 2 | 2 | 2 | 2 | 2 | ja |
| language-r3-B | 2 | 2 | 2 | 2 | 2 | 2 | 2 | ja |
| language-r3-B2 | 2 | 2 | 2 | 2 | 2 | 2 | 2 | ja |
| heat-r3-B | 1 | 2 | 2 | 2 | 1 | 2 | 2 | voorwaardelijk |
| heat-r3-B2 | 2 | 2 | 2 | 2 | 2 | 2 | 2 | ja |
| heat-r3-A | 2 | 1 | 1 | 2 | 2 | 2 | 1 | voorwaardelijk |

## Werkelijk tokenverbruik en kosten per voldoende output

Usage komt rechtstreeks uit de 27 providerresponses, niet uit lokale schattingen. Alle calls HTTP 200, model gpt-6-luna, finish_reason stop. Geen truncatie, parserfout of structuurfout; alle vijf headings in volgorde. Geen evaluatiecalls. De lopende Work-beoordeling is assistentwerk; haar platformkosten staan niet in deze experimentele providerusage.

| Arm | Calls | Voldoende | Input | Output | I+O | I+4O | Tokens/voldoende | I+4O/voldoende |
|---|---:|---:|---:|---:|---:|---:|---:|---:|
| A | 9 | 8 | 91.440 | 22.169 | 113.609 | 180.116 | 14.201,1 | 22.514,5 |
| B | 9 | 4 | 10.017 | 15.495 | 25.512 | 71.997 | 6.378,0 | 17.999,2 |
| B2 | 9 | 7 | 11.304 | 13.160 | 24.464 | 63.944 | 3.494,9 | 9.134,9 |

Totaal **163.585 tokens: 112.761 input en 50.824 output**. Reasoning: 18.046, al in output inbegrepen en niet opnieuw opgeteld. I+4O is een gevoeligheidsweging, geen geldtarief. B2 bespaart tegenover A 78,47% I+O en 64,50% I+4O; per strikt voldoende output respectievelijk 75,39% en 59,43%. B bespaart 77,54% I+O en 60,03% I+4O; per strikt voldoende output 55,09% en 20,05%. B2 gebruikt 4,11% minder I+O dan B, ondanks de grotere prompt; output/reasoningvariatie draagt daaraan bij. Dit kleine verschil toont geen stabiele kostenrangorde tussen B en B2.

Cached input A/B/B2: 66.434 / 6.660 / 7.518. Cache-writevelden: 24.979 / 3.330 / 3.759. Deze zijn inputdetails en worden niet bovenop token-total opgeteld. B en B2 hebben in hun eerste drie calls geen cached tokens; A heeft al 2.746 cached tokens bij taal-r1 en heat-r1, naast nul bij water-r1. Latere calls profiteren bij alle armen van vrijwel volledige inputcache. Cachegedrag is niet gerandomiseerd of geïsoleerd en kan door gedeelde promptprefixen en tijdsvolgorde zijn beïnvloed. Vergelijking van alleen totaaltokens overschat mogelijk geldbesparing als cached input veel goedkoper is. Na aftrek van cached input zijn de niet-cached inputs per strikt voldoende output ongeveer A 3.125,8; B 839,3; B2 540,9. Output per voldoende: A 2.771,1; B 3.873,8; B2 1.880,0. B2 gebruikt dus onder deze scores zowel minder niet-cached input als minder output per voldoende resultaat dan A/B; ook cached input per voldoende ligt lager. Werkelijke prijs blijft conditioneel.

Voor geverifieerde tarieven per miljoen: `((I-C)*p_input + C*p_cached + O*p_output)/1.000.000`, plus alleen aantoonbare afzonderlijke cache-write-opslag. Deel de volledige armkosten door het aantal voldoende outputs; geen kosten van onvoldoende outputs wegstrepen. Nul successen zou ongedefinieerde kosten per succes geven. Geen verifieerbaar Luna-tarief/factuur beschikbaar: geen euro/dollarprijs geclaimd. Onder de mildere lezing waarin kleine buiten-projectiefouten acceptabel zijn, zijn B/B2 kosten per voldoende uiteraard lager doordat de noemer 9 wordt; deze kosten hangen dus ook van beoordelingsstrengheid af.

## Herhaalbaarheid, besluit en beperking

Alle drie armen bewaren de kern over 3/3 repeats per bron. A mist één keer een belangrijk financieel voorbehoud. B's buiten-projectiechronologie is terugkerend bij water (3/3) en heat (2/3). B2 heeft daar één waterincident en één taalincident met te stellige deploystatus. De individuele fouten zijn niet stabiel over alle herhalingen; een geslaagde eerste output zou de andere gebreken missen. B2 toont hier minder kleine precisieproblemen dan B, maar geen bewezen algemeen kwaliteitsvoordeel. De 27 outputs zijn afhankelijk binnen slechts drie geconstrueerde bron/Idea-paren; negen outputs per arm zijn geen negen onafhankelijke domeinen en leveren geen statistische zekerheid.

Bevroren GO-poort: minstens 8/9 strikt voldoende, elke bron minstens 2/3, minstens evenveel als A/B, geen herhaalde kernfout of materiële regressie tegenover voldoende A, minstens 20% lagere tokens én gewogen tokens dan A ook per voldoende output. B2 haalt de case-, kern- en kosteneisen maar faalt 8/9 en minstens zoveel als A. **Daarom NO-GO, zonder de drempel achteraf te versoepelen.** Geen reden uit deze proef om A als algemeen betrouwbaarder of B2 als nutteloos te verklaren.

**Research blijft open.** De proef laat veelbelovende compacte evidence zien, maar onafhankelijk gevalideerde labels en natuurlijke, langere gevarieerde bronnen ontbreken. Een zinvolle volgende afzonderlijke proef zou de huidige B/B2 bevroren houden, de bestaande precisieoordelen onafhankelijk laten controleren en vooraf nieuwe natuurlijke bron/Idea-paren selecteren. Geen automatische nieuwe calls of verdere prompttuning uitgevoerd. Zelfs een GO op deze synthetische proef zou geen productiepromotie of volledige afronding rechtvaardigen; daarvoor is bredere en onafhankelijke steun nodig. De bronwaarschuwingen maken dit vooral een test van het bewaren van expliciete beperkingen, niet van het zelfstandig ontdekken van verborgen methodologische problemen.

## Reproduceerbaarheid en integriteit

Runnerbuild slaagt met nul warnings/fouten. [verification-offline-replay.json](verification-offline-replay.json): alle 27 ruwe antwoorden opnieuw verwerkt door actuele productieparser, ResearchResults exact gelijk en alle projecties exact gelijk aan het echte filter. Geen netwerkclient bij replay. Geen brede testsuite opnieuw uitgevoerd omdat productiecode/configuratie niet is gewijzigd; compile, exacte replay, requestcontrole, citaatcontrole en hashcontrole richten zich op de relevante risico's.

[verification-integrity.json](verification-integrity.json): 1.592 freezehashes intact, inclusief alle 1.508 beschermde gevolgde bestanden. [verification-requests.json](verification-requests.json): 27/27 exact juiste requests en HTTP 200. `git diff --check` schoon; gitstatus toont uitsluitend deze nieuwe experimentmap. Geen oorspronkelijke bron, prompt, code, configuratie of eerder experiment veranderd. Geen commit/push.

Artefacten: `prompts/`, `cases/`, `labels.json`, `rubric.md`, `protocol.md`, `plan.json`, `freeze.json`, `protected-before.json`, `head.txt`, `runner/`; `calls/<id>-Research/` met attemptmarker, input, systeemtekst, exact request, raw response, metadata, volledige Research en echte projectie; `usage.json`, `results.json`, `assessments.json`, `traceability.json/.md`, `precision-audit.json`, verificaties, build/run/replaylogs. Zie [REPLAY.md](REPLAY.md) voor offline gebruik. **Niet opnieuw live uitvoeren; run.lock en attemptmarkers behouden.**
