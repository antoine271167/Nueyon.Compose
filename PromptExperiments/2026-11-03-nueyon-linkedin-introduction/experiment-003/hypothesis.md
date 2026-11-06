# Experiment hypothesis

**Experiment ID:** NC-2026-11-03-001 / experiment-003  
**Problem:** The Synthesis stage repeatedly narrows the company introduction to an orchestration insight. This persists even after Narrative’s stale fixed storyline is removed.

**Evidence:** The Synthesis system prompt says the central insight must describe a specific discovery, development, tension, decision, or lesson, and explicitly excludes a summary of the product’s overall purpose. The Idea prompts and source material explicitly permit and request a company introduction grounded in its identity, first product, and intended direction. Baseline and candidate outputs show the technical framing taking precedence.

**Root cause:** The Synthesis system prompt’s central-insight rule is too restrictive for a Selected Idea whose editorial purpose is a company introduction. It pushes selection toward a narrow technical principle even when the source-supported identity and product direction are the appropriate focus.

**Target prompt:** `src/Nueyon.Compose.Application/Prompts/Synthesis/Synthesis.system.prompt.txt` only.

**Hypothesis:** Allowing the central insight to match a supported company/product-introduction purpose, while preserving the narrow-scope rule, will keep Synthesis aligned with the Selected Idea. Downstream Narrative and Compose should then foreground Nuëyon’s identity and first product direction rather than treating orchestration as the whole story.

**Proposed change:** Replace the Synthesis system prompt’s fixed definition and topic-specific example with purpose-aligned guidance that explicitly permits a source-supported company/product introduction. Leave all other prompts and application behavior unchanged.

**Expected result:** Synthesis will select the company identity and Compose as first direction as the central story, keep orchestration as supporting context, and preserve all status, audience, and roadmap limits.

**Evaluation criteria:** Compare full pipeline outputs against the frozen production baseline. Check Selected Idea alignment, central story, factual integrity, uncertainty, agent responsibilities, company/Compose coverage, technical-detail prominence, final LinkedIn readability, and regressions. Any unsupported audience, capability, outcome, or implementation claim fails the factual-integrity gate.

**Regression risks:** Synthesis could broaden into a product overview or include too much roadmap context. Narrative’s existing fixed instruction may still bias the full run. Model output is nondeterministic; the condition is sampled once.

