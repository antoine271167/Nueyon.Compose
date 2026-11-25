# Reproduce Experiment 22

From the repository root in PowerShell, with the source revision and model in model-configuration.json:

`dotnet build src/Nueyon.Compose.slnx --nologo`

```powershell
$exp = 'PromptExperiments/Narrative/experiment-22-semantic-fidelity-guard'
$env:EXP22_COPILOT_LOADER = Join-Path (Split-Path (Get-Command copilot.cmd).Source) 'node_modules/@github/copilot/npm-loader.js'
node "$exp/run.cjs" verify
node "$exp/score.cjs"
git log -1 --format=%H -- "$exp/report.md"
```

For fresh model execution, use a separate checkout at the delivery commit. In that checkout set aside the three condition output trees and runs/reviews, then run `node "$exp/run.cjs" development`, `node "$exp/run.cjs" evaluation`, and `node "$exp/independent-review.cjs"`. Review the new exact assertion and evidence matches before rescoring. Existing attempt slots are retained. A partially written slot is intentionally not overwritten. The delivered raw results are the authoritative replay inputs. Provider sampling is uncontrolled. No Compose call is needed. The new natural briefs were prepared via `generate.cjs`, with the exact handoffs archived. Do not regenerate or re-freeze them for replay.
