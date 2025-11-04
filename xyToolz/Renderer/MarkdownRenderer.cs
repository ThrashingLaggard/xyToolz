using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using xyToolz.Docs;
using xyToolz.Markdown;
using xyToolz.QOL;

namespace xyToolz.Renderer
{
#nullable enable
    /// <summary>
    /// Provides static methods to generate structured code documentation
    /// in Markdown format.
    /// </summary>
    public static class MarkdownRenderer
    {
        /// <summary>
        /// Renders a TypeDoc object and all its nested types recursively
        /// into a Markdown string.
        /// </summary>
        /// <param name="td_Type">The TypeDoc object to be rendered.</param>
        /// <param name="level_">The starting heading level for the Markdown output (e.g., 1 for #, 2 for ##).</param>
        /// <param name="prebuiltAnchorMap"></param>
        /// <returns>A string containing the generated Markdown documentation.</returns>
        public static string Render(TypeDoc td_Type, int level_ = 1, Dictionary<string, string>? prebuiltAnchorMap = null)
        {
            // The StringBuilder is used for efficient string manipulation and building.
            StringBuilder sb_MarkdownBuilder = new();

            // Create the central context map for internal linking
            var anchorMap = prebuiltAnchorMap ?? MdAnchor.BuildAnchorMap(td_Type);

            // Add a global top anchor so we can link "Back to top" reliably
            // Keep it invisible and at the very beginning of the document.
            sb_MarkdownBuilder.AppendLine("<span id=\"top\"></span>");

            // Optional: Table of Contents only for top-level calls (level==1) to avoid duplication when recursing
            if (level_ == 1)
            {
                MdMembersTable.RenderTableOfContents(sb_MarkdownBuilder, td_Type, anchorMap);
                sb_MarkdownBuilder.AppendLine(); // spacing after TOC
            }


            // Passing the map to all helper methods for anchoring and linking.
            MdSections.RenderHeader(sb_MarkdownBuilder, td_Type, level_, anchorMap);
            MdSections.RenderMetadata(sb_MarkdownBuilder, td_Type,level_ ,anchorMap);
            MdSections.RenderDescriptionFromXmlSummaryInTypeDoc(sb_MarkdownBuilder, td_Type, level_,anchorMap);
            MdMembersTable.RenderAllMembers(sb_MarkdownBuilder, td_Type,level_, anchorMap);
            MdSections.RenderNestedTypes(sb_MarkdownBuilder, td_Type, level_, anchorMap);

            // Add a subtle back-to-top link at the end of each rendered type block
            sb_MarkdownBuilder.AppendLine();
            sb_MarkdownBuilder.AppendLine("↩︎ [Back to top](#top)");
            // Returns the final Markdown string, removing any leading or trailing whitespace.
            return sb_MarkdownBuilder.ToString().Trim();
        }

    }
}
