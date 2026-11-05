namespace Nueyon.PromptRunner;

internal static class HelpText
{
    public const string Main =
        """
        Nueyon.PromptRunner - run one LLM agent invocation from external prompt files.

        Pipeline: Load -> Render -> Execute -> Output

        USAGE
          Nueyon.PromptRunner --system <path> --user <path> --data <path> [--validate | --print-rendered]
          Nueyon.PromptRunner --help | --help-templates | --example

        REQUIRED (for running, --validate and --print-rendered)
          --system <path>     System prompt file (text or Markdown, placeholders allowed)
          --user <path>       User prompt file (text or Markdown, placeholders allowed)
          --data <path>       YAML file with placeholder values

        OPTIONAL
          --help              Show this help
          --help-templates    Explain placeholder and YAML syntax
          --example           Show a complete working example
          --validate          Check files and placeholders; no LLM call
          --print-rendered    Print rendered prompts; no LLM call

        PATHS
          Absolute or relative; relative paths resolve against the current working directory.
          Files are re-read on every run; no rebuild is needed after editing them.

        OUTPUT
          Normal run: stdout contains only the model's response. Errors and diagnostics go to stderr.

        CONFIGURATION
          appsettings.json next to the executable: Llm:Provider (only "OpenAI") and Llm:Model.
          Credentials: environment variable OPENAI_API_KEY.

        EXIT CODES
          0 success
          1 invalid arguments, configuration, or input files
          2 template rendering failure
          3 agent execution failure
        """;

    public const string Templates =
        """
        PROMPT FILES
          Plain text or Markdown, read as-is (Markdown is not interpreted).

        PLACEHOLDERS
          {path.to.value}   dotted path into the YAML data. Segments: letters, digits, underscore;
                            the first segment must not start with a digit.
          Example: {input.SelectedIdea.Title}

        LITERAL BRACES
          {{ renders as {   and   }} renders as }
          Single braces not forming a placeholder (e.g. {"a": 1} or a lone '{') stay literal.
          Note: '{}' or '{some-text}' is treated as a malformed placeholder; write {{}} or {{some-text}}.

        YAML DATA
          Any structure of nested mappings. Referenced leaves must be scalars
          (string, number, boolean). Multiline strings (| or >) are supported and inserted verbatim.
          Scalars are inserted as written in the YAML text.

        ERRORS
          Missing values, null values, mappings/lists, and malformed placeholders fail with exit code 2
          and name the file, line, and placeholder. Nothing is sent to the LLM.

        LIMITATIONS
          No loops, conditionals, expressions, functions, or list indexing.
        """;

    public const string ExampleSystem =
        """
        You are a Narrative Structurer.
        Preserve factual meaning. Output JSON like {{"title": "..."}}.
        """;

    public const string ExampleUser =
        """
        Create a narrative for the following idea.

        Title: {input.SelectedIdea.Title}
        Description: {input.SelectedIdea.Description}

        Synthesis:
        {input.Synthesis.Content}
        """;

    public const string ExampleYaml =
        """
        input:
          SelectedIdea:
            Title: "The future of AI orchestration"
            Description: "Why orchestration matters for AI-native systems"
          Synthesis:
            Content: |
              AI orchestration coordinates specialized agents
              into structured workflows.
        """;

    public static string Example() =>
        $$$"""
           === system.md ===
           {{{ExampleSystem}}}

           === user.md ===
           {{{ExampleUser}}}

           === experiment.yaml ===
           {{{ExampleYaml}}}

           === command ===
           Nueyon.PromptRunner --system system.md --user user.md --data experiment.yaml --print-rendered

           === expected rendered system prompt ===
           You are a Narrative Structurer.
           Preserve factual meaning. Output JSON like {"title": "..."}.

           === expected rendered user
           Create a narrative for the following idea.

           Title: The future of AI orchestration
           Description: Why orchestration matters for AI-native systems

           Synthesis:
           AI orchestration coordinates specialized agents
           into structured workflows.

           Drop --print-rendered to run the agent, or use --validate to only check the inputs.
           """;
}