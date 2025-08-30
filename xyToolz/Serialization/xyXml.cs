using System.Runtime.Serialization;
using System.Xml.Serialization;
using xyToolz.Helper.Logging;


namespace xyToolz.Serialization
{
    /// <summary>
    /// Helper class to (de)serialize objects from and to XML
    /// </summary>
    public static class xyXml
    {
        /// <summary>
        /// Deserialize the target from xml and print it in the console if needed
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <param name="outputTargetInConsole"></param>
        /// <returns></returns>
        public static T FromXml<T>(string xml, bool? outputTargetInConsole = false)
        {
            try
            {
                XmlSerializer deserializer = new(typeof(T));
                using StringReader reader = new StringReader(xml);
                if (deserializer.Deserialize(reader) is T target)
                {
                    xyLog.Log($"{target} has been deserialized!");
                    if (outputTargetInConsole is true)
                    {
                        xyLog.Log(xml);
                    }
                    return target;
                }
                else
                {
                    xyLog.Log($"An error occured while trying to deserialize {nameof(xml)}");
                    throw new SerializationException();
                }
            }
            catch(Exception ex)
            {
                xyLog.ExLog(ex);
                return default!;
            }
        }

        /// <summary>
        /// Deserialize the target from xml 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static T FromXml<T>(string xml) => (T)new XmlSerializer(typeof(T)).Deserialize(new StringReader(xml?? "Error:    "))!;
        
        /// <summary>
        /// Serialize the target into a xml string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string ToXML<T>(T target) 
        {
            try
            {
                using (StringWriter stringWriter = new()) 
                {
                    
                    XmlSerializer xmlSerializer = new(typeof(T));
                    xmlSerializer.Serialize(stringWriter, target);
                    return stringWriter.ToString();
                } ;
            }
            catch(Exception ex)
            {
                xyLog.ExLog(ex);
            }
            string msg = $"An Error occured while trying to serialize {target}";
            xyLog.Log(msg);
            return msg;
        }
    }



}
