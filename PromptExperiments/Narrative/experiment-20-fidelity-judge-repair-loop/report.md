# Experiment 20 — Narrative Fidelity Judge & Repair Loop

**NO-GO for production integration.** Two of eight known material errors were detected and repaired. Six remained, all in briefs the loop eventually accepted. The mandatory detection and repair thresholds were 90% and 80%, respectively. Recurring species and chronology misses persisted despite explicit checks. Editorial scores held steady within this corpus.

## Design and corpus

Each pair shared the exact initial brief. Arm A retained it. Arm B ran a structured audit of entity identity, temporal relationships, actors and objects, causality, and quantitative meaning against the Selected Idea, Synthesis and original source. A confirmed blocker could trigger a targeted full-brief repair and fresh reassessment. The loop allowed up to two repairs, capped each Judge reply at three blockers, and recorded uncertainty or technical failures as unresolved. The schema and validation rules were frozen with the corpus, prompts and thresholds.

| Cohort | Subjects | Briefs | Initial F0 / F1 / F2 | Final F0 / F1 / F2 | Editorial mean before → after |
| --- | ---: | ---: | ---: | ---: | ---: |
| Historical regressions | 5 | 8 | 0 / 0 / 8 | 2 / 0 / 6 | 4.18 → 4.23 |
| Historical accepted | 4 | 4 | 3 / 1 / 0 | 3 / 1 / 0 | 4.15 → 4.15 |
| Fresh production CLI | 8 | 24 | 20 / 4 / 0 | 20 / 4 / 0 | 4.14 → 4.14 |
| Total | 16 | 36 | 23 / 5 / 8 | 25 / 5 / 6 | 4.15 → 4.16 |

One historical subject appears in both archived cohorts. The fresh cohort used eight distinct NOAA source subjects, with three generations per subject. Editorial handoffs were prepared from the source evidence and production Narrative generated the briefs. The 24 fresh briefs are repeated observations on eight sources and offer no fresh F2 detection evidence. Source material clusters within ocean and coastal science. See [dataset manifest](dataset-manifest.csv) and [provenance](source-provenance.md).

## Detection and repair

- Initial Judge precision was **2/3 (66.7%)** and recall was **2/8 (25%)** using independent source judgments and issue matches. No brief received UNCERTAIN. Five initial F2 briefs received GO, and one REVISE addressed a separate minor issue.
- **2/8 initially F2 briefs (25%)** were corrected, both on the first attempt and on different subjects. One Wolf brief corrected Dutch prey terminology to wapiti. EIA corrected the Btu scale from quadriljoen to biljard. No live case needed a second repair.
- **Six F2 briefs remained**: two Wolf species substitutions, two Webb chronology errors, one FAA category error, and one Digital sprint chronology error. None of the three repairs introduced a new F2.
- **0/28 F0/F1 briefs** received a REVISE, and none was rewritten. One Webb brief with an independent F2 elsewhere received a minor mirror-terminology blocker and a rewrite that left the material cooling error in place.

## Editorial retention

**35/36** briefs passed the Experiment 15 editorial gate both before and after. Among the 34 operationally accepted outputs, **33/34** passed editorially. Mean direction rose by 0.06 and development was unchanged. Review of the three repairs found no flattened story arc or loss of the chosen angle. The small repair sample limits this finding.

Fresh scoring used separate model sessions that received only the brief and its evidence. The operator reconciled archived material errors, disputed findings, repairs and borderline editorial cases against the source. Both review and workflow used GPT-6 Luna, and the operator knew the archived findings. See [error analysis](error-analysis.md), [repair analysis](repair-analysis.md), and [second review](second-review.md).

## Operations

- **66 workflow calls**: 24 new Narrative generations, 39 Judge calls and three repairs. Average per pair was **1.08 Judge calls** and **0.08 repair calls**. Another 48 calls provided independent assessment, for **114 experiment invocations** excluding connectivity probes. Token totals were unavailable in the archived CLI outputs.
- Mean added per-brief latency was **12.82 seconds** (median **10.08**, p95 **34.91**). Production generation averaged **5.55 seconds** for fresh briefs. Judge calls averaged **11.22 seconds** and repairs **7.97 seconds**. Added latency sums per-brief call durations and excludes independent review.
- **31** pairs were accepted immediately, **three** after one repair, and **two** ended in technical failure after invalid Judge JSON. The two failed calls were different Blue Carbon repetitions. Each retained its initial brief for paired comparison and remained operationally unaccepted. Two independent review calls failed excerpt validation and had valid second-pass assessments. No transport failures or incomplete generation occurred.

[Metrics by cohort](metrics.json) · [Frozen decisions](decision.json) · [Run manifest](run-manifest.csv) · [Reproduction](REPRODUCE.md)

Keep the Judge experimental. Before revisiting integration, test whether an explicit source-grounding step for cross-language referents and overlapping events can recover the specific Wolf and Webb misses on complete held-out briefs. The observed repairs show that evidence-led correction can work when the Judge finds the right discrepancy.
