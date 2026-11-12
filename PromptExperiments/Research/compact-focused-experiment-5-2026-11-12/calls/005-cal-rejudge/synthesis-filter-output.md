### Facts

1. The objective was to test whether a reusable editorial-hierarchy rule in the Narrative prompt would preserve Synthesis’s primary message, audience, and theme hierarchy. The expected emphasis was Nuëyon’s AI-first ambition for other AI-first businesses, Compose as its first concrete product direction, and orchestration as supporting explanation. (“Objective and hypothesis”)
2. The candidate added a generic “Preserve Editorial Intent from the Synthesis” section to the Narrative user prompt. It contained no Nuëyon-specific wording. (“Prompt change”)
3. Only `Narrative.user.prompt.txt` changed in the candidate condition; other prompts and workflow inputs remained identical. There were three baseline runs, reused from Experiment 004, and three new candidate runs. (“Controlled comparison and configuration”)
4. The baseline Synthesis paired company identity with Compose in its top-level emphasis, but later called orchestration “central to the product direction.” The source says this internal tension may have reinforced the technical angle; it did not change the experiment’s controlled result. (“Analysis against the required criteria”)
5. All three candidate Narrative runs made orchestration or orchestration-first positioning central and did not preserve the intended audience and company-level ambition as the primary message. (“Analysis against the required criteria”)
6. All three candidate Compose runs also made orchestration central; none passed the factual/editorial-restraint gate. (“Analysis against the required criteria”)
7. The source reports unsupported additions in candidate Narrative outputs, including validation intent or commitment, an audience such as founders/product leaders/builders, or a need for coordinated AI. Candidate Compose outputs repeated or extended such additions. (“Analysis against the required criteria”)
8. Narrative scores were 7, 5, and 6/10 for baseline (reported mean 6.00) and 4, 4, and 4/10 for candidate (mean 4.00). Compose scores were 7, 4, and 6/10 for baseline (mean 5.67) and 3, 4, and 3/10 for candidate (mean 3.33). The source identifies these as subjective scores based on the stated rubric and saved outputs, not automated measurements. (“Run scores”; “Limitations”)
9. Candidate prose was rated 4/5 for readability and coherence in both stages; the source says the change did not improve those qualities. Candidate source-fidelity ratings were 2/5 for every Narrative and Compose run. (“Run scores”; “Analysis against the required criteria”)
10. The decision was not to promote: the candidate had lower mean scores in both stages, failed the hierarchy and factual-restraint gates in every run, and introduced regressions. (“Build, tests, and promotion decision”)
11. The production Narrative user prompt was restored byte-for-byte to baseline; its restored SHA-256 matched the frozen baseline. (“Build, tests, and promotion decision”)
12. Baseline runs were produced on 2026-11-03 and candidate runs on 2026-11-04. The source, prompts, commit, model alias, and workflow settings matched, but the provider may have changed the backend behind the alias. Three runs per condition show consistency in these samples, not statistical significance. (“Limitations”)

### Unknowns

- The comparison does not establish statistical significance or performance in other settings, with other prompts, or across further runs (Fact 12).
- Whether the provider actually changed the model backend between conditions is unknown (Fact 12).
- The source does not establish why candidate outputs shifted toward orchestration or added unsupported framing. The conflicting emphasis in the supplied Synthesis is identified only as a possible contributing factor (Fact 4).
- Because the supplied Synthesis itself called orchestration “central” later in its description, the experiment does not show that the candidate prompt alone caused the hierarchy failure (Facts 4–6).