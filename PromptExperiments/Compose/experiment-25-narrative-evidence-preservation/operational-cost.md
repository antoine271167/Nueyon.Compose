# Operational cost and reliability

| Stage | Started | Valid raw | Invalid raw | Interrupted | Process seconds mean / median |
| --- | --- | --- | --- | --- | --- |
| evidence-inventories | 36 | 36 | 0 | 0 | 13.4 / 13.2 |
| narratives | 36 | 36 | 0 | 0 | 5.9 / 5.4 |
| outputs | 152 | 152 | 0 | 0 | 4.5 / 3.4 |
| reviews | 529 | 439 | 90 | 0 | 19.4 / 18.3 |
| recovery | 6 | 0 | 6 | 0 | 0.2 / 0.2 |

Narrative and Compose are separate production CLI processes. The outputs folder counts development Article, holdout Article and LinkedIn. C references are operator-generated files and use no production model slot. The six recovered input casing rejections are accounted for separately and stopped before provider invocation. The named raw status counts include review JSON mismatches that deterministic normalization could recover. Raw failures and review-only secondary calls remain in the archive.

The holdout Narrative median CLI times were 5.4 seconds for A and 6.2 seconds for B. Process time includes application startup and provider latency. Billed cost, tokens and provider build were unavailable. The preflight GPT-6 Luna Narrative and Copilot calls were outside the measured plan.

The solution built successfully and 303 discovered tests passed. The Domain.Tests project contained no discovered tests. See [build](build.log) and [tests](tests.log).
