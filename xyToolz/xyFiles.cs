using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using xyToolz.Helper;

#if ANDROID
using Android.Content;
using Android.App;
#endif

namespace xyToolz
{
    /// <summary>
    /// Provides file system utilities for reading, writing, copying, renaming,
    /// and validating files or folders across supported platforms.
    ///
    /// <para><b>Available Features:</b></para>
    /// <list type="bullet">
    ///   <item><description>Directory management (EnsureDirectory, CheckForDirectories).</description></item>
    ///   <item><description>File path validation (EnsurePathExistsAsync).</description></item>
    ///   <item><description>File metadata & manipulation (Inventory, InventoryNames, RenameFileAsync, OpenAsync, CopyAsync).</description></item>
    ///   <item><description>File content handling (ReadLinesAsync, GetStreamFromFileAsync, SaveToFileAsync, LoadFileAsync, DeleteFile).</description></item>
    /// </list>
    ///
    /// <para><b>Thread Safety:</b></para>
    /// Thread-safe: only static methods, no mutable instance state.
    ///
    /// <para><b>Limitations:</b></para>
    /// Does not support file system transactions or advanced locking; limited to System.IO capabilities.
    ///
    /// <para><b>Performance:</b></para>
    /// Asynchronous methods use buffered I/O; performance depends on file size and underlying storage.
    ///
    /// <para><b>Configuration:</b></para>
    /// Serializer options via xyJson.defaultJsonOptions; path resolution via xyPathHelper.
    ///
    /// <para><b>Example Usage:</b></para>
    /// <code>
    /// // List all file names
    /// var names = xyFiles.InventoryNames("C:\\Temp");
    ///
    /// // Rename a file
    /// if (await xyFiles.RenameFileAsync("old.txt", "new.txt", out var newPath))
    /// {
    ///     Console.WriteLine($"Renamed to {newPath}");
    /// }
    ///
    /// // Read lines from a file
    /// var lines = await xyFiles.ReadLinesAsync("data.txt");
    ///
    /// // Delete a file
    /// bool deleted = xyFiles.DeleteFile("AppData", "config.json");
    /// </code>
    ///
    /// <para><b>Related:</b></para>
    /// <see cref="System.IO.File"/>, <see cref="System.IO.Directory"/>, <see cref="xyToolz.Helper.xyPathHelper"/>
    /// </summary>
    public static class xyFiles
    {
        private static readonly JsonSerializerOptions DefaultJsonOptions = xyJson.defaultJsonOptions;

        #region Directory Management

        /// <summary>
        /// Checks whether all specified directories currently exist.
        /// </summary>
        /// <param name="directories">An array of directory paths.</param>
        /// <returns>True if all directories exist; otherwise, false.</returns>
        public static bool CheckForDirectories(string[] directories) =>
            directories.All(dir => Directory.Exists(dir));

        /// <summary>
        /// Ensures the target directory path is constructed appropriately across platforms.
        /// </summary>
        /// <param name="dir">Relative or absolute directory path.</param>
        /// <returns>The resolved directory path.</returns>
        private static string EnsureDirectory(string dir)
        {
    #if ANDROID
            string path = Path.Combine(Android.App.Application.Context.FilesDir(null)!.AbsolutePath, dir);
            if (string.IsNullOrEmpty(path))
                path = Path.Combine(Android.App.Application.Context.GetExternalFilesDir(null)!.AbsolutePath, dir);
    #else
            string path = xyPathHelper.EnsureDirectory(dir);
    #endif
            return path;
        }

        #endregion

        #region File Path Validation

        /// <summary>
        /// Ensures the target file path exists by creating it if necessary.
        /// </summary>
        /// <param name="filePath">Full path to the file.</param>
        /// <returns>True if the file exists or was created successfully; otherwise, false.</returns>
        public static async Task<bool> EnsurePathExistsAsync(string filePath)
        {
            const string errorEmptyPath = "The given file path is null or empty.";
            string createdMsg = $"{filePath} was created.";
            string existsMsg = $"{filePath} already exists.";

            if (string.IsNullOrWhiteSpace(filePath))
            {
                await xyLog.AsxLog(errorEmptyPath);
                return false;
            }

            try
            {
                if (!File.Exists(filePath))
                {
                    using (File.Create(filePath)) { }
                    await xyLog.AsxLog(createdMsg);
                }
                else
                {
                    await xyLog.AsxLog(existsMsg);
                }
                return true;
            }
            catch (Exception ex)
            {
                await xyLog.AsxExLog(ex);
                return false;
            }
        }

        #endregion

        #region File Metadata & Manipulation

        /// <summary>
        /// Returns a list of <see cref="FileInfo"/> objects for all files in the given directory.
        /// </summary>
        /// <param name="path">Directory path.</param>
        /// <returns>List of FileInfo.</returns>
        public static List<FileInfo> Inventory(string path)
        {
            List<FileInfo> fileList = new();
            foreach (string file in Directory.GetFiles(xyPathHelper.Combine(path)))
            {
                FileInfo fileInfo = new(file);
                fileList.Add(fileInfo);
                xyLog.Log(fileInfo.FullName);
            }
            return fileList;
        }

        /// <summary>
        /// Returns a list of full file names (paths) from the specified directory.
        /// </summary>
        /// <param name="path">Directory path.</param>
        /// <returns>List of file path strings.</returns>
        public static List<string> InventoryNames(string path) =>
            Inventory(path).Select(f => f.FullName).ToList();

        /// <summary>
        /// Renames a file to a new name within its directory, returning success status.
        /// </summary>
        /// <param name="completePath">The full original path of the file.</param>
        /// <param name="newName">The new name of the file (must not include directory separators).</param>
        /// <param name="newPath">Returns the newly constructed file path if successful.</param>
        /// <returns>True if the file was renamed successfully; otherwise, false.</returns>
        public static async Task<bool> RenameFileAsync(string completePath, string newName)
        {
            string newPath;
            string errorMissingInput = "The provided file path or new file name is null or empty.";
            string errorInvalidName  = "The new file name contains invalid characters or is a directory path.";
            string errorFileNotFound = "The original file does not exist.";
            string errorTargetExists = "A file with the target name already exists.";
            string successTemplate   = "File renamed successfully from '{0}' to '{1}'.";

            if (string.IsNullOrWhiteSpace(completePath) || string.IsNullOrWhiteSpace(newName))
            {
                await xyLog.AsxLog(errorMissingInput);
                return false;
            }

            if (newName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0 ||newName.Contains(Path.DirectorySeparatorChar))
            {
                await xyLog.AsxLog(errorInvalidName);
                return false;
            }

            try
            {
                if (!File.Exists(completePath))
                {
                    await xyLog.AsxLog(errorFileNotFound);
                    return false;
                }

                string? dirPath = Path.GetDirectoryName(completePath);
                if (string.IsNullOrWhiteSpace(dirPath))
                    return false;

                newPath = Path.Combine(dirPath, newName);
                if (File.Exists(newPath))
                {
                    await xyLog.AsxLog(errorTargetExists);
                    return false;
                }

                File.Move(completePath, newPath);
                await xyLog.AsxLog(string.Format(successTemplate, completePath, newPath));
                return true;
            }
            catch (Exception ex)
            {
                await xyLog.AsxExLog(ex);
                return false;
            }
        }

        /// <summary>
        /// Asynchronously opens a file or folder in the system’s default file explorer.
        /// Cross‑platform (Windows, Linux, macOS) and fully logged.
        /// </summary>
        /// <param name="fullPath">Absolute path to the file or directory.</param>
        /// <returns>True if opened successfully; otherwise, false.</returns>
        public static async Task<bool> OpenAsync(string fullPath)
        {
            const string invalidPathMsg   = "The given path was null or empty.";
            const string notFoundMsg      = "Target does not exist:";
            const string successTemplate  = "Explorer opened in {0} ms: {1}";
            const string unsupportedOsMsg = "No suitable explorer command found for this OS.";

            if (string.IsNullOrWhiteSpace(fullPath))
            {
                await xyLog.AsxLog(invalidPathMsg);
                return false;
            }
            if (!File.Exists(fullPath) && !Directory.Exists(fullPath))
            {
                await xyLog.AsxLog($"{notFoundMsg} {fullPath}");
                return false;
            }

            string cmd, args;
            var stopwatch = new Stopwatch();

            if (OperatingSystem.IsWindows())
            {
                cmd  = "explorer";
                args = $"\"{fullPath}\"";
            }
            else if (OperatingSystem.IsLinux())
            {
                cmd  = "xdg-open";
                args = fullPath;
            }
            else if (OperatingSystem.IsMacOS())
            {
                cmd  = "open";
                args = fullPath;
            }
            else
            {
                await xyLog.AsxLog(unsupportedOsMsg);
                return false;
            }

            try
            {
                var psi = new ProcessStartInfo
                {
                    FileName        = cmd,
                    Arguments       = args,
                    UseShellExecute = true,
                    CreateNoWindow  = true
                };

                stopwatch.Start();
                Process.Start(psi);
                stopwatch.Stop();

                await xyLog.AsxLog(string.Format(successTemplate, stopwatch.ElapsedMilliseconds, fullPath));
                return true;
            }
            catch (Exception ex)
            {
                await xyLog.AsxExLog(ex);
                return false;
            }
        }

        #endregion

        #region File Content Handling

        /// <summary>
        /// Asynchronously reads all lines from the specified file.
        /// Returns an empty sequence if the path is invalid or on error.
        /// </summary>
        public static async Task<IEnumerable<string>> ReadLinesAsync(string filePath)
        {
            string noFile     = $"File does not exist:{filePath}";
            string error    = $"Error reading file:{filePath}";
            string success= $"Bytes have been read from {filePath}:";
            string [] allLines ;

            if (!await EnsurePathExistsAsync(filePath))
            {
                await xyLog.AsxLog($"{noFile} {filePath}");
                return Enumerable.Empty<string>();
            }

            try
            {
                allLines = await File.ReadAllLinesAsync(filePath);
                await xyLog.AsxLog(string.Format(success, allLines.Length) + $" {filePath}");
                return allLines;
            }
            catch (Exception ex)
            {
                await xyLog.AsxExLog(ex);
                await xyLog.AsxLog($"{error} {filePath}");
                return Enumerable.Empty<string>();
            }
        }

        /// <summary>
        /// Asynchronously reads the entire file into a MemoryStream.
        /// Returns an empty stream on error or if the file doesn’t exist.
        /// </summary>
        public static async Task<Stream?> GetStreamFromFileAsync(string filePath)
        {
            const string invalidPathMsg  = "The given file path is null or empty.";
            string notFoundMsg     = "File does not exist:";
            string errorReadMsg    = "Error reading file into stream:";
            string success= $"Loaded file into stream from {filePath}:";

            if (string.IsNullOrWhiteSpace(filePath))
            {
                await xyLog.AsxLog(invalidPathMsg);
                return null;
            }

            bool exists = await EnsurePathExistsAsync(filePath);
            if (!exists || !File.Exists(filePath))
            {
                await xyLog.AsxLog($"{notFoundMsg} {filePath}");
                return null; ;
            }

            byte[] buffer;
            MemoryStream memoryStream;
            try
            {
                buffer = await File.ReadAllBytesAsync(filePath);
                memoryStream = new MemoryStream(buffer) { Position = 0 };

                await xyLog.AsxLog($"{ buffer.Length} bytes");
                return memoryStream;
            }
            catch (Exception ex)
            {
                await xyLog.AsxExLog(ex);
                await xyLog.AsxLog($"{errorReadMsg} {filePath}");
                return null;
            }
        }

        /// <summary>
        /// Asynchronously saves the given content to a file in the specified subfolder.
        /// Ensures the directory exists, writes the content, and logs success or failure.
        /// </summary>
        public static async Task<bool> SaveToFile(string content, string filePath = "config.json")
        {
   
            string saveSuccessMsg = "Successfully saved file to:";
            string saveErrorMsg = "Error saving file:";

            try
            {
                await File.WriteAllTextAsync(filePath, content);
                await xyLog.AsxLog(saveSuccessMsg + $" {filePath}");
                return true;
            }
            catch (Exception ex)
            {
                await xyLog.AsxExLog(ex);
                await xyLog.AsxLog($"{saveErrorMsg} {filePath}");
                return false;
            }
        }
        public static async Task<bool> SaveBytesToFileAsync(byte[] data, string filePath = "config.json")
        {
                string content = xy.BytesToString(data);
                return await SaveToFile(content, filePath);
        }
        public static async Task<bool> SaveStringToFileAsync(string content, string subfolder = "AppData", string fileName = "config.json")
        {
            string invalidContentMsg = "Content is null or empty.";
            string invalidPathMsg    = "Subfolder or filename is null or empty.";
       

            if (string.IsNullOrEmpty(content))
            {
                await xyLog.AsxLog(invalidContentMsg);
                return false;
            }
            if (string.IsNullOrWhiteSpace(subfolder) || string.IsNullOrWhiteSpace(fileName))
            {
                await xyLog.AsxLog(invalidPathMsg);
                return false;
            }

            string directoryPath = EnsureDirectory(subfolder);
            string filePath      = xyPathHelper.Combine(directoryPath, fileName);

            return await SaveToFile(content, filePath);
        }

  


        /// <summary>
        /// Asynchronously loads the content of a file as a string.
        /// Returns null if the file doesn’t exist or an error occurs.
        /// </summary>
        public static async Task<string?> LoadFileAsync(string subfolder = "AppData", string fileName = "config.json")
        {
            string directoryPath = EnsureDirectory(subfolder);
            string filePath      = xyPathHelper.Combine(directoryPath, fileName);
            return await LoadFileAsync(filePath);
        }
        /// <summary>
        /// Asynchronously loads the content of a file as a string.
        /// Returns null if the file doesn’t exist or an error occurs.
        /// </summary>
        public static async Task<string?> LoadFileAsync(string fileName = "config.json")
        {
            string noFile = "File does not exist:";

            if (!File.Exists(fileName))
            {
                await xyLog.AsxLog($"{noFile} {fileName}");
                return null;
            }
            else
            {
                try
                {
                    string content = await File.ReadAllTextAsync(fileName);
                    await xyLog.AsxLog("Successfully loaded content from: " + fileName);
                    return content;
                }
                catch (Exception ex)
                {
                    await xyLog.AsxExLog(ex);
                    return null;
                }
            }
        }


        public static async Task<byte[]?> LoadBytesFromFile(string fullPath)
        {
            string noBytes = "No bytes to read";
            try
            {
                if (File.Exists(fullPath))
                {
                    if(await File.ReadAllBytesAsync(fullPath) is   byte[] bytes)
                    {
                        return bytes;
                    }
                    await xyLog.AsxLog(noBytes);
                    return null;
                }
                await xyLog.AsxLog($"File not found: {fullPath}");
                return null;
            }
            catch (Exception ex)
            {
                await xyLog.AsxExLog(ex);
                return null;
            }
        }
        public static async Task<byte[]?> LoadBytes(string fullPath)
        {
            string noBytes = "No bytes to read";
            byte[] bytes = [];

            if (await LoadFileAsync(fullPath) is    string content)
            {
                bytes = xy.StringToBytes(content);
                if (bytes.Length == 0)
                {
                    await xyLog.AsxLog(noBytes);
                }
            }            
            return bytes;
        }

        /// <summary>
        /// Deletes the specified file under the given subfolder.
        /// Validates inputs, ensures the directory exists, logs success or failure.
        /// </summary>
        public static bool DeleteFile(string subfolder = "AppData", string fileName = "config.json")
        {
            const string invalidPathMsg    = "Subfolder or filename is null or empty.";
            const string notFoundMsg       = "File to delete does not exist:";
            const string deleteSuccessMsg  = "File deleted successfully:";
            const string deleteErrorMsg    = "Error deleting file:";

            if (string.IsNullOrWhiteSpace(subfolder) || string.IsNullOrWhiteSpace(fileName))
            {
                xyLog.Log(invalidPathMsg);
                return false;
            }

            string directoryPath = EnsureDirectory(subfolder);
            string filePath      = xyPathHelper.Combine(directoryPath, fileName);

            if (!File.Exists(filePath))
            {
                xyLog.Log($"{notFoundMsg} {filePath}");
                return false;
            }

            try
            {
                File.Delete(filePath);
                xyLog.Log($"{deleteSuccessMsg} {filePath}");
                return true;
            }
            catch (Exception ex)
            {
                xyLog.ExLog(ex);
                xyLog.Log($"{deleteErrorMsg} {filePath}");
                return false;
            }
        }

        #endregion
    }
}
