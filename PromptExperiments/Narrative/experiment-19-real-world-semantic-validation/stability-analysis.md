# Stability analysis

All 36 primary pairs used byte-identical input JSON and exact rendered validator prompts. Hashes are recorded per attempt. Independent GPT-6 Luna processes yielded 29 PASS/PASS pairs, 2 REVIEW/REVIEW pairs and 5 PASS/REVIEW pairs. No attempt failed during transport or parsing.

| Primary discordance | Repetition 1 | Repetition 2 |
| --- | --- | --- |
| Acid-1 | REVIEW | PASS |
| Coral-3 | PASS | REVIEW |
| Fusion-1 | PASS | REVIEW |
| Labor-2 | REVIEW | PASS |
| Python-1 | PASS | REVIEW |

Coral-1 and Python-2 returned REVIEW twice. Coral-1's first response also questioned the satellite attribution while its second only questioned the record claim. Exact status agreement therefore overstates finding-level agreement in that case. Pressure from missing context recurs across several subjects. The primary dataset contains no material brief on which to test PASS/flag instability.

The supplementary regression pairs were Wolf PASS/PASS, Webb PASS/PASS, FAA REVIEW/REVIEW, EIA FAIL/PASS and the refund service FAIL/FAIL. Wolf and Webb illustrate consistent misses. EIA illustrates instability on a numeric-scale translation even with constant evidence. The supplementary results do not affect the primary stability calculation.

