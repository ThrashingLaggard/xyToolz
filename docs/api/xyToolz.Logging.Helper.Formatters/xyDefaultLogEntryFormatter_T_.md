# class xyDefaultLogEntryFormatter<T>

Namespace: `xyToolz.Logging.Helper.Formatters`  
Visibility: `public`  
Base/Interfaces:`IMessageEntityFormatter<T>`  
Source: `xyToolz\Logging\Helper\Formatters\xyDefaultLogEntryFormatter.cs`

## Description:

/// Used to store log messages and exceptions in LogEntries or get the data out of them
    ///

## Methoden

- `string UnpackAndFormatFromEntity(T entry_, string? callerName = null, LogLevel? level_ = LogLevel.Debug)` — `public`
  
  /// Unpack the data from a LogEntry
        ///
- `xyDefaultLogEntry PackAndFormatIntoEntity(string source, LogLevel level, string message, DateTime timestamp, uint? id = null, string? description = null, string? comment= null, Exception? exception = null)` — `public`
  
  /// Pack the data from logmessages into objects
        ///

