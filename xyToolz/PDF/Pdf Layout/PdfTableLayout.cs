using PdfSharpCore.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using xyToolz.Pdf;

namespace xyToolz.Pdf_Layout
{
    internal class PdfTableLayout
    {
        public PdfTableLayout(PageWriter pageWriter_, PdfTextLayout text_, PdfPageFlow pageFlow_, PdfHeaderFooterLayout headerFooter_)
        {
            _pw = pageWriter_;
            _hf = headerFooter_;
            Text = text_;
            _pf = pageFlow_;
        }

        public PageWriter _pw { get; }
        public PdfTextLayout Text { get; }
        public PdfPageFlow _pf { get;}
    public PdfHeaderFooterLayout _hf { get; }

        private static double[] CalcColumnPixelWidths(double[] ratios, double totalWidth)
        {
            var sum = ratios.Sum();
            return ratios.Select(r => Math.Max(30, totalWidth * (r / sum))).ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="rows"></param>
        public void DrawTable(TableColumnSpec[] columns, IEnumerable<string[]> rows)
        {
            double gap = _pw.Theme.TableColGap;
            // Subtract (n-1) gaps from the content width to avoid overflow
            double availableWidth = _pw._contentWidth - (columns.Length - 1) * gap;

            // Column widths by ratio
            var widths = CalcColumnPixelWidths(columns.Select(c => c.WidthRatio).ToArray(), availableWidth);

            // Header
            var headerHeight = _pw.Theme.LineHeight(_pw.Theme.FontSmallBold!);
            _pf.EnsureSpace(headerHeight + 4);
            double x = _pw._left;
            for (int c = 0; c < columns.Length; c++)
            {
                var rect = new XRect(x, _pw.Y, widths[c], headerHeight);
                _pw.Gfx.DrawString(columns[c].Header, _pw.Theme.FontSmallBold, XBrushes.Black, rect, XStringFormats.TopLeft);
                x += widths[c] + gap;
            }
            _pw.Y += headerHeight;
            _hf.DrawHairline(alpha: 0.5);
            _pw.Spacer(4);

            // Rows
            foreach (var row in rows)
            {
                // Wrap each cell to lines
                var wrappedCells = new List<string[]>();
                var innerWidths = new List<double>();

                double rowHeight = 0;

                for (int c = 0; c < columns.Length; c++)
                {
                    var font = columns[c].Font ?? _pw.Theme.FontNormal;

                    // Use an *inner* cell width with small left/right padding to prevent touching borders
                    const double cellPad = 2.0;
                    double innerWidth = Math.Max(1, widths[c] - 2 * cellPad);
                    innerWidths.Add(innerWidth);

                    var cellText = row.Length > c ? row[c] ?? "" : "";
                    var lines = PdfTextLayout.WrapText(cellText, font!, innerWidth - 0.5, _pw.Gfx);
                    wrappedCells.Add(lines);
                    rowHeight = Math.Max(rowHeight, lines.Length * _pw.Theme.LineHeight(font!));
                }

                _pf.EnsureSpace(rowHeight + 4);

                x = _pw._left;
                for (int c = 0; c < columns.Length; c++)
                {
                    var font = columns[c].Font ?? _pw.Theme.FontNormal;

                    var cellRect = new XRect(x, _pw.Y, widths[c], rowHeight);
                    const double cellPad = 2.0;
                    var innerRect = new XRect(cellRect.X + cellPad, cellRect.Y, Math.Max(1, cellRect.Width - 2 * cellPad), cellRect.Height);

                    var state = _pw.Gfx.Save();
                    var clipRect = new XRect(innerRect.X - 0.5, innerRect.Y, innerRect.Width + 1.0, innerRect.Height + 0.5);

                    _pw.Gfx.IntersectClip(clipRect);
                    Text.DrawLines(wrappedCells[c], font!, innerRect);
                    _pw.Gfx.Restore(state);

                    x += widths[c] + gap;
                }
                // Vertical line
                double xx = _pw._left;
                for (int c = 0; c < columns.Length - 1; c++)
                {
                    xx += widths[c];
                    // draw a faint separator in the gap center
                    double sepX = xx + gap / 2.0;
                    var pen = new XPen(XColors.LightGray, 0.3);
                    pen.DashStyle = XDashStyle.Dot;
                    _pw.Gfx.DrawLine(pen, sepX, _pw.Y, sepX, _pw.Y + rowHeight);
                    xx += gap;
                }
                _pw.Y += rowHeight;
                _pf.Spacer(4);
                _hf.DrawHairline(alpha: 0.1);
                _pf.Spacer(2);
            }
        }


        private void EnsureSpaceRow(XFont f1, string t1, XFont f2, string t2)
        {
            var l1 = PdfTextLayout.WrapText(t1, f1, _pw._contentWidth * 0.20, _pw.Gfx).Length * _pw.Theme.LineHeight(f1);
            var l2 = PdfTextLayout.WrapText(t2, f2, _pw._contentWidth * 0.80, _pw.Gfx).Length * _pw.Theme.LineHeight(f2);
            _pf.EnsureSpace(Math.Max(l1, l2) + 4);
        }
    }
}
