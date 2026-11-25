# Experiment 18 — Semantic Validation Reliability

**CONDITIONAL GO**. See decision.json for the frozen gate outcomes.

The primary set contains 48 Dutch synthetic briefs on 12 independent subjects, with 24 matched F2 mutations and 24 acceptable controls. GPT-6 Luna completed 187 valid responses from 192 attempts. The validator prompt was byte-identical to Experiment 17.

| Metric | S | S+Source |
|---|---:|---:|
| Definitive precision | 44/45 (97.8%) | 46/47 (97.9%) |
| Definitive recall | 44/47 (93.6%) | 46/46 (100.0%) |
| Definitive false-positive rate | 1/46 (2.2%) | 1/48 (2.1%) |
| Definitive false-negative rate | 3/47 (6.4%) | 0/46 (0.0%) |
| Triage sensitivity | 47/47 (100.0%) | 46/46 (100.0%) |
| Triage specificity | 43/46 (93.5%) | 46/48 (95.8%) |
| False-positive flag rate | 3/46 (6.5%) | 2/48 (4.2%) |
| Silent-escape rate | 0/47 (0.0%) | 0/46 (0.0%) |
| Total flagged workload | 50/93 (53.8%) | 48/94 (51.1%) |
| Acceptable briefs sent for manual review | 2/46 (4.3%) | 1/48 (2.1%) |
| Evidence-backed blocks | 41/93 (44.1%) | 45/94 (47.9%) |
| Finding-confirmed FAIL recall | 44/47 (93.6%) | 46/46 (100.0%) |
| Finding-confirmed triage recall | 47/47 (100.0%) | 46/46 (100.0%) |
| Actionable evidence among confirmed findings | 44/47 (93.6%) | 46/47 (97.9%) |
| PASS workload | 43/93 (46.2%) | 46/94 (48.9%) |
| REVIEW workload | 5/93 (5.4%) | 1/94 (1.1%) |
| FAIL workload | 45/93 (48.4%) | 47/94 (50.0%) |
| Repeat agreement | 40/45 (88.9%) | 44/46 (95.7%) |

The denominator is valid attempts in the relevant class or set. The attempts share evidence within subjects and fixtures. The approximate 95% one-sided zero-event upper bound is 22.1% across 12 independent subjects, even under binomial random-sampling assumptions. This purposive cohort does not satisfy those assumptions. The per-attempt percentages are descriptive.

All planned F2 attempts per condition: S 47/48 flagged if all invalid or missing positive attempts are counted as unflagged for sensitivity analysis; S+Source 46/48 flagged if all invalid or missing positive attempts are counted as unflagged for sensitivity analysis. Invalid responses are not observed PASSes.

## Hypotheses

- H1. REVIEW added S 3 F2 detections, S+Source 0 F2 detections. Observed triage sensitivity improved where REVIEW surfaced a material issue.
- H2. S and S+Source had 9 versus 4 non-actionable or unconfirmed findings, 3 versus 2 acceptable flags, and 0 versus 0 escapes. The paired observations support the direction of H2.
- H3. F2 detection by category (S / S+Source): identity 10/10 (100.0%) / 10/10 (100.0%); temporal 8/8 (100.0%) / 7/7 (100.0%); actor 10/10 (100.0%) / 9/9 (100.0%); numerical 13/13 (100.0%) / 14/14 (100.0%); causality 6/6 (100.0%) / 6/6 (100.0%). See error-analysis.md for misses and stability-analysis.md for cross-run disagreement.
- H4. Manual REVIEW took 5/93 (5.4%) / 1/94 (1.1%) of valid attempts. Combined FAIL and REVIEW also includes automatic blocks. A fair operational evaluation must measure adjudication time and actual Narrative error prevalence, because the deliberate 50% F2 fixture mix changes both workload and precision.

## Interpretation

The insomnia prevalence mutation (N28) received INVALID / REVIEW with S and FAIL / FAIL with source. Source supplied the odds ratio that the handoff omitted. The OSIRIS-REx ignition mutation (N42) received REVIEW / FAIL with S and INVALID / FAIL with source. The successful source attempt quoted the actual ignition timing. The handoff only stated when the spacecraft diverted.

Experiment 17 had three held-out F2 cases, one attempt per condition and archived briefs that had already served earlier Narrative work. The independent cases here address source overlap and add actor, causality and terminology contrasts. The paired mini-briefs still contain a single focal assertion, Synthesis usually foregrounds the fact, and source excerpts are short and mostly redundant. This tests difficult semantic distinctions in isolation with limited distraction. The fixture author assessed ground truth before any validator call and also performed the finding review. An independent second reviewer and naturally generated multi-claim briefs would broaden the evidence.

See error-analysis.md for the exact case excerpts and adjudications. For a rollout, REVIEW needs an actual subsequent evidence check. Keep production unchanged. Closely examine any false flags, recurring categories and unstable pairs before a further blinded test on longer naturally generated briefs.
