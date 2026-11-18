# Narrative Experiment 5 — Focus Lock — NO-GO

Variant D haalt 9/12 op focus, gelijk aan B en onder de vooraf vastgelegde grens van 11/12. Digital eindigt in alle zes briefs bij gebruikscijfers. D verliest op brontrouw en krijgt in de LinkedIn-proef 2/4 volledig feitelijk juiste posts bij een vereiste 4/4. [Protocol](protocol.md) · [Exacte promptdiff](prompts.diff) · [Besliscriteria](score-summary.json)

## Narrative

| Aantal met score 2 | B | D |
|---|---:|---:|
| Focus | 9/12 | 9/12 |
| Progressie | 9/12 | 9/12 |
| Concrete details | 12/12 | 11/12 |
| Brontrouw | 9/12 | 5/12 |
| Compose-handoff | 12/12 | 11/12 |
| Volledig voldoende | 6/12 | 3/12 |

Volledig voldoende per case (B/D) is Webb 2/0, Wolf 3/2, Digital 0/0 en Lowell 1/1. Deze proef telt een verschuiving naar een secundair onderwerp ook als onvolledige progressie. Dat verklaart waarom de actuele B hier 9/12 haalt terwijl Experiment 4 voor B 12/12 rapporteerde. De resultaten vergelijken de twee armen binnen deze proef. [Scores per brief](narrative-scores.csv)

- **Digital.** Alle drie D-briefs formuleren de gekozen werkwijze in Central Message en Development. Alle drie laten Endpoint draaien om aantallen en percentages. D r1 verliest bovendien de concrete invulling van beide sprintweken. In r2 introduceert Central Message met ‘door’ een oorzakelijk verband tussen de werkwijze en lancering in twaalf weken. De eigen beperkingen spreken dat tegen. De twaalf weken komen uit Synthesis, dat het precieze startpunt openlaat. [D r1](calls/digital-r1-D/content.md) · [D r2](calls/digital-r2-D/content.md) · [D r3](calls/digital-r3-D/content.md) · [Synthesis](cases/digital/synthesis-result.json)
- **Webb.** Alle zes briefs leggen uit waarom Webb gevouwen moest worden. Alle drie D-briefs plaatsen afkoeling na volledige uitvouwing, tegenover één B-brief. De bron laat zien dat het afkoelen tijdens de uitvouwing al begon. In r1 en r3 daalt D daardoor gepaard van 2 naar 0 op brontrouw. D r3 zet de onjuiste volgorde zowel in Central Message als in Endpoint. [D r3](calls/webb-r3-D/content.md) · [Bron](cases/webb/source.md)
- **Wolf.** Alle zes briefs behouden het Engelse elk, hier wapiti. De twee LinkedIn-posts doen dat ook. De runs toetsen daardoor niet of een Nederlandse formulering de soort zou behouden. D r1 krijgt een lichte aftrek omdat ‘sustain’ meer afhankelijkheid suggereert dan het onderbouwde voedselaanbod. De begrenzing rond ecosysteemeffecten blijft behouden. [D r1](calls/wolf-r1-D/content.md) · [Synthesis](cases/wolf/synthesis-result.json)
- **Lowell.** D r2 schrijft erfgoedhergebruik een causale bijdrage aan de bredere opleving toe, terwijl Synthesis hergebruik en groei naast elkaar beschrijft. De gepaarde B scoort eveneens 0 door een andere causale claim, over de neergang. In beide r3-briefs verandert het vertrek van bedrijven in het vertrek van mensen. De historische scope blijft herkenbaar. [D r2](calls/lowell-r2-D/content.md) · [B r2](calls/lowell-r2-B/content.md) · [Bron](cases/lowell/source.md)

Drie gepaarde Narrative-scores dalen van 2 naar 0, alle op brontrouw. De overige dalingen betreffen brontrouw bij Wolf r1 en details en handoff bij Digital. [Gepaarde scores](score-summary.json)

## LinkedIn met productie-Compose

Eén vooraf gekozen paar per case. Compose kreeg de volledige brief via het bestaande contract. D’s Central Message bleef een intern onderdeel van de brief en verscheen nergens als label in de gegenereerde posts.

| Aantal met score 2 | B | D |
|---|---:|---:|
| Aantrekkelijkheid | 2/4 | 2/4 |
| Leesbaarheid | 4/4 | 3/4 |
| Centrale boodschap | 3/4 | 3/4 |
| Feitelijke juistheid | 1/4 | 2/4 |

- **Digital.** Beide posts sluiten af met aantallen en percentages. D beschrijft de gelijktijdige beta-activiteiten correct, maar heeft een los afsluitend aanhalingsteken. B schrijft de uiteindelijke dienstkanalen in de openingszin al toe aan de oorspronkelijke opdracht. [B](calls/linkedin-digital-r3-B/content.md) · [D](calls/linkedin-digital-r3-D/content.md)
- **Webb.** De post van B maakt met ‘Then came’ en ‘still ahead’ de afkoelvolgorde stelliger dan de brief. De post van D noemt zes maanden als totale periode, maar de plaatsing van de afkoeling na de passage over volledige uitvouwing laat de volgorde onduidelijk. [B](calls/linkedin-webb-r1-B/content.md) · [D](calls/linkedin-webb-r1-D/content.md)
- **Wolf.** D zet het concrete verband tussen jacht en voedsel voor andere dieren direct in de opening. Beide posts houden elk en de begrenzing van ecosysteemeffecten vast. [B](calls/linkedin-wolf-r2-B/content.md) · [D](calls/linkedin-wolf-r2-D/content.md)
- **Lowell.** Beide posts behouden de tegenstelling tussen hergebruik en latere terugval. B specificeert de hergebruikte gebouwen in zijn opening als molens. D veralgemeniseert de herinnering van één arbeider naar arbeiders in het algemeen. [B](calls/linkedin-lowell-r1-B/content.md) · [D](calls/linkedin-lowell-r1-D/content.md)

Deze proef laat zien dat Compose het volledige D-briefmateriaal verwerkt. Ze is niet ontworpen om het afzonderlijke effect van het label of de formulering van Central Message op de posts te isoleren. [Scores per post](linkedin-scores.csv)

## Verbruik en verificatie

| Stap | Arm | Calls | Input | Completion incl. reasoning | Totaal |
|---|---|---:|---:|---:|---:|
| Narrative | B | 12 | 10.116 | 7.135 | 17.251 |
| Narrative | D | 12 | 10.320 | 6.647 | 16.967 |
| LinkedIn | B | 4 | 23.713 | 2.925 | 26.638 |
| LinkedIn | D | 4 | 23.738 | 2.626 | 26.364 |

D gebruikt 1,6% minder Narrative-tokens. Alle 32 GPT-6 Luna-calls hebben unieke response-ID's. De 24 Narrative-calls delen de modelinstellingen en het schema, en iedere B/D-paar deelt dezelfde userprompt. De twintig fixturebestanden komen byte voor byte overeen met Experimenten 3 en 4. B komt byte voor byte overeen met Experiment 4. Vooraf gehashte Compose-prompts komen overeen met de ingebedde productieprompts. Exacte offline replay reconstrueerde alle 32 requests en reproduceerde de geparseerde resultaten. Build van oplossing en runner slaagde zonder waarschuwingen of fouten. Alle 303 gevonden tests slaagden. [Live log](live.txt) · [Replaylog](replay.txt) · [Freeze](freeze.json) · [Verificatie](verification.txt) · [Tokens per call](usage.json) · [Build](build.txt) · [Runnerbuild](runner-build.txt) · [Tests](tests.txt) · [Afbreken en hervatten](REPLAY.md) · [Verificatie herhalen](reproduction-notes.md)

De uitvoerende assistent beoordeelde de teksten handmatig, met zicht op de armen en eerdere resultaten. Vier doelgericht gekozen fixtures met vaste upstream-uitvoer, drie herhalingen en één downstream-post per case en arm beperken de generaliseerbaarheid. Aantrekkelijkheid en leesbaarheid zijn redactionele beoordelingen zonder onafhankelijke lezersmeting. De productieprompts en C#-contracten zijn ongewijzigd. Er is niets gepromoveerd, gecommit of gepusht.
