using PdfSharpCore.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xyToolz.Pdf;

namespace xyToolz.Pdf_Layout
{
#nullable enable
    internal class PdfTextLayout
    {
        public PageWriter _pw { get; }

        public PdfTextLayout(PageWriter pageWriter_)
        {
            _pw = pageWriter_;
        }



        internal void DrawLines(IEnumerable<string> lines, XFont font, XRect rect)
        {
            double y = rect.Y;
            foreach (var line in lines)
            {
                _pw.Gfx.DrawString(line, font, XBrushes.Black, new XRect(rect.X, y, rect.Width, _pw.Theme.LineHeight(font)), XStringFormats.TopLeft);
                y += _pw.Theme.LineHeight(font);
            }
        }


    

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="items"></param>
        public void DrawDefinitionList(string title, IEnumerable<(string Key, string Value)> items)
        {
            // Subheading stays as-is
            _pw.PageFlow.DrawSubheading(title);

            var keyFont = _pw.Theme.FontNormalBold;
            var valFont = _pw.Theme.FontNormal;

            // 1) Measure the widest key to auto-size the key column (tight but safe)
            double maxKey = 0;
            foreach (var (k, _) in items)
            {
                var txt = k ?? string.Empty;
                var w = _pw.Gfx.MeasureString(txt, keyFont).Width;
                if (w > maxKey) maxKey = w;
            }

            // Padding on the key column so the text doesn't touch the gutter
            const double keyPad = 1.5; // pt (very small – keeps columns visually tight)
                                       // Horizontal space between columns (the "gutter")
            const double gutter = 3.0; // ↓ reduce if you still want it tighter

            // Clamp key column between 12% and 35% of the width, using measured maxKey
            double keyWidth = Math.Min(_pw._contentWidth * 0.35, Math.Max(_pw._contentWidth * 0.12, maxKey + keyPad));
            double valWidth = Math.Max(24, _pw._contentWidth - keyWidth - gutter);


            const double lineHeightFactor = 1.5;// 0.90/ 0.85 /0.95
            double keyLH = _pw.Theme.LineHeight(keyFont!) * lineHeightFactor;
            double valLH = _pw.Theme.LineHeight(valFont!) * lineHeightFactor;
            double rowLH = Math.Max(keyLH, valLH); // same baseline step for both columns

            foreach (var (k, v) in items)
            {
                string keyText = k ?? string.Empty;
                string valText = v ?? string.Empty;

                // Wrap both sides using the actual column widths
                // (Use the gfx-based WrapText so measurement matches drawing)
                var keyLines = PdfTextLayout.WrapText(keyText, keyFont!, keyWidth, _pw.Gfx);
                var valLines = PdfTextLayout.WrapText(valText, valFont!, valWidth, _pw.Gfx);

                int linesCount = Math.Max(keyLines.Length, valLines.Length);
                double rowHeight = linesCount * rowLH;

                // Ensure there is enough space for the whole row (keys + values)
                _pw.PageFlow.EnsureSpace(rowHeight + 1);

                // --- Draw key column (left, bold) ---
                double yKey = _pw.Y;
                for (int i = 0; i < keyLines.Length; i++)
                {
                    var r = new XRect(_pw._left, yKey, keyWidth, rowLH);
                    _pw.Gfx.DrawString(keyLines[i], keyFont, XBrushes.Black, r, XStringFormats.TopLeft);
                    yKey += rowLH;
                }

                // --- Draw value column (right, normal) ---
                double xVal = _pw._left + keyWidth + gutter;
                double yVal = _pw.Y;
                for (int i = 0; i < valLines.Length; i++)
                {
                    var r = new XRect(xVal, yVal, valWidth, rowLH);
                    _pw.Gfx.DrawString(valLines[i], valFont, XBrushes.Black, r, XStringFormats.TopLeft);
                    yVal += rowLH;
                }

                // Advance Y by the full row height, then a tiny gap between rows
                _pw.Y += rowHeight;
                _pw.PageFlow.Spacer(1); // tighten: was 2
                                        // Optional hairline between rows (very subtle):
                                        // var pen = new XPen(XColors.Black, 0.2) { Transparency = 0.1 };
                                        // Gfx.DrawLine(pen, _left, Y, _right, Y);
                                        // Spacer(1);
            }

            // Smaller gap after the whole block
            _pw.PageFlow.Spacer(2); // was 4
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="font"></param>
        public void DrawParagraph(string text, XFont? font = null)
        {
            font ??= _pw.Theme.FontNormal;
            _pw.PageFlow.EnsureSpace(font!, text);
            DrawParagraph(text, new XRect(_pw._left,_pw. Y,_pw. _contentWidth, 0), font!);
            _pw.PageFlow.Spacer(_pw.Theme.ParagraphSpacing);
        }


        internal void DrawParagraph(string text, XRect rect, XFont font)
        {
            var lines = WrapText(text, font, rect.Width, _pw.Gfx);
            var h = lines.Length * _pw.Theme.LineHeight(font);
            rect = new XRect(rect.X, rect.Y, rect.Width, h);
            DrawLines(lines, font, rect);
            _pw.Y += h;
        }

        /// <summary>
        /// Word-wrap into lines that fit into 'maxWidth'. Keeps words intact.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="font"></param>
        /// <param name="maxWidth"></param>
        /// <returns></returns>
        internal List<string> WrapText(string text, XFont font, double maxWidth)
        {
            var lines = new List<string>();
            if (string.IsNullOrEmpty(text))
            {
                lines.Add(string.Empty);
                return lines;
            }

            var words = text.Split(' ');
            var line = new StringBuilder();
            foreach (var w in words)
            {
                var candidate = line.Length == 0 ? w : line.ToString() + " " + w;
                if (_pw.Gfx.MeasureString(candidate, font).Width <= maxWidth)
                {
                    if (line.Length == 0) line.Append(w); else line.Append(' ').Append(w);
                }
                else
                {
                    if (line.Length > 0) lines.Add(line.ToString());
                    line.Clear();
                    // If single word longer than maxWidth: hard-cut (rare for wide generics)
                    if (_pw.Gfx.MeasureString(w, font).Width > maxWidth)
                    {
                        var cut = new StringBuilder();
                        foreach (var ch in w)
                        {
                            var cand2 = cut.ToString() + ch;
                            if (_pw.Gfx.MeasureString(cand2, font).Width > maxWidth)
                            {
                                if (cut.Length > 0) lines.Add(cut.ToString());
                                cut.Clear();
                            }
                            cut.Append(ch);
                        }
                        if (cut.Length > 0) line.Append(cut.ToString());
                    }
                    else
                    {
                        line.Append(w);
                    }
                }
            }
            if (line.Length > 0) lines.Add(line.ToString());
            return lines;
        }


        internal static string[] WrapText(string text, XFont font, double width, XGraphics gfx)
        {
            if (string.IsNullOrEmpty(text)) return new[] { "" };
            var words = text.Replace("\r", "").Split('\n')
                            .SelectMany(line => line.Split(' ').DefaultIfEmpty(""))
                            .ToArray();

            var lines = new List<string>();
            var sb = new StringBuilder();
            foreach (var w in words)
            {
                var probe = sb.Length == 0 ? w : sb + " " + w;
                var size = gfx.MeasureString(probe, font).Width;
                if (size > width && sb.Length > 0)
                {
                    lines.Add(sb.ToString());
                    sb.Clear();
                    sb.Append(w);
                }
                else
                {
                    if (sb.Length == 0) sb.Append(w);
                    else sb.Append(' ').Append(w);
                }
            }
            if (sb.Length > 0) lines.Add(sb.ToString());
            return lines.Count == 0 ? new[] { "" } : lines.ToArray();
        }


        // Fits text into maxWidth by trimming and appending an ellipsis if needed.
        internal string FitWithEllipsis(string text, XFont font, double maxWidth)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            if (_pw.Gfx.MeasureString(text, font).Width <= maxWidth) return text;

            const string ell = "…";
            string t = text;
            while (t.Length > 1 && _pw.Gfx.MeasureString(t + ell, font).Width > maxWidth)
                t = t[..^1];
            return t + ell;
        }

    }
}
