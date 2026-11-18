# Narrative Experiment 2 — NO-GO

**De onderwerpneutrale variant haalt 4/12 volledig voldoende, gelijk aan de gelijktijdig gemeten productiearm.** Beide armen halen 5/12 op brontrouw. In Wolf r1 gaat B gepaard van correcte soortidentiteit naar soortverwisseling. In Wolf r3 gebeurt het omgekeerde. De 24 live calls en offline controles zijn geslaagd. B mist de kwaliteitsdrempels uit het vooraf vastgelegde protocol. [Protocol](protocol.md) · [Exacte promptdiff](prompts.diff) · [Per run en per paar](scores.md)

| Case | A volledig voldoende | B volledig voldoende |
|---|---:|---:|
| Webb | 0/3 | 0/3 |
| Wolf | 1/3 | 1/3 |
| Digital | 0/3 | 0/3 |
| Lowell | 3/3 | 3/3 |
| **Totaal** | **4/12** | **4/12** |

| As | A score 2 | B score 2 |
|---|---:|---:|
| Verhaallijn | 10/12 | 11/12 |
| Focus | 11/12 | 10/12 |
| Details | 12/12 | 12/12 |
| Brontrouw | 5/12 | 5/12 |
| Compose-handoff | 10/12 | 11/12 |

B mist het minimum van tien volledig voldoende runs en wint geen twee runs ten opzichte van A. Webb, Wolf en Digital missen twee volledig voldoende briefs per case. De verplichte 12/12 op brontrouw en de focusdrempel zijn niet gehaald. Wolf r1 schendt ook de grens op een gepaarde daling van 2 naar 0. De historische 3/12 van Experiment 1 is alleen context voor deze herhaling. De beslisregel gebruikt de nieuwe A.

## Bevindingen

- **Webb.** Alle zes briefs behouden de vouwreden, de uitvouwing in week twee en routinewetenschap na zes maanden. De spiegelstappen en faalrisico’s geven beide armen concrete bouwstenen. De input bundelt afkoeling, uitlijning en kalibratie over de ingebruikname en plaatst afkoeling expliciet in maand één. De bron toont afkoeling terwijl uitvouwing nog liep. Alle zes briefs plaatsen afkoeling met formuleringen als ‘daarna’ na de voltooide uitvouwing, ook als elders maand één staat. Dat is nieuwe aanscherping door Narrative. Het losse slotwoord uit Synthesis en het risico op verwarring door ‘operational configuration’ leiden hier niet tot een verzonnen storing of eerdere wetenschappelijke gereedheid. [Input](cases/webb/input.json) · [Bron](cases/webb/source.md)
- **Wolf.** Beide armen leveren een verhaal van roedels en aaseters via een concrete prooiverschuiving naar meerdere betrokken omstandigheden. Synthesis en Research gebruiken ‘elk’. In A r2/r3 en B r1/r2 wordt dat ‘eland’ in plaats van wapiti. De twee correcte briefs behouden de Engelse bronterm en zijn deels daarom brontrouw. De wisseling over de paren geeft geen verbetering op soortidentiteit. De briefs houden de grens rond een ecosysteembrede cascade in stand. [Input](cases/wolf/input.json) · [Research-projectie](cases/wolf/projection.md)
- **Digital.** De formuleringen ‘na’ en ‘in twaalf weken’ en het onbekende startpunt zitten al in Synthesis. Geen brief kiest een startpunt of schrijft lancering of cijfers toe aan de werkwijze. Vier briefs maken van overlappende private-beta-activiteiten een afwisseling, een nieuwe tijdlijnfout. B r1 voegt die toe tegenover A r1. B r3 beschrijft de activiteiten naast elkaar en verbetert op brontrouw tegenover A r3. In r2 heeft B een afzonderlijk einde en biedt het Compose een duidelijkere derde stap, maar verlegt het de nadruk van de aanpak naar uitkomstcijfers. Een slot dat de aanpak alleen herhaalt, verlaagt de verhaallijn- en handoffscore. [Input](cases/digital/input.json) · [Bron](cases/digital/source.md)
- **Lowell.** Beide armen behouden de lijn van textielverval via burgerinitiatief en hergebruik naar de latere terugval. De werkersonzekerheid en ruim honderd hergebruikte gebouwen geven de auteur concrete details. Alle briefs behouden het historische perspectief en voorkomen een monocausale verklaring van de bredere opleving. [Input](cases/lowell/input.json) · [Bron](cases/lowell/source.md)

Geen van beide armen nam de orchestrator- of harnessinstructie over. De verwijdering maakt B onderwerpneutraal, maar deze vier cases laten geen meetbaar verschil in onderwerpvervuiling zien. Beide armen gaven format-agnostische briefs. Compose-beoordeling betreft de scheiding en bruikbaarheid van de drie stappen, de Selected Story en de grenzen voor Article en LinkedIn. Er is geen Compose-output gegenereerd.

## Verbruik, uitvoering en verificatie

| Arm | Calls | Inputtokens | Waarvan gecachet | Outputtokens incl. reasoning | Totaal | Geschatte USD |
|---|---:|---:|---:|---:|---:|---:|
| A | 12 | 88.824 | 88.788 | 8.518 | 97.342 | 0,00515 |
| B | 12 | 85.236 | 68.431 | 9.504 | 94.740 | 0,00712 |
| **Totaal** | **24** | **174.060** | **157.219** | **18.022** | **192.082** | **0,01227** |

B gebruikte 4,0% minder inputtokens en 2,7% minder totale tokens. De hogere kostenraming hangt samen met het beperkte aantal ongecachete tokens in A en de hogere output van B. De armen dragen een verschillende cachehistorie, waaronder eerder gebruik van productieprompts. Gebruik deze raming als verbruikskosten voor deze uitvoering. Zij levert geen structurele kostenvergelijking op. De schatting gebruikt Standard-tarieven per miljoen tokens en cachehits uit de responses. Factuur en eventuele afwijkende accounttarieven zijn niet geverifieerd. [Modeltarieven](https://developers.openai.com/api/docs/models/gpt-6-luna) · [Usage](usage.json) · [Kosten per call](costs.json)

De 20 fixture- en referentiebestanden zijn byte voor byte gelijk aan Experiment 1. De runner bouwde met nul waarschuwingen en 34 gerichte tests slaagden. Alle 24 opgeslagen requests bevatten de verwachte prompts en invoer. De overige requestinstellingen zijn gelijk binnen ieder paar. Alle 24 responses hadden een unieke response-ID en zijn via de productieagent opnieuw identiek geparseerd. [Vastgelegde SHA-256-hashes](freeze.json) · [Verificatie](verification.txt) · [Testlog](tests.txt) · [Replay en pauzeren](REPLAY.md)

De beoordeling is handmatig en niet blind, met vier doelgericht gekozen cases en drie onafhankelijke aanvragen per arm per case. Het experiment meet de gecombineerde promptwijziging. De werkboomwijzigingen zijn beperkt tot deze experimentmap. Er is geen productiepromotie, commit of push uitgevoerd.
