
namespace xyToolz.Docs
{
    #nullable enable

    /// <summary>
    /// Represents detailed information about a single parameter of a method or constructor.
    /// This expanded record provides structured information for robust documentation generation.
    /// </summary>
    public record ParameterDoc
    {
        /// <summary>Name of the parameter</summary>
        public string Name { get; init; } = string.Empty;

        /// <summary> Call .ToString() on the parameter and store the result here. NOTE: This field seems redundant with TypeDisplayName and Name for parameter docs. </summary>
        public string? Value { get; init; }

        /// <summary>The full, canonical type name of the parameter (ie., "System.String" or "System.Collections.Generic.ListSystem.Int32")</summary>
        public string TypeFullName { get; init; } = string.Empty;

        /// <summary>The display name as it appears in code (ie., "string" or "List(int)")</summary>
        public string TypeDisplayName { get; init; } = string.Empty;

        /// <summary>Indicates if the parameter has the "ref" modifier.</summary>
        public bool IsRef { get; init; } = false;

        /// <summary>Indicates if the parameter has the "ref readonly" modifier.</summary>
        public bool IsRefReadonly { get; init; } = false;

        /// <summary>Indicates if the parameter has the "out" modifier.</summary>
        public bool IsOut { get; init; } = false;

        /// <summary>Indicates if the parameter has the "in" modifier (read-only reference).</summary>
        public bool IsIn { get; init; } = false;

        /// <summary>Indicates if the parameter has the "params" modifier (parameter array).</summary>
        public bool IsParams { get; init; } = false;

        /// <summary>Indicates if the parameter is optional and has a default value.</summary>
        public bool IsOptional { get; init; } = false;

        /// <summary> The default value expression as a string, if the parameter is optional (e.g., "null", "10", "Color.Red").  Is null if no default value is present. </summary>
        public string? DefaultValueExpression { get; init; }

        /// <summary> Documentation extracted from the XML &lt;param name="Name"&gt; comment. </summary>
        public string Summary { get; init; } = string.Empty;

        /// <summary>Optional: The explicit type of the default value expression (useful for resolving ambiguity, e.g., "int" for DefaultValueExpression="0").</summary>
        public string? DefaultValueType { get; init; }

        /// <summary>Optional: Indicates if the parameter is a generic type parameter (e.g., the 'T' in a method signature like MyMethod&lt;T&gt;(T item)).</summary>
        public bool IsGenericTypeParam { get; init; } = false;


        /// <summary>
        /// A calculated, space-separated string of modifiers (e.g., "ref readonly in params").
        /// This property is calculated on read access from the boolean flags.
        /// </summary>
        public string ModifiersString => GetModifierString(this);


        /// <summary>
        /// Helper function to create a space-separated string of all active modifiers.
        /// </summary>
        /// <param name="pd_Parameter_">The parameter record to analyze.</param>
        /// <returns>A space-separated string of modifiers.</returns>
        private static string GetModifierString(ParameterDoc pd_Parameter_)
        {
            List<string> modifiers = [];

            // Note: Order should usually match the C# language specification.
            if (pd_Parameter_.IsParams)
            {
                modifiers.Add("params");
            }
            if (pd_Parameter_.IsRefReadonly)
            {
                modifiers.Add("ref readonly");
            }
            else if (pd_Parameter_.IsRef)
            {
                modifiers.Add("ref");
            }
            if (pd_Parameter_.IsOut)
            {
                modifiers.Add("out");
            }
            else if (pd_Parameter_.IsIn) // 'in' is typically used when 'ref readonly' is not available or desired.
            {
                modifiers.Add("in");
            }

            // Optional/Generic Type Param are typically not keywords in the signature itself, but can be added for documentation if needed.
            // If they are only documentation flags, they are best handled in the renderer.
            // if (pd_Parameter_.IsOptional) modifiers.Add("optional");
            // if (pd_Parameter_.IsGenericTypeParam) modifiers.Add("T");

            return string.Join(" ", modifiers);
        }
    }
}