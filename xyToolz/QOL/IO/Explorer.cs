using xyToolz.QOL;

namespace xyToolz.QOL.IO;

/// <summary>
/// Provides access to the system's file explorer.
/// </summary>
public static class Explorer
{
    /// <summary>
    /// Opens the system file explorer at the given file or directory path.
    /// </summary>
    /// <param name="path">Absolute path to a file or directory.</param>
    /// <returns>
    /// True if the explorer was started successfully; otherwise false.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="path"/> is null.
    /// </exception>
    public static async Task<bool> OpenAsync(string path)
    {
        ArgumentNullException.ThrowIfNull(path);

        // Delegation to internal QOL implementation
        return await xy.Open(path);
    }
}
