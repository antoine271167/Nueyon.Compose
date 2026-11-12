### Facts
1. The experiment aimed to preserve a hierarchy in which Nuëyon’s AI-first ambition for other AI-first businesses was primary, Compose was its first concrete product direction, and orchestration was supporting explanation (“Objective and hypothesis”).
2. The candidate added a generic “Preserve Editorial Intent from the Synthesis” section to the Narrative user prompt. The report says this was the only prompt change; source input, commit, model alias, and workflow inputs otherwise matched the baseline (“Prompt change”; “Controlled comparison and configuration”).
3. Under the stated 0–10 rubric, candidate mean scores were lower than baseline for Narrative (4.00 vs. 6.00) and Compose (3.33 vs. 5.67). The report characterizes these as scores of the saved outputs under its rubric (“Run scores”).
4. All three candidate Narrative runs and all three candidate Compose runs failed criterion C, keeping orchestration subordinate, and criterion D, factual/editorial restraint. D was identified as a hard promotion gate. The report says candidate outputs made orchestration central and added unsupported framing, including validation intent or commitment, audience labels, or a need for coordinated AI (“Analysis against the required criteria”; “Run scores”).
5. The candidate did retain Compose as the first concrete step in all Narrative runs and Compose runs 1–2; Compose run 3 implied its role without clearly identifying it as the first step (“Analysis against the required criteria”).
6. Both conditions were generally rated readable and coherent (4/5); the report says the change did not improve these qualities. It judged candidate Compose outputs less suitable as introductory LinkedIn posts because technical exposition displaced the company-level message (“Run scores”; “Analysis against the required criteria”).
7. The decision was not to promote the candidate. The production prompt was restored byte-for-byte; its SHA-256 matched the frozen baseline (“Decision”; “Rollback verification”).
8. The report notes a possible tension in baseline Synthesis: it pairs company identity with Compose at top level but later calls orchestration “central to the product direction.” Synthesis was not changed in this experiment (“Analysis against the required criteria”).

### Interpretations
- In these sampled runs, the generic instruction did not protect the intended hierarchy, despite preserving some product-direction content (Facts 3–5). This is an interpretation of the reported results, not evidence that the rule fails universally.

### Unknowns
- The report states that three runs per condition do not establish statistical significance or performance in other settings (“Limitations”).
- The comparison does not establish that the prompt change alone caused the score differences: baseline runs were from Nov. 3 and candidate runs from Nov. 4, and the provider could change the backend behind the model alias. No temperature or seed override was configured (“Limitations”).
- Ratings are described as subjective judgments against the experiment’s rubric, not independent or automated measurements. The source reports the evaluation; it does not provide independent validation of the scores (“Scoring rubric”; “Limitations”).
- The baseline Synthesis’s competing emphasis on orchestration may have reinforced the technical angle, but its contribution was not tested because Synthesis was outside the change scope (Fact 8).

### Development sequence
1. The team reused three prior baseline runs and made three candidate runs with the Narrative user-prompt change (Fact 2).
2. Candidate outputs scored lower on average in both stages and every candidate run failed the hierarchy and restraint gates (Facts 3–4).
3. The candidate was not promoted and the production prompt was restored to the baseline bytes (Fact 7).

### Editorial relevance
Facts 1, 3–7; Interpretation 1; Unknowns 1–4.