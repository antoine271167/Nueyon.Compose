param([switch]$Replay)
$ErrorActionPreference = 'Stop'
$runner = Join-Path $PSScriptRoot 'runner/bin/Debug/net10.0/Runner.dll'
$config = Get-Content (Join-Path $PSScriptRoot 'config.json') -Raw | ConvertFrom-Json
dotnet $runner $PSScriptRoot preflight
if ($LASTEXITCODE) { throw 'Preflight failed.' }
foreach ($id in $config.schedule) {
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
