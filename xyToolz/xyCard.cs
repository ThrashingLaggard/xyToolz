
using PdfSharp.Pdf;

namespace xyToolz
{
    internal class xyCard
    {
        static PdfDocument card = new PdfDocument();
        PdfPage frontSide = new PdfPage(card);
        PdfPage backSide = new PdfPage(card);

        int Number;
        PdfDocumentInformation info;
        PdfDocumentSettings settings;
        PdfDocumentOptions options;
        PdfPageLayout layout;

        xyCard(PdfPage frontSide, PdfPage backSide, int num)
        {
            this.frontSide = frontSide;
            this.backSide = backSide;
            Number = num;
        }

        xyCard(PdfPage frontSide, PdfPage backSide, PdfDocumentInformation info,int num, PdfDocumentSettings settings, PdfDocumentOptions options, PdfPageLayout layout)
        {
            this.Number = num;
            this.frontSide = frontSide;
            this.backSide = backSide;
            this.info = info;
            this.settings = settings;
            this.options = options;
            this.layout = layout;
        }
    }

    internal class yxCard
    {
        PdfDocument frontSide = new PdfDocument();
        PdfDocument backSide = new PdfDocument();
        PdfPageLayout layout;

        PdfDocumentInformation info;
        PdfDocumentSettings settings;
        PdfDocumentOptions options;

        yxCard() 
        {
        
        }

    }

}
