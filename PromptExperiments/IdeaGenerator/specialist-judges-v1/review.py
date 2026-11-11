"""Recorded same-author review of each complete diagnosis; no model calls."""
import pathlib,json,collections
R=pathlib.Path(__file__).resolve().parent
# Explicit review exceptions after reading complete verdicts alongside frozen sources/ideas.
# (real mismatch, source faithful, non-prescriptive, reason)
exceptions={
'tuning/baseline/005-b/0':(True,True,False,'Correct causal boundary, but should describe and report that prescribe replacement framing.'),
'tuning/v2/010-b/1':(False,False,False,'The description already makes the Narrative check conditional; invents a second defect and requires it to be rewritten.'),
'tuning/editorial/v4-005-good/0':(False,False,False,'No solely claim exists in the idea. Invents a requirement to enumerate all primary reasons in an idea rather than an explanation plan.'),
'holdout/baseline/lockers-b/0':(True,True,False,'Correct causal boundary but any causal connection must be framed prescribes framing.'),
'holdout/editorial/lockers-b/0':(True,True,False,'Correct causal mismatch; imperative Do not present rather than declarative constraint.'),
'holdout/baseline/kiln-b/0':(True,True,False,'Correct intervention-benefit boundary; imperative Do not present.'),
'holdout/v2/kiln-b/0':(True,True,False,'Correct benefit boundary; imperative Do not assert or rank.'),
'holdout/v2/kiln-b/1':(True,True,False,'Correct case-specific test boundary; adds imperative do not preselect.'),
'holdout/baseline/hall-b/0':(True,True,False,'Correct Tuesday scope mismatch; imperatives Do not generalize and copy must limit prescribe revision.'),
'holdout/v2/hall-b/1':(False,False,False,'Cancellation detail is real, but source does not require it in every idea description. Invents a compulsory omission blocker.'),
'holdout/baseline/orchard-b/0':(True,True,False,'Correct unsupported motive; imperative Do not present and preserve.'),
'holdout/source/orchard-b/0':(True,True,False,'Correct unsupported motive; imperative Do not present.'),
'holdout/editorial/orchard-b/0':(True,True,False,'Correct unsupported motive; imperative Do not state or imply.'),
'holdout/baseline/pool-b/0':(True,False,False,'Core job mismatch is correct; any discussion must be attributed adds a source-unstated framing requirement.'),
'holdout/source/pool-b/0':(True,True,False,'Correct job mismatch outside specialist remit; imperative Do not treat and content must be limited.'),
'holdout/baseline/bursary-b/0':(True,True,False,'Correct status mismatch; Titles must reflect prescribes title content in addition to imperative Do not present.'),
'holdout/editorial/bursary-b/0':(True,True,False,'Correct status mismatch; imperative Do not present and limit statements.'),
'holdout/editorial/bursary-b/1':(True,False,False,'Announcing awards changes the job, but eligibility is not in source and demanding timeline/contingency in idea adds requirements. Imperative Maintain and include.'),
'holdout/baseline/roof-b/0':(True,True,False,'Correct uncertainty mismatch; must preserve and present roof only prescribes framing.'),
'holdout/source/roof-b/0':(True,False,False,'Core certainty mismatch correct; copy must reflect both possibilities requires added content the source does not mandate for this idea.'),
'repeat/baseline/005-b/0':(True,True,False,'Correct causal boundary; state it as and note limitations explicitly prescribe replacement content.'),
'repeat/v2/010-b/1':(False,False,False,'Invents a requirement that a short idea description enumerate every experimental control; the description already plans the source-supported next trial.')
}
b=json.loads((R/'verification/blockers.json').read_text())
review=[];counts=collections.defaultdict(collections.Counter)
for x in b:
 k=x['key'];real,faithful,nonprescriptive,note=exceptions.get(k,(True,True,True,'Exact material mismatch, accurate source boundary, no additional rewrite or research requirement.'))
 cond=k.split('/')[1];a=dict(key=k,realMismatch=real,sourceFaithful=faithful,nonPrescriptive=nonprescriptive,allThree=real and faithful and nonprescriptive,note=note)
 review.append(a);counts[cond].update(total=1,realMismatch=int(real),sourceFaithful=int(faithful),nonPrescriptive=int(nonprescriptive),allThree=int(a['allThree']))
(R/'verification/diagnosis-review.json').write_text(json.dumps(review,indent=2),encoding='utf-8')
(R/'verification/diagnosis-summary.json').write_text(json.dumps(counts,indent=2),encoding='utf-8')
lines=['# Complete diagnosis review','', 'Same author, not independent or blinded. Frozen labels retained. Strict gate treats imperatives as non-declarative; ordinary declarative source boundaries using must/must not can pass. This stringent language distinction is debatable. Relaxing it cannot rescue missed original v4 errors, false rejection or invented content requirements. Every blocker is scored, including duplicates and correct kernels with unsupported additions. Field labels are unconstrained strings in the existing schema; accepted parser output does not prove accurate localization.','', '| Blocker | Real | Faithful | Non-prescriptive | Note |','|---|---|---|---|---|']
for a in review:lines.append('| '+a['key']+' | '+str(a['realMismatch'])+' | '+str(a['sourceFaithful'])+' | '+str(a['nonPrescriptive'])+' | '+a['note']+' |')
(R/'diagnoses.md').write_text('\n'.join(lines)+'\n',encoding='utf-8')
print(json.dumps(counts,indent=2))
