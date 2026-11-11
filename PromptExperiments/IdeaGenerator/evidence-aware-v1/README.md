# Evidence-aware idea experiment runner

From the repository root, build with `dotnet build .\src\Nueyon.Compose.slnx`. Run without credentials or network:

```powershell
dotnet run --project .\src\Nueyon.Compose.Experiments --no-build -- offline .\PromptExperiments\IdeaGenerator\evidence-aware-v1
```

Offline checks use a temporary root and fake client, exercise the production MAF loop and experimental runner, and write `verification\evidence.schema.json` and `verification\review.schema.json`. They never modify the real ledger. The baseline prompt snapshots are checked against embedded production resources. If a freeze exists it is also verified.

Execute one cell after the parent has frozen the experiment:

```powershell
dotnet run --project .\src\Nueyon.Compose.Experiments --no-build -- run <experimentRoot> <caseId> <A|B|C> <replicate>
```

Replicates are positive integers; case IDs contain letters, digits, `_` or `-`. `cases.json` is an array of `{id,split,source}`; source is a path relative to the experiment root. Labels and rubric are never loaded into requests. Exit code 0 means success or an already completed cell; 1 means blocked or an error. A started cell without a final result is never automatically retried; investigate interruptions and ledger reservations before any manual intervention.

Runs require `verification\frozen.json` with `{files:{relativePath:sha256hex},head,budget:112}`. Every listed hash and Git HEAD must match; relative paths may refer to repository files outside the experiment root. All snapshots in `prompts\baseline\Idea` must be byte-identical to the embedded production prompts. An exclusive `verification\run.lock` is held for the run. `verification\call-ledger.jsonl` is append-only, with one flushed `{ordinal,cell,utc}` reservation before each call. Every reservation counts, including failed or interrupted calls. The global limit is 112; per case/condition/replicate limits are A=6, B=1, C=1. Never reset the ledger.

All conditions use MAF over the recording `IChatClient`; A uses the unchanged production `IdeaAgent`, structural validator, semantic judge and retry loop. B uses `prompts\evidence.system.txt` and `evidence.user.txt`; C uses `review.system.txt` and `review.user.txt`, a fresh context, the original source and the exact successful B draft for the same case and replicate. Invalid B blocks C without a call. No automatic output repair is performed.

B requires `{ideas:[{title,description,audience,rationale,evidence}],grounding:[{field,claim,sourceEvidence,inference,constraints}]}`. C requires `{review:[{field,issue,sourceEvidence,action}],revised:<B result>}`. Objects reject duplicate, missing and unknown keys; all strings must be nonblank. There must be 1–3 ideas and 1–12 grounding records, covering all five first-idea fields; `field` is one of those five names. Review may be empty; its action is `keep` or `revise`. Every `sourceEvidence` must be a verbatim substring of the source. Matching a quotation does not prove that it entails a claim.

Records are under `runs\<case>\<replicate>\<condition>`:

- `started.json` is flushed before execution; `final.json` is atomically installed with `status` (`success`, `failed`, `blocked`), `ideas`, `grounding`, `review`, `revised`, `error`, `callCount`, `totalTokensUsed`, `durationMs`, `startedUtc`, and `completedUtc`. Unused fields are null. A uses domain ideas; B stores its experimental ideas and grounding; C also stores the full revised B-shaped object.
- A writes `attempts.jsonl` from the production observer. B writes the exact `draft.json`; C saves that draft and the full `reviewed.json`.
- Each global ordinal has `call-NNN\request.json` containing transport messages and options including schema; successful calls have `response.txt` and `metadata.json` (response/model IDs, usage, finish reason and timing); failures have `error.json`. The in-process observer delegate is excluded from recorded options. A process interruption can leave an incomplete call directory.

The backend uses Console `appsettings.json`, environment-specific settings, environment variables and user secrets from `typeof(Cli).Assembly`; `OpenAiOptions` is validated. The model is explicitly `gpt-5-mini`, SDK retries are disabled, network timeout is 150 seconds, and all calls use `MaxOutputTokens=6000` and no temperature. Credentials are not included in the recorded request. The runner has not been exercised against a live provider; provider-specific schema or service behavior remains unverified. Inputs and outputs may be sensitive and should be handled accordingly.
