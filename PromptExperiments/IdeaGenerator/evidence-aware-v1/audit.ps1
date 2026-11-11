$ErrorActionPreference='Stop'
$root=$PSScriptRoot
$frozen=Get-Content "$root/verification/frozen.json" -Raw | ConvertFrom-Json -AsHashtable
$mismatch=@()
foreach($p in $frozen.files.Keys){$full=[IO.Path]::GetFullPath((Join-Path $root $p));if(!(Test-Path -LiteralPath $full) -or (Get-FileHash -LiteralPath $full -Algorithm SHA256).Hash.ToLowerInvariant() -cne $frozen.files[$p]){$mismatch+=$p}}
$entries=@();if(Test-Path "$root/verification/call-ledger.jsonl"){$entries=@(Get-Content "$root/verification/call-ledger.jsonl" | Where-Object {$_} | ForEach-Object {$_ | ConvertFrom-Json})}
$finals=@(Get-ChildItem "$root/runs" -Filter final.json -Recurse -ErrorAction SilentlyContinue)
$started=@(Get-ChildItem "$root/runs" -Filter started.json -Recurse -ErrorAction SilentlyContinue)
$incomplete=@($started | Where-Object {!(Test-Path (Join-Path $_.DirectoryName 'final.json'))} | ForEach-Object {$_.DirectoryName})
$head=(git rev-parse HEAD)
$result=[ordered]@{utc=[DateTime]::UtcNow.ToString('o');head=$head;headUnchanged=($head -ceq $frozen.head);frozenFiles=$frozen.files.Count;mismatches=$mismatch;reservedCalls=$entries.Count;budget=112;withinBudget=($entries.Count -le 112);finalCells=$finals.Count;startedCells=$started.Count;incompleteCells=$incomplete;gitStatus=@(git status --short);productionDiff=@(git diff --name-only -- src | Where-Object {$_ -ne 'src/Nueyon.Compose.slnx'})}
$result | ConvertTo-Json -Depth 14 | Set-Content "$root/verification/integrity.json" -Encoding utf8
$result | ConvertTo-Json -Depth 8
if($mismatch.Count -gt 0 -or $head -cne $frozen.head){throw 'Frozen integrity failed'}
