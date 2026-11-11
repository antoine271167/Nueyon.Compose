# Commandolog

Werkdirectory: `C:\Workspaces\Nueyon.Compose`. Alle wijzigingen zijn beperkt tot deze nieuwe experimentmap. Geen secrets opgeslagen.

1. `git status --short`, `git log -5 --oneline`, `rg --files` voor rapporten/harnas/prompts/judge; relevante bestanden gelezen met Get-Content. Werkboom was schoon.
2. `git ls-remote origin HEAD` bevestigt GitHub HEAD `2ca16edd69c1b738d1265fbb27134a95bc7fcfd1`. De gerefereerde chat is met read_thread gelezen als context; repo-artefacten zijn het bewijs.
3. `python prepare.py` was niet beschikbaar. Een tijdelijke Python 3.13.2 embedded runtime is van python.org naar `%TEMP%/nueyon-validator-python` gedownload en daar uitgepakt. Geen repo-runtime geïnstalleerd. De eerste voorbereiding stopte op Windows-tekstcodering; daarna op een vergrendeld `.vs`-indexbestand. Beide gebeurden vóór freeze/live calls. Definitieve voorbereiding gebruikt `-X utf8`, sluit `.vs`, bin en obj uit en bewaart baselinepromptbytes exact.
4. `& "$env:TEMP/nueyon-validator-python/python.exe" -X utf8 PromptExperiments/IdeaGenerator/validator-acceptance-v2/prepare.py` bevriest de set en 108-callprotocol. Daarna weigert het script opnieuw te bevriezen.
5. `dotnet build PromptExperiments/IdeaGenerator/validator-acceptance-v2/harness/ValidatorAcceptance.csproj` → verification/harness-build.txt.
6. `dotnet build src/Nueyon.Compose.slnx` → verification/build.txt; `dotnet test src/Nueyon.Compose.slnx --no-build` → verification/tests.txt. Beide builds/tests afgerond vóór live calls.
7. Met dezelfde Python-runtime: `run.py offline` → verification/offline-log.txt (90 combinaties, acht contractresponsen elk).
8. `run.py tuning` → verification/tuning-log.txt (66 live calls). Periodieke `score.py`-snapshots dienden voortgang, niet promptwijziging. Definitieve tuninguitvoer staat in verification/tuning-summary.json.
9. selectie op tuning vastgelegd in selection.md; `Get-FileHash selection.md | ConvertTo-Json` → verification/selection-hash.json. Kandidaat 2 gekozen vóór eerste live holdoutcall; tijdsvolgorde in final-integrity.json.
10. `run.py holdout` → verification/holdout-log.txt (24 calls), aansluitend `run.py repeat` → verification/repeat-log.txt (18 calls). Geen nieuwe kandidaten of automatische retries.
11. `score.py` → verification/final-summary.json; alle live blockers handmatig gelezen tegen bron/idee en bevroren rubric. `review.py` schrijft expliciete beoordelingen per blocker. `finalize.py` schrijft report.md, diagnoses.md en gates.json. `verify.py` controleert 108 slots/responses, prompt/case/harnashashes, productiehashes, selectievolgorde, git diff/status en eindmanifest.

Het call-ledger bewaart per live slot de exacte uitvoerbare opdracht en alle argumenten, UTC-reservering, voltooiing en exitcode. Alle input-, verdict- en metadata-bestanden staan naast de uitvoerlog per run. `run.py` controleert de bevroren hashes en HEAD voor iedere fase. Volledige scripts blijven bewaard voor reproduceerbaarheid; prepare.py is de constructiegeschiedenis, de bevroren bestanden zijn de feitelijke testinvoer.
