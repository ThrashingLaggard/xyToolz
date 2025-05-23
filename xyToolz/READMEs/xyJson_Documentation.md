# 📦 xyJson – JSON Utility Helper

`xyJson` is a static utility class designed for efficient reading, writing, updating, and analyzing structured JSON files. It is part of the `xyToolz` suite and provides async methods for safe and readable JSON interaction.

---

## ✨ Features

- ✅ Full-file serialization and deserialization
- 🔑 Key and subkey extraction via generic helpers
- 🔐 Built-in base64 decoding (e.g. for RSA keys)
- 🧪 Integrated error handling with `xyLog`
- 📁 Optional JSON root-tag enforcement

---

## 🧵 Thread Safety

All methods are static and stateless. The class is fully thread-safe.

---

## ⚠️ Limitations

- Does not support recursive JSON merging
- Only supports UTF-8 encoded files
- No schema validation

---

## 🚀 Performance

- Async-first design
- Minimal memory footprint via streaming
- Suitable for config files and small-to-medium data stores

---

## ⚙️ Configuration

- Logging via `xyLog`
- Uses default `JsonSerializerOptions`
- File paths can be relative or absolute

---

## 🧪 Example Usage

```csharp
await xyJson.AddOrUpdateEntry("settings.json", "TokenExpiry", 3600);

var config = await xyJson.DeserializeKeyIntoDictionary("settings.json", "JWT");
byte[] keyBytes = await xyJson.DeserializeSubKeyToBytes("settings.json", "Keys", "PrivateKey");
```

---

## 📚 API Overview

| Method | Description |
|--------|-------------|
| `SaveDataToJsonAsync<T>` | Serializes data into a JSON file |
| `SerializeDictionary` | Writes a dictionary to JSON file |
| `AddOrUpdateEntry<T>` | Adds or updates a key in JSON file |
| `DeserializeFromFile` | Deserializes entire file into `Dictionary<string, object>` |
| `DeserializeFromKey` | Extracts a single key as object |
| `DeserializeKeyToBytes` | Decodes base64 from top-level key |
| `DeserializeSubKeyToDictionary` | Reads subkey into dictionary |
| `DeserializeSubKeyToBytes` | Reads and decodes subkey to `byte[]` |
| `GetJObjectFromFile` | Returns raw `JObject` |
| `GetJTokenFromJsonFile` | Returns raw `JToken` |

---

## 🔗 Related

- `xyLog` – Logging utility
- `xy.BaseToBytes()` – Base64 decoding
- `xyFiles.GetStreamFromFile()` – File system wrapper

---

© 2025 – Built with ❤️ for clarity and extensibility.