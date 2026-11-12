### Facts

1. The experiment tested whether a reusable editorial-hierarchy rule in the Narrative Agent prompt would preserve Synthesis’s primary message, central subject, intended audience, and theme hierarchy.
2. The expected result was a company introduction centered on Nuëyon’s AI-first ambition for other AI-first businesses.
3. The expected result presented Nuëyon.Compose as the first concrete product direction.
4. The expected result treated orchestration as supporting explanation.
5. The candidate added a generic prompt section titled “Preserve Editorial Intent from the Synthesis.”
6. The added section instructed Narrative to identify the primary message, supporting themes, and contextual details.
7. The added section instructed Narrative to preserve audience scope and relative emphasis, keep technical details subordinate when Synthesis did so, and retain uncertainty when the source was ambiguous.
8. The candidate condition changed only `Narrative.user.prompt.txt`; the other prompt files and workflow inputs remained identical to the baseline condition.
9. The baseline consisted of three completed runs reused from Experiment 004.
10. The candidate consisted of three new runs.
11. The baseline Narrative mean score was 6.00/10.
12. The candidate Narrative mean score was 4.00/10.
13. The baseline Compose mean score was 5.67/10.
14. The candidate Compose mean score was 3.33/10.
15. The report states that the candidate had a lower mean score than the baseline for both Narrative and Compose.
16. The Narrative rubric’s criterion C scored orchestration as supporting explanation when it remained subordinate, and scored it zero when it became the central positioning.
17. The Narrative rubric’s criterion D assessed whether unsupported audience, need, benefit, capability, traction, launch status, or commitment was added; the report identifies D as a hard promotion gate.
18. All three candidate Narrative runs scored zero on criterion C.
19. All three candidate Narrative runs scored zero on criterion D.
20. All three candidate Compose runs scored zero on criterion C.
21. All three candidate Compose runs scored zero on criterion D.
22. The report says the candidate Narrative runs made orchestration the central positioning.
23. The report says the candidate Compose runs made orchestration or orchestration-first positioning central.
24. The report says candidate Narrative outputs did not preserve the intended audience and company-level ambition as the primary message.
25. The report says candidate Compose outputs omitted or diluted the audience scope.
26. The report says candidate Narrative outputs introduced validation intent or commitment, an audience of founders, product leaders, or builders, or a need for coordinated AI.
27. The report says candidate Compose outputs repeated or extended unsupported additions.
28. Candidate Narrative readability and coherence were each rated 4/5 in all three runs.
29. Candidate Compose readability and coherence were each rated 4/5 in all three runs.
30. The report says candidate Compose outputs remained readable but were materially less suitable for an introductory LinkedIn post because the company-level message was displaced by technical exposition.
31. The report says Compose run 1 included the editorial meta-text “The post’s purpose is simple” and a claim that the workflow would be validated before expansion.
32. The report says Compose run 2 generalized about what useful AI requires and named founders and product leaders.
33. The report says Compose run 3 reframed content creation as an orchestration problem and expanded to architecture tradeoffs and validation focus.
34. The report says the baseline Synthesis pairs company identity with Compose in its top-level emphasis.
35. The report says a later description in the baseline Synthesis calls orchestration “central to the product direction.”
36. The report states that the candidate prompt was not sufficient to protect the supplied editorial intent.
37. The decision was not to promote the candidate.
38. The production Narrative user prompt was restored byte-for-byte to the baseline.
39. The restored production prompt’s SHA-256 matched the frozen baseline prompt’s SHA-256.
40. The report states that three runs per condition show consistency in those samples, not statistical significance.
41. The report says the provider may change the backend behind the model alias.
42. Temperature and seed were not explicitly configured.
43. The report identifies the subjective scores as based on saved outputs and the stated rubric.

### Interpretations

- None.

### Unknowns

- Whether the candidate prompt change alone caused the differences in scores or output content is not established.
- The source does not establish whether the score differences are statistically significant or whether the results generalize to other settings; it explicitly limits the three runs per condition to consistency in those samples.
- The source does not establish whether a change in the provider’s backend behind the model alias affected the results.
- The source does not establish why the candidate outputs made orchestration central or introduced the unsupported framing described in the report.
- The source says the Synthesis text contained an internal tension and that this “may have reinforced” the technical angle; whether it did so is not established.

### Development sequence

- The experiment’s expected result specified a company introduction centered on Nuëyon’s AI-first ambition for other AI-first businesses, with Compose as the first concrete product direction and orchestration as supporting explanation (Facts 2–4).
- The candidate added a generic editorial-intent section to the Narrative user prompt (Facts 5–7).
- The candidate condition ran three times; the baseline condition reused three runs (Facts 9–10).
- Candidate Narrative and Compose runs received lower mean scores than their respective baseline means (Facts 11–15).
- All candidate Narrative and Compose runs failed the reported orchestration-hierarchy and factual-restraint gates (Facts 18–21).
- The candidate was not promoted, and the production prompt was restored byte-for-byte to baseline (Facts 37–39).
- Whether the prompt change alone caused the observed output differences: UNKNOWN.

### Editorial relevance

- Facts 1–8 are relevant to the tested intervention and intended hierarchy.
- Facts 11–21 are relevant to the reported scores and gate outcomes.
- Facts 22–27 and 30–33 are relevant to the reported candidate-output content and evaluations.
- Facts 36–39 are relevant to the stated finding and decision.
- Facts 40–43 and the Unknowns are relevant to the limits of the comparison.