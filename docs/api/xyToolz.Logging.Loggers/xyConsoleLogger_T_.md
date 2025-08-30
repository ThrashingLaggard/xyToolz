# class xyConsoleLogger<T>

Namespace: `xyToolz.Logging.Loggers`  
Visibility: `public`  
Attribute: `System.Diagnostics.CodeAnalysis.SuppressMessage`  
Base/Interfaces:`ILogging`  
Source: `xyToolz\Logging\Loggers\xyConsoleLogger.cs`

## Description:

/// Log formatted MESSAGES and EXCEPTIONS to the console    (currently discards LogEntries)
    ///

## Constructors

- `xyConsoleLogger( IMessageEntityFormatter<T> entFormatter_)` — `public`
  
  /// 
            ///
- `xyConsoleLogger( IMessageFormatter msgFormatter_ =null!, IExceptionFormatter excFormatter_ = null!, IMessageEntityFormatter<T> logEntryFormatter_ = null!, IExceptionEntityFormatter? exceptionEntryFormatter_ = null)` — `public`
  
  /// 
       ///
- `xyConsoleLogger(IExceptionFormatter excFormatter_)` — `public`
  
  /// 
            ///
- `xyConsoleLogger(IMessageFormatter msgFormatter_)` — `public`
  
  /// 
            ///

## Methods

- `string FormatEx(Exception ex, LogLevel level, out xyExceptionEntry excEntry, string? information = null,string? callerName = null)` — `private`
  
  /// Formats the Exception´s details for consistent logging.
        ///
- `string FormatFromEntity(T entry_, string? callerName = null, LogLevel? level = null)` — `public`
  
  /// Format a log entity into a message
        ///
- `string FormatMsg(string message, out xyDefaultLogEntry logEntry,DateTime? timestamp = null,uint? id = null,string? description = null, string? comment = null, string? callerName = null, LogLevel? level = LogLevel.Debug)` — `private`
  
  /// Formats a log message with optional caller information and log level.
        ///
- `void ExLog(Exception ex, LogLevel level, string? message = null, [CallerMemberName] string? callerName = null)` — `public`
  
  /// Logs the details of an exception at the specified log level.
        ///
- `void Log(string message, LogLevel level, [CallerMemberName] string? callerName = null)` — `public`
  
  /// Logs a formatted message with the specified log level and optional caller information.
        ///
- `void Shutdown()` — `public`
  
  (No XML‑Summary )
- `xyDefaultLogEntry FormatIntoDefaultLogEntry(string source, LogLevel level, string message, DateTime timestamp, uint? id = null, string? description = null, string? comment = null, Exception? exception = null)` — `public`
  
  /// Pack all relevant information into a xyDefaultLogEntry-instance
        ///
- `xyExceptionEntry FormatIntoExceptionEntry(Exception exception, string? information = null)` — `public`
  
  ///  Pack an Exception into an ExceptionEntry for easier serialization and storage
        ///

## Fields

- `IExceptionEntityFormatter? _excEntryFormatter` — `private readonly`
  
  (No XML‑Summary )
- `IExceptionFormatter? _excFormatter` — `private readonly`
  
  (No XML‑Summary )
- `IMessageEntityFormatter<T>? _logEntryFormatter` — `private readonly`
  
  (No XML‑Summary )
- `IMessageFormatter? _msgFormatter` — `private readonly`
  
  (No XML‑Summary )

