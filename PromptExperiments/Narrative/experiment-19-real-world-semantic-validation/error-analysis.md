# Error analysis

## Natural outputs

The independent pre-validator audit classified 35 outputs F0, one F1 and zero F2. Ground truth and 36 original Narrative files were sealed before any validator attempt. The same AI operator prepared the handoffs, audited the briefs and later adjudicated validator findings. A second reviewer was unavailable. The validator itself ran in fresh GPT-6 Luna sessions and had no access to labels. The resulting detection denominators are empty.

- **Parker-3, F1.** “Its observations confirmed two types of slow solar wind” immediately follows the description of the specific close pass. It could attribute the confirmation to that encounter. The later progression separately describes classification and WISPR images, and the brief preserves uncertainty about origins. Prefer mission-level attribution for the wind classification. Both attempts returned PASS, consistent with the rubric's treatment of minor imprecision.
- **Acid-1 and Coral-1, F0.** REVIEW messages highlighted real omissions in the supplied packets. The EPA page explicitly describes Phase II expansion. NOAA identifies the prior record and the satellite product. The ground-truth classification uses those original passages. These flags count toward workload. [Source coverage](source-coverage.md)
- **Labor-2 and Python-1/2, F0.** Context from the originals supports browsing jobs as a passive search example and distinguishes declaring free-threading compatibility from ensuring thread safety. The validator raised support questions about those statements under the supplied evidence.
- **Fusion-1, F0.** One validator attempt overread the ordinary phrasing “after delivering 2.05 megajoules to the target” as a precise claim about whether energy delivery and fusion output overlapped. The brief gives the correct comparison and preserves the input/output boundary. The other attempt accepted it.
- **GRACE-FO.** The briefs kept the spacecraft distance distinct from altitude and distinguished inferred water movement from direct imaging. All six attempts accepted this treatment.

## Archived regressions

Five previously generated outputs are taken from Experiment 17's source adjudication. The set covers elk species identity, Webb cooldown chronology, FAA wrong-surface terminology, an energy-scale translation and the refund team's sprint chronology. The refund example uses the later Experiment 17 F2 adjudication, which superseded Experiment 16's F1 label for this brief. The archived briefs may have appeared in earlier validator studies. This regression set has known positive selection and includes Dutch briefs. Its scores give context about known failure modes and are excluded from the primary metrics.

Machine-readable findings, quote checks, and manual evidence-quality judgments appear in finding-scores.csv. Exact quotes alone are insufficient for actionable findings.
