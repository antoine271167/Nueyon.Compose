# Experiment 011 hypothesis

**Problem and evidence.** Experiments 007–010 show that Narrative is the most recurrent first visible point where the selected company story shifts toward orchestration. Prior evaluations also under-scored whether a source-faithful story is engaging, coherent, readable, and worth following. Experiment 010 identifies Narrative as a recurring locus, while noting Idea/Synthesis can already drift and format leakage exists elsewhere. This trial holds upstream format references constant.

**Target.** Only `Narrative.system.prompt.txt`. The workflow confirms the Narrative system and user prompts are separate; this experiment changes only the system prompt, which carries the agent's role and editorial responsibilities. Narrative user prompt, all other prompts, code, schemas, configuration, source, and output format remain fixed.

**Hypothesis.** A narrowly scoped instruction to find an engaging, evidence-supported entry point and give each progression step new information will improve editorial quality while explicitly preserving the Selected Idea/Synthesis meaning and the Nuëyon → Compose → orchestration supporting hierarchy for the company-introduction direction.

**Candidate change.** Add general story-structuring guidance: choose a specific supported detail/contrast/tension/open question; create distinct forward movement; avoid repetition; keep company/product/technical hierarchy; use no platform conventions. Do not change source meaning, claim scope, uncertainty, audience, motivation, or results.

**Design.** Three full baseline and three full candidate pipelines, interleaved B-C-B-C-B-C, using current repository input and settings. Store complete console logs and all five stage outputs. Compare each run's Narrative input (selected Idea + Synthesis) before attributing changes to Narrative. First drift is the earliest observable divergence from the source/selected editorial direction; it does not establish hidden model causation.

**Promotion gate.** Every candidate run must pass all three independent dimensions: content integrity, editorial direction, editorial quality; contain no material factual error; and be no worse than baseline on any dimension. Any failed or uncertain gate rejects the candidate and requires byte-for-byte restoration. No dimension compensates for another.

**Limitations.** No seed/temperature override is recorded. Three runs per condition are descriptive, backend nondeterminism and temporal effects remain possible, and evidence cannot establish statistical significance.
