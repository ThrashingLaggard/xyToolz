# class xyPdf

Namespace: `xyToolz`  
Visibility: `public`  
Source: `xyToolz\xyPdf.cs`

## Description:

/// QOL stuff for handling pdf files
    ///

## Methoden

- `bool CombineAllInDirectory(string directory)` — `public static`
  
  /// Combines all PDF files in a directory into a single document.
        ///
- `bool MassConvertToPdf(string directory)` — `public static`
  
  (No XML‑Summary )
- `bool NameAllFiles(List<PdfDocument> pdfs)` — `public static`
  
  /// For ErrorPrevention gives the Files an info.title
        ///
- `bool SaveThisPicAsPdf(string filepath, string newpath)` — `public static`
  
  /// Saves the specified picture as a .pdf File after converting it with the according method.
        /// 
        /// 
        /// returns a bool for now
        ///
- `List<PdfDocument> CombineAllIntoBundles(string directory)` — `public static`
  
  /// Iterates through the selected directory and combines two documents following each other according to name
        ///
- `PdfDocument CombineThesePDFs(List<string> list_of_documents)` — `public static`
  
  /// Combines a list of PDF files into a single document.
        ///
- `PdfDocument CombineTwoPDF(PdfDocument first, PdfDocument second)` — `public static`
  
  (No XML‑Summary )
- `PdfDocument CombineTwoPDFs(PdfDocument first, PdfDocument second)` — `public static`
  
  /// Combine two PDF files and save their page count as name
        ///
- `PdfDocument ConvertPictureToPdf(string filepath)` — `public static`
  
  /// Takes ONE picture and converts it into an internal instance of a PdfSharp PdfDocument
        ///
- `PdfDocument ConvertPictureToPdf(string first_path, string other_path)` — `public static`
  
  /// Two pics combined in one doc, but without saving it as file
        ///
- `PdfDocument OpenDoc(string filepath)` — `public static`
  
  /// creates an internal PDF_Document from the specified file  -  requires said file to be .pdf
        ///

