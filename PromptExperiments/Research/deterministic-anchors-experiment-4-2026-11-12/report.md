# Experiment 4 — deterministische bronankers en matched map-ablatietest

12 november 2026. **NO-GO. Geen promotie, integratie of activering.**

Deterministische fragment-ID's lossen het letterlijk kopiëren door de mapper technisch op: **3/3 nieuwe maps, 37 entries en 123 ankerverwijzingen exact geldig**. Zij lossen selectie, semantiek en Judge-citaten niet op. Met identieke historische cases en exact dezelfde Judge-prompt vindt de Judge **zonder map 2/3 doelproblemen, met map 0/3**. Inclusief gedeelde mapgeneratie kost met-map **117.222 versus 44.273 tokens (+164,8%)** en negen versus zes calls. De map heeft in deze éénmalige gerichte proef geen aangetoonde incrementele waarde en geeft regressies. Dit bewijst geen algemene schadelijkheid van alle evidence-map-ontwerpen.

De toegestane null-check is lokaal hersteld en met een gerichte regressietest afgedekt. Solutionbuild: nul fouten/waarschuwingen; **300 tests geslaagd**. EnableResearchJudge houdt default false. Geen Research-generatorprompts, Synthesis-filter of andere workflowlogica gewijzigd. Geen Copilot CLI, commit of push.

## Uitgangspunt, freezes en budget

Repository `C:\Workspaces\Nueyon.Compose`, origin `https://github.com/antoine271167/Nueyon.Compose.git`. Begin-HEAD **094b2e539f7b625c0481d38f04d4105815a568dd**, schone beginstatus; remote HEAD is eveneens dit commit. Experiment 2 en 3, hun protocollen, bron/Idea/Research-fixtures, prompts, historische Intro-map/verdict, actuele parser, filter, defaultconfiguratie en tests geïnspecteerd. De gerefereerde chat is gelezen voor vervolgcontext; lokale owning sources en opgeslagen responses dragen de conclusies.

Vooraf vastgelegd: protocol.md, labels.json, plan.json, bronfixtures, fragmentcatalogi, mapper-/Judge-prompts en runner. freeze.json bevat **28 bestandshashes**, inclusief parser en regressietest. De offline voorbereiding had aanvankelijk een casingfout bij het lezen van `Input.Content`; deze is vóór freeze en vóór alle live calls hersteld. Geen labels, criteria, prompts of inputs na freeze aangepast. Finale freezecontrole slaagt. De oorspronkelijke Experiment-2/3-artefacten zijn alleen gelezen.

Hard toegestaan maximum: 18 live HTTP-requests. Vooraf gepland en uitgevoerd: **15 GPT-6 Luna-calls**, drie maps plus twaalf judges. De drie overige calls bleven ongebruikt. Nul retries, repairs of herhalingen. Alle 15 HTTP 200, responsemodel `gpt-6-luna`, finish_reason `stop`. Elke poging heeft een exclusieve marker, één request/response en HTTP-metadata. SDK RetryPolicy(0), handlerlimiet één, runnerlimiet 15. Per request was het outputmaximum 6000 tokens; geen truncatie. Authheaders en API-sleutels zijn niet gelogd.

De begroting legde calls en maximale completiontokens vast, niet een verzonnen geldbedrag. De 25.029 werkelijke completiontokens liggen onder het geplande maximum van 90.000. Inputtokens en kostenverhouding zijn gemeten. Er is geen betrouwbare europrijs vastgesteld.

## 1. Deterministische bronankers

De software deelt de originele bron op in ongewijzigde regels inclusief hun oorspronkelijke LF/CRLF, whitespace en Markdown. Elke span heeft een stabiele ID (`F0001` enz.), start en lengte in **UTF-16-code-units**, de exacte tekst en een SHA256 over de UTF-8-bron. De catalogus moet de gehele bron exact reconstrueren. Dit zijn stringoffsets, geen UTF-8-byteoffsets.

De mapper krijgt uitsluitend originalSource, alle geselecteerde Idea-velden en deze gecontroleerde catalogus. Hij retourneert `sourceFragmentIds` in plaats van zelf geciteerde tekst. De lokale, geïsoleerde runner weigert lege, onbekende of dubbele verwijzingen en haalt `sourceQuotes` rechtstreeks uit gevalideerde bronspans. `map-raw.json` blijft intact; `map.json` bevat de expliciet afgeleide citaten. Dit is een gedocumenteerde resolutie van referenties, geen stille reparatie van modelcitaten.

Offline checks testen volledige reconstructie, verkeerde offsets, onbekende/duplicaat-ID's, CRLF, Markdown, hoofdletters, decomposed Unicode en trailing whitespace. Casing, Unicode-normalisatie, newlineconversie of trim mogen de bron niet gelijk maken. verification-anchor-replay.json herhaalt bovendien de strikte substringcontrole op **hergebruikte** Experiment-3-maps: opnieuw slechts 3/6 volledig exact. Dat zijn geen nieuwe modelresultaten. De nieuwe drie maps hebben 123/123 exacte afgeleide ankers over 37 entries.

De garantie is beperkt tot provenance en letterlijke tekst. Een geselecteerd fragment kan irrelevant of onvoldoende zijn, en een parafrase kan alsnog onjuist zijn. De line-granulariteit geeft soms ruime citaten; tabellen kunnen aanvullende headers/context vereisen. Judge-verdicts gebruiken bewust het bestaande productiecontract en laten het model nog steeds sourceQuotes schrijven. Die grens blijft zichtbaar: de correcte Intro-controle met map bevat een corrupt Judge-citaat met `NuEbyon` en `U+0019`, ondanks exact goede mapperankers. De bestaande productieparser weigert dit terecht. Er is geen productiecontract uitgebreid.

## Voorafgaande mapaudit, zonder bijsturen

Alle maps waren gereed en map-audit.json was geschreven **vóór de eerste Judge-call**. Audit en labels zijn nooit meegestuurd. Iedere entry is apart beoordeeld op bronsteun, scope, grens en materialiteit. Geen map is verbeterd of opnieuw opgevraagd.

| Map | Nieuwe inhoudelijke dekking | Ankers |
|---|---|---|
| Intro | I3/I15 bewaren expliciet de concrete-expression-relatie; vroege status, audience en roadmap eveneens aanwezig | 15 entries, exact |
| 005 | I8 bewaart Synthesis-spanning; uitslagen, rollback, datums/backendgrens aanwezig; concrete Idea/Research-outputvariatie ontbreekt | 10 entries, exact |
| 010 | P7 bewaart Compose-feitfout met eerdere company-first-keten en hiërarchie 2/2; observatiegrenzen en aparte kwaliteitsdimensies aanwezig | 12 entries, exact |

Van twaalf vooraf bevroren historische referentiecomponenten zijn **elf volledig gedekt, één ontbreekt**. Slechts 2/3 maps dekken alle referentiecomponenten. De 005-bron bevat expliciet in F0100: `Idea and Research outputs vary between runs.` Algemene causaliteitsvoorzichtigheid in I2/I10 is geen vervanging van deze concrete waarneming. Dit is een echte selectiemisser bij de mapper.

De vrije parafrases van Intro/005 verbasteren soms de naam Nuëyon (`NuEbyon`/`Nuéyon`); de gehydrateerde broncitaten zijn intact en de referent is herkenbaar. Dit is niet stilzwijgend gecorrigeerd. Ook blijft `negativeControl` bij Intro een losse taxonomie voor bronuitsluitingen en scopegrenzen, geen uitgevoerde empirische controle. Exacte ankers maken deze inhoudelijke beperkingen controleerbaar, maar nemen ze niet weg.

## 2. Intro: historische false-negative gereproduceerd en afgebakend

De oorspronkelijke bron stelt expliciet dat Compose de bredere visie een concrete uitdrukking geeft (F0203), en noemt Compose de eerste concrete uitdrukking van de bedrijfsvisie (F0060). Baseline Facts 9/13 bewaren deze relatie. **Unknown 4 ontkent te breed dat het naast elkaar zetten van ambitie en Compose de ambitie concreet maakt**, en reduceert dit tot een redactionele Idea-beoordeling. Onbekend lezereffect en expliciet toegeschreven bronbetekenis zijn verschillende zaken. De echte filter geeft beide juiste Facts én de te brede Unknown door; de tegenstrijdigheid wordt dus niet weggefilterd.

Historisch, **hergebruikt**: Experiment 2 wees een ander punt af met een ongeldig citaat; Experiment 3 accepteerde baseline Intro ondanks map S2 met de juiste bronbetekenis. De historische acceptatie is offline uit het opgeslagen verdict gereproduceerd, niet opnieuw live aangevraagd. intro-analysis.md legt bron, Facts, Unknown, filter en oude map naast elkaar.

**Nieuw in Experiment 4:** zonder map accepteert de Judge baseline Intro opnieuw. Met map volgt een geldig geparseerde afwijzing, maar uitsluitend wegens ontbrekende duiding van AI-only. Het Unknown-4-doel wordt wederom gemist, terwijl I15 en F0203 beschikbaar zijn. Die afwijzing telt dus **niet** als targetdetectie. Voor de correcte Intro-controle produceert de met-map-Judge twee andere blockers, waarvan één met het ongeldige citaat; zonder map wordt diezelfde controle geaccepteerd.

De AI-only-grens bestaat werkelijk in de bron. De diagnose toont echter niet overtuigend dat de al gekwalificeerde Facts over een *conceived*, *potentially* AI-only bedrijf op vroege ontwikkelfase een materieel onwaar operationeel beeld geven. De tweede controlediagnose eist een extra validatiedisclaimer zonder dat Research validatie/effectiviteit claimt. Onder de bevroren conservatieve rubric zijn dit twijfelachtige/overstrenge blockers, geen bewezen nieuwe fouten die de correcte controle herlabelen. Ook bij een gunstiger interpretatie blijven targetmiss en parserfout staan.

De oude en nieuwe misses passen bij een beoordelingsprobleem ondanks beschikbaar bewijs. Mogelijke verwarring tussen toegeschreven betekenis en bewezen lezereffect, of te ruime tolerantie voor tegenstrijdige Facts/Unknowns, blijven **hypothesen**. Er is geen bewijs over verborgen aandacht, interne redenering, overbelasting of één causale verklaring.

## 3. Eerlijke ablatietest: identieke prompt en cases

De zes historische bron/Idea/Research-fixtures zijn **bytegelijk aan Experiment 3**, inclusief de aangevulde correcte 005-controle. De Judge-systemprompt is bytegelijk aan Experiment 3 in beide armen. Modellen, outputlimiet, antwoordenschema, originele bron, vijf Idea-velden, Research en werkelijke productie-filteroutput zijn gelijk. Ook zonder map heet het bronveld `originalSource`. Volledige requests per matched paar zijn geverifieerd: **het enige verschil is het aanwezige evidenceMap-veld**. Geen eerdere verdicts, labels of audit meegestuurd.

Alle drie maps worden vooraf gegenereerd; daarna wisselt de vooraf bevroren armvolgorde per case/variant. Elke map wordt ongewijzigd voor bad én correct hergebruikt. Er zijn geen twaalf onafhankelijke bronnen of zes onafhankelijke maps. Eén run per cel, geen seed/repeatabilityproef of onafhankelijke menselijke labeling. Historische casebekendheid blijft een beperking; deze vergelijking bewijst geen transfer naar nieuwe holdouts.

| Historisch document | Zonder map | Met map | Targetgerichte beoordeling |
|---|---|---|---|
| Intro baseline | accept, geldig | reject, geldig, ander issue | Beide missen Unknown 4 |
| Intro correct | accept, geldig | ruw reject, parser weigert | Met-map-controle faalt technisch; extra blockers overstreng/twijfelachtig |
| 005 baseline | reject, geldig | accept, geldig | Zonder map vindt upstreamvariatie én Synthesis-spanning; met map mist beide |
| 005 correct | accept, geldig | accept, geldig | Beide correct |
| 010 baseline | reject, geldig | accept, geldig | Zonder map vindt feitfout ondanks behouden hiërarchie; met map mist dit ondanks P7 |
| 010 correct | accept, geldig | accept, geldig | Beide correct |

Zonder map: **2/3 doelen, 3/3 correcte controles, 6/6 parsergeldig**. Met map: **0/3 doelen, 2/3 correcte controles, 5/6 parsergeldig**. Een ruw reject van een defect document om een ander punt is geen bruikbare targetdetectie. Een technisch geweigerde afwijzing is geen geaccepteerde controle of geslaagd oordeel.

De twee zonder-map-005-diagnoses zijn letterlijk geankerd, materieel en terughoudend over causaliteit; zij veranderen de gerapporteerde slechte uitkomsten niet. De zonder-map-010-diagnose bewaart het onderscheid tussen feitfout en hiërarchie. Haar exacte citaat noemt de behouden hiërarchie; de volledige originele tabelrij ondersteunt daarnaast de gekoppelde Compose-feitfout. Dit is inhoudelijk bruikbaar, al is het losse citaat niet zelfstandig bewijs van elk deel van de diagnose.

Van zes ruwe diagnosecitaten zijn vijf exact en één ongeldig: zonder map 3/3, met map 2/3. Acceptaties leveren geen citaten op en bewijzen daarom niet dat de bronankerprocedure daadwerkelijk goed is toegepast. Syntactisch zijn alle twaalf antwoorden JSON; contract/ankercontrole door de echte productieparser slaagt bij elf.

005 geeft een mogelijke mapper-selectiefout plus Judge-miss: de map mist upstreamvariatie, maar de bron was nog beschikbaar en de map had de Synthesis-spanning wel. 010 geeft een Judge-miss ondanks goede P7. Een aandachtseffect of foutoverdracht vanuit de map is hiermee niet causaal bewezen. Er wordt niets aangepast om deze uitkomsten alsnog gunstig te maken.

## Kwaliteit en kosten

| Nieuwe matched aanpak | Calls | Inputtokens | Outputtokens | Totaal |
|---|---:|---:|---:|---:|
| Zonder map, zes judges | 6 | 37.094 | 7.179 | **44.273** |
| Met map, zes judges alleen | 6 | 59.510 | 10.267 | 69.777 |
| Drie gedeelde maps | 3 | 39.862 | 7.583 | 47.445 |
| Met map inclusief generatie | **9** | **99.372** | **17.850** | **117.222** |
| Hele nieuwe proef | **15** | **136.466** | **25.029** | **161.495** |

De met-map-aanpak kost **72.949 tokens extra (+164,8%)** en **50% meer calls** tegenover zonder map. Judges alleen zijn al 57,6% duurder in tokens. De mapkosten zijn al gedeeld over twee varianten per bron/Idea-paar; er is dus geen dubbele generatie gerekend. Bij één beoordeling zonder hergebruik zijn twee calls nodig. Meer hergebruik amortiseert generatie verder, maar niet de extra mapinput en niet gedeelde mapfouten.

De mapperpayload bevat zowel originalSource als de uitgebreide bronfragmentcatalogus met tekst en offsets. Die transparante maar redundante representatie draagt flink bij aan inputkosten. Deze proef is daarmee geen bewijs dat de kosten minimaal zijn; een compacter ontwerp is hoogstens een hypothese voor apart geautoriseerd onderzoek, geen post-hoc correctie op deze gefaalde begrotingsdrempel. Beide Judge-armen hebben identieke bronpresentatie; de extra catalogus is alleen voor de mapper. Reasoningtokens zitten al in completiontokens en worden niet dubbel geteld. Ruwe cached/cache-write-details staan in usage.json. Geen geldprijs of productie-latentievoordeel bewezen.

Historische context, **uitsluitend hergebruikte cijfers**:

| Proef | Historische doelen | Correcte controles | Parsergeldige historische verdicts | Matched tokens incl. maps |
|---|---:|---:|---:|---:|
| Experiment 2 | 1/3 | 2/3 | 4/6 | 46.859 |
| Experiment 3 | 2/3 | 3/3 | 6/6 | 81.063 |
| Experiment 4 zonder map, nieuw | 2/3 | 3/3 | 6/6 | 44.273 |
| Experiment 4 met map, nieuw | 0/3 | 2/3 | 5/6 | 117.222 |

De eerste twee rijen zijn geen nieuwe replicaties en hebben verschillende prompt-/ankerontwerpen. Alleen de twee Experiment-4-armen vormen de nieuwe matched ablatie. De 2/3 van Experiment 3 tegenover 0/3 nu bewijst geen stabiele regressiekans of uitsluitend een effect van het ankermechanisme: maps, samples en uitvoeringstijd verschillen. Binnen Experiment 4 is de vergelijking beter gecontroleerd, maar klein en stochastisch.

## Volledig nieuw callledger

| Volgorde / call-id | Input | Output | Totaal |
|---|---:|---:|---:|
| 1 intro-map | 15.891 | 2.822 | 18.713 |
| 2 005-map | 10.978 | 2.221 | 13.199 |
| 3 010-map | 12.993 | 2.540 | 15.533 |
| 4 intro-bad-without | 5.531 | 956 | 6.487 |
| 5 intro-bad-with | 9.134 | 1.499 | 10.633 |
| 6 intro-correct-with | 9.146 | 3.375 | 12.521 |
| 7 intro-correct-without | 5.543 | 484 | 6.027 |
| 8 005-bad-without | 6.026 | 2.225 | 8.251 |
| 9 005-bad-with | 9.587 | 1.571 | 11.158 |
| 10 005-correct-with | 9.837 | 1.520 | 11.357 |
| 11 005-correct-without | 6.276 | 1.324 | 7.600 |
| 12 010-bad-without | 6.769 | 1.455 | 8.224 |
| 13 010-bad-with | 10.813 | 980 | 11.793 |
| 14 010-correct-with | 10.993 | 1.322 | 12.315 |
| 15 010-correct-without | 6.949 | 735 | 7.684 |
| **Totaal 15/18 toegestaan** | **136.466** | **25.029** | **161.495** |

## 4. Null-check en offline verificatie

ResearchSemanticJudge.cs controleert nu `issue is null` vóór veldtoegang. Een individuele null-blocker geeft dezelfde technische InvalidOperationException als andere onvolledige diagnoses, in plaats van een NullReferenceException. Het verdict wordt niet als semantische afwijzing of repairadvies doorgegeven.

De nieuwe regressietest voert de echte StoryWorkflow met fake modelclients uit, geeft `{"accepted":false,"blockingIssues":[null]}` terug en controleert: InvalidOperationException met de bestaande diagnosemelding, geen ResearchValidationException, exact één generator- en één Judge-aanroep, en nul Synthesis/Narrative/Compose-aanroepen. De afzonderlijke regressietest slaagt; de volledige oplossing geeft **Application 238 + Infrastructure 25 + Console 37 = 300 geslaagd**, nul mislukt/overgeslagen. Domain heeft geen discoverable tests. De build heeft nul waarschuwingen en fouten.

Alle twaalf Judge-responses zijn offline met de actuele echte productieparser herafgespeeld: identieke parse-uitkomsten en verdicts. Alle bewaarde filterprojecties matchen opnieuw de echte SynthesisResearchFilter. Requestverificatie controleert de vijftien prompts, bron/Idea, Research/map/catalogus waar van toepassing, model en outputlimiet. Zes matched requestparen zijn identiek na uitsluitend verwijderen van evidenceMap. Twaalf historische fixturebestanden zijn bytegelijk aan Experiment 3.

Alle **103 bronbestandshashes** uit protected-before.json zijn na de uitvoering gelijk. Dit snapshot is bewust na de toegestane null-check/test gemaakt; git-diff tegenover begin-HEAD toont alleen die twee productie/testbestanden. Daarnaast is alleen de nieuwe experimentmap toegevoegd. De parserfile kreeg ook een afsluitende newline. Geen externe codewijziging waargenomen. Freeze-integriteit en git diff --check slagen. De bronfreeze is geen claim dat de provider/modelbackend cryptografisch bevroren is.

Artefacten: protocol.md/freeze.json/labels.json/plan.json; fragments/; prompts/; cases/; calls/ met volledige requests/responses/usage in response-raw; map-audit.json; intro-analysis.md; results.json en responses-processed.json; usage.json/cost-summary.json; verification-*.json; build.log/tests.log/preflight.log. Scripts voor voorbereiding, vaste runner en offline replay/audit blijven beschikbaar. Bestaande locks en attemptmarkers verhinderen een onbedoelde live herstart. Reproduceerbaarheid betekent hier controleerbare inputs, code, requests en replay, niet een gegarandeerd identiek toekomstig modelantwoord.

## Besluit tegen vooraf vastgelegde criteria

| Criterium | Uitkomst |
|---|---|
| Alle drie maps exact geankerd | Geslaagd |
| Alle twaalf verdicts parsergeldig | **Mislukt: 11/12** |
| Met map alle drie doelen | **Mislukt: 0/3** |
| Met map alle correcte controles geaccepteerd | **Mislukt: 2/3** |
| Alle dragende diagnoses materieel/brongetrouw | **Niet aangetoond: overstrenge Intro-blockers en ongeldig citaat** |
| Strikte incrementele targetwinst zonder controleverlies | **Mislukt: verlies van twee detecties en één controle** |
| Tokenoverhead maximaal +80% | **Mislukt: +164,8%** |
| Calloverhead maximaal +50% | Geslaagd: +50% |

**NO-GO.** Bewaar de deterministische ankerproef als experimenteel provenance-mechanisme, niet als productiepromotie of bewezen beoordelingsverbetering. De lokale null-check/test is de enige toegestane functionele codefix. Er is geen aanleiding om de Research-generator, filter, workflow of EnableResearchJudge te veranderen. Eventueel vervolg vereist een nieuw protocol en aparte toestemming; geen extra live calls in deze run, geen herlabeling om succes te claimen en geen commit/push zonder expliciete toestemming.
