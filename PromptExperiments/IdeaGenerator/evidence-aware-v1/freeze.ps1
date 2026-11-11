$ErrorActionPreference='Stop'
$root=$PSScriptRoot
if(Test-Path "$root/verification/frozen.json"){throw 'Freeze already exists; never regenerate'}
$files=[ordered]@{}
$items=Get-ChildItem $root -Recurse -File | Where-Object { $_.FullName -notmatch '[\\/](verification|runs|scores)[\\/]' }
$items+=@(Get-Item "$root/verification/evidence.schema.json","$root/verification/review.schema.json")
$items+=@(Get-ChildItem src/Nueyon.Compose.Experiments/bin/Debug/net10.0 -File | Where-Object {$_.Name -match '\.(dll|deps\.json|runtimeconfig\.json)$'})
foreach($f in $items){$rel=[IO.Path]::GetRelativePath($root,$f.FullName).Replace('\','/');$files[$rel]=(Get-FileHash -LiteralPath $f.FullName -Algorithm SHA256).Hash.ToLowerInvariant()}
$paths=@(git ls-files src)
$paths+=@(Get-ChildItem src/Nueyon.Compose.Experiments,src/Nueyon.Compose.Experiments.Tests -Recurse -File | Where-Object {$_.FullName -notmatch '[\\/](bin|obj)[\\/]'} | ForEach-Object {$_.FullName})
foreach($p in ($paths | Sort-Object -Unique)){
 $f=Get-Item -LiteralPath $p
 $rel=[IO.Path]::GetRelativePath($root,$f.FullName).Replace('\','/')
 $files[$rel]=(Get-FileHash -LiteralPath $f.FullName -Algorithm SHA256).Hash.ToLowerInvariant()
}
[ordered]@{files=$files;head=(git rev-parse HEAD);budget=112;frozenUtc=[DateTime]::UtcNow.ToString('o')} | ConvertTo-Json -Depth 12 | Set-Content "$root/verification/frozen.json" -Encoding utf8
Write-Output "Frozen $($files.Count) files"
