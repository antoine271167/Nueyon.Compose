# Narrative Experiment 1 — productiebaseline

Op 18 november 2026 zijn twaalf runs voltooid met de echte Narrative-agent, beide productieprompts van commit 918eff3 en GPT-6 Luna. **3/12 briefs scoorden op alle vijf assen voldoende.** Alle twaalf behielden het gekozen onderwerp en gaven concrete bouwstenen. Brontrouw was de zwakste as: drie Wolf-runs verwisselden dezelfde soort. Webb gaf in alle drie de runs een te stellige afkoelvolgorde.

| Case | Volledig voldoende | Hoofdbevinding |
|---|---:|---|
| Webb | 0/3 | Het onderscheid tussen uitvouwing in week twee en routinewetenschap na zes maanden bleef staan. Alle drie plaatsten de afkoeling te stellig ná de voltooide uitvouwing. |
| Wolf | 0/3 | Concrete roedel-, voedsel- en prooirelaties, met behoud van de meervoudige ecologische context. In alle drie werd het centrale prooidier verkeerd vertaald. |
| Digital | 1/3 | De volgorde van gebruikersgroepen en de testtijd bleven centraal beschikbaar. Het endpoint verschoof in r1 naar resultaten en herhaalde in r3 de aanpak. |
| Lowell | 2/3 | Alle drie bouwden de lijn op rond textielverval, erfgoed en hergebruik, gevolgd door terugval. R3 formuleerde de bijdrage aan de opleving stelliger dan de input. |

Aantal scores van 2 per as: verhaallijn 10/12, focus 11/12, concrete details 12/12, brontrouw 4/12, format-agnostische overdraagbaarheid 10/12. Die laatste as beoordeelt structuur en redactionele bruikbaarheid van de brief. Voor een handoff die ook feitelijk voldoende is, geldt het gezamenlijke resultaat van 3/12. Geen van de briefs nam het orchestratorverhaal uit de productie-userprompt over. [Scores en onderbouwing per run](scores.md)

## Waar de afwijkingen ontstonden

- **Webb.** De bewaarde Synthesis bundelt afkoeling, uitlijning en kalibratie globaal over de zes maanden. In punt 4 specificeert zij afkoeling in maand één; de bron laat tijdens uitvouwing al afkoeling zien. Narrative maakt hier een scherpere volgtijdelijkheid van. De globale bundeling en het losse slotwoord in de Synthesis zijn upstream; het slotwoord verschijnt in geen enkele brief. [Synthesis](cases/webb/synthesis-result.json) · [Bron](cases/webb/source.md)
- **Wolf.** Zowel Synthesis als Research-projectie gebruiken het Engelse *elk*. In de Yellowstone-context verwijst dat naar wapiti. Narrative maakt er *eland* van in de prooiverschuiving en bij predatie-effecten. De drie herhalingen tonen dezelfde nieuwe fout op exact dezelfde input. [Synthesis](cases/wolf/synthesis-result.json) · [Research-projectie](cases/wolf/projection.md)
- **Digital.** De Synthesis wisselt tussen 'na twaalf weken' en 'in twaalf weken' en noemt geen startpunt. Alle briefs gebruiken 'na' in hun samenvatting zonder een startpunt te specificeren. R1 maakt zelf van een gevraagde termijn een initiële inschatting en kiest een cijfergedreven einde; r3 herhaalt de werkwijze. Geen run schrijft de lancering of cijfers toe aan de werkwijze. [Synthesis](cases/digital/synthesis-result.json) · [Bron](cases/digital/source.md)
- **Lowell.** Synthesis beschrijft hergebruik naast stijgende werkgelegenheid en bedrijfsactiviteit, met andere factoren in beeld. De formulering 'brought renewed activity' in de samenvatting van r3 trekt de relatie stelliger. De drie briefs behouden de latere terugval en de waarschuwing tegen een monocausale uitleg. [Synthesis](cases/lowell/synthesis-result.json) · [R3](calls/lowell-r3/content.md)

## Opzet, verbruik en controles

De vier fixtures zijn eerder gegenereerde Synthesis-outputs uit Experiment 4 arm A, gekozen als bruikbare upstream-baseline met enkele bestaande rafelranden. Elke fixture draaide drie keer als zelfstandige aanvraag. De runner riep de echte Narrative-agent aan met het productieschema en de ingebedde productieprompts, met een experimentele limiet van 5.000 completiontokens en zonder retries. Research, Synthesis en Compose draaiden tijdens de proef niet. De rubric, runvolgorde, runner, prompts en fixtures waren vóór de eerste call bevroren. [Protocol](protocol.md) · [Configuratie](config.json) · [SHA-256](freeze.json)

| Runs | Inputtokens | Completiontokens inclusief reasoning | Totaal |
|---:|---:|---:|---:|
| 12 | 88.824 | 8.386 | 97.210 |

Per call zijn input, exact API-request, ruwe response met usage en geparseerde brief opgeslagen. Alle twaalf requests gebruikten de verwachte gerenderde prompts en fixtures. Alle twaalf antwoorden zijn offline identiek geparseerd via de productieagent. De runner bouwde zonder waarschuwingen en 34 gerichte Narrative- en StoryWorkflow-tests slaagden. [Usage per run](usage.json) · [Verificatie](verification.txt) · [Testlog](tests.txt) · [Herhalen](REPLAY.md)

De uitvoerende Work-assistent heeft de twaalf briefs handmatig beoordeeld met zicht op de cases. Vier doelgericht gekozen fixtures, één upstreamvariant per case, drie herhalingen en het ontbreken van onafhankelijke beoordeling beperken de generaliseerbaarheid. Article- en LinkedIn-bruikbaarheid is beoordeeld aan de hand van de briefs. Er is geen downstream Compose-output gemeten en er is geen vergelijkingsarm waarmee effecten aan één promptpassage kunnen worden toegeschreven.

De werkboomwijzigingen zijn beperkt tot deze experimentmap. De resultaten zijn lokaal opgeslagen.
