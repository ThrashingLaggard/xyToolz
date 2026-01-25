using xyToolz.QOL;

namespace xyToolz.IO;

/// <summary>
/// Provides access to the system's default text editor.
/// </summary>
public static class Editor
{
    /// <summary>
    /// Opens the default editor without a file.
    /// </summary>
    public static async Task OpenAsync()
    {
        await xy.Editor();
    }

    /// <summary>
    /// Opens the default editor with the specified file.
    /// </summary>
    /// <param name="filePath">Absolute path to the file.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="filePath"/> is null.
    /// </exception>
    public static void Open(string filePath)
    {
        ArgumentNullException.ThrowIfNull(filePath);

        xy.Editor(filePath);
    }
}
