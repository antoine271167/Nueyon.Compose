# Experiment 6 — productiegeschiktheid compacte Research

12 november 2026. **NO-GO voor productiepromotie van B. Geen prompts, workflow, filter, configuratie of EnableResearchJudge gewijzigd. Geen commit of push.**

B bespaart **69,2% Research-tokens**, maar slechts **27,1% over de volledige keten**. De downstreamkosten blijven vrijwel gelijk. De eerdere 6/6 uit Experiment 5 houdt in deze strengere proef niet stand. Het duidelijkste herhaalde A/B-verschil: beide compacte ketens verliezen de concrete audio-toegankelijkheidsfouten uit het busdossier vóór de eindpost; beide productievarianten behouden die bevinding. Beide armen missen bij de archiefbron al in Research een aparte toegankelijkheidsdimensie.

De geblindeerde AI-beoordeling geeft na toepassing van de vooraf bevroren stijlgrens voorlopig **A 1/6 en B 0/6 bruikbare eindpublicaties**. Die absolute cijfers zijn onzeker: de beoordelaar maakt aantoonbare citatie- en materialiteitsfouten. Een conservatieve audit geeft **beide 0/6**. De NO-GO voor B blijft overeind zonder de betwistbare diagnoses. Dit experiment bewijst evenmin dat A betrouwbaar of productiegeschikt is.

## Actuele uitgangssituatie en eerdere experimenten

Lokale repo `C:\Workspaces\Nueyon.Compose`, origin `antoine271167/Nueyon.Compose`. Beginstatus schoon; lokale HEAD en met `git ls-remote origin HEAD` gecontroleerde remote HEAD waren beide `b016cb01d600453db4a1d6df0680f6844469ad50`. Gerefereerde chat gelezen; actuele productieagents, handoffs, promptloader, Research-parser, filter, hostmodel en eerdere rapporten/artefacten geïnspecteerd. Lokale opgeslagen bronnen en responses dragen de conclusies, niet de eerdere chatbeoordeling.

Historische context, niet opnieuw gemeten: baseline vond Research-lacunes bij Intro/005/010. Experiment 1 vond met Judge 0/3 bekende doelen en voerde de compacte generator niet uit. Experiment 2 vond 1/3 met bronankerproblemen; Experiment 3 vond 2/3 met mapper-/Judge-problemen; Experiment 4 vond zonder map 2/3 versus met map 0/3. Experiment 5 vergeleek generatie: A 4/6, sensitiviteit 5/6; B 6/6, 74,6% minder Research-tokens. Daar ontbraken herhalingen, lange nieuwe bronnen en gerealiseerde downstreamcontent; dezelfde assistent schreef en beoordeelde ongeblindeerd de nieuwe korte bronnen. Dat beperkte bewijs was aanleiding voor deze vervolgproef, geen uitgangslabel dat B moest bevestigen.

## Vooraf bevroren ontwerp

`protocol.md`, `rubric.md`, `labels.json`, `plan.json`, cases, complete promptsets, runner, build-/testlogs en runtime-DLLs zijn vóór de eerste livecall vastgelegd. `freeze.json` bevat **1.060 SHA256-hashes**, inclusief **977 bestaande gevolgde bestanden**. Geen wijziging van die bestanden tijdens de run vastgesteld. De blinde beoordelingspakketten kregen na generatie een afzonderlijk hashmanifest; ook die hashes zijn intact. Offline analyse- en rapportagescripts zijn deels na de oorspronkelijke freeze toegevoegd en worden niet als vooraf bevroren analysecode voorgesteld.

| Bron | Woorden | Status / Selected Idea |
|---|---:|---|
| 005 | 1.741 | Historische bron en alle vijf Idea-velden byte-inhoudelijk hergebruikt uit Experiment 5: generieke editorial-regel beschermde hiërarchie niet |
| HarborLink | 1.243 | Nieuw synthetisch operationeel busdossier: service, meetdefinities, toewijzing, kosten, toegankelijkheid, winterbesluit |
| City Memory | 1.374 | Nieuw synthetisch archiefdossier: ordening, feitelijke juistheid, rechten, toegankelijkheid, sampling, timing en logdefinities |

De nieuwe bronnen zijn veel langer en complexer dan de nieuwe korte Experiment-5-cases. Het zijn **realistische geconstrueerde dossiers, geen echte veldrapporten**. Hun foutfamilies zijn verwant aan eerdere tests. Auteur en labelmaker zijn dezelfde assistent; onafhankelijke menselijke referentielabels ontbreken. Dit is geen brede onafhankelijke generalisatievalidatie op natuurlijke productiedata.

Twee herhalingen per bron/variant, vooraf afwisselend A/B uitgevoerd: 3 × 2 × 2 × 4 agents = **48 ketencalls**. Daarna zes aparte geblindeerde evaluatiecalls, elk op één matched paar. Hard maximum **54 HTTP-attempts**, nul retries of repairs, maximaal 6.000 outputtokens en 180.000 UTF-8-requestbytes per call. Volledig downstreambudget vooraf gereserveerd; geen case, herhaling of kwaliteitscriterium na uitkomsten geschrapt. Alle 54 calls gebruikt. Verwachting was 350.000–700.000 totale tokens; werkelijk 473.223. Dit was een hard callbudget, geen niet-verifieerbaar geldplafond.

A is exact actuele productie. B is bytegelijk aan de twee compacte Research-prompts uit Experiment 5. Geen Judge of Evidence Mapper in de generatieketen; de post-hoc beoordelaar genereert geen feedback, reparatie of toegangsgate voor een agent. Alle calls gebruiken `gpt-6-luna`, conform hostconfiguratie. SDK RetryPolicy(0), exclusieve locks/attemptmarkers en één HTTP-request per poging. Geen Copilot CLI.

De runner roept de echte ResearchAgent, SynthesizerAgent, NarrativeAgent en ComposeAgent aan, met de echte SynthesisResearchFilter en dezelfde inputtypen/handoffs als StoryWorkflow. Originele bron en alle vijf Idea-velden zijn per matched paar identiek. Alleen Research-prompt varieert; downstreamagents gebruiken dezelfde prompts en instellingen maar krijgen de doorgegeven variabele teksten. Daardoor is een eindverschil een resultaat van de keten; de precieze interne oorzaak is hiermee niet geïsoleerd.

Geen live Idea-generatie of Idea Judge: de vaste Selected Idea is de experimentele controle. Dit test het echte pad **vanaf een bevroren Idea**, niet de betrouwbaarheid van Idea-selectie of de volledige workfloworkestratie. Geen kunstmatige vervangende Synthesis/Narrative/Compose.

## Formaatgrens en bestaande promptproblemen

Alleen ComposeInput krijgt `LinkedInPost`; geen gekozen outputformaat toegevoegd aan Research, Synthesis of Narrative. Hun prompts zijn niet gewijzigd. De bestaande Narrative-prompt bevat echter al de Article/LinkedIn-compatibiliteitszin én een hardcoded verhaal over de ontwikkeling naar orchestration. De Synthesis-prompt bevat eveneens Nuëyon-specifieke richtlijnen. De volledige bestaande pre-Compose-promptinhoud is dus **niet strikt format- en onderwerpagnostisch**. Dat kan niet eerlijk als voldaan worden gemeld zonder de verboden productiepromptwijzigingen.

De vooraf gevonden Narrative-instructie veroorzaakte niet in alle outputs een orchestration-verhaal: de eindposts blijven grotendeels bij hun vaste Idea. Wel verschijnt in Archive B r1 Synthesis de ongefundeerde instructie `beschrijf Nuëyons productrichting`. De bestaande Synthesis-regel over Nuëyons productrichting is een plausibele bron, geen hier bewezen causale verklaring. Deze contaminatie is een gedeeld productierisico en geen bewijs dat B alleen het veroorzaakt. Een tweede outputformaat is niet getest.

## Rubric, blindering en grenzen van onafhankelijkheid

Vooraf vastgelegd: brontrouw, essentiële dekking, epistemische consistentie, projectie-integriteit, downstreambehoud, hook, leesbaarheid, engagement en eindbrontrouw, elk 0–2. Per referentielabel wordt de aanwezigheid in Research, projectie, Synthesis, Narrative en Compose vastgelegd. Equivalent bewijs telt; gedeeltelijke dekking kan volstaan zolang geen materiële betekenis verdwijnt. Niet elk brongegeven hoeft in een LinkedIn-post, maar de vooraf als essentieel gemarkeerde grenzen wel.

Een bruikbare post vereist eindbrontrouw 2/2, hook/leesbaarheid/engagement elk ≥1 en samen ≥5/6, vereiste eindlabels minstens gedeeltelijk aanwezig, en geen materiële fout of misleidende omissie. GO vereist alle zes B-Researchresultaten én eindpublicaties geslaagd, herhaalbaarheid per bron, geen nieuwe materiële paired regressie, geen slechtere gemiddelde stijl, nul technische fouten en minstens 20% besparing op totale en gewogen tokens per bruikbaar eindresultaat. Eén falende poort betekent NO-GO. Geen statistische non-inferioriteitsclaim uit drie bronnen.

Alle twaalf ketens zijn zonder kwaliteitsselectie geëxporteerd naar willekeurige neutrale codes en presentatievolgorde. Zes aparte AI-sessies ontvingen originele bron, vaste Idea, kwaliteitsrubric, labels en alle stages. Zij ontvingen geen A/B-sleutel, kosten, bestandsnamen of promotiehypothese. De variantgebonden GO-paragraaf zat niet in hun rubric. De sleutel bleef apart; scores zijn opgeslagen vóór ontsleuteling. De blinde export normaliseert regeleinden in de projectietekst; de oorspronkelijke projecties en raw responses blijven behouden.

Dit is **geen menselijke of volledig onafhankelijke beoordeling**: hetzelfde modeltype beoordeelt en genereert, bron/labels komen van dezelfde assistent, stijl en lengte kunnen de variant verraden, en de uitvoerende assistent kent identiteit bij de aanvullende audit. Blindering vermindert een vorm van bias maar lost deze afhankelijkheden niet op.

## Kwaliteit en herhaalbaarheid

Primaire cijfers: ruwe geblindeerde beoordeling plus vaste stijl-/coveragechecks, zonder achteraf versoepelen.

| Case / arm | Research-pass /2 | Bruikbare eindposts /2 | Stijl r1/r2, /6 |
|---|---:|---:|---|
| 005 A | 1 | 0 | 4 / 4 |
| 005 B | 1 | 0 | 4 / 4 |
| Harbor A | 1 | 1 | 5 / 5 |
| Harbor B | 0 | 0 | 4 / 5 |
| Archive A | 0 | 0 | 4 / 4 |
| Archive B | 0 | 0 | 6 / 5 |
| **A totaal** | **2/6** | **1/6 voorlopig** | **gemiddeld 4,33** |
| **B totaal** | **1/6** | **0/6** | **gemiddeld 4,67** |

**005:** beide eerste Research-runs missen concrete Idea/Research-runvariatie. Beide tweede runs bewaren die wél, maar beide Synthesis-resultaten laten haar daarna weg. Bij B r1 verdwijnen ook de lagere scores uit het eindverhaal; B r2 bewaart ze. Het falen is dus niet volledig stabiel en niet uitsluitend B-gerelateerd. De r2-posts zijn brongetrouw en leesbaar, maar halen de vooraf vereiste stijlsom niet. Algemene statistische voorzichtigheid is geen vervanging voor de concrete upstreamvariatie.

**Harbor:** A bewaart de belangrijkste service-/meet-/kosten-/besluitgrenzen. B bewaart ook veel hiervan en is vaak zorgvuldig. B r1 mist echter vrijwillige feedback en toegeschreven cleaner-transport-betekenis al in Research; B r2 heeft die wél. Beide B-eindposts verliezen de audiofouten en vermelden slechts drie inspecties. De bevinding is nog aanwezig in Research, echte projectie en Synthesis: het verlies ontstaat later. Beide A-posts noemen de twee audiofouten, maar laten de latere reparatie weg. Dat laatste is een gedeelde tekortkoming die de AI inconsistent weegt.

**Archive:** alle vier Research-runs missen keyboard-toegankelijkheid, naast de vooraf vereiste productiviteits-/logcontext. De kerntegenstelling Cedar correct geordend/naamfout en Elm juiste namen/jaarprobleem blijft meestal wel bewaard. B-posts lezen vlotter en blijven expliciet synthetisch, maar missen de keyboarddimensie. Hun formulering dat 760 scans zijn vrijgegeven is mogelijk sterker dan rechtenvrijgave; deze diagnose is taalkundig betwistbaar en niet doorslaggevend gemaakt.

Gemiddelde AI-leesbaarheid is 2/2 in beide armen; hook A 1,33 versus B 1,50; engagement A 1,00 versus B 1,17; eindbrontrouw A 1,33 versus B 1,17. B is dus niet simpelweg slechter geschreven: iets aantrekkelijker proza kan samengaan met verloren betekenis. Volledige scores en stage-by-stage labeltabellen staan in `analysis-tables.md` en `assessments-unblinded.json`.

De echte filter werkt technisch correct: beschikbare Facts/Unknowns worden reproduceerbaar doorgegeven. Sommige dekking ontbreekt al vóór filtering; andere informatie verdwijnt pas bij Synthesis of Narrative. Geen filterbug vastgesteld. Alleen vergelijkbare eindfouten aanwijzen als bewezen Research-promptcausaliteit zou te sterk zijn.

## Kritiek op de beoordeling zelf

Alle twaalf scorecontracten zijn geldig, maar van **33 diagnose-sourceQuotes zijn slechts 22 letterlijk**; 17 diagnoses hebben zowel een letterlijke sourceQuote als een matching outputQuote, waarbij outputmatching nog geen semantisch bewijs is. Vijf outputQuotes zijn placeholders. De AI combineert soms broncitaten, verandert hoofdletters of voegt een niet-bestaande bronzin toe. Geen daarvan is stilzwijgend gerepareerd of als geldig bronanker geteld.

De AI noemt Harbor r2 Research onvoldoende wegens omissies die pas in Synthesis optreden. Alleen labelbeschikbaarheid in Research/projectie geeft A **3/6**, B **2/6**. Dat is een lokalisatiediagnose, geen nieuwe promotiepoort of ongezien betere primaire score. Ook noemt de AI 005 r2 A/B bruikbaar ondanks stijlsom 4/6; de vaste afleiding telt ze terecht niet als geslaagd.

Harbor A r1 is de enige primaire eindpass, maar mist fictieve status en de latere audioreparatie. Vergelijkbare omissies worden elders materieel genoemd. Conservatief is daarom ook deze pass niet bevestigd: **A 0/6, B 0/6**. Er is bovendien een fout in de vooraf bevroren Archive-label: `760 openbare scans` is sterker dan de bron over afgeronde rechtenvrijgave. De bron blijft leidend; label en ruwe beoordeling blijven ongewijzigd opgeslagen. Productiviteit en logs kunnen voor de specifieke Archive-Idea te breed verplicht zijn. Zonder die eisen blijft het herhaalde verlies van de centrale keyboarddimensie bestaan.

`critical-audit.md` en `audit-summary.json` scheiden primaire scores, rekenregels, rechtstreeks gecontroleerde bevindingen en interpretatiegevoeligheid. De audit is ongeblindeerd en vervangt geen onafhankelijke adjudicatie. Onder alle besproken lezingen mist B meerdere vooraf vereiste poorten; een robuust NO-GO is mogelijk zonder betrouwbare absolute succespercentages te suggereren.

## Werkelijk tokenverbruik en kosten

| Arm / stage | Calls | Input | Output | Totaal |
|---|---:|---:|---:|---:|
| A Research | 6 | 69.964 | 15.628 | 85.592 |
| A Synthesis | 6 | 34.831 | 8.697 | 43.528 |
| A Narrative | 6 | 45.465 | 4.683 | 50.148 |
| A Compose | 6 | 36.163 | 4.880 | 41.043 |
| **A keten** | **24** | **186.423** | **33.888** | **220.311** |
| B Research | 6 | 15.684 | 10.714 | 26.398 |
| B Synthesis | 6 | 32.648 | 10.744 | 43.392 |
| B Narrative | 6 | 45.544 | 4.287 | 49.831 |
| B Compose | 6 | 36.203 | 4.766 | 40.969 |
| **B keten** | **24** | **130.079** | **30.511** | **160.590** |
| Afzonderlijke evaluatie | 6 | 64.357 | 27.965 | 92.322 |
| **Gehele experiment** | **54** | **380.859** | **92.364** | **473.223** |

Researchbesparing B: **69,2%**. Downstream A 134.719 versus B 134.192 tokens: slechts **0,4%** minder. Ketenbesparing **59.721 tokens / 27,1%**. Onder vooraf vastgelegde sensitiviteit I+4O is A 321.975 versus B 252.123, **21,7%** minder. Dat is een weging, geen geldprijs.

| Primaire kostmaat per bruikbare eindpublicatie | A | B |
|---|---:|---:|
| Voorlopig bruikbare posts | 1 | 0 |
| Alle ketentokens / bruikbaar | 220.311 | Ongedefinieerd / oneindig |
| I+4O / bruikbaar | 321.975 | Ongedefinieerd / oneindig |

Alle mislukte uitkomsten tellen in de teller. Bij conservatieve audit zijn beide noemers nul; dan is voor geen van beide een eindige kostprijs per bruikbare publicatie aangetoond. De gemiddelde tokens per geproduceerde post, A 36.719 versus B 26.765, mogen niet als kosten per **bruikbare** post worden gepresenteerd. Evaluatiekosten zijn onderzoeksoverhead, geen normale B-productiekosten; inclusief onderzoek is 473.223 tokens uitgegeven voor nul of hoogstens één voorlopig bruikbaar resultaat. Geen gedeelde live Idea-kosten om toe te rekenen; die stage is niet uitgevoerd.

Herhalingen: totale ketentokens per case A 36.866/35.697 (005), 38.191/38.380 (Harbor), 34.943/36.234 (Archive); B 25.663/26.618, 27.040/29.026, 25.762/26.481. De besparing verschijnt in alle zes paren, maar kwaliteit varieert en faalt soms stabiel. Twee repeats geven geen betrouwbare variantieschatting, foutkans of interval; cases en hun paren zijn niet twaalf onafhankelijke bronnen.

Ruwe cachingvelden blijven in `usage.json`: A 90.116 cached en 96.235 cache-write; B 55.857 cached en 74.150 cache-write; evaluatie 0 cached en 64.339 cache-write. Reasoning **50.050**, al inbegrepen in output, niet dubbel geteld. Cache-write is evenmin extra bij input opgeteld. Een verifieerbaar Luna-factuurtarief ontbreekt. Geldkosten zijn conditioneel `(I-cached)*p_input + cached*p_cached + O*p_output`, gedeeld door één miljoen, plus uitsluitend aantoonbare aanvullende providerheffingen. Zonder tarief en bewezen bruikbaarheid is geen eurobusinesscase vastgesteld. Zelfs gratis cached input maakt B's lagere tokenkosten nog geen lagere kosten per geslaagde publicatie.

## Offline controle en integriteit

- Solutionbuild geslaagd, nul fouten/waarschuwingen; **300 tests geslaagd** (238 Application, 25 Infrastructure, 37 Console), nul mislukt/overgeslagen. Domain heeft geen discoverable tests. Geen live modelcalls tijdens tests.
- Runnerbuild geslaagd. Offline replay verwerkt alle **48 ketenantwoorden** opnieuw met echte productieagents/parsers en vergelijkt resultaten exact; alle twaalf Research-projecties opnieuw met de echte filter gelijk. De zes evaluatieantwoorden zijn offline JSON-/scorecontract-gecontroleerd. Samen 54 responses verantwoord.
- Alle **54 HTTP 200**, model `gpt-6-luna`, finish_reason `stop`, één request per poging. Nul parser-, heading-, structuur-, transport- of truncatiefouten. Geen technische failurecase vervangt een inhoudelijk mislukte cel.
- `verification-requests.json`: alle 54 requests gecontroleerd; systeemtekst, productie-rendering, bron/Idea, schema-/outputlimiet, echte handoffs en LinkedIn-format correct. Geen false checks. Evaluatierequests matchen de neutrale pakketten en bevatten geen identiteitssleutel.
- Alle 1.060 freezehashes en 977 beschermde bestanden intact; B matcht Experiment 5 en A productie bytegelijk. Git toont uitsluitend de nieuwe experimentmap. Geen productie- of configuratie-edit, geen EnableResearchJudge-activering, geen commit of push.

## Besluit en concrete vervolgrenzen

**NO-GO voor migratie/promotie van B.** De tokenbesparing is herhaald aantoonbaar, maar 6/6 kwalitatief bruikbare ketens, stabiele dekking en afwezigheid van paired regressies zijn niet gehaald. De kwaliteitsbeoordeling zelf verdient eveneens verbetering. A blijft het bestaande gedrag, niet een door deze proef goedgekeurde kwaliteitsstandaard.

Geen verdere tuning op deze cases binnen deze run. Bewaar locks, bronlabels, ruwe evaluaties en alle missers. Een volgende afzonderlijk geautoriseerde proef moet onafhankelijke referentielabels op natuurlijke, langere productiebronnen gebruiken, met menselijke blinde controle op materialiteit en niet alleen AI-citaten. Pak bestaande Synthesis/Narrative-onderwerp- en formaatgrenzen apart aan als dat wordt opgedragen; vermeng die wijziging niet met een nieuwe Research-A/B-claim. Definieer een scherpere grens tussen relevante Research-dekking, verplichte Synthesis-beperkingen en selectieve eindcontent.

**Voorwaardelijke migratie als B in een toekomstige nieuwe proef wél slaagt:** (1) vooraf bevroren bronlabels onafhankelijk goedgekeurd, alle kwaliteits-/repeatabilitypoorten gehaald; (2) tarief en kosten per echt bruikbare publicatie bevestigd; (3) aparte expliciete toestemming; (4) uitsluitend de twee Research-promptbestanden vervangen, overige prompts/filters/workflow/configuratie ongewijzigd; (5) vóór omschakelen A- en B-hashes vastleggen en offline build/tests/replay uitvoeren; (6) beperkte gecontroleerde uitrol met vooraf bepaalde menselijke broncontrole, geen nieuwe Judge activeren.

**Rollbackvoorwaarden:** direct terug naar exact de opgeslagen A-bytes bij een materiële bronfout, essentiële informatieomissie, herhaald scope-/zekerheidsverlies, parser/headingfout of overschrijding van de afgesproken kosten per bruikbaar resultaat. Herstel beide Research-bestanden samen, rebuild indien embedded resources worden gebruikt, controleer werkelijk verstuurde promptidentiteit, voer offline tests en vaste smokechecks uit. Geen wijziging aan EnableResearchJudge, geen automatische repair of selectief opnieuw draaien. De A/B-SHA256-waarden staan in `verification-prompt-identity.json`. Deze voorwaarden zijn een voorstel voor een latere geautoriseerde migratie, geen uitgevoerde activering.

## Artefacten

`protocol.md`, `rubric.md`, `labels.json`, `plan.json`, `freeze.json`, `protected-before.json`, `head.txt`; `cases/`, `prompts/`, `runner/`; `calls/` met exacte requests, responses/usage, inputs, metadata, geparseerde outputs, leesbare stages en projecties; `blind/` met apart gehouden sleutel en pakkethashes; `usage.json`, `cost-summary.json`, `results.json`, `assessments-unblinded.json`, `analysis-tables.md`, `critical-audit.md`, `audit-summary.json`; `verification-*.json`, build-/test-/run-/evaluatie-/replaylogs; `REPLAY.md` en `artifact-manifest.json`. Sleutels/authheaders zijn niet gelogd. Geen bestaande experimentmap gewijzigd. **Deze run niet opnieuw uitvoeren of de markers resetten.**
