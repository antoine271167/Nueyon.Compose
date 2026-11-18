# Uitvoering en offline replay

Bouw de oplossing en de experimentele runner met .NET 10. Voer vanuit de experimentmap `pwsh -NoProfile -File ./run.ps1 -Replay` uit nadat `dotnet build runner/Runner.csproj` is geslaagd. Replay verifieert de exacte input, HTTP-requestbody en geparseerde output met de opgeslagen HTTP-response, zonder netwerkaanroep. `node verify.cjs` verifieert armisolatie en berekent scores en verbruik.

Live uitvoeren kan met `pwsh -NoProfile -File ./run.ps1`. Een leeg PAUSE-bestand stopt de reeks tussen calls. Verwijder het bestand voor hervatting. Ctrl+C onderbreekt de lopende call. De runner slaat voltooide calls over en stopt bij een onafgeronde poging. De runner leest de bestaande Console user secret of OpenAI__ApiKey. De sleutel wordt niet opgeslagen.
