# class xyDefaultLogFormatter

Namespace: `xyToolz.Logging.Helper.Formatters`  
Visibility: `public`  
Base/Interfaces:`IMessageFormatter, IExceptionFormatter`  
Source: `xyToolz\Logging\Helper\Formatters\xyDefaultLogFormatter.cs`

## Description:

/// Provides default formatting for log messages and exception details.
  ///

## Methoden

- `string FormatExceptionDetails(Exception ex, LogLevel level, string? message = null,string? callerName = null)` — `public`
  
  /// Formats detailed information about an exception into a string for logging purposes.
        ///
- `string FormatMessageForLogging(string message, string? callerName = null, LogLevel? level = null)` — `public`
  
  /// Formats a log message with a timestamp, log level, and caller information.
    ///

