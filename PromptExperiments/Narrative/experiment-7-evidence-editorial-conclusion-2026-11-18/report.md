# Narrative Experiment 7 — Evidence versus Editorial Conclusion

**NO-GO na fase 1.** Variant F hield in alle drie Digital-briefs de gebruikscijfers in de rol van bewijs en eindigde bij de gekozen werkwijze. Alle drie Webb-briefs van F plaatsten de afkoeling na volledige uitvouwing. Eén Digital-brief voegde bovendien een causaal verband toe. De bevroren doorgangspoort blokkeerde Wolf, Lowell en de LinkedIn-proef. [Protocol](protocol.md) · [Exacte B→F-diff](prompts.diff) · [Beslissing per criterium](phase1-decision.json)

| Briefs met score 2 | B | F |
|---|---:|---:|
| Focus | 3/6 | 6/6 |
| Scheiding tussen bewijs en conclusie | 3/6 | 6/6 |
| Verhaalprogressie | 3/6 | 4/6 |
| Concrete details | 4/6 | 6/6 |
| Brontrouw | 4/6 | 2/6 |
| Handoff naar Compose | 6/6 | 5/6 |
| Volledig voldoende | 1/6 | 1/6 |

- **Digital.** In Experimenten 5 en 6 eindigden de varianten D en E steeds bij de cijfers. Dat gebeurde ook bij alle drie actuele B-briefs. F zette de gerapporteerde cijfers in Evidence/Observed Results en legde in Editorial Conclusion het accent op gebruikersvolgorde en tijd voor onderzoek en testen tijdens private beta. Twee van de drie conclusies herhaalden grotendeels de opening. In r3 suggereerde de opening met ‘lanceerde door’ bovendien dat de werkwijze de lancering verklaarde, hoewel de brief die causaliteit elders zelf uitsloot. Brontrouw daalde in dat paar van 2 naar 0, en handoff van 2 naar 1 door de als opdracht geformuleerde opening. [Digital B r3](calls/digital-r3-B/content.md) · [Digital F r3](calls/digital-r3-F/content.md) · [Synthesis](cases/digital/synthesis-result.json)
- **Webb.** F behield de vouwreden en het onderscheid tussen mechanisch ontplooid en wetenschappelijk gereed. Toch plaatste F de afkoeling in r1 en r3 in de conclusie na volledige ontplooiing, en in r2 zowel in Development als impliciet in de conclusie. In de bron begint afkoeling in de schaduw van het zonneschild, voor de spiegelontplooiing voltooid is. Synthesis noemt afkoeling in maand één zonder het precieze begin ten opzichte van de uitvouwing te geven. Twee B-briefs gingen ook te ver. In r1 scoorde B 2 en F 0 op brontrouw. [Webb B r1](calls/webb-r1-B/content.md) · [Webb F r1](calls/webb-r1-F/content.md) · [Webb F r2](calls/webb-r2-F/content.md) · [Webb F r3](calls/webb-r3-F/content.md) · [Bron](cases/webb/source.md) · [Synthesis](cases/webb/synthesis-result.json)

De focussom steeg van 9/12 naar 12/12. De brontrouwsom daalde van 8/12 naar 4/12. F haalde één volledig voldoende Digital-brief en nul volledig voldoende Webb-briefs, tegenover de vereiste vijf van zes totaal en twee van drie per case. Beide gepaarde dalingen naar 0 waren bronfouten. De Digital-causaliteitsfout is nieuw ten opzichte van de actuele B-arm en kwam eerder voor in D. [Handmatige scores en tekstbewijs](phase1-scores.json) · [Gepaarde dalingen](phase1-decision.json)

| Arm | Calls | Inputtokens | Completiontokens | Totaal |
|---|---:|---:|---:|---:|
| B | 6 | 5.193 | 3.137 | 8.330 |
| F | 6 | 5.397 | 3.666 | 9.063 |

F gebruikte 8,8% meer tokens en bleef onder de vastgelegde grens van 10%. Alle twaalf GPT-6 Luna-responses hadden unieke ID’s. Instellingen en schema kwamen overeen en de userprompt was per paar identiek. De offline replay reproduceerde alle twaalf requests en geparseerde resultaten exact. De oplossing en runner bouwden met nul waarschuwingen en nul fouten. Alle 303 gevonden tests slaagden. [Volgorde en instellingen](config.json) · [Live log](live-phase1.txt) · [Replay](replay-phase1.txt) · [Verificatie](verification.txt) · [Fixtures en controle](baseline-verification.txt) · [Build](build.txt) · [Runnerbuild](runner-build.txt) · [Tests](tests.txt) · [Doorgangspoort](phase2-gate-check.txt)

De uitvoering gebruikte twee doelgericht gekozen cases met drie herhalingen per arm. De uitvoerende assistent beoordeelde de briefs handmatig met zicht op de variantlabels. De proef geeft daarmee aanwijzingen dat een eigen bewijsblok Digital helpt de focus vast te houden. Of productie-Compose die scheiding benut, blijft onbeproefd. De brief bleef één Content-string in het bestaande contract. Productiecode, productieprompts en upstream-stappen bleven ongewijzigd. [Reproduceren en pauzeren](REPLAY.md)

Experimentcommit 69c9474792436edfd487686fca38c55e4502dda2. [GitHub-commit](https://github.com/antoine271167/Nueyon.Compose/commit/69c9474792436edfd487686fca38c55e4502dda2)
