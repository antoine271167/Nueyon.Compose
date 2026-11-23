# Experiment 14 — Evidence-Anchored Narrative

**NO-GO.** Candidate B reduced briefs with material factual errors from 6/18 to 4/18 and improved handoff from 12/18 to 14/18. Sufficient editorial resolution stayed at 8/18. Fully sufficient regression briefs moved from 3/12 to 4/12, below the required 10/12 and below the required gain of two. The experiment offers limited support for a fidelity benefit on Webb and Wolf, with no support for the hypothesized joint improvement in fidelity and editorial resolution. The production prompt is unmodified.

## Results

Each case has three runs per arm. A/B cells show counts of passes except Material, which counts briefs with at least one material error. Full requires eight passes.

| Subject | Fidelity | Focus | Structure | Tension | Resolution | Details | Handoff | Neutrality | Full | Material |
|---|---:|---:|---:|---:|---:|---:|---:|---:|---:|---:|
| Digital | 3/1 | 3/3 | 3/3 | 3/3 | 0/0 | 3/3 | 3/3 | 3/3 | 0/0 | 0/0 |
| Webb | 0/1 | 3/3 | 3/3 | 3/3 | 3/3 | 3/3 | 0/1 | 3/3 | 0/1 | 3/2 |
| Wolf | 0/1 | 3/3 | 3/3 | 3/3 | 0/0 | 3/3 | 0/1 | 3/3 | 0/0 | 3/2 |
| Lowell | 3/3 | 3/3 | 3/3 | 3/3 | 3/3 | 3/3 | 3/3 | 3/3 | 3/3 | 0/0 |
| Cooling | 3/3 | 3/3 | 3/3 | 3/3 | 0/0 | 3/3 | 3/3 | 3/3 | 0/0 | 0/0 |
| Library | 2/3 | 3/3 | 3/3 | 3/3 | 2/2 | 3/3 | 3/3 | 3/3 | 1/2 | 0/0 |
| **All, 18 per arm** | **11/12** | **18/18** | **18/18** | **18/18** | **8/8** | **18/18** | **12/14** | **18/18** | **4/6** | **6/4** |
| Fixed regressions, 12 per arm | 6/6 | 12/12 | 12/12 | 12/12 | 6/6 | 12/12 | 6/8 | 12/12 | 3/4 | 6/4 |
| Additional cases, 6 per arm | 5/6 | 6/6 | 6/6 | 6/6 | 2/2 | 6/6 | 6/6 | 6/6 | 1/2 | 0/0 |

Cooling and Library were the unseen subjects in Experiment 13. Both were reused here, so Experiment 14 has no genuinely unseen test subjects.

| Metric | Experiment 13 A/B | Experiment 14 A/B |
|---|---:|---:|
| Fully sufficient (of 18) | 5/6 | 4/6 |
| Fully sufficient regressions (of 12) | 3/3 | 3/4 |
| Source-faithful (of 18) | 13/12 | 11/12 |
| Sufficient resolution (of 18) | 8/10 | 8/8 |
| Handoff (of 18) | 14/13 | 12/14 |
| Briefs with material errors (of 18) | 4/5 | 6/4 |

The fresh baseline itself varied from Experiment 13, including two more briefs with material errors. The concurrent A/B comparison is the primary comparison. Experiment 13 B and Experiment 14 B are distinct candidates. [Experiment 13 report](../experiment-13-fidelity-and-resolution/report.md)

## What the evidence anchoring did

- **Digital.** All endings recapped the method or moved to a five-week snapshot. The source describes suspending submissions in the second week while continuing testing, research and application processing. None of the conclusions makes that choice meaningful. Two B briefs introduced smaller shifts, one converting the delivery request into a requirement and one blurring activities within a sprint. [Deadline wording](runs/a5b80a4a53.json) · [Sprint wording](runs/2aa5874f6e.json)
- **Webb.** The six-month routine-science milestone gave all briefs a clear resolution. Five briefs still made cooling appear subsequent to completed deployment. Synthesis places cooling in month one and full deployment in week two. The original source records major temperature changes during sunshield deployment on days 3–10. Strict sequence was introduced by Narrative, with the overlap only indirectly stated upstream. One B brief used continued cooling and retained the six-month milestone. [Workable timeline](runs/104c4b228f.json) · [Conflicting chronology](runs/85a0a87da7.json)
- **Wolf.** One B brief kept the English species term, identified the scavenged material as prey, and gave the early-winter reversal. Its endpoint still returned to the general ecosystem thesis. Five briefs translated wapiti as moose, and one additionally made the victim ambiguous in the term *wolvendoding*. The source explicitly identifies wolf as the actor and elk as the prey. [Accurate development, generic conclusion](runs/8c75dd16d9.json) · [Species and actor wording](runs/fc465ed315.json)
- **Lowell.** Both arms used the late-1980s downturn to qualify the earlier renewal. All six preserved the sequence from citizen proposals through redevelopment and treated economic changes as contextual. The bounds of the 1992 account stayed available for handoff. [Bounded conclusion](runs/41a78eddd4.json)
- **Cooling.** All six gave the direction and two-degree magnitude under the revised measurement setup in Development. All six then ended with a generic comparison or a measurement caveat. The central numerical observation was available throughout and still failed to carry the ending. [Observation lost at the endpoint](runs/e0a5d63e46.json)
- **Library.** The clearest conclusions linked 52 of 80 loans on Saturday to the longer Saturday opening. B preserved the denominator across all three runs. Both arms had one endpoint that detached the opening-hour explanation from the conclusion. [Evidence-bound conclusion](runs/504a1845ea.json) · [Ambiguous denominator](runs/6a49b78745.json)

Handoff improved because two fewer briefs required material repair, one Webb timeline and one Wolf species identification. This is a brief-level score. Compose was never executed. Focus, structure, tension, detail and format-neutrality stayed sufficient in both arms. Since the anchor is internal to the unchanged contract, the study can judge how each brief uses an observation but cannot confirm which observation the model selected internally.

## Gates and verification

B failed the zero-error gate, both regression coverage thresholds, the 15/18 resolution threshold, and both required improvements over A. The no-editorial-regression, format-neutrality and complete coverage gates passed. An alternate adjudication of an ambiguous timeline or endpoint would leave several large gaps to the acceptance thresholds.

All 36 scheduled GPT-6 Luna CLI calls succeeded with the unchanged console configuration, in manifest order, with no retries. The model credential and access were verified before the first Narrative call. The first scheduled call also served as the live CLI preflight. There were no runs of the other agents. Input hashes, prompt paths, metadata, original payloads and score excerpts verified. The build succeeded with zero warnings or errors and all 303 discovered tests passed. The CLI did not expose token usage or the raw provider envelope. The elapsed time for each CLI invocation is retained.

Reviews used opaque IDs and omitted variant labels. The same reviewer ran and scored the experiment, had seen limited assignment information during setup and verification, and had no independent second judgment. Three repetitions per cell, variation in the concurrent production baseline, and reuse of all six cases limit the inference. The final commit is scoped to this directory, with its SHA recorded in Git history.

## Recommendation

Keep the production prompt and continue acceptance work on Narrative. Change the quality-assurance strategy before another prompt iteration. Prepare a genuinely unseen holdout with human-adjudicated actor, species, overlap and denominator checks plus concrete examples of what a sufficient conclusion needs to convey. Independently review disagreements before comparing variants. The recurring factual errors and flat conclusions despite two targeted prompt experiments do not support another anchor wording change or moving on to Compose.

[Prior evidence](prior-evidence.md) · [Frozen protocol](protocol.md) · [Prompt diff](prompt.diff) · [Scores](scores.csv) · [Per-failure evidence](reviews.json) · [Decision](decision.json) · [Reproduction](REPRODUCE.md)
