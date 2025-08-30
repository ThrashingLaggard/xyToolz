# class xyDirectoryHelper

Namespace: `xyToolz.Filesystem`  
Visibility: `public static`  
Attribute: `System.Diagnostics.CodeAnalysis.SuppressMessage`  
Source: `xyToolz\Filesystem\xyDirectoryHelper.cs`

## Description:

/// Directory helpers
    ///

## Methods

- `bool DeleteFolder(string folderPath)` — `public static`
  
  /// Löscht den angegebenen Ordner mitsamt allen Inhalten (rekursiv).
        /// Gibt true zurück, wenn der Ordner erfolgreich gelöscht wurde, andernfalls false.
        ///
- `bool IsFolderEmpty(string folderPath)` — `public static`
  
  /// Prüft, ob ein Ordner leer ist (d.h. keine Dateien oder Unterordner enthält).
        /// Falls der Ordner nicht existiert, wird true zurückgegeben.
        ///
- `FileSystemWatcher MonitorFolder(string folderPath, string filter = "*.*", Action<object, FileSystemEventArgs>? onChanged = null)` — `public static`
  
  /// Überwacht einen Ordner (und optional alle Unterordner) auf Änderungen.
        /// Die Methode richtet einen FileSystemWatcher ein, der bei Dateiänderungen, -erstellungen, -löschungen oder -umbenennungen
        /// das angegebene Callback (onChanged) aufruft.
        ///
- `long GetFolderSize(string folderPath)` — `public static`
  
  /// Berechnet die Gesamtgröße eines Ordners inklusive aller Unterordner und Dateien.
        ///
- `string EnsureSubfolderExists(string basePath, string subfolder, bool createIfNotExist = true)` — `public static`
  
  /// Make sure that this  specific Subfolder exists
        ///
- `string GetApplicationFolder()` — `public static`
  
  /// Get the directory containing the application
        ///
- `string GetInnerApplicationFolder()` — `public static`
  
  /// While debugging in C# use this to get the app directory
        ///
- `string GetSolutionFolder()` — `public static`
  
  /// Get the full path of the directory containing this programs     .sln    file
        ///
- `string GetSubfolderPath(string basePath, string subfolder, bool createIfNotExist = false)` — `public static`
  
  /// Gibt den vollständigen Pfad eines Unterordners zurück. Optional kann der Ordner erstellt werden, falls er nicht existiert.
        ///
- `string MoveFolder(string sourceFolder, string destinationFolder)` — `public static`
  
  /// Verschiebt einen Ordner in einen anderen Zielordner.
        ///
- `string RenameFolder(string currentFolderPath, string newFolderName)` — `public static`
  
  /// Bennennt einen Ordner um.
        ///
- `string[] GetFiles(string folderPath, string searchPattern = "*.*", bool includeSubdirectories = false)` — `public static`
  
  /// Gibt alle Dateien im angegebenen Ordner zurück. Mit einem Suchmuster und optionaler Rekursion in Unterordnern.
        ///
- `string[] GetSubfolders(string folderPath)` — `public static`
  
  /// Gibt alle Unterordner des angegebenen Ordners zurück.
        ///
- `void ClearFolder(string folderPath)` — `public static`
  
  /// Löscht den Inhalt eines Ordners, lässt den Ordner aber bestehen.
        ///
- `void CompressFolder(string sourceFolder, string zipFilePath, bool includeBaseDirectory = false)` — `public static`
  
  /// Komprimiert den angegebenen Quellordner in eine ZIP-Datei.
        /// Falls die ZIP-Datei bereits existiert, wird sie überschrieben.
        ///
- `void CopyFolder(string sourceFolder, string destinationFolder, bool overwrite = false)` — `public static`
  
  /// Kopiert alle Dateien und Unterordner vom Quellordner in den Zielordner (rekursiv).
        ///
- `void ExtractFolder(string zipFilePath, string extractFolder)` — `public static`
  
  /// Extrahiert eine ZIP-Datei in den angegebenen Zielordner.
        /// Existiert der Zielordner nicht, wird er erstellt.
        ///

