
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Text.Json.Serialization;
using System.Text.Json;

using xyToolz.Helper;

#if ANDROID
using Android.Content;
using Android.App;
using System.IO;
#endif


namespace xyToolz
{
      /// <summary>
      /// xyFile eine Klasse zum Dateihandling
      /// </summary>
      public class xyFiles
      {
            private static readonly JsonSerializerOptions DefaultJsonOptions = xyJson.defaultJsonOptions;

            /// <summary>
            /// Stellt sicher alle angegebenen Verzeichnisse existieren
            /// </summary>
            /// <param name="directories"></param>
            /// <returns></returns>
            public static bool CheckForDirectories( string[] directories) => directories.All(dir => Directory.Exists(EnsureDirectory(dir)));
            private static string EnsureDirectory( string dir )
            {
#if ANDROID
                  string path = Path.Combine(Android.App.Application.Context.FilesDir(null)!.AbsolutePath, dir);
                  if(String.IsNullOrEmpty(path))
                  path = Path.Combine(Android.App.Application.Context.GetExternalFilesDir(null)!.AbsolutePath, dir);
#else
                  string path = xyPathHelper.EnsureDirectory(dir);
      #endif
                  return path;
            }

            internal static bool EnsurePathExists( string filePath )
            {
                  try
                  {
                        // Prüfen, ob die Datei existiert
                        if (!File.Exists(filePath))
                        {
                              // Wenn nicht, eine neue Datei erstellen
                            
                              File.Create(filePath);
                              xyLog.Log($"{filePath} was created");
                        }
                        else
                        {
                              xyLog.Log($"{filePath} already exists");
                        }
                        return true;
                  }
                  catch (Exception ex)
                  {
                        xyLog.ExLog(ex);
                        return false;
                  }
            }

            /// <summary>
            /// Return a list filled with the filnames in the target directory
            /// </summary>
            /// <param name="path"></param>
            /// <returns></returns>
            public static List<string> Inventory(string path)
            {
                  List<string> lstfullNames = new();
                  foreach (string file in Directory.GetFiles(xyPathHelper.Combine(path)))
                  {
                        FileInfo fileInfo = new(file);
                        lstfullNames.Add(fileInfo.FullName);
                        xyLog.Log(fileInfo.FullName);
                  }
                  return lstfullNames;
            }

            public static string Rename(string completePath, string newName)
            {
                  try
                  {
                        string dirPath = Path.GetDirectoryName(completePath);
                        string newPath = Path.Combine(dirPath, newName);
                        File.Move(completePath, newPath);
                        return newPath;
                  }
                  catch (Exception e)
                  {
                        xyLog.ExLog(e);
                        return "Error renaming file";
                  }
            }

            public static void Open(string fullPath)
            {
                  try
                  {
                        Process.Start("explorer.exe", fullPath);
                  }
                  catch (Exception e)
                  {
                        xyLog.ExLog(e);
                  }
            }

            public static string Copy(string fullPath, string targetPath, bool overwrite = false)
            {
                  try
                  {
                        xyPathHelper.EnsureParentDirectoryExists(targetPath);
                        File.Copy(fullPath, targetPath, overwrite);
                        return new FileInfo(targetPath).FullName;
                  }
                  catch(Exception ex)
                  {
                        xyLog.ExLog(ex);
                        return "Error copying file";
                  }
            }

           

            public static async Task<bool> SaveFileAsync(string content, string subfolder = "AppData", string fileName = "config.json")
            {
                  try
                  {
                        EnsureDirectory(subfolder);
                        string filePath = xyPathHelper.Combine(subfolder, fileName);
                        await File.WriteAllTextAsync(filePath, content);
                        return true;
                  }
                  catch (Exception ex)
                  {
                        xyLog.ExLog(ex);
                        return false;
                  }
            }

            public static async Task<string?> LoadFileAsync(string subfolder = "AppData", string fileName = "config.json")
            {
                  try
                  {
                        string filePath = xyPathHelper.Combine(subfolder, fileName);
                        return File.Exists(filePath) ? await File.ReadAllTextAsync(filePath) : null;
                  }
                  catch (Exception ex)
                  {
                        xyLog.ExLog(ex);
                        return null;
                  }
            }

            public static async Task<bool> SaveJsonAsync<T>(T data, string subfolder = "AppData", string fileName = "config.json", JsonSerializerOptions? options = null)
            {
                  string jsonData = JsonSerializer.Serialize(data, options ?? DefaultJsonOptions);
                  return await SaveFileAsync(jsonData, subfolder, fileName);
            }

            public static async Task<T?> LoadJsonAsync<T>(string subfolder = "AppData", string fileName = "config.json")
            {
                  string? jsonData = await LoadFileAsync(subfolder, fileName);
                  return jsonData != null ? JsonSerializer.Deserialize<T>(jsonData) : default;
            }

            public static bool DeleteFile(string subfolder = "AppData", string fileName = "config.json")
            {
                  try
                  {
                        string filePath = xyPathHelper.Combine(subfolder, fileName);
                        if (File.Exists(filePath))
                        {
                              File.Delete(filePath);
                              return true;
                        }
                        return false;
                  }
                  catch (Exception ex)
                  {
                        xyLog.ExLog(ex, LogLevel.Error);
                        return false;
                  }
            }
      }
}
