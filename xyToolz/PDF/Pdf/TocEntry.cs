using PdfSharpCore.Pdf;

namespace xyToolz.Pdf
{
#nullable enable

    /// <summary>
    /// Represents a specific dataset in a content table
    /// </summary>
    public class TocEntry
    {
        /// <summary>
        /// The dataset's name
        /// </summary>
        public string Title { get; set; } = "";
        
        /// <summary>
        /// Add useful information for internal use
        /// </summary>
        public string? Infos { get; set; }

        /// <summary>
        /// The member's signature
        /// </summary>
        public string? Signature { get; set; }     // i.e., generic signature
        
        /// <summary>
        /// The description for this entry
        /// </summary>
        public string? Description { get; set; } // i.e., summary snippet



        /// <summary>
        /// The number of the page in a pdf document
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// The page itself
        /// </summary>
        public PdfPage? Page { get; set; }
        
        /// <summary>
        /// Value of the Y - coordinate
        /// </summary>
        public double Y { get; set; }
    }

}
