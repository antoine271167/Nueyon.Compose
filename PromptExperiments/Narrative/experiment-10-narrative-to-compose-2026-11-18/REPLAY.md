# Uitvoering en offline replay

Uitvoeren vanaf de reporoot op de basiscommit uit config.json of deze artefactcommit met identieke productiebronnen. Vanuit PowerShell, met het experimentpad in `$x`:

    dotnet build src/Nueyon.Compose.slnx
    dotnet test src/Nueyon.Compose.slnx --no-build
    dotnet build "$x/runner/Runner.csproj"
    pwsh -NoProfile -File "$x/run.ps1" -Replay
    node "$x/verify.cjs"

Replay gebruikt de gearchiveerde HTTP-respons en controleert per call input, exacte requestbody en geparseerde Compose-output. De replayhandler roept het netwerk tijdens replay niet aan. `verify.cjs` vergelijkt beide aanvragen per paar na verwijdering van de ene toegevoegde alinea en controleert de model-ID, unieke respons-ID's, tokens, scores en beslissing.

De live runner leest de bestaande Console user secret of OpenAI__ApiKey. De sleutel wordt niet opgeslagen in de experimentartefacten. Live is bedoeld voor een werkomgeving zonder callartefacten. Een leeg PAUSE-bestand stopt de reeks vóór de volgende call. Ctrl+C annuleert de lopende call. Verwijder PAUSE om verder te gaan. De runner slaat afgeronde calls over en stopt bij een eerder begonnen, onopgeloste poging.

De reviewweergave met `node "$x/review.cjs" webb` gebruikt anonieme post-ID's. Een fixture-ID en herhalingsnummer kunnen de weergave verder beperken. Scores gebruiken, in volgorde, aantrekkelijkheid, leesbaarheid, hook, spanningsboog, afsluiting en focus. `decision.json` koppelt de review-ID's achteraf aan de arms en bevat elk paarsgewijs verschil.
