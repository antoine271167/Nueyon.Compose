# Narrative Experiment 3 — NO-GO

**Behoud de opgeschoonde productieprompts van A.** B haalt 2/12 volledig voldoende tegenover 4/12 voor A. B maakt de verhaallijn en de format-agnostische handoff consequenter en gebruikt 81,7% minder totale tokens. B verliest op focus en brontrouw en introduceert in Wolf r2 een soortfout tegenover een brontrouwe A. De vooraf vastgelegde kwaliteitsdrempels worden daarmee niet gehaald. [Protocol](protocol.md) · [Scores per brief en paar](scores.md)

| Fixture | A volledig voldoende | B volledig voldoende |
|---|---:|---:|
| Webb | 1/3 | 0/3 |
| Wolf | 1/3 | 2/3 |
| Digital | 0/3 | 0/3 |
| Lowell | 2/3 | 0/3 |
| **Totaal** | **4/12** | **2/12** |

| As | A score 2 | B score 2 |
|---|---:|---:|
| Verhaallijn | 10/12 | 12/12 |
| Focus | 11/12 | 8/12 |
| Concrete details | 12/12 | 12/12 |
| Brontrouw | 6/12 | 5/12 |
| Compose-handoff | 10/12 | 12/12 |

B mist het minimum van 10/12 volledig voldoende en de vereiste verbetering van minstens twee runs. Alleen Wolf haalt voor B het minimum van 2/3 per fixture. B haalt 5/12 op brontrouw tegenover de vereiste 12/12 en behoudt de focusscore van A niet. Wolf r2 schendt ook de gepaarde grens met een daling van 2 naar 0 op brontrouw. De overige gepaarde dalingen staan in de scoretabel.

## Brontrouw en upstream

- **Webb.** De vouwreden, de uitvouwing in week twee en routinewetenschap na zes maanden blijven herkenbaar. Synthesis bundelt de commissioning en dateert afkoeling in maand één. De oorspronkelijke bron laat zien dat het afkoelen tijdens de uitvouwing begon. In r1 en r2 maken beide armen daar een strikte volgorde van. A r3 laat overlap open. B r3 noemt in de samenvatting ‘eerste wetenschap’ en trekt de conclusie breder dan Webb. Het woord ‘operational’ voor de uitgevouwen configuratie en het losse slotwoord komen uit upstream en leidden nergens tot een verzonnen mislukking of herstelverhaal. [Synthesis](cases/webb/synthesis-result.json) · [Bron](cases/webb/source.md)
- **Wolf.** Synthesis en de Research-projectie gebruiken het Engelse *elk*, hier wapiti. A verwisselt de soort in r1 en r3, B in r2. Iedere Engelstalige brief behoudt *elk* en iedere Nederlandstalige Wolf-brief noemt eland. De taal varieerde per run en was geen experimentele variabele. Beide armen bieden concrete roedel-, aaseter- en prooidetails en behouden de grens rond een ecosysteembrede cascade. [Synthesis](cases/wolf/synthesis-result.json) · [Projectie](cases/wolf/projection.md)
- **Digital.** De afwisseling tussen ‘na’ en ‘in’ twaalf weken en het ontbrekende startpunt zitten al in Synthesis. A r3 maakt van gelijktijdig onderzoeken, testen en aanvragen behandelen een afwisseling. B behoudt die sprintindeling in alle drie briefs en geeft telkens een afzonderlijk endpoint. Dat endpoint draait bij B steeds om gebruikscijfers en verschuift de focus weg van de aanpak onder tijdsdruk. A r2 maakt dezelfde verschuiving. A r1 en r3 eindigen met een herhaling van de aanpak. Beide armen respecteren de causale grens tussen aanpak, lancering en cijfers. [Synthesis](cases/digital/synthesis-result.json) · [Bron](cases/digital/source.md)
- **Lowell.** Alle zes briefs behouden de lijn van textielverval via burgerinitiatief en hergebruik naar de latere terugval. A r1 en B r1/r3 maken de bijdrage van de omslag aan hernieuwde activiteit stelliger. Bovendien verschuift B r2/r3 het vertrek van bedrijven naar het vertrek van mensen. De upstream-formulering ‘mill closures and departures’ is beknopt, maar de bron wijst bedrijven aan als vertrekkende actoren. De historische grens blijft in beide armen beschikbaar voor Compose. [Synthesis](cases/lowell/synthesis-result.json) · [Bron](cases/lowell/source.md)

Alle briefs bieden concrete details. De duidelijker gescheiden beats verklaren de hogere handoff- en structuurcijfers van B, vooral bij Digital. De handoffscore betreft uitsluitend het briefmateriaal voor Article en LinkedIn. Deze proef bevatte geen Compose-uitvoer, Judge-loop of nieuwe upstream-uitvoer.

## Uitvoering en tokens

| Arm | Calls | Inputtokens | Completiontokens incl. reasoning | Totaal |
|---|---:|---:|---:|---:|
| A | 12 | 84.552 | 8.125 | 92.677 |
| B | 12 | 10.116 | 6.861 | 16.977 |
| **Totaal** | **24** | **94.668** | **14.986** | **109.654** |

B gebruikte 88,0% minder inputtokens en 15,6% minder completiontokens. De provider-usage bevat ook de cachegegevens per run. Er is geen kostenraming op basis van tarieven opgesteld. [Usage per run](usage.json)

De 20 fixture- en referentiebestanden zijn byte-identiek aan die uit Experiment 1 en 2. De oplossing en runner bouwden met nul waarschuwingen en nul fouten. Alle 303 gevonden tests slaagden. De 24 onafhankelijke GPT-6 Luna-aanvragen hadden verschillende response-ID's. De gespiegelde aanvragen gebruikten dezelfde invoer, modelinstellingen en het productieschema. Alle 24 responses zijn offline identiek geparseerd via de productieagent. [Verificatie](verification.txt) · [Build](build.txt) · [Tests](tests.txt) · [Replay](REPLAY.md)

De uitvoerende assistent beoordeelde alle briefs handmatig met zicht op de arm. Het gaat om vier doelgericht gekozen fixtures met elk drie herhalingen per arm. De meting betreft de compacte herschrijving als geheel en legt geen afzonderlijke bijdrage per promptwijziging vast.

## Gewijzigde bestanden

De productieopschoning beperkt zich tot de twee Narrative-prompts. De aanvullende brontrouwregel uit Experiment 2 zit niet in A. B blijft in de experimentmap. De wijziging is lokaal gebleven, zonder commit of push. [Productiediff](production.diff) · [A/B-diff](prompts.diff) · [Volledige bestandslijst](files.txt)
