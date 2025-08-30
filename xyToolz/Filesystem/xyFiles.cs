using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using xyToolz.Helper.Interfaces;
using xyToolz.Helper.Logging;
using xyToolz.QOL;
using xyToolz.Serialization;


#if ANDROID
using Android.Content;
using Android.App;
#endif

namespace xyToolz.Filesystem
{
    /// <summary>
    /// Provides file system utilities for reading, writing, copying, renaming,
    /// and validating files or folders across supported platforms.
    /// </summary>
    /// <remarks>
    /// <para><b>Available Features:</b></para>
    /// <list type="bullet">
    ///   <item><description>Directory management (EnsureDirectory, CheckForDirectories)</description></item>
    ///   <item><description>File path validation (EnsurePathExistsAsync)</description></item>
    ///   <item><description>File metadata manipulation (Inventory, InventoryNames, RenameFileAsync)</description></item>
    ///   <item><description>File content handling (ReadLinesAsync, SaveToFile, LoadFileAsync, DeleteFile)</description></item>
    /// </list>
    ///
    /// <para><b>Thread Safety:</b></para>
    /// All methods are static and stateless, making them inherently thread-safe.
    ///
    /// <para><b>Platform Compatibility:</b></para>
    /// Special handling for Android via conditional compilation (#if ANDROID).
    ///
    /// <para><b>Performance:</b></para>
    /// Uses asynchronous buffered I/O for file operations.
    /// Performance may vary depending on file size and underlying storage.
    ///
    /// <para><b>Configuration:</b></para>
    /// - JSON serialization via <c>xyJson.defaultJsonOptions</c>
    /// - Path resolution via <c>xyPathHelper</c>
    ///
    /// <para><b>Logging:</b></para>
    /// All operations use <c>xyLog</c> for asynchronous, structured logging.
    /// This includes both informational logs and detailed exception traces.
    ///
    /// <para><b>Limitations:</b></para>
    /// - Does not support advanced file system transactions or locking mechanisms.
    /// - Designed for simplicity and general-purpose usage only.
    ///
    /// <para><b>Example Usage:</b></para>
    /// <code>
    /// // List all file names
    /// var names = xyFiles.InventoryNames("C:\\\\Temp");
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
    /// <para><b>See Also:</b></para>
    /// <see cref="File"/>, <see cref="Directory"/>, <see cref="xyPath"/>
    /// </remarks>
    public static class xyFiles
    {
        private static readonly JsonSerializerOptions DefaultJsonOptions = xyJson.defaultJsonOptions;

        #region Directory Management

        /// <summary>
        /// Checks whether all specified directories currently exist.
        /// </summary>
        /// <remarks>
        /// <para><b>Behavior:</b></para>
        /// Returns true only if all provided paths refer to existing directories.
        /// Paths that are null or empty will return false.
        ///
        /// <para><b>Thread Safety:</b></para>
        /// Thread-safe due to stateless, read-only evaluation.
        ///
        /// <para><b>Platform Notes:</b></para>
        /// Works consistently across platforms including Windows, Linux, Android.
        ///
        /// <para><b>Performance:</b></para>
        /// Executes a simple check using <c>Directory.Exists</c> for each path.
        ///
        /// <para><b>Logging:</b></para>
        /// This method does not perform any logging. It is side-effect free.
        ///
        /// <para><b>Example:</b></para>
        /// <code>
        /// bool allExist = xyFiles.CheckForDirectories(new[] { "AppData", "Cache" });
        /// </code>
        ///
        /// <para><b>See Also:</b></para>
        /// <see cref="Directory.Exists(string)"/>
        /// </remarks>
        /// <param name="directories">An array of absolute or relative directory paths to verify.</param>
        /// <returns>True if all directories exist; otherwise, false.</returns>
        public static bool CheckForDirectories(string[] directories) =>
            directories.All(dir => Directory.Exists(dir));

        /// <summary>
        /// Resolves and returns a platform-correct path to a directory, ensuring compatibility.
        /// </summary>
        /// <remarks>
        /// <para><b>Behavior:</b></para>
        /// On Android, the path is combined with the app's internal file storage root.
        /// On other platforms, <c>xyPathHelper.EnsureDirectory</c> is used to resolve the path.
        ///
        /// <para><b>Thread Safety:</b></para>
        /// Thread-safe due to local processing and read-only resolution.
        ///
        /// <para><b>Platform Notes:</b></para>
        /// Uses <c>Application.Context.FilesDir</c> or <c>GetExternalFilesDir</c> on Android.
        /// On other platforms, delegates to <c>xyPathHelper</c>.
        ///
        /// <para><b>Logging:</b></para>
        /// This method does not log directly. It assumes responsibility lies with caller.
        ///
        /// <para><b>See Also:</b></para>
        /// </remarks>
        /// <param name="dir">The relative or absolute path to resolve.</param>
        /// <returns>The resolved and platform-correct directory path.</returns>
        private static string EnsureDirectory(string dir)
        {
#if ANDROID
            string path = Path.Combine(Android.App.Application.Context.FilesDir(null)!.AbsolutePath, dir);
            if (string.IsNullOrEmpty(path))
                path = Path.Combine(Android.App.Application.Context.GetExternalFilesDir(null)!.AbsolutePath, dir);
#else
            string path = xyPath.EnsureDirectory(dir)!;
#endif
            return path;
        }

        #endregion

        #region File Path Validation

        /// <summary>
        /// Ensures the target file path exists by creating the file if necessary.
        /// </summary>
        /// <remarks>
        /// <para><b>Behavior:</b></para>
        /// Verifies the existence of the target file path. If it does not exist, the method attempts to create it.
        /// The method logs whether the file was found or created successfully.
        ///
        /// <para><b>Thread Safety:</b></para>
        /// Thread-safe due to isolated file system access and async operation.
        ///
        /// <para><b>Platform Notes:</b></para>
        /// Fully compatible with all .NET-supported platforms.
        ///
        /// <para><b>Performance:</b></para>
        /// Uses asynchronous file I/O; suitable for runtime checks and quick creation.
        ///
        /// <para><b>Logging:</b></para>
        /// Logs one of the following:
        /// - If the path was empty
        /// - If the file already existed
        /// - If a new file was created
        /// - Any exceptions encountered
        ///
        /// <para><b>Exceptions:</b></para>
        /// All exceptions are caught and logged via <c>xyLog.AsxExLog</c>. Method returns false on failure.
        ///
        /// <para><b>Example:</b></para>
        /// <code>
        /// bool valid = await xyFiles.EnsurePathExistsAsync("data/logs/output.txt");
        /// </code>
        ///
        /// <para><b>See Also:</b></para>
        /// <see cref="File.Create(string)"/>, <see cref="xyLog"/>
        /// </remarks>
        /// <param name="filePath">The absolute or relative file path to check or create.</param>
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
        /// <remarks>
        /// <para><b>Behavior:</b></para>
        /// Combines the given path using <c>xyPathHelper</c> and retrieves all files within that directory.
        /// Logs each file path as it is processed.
        ///
        /// <para><b>Thread Safety:</b></para>
        /// Thread-safe due to isolated access and stateless usage of static APIs.
        ///
        /// <para><b>Platform Notes:</b></para>
        /// Compatible with all platforms supported by <c>System.IO.Directory</c>.
        ///
        /// <para><b>Performance:</b></para>
        /// Suitable for small to medium-sized directories. May become slow on large file sets.
        ///
        /// <para><b>Logging:</b></para>
        /// Each file's full path is logged individually using <c>xyLog.Log</c>.
        ///
        /// <para><b>Exceptions:</b></para>
        /// May throw exceptions if the path is invalid or access is denied. No internal exception handling.
        ///
        /// <para><b>Example:</b></para>
        /// <code>
        /// var files = xyFiles.Inventory("MyData").ToList();
        /// foreach (var file in files)
        ///     Console.WriteLine(file.FullName);
        /// </code>
        ///
        /// <para><b>See Also:</b></para>
        /// <see cref="FileInfo"/>, <see cref="Directory.GetFiles(string)"/>
        /// </remarks>
        /// <param name="path">The directory path to inspect.</param>
        /// <returns>A list of <see cref="FileInfo"/> objects representing the files in the directory.</returns>
        public static IEnumerable<FileInfo> Inventory(string path)
        {
            List<FileInfo> fileList = new();
            foreach (string file in Directory.GetFiles(xyPath.Combine(path)))
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
        /// <remarks>
        /// <para><b>Behavior:</b></para>
        /// Internally calls <see cref="Inventory"/> and selects each file's <c>FullName</c>.
        ///
        /// <para><b>Thread Safety:</b></para>
        /// Thread-safe as all operations are functional and stateless.
        ///
        /// <para><b>Logging:</b></para>
        /// Inherits logging behavior from <c>Inventory</c>, where each file is logged.
        ///
        /// <para><b>Exceptions:</b></para>
        /// Any exceptions thrown by <c>Inventory</c> will propagate.
        ///
        /// <para><b>Example:</b></para>
        /// <code>
        /// var names = xyFiles.InventoryNames("Assets").ToList();
        /// names.ForEach(Console.WriteLine);
        /// </code>
        ///
        /// <para><b>See Also:</b></para>
        /// <see cref="Inventory"/>
        /// </remarks>
        /// <param name="path">The directory to scan for files.</param>
        /// <returns>A list of full file paths as strings.</returns>
        public static IEnumerable<string> InventoryNames(string path) => Inventory(path).Select(f => f.FullName).ToList();

        /// <summary>
        /// Renames a file to a new name within its current directory.
        /// </summary>
        /// <remarks>
        /// <para><b>Behavior:</b></para>
        /// Validates inputs, constructs the new path, and moves the file to the new name.
        /// Returns false if the source file doesn't exist or the target file already exists.
        ///
        /// <para><b>Thread Safety:</b></para>
        /// Thread-safe, as it relies only on stateless static I/O APIs.
        ///
        /// <para><b>Logging:</b></para>
        /// Logs for:
        /// - Missing or invalid input
        /// - File not found
        /// - Target name conflict
        /// - Success or exception
        ///
        /// <para><b>Exceptions:</b></para>
        /// Any exception during file operations is caught and logged via <c>xyLog.AsxExLog</c>.
        ///
        /// <para><b>Validation:</b></para>
        /// - New file name must not contain directory separators or invalid characters.
        /// - Original path and new name must be non-empty.
        ///
        /// <para><b>Example:</b></para>
        /// <code>
        /// bool renamed = await xyFiles.RenameFileAsync("C:\\\\temp\\\\old.txt", "new.txt");
        /// if (renamed) Console.WriteLine("File renamed.");
        /// </code>
        ///
        /// <para><b>See Also:</b></para>
        /// <see cref="File.Move(string, string)"/>
        /// </remarks>
        /// <param name="completePath">The full original path of the file to rename.</param>
        /// <param name="newName">The new name for the file (not a full path).</param>
        /// <returns>True if renaming was successful; otherwise, false.</returns>
        public static async Task<bool> RenameFileAsync(string completePath, string newName)
        {
            string newPath;
            string errorMissingInput = "The provided file path or new file name is null or empty.";
            string errorInvalidName = "The new file name contains invalid characters or is a directory path.";
            string errorFileNotFound = "The original file does not exist.";
            string errorTargetExists = "A file with the target name already exists.";
            string successTemplate = "File renamed successfully from '{0}' to '{1}'.";

            if (string.IsNullOrWhiteSpace(completePath) || string.IsNullOrWhiteSpace(newName))
            {
                await xyLog.AsxLog(errorMissingInput);
                return false;
            }

            if (newName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0 || newName.Contains(Path.DirectorySeparatorChar))
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
        #endregion

        #region File Content Handling

        /// <summary>
        /// Asynchronously reads all lines from the specified file.
        /// </summary>
        /// <remarks>
        /// <para><b>Behavior:</b></para>
        /// Verifies that the file exists using <see cref="EnsurePathExistsAsync"/>. If the file is available, it reads all lines asynchronously.
        /// Returns an empty collection if the file is missing or an error occurs.
        ///
        /// <para><b>Thread Safety:</b></para>
        /// Thread-safe due to async file access with no shared state.
        ///
        /// <para><b>Logging:</b></para>
        /// Logs:
        /// - Missing file
        /// - Read success including byte count
        /// - Any exception via <c>xyLog.AsxExLog</c>
        ///
        /// <para><b>Exceptions:</b></para>
        /// All exceptions are caught and logged. No exceptions are thrown to the caller.
        ///
        /// <para><b>Performance:</b></para>
        /// Suitable for small to medium text files. Uses <c>File.ReadAllLinesAsync</c>.
        ///
        /// <para><b>Example:</b></para>
        /// <code>
        /// var lines = await xyFiles.ReadLinesAsync("data.txt");
        /// foreach (var line in lines)
        ///     Console.WriteLine(line);
        /// </code>
        ///
        /// <para><b>See Also:</b></para>
        /// </remarks>
        /// <param name="filePath">The full path to the file to read.</param>
        /// <returns>A collection of lines as strings, or an empty collection if failed.</returns>
        public static async Task<IEnumerable<string>> ReadLinesAsync(string filePath)
        {
            string noFile = $"File does not exist:{filePath}";
            string error = $"Error reading file:{filePath}";
            string success = $"Bytes have been read from {filePath}:";
            string[] allLines;

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
        /// Asynchronously loads a file into a <see cref="MemoryStream"/>.
        /// </summary>
        /// <remarks>
        /// <para><b>Behavior:</b></para>
        /// Loads the specified file as a byte array and returns a memory stream with its contents.
        /// If the file is invalid, not found, or an error occurs, null is returned.
        ///
        /// <para><b>Thread Safety:</b></para>
        /// Fully thread-safe via async operations.
        ///
        /// <para><b>Logging:</b></para>
        /// Logs:
        /// - Null or empty input path
        /// - File existence checks
        /// - Read success including byte size
        /// - Exceptions via <c>xyLog.AsxExLog</c>
        ///
        /// <para><b>Exceptions:</b></para>
        /// All exceptions are caught and logged. Method returns null on failure.
        ///
        /// <para><b>Example:</b></para>
        /// <code>
        /// var stream = await xyFiles.GetStreamFromFileAsync("logo.png");
        /// if (stream != null)
        ///     await stream.CopyToAsync(outputStream);
        /// </code>
        ///
        /// <para><b>See Also:</b></para>
        /// </remarks>
        /// <param name="filePath">The full file path to read.</param>
        /// <returns>A <see cref="Stream"/> containing the file content, or null on failure.</returns>
        public static async Task<Stream?> GetStreamFromFileAsync(string filePath)
        {
            const string invalidPathMsg = "The given file path is null or empty.";
            string notFoundMsg = "File does not exist:";
            string errorReadMsg = "Error reading file into stream:";
            string success = $"Loaded file into stream from {filePath}:";

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

                await xyLog.AsxLog($"{buffer.Length} bytes");
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
        /// Saves a plain string to a file asynchronously.
        /// </summary>
        /// <remarks>
        /// <para><b>Behavior:</b></para>
        /// Writes the provided string content to the specified path using UTF-8 encoding.
        /// The file will be overwritten if it already exists.
        ///
        /// <para><b>Thread Safety:</b></para>
        /// Thread-safe due to async file operations with no shared state.
        ///
        /// <para><b>Logging:</b></para>
        /// Logs success and failure messages via <c>xyLog.AsxLog</c>.
        /// Any exceptions are logged using <c>xyLog.AsxExLog</c>.
        ///
        /// <para><b>Exceptions:</b></para>
        /// All exceptions are caught internally. Returns false if any error occurs.
        ///
        /// <para><b>Example:</b></para>
        /// <code>
        /// bool saved = await xyFiles.SaveToFile("{ \"hello\": \"world\" }", "config.json");
        /// </code>
        ///
        /// <para><b>See Also:</b></para>
        /// </remarks>
        /// <param name="content">The string content to save.</param>
        /// <param name="filePath">The full path of the file. Defaults to "config.json".</param>
        /// <returns>True if the content was successfully saved; otherwise, false.</returns>
        public static async Task<bool> SaveToFile(string content, string filePath = "config.json")
        {

            string saveSuccessMsg = "Successfully saved file to:";
            string saveErrorMsg = "Error saving file:";

            try
            {
                await File.WriteAllTextAsync(filePath, content, new UTF8Encoding(false));
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
        /// <summary>
        /// Converts a byte array to a string and saves it to a file asynchronously.
        /// </summary>
        /// <remarks>
        /// <para><b>Behavior:</b></para>
        /// Uses <c>xy.BytesToString</c> to convert the byte array to a Base64 or UTF-8 compatible string.
        /// Delegates the actual write operation to <see cref="SaveToFile(string, string)"/>.
        ///
        /// <para><b>Thread Safety:</b></para>
        /// Thread-safe by nature of async and stateless operations.
        ///
        /// <para><b>Logging:</b></para>
        /// Logging is performed inside <c>SaveToFile</c>.
        ///
        /// <para><b>Exceptions:</b></para>
        /// All exceptions are handled inside <c>SaveToFile</c>. No exceptions thrown directly.
        ///
        /// <para><b>Example:</b></para>
        /// <code>
        /// byte[] data = Encoding.UTF8.GetBytes("Sample");
        /// await xyFiles.SaveBytesToFileAsync(data, "sample.txt");
        /// </code>
        ///
        /// <para><b>See Also:</b></para>
        /// <see cref="xy.BytesToString(byte[])"/>, <see cref="SaveToFile(string, string)"/>
        /// </remarks>
        /// <param name="data">The byte array to be converted and saved.</param>
        /// <param name="filePath">The full path of the file. Defaults to "config.json".</param>
        /// <returns>True if the content was successfully saved; otherwise, false.</returns>
        public static async Task<bool> SaveBytesToFileAsync(byte[] data, string filePath = "config.json")
        {
            string content = xy.BytesToString(data);
            return await SaveToFile(content, filePath);
        }
        /// <summary>
        /// Saves a string to a file within a specified subfolder asynchronously.
        /// </summary>
        /// <remarks>
        /// <para><b>Behavior:</b></para>
        /// Resolves the target path using <see cref="EnsureDirectory"/> and <c>xyPathHelper.Combine</c>,
        /// then writes the content using <see cref="SaveToFile"/>.
        ///
        /// <para><b>Thread Safety:</b></para>
        /// Fully thread-safe due to stateless processing and async I/O.
        ///
        /// <para><b>Validation:</b></para>
        /// Returns false if the content or path parameters are invalid (null or empty).
        ///
        /// <para><b>Logging:</b></para>
        /// Logs validation failures and delegates logging of I/O to <c>SaveToFile</c>.
        ///
        /// <para><b>Exceptions:</b></para>
        /// All exceptions are caught and handled internally. Returns false on error.
        ///
        /// <para><b>Example:</b></para>
        /// <code>
        /// await xyFiles.SaveStringToFileAsync("Hello!", "AppData", "hello.txt");
        /// </code>
        ///
        /// <para><b>See Also:</b></para>
        /// </remarks>
        /// <param name="content">The content to be written to the file.</param>
        /// <param name="subfolder">Subdirectory where the file will be placed.</param>
        /// <param name="fileName">Name of the file to create or overwrite.</param>
        /// <returns>True if the file was saved successfully; otherwise, false.</returns>
        public static async Task<bool> SaveStringToFileAsync(string content, string subfolder = "AppData", string fileName = "config.json")
        {
            string invalidContentMsg = "Content is null or empty.";
            string invalidPathMsg = "Subfolder or filename is null or empty.";


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
            string filePath = xyPath.Combine(directoryPath, fileName);

            return await SaveToFile(content, filePath);
        }



        /// <summary>
        /// Asynchronously loads the content of a file as a string from a specific subfolder.
        /// </summary>
        /// <remarks>
        /// <para><b>Behavior:</b></para>
        /// Resolves the full path from <paramref name="subfolder"/> and <paramref name="fileName"/> using <see cref="EnsureDirectory"/> and <c>xyPathHelper.Combine</c>,
        /// and delegates the actual read operation to <see cref="TestLoadFileAsync(string)"/>.
        ///
        /// <para><b>Thread Safety:</b></para>
        /// Thread-safe due to async and stateless file access.
        ///
        /// <para><b>Logging:</b></para>
        /// Logging is performed inside the delegated <c>LoadFileAsync</c> method.
        ///
        /// <para><b>Example:</b></para>
        /// <code>
        /// string? config = await xyFiles.LoadFileAsync("AppData", "config.json");
        /// </code>
        ///
        /// <para><b>See Also:</b></para>
        /// <see cref="TestLoadFileAsync(string)"/>
        /// </remarks>
        /// <param name="subfolder">The folder that contains the file.</param>
        /// <param name="fileName">The name of the file to load.</param>
        /// <returns>The content of the file as a string, or null if it fails.</returns>
        public static async Task<string?> LoadFileAsync(string subfolder = "AppData", string fileName = "config.json")
        {
            string directoryPath = EnsureDirectory(subfolder);
            string filePath = xyPath.Combine(directoryPath, fileName);
            return await LoadFileAsync(filePath);
        }
        /// <summary>
        /// Asynchronously loads the content of a file as a string from a full path.
        /// </summary>
        /// <remarks>
        /// <para><b>Behavior:</b></para>
        /// Returns the full file content as a string, or null if the file does not exist or cannot be read.
        ///
        /// <para><b>Thread Safety:</b></para>
        /// Thread-safe through async and stateless file handling.
        ///
        /// <para><b>Logging:</b></para>
        /// Logs:
        /// - File not found
        /// - Successful read
        /// - Any exception during reading
        ///
        /// <para><b>Exceptions:</b></para>
        /// All exceptions are caught and logged. Method returns null on error.
        ///
        /// <para><b>Example:</b></para>
        /// <code>
        /// string? text = await xyFiles.LoadFileAsync("notes.txt");
        /// </code>
        ///
        /// <para><b>See Also:</b></para>
        /// </remarks>
        /// <param name="fileName">The full file path to read.</param>
        /// <returns>The file content as string, or null on failure.</returns>
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

        /// <summary>
        /// Loads the raw bytes from a file located at the given path.
        /// </summary>
        /// <remarks>
        /// <para><b>Behavior:</b></para>
        /// If the file exists, reads all bytes and returns them.
        /// If the file doesn't exist or reading fails, returns null.
        ///
        /// <para><b>Thread Safety:</b></para>
        /// Thread-safe via async stateless file access.
        ///
        /// <para><b>Logging:</b></para>
        /// Logs file absence, empty content, and exceptions.
        ///
        /// <para><b>Exceptions:</b></para>
        /// All exceptions are caught and logged. No exception is thrown.
        ///
        /// <para><b>Example:</b></para>
        /// <code>
        /// byte[]? data = await xyFiles.LoadBytesFromFile("blob.bin");
        /// </code>
        ///
        /// <para><b>See Also:</b></para>
        /// </remarks>
        /// <param name="fullPath">Full path to the file.</param>
        /// <returns>Byte array if successful; otherwise, null.</returns>
        public static async Task<byte[]?> LoadBytesFromFile(string fullPath)
        {
            string noBytes = "No bytes to read";
            try
            {
                if (File.Exists(fullPath))
                {
                    if (await File.ReadAllBytesAsync(fullPath) is byte[] bytes)
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
        /// <summary>
        /// Loads a file as a string and converts it into a byte array.
        /// </summary>
        /// <remarks>
        /// <para><b>Behavior:</b></para>
        /// Reads the string content from a file and converts it using <c>xy.StringToBytes</c>.
        /// Returns an empty byte array if the file is empty or unreadable.
        ///
        /// <para><b>Thread Safety:</b></para>
        /// Thread-safe through functional composition and stateless processing.
        ///
        /// <para><b>Logging:</b></para>
        /// Logs missing bytes or conversion failures.
        ///
        /// <para><b>Example:</b></para>
        /// <code>
        /// byte[] bytes = await xyFiles.LoadBytes("file.txt");
        /// </code>
        ///
        /// <para><b>See Also:</b></para>
        /// <see cref="xy.StringToBytes(string)"/>, <see cref="TestLoadFileAsync(string)"/>
        /// </remarks>
        /// <param name="fullPath">The full path of the file to load.</param>
        /// <returns>A byte array derived from the file content, or an empty array.</returns>
        public static async Task<byte[]?> ReadBytes(string fullPath)
        {
            string noBytes = "No bytes to read";
            byte[] bytes = [];

            if (await LoadFileAsync(fullPath) is string content)
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
        /// Deletes a file from the specified subfolder.
        /// </summary>
        /// <remarks>
        /// <para><b>Behavior:</b></para>
        /// Resolves the full file path and attempts to delete it.
        /// Returns false if the path is invalid, the file is missing, or deletion fails.
        ///
        /// <para><b>Thread Safety:</b></para>
        /// Thread-safe through isolated file operations.
        ///
        /// <para><b>Logging:</b></para>
        /// Logs:
        /// - Invalid input
        /// - File not found
        /// - Success or exception
        ///
        /// <para><b>Exceptions:</b></para>
        /// Exceptions are caught and logged. Method returns false on error.
        ///
        /// <para><b>Example:</b></para>
        /// <code>
        /// bool deleted = xyFiles.DeleteFile("Temp", "old.json");
        /// </code>
        ///
        /// <para><b>See Also:</b></para>
        /// <see cref="File.Delete(string)"/>
        /// </remarks>
        /// <param name="subfolder">The folder where the file is located.</param>
        /// <param name="fileName">The file name to delete.</param>
        /// <returns>True if the file was deleted successfully; otherwise, false.</returns>
        public static bool DeleteFile(string subfolder = "AppData", string fileName = "config.json")
        {
            const string invalidPathMsg = "Subfolder or filename is null or empty.";
            const string notFoundMsg = "File to delete does not exist:";
            const string deleteSuccessMsg = "File deleted successfully:";
            const string deleteErrorMsg = "Error deleting file:";

            if (string.IsNullOrWhiteSpace(subfolder) || string.IsNullOrWhiteSpace(fileName))
            {
                xyLog.Log(invalidPathMsg);
                return false;
            }

            string directoryPath = EnsureDirectory(subfolder);
            string filePath = xyPath.Combine(directoryPath, fileName);

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

        #region Tests
        private static IxyFiles? _override;
        public static void OverrideForTests(IxyFiles mocked) => _override = mocked;
        public static void ResetOverride() => _override = null;

        public static Task<string?> TestLoadFileAsync(string subfolder = "AppData", string fileName = "config.json")   =>  _override?.LoadFileAsync(subfolder, fileName)?? LoadFileAsync(subfolder, fileName);

        public static Task<string?> TestLoadFileAsync(string fullPath)   =>  _override?.LoadFileAsync(fullPath) ?? LoadFileAsync(fullPath);
        #endregion

    }
}
