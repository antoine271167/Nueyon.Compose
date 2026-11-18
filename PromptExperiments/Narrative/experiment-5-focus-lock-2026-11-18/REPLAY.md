# Reproduceren en afbreken

Gebruik basiscommit 2e31288 met deze experimentmap. Open PowerShell in de repository en stel $x in op deze map.

    dotnet build "$x/runner/Runner.csproj"
    pwsh -NoProfile -File "$x/run.ps1"
    pwsh -NoProfile -File "$x/run.ps1" -Replay

De live runner verwacht OpenAI__ApiKey of de bestaande Host Console user secret. Een nieuwe live meting vereist een aparte kopie van deze experimentmap zonder calls. Offline replay gebruikt geen sleutel en stuurt niets naar het netwerk. Hij vergelijkt het gerenderde HTTP-request exact en verwerkt de bewaarde response via dezelfde productieagent.

Maak een leeg PAUSE-bestand in de experimentmap om na de lopende call te stoppen. Verwijder het bestand en herstart om voltooide calls over te slaan en verder te gaan. Ctrl+C annuleert ook een lopende call. Een poging met attempt.json zonder result.json stopt de runner bij hervatting zodat er geen ongemerkte nieuwe aanvraag wordt gedaan. Bekijk live.txt en calls per ID voor voortgang.

