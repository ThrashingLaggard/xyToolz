namespace xyToolz.Serialization
{
    /// <summary>
    /// Little helper for JSON structure and formatting.
    /// Facade over xyJson – format and structure related operations.
    /// </summary>
    public static class JsonFormat
    {
        /// <summary>
        /// Ensures the JSON file has a valid root object.
        /// Adds braces if missing.
        /// </summary>
        public static Task EnsureRootAsync(string filePath)
            => xyJson.EnsureJsonRootTag(filePath);

        /// <summary>
        /// Returns first line, last line and full content of a JSON file.
        /// Useful for diagnostics and validation.
        /// </summary>
        public static Task<(string First, string Last, string Full)> InspectAsync(string filePath)
            => xyJson.GetFirstAndLastLinesAsync(filePath);
    }
}
