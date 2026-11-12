### Facts

- The experiment tested whether a reusable editorial-hierarchy rule in the Narrative Agent prompt kept Synthesis’s primary message, central subject, intended audience, and theme hierarchy intact.
- The candidate added a section titled “Preserve Editorial Intent from the Synthesis” to the Narrative user prompt.
- The candidate section instructed Narrative to identify the primary message, supporting themes, and contextual details; preserve audience scope and relative emphasis; keep technical details subordinate when Synthesis does; and retain uncertainty when the source is ambiguous.
- The candidate section contained no Nuëyon-specific wording or story details.
- The candidate runs changed only the Narrative user prompt; the other prompt files and workflow inputs remained identical to baseline.
- The comparison used three baseline runs and three candidate runs.
- The baseline Narrative mean score was 6.00/10; the candidate Narrative mean score was 4.00/10.
- The baseline Compose mean score was 5.67/10; the candidate Compose mean score was 3.33/10.
- All three candidate Narrative runs failed the orchestration-hierarchy criterion and the factual-restraint criterion.
- All three candidate Compose runs failed the orchestration-hierarchy criterion and the factual-restraint criterion.
- The source reports that candidate Narrative outputs made orchestration the central positioning.
- The source reports that candidate Compose outputs made orchestration or orchestration-first positioning central.
- Candidate Narrative runs introduced validation intent or commitment, an audience of founders, product leaders, or builders, or a need for coordinated AI.
- Candidate Compose outputs repeated or extended unsupported additions from the Narrative outputs.
- The source reports that candidate Compose outputs did not restore the intended editorial hierarchy.
- The source states that the candidate prompt was not sufficient to protect the supplied editorial intent.
- The decision was not to promote the candidate.
- The production Narrative user prompt was restored byte-for-byte to the baseline; the restored prompt’s SHA-256 matched the frozen baseline.
- The source says that the baseline Synthesis pairs company identity with Compose in its top-level emphasis.
- The source says that a later description in the baseline Synthesis calls orchestration “central to the product direction.”
- The experiment did not change Synthesis.
- Candidate outputs were generally rated 4/5 for readability and 4/5 for coherence; the source says the prompt change did not improve those qualities.

### Unknowns

- Whether the candidate prompt would produce the same results in other settings is not established; the source says three runs per condition show consistency in these samples, not statistical significance.
- The source does not establish the exact cause of the candidate outputs’ results. It notes that the model provider may change the backend behind the model alias, and that the baseline Synthesis’s internal tension may have reinforced the technical angle.
- The source does not establish how the candidate would perform with different inputs, models, or workflow settings.
- The source does not establish whether the observed score differences would persist beyond these six runs.