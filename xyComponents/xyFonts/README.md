# xyFonts

> Extracted component of [xyToolz](https://github.com/ThrashingLaggard/xyToolz) — source is maintained in the main xyToolz repository; this package exposes only this module standalone.

Embedded-resource font resolver for PDFsharp/PdfSharpCore.

## Install

```
dotnet add package xyFonts
```

## Contents

- **`AutoResourceFontResolver`** — implements PdfSharpCore's `IFontResolver`. Resolves a Sans and Mono font family from embedded assembly resources, with regular/bold variants.
  - `ResolveTypeface(string familyName, bool isBold, bool isItalic)` — returns `FontResolverInfo`.
  - `GetFont(string faceName)` — returns the font's raw bytes.
  - `GetBytesFromAssemblyManifest(Assembly, string manifestName)` — static helper to load an embedded resource by name.
  - `GetFontStem(string fontFileName)` — static helper to strip a font file name to its stem.
  - `FamilySans`, `FamilyMono` — constant family names; `DefaultFontName` — defaults to `FamilySans`.

## Example

```csharp
var resolver = new AutoResourceFontResolver();
GlobalFontSettings.FontResolver = resolver;

var font = new XFont(AutoResourceFontResolver.FamilySans, 12);
```

## Dependencies

- `PdfSharpCore`
- `xyLogger`

## Target Framework

.NET 8.0

## License

GPL-3.0-or-later
