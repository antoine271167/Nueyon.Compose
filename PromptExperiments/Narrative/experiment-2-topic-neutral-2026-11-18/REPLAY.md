# Reproduceren

Gebruik .NET 10, Node.js en PowerShell vanuit de repositoryroot op commit e5a54759f7a355a7707f9a9304cecce8bb0888d2 met deze experimentmap aanwezig.

```powershell
$d = 'PromptExperiments/Narrative/experiment-2-topic-neutral-2026-11-18'
dotnet build "$d/runner/Runner.csproj" --nologo
& "$d/run.ps1" -Replay
node "$d/verify.cjs"
node "$d/costs.cjs"
dotnet test src/Nueyon.Compose.Application.Tests --filter 'FullyQualifiedName~Narrative|FullyQualifiedName~StoryWorkflow'
```

Preflight controleert de bevroren bestanden en vergelijkt kopie A met de ingebedde prompts. Offline replay voert iedere opgeslagen response opnieuw door de echte Narrative-agent. verify.cjs vergelijkt inputs, gerenderde requests, responses en opgeslagen outputs, controleert de gelijkheid van de overige requestinstellingen per paar en reconstrueert usage.json. costs.cjs berekent kosten uit die usage en de vastgelegde tarieven.

Voor nieuwe live aanvragen, kopieer de experimentmap naar een nieuwe locatie naast deze map met alleen cases, prompts, config.json, protocol.md, prompts.diff, freeze.json, de bronbestanden van runner, run.ps1 en verify.cjs. Laat calls weg uit de kopie. Gebruik de bestaande Host Console User Secrets of OpenAI__ApiKey. Bouw vanuit de nieuwe map zoals hierboven, voer run.ps1 uit zonder -Replay. Elke run krijgt een eigen agentcontext. Nieuwe aanvragen kunnen andere tekst opleveren.

Maak PAUSE aan in de actieve experimentmap om na de lopende aanvraag te stoppen. Verwijder PAUSE en start run.ps1 opnieuw om door te gaan met de eerste nog niet aangevraagde run. Ctrl+C annuleert de lopende aanvraag voor zover de client dit kan, en de provider kan een al ontvangen aanvraag alsnog hebben verwerkt. De attempt-marker voorkomt een nieuwe aanvraag onder dezelfde run-ID. Bewaar afgebroken pogingen in de map.
