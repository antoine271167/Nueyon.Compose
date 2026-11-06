# Experiment 001 — Idea Generator-baseline

De baseline gebruikt de Idea-prompts uit Compose op commit de6b479fb7dd4175b803834f3428528de73a2dd6. De oorspronkelijke prompts en het volledige bronbestand zijn byte-identiek gekopieerd.

## Invoer en model

- De consoleloader verpakt het ene geselecteerde Markdown-bestand met zijn gebruikelijke BEGIN SOURCE- en END SOURCE-markers. De brontekst binnen die markers is integraal en ongewijzigd.
- IdeaAgent rendert de userprompt met uitsluitend StoryInput.Content als SourceContent. De systemprompt heeft geen invoervelden. Het experiment voegt geen doelformaat of kanaal toe.
- PromptRunner leest SourceContent uit experiment-001.yaml. In de runnerkopie van de userprompt staat {SourceContent}, de syntaxis van PromptRunner. De gerenderde system- en userprompt zijn teken voor teken vergeleken met de Compose-weergave.
- Beide gebruiken de OpenAI Chat-client met gpt-5-mini en standaard samplinginstellingen. Compose vraagt bij uitvoering het IdeaResponse-schema en kan tot drie validatie-iteraties uitvoeren. PromptRunner doet één agentaanroep met vrije tekstuitvoer. Het meetpunt is daarom een promptbaseline en de uitvoer is geen test van het Compose-responscontract.
- PromptRunner schrijft alleen de antwoordtekst naar stdout. Het rapporteert geen provider-response-ID, tokengebruik of modelversie achter de alias.

## Reproduceren

Bouw vanaf de repositoryhoofdmap met .NET 10:

    dotnet build src/Nueyon.PromptRunner/Nueyon.PromptRunner.csproj -o PromptExperiments/IdeaGenerator/experiment-001/.build

Open deze experimentmap en voer het volgende commando uit:

    dotnet .build/Nueyon.PromptRunner.dll --system prompts/runner/Idea.system.prompt.txt --user prompts/runner/Idea.user.prompt.txt --data experiment-001.yaml --config appsettings.json

Voeg afzonderlijk --validate en --print-rendered toe voor de controles zonder modelaanroep. Stel voor een modelaanroep OPENAI_API_KEY in de procesomgeving in. De oorspronkelijke baseline gebruikte de lokaal geconfigureerde Compose-referentie in een tijdelijk proces. Resultaten en SHA-256-controlesommen staan onder results/.
