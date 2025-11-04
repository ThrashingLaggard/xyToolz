using PdfSharpCore.Fonts;
using xyToolz.Fonts;
using XColor = PdfSharpCore.Drawing.XColor;
using XFont = PdfSharpCore.Drawing.XFont;

namespace xyToolz.Pdf
{
#nullable enable
    /// <summary>
    /// Visual theme for PDF output (margins, spacing, colors, fonts).
    /// Fonts are resolved by your IFontResolver (e.g., AutoResourceFontResolver),
    /// so font family names must match the resolver's logical families.
    /// </summary>
    public class PdfTheme
    {
        static PdfTheme()
        {
            if (GlobalFontSettings.FontResolver == null)
                GlobalFontSettings.FontResolver = new AutoResourceFontResolver();
        }
        /// <summary>
        /// Your custom infos here:
        /// </summary>
        public string? Description { get; set; }

        /// <summary> Value for left margin; standard is 54  </summary>
        public double MarginLeft { get; init; } = 54;  // 0.75"
        /// <summary> Value for right margin; standard is 54  </summary>
        public double MarginRight { get; init; } = 54;

        /// <summary> Value for the top margin; standard is 36 </summary>
        public double MarginTop { get; init; } = 36;

        /// <summary> Value for the bottom margin; standard is 36  </summary>
        public double MarginBottom { get; init; } = 36;

        /// <summary> Value for the top of the page header; standard is 36  </summary>
        public double PageHeaderTop { get; init; } = 36;

        /// <summary> Value for the spacing in paragraphs; standard is 6  </summary>
        public double ParagraphSpacing { get; init; } = 6;
        /// <summary> Value for the spacing between colums in the table; standard is 10  </summary>
        public double TableColGap { get; init; } = 10;

        /// <summary> Value for the line  spacing ; standard is 1.0  </summary>
        public double LineSpacingHeading { get; init; } = 1.0;

        /// <summary> Pimary color (40,40,40) </summary>
        public XColor ColorPrimary { get; init; } = XColor.FromArgb(40, 40, 40);
        /// <summary> Dark color (30,30,30)</summary>
        public XColor ColorDark { get; init; } = XColor.FromArgb(30, 30, 30);

        // Fonts (resolved by the global IFontResolver)
        /// <summary>
        /// Biggest Heading
        /// </summary>
        public XFont? FontH1 { get; init; }
        /// <summary>
        /// Second biggest heading
        /// </summary>
        public XFont? FontH2 { get; init; }
        /// <summary>
        /// Third biggest header
        /// </summary>
        public XFont? FontH3 { get; init; }
        /// <summary>
        /// Fourth biggest heading
        /// </summary>
        public XFont? FontH4 { get; init; }

        /// <summary> Normal Font  </summary>
        public XFont? FontNormal { get; init; }
        /// <summary> Normal bold Font  </summary>
        public XFont? FontNormalBold { get; init; }
        /// <summary> Small Font  </summary>
        public XFont? FontSmall { get; init; }
        /// <summary> Small bold Font  </summary>
        public  XFont? FontSmallBold { get; init; }
        /// <summary> Monospace Font  </summary>
        public XFont? FontMono { get; init; }

        /// <summary>
        /// Creates a default theme using logical families provided by the resolver:
        ///   - "XY Sans" for text/headings
        ///   - "XY Mono" for code
        /// </summary>
        public static PdfTheme CreateDefault()
        {
            const string Sans = AutoResourceFontResolver.FamilySans; // "XY Sans"
            const string Mono = AutoResourceFontResolver.FamilyMono; // "XY Mono"

            System.Diagnostics.Debug.WriteLine("Resolver=" + (PdfSharpCore.Fonts.GlobalFontSettings.FontResolver?.GetType().FullName ?? "null"));

            // NOTE: keep it simple—no system font names like "Verdana"/"Consolas".
            var h1 = new XFont(Sans, 18, PdfSharpCore.Drawing.XFontStyle.Bold);
            var h2 = new XFont(Sans, 14, PdfSharpCore.Drawing.XFontStyle.Bold);
            var h3 = new XFont(Sans, 12, PdfSharpCore.Drawing.XFontStyle.Bold);
            var h4 = new XFont(Sans, 11, PdfSharpCore.Drawing.XFontStyle.Bold);
            var body = new XFont(Sans, 10, PdfSharpCore.Drawing.XFontStyle.Regular);
            var bodyBold = new XFont(Sans, 10, PdfSharpCore.Drawing.XFontStyle.Bold);
            var small = new XFont(Sans, 8, PdfSharpCore.Drawing.XFontStyle.Regular);
            var smallBold = new XFont(Sans, 8, PdfSharpCore.Drawing.XFontStyle.Bold);
            var mono = new XFont(Mono, 9.5, PdfSharpCore.Drawing.XFontStyle.Regular);

            return new PdfTheme
            {
                FontH1 = h1,
                FontH2 = h2,
                FontH3 = h3,
                FontH4 = h4,
                FontNormal = body,
                FontNormalBold = bodyBold,
                FontSmall = small,
                FontSmallBold = smallBold,
                FontMono = mono
            };
        }

        /// <summary>
        /// Returns a simple line-height estimate for the given font.
        /// </summary>
        public double LineHeight(XFont f) => f.Size * 1.5;
    }
}
