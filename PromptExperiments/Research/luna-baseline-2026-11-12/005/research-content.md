### Facts

1. The experiment was titled “Experiment 005 — Editorial Intent Fidelity.”
2. The experiment tested whether a reusable editorial-hierarchy rule in the Narrative Agent prompt would preserve Synthesis’s primary message, central subject, intended audience, and theme hierarchy.
3. The expected result was a company introduction centered on Nuëyon’s AI-first ambition for other AI-first businesses.
4. The expected result placed Nuëyon.Compose as the first concrete product direction.
5. The expected result placed orchestration as supporting explanation.
6. The candidate added a generic section titled “Preserve Editorial Intent from the Synthesis” to the Narrative user prompt.
7. The candidate section instructed Narrative to identify the primary message, supporting themes, and contextual details; preserve audience scope and relative emphasis; keep technical details subordinate when Synthesis does; and retain uncertainty when the source is ambiguous.
8. The candidate section contained no Nuëyon-specific wording or story details.
9. The candidate condition changed only `Narrative.user.prompt.txt`; the other prompt files and workflow inputs remained identical to baseline.
10. The baseline condition consisted of three completed runs reused from Experiment 004.
11. The candidate condition consisted of three new runs.
12. Both conditions used `gpt-5-mini`, the same source input, and the same commit.
13. The source reports that all three candidate Narrative runs made orchestration or orchestration-first positioning central.
14. The source reports that candidate Narrative outputs did not preserve the intended audience and company-level ambition as the primary message.
15. The source reports that candidate Narrative outputs introduced unsupported validation intent or commitment, an audience of founders, product leaders, or builders, or a need for coordinated AI.
16. The candidate Narrative runs scored 4, 4, and 4 out of 10; their reported mean was 4.00/10.
17. The baseline Narrative runs scored 7, 5, and 6 out of 10; their reported mean was 6.00/10.
18. The source reports that all three candidate Compose runs made orchestration or orchestration-first positioning central.
19. The source reports that no candidate Compose run passed the factual-restraint gate.
20. Candidate Compose runs scored 3, 4, and 3 out of 10; their reported mean was 3.33/10.
21. Baseline Compose runs scored 7, 4, and 6 out of 10; their reported mean was 5.67/10.
22. The source reports that candidate Compose outputs repeated or extended unsupported additions present in the candidate Narrative outputs.
23. Candidate outputs were rated 4/5 for readability and 4/5 for coherence in both Narrative and Compose.
24. Candidate source-fidelity ratings were 2/5 for all Narrative and Compose runs.
25. The source reports that the candidate prompt did not improve readability or coherence.
26. The decision was not to promote the candidate.
27. The source states that the candidate was not promoted because its mean score was lower for both Narrative and Compose, it failed the orchestration-hierarchy and factual-restraint gates in every run, and it introduced regressions.
28. The production Narrative user prompt was restored byte-for-byte to the baseline.
29. The restored production prompt’s SHA-256 matched the frozen baseline exactly.
30. The source states that the three runs per condition show consistency in these samples, not statistical significance.

### Interpretations

- None.

### Unknowns

- Whether the candidate prompt would produce the same results in other settings or with other runs is not established.
- Statistical significance of the observed score differences is not established.
- The source does not establish why the candidate outputs made orchestration central or introduced unsupported framing.
- The source does not establish whether a different prompt change would preserve the intended hierarchy.
- The source does not establish whether the model provider changed the backend behind the model alias between the baseline and candidate dates; it states only that this may have happened.
- The source does not state an explicit general lesson beyond the reported experiment result and decision.

### Development sequence

1. Three baseline runs were produced on 2026-11-03.
2. On 2026-11-04, three candidate runs were produced with only the Narrative user prompt changed.
3. The candidate Narrative and Compose outputs were scored against the stated criteria.
4. The candidate received a lower reported mean score than baseline for both Narrative and Compose.
5. The candidate failed the orchestration-hierarchy and factual-restraint gates in every run.
6. The decision was not to promote the candidate.
7. The production Narrative user prompt was restored byte-for-byte to baseline.

### Editorial relevance

- Facts 2–9 identify the tested intervention, its intended target, and the controlled prompt change.
- Facts 13–21 document the reported hierarchy results and Narrative and Compose scores.
- Facts 22–25 document unsupported additions and the separate readability, coherence, and source-fidelity ratings.
- Facts 26–29 document the decision and restoration.
- Fact 30 and the first two Unknowns delimit what the three-run comparison establishes.