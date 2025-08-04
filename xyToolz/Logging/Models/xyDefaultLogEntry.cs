using Microsoft.Extensions.Logging;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Xml.Serialization;


namespace xyToolz.Logging.Models
{
    /// <summary>
    /// Bundled information for a log message
    /// </summary>
    public class xyDefaultLogEntry(string source_, LogLevel level_, string message_, DateTime timestamp_, Exception? exception_ = null) : ISerializable
    {
        /// <summary>
        /// For easy administration
        /// </summary>
        public uint ID { get; set; }

        /// <summary>
        /// Add interesting information
        /// </summary>
        public string Description { get; set; } = default!;

        /// <summary>
        /// Additional information
        /// </summary>
        public string Comment { get; set; } = default!;

        /// <summary>
        /// Time of logging
        /// </summary>
        public required DateTime Timestamp { get; init; } = timestamp_;

        /// <summary>
        /// Where it was logged --> callername
        /// </summary>
        public required string Source { get; init; } = source_;

        /// <summary>
        /// The level of "severity"
        /// </summary>
        public LogLevel Level { get; set; } = level_;

        /// <summary>
        /// The logging message
        /// </summary>
        public required string Message { get; init; } = message_;

        /// <summary>
        /// The exception connected to the log
        /// </summary>
        public Exception? Exception { get; set; } = exception_ ?? default!;

        public xyExceptionEntry ExceptionEntry { get; set; } = exception_ is not null? (new xyExceptionEntry(exception_: exception_ ) { Exception = exception_}) : default!;



        /// <summary>
        /// Serialize per System.Text.Json
        /// </summary>
        /// <returns></returns>
        public string ToJson() => JsonSerializer.Serialize(this);

        /// <summary>
        /// Deserialize a json string into an instance of xyLogEntry
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public xyDefaultLogEntry FromJson(string json) 
        {
            if( JsonSerializer.Deserialize<xyDefaultLogEntry>(json) is xyDefaultLogEntry entry)
            {
                return entry;
            }
            else return new xyDefaultLogEntry("", LogLevel.Error, "", DateTime.Now)
            {
                Source = "xyLogEntry.FromJson()",
                Message = "Deserialization from JSON failed!",
                Timestamp = DateTime.Now
            };
        }
       
       
        /// <summary>
        /// Serialize per System.Xml.Serialization
        /// </summary>
        /// <returns></returns>
        public string ToXml()
        {
            XmlSerializer serializer = new(typeof(xyDefaultLogEntry));
            using StringWriter writer = new();
            serializer.Serialize(writer, this);
            return writer.ToString();
        }

        /// <summary>
        /// Deserialize a xml string into an instance of xyLogEntry
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public xyDefaultLogEntry FromXml(string xml)
        {
            XmlSerializer deserializer = new(typeof(xyDefaultLogEntry));
            using StringReader reader = new(xml);
            if (deserializer.Deserialize(reader) is xyDefaultLogEntry entry) return entry;
            else return new xyDefaultLogEntry("", LogLevel.Error,"", DateTime.Now) 
            { 
                Source = "xyLogEntry.FromXml()",
                Message = "Deserialization from xml failed!",
                Timestamp = DateTime.Now, 
            } ;
        }


        /// <summary>
        /// Method from ISerializable
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            // Überprüfen, ob die info-Instanz null ist
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

            // Fügen Sie die Eigenschaften zur SerializationInfo hinzu
            info.AddValue(nameof(ID), ID);
            info.AddValue(nameof(Description), Description);
            info.AddValue(nameof(Comment), Comment);
            info.AddValue(nameof(Timestamp), Timestamp);
            info.AddValue(nameof(Source), Source);
            info.AddValue(nameof(Level), Level);
            info.AddValue(nameof(Message), Message);
            info.AddValue(nameof(Exception), Exception?.ToString()); // Optional: Sie können auch die Exception-Details speichern
        }


        /// <summary>
        /// Get relevant information for the streaming context
        /// </summary>
        /// <param name="context"></param>
        public void ReadAllStreamingContextInfo(StreamingContext context)
        {
            // Auslesen des State
#pragma warning disable SYSLIB0050 // Typ oder Element ist veraltet
            Console.WriteLine($"StreamingContext State: {context.State}");
#pragma warning restore SYSLIB0050 // Typ oder Element ist veraltet

            // Überprüfen, ob der Context zusätzliche Informationen enthält
            if (context.Context != null)
            {
                Console.WriteLine($"Additional Context Information Type: {context.Context.GetType()}");
                Console.WriteLine($"Additional Context Information: {context.Context}");

                // Wenn der Context ein anderes Objekt ist, können Sie die Eigenschaften dynamisch auslesen
                var properties = context.Context.GetType().GetProperties();
                foreach (var property in properties)
                {
                    var value = property.GetValue(context.Context);
                    Console.WriteLine($"{property.Name}: {value}");
                }
                //// Beispiel für benutzerdefinierte Informationen
                //if (context.Context is xyContext customContext)
                //{
                //    Console.WriteLine($"UserId: {customContext.UserId}");
                //    Console.WriteLine($"SessionId: {customContext.SessionId}");
                //}
                //else
                //{
                //}
            }
            else
            {
                Console.WriteLine("No additional context information available.");
            }
        }

    }
}
