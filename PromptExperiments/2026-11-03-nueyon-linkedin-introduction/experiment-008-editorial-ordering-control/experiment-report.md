# Experiment 008 — Editorial Ordering Control

**Date:** 2026-11-04 (Europe/Amsterdam)  
**Type:** Controlled source-input ordering experiment; no production prompt, C#, schema, configuration, or architecture changes.  
**Result:** Ordering affected the selected editorial frame in the expected direction in these six runs, but Company-first (A) did not reliably preserve the requested hierarchy or factual restraint. No production change is proposed.  
**Hard-gate result for A:** 0/3 runs passed all stated gates.

## Objective and source integrity

This experiment follows the proposal recorded in Experiment 007, `experiment-007-traceability.md`, and the existing `prompts/Prompt Experiment Master.md`. I inspected Experiment 007's traceability analysis and the exact frozen reconstructed brief before preparing conditions. The live input, Experiment 006 frozen input, and this experiment's frozen copy all have SHA-256 `DB7A9CF4768684C025D02028980E16CCE8805832E1013A5B6098C00D29E8F2D0`.

The two variants are whole-section reorderings of that same source. Validation compared the preamble and each of the 14 numbered section bodies by section number. All section bodies match exactly after excluding only boundary line endings needed to keep moved headings on separate lines. No claim, caveat, uncertainty, or source text was added or omitted. The source files and both variants are preserved under `baseline/input/` and `conditions/`.

| Condition | Top-level section order | Input SHA-256 |
|---|---|---|
| A — Company-first | 1, 3, 12, 14, 5, 4, 2, 6, 7, 8, 9, 10, 11, 13 | `644E7FBB312D69CB7B9E665F92B1D554A88887D4F8B72934B51A50AF9B81AD56` |
| B — Technical-first | 4, 5, 1, 3, 14, 12, 2, 6, 7, 8, 9, 10, 11, 13 | `8D3C075E9A5D73CD003EFD5E39378784A1A353119192DB24D2222BACCE7FFB71` |

A puts company identity, intended audience, article goal, and central company message before product and technical description. B puts the AI approach and Compose before company context. The original brief's own company-first editorial boundary remains present in both conditions; B therefore tests whether ordering alone can foreground the technical material despite that retained boundary.

## Execution controls and artifacts

- Repository: `C:\Workspaces\Nueyon.Compose`, branch `main`, HEAD `be882989a7b7956e1b57199f143e7cf434fbe4e5`.
- Interleaved order: A-001, B-001, A-002, B-002, A-003, B-003.
- All six completed the full Idea → Research → Synthesis → Narrative → Compose pipeline with exit code 0. Console logs, five extracted stage outputs, timings, execution IDs, and log hashes are archived under `runs/`.
- Model/provider: OpenAI `gpt-5-mini`; .NET SDK `10.0.401`, target runtime `net10.0`; output format `LinkedInPost`. No seed or temperature override. The original workflow attachment is archived context, not a separate runtime input.
- Eleven production prompt snapshots are archived in `baseline/prompts/`; each was hash-checked against its source prompt before runs. Production prompts, schemas, source in `data/input`, app configuration, and code were not changed. The runtime copy of the input was restored and hash-verified after the runs.
- No build or tests were run, as requested. No commit or push was made.

### Run traceability and first hierarchy signal

“First stage” identifies where the run's selected editorial hierarchy is first visible. For A, this is the first stage that either departs from company-first intent or reveals a factual gate breach. For B, the technical-first framing is the intended condition; the table notes where it first appears and whether later stages preserve it.

| Run | Execution ID | Duration | First hierarchy signal / first deviation | Narrative and Compose outcome |
|---|---|---:|---|---|
| A-001 | `b1aee9ed-216b-498a-aab1-59969c2bf08e` | 144.00 s | **Idea:** selected headline makes Compose the “concrete expression” that “frames Nuëyon’s identity.” Narrative later calls the starting point “orchestration-centered.” | Identity remains mentioned, but Compose frames the company and orchestration becomes the through-line. Compose also substitutes “AI-first builders” for the source's intended audience, other AI-first companies. |
| B-001 | `29f25836-adf3-47d0-a536-df82d5c1f8cf` | 159.29 s | **Idea:** orchestration is the headline and selected principle; retained through Synthesis, Narrative, and Compose. This matches B. | Technical-first ordering is stable. Compose opens with an unsupported broad market assertion: “Most AI content workflows still treat generation as a single, one-shot prompt.” |
| A-002 | `5c93065b-1e43-4c2b-8afe-04a779abbb7c` | 155.14 s | No clear hierarchy break: Idea, Narrative, and Compose keep Nuëyon's identity first and Compose as its first product direction. First **factual** gate breach is Narrative, which invents “founders and product leads” as the article audience. | Company-first order is closest to target, but Narrative/Compose add validation, “learnable workflow,” and goal/iteration framing not established as outcomes or motivations by the source. |
| B-002 | `7c164721-1ef0-441d-9cec-8057a08a6a02` | 167.78 s | **Idea:** Compose/orchestration is selected as the tangible company vision. The title's “launch” is unsupported. B's frame remains through Compose. | Orchestration-first stays visible; source certainty is strengthened by “launch,” “implements,” and claims of a “clear foundation for future updates.” |
| A-003 | `a8cf2ece-a7b3-4ed3-a541-c290250b2fd6` | 145.35 s | **Idea:** selected idea explicitly says Compose embodies an “orchestration-first philosophy.” Narrative calls it the central idea and the company's “immediate, definitional emphasis.” | Company identity appears in the opening, but orchestration is made the company's immediate defining emphasis. Narrative also invents a “product-strategy audience”; Compose adds specific workflow steps such as locating/selecting sources that the source does not state. |
| B-003 | `46ac0152-f81b-424a-a33f-9aed1c4e8cda` | 168.41 s | **Idea:** orchestration is named the company's “core product idea,” then stays central in every stage. This matches B's ordering, but overstates the source's company-level claim. | Strong technical-first effect. The source says orchestration is central to Compose development; it does not establish orchestration as Nuëyon's core product idea. |

Exact per-run timestamps, input hashes, stage extracts, console logs, and checksums are recorded in `runs/`, `run-manifest.json`, and `execution-ids.json`.

## Scores

Scores are separate 0–2 judgments. **Factual fidelity:** 2 = claims and uncertainty stay within the brief; 1 = a limited unsupported inference/strengthening; 0 = a material unsupported audience, market, launch, validation, causal, or outcome claim. **Editorial fidelity:** A: 2 = company identity primary, Compose supporting, orchestration contextual; 1 = identity remains but Compose/technical material competes; 0 = Compose/orchestration displaces the company introduction. B: 2 = orchestration central, Compose application, company context; 1 = mixed; 0 = technical-first ordering not retained. These are editorial judgments, not automated measurements.

| Condition | Run | Factual fidelity /2 | Editorial fidelity /2 | Hard-gate notes |
|---|---:|---:|---:|---|
| A | 001 | 1 | 0 | Unsupported audience wording; Compose frames identity; orchestration becomes central. |
| A | 002 | 0 | 1 | Unsupported founders/product leads audience, validation objective, and learnability/iteration claims. |
| A | 003 | 1 | 0 | Unsupported audience and process detail; orchestration becomes the defining emphasis. |
| B | 001 | 0 | 2 | Unsupported generalization about most AI content workflows and one-shot prompts. |
| B | 002 | 0 | 2 | “Launch,” implementation certainty, and future-update foundation are unsupported. |
| B | 003 | 0 | 2 | Company-level “core product idea” claim exceeds source scope. |
| **A mean** |  | **0.67** | **0.33** | **0/3 passed all A hard gates.** |
| **B mean** |  | **0.00** | **2.00** | Technical-first hierarchy appeared in all three selected Ideas and remained in final outputs. |

The A gate fails even in A-002, whose hierarchy was closest to target, because it adds unsupported audience and validation framing. A-001 and A-003 also fail editorial hierarchy. B shows a consistent ordering effect in this small sample, but its technical-first outputs still introduce unsupported claims; observed editorial fidelity is not factual approval.

## Conclusion and recommended next step

The ordering signal is observable: technical-first input ordering yielded a technical-first selected Idea in 3/3 runs, while Company-first ordering yielded mixed results (one product-framed, one closest to company-first, and one orchestration-first). The latter condition is not robust enough for promotion or use as a factuality safeguard. All runs share the original prompts and source facts, and all outputs remain nondeterministic.

Recommended next step: keep production prompts unchanged. If more evidence is useful, run another pre-registered input-ordering replicate with the same exact variants and gates, or inspect the first point of drift in A-001/A-003 separately. Any future prompt change should be a distinct experiment and must be judged on repeated full-pipeline outputs against factual and editorial gates.

Three samples per condition are descriptive only: they do not support statistical significance or a general causal claim. The model alias may route to a changing backend; no seed/temperature override is available; Research has no separate external retrieval input; Idea exposes only its selected result in the console. Scores are qualitative, and textual lineage does not establish the model's internal causal process.

## Repository integrity

Before this experiment, `PromptExperiments/` and `prompts/` were already untracked user directories. Afterward, Git still reports only those same untracked directory roots; tracked-file diff is empty. The new files are contained in this Experiment 008 directory. Production prompt hashes match the archived snapshots, `data/input/Noeyon-Product-Strategy-reconstructed.md` remains at the frozen source hash, and the temporary runtime input copy was restored to that same hash. No existing user files were overwritten.