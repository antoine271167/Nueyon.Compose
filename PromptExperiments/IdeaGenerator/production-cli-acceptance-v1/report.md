# Acceptatieronde productie-Idea Agent

**Formeel besluit: inhoudelijke acceptatie niet behaald.** Eén van de drie eerste ideeën voldoet aan de vastgelegde acceptatiecriteria. De vereiste grens is 3/3. De Idea Agent is voor deze drie bevroren bronnen niet inhoudelijk geaccepteerd als betrouwbare overdracht naar Research.

## Uitvoering

De productieworkflow is niet gestart. Per bron is de nieuwe CLI gebruikt met de productie-DI, OpenAI `gpt-5-mini`, semantische judge en maximaal drie pogingen. Alle drie processen eindigden met exitcode 0 en schreven resultaat en trace.

- Revisie `5312288240d937ba744485dff50ecd85fec36e30`. Build geslaagd zonder waarschuwingen of fouten. 211 tests gevonden en geslaagd. Het Domain-testproject bevatte geen vindbare tests.
- De drie bronkopieën kwamen byte voor byte overeen met het manifest. De invoer bevat productie-equivalente bronmarkeringen.
- De 14 bestanden in de [promptsnapshot](prompts/) zijn byte-identiek aan de productieprompts. Iedere run meldt op stderr dat deze expliciete diskpromptmap is gebruikt.
- De discovery-documenten staan in `idea-schema.json`, `idea-describe.json`, `research-schema.json` en `research-describe.json`. [Preflight](preflight.json) legt model, commit, promptbestanden en exacte runcommando’s vast.

## Criteria en uitslag

Beoordeeld is uitsluitend `ideas[0]`, met alle vijf velden. De dimensies zijn brontrouw, redactioneel doel, focus, onderscheidende invalshoek en aantrekkingskracht, elk 0–2. Een idee slaagt met minstens 8/10, én 2/2 op brontrouw en redactioneel doel. De ronde vereist 3/3. Narrative en downstream Research zijn niet uitgevoerd.

| Bron en eerste idee | Brontrouw | Doel | Focus | Invalshoek | Aantrekkingskracht | Totaal | Besluit |
| --- | ---: | ---: | ---: | ---: | ---: | ---: | --- |
| [Nuëyon-strategie](01-strategy/idea-result.json) — bedrijfsintroductie met Compose als eerste uiting | 2 | 2 | 2 | 2 | 2 | 10/10 | Geslaagd |
| [Experiment 005](02-development/idea-result.json) — generieke promptregel kan averechts werken | 1 | 1 | 2 | 2 | 2 | 8/10 | Afgekeurd |
| [Experiment 010](04-analysis/idea-result.json) — Narrative als eerste kandidaat voor interventie | 2 | 1 | 2 | 2 | 2 | 9/10 | Afgekeurd |

### Nuëyon-strategie

Het idee houdt de bedrijfsidentiteit centraal en Compose als eerste concrete productrichting. De doelgroep sluit aan op §3 en het communicatiedoel op §12–14. De rationale bewaart de vroege fase en de onzekerheid rond latere richtingen. Sterke eerste selectie, één poging. Trace: accepted zonder feedback.

### Experiment 005

De bron rapporteert dat de kandidaatprompt niet werd gepromoveerd en dat de uitkomsten verschilden tussen de beperkte runs. Het idee generaliseert dit naar effecten die “consistently” optraden en naar een algemene les over generieke guardrails. Het maakt de kandidaatresultaten daarmee steviger en algemener dan de opzet en beperkingen rechtvaardigen. Een aansprekende invalshoek herstelt die brontrouwfout niet. Zie *Run scores*, *Analysis against the required criteria* en *Limitations* in de bevroren bron. Eén poging. De validator accepteerde het idee structureel en semantisch.

### Experiment 010

De tweede poging bewaart de belangrijke caveat dat Idea of Synthesis ook eerste drift kan vertonen en dat een Narrative-interventie gecontroleerd moet worden getest. Toch vernauwt het de bevinding tot company-first-hiërarchie en beveelt Narrative als eerste interventie aan. De bron lokaliseert verschillende driftsoorten in wisselende fasen en stelt voor per geval de vroegste onderbouwde afwijking te onderzoeken. Daarmee verandert de selectie de onderzoeksrichting. Zie *First observable drift by run*, *Cross-experiment patterns* en *Implication for the next focused trial*. Twee pogingen. De trace laat zien dat de judge poging één afwees wegens de te stellige Narrative-prioriteit. Poging twee werd geaccepteerd, maar de inhoudelijke doelpassing blijft onvoldoende.

## Artefacten en overdracht

Per bron zijn ongewijzigde bronkopie, Idea-invoer, volledige CLI-uitvoer, trace en uitvoeringsmetadata bewaard. Resultaten en traces staan onder de respectieve bronmap.

Er is voor alle drie ideeën een `research-input.json` gemaakt volgens het geëxporteerde Research-schema en de `ResearchInput(input, selectedIdea)`-constructie in `StoryWorkflow`. De inputs voor Experiment 005 en 010 zijn bewaard als reproduceerbare testfixtures, maar gelden niet als inhoudelijk geaccepteerde overdrachten. Alleen de strategie-input komt uit een geaccepteerd idee. Inhoudelijk doorsturen naar Research kan dus met de strategie. Deze ronde heeft Research niet uitgevoerd.

