# Experiment hypothesis

**Experiment ID:** NC-2026-11-03-001 / experiment-001  
**Problem:** The baseline Narrative centers Nuëyon’s introduction on an orchestration architecture, although the Selected Idea and source brief call for introducing Nuëyon’s identity and first product direction. The resulting story gives orchestration and the single-model contrast more prominence than the company vision.

**Evidence:** The baseline Narrative says the main point is an “orchestration-focused approach … rather than treating content creation as a single open-ended model call.” The baseline Compose post repeats the orchestration framing. The production Narrative user prompt contains fixed directions that “the central story is the development from setting out to build an AI agent toward designing an orchestrator,” that the endpoint should clarify an orchestration-focused direction, and examples about that same transition.

**Root cause:** The Narrative prompt includes story-specific instructions from a different topic. They conflict with the current Selected Idea and override the general rule to preserve that idea.

**Target prompt:** `src/Nueyon.Compose.Application/Prompts/Narrative/Narrative.user.prompt.txt` only.

**Hypothesis:** Replacing the stale fixed story, examples, and ending guidance with input-driven instructions will make the Narrative center the company introduction and treat orchestration as supporting detail only when the current Selected Idea and Synthesis warrant it. Compose should then produce a post that better matches the requested company introduction without changing factual meaning or certainty.

**Proposed change:** Remove the fixed agent-to-orchestrator story and its topic-specific examples. State that central focus, examples, and endpoint must be derived from the current Selected Idea and Synthesis. Keep all existing role boundaries, factual constraints, output structure, and placeholders.

**Expected result:** Narrative’s central story identifies Nuëyon’s AI-first company ambition and Compose as the first concrete direction. Orchestration does not displace the company introduction. Compose output aligns more closely with that story.

**Evaluation criteria:** Compare baseline and candidate for alignment with Selected Idea, coverage of company vision and Compose, appropriate prominence of orchestration, factual integrity and uncertainty, Narrative responsibility, downstream Compose focus/readability, and LinkedIn format. Evidence must be visible in the complete outputs.

**Regression risks:** Removing relevant product context could make the story vague; Compose could add unsupported claims to make the broader introduction sound compelling; the unchanged Synthesis may still overemphasize orchestration. Factual integrity and agent responsibilities are mandatory gates.
