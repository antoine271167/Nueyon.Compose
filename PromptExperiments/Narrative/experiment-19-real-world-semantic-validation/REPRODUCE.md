# Reproduction

From the repository root, use .NET SDK 10.0.404, Node 22.16.0 and authenticated GitHub Copilot CLI 1.0.116 with GPT-6 Luna access. The source base is recorded in model-configuration.json. Check hashes with the archived runner and build the solution before fresh generation. Use a separate checkout for fresh runs and preserve all delivered outputs.

```powershell
$exp = 'PromptExperiments/Narrative/experiment-19-real-world-semantic-validation'
$env:EXPERIMENT17_COPILOT_LOADER = Join-Path (Split-Path (Get-Command copilot.cmd).Source) 'node_modules/@github/copilot/npm-loader.js'
node "$exp/run.cjs" verify
dotnet build src/Nueyon.Compose.slnx --nologo
```

For a fresh study, retain the frozen input artifacts and runner. Remove run archives and the original ground-truth artifacts from the separate copy only. Invoke the narrative stage, independently review the new results and write fresh ground-truth.json, then invoke the seal and validator stages in that order. Each CLI invocation starts independently. Provider defaults permit new samples to differ. Keep supplementary archived results and judgments distinct.

The preflight confirmed clean main at fetched origin/main, successful GitHub read and dry-run push, a clean SDK build, the Narrative single-agent CLI and a complete smoke result with production disk prompts. The authenticated Experiment 18 CLI returned READY using gpt-6-luna and passed the archived freeze verification. The two probes are excluded from acceptance.

The delivery SHA can be recovered with `git log -1 --format=%H -- "$exp/report.md"`.
