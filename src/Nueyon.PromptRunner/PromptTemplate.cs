using System.Globalization;
using System.Text.RegularExpressions;
using YamlDotNet.Serialization;

namespace Nueyon.PromptRunner;

internal sealed class RenderException(string message) : Exception(message);

internal static partial class PromptTemplate
{
    // Alternatives: escaped open brace, escaped close brace, valid placeholder, malformed placeholder-like token.
    [GeneratedRegex(@"\{\{|\}\}|\{(?<path>[A-Za-z_][A-Za-z0-9_]*(?:\.[A-Za-z0-9_]+)*)\}|\{(?<bad>[A-Za-z0-9_.\-]*)\}")]
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
                    $"{fileLabel}: malformed placeholder '{m.Value}' at line {LineOf(template, m.Index)}. Use {{a.b.c}} or escape literal braces as {{{{ and }}}}.");
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
            IFormattable f => f.ToString(null, CultureInfo.InvariantCulture),
            bool b => b ? "true" : "false",
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