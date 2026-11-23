# Experiment 13 — vastgelegd protocol

Basis: 25ccb227354abb83f41430b12b52de5d8542fe25, live origin/main geverifieerd. Experiment 1: 3/12 volledig, 4/12 brongetrouw; soortverwisseling en afkoelvolgorde herhaald. Experiment 3: A 4/12 volledig, B 2/12, inkorting kostte brontrouw/focus. Experiment 11 stopte bij een ongeldige Compose-respons, 11b voltooide 48 Compose-slots en haalde 0/16, 0/16 en 1/16 eindpunten. Experiment 12 herleidde vier betekenisfouten tot de Narrative-brief, en andere fouten tot Compose. Historische cijfers zijn context.

Hypothese: controle op referent en relatie bij de centrale boodschap en elke verhaalstap, gecombineerd met één dragende waarneming voor het slot, verlaagt betekenisverschuiving en levert vaker een inhoudelijk einde zonder verlies van spanning. De baseline vereist al brontrouw en natuurlijke afronding. De nieuwe instructie concretiseert de transformatie en maakt een waarneming verplicht. Variatie tussen herhalingen en sterke formuleringen in Selected Idea (Digital gebruikt bijvoorbeeld 'door') blijven aparte verklaringen; Synthesis bepaalt inhoudelijke autoriteit.

A is de exacte productiepromptset. B wijzigt twee passages van de Narrative-userprompt. Vier ongewijzigde cases uit Experiment 11. Cooling en Library zijn vooraf geschreven synthetische ongeziene cases, gekozen bij gebrek aan geverifieerde ongebruikte historische fixtures. De tests toetsen respectievelijk meetvergelijkbaarheid en telbasis/voorstelstatus. Drie herhalingen, zes cases, twee armen, 36 Narrative-only CLI-slots, afwisselende armvolgorde per case en herhaling. GPT-6 Luna via bestaande consoleconfiguratie en SDK-defaults, zonder expliciete seed of temperatuur. Fouten bewaren, geen handmatige vervanging. JSON-uitvoer blijft intact. Scoring op anonieme ID's waar uitvoerbaar, met één beoordelaar, vóór koppeling van scores aan armen.

## Bronankers

- Digital: lanceringsduur en vijfweekscijfers hebben verschillende tijdsbases. Aanvrager eerst, caseworkers later. Tijdens private beta inzendingen in week twee gepauzeerd, onderzoek en testen gingen door. Geen causale toeschrijving van lancering of cijfers.
- Webb: Ariane 5-ruimte verklaart vouwen. Zonneschild en afkoeling overlappen. Uitgevouwen is een tussenmijlpaal en geen operationele wetenschapsstart. Geen nieuw startpunt voor de zesmaandsperiode.
- Wolf: elk is wapiti. Gedode prooidieren leveren karkassen; vroege-winterprooi verandert. Ungulate management omvat meer dan wapiti. Geen universeel ecosysteemeffect of verwisselde actor.
- Lowell: burgerinitiatief en herbestemming, daarna zicht op kwetsbaarheid vanuit 1992. Vertrekkende bedrijven en regionale/sectorale ontwikkelingen behouden eigen rol. Geen oorzaak toekennen op grond van samenloop.
- Cooling: tweede opstelling, vier heldere middagen; 2 graden is een verschil tussen locaties, geen effectschatting of uitspraak over nachten/comfort.
- Library: 52 van 80 uitleningen op zaterdag, 46 unieke leners, zaterdag langer open. Geen voorkeur per openingsuur of goedgekeurde vervolgproef.

## Beoordeling en beslissing

Acht assen elk voldoende/onvoldoende. Brontrouw behoudt referent, relaties, tijd, reikwijdte en onzekerheid; focus houdt één gekozen centrale lijn; structuur heeft drie onderscheidbare logische stappen; spanning koppelt een betekenisvolle tegenstelling of ontwikkeling aan de lijn; resolution maakt met een concrete waarneming duidelijk wat de ontwikkeling inhoudelijk oplevert; details zijn specifiek en relevant; handoff geeft voldoende richting zonder impliciete feiten te laten invullen; formatneutraliteit laat publicatiekeuzes aan Compose. Citaat uit brief én relevante bronpassage bij elk tekort. Classificeer betekenisveranderende afwijking als materieel, kleine onnauwkeurigheid apart, smaakverschillen tellen niet als tekort.

GO vereist volledige dekking, nul materiële fouten en nul formatspecifieke voorschriften in B op alle 18 slots, voldoende resolution op minimaal 15/18 B-slots, minimaal 10/12 volledig voldoende vaste slots en minimaal 2/3 per vast onderwerp. B moet ten minste twee extra volledig voldoende vaste slots en ten minste twee extra voldoende resolutions over alle cases halen tegenover A, zonder lagere totaalscore op focus, structuur, spanning, details of handoff. Alle andere uitkomsten blokkeren promotie. Bij ontbrekende dekking geen inhoudelijke uitspraak over kandidaatkwaliteit.
CLI strict JSON requires camelCase on Selected Idea; original input.json retained byte-for-byte and cli-input.json changes property names only. Digital preflight exited 2 before any model call.
