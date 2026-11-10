param(
    [Parameter(Mandatory)]
    [ValidateSet('offline', 'feedback', 'acceptance', 'holdout', 'confirm-holdout')]
    [string]$Stage,
    [switch]$AuthorizeModelCalls
)

$ErrorActionPreference = 'Stop'
$PSNativeCommandUseErrorActionPreference = $false
$repo = (Resolve-Path (Join-Path $PSScriptRoot '../../..')).Path
$root = $PSScriptRoot
$prompts = Join-Path $root 'candidates/pair-01/prompts'
$cliDir = Join-Path $repo 'src/Nueyon.Compose.Host.Console/bin/Debug/net10.0'
$cli = Join-Path $cliDir 'Nueyon.Compose.Host.Console.dll'
$harness = Join-Path $root 'harness/bin/Debug/net10.0/FeedbackLoop.dll'
$plan = @{
    offline = @('02-development', '04-analysis', 'holdout-01-museum')
    feedback = @('02-development', '04-analysis', 'holdout-01-museum')
    acceptance = @('01-strategy', '02-development', '04-analysis')
    holdout = @('holdout-01-museum', 'holdout-02-transit', 'holdout-03-library')
    'confirm-holdout' = @('holdout-03-library')
}
$reservation = @{offline=0; feedback=3; acceptance=6; holdout=6; 'confirm-holdout'=6}
if ($Stage -ne 'offline' -and -not $AuthorizeModelCalls) {
    throw 'This stage uses paid model calls. Obtain explicit authorization before running.'
}

Push-Location $repo
try {
    $revision = git rev-parse HEAD
    if ($LASTEXITCODE -ne 0 -or $revision -ne (Get-Content "$root/verification/revision.txt")) {
        throw 'The repository revision differs from the frozen experiment.'
    }
    $sdk = dotnet --version
    $promptHashes = Get-Content -Raw "$root/verification/prompt-hashes.json" | ConvertFrom-Json
    foreach ($file in $promptHashes) {
        if ((Get-FileHash -LiteralPath (Join-Path $prompts $file.path)).Hash -ne $file.candidate) {
            throw "Candidate prompt changed: $($file.path)"
        }
    }
    if (@(Get-ChildItem -LiteralPath $prompts -Recurse -File).Count -ne $promptHashes.Count) {
        throw 'Unexpected files in the candidate prompt directory.'
    }
    $inputs = Get-Content -Raw "$root/verification/input-hashes.json" | ConvertFrom-Json -AsHashtable
    $cwdConfig = Join-Path $repo 'appsettings.json'
    $configDir = if (Test-Path -LiteralPath $cwdConfig) { $repo } else { $cliDir }
    if ($Stage -ne 'offline') {
        & dotnet $harness preflight $configDir
        if ($LASTEXITCODE -ne 0) { throw 'Model configuration preflight failed.' }
    }

    foreach ($id in $plan[$Stage]) {
        $relativeInput = "cases/$id/idea-input.json"
        $inputFile = Join-Path $root $relativeInput
        $inputHashes = [ordered]@{ $relativeInput = (Get-FileHash -LiteralPath $inputFile).Hash }
        $slot = Join-Path $root "runs/pair-01/$Stage/$id"
        $traceFile = Join-Path $slot 'idea-trace.json'
        $resultFile = Join-Path $slot 'idea-result.json'
        $stdoutName = 'stdout.txt'
        if ($Stage -in @('offline', 'feedback')) {
            $relativeFixture = "fixtures/$id.json"
            $fixture = Join-Path $root $relativeFixture
            $inputHashes[$relativeFixture] = (Get-FileHash -LiteralPath $fixture).Hash
            $mode = if ($Stage -eq 'offline') { 'offline' } else { 'live' }
            $command = @($harness, $mode, $configDir, $prompts, $inputFile, $fixture)
            $stdoutName = 'feedback-trace.json'
        } else {
            $command = @($cli, 'run', 'idea', '--input', $inputFile, '--output', $resultFile,
                '--trace', $traceFile, '--prompts', $prompts)
        }
        foreach ($path in $inputHashes.Keys) {
            if ($inputHashes[$path] -ne $inputs[$path]) { throw "Frozen input changed: $path" }
        }

        # Reserve the slot before invocation. A failed or interrupted run still consumes its reservation.
        if (Test-Path -LiteralPath $slot) { throw "Run slot already used: $slot" }
        New-Item -ItemType Directory -Path $slot | Out-Null
        $execution = [ordered]@{
            revision = $revision
            sdk = $sdk
            cwd = $repo
            command = @('dotnet') + $command
            modelAlias = if ($Stage -eq 'offline') { $null } else { 'gpt-5-mini' }
            modelSnapshot = $null
            maxIdeaAttempts = 3
            reservedModelCalls = $reservation[$Stage]
            startedAt = [DateTimeOffset]::UtcNow
            inputs = $inputHashes
            prompts = $promptHashes
            executableSha256 = (Get-FileHash -LiteralPath $command[0]).Hash
        }
        $executionFile = Join-Path $slot 'execution.json'
        $execution | ConvertTo-Json -Depth 8 | Set-Content -Encoding utf8 $executionFile
        & dotnet @command 1> (Join-Path $slot $stdoutName) 2> (Join-Path $slot 'stderr.txt')
        $execution.exitCode = $LASTEXITCODE
        $execution.endedAt = [DateTimeOffset]::UtcNow
        $execution | ConvertTo-Json -Depth 8 | Set-Content -Encoding utf8 $executionFile
        Write-Output "$Stage $id exit $($execution.exitCode)"
    }
} finally {
    Pop-Location
}
