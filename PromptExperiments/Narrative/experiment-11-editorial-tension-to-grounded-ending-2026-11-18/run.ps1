param([switch]$Replay)
$ErrorActionPreference = 'Stop'
$dll = Join-Path $PSScriptRoot 'runner/bin/Debug/net10.0/Runner.dll'
$config = Get-Content (Join-Path $PSScriptRoot 'config.json') -Raw | ConvertFrom-Json
dotnet $dll $PSScriptRoot preflight
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
$repo = git -C $PSScriptRoot rev-parse --show-toplevel
$relative = [IO.Path]::GetRelativePath($repo, $PSScriptRoot).Replace('\', '/')
foreach ($id in $config.schedule) {
    if (-not $Replay) {
        $branch = git -C $repo branch --show-current
        $head = git -C $repo rev-parse HEAD
        $unexpected = git -C $repo status --porcelain -- . ":(exclude)$relative"
        if ($branch -ne 'main' -or $head -ne $config.baseCommit -or $unexpected) {
            throw "Branch, HEAD or worktree changed outside the experiment."
        }
    }
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
