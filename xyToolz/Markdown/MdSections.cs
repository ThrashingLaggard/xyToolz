using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using xyToolz.Docs;
using xyToolz.Renderer;
using xyToolz.QOL;

namespace xyToolz.Markdown
{
#nullable enable
    public class MdSections
    {
        /// <summary>
        /// Renders the main header for the type, including its kind and display name.
        /// </summary>
        /// <param name="sb_MarkdownBuilder_">The StringBuilder to append to.</param>
        /// <param name="type">The TypeDoc object containing the data.</param>
        /// <param name="level_">The heading level for the Markdown output.</param>
        /// <param name="dic_AnchorMap_"></param>
        public static void RenderHeader(StringBuilder sb_MarkdownBuilder_, TypeDoc type, int level_, Dictionary<string, string> dic_AnchorMap_)
        {
            // Retrieve the anchor ID using the unique name as the key.
            string uniqueName = MdAnchor.GetUniqueTypeName(type);

            if (!dic_AnchorMap_.TryGetValue(uniqueName, out string? anchorID))
            {
                // Be forgiving: if the key is missing (edge-case), generate and cache deterministically
                anchorID = MdAnchor.GetAnchorIDFromTypeName(uniqueName);
                dic_AnchorMap_[uniqueName] = anchorID;
            }
            // Insert the `invisible` HTML anchor (the target for internal links)
            //sb_MarkdownBuilder_.AppendLine($"<a id=\"{anchorID}\"></a>");

            // Not more than 6!!!
            string headingPrefix = new('#', Math.Min(level_, 6));

            var visibility = string.IsNullOrWhiteSpace(type.Modifiers) ? "" : $" · `{type.Modifiers}`";
            // Put anchor as invisible <span> at end of the heading line
            sb_MarkdownBuilder_.AppendLine($"{headingPrefix} `{type.DisplayName}` **{type.Kind}**{visibility} <span id=\"{anchorID}\"></span>");
            sb_MarkdownBuilder_.AppendLine();
        }

        /// <summary>
        /// Renders the metadata section for the type.
        /// </summary>
        /// <param name="sb_MarkdownBuilder_">The StringBuilder to append to.</param>
        /// <param name="td_TargetType_">The TypeDoc object containing the data.</param>
        /// <param name="level_"></param>
        /// <param name="dic_AnchorMap_"></param>
        public static void RenderMetadata(StringBuilder sb_MarkdownBuilder_, TypeDoc td_TargetType_, int level_, Dictionary<string, string> dic_AnchorMap_)
        {
            sb_MarkdownBuilder_.AppendLine($"{xy.Repeat("#", (ushort)(level_ + 1))} Metadata");
            sb_MarkdownBuilder_.AppendLine($"**Namespace**: `{(string.IsNullOrWhiteSpace(td_TargetType_.Namespace) ? "Global (Default)" : td_TargetType_.Namespace)}`");

            if (!string.IsNullOrWhiteSpace(td_TargetType_.Modifiers))
            {
                sb_MarkdownBuilder_.AppendLine($"**Visibility:** `{td_TargetType_.Modifiers}`");
            }

            sb_MarkdownBuilder_.AppendLine($"**Source File:** `{td_TargetType_.FilePath}`");

            // Helper method simplifies adding lists of metadata, such as attributes or base types.
            AppendMetadataList(sb_MarkdownBuilder_, "Attributes", td_TargetType_.Attributes);
            AppendMetadataList(sb_MarkdownBuilder_, "Base Classes/Interfaces", td_TargetType_.BaseTypes);

            sb_MarkdownBuilder_.AppendLine();
        }

        /// <summary>
        /// Appends a list of metadata items to the StringBuilder only if the list is not empty.
        /// </summary>
        /// <param name="sb_MarkdownRenderer_">The StringBuilder to append to.</param>
        /// <param name="title_">The title for the list (e.g., "Attributes").</param>
        /// <param name="listedItems_">The list of strings to be appended.</param>
        public static void AppendMetadataList(StringBuilder sb_MarkdownRenderer_, string title_, IReadOnlyList<string> listedItems_)
        {
            // Checks if the list exists and contains any elements.
            if (listedItems_?.Count > 0)
            {
                // Formats each item as inline code for better readability in Markdown.
                IEnumerable<string> formattedItems = listedItems_.Select(item => $"`{item}`");

                sb_MarkdownRenderer_.AppendLine($"**{title_}:** {string.Join(", ", formattedItems)}");
            }
        }

        /// <summary>
        /// Renders the summary or description section for the type.
        /// </summary>
        /// <param name="sb_MarkdownBuilder_">The StringBuilder to append to.</param>
        /// <param name="td_Type_">The TypeDoc object containing the data.</param>
        /// <param name="level_"></param>
        /// <param name="dic_AnchorMap_">Not yet used</param>
        public static void RenderDescriptionFromXmlSummaryInTypeDoc(StringBuilder sb_MarkdownBuilder_, TypeDoc td_Type_, int level_, Dictionary<string, string> dic_AnchorMap_)
        {
            sb_MarkdownBuilder_.AppendLine($"{xy.Repeat("#", (ushort)(level_ + 1))} Description");
            // Appends the summary if it's not null or whitespace.
            sb_MarkdownBuilder_.AppendLine((!string.IsNullOrWhiteSpace(td_Type_.Summary) ? td_Type_.Summary.Trim() : "(No description available)"));

            sb_MarkdownBuilder_.AppendLine();
        }

        /// <summary>
        /// Renders all nested types contained within the current type.
        /// </summary>
        /// <param name="sb_MarkdownRenderer">The StringBuilder to append to.</param>
        /// <param name="td_TargetType_">The TypeDoc object containing the data.</param>
        /// <param name="level_">The heading level for the Markdown output.</param>
        /// <param name="dic_AnchorMap_"></param>
        public static void RenderNestedTypes(StringBuilder sb_MarkdownRenderer, TypeDoc td_TargetType_, int level_, Dictionary<string, string> dic_AnchorMap_)
        {
            // FlattenNested() returns the type itself and all nested types.
            // Skip(1) skips the root element so we only process the nested types.
            IEnumerable<TypeDoc> nestedTypes = td_TargetType_.FlattenNested().Skip(1);

            if (!nestedTypes.Any()) return;

            sb_MarkdownRenderer.AppendLine("---"); // A horizontal rule for better readability.

            foreach (TypeDoc td_nestedType in nestedTypes)
            {
                // Use the SAME anchor map for all nested levels to keep links stable
                sb_MarkdownRenderer.AppendLine(MarkdownRenderer.Render(td_nestedType, level_ + 1, dic_AnchorMap_));
                sb_MarkdownRenderer.AppendLine();
            }
        }



        /// <summary>
        /// Replaces any unique type names in a signature string with a corresponding internal Markdown link.
        /// </summary>
        /// <param name="signature_"></param>
        /// <param name="dic_AnchorIdMapping_"></param>
        /// <returns></returns>
        public static string FormatSignatureWithLinks(string signature_, Dictionary<string, string> dic_AnchorIdMapping_)
        {
            //string result = signature_;

            //// Ensuring longer names (fully qualified names/...) are replaced before shorter parts, preventing partial replacement errors.
            //IOrderedEnumerable<string> sortedTypeNames = dic_AnchorIdMapping_.Keys.OrderByDescending(keyFromDictionary => keyFromDictionary.Length);

            //foreach (string typeName in sortedTypeNames)
            //{
            //    if (result.Contains(typeName))
            //    {
            //        string anchorId = dic_AnchorIdMapping_[typeName];

            //        // Creating the Markdown link: [Visible Text](#Anchor-ID)
            //        string markdownLink = $"[{typeName}](#{anchorId})";

            //        // Replace the plain type name string with the link string.
            //        result = result.Replace(typeName, markdownLink);
            //    }
            //}

            //return result;


            // Build alias map once per call – if you link viele Signaturen in einem Rutsch,
            // kannst du das auch vorziehen/cachen.
            var aliasMap = MdAnchor.BuildAnchorAliasMap(dic_AnchorIdMapping_);

            // Sort longer keys first to avoid partial replacements
            var keys = aliasMap.Keys.OrderByDescending(k => k.Length).ToList();

            string result = signature_;
            foreach (var key in keys)
            {
                if (string.IsNullOrWhiteSpace(key)) continue;
                var anchorId = aliasMap[key];
                // Replace whole words / identifier boundaries: avoid "Program" inside "Programmer"
                // \b passt bei ., <, > etc. nicht immer; identifizierbar per custom boundaries:
                // left boundary: start or non-identifier; right boundary: end or non-identifier
                var pattern = $@"(?<![A-Za-z0-9_]){RegexEscape(key)}(?![A-Za-z0-9_])";
                result = Regex.Replace(result, pattern, $"[{key}](#{anchorId})");
            }
            return result;
        }

        private static string RegexEscape(string s) => Regex.Escape(s);

    }
}
