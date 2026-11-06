# Experiment hypothesis — 009: Idea Selection Fidelity

**Problem**
Experiment 008 found the first editorial drift in the selected Idea in two of three company-first runs: Compose/orchestration displaced Nuëyon as the primary subject. The third Idea preserved company-first order, but unsupported audience/process claims emerged downstream. This raises the possibility that the Idea agent is an early point where product detail can be mistaken for company positioning.

**Evidence**
See Experiment 008, runs A-001 and A-003: A-001 frames Compose as the concrete expression defining Nuëyon's identity; A-003 selects an orchestration-first philosophy as the company emphasis. In A-002, the hierarchy is closer, but Narrative invents a founders/product-leads audience.

**Root cause**
The Idea prompt asks for the strongest editorial opportunity and considers intended purpose, but it does not explicitly preserve the company-versus-first-product hierarchy when ranking an introduction idea.

**Target prompt**
`src/Nueyon.Compose.Application/Prompts/Idea/Idea.system.prompt.txt` only.

**Hypothesis**
Adding a bounded selection rule for company introductions will make the selected Idea center Nuëyon, use Compose as a supporting first product direction, keep orchestration contextual, and carry the source's intended audience and uncertainty into the full downstream pipeline.

**Proposed change**
Append one section to the Idea system prompt. No code, schema, configuration, other prompt, or input changes.

**Expected result**
Across three candidate runs, selected Ideas should keep Nuëyon primary; Research, Synthesis, Narrative, and Compose should preserve this hierarchy and stay within source-supported audience, claims, and certainty. Candidate must pass all hard gates in every run to be considered for promotion.

**Evaluation criteria**
Per stage/run, score 0–2 for (1) Nuëyon primary, (2) Compose supporting first product direction, (3) orchestration not company positioning, (4) audience/claims/certainty within source, and (5) hierarchy preserved downstream. 2 = clearly passes; 1 = mixed/weak or non-material inference; 0 = material failure. Factual and hierarchy gates require no 0 scores. Compare per-run and mean scores with the three baseline runs. Three runs are descriptive, not statistical evidence.

**Regression risks**
The added hierarchy could over-constrain Idea selection when a source's purpose is not a company introduction, create repetitive company-overview ideas, or suppress a genuinely stronger supported technical story. It must apply only when a company-introduction purpose is evidenced.
