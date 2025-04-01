
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Text.Json.Serialization;
using System.Text.Json;

using xyToolz.Helper;
using System.Text;

#if ANDROID
using Android.Content;
using Android.App;
using System.IO;
#endif


namespace xyToolz
{
    /// <summary>
    /// Dateihandling
    /// </summary>
    public class xyFiles
    {
        private static readonly JsonSerializerOptions DefaultJsonOptions = xyJson.defaultJsonOptions;

        /// <summary>
        /// Stellt sicher alle angegebenen Verzeichnisse existieren
        /// </summary>
        /// <param name="directories"></param>
        /// <returns></returns>
        public static bool CheckForDirectories(string[] directories) => directories.All(dir => Directory.Exists(EnsureDirectory(dir)));

        /// <summary>
        /// Make sure the target directory exists by creating it if necessary
        /// </summary>
        /// <param name="dir"></param>
        /// <returns>
        /// True, if everything worked splendidly
        /// False, if an error occured
        /// </returns>
        private static string EnsureDirectory(string dir)
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

        /// <summary>
        /// Make sure the target file exists by creating it if necessary
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>
        /// True, if everything worked splendidly
        /// False, if an error occured
        /// </returns>
        internal static bool EnsurePathExists(string filePath)
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

        /// <summary>
        ///  Rename a file
        /// </summary>
        /// <param name="completePath"></param>
        /// <param name="newName"></param>
        /// <returns>new complete path</returns>
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

        /// <summary>
        /// Open the target File with the explorer
        /// </summary>
        /// <param name="fullPath"></param>
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

        /// <summary>
        /// Copy a file to the target path
        /// </summary>
        /// <param name="fullPath"></param>
        /// <param name="targetPath"></param>
        /// <param name="overwrite"></param>
        /// <returns>The new FullName => complete path</returns>
        public static string Copy(string fullPath, string targetPath, bool overwrite = false)
        {
            try
            {
                xyPathHelper.EnsureParentDirectoryExists(targetPath);
                File.Copy(fullPath, targetPath, overwrite);
                return new FileInfo(targetPath).FullName;
            }
            catch (Exception ex)
            {
                xyLog.ExLog(ex);
                return "Error copying file";
            }
        }

        /// <summary>
        /// Read a file's content into an ienumerable
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<string>> ReadIntoEnum(string filePath)
        {
            string fileError = "Unreadable file?!";
            string fileHandlingError = "Unable to handle file";

            if (xyFiles.EnsurePathExists(filePath))
            {
                if (File.ReadLines(filePath) is IEnumerable<string> lines)
                {
                    if (lines.Count() == 0) return (null!);
                    else
                    {
                        return lines;
                    }
                }
                await xyLog.AsxLog(fileError);
            }
            await xyLog.AsxLog(fileHandlingError);
            return (null!);
        }

        /// <summary>
        /// Generate a memorystream from the specified filepath
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>MemoryStream (as Stream?)</returns>
        public static async Task<Stream> GetStreamFromFile(string filePath)
        {
            try
            {
                string? contentString = await File.ReadAllTextAsync(filePath, Encoding.UTF8);
                if (!string.IsNullOrEmpty(contentString))
                {
                    if (xy.StringToBytes(contentString) is byte[] buffer)
                    {
                        MemoryStream memoryStream = new MemoryStream(buffer);
                        memoryStream.Position = 0;
                        return memoryStream;
                    }
                }
            }
            catch (Exception ex)
            {
                await xyLog.AsxExLog(ex);
            }
            await xyLog.AsxLog($"Unable to get file content from {filePath}, please check it.");
            return new MemoryStream();
        }


        /// <summary>
        /// Save a string into a file
        /// </summary>
        /// <param name="content"></param>
        /// <param name="subfolder"></param>
        /// <param name="fileName"></param>
        /// <returns>true if successfull, false if not</returns>
        public static async Task<bool> SaveToFileAsync(string content, string subfolder = "AppData", string fileName = "config.json")
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


        /// <summary>
        /// Reads the content of a file into a string
        /// </summary>
        /// <param name="subfolder"></param>
        /// <param name="fileName"></param>
        /// <returns>string filecontent</returns>
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


        /// <summary>
        /// CAUTION! 
        /// Delete the target file
        /// </summary>
        /// <param name="subfolder"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
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
