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
    public class xyConsoleLogger<T> : ILogging
    {
    //                                                                                                                                                              TODO: Add methods o digest xyLogEntrys
        private readonly IMessageFormatter _msgFormatter;
        private readonly IExceptionFormatter _excFormatter;
        private readonly IEntityFormatter<T> _entFormatter;



       /// <summary>
       /// 
       /// </summary>
       /// <param name="msgFormatter_"></param>
       /// <param name="excFormatter_"></param>
       /// <param name="entFormatter_"></param>
        public xyConsoleLogger( IMessageFormatter msgFormatter_ =null!, IExceptionFormatter excFormatter_ = null!, IEntityFormatter<T> entFormatter_ = null!)
        {
            _msgFormatter = msgFormatter_?? null!;
            _excFormatter = excFormatter_?? null!;
            _entFormatter = entFormatter_?? null!;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entFormatter_"></param>
        public xyConsoleLogger( IEntityFormatter<T> entFormatter_)
        {
            _entFormatter = entFormatter_;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="excFormatter_"></param>
        public xyConsoleLogger(IExceptionFormatter excFormatter_)
        {
            _excFormatter = excFormatter_;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msgFormatter_"></param>
        public xyConsoleLogger(IMessageFormatter msgFormatter_)
        {
            _msgFormatter = msgFormatter_;
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
        /// <returns>A formatted string that includes the provided message, and optionally the caller name and log level.</returns>
        private string FormatMsg(string message, string? callerName = null, LogLevel? level = null)
        {
            return _msgFormatter.FormatMessageForLogging(message, callerName, level);           // Should create entity
        }

        /// <summary>
        /// Formats the Exception´s details for consistent logging.
        /// </summary>
        private string FormatEx(Exception ex, LogLevel level, string? callerName = null)
        {
            return _excFormatter.FormatExceptionDetails(ex, level, callerName);                         // Should create entity
        }

        /// <summary>
        /// Format a log entity into a message
        /// </summary>
        /// <param name="entry_"></param>
        /// <param name="callerName"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        private string FormatEntity(T entry_, string? callerName = null, LogLevel? level = null)
        {
            return _entFormatter.FormatEntityForLogging(entry_, callerName, level);
        }


    }
}
