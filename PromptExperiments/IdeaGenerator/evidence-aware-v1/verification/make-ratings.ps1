$ErrorActionPreference='Stop'
$root=[IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..'))
# Explicit editorial assessments, not model-generated ratings. Each vector is
# supported by scores/review-notes.md and the frozen full source and output.
$matrix=@(
 @('005','A',1,'02221',1,0,'00202'),
 @('005','B',1,'01011',1,1,'00102'),
 @('005','C',1,'',0,0,''),
 @('010','A',1,'12222',0,1,'22202'),
 @('010','B',1,'01121',0,1,'01102'),
 @('010','C',1,'',0,0,''),
 @('ceramics','A',1,'21221',0,0,'22222'),
 @('ceramics','B',1,'22112',0,0,'22222'),
 @('ceramics','C',1,'22112',0,0,'22222'),
 @('choir','A',1,'10121',0,0,'20002'),
 @('choir','B',1,'22112',0,0,'22222'),
 @('choir','C',1,'22112',0,0,'22222'),
 @('wetland','A',1,'12222',0,0,'22202'),
 @('wetland','B',1,'22122',0,0,'22222'),
 @('wetland','C',1,'',0,0,''),
 @('bakery','A',1,'11221',0,0,'20212'),
 @('bakery','B',1,'22212',0,0,'22222'),
 @('bakery','C',1,'22212',0,0,'22222'),
 @('textile','A',1,'22211',0,0,'22222'),
 @('textile','B',1,'12111',0,0,'21222'),
 @('textile','C',1,'12111',0,0,'21222'),
 @('observatory','A',1,'22211',0,0,'22222'),
 @('observatory','B',1,'10121',0,0,'02102'),
 @('observatory','C',1,'10121',0,0,'02102'),
 @('orchard','A',1,'22211',0,0,'22222'),
 @('orchard','B',1,'22121',0,0,'22222'),
 @('orchard','C',1,'',0,0,''),
 @('translation','A',1,'00221',1,1,'00002'),
 @('translation','B',1,'00121',0,1,'21102'),
 @('translation','C',1,'00121',0,1,'21102')
)
if(Test-Path "$root/scores/repeat-assessments.json"){
 $matrix+=@(Get-Content "$root/scores/repeat-assessments.json" -Raw | ConvertFrom-Json | ForEach-Object { ,@($_.id,$_.condition,2,$_.vector,[int]$_.causal,[int]$_.intervention,$_.support) })
}
$cases=@(Get-Content "$root/cases.json" -Raw | ConvertFrom-Json)
$labels=@(Get-Content "$root/labels/boundaries.json" -Raw | ConvertFrom-Json)
$rows=foreach($m in $matrix){
 $id=$m[0];$cond=$m[1];$rep=$m[2];$dir=Join-Path $root "runs/$id/$rep/$cond"
 $final=Get-Content "$dir/final.json" -Raw | ConvertFrom-Json
 $rawIdea=$null;$rawGrounding=@()
 if($final.ideas){$rawIdea=$final.ideas[0];$rawGrounding=if($final.grounding){@($final.grounding)}else{@()}}
 elseif($cond -eq 'B' -and (Test-Path "$dir/draft.json")){$text=Get-Content "$dir/draft.json" -Raw;if(![string]::IsNullOrWhiteSpace($text)){$d=$text | ConvertFrom-Json;if($d.ideas){$rawIdea=$d.ideas[0]};if($d.grounding){$rawGrounding=@($d.grounding)}}}
 elseif($cond -eq 'C' -and (Test-Path "$dir/reviewed.json")){$d=Get-Content "$dir/reviewed.json" -Raw | ConvertFrom-Json;$rawIdea=$d.revised.ideas[0];$rawGrounding=if($d.revised.grounding){@($d.revised.grounding)}else{@($d.grounding)}}
 $rawVector=if($m[3]){@($m[3].ToCharArray() | ForEach-Object {[int]::Parse([string]$_)})}else{$null}
 $support=if($m[6]){@($m[6].ToCharArray() | ForEach-Object {[int]::Parse([string]$_)})}else{$null}
 $valid=$final.status -eq 'success'
 $vector=if($valid){$rawVector}else{@(0,0,0,0,0)}
 $sourcePath=($cases | Where-Object id -eq $id).source
 $source=[IO.File]::ReadAllText((Join-Path $root $sourcePath))
 [ordered]@{
  id=$id;condition=$cond;replicate=$rep;valid=$valid;status=$final.status;vector=$vector;rawVector=$rawVector
  rawTotal=if($rawVector){($rawVector | Measure-Object -Sum).Sum}else{$null}
  causal=[int]$m[4];intervention=[int]$m[5];supportGrades=$support
  supportUnitDefinition='Principal proposition/grounding relationship per first-idea field, in title-description-audience-rationale-evidence order.2 full,1 partial,0 unsupported. Limited editorial audit, not exhaustive sentence coverage.'
  exactGroundingQuotes=@($rawGrounding | Where-Object {$_.sourceEvidence -and $source.Contains($_.sourceEvidence,[StringComparison]::Ordinal)}).Count
  groundingCount=$rawGrounding.Count;source=$sourcePath;result="runs/$id/$rep/$cond/final.json"
  sourcePurpose=($labels | Where-Object id -eq $id).purpose
  sourceBoundary=($labels | Where-Object id -eq $id).limits
  selectedIdea=$rawIdea;notes="scores/review-notes.md ($id replicate$rep)"
  evaluator='Work assistant source comparison, unblinded; no scoring-model calls/human reviewer'
 }
}
$rows | ConvertTo-Json -Depth 24 | Set-Content "$root/scores/ratings.json" -Encoding utf8
Write-Output "$($rows.Count) ratings written"
