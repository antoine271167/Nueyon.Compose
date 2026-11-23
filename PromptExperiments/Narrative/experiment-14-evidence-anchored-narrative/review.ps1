param([string]$Case)
$ErrorActionPreference = 'Stop'
$rows = Import-Csv (Join-Path $PSScriptRoot 'run-manifest.csv')
foreach ($row in ($rows | Where-Object { !$Case -or $_.case -eq $Case } | Sort-Object case, id)) {
    $payload = Join-Path $PSScriptRoot "runs/$($row.id).json"
    if (Test-Path $payload) {
        "=== $($row.case) $($row.id) ==="
        (Get-Content $payload -Raw -Encoding UTF8 | ConvertFrom-Json).content
    }
}
