# Narrative Experiment 4 — NO-GO

**Behoud productieprompt A.** C behoudt de heldere structuur en lage tokenkosten van B, maar verbetert de redactionele focus niet en scoort lager op brontrouw. In de afzonderlijke LinkedIn-proef hadden drie C-posts een sterke opening, terwijl geen C-post volledig feitelijk juist was volgens de rubric. [Vooraf vastgelegd protocol](protocol.md) · [Exacte B→C-promptdiff](prompts.diff) · [Besliscriteria en gepaarde regressies](score-summary.json)

## Narrative

| Score 2 | A, productie | B, compacte basis | C, focusvariant |
|---|---:|---:|---:|
| Focus | 12/12 | 9/12 | 9/12 |
| Verhaalprogressie | 8/12 | 12/12 | 12/12 |
| Concrete details | 12/12 | 12/12 | 12/12 |
| Brontrouw | 4/12 | 9/12 | 8/12 |
| Formatagnostische handoff | 8/12 | 12/12 | 12/12 |
| **Volledig voldoende** | **3/12** | **6/12** | **6/12** |

| Volledig voldoende per case | A | B | C |
|---|---:|---:|---:|
| Webb | 0/3 | 3/3 | 1/3 |
| Wolf | 1/3 | 2/3 | 2/3 |
| Digital | 0/3 | 0/3 | 0/3 |
| Lowell | 2/3 | 1/3 | 3/3 |

- **Digital.** A houdt de ontwerpaanpak in alle drie de slotbeats centraal, maar herhaalt haar daar. B en C geven het slot telkens een eigen functie door alle drie de gebruikscijfers als uitkomst te presenteren. Dat verschuift de focus van gebruikersvolgorde en onderzoekstijd naar de resultaten. C r1 maakt bovendien van de beschreven aanpak de oorzaak van de snelle lancering met ‘door’, ondanks de eigen expliciete begrenzing. Dat is de gepaarde brontrouwdaling van 2→0 tegenover beide armen. A trekt in r2 en r3 juist de gelijktijdige test- en aanvraagactiviteiten uit elkaar. Het ontbrekende startpunt van de twaalf weken komt uit Synthesis. [C r1](calls/digital-r1-C/content.md) · [Synthesis](cases/digital/synthesis-result.json)
- **Webb.** De vouwreden, tweede week en zes maanden blijven in beeld. A legt in drie briefs en C in r1 en r2 de afkoeling na de voltooide uitvouwing. B laat de overlap open, ook waar r2 zegt dat nog enige afkoeling nodig was. C r3 dateert afkoeling in de eerste maand en laat overlap mogelijk. De Synthesis bundelt commissioning en vermeldt maand één. Die bundeling levert geen grond voor een strikte volgorde en de oorspronkelijke bron laat afkoeling tijdens de uitvouwing zien. [C r2](calls/webb-r2-C/content.md) · [Synthesis](cases/webb/synthesis-result.json) · [Bron](cases/webb/source.md)
- **Wolf.** De drie armen behouden roedels, voedselrelaties en de multifactoriële context. Alle drie r2-briefs verwisselen wapiti met eland. A doet dat ook in r1. Alle Engelstalige Wolf-briefs houden de Engelse bronterm *elk*. Taal was vrij en dit verband is beschrijvend. [C r2](calls/wolf-r2-C/content.md) · [Synthesis](cases/wolf/synthesis-result.json)
- **Lowell.** C houdt in alle drie de briefs hergebruik en bredere groei uit elkaar en eindigt bij de latere terugval. Twee B-briefs vullen het vertrek van fabrieken of bedrijven in als het vertrek van mensen. A r3 en B r3 schrijven hergebruik een stelliger bijdrage aan de economische opleving toe. De tekst uit 1992 geeft alleen een historisch perspectief. [C r3](calls/lowell-r3-C/content.md) · [B r3](calls/lowell-r3-B/content.md) · [Bron](cases/lowell/source.md)

C haalt drie vooraf vastgelegde onderdelen van de beslisregel, namelijk volledigheid van uitvoering, behoud van structuur/details/handoff en tokenverbruik. De kwaliteitsdrempels voor volledig voldoende briefs, focus, brontrouw, gepaarde regressies en downstream worden gemist. [Handmatige scores per brief](narrative-scores.csv) · [Alle gepaarde dalingen](score-summary.json)

## LinkedIn met productie-Compose

Per case en arm één post uit de vooraf gekozen ronde. De twaalf teksten zijn echte Compose-uitvoer en dienen als offline proefmateriaal. De tabel telt scores van 2 per as. [Per post met onderbouwing](linkedin-scores.csv)

| Score 2 | A | B | C |
|---|---:|---:|---:|
| Aantrekkelijkheid | 1/4 | 1/4 | 3/4 |
| Leesbaarheid | 4/4 | 3/4 | 3/4 |
| Centrale boodschap | 4/4 | 3/4 | 3/4 |
| Feitelijke juistheid | 1/4 | 1/4 | 0/4 |

- **Webb.** C opent scherper met het onderscheid tussen uitvouwen en wetenschap. A en C nemen de geforceerde afkoelvolgorde uit hun briefs mee. B maakt bij Compose van de volledige commissioningperiode ‘de volgende zes maanden’, wat na uitvouwing nog eens zes maanden suggereert, terwijl de brief de totale periode noemt. [A](calls/linkedin-webb-r1-A/content.md) · [B](calls/linkedin-webb-r1-B/content.md) · [C](calls/linkedin-webb-r1-C/content.md)
- **Wolf.** Alle posts nemen eland over uit hun r2-brief. Daardoor missen alle drie de feitelijke ondergrens ondanks een heldere boodschap. Het gepaarde resultaat zegt alleen iets over de vooraf gekozen ronde. [A](calls/linkedin-wolf-r2-A/content.md) · [B](calls/linkedin-wolf-r2-B/content.md) · [C](calls/linkedin-wolf-r2-C/content.md)
- **Digital.** A houdt de methode als centrale boodschap en erft de onjuiste afwisseling van werkzaamheden. B en C erven het cijfergedreven slot. B voegt bovendien een ontwikkelduur van twaalf weken toe waar de start open was. C maakt door de zinsbouw onduidelijk of het team de dienst testte of aanvragen testte. [A](calls/linkedin-digital-r3-A/content.md) · [B](calls/linkedin-digital-r3-B/content.md) · [C](calls/linkedin-digital-r3-C/content.md)
- **Lowell.** C opent met het hoofdcontrast en gebruikt een concreet detail over werkersonzekerheid. Compose maakt de regionale neergang en computerindustrie vervolgens sterker causaal met ‘speelden mee’. In de Narrative-brief staat de terugval naast die omstandigheden. A en B behouden de grenzen van de bredere opleving. [A](calls/linkedin-lowell-r1-A/content.md) · [B](calls/linkedin-lowell-r1-B/content.md) · [C](calls/linkedin-lowell-r1-C/content.md)

## Tokens en reproduceerbaarheid

| Stap | Arm | Calls | Input | Completion incl. reasoning | Totaal |
|---|---|---:|---:|---:|---:|
| Narrative | A | 12 | 84.552 | 7.410 | 91.962 |
| Narrative | B | 12 | 10.116 | 6.530 | 16.646 |
| Narrative | C | 12 | 10.236 | 6.677 | 16.913 |
| LinkedIn | A | 4 | 23.833 | 2.796 | 26.629 |
| LinkedIn | B | 4 | 23.948 | 3.632 | 27.580 |
| LinkedIn | C | 4 | 23.806 | 2.996 | 26.802 |

C gebruikt voor Narrative **81,6% minder totale tokens dan A** en **1,6% meer dan B**. LinkedIn-tokens zijn afzonderlijk gemeten voor vier posts per arm. [Providerusage per call](usage.json)

De gepushte basis was 217569e. Alle twintig fixturebestanden zijn byte-identiek aan die in Experimenten 1–3. A en Compose komen overeen met hun ingebedde productieprompts. B komt overeen met Experiment 3. Configuratie, criteria, prompts, runner en fixtures zijn vóór de eerste call vastgelegd en gehasht. Alle 48 requests leverden een ander response-ID op met het verwachte model en schema. Offline replay reconstrueerde alle 48 requests exact en produceerde dezelfde geparseerde resultaten. De oplossing en runner bouwden zonder waarschuwingen of fouten. Alle 303 gevonden tests slaagden. [Baselinecontrole](baseline-verification.txt) · [SHA-256](freeze.json) · [Live log](live.txt) · [Request- en usagecontrole](verification.txt) · [Replaylog](replay.txt) · [Build](build.txt) · [Tests](tests.txt)

Start, hervat, pauzeer of annuleer via de replay-instructies. Gebruik vanuit de repository `node "$x/verify-baseline.cjs"`, `node "$x/verify.cjs"` en `node "$x/score.cjs"` om fixturevergelijking, usage en scoreaggregatie opnieuw uit te voeren. [Reproduceren en afbreken](REPLAY.md)

De uitvoerende assistent beoordeelde handmatig met zicht op de armen en eerdere experimenten. Vooral grensgevallen in timing en causaliteit en de waardering van openingen en leesbaarheid zijn gevoelig voor beoordelaarskeuzes. De vier doelgericht gekozen fixtures, één Synthesis-uitvoer per case, vrije uitvoertaal en één downstream-post per case en arm beperken de conclusies. Er zijn geen onafhankelijke lezersscores of gebruiksmetingen.

## Vervolg

Behoud A. Test desgewenst één scherpere slotregel op de compacte basis die van de endpoint een inzicht over de gekozen aanpak maakt wanneer die de Selected Idea draagt. Leg vooraf een aparte toets vast voor exacte entiteiten, open timing en begrensde causaliteit, met geblindeerde beoordeling en enkele nieuwe cases. Neem Digital op als regressiecase en toets daarnaast expliciet Nederlandse soortnamen. Houd Compose constant om doorwerking te kunnen onderscheiden van nieuwe schrijffouten.

De werkboom bevat alleen de nieuwe experimentmap. Een volgende productiewijziging of commit/push vraagt afzonderlijke toestemming.
