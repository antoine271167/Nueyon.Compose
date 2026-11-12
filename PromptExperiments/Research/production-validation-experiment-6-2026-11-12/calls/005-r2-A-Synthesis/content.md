### Central insight
In this comparison, adding a generic editorial-intent rule to the Narrative prompt did not preserve the intended message hierarchy: candidate outputs made orchestration central and failed the factual-restraint gate, so the candidate was not promoted.

### Evidence
1. The intended hierarchy put Nuëyon’s AI-first ambition for other AI-first businesses first, Nuëyon.Compose as the first concrete product direction next, and orchestration in a supporting role.
2. The candidate added a generic instruction to preserve the Synthesis’s editorial intent, audience scope, and relative emphasis. Only `Narrative.user.prompt.txt` changed; other prompt files and workflow inputs remained identical.
3. All three candidate Narrative runs and all three candidate Compose runs scored 0 on the criteria for keeping orchestration secondary and for factual/editorial restraint. The report says orchestration became central in all six candidate runs; candidate outputs also introduced unsupported framing.
4. Candidate mean scores were lower than baseline in both stages: Narrative, 4.00/10 versus 6.00/10; Compose, 3.33/10 versus 5.67/10. The decision was not to promote the candidate, and the production Narrative prompt was restored byte-for-byte.

### What the narrative should emphasize
Keep the story on this prompt intervention and its result in the tested runs: the reusable rule did not preserve the specified hierarchy, despite explicitly addressing editorial intent and emphasis. Explain Nuëyon’s intended hierarchy only as needed to make the failure legible. Describe this as an evaluation of Nuëyon’s own workflow, not as a universal prescription for editorial prompts or a claim that the rule will always fail. Readability and coherence ratings were 4/5, while source-fidelity ratings were 2/5; this may serve as one supporting detail if useful, but need not become a separate theme.

### What should be de-emphasized
Do not turn this into a full account of the workflow, prompt wording, or every unsupported addition. The specific unsupported framings and technical details of the experiment are secondary unless needed to illustrate the restraint failure. Readability and coherence are optional context, not evidence that the intended hierarchy was preserved.

### Gaps and uncertainty
There were only three runs per condition, which do not establish statistical significance or performance in other settings. The results describe these samples and do not establish that the prompt change alone caused them; the model provider may also have changed the backend behind the alias.