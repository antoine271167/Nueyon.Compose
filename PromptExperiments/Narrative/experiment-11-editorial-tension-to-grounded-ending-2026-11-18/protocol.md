# Experiment 11 — Editorial Tension to Grounded Ending

## Vraag en interventies

Kan Compose met één concrete bewijsrichting een onderbouwd slot schrijven dat verder komt dan de opening of de cijfers? A gebruikt productie-Compose en de ongewijzigde brief. B gebruikt productie-Compose met één aanvullende, formatagnostische passage in NarrativeResult.Content. De passage benoemt wat twee al aanwezige concrete observaties samen duidelijk maken binnen de Selected Idea. De passage bevat geen slottekst, nieuwe claim of correctie van de bestaande brief. De uitvoerder schreef vier passages uit de bevroren Synthesis en gebruikte dezelfde bewijsrichting op beide briefs per onderwerp, in de taal van de brief. Dit meet het effect van een vooraf gecontroleerde handoff, zonder uitspraken over de betrouwbaarheid van automatische generatie ervan. C gebruikt de ongewijzigde brief en één gewijzigde LinkedIn-slotinstructie. B en C worden elk met A vergeleken. Er is geen combinatie-arm of historische controle. Selected Idea bepaalt focus en Synthesis autoriseert inhoudelijke claims. Compose krijgt uitsluitend de Narrative-tekst en het kanaal.

## Bevroren opzet

De exacte acht briefs uit Experiment 10, een bronveilige en een problematische brief per onderwerp, inclusief de F-uitzonderingen voor Wolf en Lowell. Twee stateless GPT-6 Luna-herhalingen per fixture en arm, 48 calls in 16 blokken. Elke arm staat per stratum twee of drie keer op elke positie. De startvolgorde wisselt binnen iedere fixture. Verse context per call. Productieschema en SDK, één HTTP-poging en vijf minuten timeout. Geen temperatuur, seed, extra tools of retry. Alle calls blijven in de analyse. Stop op HTTP-, model-, schema- of replayafwijking, conflict, Ctrl+C of PAUSE. PAUSE stopt vóór de volgende call en kan worden verwijderd voor hervatting. Een onafgeronde poging wordt niet opnieuw uitgevoerd. Onvolledige reeks betekent NO-GO.

## Rubric vooraf

Score per post zes redactionele dimensies, elk 0 afwezig/mislukt, 1 gedeeltelijk, 2 voldoende. Beoordeel willekeurig geordende posts zonder zichtbaar armlabel. Dezelfde uitvoerder schrijft de passages en beoordeelt. Herkenning blijft mogelijk.

1. Aantrekkelijkheid. Een concrete inzet motiveert doorlezen.
2. Leesbaarheid. Natuurlijke, scanbare professionele tekst die een buitenstaander begrijpt.
3. Hook. Meteen specifiek en relevant voor het onderwerp. Een materieel ongegronde openingsclaim haalt geen 2.
4. Spanningsboog. Context, ontwikkeling en betekenis hebben eigen functies en bouwen op elkaar voort.
5. Slot. De laatste inhoudelijke gedachte gebruikt concrete bewijsdetails om een lezer een onderwerpgebonden inzicht te geven dat de hook en hoofdtekst verder brengt. Een cijferlijst, generieke les, parafrase van de opening, nagekomen disclaimer of nieuwe onbewezen implicatie haalt geen 2. Noteer opening, slot en exacte Synthesis-ankers.
6. Focus. De Selected Idea stuurt de post. Details of cijfers verdringen de aanpak niet.

Brontrouw 2 volledig materieel ondersteund, 1 mogelijk materiële ambiguïteit, 0 duidelijke materiële afwijking. Beoordeel de hele post inclusief implicaties. Label iedere afwijking overgenomen of nieuw/versterkt en noteer de briefpassage of afwezigheid en het Synthesis-anker. Overgenomen en nieuwe afwijking kunnen tegelijk voorkomen. Noteer ook neutralisering van bekende brieffouten. Webb controleert Ariane 5, week twee en maand één als afzonderlijke vensters en routinewetenschap na zes maanden. Wolf controleert elk/wapiti, prooikarkassen en meerdere invloeden. Lowell controleert verslagperspectief, oude gebouwen, afzonderlijke actoren en samenloop. Digital controleert aanvragers eerst, het sprintritme en afzonderlijke tijdvensters en cijfers zonder causale koppeling.

## Beslisregels per kandidaat

Alle 48 calls en exacte offline replays vereist. Minstens 10/16 sloten scoren 2, winst van minstens vier voldoende sloten tegenover A en minstens twee voldoende sloten per case. Minstens 7/8 posts op veilige input blijven volledig brongetrouw. Het aantal posts met nieuwe afwijkingen of risico's stijgt niet. Een nieuw materieel risico op claimniveau tegenover de gepaarde A is een blokkade, ook bij een dalend totaal. Overgenomen afwijkingen op problematische input nemen niet toe. Minstens 12/16 op elk ander redactioneel criterium halen 2 en het subtotaal zonder slot daalt niet ten opzichte van A. Maximaal 10% hogere totale tokens en geschatte kosten. Gebruik vaste Experiment 10-tarieven voor vergelijking, met cache en als gevoeligheidsanalyse zonder cachekorting. Eén onvoldoende gate betekent NO-GO voor de kandidaat. Proefbesluit is GO als minstens één kandidaat slaagt. Geen productiepromotie.
