# Experiment 15 — Narrative Editorial Reassessment

**NO-GO for freezing the production Narrative as the approved structuring stage.** All 36 production briefs satisfy the editorial-only requirements. Individual acceptance is 26/36 (72.2%) after the material-fidelity gate. Each of two subjects has F2 errors in five of six production briefs. Both patterns recur across the two production cohorts and affect central story evidence. This exceeds the bounded-risk exception for a Conditional GO. Repair is possible for these archived briefs, but using the stage as-is across subjects would rely on source review to catch similar changes before publication.

## Coverage and production readiness

All 72 stored Narrative outputs are valid and byte-distinct. Every run has its original Narrative JSON, Selected Idea, Synthesis and source. Both A snapshots match the current system and user prompts byte-for-byte. The six fixtures include four archived source-based subjects and two synthetic ones. Both experiments reuse the same upstream evidence, so repeated runs measure stability on those inputs and offer limited evidence of breadth. The single reviewer knew the prior aggregate findings and performed both passes. The scoring view concealed arm and experiment labels and prior row-level outcomes. Textual cues and familiarity mean the blinding was partial. [Manifest](manifest.csv) · [Protocol](protocol.md) · [Second review](second-review.md)

| Production subject | Mean / 5 | Editorial only | Accepted | F0 / F1 / F2 |
|---|---:|---:|---:|---:|
| Cooling | 3.900 | 6/6 | 6/6 | 5 / 1 / 0 |
| Digital | 3.742 | 6/6 | 6/6 | 4 / 2 / 0 |
| Library | 3.933 | 6/6 | 6/6 | 4 / 2 / 0 |
| Lowell | 4.175 | 6/6 | 6/6 | 5 / 1 / 0 |
| Webb | 4.367 | 6/6 | 1/6 | 1 / 0 / 5 |
| Wolf | 3.933 | 6/6 | 1/6 | 1 / 0 / 5 |
| **Total** | **4.008** | **36/36** | **26/36** | **20 / 6 / 10** |

Production score distribution, score × count: 3.50 × 1, 3.75 × 3, 3.85 × 13, 3.90 × 2, 4.00 × 9, 4.25 × 1, 4.35 × 3, 4.40 × 1, 4.50 × 1, 4.75 × 2. Across all arms, 53/72 (73.6%) were accepted, the mean was 4.112, and F0/F1/F2 counts were 42/11/19. [Full distributions](decision.json) · [Per-output scores](scores.csv)

The production briefs generally give Compose a stable central angle, a useful sequence and enough concrete material to develop it. Digital’s least developed brief lists the design choices with little account of how the sprint worked. Several Digital and Cooling endings recap the method or observation, but their development supplies enough to write a coherent conclusion. Lowell generally turns the later downturn into a specific counterweight to the recovery story. Webb shows the main limit of editorial scores: the strongest production subject by mean still fails source fidelity in five of six briefs. Before publishing from affected briefs, verify the onset of Webb’s cooling against the sunshield deployment and correct every wrong prey species and any ambiguous killing actor in Wolf. [Fidelity findings](fidelity-findings.md)

## Comparison with the earlier rubric

Across the four arms, 32 of 51 previous eight-axis failures now pass. Production accounts for 17 of those 32 (9 in Experiment 13 and 8 in Experiment 14). All 21 earlier full passes still pass, and the 19 original material-error flags remain F2. Production’s old full-pass rate was 9/36 (25.0%). The biggest change is that Compose can use a brief with a plain, supported ending when the development gives it material to work with. F1 is also permitted. This does not establish any improvement in the prompt or recovery of a factual failure.

Experiment 3 retained the production baseline after the compact variant lost fidelity and focus. Experiments 11 and 11b showed that downstream publication endings can remain weak and can inherit Narrative errors. Those downstream outcomes motivated the distinction between providing enough material for a conclusion and writing the actual conclusion. They are context only. [Experiment 3](../experiment-3-compact-2026-11-18/report.md) · [Experiment 11](../experiment-11-editorial-tension-to-grounded-ending-2026-11-18/report.md) · [Experiment 11b](../experiment-11b-editorial-tension-to-grounded-ending-2026-11-18/report.md)

## Candidate comparisons

| Arm | Mean / 5 | Editorial only | Accepted | F0 / F1 / F2 | Old full | Old failure now accepted |
|---|---:|---:|---:|---:|---:|---:|
| 13A | 4.006 | 18/18 | 14/18 | 13 / 1 / 4 | 5/18 | 9 |
| 13B | 4.200 | 18/18 | 13/18 | 10 / 3 / 5 | 6/18 | 7 |
| 14A | 4.011 | 18/18 | 12/18 | 7 / 5 / 6 | 4/18 | 8 |
| 14B | 4.231 | 18/18 | 14/18 | 12 / 2 / 4 | 6/18 | 8 |

Experiment 13 B’s editorial mean is higher, but its accepted count falls by one. Its additional F2 leaves no useful overall readiness gain. Experiment 14 B has a modest practical improvement of two more accepted briefs and two fewer F2s, with some Webb and Wolf runs preserving the source correctly. Four F2s remain, and 14/18 is below the 80% individual acceptance threshold. Reused fixtures and three repeats per subject limit extrapolation. Neither candidate is recommended for promotion. [Experiment 13](../experiment-13-fidelity-and-resolution/report.md) · [Experiment 14](../experiment-14-evidence-anchored-narrative/report.md)

## Rubric suitability and next step

The rubric distinguishes usable recaps from stronger progressions that culminate in a concrete observation or reversal. Most production briefs cluster around 3.85–4.00. There were no unusable briefs in this archive, so the low end of the scale remains untested. Strong evidence can raise both E and C, and a strong reversal can raise B and D. During review, E reflected what was supplied, C whether it was enough to draft with, B how the story progressed and D what conclusion that progression supported. The F2 gate matters because a strong arc can carry a wrong claim convincingly.

Keep the production prompt in place without approving it for freeze. Continue Narrative-only work on preserving entity identity across translation and retaining overlapping chronology. Require evidence that any future fix holds on these regression fixtures and on unseen cases with similar factual relationships before moving on to Compose.
