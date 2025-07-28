using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xyToolz.Logging.Interfaces;

namespace xyToolz.Logging.Helper.Formatters
{
    public class xyMessageFormatter : IMessageFormatter
    {
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
