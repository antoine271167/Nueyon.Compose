# Experiment 9 — Research editorial-readiness acceptatie

16 november 2026. **GO voor Research met de ongewijzigde B2-kandidaat; advies: promoveer B2 na afzonderlijke toestemming.** B2 levert 12/12 voldoende resultaten tegenover A 11/12, met 68,64% minder I+O-tokens en 57,01% minder I+4O. De vooraf vastgelegde poort is gehaald zonder wijziging. Dit is een pragmatisch, beperkt Research-oordeel voor brongetrouwe en interessante contentgrondstof, geen wetenschappelijke volledigheid of bewijs van algemene foutloosheid.

Er is niets gepromoveerd, gecommit of gepusht. Productiecode, productieprompts, configuratie en eerdere experimenten blijven bytegelijk.

## Vraag en afbakening

Nuëyon.Compose maakt aantrekkelijke, interessante, prettig leesbare content voor LinkedIn, artikelen en andere kanalen. Research hoeft geen artikel of academisch dossier te schrijven. Het moet de vaste Idea dragen met betrouwbare feiten, concrete voorbeelden, relevante spanningen en betekenisveranderende beperkingen. Agents vóór Compose blijven onafhankelijk van publicatieformat.

Alleen Research is uitgevoerd: vier bron/Idea-paren × drie gepaarde herhalingen × A/B2 = 24 generaties met gpt-6-luna. Geen Idea-, Synthesis-, Narrative- of Compose-agent; geen Judge, Evidence Mapper, subagent of extra evaluatiemodel. De echte SynthesisResearchFilter is uitsluitend als deterministisch filter gebruikt, zonder Synthesis uit te voeren. Geen eindpostscores of downstreamtests.

Experiment 7 en 8 report/protocol, actuele A en exact bevroren B2 uit Experiment 8 zijn gelezen. Experiment 7 corrigeerde onterechte toeschrijving van downstreamverlies aan Research. Experiment 8 had veelbelovende B2-projecties maar faalde zijn eigen strengere poort op twee kleine buiten-projectieproblemen. Die oude resultaten/poort zijn niet gewijzigd. Experiment 9 legt een afzonderlijke editorial-readiness poort vast vóór nieuwe calls.

Lokale repo C:\Workspaces\Nueyon.Compose, HEAD ed1682f37659d619edf58c8e2d3cbbf09d5acfed; origin https://github.com/antoine271167/Nueyon.Compose.git. Geen fetch of claim dat remote intussen ongewijzigd is. De gekoppelde chat is als context gelezen; conclusies volgen uit lokale artefacten.

## Natuurlijke bronnen en bronfideliteit

Aanvankelijk was geen geschikte lokale set beschikbaar: project-sources leeg, eerdere dossiers synthetisch, data/ slechts één gereconstrueerde bedrijfsbrief. Daarom zijn eerst nul calls gedaan en broninput gevraagd. Op de vervolgvraag om passende internetteksten te downloaden zijn onderstaande oorspronkelijke bronnen opgehaald. Geen synthetische vervangers of speciaal voor deze test geschreven dossiers.

| Case | Natuurlijke bron | Woorden | Vaste Idea |
|---|---|---:|---|
| webb | [NASA — Deployment](https://science.nasa.gov/mission/webb/deployment/) | 2215 | Uitgevouwen is nog niet klaar: de lange weg naar Webbs eerste wetenschap |
| wolf | [NPS — Wolf Ecology](https://www.nps.gov/yell/learn/nature/wolf.htm) | 1620 | Een wolf is geen losse schakel: Yellowstone als levend netwerk |
| digital | [Ministry of Justice — praktijkblog uit 2018](https://mojdigital.blog.gov.uk/2018/03/13/how-to-build-and-release-a-digital-by-default-service-in-12-weeks/) | 841 | Snel digitaal leveren begon hier met keuzes, niet met een formulier |
| lowell | [NPS — Lowell, Decline and Recovery; boek uit 1992](https://www.nps.gov/articles/lowell-handbook-decline-and-recovery.htm) | 1814 | Lowell vond een nieuw gebruik voor zijn verleden, maar ontsnapte niet aan verandering |

Originele HTML staat in downloads/; sources.json legt URLs, ophaaltijd en extractie vast. HTML-tags, navigatie en promotionele voetregels zijn verwijderd, inhoudelijke tekst niet herschreven. NASA-captions en archiefteksten zijn behouden, inclusief verschillende tijdsperspectieven. Lowell is expliciet een historisch boekperspectief uit 1992. Bij NPS zijn tekstblokken in documentvolgorde geëxtraheerd; figuren/video-inhoud zijn geen broninput. De 841-woorden praktijkblog is bewust korter dan de drie bronnen van 1.620–2.215 woorden: natuurlijke genrevariatie, geen claim dat alle vier lang zijn. Het protocol noemt abusievelijk nog 883 woorden voor die blog vóór verwijdering van de promotionele footer; sources.json en de gehashte input geven correct 841. Dit wijzigt geen broninput of poort na calls.

De publicaties zijn herkenbare institutionele originelen, maar hun onderliggende waarnemingen zijn niet onafhankelijk geverifieerd. Het is geen representatieve dataset: alle teksten Engels, twee NPS, overwegend publieke instellingen en publieksuitleg. Vier bron/Idea-paren met repeats zijn geen twaalf onafhankelijke domeinen. Lastige ruwe interviews, commerciële teksten en zeer lange chaotische documenten vallen buiten deze proef.

De vijf Idea-velden per bron zijn handmatig vooraf vastgelegd in cases/*/input.json; nergens wordt een Idea-agent gebruikt. Labels zijn betekenisgericht en bevatten niet ieder bronfeit. Kleuren-genetica bij wolven, yoga bij de productmanager of ieder historisch millbedrijf zijn niet automatisch vereist voor deze Ideas.

## Prospectieve freeze, budget en beslispoort

[protocol.md](protocol.md), [plan.json](plan.json), [labels.json](labels.json), sources.json en inputs waren vóór calls vastgelegd. Freeze UTC: 2026-11-16T20:36:29.564Z. freeze.json bevat 1885 hashes van beschermde repositorybestanden, prompts, bronbestanden, protocol, labels, runner en runtime. prompt-freeze.json controleert A/B2-byteidentiteit afzonderlijk. A is de actuele productie-system/userprompt; B2 exact de Experiment-8-system/userprompt. Er is geen prompttekst gewijzigd.

De runner is geïsoleerd overgenomen uit Experiment 8; alleen de globale attemptcap is aangepast van 27 naar 24. Hij roept de echte ResearchAgent zonder judge aan, met productieparser en echt filter. Elke call heeft een verse client/sessie; afwisselende armvolgorde over bronnen en repeats; zelfde modelinstellingen, geen seed/temperatuurwijzigingen. Labels/rubric komen niet in requests.

Hard budget: 24 HTTP-attempts, retries 0, geen repairs of vervanging, maximaal 5.000 outputtokens inclusief reasoning per call (120.000 worst-case output) en 120.000 UTF-8 requestbytes per call. Bytecap is geen exacte inputtokencap. Er is geen geverifieerd Luna-geldtarief, dus geen verzonnen europlafond. Werkelijk 24/24 calls, elk HTTP 200 en finish_reason stop; geen truncatie, parser- of contractfout.

Vijf dimensies per run: brontrouw; Idea-dragende kern; betekenisveranderende nuances; voldoende concrete editorial grondstof; bruikbare echte Facts/Unknowns-projectie. 2 voldoende, 1 concreet relevant gebrek/beperkt bruikbaar, 0 materieel misleidend/onbruikbaar. Geen compenserende totaalscore; volledig voldoende vereist vijfmaal 2. Concrete grondstof vereist minstens drie verschillende brongebonden details plus een bruikbaar contrast/voorbeeld, geen pakkende proza-uitvoer. Kleine niet-materiële formulering/chronologie buiten de projectie faalt niet automatisch; binnen-projectieproblemen krijgen eveneens een expliciete materialiteitsbeoordeling.

Bevroren GO: B2 minstens 11/12 voldoende, elke bron minstens 2/3, minstens evenveel voldoende als A, geen materiële verzonnen/misleidende claim, geen terugkerende kernomissie/materiële regressie op voldoende A; minstens 20% minder I+O én I+4O, totaal én per voldoende resultaat. Alle eisen gehaald. Geen poortwijziging na inzage in antwoorden.

## Kwaliteit en concrete evidence

| Bron | A voldoende | B2 voldoende |
|---|---:|---:|
| Webb | 3/3 | 3/3 |
| Yellowstone | 3/3 | 3/3 |
| Digitale dienst | 3/3 | 3/3 |
| Lowell | 2/3 | 3/3 |
| **Totaal** | **11/12** | **12/12** |

[traceability.json](traceability.json) en [traceability.md](traceability.md) bewaren 150 bron/run-labelbeoordelingen: 149 correct en één deels. Exacte broncitaten en regelnummers staan naast volledige gevonden output/projectieregels. Regex verzamelt vindankers, beslist geen semantische juistheid. Alle vier volledige bronnen en alle 24 volledige Research-outputs zijn door de Work-assistent gelezen. De echte projectie bevat precies de opgeslagen Facts/Unknowns; aanwezigheid alleen buiten die secties telt niet als dekking. Correct op een label betekent geen foutloze hele output.

**Webb — uitgevouwen is nog geen routinewetenschap.** Alle zes projecties behouden vouwontwerp, concrete risico's, grondcontrole/telemetrie, zonneschild versus spiegelstappen, afkoeling, uitlijning, kalibratie en wetenschap na zes maanden. Bron: “The remaining five months of commissioning were all about aligning the optics and calibrating the scientific instruments.” B2-r1 Facts 8–12 bewaren dit met concrete details, waaronder heaters die water laten ontsnappen zonder ijs op de optiek, en verstoorde sterbeelden vóór uitlijning. Geen verhaalvereiste dat iedere deploymentstap moet worden opgesomd.

**Yellowstone — meer dan één roofdier dat alles bestuurt.** Alle zes bewaren 41 uitgezette dieren, sociale samenwerking, aaseters rond kills, veranderende prooikeuze en de concrete factoren buiten wolven alleen. Bron noemt “other predators, management of ungulates outside the park, and weather”. B2-r2 Facts 4–6 bewaren zowel aaseters/beren als bison vooral via aaseten en de factoren weer/beheer/andere predatoren. Brede bewezen ecosysteemherstelclaims worden niet toegevoegd. B2 bevat soms meer populatie- en ziektecontext dan A; dit is geen scorebonus op zichzelf.

**Digitale dienst — interessante praktijk, begrensde uitkomsten.** Alle zes bewaren twaalf weken, digitaal plus assisted digital zonder papier, eerst aanvragers en later caseworkers, en het concrete sprintpatroon met inzendingen pauzeren in de tweede week. De bron rapporteert “over 100k applications with 92.6% user satisfaction and an 80.2% completion rate.” Alle zes plaatsen die cijfers bij vijf weken en verwarren aanvragen niet met uitgekeerde refunds. Details van assisted-digital ondersteuning en causale effecten van losse praktijken blijven onbekend; geen academische studie nodig om de beschreven werkwijze als praktijkervaring te gebruiken.

**Lowell — bruikbaar herstelverhaal mét mensen en terugval.** Alle zes behouden burgerinitiatief, erfgoedparken, onderwijs/technologiecontext, hergebruik en late-1980s terugval, plus het historische perspectief. Alle B2-runs hebben concrete menselijke ervaringen; r1/r2 noemen paardenbloembladeren en hard brood, r3 noemt herhaald naar huis gestuurd worden bij ontbrekend werk. Het verhaal wordt geen ononderbroken succes door erfgoed alleen.

**Eén relevant A-gat:** lowell-r3-A mist het vooraf als kern gelabelde vroege contrast tussen stijgende productie en dalend werk. Bron: “Technological advances made possible gains in output even while mills trimmed their workforce.” Fact 1 bewaart de banenafname en Facts 2–4 niet-moderniseren/verplaatsing, maar de technologische outputgroei ontbreekt in volledige Research en projectie. Fact 17 over productie eind jaren dertig is een ander tijdvak en vervangt het contrast niet. Daarom kern/nuance/projectie 1: nog bruikbaar, concreet onvolledig, geen materieel verzonnen herstelclaim. De andere vijf Lowell-runs behouden het contrast. Dit enkele verschil bewijst geen algemene superioriteit van B2; zelfs als A-r3 onder een mildere lezing voldoende zou zijn, blijft B2 12/12 en voldoet het aan minstens-A.

## Kleine gebreken blijven zichtbaar

[precision-audit.md](precision-audit.md) en .json leggen exacte output- en broncitaten vast. Geen van deze punten verandert de kernbetekenis onder de bevroren rubric. Er zijn nadrukkelijk ook kleine problemen in de projectie, niet alleen erbuiten:

- Webb B2-r2 Fact 7 zegt “Deployment was followed by cooldown”. Dat suggereert te scherpe fasering; de bron spreekt ook over voortgezette afkoeling. Eerste-maand- en zesmaandengrens blijven correct. B2-r3 heeft vergelijkbare “Daarna”-compressie alleen buiten de projectie. Geen materiële nieuwe uitkomst.
- Digital B2-r3 specificeert “adres-/contactgegevens” waar de bron slechts niet-bijgewerkte “details” zegt. Onbewezen extra precisie, maar de relevante directe-refundbeperking verandert niet. Het geciteerde anker “We focused…” is niet letterlijk “My team focused…”; de onderliggende claim is wel juist. Traceability gebruikt de echte bronzin.
- Lowell B2-r3 voegt “later” toe aan het nieuwe gebruik van gebouwen; die aparte volgorde is niet vastgesteld. Hergebruik en historische scope zijn correct.
- Wolf B2-r3 suggereert buiten de projectie een onopgeloste spanning tussen het jubileum en de uitzetperiode. Een jubileum van de eerste terugkeer is gewoon verenigbaar met 1995–1997. Onnodige twijfel, geen foutieve kernclaim.
- A digital-r3 zegt buiten de projectie “Late the following year” tegenover bron “Late last year”; Fact 11 behoudt de juiste relatieve tijd. A trekt in alle drie digitale runs de duidelijke financiële aanleiding onnodig uit elkaar in Unknowns. De concrete fee- en refundfeiten blijven bruikbaar. A Webb-r1 heeft een te brede Unknown over ontwerpredenen, terwijl Facts de concrete tower/heater-rationale wel behouden.

We aanvaarden deze punten als klein om hun concrete betekenis, niet omdat B2 goedkoper is. Dezelfde materialiteitsregel geldt voor A. Er is geen uitspraak dat elk citaatanker van de gegenereerde output letterlijk correct is of elke zin perfect brongetrouw. Een systeem dat exacte citeerbare academische bewijsvoering vereist zou hiermee niet zijn gevalideerd; dat is ook niet het productdoel.

## Scores per run

BT / K / N / D / P = brontrouw, kern, nuance, concrete grondstof, echte projectie. Kleine auditpunten hierboven zijn geen automatische 1. Alleen het concrete relevante gat krijgt een lagere readinessscore.

| Run | BT | K | N | D | P | Oordeel |
|---|---:|---:|---:|---:|---:|---|
| webb-r1-B2 | 2 | 2 | 2 | 2 | 2 | voldoende |
| webb-r1-A | 2 | 2 | 2 | 2 | 2 | voldoende |
| wolf-r1-A | 2 | 2 | 2 | 2 | 2 | voldoende |
| wolf-r1-B2 | 2 | 2 | 2 | 2 | 2 | voldoende |
| digital-r1-B2 | 2 | 2 | 2 | 2 | 2 | voldoende |
| digital-r1-A | 2 | 2 | 2 | 2 | 2 | voldoende |
| lowell-r1-A | 2 | 2 | 2 | 2 | 2 | voldoende |
| lowell-r1-B2 | 2 | 2 | 2 | 2 | 2 | voldoende |
| webb-r2-A | 2 | 2 | 2 | 2 | 2 | voldoende |
| webb-r2-B2 | 2 | 2 | 2 | 2 | 2 | voldoende |
| wolf-r2-B2 | 2 | 2 | 2 | 2 | 2 | voldoende |
| wolf-r2-A | 2 | 2 | 2 | 2 | 2 | voldoende |
| digital-r2-A | 2 | 2 | 2 | 2 | 2 | voldoende |
| digital-r2-B2 | 2 | 2 | 2 | 2 | 2 | voldoende |
| lowell-r2-B2 | 2 | 2 | 2 | 2 | 2 | voldoende |
| lowell-r2-A | 2 | 2 | 2 | 2 | 2 | voldoende |
| webb-r3-B2 | 2 | 2 | 2 | 2 | 2 | voldoende |
| webb-r3-A | 2 | 2 | 2 | 2 | 2 | voldoende |
| wolf-r3-A | 2 | 2 | 2 | 2 | 2 | voldoende |
| wolf-r3-B2 | 2 | 2 | 2 | 2 | 2 | voldoende |
| digital-r3-B2 | 2 | 2 | 2 | 2 | 2 | voldoende |
| digital-r3-A | 2 | 2 | 2 | 2 | 2 | voldoende |
| lowell-r3-A | 2 | 1 | 1 | 2 | 1 | onvolledig |
| lowell-r3-B2 | 2 | 2 | 2 | 2 | 2 | voldoende |

## Werkelijke usage en kostengevoeligheid

Direct uit de 24 ruwe providerresponses; [usage.json](usage.json) bewaart alle originele usagevelden. Reasoning is al onderdeel van output, cached input van input. I+4O is uitsluitend sensitiviteit, geen factuurtarief.

| Arm | Calls | Voldoende | Input | Output | I+O | I+4O | Tokens/voldoende | I+4O/voldoende |
|---|---:|---:|---:|---:|---:|---:|---:|---:|
| A | 12 | 11 | 140.223 | 33.247 | 173.470 | 273.211 | 15.770,0 | 24.837,4 |
| B2 | 12 | 12 | 33.375 | 21.022 | 54.397 | 117.463 | 4.533,1 | 9.788,6 |

Totaal 227.867 tokens: 173.598 input en 54.269 output. A/B2 cached input: 104.442 / 22.226; reasoning: 12.551 / 6.987, niet opnieuw optellen. Besparing B2 totaal 68,64% I+O en 57,01% I+4O; per voldoende 71,26% en 60,59%. Zelfs als alle twaalf A-runs voldoende worden geteld, blijft de totale procentuele besparing per voldoende gelijk aan de totale besparing, ruim boven 20%.

Werkelijke geldkosten onbekend. Bij geverifieerde tarieven per miljoen: ((I−C)×p_input + C×p_cached + O×p_output)/1.000.000, plus alleen aantoonbare afzonderlijke cache-write-opslag. Cachevolgorde is niet geïsoleerd; gedeelde prefixes en timing kunnen kosten beïnvloeden. B2 heeft ook minder niet-cached input en output, maar tokens zijn geen geverifieerde eurobesparing. Work-beoordeling/platformkosten vallen buiten deze experimentele providerusage.

## Verificatie, beperking en besluit

De runnerbuild slaagt met nul warnings/fouten. Een eerste build zonder restore kon niet starten omdat de nieuwe experimentrunner nog geen assets had; normale restore/build slaagde daarna vóór freeze/calls. Productiecode is niet gewijzigd. [verification-requests.json](verification-requests.json): alle 24 exacte systeem- en gerenderde userrequests, juiste model/cap, HTTP 200 en stop. [verification-offline-replay.json](verification-offline-replay.json): 24/24 raw responses opnieuw door productie-ResearchAgent/parser; parsed resultaten exact gelijk en projecties exact gelijk via echte filter. Geen live client bij replay. Geen brede testsuite nodig voor alleen geïsoleerde experimentbestanden; gerichte build, replay, request- en hashcontrole toetsen de relevante risico's.

verification-integrity.json: alle 1885 freezehashes intact. protected-before.json omvat 1.795 vooraf bestaande gevolgde bestanden. Alleen deze nieuwe experimentmap verschijnt in gitstatus; git diff --check schoon. Geen eerdere experimenten of productiecode/prompts/config aangepast.

**Beperkingen:** dezelfde Work-assistent koos Ideas/kernlabels en beoordeelde outputs; geen onafhankelijke menselijke labelvalidatie, geen echte blindering, geen aanvullende evaluatorcall. Deterministische ankercontrole is geen onafhankelijke semantische review. Ook zijn kleine gebreken via materialiteitskeuzes beoordeeld; exacte evidence maakt die keuzes reviewbaar, niet objectief onbetwistbaar. Vier gekozen institutionele bronnen en drie repeats per cel bewijzen geen statistische non-inferioriteit, algemene betrouwbaarheid of downstreamkwaliteit.

**Besluit: GO voor Research en promotieadvies voor exact B2.** B2 voldoet op deze natuurlijke bronnen aan alle vooraf afgesproken kwaliteit-, repeat- en kostenvoorwaarden en levert voldoende interessante concrete grondstof. Geen materiële B2-fout of terugkerend kernverlies gevonden. Onder dit productdoel is er geen concreet ernstig Research-probleem dat nog een automatische tuningsronde rechtvaardigt. Research kan inhoudelijk worden afgerond met dit begrensde bewijs. Productiepromotie en een eventuele volgende Synthesis-opdracht blijven afzonderlijke expliciete handelingen; hier zijn ze niet uitgevoerd.

Artefacten: downloads/, sources.json, cases/, prompts/, protocol.md, labels.json, plan.json, freeze.json, runner/, calls/ met attemptmarkers, input, systeemtekst, requests, raw responses, HTTP-metadata, parsed resultaat, volledige Research en echte projectie; assessments.json, traceability.*, precision-audit.*, usage.json, results.json en verificaties. [REPLAY.md](REPLAY.md) beschrijft veilige offline herverwerking. **Niet opnieuw live uitvoeren; run.lock en attemptmarkers behouden.**
