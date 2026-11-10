param(
    [Parameter(Mandatory)]
    [ValidateSet('offline-replay', 'live-replay', 'acceptance', 'holdout', 'holdout-replay')]
    [string]$Stage,
    [string]$Candidate = 'judge-01',
    [string]$Batch = '01',
    [string]$CaseId,
    [switch]$AuthorizeModelCalls
)

$ErrorActionPreference = 'Stop'
if ($Stage -ne 'offline-replay' -and -not $AuthorizeModelCalls) {
    throw 'This stage uses paid model calls and requires explicit authorization.'
}
if ($Candidate -notmatch '^[a-z0-9]+(-[a-z0-9]+)*$' -or $Batch -notmatch '^[0-9]{2}$') {
    throw 'Use a candidate directory name and a two-digit batch number.'
}

$repo = (Resolve-Path (Join-Path $PSScriptRoot '../../..')).Path
$prompts = Join-Path $PSScriptRoot "candidates/$Candidate/prompts"
$v1 = Join-Path (Split-Path $PSScriptRoot) 'production-cli-acceptance-v1'
$baselineCases = @('01-strategy', '02-development', '04-analysis')
$caseIds = $baselineCases
if ($Stage -in 'holdout', 'holdout-replay') {
    $caseIds = @('holdout-01-museum', 'holdout-02-transit')
}
if ($CaseId) {
    if ($CaseId -notin $caseIds) {
        throw 'This case is not available in the selected stage.'
    }
    $caseIds = @($CaseId)
}

Push-Location $repo
try {
    foreach ($id in $caseIds) {
        $inputPath = Join-Path $PSScriptRoot "cases/$id/idea-input.json"
        $pathsToHash = @($inputPath)
        if ($Stage -in 'offline-replay', 'live-replay', 'holdout-replay') {
            $resultPath = if ($Stage -eq 'holdout-replay') {
                Join-Path $PSScriptRoot "fixtures/holdout-judge/$id/idea-result.json"
            } else {
                Join-Path $v1 "$id/idea-result.json"
            }
            $pathsToHash += $resultPath
            $runPath = Join-Path $PSScriptRoot "replay/$Candidate/$Stage/$Batch/$id"
            $mode = if ($Stage -eq 'offline-replay') { 'offline' } else { 'live' }
            $commandArgs = @('run', '--no-build', '--project',
                (Join-Path $PSScriptRoot 'harness/JudgeReplay.csproj'), '--',
                $mode, $prompts, $inputPath, $resultPath)
            $stdoutName = 'verdict.json'
        } else {
            $runPath = Join-Path $PSScriptRoot "runs/$Candidate/$Stage/$Batch/$id"
            $commandArgs = @('run', '--no-build', '--project', 'src/Nueyon.Compose.Host.Console',
                '--', 'run', 'idea', '--input', $inputPath, '--output', (Join-Path $runPath 'idea-result.json'),
                '--trace', (Join-Path $runPath 'idea-trace.json'), '--prompts', $prompts)
            $stdoutName = 'stdout.txt'
        }
        if (Test-Path -LiteralPath $runPath) {
            throw "Run already exists: $runPath"
        }
        $inputHashes = @(Get-FileHash -LiteralPath $pathsToHash | Select-Object Path, Hash)
        $promptHashes = @(Get-ChildItem $prompts -Recurse -Filter '*.prompt.txt' -File |
            Sort-Object FullName | Get-FileHash | Select-Object Path, Hash)
        $revision = git rev-parse HEAD
        $sdk = dotnet --version
        New-Item -ItemType Directory $runPath | Out-Null
        $startedAt = [DateTimeOffset]::UtcNow
        & dotnet @commandArgs 1> (Join-Path $runPath $stdoutName) 2> (Join-Path $runPath 'stderr.txt')
        $runExit = $LASTEXITCODE
        [ordered]@{
            revision = $revision
            sdk = $sdk
            modelAlias = 'gpt-5-mini'
            modelSnapshot = $null
            cwd = $repo
            command = @('dotnet') + $commandArgs
            startedAt = $startedAt
            endedAt = [DateTimeOffset]::UtcNow
            exitCode = $runExit
            inputs = $inputHashes
            prompts = $promptHashes
        } | ConvertTo-Json -Depth 8 | Set-Content -Encoding UTF8 (Join-Path $runPath 'execution.json')
        Write-Output "$Stage $id exit $runExit"
    }
} finally {
    Pop-Location
}
