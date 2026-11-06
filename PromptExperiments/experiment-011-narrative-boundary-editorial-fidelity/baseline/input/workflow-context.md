# Nuëyon.Compose — Reusable Workflow Context

Default locations:

- Project: `C:\Workspaces\Nueyon.Compose`
- Experiment artifacts: `C:\Workspaces\Nueyon.Compose\PromptExperiments`
- Source code: `C:\Workspaces\Nueyon.Compose\src`

Architecture rule: Idea, Research, Synthesis, and Narrative are format-agnostic and generic. Only Compose receives and applies the requested output format (LinkedIn, Facebook, article, and so on). Keep source facts about the desired publication separate from format-specific structures, voice, hooks, or platform conventions.

Evaluate two independent quality gates:

1. **Source fidelity:** factual support, audience boundaries, meaning, and certainty.
2. **Editorial quality:** a compelling story direction, an opening that earns attention, coherent progression, readability, and strong natural phrasing.

A source-accurate but flat result does not pass the editorial gate; an engaging result with unsupported claims does not pass the source-fidelity gate. Trace the earliest observable deviation through Idea → Research → Synthesis → Narrative → Compose, and label observations, interpretations, and uncertainty separately.

For diagnostic analyses, preserve user changes, use existing outputs/logs/prompt snapshots/manifests, and make no new generations or production changes unless the experiment explicitly authorizes them. Put each new artifact in its own new directory under `PromptExperiments`.
