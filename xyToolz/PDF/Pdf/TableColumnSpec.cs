using PdfSharpCore.Drawing;

namespace xyToolz.Pdf
{
#nullable enable

    /// <summary>
    /// Set the specification for the table columns
    /// </summary>
    public class TableColumnSpec
        {
            /// <summary>
            /// Get the heder text
            /// </summary>
            public string Header { get; }
            /// <summary>
            /// Get the width ratio
            /// </summary>
            public double WidthRatio { get; }

            /// <summary>
            /// Get the font
            /// </summary>
            public XFont? Font { get; }

            /// <summary>
            /// Basic constructor
            /// </summary>
            /// <param name="header"></param>
            /// <param name="widthRatio"></param>
            /// <param name="font"></param>
            public TableColumnSpec(string header, double widthRatio, XFont? font = null)
            {
                Header = header; WidthRatio = widthRatio; Font = font;
            }
        }
    }

