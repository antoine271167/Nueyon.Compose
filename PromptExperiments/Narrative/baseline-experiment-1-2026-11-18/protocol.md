# Narrative Experiment 1 — vastgelegd protocol

Productiebasis 918eff3. Vier bewaarde Synthesis-resultaten uit Experiment 4, arm A: Webb r1, Wolf r2, Digital r1 en Lowell r1. Deze vaste runs bieden bruikbare scenario's met een bekende upstream-rafelrand bij Webb en Digital. Selected Idea komt uit dezelfde case. Research-projectie en bron zijn uitsluitend beoordelingsmateriaal. Volgorde per ronde: Webb, Wolf, Digital, Lowell. Drie rondes, elk met dezelfde input en zonder gedeelde sessie.

Productie-Narrative-agent, ingebedde productieprompts en bestaand JSON-schema. GPT-6 Luna uit de lokale Host Console-configuratie, Chat Completions, providerdefaults voor sampling en reasoning, maximum 5.000 completiontokens, één request per poging, geen automatische retries. Eén run per runner-invocation. Een poging met marker wordt niet stilzwijgend herhaald. Geen andere agentstappen.

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
