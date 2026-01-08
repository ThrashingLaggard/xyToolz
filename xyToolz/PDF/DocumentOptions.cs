namespace xyToolz.Pdf
{
    // =====================================================================
    // Core primitives
    // =====================================================================

    /// <summary>
    /// Simple thickness model for margins/paddings with convenience factories.
    /// Using points (pt) as unit to match PDF coordinate system.
    /// </summary>
    public struct Thickness
    {
        /// <summary>Total left margin/padding in points.</summary>
        public double Left { get; set; }
        /// <summary>Total top margin/padding in points.</summary>
        public double Top { get; set; }
        /// <summary>Total right margin/padding in points.</summary>
        public double Right { get; set; }
        /// <summary>Total bottom margin/padding in points.</summary>
        public double Bottom { get; set; }

        /// <summary>Create a uniform thickness (all sides).</summary>
        public static Thickness Uniform(double pts) => new Thickness { Left = pts, Top = pts, Right = pts, Bottom = pts };
        /// <summary>Create a horizontal/vertical thickness pair.</summary>
        public static Thickness Symmetric(double horizontal, double vertical) => new Thickness { Left = horizontal, Right = horizontal, Top = vertical, Bottom = vertical };
        /// <summary>Create from explicit sides.</summary>
        public static Thickness From(double left, double top, double right, double bottom) => new Thickness { Left = left, Top = top, Right = right, Bottom = bottom };
        /// <summary>Construct from millimeters (1 mm ≈ 2.834645669 pt).</summary>
        public static Thickness FromMillimeters(double mmUniform)
        {
            const double PT_PER_MM = 72.0 / 25.4; // 2.834645669…
            var pts = mmUniform * PT_PER_MM;
            return Uniform(pts);
        }
    }

    /// <summary>
    /// Common horizontal alignment flags used by text and tables.
    /// </summary>
    public enum HorizontalAlign { Left, Center, Right, Justify }

    /// <summary>
    /// Common vertical alignment flags used by table cells and inline blocks.
    /// </summary>
    public enum VerticalAlign { Top, Middle, Bottom, Baseline }

    /// <summary>
    /// Page size presets. You can map these to PdfSharpCore sizes in the renderer.
    /// </summary>
    public enum PageSize
    {
        A4, A5, A3, Letter, Legal, Tabloid, Custom
    }

    /// <summary>
    /// Page orientation.
    /// </summary>
    public enum Orientation { Portrait, Landscape }

    /// <summary>
    /// Line style for borders or rules.
    /// </summary>
    public enum LineStyle { None, Solid, Dashed, Dotted }

    /// <summary>
    /// Border model with width, color (gray 0..1 for simplicity), and style.
    /// Extend to full RGB if you need.
    /// </summary>
    public struct Border
    {
        public double Width { get; set; }
        /// <summary>Gray color 0..1 (0 = black, 1 = white). Extend to RGB later if needed.</summary>
        public double Gray { get; set; }
        public LineStyle Style { get; set; }

        public static Border None() => new Border { Width = 0, Gray = 0, Style = LineStyle.None };
        public static Border Hairline(double gray = 0) => new Border { Width = 0.5, Gray = gray, Style = LineStyle.Solid };
        public static Border Solid(double width, double gray = 0) => new Border { Width = width, Gray = gray, Style = LineStyle.Solid };
    }

    // =====================================================================
    // Theme / Typography
    // =====================================================================

    /// <summary>
    /// A font specification used across the theme.
    /// Keep it intentionally minimal for PdfSharpCore (family + size + bold/italic switches).
    /// </summary>
    public sealed class FontSpec
    {
        /// <summary>Font family name (e.g., "Arial", "Times New Roman").</summary>
        public string Family { get; set; } = "Times New Roman";
        /// <summary>Base size in points.</summary>
        public double Size { get; set; } = 11.0;
        /// <summary>Bold simulation flag (renderer may map to actual font face if available).</summary>
        public bool Bold { get; set; }
        /// <summary>Italic simulation flag (renderer may map to italic face if available).</summary>
        public bool Italic { get; set; }

        public static FontSpec Make(string family, double size, bool bold = false, bool italic = false)
            => new FontSpec { Family = family, Size = size, Bold = bold, Italic = italic };
    }

    /// <summary>
    /// Color palette used by headings, emphasis, rules, and table zebra stripes.
    /// Using Gray (0..1) to avoid overdesign; expand to RGB if required.
    /// </summary>
    public sealed class Palette
    {
        /// <summary>Default text gray (0 = black).</summary>
        public double Text { get; set; } = 0.0;
        /// <summary>Heading accent gray (for H1/H2).</summary>
        public double Heading { get; set; } = 0.0;
        /// <summary>Secondary text gray (captions, metadata).</summary>
        public double Muted { get; set; } = 0.35;
        /// <summary>Light gray used for rules/borders.</summary>
        public double Rule { get; set; } = 0.70;
        /// <summary>Zebra row background gray (0..1, larger = lighter).</summary>
        public double Zebra { get; set; } = 0.94;
    }

    /// <summary>
    /// Spacing system centralizing paddings/gaps.
    /// </summary>
    public sealed class Spacing
    {
        public double Tiny { get; set; } = 2;
        public double Small { get; set; } = 4;
        public double Base { get; set; } = 8;
        public double Large { get; set; } = 12;
        public double XLarge { get; set; } = 20;
    }

    /// <summary>
    /// Global theme controlling typography and spacing used by all components.
    /// </summary>
    public sealed class xyPdfTheme
    {
        /// <summary>Body font applied to paragraphs and table cells.</summary>
        public FontSpec Body { get; set; } = FontSpec.Make("Times New Roman", 11);
        /// <summary>Monospaced font for code snippets.</summary>
        public FontSpec Mono { get; set; } = FontSpec.Make("Consolas", 10);
        /// <summary>Heading base font; renderer typically scales for H1..H4.</summary>
        public FontSpec Heading { get; set; } = FontSpec.Make("Times New Roman", 12, bold: true);
        /// <summary>Heading scale factors (H1..H4). E.g., {1.8, 1.4, 1.2, 1.0} multiplies Heading.Size.</summary>
        public double[] HeadingScale { get; set; } = new[] { 1.8, 1.4, 1.2, 1.0 };
        /// <summary>Color palette for text, rules, zebra, etc.</summary>
        public Palette Colors { get; set; } = new Palette();
        /// <summary>Global spacing ramp for gaps and paddings.</summary>
        public Spacing Space { get; set; } = new Spacing();

        /// <summary>Clone helper for functional style modifications.</summary>
        public xyPdfTheme With(Action<xyPdfTheme> mutate)
        {
            var clone = (xyPdfTheme)MemberwiseClone();
            // shallow clones are fine here because FontSpec/Palette/Spacing are reference types intentionally
            mutate(clone);
            return clone;
        }
    }

    // =====================================================================
    // Document-level options
    // =====================================================================

    /// <summary>
    /// Global options for a PDF document. These are consumed by the renderer to create
    /// pages, lay out content, and write metadata/bookmarks/TOC.
    /// </summary>
    public sealed class xyPdfOptions
    {
        // ---- Page geometry & layout ----

        /// <summary>Logical page size preset; set to Custom if you provide explicit width/height.</summary>
        public PageSize Size { get; set; } = PageSize.A4;

        /// <summary>Orientation for the document (portrait/landscape).</summary>
        public Orientation Orientation { get; set; } = Orientation.Portrait;

        /// <summary>Margins for the text area (pt).</summary>
        public Thickness Margins { get; set; } = Thickness.FromMillimeters(20);

        /// <summary>Custom page width in points. Used only when <see cref="Size"/> == Custom.</summary>
        public double CustomWidth { get; set; } = 595.28; // ≈ A4 portrait width

        /// <summary>Custom page height in points. Used only when <see cref="Size"/> == Custom.</summary>
        public double CustomHeight { get; set; } = 841.89; // ≈ A4 portrait height

        // ---- Header / Footer ----

        /// <summary>Enable drawing header and footer on all content pages.</summary>
        public bool DrawHeaderFooter { get; set; } = true;

        /// <summary>Header content provider. If null, header is omitted.</summary>
        public Func<xyHeaderFooterContext, string>? HeaderProvider { get; set; }
            = ctx => ctx.Title ?? string.Empty;

        /// <summary>Footer content provider. If null, footer is omitted.</summary>
        public Func<xyHeaderFooterContext, string>? FooterProvider { get; set; }
            = ctx => $"Page {ctx.PageNumber}";

        /// <summary>Header bottom border (rule under header).</summary>
        public Border HeaderRule { get; set; } = Border.Solid(0.5, gray: 0.8);

        /// <summary>Footer top border (rule above footer).</summary>
        public Border FooterRule { get; set; } = Border.Solid(0.5, gray: 0.8);

        /// <summary>Header inner padding.</summary>
        public Thickness HeaderPadding { get; set; } = Thickness.Symmetric(horizontal: 8, vertical: 4);

        /// <summary>Footer inner padding.</summary>
        public Thickness FooterPadding { get; set; } = Thickness.Symmetric(horizontal: 8, vertical: 4);

        // ---- TOC / Bookmarks ----

        /// <summary>Generate a Table of Contents page at the beginning.</summary>
        public bool EnableToc { get; set; } = true;

        /// <summary>
        /// Heading text for the TOC.
        /// </summary>
        public string TocTitle { get; set; } = "Table of Contents";

        /// <summary>
        /// Include H1..Hn entries in the TOC. The value denotes the maximum heading level.
        /// Example: 2 = include H1 and H2.
        /// </summary>
        public int TocMaxLevel { get; set; } = 2;

        /// <summary>Create PDF outline/bookmarks based on headings.</summary>
        public bool EnableBookmarks { get; set; } = true;

        // ---- Typography / Theme ----

        /// <summary>Global theme applied to all content.</summary>
        public xyPdfTheme Theme { get; set; } = new xyPdfTheme();

        // ---- Metadata ----

        /// <summary>Document title metadata and optional header default.</summary>
        public string? Title { get; set; }
        /// <summary>Document author metadata.</summary>
        public string? Author { get; set; }
        /// <summary>Subject/description metadata.</summary>
        public string? Subject { get; set; }
        /// <summary>Keywords metadata (comma-separated or pre-split by caller).</summary>
        public List<string> Keywords { get; set; } = new();

        // ---- Rendering / Engine knobs (advanced) ----

        /// <summary>When true, the renderer may compress streams to reduce file size.</summary>
        public bool EnableCompression { get; set; } = true;

        /// <summary>When true, image assets may be downscaled to match target DPI budget.</summary>
        public bool DownscaleImages { get; set; } = true;

        /// <summary>Target DPI for images when <see cref="DownscaleImages"/> is enabled.</summary>
        public int TargetImageDpi { get; set; } = 144;

        /// <summary>Preserve vector drawings strictly (may increase size). If false, allow rasterization.</summary>
        public bool PreferVector { get; set; } = true;

        // ---- Safety / Diagnostics ----

        /// <summary>Emit light debug markers (page frames, text baselines) to help tune layout.
        /// NEVER enable this in production exports.</summary>
        public bool DebugVisualizeLayout { get; set; } = false;
    }

    /// <summary>
    /// Context provided to header/footer providers so they can render dynamic strings.
    /// </summary>
    public sealed class xyHeaderFooterContext
    {
        public string? Title { get; init; }
        public int PageNumber { get; init; }
        public int TotalPages { get; init; }
        public DateTime NowUtc { get; init; } = DateTime.UtcNow;
    }

    // =====================================================================
    // Page / content options (used inside composer/builders)
    // =====================================================================

    /// <summary>
    /// Page-level overrides that are applied when starting a new page.
    /// All properties are optional; unset fields inherit from <see cref="xyPdfOptions"/>.
    /// </summary>
    public sealed class xyPageOptions
    {
        public PageSize? Size { get; set; }
        public Orientation? Orientation { get; set; }
        public Thickness? Margins { get; set; }
        public bool? DrawHeaderFooter { get; set; }
    }

    /// <summary>
    /// Rich text options controlling paragraph-level formatting.
    /// Use this for block text operations (not inline runs).
    /// </summary>
    public sealed class xyTextOptions
    {
        /// <summary>Font for this block; defaults to Theme.Body.</summary>
        public FontSpec? Font { get; set; }
        /// <summary>Target width for wrapping; if null, renderer uses page content width.</summary>
        public double? MaxWidth { get; set; }
        /// <summary>Line height multiplier (1.0 = natural line spacing).</summary>
        public double LineHeight { get; set; } = 1.2;
        /// <summary>Horizontal alignment for the paragraph.</summary>
        public HorizontalAlign Align { get; set; } = HorizontalAlign.Left;
        /// <summary>Space inserted before the block (pt).</summary>
        public double SpacingBefore { get; set; } = 0;
        /// <summary>Space inserted after the block (pt).</summary>
        public double SpacingAfter { get; set; } = 8;
        /// <summary>Optional left rule/border for callouts or block quotes.</summary>
        public Border LeftBorder { get; set; } = Border.None();
        /// <summary>Optional background shade for the block (gray 0..1); null = transparent.</summary>
        public double? BackgroundGray { get; set; } = null;
        /// <summary>Optional inner padding when background/border is used.</summary>
        public Thickness Padding { get; set; } = Thickness.Symmetric(8, 6);
    }

    /// <summary>
    /// Heading options that are also used for TOC/bookmark generation.
    /// </summary>
    public sealed class xyHeadingOptions
    {
        /// <summary>Heading level (1..4 typically). Used for size scaling and TOC depth.</summary>
        public int Level { get; set; } = 1;
        /// <summary>Explicit anchor name (for internal linking); auto-generated if null.</summary>
        public string? Anchor { get; set; }
        /// <summary>Override font; otherwise Theme.Heading with scaled size is used.</summary>
        public FontSpec? Font { get; set; }
        /// <summary>Additional space before the heading (pt).</summary>
        public double SpacingBefore { get; set; } = 12;
        /// <summary>Additional space after the heading (pt).</summary>
        public double SpacingAfter { get; set; } = 6;
        /// <summary>Underline the heading using palette Rule color.</summary>
        public bool Underline { get; set; } = false;
    }

    /// <summary>
    /// Options for rendering a simple table from data rows.
    /// </summary>
    public sealed class xyTableOptions
    {
        /// <summary>Automatically size columns to available width. If false, use <see cref="ColumnWidths"/>.</summary>
        public bool AutoFit { get; set; } = true;
        /// <summary>Explicit column widths in points (ignored if AutoFit = true). Sum should fit content width.</summary>
        public List<double>? ColumnWidths { get; set; }
        /// <summary>Default horizontal alignment for cell text.</summary>
        public HorizontalAlign Align { get; set; } = HorizontalAlign.Left;
        /// <summary>Optional fixed row height (pt). If null, height grows with content.</summary>
        public double? RowHeight { get; set; }
        /// <summary>Cell inner padding.</summary>
        public Thickness CellPadding { get; set; } = Thickness.Symmetric(6, 4);
        /// <summary>Draw horizontal grid lines between rows.</summary>
        public bool GridHorizontal { get; set; } = true;
        /// <summary>Draw vertical grid lines between columns.</summary>
        public bool GridVertical { get; set; } = false;
        /// <summary>Grid border style.</summary>
        public Border GridBorder { get; set; } = Border.Solid(0.5, gray: 0.8);
        /// <summary>Zebra stripe alternating row backgrounds.</summary>
        public bool Zebra { get; set; } = true;
        /// <summary>Gray value for zebra background (fallback if Theme.Colors.Zebra is not used).</summary>
        public double ZebraGray { get; set; } = 0.94;
        /// <summary>Header row styling. If null, header is omitted.</summary>
        public xyTableHeaderOptions? Header { get; set; } = new xyTableHeaderOptions();
    }

    /// <summary>Options controlling the optional header row of a table.</summary>
    public sealed class xyTableHeaderOptions
    {
        /// <summary>Header font; defaults to Theme.Heading scaled near body size.</summary>
        public FontSpec? Font { get; set; }
        /// <summary>Header cell padding.</summary>
        public Thickness Padding { get; set; } = Thickness.Symmetric(6, 4);
        /// <summary>Header bottom rule.</summary>
        public Border BottomRule { get; set; } = Border.Solid(1.0, gray: 0.65);
        /// <summary>Background fill gray (null = transparent).</summary>
        public double? BackgroundGray { get; set; } = 0.98;
        /// <summary>Horizontal alignment for header text.</summary>
        public HorizontalAlign Align { get; set; } = HorizontalAlign.Left;
    }

    /// <summary>
    /// Options for hyperlinks and cross-references.
    /// </summary>
    public sealed class xyLinkOptions
    {
        /// <summary>Underline visible links.</summary>
        public bool Underline { get; set; } = true;
        /// <summary>Link color gray (0..1). 0 = black; use muted gray for subtle style.</summary>
        public double Gray { get; set; } = 0.0;
        /// <summary>Resolve internal anchors (e.g., headings) into PDF destinations.</summary>
        public bool EnableInternalAnchors { get; set; } = true;
        /// <summary>Open external links in new window/tab (viewer hint; may be ignored by readers).</summary>
        public bool ExternalNewWindow { get; set; } = true;
    }

    /// <summary>
    /// Options for rendering code blocks with a monospaced font and optional background.
    /// </summary>
    public sealed class xyCodeBlockOptions
    {
        /// <summary>Font; defaults to Theme.Mono.</summary>
        public FontSpec? Font { get; set; }
        /// <summary>Left rule (classic code block marker).</summary>
        public Border LeftRule { get; set; } = Border.Solid(2, gray: 0.75);
        /// <summary>Background shade (null = transparent).</summary>
        public double? BackgroundGray { get; set; } = 0.97;
        /// <summary>Inner padding.</summary>
        public Thickness Padding { get; set; } = Thickness.Symmetric(10, 8);
        /// <summary>Space after the block.</summary>
        public double SpacingAfter { get; set; } = 8;
        /// <summary>Wrap long lines. If false, renderer may clip or horizontally scroll.</summary>
        public bool SoftWrap { get; set; } = true;
    }

    /// <summary>
    /// Options controlling inline or block images.
    /// </summary>
    public sealed class xyImageOptions
    {
        /// <summary>Target width in points; if null, use image natural width constrained by content area.</summary>
        public double? Width { get; set; }
        /// <summary>Target height in points; if null, preserve aspect ratio using Width.</summary>
        public double? Height { get; set; }
        /// <summary>Horizontal alignment when image is narrower than content width.</summary>
        public HorizontalAlign Align { get; set; } = HorizontalAlign.Left;
        /// <summary>Space before the image block.</summary>
        public double SpacingBefore { get; set; } = 0;
        /// <summary>Space after the image block.</summary>
        public double SpacingAfter { get; set; } = 8;
        /// <summary>Optional caption text displayed below the image.</summary>
        public string? Caption { get; set; }
        /// <summary>Caption font; defaults to Theme.Body with muted color and smaller size.</summary>
        public FontSpec? CaptionFont { get; set; }
        /// <summary>Border around the image.</summary>
        public Border Border { get; set; } = Border.None();
        /// <summary>Inner padding when border/background is used.</summary>
        public Thickness Padding { get; set; } = Thickness.Uniform(0);
        /// <summary>Optional background gray (null = transparent).
        /// Useful to indicate image bounds on white pages.</summary>
        public double? BackgroundGray { get; set; }
    }

    // =====================================================================
    // Convenience model for composer
    // =====================================================================

    /// <summary>
    /// A thin façade for building pages in a declarative way, to be used by xyPdf.Render(..., compose, options).
    /// This is only the options side; the actual builder lives in the renderer.
    /// </summary>
    public sealed class xyPdfComposer
    {
        // Intentionally empty here; your concrete builder lives in the renderer layer.
        // Kept to show intended API shape in XML docs and to keep examples compile-friendly.
    }
}
