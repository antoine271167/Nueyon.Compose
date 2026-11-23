# Reproduction

Use base commit df761f367076a30e623955b5789df7d3981a0e09, .NET SDK 10.0.404 and the normal console credentials. Preserve checkout bytes by disabling Git automatic line-ending conversion when checking out this experiment. The A and B trees and all case files explicitly preserve checked-in bytes. The frozen manifest records the original order.

Build with `dotnet build src/Nueyon.Compose.slnx --nologo` from the repository root. Use the following template for each manifest row with a fresh output path in a new directory. This is a live reproduction and may generate different text.

```powershell
dotnet src/Nueyon.Compose.Host.Console/bin/Debug/net10.0/Nueyon.Compose.Host.Console.dll run narrative --input "$d/cases/$case/cli-input.json" --output "$output" --prompts "$d/$variant"
```

`verify.ps1` checks frozen SHA-256 hashes, scheduling, run metadata, prompt paths and retained CLI payloads. `review.ps1` presents unchanged briefs by opaque ID. `evaluate.cjs` verifies that every failed score has a matching excerpt in the original payload and applies the frozen gates. The CLI preserves the original result JSON and reports the selected prompt path. It does not expose the provider response envelope, seed, or token usage for Narrative. Elapsed time includes normal CLI launch and shutdown.
