param([switch]$Replay)
$ErrorActionPreference = 'Stop'
$PSNativeCommandUseErrorActionPreference = $false
$old = Join-Path (Split-Path $PSScriptRoot) 'experiment-11-editorial-tension-to-grounded-ending-2026-11-18'
$dll = Join-Path $PSScriptRoot 'runner/bin/Debug/net10.0/Runner.dll'
$archive = Join-Path $PSScriptRoot 'archive/webb-r1-H-c1-C-original'
$expectedHead = 'c16c2adf6948ae8cac0bb0f9c852e1974dff28ca'
$config = Get-Content "$old/config.json" -Raw | ConvertFrom-Json
$repo = git -C $PSScriptRoot rev-parse --show-toplevel
$relative = [IO.Path]::GetRelativePath($repo, $PSScriptRoot).Replace('\', '/')
$invalid = 'webb-r1-H-c1-C'
$original = Join-Path $old "calls/$invalid"
foreach ($file in Get-ChildItem -LiteralPath $original -File) {
    if ((Get-FileHash $file.FullName).Hash -ne (Get-FileHash (Join-Path $archive $file.Name)).Hash) {
        throw "Original archive differs at $($file.Name)"
    }
}
dotnet $dll $old preflight
if ($LASTEXITCODE -ne 0) { throw 'Frozen preflight failed.' }
# Capture the expected parser failure without writing to either call directory.
$failure = dotnet $dll $old $invalid --replay $archive 2>&1
$failureCode = $LASTEXITCODE
if ($failureCode -eq 0 -or "$failure" -notmatch 'Compose response content is empty\.') {
    throw "Original failure did not reproduce: $failure"
}
Write-Output "$($invalid): original failure replayed (missing content)"
for ($i = 0; $i -lt $config.schedule.Count; $i++) {
    $id = $config.schedule[$i]
    $name = if ($id -eq $invalid) { "$id-replacement" } else { $id }
    $call = if ($i -lt 15) { Join-Path $old "calls/$id" } else { Join-Path $PSScriptRoot "calls/$name" }
    if ($Replay) {
        if (-not (Test-Path "$call/result.json")) { continue }
        dotnet $dll $old $id --replay $call
    } else {
        $branch = git -C $repo branch --show-current
        $head = git -C $repo rev-parse HEAD
        $unexpected = git -C $repo status --porcelain -- . ":(exclude)$relative"
        if ($branch -ne 'main' -or $head -ne $expectedHead -or $unexpected) {
            throw 'Branch, HEAD or worktree changed outside Experiment 11b.'
        }
        if (Test-Path (Join-Path $PSScriptRoot 'PAUSE')) {
            Write-Output "Paused before $name"
            exit 0
        }
        if (Test-Path "$call/result.json") { continue }
        if ($i -lt 15 -or (Test-Path $call)) { throw "Missing original or unresolved attempt: $name" }
        Write-Output "Schedule position $($i + 1) of 48, $name"
        if ($id -eq $invalid) {
            dotnet $dll $old $id --live $call "$archive/request.json"
        } else {
            dotnet $dll $old $id --live $call
        }
    }
    if ($LASTEXITCODE -ne 0) { throw "Stopped at $name" }
}

