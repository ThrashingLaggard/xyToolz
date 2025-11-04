using System;
using xyToolz.Pdf;
using XBrushes = PdfSharpCore.Drawing.XBrushes;
using XColor = PdfSharpCore.Drawing.XColor;
using XColors = PdfSharpCore.Drawing.XColors;
using XFont = PdfSharpCore.Drawing.XFont;
using XPen = PdfSharpCore.Drawing.XPen;
using XRect = PdfSharpCore.Drawing.XRect;
using XStringFormats = PdfSharpCore.Drawing.XStringFormats;

namespace xyToolz.Pdf_Layout
{
    internal class PdfHeaderFooterLayout
    {
        public PdfHeaderFooterLayout(PageWriter pageWriter_)
        {
            _pw = pageWriter_;
        }

        public PageWriter _pw { get; }

        internal void DrawHeaderFooterArea()
        {
            // 1) Create locally smaller fonts (actual glyph size down) without changing PdfTheme
            var baseHeader = _pw.Theme.FontSmall;
            var baseFooter = _pw.Theme.FontSmall;

            // scale ~85%; clamp to a sane minimum
            double headerSize = Math.Max(6.0, baseHeader!.Size * 0.85);
            double footerSize = Math.Max(6.0, baseFooter!.Size * 0.85);

            var headerFont = new XFont(baseHeader.FontFamily.Name, headerSize, baseHeader.Style);
            var footerFont = new XFont(baseFooter.FontFamily.Name, footerSize, baseFooter.Style);

            double headerLH = _pw.Theme.LineHeight(headerFont); // line height for the smaller font
            double footerLH = _pw.Theme.LineHeight(footerFont);

            // 2) Draw header inside the top margin so it doesn't steal content height.
            // Place the header so its bottom sits just a couple of points above _top.
            double headerBottom = _pw._top - 2;                 // just above content area
            double headerTop = Math.Max(6, _pw._top - 2 - headerLH);  // header box top

            string header = _pw.PageHeaderOverride?? _pw.Ctx.CurrentSectionTitle?? _pw.Ctx.Document.Info?.Title ?? "xyDocumentor";

            var headerRect = new XRect(_pw._left, headerTop, _pw._contentWidth, headerLH);
            _pw.Gfx.DrawString(header, headerFont, XBrushes.Gray, headerRect, XStringFormats.TopLeft);

            // Thin rule right above content
            var pen = new XPen(XColors.LightGray, 0.4);
            _pw.Gfx.DrawLine(pen, _pw._left, _pw._top - 2, _pw._right, _pw._top - 2);

            // IMPORTANT: Do NOT push Y down — keep content starting at _top (no hidden spacer)
            _pw.Y = _pw._top;

            // 3) Draw footer (page number) just inside the bottom margin, compact
            string pn = _pw.Ctx.PageNumber.ToString();

            // Place the baseline a couple of points inside the bottom margin area
            double pageHeight = _pw.Page.Height;                    // in points (consistent with XRect usage)
            var footerRect = new XRect(_pw._left, pageHeight - footerLH - 2, _pw._contentWidth, footerLH);
            _pw.Gfx.DrawString(pn, footerFont, XBrushes.Gray, footerRect, XStringFormats.BottomRight);
        }












        internal void DrawHairline(double alpha = 0.35)
        {
            int a = alpha <= 1 ? (int)Math.Round(alpha * 255) : (int)Math.Round(alpha);
            a = Math.Clamp(a, 0, 255);
            var pen = new XPen(XColor.FromArgb(a, 0, 0, 0), 0.5);
            _pw.Gfx.DrawLine(pen, _pw._left, _pw.Y, _pw._right, _pw.Y);
        }


    }
}
