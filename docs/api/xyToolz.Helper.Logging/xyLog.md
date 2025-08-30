# class xyLog

Namespace: `xyToolz.Helper.Logging`  
Visibility: `public static`  
Attribute: `System.Diagnostics.CodeAnalysis.SuppressMessage`  
Source: `xyToolz\Logging\Static Logging Stuff\xyLog.cs`

## Description:

/// Provides centralized logging functionality for the application.
    /// Supports both synchronous and asynchronous logging for messages and exceptions.
    /// Formats output with metadata such as timestamps, caller name, and log level.
    ///
    /// 
Available Features:

    /// 
    /// Synchronous logging of messages and exceptions
    /// Asynchronous logging variants
    /// Event-based log handling
    /// Formatted exception output (text + JSON)
    /// Caller name tracking via CallerMemberName
    /// 
    ///
    /// 
Thread Safety:

    /// 
Thread-safe log file access using locking mechanism

    ///
    /// 
Limitations:

    /// 
No filtering, buffering, or custom log routing implemented

    ///
    /// 
Performance:

    /// 
Suitable for low-to-medium frequency logging, may block on sync operations

    ///
    /// 
Configuration:

    /// 
Log file paths and sizes are hardcoded

    ///
    /// 
Method Overview:

    /// 
    /// LogLog plain message synchronously
    /// AsxLogLog plain message asynchronously
    /// ExLogLog exception details synchronously
    /// AsxExLogLog exception details asynchronously
    /// JsonExLogLog exception as formatted JSON (sync)
    /// JsonAsxExLogLog exception as formatted JSON (async)
    /// 
    ///
    /// 
Example Usage:

    /// 
    /// try
    /// {
    ///     throw new InvalidOperationException("Test error");
    /// }
    /// catch (Exception ex)
    /// {
    ///     xyLog.ExLog(ex);
    /// }
    /// 
    ///

## Methods

- `bool WriteExLog(Exception ex, LogLevel level = LogLevel.Error, [CallerMemberName] string? callerName = null)` — `public static`
  
  /// Synchronous: Writes details for the given exception to into console and file
        ///
- `bool WriteLog(string message, [CallerMemberName] string? callerName = null)` — `public static`
  
  /// Synchronous: Writes a log message into file and console
        ///
- `IEnumerable<xyLogTargets> SetLogTargets(ushort[] logTargets)` — `internal static`
  
  /// Converts raw integer targets into the xyLogTargets enum.
        ///
- `string FormatEx(Exception ex, LogLevel level, string? message = null, string? callerName = null)` — `private static`
  
  /// Formats the Exception´s details for consistent logging.
        ///
- `string FormatMsg(string message, string? callerName = null, LogLevel? level = null)` — `public static`
  
  /// Format the given message for a detailed overview
        ///
- `string Log(string message, [CallerMemberName] string? callerName = null)` — `public static`
  
  /// Logs a simple text message synchronously.
        ///
- `T ExecuteDebugging<T>(string actionName, Func<T> action, Action<Exception>? logOnError = null)` — `public static`
  
  /// Execute a message and protocol the result
        /// 
        /// logger.ExecuteWithLogging(">Screaming()", () =>Screaming(testText),Exception => logger.ExLog(Exception,LogLevel.Warning,">Screaming()"));
        /// 
        ///
- `Task AsxExLog(Exception ex, LogLevel level = LogLevel.Error, [CallerMemberName] string? callerName = null)` — `public static async`
  
  /// Logs details of an exception asynchronously.
        ///
- `Task JsonAsxExLog(Exception ex)` — `public static async`
  
  /// Logs an exception as JSON formatted string (asynchronous).
        ///
- `Task<string> AsxLog(string message, [CallerMemberName] string? callerName = null)` — `public static async`
  
  /// Logs a simple text message asynchronously.
        ///
- `Task<T> ExecuteDebuggingAsync<T>(string actionName, Func<T> action, Action<Exception>? logOnError = null)` — `public static async`
  
  /// Execute "a function" with one main return value with logging BUT ASYNC
        ///
- `void CheckFileSizeAndMoveLogsToArchiveWhenTooBig()` — `private static`
  
  /// Checks the filesize for both logtypes and archives the logs if they get "too big"
        ///
- `void ExLog(Exception ex, LogLevel level = LogLevel.Error, string? message = null, [CallerMemberName] string? callerName = null)` — `public static`
  
  /// Logs details of an exception synchronously.
        ///
- `void JsonExLog(Exception ex)` — `public static`
  
  /// Logs an exception as JSON formatted string (synchronous).
        ///
- `void OnExMessageSent(string message)` — `public static`
  
  /// Helper method to manually trigger the ExLogMessageSent event.
        ///
- `void OnMessageSent(string message)` — `public static`
  
  /// Helper method to manually trigger the LogMessageSent event.
        ///
- `void Output(string formattedMessage, string? callerName)` — `private static`
  
  /// Centralized method for printing log messages to console and firing events.
        ///

## Events

- `event Action<string, string>? ExLogMessageSent` — `public static`
  
  /// Event triggered when an exception log message is sent.
        ///
- `event Action<string, string>? LogMessageSent` — `public static`
  
  /// Event triggered when a standard log message is sent.
        ///

## Fields

- `long _maxLogFileSize` — `private static readonly`
  
  (No XML‑Summary )
- `object _threadSafetyLock` — `private static readonly`
  
  (No XML‑Summary )
- `string _exLogFilePath` — `private static readonly`
  
  (No XML‑Summary )
- `string _logFilePath` — `private static readonly`
  
  (No XML‑Summary )
- `xyLogArchiver _archiver` — `private readonly static`
  
  (No XML‑Summary )

