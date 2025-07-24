using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xyToolz.Logging.Interfaces;
using xyToolz.Logging.Models;

namespace xyToolz.Logging.Helper.Formatters
{
    public class xyLogEntryFormatter<T> : IEntityFormatter<T>
    {
        public string FormatEntityForLogging(T entry_, string? callerName = null, LogLevel? level = null)
        {
            if (entry_ is xyLogEntry logEntry)
            {
                string message = logEntry.Message;
                string timestamp = Stopwatch.GetTimestamp().ToString();


                string logLevel = level?.ToString() ?? "Information";

                callerName = string.IsNullOrEmpty(callerName) ? " / " : callerName;

                string formattedMessage = $"[{timestamp}] [{logLevel}] [{callerName}] {message}";

                return formattedMessage;
            }
            else
            {
                return "No valid instance of xyLogEntry was given!";
            }
        }
    }
}
