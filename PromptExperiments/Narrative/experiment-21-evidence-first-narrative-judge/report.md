# Experiment 21 — Evidence-First Narrative Judge

**NO-GO.** Evidence-first matched 3 of 9 material errors as supported contradictions and 0 as relevant unresolved risks. Triage was 33.3% against the 90% gate. The study stopped before repair.

## Dataset and confidence

The frozen evaluation contains 16 complete Narratives on 11 source subjects. There are six historical F2 briefs, four historical acceptable briefs, three current-production controls and three operator mutations on those same three new sources. The nine F2 briefs span 8 subjects. Only FAA, EIA and Digital were historical subject holdouts; the evaluated Wolf and Webb samples share source facts with development. Development outcomes were 2336788e5a technical_failure, 4632ab6ce2 GO.

The purposive corpus has dependent pairs and repeated subjects. Each error changes observed recall by 11.1 percentage points. The positive-subject and acceptable-brief counts fall short of the frozen breadth floor and there is no F2 causality target. There is insufficient evidence for generalization, with a demonstrated failure of the observed detection gate.

## Paired Phase A

| Condition | Material recall | Blocker precision | Triage recall | Silent escape | Acceptable REVISE | UNCERTAIN | Technical blocks |
| --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: |
| Evidence-first | 3/9 | 100.0% (4/4) | 33.3% | 0.0% | 0.0% | 43.8% | 5 |
| Experiment 20 | 6/9 | 85.7% (6/7) | 66.7% | 22.2% | 0.0% | 0.0% | 0 |

The comparator was rerun in fresh GPT-6 Luna sessions on exactly the same documents and briefs, using the archived prompt, schema and decoder. Original Experiment 20 aggregate results were 2/8 known errors detected and repaired. The paired results above are the relevant comparison. Evidence-first yielded one additional historical detection (Digital) and lost four detections the comparator made (Wolf, FAA, EIA and Bennu). Six of seven acceptable briefs were withheld by the candidate. A technical hold or an unrelated uncertainty blocks progression, but earns no factual detection credit.

The accepted extraction captured 8/9 target assertions. Correct source anchors appeared in 8/8 assessable target traces, including raw outputs later rejected on other quotations. See the decomposition for stages that dropped a relationship or erased a term during normalization.

| Category | Valid extracted claims | Reference F2 targets | Confirmed |
| --- | ---: | ---: | ---: |
| entity_identity | 16 | 3 | 0 |
| temporal_relationship | 57 | 2 | 1 |
| actor_object | 64 | 1 | 0 |
| causality | 11 | 0 | 0 |
| numerical_scale | 36 | 2 | 1 |
| scope_certainty | 50 | 1 | 1 |

The exact results for Wolf, Webb, EIA, FAA and Digital are in [error analysis](error-analysis.md). [Failure decomposition](failure-decomposition.md) traces extraction, location, normalization, comparison and aggregation separately for every F2.

## Editorial and factual outcomes

The initial reference set has 6 F0, 1 F1 and 9 F2 briefs. The full-brief Experiment 15 rubric yielded 14/16 editorial passes, with a mean of 4.01/5. The Judge only assessed briefs. Repair, editorial change, post-repair fidelity and introduction of new errors through repair were therefore unmeasured.

## Runtime, cost and unattended handling

The production console generated three new originals in 16.3 summed seconds and reported no token or monetary usage in these run archives. The candidate spent 31 evaluation calls across extraction and comparison. Aggregation was local with no model call. Repair and reverification used zero calls.

| Stage | Calls | Valid | Invalid JSON | Invalid contract | Transport failure | Mean elapsed s | AI credits and metered coverage |
| --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: |
| extraction | 16 | 15 | 0 | 1 | 0 | 25.5 | 3.029 (16/16) |
| comparison | 15 | 11 | 0 | 4 | 0 | 26.2 | 3.404 (15/15) |
| baseline | 16 | 16 | 0 | 0 | 0 | 11.6 | 1.540 (16/16) |

Per-call model latency includes CLI startup. The sequential candidate stages incurred 800.9 summed seconds and 6.433 credits across evaluation, compared with 185.6 seconds and 1.540 credits for the comparator. AI credits come from matched Copilot session shutdown records and are separate from wall time and any currency costs. Six independent review calls are archived separately with usage.

Evidence-first marked 1 eligible, withheld 3 for asserted contradictions, 7 for uncertainty and 5 for technical faults. Source and Narrative substring checks caught model quotation errors. Deterministic validation checks cover duplicate keys, empty extraction, missing assessments, invalid quotes, missing evidence, material uncertainty and contradictions. GO remains unsafe when normalization hides a discrepancy or a relationship goes unextracted.

## Decision and next action

Keep Narrative outside unattended publication. For a subsequent detection experiment, preserve claim surface wording and referents alongside normalization, ground evidence with mechanically resolvable spans, and test those changes on a new frozen subject-disjoint corpus before considering repair.

Delivery commit SHA is recoverable with the command in [REPRODUCE.md](REPRODUCE.md).
