# Bron-naar-Research-matrix Experiment 8

36 vooraf bevroren labels × 9 outputs per bron = 324 beoordelingen. De Work-assistent heeft alle volledige Research-outputs en bronnen gelezen; semantische classificaties zijn handmatige AI-beoordelingen, geen onafhankelijke menselijke labels. Regex selecteert alleen vindbare citaten en beslist geen kwaliteit. Alle geciteerde regels zijn exact uit de artefacten; afwezigheid heeft geen verzonnen outputcitaat. Volledige ankers ook in traceability.json.

## water-r1-A / ledger — kern — correct

Bron [L4](cases/water/source.md):

> The legacy fortnight recorded 46 low-pressure alerts in 1,400 logger-hours; the pilot fortnight recorded 29 in 1,120 logger-hours.

[content.md](calls/water-r1-A-Research/content.md)

- L8: 6. The legacy fortnight recorded 46 low-pressure alerts in 1,400 logger-hours.
- L9: 7. The pilot fortnight recorded 29 low-pressure alerts in 1,120 logger-hours.
- L13: 11. The final field ledger count is 29 alerts.
- L31: 29. The dossier says not to treat the battery status as a repair of the packet fault.
- L39: 37. Three replays of the same stored trace emitted 29, 31, and 28 alerts.
- L42: 40. The replay counts do not replace the final field ledger count of 29.
- L48: 46. The budget does not establish savings.
- L63: - Whether the battery replacement was connected to the delivery check, or repaired the packet fault, is not established (Facts 27–29).
- L68: - The source does not establish savings from the budget or study (Facts 44–46).
- L74: - The legacy fortnight recorded 46 alerts in 1,400 logger-hours (Fact 6); the pilot fortnight recorded 29 alerts in 1,120 logger-hours (Fact 7). The source does not establish that the difference was caused by the sensor upgrade (Fact 20).
- L75: - The presentation reported 25 pilot alerts (Fact 9); the final ledger restored four suppressed duplicates with distinct device-event timestamps (Fact 10) and recorded 29 alerts (Fact 11).
- L76: - All twelve loggers passed the static calibration check (Fact 21). Four loggers failed the overnight packet-delivery check (Facts 22–23); the fault remained open at review (Fact 25). The relationship between the battery replacement and the delivery fault is not established (Facts 27–29).
- L78: - Three replays emitted 29, 31, and 28 alerts (Fact 37); the team did not select a true replay count (Fact 39). These replay counts do not replace the final field ledger (Fact 40).
- L84: - Facts 21–29 are relevant to the calibration result and packet-delivery check.

[projection.md](calls/water-r1-A-Research/projection.md)

- L8: 6. The legacy fortnight recorded 46 low-pressure alerts in 1,400 logger-hours.
- L9: 7. The pilot fortnight recorded 29 low-pressure alerts in 1,120 logger-hours.
- L13: 11. The final field ledger count is 29 alerts.
- L31: 29. The dossier says not to treat the battery status as a repair of the packet fault.
- L39: 37. Three replays of the same stored trace emitted 29, 31, and 28 alerts.
- L42: 40. The replay counts do not replace the final field ledger count of 29.
- L48: 46. The budget does not establish savings.
- L59: - Whether the battery replacement was connected to the delivery check, or repaired the packet fault, is not established (Facts 27–29).
- L64: - The source does not establish savings from the budget or study (Facts 44–46).

## water-r1-A / method — kern — correct

Bron [L5](cases/water/source.md):

> The legacy alert threshold was 1.8 bar for 30 seconds; the pilot used 1.6 bar for 60 seconds.

[content.md](calls/water-r1-A-Research/content.md)

- L15: 13. The legacy alert threshold was 1.8 bar for 30 seconds.
- L16: 14. The pilot alert threshold was 1.6 bar for 60 seconds.
- L17: 15. Old traces were not reprocessed.

[projection.md](calls/water-r1-A-Research/projection.md)

- L15: 13. The legacy alert threshold was 1.8 bar for 30 seconds.
- L16: 14. The pilot alert threshold was 1.6 bar for 60 seconds.
- L17: 15. Old traces were not reprocessed.

## water-r1-A / conditions — belangrijk — correct

Bron [L5](cases/water/source.md):

> A planned night flushing operation occurred during the legacy fortnight only.

[content.md](calls/water-r1-A-Research/content.md)

- L18: 16. A planned night flushing operation occurred during the legacy fortnight only.

[projection.md](calls/water-r1-A-Research/projection.md)

- L18: 16. A planned night flushing operation occurred during the legacy fortnight only.

## water-r1-A / allocation — belangrijk — correct

Bron [L5](cases/water/source.md):

> Branches were selected for ease of access, not randomly; rainfall also differed.

[content.md](calls/water-r1-A-Research/content.md)

- L19: 17. The branches were selected for ease of access, not randomly.
- L20: 18. Rainfall differed between the fortnights.

[projection.md](calls/water-r1-A-Research/projection.md)

- L19: 17. The branches were selected for ease of access, not randomly.
- L20: 18. Rainfall differed between the fortnights.

## water-r1-A / contrast — kern — correct

Bron [L7](cases/water/source.md):

> All twelve loggers passed the static reference-gauge calibration at 2.0 bar. Four nevertheless failed the overnight packet-delivery check, losing 17% of scheduled messages in that check.

[content.md](calls/water-r1-A-Research/content.md)

- L23: 21. All twelve loggers passed the static reference-gauge calibration at 2.0 bar.
- L24: 22. Four loggers failed the overnight packet-delivery check.
- L25: 23. Those four loggers lost 17% of scheduled messages in that check.
- L27: 25. The packet-delivery fault remained open at review.
- L62: - Whether the proposed antenna move would address the packet-delivery fault is not established (Facts 25–26).
- L70: - The source does not state a lesson or establish a general conclusion that calibration alone determines pilot reliability (Facts 21–25, 36, 47).
- L76: - All twelve loggers passed the static calibration check (Fact 21). Four loggers failed the overnight packet-delivery check (Facts 22–23); the fault remained open at review (Fact 25). The relationship between the battery replacement and the delivery fault is not established (Facts 27–29).
- L84: - Facts 21–29 are relevant to the calibration result and packet-delivery check.

[projection.md](calls/water-r1-A-Research/projection.md)

- L23: 21. All twelve loggers passed the static reference-gauge calibration at 2.0 bar.
- L24: 22. Four loggers failed the overnight packet-delivery check.
- L25: 23. Those four loggers lost 17% of scheduled messages in that check.
- L27: 25. The packet-delivery fault remained open at review.
- L58: - Whether the proposed antenna move would address the packet-delivery fault is not established (Facts 25–26).
- L66: - The source does not state a lesson or establish a general conclusion that calibration alone determines pilot reliability (Facts 21–25, 36, 47).

## water-r1-A / open — kern — correct

Bron [L7](cases/water/source.md):

> The delivery fault remained open at review; a proposed antenna move had not been tested.

[content.md](calls/water-r1-A-Research/content.md)

- L27: 25. The packet-delivery fault remained open at review.
- L28: 26. An antenna move was proposed but had not been tested.
- L62: - Whether the proposed antenna move would address the packet-delivery fault is not established (Facts 25–26).
- L76: - All twelve loggers passed the static calibration check (Fact 21). Four loggers failed the overnight packet-delivery check (Facts 22–23); the fault remained open at review (Fact 25). The relationship between the battery replacement and the delivery fault is not established (Facts 27–29).

[projection.md](calls/water-r1-A-Research/projection.md)

- L27: 25. The packet-delivery fault remained open at review.
- L28: 26. An antenna move was proposed but had not been tested.
- L58: - Whether the proposed antenna move would address the packet-delivery fault is not established (Facts 25–26).

## water-r1-A / chronology — belangrijk — correct

Bron [L7](cases/water/source.md):

> A battery replacement was completed on two different devices, but the dossier provides no date linking that work to the delivery check.

[content.md](calls/water-r1-A-Research/content.md)

- L29: 27. A battery replacement was completed on two devices.
- L30: 28. The dossier provides no date linking the battery replacement to the delivery check.
- L31: 29. The dossier says not to treat the battery status as a repair of the packet fault.
- L63: - Whether the battery replacement was connected to the delivery check, or repaired the packet fault, is not established (Facts 27–29).
- L76: - All twelve loggers passed the static calibration check (Fact 21). Four loggers failed the overnight packet-delivery check (Facts 22–23); the fault remained open at review (Fact 25). The relationship between the battery replacement and the delivery fault is not established (Facts 27–29).

[projection.md](calls/water-r1-A-Research/projection.md)

- L29: 27. A battery replacement was completed on two devices.
- L30: 28. The dossier provides no date linking the battery replacement to the delivery check.
- L31: 29. The dossier says not to treat the battery status as a repair of the packet fault.
- L59: - Whether the battery replacement was connected to the delivery check, or repaired the packet fault, is not established (Facts 27–29).

## water-r1-A / term — kern — correct

Bron [L8](cases/water/source.md):

> A bench check recorded three brownouts: transient supply-voltage drops causing device restarts, not a regional power outage.

[content.md](calls/water-r1-A-Research/content.md)

- L32: 30. A bench check recorded three brownouts.
- L33: 31. The dossier defines brownouts as transient supply-voltage drops causing device restarts, not a regional power outage.
- L36: 34. The repeat bench check recorded no brownouts.
- L77: - A bench check recorded three brownouts (Fact 30). A capacitor swap and firmware update were made before a repeat bench check with no brownouts (Facts 32–34). Their separate contributions are not identified, and the result does not certify field uptime (Facts 35–36).
- L85: - Facts 30–36 are relevant to the brownout checks and their limits.

[projection.md](calls/water-r1-A-Research/projection.md)

- L32: 30. A bench check recorded three brownouts.
- L33: 31. The dossier defines brownouts as transient supply-voltage drops causing device restarts, not a regional power outage.
- L36: 34. The repeat bench check recorded no brownouts.

## water-r1-A / attribution — kern — correct

Bron [L8](cases/water/source.md):

> A capacitor swap and firmware update were both made before a repeat bench check with no brownouts. Their separate contributions were not identified; the bench result does not certify field uptime.

[content.md](calls/water-r1-A-Research/content.md)

- L34: 32. A capacitor swap was made before a repeat bench check.
- L35: 33. A firmware update was made before a repeat bench check.
- L37: 35. The separate contributions of the capacitor swap and firmware update were not identified.
- L38: 36. The bench result does not certify field uptime.
- L64: - The separate contributions of the capacitor swap and firmware update to the repeat bench result are not established (Facts 32–36).
- L66: - Overall field uptime or annual reliability is not established; the bench result does not certify field uptime, and no annual reliability benefit was measured (Facts 36, 47).
- L77: - A bench check recorded three brownouts (Fact 30). A capacitor swap and firmware update were made before a repeat bench check with no brownouts (Facts 32–34). Their separate contributions are not identified, and the result does not certify field uptime (Facts 35–36).

[projection.md](calls/water-r1-A-Research/projection.md)

- L34: 32. A capacitor swap was made before a repeat bench check.
- L35: 33. A firmware update was made before a repeat bench check.
- L37: 35. The separate contributions of the capacitor swap and firmware update were not identified.
- L38: 36. The bench result does not certify field uptime.
- L60: - The separate contributions of the capacitor swap and firmware update to the repeat bench result are not established (Facts 32–36).
- L62: - Overall field uptime or annual reliability is not established; the bench result does not certify field uptime, and no annual reliability benefit was measured (Facts 36, 47).

## water-r1-A / variation — kern — correct

Bron [L10](cases/water/source.md):

> Three replays of the same stored trace emitted 29, 31 and 28 alerts; a nondeterministic deduplication tie-breaker was enabled.

[content.md](calls/water-r1-A-Research/content.md)

- L39: 37. Three replays of the same stored trace emitted 29, 31, and 28 alerts.
- L40: 38. A nondeterministic deduplication tie-breaker was enabled.
- L41: 39. The team did not select one replay as the true count.
- L42: 40. The replay counts do not replace the final field ledger count of 29.
- L65: - The source does not establish which replay count, if any, is the true count (Facts 37–40).
- L78: - Three replays emitted 29, 31, and 28 alerts (Fact 37); the team did not select a true replay count (Fact 39). These replay counts do not replace the final field ledger (Fact 40).
- L86: - Facts 37–41 are relevant to replay variability and attribution limits.

[projection.md](calls/water-r1-A-Research/projection.md)

- L39: 37. Three replays of the same stored trace emitted 29, 31, and 28 alerts.
- L40: 38. A nondeterministic deduplication tie-breaker was enabled.
- L41: 39. The team did not select one replay as the true count.
- L42: 40. The replay counts do not replace the final field ledger count of 29.
- L61: - The source does not establish which replay count, if any, is the true count (Facts 37–40).

## water-r1-A / decision — kern — correct

Bron [L11](cases/water/source.md):

> The committee authorized a six-week paired measurement study with unchanged thresholds. It did not authorize network-wide deployment.

[content.md](calls/water-r1-A-Research/content.md)

- L44: 42. The committee authorized a six-week paired measurement study with unchanged thresholds.
- L45: 43. The committee did not authorize network-wide deployment.
- L79: - The committee authorized a six-week paired measurement study with unchanged thresholds (Fact 42) and did not authorize network-wide deployment (Fact 43). The source does not state the committee’s decision criteria or rationale.

[projection.md](calls/water-r1-A-Research/projection.md)

- L44: 42. The committee authorized a six-week paired measurement study with unchanged thresholds.
- L45: 43. The committee did not authorize network-wide deployment.

## water-r1-A / cost — belangrijk — correct

Bron [L11](cases/water/source.md):

> The budget of EUR 18,000 covers logger rental and field visits, excludes staff overtime and does not establish savings.

[content.md](calls/water-r1-A-Research/content.md)

- L46: 44. The EUR 18,000 budget covers logger rental and field visits.
- L47: 45. The budget excludes staff overtime.
- L48: 46. The budget does not establish savings.
- L68: - The source does not establish savings from the budget or study (Facts 44–46).

[projection.md](calls/water-r1-A-Research/projection.md)

- L46: 44. The EUR 18,000 budget covers logger rental and field visits.
- L47: 45. The budget excludes staff overtime.
- L48: 46. The budget does not establish savings.
- L64: - The source does not establish savings from the budget or study (Facts 44–46).

## water-r1-B / ledger — kern — correct

Bron [L4](cases/water/source.md):

> The legacy fortnight recorded 46 low-pressure alerts in 1,400 logger-hours; the pilot fortnight recorded 29 in 1,120 logger-hours.

[content.md](calls/water-r1-B-Research/content.md)

- L3: 2. The legacy fortnight recorded 46 low-pressure alerts in 1,400 logger-hours; the pilot fortnight recorded 29 in 1,120 logger-hours. These are alerts, not confirmed leaks or affected households. The final ledger restored four suppressed duplicates that were distinct device events, correcting a presentation’s 25 to 29 (Recorded outcomes).
- L9: 8. Three replays of the same stored trace emitted 29, 31, and 28 alerts with a nondeterministic deduplication tie-breaker enabled. The team did not designate a replay as the true count; replay counts do not replace the final field ledger’s 29. The source says repeat variability and changed thresholds limit attribution (Repeats and decision).
- L22: - The legacy fortnight was followed by the pilot fortnight; the final ledger records 29 pilot alerts after restoring four distinct events suppressed in the presentation (Fact 2).

[projection.md](calls/water-r1-B-Research/projection.md)

- L4: 2. The legacy fortnight recorded 46 low-pressure alerts in 1,400 logger-hours; the pilot fortnight recorded 29 in 1,120 logger-hours. These are alerts, not confirmed leaks or affected households. The final ledger restored four suppressed duplicates that were distinct device events, correcting a presentation’s 25 to 29 (Recorded outcomes).
- L10: 8. Three replays of the same stored trace emitted 29, 31, and 28 alerts with a nondeterministic deduplication tie-breaker enabled. The team did not designate a replay as the true count; replay counts do not replace the final field ledger’s 29. The source says repeat variability and changed thresholds limit attribution (Repeats and decision).

## water-r1-B / method — kern — correct

Bron [L5](cases/water/source.md):

> The legacy alert threshold was 1.8 bar for 30 seconds; the pilot used 1.6 bar for 60 seconds.

[content.md](calls/water-r1-B-Research/content.md)

- L4: 3. The legacy threshold was 1.8 bar for 30 seconds and the pilot threshold 1.6 bar for 60 seconds; old traces were not reprocessed. Night flushing occurred only during the legacy fortnight. Branches were chosen for access, not randomly, rainfall differed, and there was no unaffected comparison branch. The report gives no rate-adjusted treatment effect and says the lower alert count cannot isolate the sensor upgrade’s effect (Recorded outcomes).

[projection.md](calls/water-r1-B-Research/projection.md)

- L5: 3. The legacy threshold was 1.8 bar for 30 seconds and the pilot threshold 1.6 bar for 60 seconds; old traces were not reprocessed. Night flushing occurred only during the legacy fortnight. Branches were chosen for access, not randomly, rainfall differed, and there was no unaffected comparison branch. The report gives no rate-adjusted treatment effect and says the lower alert count cannot isolate the sensor upgrade’s effect (Recorded outcomes).

## water-r1-B / conditions — belangrijk — correct

Bron [L5](cases/water/source.md):

> A planned night flushing operation occurred during the legacy fortnight only.

[content.md](calls/water-r1-B-Research/content.md)

- L4: 3. The legacy threshold was 1.8 bar for 30 seconds and the pilot threshold 1.6 bar for 60 seconds; old traces were not reprocessed. Night flushing occurred only during the legacy fortnight. Branches were chosen for access, not randomly, rainfall differed, and there was no unaffected comparison branch. The report gives no rate-adjusted treatment effect and says the lower alert count cannot isolate the sensor upgrade’s effect (Recorded outcomes).

[projection.md](calls/water-r1-B-Research/projection.md)

- L5: 3. The legacy threshold was 1.8 bar for 30 seconds and the pilot threshold 1.6 bar for 60 seconds; old traces were not reprocessed. Night flushing occurred only during the legacy fortnight. Branches were chosen for access, not randomly, rainfall differed, and there was no unaffected comparison branch. The report gives no rate-adjusted treatment effect and says the lower alert count cannot isolate the sensor upgrade’s effect (Recorded outcomes).

## water-r1-B / allocation — belangrijk — correct

Bron [L5](cases/water/source.md):

> Branches were selected for ease of access, not randomly; rainfall also differed.

[content.md](calls/water-r1-B-Research/content.md)

- L4: 3. The legacy threshold was 1.8 bar for 30 seconds and the pilot threshold 1.6 bar for 60 seconds; old traces were not reprocessed. Night flushing occurred only during the legacy fortnight. Branches were chosen for access, not randomly, rainfall differed, and there was no unaffected comparison branch. The report gives no rate-adjusted treatment effect and says the lower alert count cannot isolate the sensor upgrade’s effect (Recorded outcomes).

[projection.md](calls/water-r1-B-Research/projection.md)

- L5: 3. The legacy threshold was 1.8 bar for 30 seconds and the pilot threshold 1.6 bar for 60 seconds; old traces were not reprocessed. Night flushing occurred only during the legacy fortnight. Branches were chosen for access, not randomly, rainfall differed, and there was no unaffected comparison branch. The report gives no rate-adjusted treatment effect and says the lower alert count cannot isolate the sensor upgrade’s effect (Recorded outcomes).

## water-r1-B / contrast — kern — correct

Bron [L7](cases/water/source.md):

> All twelve loggers passed the static reference-gauge calibration at 2.0 bar. Four nevertheless failed the overnight packet-delivery check, losing 17% of scheduled messages in that check.

[content.md](calls/water-r1-B-Research/content.md)

- L5: 4. All 12 loggers passed static reference-gauge calibration at 2.0 bar (Checks and defects).
- L6: 5. Four loggers failed the overnight packet-delivery check, losing 17% of scheduled messages in that check. The source does not show that the pressure readings received were inaccurate. The delivery fault remained open; a proposed antenna move had not been tested (Checks and defects).
- L7: 6. Battery replacement was completed on two different devices, but no date links that work to the delivery check. The source does not identify it as a repair of the packet-delivery fault (Checks and defects).
- L13: - The successful static calibration establishes one check result, not end-to-end telemetry reliability; the packet-delivery failures are a separate reported defect (Facts 4–5).
- L18: - It does not establish whether the proposed antenna move would resolve packet loss, whether packet delivery improved after the check, or whether the battery replacements affected that fault (Facts 5–6).
- L26: - Core support for the distinction between calibration and operational reliability: Facts 4–7.

[projection.md](calls/water-r1-B-Research/projection.md)

- L6: 4. All 12 loggers passed static reference-gauge calibration at 2.0 bar (Checks and defects).
- L7: 5. Four loggers failed the overnight packet-delivery check, losing 17% of scheduled messages in that check. The source does not show that the pressure readings received were inaccurate. The delivery fault remained open; a proposed antenna move had not been tested (Checks and defects).
- L8: 6. Battery replacement was completed on two different devices, but no date links that work to the delivery check. The source does not identify it as a repair of the packet-delivery fault (Checks and defects).
- L18: - It does not establish whether the proposed antenna move would resolve packet loss, whether packet delivery improved after the check, or whether the battery replacements affected that fault (Facts 5–6).

## water-r1-B / open — kern — correct

Bron [L7](cases/water/source.md):

> The delivery fault remained open at review; a proposed antenna move had not been tested.

[content.md](calls/water-r1-B-Research/content.md)

- L6: 5. Four loggers failed the overnight packet-delivery check, losing 17% of scheduled messages in that check. The source does not show that the pressure readings received were inaccurate. The delivery fault remained open; a proposed antenna move had not been tested (Checks and defects).
- L18: - It does not establish whether the proposed antenna move would resolve packet loss, whether packet delivery improved after the check, or whether the battery replacements affected that fault (Facts 5–6).

[projection.md](calls/water-r1-B-Research/projection.md)

- L7: 5. Four loggers failed the overnight packet-delivery check, losing 17% of scheduled messages in that check. The source does not show that the pressure readings received were inaccurate. The delivery fault remained open; a proposed antenna move had not been tested (Checks and defects).
- L18: - It does not establish whether the proposed antenna move would resolve packet loss, whether packet delivery improved after the check, or whether the battery replacements affected that fault (Facts 5–6).

## water-r1-B / chronology — belangrijk — correct

Bron [L7](cases/water/source.md):

> A battery replacement was completed on two different devices, but the dossier provides no date linking that work to the delivery check.

[content.md](calls/water-r1-B-Research/content.md)

- L7: 6. Battery replacement was completed on two different devices, but no date links that work to the delivery check. The source does not identify it as a repair of the packet-delivery fault (Checks and defects).
- L18: - It does not establish whether the proposed antenna move would resolve packet loss, whether packet delivery improved after the check, or whether the battery replacements affected that fault (Facts 5–6).

[projection.md](calls/water-r1-B-Research/projection.md)

- L8: 6. Battery replacement was completed on two different devices, but no date links that work to the delivery check. The source does not identify it as a repair of the packet-delivery fault (Checks and defects).
- L18: - It does not establish whether the proposed antenna move would resolve packet loss, whether packet delivery improved after the check, or whether the battery replacements affected that fault (Facts 5–6).

## water-r1-B / term — kern — correct

Bron [L8](cases/water/source.md):

> A bench check recorded three brownouts: transient supply-voltage drops causing device restarts, not a regional power outage.

[content.md](calls/water-r1-B-Research/content.md)

- L8: 7. A bench check recorded three brownouts, defined as transient supply-voltage drops that caused device restarts—not a regional power outage. A capacitor swap and firmware update preceded a repeat bench check with no brownouts; their separate contributions were not identified, and that bench result does not certify field uptime (Checks and defects).
- L23: - A bench check recorded three brownouts; the capacitor swap and firmware update were made before a repeat bench check with no brownouts (Fact 7).

[projection.md](calls/water-r1-B-Research/projection.md)

- L9: 7. A bench check recorded three brownouts, defined as transient supply-voltage drops that caused device restarts—not a regional power outage. A capacitor swap and firmware update preceded a repeat bench check with no brownouts; their separate contributions were not identified, and that bench result does not certify field uptime (Checks and defects).

## water-r1-B / attribution — kern — correct

Bron [L8](cases/water/source.md):

> A capacitor swap and firmware update were both made before a repeat bench check with no brownouts. Their separate contributions were not identified; the bench result does not certify field uptime.

[content.md](calls/water-r1-B-Research/content.md)

- L8: 7. A bench check recorded three brownouts, defined as transient supply-voltage drops that caused device restarts—not a regional power outage. A capacitor swap and firmware update preceded a repeat bench check with no brownouts; their separate contributions were not identified, and that bench result does not certify field uptime (Checks and defects).
- L19: - It does not establish the separate effects of the capacitor swap and firmware update, or field uptime after the bench result (Fact 7).
- L23: - A bench check recorded three brownouts; the capacitor swap and firmware update were made before a repeat bench check with no brownouts (Fact 7).

[projection.md](calls/water-r1-B-Research/projection.md)

- L9: 7. A bench check recorded three brownouts, defined as transient supply-voltage drops that caused device restarts—not a regional power outage. A capacitor swap and firmware update preceded a repeat bench check with no brownouts; their separate contributions were not identified, and that bench result does not certify field uptime (Checks and defects).
- L19: - It does not establish the separate effects of the capacitor swap and firmware update, or field uptime after the bench result (Fact 7).

## water-r1-B / variation — kern — correct

Bron [L10](cases/water/source.md):

> Three replays of the same stored trace emitted 29, 31 and 28 alerts; a nondeterministic deduplication tie-breaker was enabled.

[content.md](calls/water-r1-B-Research/content.md)

- L9: 8. Three replays of the same stored trace emitted 29, 31, and 28 alerts with a nondeterministic deduplication tie-breaker enabled. The team did not designate a replay as the true count; replay counts do not replace the final field ledger’s 29. The source says repeat variability and changed thresholds limit attribution (Repeats and decision).
- L17: - It does not establish which replay count, if any, represents a definitive count, or quantify how much variability is attributable to the deduplication tie-breaker versus other factors (Fact 8).

[projection.md](calls/water-r1-B-Research/projection.md)

- L10: 8. Three replays of the same stored trace emitted 29, 31, and 28 alerts with a nondeterministic deduplication tie-breaker enabled. The team did not designate a replay as the true count; replay counts do not replace the final field ledger’s 29. The source says repeat variability and changed thresholds limit attribution (Repeats and decision).
- L17: - It does not establish which replay count, if any, represents a definitive count, or quantify how much variability is attributable to the deduplication tie-breaker versus other factors (Fact 8).

## water-r1-B / decision — kern — correct

Bron [L11](cases/water/source.md):

> The committee authorized a six-week paired measurement study with unchanged thresholds. It did not authorize network-wide deployment.

[content.md](calls/water-r1-B-Research/content.md)

- L10: 9. The committee authorized a six-week paired measurement study with unchanged thresholds, but did not authorize network-wide deployment (Repeats and decision).
- L20: - It does not establish network-wide performance, savings, safety, leak-prevention, annual reliability benefits, or technician competence (Facts 9–10).
- L24: - The committee then authorized a six-week paired measurement study with unchanged thresholds, not network-wide deployment (Fact 9).

[projection.md](calls/water-r1-B-Research/projection.md)

- L11: 9. The committee authorized a six-week paired measurement study with unchanged thresholds, but did not authorize network-wide deployment (Repeats and decision).
- L20: - It does not establish network-wide performance, savings, safety, leak-prevention, annual reliability benefits, or technician competence (Facts 9–10).

## water-r1-B / cost — belangrijk — correct

Bron [L11](cases/water/source.md):

> The budget of EUR 18,000 covers logger rental and field visits, excludes staff overtime and does not establish savings.

[content.md](calls/water-r1-B-Research/content.md)

- L11: 10. The EUR 18,000 budget covers logger rental and field visits, excludes staff overtime, and does not establish savings. No safety, leak-prevention, or annual reliability benefit was measured. Training attendance was 10 of 12 technicians, but was not a competence test (Repeats and decision).
- L20: - It does not establish network-wide performance, savings, safety, leak-prevention, annual reliability benefits, or technician competence (Facts 9–10).

[projection.md](calls/water-r1-B-Research/projection.md)

- L12: 10. The EUR 18,000 budget covers logger rental and field visits, excludes staff overtime, and does not establish savings. No safety, leak-prevention, or annual reliability benefit was measured. Training attendance was 10 of 12 technicians, but was not a competence test (Repeats and decision).
- L20: - It does not establish network-wide performance, savings, safety, leak-prevention, annual reliability benefits, or technician competence (Facts 9–10).

## water-r1-B2 / ledger — kern — correct

Bron [L4](cases/water/source.md):

> The legacy fortnight recorded 46 low-pressure alerts in 1,400 logger-hours; the pilot fortnight recorded 29 in 1,120 logger-hours.

[content.md](calls/water-r1-B2-Research/content.md)

- L3: 2. The legacy fortnight recorded 46 low-pressure alerts in 1,400 logger-hours; the pilot fortnight recorded 29 in 1,120 logger-hours. These are alerts, not confirmed leaks or affected households. The final ledger uses 29: it restored four suppressed alerts that were distinct device events, correcting a presentation’s 25. (Recorded outcomes)
- L9: 8. Three replays of the same stored trace emitted 29, 31 and 28 alerts; a nondeterministic deduplication tie-breaker was enabled. The team did not designate a replay as the true count. These replay counts do not replace the final field ledger’s 29. The source says repeat variability and changed thresholds limit attribution. (Repeats and decision)

[projection.md](calls/water-r1-B2-Research/projection.md)

- L4: 2. The legacy fortnight recorded 46 low-pressure alerts in 1,400 logger-hours; the pilot fortnight recorded 29 in 1,120 logger-hours. These are alerts, not confirmed leaks or affected households. The final ledger uses 29: it restored four suppressed alerts that were distinct device events, correcting a presentation’s 25. (Recorded outcomes)
- L10: 8. Three replays of the same stored trace emitted 29, 31 and 28 alerts; a nondeterministic deduplication tie-breaker was enabled. The team did not designate a replay as the true count. These replay counts do not replace the final field ledger’s 29. The source says repeat variability and changed thresholds limit attribution. (Repeats and decision)

## water-r1-B2 / method — kern — correct

Bron [L5](cases/water/source.md):

> The legacy alert threshold was 1.8 bar for 30 seconds; the pilot used 1.6 bar for 60 seconds.

[content.md](calls/water-r1-B2-Research/content.md)

- L4: 3. The legacy threshold was 1.8 bar for 30 seconds; the pilot threshold was 1.6 bar for 60 seconds, and old traces were not reprocessed. The report gives no rate-adjusted treatment-effect estimate. (Recorded outcomes)
- L24: - The legacy and pilot fortnights had the recorded alert counts and different thresholds; the source does not report reprocessing old traces. (Facts 2–3)

[projection.md](calls/water-r1-B2-Research/projection.md)

- L5: 3. The legacy threshold was 1.8 bar for 30 seconds; the pilot threshold was 1.6 bar for 60 seconds, and old traces were not reprocessed. The report gives no rate-adjusted treatment-effect estimate. (Recorded outcomes)

## water-r1-B2 / conditions — belangrijk — correct

Bron [L5](cases/water/source.md):

> A planned night flushing operation occurred during the legacy fortnight only.

[content.md](calls/water-r1-B2-Research/content.md)

- L5: 4. A planned night flushing operation occurred only during the legacy fortnight. Branches were selected for ease of access, not randomly; rainfall differed, and no unaffected comparison branch was instrumented. The source says the lower alert count cannot isolate the sensor upgrade’s effect. (Recorded outcomes)

[projection.md](calls/water-r1-B2-Research/projection.md)

- L6: 4. A planned night flushing operation occurred only during the legacy fortnight. Branches were selected for ease of access, not randomly; rainfall differed, and no unaffected comparison branch was instrumented. The source says the lower alert count cannot isolate the sensor upgrade’s effect. (Recorded outcomes)

## water-r1-B2 / allocation — belangrijk — correct

Bron [L5](cases/water/source.md):

> Branches were selected for ease of access, not randomly; rainfall also differed.

[content.md](calls/water-r1-B2-Research/content.md)

- L5: 4. A planned night flushing operation occurred only during the legacy fortnight. Branches were selected for ease of access, not randomly; rainfall differed, and no unaffected comparison branch was instrumented. The source says the lower alert count cannot isolate the sensor upgrade’s effect. (Recorded outcomes)

[projection.md](calls/water-r1-B2-Research/projection.md)

- L6: 4. A planned night flushing operation occurred only during the legacy fortnight. Branches were selected for ease of access, not randomly; rainfall differed, and no unaffected comparison branch was instrumented. The source says the lower alert count cannot isolate the sensor upgrade’s effect. (Recorded outcomes)

## water-r1-B2 / contrast — kern — correct

Bron [L7](cases/water/source.md):

> All twelve loggers passed the static reference-gauge calibration at 2.0 bar. Four nevertheless failed the overnight packet-delivery check, losing 17% of scheduled messages in that check.

[content.md](calls/water-r1-B2-Research/content.md)

- L6: 5. All twelve loggers passed static reference-gauge calibration at 2.0 bar. Separately, four failed the overnight packet-delivery check, losing 17% of scheduled messages in that check. The delivered pressure readings were not shown to be inaccurate. (Checks and defects)
- L7: 6. The packet-delivery fault remained open at review; a proposed antenna move had not been tested. Battery replacement was completed on two different devices, but no date links it to the delivery check; the source says not to treat that battery status as repair of the packet fault. (Checks and defects)
- L14: 1. The successful static calibration establishes that specific check’s result, not an overall finding of reliable operation; the separate delivery failure and unresolved fault remain relevant. (Facts 5–6)

[projection.md](calls/water-r1-B2-Research/projection.md)

- L7: 5. All twelve loggers passed static reference-gauge calibration at 2.0 bar. Separately, four failed the overnight packet-delivery check, losing 17% of scheduled messages in that check. The delivered pressure readings were not shown to be inaccurate. (Checks and defects)
- L8: 6. The packet-delivery fault remained open at review; a proposed antenna move had not been tested. Battery replacement was completed on two different devices, but no date links it to the delivery check; the source says not to treat that battery status as repair of the packet fault. (Checks and defects)

## water-r1-B2 / open — kern — correct

Bron [L7](cases/water/source.md):

> The delivery fault remained open at review; a proposed antenna move had not been tested.

[content.md](calls/water-r1-B2-Research/content.md)

- L7: 6. The packet-delivery fault remained open at review; a proposed antenna move had not been tested. Battery replacement was completed on two different devices, but no date links it to the delivery check; the source says not to treat that battery status as repair of the packet fault. (Checks and defects)
- L19: - It does not establish that the proposed antenna move would resolve packet loss, or that battery replacement repaired the fault. The date connection is also unspecified. (Fact 6)
- L26: - At review, the packet fault remained open and the proposed antenna move was untested. The committee then authorized a six-week paired study with unchanged thresholds, not network-wide deployment. (Facts 6, 9)

[projection.md](calls/water-r1-B2-Research/projection.md)

- L8: 6. The packet-delivery fault remained open at review; a proposed antenna move had not been tested. Battery replacement was completed on two different devices, but no date links it to the delivery check; the source says not to treat that battery status as repair of the packet fault. (Checks and defects)
- L17: - It does not establish that the proposed antenna move would resolve packet loss, or that battery replacement repaired the fault. The date connection is also unspecified. (Fact 6)

## water-r1-B2 / chronology — belangrijk — correct

Bron [L7](cases/water/source.md):

> A battery replacement was completed on two different devices, but the dossier provides no date linking that work to the delivery check.

[content.md](calls/water-r1-B2-Research/content.md)

- L7: 6. The packet-delivery fault remained open at review; a proposed antenna move had not been tested. Battery replacement was completed on two different devices, but no date links it to the delivery check; the source says not to treat that battery status as repair of the packet fault. (Checks and defects)
- L19: - It does not establish that the proposed antenna move would resolve packet loss, or that battery replacement repaired the fault. The date connection is also unspecified. (Fact 6)

[projection.md](calls/water-r1-B2-Research/projection.md)

- L8: 6. The packet-delivery fault remained open at review; a proposed antenna move had not been tested. Battery replacement was completed on two different devices, but no date links it to the delivery check; the source says not to treat that battery status as repair of the packet fault. (Checks and defects)
- L17: - It does not establish that the proposed antenna move would resolve packet loss, or that battery replacement repaired the fault. The date connection is also unspecified. (Fact 6)

## water-r1-B2 / term — kern — correct

Bron [L8](cases/water/source.md):

> A bench check recorded three brownouts: transient supply-voltage drops causing device restarts, not a regional power outage.

[content.md](calls/water-r1-B2-Research/content.md)

- L8: 7. A bench check recorded three brownouts, defined here as transient supply-voltage drops causing device restarts, not a regional power outage. A capacitor swap and firmware update were both made before a repeat bench check recorded no brownouts. Their separate contributions were not identified, and the bench result does not certify field uptime. (Checks and defects)
- L20: - The separate effects of the capacitor swap and firmware update are unknown; absence of brownouts on the repeat bench check does not establish field uptime. (Fact 7)
- L25: - On the bench, the capacitor swap and firmware update both preceded the repeat check that recorded no brownouts; their separate contributions were not identified. (Fact 7)

[projection.md](calls/water-r1-B2-Research/projection.md)

- L9: 7. A bench check recorded three brownouts, defined here as transient supply-voltage drops causing device restarts, not a regional power outage. A capacitor swap and firmware update were both made before a repeat bench check recorded no brownouts. Their separate contributions were not identified, and the bench result does not certify field uptime. (Checks and defects)
- L18: - The separate effects of the capacitor swap and firmware update are unknown; absence of brownouts on the repeat bench check does not establish field uptime. (Fact 7)

## water-r1-B2 / attribution — kern — correct

Bron [L8](cases/water/source.md):

> A capacitor swap and firmware update were both made before a repeat bench check with no brownouts. Their separate contributions were not identified; the bench result does not certify field uptime.

[content.md](calls/water-r1-B2-Research/content.md)

- L8: 7. A bench check recorded three brownouts, defined here as transient supply-voltage drops causing device restarts, not a regional power outage. A capacitor swap and firmware update were both made before a repeat bench check recorded no brownouts. Their separate contributions were not identified, and the bench result does not certify field uptime. (Checks and defects)
- L20: - The separate effects of the capacitor swap and firmware update are unknown; absence of brownouts on the repeat bench check does not establish field uptime. (Fact 7)
- L25: - On the bench, the capacitor swap and firmware update both preceded the repeat check that recorded no brownouts; their separate contributions were not identified. (Fact 7)

[projection.md](calls/water-r1-B2-Research/projection.md)

- L9: 7. A bench check recorded three brownouts, defined here as transient supply-voltage drops causing device restarts, not a regional power outage. A capacitor swap and firmware update were both made before a repeat bench check recorded no brownouts. Their separate contributions were not identified, and the bench result does not certify field uptime. (Checks and defects)
- L18: - The separate effects of the capacitor swap and firmware update are unknown; absence of brownouts on the repeat bench check does not establish field uptime. (Fact 7)

## water-r1-B2 / variation — kern — correct

Bron [L10](cases/water/source.md):

> Three replays of the same stored trace emitted 29, 31 and 28 alerts; a nondeterministic deduplication tie-breaker was enabled.

[content.md](calls/water-r1-B2-Research/content.md)

- L9: 8. Three replays of the same stored trace emitted 29, 31 and 28 alerts; a nondeterministic deduplication tie-breaker was enabled. The team did not designate a replay as the true count. These replay counts do not replace the final field ledger’s 29. The source says repeat variability and changed thresholds limit attribution. (Repeats and decision)

[projection.md](calls/water-r1-B2-Research/projection.md)

- L10: 8. Three replays of the same stored trace emitted 29, 31 and 28 alerts; a nondeterministic deduplication tie-breaker was enabled. The team did not designate a replay as the true count. These replay counts do not replace the final field ledger’s 29. The source says repeat variability and changed thresholds limit attribution. (Repeats and decision)

## water-r1-B2 / decision — kern — correct

Bron [L11](cases/water/source.md):

> The committee authorized a six-week paired measurement study with unchanged thresholds. It did not authorize network-wide deployment.

[content.md](calls/water-r1-B2-Research/content.md)

- L10: 9. The committee authorized a six-week paired measurement study with unchanged thresholds; it did not authorize network-wide deployment. (Repeats and decision)
- L21: - No network-wide performance, safety, leak-prevention, annual reliability, savings, or technician competence outcome is established. (Facts 1, 9–10)
- L26: - At review, the packet fault remained open and the proposed antenna move was untested. The committee then authorized a six-week paired study with unchanged thresholds, not network-wide deployment. (Facts 6, 9)

[projection.md](calls/water-r1-B2-Research/projection.md)

- L11: 9. The committee authorized a six-week paired measurement study with unchanged thresholds; it did not authorize network-wide deployment. (Repeats and decision)
- L19: - No network-wide performance, safety, leak-prevention, annual reliability, savings, or technician competence outcome is established. (Facts 1, 9–10)

## water-r1-B2 / cost — belangrijk — correct

Bron [L11](cases/water/source.md):

> The budget of EUR 18,000 covers logger rental and field visits, excludes staff overtime and does not establish savings.

[content.md](calls/water-r1-B2-Research/content.md)

- L11: 10. The EUR 18,000 budget covers logger rental and field visits, excludes staff overtime, and does not establish savings. No safety, leak-prevention or annual reliability benefit was measured. Training attendance was 10 of 12 technicians, but attendance was not a competence test. (Repeats and decision)
- L21: - No network-wide performance, safety, leak-prevention, annual reliability, savings, or technician competence outcome is established. (Facts 1, 9–10)

[projection.md](calls/water-r1-B2-Research/projection.md)

- L12: 10. The EUR 18,000 budget covers logger rental and field visits, excludes staff overtime, and does not establish savings. No safety, leak-prevention or annual reliability benefit was measured. Training attendance was 10 of 12 technicians, but attendance was not a competence test. (Repeats and decision)
- L19: - No network-wide performance, safety, leak-prevention, annual reliability, savings, or technician competence outcome is established. (Facts 1, 9–10)

## language-r1-B / sample — kern — correct

Bron [L4](cases/language/source.md):

> Reviewers sampled 80 of 640 message strings: 40 Spanish and 40 Polish.

[content.md](calls/language-r1-B-Research/content.md)

- L3: 2. Reviewers sampled 80 of 640 message strings: 40 Spanish and 40 Polish. All 80 retained approved glossary entitlement terms. (Sample and linguistic checks)
- L5: 4. The source treats terminology conformity and semantic correctness as separate checks, and says neither result certifies all 640 strings. (Sample and linguistic checks)
- L11: 10. “Case folding” normalised letter case; it did not merge benefit cases or applications. On the same 80 strings, three runs of a sampling-based checker flagged six, five and six semantic issues. Human reference review identified six deadline reversals; the checker version stayed fixed while sample order varied. (Processing and reproducibility)
- L21: - The sampled strings do not establish the semantic correctness of all 640 strings; most sampled strings did not receive the second semantic review. (Facts 2–4)

[projection.md](calls/language-r1-B-Research/projection.md)

- L4: 2. Reviewers sampled 80 of 640 message strings: 40 Spanish and 40 Polish. All 80 retained approved glossary entitlement terms. (Sample and linguistic checks)
- L6: 4. The source treats terminology conformity and semantic correctness as separate checks, and says neither result certifies all 640 strings. (Sample and linguistic checks)
- L12: 10. “Case folding” normalised letter case; it did not merge benefit cases or applications. On the same 80 strings, three runs of a sampling-based checker flagged six, five and six semantic issues. Human reference review identified six deadline reversals; the checker version stayed fixed while sample order varied. (Processing and reproducibility)
- L21: - The sampled strings do not establish the semantic correctness of all 640 strings; most sampled strings did not receive the second semantic review. (Facts 2–4)

## language-r1-B / contrast — kern — correct

Bron [L4](cases/language/source.md):

> All 80 retained the glossary's approved entitlement terms. Six Spanish strings nevertheless reversed the meaning of the deadline exception. The Polish sample had no deadline reversal.

[content.md](calls/language-r1-B-Research/content.md)

- L3: 2. Reviewers sampled 80 of 640 message strings: 40 Spanish and 40 Polish. All 80 retained approved glossary entitlement terms. (Sample and linguistic checks)
- L4: 3. Six sampled Spanish strings reversed the deadline-exception meaning; the Polish sample had no such reversal. A second reviewer agreed on the six reversals, while the other strings did not receive that second semantic review. (Sample and linguistic checks)
- L5: 4. The source treats terminology conformity and semantic correctness as separate checks, and says neither result certifies all 640 strings. (Sample and linguistic checks)
- L11: 10. “Case folding” normalised letter case; it did not merge benefit cases or applications. On the same 80 strings, three runs of a sampling-based checker flagged six, five and six semantic issues. Human reference review identified six deadline reversals; the checker version stayed fixed while sample order varied. (Processing and reproducibility)
- L18: - Terminology conformity, semantic correctness, and task access are distinct dimensions; a positive result on one does not establish the others. (Facts 2–6)
- L24: - The checker’s run variation does not establish its reliability or a stable issue count; the source reports one human reference review identifying six reversals. (Facts 10–11)

[projection.md](calls/language-r1-B-Research/projection.md)

- L4: 2. Reviewers sampled 80 of 640 message strings: 40 Spanish and 40 Polish. All 80 retained approved glossary entitlement terms. (Sample and linguistic checks)
- L5: 3. Six sampled Spanish strings reversed the deadline-exception meaning; the Polish sample had no such reversal. A second reviewer agreed on the six reversals, while the other strings did not receive that second semantic review. (Sample and linguistic checks)
- L6: 4. The source treats terminology conformity and semantic correctness as separate checks, and says neither result certifies all 640 strings. (Sample and linguistic checks)
- L12: 10. “Case folding” normalised letter case; it did not merge benefit cases or applications. On the same 80 strings, three runs of a sampling-based checker flagged six, five and six semantic issues. Human reference review identified six deadline reversals; the checker version stayed fixed while sample order varied. (Processing and reproducibility)
- L24: - The checker’s run variation does not establish its reliability or a stable issue count; the source reports one human reference review identifying six reversals. (Facts 10–11)

## language-r1-B / review — belangrijk — correct

Bron [L4](cases/language/source.md):

> A second reviewer checked the six reversals and agreed. The remaining strings did not receive that second semantic review.

[content.md](calls/language-r1-B-Research/content.md)

- L4: 3. Six sampled Spanish strings reversed the deadline-exception meaning; the Polish sample had no such reversal. A second reviewer agreed on the six reversals, while the other strings did not receive that second semantic review. (Sample and linguistic checks)
- L21: - The sampled strings do not establish the semantic correctness of all 640 strings; most sampled strings did not receive the second semantic review. (Facts 2–4)

[projection.md](calls/language-r1-B-Research/projection.md)

- L5: 3. Six sampled Spanish strings reversed the deadline-exception meaning; the Polish sample had no such reversal. A second reviewer agreed on the six reversals, while the other strings did not receive that second semantic review. (Sample and linguistic checks)
- L21: - The sampled strings do not establish the semantic correctness of all 640 strings; most sampled strings did not receive the second semantic review. (Facts 2–4)

## language-r1-B / access — kern — correct

Bron [L6](cases/language/source.md):

> An automated colour-contrast scan passed the three tested page templates. Keyboard-only testing could not activate the document-upload button on the second template.

[content.md](calls/language-r1-B-Research/content.md)

- L6: 5. An automated colour-contrast scan passed on three tested page templates. Keyboard-only testing could not activate the upload button on template two, and the defect remained open. (Access and task results)
- L7: 6. On template two, screen-reader testing announced the upload label correctly but did not test keyboard activation; the source explicitly says a correct label is not evidence that a keyboard user can upload a document. (Access and task results)
- L8: 7. Management approved a keyboard-fix proposal, but no deployed fix is recorded. (Access and task results)
- L14: 13. The coordinator authorised a corrected-form usability study conditional on a verified upload fix and withheld general release. The study budget includes translation fees but excludes staff time; no cost saving is demonstrated. (Status and decision boundary)
- L22: - The scan and tests do not establish general accessibility: only three templates received the contrast scan, the upload keyboard defect was open, and no general accessibility certification was commissioned. A deployed keyboard fix is not documented. (Facts 1, 5–7)
- L26: - The source does not demonstrate population-level satisfaction, general inclusion, cost savings, or outcomes from the proposed corrected-form study. The study remains conditional on a verified upload fix; no general release was authorised. (Facts 13–15)
- L29: - At review, the upload defect remained open; management had approved a proposed fix, but no deployment was recorded. The coordinator authorised a usability study conditional on a verified fix and withheld general release. (Facts 5, 7, 13)

[projection.md](calls/language-r1-B-Research/projection.md)

- L7: 5. An automated colour-contrast scan passed on three tested page templates. Keyboard-only testing could not activate the upload button on template two, and the defect remained open. (Access and task results)
- L8: 6. On template two, screen-reader testing announced the upload label correctly but did not test keyboard activation; the source explicitly says a correct label is not evidence that a keyboard user can upload a document. (Access and task results)
- L9: 7. Management approved a keyboard-fix proposal, but no deployed fix is recorded. (Access and task results)
- L15: 13. The coordinator authorised a corrected-form usability study conditional on a verified upload fix and withheld general release. The study budget includes translation fees but excludes staff time; no cost saving is demonstrated. (Status and decision boundary)
- L22: - The scan and tests do not establish general accessibility: only three templates received the contrast scan, the upload keyboard defect was open, and no general accessibility certification was commissioned. A deployed keyboard fix is not documented. (Facts 1, 5–7)
- L26: - The source does not demonstrate population-level satisfaction, general inclusion, cost savings, or outcomes from the proposed corrected-form study. The study remains conditional on a verified upload fix; no general release was authorised. (Facts 13–15)

## language-r1-B / open — kern — correct

Bron [L6](cases/language/source.md):

> The upload defect was still open. Screen-reader testing announced the upload label correctly on that template, but did not test keyboard activation.

[content.md](calls/language-r1-B-Research/content.md)

- L6: 5. An automated colour-contrast scan passed on three tested page templates. Keyboard-only testing could not activate the upload button on template two, and the defect remained open. (Access and task results)
- L7: 6. On template two, screen-reader testing announced the upload label correctly but did not test keyboard activation; the source explicitly says a correct label is not evidence that a keyboard user can upload a document. (Access and task results)
- L22: - The scan and tests do not establish general accessibility: only three templates received the contrast scan, the upload keyboard defect was open, and no general accessibility certification was commissioned. A deployed keyboard fix is not documented. (Facts 1, 5–7)
- L29: - At review, the upload defect remained open; management had approved a proposed fix, but no deployment was recorded. The coordinator authorised a usability study conditional on a verified fix and withheld general release. (Facts 5, 7, 13)

[projection.md](calls/language-r1-B-Research/projection.md)

- L7: 5. An automated colour-contrast scan passed on three tested page templates. Keyboard-only testing could not activate the upload button on template two, and the defect remained open. (Access and task results)
- L8: 6. On template two, screen-reader testing announced the upload label correctly but did not test keyboard activation; the source explicitly says a correct label is not evidence that a keyboard user can upload a document. (Access and task results)
- L22: - The scan and tests do not establish general accessibility: only three templates received the contrast scan, the upload keyboard defect was open, and no general accessibility certification was commissioned. A deployed keyboard fix is not documented. (Facts 1, 5–7)

## language-r1-B / outcome — kern — correct

Bron [L7](cases/language/source.md):

> Task completion in the old English-only service was 33 of 50 moderated sessions. The multilingual form recorded 42 of 50 unmoderated sessions.

[content.md](calls/language-r1-B-Research/content.md)

- L9: 8. The old English-only service had 33 completions in 50 moderated sessions; the multilingual form recorded 42 in 50 unmoderated sessions. The old measure required a final submitted application, while the new dashboard counted reaching the review screen. (Access and task results)

[projection.md](calls/language-r1-B-Research/projection.md)

- L10: 8. The old English-only service had 33 completions in 50 moderated sessions; the multilingual form recorded 42 in 50 unmoderated sessions. The old measure required a final submitted application, while the new dashboard counted reaching the review screen. (Access and task results)

## language-r1-B / method — kern — correct

Bron [L7](cases/language/source.md):

> Completion formerly required a final submitted application; the new dashboard counted reaching the review screen.

[content.md](calls/language-r1-B-Research/content.md)

- L9: 8. The old English-only service had 33 completions in 50 moderated sessions; the multilingual form recorded 42 in 50 unmoderated sessions. The old measure required a final submitted application, while the new dashboard counted reaching the review screen. (Access and task results)

[projection.md](calls/language-r1-B-Research/projection.md)

- L10: 8. The old English-only service had 33 completions in 50 moderated sessions; the multilingual form recorded 42 in 50 unmoderated sessions. The old measure required a final submitted application, while the new dashboard counted reaching the review screen. (Access and task results)

## language-r1-B / allocation — belangrijk — correct

Bron [L7](cases/language/source.md):

> Participants were recruited from different channels and neither assigned randomly nor matched for prior experience.

[content.md](calls/language-r1-B-Research/content.md)

- L10: 9. Participants came from different recruitment channels and were neither randomly assigned nor matched for prior experience. The source says the apparent dashboard improvement is not a comparable completion-effect estimate. (Access and task results)
- L23: - The differing task definitions, moderation, recruitment, and participant comparability prevent attributing the apparent difference to the multilingual form or quantifying a completion effect. Successful submission or benefit uptake was not measured by the satisfaction poll. (Facts 8–9, 14)
- L28: - The source reports old-service sessions and multilingual-form dashboard sessions with different completion definitions and recruitment conditions; it does not establish a comparable effect or causal timeline. (Facts 8–9)

[projection.md](calls/language-r1-B-Research/projection.md)

- L11: 9. Participants came from different recruitment channels and were neither randomly assigned nor matched for prior experience. The source says the apparent dashboard improvement is not a comparable completion-effect estimate. (Access and task results)
- L23: - The differing task definitions, moderation, recruitment, and participant comparability prevent attributing the apparent difference to the multilingual form or quantifying a completion effect. Successful submission or benefit uptake was not measured by the satisfaction poll. (Facts 8–9, 14)

## language-r1-B / term — kern — correct

Bron [L9](cases/language/source.md):

> A preprocessing step called case folding normalised letter case; it did not merge benefit cases or applications.

[content.md](calls/language-r1-B-Research/content.md)

- L11: 10. “Case folding” normalised letter case; it did not merge benefit cases or applications. On the same 80 strings, three runs of a sampling-based checker flagged six, five and six semantic issues. Human reference review identified six deadline reversals; the checker version stayed fixed while sample order varied. (Processing and reproducibility)

[projection.md](calls/language-r1-B-Research/projection.md)

- L12: 10. “Case folding” normalised letter case; it did not merge benefit cases or applications. On the same 80 strings, three runs of a sampling-based checker flagged six, five and six semantic issues. Human reference review identified six deadline reversals; the checker version stayed fixed while sample order varied. (Processing and reproducibility)

## language-r1-B / variation — kern — correct

Bron [L9](cases/language/source.md):

> Three runs on the same 80 strings flagged six, five and six semantic issues with a sampling-based checker. A human reference review identified the six deadline reversals.

[content.md](calls/language-r1-B-Research/content.md)

- L11: 10. “Case folding” normalised letter case; it did not merge benefit cases or applications. On the same 80 strings, three runs of a sampling-based checker flagged six, five and six semantic issues. Human reference review identified six deadline reversals; the checker version stayed fixed while sample order varied. (Processing and reproducibility)
- L12: 11. The source says the run counts establish neither three different ground truths nor a stable checker. (Processing and reproducibility)
- L24: - The checker’s run variation does not establish its reliability or a stable issue count; the source reports one human reference review identifying six reversals. (Facts 10–11)
- L32: - Technical definitions and checker variation: Facts 10–11.

[projection.md](calls/language-r1-B-Research/projection.md)

- L12: 10. “Case folding” normalised letter case; it did not merge benefit cases or applications. On the same 80 strings, three runs of a sampling-based checker flagged six, five and six semantic issues. Human reference review identified six deadline reversals; the checker version stayed fixed while sample order varied. (Processing and reproducibility)
- L13: 11. The source says the run counts establish neither three different ground truths nor a stable checker. (Processing and reproducibility)
- L24: - The checker’s run variation does not establish its reliability or a stable issue count; the source reports one human reference review identifying six reversals. (Facts 10–11)

## language-r1-B / legal — kern — correct

Bron [L11](cases/language/source.md):

> The appendix says legal approval covers the Spanish consent paragraph only. It gives no approval date or temporal relationship to the sample review. Polish consent approval is not documented.

[content.md](calls/language-r1-B-Research/content.md)

- L13: 12. Legal approval covers only the Spanish consent paragraph; no approval date or relation to the sample review is given. Polish consent approval is not documented. Translation availability alone does not establish legal approval. (Status and decision boundary)
- L25: - The source does not establish Polish consent approval, the date or review-time relationship of Spanish consent approval, or general legal approval. (Fact 12)

[projection.md](calls/language-r1-B-Research/projection.md)

- L14: 12. Legal approval covers only the Spanish consent paragraph; no approval date or relation to the sample review is given. Polish consent approval is not documented. Translation availability alone does not establish legal approval. (Status and decision boundary)
- L25: - The source does not establish Polish consent approval, the date or review-time relationship of Spanish consent approval, or general legal approval. (Fact 12)

## language-r1-B / decision — kern — correct

Bron [L11](cases/language/source.md):

> The coordinator authorised a corrected-form usability study, conditional on a verified upload fix, and withheld general release.

[content.md](calls/language-r1-B-Research/content.md)

- L14: 13. The coordinator authorised a corrected-form usability study conditional on a verified upload fix and withheld general release. The study budget includes translation fees but excludes staff time; no cost saving is demonstrated. (Status and decision boundary)
- L26: - The source does not demonstrate population-level satisfaction, general inclusion, cost savings, or outcomes from the proposed corrected-form study. The study remains conditional on a verified upload fix; no general release was authorised. (Facts 13–15)
- L29: - At review, the upload defect remained open; management had approved a proposed fix, but no deployment was recorded. The coordinator authorised a usability study conditional on a verified fix and withheld general release. (Facts 5, 7, 13)

[projection.md](calls/language-r1-B-Research/projection.md)

- L15: 13. The coordinator authorised a corrected-form usability study conditional on a verified upload fix and withheld general release. The study budget includes translation fees but excludes staff time; no cost saving is demonstrated. (Status and decision boundary)
- L26: - The source does not demonstrate population-level satisfaction, general inclusion, cost savings, or outcomes from the proposed corrected-form study. The study remains conditional on a verified upload fix; no general release was authorised. (Facts 13–15)

## language-r1-B2 / sample — kern — correct

Bron [L4](cases/language/source.md):

> Reviewers sampled 80 of 640 message strings: 40 Spanish and 40 Polish.

[content.md](calls/language-r1-B2-Research/content.md)

- L3: 2. Reviewers sampled 80 of 640 message strings: 40 Spanish and 40 Polish. All 80 retained approved glossary entitlement terms, but six Spanish strings reversed the deadline-exception meaning; no such reversal was found in the Polish sample. (Sample and linguistic checks)
- L4: 3. A second reviewer agreed with the six identified reversals; the remaining strings did not receive that second semantic review. Terminology conformity and semantic correctness were separate checks, and the sample results do not certify all 640 strings. (Sample and linguistic checks)
- L10: 9. On the same 80 strings, three runs of the sampling-based checker flagged six, five and six semantic issues. The checker version was fixed, but sample order varied; a human reference review identified the six deadline reversals. The source says run counts establish neither three different ground truths nor a stable checker. (Processing and reproducibility)
- L17: - Whether the unreviewed strings, or the full set of 640, are semantically correct; whether the sampled checks generalise to all strings. (Facts 2–3)

[projection.md](calls/language-r1-B2-Research/projection.md)

- L4: 2. Reviewers sampled 80 of 640 message strings: 40 Spanish and 40 Polish. All 80 retained approved glossary entitlement terms, but six Spanish strings reversed the deadline-exception meaning; no such reversal was found in the Polish sample. (Sample and linguistic checks)
- L5: 3. A second reviewer agreed with the six identified reversals; the remaining strings did not receive that second semantic review. Terminology conformity and semantic correctness were separate checks, and the sample results do not certify all 640 strings. (Sample and linguistic checks)
- L11: 9. On the same 80 strings, three runs of the sampling-based checker flagged six, five and six semantic issues. The checker version was fixed, but sample order varied; a human reference review identified the six deadline reversals. The source says run counts establish neither three different ground truths nor a stable checker. (Processing and reproducibility)
- L18: - Whether the unreviewed strings, or the full set of 640, are semantically correct; whether the sampled checks generalise to all strings. (Facts 2–3)

## language-r1-B2 / contrast — kern — correct

Bron [L4](cases/language/source.md):

> All 80 retained the glossary's approved entitlement terms. Six Spanish strings nevertheless reversed the meaning of the deadline exception. The Polish sample had no deadline reversal.

[content.md](calls/language-r1-B2-Research/content.md)

- L3: 2. Reviewers sampled 80 of 640 message strings: 40 Spanish and 40 Polish. All 80 retained approved glossary entitlement terms, but six Spanish strings reversed the deadline-exception meaning; no such reversal was found in the Polish sample. (Sample and linguistic checks)
- L4: 3. A second reviewer agreed with the six identified reversals; the remaining strings did not receive that second semantic review. Terminology conformity and semantic correctness were separate checks, and the sample results do not certify all 640 strings. (Sample and linguistic checks)
- L10: 9. On the same 80 strings, three runs of the sampling-based checker flagged six, five and six semantic issues. The checker version was fixed, but sample order varied; a human reference review identified the six deadline reversals. The source says run counts establish neither three different ground truths nor a stable checker. (Processing and reproducibility)

[projection.md](calls/language-r1-B2-Research/projection.md)

- L4: 2. Reviewers sampled 80 of 640 message strings: 40 Spanish and 40 Polish. All 80 retained approved glossary entitlement terms, but six Spanish strings reversed the deadline-exception meaning; no such reversal was found in the Polish sample. (Sample and linguistic checks)
- L5: 3. A second reviewer agreed with the six identified reversals; the remaining strings did not receive that second semantic review. Terminology conformity and semantic correctness were separate checks, and the sample results do not certify all 640 strings. (Sample and linguistic checks)
- L11: 9. On the same 80 strings, three runs of the sampling-based checker flagged six, five and six semantic issues. The checker version was fixed, but sample order varied; a human reference review identified the six deadline reversals. The source says run counts establish neither three different ground truths nor a stable checker. (Processing and reproducibility)

## language-r1-B2 / review — belangrijk — correct

Bron [L4](cases/language/source.md):

> A second reviewer checked the six reversals and agreed. The remaining strings did not receive that second semantic review.

[content.md](calls/language-r1-B2-Research/content.md)

- L4: 3. A second reviewer agreed with the six identified reversals; the remaining strings did not receive that second semantic review. Terminology conformity and semantic correctness were separate checks, and the sample results do not certify all 640 strings. (Sample and linguistic checks)

[projection.md](calls/language-r1-B2-Research/projection.md)

- L5: 3. A second reviewer agreed with the six identified reversals; the remaining strings did not receive that second semantic review. Terminology conformity and semantic correctness were separate checks, and the sample results do not certify all 640 strings. (Sample and linguistic checks)

## language-r1-B2 / access — kern — correct

Bron [L6](cases/language/source.md):

> An automated colour-contrast scan passed the three tested page templates. Keyboard-only testing could not activate the document-upload button on the second template.

[content.md](calls/language-r1-B2-Research/content.md)

- L5: 4. An automated colour-contrast scan passed the three tested page templates. Keyboard-only testing could not activate the document-upload button on the second template, and the defect was still open. (Access and task results)
- L6: 5. On that template, screen-reader testing announced the upload label correctly but did not test keyboard activation. Management approved a keyboard-fix proposal; no deployed fix is recorded. (Access and task results)
- L12: 11. The coordinator authorised a corrected-form usability study conditional on a verified upload fix and withheld general release. The budget includes translation fees but excludes staff time; no cost saving is demonstrated. (Status and decision boundary)
- L18: - Whether the upload defect has since been fixed or keyboard users can complete upload; the source records no deployed fix. Broader accessibility is also not established by the limited checks, and no general certification was commissioned. (Facts 1, 4–5)
- L25: None; the source gives no dated chronology for the sample review, legal approval, fix proposal or decision. The study is an authorised future step conditional on a verified upload fix, not a reported completed study. (Facts 5, 10–11)

[projection.md](calls/language-r1-B2-Research/projection.md)

- L6: 4. An automated colour-contrast scan passed the three tested page templates. Keyboard-only testing could not activate the document-upload button on the second template, and the defect was still open. (Access and task results)
- L7: 5. On that template, screen-reader testing announced the upload label correctly but did not test keyboard activation. Management approved a keyboard-fix proposal; no deployed fix is recorded. (Access and task results)
- L13: 11. The coordinator authorised a corrected-form usability study conditional on a verified upload fix and withheld general release. The budget includes translation fees but excludes staff time; no cost saving is demonstrated. (Status and decision boundary)
- L19: - Whether the upload defect has since been fixed or keyboard users can complete upload; the source records no deployed fix. Broader accessibility is also not established by the limited checks, and no general certification was commissioned. (Facts 1, 4–5)

## language-r1-B2 / open — kern — correct

Bron [L6](cases/language/source.md):

> The upload defect was still open. Screen-reader testing announced the upload label correctly on that template, but did not test keyboard activation.

[content.md](calls/language-r1-B2-Research/content.md)

- L2: 1. The source describes a fictional benefits form and says it contains no real resident data; the “inclusive launch” is an ambition, not a measured inclusion outcome, and no general accessibility certification was commissioned. (Opening note)
- L5: 4. An automated colour-contrast scan passed the three tested page templates. Keyboard-only testing could not activate the document-upload button on the second template, and the defect was still open. (Access and task results)
- L6: 5. On that template, screen-reader testing announced the upload label correctly but did not test keyboard activation. Management approved a keyboard-fix proposal; no deployed fix is recorded. (Access and task results)

[projection.md](calls/language-r1-B2-Research/projection.md)

- L3: 1. The source describes a fictional benefits form and says it contains no real resident data; the “inclusive launch” is an ambition, not a measured inclusion outcome, and no general accessibility certification was commissioned. (Opening note)
- L6: 4. An automated colour-contrast scan passed the three tested page templates. Keyboard-only testing could not activate the document-upload button on the second template, and the defect was still open. (Access and task results)
- L7: 5. On that template, screen-reader testing announced the upload label correctly but did not test keyboard activation. Management approved a keyboard-fix proposal; no deployed fix is recorded. (Access and task results)

## language-r1-B2 / outcome — kern — correct

Bron [L7](cases/language/source.md):

> Task completion in the old English-only service was 33 of 50 moderated sessions. The multilingual form recorded 42 of 50 unmoderated sessions.

[content.md](calls/language-r1-B2-Research/content.md)

- L7: 6. The old English-only service recorded completion in 33 of 50 moderated sessions, where completion required a final submitted application. The multilingual form recorded 42 of 50 unmoderated sessions, with the dashboard counting reaching the review screen. (Access and task results)

[projection.md](calls/language-r1-B2-Research/projection.md)

- L8: 6. The old English-only service recorded completion in 33 of 50 moderated sessions, where completion required a final submitted application. The multilingual form recorded 42 of 50 unmoderated sessions, with the dashboard counting reaching the review screen. (Access and task results)

## language-r1-B2 / method — kern — correct

Bron [L7](cases/language/source.md):

> Completion formerly required a final submitted application; the new dashboard counted reaching the review screen.

[content.md](calls/language-r1-B2-Research/content.md)

- L7: 6. The old English-only service recorded completion in 33 of 50 moderated sessions, where completion required a final submitted application. The multilingual form recorded 42 of 50 unmoderated sessions, with the dashboard counting reaching the review screen. (Access and task results)

[projection.md](calls/language-r1-B2-Research/projection.md)

- L8: 6. The old English-only service recorded completion in 33 of 50 moderated sessions, where completion required a final submitted application. The multilingual form recorded 42 of 50 unmoderated sessions, with the dashboard counting reaching the review screen. (Access and task results)

## language-r1-B2 / allocation — belangrijk — correct

Bron [L7](cases/language/source.md):

> Participants were recruited from different channels and neither assigned randomly nor matched for prior experience.

[content.md](calls/language-r1-B2-Research/content.md)

- L8: 7. Participants came through different recruitment channels and were neither randomly assigned nor matched for prior experience. The source says the dashboard’s apparent improvement is not a comparable completion-effect estimate. (Access and task results)
- L19: - Whether the reported task rates reflect a real change in completion: the endpoints, moderation, recruitment and participant comparability differ. (Facts 6–7)

[projection.md](calls/language-r1-B2-Research/projection.md)

- L9: 7. Participants came through different recruitment channels and were neither randomly assigned nor matched for prior experience. The source says the dashboard’s apparent improvement is not a comparable completion-effect estimate. (Access and task results)
- L20: - Whether the reported task rates reflect a real change in completion: the endpoints, moderation, recruitment and participant comparability differ. (Facts 6–7)

## language-r1-B2 / term — kern — correct

Bron [L9](cases/language/source.md):

> A preprocessing step called case folding normalised letter case; it did not merge benefit cases or applications.

[content.md](calls/language-r1-B2-Research/content.md)

- L9: 8. “Case folding” meant normalising letter case; it did not merge benefit cases or applications. (Processing and reproducibility)

[projection.md](calls/language-r1-B2-Research/projection.md)

- L10: 8. “Case folding” meant normalising letter case; it did not merge benefit cases or applications. (Processing and reproducibility)

## language-r1-B2 / variation — kern — correct

Bron [L9](cases/language/source.md):

> Three runs on the same 80 strings flagged six, five and six semantic issues with a sampling-based checker. A human reference review identified the six deadline reversals.

[content.md](calls/language-r1-B2-Research/content.md)

- L10: 9. On the same 80 strings, three runs of the sampling-based checker flagged six, five and six semantic issues. The checker version was fixed, but sample order varied; a human reference review identified the six deadline reversals. The source says run counts establish neither three different ground truths nor a stable checker. (Processing and reproducibility)

[projection.md](calls/language-r1-B2-Research/projection.md)

- L11: 9. On the same 80 strings, three runs of the sampling-based checker flagged six, five and six semantic issues. The checker version was fixed, but sample order varied; a human reference review identified the six deadline reversals. The source says run counts establish neither three different ground truths nor a stable checker. (Processing and reproducibility)

## language-r1-B2 / legal — kern — correct

Bron [L11](cases/language/source.md):

> The appendix says legal approval covers the Spanish consent paragraph only. It gives no approval date or temporal relationship to the sample review. Polish consent approval is not documented.

[content.md](calls/language-r1-B2-Research/content.md)

- L11: 10. Legal approval covers only the Spanish consent paragraph; no approval date or temporal relationship to the sample review is given. Polish consent approval is not documented, and translation availability alone does not establish legal approval. (Status and decision boundary)
- L20: - Whether Polish consent has legal approval, and when the documented Spanish approval occurred relative to sampling. (Fact 10)
- L25: None; the source gives no dated chronology for the sample review, legal approval, fix proposal or decision. The study is an authorised future step conditional on a verified upload fix, not a reported completed study. (Facts 5, 10–11)

[projection.md](calls/language-r1-B2-Research/projection.md)

- L12: 10. Legal approval covers only the Spanish consent paragraph; no approval date or temporal relationship to the sample review is given. Polish consent approval is not documented, and translation availability alone does not establish legal approval. (Status and decision boundary)
- L21: - Whether Polish consent has legal approval, and when the documented Spanish approval occurred relative to sampling. (Fact 10)

## language-r1-B2 / decision — kern — correct

Bron [L11](cases/language/source.md):

> The coordinator authorised a corrected-form usability study, conditional on a verified upload fix, and withheld general release.

[content.md](calls/language-r1-B2-Research/content.md)

- L12: 11. The coordinator authorised a corrected-form usability study conditional on a verified upload fix and withheld general release. The budget includes translation fees but excludes staff time; no cost saving is demonstrated. (Status and decision boundary)

[projection.md](calls/language-r1-B2-Research/projection.md)

- L13: 11. The coordinator authorised a corrected-form usability study conditional on a verified upload fix and withheld general release. The budget includes translation fees but excludes staff time; no cost saving is demonstrated. (Status and decision boundary)

## language-r1-A / sample — kern — correct

Bron [L4](cases/language/source.md):

> Reviewers sampled 80 of 640 message strings: 40 Spanish and 40 Polish.

[content.md](calls/language-r1-A-Research/content.md)

- L7: 5. Reviewers sampled 80 of 640 message strings.
- L8: 6. The sample contained 40 Spanish strings and 40 Polish strings.
- L9: 7. All 80 sampled strings retained the glossary’s approved entitlement terms.
- L13: 11. Neither result certifies all 640 strings.
- L33: 31. Three runs on the same 80 strings flagged six, five, and six semantic issues with a sampling-based checker.
- L60: - Whether the 560 unsampled strings contain the same or other terminology or semantic issues; the source says the sample results do not certify all 640 strings.
- L77: - Reviewers sampled 80 of 640 strings: 40 Spanish and 40 Polish. The source does not date this review.
- L78: - The sample checks recorded approved entitlement terms in all 80 strings, six Spanish deadline reversals, and no Polish deadline reversal. A second reviewer later checked and agreed with the six reversals; the remaining strings did not receive that second semantic review.
- L81: - Three checker runs on the same 80 strings flagged six, five, and six issues; the checker version was fixed and sample order varied. The source does not establish that these counts are separate ground truths or demonstrate checker stability.

[projection.md](calls/language-r1-A-Research/projection.md)

- L7: 5. Reviewers sampled 80 of 640 message strings.
- L8: 6. The sample contained 40 Spanish strings and 40 Polish strings.
- L9: 7. All 80 sampled strings retained the glossary’s approved entitlement terms.
- L13: 11. Neither result certifies all 640 strings.
- L33: 31. Three runs on the same 80 strings flagged six, five, and six semantic issues with a sampling-based checker.
- L56: - Whether the 560 unsampled strings contain the same or other terminology or semantic issues; the source says the sample results do not certify all 640 strings.

## language-r1-A / contrast — kern — correct

Bron [L4](cases/language/source.md):

> All 80 retained the glossary's approved entitlement terms. Six Spanish strings nevertheless reversed the meaning of the deadline exception. The Polish sample had no deadline reversal.

[content.md](calls/language-r1-A-Research/content.md)

- L9: 7. All 80 sampled strings retained the glossary’s approved entitlement terms.
- L10: 8. Six Spanish strings reversed the meaning of the deadline exception.
- L11: 9. The Polish sample had no deadline reversal.
- L12: 10. Terminology conformity and semantic correctness were separate checks.
- L14: 12. A second reviewer checked the six reversals and agreed.
- L34: 32. A human reference review identified the six deadline reversals.
- L60: - Whether the 560 unsampled strings contain the same or other terminology or semantic issues; the source says the sample results do not certify all 640 strings.
- L78: - The sample checks recorded approved entitlement terms in all 80 strings, six Spanish deadline reversals, and no Polish deadline reversal. A second reviewer later checked and agreed with the six reversals; the remaining strings did not receive that second semantic review.

[projection.md](calls/language-r1-A-Research/projection.md)

- L9: 7. All 80 sampled strings retained the glossary’s approved entitlement terms.
- L10: 8. Six Spanish strings reversed the meaning of the deadline exception.
- L11: 9. The Polish sample had no deadline reversal.
- L12: 10. Terminology conformity and semantic correctness were separate checks.
- L14: 12. A second reviewer checked the six reversals and agreed.
- L34: 32. A human reference review identified the six deadline reversals.
- L56: - Whether the 560 unsampled strings contain the same or other terminology or semantic issues; the source says the sample results do not certify all 640 strings.

## language-r1-A / review — belangrijk — correct

Bron [L4](cases/language/source.md):

> A second reviewer checked the six reversals and agreed. The remaining strings did not receive that second semantic review.

[content.md](calls/language-r1-A-Research/content.md)

- L14: 12. A second reviewer checked the six reversals and agreed.
- L15: 13. The remaining strings did not receive that second semantic review.
- L78: - The sample checks recorded approved entitlement terms in all 80 strings, six Spanish deadline reversals, and no Polish deadline reversal. A second reviewer later checked and agreed with the six reversals; the remaining strings did not receive that second semantic review.

[projection.md](calls/language-r1-A-Research/projection.md)

- L14: 12. A second reviewer checked the six reversals and agreed.
- L15: 13. The remaining strings did not receive that second semantic review.

## language-r1-A / access — kern — correct

Bron [L6](cases/language/source.md):

> An automated colour-contrast scan passed the three tested page templates. Keyboard-only testing could not activate the document-upload button on the second template.

[content.md](calls/language-r1-A-Research/content.md)

- L16: 14. An automated colour-contrast scan passed the three tested page templates.
- L17: 15. Keyboard-only testing could not activate the document-upload button on the second template.
- L18: 16. The upload defect was still open.
- L19: 17. Screen-reader testing announced the upload label correctly on the second template.
- L20: 18. Screen-reader testing did not test keyboard activation.
- L21: 19. The source states that a visually correct label is not evidence that a keyboard user can upload a document.
- L22: 20. Management approved a keyboard-fix proposal.
- L23: 21. No deployed keyboard fix is recorded.
- L43: 41. The coordinator authorised a corrected-form usability study conditional on a verified upload fix.
- L62: - Whether the upload defect was fixed after the recorded status; no deployed fix is recorded, and the source does not give a later status.
- L63: - Whether the approved keyboard-fix proposal was implemented.
- L71: - Reasons for withholding general release beyond the stated conditional authorization of a study and the requirement for a verified upload fix.
- L73: - The source does not establish a broad finding that the service is accessible or inaccessible. It reports specific test results, including a keyboard activation failure on one tested template.
- L79: - The automated contrast scan passed three tested templates. Keyboard-only testing could not activate the upload button on the second template, and the defect was still open. Screen-reader testing announced the label correctly on that template but did not test keyboard activation. The source does not establish the chronology among these access checks.
- L83: - The coordinator authorised a corrected-form usability study conditional on a verified upload fix and withheld general release. The source does not establish a broader chronology or decision rationale beyond that stated condition.
- L88: - Facts 14–21 are relevant to the reported access checks and upload-button result.

[projection.md](calls/language-r1-A-Research/projection.md)

- L16: 14. An automated colour-contrast scan passed the three tested page templates.
- L17: 15. Keyboard-only testing could not activate the document-upload button on the second template.
- L18: 16. The upload defect was still open.
- L19: 17. Screen-reader testing announced the upload label correctly on the second template.
- L20: 18. Screen-reader testing did not test keyboard activation.
- L21: 19. The source states that a visually correct label is not evidence that a keyboard user can upload a document.
- L22: 20. Management approved a keyboard-fix proposal.
- L23: 21. No deployed keyboard fix is recorded.
- L43: 41. The coordinator authorised a corrected-form usability study conditional on a verified upload fix.
- L58: - Whether the upload defect was fixed after the recorded status; no deployed fix is recorded, and the source does not give a later status.
- L59: - Whether the approved keyboard-fix proposal was implemented.
- L67: - Reasons for withholding general release beyond the stated conditional authorization of a study and the requirement for a verified upload fix.
- L69: - The source does not establish a broad finding that the service is accessible or inaccessible. It reports specific test results, including a keyboard activation failure on one tested template.

## language-r1-A / open — kern — correct

Bron [L6](cases/language/source.md):

> The upload defect was still open. Screen-reader testing announced the upload label correctly on that template, but did not test keyboard activation.

[content.md](calls/language-r1-A-Research/content.md)

- L18: 16. The upload defect was still open.
- L19: 17. Screen-reader testing announced the upload label correctly on the second template.
- L20: 18. Screen-reader testing did not test keyboard activation.
- L21: 19. The source states that a visually correct label is not evidence that a keyboard user can upload a document.
- L73: - The source does not establish a broad finding that the service is accessible or inaccessible. It reports specific test results, including a keyboard activation failure on one tested template.
- L79: - The automated contrast scan passed three tested templates. Keyboard-only testing could not activate the upload button on the second template, and the defect was still open. Screen-reader testing announced the label correctly on that template but did not test keyboard activation. The source does not establish the chronology among these access checks.

[projection.md](calls/language-r1-A-Research/projection.md)

- L18: 16. The upload defect was still open.
- L19: 17. Screen-reader testing announced the upload label correctly on the second template.
- L20: 18. Screen-reader testing did not test keyboard activation.
- L21: 19. The source states that a visually correct label is not evidence that a keyboard user can upload a document.
- L69: - The source does not establish a broad finding that the service is accessible or inaccessible. It reports specific test results, including a keyboard activation failure on one tested template.

## language-r1-A / outcome — kern — correct

Bron [L7](cases/language/source.md):

> Task completion in the old English-only service was 33 of 50 moderated sessions. The multilingual form recorded 42 of 50 unmoderated sessions.

[content.md](calls/language-r1-A-Research/content.md)

- L24: 22. Task completion in the old English-only service was 33 of 50 moderated sessions.
- L25: 23. The multilingual form recorded 42 completions in 50 unmoderated sessions.
- L35: 33. The automated run counts do not establish three different ground truths or a stable checker.
- L44: 42. The coordinator withheld general release.

[projection.md](calls/language-r1-A-Research/projection.md)

- L24: 22. Task completion in the old English-only service was 33 of 50 moderated sessions.
- L25: 23. The multilingual form recorded 42 completions in 50 unmoderated sessions.
- L35: 33. The automated run counts do not establish three different ground truths or a stable checker.
- L44: 42. The coordinator withheld general release.

## language-r1-A / method — kern — correct

Bron [L7](cases/language/source.md):

> Completion formerly required a final submitted application; the new dashboard counted reaching the review screen.

[content.md](calls/language-r1-A-Research/content.md)

- L26: 24. Completion in the old service required a final submitted application.
- L27: 25. The new dashboard counted reaching the review screen as completion.

[projection.md](calls/language-r1-A-Research/projection.md)

- L26: 24. Completion in the old service required a final submitted application.
- L27: 25. The new dashboard counted reaching the review screen as completion.

## language-r1-A / allocation — belangrijk — correct

Bron [L7](cases/language/source.md):

> Participants were recruited from different channels and neither assigned randomly nor matched for prior experience.

[content.md](calls/language-r1-A-Research/content.md)

- L28: 26. Participants were recruited from different channels.
- L29: 27. Participants were neither assigned randomly nor matched for prior experience.
- L64: - Whether task completion would differ under comparable completion definitions, recruitment, and participant assignment; the reported figures are not a comparable completion-effect estimate.

[projection.md](calls/language-r1-A-Research/projection.md)

- L28: 26. Participants were recruited from different channels.
- L29: 27. Participants were neither assigned randomly nor matched for prior experience.
- L60: - Whether task completion would differ under comparable completion definitions, recruitment, and participant assignment; the reported figures are not a comparable completion-effect estimate.

## language-r1-A / term — kern — correct

Bron [L9](cases/language/source.md):

> A preprocessing step called case folding normalised letter case; it did not merge benefit cases or applications.

[content.md](calls/language-r1-A-Research/content.md)

- L31: 29. A preprocessing step called case folding normalised letter case.
- L32: 30. Case folding did not merge benefit cases or applications.

[projection.md](calls/language-r1-A-Research/projection.md)

- L31: 29. A preprocessing step called case folding normalised letter case.
- L32: 30. Case folding did not merge benefit cases or applications.

## language-r1-A / variation — kern — correct

Bron [L9](cases/language/source.md):

> Three runs on the same 80 strings flagged six, five and six semantic issues with a sampling-based checker. A human reference review identified the six deadline reversals.

[content.md](calls/language-r1-A-Research/content.md)

- L33: 31. Three runs on the same 80 strings flagged six, five, and six semantic issues with a sampling-based checker.
- L34: 32. A human reference review identified the six deadline reversals.
- L35: 33. The automated run counts do not establish three different ground truths or a stable checker.
- L36: 34. The checker version was held fixed.
- L37: 35. The checker’s sample order varied.
- L65: - Whether the checker would produce stable results on repeated runs; the source says the run counts do not establish a stable checker.
- L81: - Three checker runs on the same 80 strings flagged six, five, and six issues; the checker version was fixed and sample order varied. The source does not establish that these counts are separate ground truths or demonstrate checker stability.
- L90: - Facts 29–35 are relevant to the preprocessing definition and checker-run variation.

[projection.md](calls/language-r1-A-Research/projection.md)

- L33: 31. Three runs on the same 80 strings flagged six, five, and six semantic issues with a sampling-based checker.
- L34: 32. A human reference review identified the six deadline reversals.
- L35: 33. The automated run counts do not establish three different ground truths or a stable checker.
- L36: 34. The checker version was held fixed.
- L37: 35. The checker’s sample order varied.
- L61: - Whether the checker would produce stable results on repeated runs; the source says the run counts do not establish a stable checker.

## language-r1-A / legal — kern — correct

Bron [L11](cases/language/source.md):

> The appendix says legal approval covers the Spanish consent paragraph only. It gives no approval date or temporal relationship to the sample review. Polish consent approval is not documented.

[content.md](calls/language-r1-A-Research/content.md)

- L38: 36. The appendix says legal approval covers the Spanish consent paragraph only.
- L41: 39. Polish consent approval is not documented.
- L42: 40. Translation availability does not itself establish legal approval.
- L66: - Whether Polish consent received legal approval.
- L67: - The date of the stated Spanish consent approval and its timing relative to the sample review.
- L82: - The appendix states that legal approval covers the Spanish consent paragraph only; it gives no date or timing relative to the sample review. Polish consent approval is not documented.

[projection.md](calls/language-r1-A-Research/projection.md)

- L38: 36. The appendix says legal approval covers the Spanish consent paragraph only.
- L41: 39. Polish consent approval is not documented.
- L42: 40. Translation availability does not itself establish legal approval.
- L62: - Whether Polish consent received legal approval.
- L63: - The date of the stated Spanish consent approval and its timing relative to the sample review.

## language-r1-A / decision — kern — correct

Bron [L11](cases/language/source.md):

> The coordinator authorised a corrected-form usability study, conditional on a verified upload fix, and withheld general release.

[content.md](calls/language-r1-A-Research/content.md)

- L43: 41. The coordinator authorised a corrected-form usability study conditional on a verified upload fix.
- L44: 42. The coordinator withheld general release.
- L71: - Reasons for withholding general release beyond the stated conditional authorization of a study and the requirement for a verified upload fix.
- L83: - The coordinator authorised a corrected-form usability study conditional on a verified upload fix and withheld general release. The source does not establish a broader chronology or decision rationale beyond that stated condition.

[projection.md](calls/language-r1-A-Research/projection.md)

- L43: 41. The coordinator authorised a corrected-form usability study conditional on a verified upload fix.
- L44: 42. The coordinator withheld general release.
- L67: - Reasons for withholding general release beyond the stated conditional authorization of a study and the requirement for a verified upload fix.

## heat-r1-B2 / ledger — kern — correct

Bron [L4](cases/heat/source.md):

> The final metered record shows 410 kWh of useful heat from 500 kWh electrical input over the trial boundary.

[content.md](calls/heat-r1-B2-Research/content.md)

- L2: 1. The final metered record reports 410 kWh useful heat from 500 kWh electrical input over the trial boundary. A draft listed 450 kWh; 40 kWh duplicated during meter-file concatenation was removed. The ratio applies only to this trial period and boundary, not seasonal efficiency. (Performance and measurement)
- L9: 8. Three identical-input simulation runs estimated usable discharge at 402, 418 and 407 kWh. The solver used stochastic initialisation; these were simulations, not additional metered field trials, and do not supersede the measured 410 kWh. The source identifies solver variation as an attribution limit for interpreting simulation improvements. (Variability, sequencing and decision)
- L29: - A draft heat figure of 450 kWh included a 40 kWh duplicated entry, which was removed; the final metered record reports 410 kWh useful heat. (Fact 1)

[projection.md](calls/heat-r1-B2-Research/projection.md)

- L3: 1. The final metered record reports 410 kWh useful heat from 500 kWh electrical input over the trial boundary. A draft listed 450 kWh; 40 kWh duplicated during meter-file concatenation was removed. The ratio applies only to this trial period and boundary, not seasonal efficiency. (Performance and measurement)
- L10: 8. Three identical-input simulation runs estimated usable discharge at 402, 418 and 407 kWh. The solver used stochastic initialisation; these were simulations, not additional metered field trials, and do not supersede the measured 410 kWh. The source identifies solver variation as an attribution limit for interpreting simulation improvements. (Variability, sequencing and decision)

## heat-r1-B2 / method — kern — correct

Bron [L4](cases/heat/source.md):

> The legacy boiler's displayed 76% used fuel energy on a lower-heating-value basis, while the electrical trial used meter input and included pump demand. No common-boundary reanalysis exists.

[content.md](calls/heat-r1-B2-Research/content.md)

- L3: 2. The legacy boiler’s displayed 76% used fuel energy on a lower-heating-value basis; the electrical trial used meter input and included pump demand. No common-boundary reanalysis exists. (Performance and measurement)
- L20: - Seasonal efficiency and a like-for-like efficiency comparison with the legacy boiler are unsupported: the trial boundary differs from the boiler’s stated basis, and no common-boundary reanalysis is provided. (Facts 1–3)

[projection.md](calls/heat-r1-B2-Research/projection.md)

- L4: 2. The legacy boiler’s displayed 76% used fuel energy on a lower-heating-value basis; the electrical trial used meter input and included pump demand. No common-boundary reanalysis exists. (Performance and measurement)
- L18: - Seasonal efficiency and a like-for-like efficiency comparison with the legacy boiler are unsupported: the trial boundary differs from the boiler’s stated basis, and no common-boundary reanalysis is provided. (Facts 1–3)

## heat-r1-B2 / conditions — belangrijk — correct

Bron [L5](cases/heat/source.md):

> Trial shifts processed lighter products and ambient temperature was 8 degrees higher than in the boiler reference shifts.

[content.md](calls/heat-r1-B2-Research/content.md)

- L4: 3. Trial shifts processed lighter products, had ambient temperature 8 degrees higher than boiler-reference shifts, and were selected to avoid peak production. There was no random assignment or matched control; the record says it cannot isolate a storage-technology effect or quantify avoided emissions, and grid-carbon intensity was not measured. (Performance and measurement)

[projection.md](calls/heat-r1-B2-Research/projection.md)

- L5: 3. Trial shifts processed lighter products, had ambient temperature 8 degrees higher than boiler-reference shifts, and were selected to avoid peak production. There was no random assignment or matched control; the record says it cannot isolate a storage-technology effect or quantify avoided emissions, and grid-carbon intensity was not measured. (Performance and measurement)

## heat-r1-B2 / allocation — belangrijk — correct

Bron [L5](cases/heat/source.md):

> Operators selected the trial shifts to avoid peak production. There was no random assignment or matched control.

[content.md](calls/heat-r1-B2-Research/content.md)

- L4: 3. Trial shifts processed lighter products, had ambient temperature 8 degrees higher than boiler-reference shifts, and were selected to avoid peak production. There was no random assignment or matched control; the record says it cannot isolate a storage-technology effect or quantify avoided emissions, and grid-carbon intensity was not measured. (Performance and measurement)
- L21: - The storage technology’s causal contribution to performance, avoided emissions, and any emissions benefit remain unestablished; the record lacks a matched/randomized comparison and grid-carbon measurement. (Fact 3)

[projection.md](calls/heat-r1-B2-Research/projection.md)

- L5: 3. Trial shifts processed lighter products, had ambient temperature 8 degrees higher than boiler-reference shifts, and were selected to avoid peak production. There was no random assignment or matched control; the record says it cannot isolate a storage-technology effect or quantify avoided emissions, and grid-carbon intensity was not measured. (Performance and measurement)
- L19: - The storage technology’s causal contribution to performance, avoided emissions, and any emissions benefit remain unestablished; the record lacks a matched/randomized comparison and grid-carbon measurement. (Fact 3)

## heat-r1-B2 / contrast — kern — correct

Bron [L7](cases/heat/source.md):

> All four vessels passed the specified pressure-hold test. Two nevertheless failed the insulation surface-temperature check at the access hatch.

[content.md](calls/heat-r1-B2-Research/content.md)

- L5: 4. All four vessels passed the specified pressure-hold test. Two failed the insulation surface-temperature check at the access hatch; hot surfaces remained unresolved at sign-off, and a proposed shield was not installed. The source states that passing the pressure test does not demonstrate acceptable touch temperature. (Successful and failed controls)
- L11: 10. Procurement approved only a longer instrumented trial conditional on a verified hatch remedy; no purchase or factory-wide roll-out was approved. (Variability, sequencing and decision)
- L16: 1. The reported heat output and passing pressure-hold/interlock checks are limited positive results, not evidence that all quality checks passed or that the technology caused an improvement. (Facts 1, 3–5)
- L22: - Whether the failed hatch surface-temperature checks were later remedied is not established; at sign-off the hot surfaces were unresolved and the proposed shield was uninstalled. (Fact 4)
- L31: - Procurement approved a longer instrumented trial only conditional on a verified hatch remedy; the source records no purchase or factory-wide roll-out. (Fact 10)

[projection.md](calls/heat-r1-B2-Research/projection.md)

- L6: 4. All four vessels passed the specified pressure-hold test. Two failed the insulation surface-temperature check at the access hatch; hot surfaces remained unresolved at sign-off, and a proposed shield was not installed. The source states that passing the pressure test does not demonstrate acceptable touch temperature. (Successful and failed controls)
- L12: 10. Procurement approved only a longer instrumented trial conditional on a verified hatch remedy; no purchase or factory-wide roll-out was approved. (Variability, sequencing and decision)
- L20: - Whether the failed hatch surface-temperature checks were later remedied is not established; at sign-off the hot surfaces were unresolved and the proposed shield was uninstalled. (Fact 4)

## heat-r1-B2 / open — kern — correct

Bron [L7](cases/heat/source.md):

> The hot surfaces remained unresolved at sign-off, and a proposed shield was not installed.

[content.md](calls/heat-r1-B2-Research/content.md)

- L5: 4. All four vessels passed the specified pressure-hold test. Two failed the insulation surface-temperature check at the access hatch; hot surfaces remained unresolved at sign-off, and a proposed shield was not installed. The source states that passing the pressure test does not demonstrate acceptable touch temperature. (Successful and failed controls)
- L22: - Whether the failed hatch surface-temperature checks were later remedied is not established; at sign-off the hot surfaces were unresolved and the proposed shield was uninstalled. (Fact 4)

[projection.md](calls/heat-r1-B2-Research/projection.md)

- L6: 4. All four vessels passed the specified pressure-hold test. Two failed the insulation surface-temperature check at the access hatch; hot surfaces remained unresolved at sign-off, and a proposed shield was not installed. The source states that passing the pressure test does not demonstrate acceptable touch temperature. (Successful and failed controls)
- L20: - Whether the failed hatch surface-temperature checks were later remedied is not established; at sign-off the hot surfaces were unresolved and the proposed shield was uninstalled. (Fact 4)

## heat-r1-B2 / interlock — belangrijk — correct

Bron [L7](cases/heat/source.md):

> An interlock test passed for all four vessels; it did not test the hot-surface problem.

[content.md](calls/heat-r1-B2-Research/content.md)

- L6: 5. An interlock test passed for all four vessels, but did not test the hot-surface problem. (Successful and failed controls)
- L16: 1. The reported heat output and passing pressure-hold/interlock checks are limited positive results, not evidence that all quality checks passed or that the technology caused an improvement. (Facts 1, 3–5)

[projection.md](calls/heat-r1-B2-Research/projection.md)

- L7: 5. An interlock test passed for all four vessels, but did not test the hot-surface problem. (Successful and failed controls)

## heat-r1-B2 / term — kern — correct

Bron [L8](cases/heat/source.md):

> Three thermal trips occurred during initial charging. Here a thermal trip is a protective over-temperature shutdown, not a production journey or heat-delivery cycle.

[content.md](calls/heat-r1-B2-Research/content.md)

- L7: 6. Three thermal trips occurred during initial charging. The source defines a thermal trip as a protective over-temperature shutdown, not a production journey or heat-delivery cycle. (Successful and failed controls)
- L30: - Initial charging produced three thermal trips; the controller setting and sensor mounting were then changed together before a repeat charge with zero trips. (Facts 6–7)

[projection.md](calls/heat-r1-B2-Research/projection.md)

- L8: 6. Three thermal trips occurred during initial charging. The source defines a thermal trip as a protective over-temperature shutdown, not a production journey or heat-delivery cycle. (Successful and failed controls)

## heat-r1-B2 / attribution — kern — correct

Bron [L8](cases/heat/source.md):

> A controller setting and sensor mounting were changed together before a repeat charge with zero trips. The record cannot separate the two actions' contributions.

[content.md](calls/heat-r1-B2-Research/content.md)

- L8: 7. A controller setting and sensor mounting were changed together before a repeat charge, which had zero trips. The record cannot separate the two actions’ contributions; one trip-free repeat does not establish long-term reliability. (Successful and failed controls)
- L23: - The separate contributions of the controller-setting change and sensor-mounting change, and long-term trip reliability, are not established. (Fact 7)
- L30: - Initial charging produced three thermal trips; the controller setting and sensor mounting were then changed together before a repeat charge with zero trips. (Facts 6–7)

[projection.md](calls/heat-r1-B2-Research/projection.md)

- L9: 7. A controller setting and sensor mounting were changed together before a repeat charge, which had zero trips. The record cannot separate the two actions’ contributions; one trip-free repeat does not establish long-term reliability. (Successful and failed controls)
- L21: - The separate contributions of the controller-setting change and sensor-mounting change, and long-term trip reliability, are not established. (Fact 7)

## heat-r1-B2 / variation — kern — correct

Bron [L10](cases/heat/source.md):

> Three identical-input simulation runs estimated usable discharge at 402, 418 and 407 kWh; the solver used stochastic initialisation.

[content.md](calls/heat-r1-B2-Research/content.md)

- L9: 8. Three identical-input simulation runs estimated usable discharge at 402, 418 and 407 kWh. The solver used stochastic initialisation; these were simulations, not additional metered field trials, and do not supersede the measured 410 kWh. The source identifies solver variation as an attribution limit for interpreting simulation improvements. (Variability, sequencing and decision)

[projection.md](calls/heat-r1-B2-Research/projection.md)

- L10: 8. Three identical-input simulation runs estimated usable discharge at 402, 418 and 407 kWh. The solver used stochastic initialisation; these were simulations, not additional metered field trials, and do not supersede the measured 410 kWh. The source identifies solver variation as an attribution limit for interpreting simulation improvements. (Variability, sequencing and decision)

## heat-r1-B2 / cost — belangrijk — correct

Bron [L11](cases/heat/source.md):

> The finance appendix reports a EUR 9,200 rental allowance. It gives no date or sequence relative to testing. The allowance excludes installation, downtime and maintenance, so no payback claim is supported.

[content.md](calls/heat-r1-B2-Research/content.md)

- L10: 9. The finance appendix reports a EUR 9,200 rental allowance but gives no date or sequence relative to testing. It excludes installation, downtime and maintenance, so the source supports no payback claim. (Variability, sequencing and decision)
- L24: - The rental allowance’s timing relative to testing is unspecified, and the incomplete cost allowance cannot establish payback. (Fact 9)

[projection.md](calls/heat-r1-B2-Research/projection.md)

- L11: 9. The finance appendix reports a EUR 9,200 rental allowance but gives no date or sequence relative to testing. It excludes installation, downtime and maintenance, so the source supports no payback claim. (Variability, sequencing and decision)
- L22: - The rental allowance’s timing relative to testing is unspecified, and the incomplete cost allowance cannot establish payback. (Fact 9)

## heat-r1-B2 / decision — kern — correct

Bron [L11](cases/heat/source.md):

> Procurement approved only a longer instrumented trial conditional on a verified hatch remedy; no purchase or factory-wide roll-out was approved.

[content.md](calls/heat-r1-B2-Research/content.md)

- L11: 10. Procurement approved only a longer instrumented trial conditional on a verified hatch remedy; no purchase or factory-wide roll-out was approved. (Variability, sequencing and decision)
- L31: - Procurement approved a longer instrumented trial only conditional on a verified hatch remedy; the source records no purchase or factory-wide roll-out. (Fact 10)

[projection.md](calls/heat-r1-B2-Research/projection.md)

- L12: 10. Procurement approved only a longer instrumented trial conditional on a verified hatch remedy; no purchase or factory-wide roll-out was approved. (Variability, sequencing and decision)

## heat-r1-A / ledger — kern — correct

Bron [L4](cases/heat/source.md):

> The final metered record shows 410 kWh of useful heat from 500 kWh electrical input over the trial boundary.

[content.md](calls/heat-r1-A-Research/content.md)

- L5: 3. The final metered record shows 410 kWh of useful heat from 500 kWh of electrical input over the trial boundary.
- L8: 6. The 410/500 ratio concerns the trial boundary and period, not seasonal efficiency.
- L35: 33. The simulations do not supersede the measured 410 kWh.
- L64: - The two-shift factory trial produced a final metered record of 410 kWh useful heat from 500 kWh electrical input over the trial boundary (Facts 1, 3).

[projection.md](calls/heat-r1-A-Research/projection.md)

- L5: 3. The final metered record shows 410 kWh of useful heat from 500 kWh of electrical input over the trial boundary.
- L8: 6. The 410/500 ratio concerns the trial boundary and period, not seasonal efficiency.
- L35: 33. The simulations do not supersede the measured 410 kWh.

## heat-r1-A / method — kern — correct

Bron [L4](cases/heat/source.md):

> The legacy boiler's displayed 76% used fuel energy on a lower-heating-value basis, while the electrical trial used meter input and included pump demand. No common-boundary reanalysis exists.

[content.md](calls/heat-r1-A-Research/content.md)

- L9: 7. The legacy boiler's displayed 76% used fuel energy on a lower-heating-value basis.
- L10: 8. The electrical trial used meter input and included pump demand.
- L11: 9. No common-boundary reanalysis exists.
- L51: - The source does not establish a seasonal-efficiency result or a comparison based on a common energy boundary; it states that the ratio is limited to the trial boundary and period and that no common-boundary reanalysis exists (Facts 6–9).

[projection.md](calls/heat-r1-A-Research/projection.md)

- L9: 7. The legacy boiler's displayed 76% used fuel energy on a lower-heating-value basis.
- L10: 8. The electrical trial used meter input and included pump demand.
- L11: 9. No common-boundary reanalysis exists.
- L47: - The source does not establish a seasonal-efficiency result or a comparison based on a common energy boundary; it states that the ratio is limited to the trial boundary and period and that no common-boundary reanalysis exists (Facts 6–9).

## heat-r1-A / conditions — belangrijk — correct

Bron [L5](cases/heat/source.md):

> Trial shifts processed lighter products and ambient temperature was 8 degrees higher than in the boiler reference shifts.

[content.md](calls/heat-r1-A-Research/content.md)

- L12: 10. The trial shifts processed lighter products than the boiler reference shifts.
- L13: 11. Ambient temperature was 8 degrees higher during the trial shifts than during the boiler reference shifts.
- L66: - In the boiler-reference comparison, the trial shifts processed lighter products and had ambient temperature 8 degrees higher; operators selected the trial shifts to avoid peak production. There was no random assignment or matched control (Facts 7–13). The source does not establish a causal relationship between these conditions and the measured result.

[projection.md](calls/heat-r1-A-Research/projection.md)

- L12: 10. The trial shifts processed lighter products than the boiler reference shifts.
- L13: 11. Ambient temperature was 8 degrees higher during the trial shifts than during the boiler reference shifts.

## heat-r1-A / allocation — belangrijk — correct

Bron [L5](cases/heat/source.md):

> Operators selected the trial shifts to avoid peak production. There was no random assignment or matched control.

[content.md](calls/heat-r1-A-Research/content.md)

- L14: 12. Operators selected the trial shifts to avoid peak production.
- L15: 13. There was no random assignment or matched control.
- L66: - In the boiler-reference comparison, the trial shifts processed lighter products and had ambient temperature 8 degrees higher; operators selected the trial shifts to avoid peak production. There was no random assignment or matched control (Facts 7–13). The source does not establish a causal relationship between these conditions and the measured result.

[projection.md](calls/heat-r1-A-Research/projection.md)

- L14: 12. Operators selected the trial shifts to avoid peak production.
- L15: 13. There was no random assignment or matched control.

## heat-r1-A / contrast — kern — correct

Bron [L7](cases/heat/source.md):

> All four vessels passed the specified pressure-hold test. Two nevertheless failed the insulation surface-temperature check at the access hatch.

[content.md](calls/heat-r1-A-Research/content.md)

- L19: 17. All four vessels passed the specified pressure-hold test.
- L20: 18. Two vessels failed the insulation surface-temperature check at the access hatch.
- L41: 39. Procurement approved only a longer instrumented trial conditional on a verified hatch remedy.
- L53: - The source does not establish acceptable touch temperature for the two vessels that failed the insulation surface-temperature check; the hot surfaces remained unresolved at sign-off and the proposed shield was not installed (Facts 18–21).
- L58: - The source does not establish why procurement approved the longer instrumented trial beyond the stated condition of a verified hatch remedy, or that a purchase or factory-wide roll-out was approved (Facts 39–40).
- L67: - All four vessels passed the specified pressure-hold test; two failed the access-hatch insulation surface-temperature check. The hot surfaces remained unresolved at sign-off, and a proposed shield was not installed (Facts 17–20).
- L72: - Procurement approved a longer instrumented trial conditional on a verified hatch remedy; no purchase or factory-wide roll-out was approved (Facts 39–40). The source does not establish when this approval occurred relative to the other listed events.
- L77: - Facts 17–23 are relevant to the pressure-hold and interlock test results and the failed surface-temperature check.

[projection.md](calls/heat-r1-A-Research/projection.md)

- L19: 17. All four vessels passed the specified pressure-hold test.
- L20: 18. Two vessels failed the insulation surface-temperature check at the access hatch.
- L41: 39. Procurement approved only a longer instrumented trial conditional on a verified hatch remedy.
- L49: - The source does not establish acceptable touch temperature for the two vessels that failed the insulation surface-temperature check; the hot surfaces remained unresolved at sign-off and the proposed shield was not installed (Facts 18–21).
- L54: - The source does not establish why procurement approved the longer instrumented trial beyond the stated condition of a verified hatch remedy, or that a purchase or factory-wide roll-out was approved (Facts 39–40).

## heat-r1-A / open — kern — correct

Bron [L7](cases/heat/source.md):

> The hot surfaces remained unresolved at sign-off, and a proposed shield was not installed.

[content.md](calls/heat-r1-A-Research/content.md)

- L21: 19. The hot surfaces remained unresolved at sign-off.
- L22: 20. A proposed shield was not installed.
- L53: - The source does not establish acceptable touch temperature for the two vessels that failed the insulation surface-temperature check; the hot surfaces remained unresolved at sign-off and the proposed shield was not installed (Facts 18–21).
- L67: - All four vessels passed the specified pressure-hold test; two failed the access-hatch insulation surface-temperature check. The hot surfaces remained unresolved at sign-off, and a proposed shield was not installed (Facts 17–20).

[projection.md](calls/heat-r1-A-Research/projection.md)

- L21: 19. The hot surfaces remained unresolved at sign-off.
- L22: 20. A proposed shield was not installed.
- L49: - The source does not establish acceptable touch temperature for the two vessels that failed the insulation surface-temperature check; the hot surfaces remained unresolved at sign-off and the proposed shield was not installed (Facts 18–21).

## heat-r1-A / interlock — belangrijk — correct

Bron [L7](cases/heat/source.md):

> An interlock test passed for all four vessels; it did not test the hot-surface problem.

[content.md](calls/heat-r1-A-Research/content.md)

- L24: 22. The interlock test passed for all four vessels.
- L25: 23. The interlock test did not test the hot-surface problem.
- L77: - Facts 17–23 are relevant to the pressure-hold and interlock test results and the failed surface-temperature check.

[projection.md](calls/heat-r1-A-Research/projection.md)

- L24: 22. The interlock test passed for all four vessels.
- L25: 23. The interlock test did not test the hot-surface problem.

## heat-r1-A / term — kern — correct

Bron [L8](cases/heat/source.md):

> Three thermal trips occurred during initial charging. Here a thermal trip is a protective over-temperature shutdown, not a production journey or heat-delivery cycle.

[content.md](calls/heat-r1-A-Research/content.md)

- L26: 24. Three thermal trips occurred during initial charging.
- L27: 25. The memorandum defines a thermal trip as a protective over-temperature shutdown.
- L68: - Three thermal trips occurred during initial charging (Facts 24–25).

[projection.md](calls/heat-r1-A-Research/projection.md)

- L26: 24. Three thermal trips occurred during initial charging.
- L27: 25. The memorandum defines a thermal trip as a protective over-temperature shutdown.

## heat-r1-A / attribution — kern — correct

Bron [L8](cases/heat/source.md):

> A controller setting and sensor mounting were changed together before a repeat charge with zero trips. The record cannot separate the two actions' contributions.

[content.md](calls/heat-r1-A-Research/content.md)

- L28: 26. A controller setting and sensor mounting were changed together before a repeat charge.
- L29: 27. The repeat charge had zero trips.
- L30: 28. The record states that it cannot separate the contributions of the controller-setting and sensor-mounting changes.
- L31: 29. Zero trips in one repeat does not establish long-term reliability.
- L54: - The source does not establish the separate contributions of the controller-setting and sensor-mounting changes to the zero-trip repeat charge (Facts 26–29).
- L55: - The source does not establish long-term reliability from the one repeat charge with zero trips (Facts 27–29).
- L69: - A controller setting and sensor mounting were changed together before a repeat charge, which had zero trips (Facts 26–28). The source says their separate contributions cannot be determined.

[projection.md](calls/heat-r1-A-Research/projection.md)

- L28: 26. A controller setting and sensor mounting were changed together before a repeat charge.
- L29: 27. The repeat charge had zero trips.
- L30: 28. The record states that it cannot separate the contributions of the controller-setting and sensor-mounting changes.
- L31: 29. Zero trips in one repeat does not establish long-term reliability.
- L50: - The source does not establish the separate contributions of the controller-setting and sensor-mounting changes to the zero-trip repeat charge (Facts 26–29).
- L51: - The source does not establish long-term reliability from the one repeat charge with zero trips (Facts 27–29).

## heat-r1-A / variation — kern — correct

Bron [L10](cases/heat/source.md):

> Three identical-input simulation runs estimated usable discharge at 402, 418 and 407 kWh; the solver used stochastic initialisation.

[content.md](calls/heat-r1-A-Research/content.md)

- L32: 30. Three identical-input simulation runs estimated usable discharge at 402, 418, and 407 kWh.
- L33: 31. The solver used stochastic initialisation.
- L34: 32. The simulations are not three additional metered field trials.
- L35: 33. The simulations do not supersede the measured 410 kWh.
- L36: 34. Solver variation is an attribution limit for interpreting simulation improvements.
- L56: - The source does not establish that the simulations are additional metered field results or that they replace the measured result (Facts 30–34).
- L70: - Three identical-input simulation runs estimated usable discharge at 402, 418, and 407 kWh (Facts 30–34). Their sequence relative to the other trial events is not established.
- L79: - Facts 30–34 are relevant to the simulation results and their limits relative to the metered result.

[projection.md](calls/heat-r1-A-Research/projection.md)

- L32: 30. Three identical-input simulation runs estimated usable discharge at 402, 418, and 407 kWh.
- L33: 31. The solver used stochastic initialisation.
- L34: 32. The simulations are not three additional metered field trials.
- L35: 33. The simulations do not supersede the measured 410 kWh.
- L36: 34. Solver variation is an attribution limit for interpreting simulation improvements.
- L52: - The source does not establish that the simulations are additional metered field results or that they replace the measured result (Facts 30–34).

## heat-r1-A / cost — belangrijk — correct

Bron [L11](cases/heat/source.md):

> The finance appendix reports a EUR 9,200 rental allowance. It gives no date or sequence relative to testing. The allowance excludes installation, downtime and maintenance, so no payback claim is supported.

[content.md](calls/heat-r1-A-Research/content.md)

- L37: 35. The finance appendix reports a EUR 9,200 rental allowance.
- L38: 36. The finance appendix gives no date or sequence for the rental allowance relative to testing.
- L39: 37. The rental allowance excludes installation, downtime, and maintenance.
- L40: 38. The memorandum states that no payback claim is supported.
- L57: - The source does not establish a payback claim from the rental allowance (Facts 35–38).
- L71: - The finance appendix reports a EUR 9,200 rental allowance but gives no date or sequence relative to testing (Facts 35–36).
- L80: - Facts 35–40 are relevant to the rental allowance, unsupported payback claim, and procurement decision.

[projection.md](calls/heat-r1-A-Research/projection.md)

- L37: 35. The finance appendix reports a EUR 9,200 rental allowance.
- L38: 36. The finance appendix gives no date or sequence for the rental allowance relative to testing.
- L39: 37. The rental allowance excludes installation, downtime, and maintenance.
- L40: 38. The memorandum states that no payback claim is supported.
- L53: - The source does not establish a payback claim from the rental allowance (Facts 35–38).

## heat-r1-A / decision — kern — correct

Bron [L11](cases/heat/source.md):

> Procurement approved only a longer instrumented trial conditional on a verified hatch remedy; no purchase or factory-wide roll-out was approved.

[content.md](calls/heat-r1-A-Research/content.md)

- L41: 39. Procurement approved only a longer instrumented trial conditional on a verified hatch remedy.
- L42: 40. No purchase or factory-wide roll-out was approved.
- L58: - The source does not establish why procurement approved the longer instrumented trial beyond the stated condition of a verified hatch remedy, or that a purchase or factory-wide roll-out was approved (Facts 39–40).
- L72: - Procurement approved a longer instrumented trial conditional on a verified hatch remedy; no purchase or factory-wide roll-out was approved (Facts 39–40). The source does not establish when this approval occurred relative to the other listed events.
- L80: - Facts 35–40 are relevant to the rental allowance, unsupported payback claim, and procurement decision.

[projection.md](calls/heat-r1-A-Research/projection.md)

- L41: 39. Procurement approved only a longer instrumented trial conditional on a verified hatch remedy.
- L42: 40. No purchase or factory-wide roll-out was approved.
- L54: - The source does not establish why procurement approved the longer instrumented trial beyond the stated condition of a verified hatch remedy, or that a purchase or factory-wide roll-out was approved (Facts 39–40).

## heat-r1-B / ledger — kern — correct

Bron [L4](cases/heat/source.md):

> The final metered record shows 410 kWh of useful heat from 500 kWh electrical input over the trial boundary.

[content.md](calls/heat-r1-B-Research/content.md)

- L3: 2. The final metered record reports 410 kWh useful heat from 500 kWh electrical input. A draft listed 450 kWh; 40 kWh duplicated during meter-file concatenation was removed. (“final metered record”; “duplicated”)
- L4: 3. The 410/500 ratio applies only to this trial boundary and period, not seasonal efficiency. The legacy boiler’s displayed 76% used fuel energy on a lower-heating-value basis; the electrical trial used meter input and included pump demand. No common-boundary reanalysis exists. (“Performance and measurement”)
- L10: 9. Three identical-input simulation runs estimated usable discharge at 402, 418 and 407 kWh; the solver used stochastic initialisation. The source says these are not additional metered field trials and do not supersede the measured 410 kWh; solver variation limits attribution of simulation improvements. (“Variability, sequencing and decision”)
- L28: - A draft metered figure of 450 kWh was corrected by removing 40 kWh of duplicated data; the final record reports 410 kWh from 500 kWh electrical input (Fact 2).

[projection.md](calls/heat-r1-B-Research/projection.md)

- L4: 2. The final metered record reports 410 kWh useful heat from 500 kWh electrical input. A draft listed 450 kWh; 40 kWh duplicated during meter-file concatenation was removed. (“final metered record”; “duplicated”)
- L5: 3. The 410/500 ratio applies only to this trial boundary and period, not seasonal efficiency. The legacy boiler’s displayed 76% used fuel energy on a lower-heating-value basis; the electrical trial used meter input and included pump demand. No common-boundary reanalysis exists. (“Performance and measurement”)
- L11: 9. Three identical-input simulation runs estimated usable discharge at 402, 418 and 407 kWh; the solver used stochastic initialisation. The source says these are not additional metered field trials and do not supersede the measured 410 kWh; solver variation limits attribution of simulation improvements. (“Variability, sequencing and decision”)

## heat-r1-B / method — kern — correct

Bron [L4](cases/heat/source.md):

> The legacy boiler's displayed 76% used fuel energy on a lower-heating-value basis, while the electrical trial used meter input and included pump demand. No common-boundary reanalysis exists.

[content.md](calls/heat-r1-B-Research/content.md)

- L4: 3. The 410/500 ratio applies only to this trial boundary and period, not seasonal efficiency. The legacy boiler’s displayed 76% used fuel energy on a lower-heating-value basis; the electrical trial used meter input and included pump demand. No common-boundary reanalysis exists. (“Performance and measurement”)
- L19: - Seasonal efficiency and a like-for-like efficiency comparison with the legacy boiler are not established: the trial ratio is boundary- and period-specific, and there is no common-boundary reanalysis (Facts 2–4).

[projection.md](calls/heat-r1-B-Research/projection.md)

- L5: 3. The 410/500 ratio applies only to this trial boundary and period, not seasonal efficiency. The legacy boiler’s displayed 76% used fuel energy on a lower-heating-value basis; the electrical trial used meter input and included pump demand. No common-boundary reanalysis exists. (“Performance and measurement”)
- L18: - Seasonal efficiency and a like-for-like efficiency comparison with the legacy boiler are not established: the trial ratio is boundary- and period-specific, and there is no common-boundary reanalysis (Facts 2–4).

## heat-r1-B / conditions — belangrijk — correct

Bron [L5](cases/heat/source.md):

> Trial shifts processed lighter products and ambient temperature was 8 degrees higher than in the boiler reference shifts.

[content.md](calls/heat-r1-B-Research/content.md)

- L5: 4. Trial shifts processed lighter products, had ambient temperatures 8 degrees higher than the boiler-reference shifts, and were selected to avoid peak production. There was no random assignment or matched control; the record says it cannot isolate a storage-technology effect or quantify avoided emissions. Grid-carbon intensity was not measured. (“Performance and measurement”)

[projection.md](calls/heat-r1-B-Research/projection.md)

- L6: 4. Trial shifts processed lighter products, had ambient temperatures 8 degrees higher than the boiler-reference shifts, and were selected to avoid peak production. There was no random assignment or matched control; the record says it cannot isolate a storage-technology effect or quantify avoided emissions. Grid-carbon intensity was not measured. (“Performance and measurement”)

## heat-r1-B / allocation — belangrijk — correct

Bron [L5](cases/heat/source.md):

> Operators selected the trial shifts to avoid peak production. There was no random assignment or matched control.

[content.md](calls/heat-r1-B-Research/content.md)

- L5: 4. Trial shifts processed lighter products, had ambient temperatures 8 degrees higher than the boiler-reference shifts, and were selected to avoid peak production. There was no random assignment or matched control; the record says it cannot isolate a storage-technology effect or quantify avoided emissions. Grid-carbon intensity was not measured. (“Performance and measurement”)
- L20: - The storage technology’s independent effect, avoided emissions, and emissions performance are unestablished; the shifts were not matched or randomized, grid-carbon intensity was not measured, and the supplier’s phrase is not a measured conclusion (Facts 4, 12).

[projection.md](calls/heat-r1-B-Research/projection.md)

- L6: 4. Trial shifts processed lighter products, had ambient temperatures 8 degrees higher than the boiler-reference shifts, and were selected to avoid peak production. There was no random assignment or matched control; the record says it cannot isolate a storage-technology effect or quantify avoided emissions. Grid-carbon intensity was not measured. (“Performance and measurement”)
- L19: - The storage technology’s independent effect, avoided emissions, and emissions performance are unestablished; the shifts were not matched or randomized, grid-carbon intensity was not measured, and the supplier’s phrase is not a measured conclusion (Facts 4, 12).

## heat-r1-B / contrast — kern — correct

Bron [L7](cases/heat/source.md):

> All four vessels passed the specified pressure-hold test. Two nevertheless failed the insulation surface-temperature check at the access hatch.

[content.md](calls/heat-r1-B-Research/content.md)

- L6: 5. All four vessels passed the specified pressure-hold test, but two failed the insulation surface-temperature check at the access hatch. The hot surfaces remained unresolved at sign-off, and a proposed shield was not installed. The source states that passing the pressure test does not demonstrate acceptable touch temperature. (“Successful and failed controls”)
- L12: 11. Procurement approved only a longer instrumented trial conditional on a verified hatch remedy; no purchase or factory-wide roll-out was approved. (“Variability, sequencing and decision”)
- L30: - Procurement’s recorded decision was limited to a longer instrumented trial conditional on a verified hatch remedy; no purchase or factory-wide roll-out was approved (Fact 11).

[projection.md](calls/heat-r1-B-Research/projection.md)

- L7: 5. All four vessels passed the specified pressure-hold test, but two failed the insulation surface-temperature check at the access hatch. The hot surfaces remained unresolved at sign-off, and a proposed shield was not installed. The source states that passing the pressure test does not demonstrate acceptable touch temperature. (“Successful and failed controls”)
- L13: 11. Procurement approved only a longer instrumented trial conditional on a verified hatch remedy; no purchase or factory-wide roll-out was approved. (“Variability, sequencing and decision”)

## heat-r1-B / open — kern — correct

Bron [L7](cases/heat/source.md):

> The hot surfaces remained unresolved at sign-off, and a proposed shield was not installed.

[content.md](calls/heat-r1-B-Research/content.md)

- L6: 5. All four vessels passed the specified pressure-hold test, but two failed the insulation surface-temperature check at the access hatch. The hot surfaces remained unresolved at sign-off, and a proposed shield was not installed. The source states that passing the pressure test does not demonstrate acceptable touch temperature. (“Successful and failed controls”)
- L21: - Whether the hot-surface issue was remedied after sign-off, and whether any remedy was verified, is not established; the proposed shield was not installed at sign-off (Facts 5, 11).

[projection.md](calls/heat-r1-B-Research/projection.md)

- L7: 5. All four vessels passed the specified pressure-hold test, but two failed the insulation surface-temperature check at the access hatch. The hot surfaces remained unresolved at sign-off, and a proposed shield was not installed. The source states that passing the pressure test does not demonstrate acceptable touch temperature. (“Successful and failed controls”)
- L20: - Whether the hot-surface issue was remedied after sign-off, and whether any remedy was verified, is not established; the proposed shield was not installed at sign-off (Facts 5, 11).

## heat-r1-B / interlock — belangrijk — correct

Bron [L7](cases/heat/source.md):

> An interlock test passed for all four vessels; it did not test the hot-surface problem.

[content.md](calls/heat-r1-B-Research/content.md)

- L7: 6. All four vessels passed an interlock test; that test did not test the hot-surface problem. (“Successful and failed controls”)

[projection.md](calls/heat-r1-B-Research/projection.md)

- L8: 6. All four vessels passed an interlock test; that test did not test the hot-surface problem. (“Successful and failed controls”)

## heat-r1-B / term — kern — correct

Bron [L8](cases/heat/source.md):

> Three thermal trips occurred during initial charging. Here a thermal trip is a protective over-temperature shutdown, not a production journey or heat-delivery cycle.

[content.md](calls/heat-r1-B-Research/content.md)

- L8: 7. Three thermal trips occurred during initial charging. The source defines a thermal trip as a protective over-temperature shutdown, not a production journey or heat-delivery cycle. (“Successful and failed controls”)
- L29: - Initial charging produced three protective thermal trips; the controller setting and sensor mounting were then both changed before one repeat charge, which recorded zero trips (Facts 7–8).

[projection.md](calls/heat-r1-B-Research/projection.md)

- L9: 7. Three thermal trips occurred during initial charging. The source defines a thermal trip as a protective over-temperature shutdown, not a production journey or heat-delivery cycle. (“Successful and failed controls”)

## heat-r1-B / attribution — kern — correct

Bron [L8](cases/heat/source.md):

> A controller setting and sensor mounting were changed together before a repeat charge with zero trips. The record cannot separate the two actions' contributions.

[content.md](calls/heat-r1-B-Research/content.md)

- L9: 8. Before a repeat charge with zero trips, both a controller setting and sensor mounting were changed. The record cannot separate their contributions, and says zero trips in one repeat does not establish long-term reliability. (“Successful and failed controls”)
- L22: - The relative contribution of the controller change and sensor-mounting change, and long-term trip reliability, are unestablished (Fact 8).
- L29: - Initial charging produced three protective thermal trips; the controller setting and sensor mounting were then both changed before one repeat charge, which recorded zero trips (Facts 7–8).

[projection.md](calls/heat-r1-B-Research/projection.md)

- L10: 8. Before a repeat charge with zero trips, both a controller setting and sensor mounting were changed. The record cannot separate their contributions, and says zero trips in one repeat does not establish long-term reliability. (“Successful and failed controls”)
- L21: - The relative contribution of the controller change and sensor-mounting change, and long-term trip reliability, are unestablished (Fact 8).

## heat-r1-B / variation — kern — correct

Bron [L10](cases/heat/source.md):

> Three identical-input simulation runs estimated usable discharge at 402, 418 and 407 kWh; the solver used stochastic initialisation.

[content.md](calls/heat-r1-B-Research/content.md)

- L10: 9. Three identical-input simulation runs estimated usable discharge at 402, 418 and 407 kWh; the solver used stochastic initialisation. The source says these are not additional metered field trials and do not supersede the measured 410 kWh; solver variation limits attribution of simulation improvements. (“Variability, sequencing and decision”)
- L23: - The simulations do not establish additional field performance or explain an improvement independently of solver variation (Fact 9).

[projection.md](calls/heat-r1-B-Research/projection.md)

- L11: 9. Three identical-input simulation runs estimated usable discharge at 402, 418 and 407 kWh; the solver used stochastic initialisation. The source says these are not additional metered field trials and do not supersede the measured 410 kWh; solver variation limits attribution of simulation improvements. (“Variability, sequencing and decision”)
- L22: - The simulations do not establish additional field performance or explain an improvement independently of solver variation (Fact 9).

## heat-r1-B / cost — belangrijk — correct

Bron [L11](cases/heat/source.md):

> The finance appendix reports a EUR 9,200 rental allowance. It gives no date or sequence relative to testing. The allowance excludes installation, downtime and maintenance, so no payback claim is supported.

[content.md](calls/heat-r1-B-Research/content.md)

- L11: 10. The finance appendix reports a EUR 9,200 rental allowance but gives no date or sequence relative to testing. It excludes installation, downtime and maintenance; the source says no payback claim is supported. (“Variability, sequencing and decision”)
- L24: - Payback, and the rental allowance’s timing relative to testing, are unsupported by the appendix (Fact 10).

[projection.md](calls/heat-r1-B-Research/projection.md)

- L12: 10. The finance appendix reports a EUR 9,200 rental allowance but gives no date or sequence relative to testing. It excludes installation, downtime and maintenance; the source says no payback claim is supported. (“Variability, sequencing and decision”)
- L23: - Payback, and the rental allowance’s timing relative to testing, are unsupported by the appendix (Fact 10).

## heat-r1-B / decision — kern — correct

Bron [L11](cases/heat/source.md):

> Procurement approved only a longer instrumented trial conditional on a verified hatch remedy; no purchase or factory-wide roll-out was approved.

[content.md](calls/heat-r1-B-Research/content.md)

- L12: 11. Procurement approved only a longer instrumented trial conditional on a verified hatch remedy; no purchase or factory-wide roll-out was approved. (“Variability, sequencing and decision”)
- L30: - Procurement’s recorded decision was limited to a longer instrumented trial conditional on a verified hatch remedy; no purchase or factory-wide roll-out was approved (Fact 11).

[projection.md](calls/heat-r1-B-Research/projection.md)

- L13: 11. Procurement approved only a longer instrumented trial conditional on a verified hatch remedy; no purchase or factory-wide roll-out was approved. (“Variability, sequencing and decision”)

## water-r2-B / ledger — kern — correct

Bron [L4](cases/water/source.md):

> The legacy fortnight recorded 46 low-pressure alerts in 1,400 logger-hours; the pilot fortnight recorded 29 in 1,120 logger-hours.

[content.md](calls/water-r2-B-Research/content.md)

- L8: 7. The final ledger records 46 low-pressure alerts in 1,400 logger-hours for the legacy fortnight and 29 in 1,120 logger-hours for the pilot fortnight. These are alerts, not confirmed leaks or affected households. A presentation had reported 25 pilot alerts; the final ledger restored four suppressed duplicates that were distinct device events, and the source specifies using 29. (“Recorded outcomes”)
- L12: 11. Three replays of the same stored trace emitted 29, 31 and 28 alerts, with a nondeterministic deduplication tie-breaker enabled. The team did not select a replay as the true count; replay counts do not replace the final field ledger of 29. The source says repeat variability and changed thresholds limit attribution. (“Repeats and decision”)
- L27: - The legacy fortnight recorded 46 alerts; the pilot fortnight recorded 29 in the final ledger, under changed thresholds and differing conditions (Facts 7–10).

[projection.md](calls/water-r2-B-Research/projection.md)

- L9: 7. The final ledger records 46 low-pressure alerts in 1,400 logger-hours for the legacy fortnight and 29 in 1,120 logger-hours for the pilot fortnight. These are alerts, not confirmed leaks or affected households. A presentation had reported 25 pilot alerts; the final ledger restored four suppressed duplicates that were distinct device events, and the source specifies using 29. (“Recorded outcomes”)
- L13: 11. Three replays of the same stored trace emitted 29, 31 and 28 alerts, with a nondeterministic deduplication tie-breaker enabled. The team did not select a replay as the true count; replay counts do not replace the final field ledger of 29. The source says repeat variability and changed thresholds limit attribution. (“Repeats and decision”)

## water-r2-B / method — kern — correct

Bron [L5](cases/water/source.md):

> The legacy alert threshold was 1.8 bar for 30 seconds; the pilot used 1.6 bar for 60 seconds.

[content.md](calls/water-r2-B-Research/content.md)

- L9: 8. The legacy threshold was 1.8 bar for 30 seconds; the pilot threshold was 1.6 bar for 60 seconds. Old traces were not reprocessed. (“Recorded outcomes”)
- L21: - The source does not establish the upgrade’s causal effect on alerts: thresholds changed, legacy traces were not reprocessed, other conditions differed, there was no unaffected comparison branch, and no rate-adjusted treatment effect was estimated (Facts 8–10).

[projection.md](calls/water-r2-B-Research/projection.md)

- L10: 8. The legacy threshold was 1.8 bar for 30 seconds; the pilot threshold was 1.6 bar for 60 seconds. Old traces were not reprocessed. (“Recorded outcomes”)
- L19: - The source does not establish the upgrade’s causal effect on alerts: thresholds changed, legacy traces were not reprocessed, other conditions differed, there was no unaffected comparison branch, and no rate-adjusted treatment effect was estimated (Facts 8–10).

## water-r2-B / conditions — belangrijk — correct

Bron [L5](cases/water/source.md):

> A planned night flushing operation occurred during the legacy fortnight only.

[content.md](calls/water-r2-B-Research/content.md)

- L10: 9. Night flushing occurred during the legacy fortnight only. Branches were selected for ease of access, not randomly; rainfall differed, and no unaffected comparison branch was instrumented. The source says the lower alert count cannot isolate the sensor upgrade’s effect. (“Recorded outcomes”)

[projection.md](calls/water-r2-B-Research/projection.md)

- L11: 9. Night flushing occurred during the legacy fortnight only. Branches were selected for ease of access, not randomly; rainfall differed, and no unaffected comparison branch was instrumented. The source says the lower alert count cannot isolate the sensor upgrade’s effect. (“Recorded outcomes”)

## water-r2-B / allocation — belangrijk — correct

Bron [L5](cases/water/source.md):

> Branches were selected for ease of access, not randomly; rainfall also differed.

[content.md](calls/water-r2-B-Research/content.md)

- L10: 9. Night flushing occurred during the legacy fortnight only. Branches were selected for ease of access, not randomly; rainfall differed, and no unaffected comparison branch was instrumented. The source says the lower alert count cannot isolate the sensor upgrade’s effect. (“Recorded outcomes”)

[projection.md](calls/water-r2-B-Research/projection.md)

- L11: 9. Night flushing occurred during the legacy fortnight only. Branches were selected for ease of access, not randomly; rainfall differed, and no unaffected comparison branch was instrumented. The source says the lower alert count cannot isolate the sensor upgrade’s effect. (“Recorded outcomes”)

## water-r2-B / contrast — kern — correct

Bron [L7](cases/water/source.md):

> All twelve loggers passed the static reference-gauge calibration at 2.0 bar. Four nevertheless failed the overnight packet-delivery check, losing 17% of scheduled messages in that check.

[content.md](calls/water-r2-B-Research/content.md)

- L3: 2. All 12 loggers passed a static reference-gauge calibration at 2.0 bar. (“Checks and defects”)
- L4: 3. Four loggers failed the overnight packet-delivery check, losing 17% of scheduled messages in that check. The pressure readings that arrived were not shown to be inaccurate. (“Checks and defects”)
- L5: 4. The packet-delivery fault remained open; a proposed antenna move had not been tested. Battery replacement was completed on two different devices, but no date links that work to the delivery check; the source says not to treat it as repair of the packet fault. (“Checks and defects”)
- L17: - The successful static calibration establishes that specific check result, not a clean bill of operational reliability; packet delivery and field uptime have separate documented limitations (Facts 2–6).
- L32: Facts 2–6 establish the distinction between calibration, delivery faults and limited bench evidence; Facts 7–11 establish the alert comparison and attribution limits; Facts 12–13 establish the decision and measured-benefit limits. Interpretations 1–2 summarize those distinctions.

[projection.md](calls/water-r2-B-Research/projection.md)

- L4: 2. All 12 loggers passed a static reference-gauge calibration at 2.0 bar. (“Checks and defects”)
- L5: 3. Four loggers failed the overnight packet-delivery check, losing 17% of scheduled messages in that check. The pressure readings that arrived were not shown to be inaccurate. (“Checks and defects”)
- L6: 4. The packet-delivery fault remained open; a proposed antenna move had not been tested. Battery replacement was completed on two different devices, but no date links that work to the delivery check; the source says not to treat it as repair of the packet fault. (“Checks and defects”)

## water-r2-B / open — kern — correct

Bron [L7](cases/water/source.md):

> The delivery fault remained open at review; a proposed antenna move had not been tested.

[content.md](calls/water-r2-B-Research/content.md)

- L5: 4. The packet-delivery fault remained open; a proposed antenna move had not been tested. Battery replacement was completed on two different devices, but no date links that work to the delivery check; the source says not to treat it as repair of the packet fault. (“Checks and defects”)
- L22: - It is not established whether the packet fault was later corrected, whether the proposed antenna move would help, or whether the battery replacements related to that fault. Accuracy of readings that arrived is not established beyond the source’s statement that they were not shown to be inaccurate (Facts 3–4).

[projection.md](calls/water-r2-B-Research/projection.md)

- L6: 4. The packet-delivery fault remained open; a proposed antenna move had not been tested. Battery replacement was completed on two different devices, but no date links that work to the delivery check; the source says not to treat it as repair of the packet fault. (“Checks and defects”)
- L20: - It is not established whether the packet fault was later corrected, whether the proposed antenna move would help, or whether the battery replacements related to that fault. Accuracy of readings that arrived is not established beyond the source’s statement that they were not shown to be inaccurate (Facts 3–4).

## water-r2-B / chronology — belangrijk — correct

Bron [L7](cases/water/source.md):

> A battery replacement was completed on two different devices, but the dossier provides no date linking that work to the delivery check.

[content.md](calls/water-r2-B-Research/content.md)

- L5: 4. The packet-delivery fault remained open; a proposed antenna move had not been tested. Battery replacement was completed on two different devices, but no date links that work to the delivery check; the source says not to treat it as repair of the packet fault. (“Checks and defects”)
- L22: - It is not established whether the packet fault was later corrected, whether the proposed antenna move would help, or whether the battery replacements related to that fault. Accuracy of readings that arrived is not established beyond the source’s statement that they were not shown to be inaccurate (Facts 3–4).

[projection.md](calls/water-r2-B-Research/projection.md)

- L6: 4. The packet-delivery fault remained open; a proposed antenna move had not been tested. Battery replacement was completed on two different devices, but no date links that work to the delivery check; the source says not to treat it as repair of the packet fault. (“Checks and defects”)
- L20: - It is not established whether the packet fault was later corrected, whether the proposed antenna move would help, or whether the battery replacements related to that fault. Accuracy of readings that arrived is not established beyond the source’s statement that they were not shown to be inaccurate (Facts 3–4).

## water-r2-B / term — kern — correct

Bron [L8](cases/water/source.md):

> A bench check recorded three brownouts: transient supply-voltage drops causing device restarts, not a regional power outage.

[content.md](calls/water-r2-B-Research/content.md)

- L6: 5. A bench check recorded three brownouts, defined as transient supply-voltage drops causing device restarts, not a regional power outage. (“Checks and defects”)
- L7: 6. A capacitor swap and firmware update preceded a repeat bench check with no brownouts. Their separate contributions were not identified, and the bench result does not certify field uptime. (“Checks and defects”)
- L28: - The bench check recorded three brownouts; the capacitor swap and firmware update occurred before a repeat bench check with no brownouts (Facts 5–6).

[projection.md](calls/water-r2-B-Research/projection.md)

- L7: 5. A bench check recorded three brownouts, defined as transient supply-voltage drops causing device restarts, not a regional power outage. (“Checks and defects”)
- L8: 6. A capacitor swap and firmware update preceded a repeat bench check with no brownouts. Their separate contributions were not identified, and the bench result does not certify field uptime. (“Checks and defects”)

## water-r2-B / attribution — kern — correct

Bron [L8](cases/water/source.md):

> A capacitor swap and firmware update were both made before a repeat bench check with no brownouts. Their separate contributions were not identified; the bench result does not certify field uptime.

[content.md](calls/water-r2-B-Research/content.md)

- L7: 6. A capacitor swap and firmware update preceded a repeat bench check with no brownouts. Their separate contributions were not identified, and the bench result does not certify field uptime. (“Checks and defects”)
- L17: - The successful static calibration establishes that specific check result, not a clean bill of operational reliability; packet delivery and field uptime have separate documented limitations (Facts 2–6).
- L23: - The separate effects of the capacitor swap and firmware update, and field uptime after the bench changes, are not established (Fact 6).
- L28: - The bench check recorded three brownouts; the capacitor swap and firmware update occurred before a repeat bench check with no brownouts (Facts 5–6).

[projection.md](calls/water-r2-B-Research/projection.md)

- L8: 6. A capacitor swap and firmware update preceded a repeat bench check with no brownouts. Their separate contributions were not identified, and the bench result does not certify field uptime. (“Checks and defects”)
- L21: - The separate effects of the capacitor swap and firmware update, and field uptime after the bench changes, are not established (Fact 6).

## water-r2-B / variation — kern — correct

Bron [L10](cases/water/source.md):

> Three replays of the same stored trace emitted 29, 31 and 28 alerts; a nondeterministic deduplication tie-breaker was enabled.

[content.md](calls/water-r2-B-Research/content.md)

- L12: 11. Three replays of the same stored trace emitted 29, 31 and 28 alerts, with a nondeterministic deduplication tie-breaker enabled. The team did not select a replay as the true count; replay counts do not replace the final field ledger of 29. The source says repeat variability and changed thresholds limit attribution. (“Repeats and decision”)

[projection.md](calls/water-r2-B-Research/projection.md)

- L13: 11. Three replays of the same stored trace emitted 29, 31 and 28 alerts, with a nondeterministic deduplication tie-breaker enabled. The team did not select a replay as the true count; replay counts do not replace the final field ledger of 29. The source says repeat variability and changed thresholds limit attribution. (“Repeats and decision”)

## water-r2-B / decision — kern — correct

Bron [L11](cases/water/source.md):

> The committee authorized a six-week paired measurement study with unchanged thresholds. It did not authorize network-wide deployment.

[content.md](calls/water-r2-B-Research/content.md)

- L13: 12. The committee authorized a six-week paired measurement study with unchanged thresholds, but did not authorize network-wide deployment. (“Repeats and decision”)
- L24: - The source reports no measured safety, leak-prevention or annual reliability benefit, and no network-wide deployment authorization; it provides no outcome from the authorized six-week study (Facts 12–13).
- L29: - After reviewing the pilot, the committee authorized a six-week paired study with unchanged thresholds, not network-wide deployment (Fact 12).

[projection.md](calls/water-r2-B-Research/projection.md)

- L14: 12. The committee authorized a six-week paired measurement study with unchanged thresholds, but did not authorize network-wide deployment. (“Repeats and decision”)
- L22: - The source reports no measured safety, leak-prevention or annual reliability benefit, and no network-wide deployment authorization; it provides no outcome from the authorized six-week study (Facts 12–13).

## water-r2-B / cost — belangrijk — correct

Bron [L11](cases/water/source.md):

> The budget of EUR 18,000 covers logger rental and field visits, excludes staff overtime and does not establish savings.

[content.md](calls/water-r2-B-Research/content.md)

- L14: 13. The EUR 18,000 budget covers logger rental and field visits, excludes staff overtime, and does not establish savings. No safety, leak-prevention or annual reliability benefit was measured. (“Repeats and decision”)

[projection.md](calls/water-r2-B-Research/projection.md)

- L15: 13. The EUR 18,000 budget covers logger rental and field visits, excludes staff overtime, and does not establish savings. No safety, leak-prevention or annual reliability benefit was measured. (“Repeats and decision”)

## water-r2-B2 / ledger — kern — correct

Bron [L4](cases/water/source.md):

> The legacy fortnight recorded 46 low-pressure alerts in 1,400 logger-hours; the pilot fortnight recorded 29 in 1,120 logger-hours.

[content.md](calls/water-r2-B2-Research/content.md)

- L3: 2. The final ledger records 46 low-pressure alerts in 1,400 logger-hours for the legacy fortnight and 29 in 1,120 logger-hours for the pilot fortnight. These are alerts, not confirmed leaks or affected households. A presentation’s 25 pilot alerts was corrected by restoring four suppressed, distinct device events; the source says to use 29. (Recorded outcomes)
- L9: 8. Three replays of the same stored trace emitted 29, 31, and 28 alerts with a nondeterministic deduplication tie-breaker enabled. The team did not select one replay as the true count. These replay counts do not replace the final field-ledger count of 29. (Repeats and decision)

[projection.md](calls/water-r2-B2-Research/projection.md)

- L4: 2. The final ledger records 46 low-pressure alerts in 1,400 logger-hours for the legacy fortnight and 29 in 1,120 logger-hours for the pilot fortnight. These are alerts, not confirmed leaks or affected households. A presentation’s 25 pilot alerts was corrected by restoring four suppressed, distinct device events; the source says to use 29. (Recorded outcomes)
- L10: 8. Three replays of the same stored trace emitted 29, 31, and 28 alerts with a nondeterministic deduplication tie-breaker enabled. The team did not select one replay as the true count. These replay counts do not replace the final field-ledger count of 29. (Repeats and decision)

## water-r2-B2 / method — kern — correct

Bron [L5](cases/water/source.md):

> The legacy alert threshold was 1.8 bar for 30 seconds; the pilot used 1.6 bar for 60 seconds.

[content.md](calls/water-r2-B2-Research/content.md)

- L4: 3. The legacy threshold was 1.8 bar for 30 seconds; the pilot threshold was 1.6 bar for 60 seconds, and old traces were not reprocessed. The report does not estimate a rate-adjusted treatment effect. (Recorded outcomes)

[projection.md](calls/water-r2-B2-Research/projection.md)

- L5: 3. The legacy threshold was 1.8 bar for 30 seconds; the pilot threshold was 1.6 bar for 60 seconds, and old traces were not reprocessed. The report does not estimate a rate-adjusted treatment effect. (Recorded outcomes)

## water-r2-B2 / conditions — belangrijk — correct

Bron [L5](cases/water/source.md):

> A planned night flushing operation occurred during the legacy fortnight only.

[content.md](calls/water-r2-B2-Research/content.md)

- L5: 4. The legacy fortnight alone included a planned night flushing operation. Branches were selected for ease of access, not randomly; rainfall differed, and no unaffected comparison branch was instrumented. The source says the lower alert count cannot isolate the sensor upgrade’s effect. (Recorded outcomes)
- L28: - The legacy fortnight included the planned night flushing operation; the pilot fortnight used a different alert threshold. The source gives counts for both periods but does not explicitly establish their calendar order. (Facts 2–4)

[projection.md](calls/water-r2-B2-Research/projection.md)

- L6: 4. The legacy fortnight alone included a planned night flushing operation. Branches were selected for ease of access, not randomly; rainfall differed, and no unaffected comparison branch was instrumented. The source says the lower alert count cannot isolate the sensor upgrade’s effect. (Recorded outcomes)

## water-r2-B2 / allocation — belangrijk — correct

Bron [L5](cases/water/source.md):

> Branches were selected for ease of access, not randomly; rainfall also differed.

[content.md](calls/water-r2-B2-Research/content.md)

- L5: 4. The legacy fortnight alone included a planned night flushing operation. Branches were selected for ease of access, not randomly; rainfall differed, and no unaffected comparison branch was instrumented. The source says the lower alert count cannot isolate the sensor upgrade’s effect. (Recorded outcomes)

[projection.md](calls/water-r2-B2-Research/projection.md)

- L6: 4. The legacy fortnight alone included a planned night flushing operation. Branches were selected for ease of access, not randomly; rainfall differed, and no unaffected comparison branch was instrumented. The source says the lower alert count cannot isolate the sensor upgrade’s effect. (Recorded outcomes)

## water-r2-B2 / contrast — kern — correct

Bron [L7](cases/water/source.md):

> All twelve loggers passed the static reference-gauge calibration at 2.0 bar. Four nevertheless failed the overnight packet-delivery check, losing 17% of scheduled messages in that check.

[content.md](calls/water-r2-B2-Research/content.md)

- L6: 5. All twelve loggers passed static reference-gauge calibration at 2.0 bar. Separately, four failed the overnight packet-delivery check, losing 17% of scheduled messages in that check. The pressure readings that arrived were not shown to be inaccurate. (Checks and defects)
- L7: 6. The packet-delivery fault remained open at review; a proposed antenna move had not been tested. Battery replacement was completed on two different devices, but no date links that work to the delivery check; the source says not to treat battery status as repair of the packet fault. (Checks and defects)
- L15: 1. A successful static calibration check establishes that result for that check, but does not resolve the separately documented packet-delivery failure or establish field uptime. (Facts 5–7)
- L20: - Whether the proposed antenna move would resolve the open packet-delivery fault, or whether battery replacement affected that fault; the move was untested and no date links the replacements to the check. (Fact 6)

[projection.md](calls/water-r2-B2-Research/projection.md)

- L7: 5. All twelve loggers passed static reference-gauge calibration at 2.0 bar. Separately, four failed the overnight packet-delivery check, losing 17% of scheduled messages in that check. The pressure readings that arrived were not shown to be inaccurate. (Checks and defects)
- L8: 6. The packet-delivery fault remained open at review; a proposed antenna move had not been tested. Battery replacement was completed on two different devices, but no date links that work to the delivery check; the source says not to treat battery status as repair of the packet fault. (Checks and defects)
- L18: - Whether the proposed antenna move would resolve the open packet-delivery fault, or whether battery replacement affected that fault; the move was untested and no date links the replacements to the check. (Fact 6)

## water-r2-B2 / open — kern — correct

Bron [L7](cases/water/source.md):

> The delivery fault remained open at review; a proposed antenna move had not been tested.

[content.md](calls/water-r2-B2-Research/content.md)

- L7: 6. The packet-delivery fault remained open at review; a proposed antenna move had not been tested. Battery replacement was completed on two different devices, but no date links that work to the delivery check; the source says not to treat battery status as repair of the packet fault. (Checks and defects)
- L20: - Whether the proposed antenna move would resolve the open packet-delivery fault, or whether battery replacement affected that fault; the move was untested and no date links the replacements to the check. (Fact 6)

[projection.md](calls/water-r2-B2-Research/projection.md)

- L8: 6. The packet-delivery fault remained open at review; a proposed antenna move had not been tested. Battery replacement was completed on two different devices, but no date links that work to the delivery check; the source says not to treat battery status as repair of the packet fault. (Checks and defects)
- L18: - Whether the proposed antenna move would resolve the open packet-delivery fault, or whether battery replacement affected that fault; the move was untested and no date links the replacements to the check. (Fact 6)

## water-r2-B2 / chronology — belangrijk — correct

Bron [L7](cases/water/source.md):

> A battery replacement was completed on two different devices, but the dossier provides no date linking that work to the delivery check.

[content.md](calls/water-r2-B2-Research/content.md)

- L7: 6. The packet-delivery fault remained open at review; a proposed antenna move had not been tested. Battery replacement was completed on two different devices, but no date links that work to the delivery check; the source says not to treat battery status as repair of the packet fault. (Checks and defects)
- L20: - Whether the proposed antenna move would resolve the open packet-delivery fault, or whether battery replacement affected that fault; the move was untested and no date links the replacements to the check. (Fact 6)

[projection.md](calls/water-r2-B2-Research/projection.md)

- L8: 6. The packet-delivery fault remained open at review; a proposed antenna move had not been tested. Battery replacement was completed on two different devices, but no date links that work to the delivery check; the source says not to treat battery status as repair of the packet fault. (Checks and defects)
- L18: - Whether the proposed antenna move would resolve the open packet-delivery fault, or whether battery replacement affected that fault; the move was untested and no date links the replacements to the check. (Fact 6)

## water-r2-B2 / term — kern — correct

Bron [L8](cases/water/source.md):

> A bench check recorded three brownouts: transient supply-voltage drops causing device restarts, not a regional power outage.

[content.md](calls/water-r2-B2-Research/content.md)

- L8: 7. A bench check recorded three brownouts, defined here as transient supply-voltage drops causing device restarts, not a regional power outage. A capacitor swap and firmware update were both made before a repeat bench check with no brownouts. Their separate contributions were not identified, and the bench result does not certify field uptime. (Checks and defects)
- L21: - Whether the capacitor swap, firmware update, or both account for the no-brownout repeat bench result, and whether that result corresponds to field uptime. (Fact 7)
- L29: - The capacitor swap and firmware update preceded the repeat bench check with no brownouts. (Fact 7)

[projection.md](calls/water-r2-B2-Research/projection.md)

- L9: 7. A bench check recorded three brownouts, defined here as transient supply-voltage drops causing device restarts, not a regional power outage. A capacitor swap and firmware update were both made before a repeat bench check with no brownouts. Their separate contributions were not identified, and the bench result does not certify field uptime. (Checks and defects)
- L19: - Whether the capacitor swap, firmware update, or both account for the no-brownout repeat bench result, and whether that result corresponds to field uptime. (Fact 7)

## water-r2-B2 / attribution — kern — correct

Bron [L8](cases/water/source.md):

> A capacitor swap and firmware update were both made before a repeat bench check with no brownouts. Their separate contributions were not identified; the bench result does not certify field uptime.

[content.md](calls/water-r2-B2-Research/content.md)

- L8: 7. A bench check recorded three brownouts, defined here as transient supply-voltage drops causing device restarts, not a regional power outage. A capacitor swap and firmware update were both made before a repeat bench check with no brownouts. Their separate contributions were not identified, and the bench result does not certify field uptime. (Checks and defects)
- L15: 1. A successful static calibration check establishes that result for that check, but does not resolve the separately documented packet-delivery failure or establish field uptime. (Facts 5–7)
- L21: - Whether the capacitor swap, firmware update, or both account for the no-brownout repeat bench result, and whether that result corresponds to field uptime. (Fact 7)
- L29: - The capacitor swap and firmware update preceded the repeat bench check with no brownouts. (Fact 7)

[projection.md](calls/water-r2-B2-Research/projection.md)

- L9: 7. A bench check recorded three brownouts, defined here as transient supply-voltage drops causing device restarts, not a regional power outage. A capacitor swap and firmware update were both made before a repeat bench check with no brownouts. Their separate contributions were not identified, and the bench result does not certify field uptime. (Checks and defects)
- L19: - Whether the capacitor swap, firmware update, or both account for the no-brownout repeat bench result, and whether that result corresponds to field uptime. (Fact 7)

## water-r2-B2 / variation — kern — correct

Bron [L10](cases/water/source.md):

> Three replays of the same stored trace emitted 29, 31 and 28 alerts; a nondeterministic deduplication tie-breaker was enabled.

[content.md](calls/water-r2-B2-Research/content.md)

- L9: 8. Three replays of the same stored trace emitted 29, 31, and 28 alerts with a nondeterministic deduplication tie-breaker enabled. The team did not select one replay as the true count. These replay counts do not replace the final field-ledger count of 29. (Repeats and decision)

[projection.md](calls/water-r2-B2-Research/projection.md)

- L10: 8. Three replays of the same stored trace emitted 29, 31, and 28 alerts with a nondeterministic deduplication tie-breaker enabled. The team did not select one replay as the true count. These replay counts do not replace the final field-ledger count of 29. (Repeats and decision)

## water-r2-B2 / decision — kern — correct

Bron [L11](cases/water/source.md):

> The committee authorized a six-week paired measurement study with unchanged thresholds. It did not authorize network-wide deployment.

[content.md](calls/water-r2-B2-Research/content.md)

- L11: 10. The committee authorized a six-week paired measurement study with unchanged thresholds; it did not authorize network-wide deployment. (Repeats and decision)
- L25: - The outcome of the authorized six-week paired study; the source records authorization, not its completion or results. (Fact 10)
- L30: - The committee authorized a future six-week paired study; network-wide deployment was not authorized. (Fact 10)

[projection.md](calls/water-r2-B2-Research/projection.md)

- L12: 10. The committee authorized a six-week paired measurement study with unchanged thresholds; it did not authorize network-wide deployment. (Repeats and decision)
- L23: - The outcome of the authorized six-week paired study; the source records authorization, not its completion or results. (Fact 10)

## water-r2-B2 / cost — belangrijk — correct

Bron [L11](cases/water/source.md):

> The budget of EUR 18,000 covers logger rental and field visits, excludes staff overtime and does not establish savings.

[content.md](calls/water-r2-B2-Research/content.md)

- L12: 11. The EUR 18,000 budget covers logger rental and field visits, excludes staff overtime, and does not establish savings. No safety, leak-prevention, or annual reliability benefit was measured. Training attendance was 10 of 12 technicians, but attendance was not a competence test. (Repeats and decision)
- L23: - Whether the pilot improves safety, prevents leaks, yields annual reliability benefits, or produces savings; none was measured or established in the source. (Fact 11)

[projection.md](calls/water-r2-B2-Research/projection.md)

- L13: 11. The EUR 18,000 budget covers logger rental and field visits, excludes staff overtime, and does not establish savings. No safety, leak-prevention, or annual reliability benefit was measured. Training attendance was 10 of 12 technicians, but attendance was not a competence test. (Repeats and decision)
- L21: - Whether the pilot improves safety, prevents leaks, yields annual reliability benefits, or produces savings; none was measured or established in the source. (Fact 11)

## water-r2-A / ledger — kern — correct

Bron [L4](cases/water/source.md):

> The legacy fortnight recorded 46 low-pressure alerts in 1,400 logger-hours; the pilot fortnight recorded 29 in 1,120 logger-hours.

[content.md](calls/water-r2-A-Research/content.md)

- L8: 6. The legacy fortnight recorded 46 low-pressure alerts in 1,400 logger-hours.
- L9: 7. The pilot fortnight recorded 29 low-pressure alerts in 1,120 logger-hours.
- L31: 29. The source defines the brownouts as transient supply-voltage drops causing device restarts, not a regional power outage.
- L36: 34. Three replays of the same stored trace emitted 29, 31, and 28 alerts.
- L39: 37. The replay counts do not replace the final field ledger count of 29.
- L48: 46. Attendance was not a competence test.
- L73: - The legacy fortnight recorded 46 alerts in 1,400 logger-hours; its threshold was 1.8 bar for 30 seconds. A planned night flushing operation occurred during this fortnight.
- L74: - The pilot fortnight recorded 29 alerts in 1,120 logger-hours; its threshold was 1.6 bar for 60 seconds. Old traces were not reprocessed.
- L75: - A presentation reported 25 pilot alerts; the final ledger restored four suppressed duplicates with distinct device-event timestamps and recorded 29. The ledger count is the reported final field count.
- L80: - Three replays of the same stored trace emitted 29, 31, and 28 alerts with a nondeterministic deduplication tie-breaker enabled. The team did not select a replay as the true count; these replay counts do not replace the final field ledger count of 29.

[projection.md](calls/water-r2-A-Research/projection.md)

- L8: 6. The legacy fortnight recorded 46 low-pressure alerts in 1,400 logger-hours.
- L9: 7. The pilot fortnight recorded 29 low-pressure alerts in 1,120 logger-hours.
- L31: 29. The source defines the brownouts as transient supply-voltage drops causing device restarts, not a regional power outage.
- L36: 34. Three replays of the same stored trace emitted 29, 31, and 28 alerts.
- L39: 37. The replay counts do not replace the final field ledger count of 29.
- L48: 46. Attendance was not a competence test.

## water-r2-A / method — kern — correct

Bron [L5](cases/water/source.md):

> The legacy alert threshold was 1.8 bar for 30 seconds; the pilot used 1.6 bar for 60 seconds.

[content.md](calls/water-r2-A-Research/content.md)

- L14: 12. The legacy alert threshold was 1.8 bar for 30 seconds.
- L15: 13. The pilot alert threshold was 1.6 bar for 60 seconds.
- L16: 14. Old traces were not reprocessed.
- L73: - The legacy fortnight recorded 46 alerts in 1,400 logger-hours; its threshold was 1.8 bar for 30 seconds. A planned night flushing operation occurred during this fortnight.
- L74: - The pilot fortnight recorded 29 alerts in 1,120 logger-hours; its threshold was 1.6 bar for 60 seconds. Old traces were not reprocessed.

[projection.md](calls/water-r2-A-Research/projection.md)

- L14: 12. The legacy alert threshold was 1.8 bar for 30 seconds.
- L15: 13. The pilot alert threshold was 1.6 bar for 60 seconds.
- L16: 14. Old traces were not reprocessed.

## water-r2-A / conditions — belangrijk — correct

Bron [L5](cases/water/source.md):

> A planned night flushing operation occurred during the legacy fortnight only.

[content.md](calls/water-r2-A-Research/content.md)

- L17: 15. A planned night flushing operation occurred during the legacy fortnight only.
- L73: - The legacy fortnight recorded 46 alerts in 1,400 logger-hours; its threshold was 1.8 bar for 30 seconds. A planned night flushing operation occurred during this fortnight.

[projection.md](calls/water-r2-A-Research/projection.md)

- L17: 15. A planned night flushing operation occurred during the legacy fortnight only.

## water-r2-A / allocation — belangrijk — correct

Bron [L5](cases/water/source.md):

> Branches were selected for ease of access, not randomly; rainfall also differed.

[content.md](calls/water-r2-A-Research/content.md)

- L18: 16. Branches were selected for ease of access, not randomly.
- L19: 17. Rainfall differed between the fortnights.
- L76: - The source states that the lower count cannot isolate the sensor upgrade’s effect; it also identifies differing thresholds, rainfall, branch selection, and the absence of an unaffected comparison branch.

[projection.md](calls/water-r2-A-Research/projection.md)

- L18: 16. Branches were selected for ease of access, not randomly.
- L19: 17. Rainfall differed between the fortnights.

## water-r2-A / contrast — kern — correct

Bron [L7](cases/water/source.md):

> All twelve loggers passed the static reference-gauge calibration at 2.0 bar. Four nevertheless failed the overnight packet-delivery check, losing 17% of scheduled messages in that check.

[content.md](calls/water-r2-A-Research/content.md)

- L22: 20. All twelve loggers passed the static reference-gauge calibration at 2.0 bar.
- L23: 21. Four loggers failed the overnight packet-delivery check.
- L24: 22. Those four loggers lost 17% of scheduled messages in that check.
- L56: - Whether passing the static reference-gauge calibration at 2.0 bar establishes reliability under other conditions or overall field reliability; the source reports the calibration result but does not establish either conclusion.
- L60: - Whether the proposed antenna move would address the packet-delivery fault; it had not been tested.
- L61: - Whether the battery replacements were related to, or repaired, the packet-delivery fault; the dossier gives no date linking the work to the check.
- L77: - All twelve loggers passed the static reference-gauge calibration at 2.0 bar. Four failed the overnight packet-delivery check, losing 17% of scheduled messages in that check. Relationship between the calibration result and packet-delivery performance: not established.
- L86: - Facts 20–33 are relevant to calibration, packet delivery, brownouts, and the limits of the reported checks.

[projection.md](calls/water-r2-A-Research/projection.md)

- L22: 20. All twelve loggers passed the static reference-gauge calibration at 2.0 bar.
- L23: 21. Four loggers failed the overnight packet-delivery check.
- L24: 22. Those four loggers lost 17% of scheduled messages in that check.
- L52: - Whether passing the static reference-gauge calibration at 2.0 bar establishes reliability under other conditions or overall field reliability; the source reports the calibration result but does not establish either conclusion.
- L56: - Whether the proposed antenna move would address the packet-delivery fault; it had not been tested.
- L57: - Whether the battery replacements were related to, or repaired, the packet-delivery fault; the dossier gives no date linking the work to the check.

## water-r2-A / open — kern — correct

Bron [L7](cases/water/source.md):

> The delivery fault remained open at review; a proposed antenna move had not been tested.

[content.md](calls/water-r2-A-Research/content.md)

- L26: 24. The delivery fault remained open at review.
- L27: 25. A proposed antenna move had not been tested.
- L60: - Whether the proposed antenna move would address the packet-delivery fault; it had not been tested.
- L78: - The delivery fault remained open at review; the proposed antenna move had not been tested. The battery replacement work was on two different devices, with no date linking it to the delivery check.

[projection.md](calls/water-r2-A-Research/projection.md)

- L26: 24. The delivery fault remained open at review.
- L27: 25. A proposed antenna move had not been tested.
- L56: - Whether the proposed antenna move would address the packet-delivery fault; it had not been tested.

## water-r2-A / chronology — belangrijk — correct

Bron [L7](cases/water/source.md):

> A battery replacement was completed on two different devices, but the dossier provides no date linking that work to the delivery check.

[content.md](calls/water-r2-A-Research/content.md)

- L28: 26. Battery replacement was completed on two different devices.
- L29: 27. The dossier provides no date linking the battery replacement work to the delivery check.
- L61: - Whether the battery replacements were related to, or repaired, the packet-delivery fault; the dossier gives no date linking the work to the check.
- L78: - The delivery fault remained open at review; the proposed antenna move had not been tested. The battery replacement work was on two different devices, with no date linking it to the delivery check.

[projection.md](calls/water-r2-A-Research/projection.md)

- L28: 26. Battery replacement was completed on two different devices.
- L29: 27. The dossier provides no date linking the battery replacement work to the delivery check.
- L57: - Whether the battery replacements were related to, or repaired, the packet-delivery fault; the dossier gives no date linking the work to the check.

## water-r2-A / term — kern — correct

Bron [L8](cases/water/source.md):

> A bench check recorded three brownouts: transient supply-voltage drops causing device restarts, not a regional power outage.

[content.md](calls/water-r2-A-Research/content.md)

- L30: 28. A bench check recorded three brownouts.
- L31: 29. The source defines the brownouts as transient supply-voltage drops causing device restarts, not a regional power outage.
- L33: 31. The repeat bench check recorded no brownouts.
- L63: - Whether the absence of brownouts in the repeat bench check corresponds to field uptime; the source says the bench result does not certify field uptime.
- L79: - A bench check recorded three brownouts. A capacitor swap and firmware update were made before a repeat bench check with no brownouts. Their separate contributions are not identified, and the bench result does not certify field uptime.
- L86: - Facts 20–33 are relevant to calibration, packet delivery, brownouts, and the limits of the reported checks.

[projection.md](calls/water-r2-A-Research/projection.md)

- L30: 28. A bench check recorded three brownouts.
- L31: 29. The source defines the brownouts as transient supply-voltage drops causing device restarts, not a regional power outage.
- L33: 31. The repeat bench check recorded no brownouts.
- L59: - Whether the absence of brownouts in the repeat bench check corresponds to field uptime; the source says the bench result does not certify field uptime.

## water-r2-A / attribution — kern — correct

Bron [L8](cases/water/source.md):

> A capacitor swap and firmware update were both made before a repeat bench check with no brownouts. Their separate contributions were not identified; the bench result does not certify field uptime.

[content.md](calls/water-r2-A-Research/content.md)

- L32: 30. A capacitor swap and a firmware update were both made before a repeat bench check.
- L34: 32. The separate contributions of the capacitor swap and firmware update were not identified.
- L35: 33. The bench result does not certify field uptime.
- L62: - The separate contributions of the capacitor swap and firmware update to the repeat bench result.
- L63: - Whether the absence of brownouts in the repeat bench check corresponds to field uptime; the source says the bench result does not certify field uptime.
- L79: - A bench check recorded three brownouts. A capacitor swap and firmware update were made before a repeat bench check with no brownouts. Their separate contributions are not identified, and the bench result does not certify field uptime.

[projection.md](calls/water-r2-A-Research/projection.md)

- L32: 30. A capacitor swap and a firmware update were both made before a repeat bench check.
- L34: 32. The separate contributions of the capacitor swap and firmware update were not identified.
- L35: 33. The bench result does not certify field uptime.
- L58: - The separate contributions of the capacitor swap and firmware update to the repeat bench result.
- L59: - Whether the absence of brownouts in the repeat bench check corresponds to field uptime; the source says the bench result does not certify field uptime.

## water-r2-A / variation — kern — correct

Bron [L10](cases/water/source.md):

> Three replays of the same stored trace emitted 29, 31 and 28 alerts; a nondeterministic deduplication tie-breaker was enabled.

[content.md](calls/water-r2-A-Research/content.md)

- L36: 34. Three replays of the same stored trace emitted 29, 31, and 28 alerts.
- L37: 35. A nondeterministic deduplication tie-breaker was enabled.
- L38: 36. The team did not select one replay as the true count.
- L39: 37. The replay counts do not replace the final field ledger count of 29.
- L64: - Which replay count, if any, should be treated as the true replay count; the team did not select one.
- L65: - The cause of the differences between the replay counts.
- L80: - Three replays of the same stored trace emitted 29, 31, and 28 alerts with a nondeterministic deduplication tie-breaker enabled. The team did not select a replay as the true count; these replay counts do not replace the final field ledger count of 29.

[projection.md](calls/water-r2-A-Research/projection.md)

- L36: 34. Three replays of the same stored trace emitted 29, 31, and 28 alerts.
- L37: 35. A nondeterministic deduplication tie-breaker was enabled.
- L38: 36. The team did not select one replay as the true count.
- L39: 37. The replay counts do not replace the final field ledger count of 29.
- L60: - Which replay count, if any, should be treated as the true replay count; the team did not select one.
- L61: - The cause of the differences between the replay counts.

## water-r2-A / decision — kern — correct

Bron [L11](cases/water/source.md):

> The committee authorized a six-week paired measurement study with unchanged thresholds. It did not authorize network-wide deployment.

[content.md](calls/water-r2-A-Research/content.md)

- L41: 39. The committee authorized a six-week paired measurement study with unchanged thresholds.
- L42: 40. The committee did not authorize network-wide deployment.
- L66: - Whether the authorized six-week study will support network-wide deployment or establish measured benefits; the source reports the authorization, not its results.
- L81: - The committee authorized a six-week paired measurement study with unchanged thresholds and did not authorize network-wide deployment. Results of that study are not reported in the source.

[projection.md](calls/water-r2-A-Research/projection.md)

- L41: 39. The committee authorized a six-week paired measurement study with unchanged thresholds.
- L42: 40. The committee did not authorize network-wide deployment.
- L62: - Whether the authorized six-week study will support network-wide deployment or establish measured benefits; the source reports the authorization, not its results.

## water-r2-A / cost — belangrijk — correct

Bron [L11](cases/water/source.md):

> The budget of EUR 18,000 covers logger rental and field visits, excludes staff overtime and does not establish savings.

[content.md](calls/water-r2-A-Research/content.md)

- L43: 41. The EUR 18,000 budget covers logger rental and field visits.
- L44: 42. The budget excludes staff overtime.
- L45: 43. The budget does not establish savings.
- L67: - Whether the pilot produced savings or safety, leak-prevention, or annual reliability benefits; none was measured in this dossier.

[projection.md](calls/water-r2-A-Research/projection.md)

- L43: 41. The EUR 18,000 budget covers logger rental and field visits.
- L44: 42. The budget excludes staff overtime.
- L45: 43. The budget does not establish savings.
- L63: - Whether the pilot produced savings or safety, leak-prevention, or annual reliability benefits; none was measured in this dossier.

## language-r2-B2 / sample — kern — correct

Bron [L4](cases/language/source.md):

> Reviewers sampled 80 of 640 message strings: 40 Spanish and 40 Polish.

[content.md](calls/language-r2-B2-Research/content.md)

- L3: 2. Reviewers sampled 80 of 640 message strings: 40 Spanish and 40 Polish. All 80 retained approved glossary entitlement terms, but six Spanish strings reversed the deadline exception’s meaning; no such reversal was found in the Polish sample. Terminology conformity and semantic correctness were separate checks, and neither result certifies all 640 strings. (Sample and linguistic checks)
- L8: 7. “Case folding” means normalising letter case in preprocessing; it did not merge benefit cases or applications. On the same 80 strings, a sampling-based checker flagged six, five and six semantic issues across three runs. A human reference review identified six deadline reversals. The checker version was fixed, but sample order varied; the run counts do not establish three different ground truths or a stable checker. (Processing and reproducibility)
- L18: - The semantic correctness of all 640 strings is not established: only 80 were sampled, and the remaining strings did not receive the second semantic review. The source does not establish whether unsampled strings contain errors. (Facts 2–3)

[projection.md](calls/language-r2-B2-Research/projection.md)

- L4: 2. Reviewers sampled 80 of 640 message strings: 40 Spanish and 40 Polish. All 80 retained approved glossary entitlement terms, but six Spanish strings reversed the deadline exception’s meaning; no such reversal was found in the Polish sample. Terminology conformity and semantic correctness were separate checks, and neither result certifies all 640 strings. (Sample and linguistic checks)
- L9: 7. “Case folding” means normalising letter case in preprocessing; it did not merge benefit cases or applications. On the same 80 strings, a sampling-based checker flagged six, five and six semantic issues across three runs. A human reference review identified six deadline reversals. The checker version was fixed, but sample order varied; the run counts do not establish three different ground truths or a stable checker. (Processing and reproducibility)
- L16: - The semantic correctness of all 640 strings is not established: only 80 were sampled, and the remaining strings did not receive the second semantic review. The source does not establish whether unsampled strings contain errors. (Facts 2–3)

## language-r2-B2 / contrast — kern — correct

Bron [L4](cases/language/source.md):

> All 80 retained the glossary's approved entitlement terms. Six Spanish strings nevertheless reversed the meaning of the deadline exception. The Polish sample had no deadline reversal.

[content.md](calls/language-r2-B2-Research/content.md)

- L3: 2. Reviewers sampled 80 of 640 message strings: 40 Spanish and 40 Polish. All 80 retained approved glossary entitlement terms, but six Spanish strings reversed the deadline exception’s meaning; no such reversal was found in the Polish sample. Terminology conformity and semantic correctness were separate checks, and neither result certifies all 640 strings. (Sample and linguistic checks)
- L4: 3. A second reviewer agreed with the six identified reversals; the other strings did not receive that second semantic review. (Sample and linguistic checks)
- L8: 7. “Case folding” means normalising letter case in preprocessing; it did not merge benefit cases or applications. On the same 80 strings, a sampling-based checker flagged six, five and six semantic issues across three runs. A human reference review identified six deadline reversals. The checker version was fixed, but sample order varied; the run counts do not establish three different ground truths or a stable checker. (Processing and reproducibility)

[projection.md](calls/language-r2-B2-Research/projection.md)

- L4: 2. Reviewers sampled 80 of 640 message strings: 40 Spanish and 40 Polish. All 80 retained approved glossary entitlement terms, but six Spanish strings reversed the deadline exception’s meaning; no such reversal was found in the Polish sample. Terminology conformity and semantic correctness were separate checks, and neither result certifies all 640 strings. (Sample and linguistic checks)
- L5: 3. A second reviewer agreed with the six identified reversals; the other strings did not receive that second semantic review. (Sample and linguistic checks)
- L9: 7. “Case folding” means normalising letter case in preprocessing; it did not merge benefit cases or applications. On the same 80 strings, a sampling-based checker flagged six, five and six semantic issues across three runs. A human reference review identified six deadline reversals. The checker version was fixed, but sample order varied; the run counts do not establish three different ground truths or a stable checker. (Processing and reproducibility)

## language-r2-B2 / review — belangrijk — correct

Bron [L4](cases/language/source.md):

> A second reviewer checked the six reversals and agreed. The remaining strings did not receive that second semantic review.

[content.md](calls/language-r2-B2-Research/content.md)

- L4: 3. A second reviewer agreed with the six identified reversals; the other strings did not receive that second semantic review. (Sample and linguistic checks)
- L18: - The semantic correctness of all 640 strings is not established: only 80 were sampled, and the remaining strings did not receive the second semantic review. The source does not establish whether unsampled strings contain errors. (Facts 2–3)

[projection.md](calls/language-r2-B2-Research/projection.md)

- L5: 3. A second reviewer agreed with the six identified reversals; the other strings did not receive that second semantic review. (Sample and linguistic checks)
- L16: - The semantic correctness of all 640 strings is not established: only 80 were sampled, and the remaining strings did not receive the second semantic review. The source does not establish whether unsampled strings contain errors. (Facts 2–3)

## language-r2-B2 / access — kern — correct

Bron [L6](cases/language/source.md):

> An automated colour-contrast scan passed the three tested page templates. Keyboard-only testing could not activate the document-upload button on the second template.

[content.md](calls/language-r2-B2-Research/content.md)

- L5: 4. An automated colour-contrast scan passed on the three tested page templates. Keyboard-only testing could not activate the document-upload button on the second template, and the defect was still open. (Access and task results)
- L6: 5. On that template, screen-reader testing announced the upload label correctly but did not test keyboard activation. The source explicitly says a visually correct label does not show that a keyboard user can upload a document. Management approved a keyboard-fix proposal; no deployed fix is recorded. (Access and task results)
- L10: 9. The coordinator authorised a corrected-form usability study conditional on a verified upload fix and withheld general release. The study budget includes translation fees but excludes staff time; no cost saving is demonstrated. (Status and decision boundary)
- L19: - Keyboard operation beyond the tested failure is not established, and the approved fix proposal’s implementation or verification is not recorded. A contrast pass on three templates and correct screen-reader announcement of one label do not establish overall accessibility. (Facts 1, 4–5, 9)
- L21: - The three checker counts do not establish checker stability or separate ground truths; the source does not report a verified general-release outcome, a deployed upload fix, or demonstrated cost savings. (Facts 7, 9)
- L25: No dated chronology is given. The source records the upload defect as open, a fix proposal as approved but not deployed, a usability study as authorised conditional on a verified fix, and general release as withheld. (Facts 4–5, 9)
- L28: Facts 2–3 (translation and semantic checks); 4–5 (access checks and unresolved upload defect); 6–7 (measurement and reproducibility limits); 8–9 (approval and bounded decision); 1 and 10 (limits on broader inclusion and satisfaction claims).

[projection.md](calls/language-r2-B2-Research/projection.md)

- L6: 4. An automated colour-contrast scan passed on the three tested page templates. Keyboard-only testing could not activate the document-upload button on the second template, and the defect was still open. (Access and task results)
- L7: 5. On that template, screen-reader testing announced the upload label correctly but did not test keyboard activation. The source explicitly says a visually correct label does not show that a keyboard user can upload a document. Management approved a keyboard-fix proposal; no deployed fix is recorded. (Access and task results)
- L11: 9. The coordinator authorised a corrected-form usability study conditional on a verified upload fix and withheld general release. The study budget includes translation fees but excludes staff time; no cost saving is demonstrated. (Status and decision boundary)
- L17: - Keyboard operation beyond the tested failure is not established, and the approved fix proposal’s implementation or verification is not recorded. A contrast pass on three templates and correct screen-reader announcement of one label do not establish overall accessibility. (Facts 1, 4–5, 9)
- L19: - The three checker counts do not establish checker stability or separate ground truths; the source does not report a verified general-release outcome, a deployed upload fix, or demonstrated cost savings. (Facts 7, 9)

## language-r2-B2 / open — kern — correct

Bron [L6](cases/language/source.md):

> The upload defect was still open. Screen-reader testing announced the upload label correctly on that template, but did not test keyboard activation.

[content.md](calls/language-r2-B2-Research/content.md)

- L5: 4. An automated colour-contrast scan passed on the three tested page templates. Keyboard-only testing could not activate the document-upload button on the second template, and the defect was still open. (Access and task results)
- L6: 5. On that template, screen-reader testing announced the upload label correctly but did not test keyboard activation. The source explicitly says a visually correct label does not show that a keyboard user can upload a document. Management approved a keyboard-fix proposal; no deployed fix is recorded. (Access and task results)
- L19: - Keyboard operation beyond the tested failure is not established, and the approved fix proposal’s implementation or verification is not recorded. A contrast pass on three templates and correct screen-reader announcement of one label do not establish overall accessibility. (Facts 1, 4–5, 9)
- L25: No dated chronology is given. The source records the upload defect as open, a fix proposal as approved but not deployed, a usability study as authorised conditional on a verified fix, and general release as withheld. (Facts 4–5, 9)

[projection.md](calls/language-r2-B2-Research/projection.md)

- L6: 4. An automated colour-contrast scan passed on the three tested page templates. Keyboard-only testing could not activate the document-upload button on the second template, and the defect was still open. (Access and task results)
- L7: 5. On that template, screen-reader testing announced the upload label correctly but did not test keyboard activation. The source explicitly says a visually correct label does not show that a keyboard user can upload a document. Management approved a keyboard-fix proposal; no deployed fix is recorded. (Access and task results)
- L17: - Keyboard operation beyond the tested failure is not established, and the approved fix proposal’s implementation or verification is not recorded. A contrast pass on three templates and correct screen-reader announcement of one label do not establish overall accessibility. (Facts 1, 4–5, 9)

## language-r2-B2 / outcome — kern — correct

Bron [L7](cases/language/source.md):

> Task completion in the old English-only service was 33 of 50 moderated sessions. The multilingual form recorded 42 of 50 unmoderated sessions.

[content.md](calls/language-r2-B2-Research/content.md)

- L7: 6. The old English-only service recorded completion in 33 of 50 moderated sessions; the multilingual form recorded 42 of 50 unmoderated sessions. The old measure required a final submitted application, while the new dashboard counted reaching the review screen. Participants came from different recruitment channels and were neither randomly assigned nor matched for prior experience. The source says the apparent dashboard improvement is not a comparable completion-effect estimate. (Access and task results)

[projection.md](calls/language-r2-B2-Research/projection.md)

- L8: 6. The old English-only service recorded completion in 33 of 50 moderated sessions; the multilingual form recorded 42 of 50 unmoderated sessions. The old measure required a final submitted application, while the new dashboard counted reaching the review screen. Participants came from different recruitment channels and were neither randomly assigned nor matched for prior experience. The source says the apparent dashboard improvement is not a comparable completion-effect estimate. (Access and task results)

## language-r2-B2 / method — kern — correct

Bron [L7](cases/language/source.md):

> Completion formerly required a final submitted application; the new dashboard counted reaching the review screen.

[content.md](calls/language-r2-B2-Research/content.md)

- L7: 6. The old English-only service recorded completion in 33 of 50 moderated sessions; the multilingual form recorded 42 of 50 unmoderated sessions. The old measure required a final submitted application, while the new dashboard counted reaching the review screen. Participants came from different recruitment channels and were neither randomly assigned nor matched for prior experience. The source says the apparent dashboard improvement is not a comparable completion-effect estimate. (Access and task results)

[projection.md](calls/language-r2-B2-Research/projection.md)

- L8: 6. The old English-only service recorded completion in 33 of 50 moderated sessions; the multilingual form recorded 42 of 50 unmoderated sessions. The old measure required a final submitted application, while the new dashboard counted reaching the review screen. Participants came from different recruitment channels and were neither randomly assigned nor matched for prior experience. The source says the apparent dashboard improvement is not a comparable completion-effect estimate. (Access and task results)

## language-r2-B2 / allocation — belangrijk — correct

Bron [L7](cases/language/source.md):

> Participants were recruited from different channels and neither assigned randomly nor matched for prior experience.

[content.md](calls/language-r2-B2-Research/content.md)

- L7: 6. The old English-only service recorded completion in 33 of 50 moderated sessions; the multilingual form recorded 42 of 50 unmoderated sessions. The old measure required a final submitted application, while the new dashboard counted reaching the review screen. Participants came from different recruitment channels and were neither randomly assigned nor matched for prior experience. The source says the apparent dashboard improvement is not a comparable completion-effect estimate. (Access and task results)

[projection.md](calls/language-r2-B2-Research/projection.md)

- L8: 6. The old English-only service recorded completion in 33 of 50 moderated sessions; the multilingual form recorded 42 of 50 unmoderated sessions. The old measure required a final submitted application, while the new dashboard counted reaching the review screen. Participants came from different recruitment channels and were neither randomly assigned nor matched for prior experience. The source says the apparent dashboard improvement is not a comparable completion-effect estimate. (Access and task results)

## language-r2-B2 / term — kern — correct

Bron [L9](cases/language/source.md):

> A preprocessing step called case folding normalised letter case; it did not merge benefit cases or applications.

[content.md](calls/language-r2-B2-Research/content.md)

- L8: 7. “Case folding” means normalising letter case in preprocessing; it did not merge benefit cases or applications. On the same 80 strings, a sampling-based checker flagged six, five and six semantic issues across three runs. A human reference review identified six deadline reversals. The checker version was fixed, but sample order varied; the run counts do not establish three different ground truths or a stable checker. (Processing and reproducibility)

[projection.md](calls/language-r2-B2-Research/projection.md)

- L9: 7. “Case folding” means normalising letter case in preprocessing; it did not merge benefit cases or applications. On the same 80 strings, a sampling-based checker flagged six, five and six semantic issues across three runs. A human reference review identified six deadline reversals. The checker version was fixed, but sample order varied; the run counts do not establish three different ground truths or a stable checker. (Processing and reproducibility)

## language-r2-B2 / variation — kern — correct

Bron [L9](cases/language/source.md):

> Three runs on the same 80 strings flagged six, five and six semantic issues with a sampling-based checker. A human reference review identified the six deadline reversals.

[content.md](calls/language-r2-B2-Research/content.md)

- L8: 7. “Case folding” means normalising letter case in preprocessing; it did not merge benefit cases or applications. On the same 80 strings, a sampling-based checker flagged six, five and six semantic issues across three runs. A human reference review identified six deadline reversals. The checker version was fixed, but sample order varied; the run counts do not establish three different ground truths or a stable checker. (Processing and reproducibility)
- L21: - The three checker counts do not establish checker stability or separate ground truths; the source does not report a verified general-release outcome, a deployed upload fix, or demonstrated cost savings. (Facts 7, 9)

[projection.md](calls/language-r2-B2-Research/projection.md)

- L9: 7. “Case folding” means normalising letter case in preprocessing; it did not merge benefit cases or applications. On the same 80 strings, a sampling-based checker flagged six, five and six semantic issues across three runs. A human reference review identified six deadline reversals. The checker version was fixed, but sample order varied; the run counts do not establish three different ground truths or a stable checker. (Processing and reproducibility)
- L19: - The three checker counts do not establish checker stability or separate ground truths; the source does not report a verified general-release outcome, a deployed upload fix, or demonstrated cost savings. (Facts 7, 9)

## language-r2-B2 / legal — kern — correct

Bron [L11](cases/language/source.md):

> The appendix says legal approval covers the Spanish consent paragraph only. It gives no approval date or temporal relationship to the sample review. Polish consent approval is not documented.

[content.md](calls/language-r2-B2-Research/content.md)

- L9: 8. The appendix says legal approval covers only the Spanish consent paragraph. It gives no approval date or temporal relationship to the sample review; Polish consent approval is not documented. The source states that translation availability alone does not establish legal approval. (Status and decision boundary)
- L22: - Legal approval beyond the Spanish consent paragraph—including Polish consent approval—is not established; the approval’s date and relation to the sample review are unspecified. (Fact 8)

[projection.md](calls/language-r2-B2-Research/projection.md)

- L10: 8. The appendix says legal approval covers only the Spanish consent paragraph. It gives no approval date or temporal relationship to the sample review; Polish consent approval is not documented. The source states that translation availability alone does not establish legal approval. (Status and decision boundary)
- L20: - Legal approval beyond the Spanish consent paragraph—including Polish consent approval—is not established; the approval’s date and relation to the sample review are unspecified. (Fact 8)

## language-r2-B2 / decision — kern — correct

Bron [L11](cases/language/source.md):

> The coordinator authorised a corrected-form usability study, conditional on a verified upload fix, and withheld general release.

[content.md](calls/language-r2-B2-Research/content.md)

- L10: 9. The coordinator authorised a corrected-form usability study conditional on a verified upload fix and withheld general release. The study budget includes translation fees but excludes staff time; no cost saving is demonstrated. (Status and decision boundary)
- L25: No dated chronology is given. The source records the upload defect as open, a fix proposal as approved but not deployed, a usability study as authorised conditional on a verified fix, and general release as withheld. (Facts 4–5, 9)

[projection.md](calls/language-r2-B2-Research/projection.md)

- L11: 9. The coordinator authorised a corrected-form usability study conditional on a verified upload fix and withheld general release. The study budget includes translation fees but excludes staff time; no cost saving is demonstrated. (Status and decision boundary)

## language-r2-A / sample — kern — correct

Bron [L4](cases/language/source.md):

> Reviewers sampled 80 of 640 message strings: 40 Spanish and 40 Polish.

[content.md](calls/language-r2-A-Research/content.md)

- L7: - F5. Reviewers sampled 80 of 640 message strings.
- L8: - F6. The sample contained 40 Spanish strings.
- L9: - F7. The sample contained 40 Polish strings.
- L10: - F8. All 80 sampled strings retained the glossary’s approved entitlement terms.
- L14: - F12. Neither check result certifies all 640 strings.
- L34: - F32. Three runs on the same 80 strings flagged six, five, and six semantic issues with a sampling-based checker.
- L63: - The source does not establish that all 640 message strings were semantically correct; only the stated sample results are reported (F5, F9–F14).

[projection.md](calls/language-r2-A-Research/projection.md)

- L7: - F5. Reviewers sampled 80 of 640 message strings.
- L8: - F6. The sample contained 40 Spanish strings.
- L9: - F7. The sample contained 40 Polish strings.
- L10: - F8. All 80 sampled strings retained the glossary’s approved entitlement terms.
- L14: - F12. Neither check result certifies all 640 strings.
- L34: - F32. Three runs on the same 80 strings flagged six, five, and six semantic issues with a sampling-based checker.
- L59: - The source does not establish that all 640 message strings were semantically correct; only the stated sample results are reported (F5, F9–F14).

## language-r2-A / contrast — kern — correct

Bron [L4](cases/language/source.md):

> All 80 retained the glossary's approved entitlement terms. Six Spanish strings nevertheless reversed the meaning of the deadline exception. The Polish sample had no deadline reversal.

[content.md](calls/language-r2-A-Research/content.md)

- L10: - F8. All 80 sampled strings retained the glossary’s approved entitlement terms.
- L11: - F9. Six Spanish strings reversed the meaning of the deadline exception.
- L12: - F10. The Polish sample had no deadline reversal.
- L13: - F11. The source treats terminology conformity and semantic correctness as separate checks.
- L15: - F13. A second reviewer checked the six reversals and agreed.
- L35: - F33. A human reference review identified the six deadline reversals.
- L73: - The source does not establish that terminology conformity alone means the wording was semantically correct; it reports separate checks and six Spanish deadline reversals (F8–F12).
- L77: - The source reports sampling and linguistic-check results: F5–F14. It does not establish an event chronology beyond the reported checks; the relationship between the terminology result and the semantic errors is not stated as causal.
- L86: - Facts F5–F14 concern sample size, terminology conformity, semantic errors, and review coverage.

[projection.md](calls/language-r2-A-Research/projection.md)

- L10: - F8. All 80 sampled strings retained the glossary’s approved entitlement terms.
- L11: - F9. Six Spanish strings reversed the meaning of the deadline exception.
- L12: - F10. The Polish sample had no deadline reversal.
- L13: - F11. The source treats terminology conformity and semantic correctness as separate checks.
- L15: - F13. A second reviewer checked the six reversals and agreed.
- L35: - F33. A human reference review identified the six deadline reversals.
- L69: - The source does not establish that terminology conformity alone means the wording was semantically correct; it reports separate checks and six Spanish deadline reversals (F8–F12).

## language-r2-A / review — belangrijk — correct

Bron [L4](cases/language/source.md):

> A second reviewer checked the six reversals and agreed. The remaining strings did not receive that second semantic review.

[content.md](calls/language-r2-A-Research/content.md)

- L15: - F13. A second reviewer checked the six reversals and agreed.
- L16: - F14. The remaining strings did not receive that second semantic review.

[projection.md](calls/language-r2-A-Research/projection.md)

- L15: - F13. A second reviewer checked the six reversals and agreed.
- L16: - F14. The remaining strings did not receive that second semantic review.

## language-r2-A / access — kern — correct

Bron [L6](cases/language/source.md):

> An automated colour-contrast scan passed the three tested page templates. Keyboard-only testing could not activate the document-upload button on the second template.

[content.md](calls/language-r2-A-Research/content.md)

- L17: - F15. An automated colour-contrast scan passed the three tested page templates.
- L18: - F16. Keyboard-only testing could not activate the document-upload button on the second template.
- L19: - F17. The upload defect was still open.
- L20: - F18. Screen-reader testing announced the upload label correctly on the second template.
- L21: - F19. Screen-reader testing did not test keyboard activation.
- L22: - F20. The source states that a visually correct label is not evidence that a keyboard user can upload a document.
- L23: - F21. Management approved a keyboard-fix proposal.
- L24: - F22. No deployed keyboard fix is recorded.
- L45: - F43. The coordinator authorized a corrected-form usability study, conditional on a verified upload fix.
- L64: - The source does not establish keyboard upload access beyond the reported test of the second template, or that the proposed fix was deployed (F16–F17, F21–F22).
- L72: - The source does not state the scope, measures, or results of the authorized usability study beyond its being a corrected-form study conditional on a verified upload fix (F43).
- L78: - The source reports access-test results and the upload-defect status: F15–F22. It records management’s approval of a fix proposal and the absence of a recorded deployed fix, but does not establish that the proposal was implemented (F21–F22).
- L87: - Facts F15–F22 concern contrast, keyboard activation, screen-reader label announcement, and the status of the proposed keyboard fix.

[projection.md](calls/language-r2-A-Research/projection.md)

- L17: - F15. An automated colour-contrast scan passed the three tested page templates.
- L18: - F16. Keyboard-only testing could not activate the document-upload button on the second template.
- L19: - F17. The upload defect was still open.
- L20: - F18. Screen-reader testing announced the upload label correctly on the second template.
- L21: - F19. Screen-reader testing did not test keyboard activation.
- L22: - F20. The source states that a visually correct label is not evidence that a keyboard user can upload a document.
- L23: - F21. Management approved a keyboard-fix proposal.
- L24: - F22. No deployed keyboard fix is recorded.
- L45: - F43. The coordinator authorized a corrected-form usability study, conditional on a verified upload fix.
- L60: - The source does not establish keyboard upload access beyond the reported test of the second template, or that the proposed fix was deployed (F16–F17, F21–F22).
- L68: - The source does not state the scope, measures, or results of the authorized usability study beyond its being a corrected-form study conditional on a verified upload fix (F43).

## language-r2-A / open — kern — correct

Bron [L6](cases/language/source.md):

> The upload defect was still open. Screen-reader testing announced the upload label correctly on that template, but did not test keyboard activation.

[content.md](calls/language-r2-A-Research/content.md)

- L19: - F17. The upload defect was still open.
- L20: - F18. Screen-reader testing announced the upload label correctly on the second template.
- L21: - F19. Screen-reader testing did not test keyboard activation.
- L22: - F20. The source states that a visually correct label is not evidence that a keyboard user can upload a document.
- L87: - Facts F15–F22 concern contrast, keyboard activation, screen-reader label announcement, and the status of the proposed keyboard fix.

[projection.md](calls/language-r2-A-Research/projection.md)

- L19: - F17. The upload defect was still open.
- L20: - F18. Screen-reader testing announced the upload label correctly on the second template.
- L21: - F19. Screen-reader testing did not test keyboard activation.
- L22: - F20. The source states that a visually correct label is not evidence that a keyboard user can upload a document.

## language-r2-A / outcome — kern — correct

Bron [L7](cases/language/source.md):

> Task completion in the old English-only service was 33 of 50 moderated sessions. The multilingual form recorded 42 of 50 unmoderated sessions.

[content.md](calls/language-r2-A-Research/content.md)

- L25: - F23. Task completion in the old English-only service was 33 of 50 moderated sessions.
- L26: - F24. The multilingual form recorded 42 of 50 unmoderated sessions.
- L35: - F33. A human reference review identified the six deadline reversals.
- L44: - F42. Translation availability does not itself establish legal approval.
- L67: - The source does not establish an approval date or when the Spanish consent approval occurred relative to the sample review; Polish consent approval is not documented (F38–F42).

[projection.md](calls/language-r2-A-Research/projection.md)

- L25: - F23. Task completion in the old English-only service was 33 of 50 moderated sessions.
- L26: - F24. The multilingual form recorded 42 of 50 unmoderated sessions.
- L35: - F33. A human reference review identified the six deadline reversals.
- L44: - F42. Translation availability does not itself establish legal approval.
- L63: - The source does not establish an approval date or when the Spanish consent approval occurred relative to the sample review; Polish consent approval is not documented (F38–F42).

## language-r2-A / method — kern — correct

Bron [L7](cases/language/source.md):

> Completion formerly required a final submitted application; the new dashboard counted reaching the review screen.

[content.md](calls/language-r2-A-Research/content.md)

- L27: - F25. Completion in the old service required a final submitted application.
- L28: - F26. The new dashboard counted reaching the review screen as completion.

[projection.md](calls/language-r2-A-Research/projection.md)

- L27: - F25. Completion in the old service required a final submitted application.
- L28: - F26. The new dashboard counted reaching the review screen as completion.

## language-r2-A / allocation — belangrijk — correct

Bron [L7](cases/language/source.md):

> Participants were recruited from different channels and neither assigned randomly nor matched for prior experience.

[content.md](calls/language-r2-A-Research/content.md)

- L29: - F27. Participants were recruited from different channels.
- L30: - F28. Participants were neither assigned randomly nor matched for prior experience.
- L65: - The task-session figures do not establish a comparable completion-effect estimate; the source identifies different completion definitions and participant-recruitment and assignment limitations (F23–F29).

[projection.md](calls/language-r2-A-Research/projection.md)

- L29: - F27. Participants were recruited from different channels.
- L30: - F28. Participants were neither assigned randomly nor matched for prior experience.
- L61: - The task-session figures do not establish a comparable completion-effect estimate; the source identifies different completion definitions and participant-recruitment and assignment limitations (F23–F29).

## language-r2-A / term — kern — correct

Bron [L9](cases/language/source.md):

> A preprocessing step called case folding normalised letter case; it did not merge benefit cases or applications.

[content.md](calls/language-r2-A-Research/content.md)

- L32: - F30. The preprocessing step called case folding normalized letter case.
- L33: - F31. Case folding did not merge benefit cases or applications.
- L89: - Facts F30–F37 concern case folding and variation across checker runs.

[projection.md](calls/language-r2-A-Research/projection.md)

- L32: - F30. The preprocessing step called case folding normalized letter case.
- L33: - F31. Case folding did not merge benefit cases or applications.

## language-r2-A / variation — kern — correct

Bron [L9](cases/language/source.md):

> Three runs on the same 80 strings flagged six, five and six semantic issues with a sampling-based checker. A human reference review identified the six deadline reversals.

[content.md](calls/language-r2-A-Research/content.md)

- L34: - F32. Three runs on the same 80 strings flagged six, five, and six semantic issues with a sampling-based checker.
- L35: - F33. A human reference review identified the six deadline reversals.
- L37: - F35. The automated run counts do not establish a stable checker.
- L38: - F36. The checker version was held fixed.
- L66: - The source does not establish that the checker is stable or that the three run counts represent different ground truths (F32–F37).
- L80: - The source reports preprocessing details and checker-run results: F30–F37. It does not establish that run-to-run variation represents different ground truths or a stable checker.
- L89: - Facts F30–F37 concern case folding and variation across checker runs.

[projection.md](calls/language-r2-A-Research/projection.md)

- L34: - F32. Three runs on the same 80 strings flagged six, five, and six semantic issues with a sampling-based checker.
- L35: - F33. A human reference review identified the six deadline reversals.
- L37: - F35. The automated run counts do not establish a stable checker.
- L38: - F36. The checker version was held fixed.
- L62: - The source does not establish that the checker is stable or that the three run counts represent different ground truths (F32–F37).

## language-r2-A / legal — kern — correct

Bron [L11](cases/language/source.md):

> The appendix says legal approval covers the Spanish consent paragraph only. It gives no approval date or temporal relationship to the sample review. Polish consent approval is not documented.

[content.md](calls/language-r2-A-Research/content.md)

- L40: - F38. The appendix says legal approval covers the Spanish consent paragraph only.
- L43: - F41. Polish consent approval is not documented.
- L44: - F42. Translation availability does not itself establish legal approval.
- L67: - The source does not establish an approval date or when the Spanish consent approval occurred relative to the sample review; Polish consent approval is not documented (F38–F42).
- L90: - Facts F38–F44 concern consent-approval documentation, the conditional usability study, and the general-release decision.

[projection.md](calls/language-r2-A-Research/projection.md)

- L40: - F38. The appendix says legal approval covers the Spanish consent paragraph only.
- L43: - F41. Polish consent approval is not documented.
- L44: - F42. Translation availability does not itself establish legal approval.
- L63: - The source does not establish an approval date or when the Spanish consent approval occurred relative to the sample review; Polish consent approval is not documented (F38–F42).

## language-r2-A / decision — kern — correct

Bron [L11](cases/language/source.md):

> The coordinator authorised a corrected-form usability study, conditional on a verified upload fix, and withheld general release.

[content.md](calls/language-r2-A-Research/content.md)

- L45: - F43. The coordinator authorized a corrected-form usability study, conditional on a verified upload fix.
- L46: - F44. The coordinator withheld general release.
- L72: - The source does not state the scope, measures, or results of the authorized usability study beyond its being a corrected-form study conditional on a verified upload fix (F43).
- L81: - The source reports approval documentation and then the coordinator’s study authorization and general-release decision: F38–F44. It gives no approval date or temporal relationship between the Spanish approval and the sample review (F39–F40).
- L90: - Facts F38–F44 concern consent-approval documentation, the conditional usability study, and the general-release decision.

[projection.md](calls/language-r2-A-Research/projection.md)

- L45: - F43. The coordinator authorized a corrected-form usability study, conditional on a verified upload fix.
- L46: - F44. The coordinator withheld general release.
- L68: - The source does not state the scope, measures, or results of the authorized usability study beyond its being a corrected-form study conditional on a verified upload fix (F43).

## language-r2-B / sample — kern — correct

Bron [L4](cases/language/source.md):

> Reviewers sampled 80 of 640 message strings: 40 Spanish and 40 Polish.

[content.md](calls/language-r2-B-Research/content.md)

- L3: 2. Reviewers namen 80 van 640 berichtstrings: 40 Spaans en 40 Pools. Alle 80 behielden de goedgekeurde entitlement-termen, maar zes Spaanse strings keerden de betekenis van de deadline-uitzondering om; in de Poolse steekproef waren er geen deadline-omkeringen. Een tweede reviewer bevestigde de zes gevallen; de overige strings kregen geen tweede semantische review. De bron zegt dat geen van beide resultaten alle 640 strings certificeert. (“Sample and linguistic checks”)
- L6: 5. “Case folding” normaliseerde hoofd- en kleine letters; het voegde geen uitkeringszaken of aanvragen samen. Drie runs op dezelfde 80 strings signaleerden met een steekproefgebaseerde checker respectievelijk zes, vijf en zes semantische problemen. Een menselijke referentiebeoordeling identificeerde de zes deadline-omkeringen. De checkerversie bleef gelijk, maar de steekproefvolgorde varieerde; de runaantallen bewijzen volgens de bron noch drie verschillende grondwaarheden, noch een stabiele checker. (“Processing and reproducibility”)
- L15: - Semantische juistheid van de niet-beoordeelde strings, en dus van alle 640 strings, is niet vastgesteld; de bevinding van zes omkeringen betreft de Spaanse steekproef. (Feit 2)

[projection.md](calls/language-r2-B-Research/projection.md)

- L4: 2. Reviewers namen 80 van 640 berichtstrings: 40 Spaans en 40 Pools. Alle 80 behielden de goedgekeurde entitlement-termen, maar zes Spaanse strings keerden de betekenis van de deadline-uitzondering om; in de Poolse steekproef waren er geen deadline-omkeringen. Een tweede reviewer bevestigde de zes gevallen; de overige strings kregen geen tweede semantische review. De bron zegt dat geen van beide resultaten alle 640 strings certificeert. (“Sample and linguistic checks”)
- L7: 5. “Case folding” normaliseerde hoofd- en kleine letters; het voegde geen uitkeringszaken of aanvragen samen. Drie runs op dezelfde 80 strings signaleerden met een steekproefgebaseerde checker respectievelijk zes, vijf en zes semantische problemen. Een menselijke referentiebeoordeling identificeerde de zes deadline-omkeringen. De checkerversie bleef gelijk, maar de steekproefvolgorde varieerde; de runaantallen bewijzen volgens de bron noch drie verschillende grondwaarheden, noch een stabiele checker. (“Processing and reproducibility”)
- L13: - Semantische juistheid van de niet-beoordeelde strings, en dus van alle 640 strings, is niet vastgesteld; de bevinding van zes omkeringen betreft de Spaanse steekproef. (Feit 2)

## language-r2-B / contrast — kern — correct

Bron [L4](cases/language/source.md):

> All 80 retained the glossary's approved entitlement terms. Six Spanish strings nevertheless reversed the meaning of the deadline exception. The Polish sample had no deadline reversal.

[content.md](calls/language-r2-B-Research/content.md)

- L3: 2. Reviewers namen 80 van 640 berichtstrings: 40 Spaans en 40 Pools. Alle 80 behielden de goedgekeurde entitlement-termen, maar zes Spaanse strings keerden de betekenis van de deadline-uitzondering om; in de Poolse steekproef waren er geen deadline-omkeringen. Een tweede reviewer bevestigde de zes gevallen; de overige strings kregen geen tweede semantische review. De bron zegt dat geen van beide resultaten alle 640 strings certificeert. (“Sample and linguistic checks”)
- L6: 5. “Case folding” normaliseerde hoofd- en kleine letters; het voegde geen uitkeringszaken of aanvragen samen. Drie runs op dezelfde 80 strings signaleerden met een steekproefgebaseerde checker respectievelijk zes, vijf en zes semantische problemen. Een menselijke referentiebeoordeling identificeerde de zes deadline-omkeringen. De checkerversie bleef gelijk, maar de steekproefvolgorde varieerde; de runaantallen bewijzen volgens de bron noch drie verschillende grondwaarheden, noch een stabiele checker. (“Processing and reproducibility”)
- L11: - De positieve terminologie-, contrast- en schermlezercontroles zijn begrensde deelresultaten; ze stellen geen algemene semantische juistheid, toetsenbordtoegang of inclusie vast. (Feiten 1–3)
- L15: - Semantische juistheid van de niet-beoordeelde strings, en dus van alle 640 strings, is niet vastgesteld; de bevinding van zes omkeringen betreft de Spaanse steekproef. (Feit 2)

[projection.md](calls/language-r2-B-Research/projection.md)

- L4: 2. Reviewers namen 80 van 640 berichtstrings: 40 Spaans en 40 Pools. Alle 80 behielden de goedgekeurde entitlement-termen, maar zes Spaanse strings keerden de betekenis van de deadline-uitzondering om; in de Poolse steekproef waren er geen deadline-omkeringen. Een tweede reviewer bevestigde de zes gevallen; de overige strings kregen geen tweede semantische review. De bron zegt dat geen van beide resultaten alle 640 strings certificeert. (“Sample and linguistic checks”)
- L7: 5. “Case folding” normaliseerde hoofd- en kleine letters; het voegde geen uitkeringszaken of aanvragen samen. Drie runs op dezelfde 80 strings signaleerden met een steekproefgebaseerde checker respectievelijk zes, vijf en zes semantische problemen. Een menselijke referentiebeoordeling identificeerde de zes deadline-omkeringen. De checkerversie bleef gelijk, maar de steekproefvolgorde varieerde; de runaantallen bewijzen volgens de bron noch drie verschillende grondwaarheden, noch een stabiele checker. (“Processing and reproducibility”)
- L13: - Semantische juistheid van de niet-beoordeelde strings, en dus van alle 640 strings, is niet vastgesteld; de bevinding van zes omkeringen betreft de Spaanse steekproef. (Feit 2)

## language-r2-B / review — belangrijk — correct

Bron [L4](cases/language/source.md):

> A second reviewer checked the six reversals and agreed. The remaining strings did not receive that second semantic review.

[content.md](calls/language-r2-B-Research/content.md)

- L3: 2. Reviewers namen 80 van 640 berichtstrings: 40 Spaans en 40 Pools. Alle 80 behielden de goedgekeurde entitlement-termen, maar zes Spaanse strings keerden de betekenis van de deadline-uitzondering om; in de Poolse steekproef waren er geen deadline-omkeringen. Een tweede reviewer bevestigde de zes gevallen; de overige strings kregen geen tweede semantische review. De bron zegt dat geen van beide resultaten alle 640 strings certificeert. (“Sample and linguistic checks”)

[projection.md](calls/language-r2-B-Research/projection.md)

- L4: 2. Reviewers namen 80 van 640 berichtstrings: 40 Spaans en 40 Pools. Alle 80 behielden de goedgekeurde entitlement-termen, maar zes Spaanse strings keerden de betekenis van de deadline-uitzondering om; in de Poolse steekproef waren er geen deadline-omkeringen. Een tweede reviewer bevestigde de zes gevallen; de overige strings kregen geen tweede semantische review. De bron zegt dat geen van beide resultaten alle 640 strings certificeert. (“Sample and linguistic checks”)

## language-r2-B / access — kern — correct

Bron [L6](cases/language/source.md):

> An automated colour-contrast scan passed the three tested page templates. Keyboard-only testing could not activate the document-upload button on the second template.

[content.md](calls/language-r2-B-Research/content.md)

- L4: 3. Een automatische kleurcontrastscan slaagde voor de drie geteste paginasjablonen. Op sjabloon twee kon een gebruiker met alleen toetsenbord de uploadknop niet activeren; het defect bleef open. Een schermlezer kondigde daar het uploadlabel correct aan, maar testte geen toetsenbordactivering. Een fixvoorstel was goedgekeurd; een uitgerolde fix is niet vermeld. (“Access and task results”)
- L7: 6. De bijlage zegt dat juridische goedkeuring alleen het Spaanse toestemmingsgedeelte dekt; goedkeuringsdatum en temporele relatie tot de steekproefreview ontbreken. Goedkeuring voor Poolse toestemming is niet gedocumenteerd. Beschikbaarheid van vertalingen bewijst op zichzelf geen juridische goedkeuring. De coördinator gaf toestemming voor een usability-onderzoek met het gecorrigeerde formulier, onder voorwaarde van een geverifieerde uploadfix, en hield algemene uitrol tegen. Het onderzoeksbudget bevat vertaalvergoedingen maar geen personeelsuren; er is geen kostenbesparing aangetoond. (“Status and decision boundary”)
- L11: - De positieve terminologie-, contrast- en schermlezercontroles zijn begrensde deelresultaten; ze stellen geen algemene semantische juistheid, toetsenbordtoegang of inclusie vast. (Feiten 1–3)
- L16: - Brede toegankelijkheid en daadwerkelijke documentupload met toetsenbord blijven onbewezen; de bron meldt een open defect en geen uitgerolde fix. De uitkomst van het voorwaardelijke usability-onderzoek is niet gegeven. (Feit 3, 6)
- L23: - Vooruitkijkend: een usability-onderzoek met het gecorrigeerde formulier is alleen toegestaan na verificatie van de uploadfix; algemene uitrol is tegengehouden. Een uitgerolde fix of onderzoeksresultaat is niet vermeld. (Feit 6)

[projection.md](calls/language-r2-B-Research/projection.md)

- L5: 3. Een automatische kleurcontrastscan slaagde voor de drie geteste paginasjablonen. Op sjabloon twee kon een gebruiker met alleen toetsenbord de uploadknop niet activeren; het defect bleef open. Een schermlezer kondigde daar het uploadlabel correct aan, maar testte geen toetsenbordactivering. Een fixvoorstel was goedgekeurd; een uitgerolde fix is niet vermeld. (“Access and task results”)
- L8: 6. De bijlage zegt dat juridische goedkeuring alleen het Spaanse toestemmingsgedeelte dekt; goedkeuringsdatum en temporele relatie tot de steekproefreview ontbreken. Goedkeuring voor Poolse toestemming is niet gedocumenteerd. Beschikbaarheid van vertalingen bewijst op zichzelf geen juridische goedkeuring. De coördinator gaf toestemming voor een usability-onderzoek met het gecorrigeerde formulier, onder voorwaarde van een geverifieerde uploadfix, en hield algemene uitrol tegen. Het onderzoeksbudget bevat vertaalvergoedingen maar geen personeelsuren; er is geen kostenbesparing aangetoond. (“Status and decision boundary”)
- L14: - Brede toegankelijkheid en daadwerkelijke documentupload met toetsenbord blijven onbewezen; de bron meldt een open defect en geen uitgerolde fix. De uitkomst van het voorwaardelijke usability-onderzoek is niet gegeven. (Feit 3, 6)

## language-r2-B / open — kern — correct

Bron [L6](cases/language/source.md):

> The upload defect was still open. Screen-reader testing announced the upload label correctly on that template, but did not test keyboard activation.

[content.md](calls/language-r2-B-Research/content.md)

- L4: 3. Een automatische kleurcontrastscan slaagde voor de drie geteste paginasjablonen. Op sjabloon twee kon een gebruiker met alleen toetsenbord de uploadknop niet activeren; het defect bleef open. Een schermlezer kondigde daar het uploadlabel correct aan, maar testte geen toetsenbordactivering. Een fixvoorstel was goedgekeurd; een uitgerolde fix is niet vermeld. (“Access and task results”)
- L11: - De positieve terminologie-, contrast- en schermlezercontroles zijn begrensde deelresultaten; ze stellen geen algemene semantische juistheid, toetsenbordtoegang of inclusie vast. (Feiten 1–3)
- L16: - Brede toegankelijkheid en daadwerkelijke documentupload met toetsenbord blijven onbewezen; de bron meldt een open defect en geen uitgerolde fix. De uitkomst van het voorwaardelijke usability-onderzoek is niet gegeven. (Feit 3, 6)

[projection.md](calls/language-r2-B-Research/projection.md)

- L5: 3. Een automatische kleurcontrastscan slaagde voor de drie geteste paginasjablonen. Op sjabloon twee kon een gebruiker met alleen toetsenbord de uploadknop niet activeren; het defect bleef open. Een schermlezer kondigde daar het uploadlabel correct aan, maar testte geen toetsenbordactivering. Een fixvoorstel was goedgekeurd; een uitgerolde fix is niet vermeld. (“Access and task results”)
- L14: - Brede toegankelijkheid en daadwerkelijke documentupload met toetsenbord blijven onbewezen; de bron meldt een open defect en geen uitgerolde fix. De uitkomst van het voorwaardelijke usability-onderzoek is niet gegeven. (Feit 3, 6)

## language-r2-B / outcome — kern — correct

Bron [L7](cases/language/source.md):

> Task completion in the old English-only service was 33 of 50 moderated sessions. The multilingual form recorded 42 of 50 unmoderated sessions.

[content.md](calls/language-r2-B-Research/content.md)

- L5: 4. De oude Engelstalige dienst registreerde voltooiing in 33 van 50 gemodereerde sessies; het meertalige formulier in 42 van 50 ongemodereerde sessies. In de oude dienst betekende voltooiing een definitief ingediende aanvraag; het nieuwe dashboard telde het bereiken van het controlescherm. Deelnemers kwamen uit verschillende wervingskanalen, waren niet willekeurig toegewezen en waren niet gematcht op ervaring. De bron stelt daarom dat de schijnbare verbetering geen vergelijkbare schatting van het voltooiingseffect is. (“Access and task results”)

[projection.md](calls/language-r2-B-Research/projection.md)

- L6: 4. De oude Engelstalige dienst registreerde voltooiing in 33 van 50 gemodereerde sessies; het meertalige formulier in 42 van 50 ongemodereerde sessies. In de oude dienst betekende voltooiing een definitief ingediende aanvraag; het nieuwe dashboard telde het bereiken van het controlescherm. Deelnemers kwamen uit verschillende wervingskanalen, waren niet willekeurig toegewezen en waren niet gematcht op ervaring. De bron stelt daarom dat de schijnbare verbetering geen vergelijkbare schatting van het voltooiingseffect is. (“Access and task results”)

## language-r2-B / method — kern — correct

Bron [L7](cases/language/source.md):

> Completion formerly required a final submitted application; the new dashboard counted reaching the review screen.

[content.md](calls/language-r2-B-Research/content.md)

- L5: 4. De oude Engelstalige dienst registreerde voltooiing in 33 van 50 gemodereerde sessies; het meertalige formulier in 42 van 50 ongemodereerde sessies. In de oude dienst betekende voltooiing een definitief ingediende aanvraag; het nieuwe dashboard telde het bereiken van het controlescherm. Deelnemers kwamen uit verschillende wervingskanalen, waren niet willekeurig toegewezen en waren niet gematcht op ervaring. De bron stelt daarom dat de schijnbare verbetering geen vergelijkbare schatting van het voltooiingseffect is. (“Access and task results”)

[projection.md](calls/language-r2-B-Research/projection.md)

- L6: 4. De oude Engelstalige dienst registreerde voltooiing in 33 van 50 gemodereerde sessies; het meertalige formulier in 42 van 50 ongemodereerde sessies. In de oude dienst betekende voltooiing een definitief ingediende aanvraag; het nieuwe dashboard telde het bereiken van het controlescherm. Deelnemers kwamen uit verschillende wervingskanalen, waren niet willekeurig toegewezen en waren niet gematcht op ervaring. De bron stelt daarom dat de schijnbare verbetering geen vergelijkbare schatting van het voltooiingseffect is. (“Access and task results”)

## language-r2-B / allocation — belangrijk — correct

Bron [L7](cases/language/source.md):

> Participants were recruited from different channels and neither assigned randomly nor matched for prior experience.

[content.md](calls/language-r2-B-Research/content.md)

- L5: 4. De oude Engelstalige dienst registreerde voltooiing in 33 van 50 gemodereerde sessies; het meertalige formulier in 42 van 50 ongemodereerde sessies. In de oude dienst betekende voltooiing een definitief ingediende aanvraag; het nieuwe dashboard telde het bereiken van het controlescherm. Deelnemers kwamen uit verschillende wervingskanalen, waren niet willekeurig toegewezen en waren niet gematcht op ervaring. De bron stelt daarom dat de schijnbare verbetering geen vergelijkbare schatting van het voltooiingseffect is. (“Access and task results”)

[projection.md](calls/language-r2-B-Research/projection.md)

- L6: 4. De oude Engelstalige dienst registreerde voltooiing in 33 van 50 gemodereerde sessies; het meertalige formulier in 42 van 50 ongemodereerde sessies. In de oude dienst betekende voltooiing een definitief ingediende aanvraag; het nieuwe dashboard telde het bereiken van het controlescherm. Deelnemers kwamen uit verschillende wervingskanalen, waren niet willekeurig toegewezen en waren niet gematcht op ervaring. De bron stelt daarom dat de schijnbare verbetering geen vergelijkbare schatting van het voltooiingseffect is. (“Access and task results”)

## language-r2-B / term — kern — correct

Bron [L9](cases/language/source.md):

> A preprocessing step called case folding normalised letter case; it did not merge benefit cases or applications.

[content.md](calls/language-r2-B-Research/content.md)

- L6: 5. “Case folding” normaliseerde hoofd- en kleine letters; het voegde geen uitkeringszaken of aanvragen samen. Drie runs op dezelfde 80 strings signaleerden met een steekproefgebaseerde checker respectievelijk zes, vijf en zes semantische problemen. Een menselijke referentiebeoordeling identificeerde de zes deadline-omkeringen. De checkerversie bleef gelijk, maar de steekproefvolgorde varieerde; de runaantallen bewijzen volgens de bron noch drie verschillende grondwaarheden, noch een stabiele checker. (“Processing and reproducibility”)

[projection.md](calls/language-r2-B-Research/projection.md)

- L7: 5. “Case folding” normaliseerde hoofd- en kleine letters; het voegde geen uitkeringszaken of aanvragen samen. Drie runs op dezelfde 80 strings signaleerden met een steekproefgebaseerde checker respectievelijk zes, vijf en zes semantische problemen. Een menselijke referentiebeoordeling identificeerde de zes deadline-omkeringen. De checkerversie bleef gelijk, maar de steekproefvolgorde varieerde; de runaantallen bewijzen volgens de bron noch drie verschillende grondwaarheden, noch een stabiele checker. (“Processing and reproducibility”)

## language-r2-B / variation — kern — correct

Bron [L9](cases/language/source.md):

> Three runs on the same 80 strings flagged six, five and six semantic issues with a sampling-based checker. A human reference review identified the six deadline reversals.

[content.md](calls/language-r2-B-Research/content.md)

- L6: 5. “Case folding” normaliseerde hoofd- en kleine letters; het voegde geen uitkeringszaken of aanvragen samen. Drie runs op dezelfde 80 strings signaleerden met een steekproefgebaseerde checker respectievelijk zes, vijf en zes semantische problemen. Een menselijke referentiebeoordeling identificeerde de zes deadline-omkeringen. De checkerversie bleef gelijk, maar de steekproefvolgorde varieerde; de runaantallen bewijzen volgens de bron noch drie verschillende grondwaarheden, noch een stabiele checker. (“Processing and reproducibility”)
- L22: - In de volgorde van de notitie: steekproef- en taalcontroles (feit 2), toegangstests en taakresultaten (feiten 3–4), checker-runs en reproduceerbaarheidsbeperkingen (feit 5), vervolgens status en besluit (feit 6) en de poll (feit 7). Dit is de documentvolgorde, geen vastgestelde chronologie van de gebeurtenissen.

[projection.md](calls/language-r2-B-Research/projection.md)

- L7: 5. “Case folding” normaliseerde hoofd- en kleine letters; het voegde geen uitkeringszaken of aanvragen samen. Drie runs op dezelfde 80 strings signaleerden met een steekproefgebaseerde checker respectievelijk zes, vijf en zes semantische problemen. Een menselijke referentiebeoordeling identificeerde de zes deadline-omkeringen. De checkerversie bleef gelijk, maar de steekproefvolgorde varieerde; de runaantallen bewijzen volgens de bron noch drie verschillende grondwaarheden, noch een stabiele checker. (“Processing and reproducibility”)

## language-r2-B / legal — kern — correct

Bron [L11](cases/language/source.md):

> The appendix says legal approval covers the Spanish consent paragraph only. It gives no approval date or temporal relationship to the sample review. Polish consent approval is not documented.

[content.md](calls/language-r2-B-Research/content.md)

- L7: 6. De bijlage zegt dat juridische goedkeuring alleen het Spaanse toestemmingsgedeelte dekt; goedkeuringsdatum en temporele relatie tot de steekproefreview ontbreken. Goedkeuring voor Poolse toestemming is niet gedocumenteerd. Beschikbaarheid van vertalingen bewijst op zichzelf geen juridische goedkeuring. De coördinator gaf toestemming voor een usability-onderzoek met het gecorrigeerde formulier, onder voorwaarde van een geverifieerde uploadfix, en hield algemene uitrol tegen. Het onderzoeksbudget bevat vertaalvergoedingen maar geen personeelsuren; er is geen kostenbesparing aangetoond. (“Status and decision boundary”)
- L18: - De temporele relatie en datum van de Spaanse juridische goedkeuring, Poolse juridische goedkeuring en algemene juridische goedkeuring zijn niet vastgesteld. (Feit 6)

[projection.md](calls/language-r2-B-Research/projection.md)

- L8: 6. De bijlage zegt dat juridische goedkeuring alleen het Spaanse toestemmingsgedeelte dekt; goedkeuringsdatum en temporele relatie tot de steekproefreview ontbreken. Goedkeuring voor Poolse toestemming is niet gedocumenteerd. Beschikbaarheid van vertalingen bewijst op zichzelf geen juridische goedkeuring. De coördinator gaf toestemming voor een usability-onderzoek met het gecorrigeerde formulier, onder voorwaarde van een geverifieerde uploadfix, en hield algemene uitrol tegen. Het onderzoeksbudget bevat vertaalvergoedingen maar geen personeelsuren; er is geen kostenbesparing aangetoond. (“Status and decision boundary”)
- L16: - De temporele relatie en datum van de Spaanse juridische goedkeuring, Poolse juridische goedkeuring en algemene juridische goedkeuring zijn niet vastgesteld. (Feit 6)

## language-r2-B / decision — kern — correct

Bron [L11](cases/language/source.md):

> The coordinator authorised a corrected-form usability study, conditional on a verified upload fix, and withheld general release.

[content.md](calls/language-r2-B-Research/content.md)

- L7: 6. De bijlage zegt dat juridische goedkeuring alleen het Spaanse toestemmingsgedeelte dekt; goedkeuringsdatum en temporele relatie tot de steekproefreview ontbreken. Goedkeuring voor Poolse toestemming is niet gedocumenteerd. Beschikbaarheid van vertalingen bewijst op zichzelf geen juridische goedkeuring. De coördinator gaf toestemming voor een usability-onderzoek met het gecorrigeerde formulier, onder voorwaarde van een geverifieerde uploadfix, en hield algemene uitrol tegen. Het onderzoeksbudget bevat vertaalvergoedingen maar geen personeelsuren; er is geen kostenbesparing aangetoond. (“Status and decision boundary”)
- L16: - Brede toegankelijkheid en daadwerkelijke documentupload met toetsenbord blijven onbewezen; de bron meldt een open defect en geen uitgerolde fix. De uitkomst van het voorwaardelijke usability-onderzoek is niet gegeven. (Feit 3, 6)
- L23: - Vooruitkijkend: een usability-onderzoek met het gecorrigeerde formulier is alleen toegestaan na verificatie van de uploadfix; algemene uitrol is tegengehouden. Een uitgerolde fix of onderzoeksresultaat is niet vermeld. (Feit 6)

[projection.md](calls/language-r2-B-Research/projection.md)

- L8: 6. De bijlage zegt dat juridische goedkeuring alleen het Spaanse toestemmingsgedeelte dekt; goedkeuringsdatum en temporele relatie tot de steekproefreview ontbreken. Goedkeuring voor Poolse toestemming is niet gedocumenteerd. Beschikbaarheid van vertalingen bewijst op zichzelf geen juridische goedkeuring. De coördinator gaf toestemming voor een usability-onderzoek met het gecorrigeerde formulier, onder voorwaarde van een geverifieerde uploadfix, en hield algemene uitrol tegen. Het onderzoeksbudget bevat vertaalvergoedingen maar geen personeelsuren; er is geen kostenbesparing aangetoond. (“Status and decision boundary”)
- L14: - Brede toegankelijkheid en daadwerkelijke documentupload met toetsenbord blijven onbewezen; de bron meldt een open defect en geen uitgerolde fix. De uitkomst van het voorwaardelijke usability-onderzoek is niet gegeven. (Feit 3, 6)

## heat-r2-A / ledger — kern — correct

Bron [L4](cases/heat/source.md):

> The final metered record shows 410 kWh of useful heat from 500 kWh electrical input over the trial boundary.

[content.md](calls/heat-r2-A-Research/content.md)

- L3: 1. The final metered record reports 410 kWh of useful heat from 500 kWh of electrical input over the trial boundary.
- L6: 4. The 410/500 ratio applies to the trial boundary and period, not seasonal efficiency.
- L32: 30. The simulations are not three additional metered field trials and do not supersede the measured 410 kWh.
- L52: - The source does not establish a common-boundary comparison between the legacy boiler's displayed 76% and the trial's 410/500 ratio; it states that no common-boundary reanalysis exists.
- L64: - A draft listed 450 kWh of useful heat; 40 kWh duplicated during meter-file concatenation was removed; the final metered record reports 410 kWh of useful heat from 500 kWh electrical input. (Facts 1–3)

[projection.md](calls/heat-r2-A-Research/projection.md)

- L3: 1. The final metered record reports 410 kWh of useful heat from 500 kWh of electrical input over the trial boundary.
- L6: 4. The 410/500 ratio applies to the trial boundary and period, not seasonal efficiency.
- L32: 30. The simulations are not three additional metered field trials and do not supersede the measured 410 kWh.
- L48: - The source does not establish a common-boundary comparison between the legacy boiler's displayed 76% and the trial's 410/500 ratio; it states that no common-boundary reanalysis exists.

## heat-r2-A / method — kern — correct

Bron [L4](cases/heat/source.md):

> The legacy boiler's displayed 76% used fuel energy on a lower-heating-value basis, while the electrical trial used meter input and included pump demand. No common-boundary reanalysis exists.

[content.md](calls/heat-r2-A-Research/content.md)

- L7: 5. The legacy boiler's displayed 76% used fuel energy on a lower-heating-value basis.
- L8: 6. The electrical trial used meter input and included pump demand.
- L9: 7. No common-boundary reanalysis exists.
- L52: - The source does not establish a common-boundary comparison between the legacy boiler's displayed 76% and the trial's 410/500 ratio; it states that no common-boundary reanalysis exists.

[projection.md](calls/heat-r2-A-Research/projection.md)

- L7: 5. The legacy boiler's displayed 76% used fuel energy on a lower-heating-value basis.
- L8: 6. The electrical trial used meter input and included pump demand.
- L9: 7. No common-boundary reanalysis exists.
- L48: - The source does not establish a common-boundary comparison between the legacy boiler's displayed 76% and the trial's 410/500 ratio; it states that no common-boundary reanalysis exists.

## heat-r2-A / conditions — belangrijk — correct

Bron [L5](cases/heat/source.md):

> Trial shifts processed lighter products and ambient temperature was 8 degrees higher than in the boiler reference shifts.

[content.md](calls/heat-r2-A-Research/content.md)

- L10: 8. Trial shifts processed lighter products than the boiler reference shifts.
- L11: 9. Ambient temperature was 8 degrees higher during the trial shifts than during the boiler reference shifts.

[projection.md](calls/heat-r2-A-Research/projection.md)

- L10: 8. Trial shifts processed lighter products than the boiler reference shifts.
- L11: 9. Ambient temperature was 8 degrees higher during the trial shifts than during the boiler reference shifts.

## heat-r2-A / allocation — belangrijk — correct

Bron [L5](cases/heat/source.md):

> Operators selected the trial shifts to avoid peak production. There was no random assignment or matched control.

[content.md](calls/heat-r2-A-Research/content.md)

- L12: 10. Operators selected the trial shifts to avoid peak production.
- L13: 11. There was no random assignment or matched control.

[projection.md](calls/heat-r2-A-Research/projection.md)

- L12: 10. Operators selected the trial shifts to avoid peak production.
- L13: 11. There was no random assignment or matched control.

## heat-r2-A / contrast — kern — correct

Bron [L7](cases/heat/source.md):

> All four vessels passed the specified pressure-hold test. Two nevertheless failed the insulation surface-temperature check at the access hatch.

[content.md](calls/heat-r2-A-Research/content.md)

- L17: 15. All four vessels passed the specified pressure-hold test.
- L18: 16. Two vessels failed the insulation surface-temperature check at the access hatch.
- L38: 36. Procurement approved only a longer instrumented trial conditional on a verified hatch remedy.
- L54: - The source does not establish acceptable touch temperature at the two access hatches at sign-off; the hot surfaces remained unresolved and the proposed shield was not installed.
- L59: - The source does not state why procurement made the longer trial conditional on a verified hatch remedy, beyond reporting that condition.
- L67: - Procurement approved only a longer instrumented trial conditional on a verified hatch remedy; no purchase or factory-wide roll-out was approved. The source does not establish the date of this decision relative to the other reported trial events. (Facts 36–37)
- L72: - Facts 15–21 concern pressure, surface-temperature, and interlock checks.

[projection.md](calls/heat-r2-A-Research/projection.md)

- L17: 15. All four vessels passed the specified pressure-hold test.
- L18: 16. Two vessels failed the insulation surface-temperature check at the access hatch.
- L38: 36. Procurement approved only a longer instrumented trial conditional on a verified hatch remedy.
- L50: - The source does not establish acceptable touch temperature at the two access hatches at sign-off; the hot surfaces remained unresolved and the proposed shield was not installed.
- L55: - The source does not state why procurement made the longer trial conditional on a verified hatch remedy, beyond reporting that condition.

## heat-r2-A / open — kern — correct

Bron [L7](cases/heat/source.md):

> The hot surfaces remained unresolved at sign-off, and a proposed shield was not installed.

[content.md](calls/heat-r2-A-Research/content.md)

- L19: 17. The hot surfaces remained unresolved at sign-off.
- L20: 18. A proposed shield was not installed.
- L54: - The source does not establish acceptable touch temperature at the two access hatches at sign-off; the hot surfaces remained unresolved and the proposed shield was not installed.
- L66: - At sign-off, hot surfaces remained unresolved and a proposed shield had not been installed. (Facts 17–18)

[projection.md](calls/heat-r2-A-Research/projection.md)

- L19: 17. The hot surfaces remained unresolved at sign-off.
- L20: 18. A proposed shield was not installed.
- L50: - The source does not establish acceptable touch temperature at the two access hatches at sign-off; the hot surfaces remained unresolved and the proposed shield was not installed.

## heat-r2-A / interlock — belangrijk — correct

Bron [L7](cases/heat/source.md):

> An interlock test passed for all four vessels; it did not test the hot-surface problem.

[content.md](calls/heat-r2-A-Research/content.md)

- L22: 20. All four vessels passed an interlock test.
- L23: 21. The interlock test did not test the hot-surface problem.
- L72: - Facts 15–21 concern pressure, surface-temperature, and interlock checks.

[projection.md](calls/heat-r2-A-Research/projection.md)

- L22: 20. All four vessels passed an interlock test.
- L23: 21. The interlock test did not test the hot-surface problem.

## heat-r2-A / term — kern — correct

Bron [L8](cases/heat/source.md):

> Three thermal trips occurred during initial charging. Here a thermal trip is a protective over-temperature shutdown, not a production journey or heat-delivery cycle.

[content.md](calls/heat-r2-A-Research/content.md)

- L24: 22. Three thermal trips occurred during initial charging.
- L25: 23. The source defines a thermal trip as a protective over-temperature shutdown, not a production journey or heat-delivery cycle.
- L65: - Three thermal trips occurred during initial charging. A controller setting and sensor mounting were changed together before a repeat charge, which had zero trips. The separate contributions of the two changes are UNKNOWN. (Facts 22–27)
- L73: - Facts 22–31 concern thermal trips, the repeat charge, simulations, and attribution limits.

[projection.md](calls/heat-r2-A-Research/projection.md)

- L24: 22. Three thermal trips occurred during initial charging.
- L25: 23. The source defines a thermal trip as a protective over-temperature shutdown, not a production journey or heat-delivery cycle.

## heat-r2-A / attribution — kern — correct

Bron [L8](cases/heat/source.md):

> A controller setting and sensor mounting were changed together before a repeat charge with zero trips. The record cannot separate the two actions' contributions.

[content.md](calls/heat-r2-A-Research/content.md)

- L26: 24. A controller setting and sensor mounting were changed together before a repeat charge.
- L27: 25. The repeat charge had zero trips.
- L28: 26. The record says it cannot separate the contributions of the controller-setting and sensor-mounting changes.
- L29: 27. Zero trips in one repeat does not establish long-term reliability.
- L55: - The record does not establish the separate contributions of the controller-setting change and sensor-mounting change to the zero-trip repeat charge.
- L65: - Three thermal trips occurred during initial charging. A controller setting and sensor mounting were changed together before a repeat charge, which had zero trips. The separate contributions of the two changes are UNKNOWN. (Facts 22–27)

[projection.md](calls/heat-r2-A-Research/projection.md)

- L26: 24. A controller setting and sensor mounting were changed together before a repeat charge.
- L27: 25. The repeat charge had zero trips.
- L28: 26. The record says it cannot separate the contributions of the controller-setting and sensor-mounting changes.
- L29: 27. Zero trips in one repeat does not establish long-term reliability.
- L51: - The record does not establish the separate contributions of the controller-setting change and sensor-mounting change to the zero-trip repeat charge.

## heat-r2-A / variation — kern — correct

Bron [L10](cases/heat/source.md):

> Three identical-input simulation runs estimated usable discharge at 402, 418 and 407 kWh; the solver used stochastic initialisation.

[content.md](calls/heat-r2-A-Research/content.md)

- L30: 28. Three identical-input simulation runs estimated usable discharge at 402, 418, and 407 kWh.
- L31: 29. The solver used stochastic initialisation.
- L32: 30. The simulations are not three additional metered field trials and do not supersede the measured 410 kWh.
- L33: 31. The source identifies solver variation as an attribution limit for interpreting simulation improvements.
- L57: - The source does not establish that the simulation estimates are additional metered field results or that solver variation represents a change in field performance.
- L73: - Facts 22–31 concern thermal trips, the repeat charge, simulations, and attribution limits.

[projection.md](calls/heat-r2-A-Research/projection.md)

- L30: 28. Three identical-input simulation runs estimated usable discharge at 402, 418, and 407 kWh.
- L31: 29. The solver used stochastic initialisation.
- L32: 30. The simulations are not three additional metered field trials and do not supersede the measured 410 kWh.
- L33: 31. The source identifies solver variation as an attribution limit for interpreting simulation improvements.
- L53: - The source does not establish that the simulation estimates are additional metered field results or that solver variation represents a change in field performance.

## heat-r2-A / cost — belangrijk — correct

Bron [L11](cases/heat/source.md):

> The finance appendix reports a EUR 9,200 rental allowance. It gives no date or sequence relative to testing. The allowance excludes installation, downtime and maintenance, so no payback claim is supported.

[content.md](calls/heat-r2-A-Research/content.md)

- L34: 32. The finance appendix reports a EUR 9,200 rental allowance.
- L36: 34. The allowance excludes installation, downtime, and maintenance.
- L37: 35. The source says no payback claim is supported.
- L58: - The source gives no date or sequence for the rental allowance relative to testing, and states that no payback claim is supported.
- L74: - Facts 32–37 concern the rental allowance and procurement decision.

[projection.md](calls/heat-r2-A-Research/projection.md)

- L34: 32. The finance appendix reports a EUR 9,200 rental allowance.
- L36: 34. The allowance excludes installation, downtime, and maintenance.
- L37: 35. The source says no payback claim is supported.
- L54: - The source gives no date or sequence for the rental allowance relative to testing, and states that no payback claim is supported.

## heat-r2-A / decision — kern — correct

Bron [L11](cases/heat/source.md):

> Procurement approved only a longer instrumented trial conditional on a verified hatch remedy; no purchase or factory-wide roll-out was approved.

[content.md](calls/heat-r2-A-Research/content.md)

- L38: 36. Procurement approved only a longer instrumented trial conditional on a verified hatch remedy.
- L39: 37. No purchase or factory-wide roll-out was approved.
- L59: - The source does not state why procurement made the longer trial conditional on a verified hatch remedy, beyond reporting that condition.
- L67: - Procurement approved only a longer instrumented trial conditional on a verified hatch remedy; no purchase or factory-wide roll-out was approved. The source does not establish the date of this decision relative to the other reported trial events. (Facts 36–37)
- L74: - Facts 32–37 concern the rental allowance and procurement decision.

[projection.md](calls/heat-r2-A-Research/projection.md)

- L38: 36. Procurement approved only a longer instrumented trial conditional on a verified hatch remedy.
- L39: 37. No purchase or factory-wide roll-out was approved.
- L55: - The source does not state why procurement made the longer trial conditional on a verified hatch remedy, beyond reporting that condition.

## heat-r2-B / ledger — kern — correct

Bron [L4](cases/heat/source.md):

> The final metered record shows 410 kWh of useful heat from 500 kWh electrical input over the trial boundary.

[content.md](calls/heat-r2-B-Research/content.md)

- L2: 1. The final metered record reports 410 kWh useful heat from 500 kWh electrical input over the trial boundary. A draft’s 450 kWh figure included 40 kWh duplicated during meter-file concatenation, which was removed. The ratio is for this period and boundary, not seasonal efficiency. (Performance and measurement)
- L10: 9. Three identical-input simulation runs estimated usable discharge at 402, 418 and 407 kWh. The solver used stochastic initialisation; the simulations are not additional metered field trials and do not supersede the measured 410 kWh. Solver variation limits attribution of simulation improvements. (Variability, sequencing and decision)

[projection.md](calls/heat-r2-B-Research/projection.md)

- L3: 1. The final metered record reports 410 kWh useful heat from 500 kWh electrical input over the trial boundary. A draft’s 450 kWh figure included 40 kWh duplicated during meter-file concatenation, which was removed. The ratio is for this period and boundary, not seasonal efficiency. (Performance and measurement)
- L11: 9. Three identical-input simulation runs estimated usable discharge at 402, 418 and 407 kWh. The solver used stochastic initialisation; the simulations are not additional metered field trials and do not supersede the measured 410 kWh. Solver variation limits attribution of simulation improvements. (Variability, sequencing and decision)

## heat-r2-B / method — kern — correct

Bron [L4](cases/heat/source.md):

> The legacy boiler's displayed 76% used fuel energy on a lower-heating-value basis, while the electrical trial used meter input and included pump demand. No common-boundary reanalysis exists.

[content.md](calls/heat-r2-B-Research/content.md)

- L3: 2. The legacy boiler’s displayed 76% used fuel energy on a lower-heating-value basis; the electrical trial used meter input and included pump demand. No common-boundary reanalysis exists. (Performance and measurement)
- L21: - The source does not establish seasonal efficiency or a comparable efficiency advantage over the legacy boiler; the boundaries and energy bases differ and there is no common-boundary reanalysis (Facts 1–2).

[projection.md](calls/heat-r2-B-Research/projection.md)

- L4: 2. The legacy boiler’s displayed 76% used fuel energy on a lower-heating-value basis; the electrical trial used meter input and included pump demand. No common-boundary reanalysis exists. (Performance and measurement)
- L19: - The source does not establish seasonal efficiency or a comparable efficiency advantage over the legacy boiler; the boundaries and energy bases differ and there is no common-boundary reanalysis (Facts 1–2).

## heat-r2-B / conditions — belangrijk — correct

Bron [L5](cases/heat/source.md):

> Trial shifts processed lighter products and ambient temperature was 8 degrees higher than in the boiler reference shifts.

[content.md](calls/heat-r2-B-Research/content.md)

- L4: 3. Trial shifts processed lighter products, had ambient temperature 8 degrees higher than boiler-reference shifts, and were selected to avoid peak production. There was no random assignment or matched control. (Performance and measurement)

[projection.md](calls/heat-r2-B-Research/projection.md)

- L5: 3. Trial shifts processed lighter products, had ambient temperature 8 degrees higher than boiler-reference shifts, and were selected to avoid peak production. There was no random assignment or matched control. (Performance and measurement)

## heat-r2-B / allocation — belangrijk — correct

Bron [L5](cases/heat/source.md):

> Operators selected the trial shifts to avoid peak production. There was no random assignment or matched control.

[content.md](calls/heat-r2-B-Research/content.md)

- L4: 3. Trial shifts processed lighter products, had ambient temperature 8 degrees higher than boiler-reference shifts, and were selected to avoid peak production. There was no random assignment or matched control. (Performance and measurement)

[projection.md](calls/heat-r2-B-Research/projection.md)

- L5: 3. Trial shifts processed lighter products, had ambient temperature 8 degrees higher than boiler-reference shifts, and were selected to avoid peak production. There was no random assignment or matched control. (Performance and measurement)

## heat-r2-B / contrast — kern — correct

Bron [L7](cases/heat/source.md):

> All four vessels passed the specified pressure-hold test. Two nevertheless failed the insulation surface-temperature check at the access hatch.

[content.md](calls/heat-r2-B-Research/content.md)

- L6: 5. All four vessels passed the specified pressure-hold test, but two failed the insulation surface-temperature check at the access hatch. Hot surfaces remained unresolved at sign-off, and a proposed shield was not installed. (Successful and failed controls)
- L12: 11. Procurement approved only a longer instrumented trial conditional on a verified hatch remedy. No purchase or factory-wide roll-out was approved. (Variability, sequencing and decision)
- L18: - Positive pressure-hold and interlock results coexist with an unresolved hot-surface failure; they establish different checks, not overall safety acceptance (Facts 5–6).
- L23: - Acceptable touch temperature or resolution of the hatch hazard is not established: two checks failed, the issue remained unresolved at sign-off, and the proposed shield was not installed (Facts 5–6).
- L31: - Procurement subsequently approved only a longer instrumented trial, conditional on a verified hatch remedy; no purchase or factory-wide roll-out was approved (Fact 11). The source does not date the rental allowance relative to testing (Fact 10).

[projection.md](calls/heat-r2-B-Research/projection.md)

- L7: 5. All four vessels passed the specified pressure-hold test, but two failed the insulation surface-temperature check at the access hatch. Hot surfaces remained unresolved at sign-off, and a proposed shield was not installed. (Successful and failed controls)
- L13: 11. Procurement approved only a longer instrumented trial conditional on a verified hatch remedy. No purchase or factory-wide roll-out was approved. (Variability, sequencing and decision)
- L21: - Acceptable touch temperature or resolution of the hatch hazard is not established: two checks failed, the issue remained unresolved at sign-off, and the proposed shield was not installed (Facts 5–6).

## heat-r2-B / open — kern — correct

Bron [L7](cases/heat/source.md):

> The hot surfaces remained unresolved at sign-off, and a proposed shield was not installed.

[content.md](calls/heat-r2-B-Research/content.md)

- L6: 5. All four vessels passed the specified pressure-hold test, but two failed the insulation surface-temperature check at the access hatch. Hot surfaces remained unresolved at sign-off, and a proposed shield was not installed. (Successful and failed controls)
- L18: - Positive pressure-hold and interlock results coexist with an unresolved hot-surface failure; they establish different checks, not overall safety acceptance (Facts 5–6).
- L23: - Acceptable touch temperature or resolution of the hatch hazard is not established: two checks failed, the issue remained unresolved at sign-off, and the proposed shield was not installed (Facts 5–6).

[projection.md](calls/heat-r2-B-Research/projection.md)

- L7: 5. All four vessels passed the specified pressure-hold test, but two failed the insulation surface-temperature check at the access hatch. Hot surfaces remained unresolved at sign-off, and a proposed shield was not installed. (Successful and failed controls)
- L21: - Acceptable touch temperature or resolution of the hatch hazard is not established: two checks failed, the issue remained unresolved at sign-off, and the proposed shield was not installed (Facts 5–6).

## heat-r2-B / interlock — belangrijk — correct

Bron [L7](cases/heat/source.md):

> An interlock test passed for all four vessels; it did not test the hot-surface problem.

[content.md](calls/heat-r2-B-Research/content.md)

- L7: 6. The interlock test passed for all four vessels, but did not test the hot-surface problem. The source states that passing the pressure test does not demonstrate acceptable touch temperature. (Successful and failed controls)
- L18: - Positive pressure-hold and interlock results coexist with an unresolved hot-surface failure; they establish different checks, not overall safety acceptance (Facts 5–6).

[projection.md](calls/heat-r2-B-Research/projection.md)

- L8: 6. The interlock test passed for all four vessels, but did not test the hot-surface problem. The source states that passing the pressure test does not demonstrate acceptable touch temperature. (Successful and failed controls)

## heat-r2-B / term — kern — correct

Bron [L8](cases/heat/source.md):

> Three thermal trips occurred during initial charging. Here a thermal trip is a protective over-temperature shutdown, not a production journey or heat-delivery cycle.

[content.md](calls/heat-r2-B-Research/content.md)

- L8: 7. Three thermal trips occurred during initial charging; the source defines these as protective over-temperature shutdowns, not production journeys or heat-delivery cycles. (Successful and failed controls)
- L30: - Initial charging produced three protective thermal trips; the controller setting and sensor mounting were then changed together, followed by one repeat charge with zero trips (Facts 7–8).

[projection.md](calls/heat-r2-B-Research/projection.md)

- L9: 7. Three thermal trips occurred during initial charging; the source defines these as protective over-temperature shutdowns, not production journeys or heat-delivery cycles. (Successful and failed controls)

## heat-r2-B / attribution — kern — correct

Bron [L8](cases/heat/source.md):

> A controller setting and sensor mounting were changed together before a repeat charge with zero trips. The record cannot separate the two actions' contributions.

[content.md](calls/heat-r2-B-Research/content.md)

- L9: 8. A controller setting and sensor mounting were changed together before a repeat charge with zero trips. The record cannot separate their contributions, and states that zero trips in one repeat does not establish long-term reliability. (Successful and failed controls)
- L24: - The cause of the zero-trip repeat result and long-term trip reliability remain unestablished because two changes were made together and only one repeat charge is reported (Fact 8).
- L30: - Initial charging produced three protective thermal trips; the controller setting and sensor mounting were then changed together, followed by one repeat charge with zero trips (Facts 7–8).

[projection.md](calls/heat-r2-B-Research/projection.md)

- L10: 8. A controller setting and sensor mounting were changed together before a repeat charge with zero trips. The record cannot separate their contributions, and states that zero trips in one repeat does not establish long-term reliability. (Successful and failed controls)
- L22: - The cause of the zero-trip repeat result and long-term trip reliability remain unestablished because two changes were made together and only one repeat charge is reported (Fact 8).

## heat-r2-B / variation — kern — correct

Bron [L10](cases/heat/source.md):

> Three identical-input simulation runs estimated usable discharge at 402, 418 and 407 kWh; the solver used stochastic initialisation.

[content.md](calls/heat-r2-B-Research/content.md)

- L10: 9. Three identical-input simulation runs estimated usable discharge at 402, 418 and 407 kWh. The solver used stochastic initialisation; the simulations are not additional metered field trials and do not supersede the measured 410 kWh. Solver variation limits attribution of simulation improvements. (Variability, sequencing and decision)
- L25: - The simulations do not establish additional field performance or a measured improvement; stochastic solver variation limits attribution (Fact 9).

[projection.md](calls/heat-r2-B-Research/projection.md)

- L11: 9. Three identical-input simulation runs estimated usable discharge at 402, 418 and 407 kWh. The solver used stochastic initialisation; the simulations are not additional metered field trials and do not supersede the measured 410 kWh. Solver variation limits attribution of simulation improvements. (Variability, sequencing and decision)
- L23: - The simulations do not establish additional field performance or a measured improvement; stochastic solver variation limits attribution (Fact 9).

## heat-r2-B / cost — belangrijk — correct

Bron [L11](cases/heat/source.md):

> The finance appendix reports a EUR 9,200 rental allowance. It gives no date or sequence relative to testing. The allowance excludes installation, downtime and maintenance, so no payback claim is supported.

[content.md](calls/heat-r2-B-Research/content.md)

- L11: 10. The finance appendix reports a EUR 9,200 rental allowance but gives no date or sequence relative to testing. It excludes installation, downtime and maintenance; the source supports no payback claim. (Variability, sequencing and decision)
- L26: - Payback, the rental allowance’s timing relative to tests, measured noise levels, and a physical sensor replacement are not established (Facts 10, 12–13).
- L31: - Procurement subsequently approved only a longer instrumented trial, conditional on a verified hatch remedy; no purchase or factory-wide roll-out was approved (Fact 11). The source does not date the rental allowance relative to testing (Fact 10).

[projection.md](calls/heat-r2-B-Research/projection.md)

- L12: 10. The finance appendix reports a EUR 9,200 rental allowance but gives no date or sequence relative to testing. It excludes installation, downtime and maintenance; the source supports no payback claim. (Variability, sequencing and decision)
- L24: - Payback, the rental allowance’s timing relative to tests, measured noise levels, and a physical sensor replacement are not established (Facts 10, 12–13).

## heat-r2-B / decision — kern — correct

Bron [L11](cases/heat/source.md):

> Procurement approved only a longer instrumented trial conditional on a verified hatch remedy; no purchase or factory-wide roll-out was approved.

[content.md](calls/heat-r2-B-Research/content.md)

- L12: 11. Procurement approved only a longer instrumented trial conditional on a verified hatch remedy. No purchase or factory-wide roll-out was approved. (Variability, sequencing and decision)
- L27: - A purchase or factory-wide deployment was not approved; the source establishes only conditional approval for a longer instrumented trial (Fact 11).
- L31: - Procurement subsequently approved only a longer instrumented trial, conditional on a verified hatch remedy; no purchase or factory-wide roll-out was approved (Fact 11). The source does not date the rental allowance relative to testing (Fact 10).

[projection.md](calls/heat-r2-B-Research/projection.md)

- L13: 11. Procurement approved only a longer instrumented trial conditional on a verified hatch remedy. No purchase or factory-wide roll-out was approved. (Variability, sequencing and decision)
- L25: - A purchase or factory-wide deployment was not approved; the source establishes only conditional approval for a longer instrumented trial (Fact 11).

## heat-r2-B2 / ledger — kern — correct

Bron [L4](cases/heat/source.md):

> The final metered record shows 410 kWh of useful heat from 500 kWh electrical input over the trial boundary.

[content.md](calls/heat-r2-B2-Research/content.md)

- L3: 2. The final metered record reports 410 kWh useful heat from 500 kWh electrical input. A draft’s 450 kWh useful-heat figure included 40 kWh duplicated during meter-file concatenation, which was removed. (Performance and measurement)
- L4: 3. The 410/500 ratio applies only to the trial boundary and period. The legacy boiler’s displayed 76% used fuel energy on a lower-heating-value basis; the electrical trial used meter input and included pump demand. The source says there is no common-boundary reanalysis. (Performance and measurement)
- L9: 8. Three identical-input simulation runs estimated usable discharge at 402, 418 and 407 kWh; the solver used stochastic initialisation. The source says these are not additional metered field trials and do not supersede the measured 410 kWh, and identifies solver variation as an attribution limit for interpreting simulation improvements. (Variability, sequencing and decision)
- L28: - A draft useful-heat figure of 450 kWh was corrected after 40 kWh of duplicated meter data was removed; the final record reports 410 kWh (Fact 2).

[projection.md](calls/heat-r2-B2-Research/projection.md)

- L4: 2. The final metered record reports 410 kWh useful heat from 500 kWh electrical input. A draft’s 450 kWh useful-heat figure included 40 kWh duplicated during meter-file concatenation, which was removed. (Performance and measurement)
- L5: 3. The 410/500 ratio applies only to the trial boundary and period. The legacy boiler’s displayed 76% used fuel energy on a lower-heating-value basis; the electrical trial used meter input and included pump demand. The source says there is no common-boundary reanalysis. (Performance and measurement)
- L10: 8. Three identical-input simulation runs estimated usable discharge at 402, 418 and 407 kWh; the solver used stochastic initialisation. The source says these are not additional metered field trials and do not supersede the measured 410 kWh, and identifies solver variation as an attribution limit for interpreting simulation improvements. (Variability, sequencing and decision)

## heat-r2-B2 / method — kern — correct

Bron [L4](cases/heat/source.md):

> The legacy boiler's displayed 76% used fuel energy on a lower-heating-value basis, while the electrical trial used meter input and included pump demand. No common-boundary reanalysis exists.

[content.md](calls/heat-r2-B2-Research/content.md)

- L4: 3. The 410/500 ratio applies only to the trial boundary and period. The legacy boiler’s displayed 76% used fuel energy on a lower-heating-value basis; the electrical trial used meter input and included pump demand. The source says there is no common-boundary reanalysis. (Performance and measurement)
- L19: - Seasonal efficiency and a directly comparable boiler-versus-store efficiency result are not established; the reported ratio is limited to the trial boundary and the source reports no common-boundary reanalysis (Facts 2–3).

[projection.md](calls/heat-r2-B2-Research/projection.md)

- L5: 3. The 410/500 ratio applies only to the trial boundary and period. The legacy boiler’s displayed 76% used fuel energy on a lower-heating-value basis; the electrical trial used meter input and included pump demand. The source says there is no common-boundary reanalysis. (Performance and measurement)
- L18: - Seasonal efficiency and a directly comparable boiler-versus-store efficiency result are not established; the reported ratio is limited to the trial boundary and the source reports no common-boundary reanalysis (Facts 2–3).

## heat-r2-B2 / conditions — belangrijk — correct

Bron [L5](cases/heat/source.md):

> Trial shifts processed lighter products and ambient temperature was 8 degrees higher than in the boiler reference shifts.

[content.md](calls/heat-r2-B2-Research/content.md)

- L5: 4. Trial shifts processed lighter products, had ambient temperature 8 degrees higher than the boiler-reference shifts, and were selected by operators to avoid peak production. There was no random assignment or matched control. The record says it cannot isolate a storage-technology effect or quantify avoided emissions; grid-carbon intensity was not measured. (Performance and measurement)

[projection.md](calls/heat-r2-B2-Research/projection.md)

- L6: 4. Trial shifts processed lighter products, had ambient temperature 8 degrees higher than the boiler-reference shifts, and were selected by operators to avoid peak production. There was no random assignment or matched control. The record says it cannot isolate a storage-technology effect or quantify avoided emissions; grid-carbon intensity was not measured. (Performance and measurement)

## heat-r2-B2 / allocation — belangrijk — correct

Bron [L5](cases/heat/source.md):

> Operators selected the trial shifts to avoid peak production. There was no random assignment or matched control.

[content.md](calls/heat-r2-B2-Research/content.md)

- L5: 4. Trial shifts processed lighter products, had ambient temperature 8 degrees higher than the boiler-reference shifts, and were selected by operators to avoid peak production. There was no random assignment or matched control. The record says it cannot isolate a storage-technology effect or quantify avoided emissions; grid-carbon intensity was not measured. (Performance and measurement)

[projection.md](calls/heat-r2-B2-Research/projection.md)

- L6: 4. Trial shifts processed lighter products, had ambient temperature 8 degrees higher than the boiler-reference shifts, and were selected by operators to avoid peak production. There was no random assignment or matched control. The record says it cannot isolate a storage-technology effect or quantify avoided emissions; grid-carbon intensity was not measured. (Performance and measurement)

## heat-r2-B2 / contrast — kern — correct

Bron [L7](cases/heat/source.md):

> All four vessels passed the specified pressure-hold test. Two nevertheless failed the insulation surface-temperature check at the access hatch.

[content.md](calls/heat-r2-B2-Research/content.md)

- L6: 5. All four vessels passed the specified pressure-hold test, but two failed the insulation surface-temperature check at the access hatch. Hot surfaces remained unresolved at sign-off, and a proposed shield was not installed. The source explicitly says the pressure test does not demonstrate acceptable touch temperature. (Successful and failed controls)
- L11: 10. Procurement approved only a longer instrumented trial conditional on a verified hatch remedy; no purchase or factory-wide roll-out was approved. (Variability, sequencing and decision)
- L24: - Whether the hatch hot-surface issue was remedied after sign-off, or whether the physical sensor was replaced, is not established (Facts 5, 12).
- L30: - Procurement approved a longer instrumented trial conditional on a verified hatch remedy; the dossier reports no purchase or factory-wide roll-out (Fact 10).

[projection.md](calls/heat-r2-B2-Research/projection.md)

- L7: 5. All four vessels passed the specified pressure-hold test, but two failed the insulation surface-temperature check at the access hatch. Hot surfaces remained unresolved at sign-off, and a proposed shield was not installed. The source explicitly says the pressure test does not demonstrate acceptable touch temperature. (Successful and failed controls)
- L12: 10. Procurement approved only a longer instrumented trial conditional on a verified hatch remedy; no purchase or factory-wide roll-out was approved. (Variability, sequencing and decision)
- L23: - Whether the hatch hot-surface issue was remedied after sign-off, or whether the physical sensor was replaced, is not established (Facts 5, 12).

## heat-r2-B2 / open — kern — correct

Bron [L7](cases/heat/source.md):

> The hot surfaces remained unresolved at sign-off, and a proposed shield was not installed.

[content.md](calls/heat-r2-B2-Research/content.md)

- L6: 5. All four vessels passed the specified pressure-hold test, but two failed the insulation surface-temperature check at the access hatch. Hot surfaces remained unresolved at sign-off, and a proposed shield was not installed. The source explicitly says the pressure test does not demonstrate acceptable touch temperature. (Successful and failed controls)

[projection.md](calls/heat-r2-B2-Research/projection.md)

- L7: 5. All four vessels passed the specified pressure-hold test, but two failed the insulation surface-temperature check at the access hatch. Hot surfaces remained unresolved at sign-off, and a proposed shield was not installed. The source explicitly says the pressure test does not demonstrate acceptable touch temperature. (Successful and failed controls)

## heat-r2-B2 / interlock — belangrijk — correct

Bron [L7](cases/heat/source.md):

> An interlock test passed for all four vessels; it did not test the hot-surface problem.

[content.md](calls/heat-r2-B2-Research/content.md)

- L7: 6. The interlock test passed for all four vessels, but did not test the hot-surface problem. (Successful and failed controls)

[projection.md](calls/heat-r2-B2-Research/projection.md)

- L8: 6. The interlock test passed for all four vessels, but did not test the hot-surface problem. (Successful and failed controls)

## heat-r2-B2 / term — kern — correct

Bron [L8](cases/heat/source.md):

> Three thermal trips occurred during initial charging. Here a thermal trip is a protective over-temperature shutdown, not a production journey or heat-delivery cycle.

[content.md](calls/heat-r2-B2-Research/content.md)

- L8: 7. Three thermal trips occurred during initial charging. The source defines a thermal trip as a protective over-temperature shutdown, not a production journey or heat-delivery cycle. A controller setting and sensor mounting were changed together before a repeat charge, which had zero trips; the record cannot separate their contributions and says one repeat does not establish long-term reliability. (Successful and failed controls)
- L29: - Three thermal trips occurred during initial charging. The controller setting and sensor mounting were then changed together before a repeat charge with zero trips (Fact 7).

[projection.md](calls/heat-r2-B2-Research/projection.md)

- L9: 7. Three thermal trips occurred during initial charging. The source defines a thermal trip as a protective over-temperature shutdown, not a production journey or heat-delivery cycle. A controller setting and sensor mounting were changed together before a repeat charge, which had zero trips; the record cannot separate their contributions and says one repeat does not establish long-term reliability. (Successful and failed controls)

## heat-r2-B2 / attribution — kern — correct

Bron [L8](cases/heat/source.md):

> A controller setting and sensor mounting were changed together before a repeat charge with zero trips. The record cannot separate the two actions' contributions.

[content.md](calls/heat-r2-B2-Research/content.md)

- L8: 7. Three thermal trips occurred during initial charging. The source defines a thermal trip as a protective over-temperature shutdown, not a production journey or heat-delivery cycle. A controller setting and sensor mounting were changed together before a repeat charge, which had zero trips; the record cannot separate their contributions and says one repeat does not establish long-term reliability. (Successful and failed controls)
- L21: - Long-term reliability, and the separate contributions of the controller-setting and sensor-mounting changes to the zero-trip repeat, are not established (Fact 7).
- L29: - Three thermal trips occurred during initial charging. The controller setting and sensor mounting were then changed together before a repeat charge with zero trips (Fact 7).

[projection.md](calls/heat-r2-B2-Research/projection.md)

- L9: 7. Three thermal trips occurred during initial charging. The source defines a thermal trip as a protective over-temperature shutdown, not a production journey or heat-delivery cycle. A controller setting and sensor mounting were changed together before a repeat charge, which had zero trips; the record cannot separate their contributions and says one repeat does not establish long-term reliability. (Successful and failed controls)
- L20: - Long-term reliability, and the separate contributions of the controller-setting and sensor-mounting changes to the zero-trip repeat, are not established (Fact 7).

## heat-r2-B2 / variation — kern — correct

Bron [L10](cases/heat/source.md):

> Three identical-input simulation runs estimated usable discharge at 402, 418 and 407 kWh; the solver used stochastic initialisation.

[content.md](calls/heat-r2-B2-Research/content.md)

- L9: 8. Three identical-input simulation runs estimated usable discharge at 402, 418 and 407 kWh; the solver used stochastic initialisation. The source says these are not additional metered field trials and do not supersede the measured 410 kWh, and identifies solver variation as an attribution limit for interpreting simulation improvements. (Variability, sequencing and decision)
- L22: - The simulations do not establish additional metered field performance; the record does not establish an improvement attributable to a particular change (Fact 8).

[projection.md](calls/heat-r2-B2-Research/projection.md)

- L10: 8. Three identical-input simulation runs estimated usable discharge at 402, 418 and 407 kWh; the solver used stochastic initialisation. The source says these are not additional metered field trials and do not supersede the measured 410 kWh, and identifies solver variation as an attribution limit for interpreting simulation improvements. (Variability, sequencing and decision)
- L21: - The simulations do not establish additional metered field performance; the record does not establish an improvement attributable to a particular change (Fact 8).

## heat-r2-B2 / cost — belangrijk — correct

Bron [L11](cases/heat/source.md):

> The finance appendix reports a EUR 9,200 rental allowance. It gives no date or sequence relative to testing. The allowance excludes installation, downtime and maintenance, so no payback claim is supported.

[content.md](calls/heat-r2-B2-Research/content.md)

- L10: 9. The finance appendix reports a EUR 9,200 rental allowance but gives no date or sequence relative to testing. It excludes installation, downtime and maintenance; the source says no payback claim is supported. (Variability, sequencing and decision)
- L23: - Payback is unsupported by the stated allowance, which excludes costs and has no stated timing relative to testing (Fact 9).

[projection.md](calls/heat-r2-B2-Research/projection.md)

- L11: 9. The finance appendix reports a EUR 9,200 rental allowance but gives no date or sequence relative to testing. It excludes installation, downtime and maintenance; the source says no payback claim is supported. (Variability, sequencing and decision)
- L22: - Payback is unsupported by the stated allowance, which excludes costs and has no stated timing relative to testing (Fact 9).

## heat-r2-B2 / decision — kern — correct

Bron [L11](cases/heat/source.md):

> Procurement approved only a longer instrumented trial conditional on a verified hatch remedy; no purchase or factory-wide roll-out was approved.

[content.md](calls/heat-r2-B2-Research/content.md)

- L11: 10. Procurement approved only a longer instrumented trial conditional on a verified hatch remedy; no purchase or factory-wide roll-out was approved. (Variability, sequencing and decision)
- L30: - Procurement approved a longer instrumented trial conditional on a verified hatch remedy; the dossier reports no purchase or factory-wide roll-out (Fact 10).

[projection.md](calls/heat-r2-B2-Research/projection.md)

- L12: 10. Procurement approved only a longer instrumented trial conditional on a verified hatch remedy; no purchase or factory-wide roll-out was approved. (Variability, sequencing and decision)

## water-r3-B2 / ledger — kern — correct

Bron [L4](cases/water/source.md):

> The legacy fortnight recorded 46 low-pressure alerts in 1,400 logger-hours; the pilot fortnight recorded 29 in 1,120 logger-hours.

[content.md](calls/water-r3-B2-Research/content.md)

- L3: 2. The legacy fortnight recorded 46 low-pressure alerts in 1,400 logger-hours; the pilot fortnight recorded 29 in 1,120 logger-hours. These are alert counts, not confirmed leaks or affected households. The final ledger uses 29 after restoring four suppressed duplicates that were distinct device events; a presentation’s 25 is not the final count. The report does not estimate a rate-adjusted treatment effect. (“Recorded outcomes”)
- L8: 7. Three replays of the same stored trace emitted 29, 31 and 28 alerts; a nondeterministic deduplication tie-breaker was enabled. The team did not select a replay as the true count. These replay counts do not replace the final field ledger of 29; the source says repeat variability and changed thresholds limit attribution. (“Repeats and decision”)

[projection.md](calls/water-r3-B2-Research/projection.md)

- L4: 2. The legacy fortnight recorded 46 low-pressure alerts in 1,400 logger-hours; the pilot fortnight recorded 29 in 1,120 logger-hours. These are alert counts, not confirmed leaks or affected households. The final ledger uses 29 after restoring four suppressed duplicates that were distinct device events; a presentation’s 25 is not the final count. The report does not estimate a rate-adjusted treatment effect. (“Recorded outcomes”)
- L9: 7. Three replays of the same stored trace emitted 29, 31 and 28 alerts; a nondeterministic deduplication tie-breaker was enabled. The team did not select a replay as the true count. These replay counts do not replace the final field ledger of 29; the source says repeat variability and changed thresholds limit attribution. (“Repeats and decision”)

## water-r3-B2 / method — kern — correct

Bron [L5](cases/water/source.md):

> The legacy alert threshold was 1.8 bar for 30 seconds; the pilot used 1.6 bar for 60 seconds.

[content.md](calls/water-r3-B2-Research/content.md)

- L4: 3. The legacy threshold was 1.8 bar for 30 seconds; the pilot threshold was 1.6 bar for 60 seconds, and old traces were not reprocessed. Night flushing occurred during the legacy fortnight only; branches were selected for ease of access, not randomly; rainfall differed; and no unaffected comparison branch was instrumented. The source says the lower alert count cannot isolate the sensor upgrade’s effect. (“Recorded outcomes”)

[projection.md](calls/water-r3-B2-Research/projection.md)

- L5: 3. The legacy threshold was 1.8 bar for 30 seconds; the pilot threshold was 1.6 bar for 60 seconds, and old traces were not reprocessed. Night flushing occurred during the legacy fortnight only; branches were selected for ease of access, not randomly; rainfall differed; and no unaffected comparison branch was instrumented. The source says the lower alert count cannot isolate the sensor upgrade’s effect. (“Recorded outcomes”)

## water-r3-B2 / conditions — belangrijk — correct

Bron [L5](cases/water/source.md):

> A planned night flushing operation occurred during the legacy fortnight only.

[content.md](calls/water-r3-B2-Research/content.md)

- L4: 3. The legacy threshold was 1.8 bar for 30 seconds; the pilot threshold was 1.6 bar for 60 seconds, and old traces were not reprocessed. Night flushing occurred during the legacy fortnight only; branches were selected for ease of access, not randomly; rainfall differed; and no unaffected comparison branch was instrumented. The source says the lower alert count cannot isolate the sensor upgrade’s effect. (“Recorded outcomes”)
- L14: - The source does not establish whether the sensor upgrade caused a reduction in alerts, nor provide a rate-adjusted treatment effect; thresholds, flushing, branch selection, rainfall, lack of a comparison branch, and replay variability constrain attribution. (Facts 2–3, 7)

[projection.md](calls/water-r3-B2-Research/projection.md)

- L5: 3. The legacy threshold was 1.8 bar for 30 seconds; the pilot threshold was 1.6 bar for 60 seconds, and old traces were not reprocessed. Night flushing occurred during the legacy fortnight only; branches were selected for ease of access, not randomly; rainfall differed; and no unaffected comparison branch was instrumented. The source says the lower alert count cannot isolate the sensor upgrade’s effect. (“Recorded outcomes”)
- L14: - The source does not establish whether the sensor upgrade caused a reduction in alerts, nor provide a rate-adjusted treatment effect; thresholds, flushing, branch selection, rainfall, lack of a comparison branch, and replay variability constrain attribution. (Facts 2–3, 7)

## water-r3-B2 / allocation — belangrijk — correct

Bron [L5](cases/water/source.md):

> Branches were selected for ease of access, not randomly; rainfall also differed.

[content.md](calls/water-r3-B2-Research/content.md)

- L4: 3. The legacy threshold was 1.8 bar for 30 seconds; the pilot threshold was 1.6 bar for 60 seconds, and old traces were not reprocessed. Night flushing occurred during the legacy fortnight only; branches were selected for ease of access, not randomly; rainfall differed; and no unaffected comparison branch was instrumented. The source says the lower alert count cannot isolate the sensor upgrade’s effect. (“Recorded outcomes”)
- L14: - The source does not establish whether the sensor upgrade caused a reduction in alerts, nor provide a rate-adjusted treatment effect; thresholds, flushing, branch selection, rainfall, lack of a comparison branch, and replay variability constrain attribution. (Facts 2–3, 7)

[projection.md](calls/water-r3-B2-Research/projection.md)

- L5: 3. The legacy threshold was 1.8 bar for 30 seconds; the pilot threshold was 1.6 bar for 60 seconds, and old traces were not reprocessed. Night flushing occurred during the legacy fortnight only; branches were selected for ease of access, not randomly; rainfall differed; and no unaffected comparison branch was instrumented. The source says the lower alert count cannot isolate the sensor upgrade’s effect. (“Recorded outcomes”)
- L14: - The source does not establish whether the sensor upgrade caused a reduction in alerts, nor provide a rate-adjusted treatment effect; thresholds, flushing, branch selection, rainfall, lack of a comparison branch, and replay variability constrain attribution. (Facts 2–3, 7)

## water-r3-B2 / contrast — kern — correct

Bron [L7](cases/water/source.md):

> All twelve loggers passed the static reference-gauge calibration at 2.0 bar. Four nevertheless failed the overnight packet-delivery check, losing 17% of scheduled messages in that check.

[content.md](calls/water-r3-B2-Research/content.md)

- L5: 4. All 12 loggers passed the static reference-gauge calibration at 2.0 bar. Separately, four failed the overnight packet-delivery check, losing 17% of scheduled messages in that check. The source does not show that the pressure readings which arrived were inaccurate. (“Checks and defects”)
- L6: 5. The packet-delivery fault remained open at review. A proposed antenna move had not been tested. Battery replacement was completed on two different devices, but no date links that work to the delivery check; the source says not to treat battery status as a repair of the packet fault. (“Checks and defects”)
- L11: 1. Passing the static calibration establishes that check’s result only; it does not resolve the separately documented delivery fault or establish field uptime. (Facts 4–6)
- L16: - The cause or repair of the open packet-delivery fault is unresolved; the proposed antenna move was untested, and the battery work is not linked to or established as a repair of that fault. (Fact 5)

[projection.md](calls/water-r3-B2-Research/projection.md)

- L6: 4. All 12 loggers passed the static reference-gauge calibration at 2.0 bar. Separately, four failed the overnight packet-delivery check, losing 17% of scheduled messages in that check. The source does not show that the pressure readings which arrived were inaccurate. (“Checks and defects”)
- L7: 5. The packet-delivery fault remained open at review. A proposed antenna move had not been tested. Battery replacement was completed on two different devices, but no date links that work to the delivery check; the source says not to treat battery status as a repair of the packet fault. (“Checks and defects”)
- L16: - The cause or repair of the open packet-delivery fault is unresolved; the proposed antenna move was untested, and the battery work is not linked to or established as a repair of that fault. (Fact 5)

## water-r3-B2 / open — kern — correct

Bron [L7](cases/water/source.md):

> The delivery fault remained open at review; a proposed antenna move had not been tested.

[content.md](calls/water-r3-B2-Research/content.md)

- L6: 5. The packet-delivery fault remained open at review. A proposed antenna move had not been tested. Battery replacement was completed on two different devices, but no date links that work to the delivery check; the source says not to treat battery status as a repair of the packet fault. (“Checks and defects”)
- L16: - The cause or repair of the open packet-delivery fault is unresolved; the proposed antenna move was untested, and the battery work is not linked to or established as a repair of that fault. (Fact 5)
- L22: - At review, the packet fault remained open and the antenna move was still proposed and untested. The committee authorized a six-week paired measurement study, but not network-wide deployment. (Facts 5, 8)

[projection.md](calls/water-r3-B2-Research/projection.md)

- L7: 5. The packet-delivery fault remained open at review. A proposed antenna move had not been tested. Battery replacement was completed on two different devices, but no date links that work to the delivery check; the source says not to treat battery status as a repair of the packet fault. (“Checks and defects”)
- L16: - The cause or repair of the open packet-delivery fault is unresolved; the proposed antenna move was untested, and the battery work is not linked to or established as a repair of that fault. (Fact 5)

## water-r3-B2 / chronology — belangrijk — correct

Bron [L7](cases/water/source.md):

> A battery replacement was completed on two different devices, but the dossier provides no date linking that work to the delivery check.

[content.md](calls/water-r3-B2-Research/content.md)

- L6: 5. The packet-delivery fault remained open at review. A proposed antenna move had not been tested. Battery replacement was completed on two different devices, but no date links that work to the delivery check; the source says not to treat battery status as a repair of the packet fault. (“Checks and defects”)
- L16: - The cause or repair of the open packet-delivery fault is unresolved; the proposed antenna move was untested, and the battery work is not linked to or established as a repair of that fault. (Fact 5)

[projection.md](calls/water-r3-B2-Research/projection.md)

- L7: 5. The packet-delivery fault remained open at review. A proposed antenna move had not been tested. Battery replacement was completed on two different devices, but no date links that work to the delivery check; the source says not to treat battery status as a repair of the packet fault. (“Checks and defects”)
- L16: - The cause or repair of the open packet-delivery fault is unresolved; the proposed antenna move was untested, and the battery work is not linked to or established as a repair of that fault. (Fact 5)

## water-r3-B2 / term — kern — correct

Bron [L8](cases/water/source.md):

> A bench check recorded three brownouts: transient supply-voltage drops causing device restarts, not a regional power outage.

[content.md](calls/water-r3-B2-Research/content.md)

- L7: 6. A bench check recorded three brownouts, defined here as transient supply-voltage drops causing device restarts, not a regional power outage. A capacitor swap and firmware update were both made before a repeat bench check with no brownouts. Their separate contributions were not identified, and the bench result does not certify field uptime. (“Checks and defects”)
- L21: - The capacitor swap and firmware update preceded a repeat bench check that recorded no brownouts; their individual effects and field performance remain unresolved. (Fact 6)

[projection.md](calls/water-r3-B2-Research/projection.md)

- L8: 6. A bench check recorded three brownouts, defined here as transient supply-voltage drops causing device restarts, not a regional power outage. A capacitor swap and firmware update were both made before a repeat bench check with no brownouts. Their separate contributions were not identified, and the bench result does not certify field uptime. (“Checks and defects”)

## water-r3-B2 / attribution — kern — correct

Bron [L8](cases/water/source.md):

> A capacitor swap and firmware update were both made before a repeat bench check with no brownouts. Their separate contributions were not identified; the bench result does not certify field uptime.

[content.md](calls/water-r3-B2-Research/content.md)

- L7: 6. A bench check recorded three brownouts, defined here as transient supply-voltage drops causing device restarts, not a regional power outage. A capacitor swap and firmware update were both made before a repeat bench check with no brownouts. Their separate contributions were not identified, and the bench result does not certify field uptime. (“Checks and defects”)
- L11: 1. Passing the static calibration establishes that check’s result only; it does not resolve the separately documented delivery fault or establish field uptime. (Facts 4–6)
- L17: - The separate effects of the capacitor swap and firmware update, and field uptime after those changes, are not established by the repeat bench check. (Fact 6)
- L21: - The capacitor swap and firmware update preceded a repeat bench check that recorded no brownouts; their individual effects and field performance remain unresolved. (Fact 6)

[projection.md](calls/water-r3-B2-Research/projection.md)

- L8: 6. A bench check recorded three brownouts, defined here as transient supply-voltage drops causing device restarts, not a regional power outage. A capacitor swap and firmware update were both made before a repeat bench check with no brownouts. Their separate contributions were not identified, and the bench result does not certify field uptime. (“Checks and defects”)
- L17: - The separate effects of the capacitor swap and firmware update, and field uptime after those changes, are not established by the repeat bench check. (Fact 6)

## water-r3-B2 / variation — kern — correct

Bron [L10](cases/water/source.md):

> Three replays of the same stored trace emitted 29, 31 and 28 alerts; a nondeterministic deduplication tie-breaker was enabled.

[content.md](calls/water-r3-B2-Research/content.md)

- L8: 7. Three replays of the same stored trace emitted 29, 31 and 28 alerts; a nondeterministic deduplication tie-breaker was enabled. The team did not select a replay as the true count. These replay counts do not replace the final field ledger of 29; the source says repeat variability and changed thresholds limit attribution. (“Repeats and decision”)
- L14: - The source does not establish whether the sensor upgrade caused a reduction in alerts, nor provide a rate-adjusted treatment effect; thresholds, flushing, branch selection, rainfall, lack of a comparison branch, and replay variability constrain attribution. (Facts 2–3, 7)

[projection.md](calls/water-r3-B2-Research/projection.md)

- L9: 7. Three replays of the same stored trace emitted 29, 31 and 28 alerts; a nondeterministic deduplication tie-breaker was enabled. The team did not select a replay as the true count. These replay counts do not replace the final field ledger of 29; the source says repeat variability and changed thresholds limit attribution. (“Repeats and decision”)
- L14: - The source does not establish whether the sensor upgrade caused a reduction in alerts, nor provide a rate-adjusted treatment effect; thresholds, flushing, branch selection, rainfall, lack of a comparison branch, and replay variability constrain attribution. (Facts 2–3, 7)

## water-r3-B2 / decision — kern — correct

Bron [L11](cases/water/source.md):

> The committee authorized a six-week paired measurement study with unchanged thresholds. It did not authorize network-wide deployment.

[content.md](calls/water-r3-B2-Research/content.md)

- L9: 8. The committee authorized a six-week paired measurement study with unchanged thresholds, but did not authorize network-wide deployment. The EUR 18,000 budget covers logger rental and field visits, excludes staff overtime, and does not establish savings. No safety, leak-prevention or annual reliability benefit was measured. Training attendance was 10 of 12 technicians, but attendance was not a competence test. (“Repeats and decision”)
- L18: - The authorized paired study has not been reported as completed here; no network-wide deployment decision, savings, safety benefit, leak-prevention benefit, annual reliability benefit, or technician competence result is established. (Fact 8)
- L22: - At review, the packet fault remained open and the antenna move was still proposed and untested. The committee authorized a six-week paired measurement study, but not network-wide deployment. (Facts 5, 8)

[projection.md](calls/water-r3-B2-Research/projection.md)

- L10: 8. The committee authorized a six-week paired measurement study with unchanged thresholds, but did not authorize network-wide deployment. The EUR 18,000 budget covers logger rental and field visits, excludes staff overtime, and does not establish savings. No safety, leak-prevention or annual reliability benefit was measured. Training attendance was 10 of 12 technicians, but attendance was not a competence test. (“Repeats and decision”)
- L18: - The authorized paired study has not been reported as completed here; no network-wide deployment decision, savings, safety benefit, leak-prevention benefit, annual reliability benefit, or technician competence result is established. (Fact 8)

## water-r3-B2 / cost — belangrijk — correct

Bron [L11](cases/water/source.md):

> The budget of EUR 18,000 covers logger rental and field visits, excludes staff overtime and does not establish savings.

[content.md](calls/water-r3-B2-Research/content.md)

- L9: 8. The committee authorized a six-week paired measurement study with unchanged thresholds, but did not authorize network-wide deployment. The EUR 18,000 budget covers logger rental and field visits, excludes staff overtime, and does not establish savings. No safety, leak-prevention or annual reliability benefit was measured. Training attendance was 10 of 12 technicians, but attendance was not a competence test. (“Repeats and decision”)
- L18: - The authorized paired study has not been reported as completed here; no network-wide deployment decision, savings, safety benefit, leak-prevention benefit, annual reliability benefit, or technician competence result is established. (Fact 8)

[projection.md](calls/water-r3-B2-Research/projection.md)

- L10: 8. The committee authorized a six-week paired measurement study with unchanged thresholds, but did not authorize network-wide deployment. The EUR 18,000 budget covers logger rental and field visits, excludes staff overtime, and does not establish savings. No safety, leak-prevention or annual reliability benefit was measured. Training attendance was 10 of 12 technicians, but attendance was not a competence test. (“Repeats and decision”)
- L18: - The authorized paired study has not been reported as completed here; no network-wide deployment decision, savings, safety benefit, leak-prevention benefit, annual reliability benefit, or technician competence result is established. (Fact 8)

## water-r3-A / ledger — kern — correct

Bron [L4](cases/water/source.md):

> The legacy fortnight recorded 46 low-pressure alerts in 1,400 logger-hours; the pilot fortnight recorded 29 in 1,120 logger-hours.

[content.md](calls/water-r3-A-Research/content.md)

- L9: 7. The legacy fortnight recorded 46 low-pressure alerts in 1,400 logger-hours.
- L10: 8. The pilot fortnight recorded 29 low-pressure alerts in 1,120 logger-hours.
- L31: 29. The dossier states that the battery status should not be treated as a repair of the packet fault.
- L39: 37. Three replays of the same stored trace emitted 29, 31, and 28 alerts.
- L42: 40. The dossier states that the replay counts do not replace the final field ledger of 29.
- L48: 46. The dossier states that the budget does not establish savings.
- L76: - The legacy fortnight recorded 46 alerts in 1,400 logger-hours; the pilot fortnight recorded 29 alerts in 1,120 logger-hours. The source does not establish that the difference was caused by the sensor upgrade.
- L77: - A presentation reported 25 pilot alerts; the final ledger restored four suppressed entries for distinct device events and recorded 29. The source does not select a replay count as the true count, and says the replay counts do not replace the final field ledger.
- L80: - Three replays of the same stored trace emitted 29, 31, and 28 alerts; the team did not select one as the true count.

[projection.md](calls/water-r3-A-Research/projection.md)

- L9: 7. The legacy fortnight recorded 46 low-pressure alerts in 1,400 logger-hours.
- L10: 8. The pilot fortnight recorded 29 low-pressure alerts in 1,120 logger-hours.
- L31: 29. The dossier states that the battery status should not be treated as a repair of the packet fault.
- L39: 37. Three replays of the same stored trace emitted 29, 31, and 28 alerts.
- L42: 40. The dossier states that the replay counts do not replace the final field ledger of 29.
- L48: 46. The dossier states that the budget does not establish savings.

## water-r3-A / method — kern — correct

Bron [L5](cases/water/source.md):

> The legacy alert threshold was 1.8 bar for 30 seconds; the pilot used 1.6 bar for 60 seconds.

[content.md](calls/water-r3-A-Research/content.md)

- L14: 12. The legacy alert threshold was 1.8 bar for 30 seconds.
- L15: 13. The pilot alert threshold was 1.6 bar for 60 seconds.
- L16: 14. Old traces were not reprocessed.

[projection.md](calls/water-r3-A-Research/projection.md)

- L14: 12. The legacy alert threshold was 1.8 bar for 30 seconds.
- L15: 13. The pilot alert threshold was 1.6 bar for 60 seconds.
- L16: 14. Old traces were not reprocessed.

## water-r3-A / conditions — belangrijk — correct

Bron [L5](cases/water/source.md):

> A planned night flushing operation occurred during the legacy fortnight only.

[content.md](calls/water-r3-A-Research/content.md)

- L17: 15. A planned night flushing operation occurred during the legacy fortnight only.

[projection.md](calls/water-r3-A-Research/projection.md)

- L17: 15. A planned night flushing operation occurred during the legacy fortnight only.

## water-r3-A / allocation — belangrijk — correct

Bron [L5](cases/water/source.md):

> Branches were selected for ease of access, not randomly; rainfall also differed.

[content.md](calls/water-r3-A-Research/content.md)

- L18: 16. Branches were selected for ease of access, not randomly.
- L19: 17. Rainfall differed between the fortnights.

[projection.md](calls/water-r3-A-Research/projection.md)

- L18: 16. Branches were selected for ease of access, not randomly.
- L19: 17. Rainfall differed between the fortnights.

## water-r3-A / contrast — kern — correct

Bron [L7](cases/water/source.md):

> All twelve loggers passed the static reference-gauge calibration at 2.0 bar. Four nevertheless failed the overnight packet-delivery check, losing 17% of scheduled messages in that check.

[content.md](calls/water-r3-A-Research/content.md)

- L23: 21. All twelve loggers passed the static reference-gauge calibration at 2.0 bar.
- L24: 22. Four loggers failed the overnight packet-delivery check.
- L25: 23. Those four loggers lost 17% of scheduled messages in that check.
- L27: 25. The packet-delivery fault remained open at review.
- L30: 28. The dossier provides no date linking the battery replacement to the packet-delivery check.
- L62: - The source does not establish the overall reliability of the pilot from the calibration result or other checks.
- L63: - The source does not establish that the pressure readings received during the packet-delivery check were inaccurate; it says they were not shown to be inaccurate.
- L64: - Whether the battery replacement was related to the packet-delivery check, or repaired the packet fault, is not established.
- L65: - Whether the proposed antenna move would address the packet-delivery fault is unknown; it had not been tested.
- L78: - All twelve loggers passed the static reference-gauge calibration at 2.0 bar. Separately, four loggers failed the overnight packet-delivery check, losing 17% of scheduled messages in that check.
- L86: - Facts 21–36 concern calibration, packet delivery, brownouts, and the limits of the reported checks.

[projection.md](calls/water-r3-A-Research/projection.md)

- L23: 21. All twelve loggers passed the static reference-gauge calibration at 2.0 bar.
- L24: 22. Four loggers failed the overnight packet-delivery check.
- L25: 23. Those four loggers lost 17% of scheduled messages in that check.
- L27: 25. The packet-delivery fault remained open at review.
- L30: 28. The dossier provides no date linking the battery replacement to the packet-delivery check.
- L58: - The source does not establish the overall reliability of the pilot from the calibration result or other checks.
- L59: - The source does not establish that the pressure readings received during the packet-delivery check were inaccurate; it says they were not shown to be inaccurate.
- L60: - Whether the battery replacement was related to the packet-delivery check, or repaired the packet fault, is not established.
- L61: - Whether the proposed antenna move would address the packet-delivery fault is unknown; it had not been tested.

## water-r3-A / open — kern — correct

Bron [L7](cases/water/source.md):

> The delivery fault remained open at review; a proposed antenna move had not been tested.

[content.md](calls/water-r3-A-Research/content.md)

- L27: 25. The packet-delivery fault remained open at review.
- L28: 26. A proposed antenna move had not been tested.
- L65: - Whether the proposed antenna move would address the packet-delivery fault is unknown; it had not been tested.

[projection.md](calls/water-r3-A-Research/projection.md)

- L27: 25. The packet-delivery fault remained open at review.
- L28: 26. A proposed antenna move had not been tested.
- L61: - Whether the proposed antenna move would address the packet-delivery fault is unknown; it had not been tested.

## water-r3-A / chronology — belangrijk — correct

Bron [L7](cases/water/source.md):

> A battery replacement was completed on two different devices, but the dossier provides no date linking that work to the delivery check.

[content.md](calls/water-r3-A-Research/content.md)

- L29: 27. Battery replacement was completed on two different devices.
- L30: 28. The dossier provides no date linking the battery replacement to the packet-delivery check.
- L31: 29. The dossier states that the battery status should not be treated as a repair of the packet fault.
- L64: - Whether the battery replacement was related to the packet-delivery check, or repaired the packet fault, is not established.

[projection.md](calls/water-r3-A-Research/projection.md)

- L29: 27. Battery replacement was completed on two different devices.
- L30: 28. The dossier provides no date linking the battery replacement to the packet-delivery check.
- L31: 29. The dossier states that the battery status should not be treated as a repair of the packet fault.
- L60: - Whether the battery replacement was related to the packet-delivery check, or repaired the packet fault, is not established.

## water-r3-A / term — kern — correct

Bron [L8](cases/water/source.md):

> A bench check recorded three brownouts: transient supply-voltage drops causing device restarts, not a regional power outage.

[content.md](calls/water-r3-A-Research/content.md)

- L32: 30. A bench check recorded three brownouts.
- L33: 31. The dossier defines the brownouts as transient supply-voltage drops causing device restarts, not a regional power outage.
- L36: 34. The repeat bench check recorded no brownouts.
- L79: - A bench check recorded three brownouts. A capacitor swap and firmware update were both made before a repeat bench check, which recorded no brownouts. Their separate contributions are UNKNOWN.
- L86: - Facts 21–36 concern calibration, packet delivery, brownouts, and the limits of the reported checks.

[projection.md](calls/water-r3-A-Research/projection.md)

- L32: 30. A bench check recorded three brownouts.
- L33: 31. The dossier defines the brownouts as transient supply-voltage drops causing device restarts, not a regional power outage.
- L36: 34. The repeat bench check recorded no brownouts.

## water-r3-A / attribution — kern — correct

Bron [L8](cases/water/source.md):

> A capacitor swap and firmware update were both made before a repeat bench check with no brownouts. Their separate contributions were not identified; the bench result does not certify field uptime.

[content.md](calls/water-r3-A-Research/content.md)

- L34: 32. A capacitor swap was made before a repeat bench check.
- L35: 33. A firmware update was made before a repeat bench check.
- L37: 35. The separate contributions of the capacitor swap and firmware update were not identified.
- L38: 36. The dossier states that the bench result does not certify field uptime.
- L66: - The separate contributions of the capacitor swap and firmware update to the repeat bench result are not identified.
- L67: - Field uptime is not certified by the bench result.
- L79: - A bench check recorded three brownouts. A capacitor swap and firmware update were both made before a repeat bench check, which recorded no brownouts. Their separate contributions are UNKNOWN.

[projection.md](calls/water-r3-A-Research/projection.md)

- L34: 32. A capacitor swap was made before a repeat bench check.
- L35: 33. A firmware update was made before a repeat bench check.
- L37: 35. The separate contributions of the capacitor swap and firmware update were not identified.
- L38: 36. The dossier states that the bench result does not certify field uptime.
- L62: - The separate contributions of the capacitor swap and firmware update to the repeat bench result are not identified.
- L63: - Field uptime is not certified by the bench result.

## water-r3-A / variation — kern — correct

Bron [L10](cases/water/source.md):

> Three replays of the same stored trace emitted 29, 31 and 28 alerts; a nondeterministic deduplication tie-breaker was enabled.

[content.md](calls/water-r3-A-Research/content.md)

- L39: 37. Three replays of the same stored trace emitted 29, 31, and 28 alerts.
- L40: 38. A nondeterministic deduplication tie-breaker was enabled.
- L41: 39. The team did not select one replay as the true count.
- L42: 40. The dossier states that the replay counts do not replace the final field ledger of 29.
- L68: - The team did not identify one replay count as the true count; the source does not establish which replay count is definitive.
- L77: - A presentation reported 25 pilot alerts; the final ledger restored four suppressed entries for distinct device events and recorded 29. The source does not select a replay count as the true count, and says the replay counts do not replace the final field ledger.
- L80: - Three replays of the same stored trace emitted 29, 31, and 28 alerts; the team did not select one as the true count.
- L85: - Facts 7–20 and 37–41 concern the recorded alert outcomes, differing thresholds, limitations on attribution, and replay variability.

[projection.md](calls/water-r3-A-Research/projection.md)

- L39: 37. Three replays of the same stored trace emitted 29, 31, and 28 alerts.
- L40: 38. A nondeterministic deduplication tie-breaker was enabled.
- L41: 39. The team did not select one replay as the true count.
- L42: 40. The dossier states that the replay counts do not replace the final field ledger of 29.
- L64: - The team did not identify one replay count as the true count; the source does not establish which replay count is definitive.

## water-r3-A / decision — kern — correct

Bron [L11](cases/water/source.md):

> The committee authorized a six-week paired measurement study with unchanged thresholds. It did not authorize network-wide deployment.

[content.md](calls/water-r3-A-Research/content.md)

- L44: 42. The committee authorized a six-week paired measurement study with unchanged thresholds.
- L45: 43. The committee did not authorize network-wide deployment.
- L71: - The source does not state why the committee authorized the paired study or did not authorize network-wide deployment.
- L81: - The committee authorized a six-week paired measurement study with unchanged thresholds and did not authorize network-wide deployment. The source does not state the decision rationale.

[projection.md](calls/water-r3-A-Research/projection.md)

- L44: 42. The committee authorized a six-week paired measurement study with unchanged thresholds.
- L45: 43. The committee did not authorize network-wide deployment.
- L67: - The source does not state why the committee authorized the paired study or did not authorize network-wide deployment.

## water-r3-A / cost — belangrijk — correct

Bron [L11](cases/water/source.md):

> The budget of EUR 18,000 covers logger rental and field visits, excludes staff overtime and does not establish savings.

[content.md](calls/water-r3-A-Research/content.md)

- L46: 44. The budget of EUR 18,000 covers logger rental and field visits.
- L47: 45. The budget excludes staff overtime.
- L48: 46. The dossier states that the budget does not establish savings.
- L69: - Whether any savings, safety benefit, leak-prevention benefit, or annual reliability benefit occurred is not established; those benefits were not measured.

[projection.md](calls/water-r3-A-Research/projection.md)

- L46: 44. The budget of EUR 18,000 covers logger rental and field visits.
- L47: 45. The budget excludes staff overtime.
- L48: 46. The dossier states that the budget does not establish savings.
- L65: - Whether any savings, safety benefit, leak-prevention benefit, or annual reliability benefit occurred is not established; those benefits were not measured.

## water-r3-B / ledger — kern — correct

Bron [L4](cases/water/source.md):

> The legacy fortnight recorded 46 low-pressure alerts in 1,400 logger-hours; the pilot fortnight recorded 29 in 1,120 logger-hours.

[content.md](calls/water-r3-B-Research/content.md)

- L3: 2. The legacy fortnight recorded 46 low-pressure alerts in 1,400 logger-hours; the pilot fortnight recorded 29 in 1,120 logger-hours. These are alert counts, not confirmed leaks or affected households. The final ledger’s 29 restores four suppressed events with distinct timestamps; an earlier presentation’s 25 is not the final count. (Recorded outcomes)
- L9: 8. Three replays of the same stored trace emitted 29, 31, and 28 alerts with a nondeterministic deduplication tie-breaker enabled. The team did not designate one replay as the true count; replay counts do not replace the final field ledger’s 29. (Repeats and decision)

[projection.md](calls/water-r3-B-Research/projection.md)

- L4: 2. The legacy fortnight recorded 46 low-pressure alerts in 1,400 logger-hours; the pilot fortnight recorded 29 in 1,120 logger-hours. These are alert counts, not confirmed leaks or affected households. The final ledger’s 29 restores four suppressed events with distinct timestamps; an earlier presentation’s 25 is not the final count. (Recorded outcomes)
- L10: 8. Three replays of the same stored trace emitted 29, 31, and 28 alerts with a nondeterministic deduplication tie-breaker enabled. The team did not designate one replay as the true count; replay counts do not replace the final field ledger’s 29. (Repeats and decision)

## water-r3-B / method — kern — correct

Bron [L5](cases/water/source.md):

> The legacy alert threshold was 1.8 bar for 30 seconds; the pilot used 1.6 bar for 60 seconds.

[content.md](calls/water-r3-B-Research/content.md)

- L4: 3. The alert thresholds differed: legacy was 1.8 bar for 30 seconds; pilot was 1.6 bar for 60 seconds. Old traces were not reprocessed, and the report does not estimate a rate-adjusted treatment effect. (Recorded outcomes)
- L17: - The sensor upgrade’s causal effect on alerts, and any rate-adjusted treatment effect, are not established; thresholds changed, old traces were not reprocessed, and the listed confounders remain. (Facts 3–4)

[projection.md](calls/water-r3-B-Research/projection.md)

- L5: 3. The alert thresholds differed: legacy was 1.8 bar for 30 seconds; pilot was 1.6 bar for 60 seconds. Old traces were not reprocessed, and the report does not estimate a rate-adjusted treatment effect. (Recorded outcomes)
- L15: - The sensor upgrade’s causal effect on alerts, and any rate-adjusted treatment effect, are not established; thresholds changed, old traces were not reprocessed, and the listed confounders remain. (Facts 3–4)

## water-r3-B / conditions — belangrijk — correct

Bron [L5](cases/water/source.md):

> A planned night flushing operation occurred during the legacy fortnight only.

[content.md](calls/water-r3-B-Research/content.md)

- L5: 4. Attribution is limited: night flushing occurred only during the legacy fortnight; branches were selected for access, not randomly; rainfall differed; and no unaffected comparison branch was instrumented. The source says the lower alert count cannot isolate the sensor upgrade’s effect. (Recorded outcomes)

[projection.md](calls/water-r3-B-Research/projection.md)

- L6: 4. Attribution is limited: night flushing occurred only during the legacy fortnight; branches were selected for access, not randomly; rainfall differed; and no unaffected comparison branch was instrumented. The source says the lower alert count cannot isolate the sensor upgrade’s effect. (Recorded outcomes)

## water-r3-B / allocation — belangrijk — correct

Bron [L5](cases/water/source.md):

> Branches were selected for ease of access, not randomly; rainfall also differed.

[content.md](calls/water-r3-B-Research/content.md)

- L5: 4. Attribution is limited: night flushing occurred only during the legacy fortnight; branches were selected for access, not randomly; rainfall differed; and no unaffected comparison branch was instrumented. The source says the lower alert count cannot isolate the sensor upgrade’s effect. (Recorded outcomes)

[projection.md](calls/water-r3-B-Research/projection.md)

- L6: 4. Attribution is limited: night flushing occurred only during the legacy fortnight; branches were selected for access, not randomly; rainfall differed; and no unaffected comparison branch was instrumented. The source says the lower alert count cannot isolate the sensor upgrade’s effect. (Recorded outcomes)

## water-r3-B / contrast — kern — correct

Bron [L7](cases/water/source.md):

> All twelve loggers passed the static reference-gauge calibration at 2.0 bar. Four nevertheless failed the overnight packet-delivery check, losing 17% of scheduled messages in that check.

[content.md](calls/water-r3-B-Research/content.md)

- L6: 5. All twelve loggers passed static reference-gauge calibration at 2.0 bar. However, four failed the overnight packet-delivery check, losing 17% of scheduled messages in that check. The pressure readings that arrived were not shown to be inaccurate. (Checks and defects)
- L7: 6. The packet-delivery fault remained open at review; a proposed antenna move had not been tested. Two different devices had battery replacements, but no date links that work to the delivery check; the source says not to treat the battery work as a repair of the packet fault. (Checks and defects)
- L13: - The passing static calibration and the separate packet-delivery failure concern different checks; passing the former does not establish end-to-end telemetry reliability. (Facts 5–6)
- L18: - Calibration establishes a pass only at the stated static 2.0-bar check. Accuracy beyond that check, including for readings not delivered, is not established; the source only says arrived readings were not shown to be inaccurate. (Fact 5)

[projection.md](calls/water-r3-B-Research/projection.md)

- L7: 5. All twelve loggers passed static reference-gauge calibration at 2.0 bar. However, four failed the overnight packet-delivery check, losing 17% of scheduled messages in that check. The pressure readings that arrived were not shown to be inaccurate. (Checks and defects)
- L8: 6. The packet-delivery fault remained open at review; a proposed antenna move had not been tested. Two different devices had battery replacements, but no date links that work to the delivery check; the source says not to treat the battery work as a repair of the packet fault. (Checks and defects)
- L16: - Calibration establishes a pass only at the stated static 2.0-bar check. Accuracy beyond that check, including for readings not delivered, is not established; the source only says arrived readings were not shown to be inaccurate. (Fact 5)

## water-r3-B / open — kern — correct

Bron [L7](cases/water/source.md):

> The delivery fault remained open at review; a proposed antenna move had not been tested.

[content.md](calls/water-r3-B-Research/content.md)

- L7: 6. The packet-delivery fault remained open at review; a proposed antenna move had not been tested. Two different devices had battery replacements, but no date links that work to the delivery check; the source says not to treat the battery work as a repair of the packet fault. (Checks and defects)
- L19: - The packet fault’s eventual resolution and field delivery performance after any repair are unknown. The proposed antenna move was untested, and the battery replacements are not linked to or established as a repair for that fault. (Fact 6)

[projection.md](calls/water-r3-B-Research/projection.md)

- L8: 6. The packet-delivery fault remained open at review; a proposed antenna move had not been tested. Two different devices had battery replacements, but no date links that work to the delivery check; the source says not to treat the battery work as a repair of the packet fault. (Checks and defects)
- L17: - The packet fault’s eventual resolution and field delivery performance after any repair are unknown. The proposed antenna move was untested, and the battery replacements are not linked to or established as a repair for that fault. (Fact 6)

## water-r3-B / chronology — belangrijk — correct

Bron [L7](cases/water/source.md):

> A battery replacement was completed on two different devices, but the dossier provides no date linking that work to the delivery check.

[content.md](calls/water-r3-B-Research/content.md)

- L7: 6. The packet-delivery fault remained open at review; a proposed antenna move had not been tested. Two different devices had battery replacements, but no date links that work to the delivery check; the source says not to treat the battery work as a repair of the packet fault. (Checks and defects)
- L19: - The packet fault’s eventual resolution and field delivery performance after any repair are unknown. The proposed antenna move was untested, and the battery replacements are not linked to or established as a repair for that fault. (Fact 6)

[projection.md](calls/water-r3-B-Research/projection.md)

- L8: 6. The packet-delivery fault remained open at review; a proposed antenna move had not been tested. Two different devices had battery replacements, but no date links that work to the delivery check; the source says not to treat the battery work as a repair of the packet fault. (Checks and defects)
- L17: - The packet fault’s eventual resolution and field delivery performance after any repair are unknown. The proposed antenna move was untested, and the battery replacements are not linked to or established as a repair for that fault. (Fact 6)

## water-r3-B / term — kern — correct

Bron [L8](cases/water/source.md):

> A bench check recorded three brownouts: transient supply-voltage drops causing device restarts, not a regional power outage.

[content.md](calls/water-r3-B-Research/content.md)

- L8: 7. A bench check recorded three brownouts, defined as transient supply-voltage drops causing device restarts—not a regional power outage. A capacitor swap and firmware update preceded a repeat bench check with no brownouts. Their separate contributions were not identified, and the bench result does not certify field uptime. (Checks and defects)
- L26: - On the bench, three brownouts were recorded; the capacitor swap and firmware update were then made before a repeat check recorded none. The source does not isolate the changes’ effects or establish field results. (Fact 7)

[projection.md](calls/water-r3-B-Research/projection.md)

- L9: 7. A bench check recorded three brownouts, defined as transient supply-voltage drops causing device restarts—not a regional power outage. A capacitor swap and firmware update preceded a repeat bench check with no brownouts. Their separate contributions were not identified, and the bench result does not certify field uptime. (Checks and defects)

## water-r3-B / attribution — kern — correct

Bron [L8](cases/water/source.md):

> A capacitor swap and firmware update were both made before a repeat bench check with no brownouts. Their separate contributions were not identified; the bench result does not certify field uptime.

[content.md](calls/water-r3-B-Research/content.md)

- L8: 7. A bench check recorded three brownouts, defined as transient supply-voltage drops causing device restarts—not a regional power outage. A capacitor swap and firmware update preceded a repeat bench check with no brownouts. Their separate contributions were not identified, and the bench result does not certify field uptime. (Checks and defects)
- L20: - The individual contributions of the capacitor swap and firmware update, and whether the repeat bench result predicts field uptime, are unknown. (Fact 7)
- L26: - On the bench, three brownouts were recorded; the capacitor swap and firmware update were then made before a repeat check recorded none. The source does not isolate the changes’ effects or establish field results. (Fact 7)

[projection.md](calls/water-r3-B-Research/projection.md)

- L9: 7. A bench check recorded three brownouts, defined as transient supply-voltage drops causing device restarts—not a regional power outage. A capacitor swap and firmware update preceded a repeat bench check with no brownouts. Their separate contributions were not identified, and the bench result does not certify field uptime. (Checks and defects)
- L18: - The individual contributions of the capacitor swap and firmware update, and whether the repeat bench result predicts field uptime, are unknown. (Fact 7)

## water-r3-B / variation — kern — correct

Bron [L10](cases/water/source.md):

> Three replays of the same stored trace emitted 29, 31 and 28 alerts; a nondeterministic deduplication tie-breaker was enabled.

[content.md](calls/water-r3-B-Research/content.md)

- L9: 8. Three replays of the same stored trace emitted 29, 31, and 28 alerts with a nondeterministic deduplication tie-breaker enabled. The team did not designate one replay as the true count; replay counts do not replace the final field ledger’s 29. (Repeats and decision)
- L21: - No replay was selected as the true count; the source does not establish why counts varied beyond noting the nondeterministic tie-breaker. (Fact 8)

[projection.md](calls/water-r3-B-Research/projection.md)

- L10: 8. Three replays of the same stored trace emitted 29, 31, and 28 alerts with a nondeterministic deduplication tie-breaker enabled. The team did not designate one replay as the true count; replay counts do not replace the final field ledger’s 29. (Repeats and decision)
- L19: - No replay was selected as the true count; the source does not establish why counts varied beyond noting the nondeterministic tie-breaker. (Fact 8)

## water-r3-B / decision — kern — correct

Bron [L11](cases/water/source.md):

> The committee authorized a six-week paired measurement study with unchanged thresholds. It did not authorize network-wide deployment.

[content.md](calls/water-r3-B-Research/content.md)

- L10: 9. The committee authorized a six-week paired measurement study with unchanged thresholds, but did not authorize network-wide deployment. The EUR 18,000 budget covers logger rental and field visits, excludes staff overtime, and does not establish savings. No safety, leak-prevention, or annual reliability benefit was measured. (Repeats and decision)
- L22: - Network-wide performance, savings, safety, leak-prevention, and annual reliability benefits remain unmeasured or unauthorized as stated. (Fact 9)
- L27: - After review, the committee authorized a six-week paired study with unchanged thresholds; it did not authorize network-wide deployment. This is an authorization, not a completed study. (Fact 9)

[projection.md](calls/water-r3-B-Research/projection.md)

- L11: 9. The committee authorized a six-week paired measurement study with unchanged thresholds, but did not authorize network-wide deployment. The EUR 18,000 budget covers logger rental and field visits, excludes staff overtime, and does not establish savings. No safety, leak-prevention, or annual reliability benefit was measured. (Repeats and decision)
- L20: - Network-wide performance, savings, safety, leak-prevention, and annual reliability benefits remain unmeasured or unauthorized as stated. (Fact 9)

## water-r3-B / cost — belangrijk — correct

Bron [L11](cases/water/source.md):

> The budget of EUR 18,000 covers logger rental and field visits, excludes staff overtime and does not establish savings.

[content.md](calls/water-r3-B-Research/content.md)

- L10: 9. The committee authorized a six-week paired measurement study with unchanged thresholds, but did not authorize network-wide deployment. The EUR 18,000 budget covers logger rental and field visits, excludes staff overtime, and does not establish savings. No safety, leak-prevention, or annual reliability benefit was measured. (Repeats and decision)
- L22: - Network-wide performance, savings, safety, leak-prevention, and annual reliability benefits remain unmeasured or unauthorized as stated. (Fact 9)

[projection.md](calls/water-r3-B-Research/projection.md)

- L11: 9. The committee authorized a six-week paired measurement study with unchanged thresholds, but did not authorize network-wide deployment. The EUR 18,000 budget covers logger rental and field visits, excludes staff overtime, and does not establish savings. No safety, leak-prevention, or annual reliability benefit was measured. (Repeats and decision)
- L20: - Network-wide performance, savings, safety, leak-prevention, and annual reliability benefits remain unmeasured or unauthorized as stated. (Fact 9)

## language-r3-A / sample — kern — correct

Bron [L4](cases/language/source.md):

> Reviewers sampled 80 of 640 message strings: 40 Spanish and 40 Polish.

[content.md](calls/language-r3-A-Research/content.md)

- L7: 5. Reviewers sampled 80 of 640 message strings: 40 Spanish and 40 Polish.
- L8: 6. All 80 sampled strings retained the glossary’s approved entitlement terms.
- L12: 10. The source says neither check result certifies all 640 strings.
- L32: 30. Three runs on the same 80 strings flagged six, five, and six semantic issues with a sampling-based checker.
- L57: - The source does not establish the semantic correctness of all 640 strings; the reported linguistic review covers a sample, and the remaining strings did not receive the second semantic review.
- L72: - Reviewers sampled 80 of 640 message strings; the source does not establish when this review occurred relative to the other checks.
- L77: - Three checker runs on the same 80 strings flagged six, five, and six issues; a human reference review identified six deadline reversals. The source says the automated counts do not establish different ground truths or a stable checker.

[projection.md](calls/language-r3-A-Research/projection.md)

- L7: 5. Reviewers sampled 80 of 640 message strings: 40 Spanish and 40 Polish.
- L8: 6. All 80 sampled strings retained the glossary’s approved entitlement terms.
- L12: 10. The source says neither check result certifies all 640 strings.
- L32: 30. Three runs on the same 80 strings flagged six, five, and six semantic issues with a sampling-based checker.
- L53: - The source does not establish the semantic correctness of all 640 strings; the reported linguistic review covers a sample, and the remaining strings did not receive the second semantic review.

## language-r3-A / contrast — kern — correct

Bron [L4](cases/language/source.md):

> All 80 retained the glossary's approved entitlement terms. Six Spanish strings nevertheless reversed the meaning of the deadline exception. The Polish sample had no deadline reversal.

[content.md](calls/language-r3-A-Research/content.md)

- L8: 6. All 80 sampled strings retained the glossary’s approved entitlement terms.
- L9: 7. Six Spanish strings reversed the meaning of the deadline exception.
- L10: 8. The Polish sample had no deadline reversal.
- L11: 9. The source says terminology conformity and semantic correctness were separate checks.
- L13: 11. A second reviewer checked the six reversals and agreed.
- L33: 31. A human reference review identified the six deadline reversals.
- L56: - The source does not establish why the six Spanish deadline exceptions were reversed.
- L73: - The sample included six Spanish deadline reversals; a second reviewer checked those six and agreed. The remaining strings did not receive that second semantic review.
- L77: - Three checker runs on the same 80 strings flagged six, five, and six issues; a human reference review identified six deadline reversals. The source says the automated counts do not establish different ground truths or a stable checker.
- L82: - Facts 5–12 are relevant to the sampled terminology and semantic checks and their coverage limits.

[projection.md](calls/language-r3-A-Research/projection.md)

- L8: 6. All 80 sampled strings retained the glossary’s approved entitlement terms.
- L9: 7. Six Spanish strings reversed the meaning of the deadline exception.
- L10: 8. The Polish sample had no deadline reversal.
- L11: 9. The source says terminology conformity and semantic correctness were separate checks.
- L13: 11. A second reviewer checked the six reversals and agreed.
- L33: 31. A human reference review identified the six deadline reversals.
- L52: - The source does not establish why the six Spanish deadline exceptions were reversed.

## language-r3-A / review — belangrijk — correct

Bron [L4](cases/language/source.md):

> A second reviewer checked the six reversals and agreed. The remaining strings did not receive that second semantic review.

[content.md](calls/language-r3-A-Research/content.md)

- L13: 11. A second reviewer checked the six reversals and agreed.
- L14: 12. The remaining strings did not receive that second semantic review.
- L57: - The source does not establish the semantic correctness of all 640 strings; the reported linguistic review covers a sample, and the remaining strings did not receive the second semantic review.
- L73: - The sample included six Spanish deadline reversals; a second reviewer checked those six and agreed. The remaining strings did not receive that second semantic review.

[projection.md](calls/language-r3-A-Research/projection.md)

- L13: 11. A second reviewer checked the six reversals and agreed.
- L14: 12. The remaining strings did not receive that second semantic review.
- L53: - The source does not establish the semantic correctness of all 640 strings; the reported linguistic review covers a sample, and the remaining strings did not receive the second semantic review.

## language-r3-A / access — kern — correct

Bron [L6](cases/language/source.md):

> An automated colour-contrast scan passed the three tested page templates. Keyboard-only testing could not activate the document-upload button on the second template.

[content.md](calls/language-r3-A-Research/content.md)

- L15: 13. An automated colour-contrast scan passed the three tested page templates.
- L16: 14. Keyboard-only testing could not activate the document-upload button on the second template.
- L17: 15. The upload defect was still open.
- L18: 16. Screen-reader testing announced the upload label correctly on the second template.
- L19: 17. Screen-reader testing did not test keyboard activation.
- L20: 18. The source says a visually correct label is not evidence that a keyboard user can upload a document.
- L21: 19. Management approved a keyboard-fix proposal.
- L22: 20. The source records no deployed keyboard fix.
- L40: 38. The coordinator authorised a corrected-form usability study conditional on a verified upload fix.
- L58: - The source does not establish accessibility across all pages, templates, tasks, or users. It reports results for three tested templates and identifies an open upload defect on the second template; it also says no general accessibility certification was commissioned.
- L59: - The source does not establish whether a keyboard fix was later deployed or whether the upload defect was resolved.
- L68: - The source does not establish the outcome of the authorised usability study, which was conditional on a verified upload fix.
- L74: - The source reports access checks: the contrast scan passed on three templates; keyboard-only testing could not activate the upload button on the second template; screen-reader testing announced the label on that template but did not test keyboard activation. The temporal order among these checks is not established.
- L75: - The upload defect was still open. Management approved a keyboard-fix proposal; no deployed fix is recorded. The source does not establish whether the proposal was implemented later.
- L78: - The coordinator authorised a corrected-form usability study conditional on a verified upload fix and withheld general release. The study’s outcome is not reported.
- L83: - Facts 13–20 are relevant to the reported access checks, upload defect, and fix status.

[projection.md](calls/language-r3-A-Research/projection.md)

- L15: 13. An automated colour-contrast scan passed the three tested page templates.
- L16: 14. Keyboard-only testing could not activate the document-upload button on the second template.
- L17: 15. The upload defect was still open.
- L18: 16. Screen-reader testing announced the upload label correctly on the second template.
- L19: 17. Screen-reader testing did not test keyboard activation.
- L20: 18. The source says a visually correct label is not evidence that a keyboard user can upload a document.
- L21: 19. Management approved a keyboard-fix proposal.
- L22: 20. The source records no deployed keyboard fix.
- L40: 38. The coordinator authorised a corrected-form usability study conditional on a verified upload fix.
- L54: - The source does not establish accessibility across all pages, templates, tasks, or users. It reports results for three tested templates and identifies an open upload defect on the second template; it also says no general accessibility certification was commissioned.
- L55: - The source does not establish whether a keyboard fix was later deployed or whether the upload defect was resolved.
- L64: - The source does not establish the outcome of the authorised usability study, which was conditional on a verified upload fix.

## language-r3-A / open — kern — correct

Bron [L6](cases/language/source.md):

> The upload defect was still open. Screen-reader testing announced the upload label correctly on that template, but did not test keyboard activation.

[content.md](calls/language-r3-A-Research/content.md)

- L17: 15. The upload defect was still open.
- L18: 16. Screen-reader testing announced the upload label correctly on the second template.
- L19: 17. Screen-reader testing did not test keyboard activation.
- L20: 18. The source says a visually correct label is not evidence that a keyboard user can upload a document.
- L58: - The source does not establish accessibility across all pages, templates, tasks, or users. It reports results for three tested templates and identifies an open upload defect on the second template; it also says no general accessibility certification was commissioned.
- L74: - The source reports access checks: the contrast scan passed on three templates; keyboard-only testing could not activate the upload button on the second template; screen-reader testing announced the label on that template but did not test keyboard activation. The temporal order among these checks is not established.
- L75: - The upload defect was still open. Management approved a keyboard-fix proposal; no deployed fix is recorded. The source does not establish whether the proposal was implemented later.

[projection.md](calls/language-r3-A-Research/projection.md)

- L17: 15. The upload defect was still open.
- L18: 16. Screen-reader testing announced the upload label correctly on the second template.
- L19: 17. Screen-reader testing did not test keyboard activation.
- L20: 18. The source says a visually correct label is not evidence that a keyboard user can upload a document.
- L54: - The source does not establish accessibility across all pages, templates, tasks, or users. It reports results for three tested templates and identifies an open upload defect on the second template; it also says no general accessibility certification was commissioned.

## language-r3-A / outcome — kern — correct

Bron [L7](cases/language/source.md):

> Task completion in the old English-only service was 33 of 50 moderated sessions. The multilingual form recorded 42 of 50 unmoderated sessions.

[content.md](calls/language-r3-A-Research/content.md)

- L23: 21. Task completion in the old English-only service was 33 of 50 moderated sessions.
- L24: 22. The multilingual form recorded 42 of 50 unmoderated sessions.
- L35: 33. The checker version was held fixed, but its sample order varied.
- L44: 42. A separate satisfaction poll received 18 responses from 90 invitations.
- L76: - The old English-only service recorded 33 completions in 50 moderated sessions; the multilingual form recorded 42 in 50 unmoderated sessions. These are different measures and conditions, and the source says they do not provide a comparable completion-effect estimate.
- L85: - Facts 28–33 are relevant to the processing definition and checker-run variation.
- L87: - Facts 42–46 are relevant to the satisfaction poll and the limits on reporting the inclusive-launch phrase as a finding.

[projection.md](calls/language-r3-A-Research/projection.md)

- L23: 21. Task completion in the old English-only service was 33 of 50 moderated sessions.
- L24: 22. The multilingual form recorded 42 of 50 unmoderated sessions.
- L35: 33. The checker version was held fixed, but its sample order varied.
- L44: 42. A separate satisfaction poll received 18 responses from 90 invitations.

## language-r3-A / method — kern — correct

Bron [L7](cases/language/source.md):

> Completion formerly required a final submitted application; the new dashboard counted reaching the review screen.

[content.md](calls/language-r3-A-Research/content.md)

- L25: 23. In the old service, completion required a final submitted application.
- L26: 24. The new dashboard counted reaching the review screen as completion.

[projection.md](calls/language-r3-A-Research/projection.md)

- L25: 23. In the old service, completion required a final submitted application.
- L26: 24. The new dashboard counted reaching the review screen as completion.

## language-r3-A / allocation — belangrijk — correct

Bron [L7](cases/language/source.md):

> Participants were recruited from different channels and neither assigned randomly nor matched for prior experience.

[content.md](calls/language-r3-A-Research/content.md)

- L27: 25. Participants were recruited from different channels.
- L28: 26. Participants were neither assigned randomly nor matched for prior experience.

[projection.md](calls/language-r3-A-Research/projection.md)

- L27: 25. Participants were recruited from different channels.
- L28: 26. Participants were neither assigned randomly nor matched for prior experience.

## language-r3-A / term — kern — correct

Bron [L9](cases/language/source.md):

> A preprocessing step called case folding normalised letter case; it did not merge benefit cases or applications.

[content.md](calls/language-r3-A-Research/content.md)

- L30: 28. A preprocessing step called case folding normalised letter case.
- L31: 29. Case folding did not merge benefit cases or applications.

[projection.md](calls/language-r3-A-Research/projection.md)

- L30: 28. A preprocessing step called case folding normalised letter case.
- L31: 29. Case folding did not merge benefit cases or applications.

## language-r3-A / variation — kern — correct

Bron [L9](cases/language/source.md):

> Three runs on the same 80 strings flagged six, five and six semantic issues with a sampling-based checker. A human reference review identified the six deadline reversals.

[content.md](calls/language-r3-A-Research/content.md)

- L32: 30. Three runs on the same 80 strings flagged six, five, and six semantic issues with a sampling-based checker.
- L33: 31. A human reference review identified the six deadline reversals.
- L34: 32. The source says automated run counts do not establish three different ground truths or a stable checker.
- L35: 33. The checker version was held fixed, but its sample order varied.
- L77: - Three checker runs on the same 80 strings flagged six, five, and six issues; a human reference review identified six deadline reversals. The source says the automated counts do not establish different ground truths or a stable checker.
- L85: - Facts 28–33 are relevant to the processing definition and checker-run variation.

[projection.md](calls/language-r3-A-Research/projection.md)

- L32: 30. Three runs on the same 80 strings flagged six, five, and six semantic issues with a sampling-based checker.
- L33: 31. A human reference review identified the six deadline reversals.
- L34: 32. The source says automated run counts do not establish three different ground truths or a stable checker.
- L35: 33. The checker version was held fixed, but its sample order varied.

## language-r3-A / legal — kern — correct

Bron [L11](cases/language/source.md):

> The appendix says legal approval covers the Spanish consent paragraph only. It gives no approval date or temporal relationship to the sample review. Polish consent approval is not documented.

[content.md](calls/language-r3-A-Research/content.md)

- L36: 34. The appendix says legal approval covers only the Spanish consent paragraph.
- L37: 35. The appendix gives no approval date or temporal relationship between legal approval and the sample review.
- L38: 36. Polish consent approval is not documented.
- L39: 37. The source says translation availability does not itself establish legal approval.
- L63: - The source does not establish a temporal relationship between Spanish consent legal approval and the sample review, or documented legal approval for Polish consent.
- L64: - The source does not establish that translation availability means legal approval.

[projection.md](calls/language-r3-A-Research/projection.md)

- L36: 34. The appendix says legal approval covers only the Spanish consent paragraph.
- L37: 35. The appendix gives no approval date or temporal relationship between legal approval and the sample review.
- L38: 36. Polish consent approval is not documented.
- L39: 37. The source says translation availability does not itself establish legal approval.
- L59: - The source does not establish a temporal relationship between Spanish consent legal approval and the sample review, or documented legal approval for Polish consent.
- L60: - The source does not establish that translation availability means legal approval.

## language-r3-A / decision — kern — correct

Bron [L11](cases/language/source.md):

> The coordinator authorised a corrected-form usability study, conditional on a verified upload fix, and withheld general release.

[content.md](calls/language-r3-A-Research/content.md)

- L40: 38. The coordinator authorised a corrected-form usability study conditional on a verified upload fix.
- L41: 39. The coordinator withheld general release.
- L68: - The source does not establish the outcome of the authorised usability study, which was conditional on a verified upload fix.
- L78: - The coordinator authorised a corrected-form usability study conditional on a verified upload fix and withheld general release. The study’s outcome is not reported.

[projection.md](calls/language-r3-A-Research/projection.md)

- L40: 38. The coordinator authorised a corrected-form usability study conditional on a verified upload fix.
- L41: 39. The coordinator withheld general release.
- L64: - The source does not establish the outcome of the authorised usability study, which was conditional on a verified upload fix.

## language-r3-B / sample — kern — correct

Bron [L4](cases/language/source.md):

> Reviewers sampled 80 of 640 message strings: 40 Spanish and 40 Polish.

[content.md](calls/language-r3-B-Research/content.md)

- L3: 2. Reviewers sampled 80 of 640 message strings: 40 Spanish and 40 Polish. All 80 retained approved glossary entitlement terms, but six Spanish strings reversed the deadline exception; none of the Polish sample did. Terminology conformity and semantic correctness were separate checks. (“Sample and linguistic checks”)
- L4: 3. A second reviewer agreed with the six Spanish reversals. The remaining strings did not receive that second semantic review; the sample results do not certify all 640 strings. (“Sample and linguistic checks”)
- L9: 8. “Case folding” normalised letter case; it did not merge benefit cases or applications. On the same 80 strings, three runs of a sampling-based checker flagged six, five and six semantic issues; the checker version was fixed, but sample order varied. (“Processing and reproducibility”)
- L20: - The source does not establish semantic correctness across all 640 strings, or independent second-review confirmation beyond the six identified reversals (Facts 2–3).

[projection.md](calls/language-r3-B-Research/projection.md)

- L4: 2. Reviewers sampled 80 of 640 message strings: 40 Spanish and 40 Polish. All 80 retained approved glossary entitlement terms, but six Spanish strings reversed the deadline exception; none of the Polish sample did. Terminology conformity and semantic correctness were separate checks. (“Sample and linguistic checks”)
- L5: 3. A second reviewer agreed with the six Spanish reversals. The remaining strings did not receive that second semantic review; the sample results do not certify all 640 strings. (“Sample and linguistic checks”)
- L10: 8. “Case folding” normalised letter case; it did not merge benefit cases or applications. On the same 80 strings, three runs of a sampling-based checker flagged six, five and six semantic issues; the checker version was fixed, but sample order varied. (“Processing and reproducibility”)
- L18: - The source does not establish semantic correctness across all 640 strings, or independent second-review confirmation beyond the six identified reversals (Facts 2–3).

## language-r3-B / contrast — kern — correct

Bron [L4](cases/language/source.md):

> All 80 retained the glossary's approved entitlement terms. Six Spanish strings nevertheless reversed the meaning of the deadline exception. The Polish sample had no deadline reversal.

[content.md](calls/language-r3-B-Research/content.md)

- L3: 2. Reviewers sampled 80 of 640 message strings: 40 Spanish and 40 Polish. All 80 retained approved glossary entitlement terms, but six Spanish strings reversed the deadline exception; none of the Polish sample did. Terminology conformity and semantic correctness were separate checks. (“Sample and linguistic checks”)
- L4: 3. A second reviewer agreed with the six Spanish reversals. The remaining strings did not receive that second semantic review; the sample results do not certify all 640 strings. (“Sample and linguistic checks”)
- L10: 9. A human reference review identified six deadline reversals. The source says the automated counts establish neither three different ground truths nor a stable checker. (“Processing and reproducibility”)
- L16: - The evidence supports separating terminology conformity, semantic accuracy, technical access and task outcomes rather than treating any one positive check as proof of overall service accessibility (Facts 2–7).
- L20: - The source does not establish semantic correctness across all 640 strings, or independent second-review confirmation beyond the six identified reversals (Facts 2–3).
- L29: - The review sampled strings and reported terminology and semantic checks, including the six Spanish deadline reversals (Facts 2–3); the source does not date these checks relative to legal approval (Fact 10).

[projection.md](calls/language-r3-B-Research/projection.md)

- L4: 2. Reviewers sampled 80 of 640 message strings: 40 Spanish and 40 Polish. All 80 retained approved glossary entitlement terms, but six Spanish strings reversed the deadline exception; none of the Polish sample did. Terminology conformity and semantic correctness were separate checks. (“Sample and linguistic checks”)
- L5: 3. A second reviewer agreed with the six Spanish reversals. The remaining strings did not receive that second semantic review; the sample results do not certify all 640 strings. (“Sample and linguistic checks”)
- L11: 9. A human reference review identified six deadline reversals. The source says the automated counts establish neither three different ground truths nor a stable checker. (“Processing and reproducibility”)
- L18: - The source does not establish semantic correctness across all 640 strings, or independent second-review confirmation beyond the six identified reversals (Facts 2–3).

## language-r3-B / review — belangrijk — correct

Bron [L4](cases/language/source.md):

> A second reviewer checked the six reversals and agreed. The remaining strings did not receive that second semantic review.

[content.md](calls/language-r3-B-Research/content.md)

- L4: 3. A second reviewer agreed with the six Spanish reversals. The remaining strings did not receive that second semantic review; the sample results do not certify all 640 strings. (“Sample and linguistic checks”)
- L20: - The source does not establish semantic correctness across all 640 strings, or independent second-review confirmation beyond the six identified reversals (Facts 2–3).

[projection.md](calls/language-r3-B-Research/projection.md)

- L5: 3. A second reviewer agreed with the six Spanish reversals. The remaining strings did not receive that second semantic review; the sample results do not certify all 640 strings. (“Sample and linguistic checks”)
- L18: - The source does not establish semantic correctness across all 640 strings, or independent second-review confirmation beyond the six identified reversals (Facts 2–3).

## language-r3-B / access — kern — correct

Bron [L6](cases/language/source.md):

> An automated colour-contrast scan passed the three tested page templates. Keyboard-only testing could not activate the document-upload button on the second template.

[content.md](calls/language-r3-B-Research/content.md)

- L5: 4. Automated colour-contrast testing passed three page templates. Keyboard-only testing could not activate the document-upload button on template two, and the defect remained open. Screen-reader testing correctly announced its label but did not test keyboard activation. (“Access and task results”)
- L6: 5. Management approved a keyboard-fix proposal, but no deployed fix is recorded. The source explicitly cautions that a visually correct label does not establish that keyboard users can upload a document. (“Access and task results”)
- L12: 11. The coordinator authorised a corrected-form usability study conditional on a verified upload fix, and withheld general release. The study budget includes translation fees but excludes staff time; no cost saving is demonstrated. (“Status and decision boundary”)
- L21: - It does not establish keyboard upload access on template two after a fix: the defect was open, and no deployed fix or post-fix verification is recorded (Facts 4–5, 11).
- L22: - It does not establish overall accessibility from the three-template contrast scan, correct screen-reader label, or any accessibility certification (Facts 1, 4–5).
- L30: - Access tests recorded contrast passes alongside the open keyboard upload failure; a fix proposal was approved, but no deployed fix is recorded (Facts 4–5).
- L31: - The coordinator authorised a corrected-form usability study conditional on a verified upload fix and withheld general release; this is a conditional plan, not a completed study or release (Fact 11).

[projection.md](calls/language-r3-B-Research/projection.md)

- L6: 4. Automated colour-contrast testing passed three page templates. Keyboard-only testing could not activate the document-upload button on template two, and the defect remained open. Screen-reader testing correctly announced its label but did not test keyboard activation. (“Access and task results”)
- L7: 5. Management approved a keyboard-fix proposal, but no deployed fix is recorded. The source explicitly cautions that a visually correct label does not establish that keyboard users can upload a document. (“Access and task results”)
- L13: 11. The coordinator authorised a corrected-form usability study conditional on a verified upload fix, and withheld general release. The study budget includes translation fees but excludes staff time; no cost saving is demonstrated. (“Status and decision boundary”)
- L19: - It does not establish keyboard upload access on template two after a fix: the defect was open, and no deployed fix or post-fix verification is recorded (Facts 4–5, 11).
- L20: - It does not establish overall accessibility from the three-template contrast scan, correct screen-reader label, or any accessibility certification (Facts 1, 4–5).

## language-r3-B / open — kern — correct

Bron [L6](cases/language/source.md):

> The upload defect was still open. Screen-reader testing announced the upload label correctly on that template, but did not test keyboard activation.

[content.md](calls/language-r3-B-Research/content.md)

- L5: 4. Automated colour-contrast testing passed three page templates. Keyboard-only testing could not activate the document-upload button on template two, and the defect remained open. Screen-reader testing correctly announced its label but did not test keyboard activation. (“Access and task results”)
- L6: 5. Management approved a keyboard-fix proposal, but no deployed fix is recorded. The source explicitly cautions that a visually correct label does not establish that keyboard users can upload a document. (“Access and task results”)
- L21: - It does not establish keyboard upload access on template two after a fix: the defect was open, and no deployed fix or post-fix verification is recorded (Facts 4–5, 11).
- L22: - It does not establish overall accessibility from the three-template contrast scan, correct screen-reader label, or any accessibility certification (Facts 1, 4–5).
- L30: - Access tests recorded contrast passes alongside the open keyboard upload failure; a fix proposal was approved, but no deployed fix is recorded (Facts 4–5).

[projection.md](calls/language-r3-B-Research/projection.md)

- L6: 4. Automated colour-contrast testing passed three page templates. Keyboard-only testing could not activate the document-upload button on template two, and the defect remained open. Screen-reader testing correctly announced its label but did not test keyboard activation. (“Access and task results”)
- L7: 5. Management approved a keyboard-fix proposal, but no deployed fix is recorded. The source explicitly cautions that a visually correct label does not establish that keyboard users can upload a document. (“Access and task results”)
- L19: - It does not establish keyboard upload access on template two after a fix: the defect was open, and no deployed fix or post-fix verification is recorded (Facts 4–5, 11).
- L20: - It does not establish overall accessibility from the three-template contrast scan, correct screen-reader label, or any accessibility certification (Facts 1, 4–5).

## language-r3-B / outcome — kern — correct

Bron [L7](cases/language/source.md):

> Task completion in the old English-only service was 33 of 50 moderated sessions. The multilingual form recorded 42 of 50 unmoderated sessions.

[content.md](calls/language-r3-B-Research/content.md)

- L7: 6. The old English-only service recorded completion in 33 of 50 moderated sessions; the multilingual form recorded 42 of 50 unmoderated sessions. The old measure required a final submitted application, while the new dashboard counted reaching the review screen. (“Access and task results”)

[projection.md](calls/language-r3-B-Research/projection.md)

- L8: 6. The old English-only service recorded completion in 33 of 50 moderated sessions; the multilingual form recorded 42 of 50 unmoderated sessions. The old measure required a final submitted application, while the new dashboard counted reaching the review screen. (“Access and task results”)

## language-r3-B / method — kern — correct

Bron [L7](cases/language/source.md):

> Completion formerly required a final submitted application; the new dashboard counted reaching the review screen.

[content.md](calls/language-r3-B-Research/content.md)

- L7: 6. The old English-only service recorded completion in 33 of 50 moderated sessions; the multilingual form recorded 42 of 50 unmoderated sessions. The old measure required a final submitted application, while the new dashboard counted reaching the review screen. (“Access and task results”)

[projection.md](calls/language-r3-B-Research/projection.md)

- L8: 6. The old English-only service recorded completion in 33 of 50 moderated sessions; the multilingual form recorded 42 of 50 unmoderated sessions. The old measure required a final submitted application, while the new dashboard counted reaching the review screen. (“Access and task results”)

## language-r3-B / allocation — belangrijk — correct

Bron [L7](cases/language/source.md):

> Participants were recruited from different channels and neither assigned randomly nor matched for prior experience.

[content.md](calls/language-r3-B-Research/content.md)

- L8: 7. Participants came through different recruitment channels and were neither randomly assigned nor matched for prior experience. The source says the apparent dashboard improvement is not a comparable completion-effect estimate. (“Access and task results”)

[projection.md](calls/language-r3-B-Research/projection.md)

- L9: 7. Participants came through different recruitment channels and were neither randomly assigned nor matched for prior experience. The source says the apparent dashboard improvement is not a comparable completion-effect estimate. (“Access and task results”)

## language-r3-B / term — kern — correct

Bron [L9](cases/language/source.md):

> A preprocessing step called case folding normalised letter case; it did not merge benefit cases or applications.

[content.md](calls/language-r3-B-Research/content.md)

- L9: 8. “Case folding” normalised letter case; it did not merge benefit cases or applications. On the same 80 strings, three runs of a sampling-based checker flagged six, five and six semantic issues; the checker version was fixed, but sample order varied. (“Processing and reproducibility”)

[projection.md](calls/language-r3-B-Research/projection.md)

- L10: 8. “Case folding” normalised letter case; it did not merge benefit cases or applications. On the same 80 strings, three runs of a sampling-based checker flagged six, five and six semantic issues; the checker version was fixed, but sample order varied. (“Processing and reproducibility”)

## language-r3-B / variation — kern — correct

Bron [L9](cases/language/source.md):

> Three runs on the same 80 strings flagged six, five and six semantic issues with a sampling-based checker. A human reference review identified the six deadline reversals.

[content.md](calls/language-r3-B-Research/content.md)

- L9: 8. “Case folding” normalised letter case; it did not merge benefit cases or applications. On the same 80 strings, three runs of a sampling-based checker flagged six, five and six semantic issues; the checker version was fixed, but sample order varied. (“Processing and reproducibility”)
- L10: 9. A human reference review identified six deadline reversals. The source says the automated counts establish neither three different ground truths nor a stable checker. (“Processing and reproducibility”)
- L24: - It does not establish checker stability or a single automated ground truth from the three run counts (Facts 8–9).

[projection.md](calls/language-r3-B-Research/projection.md)

- L10: 8. “Case folding” normalised letter case; it did not merge benefit cases or applications. On the same 80 strings, three runs of a sampling-based checker flagged six, five and six semantic issues; the checker version was fixed, but sample order varied. (“Processing and reproducibility”)
- L11: 9. A human reference review identified six deadline reversals. The source says the automated counts establish neither three different ground truths nor a stable checker. (“Processing and reproducibility”)
- L22: - It does not establish checker stability or a single automated ground truth from the three run counts (Facts 8–9).

## language-r3-B / legal — kern — correct

Bron [L11](cases/language/source.md):

> The appendix says legal approval covers the Spanish consent paragraph only. It gives no approval date or temporal relationship to the sample review. Polish consent approval is not documented.

[content.md](calls/language-r3-B-Research/content.md)

- L11: 10. Legal approval covers only the Spanish consent paragraph. No approval date or temporal relationship to the sample review is given, and Polish consent approval is not documented; translation availability alone does not establish legal approval. (“Status and decision boundary”)
- L25: - It does not establish Polish consent approval, when Spanish approval occurred, or whether that approval preceded the sample review (Fact 10).
- L29: - The review sampled strings and reported terminology and semantic checks, including the six Spanish deadline reversals (Facts 2–3); the source does not date these checks relative to legal approval (Fact 10).

[projection.md](calls/language-r3-B-Research/projection.md)

- L12: 10. Legal approval covers only the Spanish consent paragraph. No approval date or temporal relationship to the sample review is given, and Polish consent approval is not documented; translation availability alone does not establish legal approval. (“Status and decision boundary”)
- L23: - It does not establish Polish consent approval, when Spanish approval occurred, or whether that approval preceded the sample review (Fact 10).

## language-r3-B / decision — kern — correct

Bron [L11](cases/language/source.md):

> The coordinator authorised a corrected-form usability study, conditional on a verified upload fix, and withheld general release.

[content.md](calls/language-r3-B-Research/content.md)

- L12: 11. The coordinator authorised a corrected-form usability study conditional on a verified upload fix, and withheld general release. The study budget includes translation fees but excludes staff time; no cost saving is demonstrated. (“Status and decision boundary”)
- L31: - The coordinator authorised a corrected-form usability study conditional on a verified upload fix and withheld general release; this is a conditional plan, not a completed study or release (Fact 11).

[projection.md](calls/language-r3-B-Research/projection.md)

- L13: 11. The coordinator authorised a corrected-form usability study conditional on a verified upload fix, and withheld general release. The study budget includes translation fees but excludes staff time; no cost saving is demonstrated. (“Status and decision boundary”)

## language-r3-B2 / sample — kern — correct

Bron [L4](cases/language/source.md):

> Reviewers sampled 80 of 640 message strings: 40 Spanish and 40 Polish.

[content.md](calls/language-r3-B2-Research/content.md)

- L3: 2. Reviewers sampled 80 of 640 message strings: 40 Spanish and 40 Polish. All 80 retained approved glossary entitlement terms, but this does not certify the remaining strings. (Sample and linguistic checks)
- L8: 7. On the same 80 strings, three checker runs flagged six, five, and six semantic issues. The checker version was fixed, but sample order varied. A human reference review identified the six deadline reversals. The automated counts do not establish three different ground truths or a stable checker. (Processing and reproducibility)

[projection.md](calls/language-r3-B2-Research/projection.md)

- L4: 2. Reviewers sampled 80 of 640 message strings: 40 Spanish and 40 Polish. All 80 retained approved glossary entitlement terms, but this does not certify the remaining strings. (Sample and linguistic checks)
- L9: 7. On the same 80 strings, three checker runs flagged six, five, and six semantic issues. The checker version was fixed, but sample order varied. A human reference review identified the six deadline reversals. The automated counts do not establish three different ground truths or a stable checker. (Processing and reproducibility)

## language-r3-B2 / contrast — kern — correct

Bron [L4](cases/language/source.md):

> All 80 retained the glossary's approved entitlement terms. Six Spanish strings nevertheless reversed the meaning of the deadline exception. The Polish sample had no deadline reversal.

[content.md](calls/language-r3-B2-Research/content.md)

- L3: 2. Reviewers sampled 80 of 640 message strings: 40 Spanish and 40 Polish. All 80 retained approved glossary entitlement terms, but this does not certify the remaining strings. (Sample and linguistic checks)
- L4: 3. Six Spanish strings reversed the deadline exception’s meaning; the Polish sample had no such reversal. Terminology conformity and semantic correctness were separate checks. A second reviewer agreed on the six reversals; the remaining strings did not receive that second semantic review. (Sample and linguistic checks)
- L8: 7. On the same 80 strings, three checker runs flagged six, five, and six semantic issues. The checker version was fixed, but sample order varied. A human reference review identified the six deadline reversals. The automated counts do not establish three different ground truths or a stable checker. (Processing and reproducibility)
- L18: - Whether unreviewed strings contain semantic errors; the second semantic review covered only the six identified reversals (F3).
- L29: F2–F3: terminology and semantic findings; F4: access checks and unresolved upload defect; F5 and F7: measurement and attribution limits; F8–F11: approval, decision boundary, cost, and poll limits.

[projection.md](calls/language-r3-B2-Research/projection.md)

- L4: 2. Reviewers sampled 80 of 640 message strings: 40 Spanish and 40 Polish. All 80 retained approved glossary entitlement terms, but this does not certify the remaining strings. (Sample and linguistic checks)
- L5: 3. Six Spanish strings reversed the deadline exception’s meaning; the Polish sample had no such reversal. Terminology conformity and semantic correctness were separate checks. A second reviewer agreed on the six reversals; the remaining strings did not receive that second semantic review. (Sample and linguistic checks)
- L9: 7. On the same 80 strings, three checker runs flagged six, five, and six semantic issues. The checker version was fixed, but sample order varied. A human reference review identified the six deadline reversals. The automated counts do not establish three different ground truths or a stable checker. (Processing and reproducibility)
- L17: - Whether unreviewed strings contain semantic errors; the second semantic review covered only the six identified reversals (F3).

## language-r3-B2 / review — belangrijk — correct

Bron [L4](cases/language/source.md):

> A second reviewer checked the six reversals and agreed. The remaining strings did not receive that second semantic review.

[content.md](calls/language-r3-B2-Research/content.md)

- L3: 2. Reviewers sampled 80 of 640 message strings: 40 Spanish and 40 Polish. All 80 retained approved glossary entitlement terms, but this does not certify the remaining strings. (Sample and linguistic checks)
- L4: 3. Six Spanish strings reversed the deadline exception’s meaning; the Polish sample had no such reversal. Terminology conformity and semantic correctness were separate checks. A second reviewer agreed on the six reversals; the remaining strings did not receive that second semantic review. (Sample and linguistic checks)
- L18: - Whether unreviewed strings contain semantic errors; the second semantic review covered only the six identified reversals (F3).

[projection.md](calls/language-r3-B2-Research/projection.md)

- L4: 2. Reviewers sampled 80 of 640 message strings: 40 Spanish and 40 Polish. All 80 retained approved glossary entitlement terms, but this does not certify the remaining strings. (Sample and linguistic checks)
- L5: 3. Six Spanish strings reversed the deadline exception’s meaning; the Polish sample had no such reversal. Terminology conformity and semantic correctness were separate checks. A second reviewer agreed on the six reversals; the remaining strings did not receive that second semantic review. (Sample and linguistic checks)
- L17: - Whether unreviewed strings contain semantic errors; the second semantic review covered only the six identified reversals (F3).

## language-r3-B2 / access — kern — correct

Bron [L6](cases/language/source.md):

> An automated colour-contrast scan passed the three tested page templates. Keyboard-only testing could not activate the document-upload button on the second template.

[content.md](calls/language-r3-B2-Research/content.md)

- L5: 4. An automated contrast scan passed on three tested page templates. Keyboard-only testing could not activate the document-upload button on the second template, and that defect was still open. Screen-reader testing announced the label correctly there but did not test keyboard activation. Management approved a keyboard-fix proposal; no deployed fix is recorded. (Access and task results)
- L10: 9. The coordinator authorised a corrected-form usability study conditional on a verified upload fix and withheld general release. (Status and decision boundary)
- L19: - Accessibility beyond the specific checks reported, and whether keyboard upload works after a fix; the defect was open and no deployed fix is recorded (F1, F4).
- L29: F2–F3: terminology and semantic findings; F4: access checks and unresolved upload defect; F5 and F7: measurement and attribution limits; F8–F11: approval, decision boundary, cost, and poll limits.

[projection.md](calls/language-r3-B2-Research/projection.md)

- L6: 4. An automated contrast scan passed on three tested page templates. Keyboard-only testing could not activate the document-upload button on the second template, and that defect was still open. Screen-reader testing announced the label correctly there but did not test keyboard activation. Management approved a keyboard-fix proposal; no deployed fix is recorded. (Access and task results)
- L11: 9. The coordinator authorised a corrected-form usability study conditional on a verified upload fix and withheld general release. (Status and decision boundary)
- L18: - Accessibility beyond the specific checks reported, and whether keyboard upload works after a fix; the defect was open and no deployed fix is recorded (F1, F4).

## language-r3-B2 / open — kern — correct

Bron [L6](cases/language/source.md):

> The upload defect was still open. Screen-reader testing announced the upload label correctly on that template, but did not test keyboard activation.

[content.md](calls/language-r3-B2-Research/content.md)

- L2: 1. The note describes a fictional benefits form and contains no real resident data. Management’s “inclusive launch” phrase states an ambition, not a measured inclusion outcome; no general accessibility certification was commissioned. (Opening)
- L5: 4. An automated contrast scan passed on three tested page templates. Keyboard-only testing could not activate the document-upload button on the second template, and that defect was still open. Screen-reader testing announced the label correctly there but did not test keyboard activation. Management approved a keyboard-fix proposal; no deployed fix is recorded. (Access and task results)
- L19: - Accessibility beyond the specific checks reported, and whether keyboard upload works after a fix; the defect was open and no deployed fix is recorded (F1, F4).

[projection.md](calls/language-r3-B2-Research/projection.md)

- L3: 1. The note describes a fictional benefits form and contains no real resident data. Management’s “inclusive launch” phrase states an ambition, not a measured inclusion outcome; no general accessibility certification was commissioned. (Opening)
- L6: 4. An automated contrast scan passed on three tested page templates. Keyboard-only testing could not activate the document-upload button on the second template, and that defect was still open. Screen-reader testing announced the label correctly there but did not test keyboard activation. Management approved a keyboard-fix proposal; no deployed fix is recorded. (Access and task results)
- L18: - Accessibility beyond the specific checks reported, and whether keyboard upload works after a fix; the defect was open and no deployed fix is recorded (F1, F4).

## language-r3-B2 / outcome — kern — correct

Bron [L7](cases/language/source.md):

> Task completion in the old English-only service was 33 of 50 moderated sessions. The multilingual form recorded 42 of 50 unmoderated sessions.

[content.md](calls/language-r3-B2-Research/content.md)

- L6: 5. The old English-only service completed 33 of 50 moderated sessions, with completion requiring a final submitted application. The multilingual form recorded 42 of 50 unmoderated sessions, counting reaching the review screen. Participants came from different recruitment channels and were neither randomly assigned nor matched for prior experience. The source says this is not a comparable completion-effect estimate. (Access and task results)

[projection.md](calls/language-r3-B2-Research/projection.md)

- L7: 5. The old English-only service completed 33 of 50 moderated sessions, with completion requiring a final submitted application. The multilingual form recorded 42 of 50 unmoderated sessions, counting reaching the review screen. Participants came from different recruitment channels and were neither randomly assigned nor matched for prior experience. The source says this is not a comparable completion-effect estimate. (Access and task results)

## language-r3-B2 / method — kern — correct

Bron [L7](cases/language/source.md):

> Completion formerly required a final submitted application; the new dashboard counted reaching the review screen.

[content.md](calls/language-r3-B2-Research/content.md)

- L6: 5. The old English-only service completed 33 of 50 moderated sessions, with completion requiring a final submitted application. The multilingual form recorded 42 of 50 unmoderated sessions, counting reaching the review screen. Participants came from different recruitment channels and were neither randomly assigned nor matched for prior experience. The source says this is not a comparable completion-effect estimate. (Access and task results)

[projection.md](calls/language-r3-B2-Research/projection.md)

- L7: 5. The old English-only service completed 33 of 50 moderated sessions, with completion requiring a final submitted application. The multilingual form recorded 42 of 50 unmoderated sessions, counting reaching the review screen. Participants came from different recruitment channels and were neither randomly assigned nor matched for prior experience. The source says this is not a comparable completion-effect estimate. (Access and task results)

## language-r3-B2 / allocation — belangrijk — correct

Bron [L7](cases/language/source.md):

> Participants were recruited from different channels and neither assigned randomly nor matched for prior experience.

[content.md](calls/language-r3-B2-Research/content.md)

- L6: 5. The old English-only service completed 33 of 50 moderated sessions, with completion requiring a final submitted application. The multilingual form recorded 42 of 50 unmoderated sessions, counting reaching the review screen. Participants came from different recruitment channels and were neither randomly assigned nor matched for prior experience. The source says this is not a comparable completion-effect estimate. (Access and task results)
- L20: - A comparable completion effect for the multilingual form: the measures, recruitment, and participant comparability differ (F5).

[projection.md](calls/language-r3-B2-Research/projection.md)

- L7: 5. The old English-only service completed 33 of 50 moderated sessions, with completion requiring a final submitted application. The multilingual form recorded 42 of 50 unmoderated sessions, counting reaching the review screen. Participants came from different recruitment channels and were neither randomly assigned nor matched for prior experience. The source says this is not a comparable completion-effect estimate. (Access and task results)
- L19: - A comparable completion effect for the multilingual form: the measures, recruitment, and participant comparability differ (F5).

## language-r3-B2 / term — kern — correct

Bron [L9](cases/language/source.md):

> A preprocessing step called case folding normalised letter case; it did not merge benefit cases or applications.

[content.md](calls/language-r3-B2-Research/content.md)

- L7: 6. “Case folding” means normalising letter case; it did not merge benefit cases or applications. (Processing and reproducibility)

[projection.md](calls/language-r3-B2-Research/projection.md)

- L8: 6. “Case folding” means normalising letter case; it did not merge benefit cases or applications. (Processing and reproducibility)

## language-r3-B2 / variation — kern — correct

Bron [L9](cases/language/source.md):

> Three runs on the same 80 strings flagged six, five and six semantic issues with a sampling-based checker. A human reference review identified the six deadline reversals.

[content.md](calls/language-r3-B2-Research/content.md)

- L8: 7. On the same 80 strings, three checker runs flagged six, five, and six semantic issues. The checker version was fixed, but sample order varied. A human reference review identified the six deadline reversals. The automated counts do not establish three different ground truths or a stable checker. (Processing and reproducibility)
- L21: - Whether the checker’s varying counts reflect sampling variation or other causes; the source does not establish checker stability or separate ground truths (F7).

[projection.md](calls/language-r3-B2-Research/projection.md)

- L9: 7. On the same 80 strings, three checker runs flagged six, five, and six semantic issues. The checker version was fixed, but sample order varied. A human reference review identified the six deadline reversals. The automated counts do not establish three different ground truths or a stable checker. (Processing and reproducibility)
- L20: - Whether the checker’s varying counts reflect sampling variation or other causes; the source does not establish checker stability or separate ground truths (F7).

## language-r3-B2 / legal — kern — correct

Bron [L11](cases/language/source.md):

> The appendix says legal approval covers the Spanish consent paragraph only. It gives no approval date or temporal relationship to the sample review. Polish consent approval is not documented.

[content.md](calls/language-r3-B2-Research/content.md)

- L9: 8. Legal approval covers only the Spanish consent paragraph. No approval date or timing relative to the sample review is given; Polish consent approval is not documented. Translation availability alone does not establish legal approval. (Status and decision boundary)
- L22: - The date of Spanish consent approval and its timing relative to the sample review, as well as whether Polish consent received approval (F8).

[projection.md](calls/language-r3-B2-Research/projection.md)

- L10: 8. Legal approval covers only the Spanish consent paragraph. No approval date or timing relative to the sample review is given; Polish consent approval is not documented. Translation availability alone does not establish legal approval. (Status and decision boundary)
- L21: - The date of Spanish consent approval and its timing relative to the sample review, as well as whether Polish consent received approval (F8).

## language-r3-B2 / decision — kern — correct

Bron [L11](cases/language/source.md):

> The coordinator authorised a corrected-form usability study, conditional on a verified upload fix, and withheld general release.

[content.md](calls/language-r3-B2-Research/content.md)

- L10: 9. The coordinator authorised a corrected-form usability study conditional on a verified upload fix and withheld general release. (Status and decision boundary)
- L26: No dated development sequence is supplied. The source reports old-service and multilingual-form task results as comparison states (F5), and records a conditional authorization for a corrected-form study alongside withheld general release (F9); it does not date or order these decisions relative to the other checks.

[projection.md](calls/language-r3-B2-Research/projection.md)

- L11: 9. The coordinator authorised a corrected-form usability study conditional on a verified upload fix and withheld general release. (Status and decision boundary)

## heat-r3-B / ledger — kern — correct

Bron [L4](cases/heat/source.md):

> The final metered record shows 410 kWh of useful heat from 500 kWh electrical input over the trial boundary.

[content.md](calls/heat-r3-B-Research/content.md)

- L2: 1. The final metered record reports 410 kWh useful heat from 500 kWh electrical input for the trial boundary. A draft’s 450 kWh figure included 40 kWh duplicated during meter-file concatenation, which was removed. (“Performance and measurement”)
- L3: 2. The 410/500 ratio applies only to this trial boundary and period, not seasonal efficiency. The legacy boiler’s displayed 76% used fuel energy on a lower-heating-value basis; the electrical trial used meter input and included pump demand. No common-boundary reanalysis exists. (“not seasonal efficiency”; “No common-boundary reanalysis”)
- L8: 7. Three identical-input simulation runs estimated usable discharge at 402, 418 and 407 kWh. They were stochastic simulations, not additional metered field trials, and do not supersede the measured 410 kWh; solver variation limits attribution of simulation improvements. (“Variability, sequencing and decision”)
- L28: - Separately, the draft 450 kWh figure was corrected by removing 40 kWh of duplicated data, yielding the final 410 kWh metered record (Fact 1).

[projection.md](calls/heat-r3-B-Research/projection.md)

- L3: 1. The final metered record reports 410 kWh useful heat from 500 kWh electrical input for the trial boundary. A draft’s 450 kWh figure included 40 kWh duplicated during meter-file concatenation, which was removed. (“Performance and measurement”)
- L4: 2. The 410/500 ratio applies only to this trial boundary and period, not seasonal efficiency. The legacy boiler’s displayed 76% used fuel energy on a lower-heating-value basis; the electrical trial used meter input and included pump demand. No common-boundary reanalysis exists. (“not seasonal efficiency”; “No common-boundary reanalysis”)
- L9: 7. Three identical-input simulation runs estimated usable discharge at 402, 418 and 407 kWh. They were stochastic simulations, not additional metered field trials, and do not supersede the measured 410 kWh; solver variation limits attribution of simulation improvements. (“Variability, sequencing and decision”)

## heat-r3-B / method — kern — correct

Bron [L4](cases/heat/source.md):

> The legacy boiler's displayed 76% used fuel energy on a lower-heating-value basis, while the electrical trial used meter input and included pump demand. No common-boundary reanalysis exists.

[content.md](calls/heat-r3-B-Research/content.md)

- L3: 2. The 410/500 ratio applies only to this trial boundary and period, not seasonal efficiency. The legacy boiler’s displayed 76% used fuel energy on a lower-heating-value basis; the electrical trial used meter input and included pump demand. No common-boundary reanalysis exists. (“not seasonal efficiency”; “No common-boundary reanalysis”)
- L18: - Seasonal efficiency and a valid efficiency comparison with the legacy boiler are not established; the source provides no common-boundary reanalysis (Fact 2).

[projection.md](calls/heat-r3-B-Research/projection.md)

- L4: 2. The 410/500 ratio applies only to this trial boundary and period, not seasonal efficiency. The legacy boiler’s displayed 76% used fuel energy on a lower-heating-value basis; the electrical trial used meter input and included pump demand. No common-boundary reanalysis exists. (“not seasonal efficiency”; “No common-boundary reanalysis”)
- L16: - Seasonal efficiency and a valid efficiency comparison with the legacy boiler are not established; the source provides no common-boundary reanalysis (Fact 2).

## heat-r3-B / conditions — belangrijk — correct

Bron [L5](cases/heat/source.md):

> Trial shifts processed lighter products and ambient temperature was 8 degrees higher than in the boiler reference shifts.

[content.md](calls/heat-r3-B-Research/content.md)

- L4: 3. Trial shifts processed lighter products, had ambient temperatures 8 degrees higher than boiler-reference shifts, and were selected to avoid peak production. There was no random assignment or matched control; the record says it cannot isolate a storage-technology effect or quantify avoided emissions, and grid-carbon intensity was not measured. (“Performance and measurement”)

[projection.md](calls/heat-r3-B-Research/projection.md)

- L5: 3. Trial shifts processed lighter products, had ambient temperatures 8 degrees higher than boiler-reference shifts, and were selected to avoid peak production. There was no random assignment or matched control; the record says it cannot isolate a storage-technology effect or quantify avoided emissions, and grid-carbon intensity was not measured. (“Performance and measurement”)

## heat-r3-B / allocation — belangrijk — correct

Bron [L5](cases/heat/source.md):

> Operators selected the trial shifts to avoid peak production. There was no random assignment or matched control.

[content.md](calls/heat-r3-B-Research/content.md)

- L4: 3. Trial shifts processed lighter products, had ambient temperatures 8 degrees higher than boiler-reference shifts, and were selected to avoid peak production. There was no random assignment or matched control; the record says it cannot isolate a storage-technology effect or quantify avoided emissions, and grid-carbon intensity was not measured. (“Performance and measurement”)
- L19: - The extent to which storage technology caused the measured result, or how much energy or emissions it avoided, is not established because of the shift differences and absent matched control, random assignment and grid-carbon measurement (Fact 3).

[projection.md](calls/heat-r3-B-Research/projection.md)

- L5: 3. Trial shifts processed lighter products, had ambient temperatures 8 degrees higher than boiler-reference shifts, and were selected to avoid peak production. There was no random assignment or matched control; the record says it cannot isolate a storage-technology effect or quantify avoided emissions, and grid-carbon intensity was not measured. (“Performance and measurement”)
- L17: - The extent to which storage technology caused the measured result, or how much energy or emissions it avoided, is not established because of the shift differences and absent matched control, random assignment and grid-carbon measurement (Fact 3).

## heat-r3-B / contrast — kern — correct

Bron [L7](cases/heat/source.md):

> All four vessels passed the specified pressure-hold test. Two nevertheless failed the insulation surface-temperature check at the access hatch.

[content.md](calls/heat-r3-B-Research/content.md)

- L5: 4. All four vessels passed the specified pressure-hold test, but two failed the insulation surface-temperature check at the access hatch. Hot surfaces remained unresolved at sign-off, and a proposed shield was not installed. The source states that passing the pressure test does not demonstrate acceptable touch temperature. (“Successful and failed controls”)
- L10: 9. Procurement approved only a longer instrumented trial conditional on a verified hatch remedy; it approved neither purchase nor factory-wide roll-out. (“Variability, sequencing and decision”)
- L20: - Whether the hatch hot-surface issue was remedied after sign-off is not established; the proposed shield was not installed and the remedy is a condition of the longer trial (Facts 4, 9).
- L27: - At sign-off, two vessels still failed the hatch surface-temperature check and the proposed shield had not been installed (Fact 4). Procurement subsequently approved a longer instrumented trial conditional on a verified hatch remedy, not purchase or roll-out (Fact 9).

[projection.md](calls/heat-r3-B-Research/projection.md)

- L6: 4. All four vessels passed the specified pressure-hold test, but two failed the insulation surface-temperature check at the access hatch. Hot surfaces remained unresolved at sign-off, and a proposed shield was not installed. The source states that passing the pressure test does not demonstrate acceptable touch temperature. (“Successful and failed controls”)
- L11: 9. Procurement approved only a longer instrumented trial conditional on a verified hatch remedy; it approved neither purchase nor factory-wide roll-out. (“Variability, sequencing and decision”)
- L18: - Whether the hatch hot-surface issue was remedied after sign-off is not established; the proposed shield was not installed and the remedy is a condition of the longer trial (Facts 4, 9).

## heat-r3-B / open — kern — correct

Bron [L7](cases/heat/source.md):

> The hot surfaces remained unresolved at sign-off, and a proposed shield was not installed.

[content.md](calls/heat-r3-B-Research/content.md)

- L5: 4. All four vessels passed the specified pressure-hold test, but two failed the insulation surface-temperature check at the access hatch. Hot surfaces remained unresolved at sign-off, and a proposed shield was not installed. The source states that passing the pressure test does not demonstrate acceptable touch temperature. (“Successful and failed controls”)
- L14: 1. The record shows a positive measured heat-output result alongside unresolved control and comparison limits (Facts 1–6); it does not establish broad system quality or comparative efficiency.
- L20: - Whether the hatch hot-surface issue was remedied after sign-off is not established; the proposed shield was not installed and the remedy is a condition of the longer trial (Facts 4, 9).
- L27: - At sign-off, two vessels still failed the hatch surface-temperature check and the proposed shield had not been installed (Fact 4). Procurement subsequently approved a longer instrumented trial conditional on a verified hatch remedy, not purchase or roll-out (Fact 9).

[projection.md](calls/heat-r3-B-Research/projection.md)

- L6: 4. All four vessels passed the specified pressure-hold test, but two failed the insulation surface-temperature check at the access hatch. Hot surfaces remained unresolved at sign-off, and a proposed shield was not installed. The source states that passing the pressure test does not demonstrate acceptable touch temperature. (“Successful and failed controls”)
- L18: - Whether the hatch hot-surface issue was remedied after sign-off is not established; the proposed shield was not installed and the remedy is a condition of the longer trial (Facts 4, 9).

## heat-r3-B / interlock — belangrijk — correct

Bron [L7](cases/heat/source.md):

> An interlock test passed for all four vessels; it did not test the hot-surface problem.

[content.md](calls/heat-r3-B-Research/content.md)

- L6: 5. The interlock test passed on all four vessels but did not test the hot-surface problem. (“Successful and failed controls”)

[projection.md](calls/heat-r3-B-Research/projection.md)

- L7: 5. The interlock test passed on all four vessels but did not test the hot-surface problem. (“Successful and failed controls”)

## heat-r3-B / term — kern — correct

Bron [L8](cases/heat/source.md):

> Three thermal trips occurred during initial charging. Here a thermal trip is a protective over-temperature shutdown, not a production journey or heat-delivery cycle.

[content.md](calls/heat-r3-B-Research/content.md)

- L7: 6. Three thermal trips—defined as protective over-temperature shutdowns, not production journeys or heat-delivery cycles—occurred during initial charging. Controller setting and sensor mounting were changed together before a repeat charge with zero trips; the record cannot separate their contributions, and one zero-trip repeat does not establish long-term reliability. (“Successful and failed controls”)
- L26: - Initial charging produced three protective over-temperature shutdowns; the controller setting and sensor mounting were then changed together, followed by one repeat charge with zero trips (Fact 6).

[projection.md](calls/heat-r3-B-Research/projection.md)

- L8: 6. Three thermal trips—defined as protective over-temperature shutdowns, not production journeys or heat-delivery cycles—occurred during initial charging. Controller setting and sensor mounting were changed together before a repeat charge with zero trips; the record cannot separate their contributions, and one zero-trip repeat does not establish long-term reliability. (“Successful and failed controls”)

## heat-r3-B / attribution — kern — correct

Bron [L8](cases/heat/source.md):

> A controller setting and sensor mounting were changed together before a repeat charge with zero trips. The record cannot separate the two actions' contributions.

[content.md](calls/heat-r3-B-Research/content.md)

- L7: 6. Three thermal trips—defined as protective over-temperature shutdowns, not production journeys or heat-delivery cycles—occurred during initial charging. Controller setting and sensor mounting were changed together before a repeat charge with zero trips; the record cannot separate their contributions, and one zero-trip repeat does not establish long-term reliability. (“Successful and failed controls”)
- L21: - The separate contributions of the controller change and sensor-mounting change to the zero-trip repeat, and longer-term trip reliability, are not established (Fact 6).
- L26: - Initial charging produced three protective over-temperature shutdowns; the controller setting and sensor mounting were then changed together, followed by one repeat charge with zero trips (Fact 6).

[projection.md](calls/heat-r3-B-Research/projection.md)

- L8: 6. Three thermal trips—defined as protective over-temperature shutdowns, not production journeys or heat-delivery cycles—occurred during initial charging. Controller setting and sensor mounting were changed together before a repeat charge with zero trips; the record cannot separate their contributions, and one zero-trip repeat does not establish long-term reliability. (“Successful and failed controls”)
- L19: - The separate contributions of the controller change and sensor-mounting change to the zero-trip repeat, and longer-term trip reliability, are not established (Fact 6).

## heat-r3-B / variation — kern — correct

Bron [L10](cases/heat/source.md):

> Three identical-input simulation runs estimated usable discharge at 402, 418 and 407 kWh; the solver used stochastic initialisation.

[content.md](calls/heat-r3-B-Research/content.md)

- L8: 7. Three identical-input simulation runs estimated usable discharge at 402, 418 and 407 kWh. They were stochastic simulations, not additional metered field trials, and do not supersede the measured 410 kWh; solver variation limits attribution of simulation improvements. (“Variability, sequencing and decision”)
- L22: - The simulations do not establish additional field performance or an improvement attributable to a change (Fact 7).

[projection.md](calls/heat-r3-B-Research/projection.md)

- L9: 7. Three identical-input simulation runs estimated usable discharge at 402, 418 and 407 kWh. They were stochastic simulations, not additional metered field trials, and do not supersede the measured 410 kWh; solver variation limits attribution of simulation improvements. (“Variability, sequencing and decision”)
- L20: - The simulations do not establish additional field performance or an improvement attributable to a change (Fact 7).

## heat-r3-B / cost — belangrijk — correct

Bron [L11](cases/heat/source.md):

> The finance appendix reports a EUR 9,200 rental allowance. It gives no date or sequence relative to testing. The allowance excludes installation, downtime and maintenance, so no payback claim is supported.

[content.md](calls/heat-r3-B-Research/content.md)

- L9: 8. The finance appendix reports a EUR 9,200 rental allowance but gives no date or sequence relative to testing. It excludes installation, downtime and maintenance, so the source supports no payback claim. (“The finance appendix”; “no payback claim is supported”)
- L23: - Payback, purchase, factory-wide deployment, and a measured low-carbon outcome are not established (Facts 8–10).

[projection.md](calls/heat-r3-B-Research/projection.md)

- L10: 8. The finance appendix reports a EUR 9,200 rental allowance but gives no date or sequence relative to testing. It excludes installation, downtime and maintenance, so the source supports no payback claim. (“The finance appendix”; “no payback claim is supported”)
- L21: - Payback, purchase, factory-wide deployment, and a measured low-carbon outcome are not established (Facts 8–10).

## heat-r3-B / decision — kern — correct

Bron [L11](cases/heat/source.md):

> Procurement approved only a longer instrumented trial conditional on a verified hatch remedy; no purchase or factory-wide roll-out was approved.

[content.md](calls/heat-r3-B-Research/content.md)

- L10: 9. Procurement approved only a longer instrumented trial conditional on a verified hatch remedy; it approved neither purchase nor factory-wide roll-out. (“Variability, sequencing and decision”)
- L15: 2. The procurement decision is conditional further testing, not deployment approval (Fact 9).
- L23: - Payback, purchase, factory-wide deployment, and a measured low-carbon outcome are not established (Facts 8–10).
- L27: - At sign-off, two vessels still failed the hatch surface-temperature check and the proposed shield had not been installed (Fact 4). Procurement subsequently approved a longer instrumented trial conditional on a verified hatch remedy, not purchase or roll-out (Fact 9).

[projection.md](calls/heat-r3-B-Research/projection.md)

- L11: 9. Procurement approved only a longer instrumented trial conditional on a verified hatch remedy; it approved neither purchase nor factory-wide roll-out. (“Variability, sequencing and decision”)
- L21: - Payback, purchase, factory-wide deployment, and a measured low-carbon outcome are not established (Facts 8–10).

## heat-r3-B2 / ledger — kern — correct

Bron [L4](cases/heat/source.md):

> The final metered record shows 410 kWh of useful heat from 500 kWh electrical input over the trial boundary.

[content.md](calls/heat-r3-B2-Research/content.md)

- L2: 1. The final metered record reports 410 kWh useful heat from 500 kWh electrical input over the trial boundary. A draft’s 450 kWh was corrected because 40 kWh had been duplicated during meter-file concatenation. The ratio is for this period and boundary, not seasonal efficiency. (Performance and measurement)
- L9: 8. Three identical-input simulation runs estimated usable discharge at 402, 418 and 407 kWh; the solver used stochastic initialisation. These were not additional metered field trials and do not supersede the measured 410 kWh. The source identifies solver variation as an attribution limit for interpreting simulation improvements. (Variability, sequencing and decision)

[projection.md](calls/heat-r3-B2-Research/projection.md)

- L3: 1. The final metered record reports 410 kWh useful heat from 500 kWh electrical input over the trial boundary. A draft’s 450 kWh was corrected because 40 kWh had been duplicated during meter-file concatenation. The ratio is for this period and boundary, not seasonal efficiency. (Performance and measurement)
- L10: 8. Three identical-input simulation runs estimated usable discharge at 402, 418 and 407 kWh; the solver used stochastic initialisation. These were not additional metered field trials and do not supersede the measured 410 kWh. The source identifies solver variation as an attribution limit for interpreting simulation improvements. (Variability, sequencing and decision)

## heat-r3-B2 / method — kern — correct

Bron [L4](cases/heat/source.md):

> The legacy boiler's displayed 76% used fuel energy on a lower-heating-value basis, while the electrical trial used meter input and included pump demand. No common-boundary reanalysis exists.

[content.md](calls/heat-r3-B2-Research/content.md)

- L3: 2. The legacy boiler’s displayed 76% used fuel energy on a lower-heating-value basis; the electrical trial used meter input and included pump demand. The source says no common-boundary reanalysis exists. (Performance and measurement)
- L21: - Seasonal performance and a valid like-for-like efficiency comparison remain unestablished: the source provides no seasonal model or common-boundary reanalysis. (Facts 1–3)

[projection.md](calls/heat-r3-B2-Research/projection.md)

- L4: 2. The legacy boiler’s displayed 76% used fuel energy on a lower-heating-value basis; the electrical trial used meter input and included pump demand. The source says no common-boundary reanalysis exists. (Performance and measurement)
- L19: - Seasonal performance and a valid like-for-like efficiency comparison remain unestablished: the source provides no seasonal model or common-boundary reanalysis. (Facts 1–3)

## heat-r3-B2 / conditions — belangrijk — correct

Bron [L5](cases/heat/source.md):

> Trial shifts processed lighter products and ambient temperature was 8 degrees higher than in the boiler reference shifts.

[content.md](calls/heat-r3-B2-Research/content.md)

- L4: 3. Trial shifts processed lighter products, had ambient temperature 8 degrees higher than the boiler-reference shifts, and were selected to avoid peak production. There was no random assignment or matched control; the record cannot isolate a storage-technology effect or quantify avoided emissions. Grid-carbon intensity was not measured. (Performance and measurement)

[projection.md](calls/heat-r3-B2-Research/projection.md)

- L5: 3. Trial shifts processed lighter products, had ambient temperature 8 degrees higher than the boiler-reference shifts, and were selected to avoid peak production. There was no random assignment or matched control; the record cannot isolate a storage-technology effect or quantify avoided emissions. Grid-carbon intensity was not measured. (Performance and measurement)

## heat-r3-B2 / allocation — belangrijk — correct

Bron [L5](cases/heat/source.md):

> Operators selected the trial shifts to avoid peak production. There was no random assignment or matched control.

[content.md](calls/heat-r3-B2-Research/content.md)

- L4: 3. Trial shifts processed lighter products, had ambient temperature 8 degrees higher than the boiler-reference shifts, and were selected to avoid peak production. There was no random assignment or matched control; the record cannot isolate a storage-technology effect or quantify avoided emissions. Grid-carbon intensity was not measured. (Performance and measurement)

[projection.md](calls/heat-r3-B2-Research/projection.md)

- L5: 3. Trial shifts processed lighter products, had ambient temperature 8 degrees higher than the boiler-reference shifts, and were selected to avoid peak production. There was no random assignment or matched control; the record cannot isolate a storage-technology effect or quantify avoided emissions. Grid-carbon intensity was not measured. (Performance and measurement)

## heat-r3-B2 / contrast — kern — correct

Bron [L7](cases/heat/source.md):

> All four vessels passed the specified pressure-hold test. Two nevertheless failed the insulation surface-temperature check at the access hatch.

[content.md](calls/heat-r3-B2-Research/content.md)

- L5: 4. All four vessels passed the specified pressure-hold test. Two failed the insulation surface-temperature check at the access hatch; hot surfaces remained unresolved at sign-off, and a proposed shield was not installed. The source explicitly says pressure-test success does not demonstrate acceptable touch temperature. (Successful and failed controls)
- L11: 10. Procurement approved only a longer instrumented trial, conditional on a verified hatch remedy. No purchase or factory-wide roll-out was approved. (Variability, sequencing and decision)
- L18: 2. Passing pressure-hold and interlock checks does not resolve the separate access-hatch surface-temperature failures. (Facts 4–5)
- L23: - The source does not report a completed hatch remedy or acceptable touch temperatures at sign-off; the proposed shield was not installed. (Fact 4)
- L32: - At sign-off, the access-hatch surface-temperature failures remained unresolved and the proposed shield had not been installed. Procurement’s approved next step was a longer instrumented trial conditional on a verified hatch remedy; purchase and factory-wide roll-out were not approved. (Facts 4, 10)

[projection.md](calls/heat-r3-B2-Research/projection.md)

- L6: 4. All four vessels passed the specified pressure-hold test. Two failed the insulation surface-temperature check at the access hatch; hot surfaces remained unresolved at sign-off, and a proposed shield was not installed. The source explicitly says pressure-test success does not demonstrate acceptable touch temperature. (Successful and failed controls)
- L12: 10. Procurement approved only a longer instrumented trial, conditional on a verified hatch remedy. No purchase or factory-wide roll-out was approved. (Variability, sequencing and decision)
- L21: - The source does not report a completed hatch remedy or acceptable touch temperatures at sign-off; the proposed shield was not installed. (Fact 4)

## heat-r3-B2 / open — kern — correct

Bron [L7](cases/heat/source.md):

> The hot surfaces remained unresolved at sign-off, and a proposed shield was not installed.

[content.md](calls/heat-r3-B2-Research/content.md)

- L5: 4. All four vessels passed the specified pressure-hold test. Two failed the insulation surface-temperature check at the access hatch; hot surfaces remained unresolved at sign-off, and a proposed shield was not installed. The source explicitly says pressure-test success does not demonstrate acceptable touch temperature. (Successful and failed controls)
- L23: - The source does not report a completed hatch remedy or acceptable touch temperatures at sign-off; the proposed shield was not installed. (Fact 4)
- L32: - At sign-off, the access-hatch surface-temperature failures remained unresolved and the proposed shield had not been installed. Procurement’s approved next step was a longer instrumented trial conditional on a verified hatch remedy; purchase and factory-wide roll-out were not approved. (Facts 4, 10)

[projection.md](calls/heat-r3-B2-Research/projection.md)

- L6: 4. All four vessels passed the specified pressure-hold test. Two failed the insulation surface-temperature check at the access hatch; hot surfaces remained unresolved at sign-off, and a proposed shield was not installed. The source explicitly says pressure-test success does not demonstrate acceptable touch temperature. (Successful and failed controls)
- L21: - The source does not report a completed hatch remedy or acceptable touch temperatures at sign-off; the proposed shield was not installed. (Fact 4)

## heat-r3-B2 / interlock — belangrijk — correct

Bron [L7](cases/heat/source.md):

> An interlock test passed for all four vessels; it did not test the hot-surface problem.

[content.md](calls/heat-r3-B2-Research/content.md)

- L6: 5. The interlock test passed for all four vessels, but did not test the hot-surface problem. (Successful and failed controls)
- L18: 2. Passing pressure-hold and interlock checks does not resolve the separate access-hatch surface-temperature failures. (Facts 4–5)

[projection.md](calls/heat-r3-B2-Research/projection.md)

- L7: 5. The interlock test passed for all four vessels, but did not test the hot-surface problem. (Successful and failed controls)

## heat-r3-B2 / term — kern — correct

Bron [L8](cases/heat/source.md):

> Three thermal trips occurred during initial charging. Here a thermal trip is a protective over-temperature shutdown, not a production journey or heat-delivery cycle.

[content.md](calls/heat-r3-B2-Research/content.md)

- L7: 6. Three thermal trips occurred during initial charging. The source defines a thermal trip as a protective over-temperature shutdown, not a production journey or heat-delivery cycle. (Successful and failed controls)
- L31: - Three protective over-temperature shutdowns occurred during initial charging; a controller setting and sensor mounting were then changed together before a repeat charge with zero trips. The source does not isolate their contributions. (Facts 6–7)

[projection.md](calls/heat-r3-B2-Research/projection.md)

- L8: 6. Three thermal trips occurred during initial charging. The source defines a thermal trip as a protective over-temperature shutdown, not a production journey or heat-delivery cycle. (Successful and failed controls)

## heat-r3-B2 / attribution — kern — correct

Bron [L8](cases/heat/source.md):

> A controller setting and sensor mounting were changed together before a repeat charge with zero trips. The record cannot separate the two actions' contributions.

[content.md](calls/heat-r3-B2-Research/content.md)

- L8: 7. A controller setting and sensor mounting were changed together before a repeat charge, which had zero trips. The record cannot separate the actions’ contributions, and says zero trips in one repeat does not establish long-term reliability. (Successful and failed controls)
- L24: - The individual contributions of the controller-setting and sensor-mounting changes, and long-term trip reliability, are not established. (Fact 7)
- L31: - Three protective over-temperature shutdowns occurred during initial charging; a controller setting and sensor mounting were then changed together before a repeat charge with zero trips. The source does not isolate their contributions. (Facts 6–7)

[projection.md](calls/heat-r3-B2-Research/projection.md)

- L9: 7. A controller setting and sensor mounting were changed together before a repeat charge, which had zero trips. The record cannot separate the actions’ contributions, and says zero trips in one repeat does not establish long-term reliability. (Successful and failed controls)
- L22: - The individual contributions of the controller-setting and sensor-mounting changes, and long-term trip reliability, are not established. (Fact 7)

## heat-r3-B2 / variation — kern — correct

Bron [L10](cases/heat/source.md):

> Three identical-input simulation runs estimated usable discharge at 402, 418 and 407 kWh; the solver used stochastic initialisation.

[content.md](calls/heat-r3-B2-Research/content.md)

- L9: 8. Three identical-input simulation runs estimated usable discharge at 402, 418 and 407 kWh; the solver used stochastic initialisation. These were not additional metered field trials and do not supersede the measured 410 kWh. The source identifies solver variation as an attribution limit for interpreting simulation improvements. (Variability, sequencing and decision)
- L25: - The simulations do not establish additional field-trial performance; their run-to-run differences cannot be attributed to a technology improvement from this record. (Fact 8)

[projection.md](calls/heat-r3-B2-Research/projection.md)

- L10: 8. Three identical-input simulation runs estimated usable discharge at 402, 418 and 407 kWh; the solver used stochastic initialisation. These were not additional metered field trials and do not supersede the measured 410 kWh. The source identifies solver variation as an attribution limit for interpreting simulation improvements. (Variability, sequencing and decision)
- L23: - The simulations do not establish additional field-trial performance; their run-to-run differences cannot be attributed to a technology improvement from this record. (Fact 8)

## heat-r3-B2 / cost — belangrijk — correct

Bron [L11](cases/heat/source.md):

> The finance appendix reports a EUR 9,200 rental allowance. It gives no date or sequence relative to testing. The allowance excludes installation, downtime and maintenance, so no payback claim is supported.

[content.md](calls/heat-r3-B2-Research/content.md)

- L10: 9. The finance appendix reports a EUR 9,200 rental allowance, but gives no date or sequence relative to testing. It excludes installation, downtime and maintenance; the source says no payback claim is supported. (Variability, sequencing and decision)
- L26: - The rental allowance does not establish payback, and its timing relative to testing is unspecified. (Fact 9)

[projection.md](calls/heat-r3-B2-Research/projection.md)

- L11: 9. The finance appendix reports a EUR 9,200 rental allowance, but gives no date or sequence relative to testing. It excludes installation, downtime and maintenance; the source says no payback claim is supported. (Variability, sequencing and decision)
- L24: - The rental allowance does not establish payback, and its timing relative to testing is unspecified. (Fact 9)

## heat-r3-B2 / decision — kern — correct

Bron [L11](cases/heat/source.md):

> Procurement approved only a longer instrumented trial conditional on a verified hatch remedy; no purchase or factory-wide roll-out was approved.

[content.md](calls/heat-r3-B2-Research/content.md)

- L11: 10. Procurement approved only a longer instrumented trial, conditional on a verified hatch remedy. No purchase or factory-wide roll-out was approved. (Variability, sequencing and decision)
- L32: - At sign-off, the access-hatch surface-temperature failures remained unresolved and the proposed shield had not been installed. Procurement’s approved next step was a longer instrumented trial conditional on a verified hatch remedy; purchase and factory-wide roll-out were not approved. (Facts 4, 10)

[projection.md](calls/heat-r3-B2-Research/projection.md)

- L12: 10. Procurement approved only a longer instrumented trial, conditional on a verified hatch remedy. No purchase or factory-wide roll-out was approved. (Variability, sequencing and decision)

## heat-r3-A / ledger — kern — correct

Bron [L4](cases/heat/source.md):

> The final metered record shows 410 kWh of useful heat from 500 kWh electrical input over the trial boundary.

[content.md](calls/heat-r3-A-Research/content.md)

- L3: 1. The final metered record reports 410 kWh of useful heat from 500 kWh of electrical input over the trial boundary.
- L7: 5. The 410/500 ratio concerns the trial boundary and period, not seasonal efficiency.
- L35: 33. The simulations do not supersede the measured 410 kWh.
- L47: - The source does not establish seasonal efficiency from the reported 410/500 trial-boundary ratio.
- L59: - A draft listed 450 kWh of useful heat; 40 kWh was identified as duplicated during meter-file concatenation and removed. The final metered record reports 410 kWh useful heat from 500 kWh electrical input over the trial boundary.

[projection.md](calls/heat-r3-A-Research/projection.md)

- L3: 1. The final metered record reports 410 kWh of useful heat from 500 kWh of electrical input over the trial boundary.
- L7: 5. The 410/500 ratio concerns the trial boundary and period, not seasonal efficiency.
- L35: 33. The simulations do not supersede the measured 410 kWh.
- L43: - The source does not establish seasonal efficiency from the reported 410/500 trial-boundary ratio.

## heat-r3-A / method — kern — correct

Bron [L4](cases/heat/source.md):

> The legacy boiler's displayed 76% used fuel energy on a lower-heating-value basis, while the electrical trial used meter input and included pump demand. No common-boundary reanalysis exists.

[content.md](calls/heat-r3-A-Research/content.md)

- L8: 6. The legacy boiler’s displayed 76% used fuel energy on a lower-heating-value basis.
- L9: 7. The electrical trial used meter input and included pump demand.
- L10: 8. No common-boundary reanalysis exists.
- L48: - The source does not establish a common-boundary performance comparison between the thermal store and the legacy boiler; it states that no common-boundary reanalysis exists.

[projection.md](calls/heat-r3-A-Research/projection.md)

- L8: 6. The legacy boiler’s displayed 76% used fuel energy on a lower-heating-value basis.
- L9: 7. The electrical trial used meter input and included pump demand.
- L10: 8. No common-boundary reanalysis exists.
- L44: - The source does not establish a common-boundary performance comparison between the thermal store and the legacy boiler; it states that no common-boundary reanalysis exists.

## heat-r3-A / conditions — belangrijk — correct

Bron [L5](cases/heat/source.md):

> Trial shifts processed lighter products and ambient temperature was 8 degrees higher than in the boiler reference shifts.

[content.md](calls/heat-r3-A-Research/content.md)

- L11: 9. Trial shifts processed lighter products than the boiler reference shifts.
- L12: 10. Ambient temperature was 8 degrees higher in the trial shifts than in the boiler reference shifts.

[projection.md](calls/heat-r3-A-Research/projection.md)

- L11: 9. Trial shifts processed lighter products than the boiler reference shifts.
- L12: 10. Ambient temperature was 8 degrees higher in the trial shifts than in the boiler reference shifts.

## heat-r3-A / allocation — belangrijk — correct

Bron [L5](cases/heat/source.md):

> Operators selected the trial shifts to avoid peak production. There was no random assignment or matched control.

[content.md](calls/heat-r3-A-Research/content.md)

- L13: 11. Operators selected the trial shifts to avoid peak production.
- L14: 12. There was no random assignment.
- L15: 13. There was no matched control.

[projection.md](calls/heat-r3-A-Research/projection.md)

- L13: 11. Operators selected the trial shifts to avoid peak production.
- L14: 12. There was no random assignment.
- L15: 13. There was no matched control.

## heat-r3-A / contrast — kern — correct

Bron [L7](cases/heat/source.md):

> All four vessels passed the specified pressure-hold test. Two nevertheless failed the insulation surface-temperature check at the access hatch.

[content.md](calls/heat-r3-A-Research/content.md)

- L19: 17. All four vessels passed the specified pressure-hold test.
- L20: 18. Two vessels failed the insulation surface-temperature check at the access hatch.
- L37: 35. Procurement approved only a longer instrumented trial conditional on a verified hatch remedy.
- L54: - The source does not establish why procurement selected the longer instrumented trial beyond stating its condition: a verified hatch remedy.
- L55: - The source does not establish a relationship between the trial’s pressure-hold and interlock passes and the failed insulation surface-temperature checks; it explicitly says the pressure test does not demonstrate acceptable touch temperature and the interlock test did not test the hot-surface problem.
- L60: - All four vessels passed the specified pressure-hold test. Two failed the insulation surface-temperature check at the access hatch; the hot surfaces remained unresolved at sign-off, and a proposed shield was not installed.
- L63: - Procurement approved only a longer instrumented trial conditional on a verified hatch remedy. The source does not establish its timing relative to the metered results, controls, or simulations.

[projection.md](calls/heat-r3-A-Research/projection.md)

- L19: 17. All four vessels passed the specified pressure-hold test.
- L20: 18. Two vessels failed the insulation surface-temperature check at the access hatch.
- L37: 35. Procurement approved only a longer instrumented trial conditional on a verified hatch remedy.
- L50: - The source does not establish why procurement selected the longer instrumented trial beyond stating its condition: a verified hatch remedy.
- L51: - The source does not establish a relationship between the trial’s pressure-hold and interlock passes and the failed insulation surface-temperature checks; it explicitly says the pressure test does not demonstrate acceptable touch temperature and the interlock test did not test the hot-surface problem.

## heat-r3-A / open — kern — correct

Bron [L7](cases/heat/source.md):

> The hot surfaces remained unresolved at sign-off, and a proposed shield was not installed.

[content.md](calls/heat-r3-A-Research/content.md)

- L21: 19. The hot surfaces remained unresolved at sign-off.
- L22: 20. A proposed shield was not installed.
- L60: - All four vessels passed the specified pressure-hold test. Two failed the insulation surface-temperature check at the access hatch; the hot surfaces remained unresolved at sign-off, and a proposed shield was not installed.

[projection.md](calls/heat-r3-A-Research/projection.md)

- L21: 19. The hot surfaces remained unresolved at sign-off.
- L22: 20. A proposed shield was not installed.

## heat-r3-A / interlock — belangrijk — correct

Bron [L7](cases/heat/source.md):

> An interlock test passed for all four vessels; it did not test the hot-surface problem.

[content.md](calls/heat-r3-A-Research/content.md)

- L24: 22. The interlock test passed for all four vessels.
- L25: 23. The interlock test did not test the hot-surface problem.
- L55: - The source does not establish a relationship between the trial’s pressure-hold and interlock passes and the failed insulation surface-temperature checks; it explicitly says the pressure test does not demonstrate acceptable touch temperature and the interlock test did not test the hot-surface problem.

[projection.md](calls/heat-r3-A-Research/projection.md)

- L24: 22. The interlock test passed for all four vessels.
- L25: 23. The interlock test did not test the hot-surface problem.
- L51: - The source does not establish a relationship between the trial’s pressure-hold and interlock passes and the failed insulation surface-temperature checks; it explicitly says the pressure test does not demonstrate acceptable touch temperature and the interlock test did not test the hot-surface problem.

## heat-r3-A / term — kern — correct

Bron [L8](cases/heat/source.md):

> Three thermal trips occurred during initial charging. Here a thermal trip is a protective over-temperature shutdown, not a production journey or heat-delivery cycle.

[content.md](calls/heat-r3-A-Research/content.md)

- L26: 24. Three thermal trips occurred during initial charging.
- L27: 25. The source defines a thermal trip as a protective over-temperature shutdown.
- L61: - Three thermal trips occurred during initial charging. A controller setting and sensor mounting were changed together before a repeat charge, which had zero trips. The source cannot separate the changes’ contributions.
- L70: - Facts 24–29 concern thermal trips, the repeat charge, and attribution and reliability limits.

[projection.md](calls/heat-r3-A-Research/projection.md)

- L26: 24. Three thermal trips occurred during initial charging.
- L27: 25. The source defines a thermal trip as a protective over-temperature shutdown.

## heat-r3-A / attribution — kern — correct

Bron [L8](cases/heat/source.md):

> A controller setting and sensor mounting were changed together before a repeat charge with zero trips. The record cannot separate the two actions' contributions.

[content.md](calls/heat-r3-A-Research/content.md)

- L28: 26. Before a repeat charge, a controller setting and sensor mounting were changed together.
- L29: 27. The repeat charge had zero trips.
- L30: 28. The record cannot separate the contributions of the controller-setting change and sensor-mounting change.
- L31: 29. Zero trips in one repeat does not establish long-term reliability.
- L51: - The source does not establish the separate contributions, if any, of the controller-setting change and sensor-mounting change to the zero-trip repeat charge.
- L52: - The source does not establish long-term reliability from the one repeat charge with zero trips.
- L61: - Three thermal trips occurred during initial charging. A controller setting and sensor mounting were changed together before a repeat charge, which had zero trips. The source cannot separate the changes’ contributions.

[projection.md](calls/heat-r3-A-Research/projection.md)

- L28: 26. Before a repeat charge, a controller setting and sensor mounting were changed together.
- L29: 27. The repeat charge had zero trips.
- L30: 28. The record cannot separate the contributions of the controller-setting change and sensor-mounting change.
- L31: 29. Zero trips in one repeat does not establish long-term reliability.
- L47: - The source does not establish the separate contributions, if any, of the controller-setting change and sensor-mounting change to the zero-trip repeat charge.
- L48: - The source does not establish long-term reliability from the one repeat charge with zero trips.

## heat-r3-A / variation — kern — correct

Bron [L10](cases/heat/source.md):

> Three identical-input simulation runs estimated usable discharge at 402, 418 and 407 kWh; the solver used stochastic initialisation.

[content.md](calls/heat-r3-A-Research/content.md)

- L32: 30. Three identical-input simulation runs estimated usable discharge at 402, 418, and 407 kWh.
- L33: 31. The solver used stochastic initialisation.
- L34: 32. The simulations are not three additional metered field trials.
- L35: 33. The simulations do not supersede the measured 410 kWh.
- L36: 34. Solver variation is an attribution limit for interpreting simulation improvements.
- L53: - The source does not establish the relationship between the three simulated discharge estimates and any change in simulation performance; it identifies solver variation as an attribution limit.
- L62: - Three identical-input simulation runs estimated usable discharge at 402, 418, and 407 kWh. The source does not establish a temporal or causal relationship between these simulations and the metered trial result.
- L63: - Procurement approved only a longer instrumented trial conditional on a verified hatch remedy. The source does not establish its timing relative to the metered results, controls, or simulations.

[projection.md](calls/heat-r3-A-Research/projection.md)

- L32: 30. Three identical-input simulation runs estimated usable discharge at 402, 418, and 407 kWh.
- L33: 31. The solver used stochastic initialisation.
- L34: 32. The simulations are not three additional metered field trials.
- L35: 33. The simulations do not supersede the measured 410 kWh.
- L36: 34. Solver variation is an attribution limit for interpreting simulation improvements.
- L49: - The source does not establish the relationship between the three simulated discharge estimates and any change in simulation performance; it identifies solver variation as an attribution limit.

## heat-r3-A / cost — belangrijk — ontbreekt

Bron [L11](cases/heat/source.md):

> The finance appendix reports a EUR 9,200 rental allowance. It gives no date or sequence relative to testing. The allowance excludes installation, downtime and maintenance, so no payback claim is supported.

[content.md](calls/heat-r3-A-Research/content.md)

Ontbreekt; geen equivalent bewijs gevonden in de volledige secties.

[projection.md](calls/heat-r3-A-Research/projection.md)

Ontbreekt; geen equivalent bewijs gevonden in de volledige secties.

## heat-r3-A / decision — kern — correct

Bron [L11](cases/heat/source.md):

> Procurement approved only a longer instrumented trial conditional on a verified hatch remedy; no purchase or factory-wide roll-out was approved.

[content.md](calls/heat-r3-A-Research/content.md)

- L37: 35. Procurement approved only a longer instrumented trial conditional on a verified hatch remedy.
- L38: 36. No purchase was approved.
- L39: 37. No factory-wide roll-out was approved.
- L54: - The source does not establish why procurement selected the longer instrumented trial beyond stating its condition: a verified hatch remedy.
- L63: - Procurement approved only a longer instrumented trial conditional on a verified hatch remedy. The source does not establish its timing relative to the metered results, controls, or simulations.
- L72: - Facts 35–37 concern the procurement decision and its limits.

[projection.md](calls/heat-r3-A-Research/projection.md)

- L37: 35. Procurement approved only a longer instrumented trial conditional on a verified hatch remedy.
- L38: 36. No purchase was approved.
- L39: 37. No factory-wide roll-out was approved.
- L50: - The source does not establish why procurement selected the longer instrumented trial beyond stating its condition: a verified hatch remedy.
