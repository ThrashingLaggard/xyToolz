using Microsoft.EntityFrameworkCore;
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
    /// <summary>
    /// Used to store log messages and exceptions in LogEntries or get the data out of them
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class xyLogEntryFormatter<T> : IEntityFormatter<T>
    {
        /// <summary>
        /// Unpack the data from a LogEntry
        /// </summary>
        /// <param name="entry_"></param>
        /// <param name="callerName"></param>
        /// <param name="level_"></param>
        /// <returns></returns>
        public string UnpackAndFormatFromEntity(T entry_, string? callerName = null, LogLevel? level_ = LogLevel.Debug)
        {
            if (entry_ is xyLogEntry logEntry)
            {
                uint ID = logEntry.ID;
                string description ="Info:" +  logEntry.Description ?? "";
                string comment = "Comment:" + logEntry.Comment??"";
                string source = logEntry.Source;
                LogLevel level = level_ ?? logEntry.Level;
                string timestamp = DateTime.Now.ToString();
                string message = logEntry.Message;
                Exception? exception = logEntry.Exception?? default;

                string formattedExceptionInformation = new xyExceptionFormatter().FormatExceptionDetails(exception,level,callerName);
                
                //callerName = string.IsNullOrEmpty(callerName) ? " / " : callerName;

                string formattedMessage = $"[{ID}{timestamp}] [{level+""}] [{source}] \n{description}\n{comment}\n{message}\n{formattedExceptionInformation}";

                return formattedMessage;
            }
            else
            {
                return "No valid instance of xyLogEntry was given!";
            }
        }

        /// <summary>
        /// Pack the data from logmessages into objects
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
        public xyLogEntry PackAndFormatIntoEntity(string source, LogLevel level, string message, DateTime timestamp,  uint? id = null, string? description = null, string? comment= null, Exception? exception = null)
        {
            xyLogEntry entry = new(source_: source, level_: level, message_: message, exception_: exception)
            {
                ID = id ?? 0,
                Description = description ?? "",
                Comment = comment ?? "",
                Source = source,
                Level = level,
                Timestamp = timestamp,
                Message = message,
                Exception = exception ?? default,
            };
            return entry;
        }
    }
}
