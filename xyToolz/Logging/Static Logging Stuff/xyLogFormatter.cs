using Microsoft.Extensions.Logging;
using System.Collections;
using System.Globalization;
using System.Net.Mail;
using System.Text;
using System.Text.Json;

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
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Benennungsstile", Justification = "mimimimimimimimiimmiimmiimi")]
    public static class xyLogFormatter
    {
        #region Constants

        private const string TimestampLabel = "Timestamp: ";
        private const string FromLabel = "From :";
        private const string SenderLabel = "Sender: ";
        private const string ToLabel = "To: ";
        private const string SubjectLabel = "Subject: ";
        private const string AttachmentsLabel = "AttachmentsCount: ";
        private const string HeaderLabel = "Header: ";
        private const string BodyLabel = "Body: ";
        private const string OperationLabel = "Operation: ";
        private const string DurationLabel = "Duration: ";
        private const string CcLabel = "Cc:";
        private const string BccLabel= "Bcc: ";
        private const int MaxDepth = 69;


        public static JsonSerializerOptions jsonOptions = new JsonSerializerOptions()
        {
            WriteIndented = true,
        };
        #endregion

        #region Exception Formatting

        /// <summary>
        /// Formats an exception into a structured multi-line message including message, source, stack trace,
        /// and any inner exceptions or custom data. Includes depth and unique identifier.
        /// </summary>
        /// <param name="ex">The exception to format.</param>
        /// <param name="level">The log level context in which the exception occurred.</param>
        /// <param name="message">optional additional information</param>
        /// <param name="callerName">Optional: The method or class name that triggered the exception.</param>
        /// <param name="depth">Internal recursion depth counter (default 1).</param>
        /// <returns>A full textual description of the exception hierarchy.</returns>
        public static string FormatExceptionDetails(Exception ex, LogLevel? level = LogLevel.Error, string? message = null, string? callerName = null, int depth = 1)
        {
            StringBuilder sb = new(1024);
            string id = Guid.NewGuid().ToString();
            FormattingExceptionDetails(sb, ex, id, level, message, callerName, depth);

            return sb.ToString();
        }

        private static void FormattingExceptionDetails(StringBuilder sb, Exception ex, string id, LogLevel? level = LogLevel.Error, string? message = null, string? callerName = null, int depth = 1)
        {


            if (depth > MaxDepth)
            {
                sb.AppendLine("### Safety break!!! ###");
                sb.AppendLine("The depth of ").Append(MaxDepth).AppendLine(" Exceptions has been exceeded. Please seek help from a better programmer!");
                return;
            }

            if (!string.IsNullOrEmpty(message))
            {
                sb.Append("External Message: ").Append(message).AppendLine().AppendLine(); ;
            }

            sb.Append(DateTime.UtcNow).Append("  [").Append(callerName ?? "Unknown").Append("]  [").Append(level).AppendLine("]");

            sb.AppendLine($"====================[ EXCEPTION ]====================");
            sb.Append("Exception-ID: ").AppendLine(id);
            sb.Append("Depth: ").Append(depth).AppendLine();
            sb.Append("Type: ").AppendLine(ex.GetType().Name);
            sb.Append("Message: ").AppendLine(ex.Message);
            sb.Append("TargetSite: ").AppendLine(ex.TargetSite?.ToString());
            sb.Append("Source: ").AppendLine(ex.Source);
            sb.Append("StackTrace: ").AppendLine(ex.StackTrace);

            if (ex.Data?.Count > 0)
            {
                sb.AppendLine("Custom Data:");
                foreach (DictionaryEntry key in ex.Data)
                    sb.Append("     ").Append(key.Key).Append("  :  ").AppendLine(key.Value?.ToString());
            }

            if (ex.InnerException != null)
            {
                sb.AppendLine("Inner Exception Details:");
                FormattingExceptionDetails(sb, ex.InnerException, id, level, callerName, depth: depth + 1);
            }
            return;
        }

        /// <summary>
        /// Serializes exception details to a JSON string (shallow structure only).
        /// </summary>
        /// <param name="ex">The exception to serialize.</param>
        /// <returns>A JSON string representation of the exception.</returns>
        public static string FormatExceptionAsJson(Exception ex)
        {
            try
            {
                if (BuildExceptionDictionary(ex) is Dictionary<string, object?> exceptionInfo)
                    return JsonSerializer.Serialize(exceptionInfo, jsonOptions);
                else return JsonSerializer.Serialize(ex);
            }
            catch (Exception serEx) 
            {
                xyLog.ExLog(serEx);
                return new StringBuilder(256).Append("{\"Error\":\"JSON serialization failed\",\"OriginalMessage\":").Append(JsonSerializer.Serialize(ex.Message)).Append(",\"SerializerError\":").Append(JsonSerializer.Serialize(serEx.Message)).Append("}").ToString();
            }
        }

        private static Dictionary<string, object?>? BuildExceptionDictionary(Exception ex, int depth = 1) 
        {
            Dictionary<string, object?> exceptionInfo;
            try
            {
                exceptionInfo = new()
                {
                    ["Type"] = ex.GetType().FullName,
                    ["Message"] = ex.Message,
                    ["TargetSite"] = ex.TargetSite?.ToString(),
                    ["Source"] = ex.Source,
                    ["StackTrace"] = ex.StackTrace,
                    ["Timestamp"] = DateTime.UtcNow.ToString("o"),
                    ["Data"] = ex.Data?.Count > 0 ? ex.Data.Cast<DictionaryEntry>().ToDictionary(e => e.Key.ToString()!, e => e.Value?.ToString()) : null,
                    ["InnerException"] = ex.InnerException is not null && depth <= MaxDepth ? BuildExceptionDictionary(ex.InnerException, depth + 1) : null
                };
                return exceptionInfo;
            }
            catch (Exception dicEx)
            {
                xyLog.ExLog(dicEx,"An Error occured while trying to build a dictionary for serialization!");
            }
            return null;
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
#if NET6_0_OR_GREATER
            return string.Create(CultureInfo.InvariantCulture,$"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff}] [{level ?? LogLevel.Debug}] [{(string.IsNullOrEmpty(callerName) ? "UnknownCaller" : callerName)}] {message}");
#else
        return new StringBuilder(128).Append('[').Append(DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff")).Append("] [").Append(level?.ToString() ?? "Debug").Append("] [").Append(string.IsNullOrEmpty(callerName) ? "UnknownCaller" : callerName).Append("] ").Append(message).ToString();
#endif
        }

        /// <summary>
        /// Format a log message and serialize to JSON,
        /// Adds a timestamp, optional caller name and severity.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="callerName">Optional: Caller class/method name.</param>
        /// <param name="level">Optional: Severity level of the log message (default = Information).</param>
        /// <returns>A formatted JSON string for logging.</returns>
        public static string FormatMessageAsJson(string message, string? callerName = null, LogLevel? level = null)
        {
            Dictionary<string, string?> payload = new ()
            {
                ["Timestamp"] = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                ["Level"] = level?.ToString() ?? "Debug",
                ["Caller"] = string.IsNullOrEmpty(callerName) ? "UnknownCaller" : callerName,
                ["Message"] = message
            };
            return JsonSerializer.Serialize(payload, jsonOptions);
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
            StringBuilder sb = new(512);

            sb.Append(TimestampLabel).Append(' ').AppendLine(DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            sb.Append(FromLabel).Append(' ').AppendLine(mailMessage.From?.Address);
            sb.Append(SenderLabel).Append(' ').AppendLine(mailMessage.Sender?.Address);

            sb.Append(ToLabel).Append(' ')
              .AppendLine(string.Join(", ", mailMessage.To.Select(m => m.Address)));

            if (mailMessage.CC.Count > 0)sb.Append(CcLabel).Append(' ').AppendLine(string.Join(", ", mailMessage.CC.Select(m => m.Address)));

            if (mailMessage.Bcc.Count > 0)sb.Append(BccLabel).Append(' ').AppendLine(string.Join(", ", mailMessage.Bcc.Select(m => m.Address)));

            sb.Append(SubjectLabel).Append(' ').AppendLine(mailMessage.Subject);
            sb.Append("IsBodyHtml: ").AppendLine(mailMessage.IsBodyHtml.ToString());
            sb.Append(BodyLabel).Append(' ').AppendLine(mailMessage.Body);

            sb.Append(AttachmentsLabel).Append(' ').AppendLine(mailMessage.Attachments.Count.ToString());
            if (mailMessage.Attachments.Count > 0)
            {
                foreach (var att in mailMessage.Attachments)
                {
                    sb.Append("  ").AppendLine(att.Name);
                }
            }

            if (mailMessage.Headers.Count > 0)
            {
                sb.AppendLine(HeaderLabel);
                foreach (string key in mailMessage.Headers.Keys)
                {
                    sb.Append("  ").Append(key).Append(": ")
                      .AppendLine(mailMessage.Headers[key]);
                }
            }

            return sb.ToString();
        }

        #endregion 
    }
}
