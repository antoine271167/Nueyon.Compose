# Reproduction

Use base 1e7f183911115146e62244858c7f24e242597347, .NET SDK 10.0.404, Node.js 22 and the normal console configuration and credentials for GPT-6 Luna. From the repository root, build with `dotnet build src/Nueyon.Compose.slnx --nologo`.

The immutable cohort is in `run-manifest.csv`. `freeze.sha256` covers the frozen prompt trees, cases, manifest, configuration, protocol, hypothesis and runner. The runner rechecks those hashes before and after the cohort. Run `node <experiment-dir>/run.cjs` from a fresh copy of the repository and experiment directory with no existing `runs` directory to generate fresh outputs. Each invocation of the application uses `run narrative` with the manifest input and matching external prompt tree. The preflight output is a separate smoke test. Reproduction generates new samples and can yield different text.

To inspect the archived brief text with arms hidden, run `node <experiment-dir>/review.cjs <subject>`. Review against that subject's source and exact CLI input. `blind-assessments.json` records the assessment by opaque ID. `node <experiment-dir>/evaluate.cjs` verifies quoted excerpts and original result payloads, joins the frozen manifest, recomputes the scores and applies the gates.

The original fixture bytes are preserved by the experiment's Git attributes. The frozen protocol's two question marks before the editorial cutoffs are an encoding artifact for “at least”. The assignment, frozen 20/24 promotion threshold, and scoring implementation apply 3.50 or higher and all three core criteria at 3 or higher.

