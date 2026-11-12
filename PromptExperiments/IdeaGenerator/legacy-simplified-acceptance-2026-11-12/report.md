# Legacy versus Simplified — beslissende Idea-acceptatieronde

12 november 2026. Advies: **NO-GO voor automatische overdracht naar Research; kies Simplified als beste basis.** Simplified bewaart bij de introductie het bedrijfsverhaal beter. Legacy is nergens duidelijk beter. Eén werkelijk blokkerend probleem: de semantische gate accepteert ideeën die bronobservaties verheffen tot causale conclusies of interventieprioriteit. Er zijn geen wijzigingen uitgevoerd om dit te herstellen.

## Uitvoering en grenzen

- Exact zes echte CLI-runs op commit `defde1076f3abecb7b7935e282a17d59a9d4af3d`, GPT-5-mini, uitsluitend `run idea`, bestaande productiepromptboom expliciet meegegeven. Geen Research of verdere pipeline uitgevoerd.
- Volgorde: intro Legacy/Simplified, 005 Legacy/Simplified, 010 Legacy/Simplified. Per case exact dezelfde JSON-broninhoud. Manifest bevat absolute bronpaden en SHA-256-hashes.
- Intro gebruikt de oorspronkelijke reeds aanwezige bevroren **reconstructed** bronbrief uit de introductie-experimenten, geen nieuw geschreven bron. Het archief noemt die reconstructie expliciet; geen claim van toegang tot een eerdere onbewerkte auteursbron. De bron bevat missie, producten/capabilities, plan en roadmap. De aparte original-instruction.txt is experimentinstructie en is niet als bron toegevoegd.
- API-credential aanwezig; GET modelmetadata gaf toegang tot gpt-5-mini; de zes geslaagde runs bevestigen generatie-toegang. Build geslaagd, nul fouten/waarschuwingen. Accountsaldo/bestedingslimiet niet uitleesbaar. Vooraf begrensd op maximaal 30 logische modelcalls, exclusief providertransportherpogingen; geen extra betaalde proefcalls.
- Modus alleen per proces via de gedocumenteerde omgevingsvariabele geselecteerd; model gpt-5-mini. Geen C#, configuratiebestanden of prompts gewijzigd. Geen commits/pushes. Eindcontrole: prompts en bronbestanden ongewijzigd.

## Vergelijking

Brontrouw is een inhoudelijke beoordeling tegen de originele bron van elke case, los van judge-acceptatie. Invalshoek beoordeelt of het idee interessant en specifiek is; een aantrekkelijke maar onjuiste formulering kan daarop slagen en toch niet naar Research mogen.

| Case | Modus | Judge | Reparaties | Calls (gen+judge) | Eindstatus | Brontrouw | Invalshoek | Naar Research |
|---|---|---|---:|---:|---|---|---|---|
| Intro | Legacy | direct akkoord | 0 | 1+1 | accepted / exit 0 | onvoldoende: orchestration-first verdringt bedrijfsintro | specifiek | nee |
| Intro | Simplified | direct akkoord | 0 | 1+1 | accepted / exit 0 | voldoende, publiekskanttekening | specifiek | ja |
| 005 | Legacy | direct akkoord | 0 | 1+1 | accepted / exit 0 | onvoldoende: causale attributie | specifiek | nee |
| 005 | Simplified | direct akkoord | 0 | 1+1 | accepted / exit 0 | onvoldoende: causaliteit en herhaalbaarheid te sterk | specifiek | nee |
| 010 | Legacy | direct akkoord | 0 | 1+1 | accepted / exit 0 | onvoldoende: Narrative-prioriteit en opbrengst onbewezen | specifiek | nee |
| 010 | Simplified | direct akkoord | 0 | 1+1 | accepted / exit 0 | onvoldoende: Narrative-interventie te vroeg gekozen | specifiek | nee |

## Geselecteerde eerste ideeën en broncontrole
### intro-Legacy

**Nuëyon.Compose as the concrete expression of Nuëyon’s orchestration-first ambition**

Orchestration-first wordt bedrijfsambitie en hoofdverhaal; bronsecties 12–13 verlangen bedrijfsidentiteit eerst. Extra publiek (early partners/observers) is niet expliciet onderbouwd.

### intro-Simplified

**An AI-first company building reusable products for other AI-first businesses**

Bedrijfsintroductie en herbruikbare producten voor AI-first bedrijven blijven centraal. Audience noemt ook investeerders/partners: ruimere lezersrelevantie, geen bewezen doelgroep; niet als marktfeit overnemen. Geen blokkade van het centrale verhaal.

### 005-Legacy

**A small, generic ‘preserve editorial intent’ prompt made outputs drift and failed promotion**

Titel “prompt made outputs drift” en rationale “direct evidence ... constraints can change” schrijven de verschillen causaal toe aan de wijziging. Bron meldt kleine niet-gelijktijdige vergelijking, upstreamvariatie en modelaliasvariatie.

### 005-Simplified

**Candidate prompt change displaced the intended editorial hierarchy and triggered a rollback**

“produced ... repeatable degradations” en “can reliably alter” overstijgen drie kandidaat-runs en de niet-gelijktijdige vergelijking. Evidence noemt correlatie, maar herstelt de sterkere titel/description/rationale niet.

### 010-Legacy

**Narrative is the recurrent first-visible locus of pipeline hierarchy drift**

“likeliest to succeed”, “Prioritizing Narrative fixes limits rework” maken zichtbare frequentie tot interventieprioriteit en onbewezen opbrengst. Bron vereist lokalisatie voor de gekozen afwijking en vergelijking met feitelijke upstream-invoer.

### 010-Simplified

**Narrative is the most frequent visible breaking point for company-first hierarchy**

Narrative als “locus to target” en correctieve trial/guardrails als evidence-supported intervention worden generiek gekozen. Bron maakt die keuze afhankelijk van bewijs voor de gekozen afwijking en inspectie van upstream Synthesis; root-causevoorbehoud in evidence maakt deze prioriteit niet brongetrouw.

## Kosten, bewijs en advies

**12 waargenomen logische modelcalls: 6 generator + 6 judge**, gelijk verdeeld over de modi (6 per modus). Geen herstelcalls en geen extra iteraties. Totale gemeten procesduur ongeveer 224 seconden; Legacy 124 s, Simplified 100 s. Tokens, factuurbedrag en transportherpogingen worden door deze CLI niet geëxporteerd en zijn dus onbekend; geen schijnprecisie in euro's of tokens. De modelmetadata-check is geen generatiecall. Simplified heeft hier geen callbesparing aangetoond omdat alles direct werd aangenomen; het theoretische maximum is wel lager (4 versus 6 per run).

Alle zes judge-besluiten zijn acceptance zonder feedback. Geen live afwijzing of reparatie geobserveerd; reparatiekwaliteit blijft dus ongetest. De traces leggen de pogingstatus vast, niet de ruwe judge-providerrespons. Volledige eerste ideeën, alle kandidaten, traces, stdout/stderr, tijdmeting en onafhankelijke beoordeling zijn bewaard. `comparison.json` bevat de volledige geselecteerde eerste ideeën en beoordelingen; `*-selected.json` biedt elk eerste idee apart.

Simplified scoort 1/3 Research-bruikbaar versus Legacy 0/3. Kies **Simplified als basis**, maar geef **geen GO voor onbeheerde productieoverdracht**: op beide bekende valkuilen faalt de brontrouw terwijl de judge akkoord geeft. Geen conclusie van statistische superioriteit of herhaalbaarheid op één run per combinatie. De beoordeling is handmatig door dezelfde uitvoerende assistent, niet extern of geblindeerd. Er is uitsluitend Idea getest; “Research-bruikbaar” is een beoordeling van de input, geen bewezen downstreamresultaat.

Deze ronde is afgerond en gestopt; geen vervolgexperimenten of wijzigingen uitgevoerd.
