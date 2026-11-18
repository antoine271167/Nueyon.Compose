# Narrative Experiment 8 — Fidelity Guarded Editorial Conclusion

**NO-GO na fase 1.** G hield in alle Digital-briefs de werkwijze centraal en de cijfers in de rol van bewijs. Alle drie G-briefs over Webb plaatsten afkoeling expliciet of impliciet na volledige ontplooiing. Geen G-conclusie leverde een voldoende onderscheidend onderbouwd inzicht. De vastgelegde poort blokkeerde Wolf, Lowell en echte LinkedIn Compose-calls. [Protocol](protocol.md) · [F→G-diff](prompts.diff) · [Beslissing](phase1-decision.json) · [Poortcontrole](phase2-gate-check.txt)

| Briefs met score 2 | Controle F | Variant G |
|---|---:|---:|
| Focus | 6/6 | 6/6 |
| Niet-repetitieve, onderbouwde conclusie | 0/6 | 0/6 |
| Structuur | 6/6 | 6/6 |
| Concrete details | 6/6 | 5/6 |
| Brontrouw | 3/6 | 3/6 |
| Handoff | 3/6 | 2/6 |
| Volledig voldoende | 0/6 | 0/6 |

- **Digital.** F en G scoorden beiden 3/3 op focus en brontrouw. De causaliteitsfout uit Experiment 7 F r3 kwam in geen van beide verse Digital-armen terug. De conclusies omschreven opnieuw de gebruikersvolgorde en gereserveerde onderzoekstijd uit de opening. G r3 liet het onderscheid tussen de sprintweken weg, met gepaarde dalingen van 2 naar 1 voor details en handoff. De beoordeling telde de expliciet door Synthesis ondersteunde samenloop van testen, onderzoek en aanvragen aannemen binnen de eerste week als parallel werk. Het specifiek parallel bouwen van case management en itereren van de frontend staat wel in de projectie, maar ontbreekt in de Synthesis-input en is daarom niet vereist of aangevuld. [G r3](calls/digital-r3-G/content.md) · [F r3](calls/digital-r3-F/content.md) · [Synthesis](cases/digital/synthesis-result.json) · [Projectie](cases/digital/projection.md)
- **Webb.** Synthesis noemt twee beschikbare tijdvensters die elkaar kunnen overlappen: volledige ontplooiing in week twee en enkele weken afkoeling in maand één. De bron plaatst het begin van afkoeling in de schaduw van het zonneschild, met temperatuureffecten op dagen 3–10, voor de afronding van spiegelontplooiing. Synthesis laat het precieze begin weg. Narrative vult die upstream leemte ten onrechte in. Bij G r1 gebeurt dat met “after the telescope had unfolded” en “still lay ahead”. G r2 plaatst de volledige zes maanden na week twee in de opening. In G r3 gebeurt het indirect via Evidence, waar de maanden van ingebruikname na de configuratie komen, terwijl Development en het slot die periode inclusief afkoeling beschrijven. Ik beoordeelde G r2 als dezelfde materiële foutfamilie met een extra verschuiving van de duur, en G r3 als strikt impliciet bij lezing van de volledige brief. Alle drie actuele F-briefs maken dezelfde volgordefout. Ook bij een mildere beoordeling van de impliciete gevallen blijft de poort gesloten. [G r1](calls/webb-r1-G/content.md) · [G r2](calls/webb-r2-G/content.md) · [G r3](calls/webb-r3-G/content.md) · [Synthesis](cases/webb/synthesis-result.json) · [Bron](cases/webb/source.md)

G verhoogde de conclusiesom van 4/12 naar 5/12, doordat bij G r3 over Webb de extra causale bewering in het F-slot verviel. De tijdsfout bleef in de G-brief staan. De brontrouwsom bleef 6/12 en voldeed daarmee niet aan de verplichte verbetering. Geen enkele dimensie daalde gepaard naar 0. [Handmatige scores en tekstbewijs](phase1-scores.json) · [Gepaarde dalingen](phase1-decision.json)

| Arm | Calls | Inputtokens | Completiontokens | Totaal | Geschatte Standard-kosten |
|---|---:|---:|---:|---:|---:|
| F | 6 | 5.397 | 3.813 | 9.210 | $0,002446 |
| G | 6 | 5.619 | 3.509 | 9.128 | $0,002132 |
| Samen | 12 | 11.016 | 7.322 | 18.338 | $0,004578 |

G gebruikte 0,9% minder tokens. De schatting gebruikt de gerapporteerde cachetokens en publieke Standard-tarieven. De werkelijke factuur is niet ingezien. Alle twaalf GPT-6 Luna-responses hadden unieke ID’s. Per paar waren de userboodschap en modelinstellingen identiek en wisselde de startarm gebalanceerd over de fase. Offline replay reproduceerde twaalf requestbodies en geparseerde resultaten exact. De oplossing en runner bouwden zonder waarschuwingen of fouten. Alle 303 ontdekte tests slaagden. [Usage](phase1-usage.json) · [Kosten per call](phase1-costs.json) · [OpenAI-prijzen](https://developers.openai.com/api/docs/pricing) · [Live-log](live-phase1.txt) · [Offline replay](replay-phase1.txt) · [Verificatie](verification.txt) · [Build](build.txt) · [Runnerbuild](runner-build.txt) · [Tests](tests.txt)

De uitvoerende assistent scoorde de twaalf briefs handmatig met zicht op de arm. De proef bevat twee doelgericht gekozen cases met drie herhalingen per arm. Het resultaat ondersteunt een stop voor G onder dit protocol. De ene nieuwe zin combineert een opdracht voor de conclusie met een algemene trouwregel, waardoor de proef hun afzonderlijke bijdragen niet kan isoleren. De handoffscores beoordelen de brief als invoer voor Compose en zijn geen meting van LinkedIn-posts. Productieprompts, productiecode, Compose, upstream agents en contracten zijn byte voor byte behouden. [Reproduceren en pauzeren](REPLAY.md)

## Publicatie

Branch `narrative/experiment-8-fidelity-guarded-editorial-conclusion`. De hash van de artefactcommit wordt in een tweede commit aan dit rapport toegevoegd.

