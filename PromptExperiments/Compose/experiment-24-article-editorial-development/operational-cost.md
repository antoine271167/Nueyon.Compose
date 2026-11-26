# Operational measurements

All 36 development and all 126 holdout generation attempts were valid. All eight supplemental Narrative preparations succeeded. One separately reported production preflight generated an Article using the same configured alias. No Compose slot was replaced.

| Holdout latency in seconds | A | B | C |
|---|---:|---:|---:|
| Median | 4.063 | 4.080 | 4.140 |
| 90th percentile | 5.377 | 5.673 | 5.974 |

These are host-observed elapsed times, including process and CLI overhead, under a common interleaved queue with up to six concurrent Compose calls. The differences are below the frozen operational thresholds. Read [attempt records](outputs/) for individual seconds, status and any errors.

The production CLI uses its normal OpenAI configuration and contract and does not expose token use, provider retries, raw HTTP response bodies or amounts billed. Copilot reviewer calls were separate. No cost estimate is inferred from words, model aliases or elapsed time. A machine-held OpenAI credential and no overriding model alias were verified without logging secret values. Copilot CLI 1.0.116 returned READY using the selected review alias. The .NET SDK was 10.0.404 and Node was 22.16.0.

Editorial reviewers' named score objects and factual reviewers' label descriptions were often rejected by the runner's strict parser. Analysis reused Experiment 23's deterministic normalization on the archived JSON. Review parsing/coverage issues are distinct from valid Compose generation. The refusal to replace completed attempts preserved the original reviews. The first development review scans started before the last generation slot was fully recorded, so a follow-up scan filled the missing invocation for each review type. A concurrent factual follow-up encountered a second-pass directory already being written by the first scan. The runner refused that duplicate start, and the original attempt remained authoritative.

All 484 blind reviewer invocations returned exit code 0. Normalization recovered all editorial scores and at least one usable blind factual review for 124 of 126 holdout Articles. Thirteen holdout Articles lacked full required independent coverage and one development C Article lacked a complete usable pair. The two holdout Articles with no usable blind factual review were source-checked manually. See the [review operation counts](review-operations.json) and [manual coverage notes](manual-coverage.md).
