# Idea Generator Experiment 004

## Discrepantie met Experiment 003

Het rapport van 003 beschrijft de optionele Idea-testmodus. De versie op Git-HEAD `9c7af69f27b45c1d5b3dbdd828a8d19b23d5c85b` heeft die optie niet. De huidige werkkopie van `Program.cs` bevat al lokale wijzigingen met een aparte `--idea-output`-tak. Die tak gaat via `TestRun.cs` naar de productie-SourceContextLoader, het IdeaResponse-schema en de productievalidator in een LoopAgent met maximaal drie iteraties. De drie geselecteerde 003-outputs en hun bewaarde bestanden `iteration-1.*` zijn consistent met die testroute en één modelpoging per bron. De generieke route maakt één agent met `AsAIAgent` en doet één `RunAsync`. De CLI-optie `--validate` controleert invoer en placeholders. De generieke modelaanroep retourneert tekst zonder productie-IdeaResponse-schema of -validator.

De strategie-output van 003 kiest een bedrijfsintroductie met Compose als eerste concrete richting, maar voegt lezersgroepen toe die de bron niet aanwijst. De 005-output trekt een algemene les over de betrouwbaarheid van een generieke promptregel uit drie kandidaat- en drie baselineruns. De 010-output schuift Narrative naar voren als primair interventiepunt. De diagnose rapporteert Narrative als terugkerende eerste zichtbare locatie en vraagt per gekozen fout eerst de betreffende fase en invoer te onderzoeken.

## Opzet

Gebruik de generieke PromptRunner met OpenAI `gpt-5-mini` en de standaardinstellingen van de SDK. De systemprompt blijft byte-identiek aan 003. Voeg aan de kandidaat-userprompt van 003 één alinea toe bij de controle van het eerste idee. Begrens observaties tot de onderzochte gevallen, houd waarneming, mogelijke verklaring en aanbeveling uit elkaar, en bewaar dezelfde beperking in de centrale claim. Een aparte formaatregel beschrijft de vijf bekende JSON-velden in tekst omdat de generieke route geen schema meestuurt. Dit uitvoeringsverschil en de ene run per bron beperken vergelijking met 003.

Gebruik de drie exacte bronsnapshots uit 003 in dezelfde volgorde. Controleer de YAML en de gerenderde bron vóór de modelaanroepen. Beoordeel telkens `ideas[0]` uit de ruwe tekstuitvoer op brontrouw zonder ernstige overclaims, passende redactionele richting en bruikbaarheid voor Research. Een vlakke titel telt niet als blokkade. GO vereist drie bruikbare eerste ideeën die alle drie criteria halen. De drie geplande modelruns vormen de volledige screening.

## Uitvoering en besluit

| Run | Bron | PromptRunner | Tijd | Eerste idee |
| --- | --- | --- | ---: | --- |
| 1 | Nuëyon-strategie | Exit 0 | 38,59 s | [Selectie](runs/01-strategy/selected-idea.json) |
| 2 | Experiment 005 | Exit 0 | 28,78 s | [Selectie](runs/02-experiment-005/selected-idea.json) |
| 3 | Experiment 010 | Exit 0 | 31,87 s | [Selectie](runs/03-experiment-010/selected-idea.json) |

**NO-GO voor Research.** Eén van de drie geselecteerde ideeën voldoet aan de pragmatische criteria. Experiment 005 houdt de titel dichter bij de proef dan 003, maar de rationale maakt van het waargenomen resultaat opnieuw een oorzakelijke formulering en onderbouwing voor ongeteste maatregelen. Experiment 010 maakt de vaakst waargenomen locatie opnieuw tot voorkeursinterventie. Dit zijn bevindingen over de drie opgeslagen outputs. Met één run per bron en een andere uitvoermodus kan het effect van de promptalinea op zichzelf niet worden vastgesteld.

De [evaluatie](evaluation.md) bevat de onderbouwing per bron. [Preflight](preflight.json) registreert de bron-, prompt- en buildhashes. Per run zijn de Markdown-snapshot, YAML-invoer, gerenderde prompt, uitvoer, foutuitvoer, uitvoeringsmetadata en de ongewijzigde eerste selectie bewaard. De vormcontrole achteraf verifieerde de vijf gevraagde velden in het eerste idee. De drie runs gebruikten uitsluitend de generieke PromptRunner, met één agentaanroep per run. Daarna is gestopt. De productieprompts zijn geverifieerd tegen hun hashes van vóór de runs. Er is geen commit of push uitgevoerd.
