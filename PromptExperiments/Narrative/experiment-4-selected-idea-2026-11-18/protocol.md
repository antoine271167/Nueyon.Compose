# Narrative Experiment 4

Basis 217569e56a992a64ffde6f80afd9d79f9fa375b0. A is de ingebedde productieprompt, B exact de compacte prompt uit Experiment 3, C B met één vervangen alinea. Vier byte-identieke fixtures en dezelfde beoordelingsbronnen uit Experimenten 1–3. Synthesis is de enige inhoudelijke autoriteit. Selected Idea bepaalt de redactionele invalshoek. De Narrative-brief blijft formatagnostisch.

## Calls

36 onafhankelijke Narrative-aanvragen, drie per arm per case. Alle zes armpermutaties komen twee keer voor, met vier optredens per arm op iedere positie. Zelfde GPT-6 Luna, productieagent en schema, providerdefaults en 5000 completiontokens zoals Experiment 3. Geen gedeelde sessie. Eén HTTP-aanvraag per poging, nul retries. Een begonnen poging blijft zichtbaar en wordt niet vervangen.

Daarna twaalf zelfstandige LinkedIn-aanvragen via de productie-Compose-agent met ingebedde productieprompts, GPT-6 Luna, productieschema en providerdefaults zonder experimentele tokenlimiet. Webb r1, Wolf r2, Digital r3, Lowell r1 worden vooraf geselecteerd voor alle armen, zonder kwaliteitsselectie. Dit is een vaste doorsnede over de vier cases met één herhaling per case en arm. Compose krijgt alleen de volledige geselecteerde Narrative-brief en LinkedInPost zoals in de workflow.

## Handmatige rubric Narrative

Score 2 bij voldoende, 1 bij herstelbare tekortkoming, 0 bij materiële tekortkoming. Volledig voldoende vereist vijf keer 2. Gebruik de hele brief, inclusief beperkingen. Noteer gepaarde dalingen op alle assen tegenover A én B.

- Focus. Gesteunde invalshoek van Selected Idea centraal in boodschap, detailkeuze en endpoint. 1 bij verschuiving of zijpad, 0 bij een andere hoofdzaak.
- Verhaalprogressie. Context, ontwikkeling en eigen uitkomst volgen inhoudelijk. 1 bij een herhaald of schetsmatig slot, 0 bij ontbrekende verhaallijn.
- Details. Concrete gesteunde details met een functie in het verhaal. 1 bij een gemiste dragende bouwsteen, 0 bij vrijwel alleen abstractie.
- Brontrouw. Behoud actoren, soorten, timing, causaliteit, scope en zekerheid. 1 bij lokale aanscherping, 0 bij materiële betekenisfout. Score tegen Synthesis. Gebruik bron en projectie om upstream-afwijkingen te lokaliseren. Afwijkingen tellen tegen Narrative wanneer de brief ze introduceert of versterkt.
- Handoff. Zelfstandig bruikbare Selected Story en drie onderscheiden beats, ruimte voor verschillende publicatievormen, specifieke feitelijke grenzen waar nodig. 1 bij gerichte revisie, 0 bij onbruikbaarheid.

Ankers uit eerdere proeven. Webb bewaart vouwreden, uitvouwing in week twee en routinewetenschap na zes maanden, met commissioning en open overlap. Wolf bewaart roedels, prooi en aaseters, veranderende predatie en multifactoriële context, elk betekent wapiti. Digital bewaart POA, aanvragers eerst, vervolgens caseworkers, onderzoek/testen parallel aan behandeling waar van toepassing. Twaalf weken heeft een open startpunt, cijfers illustreren hoogstens. Lowell bewaart neergang van textielwerk, samenwerking, hergebruik en latere terugval met historisch perspectief en proportionele causaliteit. Aanvullende feiten zijn facultatief als de kern blijft staan.

## Handmatige rubric LinkedIn

Vier aparte scores 0–2 met dezelfde betekenis. Aantrekkelijkheid beoordeelt concrete opening en leesreden. Leesbaarheid beoordeelt heldere tekst, samenhang en passend ritme. Centrale boodschap volgt de gesteunde Selected Idea door het einde heen. Feitelijke juistheid volgt Synthesis, met oorspronkelijke bron voor de herkomst van fouten. Traceer iedere geconstateerde afwijking naar upstream, Narrative of Compose, inclusief correctie, doorwerking en versterking. Noteer ook wanneer Compose een zijpad uit de brief centraal maakt.

## GO

Alle 48 live en replay-controles slagen. Voor C minimaal 10/12 volledig voldoende, minimaal twee meer dan B, minimaal evenveel als A en minimaal 2/3 per case. Focus minimaal 11/12 en hoger dan B of gelijk bij een gedeeld plafond van 12/12, en minstens A. Verhaalprogressie, details en handoff minstens B. Brontrouw 12/12. Geen gepaarde 2→0 op enige as tegenover A of B. C gebruikt maximaal 115% van B's totale Narrative-tokens en maximaal 50% van A's totaal. Downstream heeft C minstens evenveel scores van 2 als A en B op elke LinkedIn-as, minimaal 3/4 voor centrale boodschap en geen materiële feitelijke fout. Anders NO-GO. GO biedt grond voor onafhankelijke validatie, waarna promotie afzonderlijke toestemming vraagt.

Eén handmatige beoordelaar voert ook het experiment uit. Armkennis, herhaalde cases en voorkennis kunnen scores beïnvloeden. Vier doelgericht gekozen cases, één upstream-output per case en één downstream-uitvoering per case en arm. Resultaten zijn beschrijvend. Tokens komen uit providerusage, financiële kosten worden niet geschat.
