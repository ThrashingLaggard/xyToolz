namespace xyToolz.Pdf
{
    using PdfSharpCore.Pdf;

    /// <summary>
    /// Value object for anchor destinations inside the PDF.
    /// </summary>
    public readonly record struct AnchorTarget(PdfPage Page, double Y);
}
