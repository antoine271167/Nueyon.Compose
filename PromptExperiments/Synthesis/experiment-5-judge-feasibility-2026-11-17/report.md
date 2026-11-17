# Synthesis Experiment 5 — NO-GO

17 november 2026. Deze Judge is onvoldoende betrouwbaar om een optionele correctielus aan te sturen. Op de 24 opgeslagen Synthesis-outputs uit Experiment 4 zette hij drie van zes vooraf gelabelde gebreken op FAIL, bij slechts twee daarvan met een blocker die het bedoelde probleem benoemde. Hij keurde vijf van de achttien historisch voldoende outputs af en leverde drie keer geen verdict.

## Verwarringstabel

FAIL geldt als positief. Ongeldige responses staan apart, zodat een afgebroken beoordeling niet als PASS of FAIL wordt ingevuld.

| Set | TP | FP | TN | FN | Ongeldig |
|---|---:|---:|---:|---:|---:|
| Experiment 4, beide armen | 3 | 5 | 12 | 1 | 3 |
| Vijf herbeoordelingen uit Experiment 3 | 4 | 0 | 0 | 1 | 0 |

De primaire referentie komt uit de historische vijf dimensies van Experiment 4. Voor de Judge waren drie eerder als precisiegebrek beoordeelde tijdlijnfouten vooraf als corrigeerbaar gelabeld, naast twee materiële soortverwisselingen en één essentiële omissie. Het historische cohort is compleet voor de vijf eerdere herbeoordelingen en bevat uitsluitend vooraf bekende defecten. Het levert geen specificiteitsschatting op.

## Doelfouten en legitieme selectie

| Doel | Resultaat |
|---|---|
| **Yellowstone elk → eland** | 0/2 primair en 0/1 historisch inhoudelijk gedetecteerd. Call 10 is een TP op verdict maar corrigeert alleen «Wolvenkadavers». Calls 09 en 13 geven PASS. |
| **Webb formaat → vouwen** | Primaire call 02 stopte zonder verdict. Historische calls 03 en 25 herkennen beide de ontbrekende ontwerpverbinding. Beide hebben minimaal één anker met toegevoegde aanhalingstekens en falen de bevroren letterlijke controle. |
| **Onterechte Nuëyon-actor** | Historische call 27 herkent de onjuiste toeschrijving aan Nuëyon. De minimale correctie is passend, maar de opgegeven ankers bevatten toegevoegde aanhalingstekens. Call 25 maakt van de losse Webb-merkvermelding geen blocker. Experiment 4 bevat geen positieve actorcase. |
| **Tijdlijnrelaties** | Calls 21 en 23 vinden beide verzonnen startpunten van de twaalf weken en geven verankerde correcties. Call 05 over vouwen tijdens de lancering stopt zonder verdict. Call 27 benoemt ook het historische verschil tussen een deadline en gerapporteerde doorlooptijd. |
| **Lowell** | Vier PASS, twee false positives (08 en 17). De Judge eist institutionele context over universiteit en Wang als extra herstelbijdragen. Het vroege productie/banencontrast was facultatief en werd geen enkele keer geëist. |
| **Overige valide selectie** | Call 12 eist facultatieve grondbesturing/telemetrie. Calls 14 en 24 proberen de Selected Idea te corrigeren op passages die buiten de te beoordelen Synthesis staan. Dezelfde scopefout levert twee extra blockers in de terecht afgekeurde call 21. |

Vier van zestien blockers zijn onder de bevroren rubric direct bruikbaar, met letterlijke ankers en een kleine onderbouwde ingreep. Bij de historische Wolf-focuscase (11) dekt de bruikbare blocker de ontbrekende sociale samenwerking, maar vraagt hij geen concreet aaseterdetail. Een verkennende controle waarbij alleen het omhullende paar aanhalingstekens wordt genegeerd zou drie extra blockers bruikbaar maken (03, 25, 27 actor). Ook dan blijft de uitslag NO-GO. De poort is uitsluitend toegepast zoals bevroren. Geen voorgestelde correctie introduceerde volgens de handmatige beoordeling een nieuwe materiële feitelijke fout.

## Uitvoering en budget

29 eenmalige GPT-6 Luna Judge-requests kregen HTTP 200. Drie consumeerden hun volledige budget van 2.000 reasoningtokens en gaven geen tekst (02, 05 en 26). Geen retries, vervangingscalls, upstream- of downstreamgeneratie of correctielus. Voor de calls zijn protocol, prompt, labels, runner en alle exacte requests gehasht. De Judge zag de volledige bron, Facts/Unknowns-projectie, Selected Idea en Synthesis. Historische scores en volledige Research bleven buiten de requests.

| Usage | Tokens |
|---|---:|
| Input | 118.742 |
| Completion incl. reasoning | 31.758 |
| Waarvan reasoning | 28.604 |
| Totaal | 150.500 |

De bevroren completionlimiet was 58.000 tokens in totaal. De conservatieve bovengrens voor deze inputset was 596.144, op basis van requestbytes plus framing. De API rapporteerde nul gecachete inputtokens. Iedere call heeft de oorspronkelijke request, response en tijden. De evaluatie bevat individuele verdicts, blockers, historische labels, ankercontrole, usage en alle poortvoorwaarden.

## Vervolg

Laat een onafhankelijke beoordelaar vooraf nieuwe, ongeziene voorbeelden labelen waarin dezelfde fouttypen én acceptabele weglatingen voorkomen. Test daarop alleen detectie. Maak het reviewobject explicieter afgebakend van de Selected Idea en controleer de letterlijke ankers automatisch. Gebruik het aandeel afgebroken calls als aparte betrouwbaarheidseis. Overweeg pas een geïsoleerde correctieproef als die Judge materiële fouten herkent, valide selecties laat staan en bruikbare correcties levert. Deze proef telt vier terugkerende onderwerpen, deels herhaalde outputs en één handmatige beoordeling door een assistent. Ze meet geen effect van reparatie.

[Protocol](protocol.md) · [Judge-prompt](judge.md) · [Freeze](freeze.json) · [Labels](gold.json) · [Evaluatie](evaluation.json) · [Blockerbeoordeling](feedback-review.json) · [Offline replay](REPLAY.md) · [Artefacthashes](artifact-manifest.json)

Alle wijzigingen staan in de nieuwe Experiment 5-map. Geen productiecode, configuratie, productieprompts of eerdere experimenten gewijzigd. Geen Copilot CLI, promotie, cleanup, commit of push.
