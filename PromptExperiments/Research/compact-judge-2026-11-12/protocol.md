# Research compact + independent judge

Frozen before calls, 2026-11-12. No Copilot CLI; no commit or push.

Hard budget: 12 HTTP requests, SDK retries disabled, no reruns. Model gpt-6-luna, matching current host configuration. Calls 1-3: judge frozen baselines. Calls 4-6: judge minimally corrected controls. Offline calibration means frozen artifacts with no retrieval or workflow generations; these assessments still require model calls. Gate: detect each named baseline defect with faithful source anchors; accept corrected controls without rejecting correct facts. Inspect diagnoses separately from acceptance flags.

Only after calibration passes: calls 7-9 compact Research with identical frozen source/selected-Idea pairs; 10-12 same judge on candidates. Gate: all known defects resolved, no material new fidelity/epistemic error or essential downstream loss. Dimensions fidelity, coverage, uncertainty, usability each scored 0 (material defect), 1 (limited), 2 (adequate). Judge rubric generic and frozen before calibration; no case labels or expected answers supplied. Controls preserve all baseline facts and only address known omissions/qualification.

No live repair or Synthesis calls; repair integration verified with mocks. Insufficient calibration prevents judge activation; insufficient candidate quality prevents prompt promotion. Small unblinded sample, no independent human labeling, statistical reliability or downstream editorial-quality claim. Baseline/candidate generations are not interleaved; temporal backend variation remains a confounder. Report actual tokens, judge overhead separately, costs only if reliable.
