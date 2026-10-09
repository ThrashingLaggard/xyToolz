using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using xyLogger.Loggers;


namespace xyToolz.Serialization
{
    /// <summary>
    /// Helper class to (de)serialize objects from and to XML
    /// </summary>
    public static class xyXml
    {
        /// <summary>
        /// Deserialize the target from XML and print it in the console if needed
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <param name="outputTargetInConsole"></param>
        /// <returns></returns>
        public static T FromXml<T>(string xml, bool? outputTargetInConsole = false)
        {
            try
            {
                using StringReader reader = new(xml);
                XmlSerializer deserializer = new(typeof(T));
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
        /// Deserialize the target from XML 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static T FromXml<T>(string xml) => (T)new XmlSerializer(typeof(T)).Deserialize(new StringReader(xml?? "Error:    "))!;
        
        /// <summary>
        /// Serialize the target into an XML string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string ToXml<T>(T target) 
        {
            try
            {
                using StringWriter stringWriter = new();
                XmlSerializer xmlSerializer = new(typeof(T));
                xmlSerializer.Serialize(stringWriter, target);
                return stringWriter.ToString();
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
