# Experiment v4 · productieacceptatie Idea Agent

**Besluit op 10 november 2026 — promotie afgewezen.** Op revisie `525b2ea1cfdbc97e4df6f9cbe9858d29db133eda` scoort de oorspronkelijke reeks **1/3**, met **10/10** voor strategie. De holdouts scoren **0/4**. De validator gaf alle zeven eerste ideeën direct akkoord. Zes bron- of doelproblemen bereikten daardoor de herstelstap niet. De productiepromptset is behouden.

## Onafhankelijke beoordeling

| Case | Brontrouw | Doel | Focus | Invalshoek | Aantrekkingskracht | Totaal | Oordeel |
| --- | ---: | ---: | ---: | ---: | ---: | ---: | --- |
| [Strategie](ratings/01-strategy.json) | 2 | 2 | 2 | 2 | 2 | 10 | Geslaagd |
| [005](ratings/02-development.json) | 1 | 1 | 2 | 2 | 2 | 8 | Afgekeurd |
| [010](ratings/04-analysis.json) | 1 | 1 | 2 | 2 | 2 | 8 | Afgekeurd |
| [Museum](ratings/holdout-01-museum.json) | 1 | 0 | 2 | 2 | 2 | 7 | Afgekeurd |
| [Vervoer](ratings/holdout-02-transit.json) | 2 | 1 | 2 | 2 | 1 | 8 | Afgekeurd |
| [Bibliotheek](ratings/holdout-03-library.json) | 1 | 2 | 1 | 2 | 2 | 8 | Afgekeurd |
| [Zaadbank, nieuw](ratings/holdout-04-seedbank.json) | 1 | 2 | 2 | 2 | 1 | 8 | Afgekeurd |

**005 en 010.** Bij 005 kloppen de scores, de herhaalde hiërarchieproblemen en de niet-promotie. Titel en rationale maken daarvan een causale les over generieke promptregels. De kleine, niet-gelijktijdige vergelijking en mogelijke upstream- en backendvariatie begrenzen die conclusie. Bij 010 verandert *Target the Narrative stage* een terugkerende observatie in een vooraf bepaalde interventie. De rationale belooft bovendien het hoogste onmiddellijke effect. De bron vereist eerst lokalisatie van de gekozen afwijking en inspectie van de feitelijke upstream-invoer. [005-run](runs/acceptance/02-development/) · [010-run](runs/acceptance/04-analysis/)

**Holdouts.** Museum verschuift van een hoogwaterbericht voor bezoekers naar een evaluatie voor medewerkers, met een toegevoegde meetagenda. Vervoer bewaart de gekozen vijfminutenproef en de metingen, maar laat de concrete mismatch weg en richt zich vooral op beleids- en operationele lezers. Bibliotheek bewaart de praktische instructies en voegt het algemene publiek en extra uitwerking over kanalen en bewegwijzering toe. De nieuwe zaadbankcase bewaart de beperkte voortzetting en de kiemcijfers, maar schrijft het commissiebesluit toe aan de resultaten terwijl de bron de motivatie niet vastlegt. [Holdoutruns](runs/holdout/)

## Validator en herstel

De offline controles gingen vooraf aan de betaalde runs. Alle zes harnasreeksen slaagden. Drie namen na twee afwijzingen de derde generatie aan. Drie gaven na exact drie afwijzingen de expliciete fout, zonder resultaat. Iedere herpoging kreeg de exacte oorspronkelijke taak en bron, het vorige eerste idee en de laatste diagnose in een schone sessie. De judge zag alleen de ongewijzigde bron en de actuele eerste selectie. De volledige tests bevestigen ook afwijzing van oude, dubbele of tegenstrijdige verdicts en het uitblijven van Research-overdracht bij uitputting. [Offline overzicht](verification/offline-summary.json) · [Eindtests](verification/final-tests.txt)

De vaste diagnoses toetsen transport en het nieuwe contract. De afzonderlijke broncontrole bevestigde dat hun grenzen uit de bron kwamen. Voor 005 betrof dit tweemaal de empirische reikwijdte en attributie. Voor 010 betrof dit tweemaal de keuze van het interventiepunt op basis van het concrete geval. Voor museum betrof het eenmaal de onbewezen tevredenheidsclaim en eenmaal de verwisseling van publiek en doel. De velden bevatten geen vervangende tekst of extra onderzoeksopdracht. [Vaste diagnoses](fixtures/)

In de zeven echte CLI-runs was **1/7 judgeverdicts inhoudelijk correct** volgens de onafhankelijke beoordeling. Er verschenen **nul live diagnoses en nul herpogingen**. De inhoudelijke kwaliteit van echte diagnoses en zelfstandig herstel blijft daarmee ongemeten. De offline proeven tonen dat de code deze paden uitvoert wanneer een blokkade wordt teruggegeven. Ze zeggen niets over de kans dat GPT-5-mini die blokkade herkent of een reparatie goed uitvoert. [Verdictbeoordeling](ratings/judge-verdicts.json)

## Vergelijking met eerdere rondes

| Ronde | Oorspronkelijke cases | Strategie | 005 | 010 | Volledige holdouts | Waargenomen probleem |
| --- | ---: | ---: | ---: | ---: | ---: | --- |
| [v1](../production-cli-acceptance-v1/report.md) | 1/3 | 10 | 8 | 9 | — | Causale generalisatie en Narrative-prioriteit |
| [v2](../production-cli-acceptance-v2/report.md) | 1/3 | 10 | 9 | 8 | 0/1 | Replays herkenden meer fouten. In CLI bleven overclaim en koerswijziging staan |
| [v3](../production-cli-acceptance-v3/report.md) | 1/3 | 10 | 6 | 7 | 2/3 | Correcte verdictreeksen konden nog voorschrijvende of onjuiste correcties geven |
| v4 | 1/3 | 10 | 8 | 8 | 0/4 | Directe acceptatie liet alle gemiste blokkades zonder herstel |

De v4-conditie test de huidige productiecombinatie. V2 en v3 gebruikten andere promptkandidaten, en de provideralias kan modelvariatie verbergen. De vergelijking toont terugkerende foutpatronen, zonder het effect van de C#-wijziging afzonderlijk te schatten. Er is één run per bron. De nieuwe holdout is synthetisch, opgesteld na de keuze van de promptconditie en vooraf bevroren. Dezelfde beoordelaar stelde deze bron op en scoorde de ideeën voordat hij de diagnoses las. De beoordeling was niet extern of volledig geblindeerd. De bevestigingsrun verviel door de gemiste gates. [Gates](ratings/initial-gates.json) · [Protocol](protocol.md)

## Artefacten en vervolg

- **14 waargenomen logische aanroepen**, zeven generator en zeven judge. De zeven processen reserveerden 42 van het vooraf vastgelegde plafond van 48. Providerfacturering, modelresolutie en transportherpogingen zijn buiten de CLI-trace. [Aanroeplogboek](verification/model-call-ledger.json)
- De zeven volledige CLI-traces, uitvoer en metadata staan onder [runs](runs/). De zeven offline reconstructies reproduceerden de berichten en schema's op applicatieniveau uit dezelfde prompts, code en trace. De productie-CLI exporteert de judge-uitkomst als pogingstatus en eventuele blokkerdiagnose, zonder ruwe providerrespons. [Reconstructies](verification/request-replays.json) · [Bevroren hashes](verification/frozen.json)
- Alleen [strategie](approved-research/01-strategy/research-input.json) leverde een schema-valide ResearchInput op met exact dezelfde broninhoud en hetzelfde geselecteerde idee. Zes [afgekeurde fixtures](fixtures/rejected-cli/) bewaren het oorspronkelijke resultaat, de trace en score.
- De eindbuild slaagde met nul fouten en waarschuwingen. Alle 230 vindbare tests slaagden. Domain bevatte geen vindbare tests. Alle 1.251 gecontroleerde oudere bestanden, 14 productiepromptbestanden en 36 bevroren v4-bestanden behielden hun hashes. De Git-revisie bleef op 525b2ea. [Build](verification/final-build.txt) · [Integriteit](verification/final-integrity.json)

**Aanbeveling.** Richt de volgende proef eerst op detectie. Gebruik een kleine bevroren set paren met een juist idee en één materiële afwijking per paar. Neem causale attributie, voorwaardelijke interventieprioriteit en publieksverschuiving op. Vereis correcte verdicts én brongetrouwe diagnoses voordat een volgende volledige CLI-reeks de zelfstandige reparatie meet.
