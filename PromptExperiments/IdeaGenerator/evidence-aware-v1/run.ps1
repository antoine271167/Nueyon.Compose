param([ValidateSet('primary','repeat','all')] [string]$Phase='all')
$ErrorActionPreference='Stop'
$root=$PSScriptRoot
$repo=[IO.Path]::GetFullPath((Join-Path $root '../../..'))
Set-Location $repo
$cases=@(Get-Content "$root/cases.json" -Raw | ConvertFrom-Json)
$tasks=@()
if($Phase -in @('primary','all')){
 for($i=0;$i -lt $cases.Count;$i++){
  $order=if($i%2 -eq 0){@('A','B','C')}else{@('B','A','C')}
  foreach($cond in $order){$tasks+=@{id=$cases[$i].id;condition=$cond;replicate=1}}
 }
}
if($Phase -in @('repeat','all')){foreach($id in @('005','010','ceramics','choir')){foreach($cond in @('B','A','C')){$tasks+=@{id=$id;condition=$cond;replicate=2}}}}
foreach($t in $tasks){
 $started=[DateTime]::UtcNow.ToString('o')
 $cmd=@('run','--project','src/Nueyon.Compose.Experiments','--no-build','--','run',$root,$t.id,$t.condition,[string]$t.replicate)
 [ordered]@{utc=$started;command='dotnet';args=$cmd} | ConvertTo-Json -Compress | Add-Content "$root/verification/command-log.jsonl" -Encoding utf8
 $log="$root/verification/run-$($t.id)-$($t.condition)-$($t.replicate).log"
 & dotnet @cmd *> $log
 $exit=$LASTEXITCODE
 [ordered]@{utc=[DateTime]::UtcNow.ToString('o');id=$t.id;condition=$t.condition;replicate=$t.replicate;exitCode=$exit;log=$log} | ConvertTo-Json -Compress | Add-Content "$root/verification/command-log.jsonl" -Encoding utf8
 if($exit -ne 0){
  $final=Join-Path $root "runs/$($t.id)/$($t.replicate)/$($t.condition)/final.json"
  if(!(Test-Path $final)){throw "Runner exited $exit without final state for $($t.id)/$($t.condition)/$($t.replicate). Audit before resuming."}
  Write-Output "$($t.id) $($t.condition) $($t.replicate) failed/blocked; retained, no retry"
 }
 Write-Output "$($t.id) $($t.condition) $($t.replicate) complete"
}
