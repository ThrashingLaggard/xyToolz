# Class Documentation: `xyFiles`

## Overview
Static utility class that provides cross-platform file and directory handling features and qol stuff for .NET applications. 
Its designed to simplify common I/O tasks such as reading, writing, renaming, validating, and deleting files or directories.

---

## Features

- **Directory Management**
  - `EnsureDirectory(dir)`
  - `CheckForDirectories(string[])`

- **Path Validation**
  - `EnsurePathExistsAsync(path)`

- **File Metadata & Manipulation**
  - `Inventory(path)`
  - `InventoryNames(path)`
  - `RenameFileAsync(completePath, newName)`

- **File Content Handling**
  - `ReadLinesAsync(path)`
  - `GetStreamFromFileAsync(path)`
  - `SaveToFile(content, path)`
  - `SaveBytesToFileAsync(data, path)`
  - `SaveStringToFileAsync(content, subfolder, fileName)`
  - `LoadFileAsync(subfolder, fileName)`
  - `LoadFileAsync(filePath)`
  - `LoadBytesFromFile(fullPath)`
  - `LoadBytes(fullPath)`
  - `DeleteFile(subfolder, fileName)`

---

## Thread Safety
All methods in `xyFiles` are declared as `static`, making them inherently thread-safe as they do not maintain any shared mutable state.

---

## Platform Compatibility
Platform-specific behavior is handled via conditional compilation:
- On **Android**, file paths are resolved using `Application.Context.FilesDir`.
- On **non-Android** platforms, helper classes like `xyPathHelper` are used for directory resolution.

---

## Configuration & Dependencies

- **Logging**: All operations make use of `xyLog`, supporting both regular and exception-aware asynchronous logging.
- **Serialization**: Relies on `xyJson.defaultJsonOptions` for consistent JSON operations.
- **Path Handling**: Uses `xyPathHelper.Combine(...)` for reliable path creation.
- **Data Conversion**: Uses `xy.BytesToString` and `xy.StringToBytes` for content serialization.

---

## Example Usage

```csharp
// List all files in a folder
var files = xyFiles.InventoryNames("C:\Temp");

// Rename a file
if (await xyFiles.RenameFileAsync("old.txt", "new.txt"))
{
    Console.WriteLine("File renamed.");
}

// Read lines from a file
var lines = await xyFiles.ReadLinesAsync("data.txt");

// Save string to file
await xyFiles.SaveStringToFileAsync("Hello, world!", "AppData", "greeting.txt");

// Delete a file
bool deleted = xyFiles.DeleteFile("AppData", "config.json");
```

---

## Related Components

- [`System.IO.File`](https://learn.microsoft.com/en-us/dotnet/api/system.io.file)
- [`System.IO.Directory`](https://learn.microsoft.com/en-us/dotnet/api/system.io.directory)
- `xyLog`, `xyJson`, `xyPathHelper`, `xy.BytesToString`, `xy.StringToBytes`

