# xySerialization

> Extracted component of [xyToolz](https://github.com/ThrashingLaggard/xyToolz) — source is maintained in the main xyToolz repository; this package exposes only this module standalone.

JSON and XML (de)serialization helpers for .NET, with a mockable file-access layer.

## Install

```
dotnet add package xySerialization
```

## Contents

- **`xyJson`** — JSON file read/write and key-based access:
  - `SaveDataToJsonAsync<T>` (single object or `params object[]`), `SerializeDictionary`, `AddOrUpdateEntry<T>`
  - `DeserializeFromFile<T>` / `DeserializeFromFile`, `DeserializeFromKey`, `DeserializeKeyIntoDictionary`, `DeserializeKeyToBytes`
  - `DeserializeSubKey`, `DeserializeSubKeyToDictionary`, `DeserializeSubKeyToBytes`
  - `GetJObjectFromFile`, `GetJTokenFromKey`, `GetStringFromJsonFile`, `GetFirstAndLastLinesAsync`
  - `EnsureJsonRootTag(string filePath)`
  - `OverrideForTests` / `ResetOverride` — swap in an `IxyJson` test double.

- **`xyXml`** — XML (de)serialization:
  - `FromXml<T>(string xml)` (with optional console output), `ToXML<T>(T target)`.

- **`IxyJson`** — interface for mocking `xyJson` in unit tests.

## Example

```csharp
var config = await xyJson.DeserializeFromFile<MyConfig>("settings.json");
await xyJson.SaveDataToJsonAsync(config, "settings.json");

string xml = xyXml.ToXML(config);
MyConfig fromXml = xyXml.FromXml<MyConfig>(xml);
```

## Dependencies

- `Newtonsoft.Json`
- `System.Text.Json`
- `xyLogger`
- `xyMessageFactory`
- `xyExtensions` (project reference)

## Target Framework

.NET 8.0

## License

GPL-3.0-or-later
