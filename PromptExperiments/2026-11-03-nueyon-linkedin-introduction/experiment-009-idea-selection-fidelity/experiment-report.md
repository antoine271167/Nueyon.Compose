# Evaluation — Experiment 009

## Rubric

Each score is 0–2, evaluated across the selected Idea and downstream Research, Synthesis, Narrative, and Compose: **2** clear pass, **1** mixed/limited concern, **0** material failure. Dimensions: (1) Nuëyon remains primary; (2) Compose is the supporting first product direction; (3) orchestration stays contextual rather than company positioning; (4) audience, claims, and certainty stay within the source; (5) that hierarchy survives downstream. Score totals are descriptive, not a statistical measure. A run passes the promotion gate only if every dimension is 2 and no material factual issue appears.

| Run | Nuëyon primary | Compose supporting | Orchestration contextual | Audience / claims / certainty | Downstream hierarchy | Total / 10 | Gate |
|---|---:|---:|---:|---:|---:|---:|---|
| Baseline 001 | 2 | 2 | 2 | 1 | 2 | 9 | Fail: Compose says the process “reliably converts” ideas, an outcome not established in the source. |
| Baseline 002 | 2 | 2 | 0 | 2 | 0 | 6 | Fail: Compose calls the immediate product direction “orchestration-first.” |
| Baseline 003 | 2 | 2 | 1 | 1 | 1 | 7 | Fail: technical framing competes with the company introduction; adds unsupported rationale/process language. |
| Candidate 001 | 2 | 2 | 1 | 2 | 1 | 8 | Fail: Narrative/Compose describe an “orchestration-first product” as the expression of company identity. |
| Candidate 002 | 2 | 2 | 0 | 1 | 0 | 5 | Fail: Narrative and Compose make orchestration the “immediate directional focus” and “central design emphasis”; adds clarity/systematic outcome language. |
| Candidate 003 | 2 | 2 | 0 | 0 | 0 | 4 | Fail: Narrative invents “founders and operators” and “early-stage partners”; Compose says company identity is “anchored” in orchestration. |
| **Baseline mean** | **2.00** | **2.00** | **1.00** | **1.33** | **1.00** | **7.33** | **0/3 passed** |
| **Candidate mean** | **2.00** | **2.00** | **0.33** | **1.00** | **0.33** | **5.67** | **0/3 passed** |

The selected Idea alone kept Nuëyon primary and Compose as the first product direction in all six runs. The experiment provides no repeatable upstream selection gain. Candidate runs then drifted downstream in all three samples; this means the Idea prompt, as tested, did not secure the requested end-to-end hierarchy.

## Evidence examples

- **Baseline 001:** Idea selected “Introduce Nuëyon as an AI-first company” with Compose as its concrete first step. Compose later claimed a “repeatable process that reliably converts” material to publishable content.
- **Baseline 002:** Narrative says the identity is made tangible “through an orchestration-focused product”; Compose concludes, “Nuëyon’s immediate product direction is orchestration-first.”
- **Candidate 001:** Idea correctly describes a company introduction and first product. Compose nevertheless concludes the current direction is “to validate an orchestration-first product in Compose as a practical expression of its AI-first identity.”
- **Candidate 002:** Narrative calls the “immediate, directional focus” an orchestration-centered content workflow. Compose repeats that orchestration should make work “clearer and more systematic.”
- **Candidate 003:** Narrative broadens the source audience to “founders and operators of AI-first companies and early-stage partners.” Compose states Nuëyon’s public identity is “being anchored” in orchestration.

## Decision

**NO IMPROVEMENT / REGRESSION; candidate rejected.** The candidate improved neither selected-Idea hierarchy (already company-first in all baseline runs) nor end-to-end preservation. It repeatedly cast orchestration as the defining company narrative and introduced an unsupported audience in one candidate run. It fails the source-fidelity and editorial-hierarchy promotion gates. The candidate remains archived; production was restored byte-for-byte to the frozen prompt. No prompt was promoted.

## Method and limitations

Three full baseline runs were followed by three full candidate runs through Idea → Research → Synthesis → Narrative → Compose. The source and runtime source hashes match (`DB7A9C…E8F2D0`), the model was `gpt-5-mini`, output format was `LinkedInPost`, and all other prompt snapshots were identical. The Idea system prompt was the sole candidate change. Run manifests, full console logs, stage outputs, prompt snapshots, hypothesis, and integrity checks are archived with this report.

Runs were not interleaved; baseline preceded candidate. The backend is nondeterministic, with no seed or temperature override. These six samples are descriptive and cannot establish statistical significance or isolate time-varying model-backend effects. The qualitative scores record observed output, not model intent.

## Validation and repository status

- Build before runs and after rollback: passed, 0 warnings and 0 errors.
- Tests before runs, after candidate change, and after rollback: 165 passed, 0 failed, 0 skipped; Domain test project had no discoverable tests.
- All 11 production `.prompt.txt` files match their frozen baseline snapshots after rollback.
- Source brief is unchanged; runtime brief hash equals the repository brief hash.
- No C#, schema, configuration, or other prompt was changed. No commits or pushes were made.
- Git remains on `main` at `be882989a7b7956e1b57199f143e7cf434fbe4e5`. Pre-existing untracked `PromptExperiments/` and `prompts/` remain untracked; this experiment added files only under its new `experiment-009-idea-selection-fidelity/` directory.
