param([Parameter(Mandatory)][ValidateSet('offline','tuning','holdout','repeat')][string]$Phase)
$ErrorActionPreference = 'Stop'
$root = $PSScriptRoot
Set-Location (Resolve-Path "$root/../../..")
$dll = "$root/harness/bin/Debug/net10.0/ValidatorAcceptance.dll"
$ledger = "$root/verification/call-ledger.jsonl"
if ($Phase -ne 'offline') {
    foreach ($entry in (Get-Content "$root/verification/frozen.json" -Raw | ConvertFrom-Json)) {
        if ((Get-FileHash -LiteralPath (Join-Path $root $entry.path)).Hash -ne $entry.sha256) {
            throw "Frozen artifact changed: $($entry.path)"
        }
    }
    if ((git rev-parse HEAD) -ne (Get-Content "$root/verification/head.txt")) { throw 'HEAD changed.' }
    if (!(Test-Path "$root/verification/offline.ok")) { throw 'Offline verification is required.' }
    if ($Phase -in 'holdout','repeat' -and !(Test-Path "$root/selection.md")) { throw 'Record selection first.' }
}
$out = "$root/runs/$Phase"
New-Item -ItemType Directory -Path $out -Force | Out-Null
$sets = switch ($Phase) { offline { 'tuning'; 'holdout' } holdout { 'holdout' } default { 'tuning' } }
foreach ($condition in 'baseline','candidate') {
    foreach ($set in $sets) {
        $manifest = "$root/$set/cases.json"
        $cases = Get-Content $manifest -Raw | ConvertFrom-Json
        if ($Phase -eq 'repeat') { $cases = $cases | Where-Object id -in '005-b','010-b' }
        foreach ($case in $cases) {
            $key = "$Phase/$condition/$($case.id)"
            $prefix = "$out/$condition-$($case.id)"
            $mode = if ($Phase -eq 'offline') { 'offline' } else { 'live' }
            $arguments = @($dll, $mode, "$root/prompts/$condition", $manifest, $case.id, $prefix)
            if ($mode -eq 'live') {
                $reserved = @(if (Test-Path $ledger) {
                    Get-Content $ledger | ForEach-Object { $_ | ConvertFrom-Json } | Where-Object event -eq 'reserved'
                })
                if ($key -in $reserved.key) { continue }
                if ($reserved.Count -ge 28) { throw 'Call cap reached.' }
                @{ event = 'reserved'; key = $key; utc = [DateTime]::UtcNow.ToString('o'); alias = 'gpt-5-mini'
                    executable = 'dotnet'; arguments = $arguments } | ConvertTo-Json -Compress | Add-Content $ledger
            }
            & dotnet @arguments *> "$prefix.log"
            $exit = $LASTEXITCODE
            if ($mode -eq 'live') {
                @{ event = 'completed'; key = $key; utc = [DateTime]::UtcNow.ToString('o'); exit = $exit
                    requestPrepared = Test-Path "$prefix.input.json"; responseSaved = Test-Path "$prefix.verdict.json" } |
                    ConvertTo-Json -Compress | Add-Content $ledger
                if (!(Test-Path "$prefix.input.json")) { throw "Live preflight failed. See $prefix.log." }
            } elseif ($exit -ne 0) {
                throw "Offline preflight failed. See $prefix.log."
            }
        }
    }
}
if ($Phase -eq 'offline') { 'All 24 probes passed' | Set-Content "$root/verification/offline.ok" }
