param([switch]$Replay)
$ErrorActionPreference = 'Stop'
$dll = Join-Path $PSScriptRoot 'runner/bin/Debug/net10.0/Runner.dll'
$config = Get-Content (Join-Path $PSScriptRoot 'config.json') -Raw | ConvertFrom-Json
dotnet $dll $PSScriptRoot preflight
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
foreach ($id in $config.schedule) {
    if (-not $Replay -and (Test-Path (Join-Path $PSScriptRoot PAUSE))) {
        Write-Output "Paused before $id"
        exit 0
    }
    $call = Join-Path $PSScriptRoot "calls/$id"
    if ($Replay) {
        if (-not (Test-Path (Join-Path $call result.json))) { continue }
        dotnet $dll $PSScriptRoot $id --replay
    } else {
        if (Test-Path (Join-Path $call result.json)) { continue }
        if (Test-Path (Join-Path $call attempt.json)) { throw "Unresolved attempt: $id" }
        dotnet $dll $PSScriptRoot $id
    }
    if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
}
