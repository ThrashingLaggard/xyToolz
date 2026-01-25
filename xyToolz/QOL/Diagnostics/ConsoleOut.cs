using xyToolz.QOL;

namespace xyToolz.QOL.Diagnostics;

/// <summary>
/// Simple console output helpers.
/// Part of the public xyToolz diagnostics API.
/// </summary>
public static class ConsoleOut
{
    /// <summary>
    /// Prints a message to the console without a line break.
    /// </summary>
    /// <param name="message">The message to print.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="message"/> is null.
    /// </exception>
    public static void Print(string message)
    {
        ArgumentNullException.ThrowIfNull(message);

        // Delegation to internal logging/console mechanism
        xy.Print(message);
    }

    /// <summary>
    /// Prints a message to the console followed by a line break.
    /// </summary>
    /// <param name="message">The message to print.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="message"/> is null.
    /// </exception>
    public static void PrintLine(string message)
    {
        ArgumentNullException.ThrowIfNull(message);

        xy.Print(message + Environment.NewLine);
    }
}

