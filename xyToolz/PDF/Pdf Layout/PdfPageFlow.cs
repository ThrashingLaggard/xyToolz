using PdfSharpCore.Drawing;
using xyToolz.Pdf;

namespace xyToolz.Pdf_Layout
{
    internal class PdfPageFlow
    {
        public PdfPageFlow(PageWriter pageWriter, PdfTextLayout text, PdfHeaderFooterLayout headerFooter)
        {
            _pw = pageWriter;
            _text = text;
            _hf = headerFooter;
        }

        public PageWriter _pw { get; }
        public PdfTextLayout _text { get; }
        public PdfHeaderFooterLayout _hf { get; }

        internal void EnsureSpace(XFont font, string text, double lineSpacing = 1.0)
        {
            var lines = PdfTextLayout.WrapText(text, font, _pw._contentWidth, _pw.Gfx);
            var needed = lines.Length * _pw.Theme.LineHeight(font) * lineSpacing + 2;
            EnsureSpace(needed);
        }



        internal  void EnsureSpace(double heightNeeded)
        {
            if (_pw.Y + heightNeeded <= _pw._bottom) return;

            _pw.Gfx.Dispose();
            // New page
            _pw.Page = _pw.Ctx.AddPage();
            
            _pw.Gfx = XGraphics.FromPdfPage(_pw.Page);

            _pw.Y = _pw._top;
            if (_pw.DrawHeaderFooter) _hf.DrawHeaderFooterArea();
        }




        // when you need a new page during layout:
        internal void NewPage()
        {
            // Create a brand new page owned by this document
            var next = _pw.Ctx.Document.AddPage();
            next.Size = PdfSharpCore.PageSize.A4;

            // swap graphics to the new page
            _pw.Gfx.Dispose();
            _pw.Page = next;
            _pw.Gfx = XGraphics.FromPdfPage(_pw.Page);
            // reset Y / header/footer as you already do...
        }


        /// <summary>
        /// Draw the header and write the enclosed text
        /// </summary>
        /// <param name="level"></param>
        /// <param name="text"></param>
        public void DrawHeading(int level, string text)
        {
            var (font, color, spacing) = level switch
            {
                1 => (_pw.Theme.FontH1, _pw.Theme.ColorPrimary, 10d),
                2 => (_pw.Theme.FontH2, _pw.Theme.ColorPrimary, 8d),
                _ => (_pw.Theme.FontH3, _pw.Theme.ColorDark, 6d),
            };
            EnsureSpace(font!, text, lineSpacing: _pw.Theme.LineSpacingHeading);
            _pw.Gfx.DrawString(text, font, new XSolidBrush(color), new XRect(_pw._left, _pw.Y, _pw._contentWidth, _pw.Theme.LineHeight(font!)), XStringFormats.TopLeft);
            _pw.Y += _pw.Theme.LineHeight(font!) + spacing;
            _hf.DrawHairline();
            Spacer(level == 1 ? 8 : 6);
        }

        /// <summary>
        /// Draw the sub header and write the txt in it
        /// </summary>
        /// <param name="text"></param>
        public void DrawSubheading(string text)
        {
            EnsureSpace(_pw.Theme.FontH4!, text);
            _pw.Gfx.DrawString(text, _pw.Theme.FontH4, XBrushes.Black, new XRect(_pw._left, _pw.Y, _pw._contentWidth, _pw.Theme.LineHeight(_pw.Theme.FontH4!)), XStringFormats.TopLeft);
            _pw.Y += _pw.Theme.LineHeight(_pw.Theme.FontH4!) / 2;
            _hf.DrawHairline(alpha: 0.25);
            Spacer(4);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="value"></param>
        public void DrawBulletLine(string title, string value)
        {
            EnsureSpace(_pw.Theme.FontNormal!, $"{title}: {value}");
            var text = $"{title}: {value}";
            _text.DrawParagraph(text, new XRect(_pw._left, _pw.Y, _pw._contentWidth, 0), _pw.Theme.FontNormal!);
            Spacer(4);
        }


        /// <summary>
        /// Define how much space comes between the [lines/...????]
        /// </summary>
        /// <param name="points"></param>
        public void Spacer(double points) => _pw.Y += points;



    }
}
