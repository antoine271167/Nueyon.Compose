# Operating measurements

The production CLI made 72 scheduled Compose attempts. Article returned 36/36 valid outputs with mean elapsed time 4.84 s and median 4.78 s. LinkedIn returned 35/36 valid outputs with mean 5.82 s and median 5.97 s, including the runtime failure. The combined elapsed process time was 383.65 s. Concurrency makes this sum different from wall-clock duration. Article averaged 112 words and LinkedIn 97 words by whitespace count.

LinkedIn Wolf repetition 1 exited with InvalidOperationException and produced no reader-facing result. The production host did not expose the underlying exception message or raw model response, so the cause is unclassified. The attempt remains in the denominator. No production attempt was retried.

Review invocations are counted in [metrics](metrics.json), including first and second factual sessions and parser differences. The authenticated review preflight is excluded from the review counts. Provider-side HTTP retries, tokens, usage-based cost and resolved provider model versions were unavailable through these CLI paths. No cost was estimated from word count.

Full solution test command returned exit 0. Application 241, Infrastructure 25 and Console 37 tests passed. The Domain test assembly reported no discoverable tests. The test run built the production and test projects.
