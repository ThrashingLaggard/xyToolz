# xyToolz

**xyToolz** is the core utility library of the xyToolz suite. It offers reusable, modular helper classes for developers working with .NET. The focus lies on simplifying common tasks like logging, file operations, encryption, JSON handling, and data conversion.

---

## 🔍 Overview

This project contains the foundation of the xyToolz ecosystem. It can be used independently or embedded into other xyToolz modules (such as xyPorts or xyAndroid).

---

## 📦 Modules & Key Classes

### 🔐 `xyHashHelper`

A robust and secure utility for password hashing and AES-compatible key derivation.

#### 📌 Features

- PBKDF2 hashing with `Rfc2898DeriveBytes`
- Secure salt generation
- Optional pepper via environment variable `PEPPER`
- SHA256 and SHA512 support
- Fixed-time password verification
- Compatible with AES encryption
- Extensive logging

#### 🔧 Configuration

| Option         | Description                             | Default    |
|----------------|-----------------------------------------|------------|
| `PEPPER`       | Additional entropy from environment     | `"Ahuhu"`  |
| `Iterations`   | PBKDF2 iterations                       | `100_000`  |
| `KeyLength256` | AES-256 / SHA256 key length             | `32 Bytes` |
| `KeyLength512` | SHA512 key length                       | `64 Bytes` |

#### 📌 Methods

| Method                        | Purpose                                         |
|-------------------------------|-------------------------------------------------|
| `BuildSaltedHash()`           | Generates new salt+hash as `salt:hash` string   |
| `VerifyPassword()`            | Validates input password against stored hash    |
| `BuildKeyFromPassword()`      | Derives AES key from password and salt          |
| `GenerateSalt()`              | Returns secure random salt                      |
| `HashToBytes()` / `ToString()`| Produces raw or Base64-encoded hash             |
| `TryVerifyPassword()`         | Safe verification returning `bool` via `out`    |

#### 🧪 Example

```csharp
string pw = "Secure123!";
string saltedHash = xyHashHelper.BuildSaltedHash(HashAlgorithmName.SHA256, pw, out var salt);
bool valid = xyHashHelper.VerifyPassword(HashAlgorithmName.SHA256, pw, saltedHash);
```

---

### 🔐 `xyRsa`

Static class for handling JWTs and RSA-based encryption in APIs and apps.

#### 📌 Features

- JWT signing using `RS256`
- Validation with expiration & audience check
- Load PEM-formatted keys
- Export public key as PEM
- Logging via `xyLog`

#### 🧪 Example

```csharp
await xyRsa.LoadKeysAsync(pubPem, privPem);
await xyRsa.ConfigureAsync("issuer", "audience");

string token = await xyRsa.GenerateJwtAsync(new() {
    { "sub", "user42" }, { "role", "admin" }
}, TimeSpan.FromHours(1));

var principal = await xyRsa.ValidateJwtAsync(token);
```

---

### 📂 `xyFiles`

Cross-platform static file/directory utility.

#### 📌 Features

- Ensure/Check/Create directories
- List files: `Inventory`, `InventoryNames`
- Async I/O: `ReadLinesAsync`, `SaveToFile`, `LoadFileAsync`
- Rename/Delete files
- Read/Write binary: `SaveBytesToFileAsync`, `LoadBytesFromFile`

#### ✅ Thread Safety
Stateless & static — thread-safe by design.

#### 🧪 Example

```csharp
var lines = await xyFiles.ReadLinesAsync("settings.txt");
await xyFiles.SaveToFile("output.txt", "Hello!");
```

---

### 📄 `xyJson`

Simplifies reading and writing JSON using generics and safe IO.

---

### 🔄 `xyConversion`

Helper methods for converting strings, types, and basic parsing tasks.

---

### 🪵 `xyLog`, `xyLogFormatter`, `xyLogArchiver`

Logging infrastructure with output targets, formatting, and archiving support.

---

### 🧪 `xyCard`, `yxCard`

Custom wrapper classes for card-style data transport or templating.

---

## 🧩 Interfaces

| Interface         | Description                      |
|-------------------|----------------------------------|
| `IxyFiles`        | Abstraction layer for file ops   |
| `IxyJson`         | JSON read/write abstraction      |
| `IxyDataProtector`| Secure storage abstraction       |

---

## ✅ Requirements

- .NET 6 or newer (.NET 8 ready)
- Zero external dependencies
- Optional Android and Avalonia support via submodules

---

## 📜 License

Licensed under **GNU GPLv3**  
See: [https://www.gnu.org/licenses/gpl-3.0.en.html](https://www.gnu.org/licenses/gpl-3.0.en.html)

---

## 👤 Author

Created by the ThrashingLaggard
> Part of the `xyToolz` ecosystem  
> Created for internal tooling, education, and experimentation.
---

### 🪵 `xyLog`, `xyLogFormatter`, `xyLogTargets`, `xyLogArchiver`

A full-featured logging system supporting multiple outputs, formatting, and optional archiving.

#### 📌 Features

- Console/file/memory logging
- LogLevel support
- Contextual formatting (method, time, etc.)
- Archiving old logs by date or size
- Optional async methods
- Can be integrated with `Microsoft.Extensions.Logging`

#### 🧪 Example

```csharp
xyLog.Log("Started app");
xyLog.ExLog(new Exception("Something failed"));
```

---

### 🧠 `xyConversion`

Utility class for common type and string conversions.

#### 📌 Features

- Safe `ToInt`, `ToDouble`, `ToBool`, etc.
- Culture-invariant handling
- Nullable-friendly parsing
- Fallback/default value support

#### 🧪 Example

```csharp
int result = xyConversion.ToInt("1234", fallback: 0);
bool ok = xyConversion.ToBool("true");
```

---

### 🧾 `xyJson`

Simplifies JSON I/O by abstracting `System.Text.Json`.

#### 📌 Features

- Generic load/save to file
- Safe deserialization with fallback
- Formatting control
- Optional file existence checks

#### 🧪 Example

```csharp
var obj = xyJson.ReadFromFile<MyConfig>("settings.json");
xyJson.WriteToFile("settings.json", obj);
```

---

### 🔐 `xyDataProtector`

Wrapper for secure data encryption using Windows DPAPI.

#### 📌 Features

- `ProtectAsync<T>()` and `UnprotectAsync<T>()`
- Encrypted storage for strings, objects, secrets
- Logging of exceptions and recovery
- Optional entropy for additional protection

#### 🧪 Example

```csharp
await xyDataProtector.ProtectAsync("secret.txt", myObject);
var restored = await xyDataProtector.UnprotectAsync<MyType>("secret.txt");
```

---

### 📚 `xy`, `xyDirUtils`, `xyCard`, `yxCard`

Miscellaneous helpers:

- `xy`: static extensions and core tools
- `xyDirUtils`: directory validation and utilities
- `xyCard`, `yxCard`: generic data holder classes with optional metadata
---

### 🧰 `xy`

The central static helper class containing reusable base utilities and extension methods.

#### 📌 Features

- Byte/String conversions (UTF8, Base64)
- Random string generation
- Path normalization and manipulation
- Type checking helpers
- Fast boolean/string parsing
- Memory-safe string splitting

#### 🧪 Example

```csharp
string data = "Hello";
byte[] bytes = xy.StringToBytes(data);
string back = xy.BytesToString(bytes);

bool ok = xy.IsNullOrWhiteSpace(" ");
string safePath = xy.NormalizePath("folder\..\data");
```

---

### 📁 `xyDirUtils`

Provides static directory-related utilities for cross-platform use.

#### 📌 Features

- Ensure directory exists (create if needed)
- Validate directory names
- Recursive file/folder inventory
- Temporary folder generation
- Auto-cleanup helpers

#### 🧪 Example

```csharp
string path = "myfolder/data";
xyDirUtils.EnsureDirectory(path);

bool exists = xyDirUtils.CheckForDirectories("myfolder");
var all = xyDirUtils.Inventory("myfolder");
```