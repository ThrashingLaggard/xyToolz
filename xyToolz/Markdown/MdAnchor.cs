using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using xyToolz.Docs;

namespace xyToolz.Markdown
{
    public class MdAnchor
    {





        /// <summary>
        /// Creates the unique name (e.g., Namespace.Parent.Name) for anchoring and mapping.
        /// </summary>
        /// <param name="td_TargetType"></param>
        /// <returns></returns>
        public static string GetUniqueTypeName(TypeDoc td_TargetType) => !string.IsNullOrWhiteSpace(td_TargetType.Namespace) ? $"{td_TargetType.Namespace}.{td_TargetType.DisplayName}" : $"Global (Default).{td_TargetType.DisplayName}";


        /// <summary>
        /// Cleans the type's name and changes it to lowercase 
        /// </summary>
        /// <param name="name_"></param>
        /// <returns></returns>
        //private static string GetAnchorIDFromTypeName(string uniqueName_) => new string(uniqueName_.ToLowerInvariant().Replace(" ", "-").Replace('.', '-').Replace('<', '-').Replace('>', '-').Replace(',', '-').Replace("(", "-").Replace(")", "-").Where(ch => char.IsLetterOrDigit(ch) || ch == '-' || ch == '_').ToArray());
        public static string GetAnchorIDFromTypeName(string name_)
        {
            string clean = Regex.Replace(name_.ToLowerInvariant(), @"[^a-z0-9\-]+", "-");
            int hash = name_.GetHashCode();
            return $"{clean}-{Math.Abs(hash % 10000)}";
        }





        /// <summary>
        /// Builds the central map of all TypeDocs (root and nested) using the FlattenNested extension.
        /// The map stores the Unique Name (key) and the Anchor ID (value).
        /// </summary>
        /// <param name="td_RootType_"></param>
        /// <returns></returns>
        public static Dictionary<string, string> BuildAnchorMap(TypeDoc td_RootType_)
        {
            var map = new Dictionary<string, string>(StringComparer.Ordinal);

            // Use the existing extension method to traverse all types.
            foreach (var td in td_RootType_.FlattenNested())
            {
                // Create the unique name: Namespace.DisplayName 
                // This guarantees the key is unique across the entire document.
                string uniqueName = GetUniqueTypeName(td);

                map[uniqueName] = GetAnchorIDFromTypeName(uniqueName);
            }
            return map;
        }


        public static Dictionary<string, string> BuildAnchorAliasMap(Dictionary<string, string> anchorMap)
        {
            // Result: maps several textual aliases → same anchor id
            var aliases = new Dictionary<string, string>(StringComparer.Ordinal);
            foreach (var kv in anchorMap)
            {
                string uniqueKey = kv.Key;     // e.g., "Global (Default).Program" or "Demo.Outer.Inner"
                string anchorId = kv.Value;

                // 1) Full unique key (bereits in anchorMap)
                if (!aliases.ContainsKey(uniqueKey)) aliases[uniqueKey] = anchorId;

                // 2) DisplayName = last segment(s) after namespace, we can infer it by splitting on first '.' from left of namespace part.
                //    In deinem GetUniqueTypeName setzt du key = "<NS>.<DisplayName>" oder "Global (Default).<DisplayName>".
                //    Also ist DisplayName einfach der Teil nach dem ersten '.'.
                var dotIndex = uniqueKey.IndexOf('.');
                if (dotIndex >= 0 && dotIndex + 1 < uniqueKey.Length)
                {
                    string displayName = uniqueKey.Substring(dotIndex + 1); // e.g., "Program" or "Outer.Inner"
                    if (!aliases.ContainsKey(displayName)) aliases[displayName] = anchorId;

                    // 3) Simple name (letztes Segment von DisplayName)
                    var lastDot = displayName.LastIndexOf('.');
                    string simple = lastDot >= 0 ? displayName.Substring(lastDot + 1) : displayName; // e.g., "Inner" from "Outer.Inner"
                    if (!aliases.ContainsKey(simple)) aliases[simple] = anchorId;

                    // 4) Generic short form (strip type parameters), e.g., "List<T>" -> "List"
                    string genericShort = StripGenericArity(displayName);
                    if (genericShort != displayName && !aliases.ContainsKey(genericShort))
                        aliases[genericShort] = anchorId;
                }
            }
            return aliases;
        }

        /// <summary>
        ///  Strips generic type parameters like "T,U" (simple textual approach)
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string StripGenericArity(string name)
        {
            int lt = name.IndexOf('<');
            return lt > 0 ? name.Substring(0, lt) : name;
        }
    }
}
