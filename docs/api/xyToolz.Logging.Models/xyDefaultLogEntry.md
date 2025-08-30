# class xyDefaultLogEntry

Namespace: `xyToolz.Logging.Models`  
Visibility: `public`  
Base/Interfaces:`ISerializable`  
Source: `xyToolz\Logging\Models\xyDefaultLogEntry.cs`

## Description:

/// Bundled information for a log message
    ///

## Eigenschaften

- `DateTime Timestamp{ get; init; }` — `public required`
  
  /// Time of logging
        ///
- `Exception? Exception{ get; set; }` — `public`
  
  /// The exception connected to the log
        ///
- `LogLevel Level{ get; set; }` — `public`
  
  /// The level of "severity"
        ///
- `string Comment{ get; set; }` — `public`
  
  /// Additional information
        ///
- `string Description{ get; set; }` — `public`
  
  /// Add interesting information
        ///
- `string Message{ get; init; }` — `public required`
  
  /// The logging message
        ///
- `string Source{ get; init; }` — `public required`
  
  /// Where it was logged --> callername
        ///
- `uint ID{ get; set; }` — `public`
  
  /// For easy administration
        ///
- `xyExceptionEntry ExceptionEntry{ get; set; }` — `public`
  
  (No XML‑Summary )

## Methoden

- `string ToJson()` — `public`
  
  /// Serialize per System.Text.Json
        ///
- `string ToXml()` — `public`
  
  /// Serialize per System.Xml.Serialization
        ///
- `void GetObjectData(SerializationInfo info, StreamingContext context)` — `public`
  
  /// Method from ISerializable
        ///
- `void ReadAllStreamingContextInfo(StreamingContext context)` — `public`
  
  /// Get relevant information for the streaming context
        ///
- `xyDefaultLogEntry FromJson(string json)` — `public`
  
  /// Deserialize a json string into an instance of xyLogEntry
        ///
- `xyDefaultLogEntry FromXml(string xml, bool outputTargetInConsole = false)` — `public`
  
  /// Deserialize a xml string into an instance of xyLogEntry
        ///

