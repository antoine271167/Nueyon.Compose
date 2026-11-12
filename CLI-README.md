# Nuëyon.Compose CLI

From the repository root in PowerShell, build once and use `--no-build` for subsequent
invocations. Discovery and help do not load configuration, prompts, DI, or credentials:

```powershell
dotnet build .\src\Nueyon.Compose.slnx
dotnet run --no-build --project .\src\Nueyon.Compose.Host.Console -- --help
dotnet run --no-build --project .\src\Nueyon.Compose.Host.Console -- describe --format json
dotnet run --no-build --project .\src\Nueyon.Compose.Host.Console -- agents list
dotnet run --no-build --project .\src\Nueyon.Compose.Host.Console -- agents describe idea --format json
dotnet run --no-build --project .\src\Nueyon.Compose.Host.Console -- agents schema compose --format json
dotnet run --no-build --project .\src\Nueyon.Compose.Host.Console -- run --help
dotnet run --no-build --project .\src\Nueyon.Compose.Host.Console -- run workflow --help
```

Commands (help, dispatch, defaults and JSON metadata come from shared definitions):
`run workflow`, `run <agent>`, `agents list`, `agents describe <agent> [--format text|json]`,
`agents schema <agent> --format json`, and `describe --format json`.

`describe --format json` is the global discovery document: every command (literal
`path`, whether it takes an agent, options with accepted `formats`/applicable `agents`),
every agent with its input, result and trace schemas and machine-readable `validation`,
the `--help` convention and no-arguments workflow default, exit codes, prompt defaults,
and runtime/credential requirements. `agents schema <agent>
--format json` emits only the standalone, documented input schema; `agents describe
<agent> --format json` adds the result schema, trace schema (Idea), validation and the
commands that apply to that agent.

The JSON discovery commands write only JSON to stdout; diagnostics go to stderr.
Agent names are case-insensitive. Exit codes: `0` success, `2` usage or input error,
`1` runtime or configuration failure, `130` cancellation. Without arguments (the legacy
workflow), errors go to stderr and input errors keep their original exit code `1`.

No arguments and `run workflow` run the existing production workflow, which reads
`data\input` by default or `--input-directory <dir>`. Input errors exit `1` without
arguments and `2` with explicit `run workflow`. For example, the following invoke model
services and may incur costs:

```powershell
dotnet run --no-build --project .\src\Nueyon.Compose.Host.Console -- run workflow --capture-inputs .\captures
dotnet run --no-build --project .\src\Nueyon.Compose.Host.Console -- run idea --input .\idea-input.json --output .\idea-result.json --trace .\idea-trace.json
```

`run` accepts `idea`, `research`, `synthesis`, `narrative`, or `compose`. Use
discovery for the actual input and result schemas. Input is UTF-8 JSON with
case-sensitive camelCase properties; constructor fields must be present and
non-null, unknown properties are rejected, and enums use the exact exported
string values (not numbers or other casings). A minimal Idea input is
`{"content":"source text"}`. The output is the actual agent result, not a workflow
wrapper; Idea returns an array in ranked order, whose first element is the one the
workflow selects.

The production Idea parser can keep a missing or null `evidence` on the selected idea,
and it reaches Research, Synthesis and Narrative. Captured inputs therefore contain
`"evidence": null`, and replay accepts a missing or null `evidence` (and only that)
wherever an idea appears; all other fields stay required and non-null. Alternative
ideas in the Idea result may contain null fields, which that output schema advertises;
other result schemas are strict.

The Idea agent uses the production structural and semantic validation loop: the
first idea must pass both; alternatives are not judged. It can make at most **two
generator and two semantic judge calls per source**, plus transport retries (the
discovery `validation` object exposes this as `maxAttempts`). Do not invoke the
workflow or an agent without appropriate cost authorization.

Every Idea retry starts from the original source and task, the previous first
idea when it can be parsed, and the latest diagnosis. The semantic judge
independently evaluates the current first idea against the original source.
Exhausting the attempts fails the Idea stage before Research can run.

### Idea workflow

One production prompt set lives in `Prompts/Idea/`. Structural and semantic failures
share a single repair budget: two generations total, with a mandatory judge.
Blockers are limited to `sourceContradiction`, `unsupportedClaim`, `editorialPurpose`
and fields `title`, `description`, `audience`, `rationale`, `evidence`.
A failed Idea stage shows the category, field and problem of the last blocker;
full feedback is available in the optional trace. There is no mode setting.

The 12 November Luna comparison yielded three usable final ideas, including one
repair of case 010. This small assistant-reviewed sample does not establish general
judge accuracy or downstream quality. See [experiment lessons](docs/idea-experiment-lessons.md).

Custom Idea validation prompts and judge fixtures must use
`{"accepted":true,"blockingIssues":[]}` for acceptance. Rejection uses
`accepted: false` and one to three blockers, each with `category`, `field`,
`problem`, `sourceEvidence`, and `constraint`. Old `problem`/`correction`
verdicts fail validation.

`--capture-inputs <dir>` is `run workflow`-only and creates a unique `run-<id>` child
containing `<agent>.json` at each agent boundary. `--trace <json>` is Idea-only.
The trace schema is included in Idea JSON discovery. Its `status` is `accepted`,
`exhausted`, or `incomplete`; `attempts` contain the raw generator `response`,
one-based `attempt`, zero-based `retry`, `status` (`accepted`, `rejected`, or
`incomplete`), and nullable validation `feedback`. Semantic rejection feedback is
a JSON-encoded blocker array inside the existing string field. Consumers that
parse the previous correction text must update. A model/judge failure can leave
an incomplete attempt or an empty list. An accepted first idea is available in
the result file; rejected feedback is recorded only in the opt-in trace, never in
error messages or default logs. Top-level `accepted` means the result was published;
a per-attempt `accepted` only means that generation passed validation. The accepted
trace is written after the atomic result publication, which is the commit point (it is
finished even if Ctrl+C arrives afterwards). A trace for a failure before publication
is best effort and never replaces the original failure or cancellation. If only the
trace write fails after publication, the result is kept, stderr reports
`The result was written, but the requested trace could not be written.`, and the exit
code is `1`.

**Diagnostics:** configuration problems (missing/malformed `appsettings*.json`,
missing `OpenAI:ApiKey` or `OpenAI:Model`) print an actionable `Configuration error`
on stderr, exit `1`. Other runtime failures print only a sanitized stage and category
(for example `Runtime error in stage 'research': the model service request failed
(HTTP 429)`), never keys, prompts, full model output, the source or exception messages.
The one exception is a typed Idea validation stop: it prints a single bounded line
(300 characters per field) with the judge's category, field and `problem`, and that
text is model-generated, so treat stderr from such a run as potentially sensitive.
Ctrl+C is
installed before input is read; an interrupted run exits `130` and does not report
success before the result is written.

**Privacy:** input, captures, traces, prompts, and results can contain sensitive
source and generated content. Store them in access-controlled local locations,
exclude them from version control, and delete them when no longer needed. Neither
capture nor trace is enabled by default. Result and trace paths must not exist;
files are written via a temporary file and an atomic no-overwrite move. Capture
files also never overwrite existing files.

Both runtime commands accept `--prompts <dir>` for a complete prompt tree. It is
strict: every embedded prompt, including the Idea judge prompt, must exist there;
no disk/embedded mixing occurs, and an explicit directory never falls back. The single production tree contains no profile subdirectories. Without an explicit override, the host uses
`src\Nueyon.Compose.Application\Prompts` when launched from the repository root,
otherwise the `Prompts` directory next to the executable if present, otherwise
embedded prompts. Disk templates are read on each process execution. To pick up
edits while using `--no-build`, run from the repository root or pass `--prompts`
explicitly. Configuration first uses `appsettings.json` in the working directory,
then the repository host project, then the executable directory. Normal runtime
configuration and credentials are still required for model calls. Runtime commands
report the selected source on stderr before running: `Prompts: disk (<resolved path>).`
or `Prompts: embedded (no disk prompt directory found).` Discovery and help report nothing.
