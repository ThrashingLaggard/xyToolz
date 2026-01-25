namespace xyToolz.Filesystem;

/// <summary>
/// Facade for filesystem change monitoring.
/// </summary>
public static class Watch
{
    /// <summary>
    /// Monitors a folder (and its subdirectories) for changes.
    /// The caller is responsible for disposing the returned watcher.
    /// </summary>
    public static FileSystemWatcher MonitorFolder(string folderPath,string filter = "*.*",Action<object, FileSystemEventArgs>? onChanged = null)=> xyDirectory.MonitorFolder(folderPath, filter, onChanged);
}
