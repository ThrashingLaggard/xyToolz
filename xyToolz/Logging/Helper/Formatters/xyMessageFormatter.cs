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
    /// Default Log-Message Formatter
    /// </summary>
    public class xyMessageFormatter : IMessageFormatter
    {
        /// <summary>
        /// Format a message for consistent logging
        /// </summary>
        /// <param name="message"></param>
        /// <param name="callerName"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public string FormatMessageForLogging(string message, string? callerName = null, LogLevel? level = null)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

            string logLevel = level?.ToString() ?? "Information";

            callerName = string.IsNullOrEmpty(callerName) ? " / " : callerName;

            string formattedMessage = $"[{timestamp}] [{logLevel}] [{callerName}] \n{message}";

            return formattedMessage;
        }
    }
}
