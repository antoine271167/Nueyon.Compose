param(
    [ValidateSet('phase1', 'phase2', 'linkedin')][string]$Stage = 'phase1',
    [switch]$Replay
)
$ErrorActionPreference = 'Stop'
$runner = Join-Path $PSScriptRoot 'runner/bin/Debug/net10.0/Runner.dll'
$config = Get-Content (Join-Path $PSScriptRoot 'config.json') -Raw | ConvertFrom-Json
dotnet $runner $PSScriptRoot preflight
if ($LASTEXITCODE) { throw 'Preflight failed.' }
if (!$Replay -and $Stage -ne 'phase1') {
    $decision = Get-Content (Join-Path $PSScriptRoot 'phase1-decision.json') -Raw | ConvertFrom-Json
    if ($decision.decision -ne 'GO') { throw 'Phase 1 did not pass.' }
    if ($Stage -eq 'linkedin') {
        foreach ($id in $config.phase2) {
            if (!(Test-Path (Join-Path $PSScriptRoot "calls/$id/result.json"))) { throw "Missing phase 2 result: $id" }
        }
    }
}
foreach ($id in $config.$Stage) {
    $call = Join-Path $PSScriptRoot "calls/$id"
    if ($Replay) {
        dotnet $runner $PSScriptRoot $id --replay
    } else {
        if (Test-Path (Join-Path $PSScriptRoot 'PAUSE')) { Write-Host 'Paused'; break }
        if (Test-Path (Join-Path $call 'result.json')) { continue }
        if (Test-Path (Join-Path $call 'attempt.json')) { throw "Unresolved attempt: $id" }
        dotnet $runner $PSScriptRoot $id
    }
    if ($LASTEXITCODE) { throw "Failed: $id" }
}
