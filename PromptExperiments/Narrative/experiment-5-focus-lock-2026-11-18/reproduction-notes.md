
Na voltooiing controleer je vanuit de repository de opgeslagen requests en responses met node "$x/verify.cjs". Genereer daarna de beslisuitkomst opnieuw met node "$x/score.cjs". De handmatige beoordelingen staan in narrative-scores.csv, linkedin-scores.csv en gate-evidence.json. Het script voor scores leest ook het replaylog en de geverifieerde gebruiksgegevens.

Build van de oplossing met dotnet build src/Nueyon.Compose.slnx. Tests met dotnet test src/Nueyon.Compose.slnx --no-build. Verifieer bestandsidentiteit met de SHA-256-waarden in freeze.json; baseline-verification.txt bevat de vooraf gebruikte fixture- en B-vergelijking. Live replay met nieuwe aanvragen is een nieuwe meting en kan andere teksten opleveren.
