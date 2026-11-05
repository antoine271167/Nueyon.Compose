using System.Text.RegularExpressions;
using YamlDotNet.Serialization;

namespace Nueyon.PromptRunner;

internal sealed class RenderException(string message) : Exception(message);

internal static partial class PromptTemplate
{
    // Alternatives: escaped braces, valid placeholder, malformed placeholder attempt.
    // A '{' followed directly by a letter, digit, '_', '.' or '}' is a placeholder attempt and must be valid.
    // Any other '{' (e.g. followed by a quote, whitespace or newline, as in JSON) is literal text.
    [GeneratedRegex(
        @"\{\{|\}\}|\{(?<path>[A-Za-z_][A-Za-z0-9_]*(?:\.[A-Za-z0-9_]+)*)\}|\{(?<bad>[A-Za-z0-9_.][^\s{}""]*\}?|\})")]
    private static partial Regex TokenRegex();

    public static object? LoadYaml(string text) => new DeserializerBuilder().Build().Deserialize<object?>(text);

    public static string Render(string template, object? data, string fileLabel)
    {
        return TokenRegex().Replace(template, m =>
        {
            if (m.Value == "{{")
            {
                return "{";
            }

            if (m.Value == "}}")
            {
                return "}";
            }

            if (m.Groups["bad"].Success)
            {
                throw new RenderException(
                    $"{fileLabel}: malformed placeholder '{m.Value}' at line {LineOf(template, m.Index)}. Expected syntax: {{path.to.value}} (escape literal braces as {{{{ and }}}}).");
            }

            return Resolve(m.Groups["path"].Value, data, fileLabel, LineOf(template, m.Index));
        });
    }

    private static string Resolve(string path, object? data, string fileLabel, int line)
    {
        var current = data;
        foreach (var segment in path.Split('.'))
        {
            if (current is not IDictionary<object, object> map || !map.TryGetValue(segment, out current))
            {
                throw new RenderException(
                    $"{fileLabel}: unresolved placeholder '{{{path}}}' at line {line} (no value for '{segment}').");
            }
        }

        return current switch
        {
            string s => s,
            null => throw new RenderException($"{fileLabel}: placeholder '{{{path}}}' at line {line} is null."),
            _ => throw new RenderException(
                $"{fileLabel}: placeholder '{{{path}}}' at line {line} refers to a mapping or list; only scalar values are supported.")
        };
    }

    private static int LineOf(string text, int index)
    {
        var line = 1;
        for (var i = 0; i < index; i++)
        {
            if (text[i] == '\n')
            {
                line++;
            }
        }

        return line;
    }
}