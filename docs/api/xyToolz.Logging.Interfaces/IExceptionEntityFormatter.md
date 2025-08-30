# interface IExceptionEntityFormatter

Namespace: `xyToolz.Logging.Interfaces`  
Visibility: `public`  
Source: `xyToolz\Logging\Interfaces\IExceptionEntityFormatter.cs`

## Description:

/// Interface for Exception-Formatters
    ///

## Methods

- `string UnpackAndFormatFromEntity<T, TKey, TValue>(T entry_, string? callerName = null, LogLevel? level = LogLevel.Debug)`
  
  /// Unpack information from entity
        ///
- `xyExceptionEntry PackAndFormatIntoEntity(Exception exception, DateTime? timestamp = null, string? message = null, uint? id = null, string? description = null)`
  
  /// Pack information into an entity
        ///

