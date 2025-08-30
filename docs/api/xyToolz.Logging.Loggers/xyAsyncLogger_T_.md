# class xyAsyncLogger<T>

Namespace: `xyToolz.Logging.Loggers`  
Visibility: `public`  
Attribute: `System.Diagnostics.CodeAnalysis.SuppressMessage`  
Base/Interfaces:`ILogging, IDisposable`  
Source: `xyToolz\Logging\Loggers\xyAsyncLogger.cs`

## Description:

/// 
    ///

## Konstruktoren

- `xyAsyncLogger(string? filepath= null,IMessageFormatter? messageFormatter_ =null, IExceptionFormatter? exceptionFormatter_ = null, IMessageEntityFormatter<T>? messageEntryFormatter_ = null, IExceptionEntityFormatter? exceptionEntryFormatter_ = null)` — `public`
  
  (No XML‑Summary )

## Eigenschaften

- `IExceptionEntityFormatter? ExceptionEntryFormatter{ get; set; }` — `public`
  
  (No XML‑Summary )
- `IExceptionFormatter? ExceptionFormatter{ get; set; }` — `public`
  
  (No XML‑Summary )
- `IMessageEntityFormatter<T>? MessageEntryFormatter{ get; set; }` — `public`
  
  (No XML‑Summary )
- `IMessageFormatter? MessageFormatter{ get; set; }` — `public`
  
  (No XML‑Summary )

## Methoden

- `string FormatFromEntity(T entry_, string? callerName = null, LogLevel? level = null)` — `public`
  
  /// Format a log entity into a message
        ///
- `void Dispose()` — `public`
  
  /// 
        ///
- `void ExLog(Exception ex, LogLevel level, string? message = null, [CallerMemberName] string? callerName = null)` — `public`
  
  /// Writes an exception
        ///
- `void Log(string message, LogLevel level, [CallerMemberName] string? callerName = null)` — `public`
  
  /// Writes an informative message
        ///
- `void Shutdown()` — `public`
  
  /// 
        ///
- `xyDefaultLogEntry FormatIntoDefaultLogEntry(string source, LogLevel level, string message, DateTime timestamp, uint? id = null, string? description = null, string? comment = null, Exception? exception = null)` — `public`
  
  /// Pack all relevant information into a xyDefaultLogEntry-instance
        ///
- `xyExceptionEntry FormatIntoExceptionEntry(Exception exception, string? information = null)` — `public`
  
  ///  Pack an Exception into an ExceptionEntry for easier serialization and storage
        ///

