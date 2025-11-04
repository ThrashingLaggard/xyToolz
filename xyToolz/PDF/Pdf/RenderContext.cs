using PdfSharpCore.Pdf;
using System;

namespace xyToolz.Pdf
{
#nullable enable
    /// <summary>
    /// Conveniently stores information for better oversight
    /// </summary>
    public class RenderContext
    {
        /// <summary>
        /// Name of this section
        /// </summary>
        public string? CurrentSectionTitle { get; set; }
        /// <summary>
        /// Add custom infos
        /// </summary>
        public string? Description{ get; set; }
        /// <summary>
        /// 
        /// </summary>
        public PdfDocument Document { get; }
        /// <summary>
        /// 
        /// </summary>
        public PdfTheme Theme { get; }
        /// <summary>
        /// 
        /// </summary>
        public PageWriter? Writer { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public AnchorRegistry Anchors { get; } = new AnchorRegistry();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pdf_Document_"></param>
        /// <param name="pdf_Theme_"></param>
        /// <param name="pwr_Writer_"></param>
        public RenderContext(PdfDocument pdf_Document_, PdfTheme pdf_Theme_, PageWriter? pwr_Writer_ = null)
        {
            Document = pdf_Document_;
            Theme = pdf_Theme_;
            Writer = pwr_Writer_ ?? null; 



        }

        /// <summary>
        /// Adds either the target page or a clean and fresh one to the Document property
        /// </summary>
        /// <param name="pdf_Page_"></param>
        /// <returns>The newly added PdfPage, basically either the param or a blank, lol</returns>
        public PdfPage AddPage(PdfPage? pdf_Page_ = null)
        {
            PdfPage page;

            if (pdf_Page_ is not null)
            {
                // Wenn Seite noch keinem Document gehört → hinzufügen
                if (pdf_Page_.Owner == null)
                {
                    Document.Pages.Add(pdf_Page_);
                }
                else if (!ReferenceEquals(pdf_Page_.Owner, Document))
                {
                    // Sicherheit: Seite gehört zu einem anderen Document
                    throw new InvalidOperationException("Cannot add a page that belongs to another PdfDocument.");
                }

                page = pdf_Page_;
            }
            else
            {
                // Neue Seite direkt in diesem Document erzeugen
                page = Document.AddPage();
                page.Size = PdfSharpCore.PageSize.A4;
            }

            return page;
        }



        /// <summary>
        /// Gets the current page number from a one based indexxx
        /// </summary>
        public int PageNumber => Document.Pages.Count; // 1-based user expectation
    }



}
