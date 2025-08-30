# interface ILogging

Namespace: `xyToolz.Logging.Interfaces`  
Visibility: `public`  
Source: `xyToolz\Logging\Interfaces\ILogging.cs`

## Description:

/// Interface for my own loggers
    ///

## Methods

- `void ExLog(Exception ex, LogLevel level, string? message = null, [CallerMemberName] string? callerName = null)`
  
  /// Write an exception
        ///
- `void Log(string message, LogLevel level, [CallerMemberName] string? callerName = null)`
  
  /// Write a message 
        ///
- `void Shutdown()`
  
  /// Set reference to null
        ///

