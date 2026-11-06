# Experiment hypothesis

**Experiment ID:** NC-2026-11-03-002 / experiment-004-opening-and-ending  
**Problem:** The production baseline post opens with a generic company description and repeats the central orchestration point in its ending, reducing engagement and focus.

**Evidence:** The baseline Compose post begins “Nuëyon is an early-stage, AI-first software company…” and later restates that Nuëyon’s AI-first ambition is expressed through orchestration. The same post already contains a more specific supported premise: access to models alone does not automatically make AI useful, and Nuëyon’s first product direction is Compose.

**Root cause:** Existing Compose guidance asks for a compelling opening and non-repetitive ending, but the baseline output still defaults to a company-description opening and a summary conclusion.

**Target prompt:** `src/Nueyon.Compose.Application/Prompts/Compose/Compose.linkedin.user.prompt.txt` only.

**Hypothesis:** More concrete, conditional guidance to lead with a relevant supported observation when one is present, then end after the last new point, will improve the opening and remove redundant conclusions without changing the story or adding claims.

**Proposed change:** Add one concise LinkedIn-specific rule: prefer a supported tension/observation over a generic company-description opening when the Narrative provides one; require the final paragraph to add new information and remove a conclusion that only repeats the central claim. Preserve the existing ban on invented facts and keep a plain identity-first opening when no supported observation is available.

**Evaluation design:** Run three baseline and three candidate full-pipeline executions with identical input, provider/model, settings, and unchanged upstream prompts. Compare opening specificity/interest, redundancy, clarity, LinkedIn readability, narrative fidelity, and factual integrity. A factual-integrity failure in any candidate sample is a promotion veto. Look for a repeated improvement across samples, not one favorable output.

**Regression risks:** A forced hook could exaggerate the source, or a shorter ending could omit a meaningful conclusion. Upstream output varies by model run, and the application does not expose a deterministic seed.
