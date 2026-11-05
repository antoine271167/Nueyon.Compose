#requires -Version 5.1
# Deterministic smoke tests for Nueyon.PromptRunner. No network, API key or LLM call is needed.
# Usage: powershell -File ./smoke-tests.ps1 [-NoBuild]   (also works with pwsh)
# Builds once, then runs the compiled DLL directly for every case (never `dotnet run`).
param([switch]$NoBuild)
$ErrorActionPreference = 'Stop'
$proj = $PSScriptRoot
if (-not $NoBuild) {
	dotnet build $proj -c Debug -v q --nologo | Out-Null
	if ($LASTEXITCODE -ne 0) { Write-Host 'Build failed'; exit 1 }
}
$found = Get-ChildItem (Join-Path $proj 'bin/Debug') -Recurse -Filter 'Nueyon.PromptRunner.dll' | Select-Object -First 1
if (-not $found) { Write-Host 'Built dll not found'; exit 1 }
$dll = $found.FullName
Remove-Item Env:OPENAI_API_KEY -ErrorAction SilentlyContinue

$tmp = Join-Path ([IO.Path]::GetTempPath()) ("promptrunner-" + [guid]::NewGuid())
New-Item -ItemType Directory $tmp | Out-Null
$emptyDir = Join-Path $tmp 'empty'
New-Item -ItemType Directory $emptyDir | Out-Null
$script:fail = 0

# Runs the compiled app; optional working directory. Captures stdout, stderr and exit code separately.
function Run([string[]]$a, [string]$cwd = $emptyDir) {
	$ErrorActionPreference = 'Continue'
	$errFile = Join-Path $tmp 'err.txt'
	Push-Location $cwd
	try { $o = & dotnet $dll @a 2>$errFile; $code = $LASTEXITCODE } finally { Pop-Location }
	$e = ''
	if (Test-Path $errFile) { $e = [IO.File]::ReadAllText($errFile) }
	New-Object psobject -Property @{ Code = $code; Out = (@($o) -join "`n"); Err = $e }
}
function Check([string]$name, [bool]$ok, $r) {
	if ($ok) { Write-Host "PASS $name" }
	else { $script:fail++; Write-Host "FAIL $name (code=$($r.Code))`n  out: $($r.Out)`n  err: $($r.Err)" }
}
function W([string]$name, [string]$text) {
	$p = Join-Path $tmp $name
	[IO.File]::WriteAllText($p, $text)
	$p
}

try {
	$sys = W 's.md' "Sys {input.A.Title} {{literal}} {{input.A.Title}}`n{`"a`": 1}`n{`n  `"count`": 3`n}"
	$usr = W 'u.md' "S={input.A.Title} I={input.n} F={input.f} B={input.b} E={input.e}`n{input.A.Content}"
	$yml = W 'd.yaml' ("input:`n  n: 42`n  f: 1.50`n  b: true`n  e: 1e3`n  nul:`n  list: [1, 2]`n  A:`n    Title: Hello`n    Content: |`n      First.`n`n      Second.`n")
	$base = @('--system', $sys, '--user', $usr, '--data', $yml)

	# CLI and help
	foreach ($h in '--help', '--help-templates', '--example') {
		$r = Run @($h); Check "$h works without API key" (($r.Code -eq 0) -and ($r.Out.Length -gt 50)) $r
	}
	$r = Run @('--help'); Check '--help documents --config' ($r.Out -match '--config') $r
	$r = Run ($base + '--bogus'); Check 'unknown argument exit 1' ($r.Code -eq 1) $r
	$r = Run ($base + @('--system', $sys, '--validate')); Check 'duplicate argument exit 1' (($r.Code -eq 1) -and ($r.Err -match 'Duplicate')) $r
	$r = Run @('--system', $sys, '--user', $usr); Check 'missing required argument exit 1' ($r.Code -eq 1) $r
	$r = Run ($base + @('--validate', '--print-rendered')); Check 'validate + print-rendered exit 1' ($r.Code -eq 1) $r
	$r = Run ($base + 'stray'); Check 'positional argument exit 1' ($r.Code -eq 1) $r

	# Template rendering
	$r = Run ($base + '--print-rendered')
	Check 'nested placeholders resolve in user prompt' (($r.Code -eq 0) -and ($r.Out -match 'S=Hello ')) $r
	Check 'placeholders resolve in system prompt' ($r.Out -match 'Sys Hello ') $r
	Check 'escaped braces stay literal' ($r.Out -match '\{literal\}') $r
	Check 'escaped placeholder is not resolved' ($r.Out -match '\{input\.A\.Title\}') $r
	Check 'literal JSON intact' (($r.Out -match '\{"a": 1\}') -and ($r.Out -match '"count": 3')) $r

	# YAML values
	Check 'integer renders' ($r.Out -match 'I=42 ') $r
	Check 'float renders as deserialized text' ($r.Out -match 'F=1\.50 ') $r
	Check 'boolean renders as deserialized text' ($r.Out -match 'B=true ') $r
	Check 'exponent scalar renders as deserialized text' ($r.Out -match 'E=1e3') $r
	Check 'multiline string preserved' ($r.Out -match "First\.\s*\n\s*\n\s*Second\.") $r
	$badYaml = W 'bad.yaml' "input: [unclosed`n  x: : :"
	$r = Run @('--system', $sys, '--user', $usr, '--data', $badYaml, '--validate'); Check 'invalid YAML exit 1' ($r.Code -eq 1) $r
	$r = Run @('--system', "$tmp/none.md", '--user', $usr, '--data', $yml, '--validate'); Check 'missing file exit 1' ($r.Code -eq 1) $r

	# Escape regression: assert the exact rendered user prompt.
	$escYaml = W 'esc.yaml' "input:`n  Title: Hello`n  B: World`n"
	$escSys = W 'esc.sys.md' 'x'
	$escCases = @(
		@('{input.Title}', 'Hello'),
		@('{{input.Title}}', '{input.Title}'),
		@('{{literal}}', '{literal}'),
		@('{{{input.Title}}}', '{Hello}'),
		@('{{"title": "example"}}', '{"title": "example"}'),
		@('{"title": "example"}', '{"title": "example"}'),
		@('{input.Title}{input.B}', 'HelloWorld'),
		@('{{a}} {{b}} {input.Title}', '{a} {b} Hello'),
		@('{"k": "{input.Title}"}', '{"k": "Hello"}'),
		@('{{{{x}}}}', '{{x}}')
	)
	$ei = 0
	foreach ($ec in $escCases) {
		$ei++
		$eu = W "esc$ei.md" $ec[0]
		$r = Run @('--system', $escSys, '--user', $eu, '--data', $escYaml, '--print-rendered')
		$parts = $r.Out -split "===== USER =====`n"
		$actual = ''
		if ($parts.Count -gt 1) { $actual = ($parts[1] -split "`n===== END =====")[0].TrimEnd("`r", "`n") }
		Check "escape exact: $($ec[0]) => $($ec[1])" (($r.Code -eq 0) -and ($actual -ceq $ec[1])) $r
	}

	# Rendering errors: exit 2, filename, line number, offending expression.
	$cases = @(
		@{ n = 'missing'; t = '{input.nope}'; m = '\{input\.nope\}' },
		@{ n = 'null'; t = '{input.nul}'; m = '\{input\.nul\}' },
		@{ n = 'mapping'; t = '{input.A}'; m = '\{input\.A\}' },
		@{ n = 'list'; t = '{input.list}'; m = '\{input\.list\}' },
		@{ n = 'bang'; t = '{input.Title!}'; m = "'\{input\.Title!\}'" },
		@{ n = 'double-dot'; t = '{input..Title}'; m = "'\{input\.\.Title\}'" },
		@{ n = 'unclosed'; t = '{input.Title'; m = "'\{input\.Title'" },
		@{ n = 'dash'; t = '{input.Title-name}'; m = "'\{input\.Title-name\}'" },
		@{ n = 'empty'; t = '{}'; m = "'\{\}'" }
	)
	foreach ($c in $cases) {
		$file = "bad-$($c.n).md"
		$u = W $file ("line one`nline two`n" + $c.t + " end")
		$r = Run @('--system', $sys, '--user', $u, '--data', $yml, '--validate')
		$ok = ($r.Code -eq 2) -and ($r.Err -match [regex]::Escape($file)) -and ($r.Err -match 'line 3') -and ($r.Err -match $c.m)
		Check "rendering error exit 2 with file, line and expression: $($c.n)" $ok $r
	}
	$r = Run @('--system', $sys, '--user', (W 'bad2.md' '{input.Title!}'), '--data', $yml, '--validate')
	Check 'malformed error states expected syntax' ($r.Err -match 'Expected syntax') $r

	# Runtime behavior
	$r = Run ($base + '--validate'); Check '--validate works without API key' (($r.Code -eq 0) -and ($r.Out -match 'OK')) $r
	$r = Run ($base + '--print-rendered'); Check '--print-rendered works without API key' ($r.Code -eq 0) $r
	$badCfg = W 'invalid-config.json' 'not json'
	$r = Run ($base + @('--validate', '--config', $badCfg)); Check '--validate does not load LLM config' ($r.Code -eq 0) $r
	$r = Run ($base + @('--print-rendered', '--config', $badCfg)); Check '--print-rendered does not load LLM config' ($r.Code -eq 0) $r

	$u2 = W 'u2.md' 'Version one'
	$r1 = Run @('--system', $sys, '--user', $u2, '--data', $yml, '--print-rendered')
	[IO.File]::WriteAllText($u2, 'Version two')
	$r2 = Run @('--system', $sys, '--user', $u2, '--data', $yml, '--print-rendered')
	Check 'prompt edit picked up without rebuild' (($r1.Out -match 'Version one') -and ($r2.Out -match 'Version two')) $r2
	$y2 = W 'y2.yaml' "input:`n  A:`n    Title: One`n"
	$u3 = W 'u3.md' '{input.A.Title}'
	$r1 = Run @('--system', $sys, '--user', $u3, '--data', $y2, '--print-rendered')
	[IO.File]::WriteAllText($y2, "input:`n  A:`n    Title: Two`n")
	$r2 = Run @('--system', $sys, '--user', $u3, '--data', $y2, '--print-rendered')
	Check 'YAML edit picked up without rebuild' (($r1.Out -match 'One') -and ($r2.Out -match 'Two')) $r2

	# Configuration (normal execution stops before any LLM call because no API key is set).
	$c = W 'c.json' '{"Llm":{"Provider":"Other","Model":"m1"}}'
	$r1 = Run ($base + @('--config', $c))
	[IO.File]::WriteAllText($c, '{"Llm":{"Provider":"OpenAI","Model":"m2"}}')
	$r2 = Run ($base + @('--config', $c))
	Check 'explicit --config accepted and re-read at runtime' (($r1.Code -eq 1) -and ($r1.Err -match 'Unsupported Llm:Provider') -and ($r2.Code -eq 1) -and ($r2.Err -match 'OPENAI_API_KEY')) $r2

	$cwdDir = Join-Path $tmp 'cwd'
	New-Item -ItemType Directory $cwdDir | Out-Null
	[IO.File]::WriteAllText((Join-Path $cwdDir 'appsettings.json'), '{"Llm":{"Provider":"FromCwd","Model":"m"}}')
	$r = Run $base $cwdDir
	Check 'cwd appsettings.json used when present' (($r.Code -eq 1) -and ($r.Err -match "Unsupported Llm:Provider 'FromCwd'")) $r
	$r = Run $base $emptyDir
	Check 'executable-directory config used when cwd has none' (($r.Code -eq 1) -and ($r.Err -match 'OPENAI_API_KEY')) $r
	$r = Run ($base + @('--config', $c)) $cwdDir
	Check '--config takes precedence over cwd config' (($r.Code -eq 1) -and ($r.Err -match 'OPENAI_API_KEY')) $r
	$r = Run ($base + @('--config', (Join-Path $tmp 'missing.json'))) $cwdDir
	Check 'invalid explicit --config does not fall back' (($r.Code -eq 1) -and ($r.Err -notmatch 'FromCwd') -and ($r.Err -notmatch 'OPENAI_API_KEY')) $r
}
finally {
	Remove-Item $tmp -Recurse -Force -ErrorAction SilentlyContinue
}

if ($script:fail -gt 0) { Write-Host "$script:fail test(s) failed"; exit 1 }
Write-Host 'All smoke tests passed'
exit 0
