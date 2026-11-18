# Experiment 11 — Editorial Tension to Grounded Ending

**NO-GO voor beide kandidaten wegens onvolledige dekking.** GPT-6 Luna gaf bij de zestiende aanvraag HTTP 200 en finish reason `stop`, met `{}` als antwoord. Het vereiste veld `content` ontbrak. De productieagent weigerde de respons en de reeks stopte volgens protocol. Er zijn 15 voltooide LinkedIn-posts in vijf blokken, één ongeldige respons en 32 niet uitgevoerde aanvragen. Hetzelfde probleem verscheen bij offline replay van de ongeldige respons. Eén mislukte call geeft geen schatting van de faalkans per arm. [Protocol](protocol.md) · [Live-log](live.txt) · [Ongeldige respons](calls/webb-r1-H-c1-C/response-raw.json) · [Offline foutreplay](replay-invalid.txt)

## Wat de eerste vijf blokken laten zien

A is productie-Compose met de oorspronkelijke brief. B voegt een handgeschreven bewijsrichting toe aan de bestaande Narrative-inhoud en gebruikt productie-Compose. C gebruikt de oorspronkelijke brief en wijzigt één regel in de LinkedIn-slotinstructie. Alle 15 posts kwamen uit de productieagent. Dezelfde uitvoerder beoordeelde aan de hand van Synthesis en Selected Idea. De reviewweergave verborg de armlabels. De uitvoerder kende de opzet en tekstkenmerken konden de arm verraden. [Vooraf vastgelegde audit](precall-audit.md) · [Bewijsrichtingen](direction/) · [Compose-diff](prompts.diff) · [Scores en paren](decision.json)

| Maatstaf | A · productie | B · bewijsrichting | C · slotinstructie |
|---|---:|---:|---:|
| Aantrekkelijkheid 2 | 5/5 | 5/5 | 5/5 |
| Leesbaarheid 2 | 5/5 | 5/5 | 4/5 |
| Hook 2 | 4/5 | 3/5 | 3/5 |
| Spanningsboog 2 | 5/5 | 5/5 | 5/5 |
| Slot 2 | 0/5 | 0/5 | 1/5 |
| Focus 2 | 5/5 | 5/5 | 5/5 |
| Geheel brongetrouw | 2/5 | 2/5 | 4/5 |
| Geheel brongetrouw op veilige brief | 1/2 | 0/2 | 2/2 |
| Nieuwe of versterkte afwijking of mogelijk materieel risico | 2/5 | 3/5 | 0/5 |
| Overgenomen afwijking of mogelijk materieel risico op problematische brief | 2/3 | 1/3 | 1/3 |

- **Digital.** Alle drie posts op H r3 neutraliseerden het causale ‘door’ tussen aanpak en lancering. Hun sloten herhaalden de ontwerpvolgorde of testtijd. Op H r1 eindigden A en B op de vijfweekscijfers. C voegde een recap toe en sloot af met beide tijdvensters. B maakte de aanvankelijke vraag stelliger met ‘moest binnen enkele weken’. Evidence 2–3 bieden een concretere slotrichting: het team liet de prioriteit voor aanvragers zien in alpha en bepaalde tijdens private beta zelfs wanneer nieuwe aanvragen moesten wachten om test-, onderzoeks- en verwerkingswerk ruimte te geven. De proef liet geen post zien die dat inzicht in het slot zelfstandig uitwerkte. [Digital-scores](scores/digital.json)
- **Webb.** Op H r3 nam A ‘Daarna volgde een langere ingebruikname’ inclusief de afkoeling over. B en C hielden week twee en maand één apart. Alle drie behielden de Ariane 5-vouwreden, maar herhaalden in het slot vooral het verschil tussen uitgevouwen zijn en routinewetenschap. Synthesis geeft een concretere betekenis via twee verschillende eisen: in de kuip passen en de spiegels en instrumentmodi gereedmaken voor waarnemingen. De veilige H r1-brief leverde één ongeldige C-respons en geen post in A of B op. [Webb-scores](scores/webb.json)
- **Wolf.** Op veilige F r3 behielden alle drie de identiteit van elk en de herkomst van de prooikarkassen. A en B versmalden ‘ungulate management’ tot ‘elk management’. C behield het ruimere begrip. De roofdierrelaties werden concreet in de openingen over een prooi die ook aaseters voedt. De sloten bleven bij algemene formuleringen over samenwerkende factoren. De problematische H-brief met ‘elandkalveren’ en ‘karkassen van wolven’ kwam niet aan de beurt. [Wolf-scores](scores/wolf.json)
- **Lowell.** Alle drie posts namen de geërfde causaliteit ‘had ended the boom’ over. A en B versmalden ‘old buildings’ in de hook tot molengebouwen. Het C-slot verbond concreet hergebruik met de grens die de terugval stelt aan de betekenis ervan, verder dan de algemene hook over een opleving die geen rechte lijn volgde. Dat ene redactioneel voldoende slot komt uit een post met een materiële fout eerder in de tekst. De veilige H-brief kwam niet aan de beurt. [Lowell-scores](scores/lowell.json)

## Interpretatie

De te onderzoeken handoff is een betekenisrelatie tussen twee concrete observaties die al in de brief staan. Eén observatie maakt het verhaal tastbaar. De andere laat zien hoe ver de interpretatie reikt. Bij Lowell zijn dat de nieuwe functies voor oude gebouwen en de latere terugval. Bij Digital zijn het de prioritering van aanvragers en de sprintweek waarin aanvragen tijdelijk werden gepauzeerd. De slotrichting moet specifieker zijn dan het abstracte contrast dat een hook kan introduceren, en onafhankelijk blijven van ongegronde verklaringen over de oorzaak of volgorde van gebeurtenissen.

B testte die richting rechtstreeks, maar de vijf beschikbare posts brachten haar niet tot een voldoende slot. De C-instructie leverde één voldoende slot op en heeft op deze vijf posts minder nieuwe feitelijke risico's dan de controle. Dat patroon is beschrijvend. Het begon bij slechts vijf van de acht doelgericht gekozen briefs, alle uit de eerste herhaling. De hele tweede herhaling ontbreekt. Geen arm laat onder deze rubric tegelijk een voldoende slot en volledig brongetrouwe post zien.

De dekkingsgate faalt voor beide kandidaten. De geplande drempels voor het aantal sloten, brontrouw en redactionele dimensies kunnen op vijf posten per kandidaat niet worden beoordeeld. De vergelijkende booleans in het beslisbestand zijn berekend op beschikbare posten. B heeft al één nieuwe gepaarde afwijking, bij de Digital-deadline, en het hoogste aantal posten met nieuwe risico's. De overige uitkomsten zijn gevoelig voor beoordeling van implicatuur, de doelgerichte selectie, de handmatige samenstelling van B en de ontbrekende herhalingen. Het onderzoek valideert geen automatische handoffgeneratie en doet geen uitspraak over het samenspel van beide ingrepen.

## Verbruik en controles

| Arm, vijf voltooide posts | Input | Waarvan cache | Output | Totaal | Geschatte USD |
|---|---:|---:|---:|---:|---:|
| A | 30.189 | 7.800 | 6.814 | 37.003 | 0,0057239 |
| B | 30.569 | 9.750 | 5.233 | 35.802 | 0,0047959 |
| C | 30.339 | 9.750 | 5.186 | 35.525 | 0,0047494 |

De ongeldige C-aanvraag verbruikte 6.514 tokens, waarvan 6.153 input, 1.950 in cache en 361 output. Met die aanvraag komt het totaal op 114.844 tokens en naar schatting $0,0158895. Zonder cachekorting komt de schatting op $0,0185220. Dit gebruikt vooraf vastgelegde Experiment 10-tarieven en gemelde tokenvolumes. De voltooide blokken leveren geen betrouwbare kostenschatting van een volledige serie. [Ruwe calls](calls/) · [Kostenberekening](decision.json)

Builds van de oplossing en runner slaagden met nul waarschuwingen en fouten. Alle 303 gevonden tests slaagden. De lokale .NET-host miste hostfxr, dus is dezelfde SDK-versie 10.0.401 vanuit een tijdelijke map buiten de repository gebruikt. Offline replay reproduceerde alle 15 aanvragen en geparseerde posts exact en reproduceerde ook de weigering van de ongeldige respons. De vijf nieuwe productie-requests zijn byte-identiek aan die uit Experiment 10. Productiecode, prompts, contracten en upstream agents bleven ongewijzigd. [Build](build.txt) · [Runnerbuild](runner-build.txt) · [Tests](tests.txt) · [Replay](replay.txt) · [Verificatie](verification.txt) · [Herkomstmatrix](origin-matrix.md)

