# Narrative Experiment 6 — Endpoint Selection — NO-GO

Variant E verandert het slot bij Digital niet op de manier die het protocol vereist. Alle drie de E-briefs eindigen bij de cijfers en alle drie de Webb-briefs plaatsen afkoeling na de voltooide ontplooiing. Fase 1 omvatte twaalf onafhankelijke GPT-6 Luna-aanvragen in gebalanceerde volgorde. De poort voor fase 2 blokkeerde. Er zijn geen aanvragen voor Wolf, Lowell of LinkedIn gedaan. [Vooraf vastgelegd protocol](protocol.md) · [Exacte promptdiff](prompts.diff) · [Beslissing per criterium](phase1-decision.json)

| Aantal briefs met score 2 | B | E |
|---|---:|---:|
| Focus | 3/6 | 3/6 |
| Verhaalprogressie | 3/6 | 3/6 |
| Concrete details | 5/6 | 6/6 |
| Brontrouw | 2/6 | 3/6 |
| Compose-handoff | 6/6 | 6/6 |
| Volledig voldoende | 0/6 | 0/6 |

- **Digital.** B en E leggen de werkwijze uit in Selected Story en Development. Alle zes Endpoints openen met de drie gerapporteerde cijfers. E bewaart de grens rond causaliteit, maar gebruikt die grens als kwalificatie bij de cijfers. Alle E-briefs behouden beide sprintweken. B r2 vat ze algemener samen en geeft met ‘na twaalf weken werk’ een impliciet startpunt. Digital-focus is 0/3 in beide armen. [E r2](calls/digital-r2-E/content.md) · [B r2](calls/digital-r2-B/content.md) · [Bevroren Synthesis](cases/digital/synthesis-result.json)
- **Webb.** Alle zes briefs behouden de reden voor de gevouwen lancering en het onderscheid tussen uitvouwen en klaar zijn voor routinewetenschap. E r1 en r2 gebruiken ‘then cooled’ na het vergrendelen van de spiegels. E r3 zet de volgorde in Selected Story, ook al laat Development overlap toe. De bron plaatst de start van de afkoeling al rond de ontplooiing van het zonneschild, voor de voltooide spiegelontplooiing. B heeft twee expliciete volgordefouten en één ambigue formulering. In paar r2 daalt brontrouw van 1 naar 0. De fout was als type al aanwezig in B, maar de expliciete volgorde is een gepaarde regressie. [E r2](calls/webb-r2-E/content.md) · [B r2](calls/webb-r2-B/content.md) · [E r3](calls/webb-r3-E/content.md) · [Bron](cases/webb/source.md)

De focussom blijft 9/12 en de brontrouwsom blijft 6/12. E's extra volledig brongetrouwe Digital-brief compenseert in de som de verslechtering bij Webb r2. De nieuwe instructie ging samen met behoud van de fase-indeling en een bruikbare handoff, maar loste geen van beide vooraf aangewezen problemen op. Synthesis zet bij Digital de cijfers in een afzonderlijk bewijsitem en staat optioneel gebruik toe. Bij Webb beschrijft Synthesis afkoeling in maand één zonder de overlap expliciet uit te schrijven. Dat zijn mogelijke verklaringen voor het patroon. Deze proef isoleert het effect van de nieuwe slotinstructie onder vaste invoer en kan niet bepalen welke passages de formuleringen hebben uitgelokt. [Beoordeling per brief](phase1-scores.json) · [Webb Synthesis](cases/webb/synthesis-result.json)

| Arm | Calls | Input | Completion | Totaal |
|---|---:|---:|---:|---:|
| B | 6 | 5.193 | 3.048 | 8.241 |
| E | 6 | 5.427 | 3.477 | 8.904 |

E verbruikte 8,0% meer tokens en bleef binnen de grens van 10%. De 12 response-ID’s zijn uniek. Instellingen, schema en userprompt per paar komen overeen. Offline replay reconstrueerde alle 12 requests exact en reproduceerde de geparseerde resultaten. De oplossing en runner bouwden zonder waarschuwingen of fouten. Alle 303 gevonden tests slaagden. [Live log](live-phase1.txt) · [Verbruiksdetails](phase1-usage.json) · [Replaylog](replay-phase1.txt) · [Verificatie](verification.txt) · [Vergelijking met Experiment 5](baseline-verification.txt) · [Build](build.txt) · [Runnerbuild](runner-build.txt) · [Tests](tests.txt) · [Fase 2-poort](phase2-gate-check.txt)

De uitvoerende assistent beoordeelde alle briefings handmatig en kende de variantlabels. Twee doelgericht gekozen cases en drie herhalingen per arm beperken de conclusie tot deze proef. De scores meten de geschiktheid van de handoff op briefniveau. Het protocol leverde geen downstream-metingen op. De productieprompts, schema’s, C#-contracten en upstream-stappen zijn ongewijzigd. Geen promotie, commit of push. [Reproduceren en afbreken](REPLAY.md)
