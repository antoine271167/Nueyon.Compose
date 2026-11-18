# Baseline opnieuw uitvoeren

Gebruik .NET 10 en Node.js vanuit de repositoryroot op de productieversie uit het protocol. De prompts worden geladen uit de productieassembly. De kopieën en het freeze-bestand bewaken de versie van prompts, inputs, rubric en runner. Zet voor live gebruik de bestaande lokale OpenAI-ontwikkelsleutel in de Host Console User Secrets of gebruik de omgevingsvariabele OpenAI__ApiKey.

```powershell
$d = 'PromptExperiments/Narrative/baseline-experiment-1-2026-11-18'
dotnet build "$d/runner/Runner.csproj" --nologo
$runner = "$d/runner/bin/Debug/net10.0/Runner.dll"
dotnet $runner $d preflight
```

## Offline verificatie

```powershell
$config = Get-Content "$d/config.json" -Raw | ConvertFrom-Json
foreach ($id in $config.schedule) {
    dotnet $runner $d $id --replay
    if ($LASTEXITCODE) { throw "Replay failed for $id" }
}
node "$d/verify.cjs"
dotnet test src/Nueyon.Compose.Application.Tests --filter 'FullyQualifiedName~Narrative|FullyQualifiedName~StoryWorkflow'
```

## Live herhaling

Kopieer de experimentmap naar een nieuwe map onder dezelfde parent. Neem config.json, freeze.json, protocol.md, cases, prompts, runner, en verify.cjs mee. Begin met een lege calls-map. Richt $d op de nieuwe map en bouw de runner zoals hierboven. Onderstaand schema opent per run een nieuwe agentsessie. Maak tijdens de uitvoering een bestand PAUSE in de experimentmap om na de lopende call te pauzeren. Ctrl+C annuleert de lopende runner. Bewaar een eventueel mislukte of afgebroken poging; een bestaande marker blokkeert een tweede request met dezelfde run-ID.

```powershell
$config = Get-Content "$d/config.json" -Raw | ConvertFrom-Json
foreach ($id in $config.schedule) {
    if (Test-Path "$d/PAUSE") { break }
    if (Test-Path "$d/calls/$id/result.json") { continue }
    dotnet $runner $d $id
    if ($LASTEXITCODE) { throw "Run failed for $id" }
}
```

Een live herhaling gebruikt dezelfde inputs en configuratie. De provider kan andere tekst genereren. Offline replay toetst de productieparser tegen de opgeslagen response.
