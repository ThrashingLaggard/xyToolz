# class xyLoggerManager

Namespace: `xyToolz.Logging.Helper`  
Visibility: `public`  
Source: `xyToolz\Logging\Helper\xyLoggerManager.cs`

## Description:

/// Manages a collection of loggers (ILogging) and provides methods for logging messages and exceptions.
    /// 
    ///                                                                                                                                                                                     Stuff for Eventhandlers is planned!
    ///

## Constructors

- `xyLoggerManager()` — `public`
  
  /// 
        ///

## Properties

- `string Description{ get; set; }` — `public`
  
  /// Add useful information
        ///
- `ushort Count{ get; private set; }` — `public`
  
  (No XML‑Summary )

## Methods

- `string OutputMissingLogger(ILogging logger)` — `private`
  
  (No XML‑Summary )
- `void ExLog(Exception ex, LogLevel level = LogLevel.Error)` — `public`
  
  /// Logs the specified exception at the given log level using all registered loggers.
        ///
- `void Log(string message, LogLevel level = LogLevel.Debug)` — `public`
  
  /// Logs a message with the specified log level to all registered loggers.
        ///
- `void RegisterLogger(params ILogging[] loggers)` — `public`
  
  /// Registers (a) new logger(s) to the logging system.
        ///
- `void UnregisterLogger(ILogging target)` — `public`
  
  /// Unregisters the specified logger from the logging system.
        ///

## Fields

- `IEnumerable<ILogging> _loggers` — `private readonly`
  
  (No XML‑Summary )

