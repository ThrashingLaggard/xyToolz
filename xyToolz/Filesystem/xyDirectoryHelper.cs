using System.IO.Compression;
using xyToolz.Helper.Logging;

namespace xyToolz.Filesystem
{
    /// <summary>
    /// Directory helpers
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Benennungsstile", Justification = "<I want it that way>")]
    public static class xyDirectoryHelper
    {
        /// <summary>
        /// Get the full path of the directory containing this programs     .sln    file
        /// </summary>
        /// <returns></returns>
        public static string GetSolutionFolder() => Directory.GetParent(GetInnerApplicationFolderDebug())!.FullName;

        /// <summary>
        /// Get the directory containing the application
        /// </summary>
        /// <returns></returns>
        public static string GetApplicationFolder() => Environment.CurrentDirectory;

        /// <summary>
        /// While debugging in C# use this to get the app directory
        /// </summary>
        /// <returns></returns>
        public static string GetInnerApplicationFolderDebug()
        {
            string net8_0 = Environment.CurrentDirectory; xyLog.Log(net8_0);

            string debug = Directory.GetParent(net8_0)!.FullName; xyLog.Log(debug);

            string bin = Directory.GetParent(debug)!.FullName; xyLog.Log(bin);

            string appFolder = Directory.GetParent(bin)!.FullName; xyLog.Log(appFolder);

            return appFolder;
        }

        /// <summary>
        /// Deleting the target folder and its contents (recursive).
        /// True: deleted, false : not deleted.
        /// </summary>
        /// <param name="folderPath">Pfad des zu löschenden Ordners.</param>
        public static bool DeleteFolder(string folderPath)
        {
            if (Directory.Exists(folderPath))
            {
                try
                {
                    Directory.Delete(folderPath, true);
                    return true;
                }
                catch (Exception ex)
                {
                    xyLog.ExLog(ex);
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// Gibt alle Dateien im angegebenen Ordner zurück. Mit einem Suchmuster und optionaler Rekursion in Unterordnern.
        /// </summary>
        /// <param name="folderPath">Der zu durchsuchende Ordner.</param>
        /// <param name="searchPattern">Das Suchmuster (Standard: "*.*").</param>
        /// <param name="includeSubdirectories">Falls true, werden auch Dateien aus Unterordnern zurückgegeben.</param>
        /// <returns>Array von Dateipfaden.</returns>
        public static string[] GetFiles(string folderPath, string searchPattern = "*.*", bool includeSubdirectories = false)
        {
            if (Directory.Exists(folderPath))
            {
                var option = includeSubdirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
                return Directory.GetFiles(folderPath, searchPattern, option);
            }
            return Array.Empty<string>();
        }

        /// <summary>
        /// Prüft, ob ein Ordner leer ist (d.h. keine Dateien oder Unterordner enthält).
        /// Falls der Ordner nicht existiert, wird true zurückgegeben.
        /// </summary>
        /// <param name="folderPath">Der zu prüfende Ordnerpfad.</param>
        /// <returns>True, wenn der Ordner leer ist oder nicht existiert, sonst false.</returns>
        public static bool IsFolderEmpty(string folderPath)
        {
            if (Directory.Exists(folderPath))
            {
                return !Directory.EnumerateFileSystemEntries(folderPath).Any();
            }
            return true;
        }

        /// <summary>
        /// Kopiert alle Dateien und Unterordner vom Quellordner in den Zielordner (rekursiv).
        /// </summary>
        /// <param name="sourceFolder">Der Quellordner.</param>
        /// <param name="destinationFolder">Der Zielordner.</param>
        /// <param name="overwrite">Falls true, werden vorhandene Dateien überschrieben.</param>
        public static void CopyFolder(string sourceFolder, string destinationFolder, bool overwrite = false)
        {
            if (!Directory.Exists(sourceFolder))
                throw new DirectoryNotFoundException($"Source folder not found: {sourceFolder}");

            // Erstelle den Zielordner, falls nicht vorhanden.
            Directory.CreateDirectory(destinationFolder);

            // Kopiere alle Dateien im aktuellen Ordner.
            foreach (var file in Directory.GetFiles(sourceFolder))
            {
                string destFile = Path.Combine(destinationFolder, Path.GetFileName(file));
                File.Copy(file, destFile, overwrite);
            }

            // Rekursives Kopieren der Unterordner.
            foreach (var subfolder in Directory.GetDirectories(sourceFolder))
            {
                string destSubfolder = Path.Combine(destinationFolder, Path.GetFileName(subfolder));
                CopyFolder(subfolder, destSubfolder, overwrite);
            }
        }


        /// <summary>
        /// Bennennt einen Ordner um.
        /// </summary>
        /// <param name="currentFolderPath">Der aktuelle Ordnerpfad.</param>
        /// <param name="newFolderName">Der neue Name für den Ordner.</param>
        /// <returns>Der neue Ordnerpfad, oder null bei einem Fehler.</returns>
        public static string RenameFolder(string currentFolderPath, string newFolderName)
        {
            if (Directory.Exists(currentFolderPath))
            {
                try
                {
                    string? parentPath = Directory.GetParent(currentFolderPath)?.FullName;
                    if (parentPath == null)
                        return null!;

                    string newFolderPath = Path.Combine(parentPath, newFolderName);
                    Directory.Move(currentFolderPath, newFolderPath);
                    return newFolderPath;
                }
                catch (Exception e)
                {
                    xyLog.ExLog(e);
                }
            }
            return null!;
        }

        /// <summary>
        /// Verschiebt einen Ordner in einen anderen Zielordner.
        /// </summary>
        /// <param name="sourceFolder">Der Quellordnerpfad.</param>
        /// <param name="destinationFolder">Der Zielordnerpfad, in den der Ordner verschoben werden soll.</param>
        /// <returns>Der neue Pfad des verschobenen Ordners, oder null bei einem Fehler.</returns>
        public static string MoveFolder(string sourceFolder, string destinationFolder)
        {
            if (Directory.Exists(sourceFolder))
            {
                try
                {
                    // Stelle sicher, dass der Zielordner existiert
                    Directory.CreateDirectory(destinationFolder);

                    string folderName = new DirectoryInfo(sourceFolder).Name;
                    string destinationPath = Path.Combine(destinationFolder, folderName);

                    Directory.Move(sourceFolder, destinationPath);
                    return destinationPath;
                }
                catch (Exception e)
                {
                    xyLog.ExLog(e);
                }
            }
            return null!;
        }

        /// <summary>
        /// Löscht den Inhalt eines Ordners, lässt den Ordner aber bestehen.
        /// </summary>
        /// <param name="folderPath">Der Ordner, dessen Inhalt gelöscht werden soll.</param>
        public static void ClearFolder(string folderPath)
        {
            if (Directory.Exists(folderPath))
            {
                try
                {
                    // Dateien löschen
                    foreach (var file in Directory.GetFiles(folderPath))
                    {
                        File.Delete(file);
                    }

                    // Unterordner löschen
                    foreach (var dir in Directory.GetDirectories(folderPath))
                    {
                        Directory.Delete(dir, true);
                    }
                }
                catch (Exception e)
                {
                    xyLog.ExLog(e);
                }
            }
        }

        /// <summary>
        /// Returns all subfolders
        /// </summary>
        /// <param name="folderPath">Target folder.</param>
        /// <returns>Array of subfolders or empty array if none</returns>
        public static string[] GetSubfolders(string folderPath)
        {
            try
            {
                return (Directory.Exists(folderPath))? Directory.GetDirectories(folderPath) : Array.Empty<string>();
            }
            catch (Exception e)
            {
                xyLog.ExLog(e);
                return Array.Empty<string>();
            }
        }

        /// <summary>
        /// Calculates the total size of a folder, including all subfolders and files.
        /// </summary>
        /// <param name="folderPath">Target folder</param>
        /// <returns>Die Gesamtgröße in Bytes.</returns>
        public static long GetFolderSize(string folderPath)
        {
            long size = 0;
            if (Directory.Exists(folderPath))
            {
                try
                {                    
                    size += Directory.GetFiles(folderPath).Sum(file => new FileInfo(file).Length);

                    foreach (string dir in Directory.GetDirectories(folderPath))
                    {
                        size += GetFolderSize(dir);
                    }
                }
                catch (Exception e)
                {
                    xyLog.ExLog(e);
                }
            }
            return size;
        }

        /// <summary>
        /// Compress fodler into ZIP-file.
        /// Overwrites old 
        /// </summary>
        /// <param name="sourceFolder">Target folder.</param>
        /// <param name="zipFilePath">Path of the ZIP-file.</param>
        /// <param name="includeBaseDirectory">Choose whether to include the parent directory in zip structure </param>
        public static void CompressFolder(string sourceFolder, string zipFilePath, bool includeBaseDirectory = false)
        {
            try
            {
                if (!Directory.Exists(sourceFolder))    throw new DirectoryNotFoundException($"Source folder not found: {sourceFolder}");
                
                if (File.Exists(zipFilePath))   File.Delete(zipFilePath);

                ZipFile.CreateFromDirectory(sourceFolder, zipFilePath, CompressionLevel.Fastest, includeBaseDirectory);
            }
            catch (Exception ex)
            {
                xyLog.ExLog(ex);
            }
        }

        /// <summary>
        /// Extract a ZIP-file into the target folder.
        /// Creates the folder, if necessary.
        /// </summary>
        /// <param name="zipFilePath">Path of ZIP-file.</param>
        /// <param name="extractFolder">Target folder to extract the ZIP in</param>
        public static void ExtractFolder(string zipFilePath, string extractFolder)
        {
            try
            {
                if (!File.Exists(zipFilePath))  throw new FileNotFoundException($"Zip file not found: {zipFilePath}");

                Directory.CreateDirectory(extractFolder);

                ZipFile.ExtractToDirectory(zipFilePath, extractFolder, overwriteFiles: true);
            }
            catch (Exception ex)
            {
                xyLog.ExLog(ex);
            }
        }

        /// <summary>
        /// Monitors a folder (and optionally all subfolders) for changes. 
        /// The method invokes the specified callback (onChanged) on file changes, creations, deletions, or renamings.
        /// </summary>
        /// <param name="folderPath">Path of target folder.</param>
        /// <param name="filter">(z. B. "*.json" oder "*.*") ==> Standard is "*.*" </param>
        /// <param name="onChanged"> Callback, in case of changes </param>
        /// <returns>
        /// Ready FileSystemWatcher... Dispose it, when not needed anymore!!!
        /// </returns>
        public static FileSystemWatcher MonitorFolder(string folderPath, string filter = "*.*", Action<object, FileSystemEventArgs>? onChanged = null)
        {
            try
            {
                if (!Directory.Exists(folderPath))
                    throw new DirectoryNotFoundException($"Folder not found: {folderPath}");

                FileSystemWatcher watcher = new(folderPath, filter)
                {
                    NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.LastWrite | NotifyFilters.CreationTime,
                    IncludeSubdirectories = true,
                    EnableRaisingEvents = true
                };

                watcher.Changed += (s, e) => onChanged?.Invoke(s, e);
                watcher.Created += (s, e) => onChanged?.Invoke(s, e);
                watcher.Deleted += (s, e) => onChanged?.Invoke(s, e);
                watcher.Renamed += (s, e) => onChanged?.Invoke(s, e);

                return watcher;
            }
            catch (Exception ex)
            {
                xyLog.ExLog(ex);
                return null!;
            }
        }

        /// <summary>
        /// Computes a reasonable default source root:
        /// - In DEBUG builds, walk up from /bin/Debug/... to approximate the repo root.
        /// - In RELEASE builds, use the current working directory.
        /// </summary>
        private static string GetDefaultRoot()
        {
#if DEBUG
            // project directory: /bin/Debug/... → step up to repo root-ish
            var cwd = Directory.GetCurrentDirectory();
            var d = Directory.GetParent(cwd);
            if (d?.Parent?.Parent != null)
                return d.Parent.Parent.FullName;
            return cwd;
#else
            return Environment.CurrentDirectory;
#endif
        }


    }
}
