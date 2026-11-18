# Experiment 11b · Van redactionele spanning naar een brongetrouw slot

**NO-GO voor B en C.** A en B leveren elk 0 van 16 voldoende sloten, C 1 van 16. De vooraf vastgelegde drempel is 10 van 16 met minstens vier extra voldoende sloten tegenover A en minstens twee per case. C haalt 7 van 8 volledig brongetrouwe posts op veilige input, maar zijn enige voldoende slot staat in een post met een materiële bronfout. Geen van de 48 posts combineert slot 2 met brontrouw 2. [Beslisbestand met alle scores en paren](decision.json)

## Uitvoering

De proef gebruikt acht doelgericht gekozen Narrative-briefs, twee herhalingen per brief, drie armen en 16 gepaarde blokken. A gebruikt productie-Compose en de originele brief. B voegt de vooraf geschreven Synthesis-bewijsrichting toe aan die brief en gebruikt productie-Compose. C houdt de brief gelijk en verandert één LinkedIn-slotregel. De eerste 15 posts en hun scores komen ongewijzigd uit Experiment 11. [Protocol](../experiment-11-editorial-tension-to-grounded-ending-2026-11-18/protocol.md) · [Bronnen en fixtures](../experiment-11-editorial-tension-to-grounded-ending-2026-11-18/) · [Promptdiff](../experiment-11-editorial-tension-to-grounded-ending-2026-11-18/prompts.diff)

De oorspronkelijke zestiende poging kreeg HTTP 200 en finish reason `stop`, met `{}` als inhoud. Het archief bewaart de ruwe respons, aanvraag en fout. De aanvulling op het protocol werd vóór een nieuwe live call vastgelegd. Eén apart gelabelde, stateless vervanging met exact dezelfde aanvraag slaagde. Daarna zijn de resterende 32 GPT-6 Luna-aanvragen in de vastgelegde volgorde uitgevoerd. Er waren geen verdere ongeldige antwoorden. De reeks telt 48 geldige posts en 49 pogingen. Beide pogingen op positie 16 staan in het beslisbestand met hun eigen tijd, response-ID en status. [Amendement](amendement.md) · [Tijdstippen en hashes](amendment-audit.txt) · [Originele fout](archive/webb-r1-H-c1-C-original/) · [Vervanging](calls/webb-r1-H-c1-C-replacement/) · [Uitvoeringslog](live.txt)

## Redactionele beoordeling en brontrouw

| Maatstaf | A · productie | B · bewijsrichting | C · slotinstructie |
|---|---:|---:|---:|
| Aantrekkelijkheid 2 | 16/16 | 16/16 | 16/16 |
| Leesbaarheid 2 | 15/16 | 15/16 | 13/16 |
| Hook 2 | 9/16 | 12/16 | 8/16 |
| Spanningsboog 2 | 16/16 | 16/16 | 16/16 |
| Slot 2 | 0/16 | 0/16 | 1/16 |
| Focus 2 | 15/16 | 15/16 | 15/16 |
| Redactioneel subtotaal zonder slot | 151/160 | 154/160 | 148/160 |
| Volledig brongetrouw, alle briefs | 4/16 | 5/16 | 10/16 |
| Volledig brongetrouw, veilige briefs | 2/8 | 2/8 | 7/8 |
| Volledig brongetrouw, problematische briefs | 2/8 | 3/8 | 3/8 |
| Posts met nieuwe afwijking of mogelijk materieel risico | 8/16 | 7/16 | 1/16 |
| Posts met overgenomen afwijking of risico op problematische brief | 6/8 | 5/8 | 5/8 |

Op de volledige slotschaal heeft B 16 gelijke paren tegenover A. C wint twee paren en speelt 14 gelijk. Bij Digital H r1 gaat het slot van een cijferlijst (0) naar een recap met herhaalde tijdvensters (1). Bij Lowell F r1 gaat het van 1 naar 2. B haalt alle overige redactionele drempels. Bij C blijven zowel de hookscore als het subtotaal zonder slot onder de drempel. Sommige C-hooks blijven algemeen en een Digital-post eindigt met `/final`. Een B-post over Wolf eindigt met een vreemd teken. Deze twee uitvoerresten staan in de ruwe posts en zijn beoordeeld onder leesbaarheid. [Digital C](calls/digital-r3-H-c2-C/content.md) · [Wolf B](calls/wolf-r2-H-c2-B/content.md)

| Case | Slot 2 A/B/C, elk op 4 | Brontrouw 2 A/B/C, elk op 4 | Herkomst en effect |
|---|---:|---:|---|
| Digital | 0 / 0 / 0 | 4 / 3 / 4 | Alle armen laten het causale ‘door’ uit H r3 vallen. B maakt de oorspronkelijke vraag in veilige H r1 één keer stelliger met ‘moest binnen enkele weken’ (Synthesis Evidence 1). De beste ontwikkeling neemt aanvragers en de aanvraagpauze als concrete keuzes (2–3). Sloten blijven bij cijfers, recaps of een disclaimer (5 en Gaps). |
| Webb | 0 / 0 / 0 | 0 / 1 / 2 | De Ariane 5-vouwreden blijft behouden (1). Op veilige H r1 voegen A en B in beide herhalingen een volgorde toe aan week twee en maand één. C doet dat in de tweede herhaling. A maakt van ingebruikname in die herhaling ook zes extra maanden. In H r3 komt de fasevolgorde al uit Narrative. B en C neutraliseren haar in de eerste herhaling, maar nemen haar in de tweede weer over. De meeste sloten herhalen de tegenstelling tussen uitgevouwen en klaar voor routine (3–4). |
| Wolf | 0 / 0 / 0 | 0 / 0 / 2 | Op veilige F r3 versmallen A en B in beide herhalingen ‘ungulate management’ tot ‘elk management’ (4). C behoudt de scope. Alle zes posts op problematische H r2 corrigeren de karkasreferent (2) en behouden de verkeerde soortterm ‘elandkalveren’ (3–4). De prooiverschuiving staat in de hoofdtekst en de eindgedachte keert meestal terug naar een algemeen netwerk. |
| Lowell | 0 / 0 / 1 | 0 / 1 / 2 | Alle zes F r1-posts behouden de geërfde oorzakelijke terugval (4). Op veilige H r1 geeft A samenloop eenmaal causale lading en voegt eenmaal zowel die lading als een herstelbijdrage voor universiteit en Wang toe. B suggereert die bijdrage eenmaal met ‘niet de enige motoren’. C houdt op H r1 verslagperspectief, actoren en samenloop intact (3–4 en Gaps). Het voldoende C-slot over hergebruik en de kwetsbaarheid van de opleving komt uit F r1, waar eerder ‘had ended the boom’ staat. |

Bij B daalt het totale aantal posts met nieuwe risico’s van acht naar zeven. Toch faalt de gepaarde claimregel tweemaal. A heeft geen aangescherpte deadline in Digital H r1, en in Lowell H r1 schrijft B de universiteit en Wang een herstelbijdrage toe waar de gepaarde A een ander risico introduceert. C slaagt voor die claimregel en haalt de drempel op veilige input precies. Op problematische input blijven de belangrijkste Narrative-fouten zichtbaar. De brontrouwverbetering van C draagt daarmee geen besluit over de slotinterventie. Concrete openingen, slotpassages, zes dimensies en Synthesis-ankers per post staan onder `rows` in het beslisbestand. [Alle posten, gate-uitkomsten en claimverschillen](decision.json)

## Tokens, cache en kosten

De kostengates gebruiken de tarieven uit de bevroren config per miljoen tokens, in USD 0,10 input, 0,01 gecachte input en 0,50 output. Dit zijn schattingen tegen het vooraf vastgelegde tarief.

| Arm, geldige posts | Input | Gecachte input | Output | Totaal | USD | USD zonder cachekorting |
|---|---:|---:|---:|---:|---:|---:|
| A | 96.712 | 61.982 | 15.956 | 112.668 | 0,01207082 | 0,01764920 |
| B | 97.944 | 64.548 | 17.505 | 115.449 | 0,01273758 | 0,01854690 |
| C | 97.192 | 68.372 | 16.096 | 113.288 | 0,01161372 | 0,01776720 |

B gebruikt 2,47% meer tokens en kost naar schatting 5,52% meer dan A. C gebruikt 0,55% meer tokens en kost met cachekorting 3,79% minder. Zonder cachekorting ligt C 0,67% boven A. Beide kandidaten blijven onder de stijgingsgrens van 10%. Inclusief de ongeldige poging gebruikt C 119.802 tokens en kost die arm naar schatting USD 0,01223402, 1,35% meer dan A. Alle 49 pogingen samen komen op 347.919 tokens, waarvan 196.852 gecachte input, en USD 0,03704242 (USD 0,05475910 zonder cachekorting). De 33 nieuwe pogingen voegen 233.075 tokens en naar schatting USD 0,02115292 toe. Het beslisbestand bevat ook het verbruik per case en stratum. [Verbruiksgegevens](decision.json)

## Reproduceerbaarheid en beperkingen

De oplossing en runner bouwden met nul fouten en waarschuwingen. Alle 303 gevonden tests slaagden. De runner verifieerde de hashes uit de oorspronkelijke freeze en bevestigde dat de ingebedde productieprompts overeenkomen met de snapshot. De aanvraagcontrole vergelijkt de armingrepen binnen elk blok, beide herhalingen en de vervanging met de oorspronkelijke requestbody. Elke request bevatte alleen de systeem- en gebruikersboodschap. Offline replay reproduceerde alle 48 aanvragen, resultaten en posts exact en kreeg ook dezelfde parserfout bij de oorspronkelijke ongeldige respons. [Build](build.txt) · [Runnerbuild](runner-build.txt) · [Tests](tests.txt) · [Aanvraagcontrole](verification.txt) · [Replay](replay.txt)

Onder het oorspronkelijke stopprotocol blijft Experiment 11 onvolledig. Onder het amendement slaagt de dekkingsgate. Als gevoeligheidscontrole houdt weglaten van de vervanging 47 posts en 15 complete A/C-paren over. C houdt dan één voldoende slot op 15 en zes volledig brongetrouwe veilige posts op zeven. Het besluit voor beide kandidaten blijft NO-GO. De vervanging beïnvloedt C’s brontrouwscore en cacheverbruik. Ongeveer elf minuten tussen de oorspronkelijke fout en de vervanging, een andere cachestand, en selectie van een geldige respons op die positie beperken de vergelijking. Eén ongeldige respons volstaat niet om een verschil in faalkans tussen armen te schatten.

Voor nieuwe posts sorteerde de review op pseudoniem en liet zij het armlabel weg. De beoordelaar kende het proefontwerp en kon armspecifieke formuleringen herkennen. De oorspronkelijke en nieuwe scores stammen uit verschillende beoordelingsmomenten en er was geen onafhankelijke tweede beoordeling. De cases en briefs zijn doelgericht gekozen. De proef laat geen conclusie toe over automatische generatie van de B-bewijsrichting, andere onderwerpen of een combinatie van beide ingrepen. [Replay- en reviewinstructies](REPLAY.md)


Het PowerShell-commando rond de live runner gaf bij het afsluiten exitcode 1 door een fout in de afsluitexpressie. De fout trad op na de voltooiingsmelding voor positie 48. De 33 opgeslagen nieuwe resultaten, de planning en de volledige offline replay bevestigen de voltooiing. Er zijn geen extra live aanvragen gedaan om de afsluitfout te herstellen.
