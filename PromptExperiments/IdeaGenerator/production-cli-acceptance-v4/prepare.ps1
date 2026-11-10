$ErrorActionPreference = 'Stop'
$repo = (Resolve-Path "$PSScriptRoot/../../..").Path
$v3 = Join-Path $repo 'PromptExperiments/IdeaGenerator/production-cli-acceptance-v3'
$jsonDepth = 30
function Write-Json($Value, $Path) {
    $Value | ConvertTo-Json -Depth $jsonDepth | Set-Content -Encoding utf8 $Path
}
if (Test-Path "$PSScriptRoot/verification/frozen.json") { throw 'Experiment is already frozen.' }
$diagnoses = @{
    '02-development' = @(
        @{category='sourceFidelity';field='title, description, rationale';problem='The idea attributes worse fidelity to a generic rule and generalizes beyond this candidate comparison.';sourceEvidence='Experiment 005 Limitations describes three runs per condition, non-contemporaneous baselines and possible backend variability. Analysis notes tension already in Synthesis.';constraint='Observed scores and non-promotion apply to this candidate and sample. The comparison does not establish a general causal effect.'},
        @{category='sourceFidelity';field='title';problem='The title still states that adding the instruction worsened fidelity, despite qualifying correlation elsewhere.';sourceEvidence='Experiment 005 Limitations and Analysis bound attribution. The score differences are observations from the archived baseline and candidate runs.';constraint='Attribution in every field remains within the evidence of this limited comparison.'}
    )
    '04-analysis' = @(
        @{category='editorialPurpose';field='title, description';problem='Narrative is made the primary intervention and prevention is claimed from recurrence.';sourceEvidence='Cross-experiment patterns and Implication for the next focused trial describe varying first drift across Idea, Synthesis and Narrative and inspecting actual Synthesis before changing Narrative.';constraint='Intervention selection depends on the earliest supported deviation in the chosen case. Recurrence establishes neither cause nor preventive effect.'},
        @{category='editorialPurpose';field='title, description';problem='The proposed controlled trial still fixes Narrative as the decision boundary in advance.';sourceEvidence='Implication for the next focused trial conditions intervention on the chosen failure and upstream evidence.';constraint='The source leaves the intervention stage conditional on case evidence, including possible upstream drift.'}
    )
    'holdout-01-museum' = @(
        @{category='sourceFidelity';field='title, description';problem='Visitor satisfaction and broad weather-closure benefits are asserted without supporting measurements.';sourceEvidence='The museum counted 38 copies at two wet Sunday openings, did not systematically count completed routes and did not track satisfaction. The route is offered for high-water garden closures.';constraint='The evidence supports availability of a visitor alternative during high water, without a demonstrated satisfaction outcome.'},
        @{category='editorialPurpose';field='title, description, audience';problem='An internal measurement evaluation replaces the intended visitor communication.';sourceEvidence='The note is intended to help visitors adapt their visit when the garden is closed.';constraint='The intended piece serves visitors adapting a planned garden visit. The source does not establish an internal evaluation assignment.'}
    )
}
foreach ($id in $diagnoses.Keys) {
    $old = Get-Content "$v3/fixtures/$id.json" -Raw | ConvertFrom-Json
    $verdicts = @(
        @{accepted=$false;blockingIssues=@($diagnoses[$id][0])},
        @{accepted=$false;blockingIssues=@($diagnoses[$id][1])},
        @{accepted=$true;blockingIssues=@()}
    )
    Write-Json ([ordered]@{origins=$old.origins;ideas=$old.ideas;expected=@('rejected','rejected','accepted');verdicts=$verdicts;error=$null}) "$PSScriptRoot/fixtures/$id.json"
    Write-Json ([ordered]@{
        ideas=@($old.ideas[0],$old.ideas[0],$old.ideas[0])
        expected=@('rejected','rejected','rejected')
        verdicts=@($verdicts[0],$verdicts[0],$verdicts[0])
        error='InvalidOperationException: Idea agent failed to produce valid ideas after 3 attempts.'
    }) "$PSScriptRoot/fixtures/$id.exhausted.json"
}
$newCase = "$PSScriptRoot/cases/holdout-04-seedbank"
New-Item -ItemType Directory $newCase | Out-Null
$source = @'
# Seed bank donor update: continue the storage trial on its present scale

The community seed bank needs a donor update explaining its decision on a small storage trial. This is an account of the trial and its limited continuation, not an invitation to buy seed.

In February, volunteers tested two retained lots, one of bean Alder and one of bean Reed, after six months in the new dry cupboard. In a germination check, 18 of 20 Alder seeds and 14 of 20 Reed seeds sprouted. Last year's notebook records 16 of 20 Alder and 15 of 20 Reed in an older room. The years used different harvest lots. Temperature was logged only in the new cupboard. No paired comparison of the same lot in both places was run. Neither growing-season yield nor performance in gardens was measured.

At its March meeting, the committee chose to keep the same two lots in the cupboard for another six months and repeat the germination check. It has not approved moving other varieties into the cupboard. A volunteer proposed replacing the older room, but the committee deferred that decision. The donor update should explain what was observed and why the decision remains a limited continuation. The notes give no claim that the cupboard caused an improvement and no promise of better garden results.
'@
$source | Set-Content -Encoding utf8 "$newCase/seed-bank-update.md"
Write-Json @{content="--- BEGIN SOURCE: seed-bank-update.md ---`n`n$source`n`n--- END SOURCE: seed-bank-update.md ---"} "$newCase/idea-input.json"
$files = Get-ChildItem "$PSScriptRoot/cases","$PSScriptRoot/fixtures","$PSScriptRoot/prompts","$PSScriptRoot/harness" -Recurse -File |
    Where-Object { $_.FullName -notmatch '[\\/](bin|obj)[\\/]' }
$hashes = @($files | ForEach-Object {
    @{path=[IO.Path]::GetRelativePath($PSScriptRoot,$_.FullName);sha256=(Get-FileHash -LiteralPath $_.FullName).Hash}
})
$preserved = @(
    Get-ChildItem (Join-Path $repo 'PromptExperiments') -Recurse -File |
        Where-Object { -not $_.FullName.StartsWith($PSScriptRoot) -and $_.FullName -notmatch '[\\/](bin|obj)[\\/]' } |
        ForEach-Object {@{path=[IO.Path]::GetRelativePath($repo,$_.FullName);sha256=(Get-FileHash -LiteralPath $_.FullName).Hash}}
)
Write-Json $preserved "$PSScriptRoot/verification/older-experiments-hashes.json"
$production = @(Get-ChildItem "$repo/src/Nueyon.Compose.Application/Prompts" -Recurse -File | ForEach-Object {
    $relative=[IO.Path]::GetRelativePath("$repo/src/Nueyon.Compose.Application/Prompts",$_.FullName)
    $hash=(Get-FileHash -LiteralPath $_.FullName).Hash
    if ((Get-FileHash -LiteralPath "$PSScriptRoot/prompts/$relative").Hash -ne $hash) {throw 'Prompt snapshot mismatch'}
    @{path=$relative;sha256=$hash}
})
Write-Json $production "$PSScriptRoot/verification/production-prompt-hashes.json"
Write-Json ([ordered]@{
    revision=(git -C $repo rev-parse HEAD);frozenAt=[DateTimeOffset]::UtcNow
    condition='Byte-identical current production prompts. No tuning after freeze.'
    authorization='Current user request authorizes bounded paid GPT-5-mini calls for v4.'
    maxLogicalCalls=48
    runs=@('01-strategy','02-development','04-analysis','holdout-01-museum','holdout-02-transit','holdout-03-library','holdout-04-seedbank')
    maxAttemptsPerRun=3;reservedCallsPerRun=6
    confirmation='One further run of the new seed-bank holdout only if all initial quality and feedback gates pass.'
    logicalCallDefinition='One generator or semantic judge invocation. SDK transport retries may add requests.'
    moneyCap=$null;billing='Provider billed totals, token use and resolved model snapshot are not exposed by the production CLI.'
    promotion='All offline gates, originals 3/3, strategy 10/10, known holdouts 3/3, new holdout pass, every diagnosis faithful and free of new assignment, and new holdout confirmation pass.'
    scoreOrder=@('sourceFidelity','editorialPurpose','focus','distinctiveAngle','appeal')
    scoreRange=@(0,2);minimumTotal=8;requiredSourceFidelity=2;requiredEditorialPurpose=2
    assessment='Same agent assesses complete sources and final first ideas independently of judge verdict, before inspecting diagnostics. Not an external or blinded evaluator.'
    newHoldout='New synthetic seed-bank source authored after freezing the production prompt choice. Never used for tuning.'
    files=$hashes
}) "$PSScriptRoot/verification/frozen.json"
