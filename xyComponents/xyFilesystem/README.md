# xyFilesystem

> Extracted component of [xyToolz](https://github.com/ThrashingLaggard/xyToolz) — source is maintained in the main xyToolz repository; this package exposes only this module standalone.

Async file and directory helpers for .NET, including a mockable file-access layer for testing.

## Install

```
dotnet add package xyFilesystem
```

## Contents

- **`xyFiles`** — async file operations:
  - `EnsurePathExistsAsync`, `Inventory` / `InventoryNames`, `RenameFileAsync`
  - `ReadLinesAsync`, `ReadBytes`, `GetStreamFromFileAsync`
  - `SaveToFile`, `SaveBytesToFileAsync`, `SaveStringToFileAsync`
  - `LoadFileAsync` (path or subfolder+filename overloads), `LoadBytesFromFile`
  - `DeleteFile`
  - `OverrideForTests` / `ResetOverride` — swap in an `IxyFiles` test double.

- **`xyDirectoryHelper`** — directory operations:
  - `GetSolutionFolder`, `GetApplicationFolder`, `GetInnerApplicationFolderDebug`
  - `GetFiles`, `GetSubfolders`, `GetFolderSize`, `IsFolderEmpty`
  - `CopyFolder`, `MoveFolder`, `RenameFolder`, `ClearFolder`, `DeleteFolder`
  - `CompressFolder`, `ExtractFolder`
  - `MonitorFolder` — wraps `FileSystemWatcher`.

- **`xyPath`** — path helpers:
  - `BasePath` (Android-aware via `#if ANDROID`, falls back to `AppContext.BaseDirectory`)
  - `Combine(params string[])`, `EnsureDirectory(params string[])`, `EnsureParentDirectoryExists(string)`

- **`IxyFiles`** — interface for mocking `xyFiles` in unit tests.

## Example

```csharp
await xyFiles.SaveToFile("Hello!", "output.txt");
var lines = await xyFiles.ReadLinesAsync("settings.txt");

xyDirectoryHelper.CompressFolder(sourceFolder: "data", zipFilePath: "data.zip");
string safePath = xyPath.Combine("AppData", "config.json");
```

## Dependencies

- `xyLogger`
- `xyMessageFactory`
- `xyExtensions` (project reference)

## Target Framework

.NET 8.0

## License

GPL-3.0-or-later
