$ErrorActionPreference = 'Stop'
$repo = 'C:\Workspaces\Nueyon.Compose'
$experiment = Join-Path $repo 'PromptExperiments\experiment-011-narrative-boundary-editorial-fidelity'
$production = Join-Path $repo 'src\Nueyon.Compose.Application\Prompts\Narrative\Narrative.system.prompt.txt'
$baseline = Join-Path $experiment 'baseline\prompts\Narrative\Narrative.system.prompt.txt'
$candidate = Join-Path $experiment 'candidate\prompts\Narrative.system.prompt.txt'
$solution = Join-Path $repo 'src\Nueyon.Compose.slnx'
$order = @(
    @{ name = 'candidate-002'; condition = 'candidate' },
    @{ name = 'baseline-003'; condition = 'baseline' },
)
function Get-EmbeddedPromptHash([string]$Root) {
    $helperPath = Join-Path $experiment 'verify-embedded-prompt.ps1'
    $hashLines = & pwsh -NoProfile -File $helperPath -Root $Root
    if ($LASTEXITCODE -ne 0) { throw 'Could not read embedded prompt hash in helper process.' }
    return ([string]($hashLines | Select-Object -Last 1)).Trim()
}
$manifest = [System.Collections.Generic.List[object]]::new()
$manifestPath = Join-Path $experiment 'run-manifest.json'
if (Test-Path $manifestPath) { foreach ($item in (Get-Content -Raw $manifestPath | ConvertFrom-Json)) { $manifest.Add($item) } }
try {
    foreach ($run in $order) {
        $prompt = if ($run.condition -eq 'baseline') { $baseline } else { $candidate }
        Copy-Item -LiteralPath $prompt -Destination $production -Force
        [IO.File]::SetLastWriteTimeUtc($production, [DateTime]::UtcNow.AddSeconds(10))
        $runDir = Join-Path $experiment ($run.condition + '\runs\' + $run.name)
        New-Item -ItemType Directory -Path $runDir -Force | Out-Null
        Push-Location $repo
        try {
            & dotnet build $solution --no-restore --verbosity minimal 2>&1 | Tee-Object -FilePath (Join-Path $runDir 'embedding-build.log')
            $buildCode = $LASTEXITCODE
        }
        finally { Pop-Location }
        if ($buildCode -ne 0) { throw "Prompt embedding build failed for $($run.name)." }
        $actualHash = Get-EmbeddedPromptHash $repo
        $expectedHash = (Get-FileHash $prompt -Algorithm SHA256).Hash
        if ($actualHash -ne $expectedHash) { throw "Embedded prompt hash mismatch before $($run.name); model run skipped." }
        $embedded = Join-Path $repo 'src\Nueyon.Compose.Application\bin\Debug\net10.0\Nueyon.Compose.Application.Prompts.Narrative.Narrative.system.prompt.txt'
        $start = (Get-Date).ToString('o')
        $timer = [Diagnostics.Stopwatch]::StartNew()
        & dotnet 'Nueyon.Compose.Host.Console.dll' 2>&1 | Tee-Object -FilePath (Join-Path $runDir 'console.log')
        $exitCode = $LASTEXITCODE
        $timer.Stop()
        $end = (Get-Date).ToString('o')
        $logPath = Join-Path $runDir 'console.log'
        $log = [IO.File]::ReadAllText($logPath)
        $patterns = [ordered]@{
            'selected-idea.txt' = '(?ms)^Selected Idea\r?\n-+\r?\n(?<x>.*?)(?=^Research\r?\n-+\r?\n)'
            'research.txt' = '(?ms)^Research\r?\n-+\r?\n(?<x>.*?)(?=^Synthesis\r?\n-+\r?\n)'
            'synthesis.txt' = '(?ms)^Synthesis\r?\n-+\r?\n(?<x>.*?)(?=^Narrative\r?\n-+\r?\n)'
            'narrative.txt' = '(?ms)^Narrative\r?\n-+\r?\n(?<x>.*?)(?=^Compose\r?\n-+\r?\n)'
            'compose.txt' = '(?ms)^Compose\r?\n-+\r?\n(?<x>.*?)(?=\r?\n\s*\z)'
        }
        foreach ($entry in $patterns.GetEnumerator()) {
            $match = [regex]::Match($log, $entry.Value)
            if ($match.Success) {
                [IO.File]::WriteAllText((Join-Path $runDir $entry.Key), $match.Groups['x'].Value.TrimEnd() + "`r`n", [Text.UTF8Encoding]::new($false))
            }
        }
        $ideaPath = Join-Path $runDir 'selected-idea.txt'
        $synthesisPath = Join-Path $runDir 'synthesis.txt'
        if ((Test-Path $ideaPath) -and (Test-Path $synthesisPath)) {
            $narrativeInput = "SELECTED IDEA`r`n`r`n" + [IO.File]::ReadAllText($ideaPath) + "`r`nSYNTHESIS`r`n`r`n" + [IO.File]::ReadAllText($synthesisPath)
            [IO.File]::WriteAllText((Join-Path $runDir 'narrative-input.txt'), $narrativeInput, [Text.UTF8Encoding]::new($false))
        }
        $manifest.Add([ordered]@{ run = $run.name; condition = $run.condition; started = $start; ended = $end; durationSeconds = [Math]::Round($timer.Elapsed.TotalSeconds, 2); exitCode = $exitCode; embeddedPromptSha256 = Get-EmbeddedPromptHash $repo; expectedPromptSha256 = (Get-FileHash $prompt -Algorithm SHA256).Hash; buildLogSha256 = (Get-FileHash (Join-Path $runDir 'embedding-build.log') -Algorithm SHA256).Hash; logSha256 = (Get-FileHash $logPath -Algorithm SHA256).Hash; outputs = (Get-ChildItem $runDir -File | Select-Object -ExpandProperty Name) })
        $manifest | ConvertTo-Json -Depth 5 | Set-Content -LiteralPath (Join-Path $experiment 'run-manifest.json') -Encoding utf8
        if ($exitCode -ne 0) { throw "Run $($run.name) exited with $exitCode; stopping the experiment." }
        if ((Get-EmbeddedPromptHash $repo) -ne (Get-FileHash $prompt -Algorithm SHA256).Hash) { throw "Embedded prompt hash mismatch during $($run.name)." }
        Write-Output "Completed $($run.name), embedded prompt SHA-256 $(Get-EmbeddedPromptHash $repo)."
    }
}
finally {
    Copy-Item -LiteralPath $baseline -Destination $production -Force
    Push-Location $repo
    try {
        & dotnet build $solution --no-restore --verbosity minimal 2>&1 | Tee-Object -FilePath (Join-Path $experiment 'integrity\final-rollback-build.log')
        if ($LASTEXITCODE -ne 0) { throw 'Build failed while restoring baseline prompt assembly.' }
    }
    finally { Pop-Location }
    Write-Output 'Production and embedded Narrative prompt restored to baseline.'
}







