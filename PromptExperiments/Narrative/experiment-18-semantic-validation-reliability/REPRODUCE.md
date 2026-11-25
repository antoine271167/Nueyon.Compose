# Reproduce Experiment 18

From the repository root, use Windows, Node.js 22.16.0 and an authenticated GitHub Copilot CLI 1.0.116 with GPT-6 Luna access. The validator and JSON parser are linked to adjacent archived Experiment 17. Verify with the archived runner and rescore without network calls.

```powershell
$exp = 'PromptExperiments/Narrative/experiment-18-semantic-validation-reliability'
$env:EXPERIMENT17_COPILOT_LOADER = Join-Path (Split-Path (Get-Command copilot.cmd).Source) 'node_modules/@github/copilot/npm-loader.js'
node -e "require('./$exp/run.cjs').verify()"
node "$exp/score.cjs"
node "$exp/report.cjs"
node "$exp/verify-results.cjs"
```

Use a separate checkout or copy of both experiment directories without the Experiment 18 `runs/` directory for fresh calls. Run `node "$exp/run.cjs"` once in that copy. The runner preserves any existing completed attempts, verifies frozen manifests before calling the model, and records what went to each session. Four workers run eight-call batches. Stop and keep the archives if transport failure is widespread. Provider defaults permit run-to-run differences. For a fresh run use `node "$exp/score.cjs" --inspect` and prepare a fresh `finding-review.json`, keyed by example, condition, repetition and one-based finding index. Provenance and ground truth remain frozen. Per-finding review booleans are confirmed, relevant, explained, typed and corrected. Exact quotes are checked mechanically against decoded input fields.

The delivery commit can be recovered with `git log -1 --format=%H -- "$exp/report.md"`. Raw per-call logs are under `runs/`. The initial repository commit was `d24f18a8a373e05c7a363901db43533ad74dd97d` and matched fetched origin/main. Dry-run push connected successfully. A separate model-only check yielded READY and is outside the 192 validator calls.

