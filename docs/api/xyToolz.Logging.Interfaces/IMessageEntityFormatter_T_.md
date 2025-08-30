# interface IMessageEntityFormatter<T>

Namespace: `xyToolz.Logging.Interfaces`  
Visibility: `public`  
Source: `xyToolz\Logging\Interfaces\IMessageEntityFormatter.cs`

## Description:

/// Interface for Entity-Formatters
    ///

## Methods

- `string UnpackAndFormatFromEntity(T entry_, string? callerName = null, LogLevel? level = null)`
  
  /// Read information from entity
        ///
- `xyDefaultLogEntry PackAndFormatIntoEntity(string source, LogLevel level, string message, DateTime timestamp, uint? id = null, string? description = null, string? comment = null, Exception? exception = null)`
  
  /// Pack information into entity
        ///

