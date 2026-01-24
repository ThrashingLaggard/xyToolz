using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using xyToolz.Helper.Formatters;
using xyToolz.Logging.Interfaces;
using xyToolz.Logging.Models;

namespace xyToolz.Logging.Loggers
{
    /// <summary>
    /// 
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Benennungsstile", Justification = "<This is the way>")]
    public class xyAsyncLogger<T> : ILogging, IDisposable
    {
        private readonly BlockingCollection<string> _logQueue = [];
        private readonly CancellationTokenSource _cts = new();
        private readonly Task _worker;
        private readonly StreamWriter _writer;

        public IMessageFormatter? MessageFormatter { get; set; }
        public IExceptionFormatter? ExceptionFormatter { get; set; }
        public IMessageEntityFormatter<T>? MessageEntryFormatter { get; set; } 
        public IExceptionEntityFormatter? ExceptionEntryFormatter { get; set; }

        public xyAsyncLogger(string? filepath= null,IMessageFormatter? messageFormatter_ =null, IExceptionFormatter? exceptionFormatter_ = null, IMessageEntityFormatter<T>? messageEntryFormatter_ = null, IExceptionEntityFormatter? exceptionEntryFormatter_ = null)
        {
            MessageFormatter = messageFormatter_?? default;
            ExceptionFormatter = exceptionFormatter_ ?? default;
            ExceptionEntryFormatter = exceptionEntryFormatter_ ?? default;
            MessageEntryFormatter = messageEntryFormatter_ ?? default;

            // Setup the writer
            _writer = new(filepath!, true) 
            {
                AutoFlush = true,
            };

            // Starting the asynchronous work
            _worker = Task.Run(() => ProcessQueue(), _cts.Token);
        }
        /// <summary>
        /// Writes an informative message
        /// </summary>
        /// <param name="message"></param>
        /// <param name="level"></param>
        /// <param name="callerName"></param>
        public void Log(string message, LogLevel level, [CallerMemberName] string? callerName = null)
        {
            // xyDefaultLogEntry? logEntry = default;
            string formattedMsg = FormatMsg(message, out _, DateTime.Now, null, null, null, callerName, level);// 
            Enqueue(formattedMsg, callerName);
        }

        /// <summary>
        /// Writes an exception
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="level"></param>
        /// <param name="message">Optional: additional informationen</param>
        /// <param name="callerName"></param>
        public void ExLog(Exception ex, LogLevel level, string? message = null, [CallerMemberName] string? callerName = null)
        {
            //xyExceptionEntry? excEntry =  default;
            string exMessage = FormatEx(ex, level, out _, message, callerName);

            Enqueue(exMessage, callerName);
        }


        /// <summary>
        /// Add a message to the queue for logging
        /// </summary>
        /// <param name="message"></param>
        /// <param name="callerName"></param>
        private void Enqueue(string message, [CallerMemberName] string? callerName = null)
        {
            try
            {
                _logQueue.Add(message);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ExceptionFormatter?.FormatExceptionDetails(ex,LogLevel.Error,callerName));
                Console.Out.Flush();
            }
        }

        /// <summary>
        /// Work through the queued log messages
        /// </summary>
        private async Task ProcessQueue()
        {
            try
            {
                foreach (string message in _logQueue.GetConsumingEnumerable(_cts.Token))
                {
                    await WriteToTarget(message);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ExceptionFormatter?.FormatExceptionDetails(ex, LogLevel.Error));
                await Console.Out.FlushAsync();
            }
        }

        /// <summary>
        /// Write the output to the target =>   0 for console,    1 for file
        /// </summary>
        /// <param name="message"></param>
        /// <param name="logTargets"></param>
        /// <returns></returns>
        private async Task WriteToTarget(string message, params int[] logTargets)
        {
            if (logTargets.Contains(0))
            {
                Console.WriteLine(message);
                await Console.Out.FlushAsync();
            }
            else if(logTargets.Contains(1))
            {
                await _writer.WriteLineAsync(message);
            }
        }



        /// <summary>
        /// Formats a log message with optional caller information and log level.
        /// </summary>
        /// <remarks>The exact format of the returned string is determined by the underlying formatter
        /// implementation.</remarks>
        /// <returns>A formatted string that includes the provided message, and optionally the caller name and log level.</returns>
        private string FormatMsg(string message, out xyDefaultLogEntry logEntry, DateTime? timestamp = null, uint? id = null, string? description = null, string? comment = null, string? callerName = null, LogLevel? level = LogLevel.Debug)
        {
            logEntry = FormatIntoDefaultLogEntry(callerName!, (LogLevel)level!, message, timestamp ?? DateTime.Now, id, description, comment, null);

            if (MessageFormatter is not null)
            {
                return MessageFormatter.FormatMessageForLogging(message, callerName, level);
            }
            else
            {
                string outputMessage = new xyDefaultLogFormatter().FormatMessageForLogging(message, callerName, level);
                return outputMessage;
            }
        }

        /// <summary>
        /// Formats the Exception´s details for consistent logging.
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="level"></param>
        /// <param name="excEntry"></param>
        /// <param name="information"></param>
        /// <param name="callerName"></param>
        /// <returns></returns>
        private string FormatEx(Exception ex, LogLevel level, out xyExceptionEntry excEntry, string? information = null, string? callerName = null)
        {
            excEntry = FormatIntoExceptionEntry(ex, information);

            if (ExceptionFormatter is not null)
            {
                return ExceptionFormatter.FormatExceptionDetails(ex, level, callerName);
            }
            else
            {
                string outputMessage = new xyDefaultExceptionFormatter().FormatExceptionDetails(ex, level, callerName);
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
            if (MessageEntryFormatter is not null)
            {
                return MessageEntryFormatter.UnpackAndFormatFromEntity(entry_, callerName, level);
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
            if (MessageEntryFormatter is not null)
            {
                return MessageEntryFormatter.PackAndFormatIntoEntity(source, level, message, timestamp, id, description, comment, exception);
            }
            else    // fallback for when DI fails
            {
                xyDefaultLogEntryFormatter<T> formatter = new();
                return formatter.PackAndFormatIntoEntity(source, level, message, timestamp, id, description, comment, exception);
            }
        }

        /// <summary>
        ///  Pack an Exception into an ExceptionEntry for easier serialization and storage
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="information"></param>
        /// <returns></returns>
        public xyExceptionEntry FormatIntoExceptionEntry(Exception exception, string? information = null)
        {
            try
            {
                return ExceptionEntryFormatter!.PackAndFormatIntoEntity(exception, DateTime.Now, information);
            }
            catch (Exception ex)
            {
                xyExceptionEntry exEntry = new(ex)
                {
                    Exception = ex,
                    Timestamp = DateTime.Now,
                    Message = "Fallback solution!!! There seems to be no ExceptionEntry formatter at work!",
                };
                return exEntry;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Shutdown()
        {
            _logQueue.CompleteAdding();
            _cts.Cancel();
            _worker.Wait();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Shutdown();
            _worker.Dispose();
            _writer.Dispose();
            _logQueue.Dispose();
            _cts.Dispose();
        }
    }
}
