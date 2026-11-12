# Idea experiment lessons and cleanup — 12 November 2026

The repository started clean. The solution contained four production projects, four production test projects and two temporary experiment projects; six further harness projects lived in historical result directories. The cleanup removes the temporary projects and historical raw runs, retaining both complete 12 November comparisons and their frozen intro/005/010 inputs. Historical evidence is immutable: paths and settings inside those records describe the original runs, not current configuration.

## Production decision

Use gpt-6-luna with one Idea workflow, mandatory semantic judge and at most two generations (one shared structural/semantic repair). The three promoted prompt files match the recorded acceptance manifest byte-for-byte; see idea-prompt-integrity.json. The judge user message is generated as JSON in C#, not loaded from a fourth template. No downstream agent prompts or behavior are changed.

Luna produced 3/3 usable final ideas against GPT-5-mini 1/3; Luna initially passed 2/3 and repaired 010 once. This is a three-case assistant-reviewed sample, not a blinded independent evaluation or proof of general reliability. No full downstream pipeline was tested. See ../PromptExperiments/IdeaGenerator/luna-simplified-comparison-2026-11-12/report.md and comparison.json. The preceding model/profile comparison and exact frozen inputs remain in ../PromptExperiments/IdeaGenerator/legacy-simplified-acceptance-2026-11-12/.

## Durable regression lessons

- 005: keep observed degradation scoped to the comparison. Non-concurrent runs and upstream variation do not establish a general causal or repeatable effect.
- 010: first visible drift varies by stage. A Compose factual overclaim is distinct from company-first hierarchy drift; do not prescribe a Narrative-first intervention without evidence.
- Intro: the input is a frozen reconstructed brief; preserve company-first intent with Compose as the concrete starting point, not proven market validation.
- Automatic acceptance alone is insufficient. Preserve source and editorial intent, rejudge repairs, fail closed before Research, and keep diagnosis separate from proposed wording.
- Fake-client tests verify orchestration and contracts, not real judge accuracy. Stale embedded resources and unequal execution paths can invalidate comparisons.

## Historical decisions

| Experiment group | Finding and decision |
|---|---|
| Initial prompt and editorial ordering trials | Candidates regressed; production prompts restored. Traceability suggested cross-stage hierarchy drift without proving a single cause. |
| Baseline 002 | 1/15 combined passes against 14/15 required; rejected. |
| Generator trials 003–005 | Causal overclaims in 005 and unsupported intervention priority in 010 persisted. Trial 003 had an unrun case, so full acceptance was undetermined. Trials 004/005 passed only 1/3; no promotion. |
| Production CLI acceptance v1–v4 | Early rounds failed content acceptance. V3 original cases passed 1/3 and holdouts 2/3. V4 automatic acceptance of seven first attempts hid failures: only strategy passed source review; live repair remained untested. No prompt promotion. |
| Validator acceptance v1/v2 | Initial tuning gains did not survive repeatability, critical-case and diagnosis-quality gates; no promotion. |
| Specialist judges | Both specialists missed known 005/010 errors; their combination also falsely rejected a correct idea. No production adoption. |
| Evidence-aware generation/review | Baseline 3/10, evidence-aware 3/10, review 2/10; NO-GO. No robust first-idea repair gains. Unblinded assistant scoring limits inference; this does not rule out all evidence/review methods. |
| Drift localization and Narrative boundary trials | First visible drift varied. Stale embedded prompt resources invalidated part of the comparison; no causal conclusion or downstream prompt promotion. |

The old temporary harnesses and raw historical result folders are removed. Git history retains tracked historical material. Both complete 12 November comparisons remain in this checkout as the evidence for the current decision; their historical file paths and settings are records, not current executable references.
