# Experiment 001 evaluation

**Decision: REGRESSION — not promoted.**

The candidate used the same source brief, workflow instruction, model (`gpt-5-mini`), application configuration, and pipeline. Only the Narrative user prompt differed from the frozen production prompt. The model is nondeterministic and no seed or temperature override is configured, so Idea and Research wording varied between runs.

| Criterion | Result | Evidence |
|---|---|---|
| Targeted story focus | Improved in part | Candidate Narrative opens with Nuëyon’s identity and Compose as its first product direction. It still makes orchestration the main explanatory thread. |
| Selected Idea alignment | Improved in part | Candidate selected idea explicitly covers Nuëyon’s identity, intended audience, reason for existing, and Compose’s role. The resulting Narrative retains most of this direction. |
| Factual integrity | Regressed | Candidate Compose calls the process a “repeatable sequence that produces finished content.” The source does not establish repeatability or demonstrated results; the Synthesis and Compose prompt rules explicitly prohibit inferring those benefits. Candidate Narrative also calls the product a “focused experiment” and “pragmatic” starting point, which are not stated as such in the source. |
| Agent responsibility | Mixed | Narrative now frames the company introduction rather than an agent-to-orchestrator transition, but its guidance still overweights architecture relative to the company introduction. Compose adds an unsupported process outcome. |
| Completeness | Unchanged | Candidate still does not explain the longer-term roadmap or broader planned directions in the final post. |
| Relevance | Improved in part | Company identity and Compose are prominent; the orchestration explanation occupies substantial space. |
| Clarity | Unchanged | The candidate is understandable, but uses several abstract phrases (“orchestration-first,” “product-oriented identity,” “pragmatic starting point”). |
| Focus | Regressed | The post repeats the “narrow starting point” and orchestration point across multiple paragraphs. |
| Readability | Regressed | Candidate is longer than baseline (172 versus 143 words) and adds another paragraph without a stronger opening. |
| LinkedIn format | Unchanged | Short paragraphs and whitespace are present, but the opening remains a generic company statement rather than a compelling hook. |
| Overall usefulness | Regressed | A partial focus improvement does not offset the factual-integrity failure and the lack of a stronger opening. |

## Before and after excerpts

**Baseline Compose:** “Nuëyon is an early-stage, AI-first software company building reusable AI-native products…”

**Candidate Compose:** “Compose centers on an orchestration-first workflow … the emphasis is on coordinating intelligence, tools, context, and knowledge into a repeatable sequence that produces finished content.”

The candidate makes the intended company identity more explicit in the first paragraph, but its later wording adds a claim the source does not support. The baseline already framed Compose as a narrow first direction and retained the early-stage caveat.

## Promotion

Not promoted. The production prompt was restored from the preserved rollback copy. Candidate prompt and outputs remain in this experiment directory.

