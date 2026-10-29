using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace Nueyon.Compose.Application.Prompts;

/// <summary>
///     Loads prompt templates embedded in this assembly and renders named <c>{{Token}}</c> placeholders.
/// </summary>
internal static partial class PromptLoader
{
    private const string ResourcePrefix = "Nueyon.Compose.Application.Prompts.";
    private static readonly Assembly _assembly = typeof(PromptLoader).Assembly;

    [GeneratedRegex(@"\{\{([A-Za-z][A-Za-z0-9]*)\}\}")]
    private static partial Regex TokenRegex();

    /// <summary>
    ///     Loads a prompt template as UTF-8 text.
    /// </summary>
    /// <param name="promptPath">Path relative to the Prompts directory, e.g. "Idea/Idea.system.prompt.txt".</param>
    /// <exception cref="FileNotFoundException">Thrown when the prompt is not embedded in the assembly.</exception>
    public static string Load(string promptPath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(promptPath);

        var resourceName = ResourcePrefix + promptPath.Replace('/', '.').Replace('\\', '.');
        using var stream = _assembly.GetManifestResourceStream(resourceName)
                           ?? throw new FileNotFoundException(
                               $"Prompt '{promptPath}' was not found (embedded resource '{resourceName}').",
                               promptPath);
        using var reader = new StreamReader(stream, new UTF8Encoding(false), false);
        return reader.ReadToEnd();
    }

    /// <summary>
    ///     Loads a prompt template and replaces every <c>{{Token}}</c> in a single pass.
    ///     Inserted values are never re-scanned for tokens.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when the template contains a token without a supplied value, or a supplied value is not used.
    /// </exception>
    public static string Render(string promptPath, params (string Name, string Value)[] values)
    {
        var template = Load(promptPath);
        var lookup = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (var (name, value) in values)
        {
            lookup[name] = value;
        }

        var used = new HashSet<string>(StringComparer.Ordinal);
        var missing = new SortedSet<string>(StringComparer.Ordinal);

        var rendered = TokenRegex().Replace(template, match =>
        {
            var name = match.Groups[1].Value;
            if (lookup.TryGetValue(name, out var value))
            {
                used.Add(name);
                return value;
            }

            missing.Add(name);
            return match.Value;
        });

        if (missing.Count > 0)
        {
            throw new InvalidOperationException(
                $"Prompt '{promptPath}' has unresolved tokens: {string.Join(", ", missing)}.");
        }

        var unused = lookup.Keys.Where(k => !used.Contains(k)).Order(StringComparer.Ordinal).ToList();
        if (unused.Count > 0)
        {
            throw new InvalidOperationException(
                $"Prompt '{promptPath}' does not contain supplied tokens: {string.Join(", ", unused)}.");
        }

        return rendered;
    }
}