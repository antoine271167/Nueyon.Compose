# Experiment 17 — Narrative Semantic Validation

**NO-GO.** GPT-6 Luna detected two of three material changes on eight held-out subjects with either input. Adding original source text improved observed precision and reduced review workload. Recall remained 66.7% against the frozen 90% target. Wolf and Webb showed that the same validator can miss a supported species discrepancy and fail to use direct chronology evidence.

## Dataset and method

The inventory checks 120 archived outputs from Experiments 13, 14 and 16, with no identical output bytes. A purposive selection of 21 briefs spans 10 subjects, with five Wolf and Webb briefs in development and 16 briefs on eight disjoint subjects in evaluation. Evaluation contains eight F0, five F1 and three F2 briefs. The three positives represent three sources. The operator reviewed the underlying evidence independently of validator results, with historical labels visible, and upgraded one Digital brief from F1 to F2 before freezing. FAA, EIA, NIST and suffrage use curated evidence fixtures. Library and cooling use synthetic sources. No Narrative or Compose runs were launched.

One frozen prompt checked material meaning, including translation, time, causality and quantities. It returned PASS, REVIEW or FAIL with exact excerpts and a correction direction. S used Selected Idea, Synthesis and Narrative. S+Source supplied the same fields plus archived source text. Forty-two GPT-6 Luna responses were archived and schema-validated on the first attempt. Sampling parameters followed provider defaults.

## Held-out evaluation

| Metric | S | S+Source |
|---|---:|---:|
| True positives / false positives / true negatives / false negatives | 2 / 1 / 12 / 1 | 2 / 0 / 13 / 1 |
| Precision | 2/3 (66.7%) | 2/2 (100%) |
| Recall | 2/3 (66.7%) | 2/3 (66.7%) |
| False-positive rate | 1/13 (7.7%) | 0/13 (0%) |
| False-negative rate | 1/3 (33.3%) | 1/3 (33.3%) |
| REVIEW | 2/16 (12.5%) | 1/16 (6.3%) |
| FAIL or REVIEW | 5/16 (31.3%) | 3/16 (18.8%) |
| Actionable evidence among confirmed material findings | 2/2 (100%) | 2/2 (100%) |

Finding-level precision and recall match the output-level results here. The confirmed cases were the EIA English-to-Dutch number scale and the Digital private-beta sprint overlap. S missed the FAA label change and overclassified a minor NIST scope ambiguity. S+Source recognized the FAA issue as REVIEW. Among the five development briefs, recall was 0/2 in each condition, with one REVIEW each. See error-analysis.md for the evidence differences and examples.

The 13 acceptable evaluation briefs show limited interference with faithful or mildly ambiguous editorial language, with one false FAIL and two acceptable REVIEW cases for S, and zero false FAIL or acceptable REVIEW cases for S+Source. The experiment tested validation only. It did not measure subsequent writing behavior or creativity.

## Decision and next step

The frozen recall gate fails in both conditions. Three independent evaluation positives are also below the minimum of five for GO, and some source fixtures add little or no content beyond Synthesis. The apparent gains in precision and review require replication. The FAA finding shows the value of an explicit operational definition and the distinction between a material error under ground truth and a tentative judgment given limited evidence. A single operator, visible prior labels, a one-call design and a diagnostic archive partition limit inference about generalization and repeatability.

Keep the study diagnostic. Before another validator experiment, assemble independent material examples of entity identity, overlap, reversed actors and causal overstatement alongside matched faithful paraphrases. Include complete source passages where the relationship depends on a definition or a timeline. No production promotion or architecture change follows from this run.

