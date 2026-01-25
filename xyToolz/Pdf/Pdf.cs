namespace xyToolz.Pdf
{
    using System.Collections.Generic;
    using PdfSharp.Pdf;

    /// <summary>
    /// Pdfsharp Wrapper --> Facade for xyPdf.
    /// Pure pass-through. No logic, no state, no validation.
    /// </summary>
    public static class Pdf
    {
        public static PdfDocument OpenDoc(string filepath)
            => xyPdf.OpenDoc(filepath);

        public static bool NameAllFiles(List<PdfDocument> pdfs)
            => xyPdf.NameAllFiles(pdfs);

        public static PdfDocument CombineTwoPDF(PdfDocument first, PdfDocument second)
            => xyPdf.CombineTwoPDF(first, second);

        public static PdfDocument CombineTwoPDFs(PdfDocument first, PdfDocument second)
            => xyPdf.CombineTwoPDFs(first, second);

        [Obsolete]
        public static PdfDocument ConvertPictureToPdf(string filepath)
            => xyPdf.ConvertPictureToPdf(filepath);

        [Obsolete]
        public static PdfDocument ConvertPictureToPdf(string first_path, string other_path)
            => xyPdf.ConvertPictureToPdf(first_path, other_path);

        public static bool SaveThisPicAsPdf(string filepath, string newpath)
            => xyPdf.SaveThisPicAsPdf(filepath, newpath);

        public static bool MassConvertToPdf(string directory)
            => xyPdf.MassConvertToPdf(directory);

        public static List<PdfDocument> CombineAllIntoBundles(string directory)
            => xyPdf.CombineAllIntoBundles(directory);

        public static bool CombineAllInDirectory(string directory)
            => xyPdf.CombineAllInDirectory(directory);

        public static PdfDocument CombineThesePDFs(List<string> list_of_documents)
            => xyPdf.CombineThesePDFs(list_of_documents);
    }
}
