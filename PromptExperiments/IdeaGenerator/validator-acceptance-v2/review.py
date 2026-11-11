import pathlib,json
R=pathlib.Path(__file__).resolve().parent
blockers=json.loads((R/'verification/blockers.json').read_text())
# Explicit human-reviewed exceptions; all other rows retain the per-case source-bound diagnosis.
exceptions={
 ('tuning','baseline','010-b',1):(True,False,True,'Catches the real next-trial priority error but inflates it to a universal Narrative-first claim; same fidelity standard as v1.'),
 ('tuning','baseline','010-b',2):(False,False,True,'Demands complete trial controls in an editorial idea; no actual claim in description violates those controls. Extra unsupported requirement.'),
 ('tuning','candidate1','museum-b',2):(False,False,True,'Inflates ordinary pilot/evaluation wording to a formal program/pending decision and demands explicit literal characterisation. The real audience mismatch is already blocker 1.'),
 ('tuning','candidate2','005-b',1):(True,True,False,'Correct causality diagnosis, but Describe the relationship as an observed change or association is a replacement presentation instruction.'),
 ('tuning','candidate2','museum-b',2):(False,False,True,'Duplicates the real audience mismatch and implies outcome data are needed for the managerial decision; no source-defined decision criteria.'),
 ('tuning','candidate2','seedbank-b',2):(False,False,True,'Duplicates the motive problem and inflates a promising-motive assertion into replacement of the whole donor communication task; other fields still specify limited continuation.'),
 ('tuning','candidate2','v4-seedbank-original',1):(False,False,True,'Misses known results-motivated decision in description; treats reader expectations about funds/activities as a required financial-report task. Materiality of regular supporters alone is not established. Strict gate fails even though rejection verdict is TP.'),
 ('holdout','baseline','archive-a',1):(False,False,True,'News-style present repeats does not assert completed execution; description refers to limited repeat check, with no claim that it has already happened. Frozen good label retained.'),
 ('holdout','baseline','archive-b',1):(True,False,True,'Correct unsupported motive, but absence of a recorded reason is not contradiction and report no effect is strengthened into an established absence of benefit.'),
 ('holdout','baseline','review-a',1):(False,False,True,'Explain observations need not mean explain internal causes; the idea preserves counts, preceding input and conditional choice. Invented causal commitment.'),
 ('holdout','baseline','review-a',2):(False,False,True,'Use the selected failure does not say by itself or omit inspection as a step. The description explicitly retains inspection. Inflated claim.'),
 ('holdout','baseline','aquarium-b',2):(False,False,True,'Named rationale contains a compatible tentative visitor benefit, not a management evaluation. Invents outcome-data requirements for a continuation decision.'),
 ('holdout','candidate1','aquarium-b',2):(False,False,True,'Named evidence gives a reception card, notice and corridors, not evidence of effectiveness. Adds source-unspecified criteria for deciding continuation.'),
 ('holdout','candidate1','archive-a',1):(False,False,True,'Present-tense news title is not a completed-action claim; the description preserves a limited repeat check. Frozen label retained.'),
 ('holdout','candidate1','archive-b',1):(True,False,True,'Catches unrecorded motive but reinforces absence of recorded access evidence as an established absence of effect.'),
 ('holdout','candidate1','archive-b',2):(False,False,True,'Same invented completed-action reading as archive-a; not an additional real defect.'),
 ('repeat','baseline','005-b',1):(True,True,False,'Frame the relationship as observed correlation or plausible attribution is a direct replacement-framing instruction.'),
 ('repeat','baseline','museum-b',1):(False,False,True,'Audience for evaluators does not itself establish an active formal review; misses explicit visitor-purpose diagnosis and inflates the claim.'),
 ('repeat','candidate1','005-b',1):(True,True,False,'Provides alternative quoted wording: the prompt change coincided with a shift / appears to have contributed. Diagnosis-only contract excludes replacements.'),
 ('repeat','candidate2','010-b',1):(False,False,False,'Demands exhaustive controls absent from a short idea; proposes additional experimental requirements rather than identifying an actual contradictory claim.'),
 ('repeat','candidate2','010-b',2):(True,False,False,'Real Narrative-first problem, but inflated into a universal sequence; must indicate Narrative is commonly recurrent prescribes extra title content.'),
}
notes={
 '005-b':'Correctly distinguishes observed shift from established causation; quoted limitations and Synthesis tension support the boundary.',
 '010-b':'Checks next-trial conditional priority against first supported stage; inspect any extra blockers separately.',
 'museum-b':'Explicit visitor task does not support reassignment to managers.',
 'library-b':'Confirmed pickup window remains binding; opening hours are not a replacement.',
 'seedbank-b':'Recorded decision and measurements do not establish the asserted committee motive.',
 'v4-museum-original':'Original management/evaluation angle replaces the explicit visitor note.',
 'v4-transit-original':'The public briefing assignment is not preserved by the policy-reader audience; central mismatch also missing from original idea.',
 'v4-library-original':'General-public audience materially broadens the expressly scoped existing-borrower notice.',
 'v4-seedbank-original':'Inspect the actual field: original known defect is results-motivated committee decision in description.',
 'canal-b':'Booking window and Saturday-only condition are explicit operational constraints; detecting either is a real material block.',
 'archive-b':'Unrecorded board motive and unmeasured access benefit are not established by the counts.',
 'review-b':'Occurrence counts do not establish preferred intervention or greatest payoff; source explicitly withholds ranking.',
 'aquarium-b':'Source explicitly assigns affected visitors and excludes staff evaluation; audience field violates the task.',
}
rows=[]
for b in blockers:
 key=tuple(b[k] for k in ['phase','condition','id','index'])
 vals=exceptions.get(key,(True,True,True,notes.get(b['id'],'Requires explicit human review')))
 rows.append({**{k:b[k] for k in ['phase','condition','id','index']},'realMismatch':vals[0],'sourceFaithful':vals[1],'nonPrescriptive':vals[2],'note':vals[3]})
(R/'verification/diagnosis-ratings.json').write_text(json.dumps(rows,indent=2,ensure_ascii=False),encoding='utf-8')
summary=[]
for cond in ['baseline','candidate1','candidate2']:
 rs=[r for r in rows if r['condition']==cond]
 summary.append(dict(condition=cond,blockers=len(rs),realMismatch=sum(r['realMismatch'] for r in rs),sourceFaithful=sum(r['sourceFaithful'] for r in rs),nonPrescriptive=sum(r['nonPrescriptive'] for r in rs),allThree=sum(all(r[k] for k in ['realMismatch','sourceFaithful','nonPrescriptive']) for r in rs)))
(R/'verification/diagnosis-summary.json').write_text(json.dumps(summary,indent=2),encoding='utf-8')
print(json.dumps(summary,indent=2))
