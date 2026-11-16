# Experiment 7 — uitsluitend Research, offline herbeoordeling

16 november 2026. **Advies: behoud compact B als kandidaat en voer een gerichte verbetering uit; promoveer nu niet. Er is geen overtuigende Research-only reden om A als inhoudelijk betere basis te kiezen.** B bewaart in deze twaalf opgeslagen generaties de kern voor de vaste Selected Ideas ongeveer even goed als A, met 69,16% minder Research-tokens. Beide laten belangrijke informatie weg. Dit is geen bewijs van algemene gelijkwaardigheid of productiebetrouwbaarheid.

De eerdere negatieve conclusie uit eindpublicaties kan niet als Research-oordeel worden gebruikt. De oude Research-succescijfers zijn evenmin betrouwbare absolute kwaliteitspercentages: labels combineerden kerninformatie met context en soms met eisen aan latere agents. Historische bestanden en scores zijn niet gewijzigd.

## Afbakening en betrouwbaarheid

Lokale repository: `C:\Workspaces\Nueyon.Compose`, HEAD `2805559c781d6503e287c0b924a9c558e093eac2`, origin verwijst naar `antoine271167/Nueyon.Compose`. Beginstatus schoon. Geen remotecontrole: deze opdracht verbiedt netwerkgebruik. De gekoppelde chat is als context gelezen; dit was geen experimentele modelcall.

Uitsluitend de twaalf bestaande Experiment-6-Research-generaties zijn inhoudelijk beoordeeld: 005, HarborLink en City Memory, per bron A/B en r1/r2. Bronnen, vijf Selected-Idea-velden, ruwe responses, volledige Research en opgeslagen filterprojecties zijn gebruikt. Geen nieuwe generatie, Judge, Evidence Mapper, externe retrieval of andere API/modelcall uitgevoerd. Er is geen kwaliteitsoordeel over Synthesis, Narrative of Compose gegeven. De historische scores van die agents in bron 005 worden alleen als broninhoud gecontroleerd.

**De Work-assistent is geen onafhankelijke menselijke beoordelaar.** De historische labels en A/B-identiteiten waren al zichtbaar bij de eerste inventarisatie. Echte blindering is daardoor niet mogelijk en wordt niet geclaimd. De nieuwe rubric is vastgelegd vóór de systematische matrix/eindscores, maar ná die eerste inzage. Dit is een retrospectieve inhoudsaudit, geen onafhankelijk preregistreerd experiment. De zichtbare lengte van A/B zou ook bij verwijderde namen herkenbaarheid veroorzaken.

De bronrapporten zijn zelf de bewijsgrens. Harbor en archive zijn expliciet synthetisch; 005 bevat eerdere subjectieve rapportbeoordelingen. Hun onderliggende werkelijkheid is niet opnieuw gevalideerd. Twee herhalingen per cel en drie bronnen leveren geen betrouwbare foutkans of statistische non-inferioriteit.

## Werkwijze en nieuwe rubric

[protocol.md](protocol.md) legt de regels en materialiteit vast. Vijf dimensies per run krijgen 0–2: brontrouw, essentiële dekking voor de vaste Idea, relevante nuance, Facts/Unknowns-consistentie en bruikbaarheid van de echte projectie. Twee betekent voldoende; één betekent bruikbaar met een concreet gebrek; nul betekent materieel onveilig/onvolledig. Er is geen compenserende totaalscore. Herhaalbaarheid wordt per case/arm beoordeeld.

Een bronfeit is niet essentieel omdat het in de bron staat of een oud label heeft. Een essentiële beperking moet de gekozen boodschap begrenzen. Afwezigheid van context is geen fictie of automatisch falen. Om deze beoordelingskeuze zichtbaar te houden, is ook een strengere lezing doorgerekend: upstreamvariatie in 005 en keyboardtoegankelijkheid bij archive gelden daar als harde eisen.

[traceability.md](traceability.md) en [traceability.json](traceability.json) bevatten **160 bron/run-beoordelingen**, met exacte broncitaten, regelnummers, volledige Research-regels en projectielocaties. De broncitaten worden met exacte substringvergelijkingen gecontroleerd. Bij een omissie staat geen verzonnen outputcitaat. De matrix behandelt veertig brononderwerpen; dit is een gerichte inventaris van kerninformatie, beperkingen en betwiste labels, geen claim dat iedere bronzin een verplichte aparte fact is. De zoekpatronen verzamelen bewijsankers; ze zijn geen semantische Judge. De aanvullende precisieproblemen hieronder voorkomen dat 'correct' op een matrixonderwerp wordt gelezen als 'iedere zin in de run is foutloos'.

## Resultaten per run

BT = brontrouw; DK = essentiële dekking; NU = nuance; FU = Facts/Unknowns; BR = bruikbaarheid projectie. 'Voorwaardelijk' is geen productiesucces of promotie.

| Case/run | Arm | BT | DK | NU | FU | BR | Oordeel en reden |
|---|---|---:|---:|---:|---:|---:|---|
| 005 r1 | A | 2 | 2 | 1 | 2 | 1 | Voorwaardelijk: upstreamvariatie en concrete vergelijkingsdatums ontbreken; causale terughoudendheid aanwezig |
| 005 r1 | B | 2 | 2 | 1 | 2 | 1 | Voorwaardelijk: upstreamvariatie ontbreekt; datums/backend wel expliciet |
| 005 r2 | A | 2 | 2 | 2 | 2 | 2 | Voldoende voor vaste Idea; variatie/datums aanwezig, mogelijke Synthesis-spanning ontbreekt als context |
| 005 r2 | B | 2 | 2 | 2 | 2 | 2 | Voldoende; variatie/datums én mogelijke spanning behouden |
| Harbor r1 | A | 2 | 2 | 2 | 2 | 2 | Voldoende, breed en gedetailleerd |
| Harbor r1 | B | 2 | 2 | 1 | 2 | 2 | Voorwaardelijk: niet-random routes en dieseltegencontrast aanwezig; vakantie/omleiding ontbreken |
| Harbor r2 | A | 2 | 2 | 2 | 2 | 2 | Voldoende; onderhoudsregisterdetail ontbreekt, maar geen betrouwbaarheidsclaim |
| Harbor r2 | B | 1 | 2 | 2 | 2 | 2 | Voorwaardelijk: ongelukkige vertaling van charger 'trips' naar 'ritten'; kern blijft intact |
| Archive r1 | A | 1 | 2 | 1 | 2 | 1 | Voorwaardelijk: keyboard ontbreekt; ongefundeerde chronologie buiten projectie |
| Archive r1 | B | 2 | 2 | 1 | 2 | 1 | Voorwaardelijk: keyboard ontbreekt; kern van vaste Idea intact |
| Archive r2 | A | 1 | 2 | 1 | 2 | 1 | Voorwaardelijk: keyboard ontbreekt; te stellige verificatieformulering |
| Archive r2 | B | 2 | 2 | 1 | 2 | 1 | Voorwaardelijk: keyboard ontbreekt; ook dashboardwijziging weggelaten |

Volgens deze expliciet Idea-gerichte rubric: A drie volledig voldoende en drie voorwaardelijk; B één volledig voldoende en vijf voorwaardelijk. Geen run krijgt een nul voor de kern van de vaste Idea. Dit betekent **niet 6/6 productiegoedkeuring**: vooral het terugkerende keyboardgat verhindert een robuust promotieadvies. De getallen zijn beschrijvingen onder deze rubric, geen objectieve benchmarkscore.

Strengere sensitiviteit met upstreamvariatie als harde eis: 005-r1 faalt bij beide armen. Met keyboard als harde eis: alle vier archive-runs falen. Dan blijven per arm drie van zes zonder deze harde omissies over (005-r2 en beide Harbor-runs), nog steeds met de aangegeven kleinere gebreken. Beide lezingen ondersteunen hetzelfde advies: geen aantoonbare B-kernregressie, wel gerichte verbetering nodig.

## Verifieerbare inhoudelijke bevindingen

### 005: concrete beperking ontbreekt bij beide eerste herhalingen

Bron, Limitations: **“Idea and Research outputs vary between runs.”** A-r1 heeft deze beperking nergens; B-r1 evenmin. A-r2 bewaart haar in Fact 36; B-r2 in Fact 10 en Unknown 2. Zie matrixonderwerp `005 / variation` voor exacte regels en citaten.

Dit is belangrijke informatie voor attributie: gelijke workflowinputs betekenen geen gelijke tussentijdse outputs. Toch claimen A-r1 en B-r1 geen geïsoleerd causaal effect. A-r1 Unknown 1 zegt letterlijk: **“Whether the candidate prompt change alone caused the differences in scores or output content is not established.”** Daarom is de omissie onder de gekozen, terughoudende Selected Idea een belangrijke onvolledigheid, geen automatische totale onbruikbaarheid. Wie Research als volledige causale evaluatiebasis wil gebruiken, moet haar wél hard afwijzen. Die strengere uitkomst staat hierboven.

Een concreet A/B-verschil: B-r1 Unknown 2 noemt baseline op Nov. 3 en kandidaat op Nov. 4, met backendmogelijkheid. A-r1 noemt backendonzekerheid, maar niet de datums. Een oude globale classificatie 'r1 mist tijdsgrens' mag dus niet op beide armen worden geplakt. B-r2 is hier vollediger dan A-r2 doordat het ook de mogelijke Synthesis-spanning behoudt. De gerapporteerde lagere scores, afwijsbeslissing, gates en bytegelijke rollback zijn in alle vier correct terug te vinden.

### Harbor: audiofouten en reparatie zijn in Research van beide armen aanwezig

Bron §4: **“All three had the specified wheelchair space, but two had an audio-stop announcement failure during inspection.”** En: **“The maintenance team restored the two audio units on 2 July, after the trial ended.”**

A-r1 Facts 58–62, A-r2 Facts 49–54, B-r1 Fact 6 en B-r2 Fact 11 bewaren deze combinatie; hun projecties eveneens. Hier is dus **geen Research-omissie** en geen verlies door het Facts/Unknowns-filter. Een historische observatie dat een latere tekst deze informatie mist, is geen negatief Research-label. Er zijn in Experiment 7 geen latere agentoutputs inhoudelijk opnieuw beoordeeld.

Alle vier behouden 2.240/2.400 tegenover 2.310/2.400, correctie van het conceptgetal 2.340, het verschil tussen diensten en uitsluitend elektrisch rijden, de 3/5-minutengrens, kostenbegrenzing en winter-specificatie zonder vlootbesluit. B-r1 is selectiever: feedback en cleaner-transportbetekenis ontbreken, maar de vaste Idea vraagt service, metingen, toegankelijkheid, kosten en winterbesluit. Het missen van reizigerscommentaar is dan geen materiële Research-fout op zichzelf.

B-r1 behoudt wel de niet-willekeurige routeverdeling en dieseltegenverbetering, maar niet de verschillende vakantiekalender en brugomleiding. Daarom nuance 1. B-r2 bewaart die omstandigheden en veel meer secundaire onderwerpen, zoals de onvergelijkbare onderhoudstelling. Compact betekent dus niet in iedere herhaling hetzelfde informatieniveau.

### Archive: echt keyboardgat, maar het oude harde label behoeft scope

Bron §6: **“A keyboard-only review still found that the language switch could not be reached on two record-page templates.”** De contrastscan was geslaagd, de navigatiereparatie nog niet uitgerold. Alle vier Research-generaties laten dit weg, ook buiten Facts/Unknowns. Dit is werkelijk generatieverlies; het filter kan de informatie niet terughalen.

De vaste Idea luidt echter: **“Een geordend archief is nog geen foutloos of volledig open archief”** en specificeert **“ordening, feitelijke beschrijving en publieke beschikbaarheid”**. Toetsenbordbediening en de publicatiegrens van beelden zijn verschillende dingen. Alle vier bewaren die drie expliciete Idea-dimensies: Cedar 60 correct geordend/9 naamfouten, Elm 60 juiste namen/8 verkeerde jaren, Rowan 60 beide goed zonder certificatie van alle 300; steekproef- en recheckbeperking; 900 records tegenover rechtenstatus 760/140; alleen auditontwerp. Het keyboardgat automatisch tot falen van deze hele Idea verheffen is strenger dan de Idea zelf voorschrijft.

Dat maakt de omissie niet wenselijk: voor een bredere uitleg van toegang is het juist een nuttig tegenvoorbeeld. Daarom belangrijk verbeterpunt en sensitiviteit als harde eis. Productiviteit en zoeklogs zijn voor deze specifieke Idea context; geen van de vier maakt een productiviteits- of adoptieclaim die hun volledige behandeling noodzakelijk maakt. Ook de afzonderlijke captain/owner-fout ontbreekt overal. Zij zou verplicht worden als Research beweerde dat de zeventien naam/jaarfouten de volledige foutinventaris waren; de huidige beperkte recheckbeschrijvingen doen dat niet.

### Correctie van het rights-label

Oud label: **“900 records versus 760 openbare scans/140 withheld”**. Bron §5: **“Rights clearance is complete for 760 of the 900 scans.”** Dit is geen expliciete garantie van publieke downloadbaarheid voor die 760. De niet-downloadbaarheid van de andere 140 is wel expliciet. Alle vier Research-resultaten gebruiken de rechtenvrijgaveformulering. A-r2 Unknown 4 bewaakt bovendien uitdrukkelijk dat publieke downloadbaarheid van alle 760 niet is vastgesteld. Het oude label is te sterk en mag niet als gouden standaard worden toegepast. Alleen dit nieuwe rapport corrigeert de interpretatie; `labels.json` en oude scores blijven intact.

## Kleine bronprecisieproblemen en ongefundeerde uitspraken

Deze aanvullende output-naar-broncontrole voorkomt een te gunstig oordeel op uitsluitend thematische aanwezigheid. Ze betreft geen nieuwe hoofdcriteria.

| Run/locatie | Exacte output | Bronvergelijking | Classificatie/materialiteit |
|---|---|---|---|
| Archive A-r2 Fact 8; projectie L10 | “Full-text transcription was not verified.” | §1 zegt: “it does not mean the original has been replaced or that full-text transcription is verified.” Niet uit digitization afleidbaar is niet hetzelfde als aantoonbaar nooit geverifieerd | Ongefundeerd sterk; klein voor deze Idea, brontrouw 1 |
| Archive A-r1 Development sequence | “At an unspecified later point, rights clearance was complete for 760 scans; 140 remained held internally and their images were not publicly downloadable.” | §5 noemt de status, maar geen tijdsrelatie met de correcties. 'later' voegt chronologie toe | Ongefundeerde volgorde; klein, buiten projectie |
| Harbor B-r2 Fact 8; projectie L10 | “de bron kan niet bepalen welke ingreep de ritten oploste.” | §3: “This record cannot identify which action resolved the trips.” Hier zijn 'trips' beveiligingsuitschakelingen van de lader, geen ritten | Deels/ongefundeerde vertaling; klein door correcte context in dezelfde Fact; wel doorgegeven |

Geen materiële verzonnen businesscase, emissiewinst, algemene toegankelijkheid of rechtenpublicatie geconstateerd. Geen materiële Facts/Unknowns-tegenstrijdigheid gevonden. Een Unknown die all-900-downloadbaarheid niet vastgesteld noemt, is logisch minder scherp dan '140 aantoonbaar niet', maar de Facts geven dat expliciet; dit is geen echte tegenspraak. Bij A staan buiten de projectie soms brede UNKNOWN-zinnen over relaties. Ze vragen om zorgvuldiger onderscheid tussen tijdsvolgorde, rationale en geïsoleerde oorzaak; ze maken de juiste Facts/Unknowns hier niet ongedaan.

## Herhaalbaarheid en kwalitatieve A/B-verschillen

| Case | A over r1/r2 | B over r1/r2 | Betekenis |
|---|---|---|---|
| 005 | Upstreamvariatie eerst weg, daarna aanwezig; spanning alleen r1 | Zelfde variatieverschil; spanning beide keren aanwezig; tijdsgrens beide keren | Kern stabiel; attributie-informatie niet volledig herhaalbaar. B niet slechter |
| Harbor | Veel detail, stabiele kern; onderhoudsregister niet stabiel behouden | Stabiele kern; r1 veel selectiever dan r2, r2 taalprobleem | B comprimeert nuttig, maar selectie van secundaire grenzen varieert |
| Archive | Stabiele kern, keyboard beide keren weg; kleine overclaims | Stabiele kern, keyboard beide keren weg; dashboard alleen r1 | Gedeelde blinde vlek; A-volume lost die niet op |

A splitst onderwerpen vaak in veel korte Facts, B bundelt ze met bronsecties. Meer Facts is geen betere dekking: A mist dezelfde keyboardbevinding. B heeft doorgaans een kortere bruikbare bewijsbasis en explicietere bronankers, maar bundelt meerdere proposities per Fact. Dat kan controle lastiger maken en is geen bewijs dat een volgende agent er beter mee werkt. De feitelijke bruikbaarheid in dit rapport betekent uitsluitend dat noodzakelijke informatie bereikbaar en voldoende begrensd is.

## Bestaande Research-tokenusage

Direct opnieuw opgeteld uit uitsluitend twaalf `response-raw.json`-usageobjecten, niet uit de ketentotalen. [usage-from-responses.json](usage-from-responses.json) bewaart alle echte usagevelden; [usage-summary.json](usage-summary.json) de afleiding.

| Arm | Generaties | Input | Output | Totaal | Cached input | Reasoning binnen output |
|---|---:|---:|---:|---:|---:|---:|
| A | 6 | 69.964 | 15.628 | 85.592 | 53.021 | 5.392 |
| B | 6 | 15.684 | 10.714 | 26.398 | 11.343 | 3.872 |

B bespaart **59.194 tokens, 69,1583%**. Met illustratieve weging I+4O is de daling **55,81%**; dit is geen tarief. Cached input is een inputdetail en reasoning een outputdetail: niet nogmaals optellen. De ruwe bestanden bewaren ook eventuele overige cachevelden. Geen nieuwe experimentele uitgaven. Geen euro-/dollarbesparing geclaimd: factureertarieven en cachevoorwaarden zijn niet offline vastgesteld. Kosten per 'geslaagd resultaat' worden niet berekend omdat rubricafhankelijke passes hier geen robuust productie-eindpunt vormen.

## Historische context, zonder herbeoordeling van andere agents

De Research-baseline van 12 november vond onder meer ontbrekende upstreamvariatie in 005 en ontbrekend onderscheid in 010. Experimenten 1–4 onderzochten vooral controle-/mapperroutes; hun uitkomsten zijn geen A/B-generatiebewijs. Experiment 5 rapporteerde B 6/6 tegenover A 4/6 (gevoeligheid 5/6), met 74,6% minder generatietokens. Dat waren andere bronnen/cellen, grotendeels één generatie per cel, met dezelfde assistent als auteur/beoordelaar. De oude scores zijn context, geen scores die Experiment 7 bevestigt.

Experiment 6 voegde langere synthetische dossiers en herhalingen toe, maar vermengde Research- en downstreamcriteria. De oude 2/6 tegenover 1/6 kan daarom niet rechtstreeks worden vergeleken met de nieuwe rubric. Het huidige onderzoek bewijst geen kwaliteitsverbetering door opnieuw scoren; het corrigeert wat de meetvraag is en maakt de gevoeligheid voor materialiteit zichtbaar.

## Offline verificatie en integriteit

De controles gebruiken uitsluitend lokale bestanden. Twaalf ruwe response-contents komen overeen met de opgeslagen Research; alle twaalf hebben de vijf headings en finish_reason stop. De twaalf opgeslagen projecties komen overeen met de filterlogica. Daarbovenop is de **actuele productiebron van SynthesisResearchFilter** zonder wijziging meegecompileerd in een geïsoleerd klein controleprogramma: twaalf van twaalf projecties gelijk (alleen newlineconventie/eindnewline genormaliseerd). De packagebronnen zijn leeg gemaakt voor deze geïsoleerde build; geen netwerkrestore nodig. Geen oude runner gestart: die heeft ook live modi en zou historische controlebestanden overschrijven.

Dit is geen volledige solutiontest of semantisch bewijs. De applicatie is niet gewijzigd, dus een volledige build/testsuite voegt voor deze inhoudsaudit weinig toe. De gerichte controles toetsen de daadwerkelijke risico's: verkeerde broncitaten, verkeerde response-extractie, verkeerde filtertoeschrijving en onbedoelde wijzigingen.

[integrity.json](integrity.json) bevat aantal beschermde bestanden, hashescontrole, HEAD en gitstatus. Productiebronnen en eerdere experimentbestanden blijven bytegelijk; uitsluitend de nieuwe Experiment-7-map is toegevoegd. Geen commit of push. De nieuwe matrix is een lokaal traceability-artefact, geen modelgebaseerde Evidence Mapper en is niet als nieuwe modelinput gebruikt.

## Besluit en concreet vervolgadvies

**Gerichte verbetering met compact B als basis voor de kandidaat**, niet terug naar A vanwege eindpublicaties. Maak de selectie van relevante contrasten robuuster: bewaar voor een gekozen kwaliteitsdimensie zowel geslaagde als mislukte checks; bewaar bij vergelijkingen concrete attributiebeperkingen; houd reeds vastgestelde informatie buiten brede blanket-Unknowns. Vermijd een catalogus van ieder bronfeit en vermijd casenamen/verwachte antwoorden in een generieke promptverbetering.

Promotie is nu te vroeg: beide armen missen herhaald een belangrijk toegankelijkheidscontrast en de kleine set is niet onafhankelijk beoordeeld. Laat bij een eventuele volgende, afzonderlijk geautoriseerde Research-proef eerst de materialiteit door een menselijke beoordelaar valideren, daarna de kern en relevante nuances met nieuwe bronnen/herhalingen controleren. Dit rapport autoriseert of start die proef niet. Er is nu niets aan prompts, agents, workflow, filter of configuratie veranderd. Idea blijft voorlopig afgerond; de andere agents zijn niet beoordeeld. Research blijft de huidige focus.
