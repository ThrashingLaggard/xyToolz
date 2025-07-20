using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using xyToolz.Logging.Interfaces;

namespace xyToolz.Logging.Loggers
{
    public class xyBaseLogger : ILogging
    {

        private readonly ILogFormatter _formatter;

        /// <summary>
        /// Initializes a new instance of the <see cref="xyBaseLogger"/> class with the specified log formatter.
        /// </summary>
        /// <param name="formatter_">The log formatter used to format log messages. Cant be null! />.</param>
        public xyBaseLogger( ILogFormatter formatter_)
        {
            _formatter = formatter_;
        }


        /// <summary>
        /// Logs the details of an exception at the specified log level.
        /// </summary>
        /// <remarks>The method formats the exception details, including the caller's name, and writes the
        /// formatted message to the console.</remarks>
        /// <param name="ex">The exception to log. This parameter cannot be <see langword="null"/>.</param>
        /// <param name="level">The severity level of the log entry.</param>
        /// <param name="callerName">The name of the calling member. This is automatically populated by the compiler if not explicitly provided.</param>
        public void ExLog(Exception ex, LogLevel level, [CallerMemberName] string? callerName = null)
        {
            string exMessage = FormatEx(ex, level, callerName);

            Console.WriteLine(exMessage);
        }

        /// <summary>
        /// Logs a formatted message with the specified log level and optional caller information.
        /// </summary>
        /// <param name="message">The message to log. Cannot be null or empty.</param>
        /// <param name="level">The severity level of the log message.</param>
        /// <param name="callerName">The name of the calling member. This is automatically populated by the compiler  if not explicitly provided.</param>
        public void Log(string message, LogLevel level, [CallerMemberName] string? callerName = null)
        {
            string formattedMsg = FormatMsg(message, callerName, level);
            Console.WriteLine(formattedMsg);
        }

        /// <summary>
        /// Formats a log message with optional caller information and log level.
        /// </summary>
        /// <remarks>The exact format of the returned string is determined by the underlying formatter
        /// implementation.</remarks>
        /// <param name="message">The main message to be logged. This value cannot be null or empty.</param>
        /// <param name="callerName">The name of the caller, typically used to identify the source of the log message.  This parameter is
        /// optional and can be null.</param>
        /// <param name="level">The log level associated with the message, such as Debug, Info, or Error.  This parameter is optional and
        /// can be null.</param>
        /// <returns>A formatted string that includes the provided message, and optionally the caller name and log level.</returns>
        public string FormatMsg(string message, string? callerName = null, LogLevel? level = null)
        {
            return _formatter.FormatMessageForLogging(message, callerName, level);
        }

        /// <summary>
        /// Formats the Exception´s details for consistent logging.
        /// </summary>
        private string FormatEx(Exception ex, LogLevel level, string? callerName = null)
        {
            return _formatter.FormatExceptionDetails(ex, level, callerName);
        }




    }
}
