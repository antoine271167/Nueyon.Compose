# Validator Acceptance Experiment · Nuëyon.Compose

**10 november 2026 · HEAD `0d0c86b` · No-go voor promotie.** De kandidaat verkleinde het aantal gemiste blokkades op de bevroren tuning-set van twee naar nul, maar gaf zelf bij 010 een voorschrijvende en onvoldoende brongetrouwe diagnose. De productieprompt blijft staan.

## Opzet

De huidige productiejudge was de baseline. Het harnas draaide de bestaande C#-methode met het actuele JSON-schema, de gewone Chat Completions-adapter en een verse agentsessie per aanroep. De modelalias was `gpt-5-mini`. De kandidaat voegde alleen twee alinea's aan de judgeprompt toe over expliciete bronvoorwaarden, de relatie tussen feiten en de begrenzing van diagnoses. Er is één kandidaat getest.

De vijf bevroren tuningparen gebruiken ongewijzigde v4-bronteksten. Ieder paar heeft een correct idee en één materiële mutatie. De mutaties isoleren causale attributie (005), onvoorwaardelijke Narrative-prioriteit (010), de overstap van museumbezoekers naar managers, een verkeerd afhaaltijdvak en een ongefundeerd commissiemotief bij de zaadbank. De nieuwe synthetische veerbootholdout bevat een correct idee en een idee dat de tijdelijke pierwijziging ten onrechte uitbreidt naar weekdagen. De kandidaat was al opgesteld toen deze bron werd gemaakt. Verwachte uitkomsten en grenzen zijn bevroren vóór de eerste live call. De selectie is vastgelegd vóór de eerste live holdoutcall. [Protocol](protocol.md) · [Tuningcases](tuning/cases.json) · [Holdoutcases](holdout/cases.json) · [Selectie](selection.md)

De v1–v4-rapporten dienen als herkomst van de foutpatronen. V1 en v2 zagen terugkerende fouten bij 005 en 010. V3 liet zien dat correcte verdicts alsnog onzuivere correcties konden opleveren. V4 had zes gemiste blokkades op zeven gegenereerde ideeën. De huidige scores betreffen gecontroleerde paren met vaste ideeën. [V1](../production-cli-acceptance-v1/report.md) · [V2](../production-cli-acceptance-v2/report.md) · [V3](../production-cli-acceptance-v3/report.md) · [V4](../production-cli-acceptance-v4/report.md)

## Verdicts

De positieve klasse is een materiële fout die moet worden geblokkeerd. Een false accept is een gemiste blokkade. Een false reject is een blokkade van een correct idee.

| Fase | Conditie | TP | FP | TN | FN | Precision | Recall |
| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: |
| Tuning | Baseline | 3 | 0 | 5 | 2 | 100% | 60% |
| Tuning | Kandidaat | 5 | 0 | 5 | 0 | 100% | 100% |
| Holdout | Baseline | 1 | 0 | 1 | 0 | 100% | 100% |
| Holdout | Kandidaat | 1 | 0 | 1 | 0 | 100% | 100% |
| Herhaling | Baseline | 1 | 0 | 0 | 1 | 100% | 50% |
| Herhaling | Kandidaat | 1 | 0 | 0 | 1 | 100% | 50% |

De baseline accepteerde zowel de Narrative-first-titel als de doelgroepverschuiving naar museumteams op de eerste meting. De kandidaat ving beide op. Bij herhaling blokkeerden beide prompts opnieuw de causale overclaim in 005. Bij 010 herhaalde de baseline zijn false accept en veranderde de kandidaat van blokkeren naar accepteren. De overeenstemming met het eerste verdict was 2/2 voor de baseline en 1/2 voor de kandidaat. Over tuning, holdout en herhaling samen waren er nul false rejects en nul schema- of uitvoerfouten. [Scores per geval](verification/scores.json)

## Diagnosekwaliteit

| Conditie | Blockers | Volledig brongetrouw | Niet-voorschrijvend | Beide |
| --- | ---: | ---: | ---: | ---: |
| Baseline | 5 | 5 | 5 | 5 |
| Kandidaat | 8 | 6 | 7 | 6 |

- **010.** De kandidaat wees in de eerste meting het juiste veld aan, maar vroeg om Narrative als meest frequente kandidaat voor ingrijpen te presenteren. De bron noemt Narrative de meest terugkerende plek waar drift zichtbaar werd. De interventiekeuze hangt af van het concrete geval en de daadwerkelijk aangeleverde Synthesis.
- **Museum.** De eerste blocker benoemde terecht dat een bezoekersbericht is omgebogen naar een managementevaluatie. Een tweede blocker gebruikte juiste tellingen, maar veronderstelde welke effectmetingen een managementbesluit zou vereisen. De bron geeft daarvoor geen besliscriteria.
- **Overige diagnoses.** De blockers begrensden de causale claim in 005, behielden het afhaaltijdvak en de weekdaguitzondering, of scheidden het zaadbankbesluit van het onbewezen motief. Geen verdict bevatte een correction-veld, alternatieve titel of nieuwe onderzoeksagenda. [Beoordeling per blocker](diagnoses.md)

## Besluit en vervolg

**No-go.** De lagere false-acceptscore op tuning houdt bij 010 geen stand. De kandidaat schendt bovendien de diagnosegate. De nieuwe holdout bevat één bron en één expliciete feitelijke tegenspraak. De herhalingen bestrijken twee negatieve gevallen. De opsteller beoordeelde de diagnoses zonder blindering. Deze opzet kan concrete tekortkomingen aantonen en geeft weinig zekerheid over de prevalentie ervan bij nieuwe ideeën.

Herhaal eerst judge-only met de oorspronkelijke gegenereerde v4-misses als regressiegevallen, aangevuld met ongeziene positieve en negatieve paren waarin de redactionele keuze subtiel verschuift. Herhaal vooral conditionele interventiekeuzes en laat een tweede beoordelaar de volledige diagnoses onafhankelijk scoren. Een volgende generatorproef kan wachten tot detectie en diagnosekwaliteit beide aan de gate voldoen.

## Reproductie en audit

- De productie- en harnasbuild hadden nul fouten en waarschuwingen. Alle 230 vindbare tests slaagden. Het Domain-project had geen vindbare tests. De 24 offline case/conditieprobes controleerden prompt- en invoertransport, het actuele schema en acceptatie of afwijzing van acht vaste verdictvarianten. [Build](verification/build.txt) · [Harnasbuild](verification/harness-build.txt) · [Tests](verification/tests.txt) · [Offline probes](runs/offline/)
- Er zijn 28 van 28 gereserveerde betaalde aanroepen uitgevoerd, met 28 geldige antwoorden. Alle antwoorden melden model-ID gpt-5-mini-2025-08-07. De SDK telde 66.216 invoertokens, waarvan 32.128 cached, en 22.366 uitvoertokens. Het logboek bevat de exacte argumenten, reserveer- en voltooiingstijd en exitcode. Per run zijn de verzonden bron/idee-JSON, onbewerkte verdicttekst en modelmetadata opgeslagen. [Aanroeplogboek](verification/call-ledger.jsonl) · [Runs](runs/) · [Integriteit](verification/final-integrity.json)
- Voor een nieuwe uitvoering vanuit dezelfde HEAD gebruikt een verse kopie van deze map op dezelfde directorydiepte de bevroren prompt- en casebestanden, de scripts en het protocol. Bouw het harnas en draai run.ps1 achtereenvolgens met offline en tuning. Beoordeel de diagnoses en leg de selectie vast in selection.md vóór holdout en repeat. Draai daarna score.ps1 en verify.ps1. De bestaande map bewaart het verbruikte callplafond en slaat al gereserveerde slots over. [Harnas](harness/Program.cs) · [Commando's](run.ps1) · [Hashes](verification/frozen.json)
- De getrackte werkboom komt overeen met HEAD. De baselinehash komt overeen met de productieprompt. De enige nieuwe map is dit experiment. Er zijn geen commits of pushes uitgevoerd.
