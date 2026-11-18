# Experiment 10 — End-to-End Fidelity / Narrative-to-Compose Handoff

## Hypothese en ontwerp

A gebruikt de ingebedde productieprompts. B voegt alleen één alinea toe aan de LinkedIn-userprompt die implicaties in hook, overgangen en slot expliciet laat controleren en de identiteit van vertaalde entiteiten laat behouden. Zie [exacte diff](prompts.diff). Beide gebruiken dezelfde productieagent, contracten, instellingen en Narrative-input binnen het paar. Compose krijgt alleen de brief, dus rechtstreeks corrigeren aan de hand van Synthesis is hier niet mogelijk. Selected Idea bepaalt de focus. Alleen Synthesis levert bewijs voor claims. De originele bron dient voor audit van mogelijke upstream-problemen, bijvoorbeeld welk dier met elk bedoeld wordt. Narrative blijft ongewijzigd en formatagnostisch.

Vier cases, twee doelgericht gekozen briefs per case (veilig en problematisch), twee onafhankelijke stateless GPT-6 Luna-aanroepen per arm en brief. Minimum 16 volledige paren, 32 calls. Bij Digital en Webb komen beide strata uit H. Bij Wolf bevat geen enkele H-brief een veilige soortvertaling, daarom is F r3 de veilige vergelijking. Bij Lowell zijn alle H-briefs veilig en wordt F r1 gebruikt voor de causale fout. Geen Narrative-calls. Zie config voor het vaste schema, in elk stratum vier starts per arm, en omgekeerde volgorde binnen dezelfde fixture. Geen conversatie-ID, tools, temperatuur, seed of aangepaste tokenlimiet. Zelfde productie-JSON-schema. Timeout vijf minuten, één HTTP-aanvraag per call, geen retries of vervanging. Geen inhoudelijke stop of uitsluiting, ook niet bij foutieve input. Stop bij fout, modelafwijking, niet-voltooide response, Ctrl+C of na de lopende call bij PAUSE. Hervatten slaat voltooide calls over en weigert onopgeloste pogingen. Onvolledige dekking betekent NO-GO.

## Uitvoering beoordeling

Beoordeel per post in gehusselde volgorde zonder zichtbare armnaam, met brief, Selected Idea en Synthesis naast de post. Uitvoerende assistent beoordeelt, dus geen onafhankelijke beoordelaar en slechts beperkte blindering. Noteer per post passage, upstream-anker, passage of afwezigheid in Narrative, gevolg in Compose en classificatie. Maak bij eenzelfde post onderscheid tussen overgenomen (ook upstream Synthesis/Idea als daar aantoonbaar de herkomst ligt), zelfstandig toegevoegd of versterkt, weggelaten/geneutraliseerd en ondersteund. Betwiste materialiteit apart melden. Een nieuwe versterking van een oude fout kan in beide categorieën tellen. Brontrouw is 2 bij geheel materieel ondersteund, 1 bij mogelijk materiële ambiguïteit en 0 bij een materiële afwijking. Tel per arm posts met overgenomen en nieuwe afwijkingen, inclusief 1 als risico, en rapporteer afwijkingen per paar en stratum. Beschouw woordkeuze, impliciete tijd, causaliteit, rol en scope als inhoud wanneer de lezing merkbaar verandert.

Zes redactionele scores van 0–2. 0 betekent mislukt/afwezig, 1 gedeeltelijk of generiek, 2 publiceerbaar op dit criterium.
- Aantrekkelijkheid: concrete inzet of contrast maakt doorlezen de moeite waard.
- Leesbaarheid: natuurlijke zinnen, begrijpelijk voor buitenstaanders, scanbaar.
- Hook: direct, specifiek en relevant, zonder onbewezen bewering.
- Spanningsboog: context, ontwikkeling en betekenis bouwen voort op elkaar.
- Afsluiting: onderwerpgebonden afronding met betekenis, zonder generieke les, herhaling of nieuw feit.
- Focus: Selected Idea blijft leidend en details of cijfers nemen de post niet over.

Vaste checks: Webb past gevouwen in Ariane 5 en week twee, afkoeling in maand één en routinewetenschap na zes maanden zijn aparte tijdvensters. Geen onset na volledig uitvouwen. Wolf: Yellowstone elk is wapiti, karkassen zijn van prooien en meerdere factoren spelen mee. Lowell: behoud 1992-perspectief, oude gebouwen en samenloop bij neergang, zonder oorzakelijke stelligheid. Digital: applicant-first en verschillende sprintweken dragen het verhaal, cijfers zijn optioneel en de werkwijze verklaart de gemelde uitkomsten niet vanzelf. Controleer ook overige claims en fouten.

## Beslissing vooraf

GO vereist alle 16 paren, minstens twee minder posts met nieuwe materiële of mogelijk materiële Compose-afwijkingen en geen nieuwe dergelijke afwijking in B waar A die niet had (beoordeeld per claim binnen paar), minstens 7/8 brontrouwe B-posts op veilige input en geen stijging in posts met overgenomen fouten op problematische input. Ten minste 10/16 B-posts scoren op elk van de zes redactionele dimensies 2 en in elk onderwerp zijn minstens twee B-posts redactioneel volledig voldoende. B verliest geen redactioneel puntentotaal per onderwerp of gemiddeld, en hoogstens drie paren hebben een lager redactioneel totaal. B mag maximaal 10% stijgen in totale tokens én geschatte kosten over alle paren. Reproduceer requests en geparseerde posts offline. Rapporteer alle gates, effectgroottes, onzekerheid en individuele regressies. Een kleine doelgerichte steekproef kan geen algemene betrouwbaarheid aantonen.
