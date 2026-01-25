using xyToolz.QOL;

namespace xyToolz.QOL.Text;

/// <summary>
/// Text-related helper methods provided by xyToolz.
/// This is part of the public product API.
/// </summary>
public static class Strings
{
    /// <summary>
    /// Repeats a string a specified number of times.
    /// </summary>
    /// <param name="text">The text to repeat.</param>
    /// <param name="count">Number of repetitions. Must be >= 0.</param>
    /// <returns>The concatenated string.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="text"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="count"/> is negative.</exception>
    public static string Repeat(string text, ushort count)
    {
        ArgumentNullException.ThrowIfNull(text);
        if (count < 0)
            throw new ArgumentOutOfRangeException(nameof(count), "Count must be non-negative.");

        // Delegation to internal implementation
        return xy.Repeat(text, count);
    }

    /// <summary>
    /// Reverses the characters in a string.
    /// </summary>
    /// <param name="input">The string to reverse.</param>
    /// <returns>The reversed string.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="input"/> is null.</exception>
    public static string Reverse(string input)
    {
        ArgumentNullException.ThrowIfNull(input);

        // Delegation to internal implementation
        return xy.Reverse(input);
    }
}
