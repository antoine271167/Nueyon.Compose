# Uitvoering en replay

De bevroren inputs, prompts, modelconfiguratie, volgorde en bronmaterialen worden rechtstreeks gelezen uit de naastliggende Experiment 11-map. Experiment 11b bewaart de nieuwe pogingen in calls, met de vervanging afzonderlijk gelabeld. Het archief bevat alle bestanden van de oorspronkelijke ongeldige poging. Zie amendement.md voor de vooraf vastgelegde verandering.

Voer vanuit deze map uit met .NET 10, PowerShell 7 en Node.js.

```powershell
dotnet build runner/Runner.csproj
pwsh -NoProfile -File ./run.ps1 -Replay
node ./verify.cjs requests
node ./verify.cjs
```

Replay gebruikt de productieagent en dezelfde SDK om de opgeslagen requests opnieuw op te bouwen en de opgeslagen responses lokaal te verwerken. De geldige calls worden vergeleken met hun opgeslagen request, input, resultaat en post. Voor de ongeldige poging wordt dezelfde parserfout verwacht. Bewaar de replay-uitvoer in replay.txt als de onderliggende artefacten opnieuw worden geverifieerd. De verifier gebruikt dat bestand voor de dekkingsgate.

Tijdens de oorspronkelijke uitvoering startte run.ps1 zonder -Replay. Het startpunt is vastgepind op de actuele main van vóór dit experiment. Plaats PAUSE in deze map om te stoppen vóór de volgende aanvraag. Verwijder het bestand vóór hervatting. Ctrl+C onderbreekt de lopende poging. Een onafgeronde poging blokkeert vervolg en wordt niet automatisch opnieuw uitgevoerd. De CLI controleert lokaal branch, commit en wijzigingen buiten de experimentmap vóór elke geplande positie.

Voor de review toont node ./verify.cjs review TOPIC de nieuwe posts gesorteerd op een stabiel pseudoniem, met het onderwerp en de fixture om de juiste bronnen en Narrative te kunnen vergelijken. Armlabels en callmetadata staan buiten de reviewweergave. Hetzelfde beoordelingsmodel kende het ontwerp. Tekstkenmerken, timing en voorkennis kunnen de blindering doorbreken. De eerst beoordeelde posts komen met hun bestaande scores uit Experiment 11.
