
namespace xyToolz.Docs
{
    /// <summary>
    /// Represents a single member of a type (field, property, method, constructor, event, enum-member)
    /// </summary>
    public record MemberDoc
    {
        /// <summary> Store usefull information in here </summary>
        public string Description { get; set; }
        /// <summary>A list of attributes applied to the member (e.g., "Obsolete", "Test")</summary>
        public IList<string> Attributes { get; init; } = new List<string>();

        /// <summary>Kind of member: "field", "property", "method", "ctor", "event", "enum-member"</summary>
        public string Kind { get; init; } = string.Empty;

        /// <summary>Signature of the member (name + parameters/type)</summary>
        public string Signature { get; init; } = string.Empty;

        /// <summary>Modifiers like "public", "private", "protected internal"</summary>
        public string Modifiers { get; init; } = string.Empty;

        /// <summary>Optional documentation/summary extracted from XML comments</summary>
        public string Summary { get; init; } = string.Empty;

        /// <summary>For methods, properties, and events: The return type or field type.</summary>
        public string ReturnType { get; init; } = string.Empty;

        /// <summary>Documentation extracted from the XML &lt;returns&gt; comment (for methods/properties).</summary>
        public string ReturnSummary { get; init; } = string.Empty;

        /// <summary>A list of detailed parameter documentation (for methods/constructors/delegates).</summary>
        public IList<ParameterDoc> Parameters { get; set; } = new List<ParameterDoc>();

        /// <summary>Documentation extracted from the XML &lt;remarks&gt; comment.</summary>
        public string Remarks { get; init; } = string.Empty;

        /// <summary>A list of constraints for generic type parameters (e.g., "where T : struct")</summary>
        public IList<string> GenericConstraints { get; init; } = new List<string>();

        /// <summary>Documentation extracted from the XML &lt;typeparam&gt; comments.</summary>
        public IDictionary<string, string> TypeParameterSummaries { get; init; } = new Dictionary<string, string>();
    }
}
