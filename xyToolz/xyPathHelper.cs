using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xyToolz
{
      public static class xyPathHelper
      {
            public static string BasePath { get; set; } = AppDomain.CurrentDomain.BaseDirectory;

            public static string Combine(params string[] paths) => Path.Combine(paths.Prepend(BasePath).ToArray());

            public static string EnsureDirectory(params string[] subPaths)
            {
                  string fullPath = Combine(subPaths);
                  if (!Directory.Exists(fullPath))
                  {
                        Directory.CreateDirectory(fullPath);
                  }
                  return fullPath;
            }

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
