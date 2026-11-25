# Reproduction

Use the repository revision and tool versions in model-configuration.json. Build with dotnet build src/Nueyon.Compose.slnx --nologo. Confirm the Narrative single-agent route using dotnet <console-dll> run narrative --input <input.json> --output <output.json>. The three source-grounded production runs are archived under evaluation. Do not regenerate them to reproduce Phase A on the frozen inputs.

From PowerShell at repository root, use the following commands in a separate copy. Preserve this delivery's raw runs. In the separate copy remove only the four generated run trees claim-extractions, evidence-alignments, judge-runs and evaluation/reviews before rerunning. setup.cjs documents construction of the frozen inputs. It is not required for replay.

~~~
$exp = 'PromptExperiments/Narrative/experiment-21-evidence-first-narrative-judge'
$env:EXP21_COPILOT_LOADER = Join-Path (Split-Path (Get-Command copilot.cmd).Source) 'node_modules/@github/copilot/npm-loader.js'
node "$exp/run.cjs" verify
node "$exp/run.cjs" development
node "$exp/run.cjs" evaluation
node "$exp/review.cjs"
node "$exp/usage.cjs"
~~~

Fresh runs use provider defaults. Re-adjudicate fresh findings by reading exact quotes against the complete brief and source. The original study's match decisions and grades apply to the archived outputs. Recover the delivery commit with git log -1 --format=%H -- "$exp/report.md".
