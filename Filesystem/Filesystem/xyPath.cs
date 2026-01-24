
using xyToolz.StaticLogging;

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
