
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Text.Json.Serialization;
using System.Text.Json;
using Xamarin.Essentials;
using xyToolz.StoragePermission;





namespace xyToolz
{
    public static class xyFiles
    {

        #region "EnsureDirectoriesExist"

        /// <summary>
        /// Checks for the target directory & creates it, if its not already there
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public static bool CheckForDirectories(string directory)
        {
            bool result = false;

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Checks for the target directories & creates them, if theyre not already there
        /// </summary>
        /// <param name="directories"></param>
        /// <returns></returns>
        public static bool CheckForDirectories(string[] directories)
        {
            bool result = false;

            foreach (string dir in directories)
            {
                if (!Directory.Exists(dir))
                {
                    result = false;
                    try
                    {
                        Directory.CreateDirectory(dir);
                        result = true;
                    }
                    catch
                    {
                        Console.WriteLine(dir);
                        Console.WriteLine("Not available & couldnt be created");
                        result = false;
                    }
                }
            }
            Console.WriteLine((result) ? result : result);
            return result;
        }


        #endregion





        /// <summary>
        /// Returns a list filled with the COMPLETE PATHs of all the files in the folder.
        /// 
        /// Add a random string as second parameter to get a list filled with the FILEINFOs for all files in the folder.
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static List<string> Inventory(string path)
        {
            List<FileInfo> lst_FileInfo = new List<FileInfo>();
            DirectoryInfo directoryinfo = new DirectoryInfo(path);

            string[] allFiles = Directory.GetFiles(directoryinfo.FullName);
            FileInfo fileInfo = new FileInfo(path);
            int u = 0;

            for (; u < allFiles.Length; u++)
            {
                fileInfo = new(allFiles[u]);
                if (!(fileInfo.FullName == null))
                {

                    lst_FileInfo.Add(fileInfo);
                }
                else
                {
                    Console.WriteLine(fileInfo.Name);
                    Console.WriteLine("Error!!!");
                }
            }

            //lst_FileInfo.Sort();
            List<string> lstfullNames = new List<string>();
            foreach (FileInfo info in lst_FileInfo)
            {
                lstfullNames.Add(info.FullName);
                Console.WriteLine(info.FullName);
            }
            return lstfullNames;

        }
        public static List<FileInfo> Inventory(string path, string _not_needed___insert_random_string_)
        {
            List<FileInfo> lst_FileInfo = new List<FileInfo>();
            DirectoryInfo directoryinfo = new DirectoryInfo(path);

            string[] allFiles = Directory.GetFiles(directoryinfo.FullName);
            FileInfo fileInfo = new FileInfo(path);
            int u = 0;

            for (; u < allFiles.Length; u++)
            {
                fileInfo = new(allFiles[u]);
                if (!(fileInfo.FullName == null))
                {

                    lst_FileInfo.Add(fileInfo);
                }
                else
                {
                    Console.WriteLine(fileInfo.Name);
                    Console.WriteLine("Error!!!");
                }
            }

            lst_FileInfo.Sort();
            List<string> lstfullNames = new List<string>();
            foreach (FileInfo info in lst_FileInfo)
            {
                lstfullNames.Add(info.FullName);
                Console.WriteLine(info.FullName);
            }
            return lst_FileInfo;

        }



      /// <summary>
      /// Rename a file if needed:
      /// 
      /// complete path needed for both!!! 
      /// Include file-endings!!!
      /// 
      /// 
      /// Rename(@"C:\Users\Hansi\Desktop\Skripte\DemoFile.pdf", "DemoName.pdf") <--The ending is needed!
      /// 
      /// 
      /// You can also move files with this!
      /// </summary>
      /// <param name="complete_path"></param>
      /// <param name="new_name"></param>
      /// <returns></returns>
      public static string Rename(string complete_path, string new_name)
      {
            try
            {
                  string dirPath = Path.GetDirectoryName(complete_path);
                  FileInfo exampleFile = new FileInfo(complete_path);
                  exampleFile.MoveTo(Path.Combine(dirPath + "\\", new_name));

                  return exampleFile.FullName;
            }
            catch
            {
                  Console.WriteLine("Couldnt be renamed");
                  return "xxx###xxx###xxx###xxxx###xxx";
            }

      }

      /// <summary>
      /// Use the Explorer to Open a file or a directory by entering the FULL_PATH
      /// </summary>
      /// <param name="full_path"></param>
      public static void Open(string full_path)
      {
      try
      {
            Process.Start("explorer.exe",full_path);
      }
      catch(Exception e)
      {
            xyLog.ExLog(e);
      }
      }

        public static string Copy(string full_path, string target_path)
        {
            try
            {
                string? dir = Path.GetDirectoryName(target_path);

                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                    DirectoryInfo info = new(dir);
                    info.Attributes = FileAttributes.Directory;
                }
                
                File.Copy(full_path, target_path, false);

                FileInfo fileInfo = new FileInfo(target_path);
                return fileInfo.FullName;
            }
            catch
            {
                return("#############################XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX#############################");
            }            
        }



        public static string Copy_Overwrite(string full_path, string target_path)
        {
            try
            {
                string dir = Path.GetDirectoryName(target_path);

                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                    DirectoryInfo info = new(dir);
                    info.Attributes = FileAttributes.Directory;
                }

                File.Copy(full_path, target_path, true);

                FileInfo fileInfo = new FileInfo(target_path);
                return fileInfo.FullName;
            }
            catch
            {
                return ("#############################XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX#############################");
            }
        }



        private static readonly JsonSerializerOptions DefaultJsonOptions = new()
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        /// <summary>
        /// Speichert beliebige Daten als Datei (z. B. JSON, TXT, CSV).
        /// </summary>
        public static async Task<bool> SaveFileAsync(string content, string subfolder = "AppData", string fileName = "config.json")
        {
            try
            {
                string basePath = await GetBasePathAsync();
                string folderPath = xyFolder.EnsureSubfolderExists(basePath, subfolder);
                string filePath = Path.Combine(folderPath, fileName);

             //   await File.WriteAllTextAsync(filePath, content);
                return true;
            }
            catch (Exception ex)
            {
                xyLog.ExLog(ex);
                return false;
            }
        }

        /// <summary>
        /// Lädt den Inhalt einer Datei als String (z. B. JSON, TXT, CSV).
        /// </summary>
        public static async Task<string?> LoadFileAsync(string subfolder = "AppData", string fileName = "config.json")
        {
            try
            {
                string basePath = await GetBasePathAsync();
                string folderPath = Path.Combine(basePath, subfolder);
                string filePath = Path.Combine(folderPath, fileName);

                if (!File.Exists(filePath))
                    return null;
                return File.ReadAllText(filePath);
              //  return await File.ReadAllTextAsync(filePath);
            }
            catch (Exception ex)
            {
                xyLog.ExLog(ex);
                return null;
            }
        }

        /// <summary>
        /// Speichert Daten als JSON-Datei.
        /// </summary>
        public static async Task<bool> SaveJsonAsync<T>(T data, string subfolder = "AppData", string fileName = "config.json", JsonSerializerOptions? options = null)
        {
            string jsonData = JsonSerializer.Serialize(data, options ?? DefaultJsonOptions);
            return await SaveFileAsync(jsonData, subfolder, fileName);
        }

        /// <summary>
        /// Lädt Daten aus einer JSON-Datei.
        /// </summary>
        public static async Task<T?> LoadJsonAsync<T>(string subfolder = "AppData", string fileName = "config.json")
        {
            string? jsonData = await LoadFileAsync(subfolder, fileName);
            return jsonData != null ? JsonSerializer.Deserialize<T>(jsonData) : default;
        }

        /// <summary>
        /// Löscht eine Datei.
        /// </summary>
        public static bool DeleteFile(string subfolder = "AppData", string fileName = "config.json")
        {
            try
            {
                string basePath = GetBasePath();
                string folderPath = Path.Combine(basePath, subfolder);
                string filePath = Path.Combine(folderPath, fileName);

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
