$ErrorActionPreference='Stop'
$repo=(Resolve-Path "$PSScriptRoot/../../..").Path
$frozen=Get-Content "$PSScriptRoot/verification/frozen.json" -Raw | ConvertFrom-Json
$ledger=@()
$handoffs=@()
foreach($executionFile in Get-ChildItem "$PSScriptRoot/runs" -Recurse -Filter execution.json) {
    $slot=$executionFile.Directory.FullName
    $id=$executionFile.Directory.Name
    $stage=$executionFile.Directory.Parent.Name
    $ratingId=if($stage -eq 'confirm'){"$id-confirm"}else{$id}
    $execution=Get-Content $executionFile.FullName -Raw | ConvertFrom-Json
    $trace=if(Test-Path "$slot/idea-trace.json"){Get-Content "$slot/idea-trace.json" -Raw | ConvertFrom-Json}
    $rating=Get-Content "$PSScriptRoot/ratings/$ratingId.json" -Raw | ConvertFrom-Json
    $calls=@()
    foreach($attempt in $trace.attempts) {
        $calls+=@{role='generator';attempt=$attempt.attempt;status='response-recorded'}
        # A semantic acceptance or a structured blocker array proves that the judge returned.
        if($attempt.status -eq 'accepted' -or $attempt.feedback -match '^\s*\[') {
            $calls+=@{role='judge';attempt=$attempt.attempt;status=$attempt.status}
        }
    }
    $ledger+=@{
        case=$id;stage=$stage;reserved=$execution.reservedModelCalls
        observedLogicalCalls=$calls.Count;calls=$calls;exitCode=$execution.exitCode
        traceStatus=$trace.status
        command=$execution.command;modelAlias=$execution.modelAlias;resolvedModel=$null
        startedAt=$execution.startedAt;endedAt=$execution.endedAt
    }
    if($rating.passed -and $trace.status -eq 'accepted') {
        $target="$PSScriptRoot/approved-research/$ratingId"
        if(Test-Path -LiteralPath $target){throw 'Research handoff already exists.'}
        New-Item -ItemType Directory $target -Force | Out-Null
        $inputObject=Get-Content "$PSScriptRoot/cases/$id/idea-input.json" -Raw | ConvertFrom-Json
        $selected=(Get-Content "$slot/idea-result.json" -Raw | ConvertFrom-Json)[0]
        @{input=$inputObject;selectedIdea=@{idea=$selected}} | ConvertTo-Json -Depth 15 |
            Set-Content -Encoding utf8 "$target/research-input.json"
        $valid=Test-Json -LiteralPath "$target/research-input.json" -SchemaFile "$PSScriptRoot/verification/research-schema.json"
        $roundtrip=Get-Content "$target/research-input.json" -Raw | ConvertFrom-Json
        if(-not $valid -or $roundtrip.input.content -cne $inputObject.content -or
            ($roundtrip.selectedIdea.idea | ConvertTo-Json -Compress) -cne ($selected | ConvertTo-Json -Compress)) {
            throw 'Research handoff validation failed.'
        }
        $handoffs+=@{case=$id;stage=$stage;schemaValid=$valid;sourcePreserved=$true;ideaPreserved=$true;sha256=(Get-FileHash "$target/research-input.json").Hash}
    } else {
        $target="$PSScriptRoot/fixtures/rejected-cli/$ratingId"
        New-Item -ItemType Directory $target -Force | Out-Null
        foreach($name in 'idea-result.json','idea-trace.json','execution.json') {
            if(Test-Path "$slot/$name"){Copy-Item -LiteralPath "$slot/$name" "$target/$name"}
        }
        Copy-Item -LiteralPath "$PSScriptRoot/ratings/$ratingId.json" "$target/rating.json"
        Copy-Item -LiteralPath "$PSScriptRoot/cases/$id/idea-input.json" "$target/idea-input.json"
    }
}
$observed=($ledger | Measure-Object -Property observedLogicalCalls -Sum).Sum
$reserved=($ledger | Measure-Object -Property reserved -Sum).Sum
if($observed -gt $reserved -or $reserved -gt $frozen.maxLogicalCalls){throw 'Budget invariant failed.'}
@{
    maximumLogicalCalls=$frozen.maxLogicalCalls;reservedLogicalCalls=$reserved;observedLogicalCalls=$observed
    providerBilledCalls=$null;tokens=$null;actualCost=$null;transportRetries=$null;runs=$ledger
} | ConvertTo-Json -Depth 20 | Set-Content -Encoding utf8 "$PSScriptRoot/verification/model-call-ledger.json"
ConvertTo-Json -InputObject @($handoffs) -Depth 10 | Set-Content -Encoding utf8 "$PSScriptRoot/verification/research-handoffs.json"

$old=Get-Content "$PSScriptRoot/verification/older-experiments-hashes.json" -Raw | ConvertFrom-Json
$oldChanged=@($old | Where-Object {(Get-FileHash -LiteralPath "$repo/$($_.path)").Hash -ne $_.sha256})
$prompts=Get-Content "$PSScriptRoot/verification/production-prompt-hashes.json" -Raw | ConvertFrom-Json
$promptsChanged=@($prompts | Where-Object {(Get-FileHash -LiteralPath "$repo/src/Nueyon.Compose.Application/Prompts/$($_.path)").Hash -ne $_.sha256})
$frozenChanged=@($frozen.files | Where-Object {(Get-FileHash -LiteralPath "$PSScriptRoot/$($_.path)").Hash -ne $_.sha256})
@{
    olderFilesChecked=$old.Count;olderFilesChanged=$oldChanged
    productionFilesChecked=$prompts.Count;productionFilesChanged=$promptsChanged
    frozenFilesChecked=$frozen.files.Count;frozenFilesChanged=$frozenChanged
    trackedWorkingChanges=@(git -C $repo status --porcelain=v1 --untracked-files=no)
} | ConvertTo-Json -Depth 10 | Set-Content -Encoding utf8 "$PSScriptRoot/verification/final-integrity.json"
if($oldChanged.Count -or $promptsChanged.Count -or $frozenChanged.Count){throw 'Integrity gate failed.'}
Write-Output "Observed $observed logical calls, reserved $reserved of $($frozen.maxLogicalCalls). Handoffs $($handoffs.Count). Older files checked $($old.Count)."
