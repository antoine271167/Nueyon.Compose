# Reproduceren

Gebruik .NET 10, Node.js en PowerShell vanuit de repositoryroot op uitgangscommit f40bf6a met production.diff toegepast en deze experimentmap aanwezig.

```powershell
$d = 'PromptExperiments/Narrative/experiment-3-compact-2026-11-18'
dotnet build "$d/runner/Runner.csproj" --nologo
& "$d/run.ps1" -Replay
node "$d/verify.cjs"
dotnet test src/Nueyon.Compose.Application.Tests --filter 'FullyQualifiedName~Narrative|FullyQualifiedName~StoryWorkflow'
```

Preflight controleert de bevroren bestanden en vergelijkt kopie A met de ingebedde prompts. Offline replay voert iedere opgeslagen response opnieuw door de echte Narrative-agent. verify.cjs vergelijkt inputs, gerenderde requests, responses en opgeslagen outputs, controleert de gelijkheid van de overige requestinstellingen per paar en reconstrueert usage.json.

Voor nieuwe live aanvragen, kopieer de experimentmap naar een nieuwe locatie naast deze map met alleen cases, prompts, config.json, protocol.md, production.diff, prompts.diff, REPLAY.md, freeze.json, de bronbestanden van runner, run.ps1 en verify.cjs. Laat calls weg uit de kopie. Gebruik de bestaande Host Console User Secrets of OpenAI__ApiKey. Bouw vanuit de nieuwe map zoals hierboven, voer run.ps1 uit zonder -Replay. Elke run krijgt een eigen agentcontext. Nieuwe aanvragen kunnen andere tekst opleveren.

Maak PAUSE aan in de actieve experimentmap om na de lopende aanvraag te stoppen. Verwijder PAUSE en start run.ps1 opnieuw om door te gaan met de eerste nog niet aangevraagde run. Ctrl+C annuleert de lopende aanvraag voor zover de client dit kan, en de provider kan een al ontvangen aanvraag alsnog hebben verwerkt. De attempt-marker voorkomt een nieuwe aanvraag onder dezelfde run-ID. Bewaar afgebroken pogingen in de map.
