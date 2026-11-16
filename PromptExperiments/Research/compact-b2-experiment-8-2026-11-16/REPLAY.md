# Offline replay Experiment 8

Vanuit C:/Workspaces/Nueyon.Compose:

```powershell
dotnet PromptExperiments/Research/compact-b2-experiment-8-2026-11-16/runner/bin/Debug/net10.0/Runner.dll replay
node PromptExperiments/Research/compact-b2-experiment-8-2026-11-16/verify.cjs
python -X utf8 PromptExperiments/Research/compact-b2-experiment-8-2026-11-16/analyze.py
```

Alleen replay gebruikt de echte ResearchAgent-parser met een in-memory ReplayClient; geen provider of geheim nodig. verify.cjs controleert requests, usage en freeze. analyze.py reconstrueert de matrix en tabellen uit reeds vastgelegde Work-beoordelingen; regex is alleen een citatenzoeker, geen semantische beoordelaar. Een normale Python 3 volstaat; lokaal is de aanwezige portable Python gebruikt. Exacte live execution: éénmaal `dotnet <Runner.dll> run` na freeze. **Niet opnieuw uitvoeren.** De exclusieve run.lock en attemptmarkers beschermen tegen dubbele uitgaven. prepare.py en freeze.cjs zijn historische voorbereidingsartefacten, geen replaystappen: niet opnieuw uitvoeren, daarmee zou de oorspronkelijke freeze worden overschreven.

De huidige freeze bindt runtime-DLLs uit de uitvoering. Build vanaf runner/Runner.csproj kan een nieuw runtimebestand opleveren en hoort in een kopie van de artefacten als die binaries ontbreken; bewaar de oorspronkelijke hashes en claim dan geen binaire identiteit zonder vergelijking. De opgeslagen requests/responses en bron/Idea/prompts maken inhoudscontrole mogelijk zonder livecalls. Geen oplossingbrede configuratie of productiepromptwijziging nodig.

Analyse/rapport/verify.cjs zijn na generatie geschreven en geen onderdeel van de prospectieve freeze. artifact-manifest.json dekt de uiteindelijke levering, behalve zichzelf; de oorspronkelijke freeze blijft behouden.
