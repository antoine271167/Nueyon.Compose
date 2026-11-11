$ErrorActionPreference='Stop'
$root=$PSScriptRoot
$ratings=@(Get-Content "$root/scores/ratings.json" -Raw | ConvertFrom-Json)
if($ratings.Count -ne 42){throw "Expected42 ratings, got $($ratings.Count)"}
$keys=@{}
foreach($r in $ratings){
 $key="$($r.id)-$($r.condition)-$($r.replicate)"
 if($keys.ContainsKey($key)){throw "Duplicate $key"};$keys[$key]=$true
 if($r.vector.Count -ne 5 -or @($r.vector | Where-Object {$_ -notin @(0,1,2)}).Count){throw "Invalid score $key"}
 $total=($r.vector | Measure-Object -Sum).Sum
 $pass=($r.valid -and $r.vector[0] -eq 2 -and $r.vector[1] -eq 2 -and $total -ge 8)
 $r | Add-Member -NotePropertyName total -NotePropertyValue $total -Force
 $r | Add-Member -NotePropertyName pass -NotePropertyValue $pass -Force
}
$summary=@()
foreach($rep in @(1,2)){foreach($c in @('A','B','C')){
 $set=@($ratings | Where-Object {$_.condition -eq $c -and $_.replicate -eq $rep})
 $hold=@($set | Where-Object {$_.id -notin @('005','010')})
 $appeal=($set | ForEach-Object {$_.vector[4]} | Measure-Object -Average).Average
 $editorial=($set | ForEach-Object {$_.vector[3]+$_.vector[4]} | Measure-Object -Average).Average
 $summary += [ordered]@{condition=$c;replicate=$rep;n=$set.Count;passes=@($set | Where-Object pass).Count;holdoutPasses=@($hold | Where-Object pass).Count;holdoutN=$hold.Count;meanTotal=($set | Measure-Object total -Average).Average;meanAppeal=$appeal;meanDistinctivePlusAppeal=$editorial;valid=@($set | Where-Object valid).Count;causalOverclaims=($set | Measure-Object causal -Sum).Sum;interventionOverclaims=($set | Measure-Object intervention -Sum).Sum}
}}
$pairs=@()
foreach($c in @('B','C')){
 $a=@($ratings | Where-Object {$_.replicate -eq 1 -and $_.condition -eq 'A'})
 $wins=0;$losses=0
 foreach($r in $a){$other=$ratings | Where-Object {$_.id -eq $r.id -and $_.condition -eq $c -and $_.replicate -eq 1};if($other.pass -and !$r.pass){$wins++};if($r.pass -and !$other.pass){$losses++}}
 $pairs += @{candidate=$c;passWins=$wins;passLosses=$losses;ties=10-$wins-$losses}
}
$ratings | ConvertTo-Json -Depth 30 | Set-Content "$root/scores/scored.json" -Encoding utf8
[ordered]@{groups=$summary;pairedVsA=$pairs;note='Descriptive within-set judgments; no independent human blinding or statistical superiority claim.'} | ConvertTo-Json -Depth 14 | Set-Content "$root/scores/summary.json" -Encoding utf8
$summary | ForEach-Object {[pscustomobject]$_} | Format-Table
