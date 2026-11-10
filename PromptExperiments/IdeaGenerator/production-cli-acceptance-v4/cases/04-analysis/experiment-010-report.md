# Experiment 010 — Pipeline Drift Localization

**Date:** 2026-11-05 (Europe/Amsterdam)  
**Type:** Read-only diagnostic analysis of existing artifacts.  
**Decision:** No new model generation, prompt/code/schema/configuration change, build, test, commit, or push was performed. This report was added in a new directory only.

## Fixed project locations and future workflow context

- Project: `C:\Workspaces\Nueyon.Compose`
- Experiment artifacts: `C:\Workspaces\Nueyon.Compose\PromptExperiments`
- Source code: `C:\Workspaces\Nueyon.Compose\src`
- This report: `C:\Workspaces\Nueyon.Compose\PromptExperiments\experiment-010-pipeline-drift-localization\report.md`

Use these paths as the default locations in future Nuëyon.Compose experiment workflows. Keep `Idea → Research → Synthesis → Narrative` format-agnostic and generic. Only Compose should receive and apply a requested format such as LinkedIn, Facebook, or article. Evaluate factual/source fidelity separately from editorial strength: a source-accurate but dull or hard-to-read result is not a complete success. Preserve all existing user changes and untracked files; isolate new artifacts in a new experiment directory.

## Executive findings

1. **There is no dedicated `experiment-007/` execution directory.** Experiment 007 is a traceability report stored inside the Experiment 006 directory. Its three candidate chains have Synthesis, Narrative, and Compose extracts and console logs, but no Idea or Research extracts. We can localize drift only from Synthesis onward for those chains; any claim about the first drift in Idea or Research would be unsupported.
2. **The first visible editorial hierarchy break varies by run.** In Experiment 007 candidate runs 001–002 it is clearest in Narrative; candidate 003 is already company/product hierarchy drift in Synthesis. In Experiment 008 Company-first runs, selected Idea already changes the hierarchy in A-001 and A-003; A-002 retains the intended hierarchy but adds an unsupported audience at Narrative. Technical-first B runs intentionally start technical, so their first technical framing in Idea is expected by that condition, though factual inflation appears later too.
3. **Experiment 009 shows that a company-first selected Idea is not sufficient.** The evaluation reports the selected Idea kept Nuëyon primary and Compose as first product direction in all six baseline/candidate runs. The reported issues then appear downstream: orchestration-centered story framing, unsupported audience/validation/outcome language, or a factual overclaim in Compose. Candidate changes to Idea did not demonstrate improvement.
4. **Factual and editorial failures are distinct.** Orchestration is source-supported as a Compose design principle; making it the defining story of a company introduction can still violate the source’s editorial intent. Separately, outputs add unsupported audience groups, validation claims, or performance-like properties.
5. **The outputs are not consistently strong LinkedIn editorial content even when broadly source-aligned.** Several chains resolve into explanatory process descriptions, architecture-heavy lists, or repeated framing such as “orchestration-focused/first” rather than a specific, engaging company story. This is an editorial-quality concern, not by itself a factual error. The artifact evaluations did not apply a consistent hook/readability/attention rubric to every run, so this observation is qualitative and not a comparative score.
6. **There is observable upstream format leakage in prompt snapshots.** The Master Workflow says upstream stages should remain format-independent, but frozen Idea, Research, and Narrative prompts include references to supported formats, article-writing roles, or Article/LinkedIn compatibility. The outputs do not prove that these instructions caused a particular drift; they are an architectural inconsistency and a plausible source of format priming.

## Evidence basis and method

Reviewed the existing Master Workflow, baseline prompt snapshots, Experiment 006/007 traceability artifacts, all Experiment 008 run manifests/reports/stage outputs, and Experiment 009 evaluation/integrity artifacts and full five-stage run outputs. Compared each available run in pipeline order: selected Idea → Research → Synthesis → Narrative → Compose. For each run, “first drift” means the earliest **observable** stage that departs from the requested company-first editorial hierarchy, introduces unsupported material, or makes a materially stronger claim. It does not imply internal model causation. In technical-first Experiment 008 B runs, technical hierarchy itself is the intended condition; the table identifies any separate scope/factual breach.

Source boundary: frozen reconstructed brief SHA-256 in Experiments 006/008/009 is `DB7A9CF4768684C025D02028980E16CCE8805832E1013A5B6098C00D29E8F2D0`. Experiment 008 reports its A/B inputs were section reorderings of this brief, with section content preserved. Experiment 009 reports the same source/runtime hash and only Idea system prompt changed for candidate runs.

## First observable drift by run

### Experiment 007 (traceability artifact over Experiment 006)

| Run | Earliest supported localization | Factual/source fidelity observation | Editorial quality observation | Evidence limit |
|---|---|---|---|---|
| Candidate 001 | **Narrative** | Synthesis keeps Nuëyon and Compose primary; its “orchestrated workflow” is a supported product descriptor. Narrative turns that support detail into the through-line (“orchestration-focused”); Compose sharpens it to “orchestration-first.” | Company introduction becomes a product/orchestration explanation. Compose mostly inherits rather than invents a new factual claim. | Baseline already had orchestration-led framing. No Idea/Research extracts in this diagnostic package. |
| Candidate 002 | **Narrative**, with an earlier Synthesis warning signal | Synthesis says Compose is the “logical product to validate Nuëyon’s approach”; source says build/validate the core workflow, not validate the company’s approach. Narrative adds validation framing, a specific audience list, and “testable” positioning; Compose adds “predictable and repeatable.” | The company-first setup gives way to a technical product thesis and process explanation. | Existing baseline also had orchestration and audience expansion; exact causal attribution is not possible. |
| Candidate 003 | **Synthesis** | Primary message and subject make Compose/orchestration the story; broader company identity and audience become context. Narrative/Compose inherit it and add a broader single-LLM framing; “testable embodiment” is an inference. | Strongest hierarchy break: reads as a technical argument about orchestration rather than a company introduction. | The diagnostic report compares against baseline, which already overweights orchestration. |

### Experiment 008 (Company-first A and Technical-first B)

| Run | Earliest observable signal/deviation | Source fidelity | Editorial quality / architecture | Evidence note |
|---|---|---|---|---|
| A-001 | **Idea** | Compose is selected as the “concrete expression” framing Nuëyon’s identity; later “AI-first builders” substitutes a narrower audience than the source’s other AI-first companies. | Product-led frame begins before Research; Narrative then makes orchestration-centered framing explicit. | Company-first input, 0/2 editorial fidelity in the experiment score. |
| B-001 | **Idea** (expected technical-first frame); **Compose** is first noted factual breach | Final Compose asserts most AI content workflows use a “single, one-shot prompt,” unsupported by the brief. | Technical frame is stable, as intended for B. This does not satisfy company-first editorial intent and is not evidence of a good company introduction. | The report scores editorial hierarchy 2/2 against B’s technical-first condition, not against the desired company introduction. |
| A-002 | No clear hierarchy break through Idea; **Narrative** is first factual breach | Narrative invents “founders and product leads”; later material adds validation, learnability, and iteration framing beyond documented motivation/outcomes. | Closest company-first result, but not clean on audience or claim discipline. | The report explicitly identifies the first factual breach at Narrative. |
| B-002 | **Idea** (expected technical-first frame); **Idea** also first reported overstatement | Idea’s “launch” is unsupported; later stages strengthen implementation certainty and future-update claims. | Technical-first framing remains stable. It is condition fidelity, not desired company-intro editorial fidelity. | 0/2 factual score. |
| A-003 | **Idea** | Selected Idea calls Compose an “orchestration-first philosophy”; Narrative invents a “product-strategy audience”; Compose adds specific source-selection steps. | Technical product frame competes with company identity from the first stage. | Company-first condition, 0/2 editorial fidelity. |
| B-003 | **Idea** (expected technical-first frame); **Idea** first exceeds source scope | Calls orchestration Nuëyon’s “core product idea,” whereas the source scopes it to Compose. | Strong technical-first ordering, but the company-level assertion is an editorial and factual-scope overreach. | Technical-order fidelity score is not an endorsement of the claim. |

### Experiment 009 (baseline and candidate)

| Run | Earliest observable material deviation | Source fidelity | Editorial quality / hierarchy | Evidence note |
|---|---|---|---|---|
| Baseline 001 | **Compose** (reported factual failure) | Final says “repeatable process that reliably converts” material into publishable content; reliable conversion is not established. | Selected Idea and earlier chain remain company-first; the artifact’s score gives downstream hierarchy 2/2. | The evaluation gives no earlier stage a material failure for this run. |
| Baseline 002 | **Narrative** (first clear editorial shift) | Orchestration-centered product direction is source-adjacent, but Compose escalates to “immediate product direction is orchestration-first.” | Company identity becomes tangible through orchestration; final hierarchy score 0/2. | Evaluation identifies the Narrative phrase and final amplification. |
| Baseline 003 | **Narrative** (technical framing competes); later Compose factual/process concerns | Evaluation reports unsupported rationale/process language; final shifts toward an orchestration-focused product description. | Company intro competes with technical framing; downstream hierarchy 1/2. | Existing evaluation is broad on the exact first unsupported phrase; local stage text is preserved for reinspection. |
| Candidate 001 | **Narrative** (hierarchy shift) | Compose describes validation of an “orchestration-first product” as an expression of identity; evaluation records no new audience issue in this run. | Correct Idea does not keep the final company-first story; hierarchy score 1/2. | Candidate Idea prompt was the sole intended prompt change. |
| Candidate 002 | **Narrative** | Narrative makes orchestration the “immediate, directional focus”; Compose adds “clearer and more systematic” outcomes. | The chosen company-first Idea does not survive. | Evaluation records audience/claims/certainty score 1/2 and hierarchy 0/2. |
| Candidate 003 | **Narrative** | Narrative invents founders/operators and early-stage partners; Compose says public identity is “anchored” in orchestration. | Factual and editorial failures coincide at Narrative, then persist in Compose. | Candidate has the lowest reported score, 4/10. |

**Experiment 009 evidence limitation:** Its saved evaluation summarizes the six runs but does not assign a formal first-failure stage to each dimension. The table’s per-run localization uses the evaluation’s named examples plus direct stage output inspection; where the exact factual precursor is not explicitly called out (notably baseline 003), the localization is lower confidence. Research outputs are source-bound extraction documents and often repeat input framing; presence of a format or topic in Research alone is not treated as editorial drift unless it introduces unsupported content or changes hierarchy.

## Cross-experiment patterns (007–009)

- **Drift is not confined to Idea.** Experiment 008 shows early selection drift can occur in Idea, and Experiment 007 run 003 shifts at Synthesis. Experiment 009 shows all selected Ideas can look company-first while Narrative/Compose later reframe the story.
- **Narrative is the most recurrent first visible hierarchy shift in the available records.** It is first in 007 runs 001–002, 008 A-002 (factual breach) and 009 baseline 002/003 and candidates 001–003. But it is not a sole root cause: some runs start in Idea or Synthesis, and in 009 baseline 001 the reported factual issue is Compose.
- **Compose can amplify, add factual strength, or simply inherit.** Examples include “orchestration-first,” “reliably converts,” “predictable and repeatable,” and unsupported market generalizations. Other cases show Compose preserves an upstream frame, so a Compose-only fix may leave the story-selection problem intact.
- **The source supports two true propositions that compete editorially:** Nuëyon is an AI-first company; orchestration is central to Compose’s design. The requested first company introduction prioritizes the former and uses Compose as the supporting first product direction. Technical truth does not decide story priority.
- **Source fidelity and editorial quality need separate gates.** A factually supported orchestration description may still be the wrong lead for this company introduction. Conversely, a company-first idea can fail through unsupported audience, validation, market, or outcome claims.
- **Engagement has been under-measured.** Earlier run evaluations mainly score factuality, hierarchy, and condition fidelity. They do not consistently score the opening’s pull, story progression, readability, or memorable phrasing. Current evidence therefore localizes hierarchy drift better than it explains whether any surviving company-first text is compelling.

## Format-agnostic architecture audit

### Observed

- The Master Workflow’s Experiment 010 section says earlier agents remain format-independent and Compose determines LinkedIn presentation.
- Frozen Idea system prompt includes “supported output formats” among possible supporting details, and calls the agent an article writer in its role exclusions.
- Frozen Research system prompt also lists “an article writer” in role exclusions; Research user prompt says not to write polished article prose or an article outline.
- Frozen Narrative system prompt says not to optimize for a particular format but then names articles, LinkedIn posts, newsletters, and other types as compatibility examples. Narrative user prompt explicitly requires the story direction to work for “both Article and LinkedIn formats.”
- These are direct format references in upstream prompt text. The source brief itself also specifies a first LinkedIn company-page article, which is source context and can reasonably be extracted as a fact; it should not turn upstream agents into LinkedIn format designers.

### Interpretation and uncertainty

This is an instruction-boundary inconsistency and a plausible source of format priming, especially where Idea uses supported formats as an editorial-selection consideration and Narrative reasons about format compatibility. It does **not** demonstrate that these references caused orchestration drift or a weak hook in any given run. Format-independent story content can be relevant to an eventual article; only Compose should choose format-specific structure, voice, length, hook conventions, or platform mechanics under the agreed architecture rule.

## Implication for the next focused trial

Do not infer that another Idea-only change or a Compose-only style change will address every failure. The next useful experiment should be **one controlled upstream-boundary trial** that:

1. Targets the first stage supported by the evidence for the chosen failure (Narrative is the most recurrent locus; compare its actual input Synthesis before changing it).
2. Tests one explicit rule: preserve the selected company-level subject and product-support relationship through story selection, while keeping technical architecture as supporting evidence when the source’s intended communication is company-first.
3. Removes output-format reasoning from the tested upstream decision boundary only if the hypothesis specifically targets that leakage; otherwise hold prompts constant to avoid confounding hierarchy and format-awareness changes.
4. Uses repeated full-pipeline runs, captures each stage, interleaves conditions if there is a baseline/candidate comparison, and keeps the reconstructed source and execution settings identical.
5. Separately evaluates: (a) factual claims, audience, certainty, and source support; (b) company-first story hierarchy; and (c) editorial quality—opening interest, clarity, narrative progression, concise readable prose, and strength of formulation. A result passes only if both content integrity and editorial quality pass.

This is a diagnostic recommendation, not a prompt rewrite or a claim that Narrative is the sole cause. Experiment 007 already demonstrates Synthesis-origin drift in one run; Experiment 008 demonstrates Idea-origin drift; Experiment 009 shows downstream drift despite a company-first Idea.

## Repository status and integrity

Observed at the start and rechecked after report creation:

- Branch: `main`
- HEAD: `be882989a7b7956e1b57199f143e7cf434fbe4e5`
- Git status before report: `## main...origin/main`, with only existing untracked roots `PromptExperiments/` and `prompts/`; no tracked-file modifications were reported.
- This report created only `PromptExperiments/experiment-010-pipeline-drift-localization/` inside the already-untracked `PromptExperiments/` tree. No existing artifacts were edited, moved, or deleted.
- No production prompt, source code, schema, application configuration, or runtime input was modified. No generation, build, test, commit, or push was performed.
- Git cannot individually distinguish newly created files inside an already-untracked parent from pre-existing untracked files without enumerating the parent tree; the creation actions in this turn were limited to this new directory.

## Artifact map

- `report.md` — full findings, stage trace, architecture audit, uncertainties, and next-trial implication.
- `workflow-context.md` — concise reusable paths and evaluation/architecture rules for later workflows.
- `evidence-manifest.json` — hashes of the principal source artifacts consulted, to help reproduce this analysis.
