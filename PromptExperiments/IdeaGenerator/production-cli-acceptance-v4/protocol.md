# Experiment v4

## Bevroren conditie en budget

Revisie 525b2ea1cfdbc97e4df6f9cbe9858d29db133eda. De actuele productiepromptset is byte-identiek gekopieerd. Er is één conditie en geen promptafstelling tijdens v4. De actuele gebruikersopdracht geldt als autorisatie voor de betaalde fase.

Het vooraf geschreven frozen.json reserveert maximaal 48 logische GPT-5-mini-aanroepen. Zeven afzonderlijke productieprocessen reserveren ieder zes aanroepen, maximaal drie generator- en drie judge-aanroepen. Alleen wanneer alle eerste gates slagen volgt een achtste proces ter bevestiging van de nieuwe holdout. Een mislukte of onderbroken run behoudt zijn reservering. Het script overschrijft gebruikte slots niet.

Het plafond betreft applicatie-aanroepen. De ongewijzigde productie-CLI geeft providerfacturering, tokenverbruik, modelsnapshot en eventuele SDK-netwerkherpogingen niet weer. Er is geen bedragplafond geïmplementeerd.

## Offline gates

De volledige oplossing moet bouwen en alle vindbare tests moeten slagen. Bestaande regressietests controleren onder meer verplichte accepted/blockingIssues, gesloten blockers met category, field, problem, sourceEvidence en constraint, afwijzing van correction en andere voorschrijvende velden, dubbele properties en tegenstrijdige verdicts.

Het aangepaste v3-harnas gebruikt de echte IdeaAgent, IdeaSemanticJudge en validatielus met vaste IChatClient-antwoorden. Drie reeksen controleren afwijzen, afwijzen, accepteren. Drie verdere reeksen vereisen exact drie afwijzingen en de expliciete eindfout. Bij iedere generatie worden de oorspronkelijke opdracht en bron bytegetrouw gecontroleerd. Herpogingen mogen uitsluitend het vorige eerste idee en de laatste diagnose toevoegen. De judge krijgt opnieuw de volledige bron en het actuele eerste idee. Elke aanvraag inclusief instructies en responsschema wordt vastgelegd.

De zes vaste diagnoses zijn handmatig beoordeeld op de oorspronkelijke bron en opdracht. Ze begrenzen claims en doel zonder vervangende tekst, nieuwe redactionele opdracht of onderzoeksagenda. Dit meet transport en contract. De inhoudelijke kwaliteit van echte modeldiagnoses wordt afzonderlijk beoordeeld.

## Cases en scores

De oorspronkelijke drie gevallen zijn strategie, Experiment 005 en Experiment 010, ongewijzigd gekopieerd uit v3. Museum is de bekende regressie. Vervoer en bibliotheek zijn bekende holdouts. De nieuwe synthetische zaadbankbron is opgesteld nadat de productiepromptconditie was gekozen en vóór haar eerste modelaanroep. Zij wordt voor geen promptafstelling gebruikt.

Beoordeel uitsluitend het definitieve eerste idee, met titel, beschrijving, doelgroep, rationale en bewijs, tegen de volledige bron. Ken voor brontrouw, redactioneel doel, focus, onderscheidende invalshoek en aantrekkingskracht elk 0–2 toe. Een 0 mist de dimensie, 1 voldoet gedeeltelijk, 2 voldoet volledig. Een score van minstens 8/10 én 2/2 voor brontrouw en doel is vereist. Strategie vereist tevens 10/10. De oorspronkelijke reeks vereist 3/3, de vier holdoutbronnen 4/4, gevolgd door een geslaagde bevestiging op de nieuwe bron.

Specifieke grenzen volgen uit de bronnen. Strategie introduceert het bedrijf met Compose als eerste richting. 005 bewaart de beperkte vergelijking en niet-promotie zonder causale generalisatie. 010 laat het interventiepunt volgen uit de vroegste onderbouwde afwijking per gekozen geval. Museum blijft een bezoekersbericht met een alternatief bij hoogwater. Vervoer behoudt de expliciet gekozen lokale tijdelijke dienstregelingproef. Bibliotheek behoudt bestaande reserveringen, tijdelijke afhaallocatie en ongewijzigde retourregeling. Zaadbank bewaart de waargenomen kieming, niet-gepaarde historische vergelijking, beperkte voortzetting met dezelfde twee partijen en onbewezen effect op tuinen.

Dezelfde agent beoordeelt de ideeën onafhankelijk van het judgeverdict voordat hij de diagnoses inspecteert. De beoordeling is geen externe of volledig geblindeerde evaluatie. CLI-exitcodes en het bestaan van resultaatbestanden kunnen zichtbaar zijn.

## Diagnoses en overdracht

Beoordeel iedere feitelijke diagnose apart op correcte signalering, brongetrouwe onderbouwing en afwezigheid van nieuwe claims, verplichte koerswijzigingen, vervangende tekst of extra onderzoek. Een correct verdict met inhoudelijk onzuivere feedback faalt de feedbackgate. Beoordeel ook iedere geaccepteerde poging op eventuele gemiste blokkades. Een brongegronde herstelgrens voor een werkelijk verkeerde opdracht telt als diagnose.

Na de onafhankelijke beoordeling ontstaan uitsluitend voor geslaagde gepubliceerde ideeën ResearchInput-bestanden. input.content en selectedIdea.idea worden ongewijzigd overgenomen en tegen het actuele CLI-schema gevalideerd. Afkeuringen blijven fixtures met resultaat, trace en beoordeling.

## Reproductie en promotie

prepare.ps1 bevriest nieuwe bronnen, fixtures, harnas en promptset eenmalig. run.ps1 bewaart revisie, SDK, exacte commandoreeks, hashes, modelalias, reservering, tijden, exitcode, stdout, stderr, resultaat en volledige CLI-trace. Het harnas reconstrueert achteraf dezelfde applicatieverzoeken offline uit de ongewijzigde CLI-trace. Deze reconstructie omvat systeminstructies, berichten en JSON-schema. Zij is geen onderschept providerverkeer en de ruwe judgeverdicts zijn niet door de CLI geëxporteerd.

Promotie vereist alle vooraf vastgelegde inhoudelijke, feedback-, holdout- en verificatiegates. Bij een gemiste gate blijft de productieconditie behouden. De vergelijking met v1–v3 is beschrijvend. De oudere kandidaten, modelvariatie en gelijktijdig gewijzigde architectuur en validatorprompt laten geen geïsoleerde causale effectschatting toe.
