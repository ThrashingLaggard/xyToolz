using xyLogger.Loggers;

namespace xyToolz.Filesystem
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Benennungsstile", Justification = "<Because its my wish to do so!>")]
    
    public static class xyPath
    {
#if ANDROID
                  public static string BasePath { get; } = Android.App.Application.Context.FilesDir.AbsolutePath;
#else
        public static string BasePath { get; } = AppContext.BaseDirectory;
#endif
        /// <summary>
        /// Combine Paths
        /// </summary>
        /// <param name="paths"></param>
        /// <returns></returns>
        public static string Combine(params string[] paths)
        {
#if ANDROID
                  return Path.Combine(BasePath, Path.Combine(paths));
#else
            return Path.Combine([.. paths.Prepend(BasePath)]);
#endif
        }

        /// <summary>
        /// Make sure the target directory exists
        /// </summary>
        /// <param name="subPaths"></param>
        /// <returns></returns>
        public static string? EnsureDirectory(params string[] subPaths)
        {
            string fullPath = Combine(subPaths);
            try
            {
                if (!Directory.Exists(fullPath))
                {
                    Directory.CreateDirectory(fullPath);
                }
                return fullPath;
            }
            catch (Exception ex)
            {
                xyLog.ExLog(ex);
            }
            return null;
        }

        /// <summary>
        /// Resolves <paramref name="relativePath"/> against <paramref name="basePath"/> and
        /// rejects any result that escapes <paramref name="basePath"/> (e.g. via "..").
        /// </summary>
        /// <param name="basePath">The directory the result must stay inside.</param>
        /// <param name="relativePath">A path, relative or absolute, to validate.</param>
        /// <returns>The resolved, validated full path.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the resolved path lies outside <paramref name="basePath"/>.
        /// </exception>
        public static string EnsureNoTraversal(string basePath, string relativePath)
        {
            ArgumentException.ThrowIfNullOrEmpty(basePath);
            ArgumentException.ThrowIfNullOrEmpty(relativePath);

            string fullBase = Path.GetFullPath(basePath);
            string fullTarget = Path.GetFullPath(Path.Combine(fullBase, relativePath));

            string baseWithSeparator = fullBase.EndsWith(Path.DirectorySeparatorChar)
                ? fullBase
                : fullBase + Path.DirectorySeparatorChar;

            if (!fullTarget.StartsWith(baseWithSeparator, StringComparison.OrdinalIgnoreCase)
                && !string.Equals(fullTarget, fullBase, StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException(
                    $"Path '{relativePath}' escapes base directory '{basePath}'.", nameof(relativePath));
            }

            return fullTarget;
        }

        /// <summary>
        /// Make sure the directory for the target path exists
        /// </summary>
        /// <param name="filePath"></param>
        public static void EnsureParentDirectoryExists(string filePath)
        {
            string? dir = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }
    }
}
