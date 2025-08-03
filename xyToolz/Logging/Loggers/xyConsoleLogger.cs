using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;
using xyToolz.Logging.Helper.Formatters;
using xyToolz.Logging.Interfaces;
using xyToolz.Logging.Models;

namespace xyToolz.Logging.Loggers
{
    /// <summary>
    /// Log formatted MESSAGES and EXCEPTIONS to the console    (currently discards LogEntries)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class xyConsoleLogger<T> : ILogging
    {

        #region "Properties"

        // Almost irrelevant, but not utterly.                                                                                                                                      
        private readonly IMessageFormatter? _msgFormatter;
        private readonly IExceptionFormatter? _excFormatter;
        private readonly IMessageEntityFormatter<T>? _entFormatter;

        #endregion



        #region "Constructors"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msgFormatter_"></param>
        /// <param name="excFormatter_"></param>
        /// <param name="entFormatter_"></param>
        public xyConsoleLogger( IMessageFormatter msgFormatter_ =null!, IExceptionFormatter excFormatter_ = null!, IMessageEntityFormatter<T> entFormatter_ = null!)
        {
            _msgFormatter = msgFormatter_?? null!;
            _excFormatter = excFormatter_?? null!;
            _entFormatter = entFormatter_?? null!;
        }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="entFormatter_"></param>
            public xyConsoleLogger( IMessageEntityFormatter<T> entFormatter_)
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

        #endregion



        #region "Logging"
        /// <summary>
        /// Logs a formatted message with the specified log level and optional caller information.
        /// </summary>
        /// <param name="message">The message to log. Cannot be null or empty.</param>
        /// <param name="level">The severity level of the log message.</param>
        /// <param name="callerName">The name of the calling member. This is automatically populated by the compiler  if not explicitly provided.</param>
        public void Log(string message, LogLevel level, [CallerMemberName] string? callerName = null)
        {
            string formattedMsg = FormatMsg(message, out _, DateTime.Now, null, null, null, callerName, level);
            Console.WriteLine(formattedMsg);
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
            string exMessage = FormatEx(ex, level,out _, null,DateTime.Now,null,null,null, callerName);
            Console.WriteLine(exMessage);
        }

        #endregion



        #region "Formatting"

        /// <summary>
        /// Formats a log message with optional caller information and log level.
        /// </summary>
        /// <remarks>The exact format of the returned string is determined by the underlying formatter
        /// implementation.</remarks>
        /// <returns>A formatted string that includes the provided message, and optionally the caller name and log level.</returns>
        private string FormatMsg(string message, out xyDefaultLogEntry logEntry,DateTime? timestamp = null,uint? id = null,string? description = null, string? comment = null, string? callerName = null, LogLevel? level = LogLevel.Debug)
        {
            logEntry = FormatIntoDefaultLogEntry(callerName!, (LogLevel)level!, message, timestamp ?? DateTime.Now, id, description, comment, null);

            if (_msgFormatter is not null)
            {
                return _msgFormatter.FormatMessageForLogging(message, callerName, level);
            }
            else
            {
                string outputMessage =new xyDefaultLogFormatter().FormatMessageForLogging(message, callerName, level);
                return outputMessage;
            }
        }

        /// <summary>
        /// Formats the Exception´s details for consistent logging.
        /// </summary>
        private string FormatEx(Exception ex, LogLevel level, out xyDefaultLogEntry logEntry, string? message = null, DateTime? timestamp = null, uint? id = null, string? description = null, string? comment = null, string? callerName = null)
        {
            logEntry = FormatIntoDefaultLogEntry(callerName!,level,message!,DateTime.Now,id,  description, comment,ex);

            if(_excFormatter is not null)
            {
                return _excFormatter.FormatExceptionDetails(ex, level, callerName);                     
            }
            else
            {
                string outputMessage = new xyDefaultExceptionFormatter().FormatExceptionDetails(ex,level,callerName);
                return outputMessage;
            }
        }

        /// <summary>
        /// Format a log entity into a message
        /// </summary>
        /// <param name="entry_"></param>
        /// <param name="callerName"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public string FormatFromEntity(T entry_, string? callerName = null, LogLevel? level = null)
        {
            if(_entFormatter is not null)
            {
                return _entFormatter.UnpackAndFormatFromEntity(entry_, callerName, level);
            }
            else  // if DI doesnt work
            {
                xyDefaultLogEntryFormatter<T> formatter = new();
                return formatter.UnpackAndFormatFromEntity(entry_, callerName, level);
            }
        }

        /// <summary>
        /// Pack all relevant information into a xyDefaultLogEntry-instance
        /// </summary>
        /// <param name="source"></param>
        /// <param name="level"></param>
        /// <param name="message"></param>
        /// <param name="timestamp"></param>
        /// <param name="id"></param>
        /// <param name="description"></param>
        /// <param name="comment"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        public xyDefaultLogEntry FormatIntoDefaultLogEntry(string source, LogLevel level, string message, DateTime timestamp, uint? id = null, string? description = null, string? comment = null, Exception? exception = null)
        {
            if (_entFormatter is not null)
            {
                return _entFormatter.PackAndFormatIntoEntity(source, level, message, timestamp, id,description,comment,exception);
            }
            else    // fallback for when DI fails
            {
                xyDefaultLogEntryFormatter<T> formatter = new();
                return formatter.PackAndFormatIntoEntity(source, level, message, timestamp, id, description, comment, exception);
            }
        }

        #endregion

    }
}
