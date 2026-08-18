namespace Nueyon.Compose.Host.Console;

/// <summary>
/// Production implementation of IConsole that delegates to System.Console.
/// </summary>
internal sealed class SystemConsole : IConsole
{
    public string? ReadLine() => System.Console.ReadLine();

    public void Write(string value) => System.Console.Write(value);

    public void WriteLine(string value) => System.Console.WriteLine(value);
}
