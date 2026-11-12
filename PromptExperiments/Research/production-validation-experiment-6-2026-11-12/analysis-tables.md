## Geblindeerde scores na ontsleuteling

| Cel | Research-pass | Bruikbare publicatie | Stijl /6 | Brontrouw eind /2 |
|---|---:|---:|---:|---:|
| 005-r1-A | False | False | 4 | 2 |
| 005-r1-B | False | False | 4 | 1 |
| 005-r2-A | True | False | 4 | 2 |
| 005-r2-B | True | False | 4 | 2 |
| archive-r1-A | False | False | 4 | 0 |
| archive-r1-B | False | False | 6 | 1 |
| archive-r2-A | False | False | 4 | 1 |
| archive-r2-B | False | False | 5 | 1 |
| harbor-r1-A | True | True | 5 | 2 |
| harbor-r1-B | False | False | 4 | 1 |
| harbor-r2-A | False | False | 5 | 1 |
| harbor-r2-B | False | False | 5 | 1 |

## Stagekosten

| Arm / stage | Calls | Input | Output | Totaal |
|---|---:|---:|---:|---:|
| A / Research | 6 | 69,964 | 15,628 | 85,592 |
| A / Synthesis | 6 | 34,831 | 8,697 | 43,528 |
| A / Narrative | 6 | 45,465 | 4,683 | 50,148 |
| A / Compose | 6 | 36,163 | 4,880 | 41,043 |
| B / Research | 6 | 15,684 | 10,714 | 26,398 |
| B / Synthesis | 6 | 32,648 | 10,744 | 43,392 |
| B / Narrative | 6 | 45,544 | 4,287 | 49,831 |
| B / Compose | 6 | 36,203 | 4,766 | 40,969 |
| evaluation / evaluation | 6 | 64,357 | 27,965 | 92,322 |

## Herhalingen en variantie

| Case / arm | Research pass /2 | Eind pass /2 | Ketentokens r1/r2 | Stijl r1/r2 |
|---|---:|---:|---|---|
| 005 / A | 1 | 0 | 36,866 / 35,697 | 4 / 4 |
| 005 / B | 1 | 0 | 25,663 / 26,618 | 4 / 4 |
| harbor / A | 1 | 1 | 38,191 / 38,380 | 5 / 5 |
| harbor / B | 0 | 0 | 27,040 / 29,026 | 4 / 5 |
| archive / A | 0 | 0 | 34,943 / 36,234 | 4 / 4 |
| archive / B | 0 | 0 | 25,762 / 26,481 | 6 / 5 |

## Scores per dimensie (gemiddelden, geen intervalschattingen)

| Dimensie | A | B |
|---|---:|---:|
| sourceFidelity | 1.83 | 1.67 |
| essentialCoverage | 1.33 | 0.83 |
| epistemicConsistency | 1.83 | 1.67 |
| projectionIntegrity | 1.67 | 1.50 |
| downstreamRetention | 1.67 | 1.17 |
| hook | 1.33 | 1.50 |
| readability | 2.00 | 2.00 |
| engagement | 1.00 | 1.17 |
| finalFidelity | 1.33 | 1.17 |

## Bronlabels per cel en stage

P=present, ~=partial, –=absent, X=contradicted. Volgorde Research / projectie / Synthesis / Narrative / Compose.

### 005-r1-A

- outcome: P / P / P / P / P — Retains the observed hierarchy failure, lower mean scores, non-promotion, and rollback; the Compose text bounds the finding to these runs.
- variation: – / – / – / – / – — Does not preserve the source’s specific point that Idea and Research outputs varied between runs.
- time: ~ / ~ / ~ / – / – — Mentions possible backend change, but omits the Nov. 3 versus Nov. 4 comparison dates in the stages where this label is required.
- tension: P / P / – / – / – — Research and projection retain the possible Synthesis tension and its uncertainty; Synthesis omits it. Later stages are outside this label’s required scope.

### 005-r1-B

- outcome: P / P / P / ~ / ~ — Research through Synthesis include the score declines and decision. Narrative and Compose retain the hierarchy failure and non-promotion but omit the lower mean scores, an important part of the selected result.
- variation: – / – / – / – / – — Does not preserve the source’s specific point that Idea and Research outputs varied between runs.
- time: P / P / P / – / – — Required stages retain the unequal dates and possible backend change; downstream stages are outside this label’s required scope.
- tension: P / P / P / – / – — Required stages preserve the competing Synthesis emphasis and note that its contribution was not tested; downstream stages are outside this label’s required scope.

### 005-r2-A

- outcome: P / P / P / P / P — Accurately retains the observed gate failures, lower candidate scores, non-promotion, and restoration; the final post limits its conclusion to these runs.
- variation: P / P / – / – / – — Research and projection note variation in Idea and Research outputs; Synthesis and later stages omit it.
- time: P / P / ~ / – / – — Research and projection report the dates and possible backend change. Synthesis mentions possible backend change but not the unequal dates.
- tension: P / P / P / – / – — Research, projection, and Synthesis preserve the existing Synthesis tension as context rather than established cause; later stages omit it.

### 005-r2-B

- outcome: P / P / P / P / P — Carries the observed lower scores, hierarchy/restraint failures, non-promotion, and restoration while keeping the conclusion specific to the comparison.
- variation: P / P / – / – / – — Research and projection report upstream Idea/Research variation; Synthesis and later stages omit it.
- time: P / P / ~ / – / – — Research and projection give the unequal dates and possible backend change. Synthesis retains backend uncertainty but omits the date difference.
- tension: P / P / P / – / – — Research, projection, and Synthesis preserve the source's mixed Synthesis signal and do not claim it caused the result; later stages omit this context.

### archive-r1-A

- dimensions: P / P / P / ~ / ~ — Research and Synthesis give the separate Cedar and Elm results. Narrative and Compose retain the difference between name accuracy and chronology but omit the exact reviewed counts and Rowan’s reviewed result.
- sample: ~ / ~ / ~ / ~ / ~ — The non-random accessible-box selection, limits, and 17-entry recheck are covered across the pipeline. The separate captain/owner-role correction is omitted.
- productivity: – / – / – / – / – — No stage gives the recorded 24-versus-15-minute comparison, its confounds and accounting exclusions, or the lack of a demonstrated tool-caused saving.
- logs: – / – / – / – / – — No stage explains that search counts use different logging rules or distinguishes searches from fulfilled requests.
- rights: P / P / ~ / ~ / ~ — Research and projection state the 760/140 boundary accurately. Later stages retain the 140 withheld-image fact but omit the 760-of-900 figure and the uncommitted review forecast.
- access: – / – / – / – / – — The pipeline omits the contrast result, the keyboard-only language-switch defect on two templates, and the fact that the planned repair was not deployed.
- decision: P / P / P / P / P — All stages distinguish approval to design an audit from an approved full-archive rollout and do not claim the audit is complete.
- meaning: ~ / ~ / – / – / – — Research/projection note that user effects are unmeasured, but omit the stated access commitment and the feedback evidence and its limitations. Later stages omit the label entirely.

### archive-r1-B

- dimensions: P / P / P / ~ / ~ — Research and Synthesis preserve the Cedar/Elm contrast and Rowan’s reviewed result. Narrative and Compose retain Cedar and Elm but omit Rowan’s result; the bounded sample and central distinction remain.
- sample: ~ / ~ / ~ / ~ / ~ — The non-random accessible-box sample and its limits are retained. Research and projection mention the 17-entry recheck, but omit the separate captain/owner-role error; Synthesis omits the correction and recheck details, and the final text does not specify them.
- productivity: – / – / – / – / – — No stage reports the 24-versus-15-minute recorded times, the material and accounting differences, or the lack of evidence for a tool-caused saving.
- logs: – / – / – / – / – — No stage preserves the non-comparable search logging rules or the distinction between searches and fulfilled requests.
- rights: P / P / ~ / ~ / ~ — The 760/140 boundary and withheld images are present. Synthesis, Narrative and Compose describe 760 scans as “vrijgegeven,” while the source states rights clearance is complete for 760; that wording overstates what the source explicitly establishes about public availability.
- access: – / – / – / – / – — The outputs omit the distinct accessibility findings: automated contrast passed, but the language switch remained unreachable on two templates and the repair was not deployed.
- decision: P / P / P / P / P — Every stage correctly describes approval to design a deeper audit, not approval of a full-archive rollout or a completed audit.
- meaning: – / – / – / – / – — The outputs omit the institution’s stated access commitment and the limits of unsolicited feedback, including that positive feedback can coexist with a factual error.

### archive-r2-A

- dimensions: P / P / P / P / P — The Cedar/Elm contrast preserves the distinction between ordering and factual description; the final also keeps the pilot’s limited scope.
- sample: P / P / P / P / P — The non-random accessible-box selection, fragile-box exclusion, and limited recheck are retained, including in the final.
- productivity: – / – / – / – / – — The recorded timing comparison and its confounds and limits are omitted. Research, projection and Synthesis are required stages for this label.
- logs: – / – / – / – / – — The non-comparable search logs and the distinct request counts are omitted. Research, projection and Synthesis are required stages for this label.
- rights: P / P / P / ~ / ~ — The 140 withheld images and their visible descriptions are correctly stated, but the final omits the 760 figure and the forecast/no-committed-date boundary.
- access: – / – / – / – / – — The contrast-scan pass alongside the unresolved keyboard-navigation defect is omitted.
- decision: P / P / ~ / ~ / ~ — The no-full-rollout decision and audit design are retained, but the later stages do not consistently specify that the proposed audit is random and stratified.
- meaning: ~ / ~ / – / – / – — Research and projection retain the explicit access commitment and unmeasured user effects, but omit the feedback/error coexistence; later stages omit this label. Only Research, projection and Synthesis are required.

### archive-r2-B

- dimensions: P / P / P / P / P — The output consistently distinguishes ordering, factual description, and image publication.
- sample: P / P / P / ~ / ~ — Research through Synthesis identify the accessible, non-random sample and its limits. Narrative and Compose refer to limited checks of accessible boxes but omit details such as the excluded fragile-binding boxes.
- productivity: – / – / – / – / – — The recorded timing comparison, confounds, and limits are omitted. Research, projection and Synthesis are required stages for this label.
- logs: – / – / – / – / – — The non-comparable search logs and the separate request-count caveat are omitted. Research, projection and Synthesis are required stages for this label.
- rights: P / P / ~ / X / X — Research and projection distinguish clearance from public downloadability. The final stages incorrectly imply that all 760 scans were publicly released; the source establishes completed rights clearance, not public availability for those scans.
- access: – / – / – / – / – — The contrast result alongside the unresolved keyboard-navigation defect is omitted.
- decision: P / P / P / P / P — The output accurately states that only designing the deeper stratified random audit was approved, not a full rollout.
- meaning: – / – / – / – / – — The explicit institutional access commitment, unmeasured user effects, and feedback/error coexistence are omitted. Only Research, projection and Synthesis are required.

### harbor-r1-A

- service: P / P / P / P / P — Correct counts, common completion definition, and the possibility of diesel-substituted completed trips are retained.
- punctuality: P / P / P / P / P — The different thresholds and absence of common-threshold reprocessing are accurately conveyed and kept separate from completion.
- comparison: P / P / ~ / ~ / ~ — Research and projection include route allocation and other comparison limits; later stages retain non-normalization and avoid causal attribution but omit some contextual detail.
- cost: P / P / P / ~ / ~ — The incomplete like-for-like cost boundary is retained, though the final text compresses the specific exclusions and temporary tariff.
- access: P / P / P / ~ / ~ — The final text preserves the split between wheelchair space and audio-announcement failures but omits the later repair status.
- status: P / P / P / P / P — The board’s authorization is correctly limited to preparing a winter-assessment specification, not conducting the assessment or approving procurement.
- feedback: P / P / – / – / – — Research and projection accurately describe the voluntary, non-representative cards; synthesis explicitly de-emphasizes this evidence and later stages omit it.
- meaning: P / P / – / – / – — Research and projection distinguish the operator’s attributed aspiration from measured environmental impact; synthesis and final stages omit the distinction.

### harbor-r1-B

- service: P / P / P / P / P — Correct completion counts and distinction between service completion and exclusive electric operation are retained.
- punctuality: P / P / P / P / P — The differing lateness thresholds and lack of a common reprocessing are accurately conveyed.
- comparison: P / P / ~ / ~ / ~ — Research and projection describe non-random allocation and the hill-route context; later stages retain the causal limitation but compress the comparison context.
- cost: P / P / P / ~ / ~ — The research and synthesis preserve the incomplete, non-like-for-like cost boundary; the final text states this generally.
- access: P / P / P / ~ / ~ — Research through synthesis accurately report the separate space and audio findings and later repair. The final stages mention inspections but omit the trial-time audio failures and repair.
- status: P / P / P / ~ / ~ — The board’s limited authorization is retained; the final text does not explicitly state that winter performance has not been tested.
- feedback: – / – / – / – / – — Volunteered rider cards and their non-representative limits are omitted.
- meaning: ~ / ~ / – / – / – — Research and projection note that net-carbon impact is unestablished, but omit the operator’s attributed cleaner-transport aspiration; later stages omit both.

### harbor-r2-A

- service: P / P / P / P / P — The correct completion figures and shared definition are retained, and the final post clarifies that the measure counts delivered trips rather than only electric-bus trips.
- punctuality: P / P / P / P / P — The threshold change and lack of a common-threshold reprocessing are retained.
- comparison: P / P / ~ / ~ / ~ — Route allocation and calendar/diversion differences are retained, but the synthesis and final stages omit the hill-route diesel improvement that provides important contextual comparison.
- cost: P / P / ~ / ~ / ~ — The final stages correctly reject a like-for-like total-cost comparison and mention the temporary tariff, but compress out the differing distance/traffic and specific excluded costs.
- access: P / P / P / ~ / ~ — The final post reports the two audio failures and the wheelchair-space finding, but omits the later repair status required alongside trial-time failures.
- status: P / P / P / P / P — The final post accurately limits the decision to preparing a winter-assessment specification and says it has not been carried out; it does not imply a fleet approval.
- feedback: P / P / – / – / – — Synthesis says rider cards can be de-emphasized but does not preserve the volunteered, potentially repeated, non-representative character required by this label.
- meaning: P / P / – / – / – — Research and projection preserve the attributed cleaner-transport aspiration and its measurement boundary; Synthesis and later stages omit that required distinction.

### harbor-r2-B

- service: P / P / P / P / P — The final-ledger comparison, shared completion definition, and distinction between service departures and exclusively electric departures are accurately represented.
- punctuality: P / P / P / P / P — Both narrative stages distinguish punctuality from completion and retain the changed time thresholds.
- comparison: P / P / ~ / ~ / ~ — The source’s non-random route allocation and contextual diesel improvement are retained in Research/Synthesis, but holiday and diversion differences are compressed out downstream.
- cost: P / P / P / ~ / ~ — The final post notes incomplete costs and the temporary tariff, but omits the different-distance/traffic basis and the emissions boundary; it does not claim savings.
- access: P / P / P / ~ / ~ — The final stages say only that three buses were inspected; they omit the two trial-time audio failures and later repairs, despite the source requiring both dimensions and repair status.
- status: P / P / P / P / P — The limited authorization for preparing a winter-assessment specification, rather than a fleet decision, is retained; winter was not tested.
- feedback: P / P / – / – / – — Synthesis explicitly de-emphasizes rider cards but does not preserve their voluntary, potentially repeated, non-representative nature; this label is not required in the final stages.
- meaning: P / P / – / – / – — Research and projection distinguish the operator’s cleaner-transport aspiration from measured impact, but Synthesis and later stages omit that attributed meaning; they make no unsupported environmental claim.
