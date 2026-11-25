# Reproduce Experiment 15

Run the following commands from the repository root with Node.js.

```powershell
$experiment = 'PromptExperiments/Narrative/experiment-15-editorial-reassessment'
node "$experiment/inventory.cjs"
node "$experiment/inventory.cjs" evidence webb
node "$experiment/inventory.cjs" review webb
node "$experiment/summarize.cjs"
```

Repeat the evidence and review commands for cooling, digital, library, lowell and wolf. For an independent review, score against the frozen protocol before consulting the manifest, scores, reviewer notes, second review, or historical reports. The scoring view omits cohort labels. A fresh rating exercise requires human or reviewer judgment. The scripts verify the frozen text hashes and recalculate the recorded scores, counts and prompt hashes from the repository files. Pass `--write` to regenerate the two CSVs. Check that the generated CSVs are byte-identical to the tracked versions and that the summary matches the statistics in the decision file. Review the factual findings and report for the judgment behind the decision.

Source coverage and provenance are recorded in the manifest. The report makes the decision using the 36 A runs only. No model calls, new Narrative outputs, Compose runs or builds were required.
