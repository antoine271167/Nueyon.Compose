$ErrorActionPreference='Stop'
$root=[IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..'))
$cases=@(Get-Content "$root/cases.json" -Raw | ConvertFrom-Json)
$ledger=@(Get-Content "$root/verification/call-ledger.jsonl" | ForEach-Object {$_ | ConvertFrom-Json})
$calls=@();$missing=@();$errors=@();$checks=@()
foreach($entry in $ledger){
 $parts=$entry.cell.Split('|');$id=$parts[0];$cond=$parts[1];$rep=[int]$parts[2]
 $dir=Join-Path $root "runs/$id/$rep/$cond/call-$('{0:D3}' -f $entry.ordinal)"
 $req=Get-Content "$dir/request.json" -Raw | ConvertFrom-Json
 $case=$cases | Where-Object id -eq $id
 $source=[IO.File]::ReadAllText((Join-Path $root $case.source))
 $schema=$req.options.responseFormat.schema
 $judge=$null -ne $schema.properties.accepted
 $fields=@($schema.properties.PSObject.Properties.Name)
 $expectedFields=if($cond -eq 'A'){if($judge){@('accepted','blockingIssues')}else{@('ideas')}}elseif($cond -eq 'B'){@('ideas','grounding')}else{@('review','revised')}
 $schemaCorrect=((($fields | Sort-Object) -join '|') -ceq (($expectedFields | Sort-Object) -join '|'))
 if($cond -in @('B','C')){
  $schemaFile=if($cond -eq 'B'){'evidence.schema.json'}else{'review.schema.json'}
  $expectedSchema=Get-Content (Join-Path $root "verification/$schemaFile") -Raw | ConvertFrom-Json
  $schemaCorrect=$schemaCorrect -and (($schema | ConvertTo-Json -Depth 70 -Compress) -ceq ($expectedSchema | ConvertTo-Json -Depth 70 -Compress))
 }
 $prompt=if($cond -eq 'A'){if($judge){'baseline/Idea/IdeaValidation.system.prompt.txt'}else{'baseline/Idea/Idea.system.prompt.txt'}}elseif($cond -eq 'B'){'evidence.system.txt'}else{'review.system.txt'}
 $instruction=[IO.File]::ReadAllText((Join-Path $root "prompts/$prompt"))
 $users=@($req.messages | Where-Object role -eq 'user' | ForEach-Object {$_.contents | Where-Object '$type' -eq 'text' | ForEach-Object text})
 $sourceExact=if($judge){(($users[0] | ConvertFrom-Json).source -ceq $source)}else{$users[0].Contains($source,[StringComparison]::Ordinal)}
 $draftExact=$true
 if($cond -eq 'C'){$draft=[IO.File]::ReadAllText((Join-Path $root "runs/$id/$rep/B/draft.json"));$draftExact=$users[0].Contains($draft,[StringComparison]::Ordinal)}
 $checks += @{ordinal=$entry.ordinal;cell=$entry.cell;max6000=($req.options.maxOutputTokens -eq 6000);temperatureUnset=($null -eq $req.options.temperature);systemExact=($req.options.instructions -ceq $instruction);sourceExact=$sourceExact;draftExact=$draftExact;schemaCorrect=$schemaCorrect;schemaFields=$fields}
 if(Test-Path "$dir/metadata.json"){
  $m=Get-Content "$dir/metadata.json" -Raw | ConvertFrom-Json
  $calls += [pscustomobject]@{ordinal=$entry.ordinal;id=$id;condition=$cond;replicate=$rep;model=$m.modelId;input=[long]$m.usage.inputTokenCount;cached=[long]$m.usage.cachedInputTokenCount;output=[long]$m.usage.outputTokenCount;reasoning=[long]$m.usage.reasoningTokenCount;latencyMs=[long]$m.durationMs;finish=$m.finishReason}
 }elseif(Test-Path "$dir/error.json"){$errors+=$dir}else{$missing+=$dir}
}
$usage=@()
foreach($c in @('A','B','C')){
 $set=@($calls | Where-Object condition -eq $c)
 $cells=@(Get-ChildItem "$root/runs" -Filter final.json -Recurse | Where-Object {$_.Directory.Name -eq $c} | ForEach-Object {Get-Content $_.FullName -Raw | ConvertFrom-Json})
 $usage += [ordered]@{condition=$c;calls=$set.Count;input=($set | Measure-Object input -Sum).Sum;cached=($set | Measure-Object cached -Sum).Sum;output=($set | Measure-Object output -Sum).Sum;reasoning=($set | Measure-Object reasoning -Sum).Sum;apiLatencySumMs=($set | Measure-Object latencyMs -Sum).Sum;meanCellMs=($cells | Measure-Object durationMs -Average).Average;statuses=@($cells | Group-Object status | Select-Object Name,Count)}
}
$quoteAudit=@();$reviews=@()
foreach($file in Get-ChildItem "$root/runs" -Recurse -Filter final.json){
 $cond=$file.Directory.Name;$rep=[int]$file.Directory.Parent.Name;$id=$file.Directory.Parent.Parent.Name
 if($cond -eq 'A'){continue}
 $source=[IO.File]::ReadAllText((Join-Path $root ($cases | Where-Object id -eq $id).source))
 $path=Join-Path $file.DirectoryName $(if($cond -eq 'B'){'draft.json'}else{'reviewed.json'})
 if(!(Test-Path $path)){continue}
 $raw=Get-Content $path -Raw | ConvertFrom-Json
 $draft=if($cond -eq 'B'){$raw}else{$raw.revised}
 $records=@()
 $groundingLocation=if($draft.grounding){'schema-location'}elseif($cond -eq 'C' -and $raw.grounding){'root-invalid-location'}else{'missing'}
 $grounding=if($draft.grounding){@($draft.grounding)}elseif($cond -eq 'C' -and $raw.grounding){@($raw.grounding)}else{@()}
 foreach($g in $grounding){
  $exact=$source.Contains($g.sourceEvidence,[StringComparison]::Ordinal)
  $space=([regex]::Replace($source,'\s+',' ')).Contains(([regex]::Replace($g.sourceEvidence,'\s+',' ')),[StringComparison]::Ordinal)
  $records+=@{field=$g.field;exact=$exact;whitespaceNormalized=$space;quote=$g.sourceEvidence;claim=$g.claim}
 }
 $quoteAudit+=@{id=$id;condition=$cond;replicate=$rep;groundingLocation=$groundingLocation;records=$records;exactCount=@($records | Where-Object exact).Count;count=$records.Count}
 if($cond -eq 'C'){
  $b=Get-Content (Join-Path $root "runs/$id/$rep/B/draft.json") -Raw | ConvertFrom-Json
  $changed=@();foreach($field in @('title','description','audience','rationale','evidence')){if($b.ideas[0].$field -cne $raw.revised.ideas[0].$field){$changed+=$field}}
  $reviews+=@{id=$id;replicate=$rep;reviewIssues=@($raw.review).Count;changedFirstFields=$changed;selectedUnchanged=($changed.Count -eq 0);review=$raw.review}
 }
}
$calls | ConvertTo-Json -Depth 8 | Set-Content "$root/verification/calls.json" -Encoding utf8
$checks | ConvertTo-Json -Depth 8 | Set-Content "$root/verification/transport-audit.json" -Encoding utf8
$quoteAudit | ConvertTo-Json -Depth 12 | Set-Content "$root/verification/quote-audit.json" -Encoding utf8
$reviews | ConvertTo-Json -Depth 12 | Set-Content "$root/verification/review-diffs.json" -Encoding utf8
[ordered]@{reserved=$ledger.Count;completed=$calls.Count;transportErrors=$errors;missingResponses=$missing;models=@($calls.model | Sort-Object -Unique);groups=$usage;totalInput=($calls | Measure-Object input -Sum).Sum;totalCached=($calls | Measure-Object cached -Sum).Sum;totalOutput=($calls | Measure-Object output -Sum).Sum;totalReasoning=($calls | Measure-Object reasoning -Sum).Sum;apiLatencySumMs=($calls | Measure-Object latencyMs -Sum).Sum;transportChecksPassed=(@($checks | Where-Object {!$_.max6000 -or !$_.temperatureUnset -or !$_.systemExact -or !$_.sourceExact -or !$_.draftExact -or !$_.schemaCorrect}).Count -eq 0)} | ConvertTo-Json -Depth 12 | Set-Content "$root/verification/usage.json" -Encoding utf8
