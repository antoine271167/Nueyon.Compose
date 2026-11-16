# Bron-naar-Research traceability matrix

Alle citaten hieronder zijn exacte substrings, automatisch tegen de oorspronkelijke bestanden gecontroleerd. L = één-gebaseerd regelnummer. Research-ankers tonen de volledige oorspronkelijke regel. Geen anker betekent geen citaat, geen verzonnen leegtebewijs. De regex in audit.cjs helpt ankers verzamelen; semantische classificatie en materialiteit staan afzonderlijk vermeld. Bestandspaden zijn relatief aan Experiment 7.

## 005 / outcome — kern

Bron [L4](../production-validation-experiment-6-2026-11-12/cases/005/source.md#L4): “Candidate not promoted; production Narrative user prompt restored byte-for-byte to baseline.”

Volledige oorspronkelijke bronregel (context en overige componenten):

> **Decision:** Candidate not promoted; production Narrative user prompt restored byte-for-byte to baseline.

| Run | Classificatie | Research / projectie |
|---|---|---|
| 005-r1-A-Research | correct | ruw L39, 66; projectie L39 |
| 005-r1-B-Research | correct | ruw L8, 23; projectie L9 |
| 005-r2-A-Research | correct | ruw L33, 58; projectie L33 |
| 005-r2-B-Research | correct | ruw L9, 24; projectie L10 |

### 005-r1-A-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/005-r1-A-Research/content.md)

- L39: 37. The decision was not to promote the candidate.
- L66: - The candidate was not promoted, and the production prompt was restored byte-for-byte to baseline (Facts 37–39).
[projection.md](../production-validation-experiment-6-2026-11-12/calls/005-r1-A-Research/projection.md)

- L39: 37. The decision was not to promote the candidate.

### 005-r1-B-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/005-r1-B-Research/content.md)

- L8: 7. The decision was not to promote the candidate. The production prompt was restored byte-for-byte; its SHA-256 matched the frozen baseline (“Decision”; “Rollback verification”).
- L23: 3. The candidate was not promoted and the production prompt was restored to the baseline bytes (Fact 7).
[projection.md](../production-validation-experiment-6-2026-11-12/calls/005-r1-B-Research/projection.md)

- L9: 7. The decision was not to promote the candidate. The production prompt was restored byte-for-byte; its SHA-256 matched the frozen baseline (“Decision”; “Rollback verification”).

### 005-r2-A-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/005-r2-A-Research/content.md)

- L33: 31. The report’s decision was not to promote the candidate.
- L58: 5. The candidate was not promoted, and the production Narrative user prompt was restored byte-for-byte to baseline.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/005-r2-A-Research/projection.md)

- L33: 31. The report’s decision was not to promote the candidate.

### 005-r2-B-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/005-r2-B-Research/content.md)

- L9: 8. The decision was not to promote the candidate. The production Narrative user prompt was restored byte-for-byte to baseline; its restored SHA-256 matched the frozen baseline hash. (Decision; Build, tests, and promotion decision; Rollback verification)
- L24: 3. The candidate was not promoted, and the production prompt was restored byte-for-byte to baseline (Fact 8).
[projection.md](../production-validation-experiment-6-2026-11-12/calls/005-r2-B-Research/projection.md)

- L10: 8. The decision was not to promote the candidate. The production Narrative user prompt was restored byte-for-byte to baseline; its restored SHA-256 matched the frozen baseline hash. (Decision; Build, tests, and promotion decision; Rollback verification)
## 005 / scores — kern

Bron [L59](../production-validation-experiment-6-2026-11-12/cases/005/source.md#L59): “Candidate mean 4.00/10”

Volledige oorspronkelijke bronregel (context en overige componenten):

> **Narrative total:** Baseline mean 6.00/10 (range 5–7; sample SD 1.00). Candidate mean 4.00/10 (range 4–4; sample SD 0.00). Candidate outputs were readable and coherent, but consistently subordinated company identity and audience scope while making orchestration the central positioning. Candidate samples also introduced unsupported validation or audience framing.

| Run | Classificatie | Research / projectie |
|---|---|---|
| 005-r1-A-Research | correct | ruw L13, 14, 15, 16; projectie L13, 14, 15, 16 |
| 005-r1-B-Research | correct | ruw L4; projectie L5 |
| 005-r2-A-Research | correct | ruw L20, 21; projectie L20, 21 |
| 005-r2-B-Research | correct | ruw L4; projectie L5 |

### 005-r1-A-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/005-r1-A-Research/content.md)

- L13: 11. The baseline Narrative mean score was 6.00/10.
- L14: 12. The candidate Narrative mean score was 4.00/10.
- L15: 13. The baseline Compose mean score was 5.67/10.
- L16: 14. The candidate Compose mean score was 3.33/10.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/005-r1-A-Research/projection.md)

- L13: 11. The baseline Narrative mean score was 6.00/10.
- L14: 12. The candidate Narrative mean score was 4.00/10.
- L15: 13. The baseline Compose mean score was 5.67/10.
- L16: 14. The candidate Compose mean score was 3.33/10.

### 005-r1-B-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/005-r1-B-Research/content.md)

- L4: 3. Under the stated 0–10 rubric, candidate mean scores were lower than baseline for Narrative (4.00 vs. 6.00) and Compose (3.33 vs. 5.67). The report characterizes these as scores of the saved outputs under its rubric (“Run scores”).
[projection.md](../production-validation-experiment-6-2026-11-12/calls/005-r1-B-Research/projection.md)

- L5: 3. Under the stated 0–10 rubric, candidate mean scores were lower than baseline for Narrative (4.00 vs. 6.00) and Compose (3.33 vs. 5.67). The report characterizes these as scores of the saved outputs under its rubric (“Run scores”).

### 005-r2-A-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/005-r2-A-Research/content.md)

- L20: 18. Candidate Narrative scores averaged 4.00/10; baseline Narrative scores averaged 6.00/10.
- L21: 19. Candidate Compose scores averaged 3.33/10; baseline Compose scores averaged 5.67/10.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/005-r2-A-Research/projection.md)

- L20: 18. Candidate Narrative scores averaged 4.00/10; baseline Narrative scores averaged 6.00/10.
- L21: 19. Candidate Compose scores averaged 3.33/10; baseline Compose scores averaged 5.67/10.

### 005-r2-B-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/005-r2-B-Research/content.md)

- L4: 3. Under the experiment’s stated rubric, Narrative mean scores were 6.00/10 baseline and 4.00/10 candidate; Compose means were 5.67/10 and 3.33/10, respectively. These are rubric scores assigned to the saved outputs, not automated measurements. (Run scores; Scoring rubric)
[projection.md](../production-validation-experiment-6-2026-11-12/calls/005-r2-B-Research/projection.md)

- L5: 3. Under the experiment’s stated rubric, Narrative mean scores were 6.00/10 baseline and 4.00/10 candidate; Compose means were 5.67/10 and 3.33/10, respectively. These are rubric scores assigned to the saved outputs, not automated measurements. (Run scores; Scoring rubric)
## 005 / gates — kern

Bron [L78](../production-validation-experiment-6-2026-11-12/cases/005/source.md#L78): “All three candidate Narrative runs and all three candidate Compose runs failed this gate.”

Volledige oorspronkelijke bronregel (context en overige componenten):

> - **(c) Orchestration as supporting explanation:** All three candidate Narrative runs and all three candidate Compose runs failed this gate. Each made orchestration/orchestration-first positioning central.

| Run | Classificatie | Research / projectie |
|---|---|---|
| 005-r1-A-Research | correct | ruw L18, 19, 20, 21, 22, 23, 56, 65; projectie L18, 19, 20, 21, 22, 23, 52 |
| 005-r1-B-Research | correct | ruw L5, 22; projectie L6 |
| 005-r2-A-Research | correct | ruw L18, 19, 22, 23, 24, 25, 57; projectie L18, 19, 22, 23, 24, 25 |
| 005-r2-B-Research | correct | ruw L5, 6; projectie L6, 7 |

### 005-r1-A-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/005-r1-A-Research/content.md)

- L18: 16. The Narrative rubric’s criterion C scored orchestration as supporting explanation when it remained subordinate, and scored it zero when it became the central positioning.
- L19: 17. The Narrative rubric’s criterion D assessed whether unsupported audience, need, benefit, capability, traction, launch status, or commitment was added; the report identifies D as a hard promotion gate.
- L20: 18. All three candidate Narrative runs scored zero on criterion C.
- L21: 19. All three candidate Narrative runs scored zero on criterion D.
- L22: 20. All three candidate Compose runs scored zero on criterion C.
- L23: 21. All three candidate Compose runs scored zero on criterion D.
- L56: - The source does not establish why the candidate outputs made orchestration central or introduced the unsupported framing described in the report.
- L65: - All candidate Narrative and Compose runs failed the reported orchestration-hierarchy and factual-restraint gates (Facts 18–21).
[projection.md](../production-validation-experiment-6-2026-11-12/calls/005-r1-A-Research/projection.md)

- L18: 16. The Narrative rubric’s criterion C scored orchestration as supporting explanation when it remained subordinate, and scored it zero when it became the central positioning.
- L19: 17. The Narrative rubric’s criterion D assessed whether unsupported audience, need, benefit, capability, traction, launch status, or commitment was added; the report identifies D as a hard promotion gate.
- L20: 18. All three candidate Narrative runs scored zero on criterion C.
- L21: 19. All three candidate Narrative runs scored zero on criterion D.
- L22: 20. All three candidate Compose runs scored zero on criterion C.
- L23: 21. All three candidate Compose runs scored zero on criterion D.
- L52: - The source does not establish why the candidate outputs made orchestration central or introduced the unsupported framing described in the report.

### 005-r1-B-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/005-r1-B-Research/content.md)

- L5: 4. All three candidate Narrative runs and all three candidate Compose runs failed criterion C, keeping orchestration subordinate, and criterion D, factual/editorial restraint. D was identified as a hard promotion gate. The report says candidate outputs made orchestration central and added unsupported framing, including validation intent or commitment, audience labels, or a need for coordinated AI (“Analysis against the required criteria”; “Run scores”).
- L22: 2. Candidate outputs scored lower on average in both stages and every candidate run failed the hierarchy and restraint gates (Facts 3–4).
[projection.md](../production-validation-experiment-6-2026-11-12/calls/005-r1-B-Research/projection.md)

- L6: 4. All three candidate Narrative runs and all three candidate Compose runs failed criterion C, keeping orchestration subordinate, and criterion D, factual/editorial restraint. D was identified as a hard promotion gate. The report says candidate outputs made orchestration central and added unsupported framing, including validation intent or commitment, audience labels, or a need for coordinated AI (“Analysis against the required criteria”; “Run scores”).

### 005-r2-A-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/005-r2-A-Research/content.md)

- L18: 16. Rubric criterion C scored whether orchestration stayed a supporting explanation; a score of 0 meant it became the central positioning.
- L19: 17. Rubric criterion D scored factual/editorial restraint; a score of 0 meant a clear unsupported claim or expansion appeared.
- L22: 20. All three candidate Narrative runs scored 0 on criterion C.
- L23: 21. All three candidate Narrative runs scored 0 on criterion D.
- L24: 22. All three candidate Compose runs scored 0 on criterion C.
- L25: 23. All three candidate Compose runs scored 0 on criterion D.
- L57: 4. The report records that all candidate runs in both stages failed the orchestration-hierarchy and factual-restraint gates.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/005-r2-A-Research/projection.md)

- L18: 16. Rubric criterion C scored whether orchestration stayed a supporting explanation; a score of 0 meant it became the central positioning.
- L19: 17. Rubric criterion D scored factual/editorial restraint; a score of 0 meant a clear unsupported claim or expansion appeared.
- L22: 20. All three candidate Narrative runs scored 0 on criterion C.
- L23: 21. All three candidate Narrative runs scored 0 on criterion D.
- L24: 22. All three candidate Compose runs scored 0 on criterion C.
- L25: 23. All three candidate Compose runs scored 0 on criterion D.

### 005-r2-B-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/005-r2-B-Research/content.md)

- L5: 4. All three candidate Narrative outputs and all three candidate Compose outputs failed the orchestration-hierarchy criterion: each made orchestration central rather than supporting. No candidate run passed the factual/editorial-restraint criterion. (Analysis against the required criteria)
- L6: 5. The report says candidate Narrative outputs introduced unsupported validation intent or commitment, audience framing, or a need for coordinated AI; Compose outputs repeated or extended unsupported additions. The rubric identifies unsupported audience, need, benefit, capability, traction, launch status, or commitment as restraint concerns. (Analysis against the required criteria; Scoring rubric)
[projection.md](../production-validation-experiment-6-2026-11-12/calls/005-r2-B-Research/projection.md)

- L6: 4. All three candidate Narrative outputs and all three candidate Compose outputs failed the orchestration-hierarchy criterion: each made orchestration central rather than supporting. No candidate run passed the factual/editorial-restraint criterion. (Analysis against the required criteria)
- L7: 5. The report says candidate Narrative outputs introduced unsupported validation intent or commitment, audience framing, or a need for coordinated AI; Compose outputs repeated or extended unsupported additions. The rubric identifies unsupported audience, need, benefit, capability, traction, launch status, or commitment as restraint concerns. (Analysis against the required criteria; Scoring rubric)
## 005 / scope — kern

Bron [L22](../production-validation-experiment-6-2026-11-12/cases/005/source.md#L22): “All other prompt files and workflow inputs remained identical.”

Volledige oorspronkelijke bronregel (context en overige componenten):

> - **Candidate:** Three new runs with only `src/Nueyon.Compose.Application/Prompts/Narrative/Narrative.user.prompt.txt` changed. All other prompt files and workflow inputs remained identical.

| Run | Classificatie | Research / projectie |
|---|---|---|
| 005-r1-A-Research | correct | ruw L10; projectie L10 |
| 005-r1-B-Research | correct | ruw L3; projectie L4 |
| 005-r2-A-Research | correct | ruw L12, 55; projectie L12 |
| 005-r2-B-Research | correct | ruw L3, 22; projectie L4 |

### 005-r1-A-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/005-r1-A-Research/content.md)

- L10: 8. The candidate condition changed only `Narrative.user.prompt.txt`; the other prompt files and workflow inputs remained identical to the baseline condition.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/005-r1-A-Research/projection.md)

- L10: 8. The candidate condition changed only `Narrative.user.prompt.txt`; the other prompt files and workflow inputs remained identical to the baseline condition.

### 005-r1-B-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/005-r1-B-Research/content.md)

- L3: 2. The candidate added a generic “Preserve Editorial Intent from the Synthesis” section to the Narrative user prompt. The report says this was the only prompt change; source input, commit, model alias, and workflow inputs otherwise matched the baseline (“Prompt change”; “Controlled comparison and configuration”).
[projection.md](../production-validation-experiment-6-2026-11-12/calls/005-r1-B-Research/projection.md)

- L4: 2. The candidate added a generic “Preserve Editorial Intent from the Synthesis” section to the Narrative user prompt. The report says this was the only prompt change; source input, commit, model alias, and workflow inputs otherwise matched the baseline (“Prompt change”; “Controlled comparison and configuration”).

### 005-r2-A-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/005-r2-A-Research/content.md)

- L12: 10. The candidate changed only `Narrative.user.prompt.txt`; all other prompt files and workflow inputs remained identical.
- L55: 2. On 2026-11-04, three candidate runs were produced with the added generic section in the Narrative user prompt; other prompt files and workflow inputs remained identical.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/005-r2-A-Research/projection.md)

- L12: 10. The candidate changed only `Narrative.user.prompt.txt`; all other prompt files and workflow inputs remained identical.

### 005-r2-B-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/005-r2-B-Research/content.md)

- L3: 2. The candidate condition changed only `Narrative.user.prompt.txt`; the source input, other prompts, commit, model alias, and workflow inputs matched the baseline. Three baseline runs were reused from the prior experiment, and three new candidate runs were completed. (Controlled comparison and configuration)
- L22: 1. On 2026-11-03, the three baseline runs were produced; on 2026-11-04, three candidate runs were produced with only the Narrative user prompt changed (Facts 2, 9).
[projection.md](../production-validation-experiment-6-2026-11-12/calls/005-r2-B-Research/projection.md)

- L4: 2. The candidate condition changed only `Narrative.user.prompt.txt`; the source input, other prompts, commit, model alias, and workflow inputs matched the baseline. Three baseline runs were reused from the prior experiment, and three new candidate runs were completed. (Controlled comparison and configuration)
## 005 / variation — belangrijk

Bron [L100](../production-validation-experiment-6-2026-11-12/cases/005/source.md#L100): “Idea and Research outputs vary between runs.”

Volledige oorspronkelijke bronregel (context en overige componenten):

> - Idea and Research outputs vary between runs. This is expected in the same full-pipeline comparison; all other stages and inputs were kept constant.

| Run | Classificatie | Research / projectie |
|---|---|---|
| 005-r1-A-Research | ontbreekt | geen anker in ruw; geen anker in projectie |
| 005-r1-B-Research | ontbreekt | geen anker in ruw; geen anker in projectie |
| 005-r2-A-Research | correct | ruw L38; projectie L38 |
| 005-r2-B-Research | correct | ruw L11, 18; projectie L12, 17 |

### 005-r1-A-Research

Concrete beperking ontbreekt; algemene causale terughoudendheid en backendonzekerheid zijn wel aanwezig.

[content.md](../production-validation-experiment-6-2026-11-12/calls/005-r1-A-Research/content.md)

Geen passend bewijsanker; volledige tekst is beslissend.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/005-r1-A-Research/projection.md)

Geen passend bewijsanker; volledige tekst is beslissend.

### 005-r1-B-Research

Concrete beperking ontbreekt; algemene causale terughoudendheid en backendonzekerheid zijn wel aanwezig.

[content.md](../production-validation-experiment-6-2026-11-12/calls/005-r1-B-Research/content.md)

Geen passend bewijsanker; volledige tekst is beslissend.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/005-r1-B-Research/projection.md)

Geen passend bewijsanker; volledige tekst is beslissend.

### 005-r2-A-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/005-r2-A-Research/content.md)

- L38: 36. The report states that Idea and Research outputs vary between runs in the full-pipeline comparison.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/005-r2-A-Research/projection.md)

- L38: 36. The report states that Idea and Research outputs vary between runs in the full-pipeline comparison.

### 005-r2-B-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/005-r2-B-Research/content.md)

- L11: 10. The report also notes that Idea and Research outputs vary between full-pipeline runs, that the console captures only the selected first Idea, and that scores are subjective judgments based on saved outputs and the stated rubric. (Limitations)
- L18: - The report does not isolate whether the baseline Synthesis tension, run-to-run variation in upstream Idea and Research outputs, or a possible provider-backend change contributed to the observed difference; it records these as context or limitations, not demonstrated causes (Facts 7, 9–10).
[projection.md](../production-validation-experiment-6-2026-11-12/calls/005-r2-B-Research/projection.md)

- L12: 10. The report also notes that Idea and Research outputs vary between full-pipeline runs, that the console captures only the selected first Idea, and that scores are subjective judgments based on saved outputs and the stated rubric. (Limitations)
- L17: - The report does not isolate whether the baseline Synthesis tension, run-to-run variation in upstream Idea and Research outputs, or a possible provider-backend change contributed to the observed difference; it records these as context or limitations, not demonstrated causes (Facts 7, 9–10).
## 005 / dates — belangrijk

Bron [L98](../production-validation-experiment-6-2026-11-12/cases/005/source.md#L98): “The baseline runs were produced on 2026-11-03; candidate runs were produced on 2026-11-04.”

Volledige oorspronkelijke bronregel (context en overige componenten):

> - The baseline runs were produced on 2026-11-03; candidate runs were produced on 2026-11-04. The source, all prompts, commit, model alias, and workflow settings matched, but the model provider may change the backend behind an alias.

| Run | Classificatie | Research / projectie |
|---|---|---|
| 005-r1-A-Research | ontbreekt | geen anker in ruw; geen anker in projectie |
| 005-r1-B-Research | correct | ruw L16; projectie L15 |
| 005-r2-A-Research | correct | ruw L13, 14, 54, 55; projectie L13, 14 |
| 005-r2-B-Research | correct | ruw L10, 22; projectie L11 |

### 005-r1-A-Research

Concrete beperking ontbreekt; algemene causale terughoudendheid en backendonzekerheid zijn wel aanwezig.

[content.md](../production-validation-experiment-6-2026-11-12/calls/005-r1-A-Research/content.md)

Geen passend bewijsanker; volledige tekst is beslissend.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/005-r1-A-Research/projection.md)

Geen passend bewijsanker; volledige tekst is beslissend.

### 005-r1-B-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/005-r1-B-Research/content.md)

- L16: - The comparison does not establish that the prompt change alone caused the score differences: baseline runs were from Nov. 3 and candidate runs from Nov. 4, and the provider could change the backend behind the model alias. No temperature or seed override was configured (“Limitations”).
[projection.md](../production-validation-experiment-6-2026-11-12/calls/005-r1-B-Research/projection.md)

- L15: - The comparison does not establish that the prompt change alone caused the score differences: baseline runs were from Nov. 3 and candidate runs from Nov. 4, and the provider could change the backend behind the model alias. No temperature or seed override was configured (“Limitations”).

### 005-r2-A-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/005-r2-A-Research/content.md)

- L13: 11. The baseline used three completed runs from Experiment 004, dated 2026-11-03.
- L14: 12. The candidate used three new runs dated 2026-11-04.
- L54: 1. The baseline condition used three completed runs produced on 2026-11-03.
- L55: 2. On 2026-11-04, three candidate runs were produced with the added generic section in the Narrative user prompt; other prompt files and workflow inputs remained identical.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/005-r2-A-Research/projection.md)

- L13: 11. The baseline used three completed runs from Experiment 004, dated 2026-11-03.
- L14: 12. The candidate used three new runs dated 2026-11-04.

### 005-r2-B-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/005-r2-B-Research/content.md)

- L10: 9. Baseline runs were produced on 2026-11-03 and candidate runs on 2026-11-04. Although the report says source, prompts, commit, model alias, and workflow settings matched, it notes the provider may change the backend behind that alias. Temperature and seed were not explicitly configured; three runs per condition do not establish statistical significance. (Limitations)
- L22: 1. On 2026-11-03, the three baseline runs were produced; on 2026-11-04, three candidate runs were produced with only the Narrative user prompt changed (Facts 2, 9).
[projection.md](../production-validation-experiment-6-2026-11-12/calls/005-r2-B-Research/projection.md)

- L11: 9. Baseline runs were produced on 2026-11-03 and candidate runs on 2026-11-04. Although the report says source, prompts, commit, model alias, and workflow settings matched, it notes the provider may change the backend behind that alias. Temperature and seed were not explicitly configured; three runs per condition do not establish statistical significance. (Limitations)
## 005 / backend — belangrijk

Bron [L98](../production-validation-experiment-6-2026-11-12/cases/005/source.md#L98): “the model provider may change the backend behind an alias.”

Volledige oorspronkelijke bronregel (context en overige componenten):

> - The baseline runs were produced on 2026-11-03; candidate runs were produced on 2026-11-04. The source, all prompts, commit, model alias, and workflow settings matched, but the model provider may change the backend behind an alias.

| Run | Classificatie | Research / projectie |
|---|---|---|
| 005-r1-A-Research | correct | ruw L43, 55; projectie L43, 51 |
| 005-r1-B-Research | correct | ruw L16; projectie L15 |
| 005-r2-A-Research | correct | ruw L37, 48; projectie L37, 44 |
| 005-r2-B-Research | correct | ruw L10, 18; projectie L11, 17 |

### 005-r1-A-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/005-r1-A-Research/content.md)

- L43: 41. The report says the provider may change the backend behind the model alias.
- L55: - The source does not establish whether a change in the provider’s backend behind the model alias affected the results.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/005-r1-A-Research/projection.md)

- L43: 41. The report says the provider may change the backend behind the model alias.
- L51: - The source does not establish whether a change in the provider’s backend behind the model alias affected the results.

### 005-r1-B-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/005-r1-B-Research/content.md)

- L16: - The comparison does not establish that the prompt change alone caused the score differences: baseline runs were from Nov. 3 and candidate runs from Nov. 4, and the provider could change the backend behind the model alias. No temperature or seed override was configured (“Limitations”).
[projection.md](../production-validation-experiment-6-2026-11-12/calls/005-r1-B-Research/projection.md)

- L15: - The comparison does not establish that the prompt change alone caused the score differences: baseline runs were from Nov. 3 and candidate runs from Nov. 4, and the provider could change the backend behind the model alias. No temperature or seed override was configured (“Limitations”).

### 005-r2-A-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/005-r2-A-Research/content.md)

- L37: 35. The source, prompts, commit, model alias, and workflow settings matched between conditions, but the model provider may change the backend behind an alias.
- L48: - The extent, if any, to which a change in the model provider’s backend affected the comparison.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/005-r2-A-Research/projection.md)

- L37: 35. The source, prompts, commit, model alias, and workflow settings matched between conditions, but the model provider may change the backend behind an alias.
- L44: - The extent, if any, to which a change in the model provider’s backend affected the comparison.

### 005-r2-B-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/005-r2-B-Research/content.md)

- L10: 9. Baseline runs were produced on 2026-11-03 and candidate runs on 2026-11-04. Although the report says source, prompts, commit, model alias, and workflow settings matched, it notes the provider may change the backend behind that alias. Temperature and seed were not explicitly configured; three runs per condition do not establish statistical significance. (Limitations)
- L18: - The report does not isolate whether the baseline Synthesis tension, run-to-run variation in upstream Idea and Research outputs, or a possible provider-backend change contributed to the observed difference; it records these as context or limitations, not demonstrated causes (Facts 7, 9–10).
[projection.md](../production-validation-experiment-6-2026-11-12/calls/005-r2-B-Research/projection.md)

- L11: 9. Baseline runs were produced on 2026-11-03 and candidate runs on 2026-11-04. Although the report says source, prompts, commit, model alias, and workflow settings matched, it notes the provider may change the backend behind that alias. Temperature and seed were not explicitly configured; three runs per condition do not establish statistical significance. (Limitations)
- L17: - The report does not isolate whether the baseline Synthesis tension, run-to-run variation in upstream Idea and Research outputs, or a possible provider-backend change contributed to the observed difference; it records these as context or limitations, not demonstrated causes (Facts 7, 9–10).
## 005 / tension — context

Bron [L84](../production-validation-experiment-6-2026-11-12/cases/005/source.md#L84): “That internal tension may have reinforced the technical angle.”

Volledige oorspronkelijke bronregel (context en overige componenten):

> The baseline Synthesis pairs company identity with Compose in its top-level emphasis, but also contains a later description calling orchestration “central to the product direction.” That internal tension may have reinforced the technical angle. This does not change the controlled result: the candidate prompt was not sufficient to protect the supplied editorial intent, and this experiment did not change Synthesis because that was outside scope.

| Run | Classificatie | Research / projectie |
|---|---|---|
| 005-r1-A-Research | correct | ruw L37, 57; projectie L37, 53 |
| 005-r1-B-Research | correct | ruw L9, 18; projectie L10, 17 |
| 005-r2-A-Research | ontbreekt | geen anker in ruw; geen anker in projectie |
| 005-r2-B-Research | correct | ruw L8, 18, 29; projectie L9, 17 |

### 005-r1-A-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/005-r1-A-Research/content.md)

- L37: 35. The report says a later description in the baseline Synthesis calls orchestration “central to the product direction.”
- L57: - The source says the Synthesis text contained an internal tension and that this “may have reinforced” the technical angle; whether it did so is not established.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/005-r1-A-Research/projection.md)

- L37: 35. The report says a later description in the baseline Synthesis calls orchestration “central to the product direction.”
- L53: - The source says the Synthesis text contained an internal tension and that this “may have reinforced” the technical angle; whether it did so is not established.

### 005-r1-B-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/005-r1-B-Research/content.md)

- L9: 8. The report notes a possible tension in baseline Synthesis: it pairs company identity with Compose at top level but later calls orchestration “central to the product direction.” Synthesis was not changed in this experiment (“Analysis against the required criteria”).
- L18: - The baseline Synthesis’s competing emphasis on orchestration may have reinforced the technical angle, but its contribution was not tested because Synthesis was outside the change scope (Fact 8).
[projection.md](../production-validation-experiment-6-2026-11-12/calls/005-r1-B-Research/projection.md)

- L10: 8. The report notes a possible tension in baseline Synthesis: it pairs company identity with Compose at top level but later calls orchestration “central to the product direction.” Synthesis was not changed in this experiment (“Analysis against the required criteria”).
- L17: - The baseline Synthesis’s competing emphasis on orchestration may have reinforced the technical angle, but its contribution was not tested because Synthesis was outside the change scope (Fact 8).

### 005-r2-A-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/005-r2-A-Research/content.md)

Geen passend bewijsanker; volledige tekst is beslissend.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/005-r2-A-Research/projection.md)

Geen passend bewijsanker; volledige tekst is beslissend.

### 005-r2-B-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/005-r2-B-Research/content.md)

- L8: 7. The report identifies an internal tension in baseline Synthesis: it pairs company identity with Compose in its top-level emphasis, but later calls orchestration “central to the product direction.” The experiment did not change Synthesis. (Analysis against the required criteria)
- L18: - The report does not isolate whether the baseline Synthesis tension, run-to-run variation in upstream Idea and Research outputs, or a possible provider-backend change contributed to the observed difference; it records these as context or limitations, not demonstrated causes (Facts 7, 9–10).
- L29: - Source tension, decision, and limits on inference: Facts 7–10.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/005-r2-B-Research/projection.md)

- L9: 7. The report identifies an internal tension in baseline Synthesis: it pairs company identity with Compose in its top-level emphasis, but later calls orchestration “central to the product direction.” The experiment did not change Synthesis. (Analysis against the required criteria)
- L17: - The report does not isolate whether the baseline Synthesis tension, run-to-run variation in upstream Idea and Research outputs, or a possible provider-backend change contributed to the observed difference; it records these as context or limitations, not demonstrated causes (Facts 7, 9–10).
## 005 / limits — kern

Bron [L99](../production-validation-experiment-6-2026-11-12/cases/005/source.md#L99): “Three runs per condition show consistency in these samples, not statistical significance.”

Volledige oorspronkelijke bronregel (context en overige componenten):

> - No seed or temperature override was available. Three runs per condition show consistency in these samples, not statistical significance.

| Run | Classificatie | Research / projectie |
|---|---|---|
| 005-r1-A-Research | correct | ruw L42, 54; projectie L42, 50 |
| 005-r1-B-Research | correct | ruw L15; projectie L14 |
| 005-r2-A-Research | correct | ruw L36, 47; projectie L36, 43 |
| 005-r2-B-Research | correct | ruw L10, 17; projectie L11, 16 |

### 005-r1-A-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/005-r1-A-Research/content.md)

- L42: 40. The report states that three runs per condition show consistency in those samples, not statistical significance.
- L54: - The source does not establish whether the score differences are statistically significant or whether the results generalize to other settings; it explicitly limits the three runs per condition to consistency in those samples.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/005-r1-A-Research/projection.md)

- L42: 40. The report states that three runs per condition show consistency in those samples, not statistical significance.
- L50: - The source does not establish whether the score differences are statistically significant or whether the results generalize to other settings; it explicitly limits the three runs per condition to consistency in those samples.

### 005-r1-B-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/005-r1-B-Research/content.md)

- L15: - The report states that three runs per condition do not establish statistical significance or performance in other settings (“Limitations”).
[projection.md](../production-validation-experiment-6-2026-11-12/calls/005-r1-B-Research/projection.md)

- L14: - The report states that three runs per condition do not establish statistical significance or performance in other settings (“Limitations”).

### 005-r2-A-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/005-r2-A-Research/content.md)

- L36: 34. Three runs per condition show consistency in these samples, not statistical significance.
- L47: - Whether the observed score differences are statistically significant; the report explicitly says the three runs per condition do not establish statistical significance.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/005-r2-A-Research/projection.md)

- L36: 34. Three runs per condition show consistency in these samples, not statistical significance.
- L43: - Whether the observed score differences are statistically significant; the report explicitly says the three runs per condition do not establish statistical significance.

### 005-r2-B-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/005-r2-B-Research/content.md)

- L10: 9. Baseline runs were produced on 2026-11-03 and candidate runs on 2026-11-04. Although the report says source, prompts, commit, model alias, and workflow settings matched, it notes the provider may change the backend behind that alias. Temperature and seed were not explicitly configured; three runs per condition do not establish statistical significance. (Limitations)
- L17: - The three runs per condition do not establish statistical significance or show whether the result generalizes to other sources, models, prompt designs, or workflow settings (Facts 2, 9–10).
[projection.md](../production-validation-experiment-6-2026-11-12/calls/005-r2-B-Research/projection.md)

- L11: 9. Baseline runs were produced on 2026-11-03 and candidate runs on 2026-11-04. Although the report says source, prompts, commit, model alias, and workflow settings matched, it notes the provider may change the backend behind that alias. Temperature and seed were not explicitly configured; three runs per condition do not establish statistical significance. (Limitations)
- L16: - The three runs per condition do not establish statistical significance or show whether the result generalizes to other sources, models, prompt designs, or workflow settings (Facts 2, 9–10).
## 005 / restore — kern

Bron [L92](../production-validation-experiment-6-2026-11-12/cases/005/source.md#L92): “The production prompt was restored from the saved byte snapshot.”

Volledige oorspronkelijke bronregel (context en overige componenten):

> - **Decision: Do not promote.** Candidate was worse on mean score for both Narrative and Compose, failed the orchestration hierarchy and factual restraint gates in every run, and introduced regressions. The production prompt was restored from the saved byte snapshot.

| Run | Classificatie | Research / projectie |
|---|---|---|
| 005-r1-A-Research | correct | ruw L40, 41, 66; projectie L40, 41 |
| 005-r1-B-Research | correct | ruw L8, 23; projectie L9 |
| 005-r2-A-Research | correct | ruw L34, 35, 58; projectie L34, 35 |
| 005-r2-B-Research | correct | ruw L9, 24; projectie L10 |

### 005-r1-A-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/005-r1-A-Research/content.md)

- L40: 38. The production Narrative user prompt was restored byte-for-byte to the baseline.
- L41: 39. The restored production prompt’s SHA-256 matched the frozen baseline prompt’s SHA-256.
- L66: - The candidate was not promoted, and the production prompt was restored byte-for-byte to baseline (Facts 37–39).
[projection.md](../production-validation-experiment-6-2026-11-12/calls/005-r1-A-Research/projection.md)

- L40: 38. The production Narrative user prompt was restored byte-for-byte to the baseline.
- L41: 39. The restored production prompt’s SHA-256 matched the frozen baseline prompt’s SHA-256.

### 005-r1-B-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/005-r1-B-Research/content.md)

- L8: 7. The decision was not to promote the candidate. The production prompt was restored byte-for-byte; its SHA-256 matched the frozen baseline (“Decision”; “Rollback verification”).
- L23: 3. The candidate was not promoted and the production prompt was restored to the baseline bytes (Fact 7).
[projection.md](../production-validation-experiment-6-2026-11-12/calls/005-r1-B-Research/projection.md)

- L9: 7. The decision was not to promote the candidate. The production prompt was restored byte-for-byte; its SHA-256 matched the frozen baseline (“Decision”; “Rollback verification”).

### 005-r2-A-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/005-r2-A-Research/content.md)

- L34: 32. The production Narrative user prompt was restored byte-for-byte to the baseline snapshot.
- L35: 33. The restored production prompt’s SHA-256 matched the frozen baseline hash.
- L58: 5. The candidate was not promoted, and the production Narrative user prompt was restored byte-for-byte to baseline.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/005-r2-A-Research/projection.md)

- L34: 32. The production Narrative user prompt was restored byte-for-byte to the baseline snapshot.
- L35: 33. The restored production prompt’s SHA-256 matched the frozen baseline hash.

### 005-r2-B-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/005-r2-B-Research/content.md)

- L9: 8. The decision was not to promote the candidate. The production Narrative user prompt was restored byte-for-byte to baseline; its restored SHA-256 matched the frozen baseline hash. (Decision; Build, tests, and promotion decision; Rollback verification)
- L24: 3. The candidate was not promoted, and the production prompt was restored byte-for-byte to baseline (Fact 8).
[projection.md](../production-validation-experiment-6-2026-11-12/calls/005-r2-B-Research/projection.md)

- L10: 8. The decision was not to promote the candidate. The production Narrative user prompt was restored byte-for-byte to baseline; its restored SHA-256 matched the frozen baseline hash. (Decision; Build, tests, and promotion decision; Rollback verification)
## harbor / synthetic — kern

Bron [L4](../production-validation-experiment-6-2026-11-12/cases/harbor/source.md#L4): “This dossier is a synthetic operational source for an evaluation, not a real transport study.”

Volledige oorspronkelijke bronregel (context en overige componenten):

> HarborLink is a fictional regional operator. This dossier is a synthetic operational source for an evaluation, not a real transport study. It records an eight-week demonstration of six leased electric buses at Seabank depot. The operator describes the vehicles as “a visible first step toward cleaner local transport”. That phrase is the operator's intended meaning, not a measured finding about emissions, public trust or service quality. The board must decide whether to commission a winter assessment, not whether to replace the entire fleet. No procurement approval has been given.

| Run | Classificatie | Research / projectie |
|---|---|---|
| harbor-r1-A-Research | correct | ruw L3; projectie L3 |
| harbor-r1-B-Research | correct | ruw L2; projectie L3 |
| harbor-r2-A-Research | correct | ruw L3; projectie L3 |
| harbor-r2-B-Research | correct | ruw L2; projectie L3 |

### harbor-r1-A-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r1-A-Research/content.md)

- L3: 1. The dossier describes HarborLink as a fictional regional operator and the material as a synthetic operational source, not a real transport study.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r1-A-Research/projection.md)

- L3: 1. The dossier describes HarborLink as a fictional regional operator and the material as a synthetic operational source, not a real transport study.

### harbor-r1-B-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r1-B-Research/content.md)

- L2: 1. HarborLink and this dossier are explicitly fictional; the dossier describes a synthetic evaluation, not a real transport study. (Section 1)
[projection.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r1-B-Research/projection.md)

- L3: 1. HarborLink and this dossier are explicitly fictional; the dossier describes a synthetic evaluation, not a real transport study. (Section 1)

### harbor-r2-A-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r2-A-Research/content.md)

- L3: 1. The dossier describes HarborLink as a fictional regional operator and the dossier itself as a synthetic operational source, not a real transport study.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r2-A-Research/projection.md)

- L3: 1. The dossier describes HarborLink as a fictional regional operator and the dossier itself as a synthetic operational source, not a real transport study.

### harbor-r2-B-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r2-B-Research/content.md)

- L2: 1. De dossier is expliciet een synthetische operationele bron over een proef met zes geleasede bussen in Seabank; het is geen echte vervoersstudie. De proef liep acht weken, van 4 mei tot 28 juni 2026. (Purpose and decision boundary)
[projection.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r2-B-Research/projection.md)

- L3: 1. De dossier is expliciet een synthetische operationele bron over een proef met zes geleasede bussen in Seabank; het is geen echte vervoersstudie. De proef liep acht weken, van 4 mei tot 28 juni 2026. (Purpose and decision boundary)
## harbor / service — kern

Bron [L9](../production-validation-experiment-6-2026-11-12/cases/harbor/source.md#L9): “the electric group completed 2,310 of 2,400 scheduled departures, or 96.3%.”

Volledige oorspronkelijke bronregel (context en overige componenten):

> In the comparison period, the six vehicles later displaced by the trial completed 2,240 scheduled departures out of 2,400, a completion rate of 93.3%. During the trial, the electric group completed 2,310 of 2,400 scheduled departures, or 96.3%. Counts are taken from the final dispatch ledger. A draft presentation used 2,340 trial departures; 30 entries were duplicate departure IDs and were removed on 9 July. The draft must not be used for the completion comparison.

| Run | Classificatie | Research / projectie |
|---|---|---|
| harbor-r1-A-Research | correct | ruw L16; projectie L16 |
| harbor-r1-B-Research | correct | ruw L3; projectie L4 |
| harbor-r2-A-Research | correct | ruw L10; projectie L10 |
| harbor-r2-B-Research | correct | ruw L3; projectie L4 |

### harbor-r1-A-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r1-A-Research/content.md)

- L16: 14. During the trial, the electric group completed 2,310 of 2,400 scheduled departures, a 96.3% completion rate.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r1-A-Research/projection.md)

- L16: 14. During the trial, the electric group completed 2,310 of 2,400 scheduled departures, a 96.3% completion rate.

### harbor-r1-B-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r1-B-Research/content.md)

- L3: 2. The demonstration ran 4 May–28 June; its comparison period ran 9 March–3 May. The final ledger records 2,240/2,400 completions (93.3%) for the six vehicles later displaced and 2,310/2,400 (96.3%) for the electric group. Completion uses the same definition in both periods. The draft’s 2,340 trial departures included 30 duplicate IDs, removed on 9 July. (Sections 1–2, “final dispatch ledger”)
[projection.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r1-B-Research/projection.md)

- L4: 2. The demonstration ran 4 May–28 June; its comparison period ran 9 March–3 May. The final ledger records 2,240/2,400 completions (93.3%) for the six vehicles later displaced and 2,310/2,400 (96.3%) for the electric group. Completion uses the same definition in both periods. The draft’s 2,340 trial departures included 30 duplicate IDs, removed on 9 July. (Sections 1–2, “final dispatch ledger”)

### harbor-r2-A-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r2-A-Research/content.md)

- L10: 8. During the trial, the electric group completed 2,310 of 2,400 scheduled departures, a completion rate of 96.3%.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r2-A-Research/projection.md)

- L10: 8. During the trial, the electric group completed 2,310 of 2,400 scheduled departures, a completion rate of 96.3%.

### harbor-r2-B-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r2-B-Research/content.md)

- L3: 2. De zes vervangen voertuigen maakten in de vergelijkingsperiode 2.240 van 2.400 geplande vertrekken af (93,3%); de elektrische groep maakte tijdens de proef 2.310 van 2.400 af (96,3%). De definitieve dispatchdefinitie is in beide perioden gelijk. De 30 dubbele vertrek-ID’s achter het conceptcijfer 2.340 zijn op 9 juli verwijderd. (Service ledger)
[projection.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r2-B-Research/projection.md)

- L4: 2. De zes vervangen voertuigen maakten in de vergelijkingsperiode 2.240 van 2.400 geplande vertrekken af (93,3%); de elektrische groep maakte tijdens de proef 2.310 van 2.400 af (96,3%). De definitieve dispatchdefinitie is in beide perioden gelijk. De 30 dubbele vertrek-ID’s achter het conceptcijfer 2.340 zijn op 9 juli verwijderd. (Service ledger)
## harbor / duplicates — kern

Bron [L9](../production-validation-experiment-6-2026-11-12/cases/harbor/source.md#L9): “30 entries were duplicate departure IDs and were removed on 9 July.”

Volledige oorspronkelijke bronregel (context en overige componenten):

> In the comparison period, the six vehicles later displaced by the trial completed 2,240 scheduled departures out of 2,400, a completion rate of 93.3%. During the trial, the electric group completed 2,310 of 2,400 scheduled departures, or 96.3%. Counts are taken from the final dispatch ledger. A draft presentation used 2,340 trial departures; 30 entries were duplicate departure IDs and were removed on 9 July. The draft must not be used for the completion comparison.

| Run | Classificatie | Research / projectie |
|---|---|---|
| harbor-r1-A-Research | correct | ruw L18, 110; projectie L18 |
| harbor-r1-B-Research | correct | ruw L3, 25; projectie L4 |
| harbor-r2-A-Research | correct | ruw L12, 94; projectie L12 |
| harbor-r2-B-Research | correct | ruw L3, 33; projectie L4 |

### harbor-r1-A-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r1-A-Research/content.md)

- L18: 16. A draft presentation used 2,340 trial departures; 30 entries were duplicate departure IDs and were removed on 9 July.
- L110: - 9 July: 30 duplicate departure IDs were removed from the draft trial count. (Fact 16)
[projection.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r1-A-Research/projection.md)

- L18: 16. A draft presentation used 2,340 trial departures; 30 entries were duplicate departure IDs and were removed on 9 July.

### harbor-r1-B-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r1-B-Research/content.md)

- L3: 2. The demonstration ran 4 May–28 June; its comparison period ran 9 March–3 May. The final ledger records 2,240/2,400 completions (93.3%) for the six vehicles later displaced and 2,310/2,400 (96.3%) for the electric group. Completion uses the same definition in both periods. The draft’s 2,340 trial departures included 30 duplicate IDs, removed on 9 July. (Sections 1–2, “final dispatch ledger”)
- L25: The March–May comparison period preceded the 4 May–28 June demonstration (Facts 2, 5). Duplicate trial departure IDs were removed from the draft count on 9 July (Fact 2); the two failed audio units were restored on 2 July, after the demonstration (Fact 6). On 18 September the board authorized preparation of a winter-assessment specification, not a further lease or procurement (Fact 10).
[projection.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r1-B-Research/projection.md)

- L4: 2. The demonstration ran 4 May–28 June; its comparison period ran 9 March–3 May. The final ledger records 2,240/2,400 completions (93.3%) for the six vehicles later displaced and 2,310/2,400 (96.3%) for the electric group. Completion uses the same definition in both periods. The draft’s 2,340 trial departures included 30 duplicate IDs, removed on 9 July. (Sections 1–2, “final dispatch ledger”)

### harbor-r2-A-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r2-A-Research/content.md)

- L12: 10. A draft presentation used 2,340 trial departures; 30 duplicate departure IDs were removed on 9 July.
- L94: - 9 July 2026: 30 duplicate departure IDs were removed from the draft trial count.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r2-A-Research/projection.md)

- L12: 10. A draft presentation used 2,340 trial departures; 30 duplicate departure IDs were removed on 9 July.

### harbor-r2-B-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r2-B-Research/content.md)

- L3: 2. De zes vervangen voertuigen maakten in de vergelijkingsperiode 2.240 van 2.400 geplande vertrekken af (93,3%); de elektrische groep maakte tijdens de proef 2.310 van 2.400 af (96,3%). De definitieve dispatchdefinitie is in beide perioden gelijk. De 30 dubbele vertrek-ID’s achter het conceptcijfer 2.340 zijn op 9 juli verwijderd. (Service ledger)
- L33: - 2 juli: de twee audio-units werden na afloop van de proef hersteld. 9 juli: 30 dubbele vertrek-ID’s werden uit het conceptcijfer verwijderd. (Facts 2, 11)
[projection.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r2-B-Research/projection.md)

- L4: 2. De zes vervangen voertuigen maakten in de vergelijkingsperiode 2.240 van 2.400 geplande vertrekken af (93,3%); de elektrische groep maakte tijdens de proef 2.310 van 2.400 af (96,3%). De definitieve dispatchdefinitie is in beide perioden gelijk. De 30 dubbele vertrek-ID’s achter het conceptcijfer 2.340 zijn op 9 juli verwijderd. (Service ledger)
## harbor / substitution — kern

Bron [L20](../production-validation-experiment-6-2026-11-12/cases/harbor/source.md#L20): “The summary completion rate is a service-delivery measure, not a measure of exclusive electric operation.”

Volledige oorspronkelijke bronregel (context en overige componenten):

> Two chargers were available. During three overnight sessions one charger was offline after a protective trip. Two buses departed on time after using the other charger; a third was substituted with a diesel bus. The final ledger counts the substituted trip as a completed route departure but does not count it as an electric-bus departure. The summary completion rate is a service-delivery measure, not a measure of exclusive electric operation. The charger vendor replaced a connector and updated firmware in the same week. This record cannot identify which action resolved the trips.

| Run | Classificatie | Research / projectie |
|---|---|---|
| harbor-r1-A-Research | correct | ruw L46, 47, 48, 106; projectie L46, 47, 48 |
| harbor-r1-B-Research | correct | ruw L4; projectie L5 |
| harbor-r2-A-Research | correct | ruw L39, 40, 41, 92, 102; projectie L39, 40, 41 |
| harbor-r2-B-Research | correct | ruw L5, 9; projectie L6, 10 |

### harbor-r1-A-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r1-A-Research/content.md)

- L46: 44. Two buses departed on time after using the other charger; a third was substituted with a diesel bus.
- L47: 45. The final ledger counts the substituted trip as a completed route departure but not as an electric-bus departure.
- L48: 46. The source describes the summary completion rate as a service-delivery measure, not a measure of exclusive electric operation.
- L106: - During the demonstration: one charger was offline during three overnight sessions; the ledger recorded the substituted diesel trip as a completed route departure, not an electric-bus departure. (Facts 43–46)
[projection.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r1-A-Research/projection.md)

- L46: 44. Two buses departed on time after using the other charger; a third was substituted with a diesel bus.
- L47: 45. The final ledger counts the substituted trip as a completed route departure but not as an electric-bus departure.
- L48: 46. The source describes the summary completion rate as a service-delivery measure, not a measure of exclusive electric operation.

### harbor-r1-B-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r1-B-Research/content.md)

- L4: 3. The completion measure is service delivery, not exclusive electric operation: a diesel-substituted trip counted as a completed route departure but not as an electric-bus departure. (Section 3)
[projection.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r1-B-Research/projection.md)

- L5: 3. The completion measure is service delivery, not exclusive electric operation: a diesel-substituted trip counted as a completed route departure but not as an electric-bus departure. (Section 3)

### harbor-r2-A-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r2-A-Research/content.md)

- L39: 37. Two buses departed on time after using the other charger; a third bus was substituted with a diesel bus.
- L40: 38. The final ledger counts the substituted trip as a completed route departure but not as an electric-bus departure.
- L41: 39. The source describes the summary completion rate as a service-delivery measure, not a measure of exclusive electric operation.
- L92: - During the demonstration: charger outages, bus substitutions, rider-card collection, driver debriefs, accessibility inspections, and the recorded daytime temperature range described in Facts 35–59.
- L102: - Facts 35–40 concern charger availability, substitutions, and the scope of the completion measure.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r2-A-Research/projection.md)

- L39: 37. Two buses departed on time after using the other charger; a third bus was substituted with a diesel bus.
- L40: 38. The final ledger counts the substituted trip as a completed route departure but not as an electric-bus departure.
- L41: 39. The source describes the summary completion rate as a service-delivery measure, not a measure of exclusive electric operation.

### harbor-r2-B-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r2-B-Research/content.md)

- L5: 4. De dieselbussen op de heuvelroute verbeterden eveneens: 1.170/1.200 vertrekken vóór en 1.188/1.200 tijdens de proef. Wegwerkzaamheden op een deel van die route eindigden in april. De bron noemt dit een contextuele vergelijking, geen gerandomiseerde controle; terrein en dispatchregels verhinderen een zuivere effectschatting. (Service ledger)
- L9: 8. Tijdens drie nachtsessies was één van twee laders buiten werking na een beveiligingsuitschakeling. Twee bussen vertrokken op tijd met de andere lader; voor een derde werd een dieselbus ingezet. Die rit telt als voltooide dienstvertrek, niet als vertrek van een elektrische bus. Een connectorvervanging en firmware-update vonden in dezelfde week plaats; de bron kan niet bepalen welke ingreep de ritten oploste. (Energy and infrastructure account)
[projection.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r2-B-Research/projection.md)

- L6: 4. De dieselbussen op de heuvelroute verbeterden eveneens: 1.170/1.200 vertrekken vóór en 1.188/1.200 tijdens de proef. Wegwerkzaamheden op een deel van die route eindigden in april. De bron noemt dit een contextuele vergelijking, geen gerandomiseerde controle; terrein en dispatchregels verhinderen een zuivere effectschatting. (Service ledger)
- L10: 8. Tijdens drie nachtsessies was één van twee laders buiten werking na een beveiligingsuitschakeling. Twee bussen vertrokken op tijd met de andere lader; voor een derde werd een dieselbus ingezet. Die rit telt als voltooide dienstvertrek, niet als vertrek van een elektrische bus. Een connectorvervanging en firmware-update vonden in dezelfde week plaats; de bron kan niet bepalen welke ingreep de ritten oploste. (Energy and infrastructure account)
## harbor / punctuality — kern

Bron [L11](../production-validation-experiment-6-2026-11-12/cases/harbor/source.md#L11): “There is no reprocessing of the old GPS records under a common threshold.”

Volledige oorspronkelijke bronregel (context en overige componenten):

> On-time arrival in March-April meant no more than three minutes late; in May-June it meant no more than five minutes late. The dashboard displays 82% and 89% respectively. There is no reprocessing of the old GPS records under a common threshold. A higher displayed punctuality percentage therefore combines possible service differences with a measurement change. Completion rate uses the same definition in both periods and does not have this particular threshold problem. The two measures must remain distinct.

| Run | Classificatie | Research / projectie |
|---|---|---|
| harbor-r1-A-Research | correct | ruw L23, 24, 25, 80, 90, 104, 105, 117; projectie L23, 24, 25, 80, 86 |
| harbor-r1-B-Research | correct | ruw L5, 18; projectie L6, 17 |
| harbor-r2-A-Research | correct | ruw L16, 17, 18, 19, 75; projectie L16, 17, 18, 19, 71 |
| harbor-r2-B-Research | correct | ruw L6, 19, 23; projectie L7, 22 |

### harbor-r1-A-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r1-A-Research/content.md)

- L23: 21. The old GPS records were not reprocessed under a common threshold.
- L24: 22. The source says the higher displayed punctuality percentage combines possible service differences with a measurement change.
- L25: 23. The source says completion rate does not have this particular threshold problem and that completion and punctuality must remain distinct measures.
- L80: 78. The source says the public update should retain the concrete improvement in completed services without merging it with the incompatible punctuality series.
- L90: - A punctuality comparison under a common threshold; the old GPS records were not reprocessed under one.
- L104: - 9 March–3 May: comparison period; the source records completion and dashboard punctuality figures for this period. (Facts 7, 13, 19–20)
- L105: - 4 May–28 June: eight-week demonstration of six leased electric buses; the source records completion and dashboard punctuality figures for this period. (Facts 2, 7, 14, 19–20)
- L117: - Facts 13–28 and 78 are relevant to completed services, punctuality, comparison limits, and the distinction between the two service measures.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r1-A-Research/projection.md)

- L23: 21. The old GPS records were not reprocessed under a common threshold.
- L24: 22. The source says the higher displayed punctuality percentage combines possible service differences with a measurement change.
- L25: 23. The source says completion rate does not have this particular threshold problem and that completion and punctuality must remain distinct measures.
- L80: 78. The source says the public update should retain the concrete improvement in completed services without merging it with the incompatible punctuality series.
- L86: - A punctuality comparison under a common threshold; the old GPS records were not reprocessed under one.

### harbor-r1-B-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r1-B-Research/content.md)

- L5: 4. The displayed punctuality rates, 82% and 89%, use different lateness thresholds (three minutes versus five); old GPS records were not reprocessed to a common threshold. The source says the difference may combine service differences with this measurement change. (Section 2)
- L18: - A comparable punctuality change under one threshold is unknown because the source provides no common-threshold reprocessing (Fact 4).
[projection.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r1-B-Research/projection.md)

- L6: 4. The displayed punctuality rates, 82% and 89%, use different lateness thresholds (three minutes versus five); old GPS records were not reprocessed to a common threshold. The source says the difference may combine service differences with this measurement change. (Section 2)
- L17: - A comparable punctuality change under one threshold is unknown because the source provides no common-threshold reprocessing (Fact 4).

### harbor-r2-A-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r2-A-Research/content.md)

- L16: 14. The old GPS records were not reprocessed under a common on-time threshold.
- L17: 15. The source says the higher displayed punctuality percentage combines possible service differences with a measurement change.
- L18: 16. Completion rate uses the same definition in both periods and does not have the particular threshold problem affecting the punctuality figures.
- L19: 17. The source says the completion and punctuality measures must remain distinct.
- L75: - A service-performance comparison using the same on-time threshold for both periods; the old GPS records were not reprocessed under a common threshold.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r2-A-Research/projection.md)

- L16: 14. The old GPS records were not reprocessed under a common on-time threshold.
- L17: 15. The source says the higher displayed punctuality percentage combines possible service differences with a measurement change.
- L18: 16. Completion rate uses the same definition in both periods and does not have the particular threshold problem affecting the punctuality figures.
- L19: 17. The source says the completion and punctuality measures must remain distinct.
- L71: - A service-performance comparison using the same on-time threshold for both periods; the old GPS records were not reprocessed under a common threshold.

### harbor-r2-B-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r2-B-Research/content.md)

- L6: 5. De weergegeven punctualiteit steeg van 82% naar 89%, maar de grens veranderde van maximaal drie minuten te laat naar maximaal vijf minuten. Oude GPS-gegevens zijn niet met één gemeenschappelijke grens herberekend. (Service ledger)
- L19: 1. en 2. ondersteunen een concrete verbetering in voltooide vertrekken volgens dezelfde definitie, maar niet op zichzelf een causaal, genormaliseerd effect van elektrische aandrijving. 5. en 8. maken duidelijk dat punctualiteit en voltooiing van dienstvertrekken niet hetzelfde bewijs leveren als punctualiteit onder een vaste meetgrens of exclusief elektrisch rijden.
- L23: - Een vergelijkbare punctualiteitsverandering onder één drempel is onbekend. (Fact 5)
[projection.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r2-B-Research/projection.md)

- L7: 5. De weergegeven punctualiteit steeg van 82% naar 89%, maar de grens veranderde van maximaal drie minuten te laat naar maximaal vijf minuten. Oude GPS-gegevens zijn niet met één gemeenschappelijke grens herberekend. (Service ledger)
- L22: - Een vergelijkbare punctualiteitsverandering onder één drempel is onbekend. (Fact 5)
## harbor / routes — kern

Bron [L6](../production-validation-experiment-6-2026-11-12/cases/harbor/source.md#L6): “The diesel comparison vehicles were not randomly assigned.”

Volledige oorspronkelijke bronregel (context en overige componenten):

> The demonstration ran from 4 May to 28 June. The comparison period was 9 March to 3 May. Both contained eight weeks, but the school holiday calendar differed. The March period contained two weeks of bridge diversion; the May period contained none. The diesel comparison vehicles were not randomly assigned. Dispatchers assigned the electric buses to two relatively flat routes close to the depot and retained diesel buses for a longer hill route. The figures below describe these assignments; they are not normalized fleet-effect estimates.

| Run | Classificatie | Research / projectie |
|---|---|---|
| harbor-r1-A-Research | correct | ruw L13; projectie L13 |
| harbor-r1-B-Research | deels | ruw L6, 17; projectie L7, 16 |
| harbor-r2-A-Research | correct | ruw L8; projectie L8 |
| harbor-r2-B-Research | correct | ruw L4; projectie L5 |

### harbor-r1-A-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r1-A-Research/content.md)

- L13: 11. Dispatchers assigned the electric buses to two relatively flat routes close to the depot and retained diesel buses for a longer hill route.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r1-A-Research/projection.md)

- L13: 11. Dispatchers assigned the electric buses to two relatively flat routes close to the depot and retained diesel buses for a longer hill route.

### harbor-r1-B-Research

Niet-random routes en dieseltegencontrast aanwezig; vakanties/omleiding ontbreken.

[content.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r1-B-Research/content.md)

- L6: 5. The electric buses were assigned non-randomly to two relatively flat routes near the depot; diesel buses retained a longer hill route. Those diesel buses’ completion rose from 1,170/1,200 to 1,188/1,200, while roadworks ended on part of that route in April. The dossier calls this contextual, not randomized, and says terrain and dispatch differences prevent a clean effect estimate. (Sections 1–2)
- L17: - Whether the higher recorded completion rate was caused by electric propulsion or would persist under comparable routes, dispatch and conditions is not established; the source identifies non-random assignment and contextual differences (Facts 2, 5).
[projection.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r1-B-Research/projection.md)

- L7: 5. The electric buses were assigned non-randomly to two relatively flat routes near the depot; diesel buses retained a longer hill route. Those diesel buses’ completion rose from 1,170/1,200 to 1,188/1,200, while roadworks ended on part of that route in April. The dossier calls this contextual, not randomized, and says terrain and dispatch differences prevent a clean effect estimate. (Sections 1–2)
- L16: - Whether the higher recorded completion rate was caused by electric propulsion or would persist under comparable routes, dispatch and conditions is not established; the source identifies non-random assignment and contextual differences (Facts 2, 5).

### harbor-r2-A-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r2-A-Research/content.md)

- L8: 6. Dispatchers assigned the electric buses to two relatively flat routes close to the depot and retained diesel buses for a longer hill route.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r2-A-Research/projection.md)

- L8: 6. Dispatchers assigned the electric buses to two relatively flat routes close to the depot and retained diesel buses for a longer hill route.

### harbor-r2-B-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r2-B-Research/content.md)

- L4: 3. De vertrekcijfers zijn geen genormaliseerde vlooteffectschatting: elektrische bussen reden op twee relatief vlakke routes dicht bij de remise; diesels bleven op een langere heuvelroute. Toewijzing was niet willekeurig. Ook verschilden schoolvakanties en bevatte de vergelijkingsperiode twee weken omleiding, tegenover geen tijdens de proef. (Purpose; Service ledger)
[projection.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r2-B-Research/projection.md)

- L5: 3. De vertrekcijfers zijn geen genormaliseerde vlooteffectschatting: elektrische bussen reden op twee relatief vlakke routes dicht bij de remise; diesels bleven op een langere heuvelroute. Toewijzing was niet willekeurig. Ook verschilden schoolvakanties en bevatte de vergelijkingsperiode twee weken omleiding, tegenover geen tijdens de proef. (Purpose; Service ledger)
## harbor / diesel-control — belangrijk

Bron [L13](../production-validation-experiment-6-2026-11-12/cases/harbor/source.md#L13): “the hill route completed 1,170 of 1,200 departures before the demonstration and 1,188 of 1,200 during it.”

Volledige oorspronkelijke bronregel (context en overige componenten):

> The diesel vehicles on the hill route completed 1,170 of 1,200 departures before the demonstration and 1,188 of 1,200 during it. They used neither electric propulsion nor the new charger schedule. Roadworks ended on part of that route in April. This is a contextual comparison, not a randomized control; the different terrain and dispatch rules prevent a clean effect estimate. It nevertheless prevents treating every contemporaneous improvement as unique to the electric group.

| Run | Classificatie | Research / projectie |
|---|---|---|
| harbor-r1-A-Research | correct | ruw L26; projectie L26 |
| harbor-r1-B-Research | correct | ruw L6; projectie L7 |
| harbor-r2-A-Research | correct | ruw L20; projectie L20 |
| harbor-r2-B-Research | correct | ruw L5; projectie L6 |

### harbor-r1-A-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r1-A-Research/content.md)

- L26: 24. The diesel vehicles on the hill route completed 1,170 of 1,200 departures before the demonstration and 1,188 of 1,200 during it.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r1-A-Research/projection.md)

- L26: 24. The diesel vehicles on the hill route completed 1,170 of 1,200 departures before the demonstration and 1,188 of 1,200 during it.

### harbor-r1-B-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r1-B-Research/content.md)

- L6: 5. The electric buses were assigned non-randomly to two relatively flat routes near the depot; diesel buses retained a longer hill route. Those diesel buses’ completion rose from 1,170/1,200 to 1,188/1,200, while roadworks ended on part of that route in April. The dossier calls this contextual, not randomized, and says terrain and dispatch differences prevent a clean effect estimate. (Sections 1–2)
[projection.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r1-B-Research/projection.md)

- L7: 5. The electric buses were assigned non-randomly to two relatively flat routes near the depot; diesel buses retained a longer hill route. Those diesel buses’ completion rose from 1,170/1,200 to 1,188/1,200, while roadworks ended on part of that route in April. The dossier calls this contextual, not randomized, and says terrain and dispatch differences prevent a clean effect estimate. (Sections 1–2)

### harbor-r2-A-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r2-A-Research/content.md)

- L20: 18. The diesel vehicles on the hill route completed 1,170 of 1,200 departures before the demonstration and 1,188 of 1,200 during it.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r2-A-Research/projection.md)

- L20: 18. The diesel vehicles on the hill route completed 1,170 of 1,200 departures before the demonstration and 1,188 of 1,200 during it.

### harbor-r2-B-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r2-B-Research/content.md)

- L5: 4. De dieselbussen op de heuvelroute verbeterden eveneens: 1.170/1.200 vertrekken vóór en 1.188/1.200 tijdens de proef. Wegwerkzaamheden op een deel van die route eindigden in april. De bron noemt dit een contextuele vergelijking, geen gerandomiseerde controle; terrein en dispatchregels verhinderen een zuivere effectschatting. (Service ledger)
[projection.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r2-B-Research/projection.md)

- L6: 4. De dieselbussen op de heuvelroute verbeterden eveneens: 1.170/1.200 vertrekken vóór en 1.188/1.200 tijdens de proef. Wegwerkzaamheden op een deel van die route eindigden in april. De bron noemt dit een contextuele vergelijking, geen gerandomiseerde controle; terrein en dispatchregels verhinderen een zuivere effectschatting. (Service ledger)
## harbor / cost — kern

Bron [L18](../production-validation-experiment-6-2026-11-12/cases/harbor/source.md#L18): “Finance has not produced a like-for-like whole-cost calculation.”

Volledige oorspronkelijke bronregel (context en overige componenten):

> Electricity expenditure was EUR 8,400 under a temporary demonstration tariff. The six comparable diesel vehicles previously used EUR 13,700 of fuel, but drove 38,000 km, faced different traffic and used a different energy source. Charger lease, electrical upgrade, standing charges and workshop training are not included in the electricity figure. The tariff expires on 31 October. Finance has not produced a like-for-like whole-cost calculation. It is legitimate to quote the recorded expenditures with their boundaries; claiming a proven annual saving or a fleet payback period is not supported.

| Run | Classificatie | Research / projectie |
|---|---|---|
| harbor-r1-A-Research | correct | ruw L37, 38, 42, 92; projectie L37, 38, 42, 88 |
| harbor-r1-B-Research | correct | ruw L8, 19; projectie L9, 18 |
| harbor-r2-A-Research | correct | ruw L31, 32, 35, 78, 101; projectie L31, 32, 35, 74 |
| harbor-r2-B-Research | correct | ruw L8; projectie L9 |

### harbor-r1-A-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r1-A-Research/content.md)

- L37: 35. Electricity expenditure was EUR 8,400 under a temporary demonstration tariff.
- L38: 36. The six comparable diesel vehicles previously used EUR 13,700 of fuel while driving 38,000 km.
- L42: 40. Finance had not produced a like-for-like whole-cost calculation.
- L92: - A like-for-like whole-cost calculation, proven annual saving, or fleet payback period.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r1-A-Research/projection.md)

- L37: 35. Electricity expenditure was EUR 8,400 under a temporary demonstration tariff.
- L38: 36. The six comparable diesel vehicles previously used EUR 13,700 of fuel while driving 38,000 km.
- L42: 40. Finance had not produced a like-for-like whole-cost calculation.
- L88: - A like-for-like whole-cost calculation, proven annual saving, or fleet payback period.

### harbor-r1-B-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r1-B-Research/content.md)

- L8: 7. Recorded electricity expenditure was EUR 8,400 under a temporary tariff; the six comparable diesel vehicles had previously used EUR 13,700 of fuel while driving 38,000 km versus 35,000 electric-bus km. The source notes different traffic and energy sources, excludes several charger and training costs from the electricity figure, and says no like-for-like whole-cost calculation was produced. The tariff expires 31 October. (Section 3)
- L19: - Whole-cost savings, annual savings or fleet payback are unsupported: a like-for-like cost calculation was not produced and recorded expenditure omits costs and uses a temporary tariff (Fact 7).
[projection.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r1-B-Research/projection.md)

- L9: 7. Recorded electricity expenditure was EUR 8,400 under a temporary tariff; the six comparable diesel vehicles had previously used EUR 13,700 of fuel while driving 38,000 km versus 35,000 electric-bus km. The source notes different traffic and energy sources, excludes several charger and training costs from the electricity figure, and says no like-for-like whole-cost calculation was produced. The tariff expires 31 October. (Section 3)
- L18: - Whole-cost savings, annual savings or fleet payback are unsupported: a like-for-like cost calculation was not produced and recorded expenditure omits costs and uses a temporary tariff (Fact 7).

### harbor-r2-A-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r2-A-Research/content.md)

- L31: 29. Electricity expenditure was EUR 8,400 under a temporary demonstration tariff.
- L32: 30. The six comparable diesel vehicles previously used EUR 13,700 of fuel while driving 38,000 km; the source says they faced different traffic and used a different energy source.
- L35: 33. Finance had not produced a like-for-like whole-cost calculation.
- L78: - A like-for-like whole-cost comparison, proven annual saving, or fleet payback period.
- L101: - Facts 23–34 concern energy figures, expenditure boundaries, and the absence of a like-for-like whole-cost calculation.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r2-A-Research/projection.md)

- L31: 29. Electricity expenditure was EUR 8,400 under a temporary demonstration tariff.
- L32: 30. The six comparable diesel vehicles previously used EUR 13,700 of fuel while driving 38,000 km; the source says they faced different traffic and used a different energy source.
- L35: 33. Finance had not produced a like-for-like whole-cost calculation.
- L74: - A like-for-like whole-cost comparison, proven annual saving, or fleet payback period.

### harbor-r2-B-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r2-B-Research/content.md)

- L8: 7. De elektriciteitsuitgave was EUR 8.400 onder een tijdelijk tarief; de vergelijkbare diesels gebruikten eerder EUR 13.700 aan brandstof, maar reden 38.000 km en verschilden in verkeer en energiebron. De elektriciteitsuitgave omvat geen lease van laders, elektrische upgrade, vaste kosten of werkplaatstraining. Het tarief vervalt op 31 oktober en Finance heeft geen vergelijkbare totale-kostenberekening gemaakt. (Energy and infrastructure account)
[projection.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r2-B-Research/projection.md)

- L9: 7. De elektriciteitsuitgave was EUR 8.400 onder een tijdelijk tarief; de vergelijkbare diesels gebruikten eerder EUR 13.700 aan brandstof, maar reden 38.000 km en verschilden in verkeer en energiebron. De elektriciteitsuitgave omvat geen lease van laders, elektrische upgrade, vaste kosten of werkplaatstraining. Het tarief vervalt op 31 oktober en Finance heeft geen vergelijkbare totale-kostenberekening gemaakt. (Energy and infrastructure account)
## harbor / carbon — kern

Bron [L16](../production-validation-experiment-6-2026-11-12/cases/harbor/source.md#L16): “Neither is a certified whole-life emissions estimate.”

Volledige oorspronkelijke bronregel (context en overige componenten):

> The electric buses used 42,000 kWh measured at the depot chargers. The figure includes charging losses up to the vehicle connection and excludes depot offices. Their recorded distance was 35,000 km. The energy team reports 1.20 kWh/km at this meter boundary. A brochure had quoted 1.05 kWh/km from vehicle telemetry, a different boundary. Neither is a certified whole-life emissions estimate. No manufacturing, battery replacement or grid-displacement analysis was commissioned. The operator's cleaner-transport aspiration must not become a demonstrated net-carbon reduction.

| Run | Classificatie | Research / projectie |
|---|---|---|
| harbor-r1-A-Research | correct | ruw L6, 35, 91; projectie L6, 35, 87 |
| harbor-r1-B-Research | correct | ruw L9, 20; projectie L10, 19 |
| harbor-r2-A-Research | correct | ruw L28, 30, 66, 77; projectie L28, 30, 66, 73 |
| harbor-r2-B-Research | correct | ruw L7, 16, 25; projectie L8, 17, 24 |

### harbor-r1-A-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r1-A-Research/content.md)

- L6: 4. The dossier says that phrase expresses the operator’s intended meaning, not a measured finding about emissions, public trust, or service quality.
- L35: 33. Neither energy figure is a certified whole-life emissions estimate.
- L91: - A certified whole-life emissions result or net-carbon reduction; the source says the available energy figures are not such an estimate and that relevant analyses were not commissioned.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r1-A-Research/projection.md)

- L6: 4. The dossier says that phrase expresses the operator’s intended meaning, not a measured finding about emissions, public trust, or service quality.
- L35: 33. Neither energy figure is a certified whole-life emissions estimate.
- L87: - A certified whole-life emissions result or net-carbon reduction; the source says the available energy figures are not such an estimate and that relevant analyses were not commissioned.

### harbor-r1-B-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r1-B-Research/content.md)

- L9: 8. The source says neither energy figure is a certified whole-life emissions estimate; manufacturing, battery replacement and grid-displacement analysis were not commissioned. It states that a net-carbon reduction is not demonstrated. (Section 3)
- L20: - Net whole-life emissions impact is not established (Fact 8).
[projection.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r1-B-Research/projection.md)

- L10: 8. The source says neither energy figure is a certified whole-life emissions estimate; manufacturing, battery replacement and grid-displacement analysis were not commissioned. It states that a net-carbon reduction is not demonstrated. (Section 3)
- L19: - Net whole-life emissions impact is not established (Fact 8).

### harbor-r2-A-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r2-A-Research/content.md)

- L28: 26. Neither energy figure is a certified whole-life emissions estimate.
- L30: 28. The source says the operator’s cleaner-transport aspiration must not be presented as a demonstrated net-carbon reduction.
- L66: 64. The operator describes the vehicles as “a visible first step toward cleaner local transport”; the source identifies this as the operator’s intended meaning, not a measured finding about emissions, public trust, or service quality.
- L77: - Whether the recorded energy figures correspond to a net-carbon reduction over the buses’ life cycle; the source says neither figure is a whole-life emissions estimate and that relevant analyses were not commissioned.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r2-A-Research/projection.md)

- L28: 26. Neither energy figure is a certified whole-life emissions estimate.
- L30: 28. The source says the operator’s cleaner-transport aspiration must not be presented as a demonstrated net-carbon reduction.
- L66: 64. The operator describes the vehicles as “a visible first step toward cleaner local transport”; the source identifies this as the operator’s intended meaning, not a measured finding about emissions, public trust, or service quality.
- L73: - Whether the recorded energy figures correspond to a net-carbon reduction over the buses’ life cycle; the source says neither figure is a whole-life emissions estimate and that relevant analyses were not commissioned.

### harbor-r2-B-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r2-B-Research/content.md)

- L7: 6. De elektrische bussen gebruikten volgens de depotmeter 42.000 kWh voor 35.000 km, oftewel 1,20 kWh/km op die meetgrens. De brochurewaarde van 1,05 kWh/km komt uit voertuigtelemetrie met een andere meetgrens. Er is geen gecertificeerde levenscyclusemissieberekening; productie, batterijvervanging en verdringing van netstroom zijn niet onderzocht. (Energy and infrastructure account)
- L16: 15. De operator noemt de bussen een “visible first step toward cleaner local transport”; de bron markeert dit als beoogde betekenis, niet als gemeten emissie-, vertrouwens- of service-uitkomst. (Purpose; Board minutes and communications)
- L25: - Netto-emissiereductie over de levenscyclus is niet vastgesteld. (Fact 6)
[projection.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r2-B-Research/projection.md)

- L8: 6. De elektrische bussen gebruikten volgens de depotmeter 42.000 kWh voor 35.000 km, oftewel 1,20 kWh/km op die meetgrens. De brochurewaarde van 1,05 kWh/km komt uit voertuigtelemetrie met een andere meetgrens. Er is geen gecertificeerde levenscyclusemissieberekening; productie, batterijvervanging en verdringing van netstroom zijn niet onderzocht. (Energy and infrastructure account)
- L17: 15. De operator noemt de bussen een “visible first step toward cleaner local transport”; de bron markeert dit als beoogde betekenis, niet als gemeten emissie-, vertrouwens- of service-uitkomst. (Purpose; Board minutes and communications)
- L24: - Netto-emissiereductie over de levenscyclus is niet vastgesteld. (Fact 6)
## harbor / audio — kern

Bron [L27](../production-validation-experiment-6-2026-11-12/cases/harbor/source.md#L27): “two had an audio-stop announcement failure during inspection.”

Volledige oorspronkelijke bronregel (context en overige componenten):

> Accessibility reviewers inspected three buses. All three had the specified wheelchair space, but two had an audio-stop announcement failure during inspection. The third passed both physical-space and audio checks. Physical access and functioning announcements are separate dimensions: a vehicle can pass one while failing the other. The maintenance team restored the two audio units on 2 July, after the trial ended. The final report must preserve both the trial-time failures and the later repair status. It must not present the whole demonstration as fully accessible merely because the space dimensions passed.

| Run | Classificatie | Research / projectie |
|---|---|---|
| harbor-r1-A-Research | correct | ruw L61, 63, 109; projectie L61, 63 |
| harbor-r1-B-Research | correct | ruw L7, 22, 25; projectie L8, 21 |
| harbor-r2-A-Research | correct | ruw L53, 55, 82, 93; projectie L53, 55, 78 |
| harbor-r2-B-Research | correct | ruw L12, 27, 33; projectie L13, 26 |

### harbor-r1-A-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r1-A-Research/content.md)

- L61: 59. Two of the three buses had an audio-stop announcement failure during inspection; the third passed both physical-space and audio checks.
- L63: 61. The maintenance team restored the two audio units on 2 July, after the trial ended.
- L109: - 2 July: the maintenance team restored the two audio units that had failed during inspection. (Facts 59, 61)
[projection.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r1-A-Research/projection.md)

- L61: 59. Two of the three buses had an audio-stop announcement failure during inspection; the third passed both physical-space and audio checks.
- L63: 61. The maintenance team restored the two audio units on 2 July, after the trial ended.

### harbor-r1-B-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r1-B-Research/content.md)

- L7: 6. Accessibility checks found the specified wheelchair space on all three inspected buses, but two had audio-stop announcement failures; the third passed both checks. The two audio units were restored on 2 July, after the trial. Physical space and functioning announcements are separate dimensions. (Section 4)
- L22: - The source does not establish that the two repaired audio units remained functional in subsequent service or that all operating accessibility conditions were met beyond the three inspections (Fact 6).
- L25: The March–May comparison period preceded the 4 May–28 June demonstration (Facts 2, 5). Duplicate trial departure IDs were removed from the draft count on 9 July (Fact 2); the two failed audio units were restored on 2 July, after the demonstration (Fact 6). On 18 September the board authorized preparation of a winter-assessment specification, not a further lease or procurement (Fact 10).
[projection.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r1-B-Research/projection.md)

- L8: 6. Accessibility checks found the specified wheelchair space on all three inspected buses, but two had audio-stop announcement failures; the third passed both checks. The two audio units were restored on 2 July, after the trial. Physical space and functioning announcements are separate dimensions. (Section 4)
- L21: - The source does not establish that the two repaired audio units remained functional in subsequent service or that all operating accessibility conditions were met beyond the three inspections (Fact 6).

### harbor-r2-A-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r2-A-Research/content.md)

- L53: 51. Two buses had an audio-stop announcement failure during inspection; the third passed both physical-space and audio checks.
- L55: 53. The maintenance team restored the two audio units on 2 July, after the trial ended.
- L82: - Whether all aspects of accessibility were functioning throughout the demonstration; the source records trial-time audio failures and their later repair, alongside the physical-space findings.
- L93: - 2 July 2026: the two failed audio units were restored.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r2-A-Research/projection.md)

- L53: 51. Two buses had an audio-stop announcement failure during inspection; the third passed both physical-space and audio checks.
- L55: 53. The maintenance team restored the two audio units on 2 July, after the trial ended.
- L78: - Whether all aspects of accessibility were functioning throughout the demonstration; the source records trial-time audio failures and their later repair, alongside the physical-space findings.

### harbor-r2-B-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r2-B-Research/content.md)

- L12: 11. Drie bussen zijn op toegankelijkheid geïnspecteerd: alle drie hadden de voorgeschreven rolstoelplek, maar bij twee faalde tijdens de inspectie de audiohalte-aankondiging. De derde slaagde voor beide controles. De twee audio-units zijn op 2 juli, na afloop van de proef, hersteld. (Rider and worker evidence)
- L27: - De bevindingen tonen niet dat alle bussen tijdens de proef volledig toegankelijk waren: twee audio-aankondigingen faalden bij inspectie en werden pas na de proef hersteld. Effect van de latere reparatie tijdens de proef is niet van toepassing. (Fact 11)
- L33: - 2 juli: de twee audio-units werden na afloop van de proef hersteld. 9 juli: 30 dubbele vertrek-ID’s werden uit het conceptcijfer verwijderd. (Facts 2, 11)
[projection.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r2-B-Research/projection.md)

- L13: 11. Drie bussen zijn op toegankelijkheid geïnspecteerd: alle drie hadden de voorgeschreven rolstoelplek, maar bij twee faalde tijdens de inspectie de audiohalte-aankondiging. De derde slaagde voor beide controles. De twee audio-units zijn op 2 juli, na afloop van de proef, hersteld. (Rider and worker evidence)
- L26: - De bevindingen tonen niet dat alle bussen tijdens de proef volledig toegankelijk waren: twee audio-aankondigingen faalden bij inspectie en werden pas na de proef hersteld. Effect van de latere reparatie tijdens de proef is niet van toepassing. (Fact 11)
## harbor / repair — kern

Bron [L27](../production-validation-experiment-6-2026-11-12/cases/harbor/source.md#L27): “The maintenance team restored the two audio units on 2 July, after the trial ended.”

Volledige oorspronkelijke bronregel (context en overige componenten):

> Accessibility reviewers inspected three buses. All three had the specified wheelchair space, but two had an audio-stop announcement failure during inspection. The third passed both physical-space and audio checks. Physical access and functioning announcements are separate dimensions: a vehicle can pass one while failing the other. The maintenance team restored the two audio units on 2 July, after the trial ended. The final report must preserve both the trial-time failures and the later repair status. It must not present the whole demonstration as fully accessible merely because the space dimensions passed.

| Run | Classificatie | Research / projectie |
|---|---|---|
| harbor-r1-A-Research | correct | ruw L63, 109; projectie L63 |
| harbor-r1-B-Research | correct | ruw L7, 25; projectie L8 |
| harbor-r2-A-Research | correct | ruw L55, 93; projectie L55 |
| harbor-r2-B-Research | correct | ruw L12, 33; projectie L13 |

### harbor-r1-A-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r1-A-Research/content.md)

- L63: 61. The maintenance team restored the two audio units on 2 July, after the trial ended.
- L109: - 2 July: the maintenance team restored the two audio units that had failed during inspection. (Facts 59, 61)
[projection.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r1-A-Research/projection.md)

- L63: 61. The maintenance team restored the two audio units on 2 July, after the trial ended.

### harbor-r1-B-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r1-B-Research/content.md)

- L7: 6. Accessibility checks found the specified wheelchair space on all three inspected buses, but two had audio-stop announcement failures; the third passed both checks. The two audio units were restored on 2 July, after the trial. Physical space and functioning announcements are separate dimensions. (Section 4)
- L25: The March–May comparison period preceded the 4 May–28 June demonstration (Facts 2, 5). Duplicate trial departure IDs were removed from the draft count on 9 July (Fact 2); the two failed audio units were restored on 2 July, after the demonstration (Fact 6). On 18 September the board authorized preparation of a winter-assessment specification, not a further lease or procurement (Fact 10).
[projection.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r1-B-Research/projection.md)

- L8: 6. Accessibility checks found the specified wheelchair space on all three inspected buses, but two had audio-stop announcement failures; the third passed both checks. The two audio units were restored on 2 July, after the trial. Physical space and functioning announcements are separate dimensions. (Section 4)

### harbor-r2-A-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r2-A-Research/content.md)

- L55: 53. The maintenance team restored the two audio units on 2 July, after the trial ended.
- L93: - 2 July 2026: the two failed audio units were restored.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r2-A-Research/projection.md)

- L55: 53. The maintenance team restored the two audio units on 2 July, after the trial ended.

### harbor-r2-B-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r2-B-Research/content.md)

- L12: 11. Drie bussen zijn op toegankelijkheid geïnspecteerd: alle drie hadden de voorgeschreven rolstoelplek, maar bij twee faalde tijdens de inspectie de audiohalte-aankondiging. De derde slaagde voor beide controles. De twee audio-units zijn op 2 juli, na afloop van de proef, hersteld. (Rider and worker evidence)
- L33: - 2 juli: de twee audio-units werden na afloop van de proef hersteld. 9 juli: 30 dubbele vertrek-ID’s werden uit het conceptcijfer verwijderd. (Facts 2, 11)
[projection.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r2-B-Research/projection.md)

- L13: 11. Drie bussen zijn op toegankelijkheid geïnspecteerd: alle drie hadden de voorgeschreven rolstoelplek, maar bij twee faalde tijdens de inspectie de audiohalte-aankondiging. De derde slaagde voor beide controles. De twee audio-units zijn op 2 juli, na afloop van de proef, hersteld. (Rider and worker evidence)
## harbor / winter — kern

Bron [L30](../production-validation-experiment-6-2026-11-12/cases/harbor/source.md#L30): “There were no freezing days and no winter heating loads.”

Volledige oorspronkelijke bronregel (context en overige componenten):

> The demonstration recorded daytime temperatures between 14 and 25 degrees Celsius. There were no freezing days and no winter heating loads. The leased buses have not been tested on the hill route in winter. The engineering team proposes a winter run with heating and matched route allocation. The proposal is not a completed trial and has no measured result. A charging-capacity spreadsheet assumes each vehicle returns by 23:00; dispatch logs show five returns after that time. It is a planning assumption with observed exceptions, not an established operating guarantee.

| Run | Classificatie | Research / projectie |
|---|---|---|
| harbor-r1-A-Research | correct | ruw L7, 65, 66, 67, 75, 96, 99, 111, 112, 121; projectie L7, 65, 66, 67, 75, 92, 95 |
| harbor-r1-B-Research | correct | ruw L10, 11, 21, 25, 28; projectie L11, 12, 20 |
| harbor-r2-A-Research | correct | ruw L57, 58, 59, 62, 83, 95, 104; projectie L57, 58, 59, 62, 79 |
| harbor-r2-B-Research | correct | ruw L13, 15, 28, 29, 34, 38; projectie L14, 16, 27, 28 |

### harbor-r1-A-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r1-A-Research/content.md)

- L7: 5. The board’s decision boundary was whether to commission a winter assessment, not whether to replace the entire fleet.
- L65: 63. Demonstration daytime temperatures ranged from 14 to 25 degrees Celsius; there were no freezing days and no winter heating loads.
- L66: 64. The leased buses had not been tested on the hill route in winter.
- L67: 65. The engineering team proposed a winter run with heating and matched route allocation; the source says this proposal is not a completed trial and has no measured result.
- L75: 73. On 18 September, the board authorized preparation of a winter-assessment specification within the existing planning budget.
- L96: - Whether the buses would perform on the hill route in winter, including with heating loads; the proposed winter run has no measured result.
- L99: - The reason the board authorized preparation of a winter-assessment specification, beyond the decision recorded in the minutes.
- L111: - 18 September: the board authorized preparation of a winter-assessment specification within the existing planning budget. It did not authorize another bus lease, depot conversion, or procurement award. (Facts 73–74)
- L112: - The proposed winter run with heating and matched route allocation remained a proposal, not a completed trial with a measured result. (Fact 65)
- L121: - Facts 63–67 are relevant to seasonal limits and the proposed winter assessment.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r1-A-Research/projection.md)

- L7: 5. The board’s decision boundary was whether to commission a winter assessment, not whether to replace the entire fleet.
- L65: 63. Demonstration daytime temperatures ranged from 14 to 25 degrees Celsius; there were no freezing days and no winter heating loads.
- L66: 64. The leased buses had not been tested on the hill route in winter.
- L67: 65. The engineering team proposed a winter run with heating and matched route allocation; the source says this proposal is not a completed trial and has no measured result.
- L75: 73. On 18 September, the board authorized preparation of a winter-assessment specification within the existing planning budget.
- L92: - Whether the buses would perform on the hill route in winter, including with heating loads; the proposed winter run has no measured result.
- L95: - The reason the board authorized preparation of a winter-assessment specification, beyond the decision recorded in the minutes.

### harbor-r1-B-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r1-B-Research/content.md)

- L10: 9. The demonstration had no freezing days or winter heating loads, and the leased buses had not been tested on the hill route in winter. The engineering team proposed a winter run with heating and matched route allocation. (Section 5)
- L11: 10. On 18 September, the board authorized preparation of a winter-assessment specification within the existing planning budget. It did not authorize another lease, depot conversion or procurement award. The board rejected draft wording that the pilot had “proved the business case” and approved a description of “encouraging operational experience” with unresolved cost, seasonality and comparison questions. (Section 6)
- L21: - Winter performance, heating energy needs and performance on the hill route remain untested; the winter run is a proposal, not a result (Fact 9).
- L25: The March–May comparison period preceded the 4 May–28 June demonstration (Facts 2, 5). Duplicate trial departure IDs were removed from the draft count on 9 July (Fact 2); the two failed audio units were restored on 2 July, after the demonstration (Fact 6). On 18 September the board authorized preparation of a winter-assessment specification, not a further lease or procurement (Fact 10).
- L28: Core evidence: Facts 2–5 (service result and comparison limits), 6 (separate accessibility dimensions), 7–8 (cost and environmental evidence boundaries), and 9–10 (winter evidence gap and limited next decision).
[projection.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r1-B-Research/projection.md)

- L11: 9. The demonstration had no freezing days or winter heating loads, and the leased buses had not been tested on the hill route in winter. The engineering team proposed a winter run with heating and matched route allocation. (Section 5)
- L12: 10. On 18 September, the board authorized preparation of a winter-assessment specification within the existing planning budget. It did not authorize another lease, depot conversion or procurement award. The board rejected draft wording that the pilot had “proved the business case” and approved a description of “encouraging operational experience” with unresolved cost, seasonality and comparison questions. (Section 6)
- L20: - Winter performance, heating energy needs and performance on the hill route remain untested; the winter run is a proposal, not a result (Fact 9).

### harbor-r2-A-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r2-A-Research/content.md)

- L57: 55. Demonstration daytime temperatures ranged from 14 to 25 degrees Celsius; there were no freezing days and no winter heating loads.
- L58: 56. The leased buses had not been tested on the hill route in winter.
- L59: 57. The engineering team proposed a winter run with heating and matched route allocation; the proposal was not a completed trial and had no measured result.
- L62: 60. On 18 September 2026, the board authorized preparation of a winter-assessment specification within the existing planning budget.
- L83: - Winter performance, including performance on the hill route with heating loads; the proposed winter run had no measured result.
- L95: - 18 September 2026: the board authorized preparation of a winter-assessment specification and did not authorize a further lease, depot conversion, or procurement award.
- L104: - Facts 55–59 concern winter exposure and the limits of the charging-capacity assumption.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r2-A-Research/projection.md)

- L57: 55. Demonstration daytime temperatures ranged from 14 to 25 degrees Celsius; there were no freezing days and no winter heating loads.
- L58: 56. The leased buses had not been tested on the hill route in winter.
- L59: 57. The engineering team proposed a winter run with heating and matched route allocation; the proposal was not a completed trial and had no measured result.
- L62: 60. On 18 September 2026, the board authorized preparation of a winter-assessment specification within the existing planning budget.
- L79: - Winter performance, including performance on the hill route with heating loads; the proposed winter run had no measured result.

### harbor-r2-B-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r2-B-Research/content.md)

- L13: 12. Tijdens de proef lagen de dagtemperaturen tussen 14 en 25 °C; er waren geen vriesdagen of winterse verwarmingsbelastingen. De bussen zijn niet in de winter op de heuvelroute getest. Engineering stelde een winterproef met verwarming en gematchte route-indeling voor. Een laadc spreadsheet neemt terugkeer vóór 23.00 uur aan, terwijl dispatchlogs vijf latere terugkomsten tonen. (Missing exposure and seasonal limits)
- L15: 14. Op 18 september gaf het bestuur toestemming om binnen het bestaande planningsbudget een specificatie voor een winterbeoordeling op te stellen; het gaf geen toestemming voor een nieuwe lease, depotombouw of aanbesteding. Het bestuur verwierp de formulering dat de proef de businesscase voor elektrificatie van de vloot had bewezen. De goedgekeurde tekst noemt de ervaring bemoedigend en vermeldt onopgeloste kosten-, seizoens- en vergelijkingsvragen. Dit is de toegeschreven bestuursduiding, geen onafhankelijke causale bevinding. (Board minutes and communications)
- L28: - Winterprestaties, prestaties op de heuvelroute onder winterse omstandigheden en de veronderstelde laadcapaciteit bij terugkeer vóór 23.00 uur zijn niet aangetoond. De voorgestelde winterbeoordeling heeft nog geen gemeten resultaat. (Fact 12)
- L29: - De bron ondersteunt geen algemene aanbeveling voor andere vervoerders en geen vlootbrede elektrificatiebeslissing; het formele besluit betrof alleen voorbereiding van een winterbeoordelingsspecificatie. (Fact 14)
- L34: - 18 september: het bestuur gaf alleen toestemming voor voorbereiding van een winterbeoordelingsspecificatie en verwierp de claim dat de businesscase bewezen was. (Fact 14)
- L38: Kern: Facts 2–5 (service en vergelijkbaarheid), 7–8 (kosten en laadcontext), 11–12 (toegankelijkheid en wintergrenzen), 14 (beperkt vervolgbesluit). Voor aanvullende grenzen: Facts 6, 9–10, 13 en 15.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r2-B-Research/projection.md)

- L14: 12. Tijdens de proef lagen de dagtemperaturen tussen 14 en 25 °C; er waren geen vriesdagen of winterse verwarmingsbelastingen. De bussen zijn niet in de winter op de heuvelroute getest. Engineering stelde een winterproef met verwarming en gematchte route-indeling voor. Een laadc spreadsheet neemt terugkeer vóór 23.00 uur aan, terwijl dispatchlogs vijf latere terugkomsten tonen. (Missing exposure and seasonal limits)
- L16: 14. Op 18 september gaf het bestuur toestemming om binnen het bestaande planningsbudget een specificatie voor een winterbeoordeling op te stellen; het gaf geen toestemming voor een nieuwe lease, depotombouw of aanbesteding. Het bestuur verwierp de formulering dat de proef de businesscase voor elektrificatie van de vloot had bewezen. De goedgekeurde tekst noemt de ervaring bemoedigend en vermeldt onopgeloste kosten-, seizoens- en vergelijkingsvragen. Dit is de toegeschreven bestuursduiding, geen onafhankelijke causale bevinding. (Board minutes and communications)
- L27: - Winterprestaties, prestaties op de heuvelroute onder winterse omstandigheden en de veronderstelde laadcapaciteit bij terugkeer vóór 23.00 uur zijn niet aangetoond. De voorgestelde winterbeoordeling heeft nog geen gemeten resultaat. (Fact 12)
- L28: - De bron ondersteunt geen algemene aanbeveling voor andere vervoerders en geen vlootbrede elektrificatiebeslissing; het formele besluit betrof alleen voorbereiding van een winterbeoordelingsspecificatie. (Fact 14)
## harbor / decision — kern

Bron [L35](../production-validation-experiment-6-2026-11-12/cases/harbor/source.md#L35): “the board authorized preparation of a winter-assessment specification within the existing planning budget.”

Volledige oorspronkelijke bronregel (context en overige componenten):

> On 18 September the board authorized preparation of a winter-assessment specification within the existing planning budget. It did not authorize another bus lease, a depot conversion or a procurement award. The communications draft saying “the pilot has proved the business case for electrifying the fleet” was rejected. The approved wording calls the demonstration encouraging operational experience, with unresolved cost, seasonality and comparison questions. That is an attributed board interpretation, not an independent causal finding.

| Run | Classificatie | Research / projectie |
|---|---|---|
| harbor-r1-A-Research | correct | ruw L75, 99, 111; projectie L75, 95 |
| harbor-r1-B-Research | correct | ruw L11, 25; projectie L12 |
| harbor-r2-A-Research | correct | ruw L62, 95; projectie L62 |
| harbor-r2-B-Research | correct | ruw L15, 29, 34; projectie L16, 28 |

### harbor-r1-A-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r1-A-Research/content.md)

- L75: 73. On 18 September, the board authorized preparation of a winter-assessment specification within the existing planning budget.
- L99: - The reason the board authorized preparation of a winter-assessment specification, beyond the decision recorded in the minutes.
- L111: - 18 September: the board authorized preparation of a winter-assessment specification within the existing planning budget. It did not authorize another bus lease, depot conversion, or procurement award. (Facts 73–74)
[projection.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r1-A-Research/projection.md)

- L75: 73. On 18 September, the board authorized preparation of a winter-assessment specification within the existing planning budget.
- L95: - The reason the board authorized preparation of a winter-assessment specification, beyond the decision recorded in the minutes.

### harbor-r1-B-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r1-B-Research/content.md)

- L11: 10. On 18 September, the board authorized preparation of a winter-assessment specification within the existing planning budget. It did not authorize another lease, depot conversion or procurement award. The board rejected draft wording that the pilot had “proved the business case” and approved a description of “encouraging operational experience” with unresolved cost, seasonality and comparison questions. (Section 6)
- L25: The March–May comparison period preceded the 4 May–28 June demonstration (Facts 2, 5). Duplicate trial departure IDs were removed from the draft count on 9 July (Fact 2); the two failed audio units were restored on 2 July, after the demonstration (Fact 6). On 18 September the board authorized preparation of a winter-assessment specification, not a further lease or procurement (Fact 10).
[projection.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r1-B-Research/projection.md)

- L12: 10. On 18 September, the board authorized preparation of a winter-assessment specification within the existing planning budget. It did not authorize another lease, depot conversion or procurement award. The board rejected draft wording that the pilot had “proved the business case” and approved a description of “encouraging operational experience” with unresolved cost, seasonality and comparison questions. (Section 6)

### harbor-r2-A-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r2-A-Research/content.md)

- L62: 60. On 18 September 2026, the board authorized preparation of a winter-assessment specification within the existing planning budget.
- L95: - 18 September 2026: the board authorized preparation of a winter-assessment specification and did not authorize a further lease, depot conversion, or procurement award.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r2-A-Research/projection.md)

- L62: 60. On 18 September 2026, the board authorized preparation of a winter-assessment specification within the existing planning budget.

### harbor-r2-B-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r2-B-Research/content.md)

- L15: 14. Op 18 september gaf het bestuur toestemming om binnen het bestaande planningsbudget een specificatie voor een winterbeoordeling op te stellen; het gaf geen toestemming voor een nieuwe lease, depotombouw of aanbesteding. Het bestuur verwierp de formulering dat de proef de businesscase voor elektrificatie van de vloot had bewezen. De goedgekeurde tekst noemt de ervaring bemoedigend en vermeldt onopgeloste kosten-, seizoens- en vergelijkingsvragen. Dit is de toegeschreven bestuursduiding, geen onafhankelijke causale bevinding. (Board minutes and communications)
- L29: - De bron ondersteunt geen algemene aanbeveling voor andere vervoerders en geen vlootbrede elektrificatiebeslissing; het formele besluit betrof alleen voorbereiding van een winterbeoordelingsspecificatie. (Fact 14)
- L34: - 18 september: het bestuur gaf alleen toestemming voor voorbereiding van een winterbeoordelingsspecificatie en verwierp de claim dat de businesscase bewezen was. (Fact 14)
[projection.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r2-B-Research/projection.md)

- L16: 14. Op 18 september gaf het bestuur toestemming om binnen het bestaande planningsbudget een specificatie voor een winterbeoordeling op te stellen; het gaf geen toestemming voor een nieuwe lease, depotombouw of aanbesteding. Het bestuur verwierp de formulering dat de proef de businesscase voor elektrificatie van de vloot had bewezen. De goedgekeurde tekst noemt de ervaring bemoedigend en vermeldt onopgeloste kosten-, seizoens- en vergelijkingsvragen. Dit is de toegeschreven bestuursduiding, geen onafhankelijke causale bevinding. (Board minutes and communications)
- L28: - De bron ondersteunt geen algemene aanbeveling voor andere vervoerders en geen vlootbrede elektrificatiebeslissing; het formele besluit betrof alleen voorbereiding van een winterbeoordelingsspecificatie. (Fact 14)
## harbor / feedback — context

Bron [L23](../production-validation-experiment-6-2026-11-12/cases/harbor/source.md#L23): “These are volunteered comments from selected sessions, not 148 independently sampled citizens and not a representative satisfaction score.”

Volledige oorspronkelijke bronregel (context en overige componenten):

> Staff handed feedback cards to riders on four Tuesday mornings. There were 148 returned cards, of which 112 described the ride as quieter, 21 were neutral and 15 reported another issue without answering the noise question. The total number approached and the number of unique riders are unknown. The same regular rider could return cards in more than one week. These are volunteered comments from selected sessions, not 148 independently sampled citizens and not a representative satisfaction score.

| Run | Classificatie | Research / projectie |
|---|---|---|
| harbor-r1-A-Research | correct | ruw L51, 54, 94; projectie L51, 54, 90 |
| harbor-r1-B-Research | ontbreekt | geen anker in ruw; geen anker in projectie |
| harbor-r2-A-Research | correct | ruw L43, 46, 80; projectie L43, 46, 76 |
| harbor-r2-B-Research | correct | ruw L10, 26; projectie L11, 25 |

### harbor-r1-A-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r1-A-Research/content.md)

- L51: 49. Staff handed feedback cards to riders on four Tuesday mornings; 148 cards were returned.
- L54: 52. The source describes the cards as volunteered comments from selected sessions, not 148 independently sampled citizens or a representative satisfaction score.
- L94: - A representative rider satisfaction result or the number of unique riders and total riders approached.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r1-A-Research/projection.md)

- L51: 49. Staff handed feedback cards to riders on four Tuesday mornings; 148 cards were returned.
- L54: 52. The source describes the cards as volunteered comments from selected sessions, not 148 independently sampled citizens or a representative satisfaction score.
- L90: - A representative rider satisfaction result or the number of unique riders and total riders approached.

### harbor-r1-B-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r1-B-Research/content.md)

Geen passend bewijsanker; volledige tekst is beslissend.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r1-B-Research/projection.md)

Geen passend bewijsanker; volledige tekst is beslissend.

### harbor-r2-A-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r2-A-Research/content.md)

- L43: 41. Staff handed feedback cards to riders on four Tuesday mornings, and 148 cards were returned.
- L46: 44. The source describes the cards as volunteered comments from selected sessions, not 148 independently sampled citizens or a representative satisfaction score.
- L80: - A representative rider satisfaction score or the number of unique riders represented by the returned cards.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r2-A-Research/projection.md)

- L43: 41. Staff handed feedback cards to riders on four Tuesday mornings, and 148 cards were returned.
- L46: 44. The source describes the cards as volunteered comments from selected sessions, not 148 independently sampled citizens or a representative satisfaction score.
- L76: - A representative rider satisfaction score or the number of unique riders represented by the returned cards.

### harbor-r2-B-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r2-B-Research/content.md)

- L10: 9. Van 148 teruggegeven feedbackkaarten beschreven 112 de rit als stiller, 21 waren neutraal en 15 noemden een ander punt zonder de geluidsvraag te beantwoorden. Het aantal benaderde mensen en unieke reizigers is onbekend; dezelfde vaste reiziger kon vaker reageren. De kaarten kwamen uit vier geselecteerde dinsdagochtenden en zijn volgens de bron geen representatieve tevredenheidsscore. (Rider and worker evidence)
- L26: - Representatieve reizigerstevredenheid, langetermijnveiligheid en medische effecten zijn niet vastgesteld. (Facts 9–10)
[projection.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r2-B-Research/projection.md)

- L11: 9. Van 148 teruggegeven feedbackkaarten beschreven 112 de rit als stiller, 21 waren neutraal en 15 noemden een ander punt zonder de geluidsvraag te beantwoorden. Het aantal benaderde mensen en unieke reizigers is onbekend; dezelfde vaste reiziger kon vaker reageren. De kaarten kwamen uit vier geselecteerde dinsdagochtenden en zijn volgens de bron geen representatieve tevredenheidsscore. (Rider and worker evidence)
- L25: - Representatieve reizigerstevredenheid, langetermijnveiligheid en medische effecten zijn niet vastgesteld. (Facts 9–10)
## harbor / meaning — context

Bron [L4](../production-validation-experiment-6-2026-11-12/cases/harbor/source.md#L4): “That phrase is the operator's intended meaning, not a measured finding about emissions, public trust or service quality.”

Volledige oorspronkelijke bronregel (context en overige componenten):

> HarborLink is a fictional regional operator. This dossier is a synthetic operational source for an evaluation, not a real transport study. It records an eight-week demonstration of six leased electric buses at Seabank depot. The operator describes the vehicles as “a visible first step toward cleaner local transport”. That phrase is the operator's intended meaning, not a measured finding about emissions, public trust or service quality. The board must decide whether to commission a winter assessment, not whether to replace the entire fleet. No procurement approval has been given.

| Run | Classificatie | Research / projectie |
|---|---|---|
| harbor-r1-A-Research | correct | ruw L5, 6; projectie L5, 6 |
| harbor-r1-B-Research | ontbreekt | geen anker in ruw; geen anker in projectie |
| harbor-r2-A-Research | correct | ruw L66; projectie L66 |
| harbor-r2-B-Research | correct | ruw L16; projectie L17 |

### harbor-r1-A-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r1-A-Research/content.md)

- L5: 3. The operator described the buses as “a visible first step toward cleaner local transport.”
- L6: 4. The dossier says that phrase expresses the operator’s intended meaning, not a measured finding about emissions, public trust, or service quality.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r1-A-Research/projection.md)

- L5: 3. The operator described the buses as “a visible first step toward cleaner local transport.”
- L6: 4. The dossier says that phrase expresses the operator’s intended meaning, not a measured finding about emissions, public trust, or service quality.

### harbor-r1-B-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r1-B-Research/content.md)

Geen passend bewijsanker; volledige tekst is beslissend.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r1-B-Research/projection.md)

Geen passend bewijsanker; volledige tekst is beslissend.

### harbor-r2-A-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r2-A-Research/content.md)

- L66: 64. The operator describes the vehicles as “a visible first step toward cleaner local transport”; the source identifies this as the operator’s intended meaning, not a measured finding about emissions, public trust, or service quality.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r2-A-Research/projection.md)

- L66: 64. The operator describes the vehicles as “a visible first step toward cleaner local transport”; the source identifies this as the operator’s intended meaning, not a measured finding about emissions, public trust, or service quality.

### harbor-r2-B-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r2-B-Research/content.md)

- L16: 15. De operator noemt de bussen een “visible first step toward cleaner local transport”; de bron markeert dit als beoogde betekenis, niet als gemeten emissie-, vertrouwens- of service-uitkomst. (Purpose; Board minutes and communications)
[projection.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r2-B-Research/projection.md)

- L17: 15. De operator noemt de bussen een “visible first step toward cleaner local transport”; de bron markeert dit als beoogde betekenis, niet als gemeten emissie-, vertrouwens- of service-uitkomst. (Purpose; Board minutes and communications)
## harbor / maintenance — context

Bron [L32](../production-validation-experiment-6-2026-11-12/cases/harbor/source.md#L32): “The count difference cannot by itself establish greater reliability.”

Volledige oorspronkelijke bronregel (context en overige componenten):

> The maintenance register had 11 entries during the demonstration and 16 during the prior period. The new form grouped repeated charger warnings into one incident, whereas the older form listed each warning. The count difference cannot by itself establish greater reliability. Separately, there were two confirmed tire replacements in each period under the same parts coding. Neither the tire entries nor the aggregate maintenance counts reveal an isolated propulsion effect.

| Run | Classificatie | Research / projectie |
|---|---|---|
| harbor-r1-A-Research | correct | ruw L63, 70, 72, 74, 98, 109, 122; projectie L63, 70, 72, 74, 94 |
| harbor-r1-B-Research | ontbreekt | geen anker in ruw; geen anker in projectie |
| harbor-r2-A-Research | ontbreekt | geen anker in ruw; geen anker in projectie |
| harbor-r2-B-Research | correct | ruw L14; projectie L15 |

### harbor-r1-A-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r1-A-Research/content.md)

- L63: 61. The maintenance team restored the two audio units on 2 July, after the trial ended.
- L70: 68. The maintenance register had 11 entries during the demonstration and 16 during the prior period.
- L72: 70. The source says the count difference cannot by itself establish greater reliability.
- L74: 72. The source says neither the tire entries nor the aggregate maintenance counts reveal an isolated propulsion effect.
- L98: - The cause of the difference in aggregate maintenance-entry counts as a measure of reliability; the forms counted warnings differently.
- L109: - 2 July: the maintenance team restored the two audio units that had failed during inspection. (Facts 59, 61)
- L122: - Facts 49–57 and 68–72 provide additional rider, driver, safety, and maintenance evidence with the stated limitations.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r1-A-Research/projection.md)

- L63: 61. The maintenance team restored the two audio units on 2 July, after the trial ended.
- L70: 68. The maintenance register had 11 entries during the demonstration and 16 during the prior period.
- L72: 70. The source says the count difference cannot by itself establish greater reliability.
- L74: 72. The source says neither the tire entries nor the aggregate maintenance counts reveal an isolated propulsion effect.
- L94: - The cause of the difference in aggregate maintenance-entry counts as a measure of reliability; the forms counted warnings differently.

### harbor-r1-B-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r1-B-Research/content.md)

Geen passend bewijsanker; volledige tekst is beslissend.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r1-B-Research/projection.md)

Geen passend bewijsanker; volledige tekst is beslissend.

### harbor-r2-A-Research

De onderhoudsregistervergelijking ontbreekt; maintenance bij audioreparatie is geen equivalent bewijs.

[content.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r2-A-Research/content.md)

Geen passend bewijsanker; volledige tekst is beslissend.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r2-A-Research/projection.md)

Geen passend bewijsanker; volledige tekst is beslissend.

### harbor-r2-B-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r2-B-Research/content.md)

- L14: 13. Het onderhoudsregister telde 11 vermeldingen tijdens de proef en 16 in de voorafgaande periode, maar het nieuwe formulier bundelde herhaalde ladermeldingen terwijl het oude die afzonderlijk noteerde. De aantallen bewijzen op zichzelf geen grotere betrouwbaarheid. Er waren in beide perioden twee bevestigde bandenvervangingen onder dezelfde codering. (Missing exposure and seasonal limits)
[projection.md](../production-validation-experiment-6-2026-11-12/calls/harbor-r2-B-Research/projection.md)

- L15: 13. Het onderhoudsregister telde 11 vermeldingen tijdens de proef en 16 in de voorafgaande periode, maar het nieuwe formulier bundelde herhaalde ladermeldingen terwijl het oude die afzonderlijk noteerde. De aantallen bewijzen op zichzelf geen grotere betrouwbaarheid. Er waren in beide perioden twee bevestigde bandenvervangingen onder dezelfde codering. (Missing exposure and seasonal limits)
## archive / output — kern

Bron [L6](../production-validation-experiment-6-2026-11-12/cases/archive/source.md#L6): “It created 900 catalogue records: 300 from each collection.”

Volledige oorspronkelijke bronregel (context en overige componenten):

> The project ran for ten weeks from 6 April to 14 June. It created 900 catalogue records: 300 from each collection. Paper originals remain in controlled storage. Digitization here means a searchable catalogue entry plus a scanned page; it does not mean the original has been replaced or that full-text transcription is verified. Images can be legible while their descriptive metadata is wrong. A catalogue can have accurate names while putting records in the wrong order. These dimensions require separate treatment.

| Run | Classificatie | Research / projectie |
|---|---|---|
| archive-r1-A-Research | correct | ruw L5, 29, 38, 48, 49, 57; projectie L5, 29, 38, 44, 45 |
| archive-r1-B-Research | correct | ruw L2, 7, 13, 16, 17, 21; projectie L3, 8, 14, 15 |
| archive-r2-A-Research | correct | ruw L5, 32, 43, 64; projectie L5, 32, 43 |
| archive-r2-B-Research | correct | ruw L3, 8, 11, 18, 20, 24; projectie L4, 9, 12, 16, 18 |

### archive-r1-A-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/archive-r1-A-Research/content.md)

- L5: 3. The pilot created 900 catalogue records, with 300 records from each collection.
- L29: 27. Rights clearance is complete for 760 of the 900 scans.
- L38: 36. For communication, the committee wants the pilot’s 900 catalogue entries acknowledged.
- L48: - Whether all 900 catalogue records have accurate descriptions or correct chronological placement is not established. The reviewed records were a non-random selection, and the recheck covered only the 17 corrected entries.
- L49: - Whether all 900 scans are publicly downloadable is not established as true; the source states that images for 140 records are not publicly downloadable.
- L57: - 6 April–14 June 2026: The pilot ran and created 900 catalogue records across Cedar, Elm and Rowan. (Facts 2–3)
[projection.md](../production-validation-experiment-6-2026-11-12/calls/archive-r1-A-Research/projection.md)

- L5: 3. The pilot created 900 catalogue records, with 300 records from each collection.
- L29: 27. Rights clearance is complete for 760 of the 900 scans.
- L38: 36. For communication, the committee wants the pilot’s 900 catalogue entries acknowledged.
- L44: - Whether all 900 catalogue records have accurate descriptions or correct chronological placement is not established. The reviewed records were a non-random selection, and the recheck covered only the 17 corrected entries.
- L45: - Whether all 900 scans are publicly downloadable is not established as true; the source states that images for 140 records are not publicly downloadable.

### archive-r1-B-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/archive-r1-B-Research/content.md)

- L2: 1. The dossier identifies City Memory as a fictional municipal archive and the report as synthetic, not a real archive report. Its ten-week pilot (6 April–14 June) created 900 catalogue records—300 each for selected shipping registers from Cedar, Elm and Rowan—not a catalogue of all holdings. (Sections 1, 7)
- L7: 6. Rights clearance is complete for 760 of the 900 scans. The remaining 140 descriptions are visible, but their page images are held internally and are not publicly downloadable. Review next quarter is an expectation, not a committed release date. (Section 5)
- L13: - The 900-entry output and specific corrections are documented, but the source supports a limited next step—designing an audit—not a full rollout or comprehensive quality certification. (Facts 1, 5, 8)
- L16: - The source does not establish the ordering or description accuracy of all 900 records, or that the reviewed records represent the full collections; fragile-binding material was excluded and the review was not random. (Facts 3–5)
- L17: - It does not establish that all 900 page images are publicly available: 140 are withheld pending rights review, with no committed release date. (Fact 6)
- L21: - 6 April–14 June: the pilot created 900 catalogue records. (Fact 1)
[projection.md](../production-validation-experiment-6-2026-11-12/calls/archive-r1-B-Research/projection.md)

- L3: 1. The dossier identifies City Memory as a fictional municipal archive and the report as synthetic, not a real archive report. Its ten-week pilot (6 April–14 June) created 900 catalogue records—300 each for selected shipping registers from Cedar, Elm and Rowan—not a catalogue of all holdings. (Sections 1, 7)
- L8: 6. Rights clearance is complete for 760 of the 900 scans. The remaining 140 descriptions are visible, but their page images are held internally and are not publicly downloadable. Review next quarter is an expectation, not a committed release date. (Section 5)
- L14: - The source does not establish the ordering or description accuracy of all 900 records, or that the reviewed records represent the full collections; fragile-binding material was excluded and the review was not random. (Facts 3–5)
- L15: - It does not establish that all 900 page images are publicly available: 140 are withheld pending rights review, with no committed release date. (Fact 6)

### archive-r2-A-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/archive-r2-A-Research/content.md)

- L5: 3. The project created 900 catalogue records.
- L32: 30. Rights clearance is complete for 760 of the 900 scans.
- L43: 41. For communication, the committee wants the pilot’s output acknowledged: 900 catalogue entries exist, and observed errors have received specific corrections.
- L64: - 6 April–14 June 2026: the ten-week pilot created 900 catalogue records across Cedar, Elm and Rowan. (Facts 1–4)
[projection.md](../production-validation-experiment-6-2026-11-12/calls/archive-r2-A-Research/projection.md)

- L5: 3. The project created 900 catalogue records.
- L32: 30. Rights clearance is complete for 760 of the 900 scans.
- L43: 41. For communication, the committee wants the pilot’s output acknowledged: 900 catalogue entries exist, and observed errors have received specific corrections.

### archive-r2-B-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/archive-r2-B-Research/content.md)

- L3: 2. The ten-week pilot, from 6 April to 14 June, created 900 catalogue records—300 each from Cedar, Elm and Rowan. The dossier defines digitization as a searchable catalogue entry plus a scanned page; paper originals remain in controlled storage (Sections 1–2).
- L8: 7. Rights clearance is complete for 760 of the 900 scans. The other 140 are held internally: their catalogue descriptions are visible, but their page images are not publicly downloadable. Review of the remainder is forecast for next quarter, without a committed release date (Section 5).
- L11: 10. The committee’s stated communication goal is to acknowledge that 900 catalogue entries exist and observed errors received specific corrections, while keeping distinct quality dimensions and the publication boundary visible (Section 7).
- L18: - The source does not establish quality rates for all 900 records, all three full collections, or material excluded from review; the reviewed records were a non-random, accessible subset (Facts 4–5).
- L20: - It does not establish that all 900 scans are publicly available: 140 are not downloadable pending rights review, and no release date is committed (Fact 7).
- L24: - The pilot ran from 6 April to 14 June and produced 900 catalogue records (Fact 2).
[projection.md](../production-validation-experiment-6-2026-11-12/calls/archive-r2-B-Research/projection.md)

- L4: 2. The ten-week pilot, from 6 April to 14 June, created 900 catalogue records—300 each from Cedar, Elm and Rowan. The dossier defines digitization as a searchable catalogue entry plus a scanned page; paper originals remain in controlled storage (Sections 1–2).
- L9: 7. Rights clearance is complete for 760 of the 900 scans. The other 140 are held internally: their catalogue descriptions are visible, but their page images are not publicly downloadable. Review of the remainder is forecast for next quarter, without a committed release date (Section 5).
- L12: 10. The committee’s stated communication goal is to acknowledge that 900 catalogue entries exist and observed errors received specific corrections, while keeping distinct quality dimensions and the publication boundary visible (Section 7).
- L16: - The source does not establish quality rates for all 900 records, all three full collections, or material excluded from review; the reviewed records were a non-random, accessible subset (Facts 4–5).
- L18: - It does not establish that all 900 scans are publicly available: 140 are not downloadable pending rights review, and no release date is committed (Fact 7).
## archive / dimensions — kern

Bron [L11](../production-validation-experiment-6-2026-11-12/cases/archive/source.md#L11): “Cedar and Elm failed different checks;”

Volledige oorspronkelijke bronregel (context en overige componenten):

> For Cedar, all 60 reviewed records were in the correct chronological order, while 9 had a mistranscribed vessel name. For Elm, all 60 vessel names matched the originals, while 8 entries were placed under the wrong year. Rowan passed both checks in all 60 reviewed records. This is not evidence that all 300 Rowan records are correct. Cedar and Elm failed different checks; it is incorrect to say both suffered ordering problems or that correct ordering guarantees accurate transcription.

| Run | Classificatie | Research / projectie |
|---|---|---|
| archive-r1-A-Research | correct | ruw L3, 15, 16, 17, 18, 19, 20, 21, 25, 57, 59, 62; projectie L3, 15, 16, 17, 18, 19, 20, 21, 25 |
| archive-r1-B-Research | correct | ruw L2, 5, 6; projectie L3, 6, 7 |
| archive-r2-A-Research | correct | ruw L3, 18, 19, 20, 21, 22, 23, 28, 64, 69; projectie L3, 18, 19, 20, 21, 22, 23, 28 |
| archive-r2-B-Research | correct | ruw L3, 5, 7, 25, 26; projectie L4, 6, 8 |

### archive-r1-A-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/archive-r1-A-Research/content.md)

- L3: 1. City Memory’s pilot covered selected shipping registers from the Cedar, Elm and Rowan collections; it was not a catalogue of the archive’s full holdings.
- L15: 13. For Cedar, all 60 reviewed records were in the correct chronological order.
- L16: 14. For Cedar, 9 of the 60 reviewed records had a mistranscribed vessel name.
- L17: 15. For Elm, all 60 reviewed vessel names matched the originals.
- L18: 16. For Elm, 8 of the 60 reviewed entries were placed under the wrong year.
- L19: 17. Rowan passed both the vessel-name and chronological-order checks in all 60 reviewed records.
- L20: 18. The Rowan review result is not evidence that all 300 Rowan records are correct.
- L21: 19. Cedar and Elm failed different checks.
- L25: 23. Nine Cedar names and eight Elm dates were corrected on 8 July.
- L57: - 6 April–14 June 2026: The pilot ran and created 900 catalogue records across Cedar, Elm and Rowan. (Facts 2–3)
- L59: - The review recorded different results by collection and check: Cedar had 9 mistranscribed names and all 60 records in correct chronological order; Elm had 8 wrong-year entries and all 60 names matched; Rowan passed both checks in all 60 reviewed records. (Facts 13–18)
- L62: - 8 July: Nine Cedar names and eight Elm dates were corrected. (Fact 23)
[projection.md](../production-validation-experiment-6-2026-11-12/calls/archive-r1-A-Research/projection.md)

- L3: 1. City Memory’s pilot covered selected shipping registers from the Cedar, Elm and Rowan collections; it was not a catalogue of the archive’s full holdings.
- L15: 13. For Cedar, all 60 reviewed records were in the correct chronological order.
- L16: 14. For Cedar, 9 of the 60 reviewed records had a mistranscribed vessel name.
- L17: 15. For Elm, all 60 reviewed vessel names matched the originals.
- L18: 16. For Elm, 8 of the 60 reviewed entries were placed under the wrong year.
- L19: 17. Rowan passed both the vessel-name and chronological-order checks in all 60 reviewed records.
- L20: 18. The Rowan review result is not evidence that all 300 Rowan records are correct.
- L21: 19. Cedar and Elm failed different checks.
- L25: 23. Nine Cedar names and eight Elm dates were corrected on 8 July.

### archive-r1-B-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/archive-r1-B-Research/content.md)

- L2: 1. The dossier identifies City Memory as a fictional municipal archive and the report as synthetic, not a real archive report. Its ten-week pilot (6 April–14 June) created 900 catalogue records—300 each for selected shipping registers from Cedar, Elm and Rowan—not a catalogue of all holdings. (Sections 1, 7)
- L5: 4. In the reviewed records, Cedar was correctly ordered in all 60, but 9 had a mistranscribed vessel name. Elm’s 60 vessel names matched the originals, but 8 entries were under the wrong year. Rowan passed both checks in all 60 reviewed records; the source says this does not establish that all 300 Rowan records are correct. (Section 2)
- L6: 5. On 1 July, the dashboard rule changed from marking a collection green if either check passed to requiring both. On 8 July, staff corrected nine Cedar names and eight Elm dates and rechecked those 17 entries—not all 180 reviewed records or the other 720 catalogue records. The source characterizes the dashboard change as a reporting-rule correction, not an improvement to the underlying records. (Section 2)
[projection.md](../production-validation-experiment-6-2026-11-12/calls/archive-r1-B-Research/projection.md)

- L3: 1. The dossier identifies City Memory as a fictional municipal archive and the report as synthetic, not a real archive report. Its ten-week pilot (6 April–14 June) created 900 catalogue records—300 each for selected shipping registers from Cedar, Elm and Rowan—not a catalogue of all holdings. (Sections 1, 7)
- L6: 4. In the reviewed records, Cedar was correctly ordered in all 60, but 9 had a mistranscribed vessel name. Elm’s 60 vessel names matched the originals, but 8 entries were under the wrong year. Rowan passed both checks in all 60 reviewed records; the source says this does not establish that all 300 Rowan records are correct. (Section 2)
- L7: 5. On 1 July, the dashboard rule changed from marking a collection green if either check passed to requiring both. On 8 July, staff corrected nine Cedar names and eight Elm dates and rechecked those 17 entries—not all 180 reviewed records or the other 720 catalogue records. The source characterizes the dashboard change as a reporting-rule correction, not an improvement to the underlying records. (Section 2)

### archive-r2-A-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/archive-r2-A-Research/content.md)

- L3: 1. The pilot covered selected shipping registers from the Cedar, Elm and Rowan collections; it was not a catalogue of the archive’s full holdings.
- L18: 16. For Cedar, all 60 reviewed records were in the correct chronological order.
- L19: 17. For Cedar, 9 reviewed records had a mistranscribed vessel name.
- L20: 18. For Elm, all 60 reviewed vessel names matched the originals.
- L21: 19. For Elm, 8 reviewed entries were placed under the wrong year.
- L22: 20. Rowan passed both checks in all 60 reviewed records.
- L23: 21. The source states that Rowan’s reviewed results are not evidence that all 300 Rowan records are correct.
- L28: 26. Nine Cedar names and eight Elm dates were corrected on 8 July.
- L64: - 6 April–14 June 2026: the ten-week pilot created 900 catalogue records across Cedar, Elm and Rowan. (Facts 1–4)
- L69: - 8 July: nine Cedar names and eight Elm dates were corrected; those seventeen entries were rechecked. (Facts 26–29)
[projection.md](../production-validation-experiment-6-2026-11-12/calls/archive-r2-A-Research/projection.md)

- L3: 1. The pilot covered selected shipping registers from the Cedar, Elm and Rowan collections; it was not a catalogue of the archive’s full holdings.
- L18: 16. For Cedar, all 60 reviewed records were in the correct chronological order.
- L19: 17. For Cedar, 9 reviewed records had a mistranscribed vessel name.
- L20: 18. For Elm, all 60 reviewed vessel names matched the originals.
- L21: 19. For Elm, 8 reviewed entries were placed under the wrong year.
- L22: 20. Rowan passed both checks in all 60 reviewed records.
- L23: 21. The source states that Rowan’s reviewed results are not evidence that all 300 Rowan records are correct.
- L28: 26. Nine Cedar names and eight Elm dates were corrected on 8 July.

### archive-r2-B-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/archive-r2-B-Research/content.md)

- L3: 2. The ten-week pilot, from 6 April to 14 June, created 900 catalogue records—300 each from Cedar, Elm and Rowan. The dossier defines digitization as a searchable catalogue entry plus a scanned page; paper originals remain in controlled storage (Sections 1–2).
- L5: 4. In the reviewed records, all 60 Cedar entries were chronologically correct, but 9 had a mistranscribed vessel name. All 60 reviewed Elm vessel names matched the originals, but 8 entries were under the wrong year. Rowan passed both checks in all 60 reviewed records (Section 2).
- L7: 6. On 8 July, staff corrected the nine Cedar vessel names and eight Elm dates. Reviewers rechecked those 17 corrected entries, not all 180 reviewed entries or the other 720 catalogue records; the source says this supports correction of the observed entries, not complete certification (Section 2).
- L25: - Review found different issues across collections: Cedar vessel-name errors, Elm year-placement errors, and no failures in the two checks among the 60 reviewed Rowan records (Fact 4).
- L26: - Staff corrected the nine Cedar names and eight Elm dates on 8 July; only those 17 entries were rechecked (Fact 6).
[projection.md](../production-validation-experiment-6-2026-11-12/calls/archive-r2-B-Research/projection.md)

- L4: 2. The ten-week pilot, from 6 April to 14 June, created 900 catalogue records—300 each from Cedar, Elm and Rowan. The dossier defines digitization as a searchable catalogue entry plus a scanned page; paper originals remain in controlled storage (Sections 1–2).
- L6: 4. In the reviewed records, all 60 Cedar entries were chronologically correct, but 9 had a mistranscribed vessel name. All 60 reviewed Elm vessel names matched the originals, but 8 entries were under the wrong year. Rowan passed both checks in all 60 reviewed records (Section 2).
- L8: 6. On 8 July, staff corrected the nine Cedar vessel names and eight Elm dates. Reviewers rechecked those 17 corrected entries, not all 180 reviewed entries or the other 720 catalogue records; the source says this supports correction of the observed entries, not complete certification (Section 2).
## archive / sampling — kern

Bron [L9](../production-validation-experiment-6-2026-11-12/cases/archive/source.md#L9): “Boxes with fragile bindings were excluded.”

Volledige oorspronkelijke bronregel (context en overige componenten):

> Reviewers checked 60 records per collection. Staff selected the first 20 items in each of three accessible boxes rather than randomly sampling all 300 entries. Boxes with fragile bindings were excluded. The observations are therefore about accessible reviewed records. They are not estimates of full-collection error rates, and the excluded material may have different difficulties. No independent probability sample has been drawn.

| Run | Classificatie | Research / projectie |
|---|---|---|
| archive-r1-A-Research | correct | ruw L12, 13, 48; projectie L12, 13, 44 |
| archive-r1-B-Research | correct | ruw L4, 16; projectie L5, 14 |
| archive-r2-A-Research | correct | ruw L15, 16, 54, 65; projectie L15, 16, 50 |
| archive-r2-B-Research | correct | ruw L6, 18; projectie L7, 16 |

### archive-r1-A-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/archive-r1-A-Research/content.md)

- L12: 10. Staff selected the first 20 items in each of three accessible boxes rather than randomly sampling all 300 entries.
- L13: 11. Boxes with fragile bindings were excluded from the review.
- L48: - Whether all 900 catalogue records have accurate descriptions or correct chronological placement is not established. The reviewed records were a non-random selection, and the recheck covered only the 17 corrected entries.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/archive-r1-A-Research/projection.md)

- L12: 10. Staff selected the first 20 items in each of three accessible boxes rather than randomly sampling all 300 entries.
- L13: 11. Boxes with fragile bindings were excluded from the review.
- L44: - Whether all 900 catalogue records have accurate descriptions or correct chronological placement is not established. The reviewed records were a non-random selection, and the recheck covered only the 17 corrected entries.

### archive-r1-B-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/archive-r1-B-Research/content.md)

- L4: 3. Reviewers checked 60 records per collection, selected from accessible boxes rather than through a random sample of all 300; fragile-binding boxes were excluded. The source says these observations are not full-collection error-rate estimates. (Section 2)
- L16: - The source does not establish the ordering or description accuracy of all 900 records, or that the reviewed records represent the full collections; fragile-binding material was excluded and the review was not random. (Facts 3–5)
[projection.md](../production-validation-experiment-6-2026-11-12/calls/archive-r1-B-Research/projection.md)

- L5: 3. Reviewers checked 60 records per collection, selected from accessible boxes rather than through a random sample of all 300; fragile-binding boxes were excluded. The source says these observations are not full-collection error-rate estimates. (Section 2)
- L14: - The source does not establish the ordering or description accuracy of all 900 records, or that the reviewed records represent the full collections; fragile-binding material was excluded and the review was not random. (Facts 3–5)

### archive-r2-A-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/archive-r2-A-Research/content.md)

- L15: 13. Staff selected the first 20 items in each of three accessible boxes rather than randomly sampling all 300 entries.
- L16: 14. Boxes with fragile bindings were excluded.
- L54: - The error profile of the excluded fragile-binding material is not established.
- L65: - Reviewers checked 60 records per collection using the described accessible-box selection, with fragile-binding boxes excluded. (Facts 12–15)
[projection.md](../production-validation-experiment-6-2026-11-12/calls/archive-r2-A-Research/projection.md)

- L15: 13. Staff selected the first 20 items in each of three accessible boxes rather than randomly sampling all 300 entries.
- L16: 14. Boxes with fragile bindings were excluded.
- L50: - The error profile of the excluded fragile-binding material is not established.

### archive-r2-B-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/archive-r2-B-Research/content.md)

- L6: 5. Reviewers examined the first 20 items in three accessible boxes per collection, not a random sample of all 300 entries; fragile-binding boxes were excluded. The source says these observations are not full-collection error-rate estimates and that excluded material may differ in difficulty (Section 2).
- L18: - The source does not establish quality rates for all 900 records, all three full collections, or material excluded from review; the reviewed records were a non-random, accessible subset (Facts 4–5).
[projection.md](../production-validation-experiment-6-2026-11-12/calls/archive-r2-B-Research/projection.md)

- L7: 5. Reviewers examined the first 20 items in three accessible boxes per collection, not a random sample of all 300 entries; fragile-binding boxes were excluded. The source says these observations are not full-collection error-rate estimates and that excluded material may differ in difficulty (Section 2).
- L16: - The source does not establish quality rates for all 900 records, all three full collections, or material excluded from review; the reviewed records were a non-random, accessible subset (Facts 4–5).
## archive / dashboard — belangrijk

Bron [L13](../production-validation-experiment-6-2026-11-12/cases/archive/source.md#L13): “This is a reporting-rule correction, not an improvement to the underlying records.”

Volledige oorspronkelijke bronregel (context en overige componenten):

> The committee's first dashboard merged the checks into a single green flag whenever either check passed. That would have marked every collection green. On 1 July the dashboard was changed to require both checks. This is a reporting-rule correction, not an improvement to the underlying records. Nine Cedar names and eight Elm dates were corrected on 8 July. Reviewers rechecked those seventeen corrected entries, not all 180 reviewed entries and not the remaining 720 catalogue records. The limited recheck supports correction of those observed entries, not complete certification.

| Run | Classificatie | Research / projectie |
|---|---|---|
| archive-r1-A-Research | correct | ruw L22, 23, 24, 60, 61, 72; projectie L22, 23, 24 |
| archive-r1-B-Research | correct | ruw L6, 22; projectie L7 |
| archive-r2-A-Research | correct | ruw L24, 25, 26, 27, 67, 68, 71, 77; projectie L24, 25, 26, 27 |
| archive-r2-B-Research | ontbreekt | geen anker in ruw; geen anker in projectie |

### archive-r1-A-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/archive-r1-A-Research/content.md)

- L22: 20. The committee’s first dashboard marked a collection green whenever either check passed.
- L23: 21. On 1 July, the dashboard rule was changed to require both checks.
- L24: 22. The source identifies the dashboard change as a reporting-rule correction, not an improvement to the underlying records.
- L60: - The committee’s first dashboard used an either-check-passes rule. (Fact 20)
- L61: - 1 July: The dashboard rule changed to require both checks. The source identifies this as a reporting-rule correction, not a change to underlying records. (Facts 21–22)
- L72: - Facts 20–26 concern the dashboard rule, observed corrections and the limited recheck.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/archive-r1-A-Research/projection.md)

- L22: 20. The committee’s first dashboard marked a collection green whenever either check passed.
- L23: 21. On 1 July, the dashboard rule was changed to require both checks.
- L24: 22. The source identifies the dashboard change as a reporting-rule correction, not an improvement to the underlying records.

### archive-r1-B-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/archive-r1-B-Research/content.md)

- L6: 5. On 1 July, the dashboard rule changed from marking a collection green if either check passed to requiring both. On 8 July, staff corrected nine Cedar names and eight Elm dates and rechecked those 17 entries—not all 180 reviewed records or the other 720 catalogue records. The source characterizes the dashboard change as a reporting-rule correction, not an improvement to the underlying records. (Section 2)
- L22: - 1 July: the dashboard was changed to require both quality checks to pass. (Fact 5)
[projection.md](../production-validation-experiment-6-2026-11-12/calls/archive-r1-B-Research/projection.md)

- L7: 5. On 1 July, the dashboard rule changed from marking a collection green if either check passed to requiring both. On 8 July, staff corrected nine Cedar names and eight Elm dates and rechecked those 17 entries—not all 180 reviewed records or the other 720 catalogue records. The source characterizes the dashboard change as a reporting-rule correction, not an improvement to the underlying records. (Section 2)

### archive-r2-A-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/archive-r2-A-Research/content.md)

- L24: 22. The committee’s first dashboard merged the checks into a single green flag whenever either check passed.
- L25: 23. That first dashboard would have marked every collection green.
- L26: 24. On 1 July, the dashboard was changed to require both checks.
- L27: 25. The source describes that dashboard change as a reporting-rule correction, not an improvement to the underlying records.
- L67: - The first dashboard used a rule under which either passed check yielded a green flag. (Facts 22–23)
- L68: - 1 July: the dashboard was changed to require both checks; this was a reporting-rule correction, not an improvement to the underlying records. (Facts 24–25)
- L71: - Relationship between the observed review results, the dashboard-rule change and the corrections: UNKNOWN; the source does not state that one caused another.
- L77: - Facts 12–29 concern the sample boundaries, separate review results, dashboard rule, corrections and limits of the recheck.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/archive-r2-A-Research/projection.md)

- L24: 22. The committee’s first dashboard merged the checks into a single green flag whenever either check passed.
- L25: 23. That first dashboard would have marked every collection green.
- L26: 24. On 1 July, the dashboard was changed to require both checks.
- L27: 25. The source describes that dashboard change as a reporting-rule correction, not an improvement to the underlying records.

### archive-r2-B-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/archive-r2-B-Research/content.md)

Geen passend bewijsanker; volledige tekst is beslissend.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/archive-r2-B-Research/projection.md)

Geen passend bewijsanker; volledige tekst is beslissend.
## archive / recheck — kern

Bron [L13](../production-validation-experiment-6-2026-11-12/cases/archive/source.md#L13): “Reviewers rechecked those seventeen corrected entries, not all 180 reviewed entries and not the remaining 720 catalogue records.”

Volledige oorspronkelijke bronregel (context en overige componenten):

> The committee's first dashboard merged the checks into a single green flag whenever either check passed. That would have marked every collection green. On 1 July the dashboard was changed to require both checks. This is a reporting-rule correction, not an improvement to the underlying records. Nine Cedar names and eight Elm dates were corrected on 8 July. Reviewers rechecked those seventeen corrected entries, not all 180 reviewed entries and not the remaining 720 catalogue records. The limited recheck supports correction of those observed entries, not complete certification.

| Run | Classificatie | Research / projectie |
|---|---|---|
| archive-r1-A-Research | correct | ruw L19, 26, 27, 28, 48, 50, 63, 72; projectie L19, 26, 27, 28, 44, 46 |
| archive-r1-B-Research | correct | ruw L6, 23; projectie L7 |
| archive-r2-A-Research | correct | ruw L19, 29, 30, 31, 55, 69, 77; projectie L19, 29, 30, 31, 51 |
| archive-r2-B-Research | correct | ruw L7, 19, 26; projectie L8, 17 |

### archive-r1-A-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/archive-r1-A-Research/content.md)

- L19: 17. Rowan passed both the vessel-name and chronological-order checks in all 60 reviewed records.
- L26: 24. Reviewers rechecked those 17 corrected entries.
- L27: 25. Reviewers did not recheck all 180 reviewed entries or the remaining 720 catalogue records.
- L28: 26. The limited recheck supports correction of those observed entries, not complete certification.
- L48: - Whether all 900 catalogue records have accurate descriptions or correct chronological placement is not established. The reviewed records were a non-random selection, and the recheck covered only the 17 corrected entries.
- L50: - Whether the archive is entirely error-free is not established; the observed corrections and limited recheck do not certify the full catalogue.
- L63: - Reviewers rechecked the 17 corrected entries, but not all reviewed or remaining catalogue records. (Facts 24–26)
- L72: - Facts 20–26 concern the dashboard rule, observed corrections and the limited recheck.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/archive-r1-A-Research/projection.md)

- L19: 17. Rowan passed both the vessel-name and chronological-order checks in all 60 reviewed records.
- L26: 24. Reviewers rechecked those 17 corrected entries.
- L27: 25. Reviewers did not recheck all 180 reviewed entries or the remaining 720 catalogue records.
- L28: 26. The limited recheck supports correction of those observed entries, not complete certification.
- L44: - Whether all 900 catalogue records have accurate descriptions or correct chronological placement is not established. The reviewed records were a non-random selection, and the recheck covered only the 17 corrected entries.
- L46: - Whether the archive is entirely error-free is not established; the observed corrections and limited recheck do not certify the full catalogue.

### archive-r1-B-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/archive-r1-B-Research/content.md)

- L6: 5. On 1 July, the dashboard rule changed from marking a collection green if either check passed to requiring both. On 8 July, staff corrected nine Cedar names and eight Elm dates and rechecked those 17 entries—not all 180 reviewed records or the other 720 catalogue records. The source characterizes the dashboard change as a reporting-rule correction, not an improvement to the underlying records. (Section 2)
- L23: - 8 July: staff corrected and rechecked 17 identified entries; the recheck was limited to those entries. (Fact 5)
[projection.md](../production-validation-experiment-6-2026-11-12/calls/archive-r1-B-Research/projection.md)

- L7: 5. On 1 July, the dashboard rule changed from marking a collection green if either check passed to requiring both. On 8 July, staff corrected nine Cedar names and eight Elm dates and rechecked those 17 entries—not all 180 reviewed records or the other 720 catalogue records. The source characterizes the dashboard change as a reporting-rule correction, not an improvement to the underlying records. (Section 2)

### archive-r2-A-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/archive-r2-A-Research/content.md)

- L19: 17. For Cedar, 9 reviewed records had a mistranscribed vessel name.
- L29: 27. Reviewers rechecked those seventeen corrected entries.
- L30: 28. Reviewers did not recheck all 180 reviewed entries or the remaining 720 catalogue records.
- L31: 29. The limited recheck supports correction of those observed entries, not complete certification.
- L55: - Whether all catalogue records are currently accurate or correctly ordered is not established; the recheck covered only the seventeen corrected entries.
- L69: - 8 July: nine Cedar names and eight Elm dates were corrected; those seventeen entries were rechecked. (Facts 26–29)
- L77: - Facts 12–29 concern the sample boundaries, separate review results, dashboard rule, corrections and limits of the recheck.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/archive-r2-A-Research/projection.md)

- L19: 17. For Cedar, 9 reviewed records had a mistranscribed vessel name.
- L29: 27. Reviewers rechecked those seventeen corrected entries.
- L30: 28. Reviewers did not recheck all 180 reviewed entries or the remaining 720 catalogue records.
- L31: 29. The limited recheck supports correction of those observed entries, not complete certification.
- L51: - Whether all catalogue records are currently accurate or correctly ordered is not established; the recheck covered only the seventeen corrected entries.

### archive-r2-B-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/archive-r2-B-Research/content.md)

- L7: 6. On 8 July, staff corrected the nine Cedar vessel names and eight Elm dates. Reviewers rechecked those 17 corrected entries, not all 180 reviewed entries or the other 720 catalogue records; the source says this supports correction of the observed entries, not complete certification (Section 2).
- L19: - It does not establish complete accuracy even for the reviewed records after correction: only the 17 corrected entries were rechecked, not all reviewed or remaining records (Fact 6).
- L26: - Staff corrected the nine Cedar names and eight Elm dates on 8 July; only those 17 entries were rechecked (Fact 6).
[projection.md](../production-validation-experiment-6-2026-11-12/calls/archive-r2-B-Research/projection.md)

- L8: 6. On 8 July, staff corrected the nine Cedar vessel names and eight Elm dates. Reviewers rechecked those 17 corrected entries, not all 180 reviewed entries or the other 720 catalogue records; the source says this supports correction of the observed entries, not complete certification (Section 2).
- L17: - It does not establish complete accuracy even for the reviewed records after correction: only the 17 corrected entries were rechecked, not all reviewed or remaining records (Fact 6).
## archive / rights — kern

Bron [L32](../production-validation-experiment-6-2026-11-12/cases/archive/source.md#L32): “Rights clearance is complete for 760 of the 900 scans.”

Volledige oorspronkelijke bronregel (context en overige componenten):

> Rights clearance is complete for 760 of the 900 scans. The remaining 140 are held internally pending review. Catalogue descriptions for these 140 are visible, but their page images are not publicly downloadable. “900 entries created” must not become “900 scans openly available”. The rights team expects to review the remainder next quarter; that is a forecast without a committed release date. No automated rights clearance was tested. The archive's intended access mission does not override the actual publication boundary.

| Run | Classificatie | Research / projectie |
|---|---|---|
| archive-r1-A-Research | correct | ruw L29, 30, 31, 49, 64, 73; projectie L29, 30, 31, 45 |
| archive-r1-B-Research | correct | ruw L7, 17; projectie L8, 15 |
| archive-r2-A-Research | correct | ruw L32, 33, 34, 35, 56, 57, 78; projectie L32, 33, 34, 35, 52, 53 |
| archive-r2-B-Research | correct | ruw L8, 20; projectie L9, 18 |

### archive-r1-A-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/archive-r1-A-Research/content.md)

- L29: 27. Rights clearance is complete for 760 of the 900 scans.
- L30: 28. The remaining 140 scans are held internally pending review.
- L31: 29. Catalogue descriptions for those 140 records are visible, but their page images are not publicly downloadable.
- L49: - Whether all 900 scans are publicly downloadable is not established as true; the source states that images for 140 records are not publicly downloadable.
- L64: - At an unspecified later point, rights clearance was complete for 760 scans; 140 remained held internally and their images were not publicly downloadable. (Facts 27–29)
- L73: - Facts 27–30 concern scan rights, the 140 images not publicly downloadable and the uncommitted review forecast.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/archive-r1-A-Research/projection.md)

- L29: 27. Rights clearance is complete for 760 of the 900 scans.
- L30: 28. The remaining 140 scans are held internally pending review.
- L31: 29. Catalogue descriptions for those 140 records are visible, but their page images are not publicly downloadable.
- L45: - Whether all 900 scans are publicly downloadable is not established as true; the source states that images for 140 records are not publicly downloadable.

### archive-r1-B-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/archive-r1-B-Research/content.md)

- L7: 6. Rights clearance is complete for 760 of the 900 scans. The remaining 140 descriptions are visible, but their page images are held internally and are not publicly downloadable. Review next quarter is an expectation, not a committed release date. (Section 5)
- L17: - It does not establish that all 900 page images are publicly available: 140 are withheld pending rights review, with no committed release date. (Fact 6)
[projection.md](../production-validation-experiment-6-2026-11-12/calls/archive-r1-B-Research/projection.md)

- L8: 6. Rights clearance is complete for 760 of the 900 scans. The remaining 140 descriptions are visible, but their page images are held internally and are not publicly downloadable. Review next quarter is an expectation, not a committed release date. (Section 5)
- L15: - It does not establish that all 900 page images are publicly available: 140 are withheld pending rights review, with no committed release date. (Fact 6)

### archive-r2-A-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/archive-r2-A-Research/content.md)

- L32: 30. Rights clearance is complete for 760 of the 900 scans.
- L33: 31. The remaining 140 scans are held internally pending review.
- L34: 32. Catalogue descriptions for the 140 scans are visible.
- L35: 33. The page images for the 140 scans are not publicly downloadable.
- L56: - The source does not establish that all 760 scans with completed rights clearance are publicly downloadable.
- L57: - The release date for the remaining 140 images is not established.
- L78: - Facts 30–35 concern rights clearance and the publication status of the remaining 140 images.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/archive-r2-A-Research/projection.md)

- L32: 30. Rights clearance is complete for 760 of the 900 scans.
- L33: 31. The remaining 140 scans are held internally pending review.
- L34: 32. Catalogue descriptions for the 140 scans are visible.
- L35: 33. The page images for the 140 scans are not publicly downloadable.
- L52: - The source does not establish that all 760 scans with completed rights clearance are publicly downloadable.
- L53: - The release date for the remaining 140 images is not established.

### archive-r2-B-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/archive-r2-B-Research/content.md)

- L8: 7. Rights clearance is complete for 760 of the 900 scans. The other 140 are held internally: their catalogue descriptions are visible, but their page images are not publicly downloadable. Review of the remainder is forecast for next quarter, without a committed release date (Section 5).
- L20: - It does not establish that all 900 scans are publicly available: 140 are not downloadable pending rights review, and no release date is committed (Fact 7).
[projection.md](../production-validation-experiment-6-2026-11-12/calls/archive-r2-B-Research/projection.md)

- L9: 7. Rights clearance is complete for 760 of the 900 scans. The other 140 are held internally: their catalogue descriptions are visible, but their page images are not publicly downloadable. Review of the remainder is forecast for next quarter, without a committed release date (Section 5).
- L18: - It does not establish that all 900 scans are publicly available: 140 are not downloadable pending rights review, and no release date is committed (Fact 7).
## archive / forecast — kern

Bron [L32](../production-validation-experiment-6-2026-11-12/cases/archive/source.md#L32): “that is a forecast without a committed release date.”

Volledige oorspronkelijke bronregel (context en overige componenten):

> Rights clearance is complete for 760 of the 900 scans. The remaining 140 are held internally pending review. Catalogue descriptions for these 140 are visible, but their page images are not publicly downloadable. “900 entries created” must not become “900 scans openly available”. The rights team expects to review the remainder next quarter; that is a forecast without a committed release date. No automated rights clearance was tested. The archive's intended access mission does not override the actual publication boundary.

| Run | Classificatie | Research / projectie |
|---|---|---|
| archive-r1-A-Research | correct | ruw L32, 73; projectie L32 |
| archive-r1-B-Research | correct | ruw L7, 17; projectie L8, 15 |
| archive-r2-A-Research | correct | ruw L37, 57; projectie L37, 53 |
| archive-r2-B-Research | correct | ruw L8, 20; projectie L9, 18 |

### archive-r1-A-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/archive-r1-A-Research/content.md)

- L32: 30. The rights team expects to review the remaining scans next quarter; the source describes this as a forecast without a committed release date.
- L73: - Facts 27–30 concern scan rights, the 140 images not publicly downloadable and the uncommitted review forecast.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/archive-r1-A-Research/projection.md)

- L32: 30. The rights team expects to review the remaining scans next quarter; the source describes this as a forecast without a committed release date.

### archive-r1-B-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/archive-r1-B-Research/content.md)

- L7: 6. Rights clearance is complete for 760 of the 900 scans. The remaining 140 descriptions are visible, but their page images are held internally and are not publicly downloadable. Review next quarter is an expectation, not a committed release date. (Section 5)
- L17: - It does not establish that all 900 page images are publicly available: 140 are withheld pending rights review, with no committed release date. (Fact 6)
[projection.md](../production-validation-experiment-6-2026-11-12/calls/archive-r1-B-Research/projection.md)

- L8: 6. Rights clearance is complete for 760 of the 900 scans. The remaining 140 descriptions are visible, but their page images are held internally and are not publicly downloadable. Review next quarter is an expectation, not a committed release date. (Section 5)
- L15: - It does not establish that all 900 page images are publicly available: 140 are withheld pending rights review, with no committed release date. (Fact 6)

### archive-r2-A-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/archive-r2-A-Research/content.md)

- L37: 35. The source describes that review timing as a forecast without a committed release date.
- L57: - The release date for the remaining 140 images is not established.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/archive-r2-A-Research/projection.md)

- L37: 35. The source describes that review timing as a forecast without a committed release date.
- L53: - The release date for the remaining 140 images is not established.

### archive-r2-B-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/archive-r2-B-Research/content.md)

- L8: 7. Rights clearance is complete for 760 of the 900 scans. The other 140 are held internally: their catalogue descriptions are visible, but their page images are not publicly downloadable. Review of the remainder is forecast for next quarter, without a committed release date (Section 5).
- L20: - It does not establish that all 900 scans are publicly available: 140 are not downloadable pending rights review, and no release date is committed (Fact 7).
[projection.md](../production-validation-experiment-6-2026-11-12/calls/archive-r2-B-Research/projection.md)

- L9: 7. Rights clearance is complete for 760 of the 900 scans. The other 140 are held internally: their catalogue descriptions are visible, but their page images are not publicly downloadable. Review of the remainder is forecast for next quarter, without a committed release date (Section 5).
- L18: - It does not establish that all 900 scans are publicly available: 140 are not downloadable pending rights review, and no release date is committed (Fact 7).
## archive / keyboard — belangrijk

Bron [L35](../production-validation-experiment-6-2026-11-12/cases/archive/source.md#L35): “A keyboard-only review still found that the language switch could not be reached on two record-page templates.”

Volledige oorspronkelijke bronregel (context en overige componenten):

> The site passed its automated contrast scan after a stylesheet fix. A keyboard-only review still found that the language switch could not be reached on two record-page templates. The contrast result and keyboard defect measure different properties. The web team plans a navigation repair but has not deployed it. A claim that the catalogue is fully accessible is unsupported. The committee did not commission a full legal-compliance audit, and none is inferred from the automated check.

| Run | Classificatie | Research / projectie |
|---|---|---|
| archive-r1-A-Research | ontbreekt | geen anker in ruw; geen anker in projectie |
| archive-r1-B-Research | ontbreekt | geen anker in ruw; geen anker in projectie |
| archive-r2-A-Research | ontbreekt | geen anker in ruw; geen anker in projectie |
| archive-r2-B-Research | ontbreekt | geen anker in ruw; geen anker in projectie |

### archive-r1-A-Research

Geen contrast/keyboarddefect in volledige Research of projectie. Belangrijk voor brede toegankelijkheid; sensitiviteit als harde eis apart rapporteren.

[content.md](../production-validation-experiment-6-2026-11-12/calls/archive-r1-A-Research/content.md)

Geen passend bewijsanker; volledige tekst is beslissend.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/archive-r1-A-Research/projection.md)

Geen passend bewijsanker; volledige tekst is beslissend.

### archive-r1-B-Research

Geen contrast/keyboarddefect in volledige Research of projectie. Belangrijk voor brede toegankelijkheid; sensitiviteit als harde eis apart rapporteren.

[content.md](../production-validation-experiment-6-2026-11-12/calls/archive-r1-B-Research/content.md)

Geen passend bewijsanker; volledige tekst is beslissend.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/archive-r1-B-Research/projection.md)

Geen passend bewijsanker; volledige tekst is beslissend.

### archive-r2-A-Research

Geen contrast/keyboarddefect in volledige Research of projectie. Belangrijk voor brede toegankelijkheid; sensitiviteit als harde eis apart rapporteren.

[content.md](../production-validation-experiment-6-2026-11-12/calls/archive-r2-A-Research/content.md)

Geen passend bewijsanker; volledige tekst is beslissend.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/archive-r2-A-Research/projection.md)

Geen passend bewijsanker; volledige tekst is beslissend.

### archive-r2-B-Research

Geen contrast/keyboarddefect in volledige Research of projectie. Belangrijk voor brede toegankelijkheid; sensitiviteit als harde eis apart rapporteren.

[content.md](../production-validation-experiment-6-2026-11-12/calls/archive-r2-B-Research/content.md)

Geen passend bewijsanker; volledige tekst is beslissend.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/archive-r2-B-Research/projection.md)

Geen passend bewijsanker; volledige tekst is beslissend.
## archive / decision — kern

Bron [L40](../production-validation-experiment-6-2026-11-12/cases/archive/source.md#L40): “The committee approved designing a deeper random audit stratified by handwriting, print quality and collection.”

Volledige oorspronkelijke bronregel (context en overige componenten):

> The committee approved designing a deeper random audit stratified by handwriting, print quality and collection. It has not approved a full-archive rollout or a claim that assisted cataloguing outperforms manual work. The audit proposal would assess names, chronology, translation roles, publication rights and access separately. Its design has not been tested and no new pass rate exists. Staff will first define a common logging rule and include training and correction time in a future cost account.

| Run | Classificatie | Research / projectie |
|---|---|---|
| archive-r1-A-Research | correct | ruw L33, 36, 37, 65, 66, 74; projectie L33, 36, 37 |
| archive-r1-B-Research | correct | ruw L9, 13, 18, 24; projectie L10, 16 |
| archive-r2-A-Research | correct | ruw L38, 41, 42, 59, 70, 79; projectie L38, 41, 42, 55 |
| archive-r2-B-Research | correct | ruw L10, 15, 21, 27; projectie L11, 19 |

### archive-r1-A-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/archive-r1-A-Research/content.md)

- L33: 31. The committee approved designing a deeper random audit stratified by handwriting, print quality and collection.
- L36: 34. The proposed audit would assess names, chronology, translation roles, publication rights and access separately.
- L37: 35. The proposed audit design has not been tested and no new pass rate exists.
- L65: - The committee approved designing a deeper random audit and had not approved a full-archive rollout or an outperformance claim. (Facts 31–35)
- L66: - Relationship between the audit approval and the specific review results or corrections: UNKNOWN; the source does not state a causal relationship.
- L74: - Facts 31–35 concern the proposed audit and the limits of the committee’s rollout decision.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/archive-r1-A-Research/projection.md)

- L33: 31. The committee approved designing a deeper random audit stratified by handwriting, print quality and collection.
- L36: 34. The proposed audit would assess names, chronology, translation roles, publication rights and access separately.
- L37: 35. The proposed audit design has not been tested and no new pass rate exists.

### archive-r1-B-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/archive-r1-B-Research/content.md)

- L9: 8. The committee approved designing a deeper random audit stratified by handwriting, print quality and collection. It has not approved full-archive rollout or a claim that assisted cataloguing outperforms manual work; the proposed audit has not been tested and has produced no new pass rate. (Section 7)
- L13: - The 900-entry output and specific corrections are documented, but the source supports a limited next step—designing an audit—not a full rollout or comprehensive quality certification. (Facts 1, 5, 8)
- L18: - The proposed deeper audit has not yet been conducted, so no broader pass rate or full-archive quality conclusion is available. (Fact 8)
- L24: - By the 7 October dossier: the committee had approved designing a deeper random audit, but not a full-archive rollout; the audit remained a proposal. (Fact 8)
[projection.md](../production-validation-experiment-6-2026-11-12/calls/archive-r1-B-Research/projection.md)

- L10: 8. The committee approved designing a deeper random audit stratified by handwriting, print quality and collection. It has not approved full-archive rollout or a claim that assisted cataloguing outperforms manual work; the proposed audit has not been tested and has produced no new pass rate. (Section 7)
- L16: - The proposed deeper audit has not yet been conducted, so no broader pass rate or full-archive quality conclusion is available. (Fact 8)

### archive-r2-A-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/archive-r2-A-Research/content.md)

- L38: 36. The committee approved designing a deeper random audit stratified by handwriting, print quality and collection.
- L41: 39. The proposed audit would assess names, chronology, translation roles, publication rights and access separately.
- L42: 40. The proposed audit design has not been tested, and no new pass rate exists.
- L59: - The committee’s approved audit design has not been tested, and its results are unknown.
- L70: - As recorded in the 7 October 2026 dossier: the committee had approved designing a deeper random audit, but had not approved a full-archive rollout or a claim that assisted cataloguing outperforms manual work. (Facts 36–40)
- L79: - Facts 36–40 concern the audit proposal and the limits of the next-step decision.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/archive-r2-A-Research/projection.md)

- L38: 36. The committee approved designing a deeper random audit stratified by handwriting, print quality and collection.
- L41: 39. The proposed audit would assess names, chronology, translation roles, publication rights and access separately.
- L42: 40. The proposed audit design has not been tested, and no new pass rate exists.
- L55: - The committee’s approved audit design has not been tested, and its results are unknown.

### archive-r2-B-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/archive-r2-B-Research/content.md)

- L10: 9. The committee approved designing a deeper random audit stratified by handwriting, print quality and collection. It has not approved a full-archive rollout or a claim that assisted cataloguing outperforms manual work; the audit has not been tested and has no new pass rate (Section 7).
- L15: - The committee’s next step is further audit design, not approval of full rollout (Fact 9).
- L21: - It does not establish that the planned audit will be conducted, what it will find, or that a full-archive rollout will follow (Fact 9).
- L27: - The committee approved designing a stratified random audit, but not a full-archive rollout (Fact 9).
[projection.md](../production-validation-experiment-6-2026-11-12/calls/archive-r2-B-Research/projection.md)

- L11: 9. The committee approved designing a deeper random audit stratified by handwriting, print quality and collection. It has not approved a full-archive rollout or a claim that assisted cataloguing outperforms manual work; the audit has not been tested and has no new pass rate (Section 7).
- L19: - It does not establish that the planned audit will be conducted, what it will find, or that a full-archive rollout will follow (Fact 9).
## archive / productivity — context

Bron [L18](../production-validation-experiment-6-2026-11-12/cases/archive/source.md#L18): “One cannot infer that the tool caused a 37.5% labor saving”

Volledige oorspronkelijke bronregel (context en overige componenten):

> One can report the recorded preparation-time difference with those boundaries. One cannot infer that the tool caused a 37.5% labor saving or that the same rate will apply to handwritten material. The tool vendor's slide saying “twice as fast” was not accepted by the committee as a result. The archive has not measured downstream researcher time, total service cost or the amount of staff judgment displaced.

| Run | Classificatie | Research / projectie |
|---|---|---|
| archive-r1-A-Research | ontbreekt | geen anker in ruw; geen anker in projectie |
| archive-r1-B-Research | ontbreekt | geen anker in ruw; geen anker in projectie |
| archive-r2-A-Research | ontbreekt | geen anker in ruw; geen anker in projectie |
| archive-r2-B-Research | ontbreekt | geen anker in ruw; geen anker in projectie |

### archive-r1-A-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/archive-r1-A-Research/content.md)

Geen passend bewijsanker; volledige tekst is beslissend.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/archive-r1-A-Research/projection.md)

Geen passend bewijsanker; volledige tekst is beslissend.

### archive-r1-B-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/archive-r1-B-Research/content.md)

Geen passend bewijsanker; volledige tekst is beslissend.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/archive-r1-B-Research/projection.md)

Geen passend bewijsanker; volledige tekst is beslissend.

### archive-r2-A-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/archive-r2-A-Research/content.md)

Geen passend bewijsanker; volledige tekst is beslissend.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/archive-r2-A-Research/projection.md)

Geen passend bewijsanker; volledige tekst is beslissend.

### archive-r2-B-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/archive-r2-B-Research/content.md)

Geen passend bewijsanker; volledige tekst is beslissend.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/archive-r2-B-Research/projection.md)

Geen passend bewijsanker; volledige tekst is beslissend.
## archive / logs — context

Bron [L23](../production-validation-experiment-6-2026-11-12/cases/archive/source.md#L23): “The visible increase is not a comparable measure of adoption”

Volledige oorspronkelijke bronregel (context en overige componenten):

> The catalogue service recorded 1,800 searches in the first month and 2,700 in the second. In month one, repeated searches by the same session were collapsed within five minutes. In month two, the log retained every search request. A publicity campaign and a local-history festival also occurred in month two. There is no recomputation of either month under the other's rule. The visible increase is not a comparable measure of adoption or evidence that bilingual labels caused more use.

| Run | Classificatie | Research / projectie |
|---|---|---|
| archive-r1-A-Research | ontbreekt | geen anker in ruw; geen anker in projectie |
| archive-r1-B-Research | ontbreekt | geen anker in ruw; geen anker in projectie |
| archive-r2-A-Research | ontbreekt | geen anker in ruw; geen anker in projectie |
| archive-r2-B-Research | ontbreekt | geen anker in ruw; geen anker in projectie |

### archive-r1-A-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/archive-r1-A-Research/content.md)

Geen passend bewijsanker; volledige tekst is beslissend.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/archive-r1-A-Research/projection.md)

Geen passend bewijsanker; volledige tekst is beslissend.

### archive-r1-B-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/archive-r1-B-Research/content.md)

Geen passend bewijsanker; volledige tekst is beslissend.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/archive-r1-B-Research/projection.md)

Geen passend bewijsanker; volledige tekst is beslissend.

### archive-r2-A-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/archive-r2-A-Research/content.md)

Geen passend bewijsanker; volledige tekst is beslissend.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/archive-r2-A-Research/projection.md)

Geen passend bewijsanker; volledige tekst is beslissend.

### archive-r2-B-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/archive-r2-B-Research/content.md)

Geen passend bewijsanker; volledige tekst is beslissend.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/archive-r2-B-Research/projection.md)

Geen passend bewijsanker; volledige tekst is beslissend.
## archive / role — context

Bron [L28](../production-validation-experiment-6-2026-11-12/cases/archive/source.md#L28): “That is a factual role error, not just a stylistic preference”

Volledige oorspronkelijke bronregel (context en overige componenten):

> The bilingual fields are in Dutch and English. A volunteer reviewed 30 English summaries, identifying six unclear terms. Staff revised four, while two remain under terminology review. In one Cedar entry, an English summary used “captain” where the original identified an owner. That is a factual role error, not just a stylistic preference, and it was corrected with the nine vessel names on 8 July. It is distinct from the nine-name count and should not silently change that count to ten name errors.

| Run | Classificatie | Research / projectie |
|---|---|---|
| archive-r1-A-Research | ontbreekt | geen anker in ruw; geen anker in projectie |
| archive-r1-B-Research | ontbreekt | geen anker in ruw; geen anker in projectie |
| archive-r2-A-Research | ontbreekt | geen anker in ruw; geen anker in projectie |
| archive-r2-B-Research | ontbreekt | geen anker in ruw; geen anker in projectie |

### archive-r1-A-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/archive-r1-A-Research/content.md)

Geen passend bewijsanker; volledige tekst is beslissend.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/archive-r1-A-Research/projection.md)

Geen passend bewijsanker; volledige tekst is beslissend.

### archive-r1-B-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/archive-r1-B-Research/content.md)

Geen passend bewijsanker; volledige tekst is beslissend.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/archive-r1-B-Research/projection.md)

Geen passend bewijsanker; volledige tekst is beslissend.

### archive-r2-A-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/archive-r2-A-Research/content.md)

Geen passend bewijsanker; volledige tekst is beslissend.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/archive-r2-A-Research/projection.md)

Geen passend bewijsanker; volledige tekst is beslissend.

### archive-r2-B-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/archive-r2-B-Research/content.md)

Geen passend bewijsanker; volledige tekst is beslissend.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/archive-r2-B-Research/projection.md)

Geen passend bewijsanker; volledige tekst is beslissend.
## archive / meaning — context

Bron [L4](../production-validation-experiment-6-2026-11-12/cases/archive/source.md#L4): “an expression of our commitment to making local history approachable”

Volledige oorspronkelijke bronregel (context en overige componenten):

> City Memory is a fictional municipal archive. This is a synthetic but operationally realistic dossier, not a real archive report. The archive describes its bilingual catalogue as “an expression of our commitment to making local history approachable”. That is the institution's stated rationale. There is no study of community belonging, public trust or historical understanding. The pilot covers selected shipping registers from three collections, Cedar, Elm and Rowan. It is not a catalogue of the archive's full holdings.

| Run | Classificatie | Research / projectie |
|---|---|---|
| archive-r1-A-Research | ontbreekt | geen anker in ruw; geen anker in projectie |
| archive-r1-B-Research | ontbreekt | geen anker in ruw; geen anker in projectie |
| archive-r2-A-Research | deels | ruw L45, 60; projectie L45, 56 |
| archive-r2-B-Research | ontbreekt | geen anker in ruw; geen anker in projectie |

### archive-r1-A-Research

Effecten op gebruikers zijn niet hetzelfde als het expliciet toegeschreven institutionele motief; geen kernvereiste.

[content.md](../production-validation-experiment-6-2026-11-12/calls/archive-r1-A-Research/content.md)

Geen passend bewijsanker; volledige tekst is beslissend.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/archive-r1-A-Research/projection.md)

Geen passend bewijsanker; volledige tekst is beslissend.

### archive-r1-B-Research

Effecten op gebruikers zijn niet hetzelfde als het expliciet toegeschreven institutionele motief; geen kernvereiste.

[content.md](../production-validation-experiment-6-2026-11-12/calls/archive-r1-B-Research/content.md)

Geen passend bewijsanker; volledige tekst is beslissend.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/archive-r1-B-Research/projection.md)

Geen passend bewijsanker; volledige tekst is beslissend.

### archive-r2-A-Research

Effecten op gebruikers zijn niet hetzelfde als het expliciet toegeschreven institutionele motief; geen kernvereiste.

[content.md](../production-validation-experiment-6-2026-11-12/calls/archive-r2-A-Research/content.md)

- L45: 43. The source states that the institution’s access commitment is explicit, while its effects on users are unmeasured.
- L60: - The effects of the access commitment on users are unmeasured.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/archive-r2-A-Research/projection.md)

- L45: 43. The source states that the institution’s access commitment is explicit, while its effects on users are unmeasured.
- L56: - The effects of the access commitment on users are unmeasured.

### archive-r2-B-Research

Effecten op gebruikers zijn niet hetzelfde als het expliciet toegeschreven institutionele motief; geen kernvereiste.

[content.md](../production-validation-experiment-6-2026-11-12/calls/archive-r2-B-Research/content.md)

Geen passend bewijsanker; volledige tekst is beslissend.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/archive-r2-B-Research/projection.md)

Geen passend bewijsanker; volledige tekst is beslissend.
## archive / praise-error — context

Bron [L30](../production-validation-experiment-6-2026-11-12/cases/archive/source.md#L30): “One message praised the interface while identifying a wrong vessel name.”

Volledige oorspronkelijke bronregel (context en overige componenten):

> Visitors submitted 26 unsolicited feedback messages: 17 praised findability, five reported unclear labels and four asked about missing images. The archive does not know how many users saw the invitation or whether messages came from unique people. One message praised the interface while identifying a wrong vessel name. Praise and a factual error can coexist. Positive comments do not establish comprehension, representative satisfaction or improved belonging. No comprehension survey or accessibility user study was conducted.

| Run | Classificatie | Research / projectie |
|---|---|---|
| archive-r1-A-Research | ontbreekt | geen anker in ruw; geen anker in projectie |
| archive-r1-B-Research | ontbreekt | geen anker in ruw; geen anker in projectie |
| archive-r2-A-Research | ontbreekt | geen anker in ruw; geen anker in projectie |
| archive-r2-B-Research | ontbreekt | geen anker in ruw; geen anker in projectie |

### archive-r1-A-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/archive-r1-A-Research/content.md)

Geen passend bewijsanker; volledige tekst is beslissend.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/archive-r1-A-Research/projection.md)

Geen passend bewijsanker; volledige tekst is beslissend.

### archive-r1-B-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/archive-r1-B-Research/content.md)

Geen passend bewijsanker; volledige tekst is beslissend.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/archive-r1-B-Research/projection.md)

Geen passend bewijsanker; volledige tekst is beslissend.

### archive-r2-A-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/archive-r2-A-Research/content.md)

Geen passend bewijsanker; volledige tekst is beslissend.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/archive-r2-A-Research/projection.md)

Geen passend bewijsanker; volledige tekst is beslissend.

### archive-r2-B-Research

Ankers inhoudelijk lezen; kerncomponenten en grenzen samen beoordelen.

[content.md](../production-validation-experiment-6-2026-11-12/calls/archive-r2-B-Research/content.md)

Geen passend bewijsanker; volledige tekst is beslissend.
[projection.md](../production-validation-experiment-6-2026-11-12/calls/archive-r2-B-Research/projection.md)

Geen passend bewijsanker; volledige tekst is beslissend.
