# class xyAsyncLogger<T>

Namespace: `xyToolz.Logging.Loggers`  
Visibility: `public`  
Attribute: `System.Diagnostics.CodeAnalysis.SuppressMessage`  
Base/Interfaces:`ILogging, IDisposable`  
Source: `xyToolz\Logging\Loggers\xyAsyncLogger.cs`

## Description:

/// 
    ///

## Constructors

- `xyAsyncLogger(string? filepath= null,IMessageFormatter? messageFormatter_ =null, IExceptionFormatter? exceptionFormatter_ = null, IMessageEntityFormatter<T>? messageEntryFormatter_ = null, IExceptionEntityFormatter? exceptionEntryFormatter_ = null)` — `public`
  
  (No XML‑Summary )

## Properties

- `IExceptionEntityFormatter? ExceptionEntryFormatter{ get; set; }` — `public`
  
  (No XML‑Summary )
- `IExceptionFormatter? ExceptionFormatter{ get; set; }` — `public`
  
  (No XML‑Summary )
- `IMessageEntityFormatter<T>? MessageEntryFormatter{ get; set; }` — `public`
  
  (No XML‑Summary )
- `IMessageFormatter? MessageFormatter{ get; set; }` — `public`
  
  (No XML‑Summary )

## Methods

- `string FormatEx(Exception ex, LogLevel level, out xyExceptionEntry excEntry, string? information = null, string? callerName = null)` — `private`
  
  /// Formats the Exception´s details for consistent logging.
        ///
- `string FormatFromEntity(T entry_, string? callerName = null, LogLevel? level = null)` — `public`
  
  /// Format a log entity into a message
        ///
- `string FormatMsg(string message, out xyDefaultLogEntry logEntry, DateTime? timestamp = null, uint? id = null, string? description = null, string? comment = null, string? callerName = null, LogLevel? level = LogLevel.Debug)` — `private`
  
  /// Formats a log message with optional caller information and log level.
        ///
- `Task ProcessQueue()` — `private async`
  
  /// Work through the queued log messages
        ///
- `Task WriteToTarget(string message, params int[] logTargets)` — `private async`
  
  /// Write the output to the target =>   0 for console,    1 for file
        ///
- `void Dispose()` — `public`
  
  /// 
        ///
- `void Enqueue(string message, [CallerMemberName] string? callerName = null)` — `private`
  
  /// Add a message to the queue for logging
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

## Fields

- `BlockingCollection<string> _logQueue` — `private readonly`
  
  (No XML‑Summary )
- `CancellationTokenSource _cts` — `private readonly`
  
  (No XML‑Summary )
- `StreamWriter _writer` — `private readonly`
  
  (No XML‑Summary )
- `Task _worker` — `private readonly`
  
  (No XML‑Summary )

