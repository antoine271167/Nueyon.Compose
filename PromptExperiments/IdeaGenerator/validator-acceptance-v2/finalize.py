import pathlib,json,datetime
R=pathlib.Path(__file__).resolve().parent
s=json.loads((R/'verification/scores.json').read_text()); d=json.loads((R/'verification/diagnosis-summary.json').read_text()); ratings=json.loads((R/'verification/diagnosis-ratings.json').read_text())
def pct(x):return f'{100*x:.1f}%' if x is not None else '—'
table='| Set | Prompt | TP | FP | TN | FN | Precision | Recall |\n|---|---|---:|---:|---:|---:|---:|---:|\n'
for row in s['stats']:
 table+='| '+ ' | '.join(str(row[k]) for k in ['phase','condition','TP','FP','TN','FN'])+' | '+pct(row['precision'])+' | '+pct(row['recall'])+' |\n'
diag='| Prompt | Blockers | Reële mismatch | Brongetrouw | Niet-voorschrijvend | Alle drie |\n|---|---:|---:|---:|---:|---:|\n'
for r in d:diag+='| '+' | '.join(str(r[k]) for k in ['condition','blockers','realMismatch','sourceFaithful','nonPrescriptive','allThree'])+' |\n'
agree={c:sum(a['agrees'] for a in s['agreement'] if a['condition']==c) for c in ['baseline','candidate1','candidate2']}
gate=[]
for cond in ['baseline','candidate1','candidate2']:
 r=next(r for r in s['stats'] if r['phase']=='unique' and r['condition']==cond); ds=next(x for x in d if x['condition']==cond)
 gate.append(dict(condition=cond,recallGate=r['recall']>=.95,zeroFalseRejects=r['FP']==0,allKnownCriticalDetected=False,diagnosisGate=ds['allThree']==ds['blockers'],promoted=False))
(R/'verification/gates.json').write_text(json.dumps(gate,indent=2),encoding='utf-8')
diagnoses='# Beoordeling per blocker\n\nHandmatig, niet geblindeerd; de volledige antwoorden staan in verification/blockers.json. Het vóór live calls bevroren protocol is leidend. Brongetrouw vereist dat ook problem en constraint geen extra aannames toevoegen. Een expliciete bronlimiet is geen voorschrift; vervangende formuleringen, extra meetvereisten of een nieuwe invalshoek wel. Dubbele blockers zijn afzonderlijk geteld; dit kan percentages beïnvloeden.\n\n| Fase | Prompt | Case | # | Reële mismatch | Brongetrouw | Niet-voorschrijvend | Reden |\n|---|---|---|---:|---|---|---|---|\n'
for r in ratings:diagnoses+='| '+' | '.join(str(r[k]) for k in ['phase','condition','id','index','realMismatch','sourceFaithful','nonPrescriptive','note'])+' |\n'
(R/'diagnoses.md').write_text(diagnoses,encoding='utf-8')
interval=next(r['recallWilson95'] for r in s['stats'] if r['condition']=='candidate2' and r['phase']=='unique')
report=f'''# Validator Experiment v2 · Nuëyon.Compose

**11 november 2026 · HEAD `2ca16edd69c1b738d1265fbb27134a95bc7fcfd1` · No-go voor promotie.** Kandidaat 2 verbetert de gemeten detectie van 8/15 naar 12/15 fouten en accepteert alle 15 correcte gevallen. De originele causale overclaim en Narrative-prioriteit blijven echter ongedetecteerd, ook bij herhaling. Geen kandidaat haalt de brongetrouwe, niet-voorschrijvende diagnosegate. Productiecode, productieprompt en Idea Generator zijn ongewijzigd. Geen commit of push.

## Wat v1 precies miste

V1-baseline accepteerde 010-b (onvoorwaardelijke Narrative-first-titel ondanks voorbehoud elders) en museum-b (bezoekers vervangen door managers). De v1-kandidaat blokkeerde beide aanvankelijk, maar accepteerde 010-b bij herhaling. Zijn eerste 010-diagnose maakte de next-trial-claim universeel en schreef voor Narrative als frequentste interventiekandidaat te presenteren: de bron noemt een plaats van zichtbare drift, geen interventierangorde. De tweede museumblocker verzon welke uitkomstgegevens een managementbesluit zou vereisen. Dit zijn diagnosefouten, ook met juiste broncitaten. V4 leverde zes originele false accepts en geen live blockers; diagnosekwaliteit was daar onwaargenomen. [Analyse vóór live calls](analysis-v1.md), [v1-snapshots](snapshots/validator-v1/), [v4-traces](snapshots/v4/).

## Bevroren ontwerp en scheiding

30 bron/idee-cases, 15 goed en 15 fout: 22 tuning (11/11), 8 definitieve holdout (4/4). Vijf v1-paren veranderen één veld; zes v4-originals zijn exact tegen de oorspronkelijke eerste selectie in de trace gecontroleerd en hebben correcte tegenvoorbeelden; vier nieuwe synthetische holdoutparen veranderen één veld. De v4-correcties mogen meerdere velden veranderen. De set bestrijkt expliciete feiten, voorzichtige interpretatie, causaliteit, interventieprioriteit, publiek/doel, motieven en operationele voorwaarden. [Tuning](tuning/cases.json), [holdout](holdout/cases.json), [protocol](protocol.md).

Kandidaat 1 voegt drie onderscheidingen toe aan de productieprompt: bewijsniveau, communicatieopdracht en diagnosegrens. Kandidaat 2 herschrijft de judgeprompt rond twee interne vragen: Source Fidelity en Editorial Intent, binnen één modelcall en hetzelfde schema. Dit is geen test van twee afzonderlijke judges. Beide prompts zijn vóór de holdoutbronnen en vóór alle live calls vastgezet; daarna niet aangepast. Verwachte verdicts en redenen, bronnen, prompts en harnas zijn vóór live calls gehasht. Alleen bron en idee bereiken het model, nooit labels, redenen, andere cases of eerdere verdicts. Per call een nieuw proces en agentsessie. Dezelfde C#-IdeaSemanticJudge, schema en Chat Completions-adapter voeren iedere conditie uit; offline probes controleren exact transport en parsergedrag.

De recall-first-selectie kiest kandidaat 2 op tuning (8/11 versus 4/11), vóór de eerste live holdoutcall. De selectie is gehasht en de volgorde is gecontroleerd. Alle drie prompts doorlopen dezelfde holdout voor vergelijking; geen herselectie of tuning achteraf. De holdout is ongezien door de validator, maar opgesteld en beoordeeld door dezelfde auteur die de prompts maakte. Er is geen onafhankelijke menselijke blindering. [Selectie](selection.md), [hashes](verification/frozen.json), [integriteit/tijdstippen](verification/final-integrity.json).

## Verdicts

Positief = materiële fout die moet worden geblokkeerd. FN is een false accept; FP is een false reject. `unique` telt de 30 bevroren case-ID's eenmaal, zonder herhalingen.

{table}

Een correcte afwijzing bewijst niet dat de bekende fout is herkend. Van de zes originele v4-misses blokkeert baseline 0/6, kandidaat 1 1/6 en kandidaat 2 4/6. Kandidaat 2 wijst zaadbank echter af wegens publiek/verwachtingen over fondsen, zonder de bekende ongefundeerde besluitmotivatie in description te noemen. Onder de volledige diagnosebeoordeling is bij kandidaat 2 in 3/6 v4-originals een reële brongebonden mismatch benoemd: museum, vervoer en bibliotheek. De causale 005 en de interventie/payoffclaim van 010 worden door alle drie prompts geaccepteerd, ook in de herhaling.

Baseline geeft twee holdout-false-rejects: de nieuwsachtige tegenwoordige tijd in de archieftitel wordt als voltooid feit gelezen; ‘Explain the observations’ wordt als een causale verklaring opgevat, en ‘use the selected failure’ als ‘zonder inspectie’. Kandidaat 1 deelt de archief-false-reject. Kandidaat 2 accepteert alle vier goede holdouts. De vooraf bevroren labels zijn behouden. De interpretatie van de archieftitel kan tussen beoordelaars verschillen; zelfs als die ene case uit de beoordeling wordt gelaten, blijven alle recall- en kritieke-foutgates falen. [Scores per call](verification/scores.json), [ruwe antwoorden](runs/).

## Consistentie

Zes moeilijke foutgevallen zijn eenmaal extra getest onder iedere prompt. Overeenstemming met de eerste verdicts: baseline {agree['baseline']}/6, kandidaat 1 {agree['candidate1']}/6, kandidaat 2 {agree['candidate2']}/6. Baseline wisselt bij 010-b van blokkeren naar accepteren en bij museum-b omgekeerd. Kandidaat 1 accepteert museum-b bij herhaling alsnog. Kandidaat 2 blokkeert 010-b pas bij herhaling, met een onzuivere extra diagnose. Overeenstemming omvat herhaalde fouten: de originele v4-005 en v4-010 blijven bij alle prompts geaccepteerd. Eén herhaling per geval schat de werkelijke variabiliteit niet betrouwbaar.

## Diagnosekwaliteit per blocker

{diag}

Alle tuning-, holdout- en repeatblockers zijn opgenomen. Strenge beoordeling omvat de volledige diagnose, niet alleen het geciteerde bronfragment. Voorbeelden: kandidaat 1 verzint een formele museumreview en geeft bij de herhaalde 005 vervangende formuleringen. Kandidaat 2 schrijft op tuning bij 005 ‘Describe the relationship as…’; bij herhaalde 010 vraagt hij om volledige proefcontroles in een korte ideebeschrijving en schrijft hij extra inhoud voor de titel voor. Baseline en kandidaat 1 voegen bij aquarium outcome-eisen voor een voortzettingsbesluit toe. Bronafwezigheid wordt soms tot een bewezen afwezig effect versterkt. Voorbeelden met correcte kern maar extra ongefundeerde voorwaarden falen eveneens.

De tweede museum- en zaadbankblockers van kandidaat 2 dupliceren/overdrijven het kernprobleem. De twee review-b-holdoutblockers beschrijven dezelfde titel vanuit twee perspectieven; beide zijn inhoudelijk geldig, maar niet onafhankelijk. De cijfers zijn dus blockergewogen en geen frequenties van onafhankelijke fouten. [Elke blocker met beoordeling](diagnoses.md), [ruwe blockers](verification/blockers.json), [rubric](scoring-rubric.md). Beoordeling is handmatig en niet geblindeerd; ruimere beoordelingen van grensgevallen veranderen het no-go niet door de gemiste kritieke fouten.

## Gates en onzekerheid

Kandidaat 2 haalt nul false rejects en 4/4 holdoutdetectie, maar slechts 80% recall op alle foutgevallen, mist bekende kritieke fouten en haalt geen 100% brongetrouwe niet-voorschrijvende diagnoses. Kandidaat 1 faalt dezelfde hoofddetectiegates en heeft bovendien een false reject. Geen promotie. [Machineleesbare gates](verification/gates.json).

De 95%-Wilsoninterval voor kandidaat 2 recall is ongeveer {pct(interval[0])}–{pct(interval[1])}; 4/4 holdoutdetectie heeft een ondergrens van ongeveer 51%. Zelfs 15/15 zou geen populatierecall van minstens 95% aantonen. Er zijn tien inhoudelijke bronnen; vijf goede tuningideeën worden met de oorspronkelijke v4-bronwrappers opnieuw gebruikt. Er zijn 25 verschillende ideeobjecten, geen 30 onafhankelijke voorbeelden. De familie-overlap en synthetische holdouts beperken de generalisatie verder. De proef toont concrete fouten en winst in deze set, geen bewezen productiereliability of geïsoleerd causaal prompteffect.

## Calls, verificatie en reproductie

Het vooraf vastgelegde plafond is volledig benut: **108/108 calls** (66 tuning, 24 holdout, 18 repeat), alle geldig. GPT-5-mini resolveerde steeds naar `{s['models'][0]}`. SDK-herpogingen stonden op nul; maximaal drie gelijktijdige calls, geen vervangende calls of generatorcalls. Invoer: **{s['usage']['inputTokenCount']:,} tokens**, waarvan **{s['usage']['cachedInputTokenCount']:,} cached**; uitvoer **{s['usage']['outputTokenCount']:,}**, inclusief **{s['usage']['reasoningTokenCount']:,} reasoning**. Geen prijsraming of factuurcontrole. Iedere call bewaart exacte verzonden JSON, ruwe antwoordtekst, response/model-ID en tokenmetadata. Het budget valt binnen de expliciete opdracht tot betaalde GPT-5-mini-tests. [Call-ledger](verification/call-ledger.jsonl).

Offline: 90 case/promptcombinaties × acht contractresponsen slagen, inclusief inconsistent schema, vier blockers, oude velden, lege velden en duplicaten. Productiebuild en harnasbuild: nul fouten/waarschuwingen. Alle **230 tests** slagen (Application 178, Infrastructure 21, Console 31); Domain heeft geen vindbare tests. De 117 bevroren artefacten en 96 bron/configuratiebestanden onder src behouden hun hashes; tracked en staged diff zijn leeg en HEAD blijft gelijk. [Build](verification/build.txt), [tests](verification/tests.txt), [offline](verification/offline-log.txt), [integriteit](verification/final-integrity.json).

Reproductie gebruikt een verse map op dezelfde diepte, hetzelfde .NET 10-platform en de bestaande lokale modelconfiguratie/credentials (niet opgeslagen). Build het harnas en voer run.py offline, tuning uit; beoordeel alle blockers, leg selectie vast, voer holdout en repeat uit, daarna score.py, review.py, finalize.py en verify.py. Bestaande gereserveerde slots worden overgeslagen; het verbruikte plafond wordt niet gereset. prepare.py weigert opnieuw bevriezen. Het harnas is een ongewijzigde kopie uit v1; alleen experimentele prompts verschillen. [Commandolog](command-log.md), [eindmanifest](verification/final-manifest.json).

## Volgende zinvolle proef

**Een splitsing Source Fidelity Judge / Editorial Intent Judge is de volgende onderbouwde hypothese, nog geen bewezen oplossing.** Kandidaat 2 helpt vooral bij expliciete publieks-/taakverschillen; dezelfde call blijft causale en prioriteitsconclusies missen en verzint soms diagnosevereisten. Test daarom eerst afzonderlijke specialistische calls: bronfeiten/voorwaarden/causaliteit/motieven/status tegenover expliciete publiekstaak/interventievoorwaarden, ieder met diagnose-only output. Combineer met afwijzing als één specialist een geldige materiële blocker geeft, zonder automatisch herschrijven. Meet de unie van false rejects expliciet: twee judges kunnen overblokkeren en kosten meer.

Gebruik deze cases voortaan uitsluitend als regressie/tuningmateriaal en stel vóór nieuwe calls een werkelijk nieuwe, onafhankelijk beoordeelde holdout op. Vergelijk de split tegen de huidige productiejudge en kandidaat 2 met gelijkwaardige totale callbudgetten en herhalingen. Prioriteit zijn de originele v4-005, v4-010, subtiele motieven en goede voorzichtige interpretaties; laat een tweede menselijke beoordelaar de labels en volledige diagnoses blind controleren. Er zijn voor die vervolgproef geen calls uitgevoerd en geen productiearchitectuurwijzigingen gemaakt.
'''
(R/'report.md').write_text(report,encoding='utf-8')
print('Report, blocker review and gates written.')
