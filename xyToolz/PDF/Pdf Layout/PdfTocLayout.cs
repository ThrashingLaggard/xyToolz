using System;
using System.Collections.Generic;
using xyToolz.Pdf;
using XBrushes = PdfSharpCore.Drawing.XBrushes;
using XFont = PdfSharpCore.Drawing.XFont;
using XRect = PdfSharpCore.Drawing.XRect;
using XStringFormats = PdfSharpCore.Drawing.XStringFormats;

namespace xyToolz.Pdf_Layout
{
#nullable enable
    internal class PdfTocLayout
    {
        public PdfTocLayout(PageWriter pageWriter, PdfTextLayout text, PdfPageFlow pageFlow)
        {
            _pw = pageWriter;
            _text = text;
            _pf = pageFlow;
        }

        public PageWriter _pw { get; }
        public PdfTextLayout _text { get; }
        public PdfPageFlow _pf { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        public XRect DrawTocLine(string title, int pageNumber)
        {
            XFont? font = _pw.Theme.FontNormal;
            double lineHeight = _pw.Theme.LineHeight(font!);

            // Left and right text parts: title and page number
            string left = $"{title}";
            string right = pageNumber.ToString();

            // Measure widths of both parts
            double rightWidth = _pw.Gfx.MeasureString(right, font).Width + 6; // small padding
            double avail = _pw._contentWidth - rightWidth; // available width for the title
            double leftWidth = _pw.Gfx.MeasureString(left, font).Width;

            // Ellipsize title if it exceeds available space
            if (leftWidth > avail)
            {
                const string ell = "…";
                while (left.Length > 4 && _pw.Gfx.MeasureString(left + ell, font).Width > avail)
                    left = left[..^1];
                left += ell;
                leftWidth = _pw.Gfx.MeasureString(left, font).Width;
            }

            // Compute how many dots fit between the title and the page number
            double dotWidth = Math.Max(1.0, _pw.Gfx.MeasureString(".", font).Width);
            double remaining = Math.Max(0, avail - leftWidth);
            int dotCount = (int)Math.Floor(Math.Max(0, remaining - 1) / dotWidth);
            string dots = dotCount > 0 ? new string('.', dotCount) : string.Empty;

            // Ensure enough vertical space for the line
            _pf.EnsureSpace(lineHeight);

            // Calculate text rectangles
            var leftRect = new XRect(_pw._left, _pw.Y, avail, lineHeight);
            var rightRect = new XRect(_pw._left + avail, _pw.Y, rightWidth, lineHeight);

            // Draw the left part (title + dots) using the text formatter
            _pw.Gfx.DrawString(left + dots, font, XBrushes.Black, leftRect, XStringFormats.TopLeft);

            // Draw the right part (page number), right-aligned
            _pw.Gfx.DrawString(right, font, XBrushes.Black, rightRect, XStringFormats.TopRight);
      
            // Define the overall line area (used for clickable TOC links later)
            var lineRect = new XRect(_pw._left, _pw.Y, _pw._contentWidth, lineHeight);

            // Move Y down for the next line
            _pw.Y += lineHeight + 2;

            // Return the drawn area
            return lineRect;
        }

        /// <summary>
        /// Draws a TOC entry consisting of:
        ///  - a narrow left "kind" column (e.g., "class", "interface"),
        ///  - the main text with dot leaders,
        ///  - the page number aligned on the far right.
        /// It does NOT change any global layout; it only splits the line horizontally.
        /// </summary>
        /// <param name="kind"></param>
        /// <param name="leftText"></param>
        /// <param name="pageNumber"></param>
        /// <param name="kindWidth"></param>
        /// <param name="gap"></param> 
        public XRect DrawTocLineWrapped(string kind, string leftText, int pageNumber, double kindWidth, double gap = 3.0)
        {
            var font = _pw.Theme.FontNormal;
            double lineHeight = _pw.Theme.LineHeight(font!);

            // Right side (page number)
            string right = pageNumber.ToString();
            double rightWidth = _pw.Gfx.MeasureString(right, font).Width + 6; // small padding for readability

            // Available width for the main text between kind column and page number
            double avail = _pw._contentWidth - kindWidth - gap - rightWidth;
            if (avail < 20) avail = 20; // guard against extremely narrow cases

            // Word-wrap the main text into lines that fit the available width
            var linesArr = PdfTextLayout.WrapText(leftText ?? string.Empty, font!, avail, _pw.Gfx); // uses your gfx-based wrap
            var lines = linesArr.Length == 0 ? new List<string> { string.Empty } : new List<string>(linesArr);

            // Compute dot leaders for the first line only
            double firstLeftWidth = _pw.Gfx.MeasureString(lines[0], font).Width;
            double dotWidth = Math.Max(1.0, _pw.Gfx.MeasureString(".", font).Width);
            double remaining = Math.Max(0, avail - firstLeftWidth);
            int dotCount = (int)Math.Floor(Math.Max(0, remaining - 1) / dotWidth);
            string dots = dotCount > 0 ? new string('.', dotCount) : string.Empty;

            // Ensure vertical space for the whole block (all wrapped lines)
            double blockHeight = lines.Count * lineHeight + 2;
            _pf.EnsureSpace(blockHeight);

            // 1) Left "kind" column (right-aligned so it stays visually narrow)
            var kindRect = new XRect(_pw._left, _pw.Y, kindWidth, lineHeight);

            // Clip to kind cell to guarantee no bleed into the main text
            var kindState = _pw.Gfx.Save();
            _pw.Gfx.IntersectClip(kindRect);

            string kindFitted = _text.FitWithEllipsis(kind ?? string.Empty, font!, Math.Max(1, kindWidth - 1));
            _pw.Gfx.DrawString(kindFitted, font, XBrushes.Gray, kindRect, XStringFormats.TopRight);

            _pw.Gfx.Restore(kindState);

            // 2) First text line (title + dots), directly to the right of the kind column
            double textX = _pw._left + kindWidth + gap;
            var firstRect = new XRect(textX, _pw.Y, avail, lineHeight);
            _pw.Gfx.DrawString(lines[0] + dots, font, XBrushes.Black, firstRect, XStringFormats.TopLeft);

            // 3) Page number on the far right, aligned to the first line
            var rightRect = new XRect(textX + avail, _pw.Y, rightWidth, lineHeight);
            _pw.Gfx.DrawString(right, font, XBrushes.Black, rightRect, XStringFormats.TopRight);

            // 4) Subsequent wrapped lines (no dots), same text column
            double yCursor = _pw.Y + lineHeight;
            for (int i = 1; i < lines.Count; i++)
            {
                var r = new XRect(textX, yCursor, avail, lineHeight);
                _pw.Gfx.DrawString(lines[i], font, XBrushes.Black, r, XStringFormats.TopLeft);
                yCursor += lineHeight;
            }

            // Clickable area for link annotations
            var lineRect = new XRect(_pw._left, _pw.Y, _pw._contentWidth, lines.Count * lineHeight);

            // Advance Y for the next TOC line
            _pw.Y = yCursor + 2;
            return lineRect;
        }




    }
}
