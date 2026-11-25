# Editorial quality

| Criterion | Article mean | LinkedIn Post mean |
| --- | ---: | ---: |
| Opening or hook | 3.06 | 3.23 |
| Development or substance | 2.83 | 3.31 |
| Readability or pacing | 3.92 | 4.03 |
| Focus or progression | 4.19 | 4.26 |
| Ending | 2.44 | 2.71 |
| Voice | 3.22 | 3.49 |

Article averages 3.30 with median 3.35. LinkedIn Post averages 3.50 with median 3.45. The 17.3-point acceptance gap favors LinkedIn Post descriptively. Format-specific scoring and repeated observations on the same subjects limit interpretation. Both fall well short of the 80% gate.

The [fusion Article](outputs/article/fusion-2/result.json) uses the input and output energy figures to develop a concrete distinction, with readability and focus each 5. The [Python LinkedIn Post](outputs/linkedin/python-3/result.json) puts a behavior change up front and distinguishes declaring support from implementing it, earning 4.20. The [digital Article](outputs/article/digital-1/result.json) follows a clear sequence but gives the reader little beyond the summary, then repeats the design sequence and reserved research time. The [labor Article](outputs/article/labor-2/result.json) earned 3.95 and still failed because its ending scored 2.

Every Article subject had a failing draft, 12/12, and 11/12 LinkedIn subjects did. Only Python passed all three LinkedIn repetitions. Article within-subject swings reached 1.00 on fusion; LinkedIn reached 0.80 on GPS. Rich Articles averaged 3.20 with 0/18 passes, and constrained Articles averaged 3.39 with 2/18. Rich LinkedIn averaged 3.33 with 2/17 passes, and constrained LinkedIn averaged 3.65 with 6/18. Richness is confounded with topic and language. A one-Narrative-per-topic design cannot estimate a causal effect of richness.

The complete score distribution, criterion values, subject and richness tables, and repeat ranges are in [metrics](metrics.json). Editorial judgments come from separate machine sessions. Nine outputs gave explicit criterion keys instead of ordered arrays and were decoded by those exact keys.
