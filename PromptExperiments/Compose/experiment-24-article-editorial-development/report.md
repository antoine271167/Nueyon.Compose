# Experiment 24 report

## Executive summary

**NO-GO for B and C. No production promotion.** This comparison did not demonstrate that either Article prompt can produce substantially more engaging and developed prose from the same Narrative without introducing new meaning. The paired production baseline accepted 2/42 Articles, B accepted 0/42 and C accepted 1/42. Both candidates averaged below the baseline and missed the required development and ending improvements. Some material source errors were already present in the Narrative and survived all three treatments. See the [decision](decision.json) and [factual ledger](factual-findings.csv).

## Baseline and hypothesis

Experiment 23 recorded 2/36 editorial acceptances, 3.30/5 overall, 2.83 development, 2.44 ending and 112 mean words. Nine Articles contained a confirmed material error, with none introduced by Compose. The hypothesis was that more concrete writing instructions would improve development under a fixed meaning boundary. The competing explanation was that the Narrative often has too little distinct substance for a developed Article. [Historical baseline](../experiment-23-compose-quality-baseline/report.md) · [Hypothesis](hypothesis.md)

A uses the exact committed Article user prompt. B adds concrete guidance on how each paragraph, opening and ending should develop existing material. C adds a private sequence for selecting, ordering and ending the story. The production system and model were common to all three conditions. No refinement was run. [Candidate comparison](candidate-comparison.md)

## Design and development selection

The 20-subject sample uses six Experiment 23 subjects for development. The holdout contains six different Experiment 23 subjects, two Experiment 16 regression subjects with new production Narratives and six freshly sourced subjects with production Narratives. The development run used two independent generations per condition per subject. The holdout used three. All 162 planned Article slots returned valid output. The eight supplemental and fresh Narrative preparations were also valid. [Dataset manifest](dataset-manifest.csv) · [Source provenance](source-provenance.md) · [Frozen protocol and decision gates](protocol.md)

Development acceptance was 1/12 for A, 1/12 for B and 2/12 for C. C had an incomplete factual review pair, which placed it behind B under the frozen rank order. B was selected before any holdout generation. C remained a secondary holdout comparison. [Development selection](development-selection.json)

## Paired editorial results

| Holdout condition | Accepted | Mean /5 | Development /5 | Ending /5 | Mean words |
|---|---:|---:|---:|---:|---:|
| A production | 2/42 (4.8%) | 3.20 | 2.50 | 2.29 | 91.4 |
| B supported development | 0/42 (0.0%) | 3.09 | 2.45 | 2.29 | 88.6 |
| C writing strategy | 1/42 (2.4%) | 3.14 | 2.43 | 2.38 | 90.8 |

The study required +20 percentage points in acceptance, +0.30 overall, +0.40 development and +0.40 ending. Neither candidate reached any of these four thresholds. C's ending improvement was 0.10 points, and generic endings fell from 22 to 16, while development declined by 0.07 points. Every holdout Article received a thin-substance tag in its independent editorial assessment. Examination of paired passages shows that a concrete detail can make a useful passage without developing the entire Article. The modest differences in word count add no evidence of improvement. [Passages and distributions](editorial-analysis.md) · [Exact subject and repetition pairs](metrics.json) · [Scores](editorial-scores.csv)

## Factual fidelity
| Holdout condition | Articles with confirmed F2 | New Compose F2 | Propagated F2 | Amplified F2 |
|---|---:|---:|---:|---:|
| A production | 10/42 | 1 | 8 | 1 |
| B supported development | 8/42 | 1 | 7 | 0 |
| C writing strategy | 10/42 | 1 | 8 | 1 |

A and B each introduced a headline that confused the two FAA categories with the three shapes. C widened a claim about distant whale calls to cross-ocean transmission. A and C each amplified inherited Webb sequencing in one Article. The required review coverage was incomplete for 13 holdout Articles. Two had no usable blind factual review and received manual coverage while the blind-review failures remained in the counts.


The key historical issues were species identity for Wolf, commissioning overlap for Webb, and within-sprint cadence for Digital. The EIA brief retained the percentage and denominator contrast but omitted the absolute quantities, limiting the numerical-scale stress test. FAA preserved its category labels in the brief, allowing the Article to be checked for changes to their meaning and to the number of shapes. See the [source audits and attribution](factual-fidelity-analysis.md), including exact discrepant claims, literal Narrative counterparts where available, and evidence for each material finding. Confirmed counts are observations from this sample. Publication safety remains unestablished.

## Input richness and editorial cause

The constrained briefs often had one real development and little new material left after a concrete opening. For example, Stonewall gives a closely bounded date, location and sequence. Several articles repeat the stated geography at the end because that is also the only supported endpoint. This supports an input-limitation explanation for some cases. The richer Voyager and Hope Diamond briefs supplied several concrete steps, yet paired candidate scores still did not improve. Their outcomes remain compatible with editorial selection limits as well as the relatively directive Narrative format. The experiment varied prompt only, kept evidence fixed, and cannot isolate a causal effect of richer input. [Subject-by-subject evidence inventory](input-richness-analysis.md)

## Reliability and limitations

All production responses satisfied the existing contract. Generation and review used the configured GPT-6 Luna alias. The provider build, token usage and billed amounts were unavailable. Editorial and factual assessments ran in fresh blinded Copilot sessions using the same model family. Each Article had one editorial score, which makes individual scores sensitive to reviewer variation. Some factual judgments depend on resolving limited archived source coverage and reviewer wording, documented separately. The purposive sample includes known historical defects and many narrow briefs. The six fresh subjects improve coverage but do not represent all likely Article topics. [Configuration](model-configuration.json) · [Operations](operational-cost.md)

## Decisions and Experiment 25

**B NO-GO.** The preselected prompt reduced acceptance and mean quality and did not improve development or ending. **C NO-GO.** The secondary candidate showed a small ending signal but reduced acceptance, mean quality and development. Neither had qualifying holdout results. Keep production Article, LinkedIn and shared prompts as they are.

For Experiment 25, hold the production Article prompt fixed and pair two format-agnostic Narrative treatments for the same verified source and selected editorial thread. The treatment should preserve several more distinct supported developments and a factual endpoint when available, with original source references kept available for review. Include constrained sources to measure ceilings without padding and cases rich enough to measure whether the additional already-supported developments reach the Article. Preserve attribution, certainty and overlap and score editorial gains independently of factual fidelity. The present evidence justifies testing the Narrative-to-Compose boundary as the next intervention, while leaving open whether a different Article prompt could still help.

