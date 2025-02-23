using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xyToolz
{
    public static class xyFolder
    {
        /// <summary>
        /// Stellt sicher, dass der Unterordner existiert. Existiert er nicht, wird er erstellt.
        /// </summary>
        /// <param name="basePath">Der Basisordner, z. B. FileSystem.AppDataDirectory oder AppContext.BaseDirectory.</param>
        /// <param name="subfolder">Der Name des gewünschten Unterordners.</param>
        /// <returns>Der vollständige Pfad zum Unterordner.</returns>
        public static string EnsureSubfolderExists(string basePath, string subfolder, bool createIfNotExist = true)
        {
            string folderPath = Path.Combine(basePath, subfolder);
            try
            {
                if (createIfNotExist && !Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
            }
            catch (Exception e)
            {
                xyLog.ExLog(e);
            }
            return folderPath;
        }


        /// <summary>
        /// Gibt den vollständigen Pfad eines Unterordners zurück. Optional kann der Ordner erstellt werden, falls er nicht existiert.
        /// </summary>
        /// <param name="basePath">Das Basisverzeichnis.</param>
        /// <param name="subfolder">Der Name des Unterordners.</param>
        /// <param name="createIfNotExist">Falls true, wird der Ordner erstellt, wenn er nicht existiert.</param>
        /// <returns>Vollständiger Pfad zum Unterordner.</returns>
        public static string GetSubfolderPath(string basePath, string subfolder, bool createIfNotExist = false)
        {
            string folderPath = Path.Combine(basePath, subfolder);
            if (createIfNotExist && !Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            return folderPath;
        }

        /// <summary>
        /// Löscht den angegebenen Ordner mitsamt allen Inhalten (rekursiv).
        /// Gibt true zurück, wenn der Ordner erfolgreich gelöscht wurde, andernfalls false.
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
                    // Optional: Fehler protokollieren oder weiter verarbeiten
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
                    string parentPath = Directory.GetParent(currentFolderPath)?.FullName;
                    if (parentPath == null)
                        return null;

                    string newFolderPath = Path.Combine(parentPath, newFolderName);
                    Directory.Move(currentFolderPath, newFolderPath);
                    return newFolderPath;
                }
                catch (Exception e)
                {
                    xyLog.ExLog(e);
                }
            }
            return null;
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
            return null;
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
        /// Gibt alle Unterordner des angegebenen Ordners zurück.
        /// </summary>
        /// <param name="folderPath">Der Ordner, dessen Unterordner gesucht werden.</param>
        /// <returns>Array der Unterordnerpfade.</returns>
        public static string[] GetSubfolders(string folderPath)
        {
            if (Directory.Exists(folderPath))
            {
                try
                {
                    return Directory.GetDirectories(folderPath);
                }
                catch (Exception e)
                {
                    xyLog.ExLog(e);
                }
            }
            return Array.Empty<string>();
        }

        /// <summary>
        /// Berechnet die Gesamtgröße eines Ordners inklusive aller Unterordner und Dateien.
        /// </summary>
        /// <param name="folderPath">Der zu messende Ordnerpfad.</param>
        /// <returns>Die Gesamtgröße in Bytes.</returns>
        public static long GetFolderSize(string folderPath)
        {
            long size = 0;
            if (Directory.Exists(folderPath))
            {
                try
                {
                    // Alle Dateien im aktuellen Ordner summieren
                    size += Directory.GetFiles(folderPath).Sum(file => new FileInfo(file).Length);

                    // Rekursiv Größe der Unterordner ermitteln
                    foreach (var dir in Directory.GetDirectories(folderPath))
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
        /// Komprimiert den angegebenen Quellordner in eine ZIP-Datei.
        /// Falls die ZIP-Datei bereits existiert, wird sie überschrieben.
        /// </summary>
        /// <param name="sourceFolder">Der Ordner, der komprimiert werden soll.</param>
        /// <param name="zipFilePath">Der vollständige Pfad der zu erstellenden ZIP-Datei.</param>
        /// <param name="includeBaseDirectory">
        /// Gibt an, ob der Basisordner (also der Name des Quellordners) in der ZIP-Struktur enthalten sein soll.
        /// </param>
        public static void CompressFolder(string sourceFolder, string zipFilePath, bool includeBaseDirectory = false)
        {
            try
            {
                if (!Directory.Exists(sourceFolder))
                    throw new DirectoryNotFoundException($"Source folder not found: {sourceFolder}");

                // Überschreibt ggf. eine vorhandene ZIP-Datei
                if (File.Exists(zipFilePath))
                    File.Delete(zipFilePath);

                // Erstelle die ZIP-Datei
                ZipFile.CreateFromDirectory(sourceFolder, zipFilePath, CompressionLevel.Fastest, includeBaseDirectory);
            }
            catch (Exception ex)
            {
                xyLog.ExLog(ex);
            }
        }

        /// <summary>
        /// Extrahiert eine ZIP-Datei in den angegebenen Zielordner.
        /// Existiert der Zielordner nicht, wird er erstellt.
        /// </summary>
        /// <param name="zipFilePath">Der Pfad der ZIP-Datei.</param>
        /// <param name="extractFolder">Der Ordner, in den die ZIP-Datei extrahiert werden soll.</param>
        public static void ExtractFolder(string zipFilePath, string extractFolder)
        {
            try
            {
                if (!File.Exists(zipFilePath))
                    throw new FileNotFoundException($"Zip file not found: {zipFilePath}");

                // Erstelle den Zielordner, falls er nicht existiert.
                Directory.CreateDirectory(extractFolder);

                // Extrahiere den Inhalt der ZIP-Datei
                ZipFile.ExtractToDirectory(zipFilePath, extractFolder, overwriteFiles: true);
            }
            catch (Exception ex)
            {
                xyLog.ExLog(ex);
            }
        }

        /// <summary>
        /// Überwacht einen Ordner (und optional alle Unterordner) auf Änderungen.
        /// Die Methode richtet einen FileSystemWatcher ein, der bei Dateiänderungen, -erstellungen, -löschungen oder -umbenennungen
        /// das angegebene Callback (onChanged) aufruft.
        /// </summary>
        /// <param name="folderPath">Der zu überwachende Ordnerpfad.</param>
        /// <param name="filter">
        /// Das Filtermuster für zu überwachende Dateien (z. B. "*.json" oder "*.*"). 
        /// Standard ist "*.*".
        /// </param>
        /// <param name="onChanged">
        /// Callback, das aufgerufen wird, wenn eine Änderung festgestellt wird. 
        /// Parameter: sender (object), FileSystemEventArgs.
        /// </param>
        /// <returns>
        /// Den eingerichteten FileSystemWatcher. Wichtig: Der Aufrufer ist dafür verantwortlich, den Watcher bei Nichtgebrauch wieder zu Dispose()en.
        /// </returns>
        public static FileSystemWatcher MonitorFolder(string folderPath, string filter = "*.*", Action<object, FileSystemEventArgs> onChanged = null)
        {
            try
            {
                if (!Directory.Exists(folderPath))
                    throw new DirectoryNotFoundException($"Folder not found: {folderPath}");

                FileSystemWatcher watcher = new FileSystemWatcher(folderPath, filter)
                {
                    NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.LastWrite | NotifyFilters.CreationTime,
                    IncludeSubdirectories = true,
                    EnableRaisingEvents = true
                };

                // Standardmäßige Event-Handler, die alle vier Eventtypen behandeln
                watcher.Changed += (s, e) => onChanged?.Invoke(s, e);
                watcher.Created += (s, e) => onChanged?.Invoke(s, e);
                watcher.Deleted += (s, e) => onChanged?.Invoke(s, e);
                watcher.Renamed += (s, e) => onChanged?.Invoke(s, e);

                return watcher;
            }
            catch (Exception ex)
            {
                xyLog.ExLog(ex);
                return null;
            }
        }
    }
}
