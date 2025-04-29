using System;
using System.Net.Mail;
using System.Text;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Collections.Generic;

namespace xyToolz.Helper.Logging
{
    /// <summary>
    /// The <c>xyLogFormatter</c> provides static formatting utilities for structured logging.
    /// 
    /// <para><b>Available Features:</b></para>
    /// <list type="bullet">
    ///   <item><description>Detailed exception formatting with recursive inner exception tracing</description></item>
    ///   <item><description>Customizable logging for messages with timestamp, caller, and severity</description></item>
    ///   <item><description>MailMessage formatting for logging purposes</description></item>
    ///   <item><description>Performance tracing by formatting operation durations</description></item>
    ///   <item><description>Optional structured JSON output for exception data</description></item>
    /// </list>
    ///
    /// <para><b>Thread Safety:</b></para>
    /// Fully thread-safe as all members are static and do not mutate shared state.
    ///
    /// <para><b>Limitations:</b></para>
    /// Intended for logging purposes only – does not support parsing or structured JSON input.
    ///
    /// <para><b>Performance:</b></para>
    /// Lightweight operations using StringBuilder, suitable for use in high-frequency log pipelines.
    ///
    /// <para><b>Configuration:</b></para>
    /// Log level and caller name are optional and customizable on each method call.
    ///
    /// <para><b>Example Usage:</b></para>
    /// <code>
    /// var ex = new InvalidOperationException("Something went wrong!");
    /// string formatted = xyLogFormatter.FormatExceptionDetails(ex, LogLevel.Error);
    /// Console.WriteLine(formatted);
    /// </code>
    /// </summary>
    public static class xyLogFormatter
    {
        #region Constants

        private const string TimestampLabel = "Timestamp:";
        private const string FromLabel = "From:";
        private const string SenderLabel = "Sender:";
        private const string ToLabel = "To:";
        private const string SubjectLabel = "Subject:";
        private const string AttachmentsLabel = "AttachmentsCount:";
        private const string HeaderLabel = "Header:";
        private const string BodyLabel = "Body:";
        private const string OperationLabel = "Operation:";
        private const string DurationLabel = "Duration:";

        #endregion

        #region Exception Formatting

        /// <summary>
        /// Formats an exception into a structured multi-line message including message, source, stack trace,
        /// and any inner exceptions or custom data. Includes depth and unique identifier.
        /// </summary>
        /// <param name="ex">The exception to format.</param>
        /// <param name="level">The log level context in which the exception occurred.</param>
        /// <param name="callerName">Optional: The method or class name that triggered the exception.</param>
        /// <param name="depth">Internal recursion depth counter (default 1).</param>
        /// <returns>A full textual description of the exception hierarchy.</returns>
        public static string FormatExceptionDetails(Exception ex, LogLevel level, string? callerName = null, int depth = 1)
        {
            StringBuilder sb = new();
            string exceptionId = Guid.NewGuid().ToString();

            sb.AppendLine($"====================[ EXCEPTION ]====================");
            sb.AppendLine($"Exception-ID: {exceptionId}");
            sb.AppendLine($"Depth: {depth}");
            sb.AppendLine($"{DateTime.Now} [{callerName ?? "Unknown"}] [{level}] Exception Details:");
            sb.AppendLine($"Type: {ex.GetType().Name}");
            sb.AppendLine($"Message: {ex.Message}");
            sb.AppendLine($"TargetSite: {ex.TargetSite}");
            sb.AppendLine($"Source: {ex.Source}");
            sb.AppendLine($"StackTrace: {ex.StackTrace}");

            if (ex.Data?.Count > 0)
            {
                sb.AppendLine("Custom Data:");
                foreach (var key in ex.Data.Keys)
                    sb.AppendLine($"  {key}: {ex.Data[key]}");
            }

            if (ex.InnerException != null)
            {
                sb.AppendLine("Inner Exception Details:");
                sb.AppendLine(FormatExceptionDetails(ex.InnerException, level, callerName, depth + 1));
            }

            return sb.ToString();
        }

        /// <summary>
        /// Serializes exception details to a JSON string (shallow structure only).
        /// </summary>
        /// <param name="ex">The exception to serialize.</param>
        /// <returns>A JSON string representation of the exception.</returns>
        public static string FormatExceptionAsJson(Exception ex)
        {
            var exceptionInfo = new Dictionary<string, object?>
            {
                ["Type"] = ex.GetType().FullName,
                ["Message"] = ex.Message,
                ["TargetSite"] = ex.TargetSite?.ToString(),
                ["Source"] = ex.Source,
                ["StackTrace"] = ex.StackTrace,
                ["Timestamp"] = DateTime.UtcNow.ToString("o"),
                ["Data"] = ex.Data?.Count > 0 ? ex.Data : null,
                ["InnerException"] = ex.InnerException?.Message
            };

            return JsonSerializer.Serialize(exceptionInfo, new JsonSerializerOptions
            {
                WriteIndented = true
            });
        }

        #endregion

        #region Message Formatting

        /// <summary>
        /// Formats a log message string for output, adding a timestamp, optional caller name and severity.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="callerName">Optional: Caller class/method name.</param>
        /// <param name="level">Optional: Severity level of the log message (default = Information).</param>
        /// <returns>A formatted string for logging.</returns>
        public static string FormatMessageForLogging(string message, string? callerName = null, LogLevel? level = null)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string logLevel = level?.ToString() ?? "Information";
            string caller = string.IsNullOrEmpty(callerName) ? "UnknownCaller" : callerName;
            return $"[{timestamp}] [{logLevel}] [{caller}] {message}";
        }

        #endregion

        #region Mail Formatting

        /// <summary>
        /// Converts a <see cref="MailMessage"/> into a detailed readable log string.
        /// Includes metadata such as sender, recipient, subject, and headers.
        /// </summary>
        /// <param name="mailMessage">The email message to log.</param>
        /// <returns>A formatted string containing email details.</returns>
        public static string FormatMailDetails(MailMessage mailMessage)
        {
            StringBuilder sb = new();
            sb.AppendLine($"{TimestampLabel} {DateTime.Now}");
            sb.AppendLine($"{FromLabel} {mailMessage.From?.Address}");
            sb.AppendLine($"{SenderLabel} {mailMessage.Sender}");
            sb.AppendLine($"{ToLabel} {string.Join(", ", mailMessage.To.Select(m => m.Address))}");
            sb.AppendLine($"{SubjectLabel} {mailMessage.Subject}");
            sb.AppendLine($"{AttachmentsLabel} {mailMessage.Attachments.Count}");
            sb.AppendLine($"{HeaderLabel} {mailMessage.Headers}");
            sb.AppendLine($"{BodyLabel} {mailMessage.Body}");
            return sb.ToString();
        }

        #endregion

        #region Performance Formatting

        /// <summary>
        /// Creates a log string summarizing a named operation and its execution duration.
        /// </summary>
        /// <param name="operationName">Descriptive name of the operation or task measured.</param>
        /// <param name="duration">The elapsed time in a <see cref="TimeSpan"/> object.</param>
        /// <returns>A formatted string suitable for performance logging.</returns>
        public static string FormatPerformanceLog(string operationName, TimeSpan duration)
        {
            StringBuilder sb = new();
            sb.AppendLine($"{TimestampLabel} {DateTime.Now}");
            sb.AppendLine($"{OperationLabel} {operationName}");
            sb.AppendLine($"{DurationLabel} {duration.TotalMilliseconds} ms");
            return sb.ToString();
        }

        #endregion
    }
}
