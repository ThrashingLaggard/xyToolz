
using PdfSharp.Pdf;

namespace xyToolz.Pdf
{
    /// <summary>
    /// Symbolizing an indexing Card for learning or ....
    /// 
    /// Needs to be implemented fully
    /// </summary>
    public class xyCard
    {
        public PdfDocument? Document;
        public PdfPage frontSide;
        public PdfPage backSide;

        public int Number;
        public PdfDocumentInformation? info;
        public PdfDocumentSettings? settings;
        public PdfDocumentOptions? options;
        public PdfPageLayout? layout;

        public xyCard(PdfPage frontSide, PdfPage backSide, int num)
        {
            this.frontSide = frontSide;
            this.backSide = backSide;
            Number = num;
        }

        public xyCard(PdfPage frontSide, PdfPage backSide, PdfDocumentInformation info,int num, PdfDocumentSettings settings, PdfDocumentOptions options, PdfPageLayout layout)
        {
            Number = num;
            this.frontSide = frontSide;
            this.backSide = backSide;
            this.info = info;
            this.settings = settings;
            this.options = options;
            this.layout = layout;
        }
    }

}
