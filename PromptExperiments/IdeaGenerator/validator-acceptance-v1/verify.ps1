$ErrorActionPreference = 'Stop'
$root = $PSScriptRoot
Set-Location (Resolve-Path "$root/../../..")
$frozen = Get-Content "$root/verification/frozen.json" -Raw | ConvertFrom-Json
foreach ($file in $frozen) {
    if ((Get-FileHash -LiteralPath (Join-Path $root $file.path)).Hash -ne $file.sha256) {
        throw "Hash mismatch: $($file.path)"
    }
}
$revision = git rev-parse HEAD
if ($revision -ne (Get-Content "$root/verification/head.txt")) { throw 'HEAD differs.' }
git diff --quiet HEAD --
if ($LASTEXITCODE -ne 0) { throw 'Tracked files differ from HEAD.' }
$production = 'src/Nueyon.Compose.Application/Prompts/Idea/IdeaValidation.system.prompt.txt'
if ((Get-FileHash $production).Hash -ne (Get-FileHash "$root/prompts/baseline/Idea/IdeaValidation.system.prompt.txt").Hash) {
    throw 'Production prompt and baseline differ.'
}
$ledger = Get-Content "$root/verification/call-ledger.jsonl" | ForEach-Object { $_ | ConvertFrom-Json }
$reservations = @($ledger | Where-Object event -eq reserved)
$completions = @($ledger | Where-Object event -eq completed)
$verdicts = @(Get-ChildItem "$root/runs" -Recurse -Filter '*.verdict.json')
$metadata = @(Get-ChildItem "$root/runs" -Recurse -Filter '*.metadata.json' | ForEach-Object {
    Get-Content $_ -Raw | ConvertFrom-Json
})
if ($reservations.Count -ne 28 -or @($reservations.key | Sort-Object -Unique).Count -ne 28 -or
    (Compare-Object ($reservations.key | Sort-Object) ($completions.key | Sort-Object))) {
    throw 'Live call ledger does not match the frozen schedule.'
}
[ordered]@{
    head = $revision
    trackedTreeMatchesHead = $true
    frozenFilesVerified = $frozen.Count
    productionPromptMatchesBaseline = $true
    reserved = $reservations.Count
    requestsPrepared = @($completions | Where-Object requestPrepared).Count
    responses = $verdicts.Count
    cleanExit = @($completions | Where-Object exit -eq 0).Count
    modelIds = @($metadata.modelId | Sort-Object -Unique)
    inputTokens = ($metadata | ForEach-Object { $_.usage.inputTokenCount } | Measure-Object -Sum).Sum
    cachedInputTokens = ($metadata | ForEach-Object { $_.usage.cachedInputTokenCount } | Measure-Object -Sum).Sum
    outputTokens = ($metadata | ForEach-Object { $_.usage.outputTokenCount } | Measure-Object -Sum).Sum
} | ConvertTo-Json -Depth 5 | Set-Content -Encoding utf8 "$root/verification/final-integrity.json"
