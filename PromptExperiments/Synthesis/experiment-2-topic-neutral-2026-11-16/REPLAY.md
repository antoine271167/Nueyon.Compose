# Offline replay en controles
Geen nieuwe modelcalls:

    dotnet PromptExperiments/Synthesis/experiment-2-topic-neutral-2026-11-16/runner/bin/Debug/net10.0/Runner.dll replay
    node PromptExperiments/Synthesis/experiment-2-topic-neutral-2026-11-16/verify.cjs
    node PromptExperiments/Synthesis/experiment-2-topic-neutral-2026-11-16/audit.cjs

Uitvoeren vanaf C:\Workspaces\Nueyon.Compose. Replay injecteert uitsluitend opgeslagen responsecontent in de echte SynthesizerAgent/parser; ReplayClient heeft geen netwerktransport. Deze controle bewijst parserreproduceerbaarheid, geen modeldeterminisme. Gebruik de opgeslagen gebouwde runner; wijzigingen/herbouw kunnen freezehashes ongeldig maken.

Run-modus niet opnieuw uitvoeren, lock/attemptmarkers niet verwijderen, freeze.cjs niet opnieuw uitvoeren. Live budget opgebruikt na 24 attempts. preflight was offline vóór freeze en controleerde embedded productieprompts en echte filterprojecties. De ongebruikte geërfde prepare-mode is geen reproduceerstap; cases zijn bytegelijk overgenomen, niet opnieuw voorbereid.

protocol.md/plan.json/prompts/cases/runner en gebouwd runtime zijn prospectief bevroren. Audit-, assessment- en rapportbestanden zijn nadien gemaakt en worden niet als prospectieve analysecode voorgesteld. prompt.diff toont alle wijzigingen; replacements.json specificeert de 11 vervangingen. verification-source-identity.json controleert dat geen andere bytes verschillen.
