# Laatste begrensde modelvergelijking — 12 november 2026

Advies: overstappen op gpt-6-luna voor de Simplified Idea Generator. GO voor gecontroleerde Idea→Research-overdracht van deze drie eindideeën. Dit bewijst geen algemene betrouwbaarheid van onbeheerde productie of downstreamresultaten.

## Uitvoering

Exact drie echte bestaande CLI-workflows, alleen run idea, volgorde intro, 005, 010. Geen extra generatietests, promptvarianten, experimentframework of verdere pipeline. Configuratie bevat gpt-6-luna; user secrets bevatten een geldige geneste OpenAI:ApiKey. De metadata-aanvraag gaf HTTP 200; succesvolle workflows bevestigen nu ook generatie- en judge-toegang. De eerdere 401 kwam uit de onjuiste controle die deze geneste sleutel miste.

Repository C:\Workspaces\Nueyon.Compose, commit 1f5abacfda3967776a05be3141972e4d61122cb2. Bronbestanden exact uit PromptExperiments/IdeaGenerator/legacy-simplified-acceptance-2026-11-12: intro-input.json, 005-input.json, 010-input.json. Geen herschreven bronnen. Intro is de reeds bevroren reconstructed brief, geen nieuwe of oorspronkelijke onbewerkte auteursbron.

Bestaande CLI gebouwd zonder codewijzigingen, nul fouten/waarschuwingen. Uitgevoerd vanuit repositoryroot via dotnet src/Nueyon.Compose.Host.Console/bin/Debug/net10.0/Nueyon.Compose.Host.Console.dll run idea --input <bevroren JSON> --output <nieuw resultaat> --trace <nieuwe trace> --prompts C:\Workspaces\Nueyon.Compose\src\Nueyon.Compose.Application\Prompts. Alleen procesvariabelen OpenAI__Model=gpt-6-luna en IdeaWorkflow__Mode=Simplified ingesteld. Geen user-secret overrides voor model of mode; generator en judge gebruiken beide dezelfde modeloptie. Stderr bevestigt Simplified, maximaal twee generaties. Productieprompts identiek aan het eerdere manifest.

## Side-by-side

GPT-5-mini-resultaten en beoordelingen overgenomen uit de bestaande comparison.json en report.md, geen nieuwe baseline-runs.

| Case | GPT-5-mini Simplified: judge / broncontrole | Luna: judge | Herstel | Calls gen+judge | Eindstatus | Broncontrole / Research |
|---|---|---|---:|---:|---|---|
| Intro | direct akkoord / GO met publiekskanttekening | direct akkoord | 0 | 1+1 | accepted, exit 0 | PASS / GO |
| 005 | direct akkoord / NO-GO: causaliteit en herhaalbaarheid te sterk | direct akkoord | 0 | 1+1 | accepted, exit 0 | PASS / GO |
| 010 | direct akkoord / NO-GO: Narrative-interventie te vroeg gekozen | afwijzing, daarna akkoord | 1 | 2+2 | accepted, exit 0 | eerste poging FAIL, hersteld PASS / GO |

Luna: 3/3 uiteindelijke ideeën bruikbaar; GPT-5-mini: 1/3 met kanttekening. Luna initieel 2/3 bruikbaar. Automatische eindacceptatie is bij beide 3/3; alleen inhoudelijke broncontrole maakt het verschil zichtbaar.

## Geselecteerde eerste ideeën en onafhankelijke broncontrole

### Intro — A broad AI-first ambition, grounded in one concrete starting point

Redactionele intentie PASS: Nuëyon en de AI-first doelgroep blijven centraal; Compose is het concrete begin van een bredere, nog ontwikkelende visie. Bronsecties 1, 3, 10, 12–14 ondersteunen deze verhouding. Geen extra investeerders/partners als doelgroep, anders dan de eerdere mini-uitvoer. Causaliteit PASS: geen bewezen effect of marktvalidatie. Interventieprioriteit PASS: de roadmap wordt gevolgd, geen nieuwe prioriteit toegevoegd. Invalshoek PASS: brede ambitie versus gefocust begin is een bruikbare, specifieke spanning. Research GO.

### 005 — A reusable editorial rule did not preserve the intended hierarchy

Redactionele intentie PASS: de concrete negatieve proef en rollback blijven centraal. Causaliteit PASS: 'In this comparison', 'in this test' en 'not proof ... in every setting' beperken de claim tot deze steekproef; geen 'can reliably alter' of 'repeatable degradations' zoals bij mini. De beperking van niet-gelijktijdige runs en upstreamvariatie wordt niet volledig herhaald, maar ook niet tegengesproken of weggepoetst door een sterke causaliteitsclaim. De bron ondersteunt lagere gemiddelden en falende kandidaatgates. Interventieprioriteit PASS: beschrijft de bestaande afwijzing/rollback zonder een nieuwe ingreep als bewezen oplossing te kiezen. Invalshoek PASS: soepel leesbare uitvoer kan toch de beoogde hiërarchie verliezen. Research GO, met behoud van alle bronbeperkingen bij verdere uitwerking.

### 010 — Pipeline drift has no single, proven starting point

Eerste poging: NO-GO. De description stelde dat de eerste zichtbare afwijking van de company-first hiërarchie ook in Compose voorkwam. De geciteerde Compose-case betreft een feitelijke overclaim terwijl de hiërarchie company-first bleef. De judge wees precies deze unsupportedClaim in description af en gaf de juiste bronpassage en herstelgrens mee.

Herstel: dezelfde titel en invalshoek behouden; Compose verwijderd uit de lijst van eerste hiërarchieafwijkingen. Evidence maakt daarna expliciet onderscheid tussen de Compose-feitelijke fout en hiërarchie. Redactionele intentie PASS: diagnostische lokalisatie met bewijsgrenzen. Causaliteit PASS: eerste zichtbaarheid wordt geen universele oorzaak. Interventieprioriteit PASS: geen Narrative-first interventie of opbrengst voorgeschreven. Invalshoek PASS: meerdere waargenomen startpunten en geen bewezen universele oorzaak vormen een concrete diagnostische invalshoek. Research GO na herstel.

Deze broncontrole is onafhankelijk van de automatische judge-uitspraak, maar handmatig uitgevoerd door dezelfde assistent, niet extern of geblindeerd. Volledige eerste en herstelde ideeën plus judgefeedback staan in comparison.json en de traces.

## Calls, kosten en grenzen

Exact 3 workflows, 4 generaties en 4 judgecalls: 8 logische modelcalls. Eén herstel binnen de bestaande Simplified-workflow. Mini gebruikte voor deze drie Simplified-cases 6 calls zonder herstel. Gemeten Luna-procesduur: intro 15,27 s; 005 21,71 s; 010 37,09 s; totaal circa 74,06 s. Geen callbesparing aangetoond. De CLI exporteert geen tokens, factuurbedrag of transportherpogingen; die blijven onbekend. Metadata-aanvragen zijn geen generatiemodelcalls. Ruwe judge-providerrespons wordt niet opgeslagen; de trace bewaart pogingstatus en afwijzingsfeedback.

Deze kleine steekproef levert geen statistisch bewijs. Eén workflow per bron/model, niet gelijktijdig of gerandomiseerd, toont geen algemene superioriteit of foutloosheid. Er is geen volledige pipeline of feitelijke Research-uitvoering getest. GO betreft de bruikbaarheid van de drie eindideeën als Research-input, met broncontrole bij overdracht.

## Integriteit en artefacten

3440 bestanden gecontroleerd: alle gevolgde repositorybestanden en alle bestanden uit de bestaande vergelijkingsmap zijn byte-identiek aan de momentopname voor de runs. Promptmanifest matcht. De reeds aanwezige wijziging in src/Nueyon.Compose.Host.Console/appsettings.json is behouden. Geen code-, prompt-, bron- of configuratiebewerking; geen bestaande resultaten overschreven, geen commit/push.

Nieuwe resultaten staan uitsluitend in deze aparte map buiten de repository. Per case: resultaat, trace, geselecteerd eindidee, metadata, stdout/stderr. comparison.json bevat volledige side-by-side-resultaten en vijf beoordelingsaspecten; before-hashes.json en verification.json ondersteunen de integriteitscontrole. Deze ronde is afgerond: geen verdere runs uitgevoerd.
