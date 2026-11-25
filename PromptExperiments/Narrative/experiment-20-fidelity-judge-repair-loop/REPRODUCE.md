# Reproduction

Use the .NET SDK, Node and authenticated Copilot CLI versions in model-configuration.json. Fetch the source commit and build the solution from that checkout or from this experiment's delivery commit. The current Narrative CLI uses the hashed production disk prompts. Copilot uses fresh sessions and provider defaults. New generations can differ.

Run the following from the repository root in PowerShell. Verification reads the frozen snapshots and production inputs. The delivered corpus has already been prepared. Use a separate copy without output directories or derived scoring files for a fresh execution. Keep delivered evidence intact. Re-review and create new evidence adjudications and match decisions for fresh runs.

```powershell
$exp20 = 'PromptExperiments/Narrative/experiment-20-fidelity-judge-repair-loop'
$env:EXP20_COPILOT_LOADER = Join-Path (Split-Path (Get-Command copilot.cmd).Source) 'node_modules/@github/copilot/npm-loader.js'
dotnet build src/Nueyon.Compose.slnx --nologo
node "$exp20/run.cjs" verify
node "$exp20/run.cjs" loop
node "$exp20/run.cjs" review
node "$exp20/score.cjs"
```

The runner resumes completed call slots from archived attempts and logs every call in terminal output. Malformed model output fails the corresponding experimental pair or review slot. Reassessment judges only the current full brief with original evidence. The report separates the independent review calls from operational Judge and repair overhead.

The initial archive fixtures were copied from the paths in archived-index.csv. The eight new handoffs are in new-handoffs.json. To re-create the freeze, use the eight source snapshots, archive references and named inputs in a clean experiment directory, then run `node "$exp20/run.cjs" prepare` before executing the loop.

Recover the delivery commit with `git log -1 --format=%H -- "$exp20/report.md"`.
