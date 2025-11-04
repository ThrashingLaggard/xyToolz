using PdfSharpCore.Drawing.Layout;
using PdfSharpCore.Pdf;
using System;
using System.Collections.Generic;
using xyToolz.Pdf_Layout;
using XGraphics = PdfSharpCore.Drawing.XGraphics;
using XRect = PdfSharpCore.Drawing.XRect;

namespace xyToolz.Pdf
{
#nullable enable
    /// <summary>
    /// Writes content to a pdf page according to the given RenderContext and PdfTheme
    /// </summary>
    public sealed class PageWriter : IDisposable
    {
        /// <summary>Add useful infos here </summary>
        public string? Description { get; set; }
        
        /// <summary> PdfPage to write on </summary>
        public PdfPage Page { get; set; }

        /// <summary> XGrafix element </summary>
        public XGraphics Gfx { get; set; }
        
        /// <summary> Height value </summary>
        public double Y { get; set; }
        
        /// <summary> Data storage </summary>
        public RenderContext Ctx { get; }
        
        /// <summary>
        /// Get the pdf theme from the RenderContext
        /// </summary>
        public PdfTheme Theme => Ctx.Theme;

        /// <summary>
        /// Draw Header and Footer
        /// </summary>
        public bool DrawHeaderFooter { get; set; } = true;

        /// <summary>
        /// Optional: override header text (e.g., parent type)
        /// </summary>
        public string? PageHeaderOverride { get; set; }

        internal readonly double _left, _top, _right, _bottom, _contentWidth;

        private readonly XTextFormatter _formatter;
        public XTextFormatter Formatter => _formatter;


        internal PdfHeaderFooterLayout HeaderFooter { get; }
        internal PdfTextLayout Text { get; }
        internal PdfPageFlow PageFlow { get; }
        internal PdfTableLayout Tables { get; }
        internal PdfTocLayout Toc { get; }

        /// <summary>
        /// Basic constructor
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="page"></param>
        public PageWriter(RenderContext ctx, PdfPage page)
        {
            Ctx = ctx;
            Page = page;
            if (Page.Owner == null)
            {
                Ctx.Document.Pages.Add(Page);
            }
            Gfx = XGraphics.FromPdfPage(page);
            _formatter = new XTextFormatter(Gfx);

            _left = Theme.MarginLeft;
            _right = page.Width - Theme.MarginRight;
            _top = Math.Max(8, Theme.MarginTop);
            _bottom = page.Height - Math.Max(8, Theme.MarginBottom - 24);

            _contentWidth = _right - _left;

            Y = _top;

            Text = new PdfTextLayout(this);
            HeaderFooter = new PdfHeaderFooterLayout(this);
            PageFlow = new PdfPageFlow(this, Text, HeaderFooter);
            Tables = new PdfTableLayout(this, Text, PageFlow,HeaderFooter);
            Toc = new PdfTocLayout(this, Text, PageFlow);
            
            if (DrawHeaderFooter)
                HeaderFooter.DrawHeaderFooterArea();
        }

        public void DrawHeading(int level, string text) => PageFlow.DrawHeading(level, text);

        public void DrawSubheading(string text) =>PageFlow.DrawSubheading(text);

        public void DrawParagraph(string text) =>Text.DrawParagraph(text);

        public void DrawDefinitionList(string title, IEnumerable<(string Key, string Value)> items) =>Text.DrawDefinitionList(title, items);

        public void DrawBulletLine(string label, string value) =>PageFlow.DrawBulletLine(label, value);

        public void Spacer(double points = 4) =>PageFlow.Spacer(points);

        public void DrawHairline() => HeaderFooter.DrawHairline(Y);

        public XRect DrawTocLineWrapped(string kind, string leftText, int pageNumber, double kindWidthPt, double kindGapPt) =>Toc.DrawTocLineWrapped(kind, leftText, pageNumber, kindWidthPt, kindGapPt);

        public void DrawTable(TableColumnSpec[] columns, IEnumerable<string[]> rows) =>Tables.DrawTable(columns, rows);

        /// <summary> Dispose of the XGraphix instance in the Gfx property </summary>
       public void Dispose()
        {
            Gfx?.Dispose();
        }
    }
    }

