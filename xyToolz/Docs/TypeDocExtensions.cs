namespace xyToolz.Docs
{
    /// <summary>
    /// Helper methods for the TypeDoc record
    /// </summary>
    public static class TypeDocExtensions
    {
        /// <summary> Add useful information here </summary>
        public static string Description { get; set; }

        private static readonly Dictionary<TypeDoc, List<TypeDoc>> NestedMapping = [];

        /// <summary>
        /// Returns all nested types stored in the mapping.
        /// The name is so stupid, because i dont want the conflict with the property again
        /// </summary>
        /// <param name="td_CallingType_"> the TypeDoc instance calling the method</param>
        /// <returns></returns>
        public static List<TypeDoc> NestedInnerTypes(this TypeDoc td_CallingType_) => td_CallingType_.NestedTypes;


        /// <summary>
        /// Canonical anchor-key used both when building anchor maps and when looking them up.
        /// Keep this formatting consistent across the codebase.
        /// Examples:
        ///   Namespace = "", Name = "Program"     -> "Global (Default).Program"
        ///   Namespace = "A.B", DisplayName="Outer.Inner" -> "A.B.Outer.Inner"
        /// </summary>
        public static string GetAnchorKey(this TypeDoc t)
        {
            var ns = string.IsNullOrWhiteSpace(t.Namespace) ? "Global (Default)" : t.Namespace;
            // Prefer DisplayName if it already includes containing types (e.g., "Outer.Inner")
            var name = string.IsNullOrWhiteSpace(t.DisplayName) ? t.Name : t.DisplayName;
            return $"{ns}.{name}";
        }


        /// <summary>
        /// Recursively yields this type + all nested types
        /// </summary>
        /// <param name="td_CallingType_"></param>
        /// <returns></returns>
        public static IEnumerable<TypeDoc> FlattenNested(this TypeDoc td_CallingType_)
        {
            // Add the caller to the output
            yield return td_CallingType_;

            // For every nested type 
            foreach (TypeDoc td_NestedType in td_CallingType_.NestedTypes)
            {
                // For every subtype
                foreach (TypeDoc td_SubType in td_NestedType.FlattenNested())
                {
                    // Add the subtype to the output
                    yield return td_SubType;
                }
            }
        }



        /// <summary>
        /// Get all members (fields, properties, methods, events) including nested types recursively
        /// </summary>
        /// <param name="td_CallingType_"></param>
        /// <returns></returns>
        public static IEnumerable<MemberDoc> AllMembers(this TypeDoc td_CallingType_)
        {
            //foreach (MemberDoc md_Field in td_CallingType_.Fields)
            //{
            //    yield return md_Field;
            //}
            //foreach (MemberDoc md_Property in td_CallingType_.Properties) 
            //{
            //    yield return md_Property;
            //}
            //foreach (MemberDoc md_Method in td_CallingType_.Methods) 
            //{
            //    yield return md_Method;
            //}
            //foreach (MemberDoc md_Constructor in td_CallingType_.Constructors)
            //{
            //    yield return md_Constructor;
            //}
            //foreach (MemberDoc md_Event in td_CallingType_.Events) 
            //{
            //    yield return md_Event;
            //}

            // Combine all direct members using LINQ Concat, then yield them all.
            // Great for as long as i dont need specific changes 
            foreach (var member in td_CallingType_.Fields
                .Concat(td_CallingType_.Properties)
                .Concat(td_CallingType_.Methods)
                .Concat(td_CallingType_.Constructors)
                .Concat(td_CallingType_.Events))
            {
                yield return member;
            }

            foreach (TypeDoc td_NestedType in td_CallingType_.NestedInnerTypes())
            {
                foreach (MemberDoc md_NestedMember in td_NestedType.AllMembers())
                {
                    yield return md_NestedMember;
                }
            }
        }

        /// <summary>
        /// Add a member to the correct list in the calling TypeDoc
        /// </summary>
        /// <param name="td_CallingType_"></param>
        /// <param name="md_Member_"></param>
        public static void AddMember(this TypeDoc td_CallingType_, MemberDoc md_Member_)
        {
            switch (md_Member_.Kind)
            {
                case "ctor": td_CallingType_.Constructors.Add(md_Member_); break;
                case "method": td_CallingType_.Methods.Add(md_Member_); break;
                case "property": td_CallingType_.Properties.Add(md_Member_); break;
                case "event": td_CallingType_.Events.Add(md_Member_); break;
                case "field":
                case "enum-member":
                    td_CallingType_.Fields.Add(md_Member_);
                    break;
            }
        }

        /// <summary>
        /// Flattening all the nested types for top-level listing
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public static IEnumerable<TypeDoc> FlattenTypes(List<TypeDoc> types) => types.SelectMany(t => t.FlattenNested());



    }
}
