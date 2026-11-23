# Work-opdracht — Nuëyon.Compose Experiment 13

## Opdracht

**Start een ChatGPT Work-sessie en voer Experiment 13 — Narrative Fidelity & Editorial Resolution uit.**

Gebruik bij voorkeur **GPT-6 luna**.

Werk zelfstandig, van voorbereiding tot evaluatie en vastlegging van de resultaten in GitHub. Vraag alleen om aanvullende informatie wanneer uitvoering daadwerkelijk geblokkeerd is.

## 1. Projectcontext

We ontwikkelen **Nuëyon.Compose**, een AI-gestuurde contentpipeline:

**Idea → Research → Synthesis → Narrative → Compose**

We optimaliseren de agents stapsgewijs, één agent tegelijk.

- Idea, Research en Synthesis zijn voorlopig afgerond.
- Narrative is de huidige focus.
- Compose wordt pas geoptimaliseerd wanneer Narrative voldoende gevalideerd is.
- Alle agents vóór Compose moeten volledig format-agnostisch blijven.
- Alleen Compose is verantwoordelijk voor de uiteindelijke publicatievorm, bijvoorbeeld LinkedIn of Article.

Kwaliteit betekent niet uitsluitend feitelijke betrouwbaarheid. Het uiteindelijke verhaal moet ook interessant, aantrekkelijk en prettig leesbaar zijn, de aandacht vasthouden en een betekenisvolle conclusie hebben.

Narrative moet daarvoor een sterke redactionele basis leveren.

## 2. Repository en omgeving

GitHub-repository: `antoine271167/Nueyon.Compose`

Branch: `main`

Lokale projectmap (indien beschikbaar): `C:\Workspaces\Nueyon.Compose`

Broncode: `src/`

Experimenten: `PromptExperiments/`

Referentiecommit Experiment 12:

`25ccb227354abb83f41430b12b52de5d8542fe25`

Gebruik de actuele `main` als uitgangspunt, maar controleer eerst of deze sinds de referentiecommit is gewijzigd. Documenteer eventuele relevante verschillen.

Gebruik de bestaande architectuur, experimentinfrastructuur en CLI-functionaliteit voor single-agent runs.

Introduceer geen nieuwe agents, evaluatieframeworks of onnodige infrastructuur.

## 3. Aanleiding: Experiment 12

Lees eerst het volledige rapport:

[Experiment 12 — Narrative Quality Diagnosis](page://page_b17bec42393481918e09d3d12d7fb7e2#eea39585-d6f0-4878-9814-c865dd5c7cb6)

Experiment 12 resulteerde in een onderbouwde **NO-GO** voor het bevriezen van Narrative.

### Sterke punten

De productiebaseline behaalde in Experiment 3, arm A:

| Criterium | Resultaat |
|---|---|
| Verhaallijn voldoende | 10/12 |
| Redactionele focus voldoende | 11/12 |
| Concrete details voldoende | 12/12 |
| Downstream handoff voldoende | 10/12 |
| Volledig brongetrouw | 6/12 |
| Volledig voldoende | 4/12 |

Narrative levert dus doorgaans een bruikbare verhaalrichting en structuur.

Een complete herontwerp van Narrative is niet gewenst.

### Vastgestelde problemen

**A. Semantic fidelity**

Narrative introduceert soms feitelijke of betekenisveranderende afwijkingen.

Concrete voorbeelden:

- Wolf: `elk` (wapiti) wordt verkeerd vertaald als eland.
- Wolf: karkassen van gedode prooidieren worden beschreven als karkassen van wolven.
- Webb: deels overlappende activiteiten worden als strikt opeenvolgende fasen gepresenteerd.
- Lowell: gelijktijdige economische ontwikkelingen worden tot oorzakelijke relaties gemaakt.
- Digital: een bepaalde werkwijze wordt zonder voldoende bewijs als oorzaak van een resultaat gepresenteerd.

Deze fouten ontstaan aantoonbaar al in Narrative.

**B. Editorial resolution**

Narrative ontwikkelt doorgaans een logische verhaallijn, maar de inhoudelijke conclusie is regelmatig onvoldoende.

De afsluiting:

- herhaalt de centrale boodschap;
- herhaalt de gebruikte aanpak;
- benut niet de belangrijkste concrete waarneming;
- of ontbreekt als betekenisvolle afronding.

Narrative moet niet de uiteindelijke LinkedIn-afsluiting schrijven, maar moet wel een duidelijke, brongebonden inhoudelijke conclusie aan Compose kunnen overdragen.

### Problemen buiten Narrative

Experiment 12 toonde ook aan dat Compose zelfstandig nieuwe afwijkingen introduceert.

Voorbeelden zijn een onjuiste tijdsinterpretatie bij Webb en een vernauwing van `ungulate management` tot `elk management`.

Deze problemen vallen buiten Experiment 13.

Wijzig Compose niet.

## 4. Doel van Experiment 13

Onderzoek of één kleine, gerichte verbetering van de Narrative-prompt de twee vastgestelde problemen kan oplossen:

1. **Semantic Fidelity:** behoud van de exacte feitelijke betekenis van de input.
2. **Editorial Resolution:** een inhoudelijke conclusie die de verhaalrichting afrondt met brongebonden bewijs.

Dit moet gebeuren zonder regressie op:

- Redactionele focus.
- Logische verhaalstructuur.
- Spanningsboog en verhaalontwikkeling.
- Concrete details.
- Bruikbaarheid voor downstream Compose.
- Format-agnosticiteit.

**Belangrijk:** optimaliseer niet uitsluitend voor veiligheid. Een feitelijk correcte maar saaie, generieke of betekenisloze Narrative is geen succesvol resultaat.

## 5. Experimentele aanpak

### Fase A — Analyse

1. Lees de actuele Narrative-systemprompt en userprompt.
2. Bestudeer de relevante resultaten van Experiment 1, 3, 11, 11b en 12.
3. Onderzoek de bestaande testcases, inclusief Selected Idea, Synthesis en brontekst.
4. Identificeer welke instructies ontbreken of onvoldoende scherp zijn.
5. Documenteer een concrete hypothese voor de verbetering.

Maak onderscheid tussen promptproblemen, modelvariatie en eventuele fouten in de upstream input.

### Fase B — Ontwerp één promptvariant

Maak precies één nieuwe kandidaatvariant naast de bestaande productiebaseline.

Beperk wijzigingen zoveel mogelijk tot de instructies die de centrale boodschap, feitelijke betekenis en inhoudelijke conclusie bepalen.

De variant moet:

- Soortnamen, entiteiten, actoren en objecten correct behouden.
- Tijdsoverlap en chronologie respecteren.
- Geen causaal verband introduceren wanneer alleen samenloop is aangetoond.
- Geen bredere of smallere claims maken dan de bron ondersteunt.
- Onzekerheden en belangrijke beperkingen behouden.
- Een betekenisvolle inhoudelijke conclusie ontwikkelen die volgt uit concreet bronbewijs.
- De centrale verhaalrichting en redactionele spanning behouden.

Vermijd overmatige waarschuwingen, herhalingen en defensieve formuleringen.

Houd de prompt compact en begrijpelijk.

Verander geen contracts, schemas of agentarchitectuur tenzij een technische blokkade dit strikt noodzakelijk maakt. Documenteer een dergelijke blokkade eerst.

### Fase C — Testontwerp

Gebruik de vier bestaande regressieonderwerpen:

- Digital
- Webb
- Wolf
- Lowell

Behoud de oorspronkelijke upstream inputs zodat de vergelijking eerlijk blijft.

Voeg daarnaast ten minste twee nieuwe, nog niet voor promptoptimalisatie gebruikte onderwerpen toe, bij voorkeur uit bestaande beschikbare bronmaterialen. Kies onderwerpen die andere semantische risico's introduceren.

Leg vóór de modelruns vast:

- De testcases en broninputs.
- De claims die behouden moeten blijven.
- Niet-onderbouwde claims die verboden zijn.
- De rubric en acceptatiecriteria.
- Het model en de modelinstellingen.
- Het aantal herhalingen.
- De exacte productie- en kandidaatprompts.

Gebruik waar mogelijk minimaal drie herhalingen per onderwerp en per variant, om incidentele modelvariatie zichtbaar te maken.

Voer geen Compose-runs uit.

### Fase D — Uitvoering

Gebruik de bestaande Composer single-agent mode om alleen Narrative uit te voeren.

Vergelijk:

**Variant A:** bestaande productieprompt.

**Variant B:** gerichte verbetering uit Experiment 13.

Gebruik voor beide varianten exact dezelfde input, modelconfiguratie en testinstellingen.

Sla alle ruwe inputs, outputs en relevante metadata op.

Beoordeel de Narrative-outputs bij voorkeur blind voor de variantidentiteit.

Maak geen handmatige inhoudelijke correcties op gegenereerde outputs voordat deze beoordeeld worden.

### Fase E — Evaluatie

Beoordeel elke Narrative-brief op:

| Criterium | Vraag |
|---|---|
| Semantic fidelity | Zijn feiten, actoren, objecten, relaties, chronologie en nuances behouden? |
| Redactionele focus | Blijft de centrale verhaalrichting scherp? |
| Verhaalstructuur | Ontwikkelt het verhaal zich logisch en interessant? |
| Spanningsboog | Is er een betekenisvolle ontwikkeling of tegenstelling? |
| Editorial resolution | Wordt de centrale verhaalrichting inhoudelijk afgerond met bronbewijs? |
| Concrete details | Worden specifieke, relevante brongegevens benut? |
| Downstream handoff | Kan Compose hiermee een overtuigend verhaal schrijven zonder ontbrekende betekenis te moeten verzinnen? |
| Format-agnosticiteit | Is de brief bruikbaar voor verschillende publicatieformaten? |

Geef bij iedere onvoldoende beoordeling een concreet citaat uit de Narrative-output en de relevante bronpassage.

Maak onderscheid tussen materiële fouten, kleine onnauwkeurigheden en stilistische voorkeuren.

Onderzoek expliciet of de kandidaatvariant fouten oplost ten koste van de redactionele kwaliteit.

## 6. GO/NO-GO-criteria

Leg de definitieve beslisregels vast vóór de eerste modelrun.

Gebruik in beginsel de volgende drempels.

### GO

- Nul materiële bronfouten op de regressieset en ongeziene cases.
- Minimaal 10/12 volledig voldoende op de vier vaste cases.
- Minimaal 2/3 volledig voldoende per vast onderwerp.
- Een duidelijke, brongebonden inhoudelijke conclusie.
- Geen relevante achteruitgang in focus, spanningsboog, structuur of downstream bruikbaarheid.
- Geen format-specifieke instructies in Narrative.

De nieuwe variant moet aantoonbaar beter zijn dan productie.

### NO-GO

Een NO-GO is noodzakelijk wanneer de kandidaatvariant:

- Materiële bronfouten blijft introduceren.
- De redactionele kwaliteit aantoonbaar verslechtert.
- Geen betekenisvolle verbetering laat zien.
- De vooraf vastgelegde acceptatiecriteria niet haalt.

Vermeld eventuele beperkingen door steekproefomvang, beoordelaarsverschillen en modelvariatie.

Pas de acceptatiedrempels niet achteraf aan om een GO te verkrijgen.

## 7. Promotie en repository

Bewaar het experiment onder een nieuwe, duidelijk benoemde directory binnen:

`PromptExperiments/Narrative/`

Leg minimaal vast:

- Hypothese.
- Testplan en vooraf vastgelegde GO/NO-GO-regels.
- Baseline- en kandidaatprompts.
- Testcase-inputs.
- Ruwe outputs.
- Beoordelingsresultaten.
- Concrete foutanalyses.
- Vergelijking met eerdere experimenten.
- Eindconclusie en aanbeveling.

Bij **GO**:

1. Promoveer uitsluitend de goedgekeurde Narrative-prompt naar productie.
2. Controleer dat de overige agents ongewijzigd blijven.
3. Voer de relevante build en tests uit.
4. Commit en push de goedgekeurde wijzigingen en experimentresultaten naar GitHub.
5. Leg de exacte productiecommit vast.
6. Adviseer om daarna Compose als volgende agent te optimaliseren.

Bij **NO-GO**:

1. Laat de productieprompt ongewijzigd.
2. Bewaar de experimentresultaten en bevindingen.
3. Commit en push uitsluitend de experimentdocumentatie en bijbehorende artifacts.
4. Formuleer één duidelijke hypothese voor een eventueel volgend Narrative-experiment.

Werk in kleine, controleerbare stappen en houd het project compileerbaar.

## 8. Autonomie en voortgang

Voer het experiment autonoom uit zonder tussentijdse bevestiging voor normale onderzoeks- en teststappen.

Geef zichtbaar voortgang bij belangrijke mijlpalen:

1. Analyse afgerond en hypothese bepaald.
2. Kandidaatprompt en testplan gereed.
3. Modelruns uitgevoerd.
4. Evaluatie afgerond.
5. GO/NO-GO vastgesteld.
6. Resultaten opgeslagen in GitHub.

Blijf binnen de scope van Narrative.

Gebruik geen GitHub Copilot CLI wanneer dat niet nodig is. Gebruik de beschikbare Work-capaciteiten en bestaande experimentinfrastructuur.

## 9. Gewenste eindrapportage

Lever na afloop een beknopt maar onderbouwd eindrapport met:

**Experiment 13 — Narrative Fidelity & Editorial Resolution**

- Definitieve beslissing: **GO of NO-GO**.
- Samenvatting van de wijzigingen.
- Resultatentabel per onderwerp en variant.
- Vergelijking met productiebaseline.
- Analyse van semantic fidelity.
- Analyse van editorial resolution.
- Eventuele regressies.
- Concrete voorbeelden van verbeteringen en resterende fouten.
- Build- en teststatus.
- GitHub-commit(s) en locaties van experimentresultaten.
- Aanbevolen volgende stap.

## 10. Belangrijkste uitgangspunt

We optimaliseren Nuëyon.Compose niet om uitsluitend formeel correcte outputs te produceren.

We willen een pipeline ontwikkelen die **feitelijk betrouwbare én redactioneel sterke verhalen** kan maken.

Narrative moet de juiste feiten omzetten in een aantrekkelijke, gerichte en betekenisvolle verhaalontwikkeling, zonder zelf nieuwe feiten of niet-onderbouwde conclusies te introduceren.

**Start nu de Work-overdracht en voer Experiment 13 uit.**
