# Acceptatieronde productie-Idea Agent via de CLI

**Status: voorbereid, inhoudelijke acceptatie open.** De drie betaalde runs wachten op expliciete toestemming. Er is nog geen modeluitvoer, validatietrace of Research-input.

## Uitvoering

- Revisie `5312288240d937ba744485dff50ecd85fec36e30`. De build slaagde zonder fouten of waarschuwingen. Alle 211 gevonden tests slaagden. Het Domain-testproject bevatte geen vindbare tests.
- Eén geïsoleerde `run idea` per bevroren bron. De productiehost gebruikt OpenAI `gpt-5-mini` voor generator en semantische judge, met maximaal drie pogingen per bron. De CLI beschrijft de judge als blokkerend voor het eerste idee.
- [Preflight](preflight.json) bevat de exacte commando’s vanaf de reporoot, de bronherkomst, de redactionele doelen en SHA-256-hashes. De Markdown-kopieën zijn byte-identiek aan het acceptatiemanifest. Elke `idea-input.json` bevat die tekst tussen dezelfde bronmarkeringen als `SourceContextLoader` in de productie-workflow.
- De complete [promptsnapshot](prompts/) bevat 14 bestanden die byte voor byte overeenkomen met de productiepromptmap. De host controleert de expliciete promptmap op volledigheid en de loader leest daar de templates. Controleer in iedere live run de regel `Prompts: disk (...)` op stderr en herverifieer de hashes. De schijfroute is nu gecontroleerd in code en tests.
- [Idea-contract en trace](idea-describe.json) · [ResearchInput-schema](research-schema.json)

## Beoordeling na de runs

Beoordeel uitsluitend het eerste idee in rangorde, inclusief titel, beschrijving, doelgroep, rationale en evidence. Gebruik de vijf dimensies van het bestaande rubric (elk 0–2). Een idee gaat door bij minimaal 8/10 en 2/2 voor zowel brontrouw als redactioneel doel. Controleer expliciet overclaims en laat de uitkomst van de judge afzonderlijk zien. De ronde slaagt bij 3/3 geslaagde eerste ideeën. Deze steekproef bevat geen downstream- of Narrative-check.

| Bron | Aandachtspunt | Uitkomst |
| --- | --- | --- |
| [Nuëyon-strategie](01-strategy/) | Nuëyon als AI-first bedrijf staat voorop. Compose illustreert de eerste productrichting. Houd doelgroep, ambitie en beschikbaarheid binnen §3 en §12–14. | Open |
| [Experiment 005](02-development/) | Koppel de afwijzing aan de waargenomen scores en beperk de conclusie tot de geteste kandidaat. Zie *Run scores*, *Analysis* en *Limitations*. | Open |
| [Experiment 010](04-analysis/) | Scheid eerste zichtbare afwijking van oorzaak en interventieprioriteit. Narrative kwam vaker in beeld, met uitzonderingen bij Idea en Synthesis. Zie *First observable drift*, *Cross-experiment patterns* en *Implication for the next focused trial*. | Open |

Bewaar per run resultaat, trace, exitcode en stderr. Analyseer welke afwijzingsfeedback in latere pogingen is verwerkt. Voor elk inhoudelijk geaccepteerd idee volgt `research-input.json`: dezelfde `StoryInput` als `input` en de ongewijzigde eerste Idea als `selectedIdea.idea`, conform de constructor in `StoryWorkflow`. Research en de volledige workflow vallen buiten deze ronde.
