# class xyLogFormatter

Namespace: `xyToolz.Helper.Logging`  
Visibility: `public static`  
Source: `xyToolz\Logging\Static Logging Stuff\xyLogFormatter.cs`

## Description:

/// The xyLogFormatter provides static formatting utilities for structured logging.
    /// 
    /// 
Available Features:

    /// 
    ///   Detailed exception formatting with recursive inner exception tracing
    ///   Customizable logging for messages with timestamp, caller, and severity
    ///   MailMessage formatting for logging purposes
    ///   Performance tracing by formatting operation durations
    ///   Optional structured JSON output for exception data
    /// 
    ///
    /// 
Thread Safety:

    /// Fully thread-safe as all members are static and do not mutate shared state.
    ///
    /// 
Limitations:

    /// Intended for logging purposes only – does not support parsing or structured JSON input.
    ///
    /// 
Performance:

    /// Lightweight operations using StringBuilder, suitable for use in high-frequency log pipelines.
    ///
    /// 
Configuration:

    /// Log level and caller name are optional and customizable on each method call.
    ///
    /// 
Example Usage:

    /// 
    /// var ex = new InvalidOperationException("Something went wrong!");
    /// string formatted = xyLogFormatter.FormatExceptionDetails(ex, LogLevel.Error);
    /// Console.WriteLine(formatted);
    /// 
    ///

## Methods

- `string FormatExceptionAsJson(Exception ex)` — `public static`
  
  /// Serializes exception details to a JSON string (shallow structure only).
        ///
- `string FormatExceptionDetails(Exception ex, LogLevel level, string? message = null,string? callerName = null, int depth = 1)` — `public static`
  
  /// Formats an exception into a structured multi-line message including message, source, stack trace,
        /// and any inner exceptions or custom data. Includes depth and unique identifier.
        ///
- `string FormatMailDetails(MailMessage mailMessage)` — `public static`
  
  /// Converts a  into a detailed readable log string.
        /// Includes metadata such as sender, recipient, subject, and headers.
        ///
- `string FormatMessageForLogging(string message, string? callerName = null, LogLevel? level = null)` — `public static`
  
  /// Formats a log message string for output, adding a timestamp, optional caller name and severity.
        ///
- `string FormatPerformanceLog(string operationName, TimeSpan duration)` — `public static`
  
  /// Creates a log string summarizing a named operation and its execution duration.
        ///

## Fields

- `string AttachmentsLabel` — `private const`
  
  (No XML‑Summary )
- `string BodyLabel` — `private const`
  
  (No XML‑Summary )
- `string DurationLabel` — `private const`
  
  (No XML‑Summary )
- `string FromLabel` — `private const`
  
  (No XML‑Summary )
- `string HeaderLabel` — `private const`
  
  (No XML‑Summary )
- `string OperationLabel` — `private const`
  
  (No XML‑Summary )
- `string SenderLabel` — `private const`
  
  (No XML‑Summary )
- `string SubjectLabel` — `private const`
  
  (No XML‑Summary )
- `string TimestampLabel` — `private const`
  
  (No XML‑Summary )
- `string ToLabel` — `private const`
  
  (No XML‑Summary )

