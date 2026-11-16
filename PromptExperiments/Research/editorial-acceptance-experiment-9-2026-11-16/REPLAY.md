# Experiment 9 offline replay

Geen live herhaling: behoud run.lock en alle attemptmarkers. Voer extract.cjs, prepare.cjs en freeze.cjs niet opnieuw uit; ze behoren tot de vooraf vastgelegde voorbereiding en kunnen input vervangen.

Vanuit repositoryroot uitsluitend:

    dotnet PromptExperiments/Research/editorial-acceptance-experiment-9-2026-11-16/runner/bin/Debug/net10.0/Runner.dll replay

Deze modus leest bestaande raw responses via ReplayClient zonder netwerk, gebruikt de echte ResearchAgent-parser en echte SynthesisResearchFilter, en vergelijkt met opgeslagen resultaten/projecties. De requestcontrole is een afzonderlijke offline stap via audit.cjs. trace.cjs maakt alleen een vindankerdraft, geen semantische beoordeling; assessments en eindtrace volgen uit de handmatige Work-audit.
