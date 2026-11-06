# Repository discovery

- **Repository:** `C:\Workspaces\Nueyon.Compose`
- **Branch / HEAD:** `main` / `be882989a7b7956e1b57199f143e7cf434fbe4e5`
- **Working tree:** no tracked modifications; pre-existing untracked `prompts/` directory was left untouched.
- **Entry point:** `src/Nueyon.Compose.Host.Console/Program.cs`; console host project targets `net10.0`.
- **Execution:** build `src/Nueyon.Compose.slnx`, then run `dotnet Nueyon.Compose.Host.Console.dll` from `src/Nueyon.Compose.Host.Console/bin/Debug/net10.0`. The output directory receives `appsettings.json` and linked `data/input/*.md` files.
- **Input:** host loads top-level Markdown files from `data/input`; the sole input is `Noeyon-Product-Strategy-reconstructed.md`. The exact workflow attachment and this input were copied to `baseline/input/`.
- **Verified execution sequence:** Idea agent returns a list; a deterministic executor selects the first item; then Research → Synthesis → Narrative → Compose. Compose is configured for `LinkedInPost` in `StoryWorkflow`.
- **Prompt loading:** 11 `.prompt.txt` resources are embedded by `Nueyon.Compose.Application.csproj`; system prompt files load through agent instruction wrappers, user prompt files render with named placeholders via `PromptLoader`.
- **Prompt locations:** `src/Nueyon.Compose.Application/Prompts/{Idea,Research,Synthesis,Narrative,Compose}/`; Compose has separate LinkedIn and article user prompts.
- **Configuration:** OpenAI provider, model `gpt-5-mini` from host `appsettings.json`; API key is configured in user secrets (value excluded). No temperature is set in application configuration. No external research retrieval tool is registered in the agent factory/wiring inspected.
- **Output and logging:** console prints selected Idea and full Research, Synthesis, Narrative, Compose contents. Information-level console logs include per-agent duration and failures. It does not print all generated Idea candidates.
- **Tests:** `dotnet test src/Nueyon.Compose.slnx`.
- **Baseline caveat:** only the selected Idea is observable through existing app output. No code changes will be made to expose additional candidates.
