# Reproducing Experiment 17

Run from the repository root on Windows with Node.js 22.16.0 and authenticated GitHub Copilot CLI 1.0.116 installed through npm. Access to GPT-6 Luna is required. Prior artifacts are referenced in place at the base commit in model-configuration.json. Use a separate copy of the experiment directory without its archived runs for a fresh execution. The runner verifies the frozen file hashes and every referenced input before and after each split and refuses to overwrite existing attempts. Provider defaults can produce different results.

```powershell
$experiment = 'PromptExperiments/Narrative/experiment-17-semantic-validation'
$env:EXPERIMENT17_COPILOT_LOADER = Join-Path (Split-Path (Get-Command copilot.cmd).Source) 'node_modules/@github/copilot/npm-loader.js'
node "$experiment/run.cjs" development
node "$experiment/run.cjs" evaluation
```

For archived scoring, set the same loader environment variable and run `node "$experiment/score.cjs"`. To inspect verbatim quotes against input and prepare a new manual finding review, add `--inspect`. The experiment operator reviewed each returned finding for match, evidentiary support and correction direction. Fresh responses require a fresh finding review. The ground-truth labels are fixed in ground-truth.md. The original response text, stderr and schema validation metadata are preserved under runs. An invalid response, if present, has valid=false in attempt.json and retains its raw output in the same directory.

The inventory is reproducible with inventory.cjs. Its selection rules were applied before writing the validator. The commit can be recovered with `git log -1 --format=%H -- PromptExperiments/Narrative/experiment-17-semantic-validation/report.md`.
