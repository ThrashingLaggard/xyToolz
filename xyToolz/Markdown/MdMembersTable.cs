using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xyToolz.Docs;

namespace xyToolz.Markdown
{
#nullable enable

    /// <summary>
    /// 
    /// </summary>
    public class MdMembersTable
    {

        /// <summary>
        /// Calls the rendering methods for all member types (constructors, properties, methods, etc.).
        /// </summary>
        /// <param name="sb_Markdownbuilder_">The StringBuilder to append to.</param>
        /// <param name="td_TargetObject_">The TypeDoc object containing the data.</param>
        /// <param name="level_"></param>
        /// <param name="dic_AnchorMap_"></param>
        public static void RenderAllMembers(StringBuilder sb_Markdownbuilder_, TypeDoc td_TargetObject_, int level_, Dictionary<string, string> dic_AnchorMap_)
        {
            // Pass the anchor map down to the table renderer
            RenderMembersAsTable(sb_Markdownbuilder_, "Constructors", td_TargetObject_.Constructors, level_, dic_AnchorMap_);
            RenderMembersAsTable(sb_Markdownbuilder_, "Properties", td_TargetObject_.Properties, level_, dic_AnchorMap_);
            RenderMembersAsTable(sb_Markdownbuilder_, "Methods", td_TargetObject_.Methods, level_, dic_AnchorMap_);
            RenderMembersAsTable(sb_Markdownbuilder_, "Events", td_TargetObject_.Events, level_, dic_AnchorMap_);
            RenderMembersAsTable(sb_Markdownbuilder_, "Fields", td_TargetObject_.Fields, level_, dic_AnchorMap_);
        }

        /// <summary>
        /// Renders a list of members (methods, properties, etc.) as a clear Markdown table.
        /// </summary>
        /// <param name="sb_MarkdownBuilder_">The StringBuilder to append to.</param>
        /// <param name="title_">The title for the member table (e.g., "Methods").</param>
        /// <param name="listedMembers_">The list of MemberDoc objects to be rendered.</param>
        /// <param name="level"></param>
        /// <param name="dic_AnchorMap_"></param>
        public static void RenderMembersAsTable(StringBuilder sb_MarkdownBuilder_, string title_, IReadOnlyList<MemberDoc> listedMembers_, int level, Dictionary<string, string> dic_AnchorMap_)
        {
       

            if (listedMembers_?.Any() != true) return;

            // Replace with actual member name when there is time
            //sb_MarkdownBuilder_.AppendLine($"{xy.Repeat("#", (ushort)(level + 1))} Title");
            sb_MarkdownBuilder_.AppendLine();
            sb_MarkdownBuilder_.AppendLine("| Signature |  Summary  |");
            sb_MarkdownBuilder_.AppendLine("|-----------|-----------|");
            foreach (var m in listedMembers_)
            {
                string summary = (m.Summary ?? string.Empty).Trim().Replace("\r\n", " ").Replace("\n", " ");
                if (string.IsNullOrWhiteSpace(summary)) summary = "—";
                string linkedSignature = MdSections.FormatSignatureWithLinks(m.Signature, dic_AnchorMap_);
                sb_MarkdownBuilder_.AppendLine($"| `{linkedSignature}` | {summary} |");
            }
            sb_MarkdownBuilder_.AppendLine();


        }


        /// <summary>
        /// Renders a simple, GitHub-friendly table of contents for the root + all nested types.
        /// </summary>
        public static void RenderTableOfContents(StringBuilder sb, TypeDoc root, Dictionary<string, string> anchorMap)
        {
            //sb.AppendLine("## Table of Contents");
            //sb.AppendLine();

            foreach (var t in root.FlattenNested())
            {
                // Derive the same unique key we use everywhere else
                string key = MdAnchor.GetUniqueTypeName(t);
                if (!anchorMap.TryGetValue(key, out string? anchor))
                {
                    anchor = MdAnchor.GetAnchorIDFromTypeName(key);
                    anchorMap[key] = anchor; // cache for consistent linking
                }
                // Indent nested entries with bullets for a visual hierarchy
                int depth = Math.Max(0, t.DisplayName.Count(c => c == '.')); // "Outer.Inner" → depth 1
                string bullet = new string(' ', depth * 2) + "-";             // 2 spaces per depth level
                sb.AppendLine($"{bullet} [{t.DisplayName}](#{anchor})");
            }
            sb.AppendLine();
        }


    }
}
