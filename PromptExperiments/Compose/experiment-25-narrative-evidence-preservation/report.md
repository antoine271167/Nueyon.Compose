# Experiment 25 — Narrative-to-Compose Evidence Preservation

## Executive summary

**NO-GO.** In this holdout, B preserved more useful, supported evidence and Compose used more of it in comparable Articles. The finished Articles did not improve enough to pass the editorial gate. A, B and C each reached **0/36** editorial acceptances. B's mean development score improved by **0.11**, against a required **0.40**. B also introduced a material species substitution at the Narrative stage. No prompt was promoted.

The evidence favors testing **Compose's selection and development of supported material** next. C passed more useful evidence through the handoff and used more of it in the resulting Articles, while C's average Article development and overall scores fell. Evidence availability was a genuine constraint in some A handoffs, but it did not solve the editorial problem in this comparison.

## 1. Baseline and competing explanations

Experiment 23 found low Article acceptance and shallow development. Experiment 24 changed the Article prompt while keeping Narrative fixed. Both candidates missed the quality thresholds and thin substance persisted. Experiment 25 held Compose fixed and intervened at the handoff. This tests whether current Narrative compression loses useful substance, whether the available evidence places a ceiling on the story, and whether Compose selects or develops the available substance effectively. [Experiment 23](../experiment-23-compose-quality-baseline/report.md) · [Experiment 24](../experiment-24-article-editorial-development/report.md) · [Hypothesis](hypothesis.md)

## 2. Design and controls

A regenerated the production Narrative from each frozen Selected Idea and Synthesis. B regenerated from the same input using one evidence-preservation paragraph in the user prompt. C reorganized an operator-verified selection of exact Synthesis spans. It used the same text content contract and included the selected editorial title. C was diagnostic. Its shape, ordering, language and concentration of evidence differ from A, so C cannot isolate an effect of evidence quantity in isolation.

The same production Compose system prompt, Article prompt, provider alias and runtime processed each Narrative. The plan rotated conditions and reversed subject order in repetition two. Each generation used a fresh CLI process. Reviews were independently prompted in separate sessions, with condition labels hidden. The operator later checked disputed source claims and important inventory mismatches with the conditions visible. [Protocol](protocol.md) · [Candidate comparison](candidate-comparison.md) · [Freeze](freeze.json)

## 3. Dataset and evidence coverage

The subject-level split was six development and twelve holdout, with six fresh source-backed holdouts. The holdout contained six rich and six constrained cases. Five archived subjects tested the historical Wolf, Webb, Digital, EIA and FAA regressions. Fresh cases covered orbital change, Bennu sample curation, post-quantum standards, earthquake-magnitude uncertainty, voting age and Git sparse checkout. The archive retained original references, evidence excerpts, upstream input and any archived production Narrative. Standalone Research was unavailable. Fresh Selected Idea and Synthesis records were operator-prepared. [Dataset](dataset-manifest.csv) · [Source provenance](source-provenance.md)

## 4. Useful information available in Synthesis

The audited inventory contained **56 useful items** in the holdout and **28** in development. Items encompass quantitative denominators and units, actors and categories, temporal and operational steps, relationships, uncertainty and qualified outcomes. Items can include several related elements whose meaning depends on being conveyed together. A preserved the central story on most subjects while compressing parts of individual propositions. The most consequential omissions included Digital's within-sprint steps and EIA's underlying quantities. [Item-level outcomes](evidence-item-outcomes.json)

## 5. Evidence preservation and Narrative editorial quality

| Holdout measure | A | B | C |
| --- | ---: | ---: | ---: |
| Useful inventory items preserved, subject mean | 74.7% | 92.9% | 100.0% |
| Raw independent classifier, before targeted adjudication | 77.2% | 85.0% | 90.1% |
| Narrative editorial mean /5 | 4.29 | 4.34 | 4.07 |
| Independently rated distinct developments | 3.25 | 3.00 | 3.67 |
| Narrative words, mean | 170.7 | 185.0 | 107.3 |

A's mean loss was 25.3% of relevant evidence by subject, including omissions and materially altered items. B regained 18.2 percentage points after targeted source-backed corrections, compared with the frozen 20-point requirement. The paired 95% subject bootstrap interval was +4.3 to +33.8 points. The independent raw estimate was +7.8 points. Corrections include checking elk identity, recognizing the complete FAA mapping, requiring EIA quantity pairs where an item includes them and verifying the DART quantities against JPL. Raw judgments and operator changes remain available.

B reduced observed repetition, added roughly 14 words and preserved more evidence, while the independent development count fell. C's much shorter text preserved the holdout evidence inventory after adjudication, but its Narrative editorial score was lower. This distinguishes coverage from progression and editorial usability. Some C spans share subsidiary content, notably Webb's repeated alignment period. [Narrative analysis](narrative-analysis.md) · [Source-backed preservation corrections](preservation-adjudications.json)

## 6. Finished Article quality

| Holdout measure | A | B | C |
| --- | ---: | ---: | ---: |
| Editorial acceptance | 0/36 | 0/36 | 0/36 |
| Weighted editorial score /5 | 3.08 | 3.12 | 2.90 |
| Development /5 | 2.39 | 2.50 | 2.25 |
| Ending /5 | 2.14 | 2.33 | 2.08 |
| Readability /5 | 3.86 | 3.72 | 3.61 |
| Mean words | 92.0 | 106.8 | 97.4 |

B's paired mean editorial gain was 0.04 (95% subject-bootstrap interval −0.05 to +0.12). Development gained 0.11 (−0.11 to +0.31), with openings and endings also directionally higher. Focus was equal on average and readability fell 0.14. B added 14.75 words on average. Those additional words did not produce a passing Article. C started less strongly, on average, and developed the story less effectively than A. The acceptance bootstrap is degenerate because there were zero observed acceptances in every condition. This does not establish zero population acceptance. [Paired Article analysis](article-analysis.md)

## 7. Realized evidence usage in Compose

Digital is a concrete instance of useful information restored at the boundary. The B Articles cover the individual sprint weeks that A summarizes. The gain in development was only one third of a point over its three generations. EIA's B Articles retained both denominator pairs and reported scales. They still often presented the material as sequential figures. Bennu retained the full five-item inventory in A and B, and development fell under B. These comparisons support an evidence-transfer mechanism on specific cases and a separate difficulty forming readable developments from those facts.

Across all 12 paired subjects, the mean useful handoff items reaching an Article rose from 3.17 in A to 4.22 in B and 4.31 in C. The B−A gain was 1.06 items (95% interval +0.36 to +1.83). The C−A gain was 1.14 (+0.50 to +1.83). Thin substance was tagged on 36/36 A, 36/36 B and 35/36 C drafts. Supported endpoint tags increased in both richer treatments, while mean C ending quality fell. These measurements distinguish inclusion of a supported proposition from editorial development. [Article analysis](article-analysis.md) · [Claim-level realization](evidence-item-outcomes.json)

## 8. Factual fidelity and attribution

B preserved the Digital cadence and EIA quantities, with a material species substitution remaining in the Wolf handoff. A introduced the same kind of Wolf substitution and a misleading Webb sequence. Source-faithful Wolf evidence in C still became Dutch *elanden* in Compose, and one EIA C Article inflated both absolute Btu quantities while preserving their ratio. The factual ledger distinguishes Narrative introductions, propagation, Compose introductions and amplification. [Source checks and attribution](factual-fidelity-analysis.md) · [Claim findings](factual-findings.csv)

## 9. Rich and constrained cases

Rich cases showed a larger preservation gain after adjudication, from 71.7% to 95.8%, with development flat at 2.44. Constrained cases improved development from 2.33 to 2.56, with zero acceptances. Some constrained briefs still provided a meaningful contrast or endpoint, but the design does not establish a general source-length ceiling. The rich-case result makes source scarcity alone an inadequate explanation. [Subgroup results](article-analysis.md)

## 10. Format neutrality

The same A and B handoffs were passed to the existing LinkedIn writer for four independent subject pairs. All eight generations succeeded. A averaged 3.14 and B 3.32, with zero editorial acceptances and no confirmed F2 in either group. B preserved the string contract and the three progression bullets. A Git sparse factual-review flag disputed format neutrality without pointing to a channel instruction. The independent editorial review and direct inspection agreed the text was format-neutral. Four pairs provide a limited transfer check. [LinkedIn results](format-neutrality-analysis.md)

## 11. Operations and verification

The study ran 36 successful production Narrative calls, assembled 18 extractive references, generated 36 development Articles and generated 108 holdout Articles. Earlier input rejection on three archived cases affected six A/B CLI invocations before the provider. A casing-only recovery preserved every field value and retried only those pre-model slots. The original errors, raw decisions and missing or failed reviews are retained. The final checks validated the initial and revised freezes, exact source and input fingerprints, paired meaning-equivalence, Narrative hashes, output contract, review linkage and unchanged production prompts. The full solution build succeeded and 303 tests passed. [Operations](operational-cost.md) · [Reproduction](REPRODUCE.md)

## 12. Limitations

Eighteen purposively chosen subjects do not represent all likely stories. Fresh inputs use constructed Synthesis rather than a new upstream pipeline capture. The independent reviews used separate sessions of one model family, with one editorial rating per output. The factual audit was supplemented by operator adjudication of exact disputed claims and key historical probes, which was not blind. Item classification remains sensitive to grouping and scope, so raw and corrected estimates are both available. Single Narrative realizations leave uncertainty about prompt stability. C's extraction altered presentation, including section breadth and mixed-language spans in some Dutch fixtures. Its fidelity and evidence-use estimates are scoped to the frozen inventory, and its quality result does not rule out differently structured evidence-rich handoffs. [Factual coverage](factual-fidelity-analysis.md)

## 13. Decision and promotion

**NO-GO for B.** The preservation change fell short of the frozen threshold, the editorial gates failed, and the material Wolf entity error independently blocks promotion. C is a diagnostic reference. Production Narrative and Compose prompts and application code remain at the baseline. [Decision gates](decision.json)

## 14. Experiment 26

Use a fixed set of verified, format-neutral evidence-rich handoffs, deliberately including disparate evidence types and clear supported relationships. Compare production Compose with one downstream-only editorial intervention that converts supported facts into connected paragraphs, varying the progression and concrete endpoint while maintaining the same source and claim boundary. Track per-paragraph proposition novelty, meaningful support and the percentage of verified handoff facts used, in addition to blinded acceptance and per-claim fidelity. Predeclare required coverage and adjudicate bilingual unit, identity and chronology checks independently. Retain a small constrained-source stratum to estimate where source availability still limits the result. [Boundary analysis](boundary-analysis.md)
