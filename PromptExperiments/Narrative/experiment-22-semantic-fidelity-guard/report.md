# Experiment 22 — Semantic Fidelity Guard

**NO-GO. Architectural recommendation C.** Keep the guard experimental. Test source-grounded verification of final article claims after Compose before considering unattended release.

The hybrid confirmed 11/13 held-out material discrepancies. The unchanged Experiment 20 Judge confirmed 11/13 and raised 1 additional relevant unresolved flag. Both confirmed 10/10 deliberate mutations. The hybrid missed or misidentified 2 of 3 naturally occurring held-out errors.

## Corpus and evidence

The corpus contains 49 complete briefs from 15 subjects. Five Wolf and Webb development cases are excluded from primary metrics. The 44 held-out briefs span 13 subjects and contain 13 reference F2 cases and 31 acceptable briefs. The 30 unmodified production outputs on ten new subjects contained zero confirmed natural F2s. Naturally occurring positive evidence is limited to the three familiar archival subjects. All ten authored mutations were separately confirmed by a blinded model review. Archived reference labels are inherited from the previous source reviews.

New production briefs used one short authoritative excerpt and an operator-prepared handoff per subject. Subject diversity is stronger than the claim-density and positive-case diversity. Source relationships and anchor expressions were supplied manually. No full-brief material-claim census was collected. [Dataset](dataset-manifest.csv) · [Sources](source-provenance.md) · [Freeze ledger](freeze.json)

## Paired first attempts

| Condition | Confirmed F2 | Additional matched risks | Confirmed blocker precision | False FAIL on acceptable | REVIEW | Technical failure | Silent F2 escapes |
|---|---:|---:|---:|---:|---:|---:|---:|
| A · Experiment 20 | 11/13 | 1 | 12/13 | 0 | 2 | 0 | 1 |
| B · Mechanical only | 0/13 | 0 | — | 0 | 44 | 0 | 0 |
| C · Hybrid | 11/13 | 0 | 11/13 | 1 | 1 | 0 | 1 |

| Cohort | A confirmed | A additional matched risks | B confirmed | C confirmed | C additional matched risks |
|---|---:|---:|---:|---:|---:|
| Evaluation | 11/13 | 1 | 0/13 | 11/13 | 0 |
| Historical regressions | 1/3 | 1 | 0/3 | 1/3 | 0 |
| New mutations | 10/10 | 0 | 0/10 | 10/10 | 0 |

| Reference category | A confirmed | C confirmed |
|---|---:|---:|
| entity identity | 3/4 | 3/4 |
| numerical scale | 3/3 | 2/3 |
| temporal relationship | 2/3 | 3/3 |
| causality | 2/2 | 2/2 |
| actor object | 1/1 | 1/1 |

B found no material discrepancy on the evaluation corpus. Every B outcome was REVIEW. The EIA scale fingerprint fails when the line also includes the distinct electricity ratio. Isolating the EIA ratio demonstrated a mechanical block, but an editorial prohibition using the same literal pattern falsely blocked in the supplementary probe. Entity and chronology routing supplied candidates and had no safe standalone block. [Mechanical probes](runs/fault-check.json)

The hybrid routed 13/13 exact held-out error spans into the candidate set. Candidate routing and evidence quoting were insufficient to establish correct interpretation. All matched hybrid blockers came from the focused model. [Coverage](coverage-analysis.md) · [Case data](scores.csv)

## Historical regressions

| Subject | Split | A · Experiment 20 | B · Mechanical | C · Hybrid |
|---|---|---|---|---|---|
| Wolf | development | FAIL, PASS, FAIL (matched) | REVIEW, REVIEW, REVIEW | FAIL (matched), FAIL (matched), FAIL (matched) |
| Webb | development | PASS, PASS | REVIEW, REVIEW | PASS, PASS |
| EIA | evaluation | FAIL (matched) | REVIEW | FAIL |
| FAA | evaluation | REVIEW (matched risk) | REVIEW | PASS |
| Digital | evaluation | PASS | REVIEW | FAIL (matched) |

Unmatched FAIL indicates a different discrepancy. In EIA, the hybrid conflated the energy and electricity comparisons; the baseline correctly identified the Dutch scale on its first attempt. In FAA, the baseline flagged the relevant category uncertainty and the hybrid accepted the changed name. Digital was the hybrid’s archival gain on the first attempt; that result disappeared on repeat. Both Webb chronology errors escaped both model conditions in development. The wolf results were obtained with the intended referents explicitly supplied.

## Editorial freedom and operating cost

No brief or upstream content was rewritten. The blinded rubric review found 8/30 new naturally generated briefs at or above the Experiment 15 editorial gate. Compact source handoffs often produced repetitive progression and thin endings. The hybrid withheld 2 of 31 acceptable briefs in primary evaluation. See the case analysis for the contested Digital finding and evidence-limited risk flags. [Editorial review](editorial-impact.md) · [False positives](false-positive-analysis.md)

A and C each used one call per brief. Summed first-attempt model-call time was 472.27 s for A and 383.84 s for C. B’s mechanical processing totaled 14.44 ms. The paired evaluation used 134 calls including repeats and took 355.50 s wall-clock. There was one invalid baseline repeat, which was held as a technical failure. All hybrid calls passed the response contract. No token, credit or price measurement was captured. [Operations](operational-cost.md)

## Decision and next action

The 90% matched-detection-or-risk gate was missed and the hybrid did not improve paired triage over the Experiment 20 Judge. The deterministic prototype demonstrated a false contradiction and had no confirmed live detection. Chronology escapes and repeated-answer inconsistency remain. The natural positives are too sparse to generalize. Scope, stability and operational value therefore do not justify adding a Narrative integration.

Evaluate a final-content release check on archived or separately generated articles. Preserve a source-backed evidence map through the pipeline, then compare the final article’s material factual claims with source evidence. Measure final-stage errors and article-level editorial quality together. Keep publication withheld when material evidence remains unresolved or verification fails. Advance toward unattended release only after this final gate is shown to be accurate and stable on diverse complete articles.

[Decision gates](decision.json) · [Metrics](metrics.json) · [Reproduction](REPRODUCE.md)
