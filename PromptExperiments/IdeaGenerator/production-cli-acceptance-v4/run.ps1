param(
    [Parameter(Mandatory)][ValidateSet('acceptance','holdout','confirm')][string]$Stage,
    [switch]$AuthorizeModelCalls
)
$ErrorActionPreference='Stop'
$PSNativeCommandUseErrorActionPreference=$false
if (-not $AuthorizeModelCalls) { throw 'Paid calls require explicit authorization.' }
$repo=(Resolve-Path "$PSScriptRoot/../../..").Path
$frozen=Get-Content "$PSScriptRoot/verification/frozen.json" -Raw | ConvertFrom-Json
$revision=git -C $repo rev-parse HEAD
if ($revision -ne $frozen.revision) {throw 'Frozen revision changed.'}
foreach($file in $frozen.files) {
    if((Get-FileHash -LiteralPath "$PSScriptRoot/$($file.path)").Hash -ne $file.sha256){throw "Frozen artifact changed $($file.path)"}
}
foreach($file in (Get-Content "$PSScriptRoot/verification/production-prompt-hashes.json" -Raw | ConvertFrom-Json)) {
    if((Get-FileHash -LiteralPath "$repo/src/Nueyon.Compose.Application/Prompts/$($file.path)").Hash -ne $file.sha256){throw 'Production prompts changed.'}
}
foreach($probe in Get-ChildItem "$PSScriptRoot/offline" -File) {
    if(-not (Get-Content $probe.FullName -Raw | ConvertFrom-Json).passed){throw 'Offline gate failed.'}
}
$cli="$repo/src/Nueyon.Compose.Host.Console/bin/Debug/net10.0/Nueyon.Compose.Host.Console.dll"
$harness="$PSScriptRoot/harness/bin/Debug/net10.0/FeedbackLoop.dll"
Push-Location $repo
try {
    dotnet $harness preflight "$repo/src/Nueyon.Compose.Host.Console" > "$PSScriptRoot/verification/model-preflight.json"
    if($LASTEXITCODE -ne 0){throw 'Effective model preflight failed.'}
    $ids=switch($Stage) {
        'acceptance' {@('01-strategy','02-development','04-analysis')}
        'holdout' {@('holdout-01-museum','holdout-02-transit','holdout-03-library','holdout-04-seedbank')}
        'confirm' {
            $gate=Get-Content "$PSScriptRoot/ratings/initial-gates.json" -Raw | ConvertFrom-Json
            if(-not $gate.allPassed){throw 'Confirmation requires all initial quality and feedback gates.'}
            @('holdout-04-seedbank')
        }
    }
    foreach($id in $ids) {
        $used=@(Get-ChildItem "$PSScriptRoot/runs" -Recurse -Filter execution.json -ErrorAction SilentlyContinue | ForEach-Object {
            Get-Content $_.FullName -Raw | ConvertFrom-Json
        })
        $reserved=($used | Measure-Object -Property reservedModelCalls -Sum).Sum
        if($reserved + 6 -gt $frozen.maxLogicalCalls){throw 'Experiment call budget exhausted.'}
        $slot="$PSScriptRoot/runs/$Stage/$id"
        if(Test-Path -LiteralPath $slot){throw "Run slot already reserved $slot"}
        New-Item -ItemType Directory $slot -Force | Out-Null
        $command=@($cli,'run','idea','--input',"$PSScriptRoot/cases/$id/idea-input.json",
            '--output',"$slot/idea-result.json",'--trace',"$slot/idea-trace.json",'--prompts',"$PSScriptRoot/prompts")
        $execution=[ordered]@{
            revision=$revision;sdk=(dotnet --version);cwd=$repo;command=@('dotnet')+$command
            modelAlias='gpt-5-mini';resolvedModel=$null;maxIdeaAttempts=3;reservedModelCalls=6
            startedAt=[DateTimeOffset]::UtcNow;inputs=$frozen.files
            executableSha256=(Get-FileHash -LiteralPath $cli).Hash
            sourceSha256=(Get-FileHash -LiteralPath "$PSScriptRoot/cases/$id/idea-input.json").Hash
            authorization='User v4 instruction, -AuthorizeModelCalls'
        }
        $execution | ConvertTo-Json -Depth 12 | Set-Content -Encoding utf8 "$slot/execution.json"
        & dotnet @command 1> "$slot/stdout.txt" 2> "$slot/stderr.txt"
        $execution.exitCode=$LASTEXITCODE
        $execution.endedAt=[DateTimeOffset]::UtcNow
        $execution | ConvertTo-Json -Depth 12 | Set-Content -Encoding utf8 "$slot/execution.json"
        Write-Output "$Stage $id exit $($execution.exitCode)"
    }
} finally {Pop-Location}
