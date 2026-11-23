$ErrorActionPreference = 'Stop'
$root = $PSScriptRoot
foreach ($line in Get-Content "$root/freeze.sha256") {
    $hash, $relative = $line -split '  ', 2
    $path = Join-Path $root $relative
    if ((Get-FileHash $path).Hash -ne $hash) {
        throw "Frozen file differs: $relative"
    }
}
$manifest = @(Import-Csv "$root/run-manifest.csv")
if ($manifest.Count -ne 36 -or ($manifest.id | Sort-Object -Unique).Count -ne 36) {
    throw 'Expected 36 unique scheduled slots.'
}
foreach ($cell in ($manifest | Group-Object case, variant)) {
    if ((($cell.Group.repeat | Sort-Object) -join ',') -ne '1,2,3') {
        throw "Invalid repetitions: $($cell.Name)"
    }
}
foreach ($row in $manifest) {
    $prefix = Join-Path $root "runs/$($row.id)"
    $metadata = Get-Content "$prefix.status.json" -Raw -Encoding UTF8 | ConvertFrom-Json
    $payload = Get-Content "$prefix.json" -Raw -Encoding UTF8 | ConvertFrom-Json
    foreach ($key in 'id', 'case', 'variant', 'repeat') {
        if ($metadata.$key -ne $row.$key) { throw "Manifest mismatch: $prefix / $key" }
    }
    if ($metadata.exitCode -ne 0 -or !$metadata.resultExists -or
        $metadata.model -ne 'gpt-6-luna' -or $metadata.elapsedMs -le 0 -or
        [string]::IsNullOrWhiteSpace($payload.content)) {
        throw "Incomplete slot: $prefix"
    }
    $log = Get-Content "$prefix.log" -Raw -Encoding UTF8
    if (!$log.Contains("\$($row.variant)).")) { throw "Prompt path mismatch: $prefix" }
}
'Frozen hashes verified. 36 original CLI payloads match the manifest, with three successful runs per case and variant.'
