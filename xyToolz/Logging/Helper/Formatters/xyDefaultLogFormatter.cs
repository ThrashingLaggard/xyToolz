using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xyToolz.Logging.Interfaces;

namespace xyToolz.Logging.Helper.Formatters
{
  /// <summary>
  /// Provides default formatting for log messages and exception details.
  /// </summary>
    public class xyDefaultLogFormatter : IMessageFormatter, IExceptionFormatter
    {
        /// <summary>
        /// Formats detailed information about an exception into a string for logging purposes.
        /// </summary>
        /// <remarks>This method is designed to provide comprehensive exception details for diagnostic and
        /// logging purposes. It includes information about the exception's type, source, target site, stack trace, and
        /// message. If the exception contains custom data or inner exceptions, those details are also included
        /// recursively.</remarks>
        /// <param name="ex">The exception to format. Cannot be <see langword="null"/>.</param>
        /// <param name="level">The log level associated with the exception details.</param>
        /// <param name="message">Optional: additional informationen</param>
        /// <param name="callerName">The name of the calling method or member. If <see langword="null"/>, a placeholder value is used.</param>
        /// <returns>A formatted string containing detailed information about the exception, including its type, source, target
        /// site, stack trace, message, custom data (if any), and details of any inner exceptions.</returns>
        public string FormatExceptionDetails(Exception ex, LogLevel level, string? message = null,string? callerName = null)
        {
            StringBuilder sb_Builder = new ();

            sb_Builder.Append($"{DateTime.Now} [{callerName ?? " / "}] [{level}]");
            
            if (!string.IsNullOrEmpty(message))
            {
                sb_Builder.AppendLine("External message " + message + "\n");
            }

            sb_Builder.AppendLine("Exception Details:");
            sb_Builder.AppendLine($"Type: {ex.GetType().Name}");
            sb_Builder.AppendLine($"Source: {ex.Source}");
            sb_Builder.AppendLine($"TargetSite: {ex.TargetSite}");
            sb_Builder.AppendLine($"StackTrace: {ex.StackTrace}");
            sb_Builder.AppendLine($"HResult: {ex.HResult}");
            sb_Builder.AppendLine($"Message: {ex.Message}");
           
            if (ex.Data != null && ex.Data.Count > 0)
            {
                sb_Builder.AppendLine("Custom Data:");
                foreach (var key in ex.Data.Keys)
                {
                    sb_Builder.AppendLine($"  {key}: {ex.Data[key]}");
                }
            }

            if (ex.InnerException != null)
            {
                sb_Builder.AppendLine("Inner Exception Details:");
                sb_Builder.AppendLine(FormatExceptionDetails(ex.InnerException, level));
            }

            return sb_Builder.ToString();
        }

        /// <summary>
    /// Formats a log message with a timestamp, log level, and caller information.
    /// </summary>
    /// <remarks>The formatted log message includes the current timestamp in the format "yyyy-MM-dd
    /// HH:mm:ss.fff", the specified or default log level, the caller name, and the provided message.</remarks>
    /// <param name="message">The message to be logged. This value cannot be null or empty.</param>
    /// <param name="callerName">The name of the caller or source of the log message. If null or empty, a default value of " / " is used.</param>
    /// <param name="level">The log level associated with the message. If null, the default log level of "Information" is used.</param>
    /// <returns>A formatted string containing the timestamp, log level, caller information, and the log message.</returns>
        public string FormatMessageForLogging(string message, string? callerName = null, LogLevel? level = null)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

                string logLevel = level?.ToString() ?? "Information";

                callerName = string.IsNullOrEmpty(callerName) ? " / " : callerName;

                string formattedMessage = $"[{timestamp}] [{logLevel}] [{callerName}] {message}";

                return formattedMessage;
        }
    }
}
