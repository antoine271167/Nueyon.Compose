# Baseline 002 — Idea Generator

**Afgewezen.** Alle 15 runs zijn uitgevoerd en beoordeeld in manifestvolgorde. Eén run slaagde, tegenover de vereiste 14. Vier runs bevatten in totaal zes ernstige brontrouwfouten. Er zijn geen ontbrekende run-ID’s.

| Bron | Uitgevoerd | Idea Quality | Downstream Viability | Gecombineerd | Runs met ernstige fout |
|---|---:|---:|---:|---:|---:|
| Productstrategie | 3/3 | 0/3 | 2/3 | 0/3 | 0 |
| Technisch ontwikkelverslag | 3/3 | 1/3 | 3/3 | 1/3 | 1 |
| Productdocumentatie | 3/3 | 0/3 | 0/3 | 0/3 | 0 |
| Diagnostische analyse | 3/3 | 0/3 | 2/3 | 0/3 | 1 |
| Historische rede | 3/3 | 0/3 | 0/3 | 0/3 | 2 |
| **Totaal** | **15/15** | **1/15** | **7/15** | **1/15** | **4** |

De Idea Quality-kolom vereist 2/2 voor brontrouw en redactioneel doel en minstens 8/10 totaal. Downstream Viability beoordeelt de Narrative ten opzichte van het geselecteerde Idea en de bevroren bron. In zes runs hield de Narrative een bruikbare kern vast of bracht zij een overclaim terug tot wat de bron draagt, terwijl het Idea de gates miste.

## Beoordeling in manifestvolgorde

| Run | Brontrouw | Doel | Focus | Invalshoek | Aantrekkingskracht | Totaal | Idea | Narrative | Ernstige fout | Run |
|---|---:|---:|---:|---:|---:|---:|---|---|---|---|
| [01-strategy-r1](runs/01-strategy-r1/evaluation.json) | 1 | 1 | 2 | 1 | 1 | 6/10 | Nee | Ja | Nee | Nee |
| [02-development-r1](runs/02-development-r1/evaluation.json) | 0 | 2 | 2 | 2 | 2 | 8/10 | Nee | Ja | Ja | Nee |
| [03-product-docs-r1](runs/03-product-docs-r1/evaluation.json) | 1 | 1 | 2 | 2 | 2 | 8/10 | Nee | Nee | Nee | Nee |
| [04-analysis-r1](runs/04-analysis-r1/evaluation.json) | 1 | 2 | 2 | 2 | 2 | 9/10 | Nee | Ja | Nee | Nee |
| [05-general-r1](runs/05-general-r1/evaluation.json) | 0 | 0 | 2 | 2 | 1 | 5/10 | Nee | Nee | Ja | Nee |
| [01-strategy-r2](runs/01-strategy-r2/evaluation.json) | 1 | 1 | 2 | 1 | 1 | 6/10 | Nee | Ja | Nee | Nee |
| [02-development-r2](runs/02-development-r2/evaluation.json) | 1 | 2 | 2 | 2 | 2 | 9/10 | Nee | Ja | Nee | Nee |
| [03-product-docs-r2](runs/03-product-docs-r2/evaluation.json) | 1 | 1 | 2 | 2 | 2 | 8/10 | Nee | Nee | Nee | Nee |
| [04-analysis-r2](runs/04-analysis-r2/evaluation.json) | 0 | 1 | 2 | 2 | 1 | 6/10 | Nee | Ja | Ja | Nee |
| [05-general-r2](runs/05-general-r2/evaluation.json) | 1 | 0 | 2 | 2 | 2 | 7/10 | Nee | Nee | Nee | Nee |
| [01-strategy-r3](runs/01-strategy-r3/evaluation.json) | 1 | 1 | 2 | 1 | 1 | 6/10 | Nee | Nee | Nee | Nee |
| [02-development-r3](runs/02-development-r3/evaluation.json) | 2 | 2 | 2 | 2 | 2 | 10/10 | Ja | Ja | Nee | Ja |
| [03-product-docs-r3](runs/03-product-docs-r3/evaluation.json) | 1 | 2 | 2 | 2 | 2 | 9/10 | Nee | Nee | Nee | Nee |
| [04-analysis-r3](runs/04-analysis-r3/evaluation.json) | 1 | 1 | 2 | 2 | 2 | 8/10 | Nee | Nee | Nee | Nee |
| [05-general-r3](runs/05-general-r3/evaluation.json) | 0 | 0 | 2 | 1 | 0 | 3/10 | Nee | Nee | Ja | Nee |

## Ernstige brontrouwfouten

| Run | Waar zichtbaar | Afwijking | Bron |
|---|---|---|---|
| 02-development-r1 | Idea | De titel maakt van een geobserveerde regressie een stellig causaal resultaat. De historische vergelijking sluit variatie tussen de afzonderlijk uitgevoerde condities niet uit. | [Vergelijking en beperkingen, regels 19–25 en 96–100](../acceptance-v1/sources/02-development/experiment-005-report.md#L96) |
| 04-analysis-r2 | Idea | De sterkste interventie aanwijzen is de dragende aanbeveling. De analyse vergeleek geen interventies en lokaliseert alleen de eerste zichtbare afwijking. | [Methode en volgende proef, regels 25–29 en 88–95](../acceptance-v1/sources/04-analysis/experiment-010-report.md#L88) |
| 05-general-r1 | Idea en Narrative | Idea maakt licentiebehoud algemeen. Narrative herkent de uitzondering deels, maar stelt behoud van de licentie en het tonen van de verplichte zin ten onrechte als alternatieven voor. | [Licentie §1.C en §1.E, regels 137–171 en 200–208](../acceptance-v1/sources/05-general/pg4.md#L137) |
| 05-general-r3 | Idea en Narrative | Idea laat de uitzondering bij het verwijderen van alle Gutenberg-verwijzingen weg. Narrative verandert die uitzondering in een vermeende toestemmingsplicht. | [Licentie §1.C en §1.E, regels 137–171 en 260–265](../acceptance-v1/sources/05-general/pg4.md#L137) |

## Bevindingen per bron

- **Productstrategie.** Alle drie Ideas beginnen bij Nuëyon en gebruiken Compose als eerste productrichting. Alle drie Audience-velden voegen ook een bredere LinkedIn-lezersgroep toe. In run 3 voegt Narrative een eigen hiërarchieafwijking toe: het slot presenteert orkestratie als de onmiddellijke richting van Nuëyon, terwijl de briefing dit als ondersteunende ontwerpkeuze bij Compose behandelt. [Bron, §3 en §12–13, regels 34–44 en 207–241](../acceptance-v1/sources/01-strategy/Noeyon-Product-Strategy-reconstructed.md#L34), [beoordeling run 3](runs/01-strategy-r3/evaluation.json).
- **Ontwikkelverslag.** Run 3 slaagt met 10/10 voor Idea en een geslaagde Narrative. De ontwikkeling van de ene promptregel, de scoreverschillen en het besluit tot rollback dragen de les. Run 1 gebruikt in de titel stellig causaal taalgebruik. De Narrative van die run herstelt de beperking tot geobserveerde samenhang. [Bron, vergelijking, resultaten en beperkingen, regels 19–32, 48–72 en 86–100](../acceptance-v1/sources/02-development/experiment-005-report.md#L19), [beoordeling run 3](runs/02-development-r3/evaluation.json).
- **Productdocumentatie.** Alle drie Ideas schrijven Homebrew-updates toe aan de GitHub CLI-deploymentworkflow. De gids koppelt die workflow aan Debian en RPM en noemt bij Homebrew `homebrew-core`. In run 3 schrijft Narrative ook aan precompiled binaries een onderhouds- en updatepad toe dat de gids daar niet vermeldt. [Bron, Recommended (Official), regels 3–13, 36–47 en 120–140](../acceptance-v1/sources/03-product-docs/install_linux.md#L120), [beoordeling run 3](runs/03-product-docs-r3/evaluation.json).
- **Diagnostische analyse.** De drie Ideas herkennen het terugkerende eerste zichtbare breukpunt. Run 2 presenteert ingrijpen bij Narrative als de sterkste maatregel. De analyse stelt een volgende proef voor en heeft geen vergelijkende interventiedata. De Narrative van run 2 bewaart dat onderscheid. Run 3 verwart in Narrative een eerste feitelijke breuk met een hiërarchieverschuiving bij 008 A-002. [Bron, Method, per-run analyse en volgende proef, regels 25–29, 41–50 en 88–95](../acceptance-v1/sources/04-analysis/experiment-010-report.md#L25), [beoordeling run 2](runs/04-analysis-r2/evaluation.json).
- **Historische rede.** Alle drie Ideas geven voorrang aan de Gutenberg-licentie. Het manifest vraagt om een gedachtegang uit de rede voor algemeen publiek. Een mogelijk brongebonden verband is de overgang van het inwijden van de grond naar de opdracht aan de levenden. De licentie bleef integraal in het bestand zoals voorgeschreven. [Bron, Gettysburg Address, regels 38–68](../acceptance-v1/sources/05-general/pg4.md#L38), [beoordeling run 1](runs/05-general-r1/evaluation.json).

## Artefacten

[Uitvoeringsgegevens](execution-notes.md) beschrijft de gebruikte conditie, modelalias, Debug-build en debugger. [Preflight](preflight.json) en [promptresources](resources.json) bevatten de hashcontroles. [Samenvatting](summary.json) bevat de status, scores en integriteitscontroles per run. Onder elk run-ID staan de vastgelegde Idea, geselecteerde Idea, Narrative, console-uitvoer, runtimeconfiguratie, exacte invoer en beoordeling. De consoletekst bevat op sommige plaatsen Windows-tekenvervanging. De volledige Idea en Narrative zijn beoordeeld op de Unicode-uitvoer van de debugger.

## Eindcontrole

Alle vijftien gebruikte invoerkopieën, werkmapconfiguraties en volledige Idea- en Narrative-opnamen doorstonden de artefactcontrole. De scan van 173 tekstbestanden vond geen waarden van de geconfigureerde API-sleutel of sleutels in het verwachte tokenformaat. HEAD en GitHub main bleven op 9738698534e337610091812f02e83aa8d5db7a79. Git meldde alleen de nieuwe Baseline-002-map. Er is niets gecommit of gepusht.

