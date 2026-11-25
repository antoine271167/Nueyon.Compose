# Experiment 23 · Compose editorial quality and factual fidelity

## Executive summary

**Article editorial readiness: NO-GO. LinkedIn Post editorial readiness: NO-GO. Unattended publication: NO-GO.** On 12 archival subjects, Article passed the editorial gate in 2 of 36 outputs (5.6%) and LinkedIn Post in 8 of 35 (22.9%). The final review confirmed 16 material discrepancies across 16 outputs. Fifteen propagated upstream errors. One was introduced by Compose. LinkedIn scored better descriptively, and both formats fell far below the predefined 80% readiness threshold.

| Measure | Article | LinkedIn Post |
| --- | ---: | ---: |
| Planned calls | 36 | 36 |
| Valid outputs | 36 | 35 |
| Mean / median editorial score | 3.30 / 3.35 | 3.50 / 3.45 |
| Editorial acceptance | 2/36 (5.6%) | 8/35 (22.9%) |
| Confirmed F2 outputs | 9/36 (25.0%) | 7/35 (20.0%) |
| Propagated / Compose-introduced F2 | 9 / 0 | 6 / 1 |
| Either editorial fail or confirmed F2 | 34/36 (94.4%) | 27/35 (77.1%) |

## Method and coverage

The [frozen protocol](protocol.md) fixed the dataset, prompts, scoring and decision rules before the first evaluated Compose call. The primary dataset uses unchanged Narrative outputs, with three independent processes for each format per subject. Each case has upstream Idea, Synthesis and source evidence. A standalone Research result was unavailable. The corpus covers standards and AI guidance, engineering, science, public policy, organizational product delivery and uncertainty. Six subjects were classified rich and six constrained before sampling. Three known-failure briefs were included to stress source fidelity. Production receives only Narrative text and the format enum. [Dataset](dataset-manifest.csv) · [Production baseline](production-baseline.md) · [Freeze](freeze.json)

Separate unattended, fresh GPT-6 Luna sessions judged editorial and factual quality without historical verdicts or Compose prompts. The factual reviewer received the frozen source extract and upstream handoff. A second session audited all scheduled repetitions and flagged cases per protocol. The operator checked source evidence for conflicting findings and inspected the regression subjects directly. Source and operator checks are visible in the artifact rather than treated as blind machine verdicts. [Source checks](source-checks.md) · [Adjudications](reviews/factual/adjudications.json)

## Article quality

Article prose was usually clear and focused, with mean 3.92 for readability and 4.19 for focus. Development averaged 2.83 and endings 2.44. Thin substance appeared on 32/36 drafts, and all 12 subjects yielded a failing draft. The strongest fusion Article uses two concrete measurements to clarify the experimental threshold. The weak digital Articles repeat the approach with limited development. [Editorial analysis](editorial-analysis.md)

## LinkedIn Post quality

LinkedIn pacing averaged 4.03 and progression 4.26. Its strongest technical posts make a concrete behavioral distinction part of the hook. Endings averaged 2.71 and were the weakest dimension. Eleven of 12 subjects yielded at least one editorial failure. Python alone passed all three LinkedIn repeats. [Editorial analysis](editorial-analysis.md)

## Factual fidelity and attribution

One usable factual audit per draft yielded 331 Article claim entries (315 F0, 7 F1, 4 F2 and 5 U) and 302 LinkedIn entries (274 F0, 11 F1, 7 F2 and 10 U). These are machine labels before evidence adjudication. Four usable secondary assessments filled gaps in the primary audits. The adjudication found 16 confirmed material discrepancies and two unresolved flagged readings. It resolved false positives from source-extract omissions and found defects missed by the machine inventory. Four Article and seven LinkedIn outputs with dual usable reviews disagreed in the presence of an F2 or U flag, among 17 and 15 dual-reviewed outputs respectively. [Factual analysis](factual-fidelity-analysis.md) · [Metrics](metrics.json)

The confirmed problems are five Yellowstone prey-species substitutions, six altered sprint cadences, four Webb cooling and deployment timing distortions and one new LinkedIn claim that makes the cap itself tradable. The 15 propagated defects were already present in Narrative. The one new case attached tradability to the wrong part of the EPA mechanism. [Error attribution](error-attribution.md)

## Recurring problems and reliability

The three most frequent editorial patterns were thin substance, report-like voice and generic endings, tied with repetition. The weak passages commonly compress a brief into clear but lightly developed paragraphs and reuse the opening as the ending. This occurred with both richness strata. Observed weakness can arise from the brief, the restrictions on elaboration and the model's choices. The design varies neither prompt nor handoff within a subject, so it cannot isolate causal effects. [Recurring patterns](recurring-patterns.md)

The generation route completed 71 valid drafts in 72 attempts. One LinkedIn call failed with an unclassified runtime exception. Mean elapsed process time was 4.84 s for Article and 5.82 s for LinkedIn. Provider tokens and dollar cost were unavailable. The widest elapsed review workload came from factual claim scans. [Operating measurements](operational-cost.md)

## Limitations and decisions

Both formats exceeded the review coverage floor but missed the acceptance and recurring-critical-weakness gates. Short curated authoritative extracts created real review disagreements. The archive overrepresents English-language US public institutions and historical stress cases, and includes only one Narrative per subject. Product coverage comes from a public-service launch. There is limited private-sector or personal-story evidence. Exact provider builds and wire responses were unavailable. Same-family reviewers and a single operator do not substitute for human validation. Reported repeat frequencies are descriptive of this dataset.

An unattended publication GO requires demonstrated verification and withholding. Neither was established. Confirmed material findings reinforce the NO-GO. The next experiment should test source-grounded final-content verification on blinded correct and incorrect drafts, measure false holds, and exercise withholding on naturally occurring species, chronology and policy-mechanism defects. Keep Article editorial development as a separately controlled follow-up. [Decisions](decision.json) · [Recommendations](recommendations.md)
