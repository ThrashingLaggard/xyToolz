namespace xyToolz.Serialization
{
    /// <summary>
    /// Facade for XML serialization and deserialization.
    /// 
    /// 
    /// - Convert XML strings to objects
    /// - Convert objects to XML strings
    /// 
    /// Pure pass-through.
    /// No logic.
    /// No state.
    /// </summary>
    public static class XmlHelper
    {
        public static T FromXml<T>(string xml)
            => xyXml.FromXml<T>(xml);

        public static T FromXml<T>(string xml, bool? outputTargetInConsole)
            => xyXml.FromXml<T>(xml, outputTargetInConsole);

        public static string ToXml<T>(T target)
            => xyXml.ToXML(target);
    }
}
