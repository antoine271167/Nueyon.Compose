# Experiment 19 — Narrative Semantic Validation: Real-World Acceptance

**NO-GO for production integration. Recommendation C: Do not approve Narrative yet.** The observed configuration exceeded the 10% false-positive flag limit and fell below the 90% exact repeat agreement threshold. The natural corpus gives **insufficient evidence for detection quality**, with no naturally occurring material errors. The supplementary regressions reproduced two systematic misses.

## Primary results

Twelve independent subjects produced 36 complete Narrative briefs in 36 attempts using the production CLI and GPT-6 Luna. The frozen, source-grounded review found 35 F0, 1 F1 and 0 F2. All 72 planned validator attempts completed. The validator prompt matched Experiment 18 byte for byte. The scoring tables count attempts and preserve the relationship between repetitions of the same brief.

| Measure | Result |
| --- | ---: |
| PASS | 63/72 (87.5%) |
| REVIEW | 9/72 (12.5%) across 7/36 briefs |
| FAIL | 0/72 (0%) |
| Invalid, missing or transport failure | 0/72 (0%) |
| Acceptable FAIL rate | 0/72 (0%) |
| Acceptable flag rate | 9/72 (12.5%) |
| Triage specificity | 63/72 (87.5%) |
| Definitive specificity | 72/72 (100%) |
| Exact repeat agreement | 31/36 (86.1%) |
| Definitive precision and recall | Undefined; zero FAILs and zero F2 |
| Triage sensitivity and silent-escape rate | Undefined; zero F2. No opportunity to observe an escape. |
| Actionable material evidence quality | Undefined; zero material findings |
| Validator latency | Mean 8.91 s, median 8.02 s, p95 13.12 s, range 6.09–18.94 s |

Definitive TP/FP/TN/FN were 0/0/72/0. Triage TP/FP/TN/FN were 0/9/63/0. Seven briefs would have been sent to review in at least one repetition. These are sample workload observations. The two attempts per output are repeated measures.

## Evidence and validity

The study used manually prepared upstream fixtures and a source document summary for each validator input. The summaries were faithful to their selected details but omitted some context relevant to the resulting briefs. Eight of the nine review attempts concerned claims supported by material in the linked original pages beyond the frozen summaries. One read Fusion-1 as making a precise timing claim where its context preserves an ordinary description of the target-energy comparison. The gross flag rate represents workload with the frozen packets, with limited evidence about workload when complete original sources are supplied. The same AI operator prepared the cases, classified outputs before validation and adjudicated findings afterward. No second reviewer was available. The sample was English-language and the manually prepared upstream inputs may be cleaner than generated Synthesis. No F2 output was manufactured. [Source coverage](source-coverage.md)

## Supplementary archived regressions

Five previously adjudicated F2 cases produced 10/10 valid results. Triage sensitivity was 5/10 (50%), definitive recall 3/10 (30%) and definitive precision 3/3 (100%). The all-positive design cannot test discrimination against acceptable briefs. Five of ten attempts silently escaped, including both Wolf species substitutions and both Webb cooldown chronology errors. The EIA translation error was correctly diagnosed in one repetition and silently escaped in the other. The refund sprint chronology was caught both times, with actionable evidence and correction directions. Both FAA attempts returned REVIEW on the narrower Dutch rendering. All 3 material findings were actionable. Exact pair agreement was 4/5. These selected regressions are excluded from the primary rates.

## Decision

The accepted editorial findings from Experiment 15 stand. Experiment 19 did not establish a reliable safeguard for material drift. The combination of unresolved prior entity and chronology errors, repeated misses on the archived checks and high primary review variability leaves unattended Narrative approval unsupported. A separate production integration decision should not proceed on this evidence. No follow-on experiment was started.

[Protocol](protocol.md) · [Sources](source-provenance.md) · [Metrics](metrics.json) · [Error analysis](error-analysis.md) · [Stability analysis](stability-analysis.md) · [Decision](decision.json) · [Reproduction](REPRODUCE.md)

