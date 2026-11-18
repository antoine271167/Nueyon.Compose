# Narrative Experiment 2

Productiecommit e5a54759f7a355a7707f9a9304cecce8bb0888d2. A draait met beide ingebedde productieprompts. B gebruikt dezelfde productieagent en het bestaande PromptDirectory-mechanisme met twee bevroren promptbestanden. De gecombineerde ingreep staat in prompts.diff. De algemene regels en de grens met Compose blijven staan.

De vier fixtures en beoordelingsankers komen byte-identiek uit Experiment 1. Elk case/ronde-paar bevat twee onafhankelijke requests in aangrenzende posities, met 6 keer A eerst en 6 keer B eerst. Iedere arm draait drie keer per case zonder gedeelde sessie. De productieagent gebruikt GPT-6 Luna via Chat Completions met providerdefaults, productieschema, maximaal 5000 completiontokens, één request per poging en nul retries. Een ongeldige, ontbrekende of afgebroken poging maakt een GO onmogelijk. Runs hebben één vaste ID en worden niet vervangen. Geen upstream-, Compose-, Judge- of zoekcalls.

## Beslisregel vooraf

GO vereist alle volgende voorwaarden. Dit is een gate voor een promotiebesluit, waarvoor expliciete toestemming vereist blijft.

- Alle 24 live calls en offline verzoek- en replaycontroles slagen.
- B behaalt minimaal 10/12 volledig voldoende en minimaal twee meer dan de gelijktijdig gemeten A. Iedere case heeft minimaal 2/3 volledig voldoende B-runs.
- B behaalt 12/12 op brontrouw. Een nieuwe soort- of actorverwisseling, geforceerde volgorde, ondersteunde causaliteit die van richting, scope of zekerheid verandert, of andere nieuwe betekenisfout verhindert GO.
- Voor verhaallijn, focus, details en Compose haalt B op iedere as minstens evenveel scores van 2 als A. Een gepaarde daling van 2 naar 0 op een van de vijf assen verhindert GO. Alle gepaarde dalingen, ook 2 naar 1, worden gerapporteerd.

Score tegen Synthesis. Gebruik Selected Idea voor focus en bron/projectie om bestaande upstream-afwijkingen te lokaliseren. Upstream-onvolkomenheden tellen alleen mee als Narrative ze versterkt of zelf betekenis toevoegt. Neutraal behoud van onduidelijke timing en weglaten van niet-essentiële details zijn toegestaan. Twee stappen als gescheiden fasen presenteren telt alleen als tijdlijnfout wanneer de bewoording overlap uitsluit of een niet-onderbouwde strikte volgorde oplegt.

De uitvoerende assistent beoordeelt alle briefs met dezelfde rubric. De beoordeling is niet onafhankelijk en de beoordelaar heeft toegang tot de armcodes. Handoff is een beoordeling van de brief voor Article en LinkedIn, zonder meting van gegenereerde Compose-output. Het gecombineerde ontwerp schat geen afzonderlijk effect per promptwijziging. Verbruik per run uit provider-usage, financiële kosten alleen bij verifieerbare tarieven of factuurdetails.
## Rubric

Per run op vijf assen beoordelen. 2 is voldoende, 1 is gedeeltelijk bruikbaar met gerichte reparatie, 0 is onvoldoende. Volledig voldoende vereist vijf keer 2. Rapporteer per case ook het aantal volledig voldoende runs; drie herhalingen leveren alleen een beschrijvende indicatie.

- **Verhaallijn:** 2 bij begrijpelijke context, onderscheidende ontwikkeling en natuurlijk endpoint. 1 bij merkbare herhaling of schetsmatige overgang. 0 bij een inventaris of ontbrekende lijn.
- **Focus:** 2 bij één passende Selected Idea met ondergeschikte details. 1 bij een zijpad of lichte verschuiving. 0 bij een andere hoofdzaak, inclusief advies of orchestratorverhaal zonder basis.
- **Details:** 2 bij concrete, brongebonden details die het verhaal dragen. 1 bij een relevante maar slappe of ontbrekende bouwsteen. 0 bij vrijwel alleen abstractie. Pas de lat aan de beschikbaarheid van relevante input aan.
- **Brontrouw:** 2 bij behoud van actoren, soort, relaties, scope en zekerheid. 1 bij een beperkte lokale betekenisverschuiving. 0 bij een materiële nieuwe claim, actor, causaliteit, soortfout of gefingeerd resultaat. Score tegen de aangeleverde Synthesis. Markeer afwijkingen die daar al in zitten apart als upstream en rapporteer hun doorwerking.
- **Compose:** 2 bij zelfstandig bruikbare, format-agnostische Selected Story en drie logisch onderscheiden beats voor Article en LinkedIn, met gerichte grenzen waar nodig. 1 bij beperkte revisie door formatvoorschrift, bronaudit, fragmentatie of gebrekkige structuur. 0 bij een onbruikbare handoff. Deze beoordeling gebruikt alleen de brief.

## Vooraf vastgelegde ankers

- Webb: gevouwen voor de Ariane 5-kuip als korte aanleiding; week twee volledig ontvouwd versus zes maanden tot routinewetenschap, met afkoeling, uitlijning en kalibratie als concrete brug. Geen storing of herstel. Synthesis laat het woord "operational" ook op de uitgevouwen configuratie slaan; bewaar het onderscheid met wetenschappelijke gereedheid. Een los slotwoord in Synthesis hoort niet bij het verhaal.
- Wolf: Yellowstone, roedels als sociale families, prooien als voedsel voor andere soorten, veranderende predatie in een context met meerdere factoren. Voorbeelden van aaseters of de verschuiving bij kalveren en mannetjes maken het concreet. Elk is wapiti; geen eland of bewezen cascade.
- Digital: POA-teruggave onder tijdsdruk, aanvragers eerst en caseworkers later, afgebakende tijd voor onderzoek en testen. Bij gebruik van twaalf weken blijft het startpunt open. Input formuleert de duur zowel met "na" als "in"; behandel die spanning als upstream. Geen aangetoonde causaliteit met lanceertermijn of metrics.
- Lowell: neergang van textielwerk, samenwerking rond erfgoed en hergebruik, een latere terugval. Werkersonzekerheid en hergebruik van ruim honderd gebouwen zijn sterke concrete details. Geen monocausale opleving en geen uitspraak over het huidige Lowell. Vroege groei is facultatief.

Beoordeling is handmatig door de uitvoerende Work-assistent met zicht op de cases. Geen onafhankelijke review.

