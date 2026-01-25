using xyToolz.Filesystem;

namespace xyToolz.Filesystem;

/// <summary>
/// Facade for (ZIP) folder archive operations.
/// </summary>
public static class Archives
{
    /// <summary>
    /// Compresses a folder into a ZIP archive.
    /// </summary>
    public static void CompressFolder(
        string sourceFolder,
        string zipFilePath,
        bool includeBaseDirectory = false)
        => xyDirectory.CompressFolder(sourceFolder, zipFilePath, includeBaseDirectory);

    /// <summary>
    /// Extracts a ZIP archive into a target folder.
    /// </summary>
    public static void ExtractFolder(string zipFilePath, string extractFolder)
        => xyDirectory.ExtractFolder(zipFilePath, extractFolder);
}
