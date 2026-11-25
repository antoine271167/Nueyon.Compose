# Experiment 16 — Semantic Fidelity

**NO-GO.** B passed editorial and format-neutrality gates and had zero material errors on the four holdouts. It failed both mandatory regression gates. Five of 12 B regression outputs had a material error, versus four of 12 for A. The production prompt remains in use and Narrative freeze is deferred.

The only candidate change is one paragraph in the Narrative user prompt. It instructs Narrative to preserve referents and factual relationships while paraphrasing, retain an original term when translation could change its identity, and avoid claiming timing or relationships beyond the handoff. The system prompt, input contract and application code were identical across arms. [Prompt diff](prompt.diff) · [Hypothesis](hypothesis.md)

| Subject | A F0/F1/F2 | B F0/F1/F2 | A editorial mean | B editorial mean |
|---|---:|---:|---:|---:|
| Wolf | 1/0/2 | 0/0/3 | 4.03 | 3.98 |
| Webb | 0/1/2 | 0/1/2 | 4.07 | 4.10 |
| Digital | 2/1/0 | 1/2/0 | 3.82 | 3.85 |
| Lowell | 3/0/0 | 2/1/0 | 4.43 | 4.00 |
| **Regression** | **6/2/4** | **3/4/5** | **4.09** | **3.98** |
| FAA hot spots | 2/0/1 | 3/0/0 | 4.00 | 3.95 |
| U.S. suffrage | 1/2/0 | 3/0/0 | 4.10 | 4.10 |
| NIST face recognition | 3/0/0 | 2/1/0 | 4.00 | 4.00 |
| EIA renewables | 2/0/1 | 3/0/0 | 3.88 | 3.92 |
| **Holdout** | **8/2/2** | **11/1/0** | **4.00** | **3.99** |
| **Overall** | **14/4/6** | **14/5/5** | **4.04** | **3.99** |

All 24 outputs per arm passed the editorial rubric, including all three outputs per subject per arm. The weighted mean fell by 0.054 points, within the frozen 0.15 margin. Direction, development and usability means were 4.000, 4.083 and 4.083 in A and 4.000 on all three in B. Lowell had the largest subject-level editorial drop, driven by less developed brief content in B. B retained format neutrality and introduced no newly recurring fidelity category under the frozen definition. [Per-output scores](scores.csv) · [Editorial rubric scores](editorial-scores.csv) · [Gate results](decision.json)

- **Wolf.** All three B briefs translated the original elk to Dutch eland or a compound derived from it, changing the species to moose. One A brief retained the original English elk. One B brief also used ambiguous wolvendoden, where the surrounding text made the actor recoverable.
- **Webb.** Two B briefs put cooling after full deployment with variants of “Daarna volgden weken van afkoeling.” NASA describes major temperature changes during sunshield deployment. The frozen Synthesis supplies the first-month cooling window and second-week deployment completion, without the explicit overlap. Narrative added a strict temporal relationship.
- **Holdouts.** B retained the FAA's wrong surface label, where one A brief narrowed it to “verkeerd baangebruik.” B also avoided the erroneous Dutch “quadriljoen” for the EIA's US English “quadrillion.” One B NIST endpoint broadened the observed relationship beyond false positives in one-to-one matching, rated minor because the lead correctly scoped the comparison.

[Exact excerpts and source passages](fidelity-findings.md) · [Second review](second-review.md)

All 48 scheduled GPT-6 Luna Narrative-only processes succeeded on their first recorded attempt. The original CLI payloads, execution metadata and frozen hashes were verified. The live preflight smoke run is archived separately. The solution built with zero errors or warnings. All 303 discovered tests passed. The Domain test assembly contained no discovered tests. The initial Git base was 1e7f183911115146e62244858c7f24e242597347. The experiment commit is the main-branch commit adding this report. Retrieve its full SHA with `git log -1 --format=%H -- PromptExperiments/Narrative/experiment-16-semantic-fidelity/report.md`. [Preflight](preflight.md) · [Reproduction](REPRODUCE.md)

The four holdouts were selected after the candidate draft. Their hand-authored Synthesis fixtures preserve curated public evidence, and they may make the relevant relationships easier to recover than production upstream outputs. Three samples per subject and a same-reviewer second pass with partial blinding limit the strength of the holdout comparison. The frozen protocol contains two question marks where the greater-than-or-equal symbols were lost to encoding. The assignment and evaluation both used the specified editorial cutoffs. [Provenance](holdout-provenance.md) · [Protocol](protocol.md)

A further generic prompt expansion is unsupported by these results. Before formally accepting Narrative or moving to Compose, test a focused semantic validation step for translated referents and implied ordering. Such a check should flag unsupported chronology when the Synthesis gives broad time windows. Confirming the actual overlap requires evidence that preserves it from the primary source.

