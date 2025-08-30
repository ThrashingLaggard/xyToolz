# class xyFiles

Namespace: `xyToolz`  
Visibility: `public static`  
Source: `xyToolz\xyFiles.cs`

## Description:

/// Provides file system utilities for reading, writing, copying, renaming,
    /// and validating files or folders across supported platforms.
    ///

## Methoden

- `bool CheckForDirectories(string[] directories)` — `public static`
  
  /// Checks whether all specified directories currently exist.
        ///
- `bool DeleteFile(string subfolder = "AppData", string fileName = "config.json")` — `public static`
  
  /// Deletes a file from the specified subfolder.
        ///
- `IEnumerable<FileInfo> Inventory(string path)` — `public static`
  
  /// Returns a list of  objects for all files in the given directory.
        ///
- `IEnumerable<string> InventoryNames(string path)` — `public static`
  
  /// Returns a list of full file names (paths) from the specified directory.
        ///
- `Task<bool> EnsurePathExistsAsync(string filePath)` — `public static async`
  
  /// Ensures the target file path exists by creating the file if necessary.
        ///
- `Task<bool> RenameFileAsync(string completePath, string newName)` — `public static async`
  
  /// Renames a file to a new name within its current directory.
        ///
- `Task<bool> SaveBytesToFileAsync(byte[] data, string filePath = "config.json")` — `public static async`
  
  /// Converts a byte array to a string and saves it to a file asynchronously.
        ///
- `Task<bool> SaveStringToFileAsync(string content, string subfolder = "AppData", string fileName = "config.json")` — `public static async`
  
  /// Saves a string to a file within a specified subfolder asynchronously.
        ///
- `Task<bool> SaveToFile(string content, string filePath = "config.json")` — `public static async`
  
  /// Saves a plain string to a file asynchronously.
        ///
- `Task<byte[]?> LoadBytesFromFile(string fullPath)` — `public static async`
  
  /// Loads the raw bytes from a file located at the given path.
        ///
- `Task<byte[]?> ReadBytes(string fullPath)` — `public static async`
  
  /// Loads a file as a string and converts it into a byte array.
        ///
- `Task<IEnumerable<string>> ReadLinesAsync(string filePath)` — `public static async`
  
  /// Asynchronously reads all lines from the specified file.
        ///
- `Task<Stream?> GetStreamFromFileAsync(string filePath)` — `public static async`
  
  /// Asynchronously loads a file into a .
        ///
- `Task<string?> LoadFileAsync(string fileName = "config.json")` — `public static async`
  
  /// Asynchronously loads the content of a file as a string from a full path.
        ///
- `Task<string?> LoadFileAsync(string subfolder = "AppData", string fileName = "config.json")` — `public static async`
  
  /// Asynchronously loads the content of a file as a string from a specific subfolder.
        ///
- `Task<string?> TestLoadFileAsync(string fullPath)` — `public static`
  
  (No XML‑Summary )
- `Task<string?> TestLoadFileAsync(string subfolder = "AppData", string fileName = "config.json")` — `public static`
  
  (No XML‑Summary )
- `void OverrideForTests(IxyFiles mocked)` — `public static`
  
  (No XML‑Summary )
- `void ResetOverride()` — `public static`
  
  (No XML‑Summary )

