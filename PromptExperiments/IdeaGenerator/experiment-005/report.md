# Idea Generator Experiment 005

**Besluit op 9 november 2026:** NO-GO voor Research. Eén van de drie eerste ideeën voldoet aan de criteria. De drempel voor een voorlopige GO was 3/3.

## Opzet

Experiment 003 liet bij het ontwikkelverslag een causale generalisatie zien en bij de diagnostiek een onbewezen voorkeursinterventie. Experiment 004 gebruikte de generieke runner. De strategie slaagde, maar in de andere twee ideeën kwamen dezelfde soorten overclaims terug. Bij het ontwikkelverslag zat de zwaarste overclaim in de rationale, terwijl de titel dichter bij de proef bleef.

De kandidaat is een kopie van de userprompt uit Experiment 004 met één vervangen alinea in §11. De nieuwe alinea laat de agent titel, beschrijving, rationale en evidence afzonderlijk toetsen aan een concrete bronpassage. Interpretaties moeten in het betreffende veld herkenbaar blijven. Geobserveerde verschillen blijven gebonden aan de onderzochte gevallen. Een voorgestelde vervolgstap mag als toegeschreven voorstel terugkomen, met behoud van de voorwaarden. De systemprompt is byte-identiek aan Experiment 003, Experiment 004 en de productieversie. De uitvoerinstructie voor vijf JSON-velden en de modelconfiguratie zijn overgenomen uit Experiment 004.

De drie Markdown-snapshots hebben dezelfde SHA-256-hashes als in 003 en 004. De YAML-bestanden zijn byte-identiek aan 004. De preflight verifieerde de bron tussen de bronmarkeringen in elke gerenderde prompt. Na normalisatie van regeleinden verschilt elke render uitsluitend door de ene vervangen alinea. De Release-build van de actuele werkkopie slaagde met nul waarschuwingen en fouten. De bestaande lokale PromptRunner-wijzigingen waren al aanwezig bij aanvang.

## Uitvoering en beperking

De drie runs gebruikten OpenAI `gpt-5-mini` met de standaardinstellingen van de SDK, één run per bron. Zonder `--idea-output` leest [Program.cs](../../../src/Nueyon.PromptRunner/Program.cs#L83) YAML, rendert de templates en voert [één `AsAIAgent`/`RunAsync`-aanroep](../../../src/Nueyon.PromptRunner/Program.cs#L171) uit. De runner schrijft de tekstrespons naar stdout. `--validate` stopt vóór de modelaanroep en controleert alleen invoer en placeholders. Het productie-`IdeaResponse`-schema en de validatielus horen bij de afzonderlijke [`TestRun`-route](../../../src/Nueyon.PromptRunner/TestRun.cs#L16), die buiten deze test bleef.

Per run is achteraf precies `ideas[0]` uit de ruwe tekst bewaard, met een controle op vijf niet-lege tekstvelden. Dit is een Idea-only screening zonder productiepariteit. Research-geschiktheid is inhoudelijk beoordeeld zonder Research uit te voeren. Er zijn geen automatische iteraties of extra runs uitgevoerd. Eén steekproef per bron kan het effect van de promptwijziging niet isoleren.

## Evaluatie

| Bevroren bron | Duur | Brontrouw | Redactionele richting | Bruikbaar voor Research |
| --- | ---: | --- | --- | --- |
| [Nuëyon-strategie](runs/01-strategy/selected-idea.json) | 25,83 s | Voldoende met afwijking in publiek | Voldoende | Voldoende met publieksafbakening |
| [Experiment 005 ontwikkelverslag](runs/02-experiment-005/selected-idea.json) | 34,68 s | Onvoldoende | Voldoende voor een terugblik op de proef | Onvoldoende |
| [Experiment 010 diagnostiek](runs/03-experiment-010/selected-idea.json) | 33,69 s | Onvoldoende | Onvoldoende | Onvoldoende |

Alle processen eindigden met exitcode 0 en leverden een eerste idee met vijf ingevulde velden. Totale looptijd van de drie modelruns was 94,20 seconden.

- **Nuëyon-strategie.** De titel en beschrijving houden het bedrijf centraal en gebruiken Compose als eerste concrete productrichting. De rationale bewaart het vroege stadium. De doelgroep van andere AI-first bedrijven is onderbouwd (§3, §13). Potentiële partners en bijdragers zijn aanvullingen zonder bronbasis, en de generieke verwijzing naar een nieuwe leverancier vergt voorzichtigheid. De centrale claim blijft bruikbaar. Research kan de bedrijfsintroductie uitwerken vanuit §1, §3, §5 en §12–14 met de expliciete doelgroep uit de bron.
- **Ontwikkelverslag.** De lagere kandidaatscores, falende criteria C en D en de rollback zijn brongetrouw. De titel stelt dat de proef bewijst dat een generieke regel de hiërarchie kan breken. De beschrijving schrijft de uitkomst rechtstreeks aan de regel toe en de rationale noemt de drift voorspelbaar en herhaalbaar. De latere nuance over steekproef en modelvariatie laat die sterkere claims staan. De bron ondersteunt de terugblik op deze kandidaat en de afwijzing na drie kandidaatruns. Met eerder uitgevoerde baselines, geen seed of temperatuurcontrole en een mogelijk veranderlijke backend kan Research de meegegeven algemene oorzaak niet onderbouwen. Zie *Controlled comparison and configuration*, *Run scores*, *Analysis against the required criteria* en *Limitations*.
- **Diagnostiek.** Narrative was een terugkerende eerste zichtbare locatie. Het idee vermeldt de uitzonderingen bij Idea en Synthesis en erkent onzekerheid over causaliteit. Toch beveelt de titel aan daar in te grijpen en noemen beschrijving en evidence Narrative de plek met de meeste invloed. De bron vergelijkt geen interventies en adviseert per gekozen afwijking de vroegste onderbouwde fase en haar werkelijke invoer te onderzoeken. Run 008 A-002 is daarbij een feitelijke publieksafwijking, geen hiërarchiebreuk. Het voorstel stuurt Research daarmee weg van de bronvraag, namelijk waar verschillende soorten drift voor het eerst zichtbaar worden. Zie *First observable drift by run*, *Cross-experiment patterns* en *Implication for the next focused trial*.

## Diagnose na NO-GO

Een aparte brontrouwcontrole vóór de overdracht aan Research is hier zinvoller om te beproeven dan nog een regel in de generatorprompt. De twee afwijzingen herhalen precies de fouten die de kandidaat expliciet adresseert. Bovendien komen de fouten voor in geldige JSON met gevulde velden. De bestaande [IdeaValidator](../../../src/Nueyon.Compose.Application/Validation/IdeaValidator.cs#L20) controleert structuur en verplichte velden en kan deze inhoudelijke fouten dus doorlaten.

Een controle kan per centrale claim een bronanker eisen, de vindbaarheid ervan programmatisch verifiëren en overdracht blokkeren bij ontbrekend bewijs. Voor de vraag of de passage ook causaliteit, reikwijdte of interventieprioriteit ondersteunt, is een aparte semantische of menselijke beoordeling nodig. De opgeslagen selecties uit 003–005 geven concrete gevallen om zo'n controle tegen te toetsen, inclusief correcte observaties die ze moet behouden. Er is in 005 geen validator gebouwd of getest.

## Bewijs

[Preflight](preflight.json) bevat commit, build, exacte argumenten en SHA-256-hashes. Per run staan de bron, YAML, render, ruwe stdout en stderr, eerste selectie, duur en hashes in `runs/`. [Eindcontrole](integrity.json) legt de vergelijking met de werkkopie bij aanvang vast. De productieprompts en bronbestanden zijn intact gebleven. Er zijn geen commits of pushes uitgevoerd.
