# xyPdf

> Extracted component of [xyToolz](https://github.com/ThrashingLaggard/xyToolz) — source is maintained in the main xyToolz repository; this package exposes only this module standalone.

PDF creation, conversion, and merging helpers built on PDFsharp/PdfSharpCore.

## Install

```
dotnet add package xyPdf
```

## Contents

- **`xyPdf`** — static PDF helpers:
  - `OpenDoc(string filepath)` — open an existing PDF as `PdfDocument`.
  - `ConvertPictureToPdf(string filepath)` / `ConvertPictureToPdf(string first, string other)` — image(s) to PDF.
  - `SaveThisPicAsPdf(string filepath, string newpath)`
  - `CombineTwoPDF(s)(PdfDocument, PdfDocument)`, `CombineThesePDFs(List<string>)` — merge documents.
  - `CombineAllIntoBundles(string directory)`, `CombineAllInDirectory(string directory)` — batch merge.
  - `MassConvertToPdf(string directory)` — convert all images in a directory to PDF.
  - `NameAllFiles(List<PdfDocument>)`.

- **`xyCard`** — internal template/card helper used by `xyPdf`; not part of the public API.

## Example

```csharp
PdfDocument doc = xyPdf.ConvertPictureToPdf("scan.png");
bool ok = xyPdf.CombineAllInDirectory("scans/");
```

## Dependencies

- `PDFsharp`
- `PdfSharpCore`
- `xyLogger`

## Target Framework

.NET 8.0

## License

GPL-3.0-or-later
