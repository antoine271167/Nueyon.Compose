### Facts

1. The experiment tested whether a reusable editorial-hierarchy rule in the Narrative Agent prompt would preserve Synthesis’s primary message, central subject, intended audience, and theme hierarchy.
2. The expected result was a company introduction centered on Nuëyon’s AI-first ambition for other AI-first businesses.
3. The expected result presented Nuëyon.Compose as the first concrete product direction.
4. The expected result treated orchestration as supporting explanation.
5. The candidate added a generic section titled “Preserve Editorial Intent from the Synthesis” to the Narrative user prompt.
6. The candidate instruction told Narrative to identify the primary message, supporting themes, and contextual details.
7. The candidate instruction told Narrative to preserve audience scope and relative emphasis.
8. The candidate instruction told Narrative to keep technical details subordinate when Synthesis does.
9. The candidate instruction told Narrative to retain uncertainty when the source is ambiguous.
10. The candidate changed only `Narrative.user.prompt.txt`; all other prompt files and workflow inputs remained identical.
11. The baseline used three completed runs from Experiment 004, dated 2026-11-03.
12. The candidate used three new runs dated 2026-11-04.
13. Both conditions used the same commit, source input, prompt snapshots apart from the candidate change, model alias, workflow settings, and LinkedIn post format.
14. The model used was OpenAI / `gpt-5-mini`.
15. Temperature and seed were not explicitly configured.
16. Rubric criterion C scored whether orchestration stayed a supporting explanation; a score of 0 meant it became the central positioning.
17. Rubric criterion D scored factual/editorial restraint; a score of 0 meant a clear unsupported claim or expansion appeared.
18. Candidate Narrative scores averaged 4.00/10; baseline Narrative scores averaged 6.00/10.
19. Candidate Compose scores averaged 3.33/10; baseline Compose scores averaged 5.67/10.
20. All three candidate Narrative runs scored 0 on criterion C.
21. All three candidate Narrative runs scored 0 on criterion D.
22. All three candidate Compose runs scored 0 on criterion C.
23. All three candidate Compose runs scored 0 on criterion D.
24. The report states that all three candidate Narrative runs made orchestration/orchestration-first positioning central.
25. The report states that all three candidate Compose runs made orchestration/orchestration-first positioning central.
26. Candidate Narrative outputs introduced validation intent or commitment, audience framing involving founders, product leaders, or builders, or a need for coordinated AI.
27. Candidate Compose outputs repeated or extended unsupported additions from the candidate Narrative outputs.
28. Candidate outputs were rated 4/5 for readability and 4/5 for coherence in both stages.
29. Candidate outputs received source-fidelity ratings of 2/5 in both stages.
30. The report states that candidate outputs did not restore the intended editorial hierarchy.
31. The report’s decision was not to promote the candidate.
32. The production Narrative user prompt was restored byte-for-byte to the baseline snapshot.
33. The restored production prompt’s SHA-256 matched the frozen baseline hash.
34. Three runs per condition show consistency in these samples, not statistical significance.
35. The source, prompts, commit, model alias, and workflow settings matched between conditions, but the model provider may change the backend behind an alias.
36. The report states that Idea and Research outputs vary between runs in the full-pipeline comparison.

### Unknowns

- Whether the candidate rule would preserve the intended hierarchy in settings beyond these six runs.
- Whether the observed score differences are statistically significant; the report explicitly says the three runs per condition do not establish statistical significance.
- The extent, if any, to which a change in the model provider’s backend affected the comparison.
- The separate causal contribution of the prompt change to the observed outputs, beyond the report’s assessment of these sampled runs.
- Whether the candidate rule would perform differently with other model settings; temperature and seed were not explicitly configured.