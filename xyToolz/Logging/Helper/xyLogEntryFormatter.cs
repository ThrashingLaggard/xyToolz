using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xyToolz.Logging.Interfaces;
using xyToolz.Logging.Models;

namespace xyToolz.Logging.Helper
{
    public class xyLogEntryFormatter<T> : ILogFormatter<T>
    {

        public string FormatMessageForLogging(T entry_, string? callerName = null, LogLevel? level = null)
        {
            if(entry_ is xyLogEntry logEntry)
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
                return "";
            }


        }
      
        
        public string FormatExceptionDetails(Exception ex, LogLevel level, string? callerName = null)
        {
            throw new NotImplementedException();
        }
    }
}
