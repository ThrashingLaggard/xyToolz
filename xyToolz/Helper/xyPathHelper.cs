using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xyToolz.Helper
{
      public static class xyPathHelper
      {
            #if ANDROID
                  public static string BasePath { get; } = Android.App.Application.Context.FilesDir.AbsolutePath;
            #else
            public static string BasePath { get; } = AppContext.BaseDirectory;
            #endif
            public static string Combine( params string [ ] paths ) 
            {
            #if ANDROID
                  return Path.Combine(BasePath, Path.Combine(paths));
            #else
                  return Path.Combine(paths.Prepend(BasePath).ToArray( ));
            #endif
            }
           

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
