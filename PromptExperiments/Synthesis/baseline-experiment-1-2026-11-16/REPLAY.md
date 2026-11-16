# Offline replay

Vanuit C:\Workspaces\Nueyon.Compose:

```powershell
dotnet run --no-build --project PromptExperiments/Synthesis/baseline-experiment-1-2026-11-16/runner -- replay
node PromptExperiments/Synthesis/baseline-experiment-1-2026-11-16/verify.cjs
```

Replay gebruikt uitsluitend opgeslagen response-raw.json via een offline IChatClient en echte SynthesizerAgent/parser; geen credentials of netwerk. De freeze wordt eerst gecontroleerd. Resultaat moet 12 exacte replays zijn. verify controleert SHA256, requestrendering, inputs en usage. Bij een rebuild kunnen runtimehashes verschillen; bewaar de oorspronkelijke freeze en binaries en onderzoek het verschil, overschrijf de freeze niet.

Run.lock/attemptmarkers nooit resetten. Live run niet opnieuw starten. Prepare/freeze zijn historische voorbereidende scripts, geen replaycommando’s.
