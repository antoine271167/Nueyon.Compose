# Uitvoering en replay

De proef gebruikt de productiecode op basiscommit uit config.json. Voer onderstaande opdrachten uit vanaf de reporoot, met het pad naar deze map in $x.

    dotnet build src/Nueyon.Compose.slnx
    dotnet test src/Nueyon.Compose.slnx --no-build
    dotnet build "$x/runner/Runner.csproj"
    pwsh -NoProfile -File "$x/run.ps1" -Stage narrative -Replay
    pwsh -NoProfile -File "$x/run.ps1" -Stage linkedin -Replay
    node "$x/verify.cjs"

Replay vergelijkt de volledige requestbody, input en geparseerde output voor elke voltooide call. Replay gebruikt geen netwerk en geen echte sleutel. Live maakt per ID een afzonderlijke stateless aanvraag met de bestaande Console user secret of OpenAI__ApiKey. Voer live uit in een verse checkout zonder historische calls. Vervang voor een nieuwe run de oorspronkelijke handmatige Narrative-scores door beoordelingen van de verse briefs vóór de LinkedIn-fase.

Maak voor een pauze na de lopende call een leeg PAUSE-bestand in deze map. Ctrl+C annuleert de lopende aanvraag. Hervatting slaat voltooide calls over en stopt bij een onopgeloste poging.

De oorspronkelijke LinkedIn-uitvoering werd na ontdekking van een soortfout gepauzeerd en met gecorrigeerde scores hervat. De twee reeds gemaakte calls die de gecorrigeerde poort afwijst, zijn afzonderlijk offline te reproduceren:

    dotnet "$x/runner/bin/Debug/net10.0/Runner.dll" "$x" linkedin-wolf-r1-H --replay
    dotnet "$x/runner/bin/Debug/net10.0/Runner.dll" "$x" linkedin-wolf-r2-F --replay
