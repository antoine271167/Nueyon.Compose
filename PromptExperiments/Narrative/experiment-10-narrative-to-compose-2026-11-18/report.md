# Experiment 10 — End-to-End Fidelity / Narrative-to-Compose Handoff

**NO-GO.** De minimale LinkedIn-instructie bracht het aantal posts met nieuwe Compose-afwijkingen van 6/16 naar 3/16. Op bronveilige briefs waren 5/8 variantposts volledig brongetrouw, onder de vereiste 7/8. Er was één nieuwe gepaarde fout in de Digital-hook. Het aantal posts met overgenomen fouten bleef 6/8 bij problematische briefs. Slechts 2/16 posts per arm haalden alle zes redactionele criteria. De variant overschreed de kostengrens. [Protocol](protocol.md) · [Diff](prompts.diff) · [Scores, verbruik en beslissing](decision.json) · [Herkomstmatrix](origin-matrix.md)

## Uitvoering

Experiment 9 vond 7/12 bronveilige H-briefs. De productieposts maakten in die proef op twee veilige Webb-briefs een nieuwe tijdclaim. Wolf liet zien hoe een verkeerde soortnaam de handoff kon passeren. Experiment 10 hergebruikte zes H- en twee F-briefs. F leverde de bronveilige Wolf-brief en de problematische Lowell-brief omdat die strata in H ontbreken. Alle acht briefs gingen door, met twee herhalingen in beide arms en omgekeerde startvolgorde per herhaling. De 32 GPT-6 Luna-responses waren uniek. Binnen elk paar verschilden de aanvragen uitsluitend in de vooraf bevroren alinea voor LinkedIn. Compose kreeg in beide arms de productie-input met alleen Narrative en format. Selected Idea en Synthesis bleven bij respectievelijk de redactionele en feitelijke beoordeling. [Experiment 9](../experiment-9-editorial-blueprint-2026-11-18/report.md) · [Callschema](config.json) · [Aanvragen en oorspronkelijke responses](calls/)

## Feitelijke herkomst

| Maatstaf per post | A · productie | B · variant |
|---|---:|---:|
| Volledig brongetrouw, alle posts | 5/16 | 7/16 |
| Volledig brongetrouw, veilige briefs | 3/8 | 5/8 |
| Volledig brongetrouw, problematische briefs | 2/8 | 2/8 |
| Nieuwe afwijking of mogelijk materieel risico | 6/16 | 3/16 |
| Overgenomen afwijking of mogelijk materieel risico, problematische briefs | 6/8 | 6/8 |
| Duidelijke materiële afwijking, alle posts | 6/16 | 6/16 |

- **Webb.** Op de veilige brief introduceerden alle vier posts een nieuwe tijdimplicatie. A schreef onder meer «Daarna volgde een ingebruikname van zes maanden». B hield in één post de tijdvakken in de hoofdtekst apart, maar sloot af met «de ingebruikname die daarop volgde». Op de problematische brief behielden alle vier posts de volgorde of een ambiguïteit daarover. B r2 maakte de al aanwezige tijdrelatie stelliger dan de A-post. Alle acht posts behielden de Ariane 5-vouwreden.
- **Wolf.** Alle vier posts op H r2 behielden «elandkalveren» en «elanden». Alle vier neutraliseerden «karkassen van wolven» door het over prooi of karkassen bij de jacht te hebben. Beide arms behielden elk op F r3. In één A-post werd «ungulate management» vernauwd tot «elk management». B hield de bredere scope in beide herhalingen.
- **Lowell.** Beide arms hielden op F r1 in beide herhalingen «had ended the boom» aan. Dat causaal verband stond al in de brief. In één A-post werden de University of Lowell en Wang bovendien onderdeel van de opleving. Op het veilige H r1 impliceerde A één keer een verklarende rol voor de parken en vernauwde A één keer het gebouwtype in de hook. Beide H-posts van B behielden oude gebouwen, samenloop en het perspectief van 1992.
- **Digital.** Beide arms neutraliseerden in alle vier posts op H r3 het causale «door» uit de brief. Op het veilige H r1 schreef B r2 «in twaalf weken lanceren begon met een andere vraag». Die hook suggereert een bekend startmoment en een anders geformuleerde opdracht. De afwisseling van sprintweken bleef in alle posts zichtbaar, maar twee A/B-posts op de veilige brief eindigden op cijfers zonder de werkwijze af te ronden. [Passages per post](origin-matrix.md)

## Redactionele beoordeling

| Criterium met score 2 | A | B |
|---|---:|---:|
| Aantrekkelijkheid | 15/16 | 14/16 |
| Leesbaarheid | 13/16 | 16/16 |
| Hook | 14/16 | 14/16 |
| Spanningsboog | 15/16 | 16/16 |
| Onderscheidend, onderbouwd slot | 3/16 | 2/16 |
| Focus | 14/16 | 15/16 |
| Volledig voldoende | 2/16 | 2/16 |

B kreeg 171 tegen 169 van 192 punten, met 6 verbeteringen, 7 gelijke paren en 3 regressies in het redactionele puntentotaal. Digital daalde van 40 naar 38. Webb ging van 42 naar 43, Wolf van 42 naar 45 en Lowell bleef op 45. De regressies waren Digital H r1 herhaling 1 (10 → 7, slot op KPI’s en minder concrete opening), Webb H r1 herhaling 1 (11 → 10, algemenere opening) en Lowell F r1 herhaling 2 (12 → 11, slot herhaalt het uitgangspunt). De meeste posts waren goed leesbaar en hielden het onderwerp vast. Hun afsluitingen herhaalden vaak de opening, voegden een scopebeperking als tweede slot toe of lieten cijfers het laatste woord krijgen. De Wolf-post met «A wolf kill in Yellowstone can feed far more than the pack» laat zien hoe een specifieke hook en een onderbouwd breder slot wél samen kunnen gaan. [Gepaarde scores](decision.json)

## Tokens en kosten

| Arm | Input | Waarvan cache | Output | Totaal | Geschatte USD |
|---|---:|---:|---:|---:|---:|
| A | 96.712 | 80.397 | 15.723 | 112.435 | 0,010297 |
| B | 97.672 | 64.412 | 16.486 | 114.158 | 0,012213 |

B gebruikte 1,5% meer tokens en kostte naar schatting 18,6% meer. De totale schatting is $0,022510. Tarieven zijn vóór de reeks vastgezet op de Standard-tarieven uit Experiment 9. Het kostenverschil is mede afhankelijk van cachetreffers. Zonder cachekorting zou het verschil bij dezelfde volumes 2,7% zijn. Eén live reeks kan het structurele kosteneffect niet vaststellen. [Tarieven en rekenbasis](config.json) · [Verbruik per paar](decision.json)

## Gates en grenzen

Geslaagd: 16 volledige paren, 32 exacte offline replays, minstens twee minder posts met nieuwe afwijkingen, geen toename in overgenomen fouten, hoogstens drie redactionele paarregressies en maximaal 10% meer tokens. Geëist en gemist: geen nieuwe gepaarde fout, minstens 7/8 brontrouw op veilige input, minstens 10/16 redactioneel volledig voldoende plus twee per case, geen redactionele daling per onderwerp en maximaal 10% hogere geschatte kosten.

De steekproef bestaat uit vier doelgericht gekozen onderwerpen. Twee stateless herhalingen per arm en brief toetsen herhaalbaarheid op die brief, zonder de inhoudelijke dekking te verdubbelen. De uitvoerende assistent beoordeelde geanonimiseerde posts in batches, zonder tweede beoordelaar of formele interbeoordelaarsmeting. Oordelen over Webb-overgangen, de Digital-hook en sommige scopeversmallingen zijn gevoelig voor implicatuur. Strikt op duidelijke materiële fouten blijft het aantal onvoldoende posts gelijk. Ook bij een soepeler oordeel over de ambiguïteiten blijven de redactionele gates buiten bereik. Met slechts Narrative als input kan deze Compose-prompt de bronfout in een overtuigende maar misleidende handoff doorgaans niet herkennen. In deze proef daalde het gemeten aantal nieuwe afwijkingen, terwijl overdracht van de hardnekkige fouten bleef bestaan.

Productiebuild en runnerbuild slaagden zonder waarschuwingen of fouten. Alle 303 ontdekte tests slaagden. Vier controle-requests waren ook byte-identiek aan hun historische productie-requests uit Experiment 9. Productiecode, prompts, agents en contracten zijn ongewijzigd. [Build](build.txt) · [Runnerbuild](runner-build.txt) · [Tests](tests.txt) · [Offline replay](replay.txt) · [Verificatie](verification.txt)

