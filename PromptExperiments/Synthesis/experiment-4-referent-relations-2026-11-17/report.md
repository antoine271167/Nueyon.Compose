# Synthesis Experiment 4 — NO-GO

17 november 2026. A is de onderwerpneutrale variant B uit Experiment 2. **A haalde 10/12 volledig voldoende, C 8/12.** C maakte dezelfde materiële soortfout als A en liet één keer de vouwreden bij Webb weg. A behield die reden in alle drie de Webb-runs. C haalde de vooraf vastgelegde poort voor onafhankelijke vervolgvalidatie niet.

## Promptverschil

Eén zin toegevoegd aan §5 van de systeemprompt. De userprompt is bytegelijk.

```diff
+Preserve entity identity when translating, and keep source-supported reasons that are essential to the Selected Idea.
```

[Exact diff](prompt.diff) · [Bevroren protocol](protocol.md) · [Schema en budget](plan.json) · [SHA-256-freeze](freeze.json)

## Resultaten per case

| Case | A voldoende | C voldoende | Waarneming |
|---|---:|---:|---|
| Webb | 3/3 | 1/3 | Vouwreden expliciet in 3/3 A en 2/3 C. Eén andere C-output situeert het opvouwen tijdens de lancering. |
| Wolf | 2/3 | 2/3 | Beide armen vertalen *elk* één keer als *eland*. De twee overige outputs per arm behouden *elk* in het Engels. |
| Digital | 2/3 | 2/3 | Elke arm specificeert eenmaal een niet-gedocumenteerd beginpunt voor de twaalf weken. |
| Lowell | 3/3 | 3/3 | Beide armen behouden menselijk verval, samenwerking, hergebruik en economische terugval. |
| **Totaal** | **10/12** | **8/12** | **C haalt de ondergrens van 11/12 en 2/3 per case niet.** |

- **Wolf.** Zowel A-r1 als C-r1 verwisselt de soort in centrale prooi- en aaseterrelaties. Dezelfde opgeslagen bron gebruikt *elk* in regels 11 en 17 en de projectie levert het in Facts 6–9. In Yellowstone verwijst *elk* naar wapiti. Alle C-runs bevatten wel concrete voedselrelaties. De twee correcte C-runs vertalen de soortnaam niet, dus deze steekproef laat geen geslaagde vertaling door C zien. [A-r1](calls/wolf-r1-A-Synthesis/content.md) · [C-r1](calls/wolf-r1-C-Synthesis/content.md) · [NPS over wapiti](https://www.nps.gov/yell/learn/nature/elk.htm) · [NPS over eland](https://www.nps.gov/yell/learn/nature/moose.htm)
- **Webb.** C-r2 noemt raketkuip en afmetingen als context, maar geeft de relatie formaat → vouwen niet door. Die staat in bronregel 3 en projectie-Fact 1. C-r1 behoudt de reden, maar zegt «door tijdens de lancering op te vouwen». De bron beschrijft een al opgevouwen lanceerconfiguratie. Beide C-outputs geven bruikbare technische details en het verschil tussen uitvouwing en routinewetenschap. [C-r2](calls/webb-r2-C-Synthesis/content.md) · [C-r1](calls/webb-r1-C-Synthesis/content.md) · [Webb-projectie](cases/webb/projection.md)
- **Digital.** A-r2 maakt van het onbekende beginpunt «de start van het team» en C-r1 «aanvang van de levering». De publieke lancering binnen twaalf weken en de ontwerpkeuzes blijven in beide intact. Het weglaten van de detailcijfers in A-r2 is passend bij de gekozen focus en telt niet als omissie. [A-r2](calls/digital-r2-A-Synthesis/content.md) · [C-r1](calls/digital-r1-C-Synthesis/content.md) · [Digital-projectie](cases/digital/projection.md)
- **Lowell.** Beide armen behouden de historische context en selecteren concrete grondstof zoals onzekere werkdagen, hergebruik van ruim honderd gebouwen en de latere terugval. Weglaten van het vroege productie- en banencontrast past bij deze Idea. [Lowell-projectie](cases/lowell/projection.md)

C scoorde op brontrouw twee paren lager en één hoger. Op essentiële betekenisrelaties scoorde C één paar lager. Focus, concrete details en bruikbaarheid voor Narrative bleven in alle paren gelijk. Beide armen gebruikten selectief bewijs. In C is geen herhaalde essentiële omissie vastgesteld. De andere verschillen, afzonderlijke scores en onderbouwing staan in [scores.json](scores.json) en de poortberekening in [gate-evaluation.json](gate-evaluation.json).

## Uitvoering en controle

Experiment 1 bepaalde de vaste cases. Experiment 2 leverde de onderwerpneutrale prompt en de case-specifieke rubric, inclusief de facultatieve Lowell-context. Experiment 3 duidde de soortverwisseling als materieel en de ontbrekende Webb-reden als een selectieprobleem. De nieuwe vergelijking gebruikt verse gepaarde calls. Historische outputs tellen niet mee.

De vier bron/Idea/Research B2-r1/projecties zijn bytegelijk aan de opgeslagen fixtures uit Experiment 2. Er is geen onafhankelijke holdout toegevoegd. De echte productiefilter reconstrueerde de vier projecties exact. Alleen de echte SynthesizerAgent draaide live via zijn bestaande promptloader, response-schema en parser. De 24 GPT-6 Luna-calls slaagden met één HTTP-request per poging en zonder retries. Er is geen upstream- of downstreamgeneratie uitgevoerd. Onderbouwing en beoordeling bleven buiten de live requests.

| Arm | Inputtokens | Completiontokens incl. reasoning | Totaal |
|---|---:|---:|---:|
| A | 64.581 | 20.777 | 85.358 |
| C | 64.821 | 19.556 | 84.377 |
| **Totaal** | **129.402** | **40.333** | **169.735** |

Van het totaal waren 95.184 inputtokens gecachet en 27.470 completiontokens reasoning. De [usage per request](usage.json) is rechtstreeks overgenomen uit de API-responses. Geen geldbedrag afgeleid.

De beoordeling is handmatig uitgevoerd door één Work-assistent die de armen kende. Drie herhalingen per case, dezelfde vier eerder gebruikte cases en wisselende outputtaal begrenzen de conclusie. De proef laat zien dat C onder deze meetlat beide doelproblemen onvoldoende ondervangt. Zij meet geen algemeen betrouwbaar effect van de ene zin.

Requests en ruwe responses staan per call in [calls](calls/). De [requestcontrole](verification-requests.json) bevestigt prompt, exacte rendering en grenzen per call. [Offline replay](verification-offline-replay.json) leverde 24/24 identieke parserresultaten op. [Replay-instructies](REPLAY.md) · [Artefacthashes](artifact-manifest.json).

Productieprompts, config, code en eerdere experimenten zijn behouden. Geen promotie, cleanup, Copilot CLI, commit of push.

