# Nuëyon LinkedIn introduction — prompt experiment report

## Executive Summary

- **Objective:** Develop a compelling, factually grounded LinkedIn introduction to Nuëyon, using the confirmed reconstructed brief as the intended source input.
- **Overall result:** Four prompt-change candidates were evaluated across two quality problems. None was safe to promote; **no production prompt was promoted**.
- **Main finding:** The baseline Narrative and Synthesis prompts both steered company introductions toward an orchestration story. Three trials did not resolve that focus problem without factual regressions. A separate three-versus-three run test of a Compose opening/ending change also showed no consistent improvement, and one candidate sample added unsupported audience categories.
- **Iterations:** 3 for the story-focus problem (the maximum), plus one separate prompt change tested across three baseline and three candidate runs for opening and ending quality.
- **Production prompt improvement:** None. The original Narrative, Synthesis, and LinkedIn Compose prompts were restored exactly to their frozen baseline versions.

## Repository and Execution

- **Repository commit:** `main`, `be882989a7b7956e1b57199f143e7cf434fbe4e5`.
- **Execution command:** `dotnet build src/Nueyon.Compose.slnx`, followed by `dotnet Nueyon.Compose.Host.Console.dll` from `src/Nueyon.Compose.Host.Console/bin/Debug/net10.0`.
- **Tests:** `dotnet test src/Nueyon.Compose.slnx --no-build`.
- **Provider/model:** OpenAI / `gpt-5-mini`. API key was available through user secrets; its value was never recorded. No temperature or seed override is configured.
- **Runtime:** .NET SDK 10.0.401.
- **Actual pipeline:** Idea returns candidates; a deterministic executor selects the first; then Research → Synthesis → Narrative → Compose. Compose is configured for `LinkedInPost`.
- **Input:** `data/input/Noeyon-Product-Strategy-reconstructed.md`; exact source and original workflow attachment are frozen in each experiment input directory.
- **Prompt loading:** 11 `.prompt.txt` files are embedded by the Application project. User prompts are rendered with named placeholders by `PromptLoader`.
- **Research limitation:** No external retrieval tool or separate research source was configured in the inspected agent wiring. Research outputs draw from the supplied source brief.
- **Capture limitation:** The console prints the selected first Idea, not every Idea candidate. It prints complete Research, Synthesis, Narrative, and Compose text. Information logs provide per-agent durations and errors.
- **Generation variability:** The model produced different Idea and Research selections across runs, and no deterministic seed is configured. Each condition was run once, so stylistic differences have limited statistical confidence.
- **Interrupted attempt:** The first baseline attempt was interrupted during Idea. Its partial log is retained; the retry completed all stages and is the baseline used for evaluation.

## Baseline Assessment

| Agent/stage | Rating | Assessment |
|---|---:|---|
| Idea | 4/5 | The selected idea was relevant and tied Nuëyon’s identity to Compose as its first concrete direction. Only the selected first idea is observable in console output. |
| Research | 4/5 | It carefully separated facts, interpretations, unknowns, and development sequence, with no external sourcing. It was much more exhaustive than this company introduction required. |
| Synthesis | 3/5 | It consolidated useful evidence and preserved early-stage limitations, but made orchestration the central insight and described the reader as “AI-first founders and product leaders who care about predictable, research-grounded content workflows,” which the source brief does not establish. |
| Narrative | 2/5 | It was coherent and retained cautionary constraints, but shifted the company introduction toward an orchestration story. It also introduced “predictable” workflows and “product leaders” as audience framing. The fixed prompt instruction about an agent-to-orchestrator transition directly matched this emphasis. |
| Compose | 3/5 | The post had readable paragraphs and an early-stage caveat, but opened with a generic company statement, repeated the orchestration point in its ending, and gave technical process detail more space than the company vision. |

## Experiment History

### Experiment 001 — Remove the fixed Narrative storyline

- **Problem:** Narrative overemphasized an agent-to-orchestrator story that did not match the company-introduction scope.
- **Hypothesis:** Removing the fixed story and deriving details/endpoints from current inputs would refocus Narrative and Compose.
- **Target:** `Narrative/Narrative.user.prompt.txt`.
- **Change:** Removed the hard-coded transition, its examples, and its prescribed orchestration endpoint; replaced them with input-driven guidance.
- **Evaluation:** The candidate made the company identity more explicit, but still centered the post on orchestration. Compose added “a repeatable sequence that produces finished content,” a benefit the source does not establish and the prompt rules prohibit inferring. The candidate was longer than baseline (172 versus 143 words) without a stronger opening.
- **Decision:** **REGRESSION.** Not promoted; original prompt restored.

### Experiment 002 — Clarify company-introduction scope hierarchy

- **Problem:** After removing stale text, Narrative still followed Synthesis’s narrower technical framing.
- **Hypothesis:** Explicitly preserve company-level scope for a company introduction and treat product/technical details as supporting context.
- **Target:** `Narrative/Narrative.user.prompt.txt` only.
- **Change:** Added a conditional scope rule and removed the same stale fixed topic from the candidate version.
- **Evaluation:** Narrative and Compose more clearly introduced Nuëyon and Compose. However, output invented readers (“early adopters” and “potential collaborators”), detailed workflow steps absent from the brief (“capturing and indexing,” “structuring an outline,” and “preparing the output for publication”), and editorial meta-text (“This note is for…”). It also described the initial implementation as validating the workflow, which overstates a roadmap priority.
- **Decision:** **REGRESSION.** Not promoted; original prompt restored.


### Experiment 003 — Allow a company introduction as the Synthesis insight

- **Problem:** Synthesis was steered toward a specific technical development even when the Selected Idea and source called for a company introduction.
- **Hypothesis:** Permit a narrow, source-supported company/product introduction as the central insight and replace the prompt’s repeated agent-to-orchestrator example.
- **Target:** `Synthesis/Synthesis.system.prompt.txt`.
- **Change:** Revised the central-insight rule and example; no other prompt changed.
- **Evaluation:** Synthesis connected Compose to the company vision, but continued to center the product workflow downstream. It also changed the source’s “aims to build a portfolio” into “building a portfolio,” strengthening certainty. The post remained technical and omitted most of the roadmap.
- **Decision:** **REGRESSION.** Not promoted; original prompt restored.

### Experiment 004 — LinkedIn opening and ending, three runs per condition

- **Problem:** The baseline post often opened with a generic company description and sometimes repeated its central point in the ending.
- **Hypothesis:** When Narrative contains a specific supported observation, lead with it before the company description; ensure the ending contributes new information.
- **Target:** `Compose/Compose.linkedin.user.prompt.txt` only.
- **Change:** Added conditional guidance for observation-led openings and a check to remove conclusions that only restate the central point.
- **Evaluation design:** Three baseline runs and three candidate runs used the same input, model, settings, and unchanged upstream prompts.
- **Evaluation:** All three candidates still opened with company identity statements. All three ended by returning to the same company/product point. One candidate added reader categories (founders, product leaders, SaaS builders) not established in the source. The hook and repetition gains were not demonstrated, and factual integrity regressed in one sample.
- **Decision:** **REGRESSION.** Not promoted; original Compose LinkedIn prompt restored.
## Before and After Comparison

**Baseline opening:** “Nuëyon is an early-stage, AI-first software company building reusable AI-native products…”

**Experiment 001 addition:** “...coordinating intelligence, tools, context, and knowledge into a repeatable sequence that produces finished content.” The repeatability/outcome claim is unsupported.

**Experiment 002 addition:** “This note is for AI founders, product leaders, early adopters, and potential collaborators…” That audience list is not established by the source brief and reads like editorial notes rather than a publishable company-page post.

All three candidates moved in places toward a clearer company introduction, but none passed the mandatory factual-integrity gate or demonstrated a safe overall improvement.

## Pipeline Quality Assessment

This assessment describes the frozen baseline production prompts, since no candidate was promoted.

- **Idea:** Relevant selected direction; alternatives not captured by existing console output.
- **Research:** Strong source extraction and uncertainty labeling, with substantial over-collection and no external retrieval.
- **Synthesis:** Useful evidence organization, but narrowed a company introduction into an orchestration insight and introduced reader-benefit assumptions.
- **Narrative:** Coherent progression, but the fixed agent-to-orchestrator language steered focus away from the requested company introduction.
- **Compose:** Readable, but the opening is not compelling, the ending repeats the central point, and technical details crowd the company vision.

## Production Changes

- **Production prompt files changed:** None in the final state.
- **Prompt candidates:** Narrative and Synthesis candidates are preserved under `experiment-001/`, `experiment-002/`, and `experiment-003/`. The Compose LinkedIn candidate and six repeated run logs are preserved under `experiment-004-opening-and-ending/`.
- **Rollback:** The original Narrative, Synthesis, and Compose LinkedIn prompts are preserved in their iteration folders and match the baseline snapshots exactly.
- **Promoted experiment identifiers:** None.
- **Git:** No commit or push was made. The pre-existing untracked `prompts/` folder was left untouched. `PromptExperiments/` contains this run’s artifacts.

## Build and Test Results

- **Final restored production build:** Passed, 0 warnings and 0 errors.
- **Final restored production tests:** 165 passed, 0 failed, 0 skipped: Application 138, Infrastructure 21, Console 6. The Domain test project reported no discoverable tests.
- **Candidate build/tests:** All candidate prompt versions built successfully and passed the same discovered test suites.
- **Application runs:** The completed baseline retry, three earlier candidate runs, and all six experiment-004 runs exited 0 with all five agent stages present in their logs.

## Final Recommendation

Do not promote any candidate. The three-iteration limit for this quality problem has been reached. The next experiment should use repeated runs per condition and a distinct hypothesis, because single samples cannot separate prompt effects from generation variability. Keep factual integrity as a hard promotion gate.



