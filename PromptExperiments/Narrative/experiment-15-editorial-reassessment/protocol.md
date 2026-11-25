# Experiment 15 editorial reassessment protocol

Score original Narrative content against the original source, Selected Idea and Synthesis. The source governs factual meaning. The Idea governs editorial direction and Synthesis establishes the upstream handoff. Use six subjects and three repeats per arm in each experiment. Keep the two B arms separate. Compare both A snapshots with the current production prompt before interpreting pooled A as current production evidence.

Identify eligible run slots from the two archived run manifests. Require nonempty JSON content. Hash original bytes and retain one observation per byte-identical group. Describe any exclusions and missing references. Assign opaque IDs and show only those IDs, original narrative text and subject-level references during review. Freeze recorded scores before joining the mapping and previous scores. This is a single-reviewer study with partial blinding.

| Criterion | Weight | What earns a 3 |
|---|---:|---|
| A Story direction | 25% | Coherent angle aligned with the selected idea |
| B Story development | 25% | Logical progression that Compose can follow |
| C Compose usability | 25% | Sufficient direction and substance to draft without filling factual gaps |
| D Editorial resolution | 15% | Supported takeaway that follows from the development |
| E Evidence and details | 10% | Enough specific, relevant evidence to carry the story |

Use integer ratings from 1 (unusable) through 2 (substantial rework), 3 (acceptable), 4 (strong) and 5 (distinctive and compelling). Judge C on editorial usability of the blueprint, including genuine gaps in evidence or direction. Record factual defects separately so the editorial-only rate can measure the potential of the structure after factual repair. A generic but supported endpoint can earn 3 if the preceding material supplies the ending. Publication polish is outside the rubric.

Compute the weighted total in integer hundredths as 25A + 25B + 25C + 15D + 10E. The editorial-only gate is total at least 350 and A, B and C each at least 3. Individual acceptance adds zero unresolved F2 errors. Use the highest severity per output. F0 is faithful, F1 is limited imprecision without material change and F2 changes an important fact, entity, actor, timeline, quantity, causal relation or outcome. Record origin per issue as upstream, introduced or amplified. Review all factual flags and borderline editorial cases a second time, including high-scoring F2s. Retain revisions with rationale if a judgment changes.

Use valid distinct production outputs as denominator for both rates. GO requires at least 80% individual acceptance, sufficient subject coverage, usable handoffs and no unresolved recurring F2 pattern. CONDITIONAL GO requires at least 80% editorial-only passes and remaining F2s that are bounded and can be addressed by targeted source correction before publication. Repeated failures that require substantial source repair, recurring structural failures or an editorial-only rate below 80% imply NO-GO. If the individual rate is below 80%, only the explicit bounded-risk exception for CONDITIONAL GO can override NO-GO. Apply these rules without rounding. Evaluate arms descriptively and compare previous binary full passes only after scoring. No generation or Compose runs.
