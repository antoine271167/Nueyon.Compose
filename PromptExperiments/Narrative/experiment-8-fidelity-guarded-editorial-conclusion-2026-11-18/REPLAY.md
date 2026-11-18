# Uitvoeren, pauzeren en offline reproduceren

Voer uit in de hoofdmap van de repository op basiscommit 94a25837948298aa96bf56c242485b979aef6fc2. Zet $x op deze experimentmap.

    dotnet build "$x/runner/Runner.csproj"
    pwsh -NoProfile -File "$x/run.ps1" -Stage phase1
    pwsh -NoProfile -File "$x/run.ps1" -Stage phase1 -Replay
    node "$x/verify.cjs" phase1

De live runner gebruikt OpenAI__ApiKey of de bestaande Console user secret. Offline replay vergelijkt de volledige HTTP-body met het bewaarde request en verwerkt de opgeslagen response via de productieagent. Er is geen netwerktoegang of sleutel nodig voor replay.

Maak een leeg PAUSE-bestand in deze experimentmap om na de lopende aanvraag te stoppen. Ctrl+C kan ook de lopende aanvraag annuleren. Verwijder PAUSE en voer hetzelfde commando uit om voltooide runs over te slaan en verder te gaan. Een poging zonder resultaat blokkeert hervatting en wordt niet stilzwijgend herhaald. De live-uitvoer en de mappen onder calls tonen de voortgang. Gebruik bij NO-GO alleen offline replay voor de uitgevoerde calls.

Het script opent phase2 uitsluitend bij een GO in phase1-decision.json. LinkedIn vereist daarnaast een GO in phase2-decision.json en alle fase 2 Narrative-resultaten. Voor reproductie van uitgevoerde fasen gebruikt u de betreffende -Stage met -Replay en verifieert u de instellingen en het tokengebruik met verify.cjs.
