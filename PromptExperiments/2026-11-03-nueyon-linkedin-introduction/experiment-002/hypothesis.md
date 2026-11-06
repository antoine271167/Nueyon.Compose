# Experiment hypothesis

**Experiment ID:** NC-2026-11-03-001 / experiment-002  
**Problem:** Experiment 001 removed the obsolete fixed narrative topic, but its output remained centered on the orchestration workflow. The unchanged Synthesis had framed orchestration as the central insight, so Narrative did not preserve the broader company-introduction scope strongly enough.

**Evidence:** Experiment 001 Narrative still said the main point was an “orchestration-first workflow” and that Compose was an “initial, pragmatic expression” of that approach. The Selected Idea called for introducing Nuëyon’s AI-first identity, intended audience, purpose, and Compose as the first product direction.

**Root cause:** The Narrative prompt states the general priority of the Selected Idea but does not explicitly resolve a mismatch where Synthesis narrows a broad introduction to one technical detail. The model continued to follow the narrower technical framing.

**Target prompt:** `src/Nueyon.Compose.Application/Prompts/Narrative/Narrative.user.prompt.txt` only.

**Hypothesis:** A clear scope-hierarchy rule will keep a company introduction at the company level while using product architecture only as supporting context. This should better match the Selected Idea and reduce unnecessary repetition of orchestration in Compose.

**Proposed change:** Starting from the frozen production prompt, remove the obsolete agent-to-orchestrator storyline and add one conditional rule: for company introductions, preserve the company-level scope and treat narrower product/technical insights as supporting context unless the Selected Idea makes them central. Also keep the stale examples/end-point guidance generalized. No other prompt or application behavior changes.

**Expected result:** Narrative leads with Nuëyon’s company purpose and identity, presents Compose as the first concrete direction, and keeps orchestration subordinate. Compose is clearer and more focused without unsupported audience needs, benefits, or product outcomes.

**Evaluation criteria:** Assess story alignment, appropriate orchestration prominence, factual integrity and certainty, Narrative responsibility, Compose readability/focus/format, and downstream regressions using complete agent outputs. Any unsupported benefit or material factual drift fails the quality gate.

**Regression risks:** Narrative may understate Compose’s actual product direction or omit context needed to explain it. Compose may still infer benefits or overstate implementation. The model is nondeterministic, so this is one observed sample per condition.
