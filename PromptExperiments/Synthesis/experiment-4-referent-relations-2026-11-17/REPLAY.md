# Offline reproduceerbaarheid

Voer vanaf de repositoryroot de volgende commando's uit met Node.js 22 en .NET 10.

```powershell
$e = (Get-Item PromptExperiments/Synthesis/experiment-4-referent-relations-2026-11-17).FullName
dotnet "$e/runner/bin/Debug/net10.0/Runner.dll" replay
node "$e/verify.cjs"
node "$e/evaluate.cjs"
```

Gebruik de bestaande bevroren build. Herbouwen kan de runtime-hashes wijzigen. Replay voert de echte SynthesizerAgent lokaal uit met de opgeslagen response als enige modelrespons. De checks vergelijken de geparseerde inhoud, alle bevroren hashes, het verzonden request, usage en de berekende acceptatiepoort. De scoreberekening hergebruikt de handmatige beoordelingen. Replay toetst geen modeldeterminisme.

`run` is uitsluitend de oorspronkelijke live-uitvoering. De lock en pogingmarkers begrenzen die uitvoering tot het vooraf vastgelegde budget.
