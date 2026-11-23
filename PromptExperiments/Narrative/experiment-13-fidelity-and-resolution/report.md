# Experiment 13 — Narrative Fidelity & Editorial Resolution

**NO-GO.** De kandidaat blijft buiten productie. Op de vier regressieonderwerpen zijn beide armen 3/12 volledig voldoende. De kandidaat haalt 10/18 brongebonden afrondingen tegenover 8/18 voor productie, onder de vooraf vereiste 15/18. Vijf kandidaatbriefs bevatten een materiële bronfout. De handoff daalt van 14/18 naar 13/18. De build slaagde zonder fouten of waarschuwingen. Alle 303 ontdekte tests slaagden. Zie test.log.

| Onderwerp | Volledig A / B | Brontrouw A / B | Afronding A / B | Materiële fout A / B |
|---|---:|---:|---:|---:|
| Digital | 0 / 0 | 3 / 3 | 0 / 0 | 0 / 0 |
| Webb | 1 / 0 | 1 / 0 | 3 / 3 | 2 / 3 |
| Wolf | 0 / 1 | 1 / 1 | 0 / 1 | 2 / 2 |
| Lowell | 2 / 2 | 3 / 2 | 2 / 3 | 0 / 0 |
| Cooling | 1 / 1 | 3 / 3 | 1 / 1 | 0 / 0 |
| Library | 1 / 2 | 2 / 3 | 2 / 2 | 0 / 0 |
| Alle 18 slots | 5 / 6 | 13 / 12 | 8 / 10 | 4 / 5 |

Elke rij vóór de totaaltelling telt drie briefs per arm. Focus, structuur, spanningsboog, details en formatneutraliteit voldoen in beide armen op 18/18. Voor de handoff telt de noodzaak om een materiële fout te herstellen als tekort. De vijf assen van eerdere studies zijn niet direct vergelijkbaar met de huidige acht assen. Experiment 1 en 3 laten hetzelfde soort- en chronologieprobleem zien. Experiment 11 en de hervatting 11b bekeken vooral Compose. Experiment 12 hielp de fouten lokaliseren en motiveerde de huidige proef als Narrative-only.

## Brontrouw en afronding

- **Webb.** Productie heeft één bruikbare formulering met afkoeling tijdens de ingebruikname. De drie kandidaatbriefs leggen de start van afkoeling na voltooide uitvouwing. Synthesis noemt afkoeling in maand één en volledige uitvouwing in week twee. De oorspronkelijke bron vermeldt temperatuurveranderingen tijdens de ontplooiing van het zonneschild op dagen 3–10. Het strikte “daarna” is door Narrative toegevoegd, terwijl het bevroren upstream materiaal de overlap slechts indirect uitdrukt. De zesmaandsmijlpaal blijft een inhoudelijk passend eindpunt.
- **Wolf.** In beide armen staat in twee briefs “eland” voor elk. In één kandidaatbrief maakt “wolvendoding” bovendien de rolverdeling ambigu. Een kandidaatbrief die de Engelse soortterm behoudt, gebruikt de omkering van de vroege-winterprooi om het patroon van veranderende predatie zichtbaar te maken. De andere eindpunten blijven bij de algemene boodschap over een netwerk.
- **Digital.** Alle zes briefs vermijden de causale toeschrijving uit de Selected Idea. Een kandidaatbrief geeft de tijdelijke inzendingspauze en voortgezet onderzoek concreet weer in Development en verschuift in het slot naar de vijfweekscijfers. Een andere kandidaatbrief herhaalt de aanpak. De drie cijfermatige eindpunten leveren alleen een gerapporteerde momentopname naast de centrale lijn.
- **Lowell.** De kandidaat eindigt alle drie keren met de neergang als grens aan het eerdere herstel en houdt in de eindpunten de economische samenloop intact. Eén centrale boodschap suggereert ten onrechte dat de burgerinitiatieven en latere herbestemming samenvielen. Een productiebrief beperkt zich tot een recapitulatie.
- **Nieuwe cases.** De meetgrenzen bij Cooling bleven in alle zes briefs behouden. Vier eindpunten verloren aandacht voor het specifieke vergelijkingsresultaat. Library B behoudt steeds de telbasis; Library A heeft één dubbelzinnige bijzin over het totaal aantal unieke leners. De langere zaterdagopening geeft de sterkste eindpunten hun inhoudelijke grens.

## Drempels en vervolg

Dekking en formatneutraliteit slagen. Bronfouten, het minimum voor afronding, het minimum op de vaste reeks, het minimum per vast onderwerp, verbetering in volledig voldoende vaste briefs en geen verlies op handoff falen. De winst van twee afrondingen haalt alleen de relatieve afrondingsdrempel. De kleine steekproef, drie herhalingen per cel, synthetische nieuwe cases, één beoordelaar en beperkte blindering maken verschillen van één of twee briefs onzeker. Er zijn geen geïsoleerde tests van de twee gewijzigde passages. De CLI legt geen ruwe provider-envelope of tokenverbruik vast. De opgeslagen JSON is de ongewijzigde CLI-resultaatpayload, aangevuld met logs, exitcodes en de armtoewijzing.

**Volgende hypothese.** Laat Narrative bij het formuleren van de centrale boodschap en het eindpunt de dragende bewijszin uit Synthesis als semantisch anker gebruiken inclusief de oorspronkelijke referenten en tijdsbepalingen. Verwachting: minder relationele fouten en een inhoudelijk slot dat voortbouwt op precies dat bewijs. Houd de upstream input in deze proef bevroren om het Narrative-effect toetsbaar te houden.

[Protocol](protocol.md) · [Manifest](run-manifest.csv) · [Scores](scores.csv) · [Citaatbewijs](scoring-notes.md)
