using System.Text.Json;
using System.Text.Json.Serialization;
using xyToolz.Docs;

namespace xyToolz.Renderer
{
    /// <summary>
    /// Renders TypeDoc objects as JSON strings.
    /// </summary>
    public static class JsonRenderer 
    {
        private static readonly JsonSerializerOptions _options = new JsonSerializerOptions
        {
            WriteIndented = true, // Schönes Format!
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull // Null-Felder weglassen
        };

        /// <summary>
        /// Serializes a TypeDoc object to JSON.
        /// </summary>
        public static string Render(TypeDoc type)
        {
            return JsonSerializer.Serialize(type, _options);
        }
    }
}
