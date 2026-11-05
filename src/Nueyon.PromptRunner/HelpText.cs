namespace Nueyon.PromptRunner;

internal static class HelpText
{
    public const string Main =
        """
        Nueyon.PromptRunner - run one LLM agent invocation from external prompt files.

        Pipeline: Load -> Render -> Execute -> Output

        USAGE
          Nueyon.PromptRunner --system <path> --user <path> --data <path> [--config <path>] [--validate | --print-rendered]
          Nueyon.PromptRunner --help | --help-templates | --example

        REQUIRED (for running, --validate and --print-rendered)
          --system <path>     System prompt file (text or Markdown, placeholders allowed)
          --user <path>       User prompt file (text or Markdown, placeholders allowed)
          --data <path>       YAML file with placeholder values

        OPTIONAL
          --config <path>     LLM configuration file (see CONFIGURATION)
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
          JSON file with Llm:Provider (only "OpenAI") and Llm:Model, read on every run.
          Precedence: 1) --config <path> (must exist; no fallback if invalid)
                      2) appsettings.json in the current working directory
                      3) appsettings.json next to the executable
          Credentials: environment variable OPENAI_API_KEY.

        ARGUMENT RULES
          Each of --system, --user, --data, --config may be given once; duplicates are an error.
          Unknown options, positional arguments and missing values are errors (exit 1).
          --validate and --print-rendered cannot be combined.
          Informational options (--help, --help-templates, --example) need no other arguments.

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

        LITERAL BRACES AND MALFORMED PLACEHOLDERS
          {{ renders as {   and   }} renders as }
          A '{' directly followed by a letter, digit, '_', '.' or '}' is a placeholder attempt and
          must be a valid placeholder, otherwise rendering fails (exit 2). Errors, for example:
          {input.Title!}  {input..Title}  {input.Title  {input.Title-name}  {}
          A '{' followed by anything else (quote, space, newline) is literal, so JSON such as
          {"a": 1} or an object opened with '{' and a newline needs no escaping.
          For literal text that would look like a placeholder (e.g. {1,2}), write {{1,2}}.
          Escapes are processed left to right: {{{input.Title}}} renders {Hello}; {{{{x}}}} renders {{x}}.

        YAML DATA
          Any structure of nested mappings. Referenced leaves must be scalars
          (string, number, boolean). Multiline strings (| or >) are supported and inserted verbatim.
          YAML is deserialized before rendering. Every scalar becomes its text value:
          strings (including multiline | and > blocks) are inserted with their parsed content;
          numbers and booleans keep their YAML notation (1.50 stays 1.50, True stays True,
          true stays true); no number or culture formatting is applied. Quote or normalize
          values in the YAML if a specific representation is required.
          Null values fail; mappings and lists cannot be inserted and fail.

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