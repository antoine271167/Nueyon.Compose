# Experiment 4 — vooraf vastgelegd protocol

12 november 2026. HEAD bij start 094b2e539f7b625c0481d38f04d4105815a568dd; werkboom schoon. Geen Copilot CLI, commit of push. Alleen de expliciet toegestane null-check en regressietest wijzigen productie. Research-generatorprompts, filter, workflow en EnableResearchJudge blijven ongewijzigd (default false).

## Hypothesen en vaste steekproef

1. Bronankers kunnen deterministisch exact blijven wanneer de mapper alleen bestaande fragment-ID's selecteert en software de bijbehorende bronspans ophaalt. Exactheid is geen bewijs van semantische steun, voldoende scope of materialiteit.
2. Historische Intro: de bron stelt expliciet de concrete-expression-relatie; Facts 9/13 bewaren die, maar baseline Unknown 4 ontkent deze te breed. Onbekend lezereffect blijft legitiem. Historische Experiment-3-acceptatie is een judge-false-negative; de nieuwe proef meet of dit opnieuw optreedt, niet de verborgen oorzaak.
3. De evidence map heeft alleen aangetoonde incrementele waarde binnen deze set als met identieke judgeprompt meer doelproblemen bruikbaar worden gevonden, zonder overrejectie/parserregressie, tegen aanvaardbare extra calls/tokens.

Exact dezelfde zes historische bron/Idea/Research-fixtures als Experiment 3, inclusief aangevulde correcte 005-controle. Geen aanpassing aan labels of teksten. labels.json kopieert de drie historische referenties; bad=reject, correct=accept. Primaire doelen: Intro Unknown 4; 005 upstreamvariatie (met Synthesis-spanning als aanvullende relevante beperking); 010 feitfout bij behouden hiërarchie. Afwijzing om een ander probleem telt niet als targetdetectie. Correcte controles worden conservatief geaccepteerd als equivalente/verspreide inhoud afdoende is. Alle diagnoses apart beoordelen op bronsteun, scope, materialiteit, niet-prescriptieve grens en exacte ankers.

Dit is een gerichte historische diagnostiek, geen nieuwe onafhankelijke holdout of bewijs van generalisatie. Labels, ontwerp en beoordeling komen van dezelfde ongeblindeerde assistent. Geen case-specifieke regels toevoegen aan prompts of productie. De identieke Judge-prompt is bytegelijk aan Experiment 3; dus geen nieuwe promptoptimalisatie op Intro. Mapperprompt verandert uitsluitend het ankermechanisme. Geen tuning na de eerste call.

## Ankercontract en offline aanvallen

Splits originele .NET/JavaScript-string deterministisch op LF, met LF/CRLF en whitespace intact binnen het fragment. IDs F0001 enzovoort; offsets en lengtes in UTF-16-code-units, plus SHA256 van UTF-8-bron. Reconstrueer de volledige bron bytegelijk in UTF-8. Lege/ongekende/duplicaat-ID's en veranderde bron/hash/span zijn fouten, geen herstel of normalisatie. Hydratatie voegt gecontroleerde sourceQuotes toe; map-raw blijft bewaard. Judge gebruikt het ongewijzigde productie-verdictcontract met letterlijke sourceQuote-controle. Deze proef maakt de mapperankers deterministisch, niet de door de Judge gegenereerde citaten.

Offline controles: ronde-trip alle drie historische bronnen; CRLF, Markdown, Unicode/decomposed accents en whitespace; hoofdletterwijziging en Unicode-normalisatie moeten ongelijk blijven; onbekend/duplicaat-ID en gemuteerde span/bron worden geweigerd. Audit de drie historische maps inhoudelijk vóór de judges; auditgegevens en labels gaan nooit in requests. Ongeldige map: markeer met-map-uitkomsten als technisch mislukt en sla bijbehorende calls over; geen gerepareerde/vervangende map. Alle uitgevoerde pogingen tellen.

## Live budget en volgorde

GPT-6 Luna (`gpt-6-luna`), uitsluitend bestaande geconfigureerde modeltoegang. Hard maximum 18 HTTP-requests; vaste planning 15: drie maps, daarna twaalf judges. De drie overige calls blijven ongebruikt. Geen retries, herhalingen, repairs, Research-generaties, Synthesis-modelcalls of externe retrieval. SDK RetryPolicy(0), één HTTP-request per poging, exclusieve runmarkers en globaal budget. Per request maximaal 6000 completion tokens, maximaal 90000 completion tokens voor planning; inputkosten gemeten, geen fictieve europrijs. Stop bij transportfout voor die poging, registreer fout en ga zonder retry naar volgende geplande call.

plan.json legt een afwisselende armvolgorde vast per case/variant (zonder/met, met/zonder). Alle maps eerst, elk gedeeld door beide varianten. Identieke originele bron, vijf Idea-velden, Research, echte SynthesisResearchFilter-output, modelinstellingen, schema en systemprompt in beide armen. Enig payloadverschil: evidenceMap aanwezig in met-map-arm. Zonder map eveneens veldnaam originalSource. Geen eerdere modelverdicts meegestuurd. Single-run pair comparison vermindert confounding maar alias/tijd/stochastiek en kleine N blijven; geen causale zekerheid of repeatabilityclaim.

Vóór eerste call freeze.json met SHA256 van protocol, labels, fixtures, catalogs, prompts, plan, runner en gewijzigde parser/test. Offline preflight vóór freeze toegestaan; geen inhoudelijke wijzigingen na freeze. Eén mapfase en één judgefase, beide niet herstartbaar. Bewaar attempt markers, volledige requests/responses zonder sleutels/authheaders, HTTP-status, model-ID, usage inclusief caches/reasoning en fouten. Ongeldige output blijft in kosten en noemer. Productieparser offline herafspelen en filterinput/requests/freeze verifiëren.

## Vooraf bepaalde GO/NO-GO-criteria

GO alleen als alle drie maps exact valide, de twaalf verdicts parsergeldig, met-map alle drie targets detecteert en alle drie correcte controles accepteert, iedere dragende diagnose materieel/brongetrouw is, en met-map strikt meer bruikbare targets vindt dan zonder-map zonder verlies op controles. Extra tokens inclusief drie maps maximaal +80% tegenover zes zonder-map-judges; calls maximaal 9 tegenover 6 (+50%). Elke falende eis = NO-GO; geen promotie of activering. Zelfs GO is alleen aanleiding voor afzonderlijke onafhankelijke validatie en vereist aparte toestemming voor productie-integratie.

Rapporteer kwaliteit, anchors, parsergeldigheid, calls en input/output/totaal tokens per arm en met geamortiseerde mapkosten. Reasoning niet dubbel tellen. Hergebruikte Experiment-2/3-data afzonderlijk, nooit als nieuwe calls/resultaten. Onderzoek Intro op bron/Fact/Unknown/map/diagnose-niveau en benoem alternatieve verklaringen als hypothesen. Offline build/relevante tests; nul-check moet InvalidOperationException opleveren zonder repair/downstream-calls. Geen overige integratie.
