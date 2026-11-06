# Experiment 006 — Synthesis Editorial Hierarchy

**Date:** 2026-11-04  
**Decision:** **REGRESSION — not promoted.** Production `Synthesis.user.prompt.txt` was restored byte-for-byte from the frozen baseline and verified by SHA-256.

## Executive summary

Experiment 005 suggested that the Synthesis output itself contained competing editorial signals: it correctly introduced Nuëyon's company identity and Compose as the first product direction, but also called orchestration “central to the product direction.” Experiment 006 tested whether a clearer hierarchy in Synthesis would prevent that technical framing from taking over downstream.

The candidate made the requested labels explicit in 3/3 Synthesis outputs. This improved structural consistency, but did not reliably establish the intended hierarchy or preserve it downstream. Orchestration still became the central theme in all three Narratives and all three Compose posts. One run introduced unsupported audience groups and claims about testability, predictability, repeatability, and validation. Mean Narrative score declined from 6.00 to 4.33; mean Compose score declined from 5.67 to 4.33. The candidate failed the hierarchy gate and factual-restraint gate, so it was rejected.

## Repository, inputs, and execution

- Checkout: `C:\Workspaces\Nueyon.Compose`; branch `main`; HEAD `be882989a7b7956e1b57199f143e7cf434fbe4e5`.
- Before the experiment, tracked files were clean. Pre-existing untracked folders `PromptExperiments/` and `prompts/` were retained; no commits or pushes were made.
- Source brief SHA-256: `DB7A9CF4768684C025D02028980E16CCE8805832E1013A5B6098C00D29E8F2D0`; it matches the frozen Experiment 005 input.
- Archived original workflow instruction SHA-256: `9711363973E35E3FFE440CDA2BE73C0A77101DFFC2F21B77CA6B957B725A5AB`. The application has no separate instruction-file argument; it reads Markdown files under `data/input`. The instruction is preserved as experiment context and was not an additional runtime input.
- Model/provider: OpenAI `gpt-5-mini`; .NET SDK `10.0.401`; LinkedIn format: `LinkedInPost`. No temperature or seed override is configured.
- Execution: `dotnet Nueyon.Compose.Host.Console.dll` from `src/Nueyon.Compose.Host.Console/bin/Debug/net10.0`.
- Baseline: reused the three completed Experiment 005 baseline runs. Their input, model alias, runtime path, production Synthesis prompts, Narrative and Compose prompts match this experiment's baseline condition. This avoids unnecessary repeat calls while providing three full-pipeline baseline samples.
- Candidate: three fresh full-pipeline runs. Only `Synthesis.user.prompt.txt` changed; the system prompt, other agent prompts, C#, schemas, configuration, input, and runtime remained unchanged. All three runs completed Idea, Research, Synthesis, Narrative, and Compose successfully.

## Hypothesis and change

**Hypothesis:** If Synthesis explicitly separates the Primary message and Primary subject from Supporting themes, Contextual details, and Explicit exclusions, Narrative and Compose will keep Nuëyon's company positioning primary, treat Compose as the first concrete supporting direction, and leave orchestration as technical context.

The candidate replaced the Synthesis output section labels with exactly these five headings and generic definitions. It did not add Nuëyon-specific words or alter the configured JSON schema (`SynthesisResult.Content`). The diff is preserved in `prompt-changes/Synthesis.user.prompt.diff`.

## Evaluation rubric

The 0–2 criteria follow Experiment 005. A: company identity and audience primary; B: Compose is the first concrete direction; C: orchestration is supporting context; D: factual/editorial restraint (a hard gate); E: suitable, focused company-introduction foundation for Narrative or publishable LinkedIn post for Compose. Total: 10. Readability, coherence, and source fidelity were also considered qualitatively.

### Narrative and Compose scores

| Condition | Run | A | B | C | D | E | Total / 10 |
|---|---:|---:|---:|---:|---:|---:|---:|
| Baseline Narrative | 1 | 2 | 2 | 1 | 0 | 2 | 7 |
| Baseline Narrative | 2 | 2 | 2 | 0 | 0 | 1 | 5 |
| Baseline Narrative | 3 | 2 | 2 | 0 | 1 | 1 | 6 |
| Candidate Narrative | 1 | 1 | 2 | 0 | 0 | 1 | 4 |
| Candidate Narrative | 2 | 2 | 2 | 0 | 0 | 1 | 5 |
| Candidate Narrative | 3 | 0 | 2 | 0 | 1 | 1 | 4 |
| Baseline Compose | 1 | 2 | 2 | 1 | 0 | 2 | 7 |
| Baseline Compose | 2 | 1 | 2 | 0 | 0 | 1 | 4 |
| Baseline Compose | 3 | 2 | 2 | 0 | 1 | 1 | 6 |
| Candidate Compose | 1 | 1 | 2 | 0 | 1 | 1 | 5 |
| Candidate Compose | 2 | 2 | 2 | 0 | 0 | 1 | 5 |
| Candidate Compose | 3 | 0 | 2 | 0 | 0 | 1 | 3 |

| Stage | Baseline mean | Candidate mean | Change |
|---|---:|---:|---:|
| Narrative | 6.00 | 4.33 | -1.67 |
| Compose | 5.67 | 4.33 | -1.34 |

Candidate prose readability and coherence were usually around 4/5, similar to baseline. That did not compensate for its persistent hierarchy failure and lower source fidelity.

## Findings

- **Synthesis format:** The five requested labels appeared in all three candidate syntheses. However, the actual hierarchy still varied: one primary message omitted the intended audience, another included it, and another centered Compose/orchestration. Supporting themes and contextual details sometimes remained lengthy and technically dominant. Labels alone did not resolve prioritization.
- **Narrative hierarchy:** All three candidate Narratives elevated orchestration into the central direction. Run 3 made Compose itself an “orchestration-first technical direction” and reframed the selected story around it. None maintained the intended company-level message with orchestration only as a supporting explanation.
- **Compose hierarchy:** All three candidate posts centered orchestration or staged coordination. Candidate Run 1 described Compose as an “orchestration-first workflow”; Run 2 called coordination the “central design choice” and repeated that the approach would be validated; Run 3 opened with a broad statement that useful AI applications need more than one LLM call and concluded with a priority to orchestrate capabilities.
- **Factual restraint:** Candidate Run 2 added “founders, product leaders, and AI product builders” as audience groups, and asserted the workflow would be predictable and repeatable, testable, and validated. Those additions are not established outcomes or audience segments in the source. The candidate therefore failed the factual-restraint gate.
- **Company identity/audience:** Company identity remained present in two candidate Narrative runs, but the intended audience was either omitted from the primary message or expanded beyond source evidence. Run 3 made Compose/orchestration primary and displaced company identity.
- **Compose first direction:** This aspect was retained in all three candidates, but it did not offset the hierarchy and factual regressions.
- **LinkedIn suitability:** Outputs were readable, but the technical opening and repeated orchestration framing did not make a focused company introduction. Candidate Run 3's Compose output also added `#AI #Orchestration`, reinforcing the technical framing.
- **Baseline comparison:** The three baseline Narratives and Composes also often emphasized orchestration. The candidate did not fix that known weakness; in these samples it further reduced mean scores. The baseline's defects are not evidence that the candidate improved them.

## Build, tests, and rollback

- Candidate build: `dotnet build src/Nueyon.Compose.slnx` passed with 0 warnings and 0 errors.
- Candidate was not promoted. Its prompt snapshot and outputs remain in the experiment artifacts.
- Production Synthesis user prompt was restored from the byte snapshot. Baseline and production hashes both equal `5D9556B075425A589F4CC92BA1D34A0385FAECC458FE55AA8EB3D3124933BF5D`.
- Restored build: passed with 0 warnings and 0 errors.
- Restored tests: 165 passed, 0 failed, 0 skipped (Application 138, Infrastructure 21, Console 6). Domain test assembly had no discoverable tests.
- `git diff` is empty after restoration. No tracked files outside the authorized prompt were changed; the pre-existing untracked folders remain untouched except for this experiment's new artifacts under `PromptExperiments/`.

## Limitations

- Three baseline outputs were reused from Experiment 005 and were not rerun. They were verified as the same production-prompt condition, but baseline and candidate calls were not interleaved.
- The model has no configured seed or temperature override. Three samples are descriptive, not statistically conclusive; the provider may change the backend behind the model alias.
- The console exposes only the selected first Idea, not all Idea candidates. Research has no configured external retrieval provider.
- Scores are editorial judgments. Full logs and extracted outputs are retained for independent review.

## Artifact index

- `baseline/input/` — frozen source and archived workflow instruction.
- `baseline/prompts/Synthesis/` — exact baseline prompts.
- `baseline/outputs/` — three reused complete baseline logs and extracted Synthesis, Narrative, and Compose text.
- `candidate/prompts/Synthesis/` — exact candidate and unchanged system prompt.
- `candidate/outputs/` — three complete candidate logs and extracted outputs for all evaluated stages.
- `prompt-changes/` — candidate diff, candidate prompt, and rollback snapshot.
- `metadata.json` — hashes, runtime conditions, run identifiers, scores, build/test results, and limitations.
