# Experiment v3 · Idea Agent

**Besluit op 10 november 2026.** Experiment afgerond. Formele acceptatie **1/3**, holdouts **2/3**, volledig brongetrouwe feedbackreeksen **0/3**. De kandidaat is afgekeurd en productie is intact.

## Diagnose op v1 en v2

| Geval | Waar v2 afweek | Grens voor v3 |
| --- | --- | --- |
| 005 | De validator verwierp eerst de generalisatie naar een klasse promptregels. Zijn vervangende titel schreef de lagere score vervolgens toe aan de toevoeging. De generator gebruikte die titel en de validator accepteerde poging 2. Het oorspronkelijke rapport vergelijkt drie eerdere baselineruns en drie nieuwe kandidaatruns. Het noemt variatie in eerdere fasen, mogelijke variatie achter de modelalias en spanning in Synthesis. | De lagere scores, de geobserveerde hiërarchie en de beslissing over de kandidaat zijn bruikbaar. De claim over de oorzaak gaat verder dan dit materiaal draagt, ook als de rest van het idee caveats bevat. |
| 010 | In v1 wees de validator de eerste stellig geformuleerde prioriteit af en gaf daarna zelf een Narrative-first-aanwijzing. In v2 accepteerde hij direct een idee dat de bron een expliciete Narrative-aanbeveling toeschreef. | Kies per concrete afwijking de eerste fase die het bewijs aanwijst. Als Narrative in beeld is, vergelijk met de werkelijke Synthesis-invoer vóór een wijziging. De meest voorkomende observatie mag wel als context worden vermeld. |
| Museum | De v2-generator maakte van een bezoekersbericht een evaluatiestuk voor museumteams. De uitgifte van 38 routekaarten werd het uitgangspunt van een verhaal over effectmeting, en de validator liet dat door. De negatieve judge-only controle testte een andere fout, namelijk een onbelegde tevredenheidsclaim. | Help de bedoelde bezoekers met de tijdelijke binnenroute bij hoogwater. Aantallen routekaarten zijn uitgiftecijfers. Het personeelsdetail is geen reden om publiek en opdracht te vervangen. |
| Strategie | V1 en v2 scoorden 10/10 voor de introductie van Nuëyon met Compose als eerste concrete productrichting. | Bewaar de bedrijfsintroductie, de in de bron afgebakende doelgroep en de vroege status. Laat orkestratie de productrichting toelichten. |

Bronnen en tekst van de eerdere uitkomsten staan in [v1](../production-cli-acceptance-v1/report.md) en [v2](../production-cli-acceptance-v2/report.md). Voor de drie fouten zijn de concrete eerste ideeën in [fixtures](fixtures/) vastgelegd, samen met de oorspronkelijke bestandsverwijzing en een brongetrouwe reparatie als positieve controle. De bekende v2-score is 1/3, ondanks 3/3 verwachte verdicts op opgeslagen v1-ideeën. Het museum scoorde 7/10 in de volledige v2-run.

## Kandidaat en uitvoering

De generator stelt eerst de redactionele opdracht vast en kiest daarbinnen een aantrekkelijke invalshoek. Hij leest de titel los van de overige velden en moet op elke nieuwe poging ook de voorgestelde correctie aan de bron toetsen. De validator reconstrueert per aanroep de opdracht, toetst brontrouw en doel afzonderlijk, en formuleert de kleinste brongebonden ingreep. De prompt vraagt hem de correctie te lezen alsof die letterlijk wordt toegepast voordat hij de feedback teruggeeft. De productiecode deed al een afzonderlijke beoordeling per poging met volledige bron en alleen het actuele eerste idee.

[pair-01](candidates/pair-01/prompts/) bevat de volledige diskpromptset van 14 bestanden. De generator gebruikt nieuwe systeem- en gebruikerstekst. De validator gebruikt nieuwe systeemtekst. De overige elf bestanden komen overeen met productie. De kandidaat was vastgelegd voordat de bibliotheekbron is opgesteld. De lokale checkout en remote-main wezen beide op revisie 83c732d330fbca4bebd4b2c239ef9ac55a471a39. Zie [prompthashes](verification/prompt-hashes.json), [invoerhashes](verification/input-hashes.json) en [v1/v2-hashes](verification/v1-v2-hashes.json).

Voer vanuit de repo-root uit. Voor betaalde fasen is vooraf toestemming nodig. Elke fase reserveert eenmalige slots, bewaart commando, revisie, SDK, tijd, hashes en exitcode, en overschrijft geen eerder resultaat.

```powershell
$v3 = 'PromptExperiments/IdeaGenerator/production-cli-acceptance-v3'
dotnet build src/Nueyon.Compose.slnx
dotnet build "$v3/harness/FeedbackLoop.csproj"
dotnet test src/Nueyon.Compose.slnx --no-build
& "$v3/run.ps1" -Stage offline
& "$v3/run.ps1" -Stage feedback -AuthorizeModelCalls
& "$v3/run.ps1" -Stage acceptance -AuthorizeModelCalls
& "$v3/run.ps1" -Stage holdout -AuthorizeModelCalls
# Bevestigingsrun voor een eventuele promotie, na onafhankelijke beoordeling.
& "$v3/run.ps1" -Stage confirm-holdout -AuthorizeModelCalls
```

- **Feedback** gebruikt de productie-loop met een vaste generator en een echte GPT-5-mini-validator. Elk van de drie foutpatronen bevat twee vastgelegde foute ideeën en een handmatig gerepareerd idee. Verwacht: afgewezen, afgewezen, geaccepteerd. Beoordeel de tekst van iedere live correctie afzonderlijk, inclusief implicaties wanneer de correctie letterlijk wordt toegepast. Een terechte blokkade met een nieuwe claim, nieuwe eis of koerswijziging faalt. De vastgelegde volgende poging is een controle op de herbeoordeling en wordt niet dynamisch uit het live advies afgeleid. De CLI-runs meten het gedrag van de echte generator en beoordelen iedere feitelijk opgetreden herpoging afzonderlijk.
- **Acceptatie** draait de echte productie-CLI met de oorspronkelijke drie bronnen, volledige diskpromptset, GPT-5-mini, maximaal drie Idea-pogingen en traces. Beoordeel alleen het definitieve `ideas[0]`, met alle vijf velden, voordat de trace en het validatorverdict de redactionele score kunnen beïnvloeden. Ken per dimensie 0 (mist), 1 (gedeeltelijk) of 2 (volledig) toe voor brontrouw, redactioneel doel, focus, onderscheidende invalshoek en aantrekkingskracht. Vereis 2/2 voor beide harde criteria, totaal ≥8/10 en 3/3. Voor de strategieregressie handhaven we ook het v1/v2-referentiepunt 10/10.
- **Holdouts** volgen dezelfde rubric en tellen apart. Museum is een bekende regressiebron die het ontwerp mede bepaalde. Vervoer is bekend uit v2 judge-only en nieuw voor een volledige productie-Idea-run. De synthetische bibliotheekbron is voor v3 opgesteld nadat de kandidaat was vastgelegd. Hij is ongezien voor de promptafstelling, maar geen extern of blind aangeleverd materiaal. Test of het idee de tijdelijke afhaalwijziging voor bestaande reserveringen bewaart en de interne oefening en suggestie over de zomer ondergeschikt houdt. Gebruik dezelfde bevroren kandidaat voor alle holdouts. Een herhaling van de bibliotheekbron meet stabiliteit op hetzelfde geval en telt als één unieke bron.
- **Promotie** vereist succesvolle feedbackcontroles inclusief de correcties, 3/3 formele acceptatie zonder strategieregressie, drie geslaagde holdoutbronnen en een geslaagde bevestigingsrun. Een volgende kandidaat die op deze resultaten wordt afgestemd heeft een nieuwe ongeziene bron nodig. Downstream kwaliteit en causaliteit van afzonderlijke promptwijzigingen vallen buiten dit vergelijkingsontwerp.
- **Overdracht** maakt na onafhankelijke acceptatie uitsluitend voor geslaagde ideeën een goedgekeurde `ResearchInput`. Neem de ongewijzigde `content` over in `input.content` en het eerste idee in `selectedIdea.idea`. Valideer tegen het [actuele CLI-schema](verification/research-schema.json). Bewaar afkeuringen als fixture met het oorspronkelijke resultaat, de trace en de beoordeling, zonder goedgekeurde Research-overdracht.

## Eindresultaat

Alle zes productieprocessen eindigden met exitcode 0 en een geaccepteerde CLI-trace. De redactionele beoordeling hieronder is gebaseerd op de inhoud van `ideas[0]` en de volledige bron, onafhankelijk van de acceptatiestatus van de validator. Voor de bibliotheekcasus was de acceptatiestatus al zichtbaar bij het uitlezen van de traces. De beoordeling was dus niet overal geblindeerd en is door dezelfde beoordelaar gedaan die de kandidaat en de nieuwe fixture opstelde.

| Reeks en bron | Brontrouw | Doel | Focus | Invalshoek | Aantrekkingskracht | Totaal | Pogingen | Oordeel |
| --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | --- |
| [Strategie](ratings/01-strategy.json) | 2 | 2 | 2 | 2 | 2 | **10/10** | 1 | Geslaagd |
| [005](ratings/02-development.json) | 1 | 1 | 1 | 2 | 1 | **6/10** | 3 | Afgekeurd |
| [010](ratings/04-analysis.json) | 1 | 1 | 1 | 2 | 2 | **7/10** | 2 | Afgekeurd |
| [Museum](ratings/holdout-01-museum.json) | 1 | 1 | 2 | 2 | 2 | **8/10** | 2 | Afgekeurd |
| [Vervoer](ratings/holdout-02-transit.json) | 2 | 2 | 2 | 2 | 1 | **9/10** | 1 | Geslaagd |
| [Bibliotheek](ratings/holdout-03-library.json) | 2 | 2 | 1 | 2 | 2 | **9/10** | 1 | Geslaagd |

De formele reeks eindigt op **1/3**, gelijk aan v1 en v2. Strategie blijft 10/10. De holdouts komen op **2/3** unieke bronnen. Het nieuwe bibliotheekgeval slaagt in één meting. Vervoer is een eerder bekende judge-only-bron en slaagt bij zijn eerste volledige CLI-test. Museum is een bekende regressiebron. De bevestigingsrun is overgeslagen omdat de andere promotievoorwaarden al waren gemist.

### Wat de feedbacklus deed

- **005.** De gecontroleerde feedbacktest herkende de twee onjuiste varianten en accepteerde de gerepareerde variant. De eerste correctie leverde echter opnieuw een titel met *reduced* en een rationale met *worsened*. In de echte productie-CLI maakte de eerste afwijzing de titel wél observationeel. De feedback voegde tegelijkertijd een nieuwe opdracht toe voor promptgovernance met grotere steekproeven, seed/temperatuur en Synthesis-controles. De tweede afwijzing betrof een checksum die niet in poging twee was genoemd. Poging drie leverde de juiste hash, maar behield de extra onderzoeksagenda. [Gecontroleerde feedback](ratings/feedback/02-development.json) · [Productietrace](runs/pair-01/acceptance/02-development/idea-trace.json)
- **010.** De gecontroleerde feedbacktest wees beide Narrative-first-varianten af en accepteerde de brongetrouwe variant. In beide correcties bleef Narrative als interventiepunt vaststaan. In de echte CLI haalde de feedback een onbelegde preventieclaim weg, maar adviseerde opnieuw een Narrative-proef en voegde een verplichte manifest- en citeerroute toe. De generator nam beide over. De tweede poging verwarde opnieuw de meest terugkerende waarneming met de fase die voor het gekozen geval uit het bewijs moet volgen. [Gecontroleerde feedback](ratings/feedback/04-analysis.json) · [Productietrace](runs/pair-01/acceptance/04-analysis/idea-trace.json)
- **Museum.** De gecontroleerde feedbacktest herkende beide gebreken, maar stelde onder meer voor de overstap naar museumteams expliciet te maken. In de echte CLI herstelde v3 de hoofdopdracht als bezoekersbericht. De feedback op de eerste poging wees terecht op onbewezen verspreidingskanalen en een effectclaim. Ze verwarde vervolgens de fysieke uitgifte van de route met het publiek voor het bericht. Het eindidee beperkte het publiek tot ter plaatse of bij de balie, verhief het bord van één eerdere opening tot onderdeel van de regeling en nam de voorgestelde website- en telefoonscripts op. [Gecontroleerde feedback](ratings/feedback/holdout-01-museum.json) · [Productietrace](runs/pair-01/holdout/holdout-01-museum/idea-trace.json)

De vaste live feedbacktests haalden **3/3 verwachte verdictreeksen** en **0/3 volledig brongetrouwe reeksen inclusief correcties**. In alle drie de productieherpogingenreeksen was minstens één correctie inhoudelijk onzuiver. De generator behield elementen uit de feedback ondanks de opdracht om die opnieuw aan de bron te toetsen. Daarmee wijzen de traces de overdracht van de fouten aan. Ze isoleren het effect van de individuele promptwijzigingen en de bijdrage van modelvariatie niet. [Beoordeling van de vier CLI-correcties](ratings/cli-feedback.json)

### Overdracht, budget en verificatie

- De goedgekeurde [ResearchInput-bestanden](approved-research/) zijn uitsluitend gemaakt voor **strategie, vervoer en bibliotheek**. Alle drie valideren tegen het actuele CLI-schema. De bron en het eerste idee zijn zonder inhoudelijke wijziging overgenomen.
- De drie afgekeurde CLI-uitkomsten, inclusief de volledige resultaten, traces en onafhankelijke beoordelingen, staan bij [afgekeurde fixtures](fixtures/rejected-cli/). De negen betaalde processen bevatten per run commando, revisie, SDK, modelalias, hashes, start- en eindtijd en exitcode.
- **29 van maximaal 51 logische modelaanroepen** zijn zichtbaar in de traces. Negen waren voor de vaste live feedbacktests en twintig voor tien productie-CLI-pogingen. Het gebruikte deel van het plan reserveerde 45 aanroepen. Providerfacturering, tokengebruik, de effectieve modelsnapshot en eventuele automatische netwerkherpogingen zijn niet zichtbaar. [Aanroeplogboek](verification/model-call-ledger.json)
- De productiebuild en harnasbuild slaagden zonder waarschuwingen of fouten. Alle **211 vindbare tests** slaagden. De drie vaste offline feedbacktests en de zes invoerschematoetsen slaagden. Alle 259 getrackte v1/v2-bestanden en de 14 productiepromptbestanden komen overeen met hun oorspronkelijke hashes. [Eindbuild](verification/final-build.txt) · [Eindtests](verification/final-tests.txt) · [Artefacthashes](verification/artifact-hashes.json)
- Er is één bevroren combinatie getest, met één volledige run per bron. De nieuwe holdout is synthetisch en heeft geen bevestigingsmeting. De overige holdouts waren bekend. Research en latere agenten zijn niet uitgevoerd. De kandidaat is lokaal en ongecommit.

