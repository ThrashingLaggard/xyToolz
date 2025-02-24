using Xamarin.Essentials;
using xyAndroid.StoragePermission;
using xyToolz;

namespace xyAndroid
{
      public class xyFilesAndroid : xyFiles
      {

           
            /// <summary>
            /// Bestimmt den Basisordner plattformabhängig & stellt Berechtigungen für Android sicher.
            /// </summary>
            private static async Task<string> GetBasePathAsync()
            {
                  if (DeviceInfo.Platform == DevicePlatform.Android)
                  {
                        bool hasPermission = await DefaultStoragePermissionService.RequestStoragePermissionsAsync();
                        if (!hasPermission)
                        {
                              var noAccessEx = new UnauthorizedAccessException("Speicherberechtigung nicht erteilt.");
                              xyLog.ExLog(noAccessEx);
                              throw noAccessEx;
                        }
                  }
                  return GetBasePath();
            }

            private static string GetBasePath()
            {
                  if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
                  {
                        return FileSystem.AppDataDirectory;
                  }
                  else if (DeviceInfo.Platform == DevicePlatform.Unknown)
                  {
                        return AppContext.BaseDirectory;
                  }
                  else
                  {
                        return FileSystem.AppDataDirectory;
                  }
            }
      }
}
