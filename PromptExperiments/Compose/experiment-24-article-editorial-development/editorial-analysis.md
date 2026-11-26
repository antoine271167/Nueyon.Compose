# Editorial analysis

| Holdout measurement | A | B | C |
|---|---:|---:|---:|
| Accepted | 2/42 (4.8%) | 0/42 (0.0%) | 1/42 (2.4%) |
| Mean weighted score | 3.20 | 3.09 | 3.14 |
| Median | 3.20 | 3.00 | 3.10 |
| Opening | 3.12 | 2.98 | 2.98 |
| Development | 2.50 | 2.45 | 2.43 |
| Readability | 3.95 | 3.76 | 3.86 |
| Focus | 4.05 | 3.93 | 4.02 |
| Ending | 2.29 | 2.29 | 2.38 |
| Voice | 3.26 | 3.14 | 3.24 |
| Scores below 3.0 / 3.0 to below 4.0 / 4.0 or above | 5 / 37 / 0 | 12 / 30 / 0 | 9 / 33 / 0 |
| Mean within-subject score variance | 0.016 | 0.044 | 0.029 |
| Mean words, including headings | 91.4 | 88.6 | 90.8 |
| Repetitive | 17/42 | 17/42 | 14/42 |
| Generic opening | 7/42 | 5/42 | 5/42 |
| Generic ending | 22/42 | 19/42 | 16/42 |
| Report-like voice | 18/42 | 21/42 | 19/42 |
| Thin substance | 42/42 | 42/42 | 42/42 |

B declined by 0.108 in mean score and by 4.8 percentage points in acceptance. C declined by 0.062 and 2.4 points, respectively. C improved ending by 0.095 and had fewer generic-ending tags, short of the 0.40-point ending target. B's 0.190 decline in readability was close to the predefined material-regression boundary. The bootstrapped 95% subject-cluster intervals for weighted score were [-0.242, +0.007] for B and [-0.174, +0.040] for C. Acceptance intervals were [-11.9, 0] and [-7.1, 0] percentage points. [Metrics](metrics.json) preserve each exact subject/repetition pair, the cohort breakdown and leave-one-subject-out results.

The fresh set averaged 3.158, 3.006 and 3.042 for A, B and C, respectively, with one acceptance under A alone. The five historical stress subjects averaged 3.163, 2.957 and 3.160. None of their articles passed. The three other archived subjects averaged 3.350, 3.494 and 3.300. Rich inputs produced mean development scores of 2.61, 2.33 and 2.33. The corresponding constrained scores were 2.42, 2.54 and 2.50. Neither the subgroups nor the paired analysis support an editorial advantage across subjects.

## Passages

- The accepted A Hope Diamond piece, 3.80, advances from a mundane package to an unusual price split: “The postage cost $2.44, and registration cost $142.85.” The paired B piece keeps the same detail, but the reviewer scored it 3.00. Small changes in the surrounding prose and reviewer variation can matter as much as a single effective sentence. [A](outputs/holdout/hope-diamond-mail-2-A/result.json) · [B](outputs/holdout/hope-diamond-mail-2-B/result.json)
- B's stronger highway piece, 3.70, uses specific developments: “The 1944 Act authorized designation of 40,000 miles for a national interstate network.” The final paragraph recaps the three distinctions already stated in the opening. This is a stronger-than-average B article that still misses acceptance. [B](outputs/holdout/highways-1-B/result.json)
- The 2.65 B Stonewall article closes with “The events took place around the inn, the park, and the surrounding area.” The preceding paragraph has already located the demonstrations. The limited brief offers few further supported developments, and the new paragraph uses none. [B](outputs/holdout/stonewall-park-2-B/result.json)

Experiment 23 supplied historical context at 112 mean words and 3.30 mean editorial score. The contemporaneous paired baseline is the causal comparator. The paired candidates were about as short as A. The fresh articles averaged roughly 65–67 words per condition, reflecting, in part, the source scope used to prepare their Narratives. Counting words separately prevents this difference from substituting for reader quality.
