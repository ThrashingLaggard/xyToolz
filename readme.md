# xyToolz

Core utility library of the xy empire. My main goal here is to save some seconds.
This provides helper classes for .NET: files, hashing/RSA, JSON/XML, PDF, dates, logging integration, and misc QOL helpers.

---

## Disclaimer
!!! Attention !!! Achtung !!! Uwaga !!!
This is my quick and dirty version for personal use, thus it can be a nightmare when it comes to dependency handling...
If you prefer the components on their own, there are corresponding NuGets for each component of this toolbox.

---
## Requirements

- .NET 8
- 
---

## Modules

### Security
- **xyHasher** — PBKDF2 password hashing, salted hash build/verify, AES key derivation from password.
- **xyRsa** — RS256 JWT signing/validation, PEM key loading/export.
- **xyDataProtector** — encrypt/decrypt objects, strings, bytes to/from file; test-double override support.

### Filesystem
- **xyFiles** — async read/write (text, bytes, streams), file inventory, rename/delete, test-double override support.
- **xyDirectoryHelper** — directory create/copy/move/rename/clear/delete, compress/extract, folder size, recursive listing, `FileSystemWatcher` helper.
- **xyPath** — path helpers.

### Serialization
- **xyJson** — generic JSON read/write to file.
- **xyXml** — XML (de)serialization.

### Chrono
- **xyDate**, **xyTime** — date/time helpers.

### Maths
- **xyConversion** — type/string conversion helpers.
- **SumOfDigits** — digit-sum utility.

### Enumerables / Extensions
- **xyList** — list helper.
- Extension methods for `string`, `byte[]`, `List<T>`, `Dictionary<K,V>`.

### PDF
- **xyPdf** — PDF generation/manipulation (built on PDFsharp/PdfSharpCore).
- **xyCard** — internal card/template helper used by `xyPdf`, not part of the public API.

### Fonts
- **AutoResourceFontResolver** — font resolution for PDF rendering.

### QOL
- **xy** — misc static helpers: byte/string conversion, random strings, path normalization, safe parsing, `TryCatch` wrappers.
- **xyPropertyHelper**, **xySpiller** — misc helpers.

### Database (outsourced due to dependencies)
Simple EF Core wrapper: https://www.nuget.org/packages/ExtendedCRUD/

### Driver (WIP)
**xyWebDriver** — Selenium-based browser automation. Currently disabled.

### Interfaces - Ignore these!!!	Only relevant for internal Unit Tests (due to tomfoolery they cant be internal but need to be visible)
`IxyFiles`, `IxyJson`, `IxyDataProtector` — abstractions used for test-double overrides in the corresponding static classes.

---

## Example

```csharp
string saltedHash = xyHasher.BuildSaltedHash(HashAlgorithmName.SHA256, "Secure123!", out var salt);
bool valid = xyHasher.VerifyPassword(HashAlgorithmName.SHA256, "Secure123!", saltedHash);

var lines = await xyFiles.ReadLinesAsync("settings.txt");
await xyFiles.SaveToFile("Hello!", "output.txt");

var config = xyJson.ReadFromFile<MyConfig>("settings.json");
```

---

## Dependencies

Haters will say its not dependency-free. Notable NuGet packages: `Newtonsoft.Json`, `PDFsharp`, `PdfSharpCore`, `SixLabors.ImageSharp`, `Microsoft.IdentityModel.Tokens`, `System.IdentityModel.Tokens.Jwt`, `xyLogger`, `xyMessageFactory`.

Logging (`xyLog`) is provided by the external `xyLogger` package, not by this repo.

---
## 📜 License

Licensed under **GNU GPLv3**  
See: [https://www.gnu.org/licenses/gpl-3.0.en.html](https://www.gnu.org/licenses/gpl-3.0.en.html)

---

## 👤 Author

Created by the ThrashingLaggard for internal tooling, education, and experiments.
https://github.com/ThrashingLaggard
  
---
