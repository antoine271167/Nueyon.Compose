# Narrative Experiment 9 — Editorial Blueprint

**NO-GO.** H leverde vaker een bruikbare redactionele richting, maar was brongetrouw in 7 van 12 briefs. Productie-Compose voegde bij beide toegelaten Webb-briefs opnieuw een ongegronde tijdrelatie toe. Slechts twee paren leverden aan beide kanten een brongetrouwe LinkedIn-post op, beide over Digital. [Vooraf vastgelegd protocol](protocol.md) · [Exacte F→H-diff](prompts.diff) · [Beslissing en dekking](decision.json)

| Maatstaf | F | H |
|---|---:|---:|
| Narrative afgerond | 12/12 | 12/12 |
| Centrale boodschap | 12/12 | 12/12 |
| Narratieve voortgang | 11/12 | 12/12 |
| Concreet bewijs | 10/12 | 9/12 |
| Redactionele richting | 8/12 | 11/12 |
| Handoff bruikbaar | 8/12 | 9/12 |
| Brongetrouw | 6/12 | 7/12 |
| Alles voldoende | 1/12 | 6/12 |

Een voldoende is een score van 2. H haalde per case respectievelijk 1, 2, 0 en 3 volledig voldoende briefs (Digital, Webb, Wolf, Lowell). Dat mist zowel de eis van minstens 10/12 als de ondergrens per case. [Narrative-scores met onderbouwing](narrative-scores.json)

- **Digital.** H r3 schreef dat het team in twaalf weken lanceerde «door» de beschreven aanpak. Synthesis documenteert de werkwijze en uitkomsten, zonder dat causale verband. F en H hielden dit verband in de andere vijf briefs achterwege. De twee gepaarde tests laten zien dat Compose het sprintritme kan gebruiken. Het H-slot rondt in r2 de gekozen werkwijze beter af, terwijl F daar eindigt bij de cijfers. [H-brief r3](calls/digital-r3-H/content.md) · [F-post r2](calls/linkedin-digital-r2-F/content.md) · [H-post r2](calls/linkedin-digital-r2-H/content.md)
- **Webb.** Alle drie F-briefs en H r3 plaatsten afkoeling na de voltooide uitvouwing. H r1 en r2 hielden week twee en maand één gescheiden. In beide H-posts schreef Compose na de spiegelontplooiing «Daarna volgde de ingebruikname» en rekende daar afkoeling toe. De overgang introduceerde zo de volgorde die de brief openliet. De bron plaatst het begin van afkoeling vóór de afronding van de uitvouwing. [Synthesis](cases/webb/synthesis-result.json) · [Bron](cases/webb/source.md) · [H-brief r1](calls/webb-r1-H/content.md) · [H-post r1](calls/linkedin-webb-r1-H/content.md) · [H-post r2](calls/linkedin-webb-r2-H/content.md)
- **Wolf.** Alle drie H-briefs en F r2 vertaalden Yellowstone elk als eland. Het betreft wapiti. H r2 noemde daarnaast karkassen van wolven waar Synthesis door wolven gedode prooi bedoelt. De eerste handmatige controle miste de vertaalfout, waardoor H r1 en F r2 toch bij Compose terechtkwamen. Beide posts namen de fout over. Na ontdekking is de reeks gepauzeerd, zijn de scores gecorrigeerd en is de nog niet uitgevoerde H r3-handoff geblokkeerd. De twee uitgevoerde onveilige handoffs tellen mee in resultaten en kosten. [Oorspronkelijke poort](narrative-scores-initial-gate.json) · [Bron voor soortidentiteit](https://www.nps.gov/yell/learn/nature/elk.htm)
- **Lowell.** Alle H-briefs behielden de terugval, het perspectief van 1992 en de begrenzing van causaliteit. Twee F-briefs maakten van regionale en sectorale samenloop een oorzaak van het einde van de hausse. De H-posts hadden op hun beurt nieuwe problemen rond gebouwsoort, verklarende factoren of de vertaling van textielfabrieken. [Narrative-scores](narrative-scores.json) · [LinkedIn-scores](linkedin-scores.json)

## LinkedIn via productie-Compose

| Voldoende per criterium | F (7 posts) | H (8 posts) |
|---|---:|---:|
| Aantrekkelijkheid | 5 | 7 |
| Leesbaarheid | 5 | 5 |
| Centrale boodschap | 7 | 8 |
| Onderbouwde afsluiting | 2 | 3 |
| Handoff gerealiseerd | 1 | 2 |
| Feitelijke juistheid | 5 | 2 |
| Alles voldoende | 0 | 0 |

Er waren 13 achteraf brongetrouwe Narrative-handoffs, twee onveilige handoffs uitgevoerd vóór de correctie, en negen geblokkeerde calls. Alle 13 toegelaten calls voltooiden, ook bij onvoldoende zachte scores. Digital r1/r2, Wolf r1 en Lowell r3 hadden twee gegenereerde posts. Wolf r1 en Lowell r3 vielen uit de brongetrouwe paarvergelijking door een H-bronfout. De enkelzijdig uitgevoerde posts waren Digital r3 F, Webb r1/r2 H, Wolf r2/r3 F en Lowell r1/r2 H. De volledige scores en poortstatus staan per run in de bijlagen.

Binnen de twee geldige Digital-paren ging de som aantrekkelijkheid plus slot van 5 naar 6 en de som van de vijf zachte criteria van 15 naar 18. Paartotalen over beide stappen gingen van 17.356 naar 17.945 tokens (+3,4%) en van $0,002913 naar $0,003002 (+3,1%). De vooraf vereiste zes veilige paren over drie cases zijn niet gehaald. De kleine vergelijking ondersteunt geen algemene conclusie over LinkedIn-kwaliteit. [Scores](linkedin-scores.json) · [Gepaarde cijfers en uitsluitingen](decision.json)

## Gebruik en verificatie

| Stap en arm | Calls | Input | Waarvan cache | Output | Totaal | USD geschat |
|---|---:|---:|---:|---:|---:|---:|
| Narrative F | 12 | 10.524 | 0 | 6.842 | 17.366 | 0,00447340 |
| Narrative H | 12 | 9.528 | 0 | 7.045 | 16.573 | 0,00447530 |
| LinkedIn F | 7 | 42.174 | 11.700 | 6.143 | 48.317 | 0,00623590 |
| LinkedIn H | 8 | 48.384 | 15.600 | 7.858 | 56.242 | 0,00736340 |

Totaal 138.498 tokens en naar schatting $0,022548. Narrative H gebruikte 4,6% minder tokens, met vrijwel gelijke geschatte kosten door meer outputtokens. De LinkedIn-arm-totalen hebben verschillende dekking. Schattingen volgen de Standard-tarieven en gemeten cachetokens. De factuur is niet ingezien. [Verbruik per call](usage-costs.json) · [OpenAI GPT-6 Luna-tarieven](https://developers.openai.com/api/docs/models/gpt-6-luna)

Alle 39 responses hadden unieke ID’s. Offline replay reproduceerde de exacte requestbodies en geparseerde outputs. Beide builds slaagden zonder fouten of waarschuwingen en alle 303 ontdekte tests slaagden. Productieprompts en productiecode bleven byte-identiek. [Verificatie](verification.txt) · [Narrative replay](replay-narrative.txt) · [LinkedIn replay](replay-linkedin.txt) · [Build](build.txt) · [Runnerbuild](runner-build.txt) · [Tests](tests.txt)

De uitvoerende assistent beoordeelde met zicht op variantlabels. De vier doelgericht gekozen fixtures, drie herhalingen en wisselend Nederlands/Engels beperken de generaliseerbaarheid. Het protocol bevriesde de tijd- en causaliteitschecks, maar de fout bij Wolf toont dat een algemene brontrouwregel bij handmatige beoordeling onvoldoende kan zijn voor vertaalde entiteiten. De term «fabrieksgebouwen» in Lowell r1 H is als mogelijk materiële versmalling gescoord. Zelfs als die post voldoende brongetrouw zou worden beoordeeld, blijven de harde drempels ruim buiten bereik. De proef is afgerond zonder productiepromotie.
