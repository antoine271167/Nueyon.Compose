# Offline replay en artefactroutes

Deze run is eenmalig. `run.lock`, `evaluate.lock` en `attempt-started.json` behouden. Geen `run` of `evaluate` opnieuw uitvoeren en geen markers verwijderen.

Vanaf de repositoryroot, uitsluitend offline replay:

```powershell
dotnet PromptExperiments/Research/production-validation-experiment-6-2026-11-12/runner/bin/Debug/net10.0/Experiment6.dll replay
```

De replay injecteert iedere opgeslagen modeltekst via een lokale IChatClient in de echte productieagent, gebruikt de productieparser en vergelijkt het resultaat exact met result.json. Research-projecties worden opnieuw met de echte filter berekend. Geen netwerkclient of secret wordt voor replay aangemaakt. Freeze wordt eerst gecontroleerd; bij afwijkende runtime/source-hashes stopt de replay. Niet opnieuw bouwen boven de bevroren DLLs voor deze replay; herstel benodigde originele artefacten in een afzonderlijke kopie als de omgeving later verandert.

`calls/<lane>-<stage>/` bevat exacte input, systeemtekst, HTTP-request zonder authheaders, ruwe providerresponse/usage, metadata, geparseerd resultaat en leesbare content. Research heeft ook de echte projectie. De lane bevat case/herhaling/variant; gebruik die map niet voor geblindeerde beoordeling.

`blind/packets/` bevat de neutrale beoordelingsdossiers. `blind/key.json` en `blind/packet-manifest.json` bevatten de identiteit en moeten gescheiden blijven van een beoordelaar. `calls/<neutrale packetcode>-evaluation/` bewaart de afzonderlijke AI-beoordeling zonder ketenfeedback. `assessments-unblinded.json` is de later ontsleutelde, afgeleide analyse. Ruwe beoordelingsantwoorden worden nooit overschreven door deze analyse.

`protocol.md`, `rubric.md`, `labels.json`, `plan.json`, `freeze.json`, `protected-before.json`, `prompts/`, `cases/` en de runner vormen de vooraf bevroren opzet. `verify_results.py`, `analysis_tables.py`, dit replaydocument en het eindrapport zijn offline analyse/rapportage achteraf; zij staan niet alle in de oorspronkelijke freeze. Het uiteindelijke artifact-manifest legt ook deze bestanden vast.
