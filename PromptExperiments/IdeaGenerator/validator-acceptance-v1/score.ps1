$ErrorActionPreference = 'Stop'
$root = $PSScriptRoot
$results = foreach ($phase in 'tuning','holdout','repeat') {
    $set = if ($phase -eq 'holdout') { 'holdout' } else { 'tuning' }
    $cases = Get-Content "$root/$set/cases.json" -Raw | ConvertFrom-Json
    if ($phase -eq 'repeat') { $cases = $cases | Where-Object id -in '005-b','010-b' }
    foreach ($condition in 'baseline','candidate') {
        foreach ($case in $cases) {
            $prefix = "$root/runs/$phase/$condition-$($case.id)"
            if (!(Test-Path "$prefix.log")) { continue }
            $verdict = $null
            if ((Test-Path "$prefix.verdict.json") -and (Select-String -Quiet -Path "$prefix.log" -Pattern '^Valid verdict')) {
                $verdict = Get-Content "$prefix.verdict.json" -Raw | ConvertFrom-Json
            }
            $label = if ($null -eq $verdict) { 'error' } elseif ($verdict.accepted) {
                if ($case.expectedAccepted) { 'TN' } else { 'FN' }
            } else {
                if ($case.expectedAccepted) { 'FP' } else { 'TP' }
            }
            [pscustomobject]@{ phase=$phase; condition=$condition; case=$case.id; expectedAccepted=$case.expectedAccepted
                result=$label; accepted=$verdict.accepted; blockers=@($verdict.blockingIssues | Where-Object { $null -ne $_ }) }
        }
    }
}
$groups = $results | Group-Object phase,condition | ForEach-Object {
    $tp = @($_.Group | Where-Object result -eq TP).Count
    $fp = @($_.Group | Where-Object result -eq FP).Count
    $tn = @($_.Group | Where-Object result -eq TN).Count
    $fn = @($_.Group | Where-Object result -eq FN).Count
    [pscustomobject]@{ phase=$_.Group[0].phase; condition=$_.Group[0].condition; tp=$tp; fp=$fp; tn=$tn; fn=$fn
        errors=@($_.Group | Where-Object result -eq error).Count
        precision=if (($tp+$fp) -gt 0) { $tp/($tp+$fp) }
        recall=if (($tp+$fn) -gt 0) { $tp/($tp+$fn) } }
}
@{ metrics=@($groups); cases=@($results) } | ConvertTo-Json -Depth 12 | Set-Content -Encoding utf8 "$root/verification/scores.json"
$groups | Format-Table
