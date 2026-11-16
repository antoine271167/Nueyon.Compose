# Research B2 productiepromotie — 16 november 2026

Promotie en offline validatie voltooid; cleanup geïnventariseerd maar verwijdering geblokkeerd. Niets gecommit of gepusht. Geen Copilot CLI, modelcalls of nieuwe promptoptimalisatie.

## Exact gewijzigde/toegevoegde paden

- `.gitattributes`: uitsluitend de twee Research-productieprompts beschermen tegen Git-regeleindeconversie.
- `src/Nueyon.Compose.Application/Prompts/Research/Research.system.prompt.md`
- `src/Nueyon.Compose.Application/Prompts/Research/Research.user.prompt.md`
- `src/Nueyon.Compose.Application.Tests/Prompts/PromptLoaderTests.cs`: SHA-256-controle van beide ruwe ingebouwde resources.
- `src/Nueyon.Compose.Application.Tests/Agents/ResearchAgentEvidenceTests.cs`: offline replay van alle 24 Experiment 9-antwoorden via de actuele productieparser en het echte SynthesisResearchFilter.
- `docs/research-cleanup-inventory-2026-11-16.json`: omvang van alle bestaande Research-/Idea-experimentmappen.
- `docs/research-promotion-verification-2026-11-16.json`: byteverificatie, exacte cachekandidaten per bestand met hashes, behouden freezeverwijzingen en testresultaten.
- `docs/research-promotion-report-2026-11-16.md`: dit rapport.

Geen bestanden verwijderd. Geen wijzigingen aan agentcode, configuratie of Idea-, Synthesis-, Narrative- en Compose-prompts. Alle overige oorspronkelijk gevolgde bestanden zijn bytegelijk aan de vooraf vastgelegde lokale snapshot, inclusief alle experimentbestanden.

## Byteverificatie

Bron voor beide bestanden: `PromptExperiments/Research/editorial-acceptance-experiment-9-2026-11-16/prompts/B2/Research/`. Rechtstreeks als bytes gekopieerd naar de overeenkomstige productiepromptbestanden, zonder tekstbewerking.

| Bestand | Bytes | SHA-256 bron, productie en ingebouwde resource |
|---|---:|---|
| Research.system.prompt.md | 2609 | `de155005b5f494e6b54c850c6315270c95bf51c2b21c46a9d38f495ceee0417f` |
| Research.user.prompt.md | 599 | `2373b036a4f538fb730589aae47b06cf8fc26134a1adb60c9e3597eced96d3ec` |

Directe bytevergelijking geslaagd; hashes komen bovendien overeen met B2 in het oorspronkelijke Experiment 9-promptfreeze-manifest. Geen onbedoelde promptwijziging. Beide productiepromptpaden hebben Git-attribuut `-text`.

## Research Judge

`OpenAiOptions.EnableResearchJudge` is een bool zonder initializer en dus standaard false. `Infrastructure/ServiceExtensions.cs` maakt alleen bij true een ResearchSemanticJudge aan; anders krijgt ResearchAgent null. Judgecode en validatieprompt zijn behouden en ongewijzigd.

Optionele route: configuratiesleutel `OpenAI:EnableResearchJudge=true`, bijvoorbeeld JSON onder OpenAI of omgevingsvariabele `OpenAI__EnableResearchJudge=true`. De CLI laadt achtereenvolgens appsettings.json, appsettings.<ASPNETCORE_ENVIRONMENT of Production>.json, omgeving en user secrets; latere bronnen hebben voorrang. In de aanwezige appsettingsbestanden, de huidige omgeving en de betreffende lokale user secrets is geen Judge-override aangetroffen. Er is niets ingeschakeld of aan configuratie veranderd. Andere werkmappen/deploymentomgevingen kunnen eigen overrides bevatten.

## Build en regressie

- `dotnet build src/Nueyon.Compose.slnx --no-restore -t:Rebuild`: geslaagd, 0 fouten, 0 waarschuwingen.
- `dotnet test src/Nueyon.Compose.slnx --no-build`: 303 geslaagd, 0 mislukt, 0 overgeslagen: Application 241, Infrastructure 25, Console 37. Het bestaande Domain-testproject bevat geen vindbare tests.
- Inclusief bestaande Research-evidence-, Judge-, DI-, promptresolver- en CLI-integratietests; twee nieuwe resourcehashchecks en één replaytest over 24 raw responses.
- Alle 24 parsed resultaten en alle 24 Facts/Unknowns-projecties zijn exact gelijk aan de opgeslagen Experiment 9-resultaten. De replay gebruikt uitsluitend een in-memory client, zonder netwerk/providerconfiguratie.
- `git diff --check`: geslaagd.

Bij de eerste incrementele build na kopiëren waren de ingebouwde resources nog oud; beide nieuwe hashchecks faalden terecht. Volledige rebuild loste dit op zonder prompt- of productiecodewijziging. Bij publicatie daarom eveneens een schone rebuild uitvoeren en de resourcehashchecks behouden.

## Cleanup en behoud

Alle twaalf resterende Research-/Idea-experimentmappen geïnventariseerd. Historische rapporten, baselines, logs, duplicaten en raw data blijven behouden: logs zijn deels gehasht bewijs en gelijkende bestanden kunnen nodig zijn voor historische scripts. Geen unieke bewijswaarde opgeofferd voor ruimtebesparing.

Negen niet-gerefereerde obj-cachemappen vormen een concrete kleine cleanup: 196 bestanden, 3.892.333 bytes. Hun exacte paden/hashes staan in het verificatiebestand. De automatische goedkeuringscontrole blokkeerde de verwijderopdracht met “blocked by policy”; de opdracht is niet uitgevoerd en er is geen omweg gebruikt. Daarom blijven ook deze caches aanwezig. Er is geen brede destructieve cleanup gestart.

De obj-mappen van Experiment 8 en 9 zijn expliciet behouden omdat freeze.json ze refereert. Alle runner/bin-mappen blijven behouden voor historische replayinstructies en/of freezehashes. Experiment 9 is volledig behouden: rapport, protocol, natuurlijke broninputs/downloads, prompts, usage, traceability, audits, verificaties, requests, raw responses, resultaten, projecties, runner, run.lock en attemptmarkers.

## Resterende beperkingen

De oorspronkelijke historische freeze-controles beschermen ook toenmalige productieprompts en testbroncode. Door deze geautoriseerde promotie kunnen historische runners in deze gewijzigde werkboom op die freezecontrole stoppen. Dat is geen verwijderde dependency: de oude snapshots, runners en manifesten zijn intact. Gebruik voor de volledige oorspronkelijke freeze-replay een aparte checkout van de oorspronkelijke commit; de nieuwe offline regressietest verifieert alle 24 antwoorden tegen de huidige parser/filter zonder historische freezes te herschrijven.

Geen nieuwe live acceptatie of downstreammodeltest uitgevoerd. De beperkte Experiment 9-bewijskracht blijft gelden: vier bron/Idea-paren en beoordeling door dezelfde assistent bewijzen geen algemene foutloosheid. Commit/push vereist nog afzonderlijke toestemming.
