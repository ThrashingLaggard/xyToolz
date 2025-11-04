using System.Text;
using xyToolz.Docs;
using xyToolz.Filesystem;

namespace xyToolz.Renderer
{
#nullable enable

    /// <summary>
    /// Renders a directory structure as tree text
    /// </summary>
    public static class FileTreeRenderer
    {
        /// <summary>
        /// Add custom infos
        /// </summary>
        public static string? Description { get; set; }

        /// <summary>
        /// Recursive method to render the tree structure for better representation of the project
        /// </summary>
        /// <param name="di_Directory_">Target folder</param>
        /// <param name="prefix_"> Prefix for this level of the tree</param>
        /// <param name="isLast_">Last folder in this directory?</param>
        /// <param name="sb_TreeBuilder_">Stores the tree</param>
        /// <param name="hs_ExcludeTheseParts_"></param>
        public static void RenderTree(DirectoryInfo di_Directory_, string prefix_, bool isLast_, StringBuilder sb_TreeBuilder_, HashSet<string> hs_ExcludeTheseParts_)
        {
            // Ignore unwanted folders
            if (hs_ExcludeTheseParts_.Contains(di_Directory_.Name))
            {
                return;
            }                

            //Read all not-to-be-ignored child directories
            DirectoryInfo[] di_SubDirectories = di_Directory_.GetDirectories().Where(d => !hs_ExcludeTheseParts_.Contains(d.Name)).OrderBy(d => d.Name).ToArray();

            // Read all not-to-be-ignored files from the target folder
            FileInfo[] fi_Files = di_Directory_.GetFiles().Where(f => !hs_ExcludeTheseParts_.Contains(f.Name)).OrderBy(f => f.Name).ToArray();

            // Build the current level of the tree
            sb_TreeBuilder_.AppendLine($"{prefix_}{(isLast_ ?"└─" : "├─")}{di_Directory_.Name}/");

            // For every subfolder: call this method on itself
            for (int i = 0; i < di_SubDirectories.Length; i++)
            {
                RenderTree(di_SubDirectories[i], prefix_ + (isLast_ ? "  " : "│ "), i == di_SubDirectories.Length - 1, sb_TreeBuilder_, hs_ExcludeTheseParts_);
            }

            // For every file: Build the current tree level
            for (int i = 0; i < fi_Files.Length; i++)
            {                                                                                                                                                                                                                                            //string fileName = fi_Files[i].Name; //sb_TreeBuilder_.AppendLine($"{prefix_}{(di_SubDirectories.Length + i == di_SubDirectories.Length + fi_Files.Length - 1 ? "└─" : "├─")}{fileName}");
                sb_TreeBuilder_.AppendLine($"{prefix_}{ChangePrefixIfIndexIsAtLastTreeLevel(di_SubDirectories,fi_Files,i)}{fi_Files[i].Name}");
            }
        }

        /// <summary>
        /// Checks if the index is at the lowest tree level and changes the prefix if so
        /// </summary>
        /// <param name="di_SubDirectories"></param>
        /// <param name="fi_Files"></param>
        /// <param name="i"></param>
        /// <returns>
        /// If Index is at last position: └─
        /// Else(Most of the time): ├─ 
        /// </returns>
        static string ChangePrefixIfIndexIsAtLastTreeLevel(DirectoryInfo[] di_SubDirectories, FileInfo[] fi_Files, int i) => !((di_SubDirectories.Length + i) == ((di_SubDirectories.Length + fi_Files.Length) -1)) ?"├─" : "└─";




        /// <summary>
        /// Builds PROJECT-STRUCTURE.md, a visual tree representation of the file system.
        /// </summary>
        /// <param name="treeBuilder"></param>
        /// <param name="format"></param>
        /// <param name="rootPath"></param>
        /// <param name="outPath"></param>
        /// <param name="excludedParts"></param>
        /// <param name="writeToDisk"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static async Task<StringBuilder> BuildProjectTree(StringBuilder treeBuilder, string format, string rootPath, string outPath, HashSet<string> excludedParts, bool writeToDisk, string prefix = "")
        {
            string headline = "# Project structure\n";
            string fileName = "PROJECT-STRUCTURE.md";

            // Adding the headline
            treeBuilder.AppendLine(headline);

            // Rendering PROJECT-STRUCTURE.md 
            FileTreeRenderer.RenderTree(new DirectoryInfo(rootPath), prefix, true, treeBuilder, excludedParts);

            if (writeToDisk)
            {
                string targetPath = Path.Combine(outPath, fileName);
                await xyFiles.SaveToFile(treeBuilder.ToString(), targetPath);
            }

            return treeBuilder;
        }

     

        /// <summary>
        /// Builds an INDEX.md file listing all documented types, grouped by namespace.
        /// </summary>
        public static async Task<StringBuilder> BuildProjectIndex(IEnumerable<TypeDoc> flattenedtypes, string format, string outpath, bool writeToDisk)
        {
            // Stores INDEX.md  ordered by namespace
            StringBuilder indexBuilder = new StringBuilder();

            // Adding the headline
            indexBuilder.AppendLine("# API‑Index (by namespace)\n");

            // Bringing everything into the right order
            IEnumerable<IGrouping<string, TypeDoc>> flattenedTypesGroupedAndInOrder = flattenedtypes.GroupBy(t => t.Namespace).OrderBy(g => g.Key);

            foreach (IGrouping<string, TypeDoc> group in flattenedTypesGroupedAndInOrder)
            {
                // Adding subheadline
                indexBuilder.AppendLine($"## `{group.Key}`");
                foreach (TypeDoc tD in group.OrderBy(t => t.DisplayName))
                {
                    // Choosing the file extension
                    string fileExt = format == "pdf" ? "pdf" : format == "html" ? "html" : format == "json" ? "json" : "md";

                    // Building the group data 
                    string rel = $"./{group.Key.Replace('<', '_').Replace('>', '_')}/{tD.DisplayName.Replace(' ', '_')}.{fileExt}";

                    // Appending data
                    indexBuilder.AppendLine($"- [{tD.DisplayName}]({rel})");
                }
                // Adding empty row
                indexBuilder.AppendLine();
            }

            // if (writeToDisk)
            {
                string indexPath = Path.Combine(outpath, "INDEX.md");
                await xyFiles.SaveToFile(indexBuilder.ToString(), indexPath);
            }
            return indexBuilder;
        }

        /// <summary>
        /// Builds both the namespace-based API index (INDEX.md) 
        /// and the project folder structure (PROJECT-STRUCTURE.md).
        /// </summary>
        /// <param name="flattenedTypes"></param>
        /// <param name="format"></param>
        /// <param name="rootPath"></param>
        /// <param name="outPath"></param>
        /// <param name="excludedParts"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static async Task BuildIndexAndTree(IEnumerable<TypeDoc> flattenedTypes, string format, string rootPath, string outPath, HashSet<string> excludedParts, string prefix = "")
        {
            StringBuilder indexBuilder = await BuildProjectIndex(flattenedTypes, format, outPath);
            indexBuilder.Clear();
            StringBuilder projectBuilder = await BuildProjectTree(indexBuilder, format, rootPath, outPath, excludedParts);
            indexBuilder = null!;
            projectBuilder = null!;
        }

  

        /// <summary>
        /// Build project index
        /// </summary>
        /// <param name="flattenedtypes"></param>
        /// <param name="format"></param>
        /// <param name="outpath"></param>
        /// <returns></returns>
        public static Task<StringBuilder> BuildProjectIndex(IEnumerable<TypeDoc> flattenedtypes, string format, string outpath)
            => BuildProjectIndex(flattenedtypes, format, outpath, writeToDisk: true);


     

        /// <summary>
        /// Build the project tree
        /// </summary>
        /// <param name="treeBuilder"></param>
        /// <param name="format"></param>
        /// <param name="rootPath"></param>
        /// <param name="outPath"></param>
        /// <param name="excludedParts"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static Task<StringBuilder> BuildProjectTree(StringBuilder treeBuilder, string format, string rootPath, string outPath, HashSet<string> excludedParts, string prefix = "")
            => BuildProjectTree(treeBuilder, format, rootPath, outPath, excludedParts, writeToDisk: true, prefix);

    }
}
