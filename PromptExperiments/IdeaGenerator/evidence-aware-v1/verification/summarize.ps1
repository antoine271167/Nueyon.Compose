$ErrorActionPreference='Stop'
$root=[IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..'))
$r=@(Get-Content "$root/scores/scored.json" -Raw | ConvertFrom-Json)
$q=@(Get-Content "$root/verification/quote-audit.json" -Raw | ConvertFrom-Json)
$calls=@(Get-Content "$root/verification/calls.json" -Raw | ConvertFrom-Json)
$summary=Get-Content "$root/scores/summary.json" -Raw | ConvertFrom-Json
$stats=[ordered]@{}
$stats.primaryRaw=foreach($c in @('A','B','C')){
 $set=@($r | Where-Object {$_.condition -eq $c -and $_.replicate -eq 1 -and $null -ne $_.rawVector})
 [pscustomobject]@{condition=$c;visible=$set.Count;rawPasses=@($set | Where-Object {$_.rawVector[0] -eq 2 -and $_.rawVector[1] -eq 2 -and $_.rawTotal -ge 8}).Count;meanRawTotal=($set | Measure-Object rawTotal -Average).Average;meanRawDistinctiveAppeal=($set | ForEach-Object {$_.rawVector[3]+$_.rawVector[4]} | Measure-Object -Average).Average}
}
$stats.quotes=foreach($rep in @(1,2)){foreach($c in @('B','C')){
 $set=@($q | Where-Object {$_.condition -eq $c -and $_.replicate -eq $rep})
 $visible=@($r | Where-Object {$_.condition -eq $c -and $_.replicate -eq $rep -and $null -ne $_.rawVector})
 $g=@($visible | ForEach-Object {$_.supportGrades})
 [pscustomobject]@{condition=$c;replicate=$rep;visibleOutputs=$visible.Count;quotes=($set | Measure-Object count -Sum).Sum;exact=($set | Measure-Object exactCount -Sum).Sum;fullSupport=@($g | Where-Object {$_ -eq 2}).Count;partialSupport=@($g | Where-Object {$_ -eq 1}).Count;unsupported=@($g | Where-Object {$_ -eq 0}).Count}
}}
$paired=@()
foreach($f in Get-ChildItem "$root/runs" -Filter final.json -Recurse | Where-Object {$_.Directory.Name -eq 'C'}){
 $c=Get-Content $f.FullName -Raw | ConvertFrom-Json
 if($c.callCount -eq 1){$b=Get-Content (Join-Path $f.Directory.Parent.FullName 'B/final.json') -Raw | ConvertFrom-Json;$paired+=[double]$b.durationMs+[double]$c.durationMs}
}
$stats.cReviewedPipelineMeanMs=($paired | Measure-Object -Average).Average
$stats.cAllPlannedPipelineMeanMs=((Get-ChildItem "$root/runs" -Filter final.json -Recurse | Where-Object {$_.Directory.Name -in @('B','C')} | ForEach-Object {(Get-Content $_.FullName -Raw | ConvertFrom-Json).durationMs} | Measure-Object -Sum).Sum/14)
$stats.perRepCalls=foreach($g in $calls | Group-Object replicate,condition){[pscustomobject]@{group=$g.Name;calls=$g.Count;input=($g.Group | Measure-Object input -Sum).Sum;output=($g.Group | Measure-Object output -Sum).Sum}}
$stats | ConvertTo-Json -Depth 12 | Set-Content "$root/verification/statistics.json" -Encoding utf8
$a=$summary.groups | Where-Object {$_.condition -eq 'A' -and $_.replicate -eq 1}
$gates=foreach($cond in @('B','C')){
 $p=$summary.groups | Where-Object {$_.condition -eq $cond -and $_.replicate -eq 1}
 $all=@($r | Where-Object condition -eq $cond)
 $critical=@($all | Where-Object {$_.id -in @('005','010')})
 $newCritical=@()
 foreach($item in $all | Where-Object replicate -eq 1){$base=$r | Where-Object {$_.id -eq $item.id -and $_.condition -eq 'A' -and $_.replicate -eq 1};if($base.pass -and $null -ne $item.rawVector -and ($item.rawVector[0] -eq 0 -or $item.rawVector[1] -eq 0)){$newCritical+=$item.id}}
 $checks=[ordered]@{primaryAtLeast8=($p.passes -ge 8);holdoutAtLeast7=($p.holdoutPasses -ge 7);originalsPassBothReplicates=(@($critical | Where-Object pass).Count -eq 4);atLeast3MorePrimaryThanA=($p.passes -ge $a.passes+3);noNewCriticalWhereAPasses=($newCritical.Count -eq 0);editorialMeanAtLeastA=($p.meanDistinctivePlusAppeal -ge $a.meanDistinctivePlusAppeal);appealAtLeast1Point5=($p.meanAppeal -ge 1.5);allPlannedCellsValid=(@($all | Where-Object valid).Count -eq 14)}
 if($cond -eq 'C'){
  $b=$summary.groups | Where-Object {$_.condition -eq 'B' -and $_.replicate -eq 1}
  $checks.atLeast2CompleteFirstIdeaRepairs=$false
  $checks.noHarmfulOrUnsupportedReview=$false
  $checks.atLeast2MorePrimaryThanB=($p.passes -ge $b.passes+2)
 }
 [ordered]@{condition=$cond;decision=if(@($checks.Values | Where-Object {!$_}).Count -eq 0){'GO'}else{'NO-GO'};checks=$checks;newCriticalCases=$newCritical;completeFirstIdeaGateRepairs=0;partialFirstIdeaNarrowingAttempts=if($cond -eq 'C'){1}else{0};note='Operational scores penalize invalid/blocked outputs; raw quality and scoring sensitivities are separate. No independently blinded human evaluator.'}
}
$gates | ConvertTo-Json -Depth 12 | Set-Content "$root/scores/gates.json" -Encoding utf8
$r | ForEach-Object {[pscustomobject]@{id=$_.id;condition=$_.condition;replicate=$_.replicate;status=$_.status;fidelity=$_.vector[0];intent=$_.vector[1];focus=$_.vector[2];distinctive=$_.vector[3];appeal=$_.vector[4];total=$_.total;pass=$_.pass;rawTotal=$_.rawTotal}} | Export-Csv "$root/scores/scores.csv" -NoTypeInformation -Encoding utf8
