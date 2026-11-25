# Operational cost and reliability

| Condition | Primary calls | Primary summed call time | Mean per primary call | Repeat calls | Repeat summed call time |
|---|---:|---:|---:|---:|---:|
| Experiment 20 Judge | 44 | 472.27 s | 10.73 s | 23 | 256.87 s |
| Mechanical only | 0 | 0 s | — | 0 | 0 s |
| Hybrid | 44 | 383.84 s | 8.72 s | 23 | 203.77 s |

Primary mechanics took 14.44 ms in aggregate for condition B. Paired evaluation wall time including repeats was 355.50 s with six slots in flight at most. Summed call time reflects individual CLI invocations and includes client startup and queue latency. The hybrid used smaller requests and recorded lower mean latency here, with the same call count and additional operator annotation. Annotation time was not measured. No reliable usage or credit figures were available. Monetary cost is unmeasured.

The 134 evaluation model attempts include repetitions. Development used another ten comparison calls. Corpus preparation took 30 production Narrative calls (158.50 summed seconds), and independent review took ten batched calls (125.13 summed seconds). These totals exclude the authenticated readiness probe.

The hybrid delivered all 67 held-out responses without transport or schema failures, with 21/23 exact repeat status agreement and 22/23 confirmed-hit agreement. The baseline had the same agreement counts. Its Homestead control repeat returned an invalid blocker quotation and became TECHNICAL_FAILURE. The original response was kept. There were no invalid JSON responses or transport failures in the comparison conditions. Decoder probes rejected malformed JSON, missing correction, an invented excerpt and an invalid status. They exercise the contract boundary without establishing production host reliability. A thrown orchestration or I/O error can stop the experiment runner and needs separate handling before any integration.

Primary review workload was 2/44 for the baseline, 1/44 for the hybrid and 44/44 for mechanics alone. The latter includes 31 acceptable briefs. Overall primary withholding on genuinely acceptable briefs was 1/31 for the baseline and 2/31 for the hybrid. [Raw scores](scores.csv) · [Raw fault probes](runs/fault-check.json)
