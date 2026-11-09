# Idea Generator Experiment 003

## Opzet

Kandidaat 001 behoudt de Idea-systemprompt byte-identiek en wijzigt alleen de userprompt. De toevoegingen vragen om herkenning van het hoofdwerk versus licenties en metadata, brongebonden formuleringen zonder onbewezen causaliteit, en een concrete lezersbelofte binnen het redactionele doel. De placeholder is aangepast naar PromptRunner-syntaxis. De vijf bronnen zijn ongewijzigd.

## Uitvoering en vergelijkbaarheid

Experiment 001 gebruikte vrije tekst zonder productievalidatie. De Idea-modus van PromptRunner gebruikt de productie-SourceContextLoader, OpenAI-chatclient, IdeaResponse-schemavraag, productie-IdeaValidationLoopEvaluator en IdeaValidator in een LoopAgent met maximaal drie iteraties. De runner deserialiseert het eindantwoord en selecteert index 0 zoals StoryWorkflow. De structurele validator beoordeelt geen inhoudelijke brontrouw. Productie laadt ingebedde promptbronnen, terwijl deze test losse bestanden rendert. De runner bewaart iedere poging, tokengebruik en het geselecteerde idee.

Baseline-002 doorliep Idea, Research, Synthesis en Narrative in Compose en slaagde voor 1 van 15 runs. Deze screening draaide alleen Idea. De resultaten zijn dus niet rechtstreeks gelijkwaardig en leveren geen Narrative-check op. Scores zijn één beoordelaarsinschatting volgens acceptance-v1/rubric.md.

Provider OpenAI, API-modelalias gpt-5-mini, standaard samplinginstellingen. PromptRunner-build geslaagd. Preflight en runbestanden bevatten commit, hashes, configuratie en ruwe uitvoer.

## Resultaten

| Bron | Geselecteerd idee | Brontrouw | Doel | Focus | Invalshoek | Aantrekkingskracht | Totaal | Tokens | Kostenindicatie USD |
| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: |
| [Productstrategie](runs/01-strategy-r1/evaluation.json) | Nuëyon als AI-first bedrijf met Compose als eerste concrete stap | 1 | 1 | 2 | 1 | 1 | 6 | 11.113 | 0,00651 |
| [Ontwikkelverslag](runs/02-development-r1/evaluation.json) | Een generieke regel voor redactionele intentie kan uitvoer minder trouw maken | 0 | 2 | 2 | 2 | 2 | 8 | 11.190 | 0,00669 |
| [Productdocumentatie](runs/03-product-docs-r1/evaluation.json) | Een keuzehulp voor officiële versus community-installatieroutes | 1 | 2 | 1 | 1 | 2 | 7 | 11.753 | 0,00699 |
| [Diagnostische analyse](runs/04-analysis-r1/evaluation.json) | Narrative als terugkerende plek om hiërarchiedrift te beheersen | 0 | 2 | 2 | 2 | 2 | 8 | 11.809 | 0,00742 |
| Historische rede | Niet uitgevoerd onder de ernstige-foutstopregel | — | — | — | — | — | — | — | — |

Vier runs zijn uitgevoerd. Twee bevatten ernstige brontrouwfouten. Het ontwikkelverslag generaliseerde de uitkomst van een kleine proef tot een algemene causale les. De analyse veranderde de vaakst waargenomen eerste driftlocatie in een voorkeursinterventie, hoewel de bron interventies niet vergeleek. Beide fouten falen de harde brontrouwgate. De vier Ideas halen geen acceptance-drempel. De Idea-only screening geeft geen uitspraak over Narrative-succes. Volgens het protocol blijft de volledige acceptatie met één onuitgevoerde run onbeslist. Kandidaat 001 wordt op basis van deze screening afgewezen voor verdere promotie.

Run 5 voor de historische rede is niet uitgevoerd omdat het protocol na een ernstige fout zegt eerder te stoppen. Geen iteraties of nieuwe runs na run 4.

Totale modelinzet was 45.865 tokens, met een geschatte API-kost van USD 0,0276. De indicatie rekent het SDK-tokengebruik tegen de standaard gepubliceerde tarieven voor ongecachete invoer, gecachete invoer en uitvoer. De provider retourneerde geen gefactureerd bedrag. [OpenAI gpt-5-mini-tarieven](https://developers.openai.com/api/docs/models/gpt-5-mini).

## Eindcontrole

De vier uitgevoerde processen eindigden met code 0 en elk gebruikte één modelpoging. Ruwe respons, selectie, tokengebruik, invoer en promptartefacten zijn per run bewaard. De vijf velden van elk geselecteerd idee zijn gecontroleerd tegen `ideas[0]`. De vijf manifestbronnen en hashes zijn vooraf gecontroleerd. Geen productieprompt of productiecode is gewijzigd. Geen commit of push.
