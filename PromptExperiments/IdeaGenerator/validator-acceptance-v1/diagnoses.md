# Diagnosebeoordeling

Elke rij beoordeelt één uitgegeven blocker. De kolommen toetsen het aangewezen veld en de kernfout, de getrouwheid van de volledige diagnose aan de bron en de afwezigheid van een voorschrift. Een bronverwijzing kan kloppen terwijl de afgeleide constraint te ver gaat. De beoordeling is handmatig en niet geblindeerd.

| Fase en conditie | Case / blocker | Kern en veld | Brontrouw | Niet-voorschrijvend | Toelichting |
| --- | --- | --- | --- | --- | --- |
| Tuning baseline | 005-b / 1 | Ja | Ja | Ja | De rationale schrijft de verschuiving causaal toe. De diagnose onderbouwt de waarneming en de alternatieve verklaringen met analyse en beperkingen. |
| Tuning baseline | library-b / 1 | Ja | Ja | Ja | De beschrijving vervangt het tijdvak op de bevestiging door de openingsuren. De constraint herhaalt de geldende tijdsgrens. |
| Tuning baseline | seedbank-b / 1 | Ja | Ja | Ja | Het verslag registreert de beslissing zonder het beweerde motief. De aanvullende caveat over het effect van de opslag is brongebonden. |
| Tuning kandidaat | 005-b / 1 | Ja | Ja | Ja | De bron toont een verband binnen de geteste runs en isoleert het causale mechanisme niet. De formulering “association or hypothesis” suggereert een mate van zekerheid, zonder vervangende tekst of nieuwe onderzoeksrichting. |
| Tuning kandidaat | 010-b / 1 | Ja | Nee | Nee | De titel bepaalt Narrative als eerste ingreep voor de komende proef. De diagnose maakt daar eerst een universele claim van en noemt Narrative vervolgens de meest frequente kandidaat voor ingrijpen. De bron noemt Narrative de meest terugkerende plek waar drift zichtbaar werd en laat de keuze afhangen van de onderzochte afwijking. “Present Narrative as” schrijft een specifieke invalshoek voor. |
| Tuning kandidaat | museum-b / 1 | Ja | Ja | Ja | Het veld wijst managers aan, terwijl de bron bezoekers en een concrete route-instructie specificeert. |
| Tuning kandidaat | museum-b / 2 | Gedeeltelijk | Nee | Ja | De cijfers en ontbrekende metingen zijn juist. De diagnose veronderstelt welke uitkomstdata een voortzettingsbesluit zou vereisen, terwijl de bron geen besluitcriteria vastlegt. Dit voorschrijven van bewijsvereisten blijft impliciet en er staat geen voorstel voor onderzoek in de blocker. |
| Tuning kandidaat | library-b / 1 | Ja | Ja | Ja | Het bevestigde tijdvak blijft gelden. De afwijkende beschrijving maakt daar vrije afhaling tijdens openingstijden van. |
| Tuning kandidaat | seedbank-b / 1 | Ja | Ja | Ja | De diagnose scheidt het geregistreerde besluit van het toegeschreven motief en benoemt de beperkingen van de vergelijking. |
| Holdout baseline | ferry-b / 1 | Ja | Ja | Ja | De weekdagen blijven volgens de bron aan pier 1. De diagnose begrenst de wijziging tot het weekend. |
| Holdout kandidaat | ferry-b / 1 | Ja | Ja | Ja | De tijdelijke wijziging geldt van 6 t/m 21 juni voor weekendafvaarten. De constraint behoudt de weekdaguitzondering. |
| Herhaling baseline | 005-b / 1 | Ja | Ja | Ja | De shift is geobserveerd. De diagnose verwijst naar de upstreamspanning en begrenst de causaliteitsclaim. |
| Herhaling kandidaat | 005-b / 1 | Ja | Ja | Ja | De diagnose noemt de associatie sterk binnen de beperkte steekproef en houdt die gescheiden van bewezen oorzaak. De geciteerde beperkingen staan in het rapport. |
