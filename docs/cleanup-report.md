# Clean-slate cleanup — 12 november 2026

Uitgevoerd in de lokale repository, zonder Copilot CLI. De werkboom was vooraf schoon. Niets gecommit of gepusht.

## Gewijzigd

- Eén Idea-workflow met verplichte semantische judge, maximaal twee generaties en één gedeelde structurele/semantische herstelpoging. Typed IdeaValidationException, bronisolatie, trace en blokkade vóór Research blijven behouden.
- Drie actuele geaccepteerde promptbestanden byte-voor-byte gepromoveerd naar `src/Nueyon.Compose.Application/Prompts/Idea/`: `Idea.system.prompt.txt`, `Idea.user.prompt.txt`, `IdeaValidation.system.prompt.txt`. Er bestaat geen afzonderlijk judge-userbestand: de bestaande JSON-opbouw in C# is behouden. Ook de bestaande hersteltekst is behouden.
- Idea-agent, promptmapping, evaluator, judge, DI, CLI-discovery, configuratie, solution, tests en CLI-README aangepast. De modelinstelling blijft `gpt-6-luna`; user secrets zijn niet bewerkt.
- Gerichte `.gitattributes`-regel beschermt de exacte promptbytes tegen automatische regeleindeconversie bij een latere commit/checkout. Drie tests controleren de SHA-256-hashes van de ingebouwde resources.

## Verwijderd

- Oude Idea-productiepromptinhoud en de extra promptsubmap.
- Workflowmode-enum, optionsklasse, configuratiesectie en conditionele selectie in DI/CLI.
- Twee tijdelijke solutionprojecten voor experimentuitvoering en bijbehorende tests; zes losse historische .NET-harnesses.
- Historische resultaatmappen: de introductie-experimenten van 3 november, driftlokalisatie, Narrative-boundaryproef, acceptance-v1, Baseline-002, generatorproeven 001/003/004/005, production-cli-acceptance-v1–v4, validator-acceptance-v1/v2, specialist-judges-v1 en evidence-aware-v1.
- Alleen tests voor vervallen modi zijn verwijderd. Productiegedragstests zijn aangepast aan het vaste budget; de workflowtestklasse is hernoemd.

De volledige lijst van verwijderde mappen met aantallen en gewijzigde gevolgde bestanden staat in [cleanup-verification.json](cleanup-verification.json). Git registreert 3296 verwijderde bestandspaden, inclusief het oude pad van de hernoemde testklasse. Gevolgde historische bestanden blijven beschikbaar in Git-historie.

## Behouden

- Vier productieprojecten en vier productietestprojecten.
- De volledige twee vergelijkingsmappen van 12 november: 65 bestanden inclusief rapporten, comparison.json, traces, resultaten en exacte intro/005/010-invoer. Historische paden en configuratiewaarden in deze bewijsstukken zijn bewust niet herschreven; zij beschrijven de oorspronkelijke uitvoering.
- Research-, Synthesis-, Narrative- en Compose-agentcode en prompts zijn ongewijzigd.
- [Belangrijkste experimentlessen en Luna-besluit](idea-experiment-lessons.md), [promptintegriteit](idea-prompt-integrity.json) en dit overzicht.

## Verificatie

- `dotnet build src/Nueyon.Compose.slnx`: geslaagd, nul fouten en waarschuwingen.
- `dotnet test src/Nueyon.Compose.slnx --no-build`: 271 geslaagd: Application 213, Infrastructure 22, Console 36. Het bestaande Domain-testproject bevat geen vindbare tests.
- CLI-discovery zonder modeluitvoering: één workflow, maximaal twee generaties, geen mode/profielen.
- CLI-promptresolver en volledige/missende overrides gecontroleerd met offline tests. De productiepromptboom vereist geen extra submap.
- Acht bestaande projectbestanden, twaalf geldige projectverwijzingen en alle solutionverwijzingen gecontroleerd.
- Drie productiepromptbestanden matchen exact het geaccepteerde SHA-256-manifest; ingebouwde resources matchen dezelfde hashes.
- Alle 65 behouden bewijsbestanden gelijk aan Git HEAD na de bestaande Git-regeleindenormalisatie; 43 bestanden van de eerdere vergelijking ook byte-voor-byte tegen de opgeslagen SHA-256-hashes gecontroleerd.
- `git diff --check`: geslaagd. Geen actieve verwijzingen naar verwijderde modeconfiguratie, projectbestanden of promptpaden.

## Grenzen en incident bij eerste testronde

Er is geen nieuwe live acceptatietest of volledige downstream-pipelinetest uitgevoerd. De bestaande driecasustest met Luna blijft een kleine, door dezelfde assistent beoordeelde steekproef; hij bewijst geen algemene modelbetrouwbaarheid.

Bij de eerste testronde bereikten twee oude configuratietests voor ongeldige modi onbedoeld de modelclient nadat de modevalidatie was verwijderd. Beide uitvoeringen eindigden met een fout. De tijdelijke configuratie gebruikte placeholders, maar de host kan user secrets met hogere prioriteit laden. Daarom kan niet worden bevestigd dat geen extern serviceverzoek of kosten zijn ontstaan. Deze obsolete tests zijn verwijderd vóór de definitieve geslaagde testronde. Er zijn geen afzonderlijke live modeltests gestart.
