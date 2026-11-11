$ErrorActionPreference='Stop'
$root=[IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..'))
$repo=[IO.Path]::GetFullPath((Join-Path $root '../../..'))
Set-Location $repo
$reportPath=Join-Path $root 'report.md'
$report=[IO.File]::ReadAllText($reportPath)
$replacements=@(@('A3/10','A 3/10'),@('B3/10','B 3/10'),@('C2/10','C 2/10'),@('A4/10','A 4/10'),@('B5/10','B 5/10'),@('A005','A 005'),@('B005','B 005'),@('B010','B 010'),@('C005','C 005'),@('CLI1.0.109','CLI 1.0.109'),@('Haiku4.5','Haiku 4.5'),@('Alle54','Alle 54'),@('Alle30','Alle 30'),@('6000','6.000'),@('replicaat2','replicaat 2'),@('alternatief2 of3','alternatief 2 of 3'))
foreach($pair in $replacements){$report=$report.Replace($pair[0],$pair[1])}
[IO.File]::WriteAllText($reportPath,$report)
[ordered]@{phase='complete';decisionB='NO-GO';decisionC='NO-GO';head=(git rev-parse HEAD);plannedCells=42;completedCells=42;reservedCalls=54;budget=112;unused=58;productionPromoted=$false;committed=$false;pushed=$false;resume='Completed cells skip; never delete ledger/started markers; see README and protocol.';utc=[DateTime]::UtcNow.ToString('o')} | ConvertTo-Json | Set-Content "$root/verification/session-state.json" -Encoding utf8
[ordered]@{recordedUtc=[DateTime]::UtcNow.ToString('o');kind='post-run-verification';commands=@('verification/make-ratings.ps1','score.ps1','verification/analyze.ps1','verification/summarize.ps1','audit.ps1','git diff --check','git diff --cached --stat');resumeProbe='dotnet run --project src/Nueyon.Compose.Experiments --no-build -- run <root> 005 B 1';result='All complete; resume skipped with54 reservations before and after; no model call'} | ConvertTo-Json -Compress | Add-Content "$root/verification/command-log.jsonl" -Encoding utf8
& "$root/audit.ps1"
$links=[regex]::Matches([IO.File]::ReadAllText($reportPath),'\]\(([^)]+)\)')
$missing=@()
foreach($match in $links){
 $target=$match.Groups[1].Value
 if($target -notmatch '^https?://' -and $target -ne 'verification/final-manifest.json' -and !(Test-Path -LiteralPath ([IO.Path]::GetFullPath((Join-Path $root $target))))){$missing+=$target}
}
if($missing.Count){throw "Missing report links: $missing"}
$syntax=@()
foreach($file in Get-ChildItem $root -Filter *.ps1 -Recurse){
 $errors=$null;$tokens=$null
 [System.Management.Automation.Language.Parser]::ParseFile($file.FullName,[ref]$tokens,[ref]$errors) | Out-Null
 if($errors){$syntax+=@{file=$file.FullName;errors=@($errors.Message)}}
}
if($syntax.Count){throw 'Analysis script parse error'}
[ordered]@{reportLinksChecked=$links.Count;missingLinks=$missing;scriptSyntaxErrors=$syntax;utc=[DateTime]::UtcNow.ToString('o')} | ConvertTo-Json -Depth 8 | Set-Content "$root/verification/final-checks.json" -Encoding utf8
$manifestFiles=[ordered]@{}
foreach($file in Get-ChildItem $root -Recurse -File | Where-Object {$_.Name -notin @('final-manifest.json','run.lock')}){
 $relative=[IO.Path]::GetRelativePath($root,$file.FullName).Replace('\','/')
 $manifestFiles[$relative]=(Get-FileHash -LiteralPath $file.FullName -Algorithm SHA256).Hash.ToLowerInvariant()
}
[ordered]@{createdUtc=[DateTime]::UtcNow.ToString('o');head=(git rev-parse HEAD);files=$manifestFiles;frozenRepositoryAndBinaryFiles='verification/frozen.json';note='Final manifest excludes itself and run.lock. No further writes required.'} | ConvertTo-Json -Depth 12 | Set-Content "$root/verification/final-manifest.json" -Encoding utf8
if(!(Test-Path "$root/verification/final-manifest.json")){throw 'Final manifest missing'}
[pscustomobject]@{artifactFiles=$manifestFiles.Count;reportLinksChecked=$links.Count;scriptErrors=$syntax.Count;head=(git rev-parse HEAD);stagedFiles=@(git diff --cached --name-only).Count} | ConvertTo-Json
