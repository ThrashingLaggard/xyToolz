using System.IO;
using xyToolz.Filesystem;

namespace xyToolz.Filesystem;

/// <summary>
/// Facade for directory-related filesystem operations.
/// </summary>
public static class Directories
{
    // -------------------------------------------------
    // Status
    // -------------------------------------------------

    public static bool IsFolderEmpty(string folderPath)
        => xyDirectory.IsFolderEmpty(folderPath);

    // -------------------------------------------------
    // Listing
    // -------------------------------------------------

    public static string[] GetFiles(
        string folderPath,
        string searchPattern = "*.*",
        bool includeSubdirectories = false)
        => xyDirectory.GetFiles(folderPath, searchPattern, includeSubdirectories);

    public static string[] GetSubfolders(string folderPath)
        => xyDirectory.GetSubfolders(folderPath);

    // -------------------------------------------------
    // Manipulation
    // -------------------------------------------------

    public static bool DeleteFolder(string folderPath)
        => xyDirectory.DeleteFolder(folderPath);

    public static void ClearFolder(string folderPath)
        => xyDirectory.ClearFolder(folderPath);

    public static void CopyFolder(
        string sourceFolder,
        string destinationFolder,
        bool overwrite = false)
        => xyDirectory.CopyFolder(sourceFolder, destinationFolder, overwrite);

    public static string RenameFolder(string currentFolderPath, string newFolderName)
        => xyDirectory.RenameFolder(currentFolderPath, newFolderName);

    public static string MoveFolder(string sourceFolder, string destinationFolder)
        => xyDirectory.MoveFolder(sourceFolder, destinationFolder);

    // -------------------------------------------------
    // Analysis
    // -------------------------------------------------

    public static long GetFolderSize(string folderPath)
        => xyDirectory.GetFolderSize(folderPath);
}
