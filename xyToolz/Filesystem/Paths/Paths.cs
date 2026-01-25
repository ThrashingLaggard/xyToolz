namespace xyToolz.Filesystem;

/// <summary>
/// Facade for application and solution path helpers.
/// </summary>
public static class Paths
{
    /// <summary>
    /// Get the directory containing the application.
    /// </summary>
    public static string GetApplicationFolder()
        => xyDirectory.GetApplicationFolder();

    /// <summary>
    /// While debugging in C#, use this to get the inner application folder.
    /// </summary>
    public static string GetInnerApplicationFolder()
        => xyDirectory.GetInnerApplicationFolder();

    /// <summary>
    /// Get the full path of the directory containing the solution (.sln).
    /// </summary>
    public static string GetSolutionFolder()
        => xyDirectory.GetSolutionFolder();

    public static Task<bool> EnsurePathExistsAsync(string filePath)
        => xyFiles.EnsurePathExistsAsync(filePath);

    public static string BasePath
       => xyPath.BasePath;

    public static string Combine(params string[] paths)
        => xyPath.Combine(paths);

    public static string? EnsureDirectory(params string[] subPaths)
        => xyPath.EnsureDirectory(subPaths);

    public static void EnsureParentDirectoryExists(string filePath)
        => xyPath.EnsureParentDirectoryExists(filePath);
}
