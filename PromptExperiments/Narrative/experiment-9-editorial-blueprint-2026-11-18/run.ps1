param(
    [ValidateSet('narrative', 'linkedin')][string]$Stage = 'narrative',
    [switch]$Replay
)
$ErrorActionPreference = 'Stop'
$runner = Join-Path $PSScriptRoot 'runner/bin/Debug/net10.0/Runner.dll'
$config = Get-Content "$PSScriptRoot/config.json" -Raw | ConvertFrom-Json
dotnet $runner $PSScriptRoot preflight
if ($LASTEXITCODE) { throw 'Preflight failed.' }
if ($Stage -eq 'linkedin') {
    $review = Get-Content "$PSScriptRoot/narrative-scores.json" -Raw | ConvertFrom-Json -AsHashtable
    foreach ($id in $config.narrative) {
        if (!$review.ContainsKey($id) -or $review[$id].fidelity -notin 0,1,2) { throw "Missing fidelity review: $id" }
    }
}
foreach ($id in $config.$Stage) {
    $call = "$PSScriptRoot/calls/$id"
    if ($Stage -eq 'linkedin' -and $review[$id.Substring(9)].fidelity -ne 2) {
        Write-Output "$id skipped (fidelity gate)"
        continue
    }
    if ($Replay) {
        if (!(Test-Path "$call/result.json")) { Write-Output "$id has no completed result"; continue }
        dotnet $runner $PSScriptRoot $id --replay
    } else {
        if (Test-Path "$PSScriptRoot/PAUSE") { Write-Output 'Paused'; break }
        if (Test-Path "$call/result.json") { continue }
        if (Test-Path "$call/attempt.json") { throw "Unresolved attempt: $id" }
        dotnet $runner $PSScriptRoot $id
    }
    if ($LASTEXITCODE) { throw "Failed: $id" }
}
