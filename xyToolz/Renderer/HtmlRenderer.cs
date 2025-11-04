using System.Collections.Generic;
using System.Text;

using xyToolz.Docs;

namespace xyToolz.Renderer
{
#nullable enable
    /// <summary>
    /// Renders TypeDoc objects as HTML pages with optional CSS styling.
    /// </summary>
    public static class HtmlRenderer
    {
        /// <summary>
        /// Add useful infos here
        /// </summary>
        public static string? Description { get; set; }


        /// <summary>
        /// Generates a full HTML page for a TypeDoc object.
        /// </summary>
        /// <param name="type">The TypeDoc object to render.</param>
        /// <param name="cssPath">Optional path to a CSS file to include in the page.</param>
        /// <param name="isNested">Indicates if the type is nested (used for CSS styling)</param>
        /// <returns>HTML string</returns>
        public static string Render(TypeDoc type, string? cssPath = null, bool isNested = false)
        {
            var sb = new StringBuilder();

            // Top-level page
            if (!isNested)
            {
                sb.AppendLine("<!DOCTYPE html>");
                sb.AppendLine("<html lang=\"en\">");
                sb.AppendLine("<head>");
                sb.AppendLine("  <meta charset=\"UTF-8\">");
                sb.AppendLine($"  <title>{type.DisplayName}</title>");
                if (!string.IsNullOrWhiteSpace(cssPath))
                    sb.AppendLine($"  <link rel=\"stylesheet\" href=\"{cssPath}\">");
                sb.AppendLine("</head>");
                sb.AppendLine("<body>");
            }

            sb.AppendLine(isNested ? "<div class=\"nested\">" : "");

            sb.AppendLine($"  <h1>{type.Kind} {type.DisplayName}</h1>");
            sb.AppendLine($"  <p><strong>Namespace:</strong> {type.Namespace}</p>");
            sb.AppendLine($"  <p><strong>Visibility:</strong> {type.Modifiers}</p>");

            if (type.Attributes.Count > 0)
                sb.AppendLine($"  <p><strong>Attributes:</strong> {string.Join(", ", type.Attributes)}</p>");

            if (type.BaseTypes.Count > 0)
                sb.AppendLine($"  <p><strong>Base/Interfaces:</strong> {string.Join(", ", type.BaseTypes)}</p>");

            sb.AppendLine($"  <p><strong>Source:</strong> {type.FilePath}</p>");
            sb.AppendLine("  <h2>Description</h2>");
            sb.AppendLine($"  <p>{type.Summary}</p>");

            // Render members
            void RenderMembers(string title, List<MemberDoc> members)
            {
                if (members.Count == 0) return;

                sb.AppendLine($"  <h3>{title}</h3>");
                sb.AppendLine("  <ul>");

                foreach (var m in members)
                {
                    sb.AppendLine($"    <li><strong>{m.Kind}:</strong> <code>{m.Signature}</code><br>{m.Summary}</li>");
                }
                sb.AppendLine("  </ul>");
            }

            RenderMembers("Constructors", type.Constructors);
            RenderMembers("Methods", type.Methods);
            RenderMembers("Properties", type.Properties);
            RenderMembers("Events", type.Events);
            RenderMembers("Fields", type.Fields);

            // --- Nested Types ---
            var nestedTypes = type.NestedInnerTypes(); // <- als Methode aufrufen!
            if (nestedTypes != null)
            {
                foreach (var nt in nestedTypes)
                {
                    sb.AppendLine(Render(nt, cssPath, true));
                }
            }

            sb.AppendLine(isNested ? "</div>" : "");

            if (!isNested)
            {
                sb.AppendLine("</body>");
                sb.AppendLine("</html>");
            }

            return sb.ToString();
        }
    }
}
