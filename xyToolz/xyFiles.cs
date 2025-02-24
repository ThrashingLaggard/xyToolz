
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Text.Json.Serialization;
using System.Text.Json;

using xyToolz.Helper;
using PdfSharp.Quality;





namespace xyToolz
{
      public class xyFiles
      {
            public static bool CheckForDirectories(params string[] directories)
            {
                  return directories.All(dir => Directory.Exists(xyPathHelper.EnsureDirectory(dir)));
            }

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
                        string newPath = xyPathHelper.Combine(dirPath, newName);
                        File.Move(completePath, newPath);
                        return newPath;
                  }
                  catch
                  {
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
                  catch
                  {
                        return "Error copying file";
                  }
            }

            private static readonly JsonSerializerOptions DefaultJsonOptions = new()
            {
                  WriteIndented = true,
                  DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            public static async Task<bool> SaveFileAsync(string content, string subfolder = "AppData", string fileName = "config.json")
            {
                  try
                  {
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
