namespace Nueyon.Compose.Host.Console;

/// <summary>
/// Abstraction for console input/output operations.
/// Enables testing of the interactive console application without actual console I/O.
/// </summary>
public interface IConsole
{
    /// <summary>
    /// Reads a line of input from the console.
    /// </summary>
    /// <returns>The input line, or null if end of input is reached.</returns>
    string? ReadLine();

    /// <summary>
    /// Writes a string to the console without a newline.
    /// </summary>
    /// <param name="value">The value to write.</param>
    void Write(string value);

    /// <summary>
    /// Writes a string to the console with a newline.
    /// </summary>
    /// <param name="value">The value to write.</param>
    void WriteLine(string value);
}
