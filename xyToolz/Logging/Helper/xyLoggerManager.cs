using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xyToolz.List;
using xyToolz.Logging.Interfaces;

namespace xyToolz.Logging.Helper
{
    /// <summary>
    /// Manages a collection of loggers and provides methods for logging messages and exceptions.
    /// 
    ///                                                                                                                                                                                     Stuff for Eventhandlers is planned!
    /// </summary>
    public class xyLoggerManager
    {
        public string Description { get; set; } = "Your advertisements here!";
        public ushort Count { get; private set; }
        
        private readonly IEnumerable<ILogging> _loggers;

        /// <summary>
        /// 
        /// </summary>
        public xyLoggerManager()
        {
            _loggers = [];
        }

        /// <summary>
        /// Registers a new logger to the logging system.
        /// </summary>
        /// <remarks>This method adds the specified logger to the internal collection of loggers.  The
        /// registered logger will be used for logging operations performed by the system.</remarks>
        /// <param name="logger">The logger instance to be registered. Cannot be null.</param>
        public void RegisterLogger(ILogging logger)
        {
            _loggers.Append(logger);
            Count++;
        }

        /// <summary>
        /// Unregisters the specified logger from the logging system.
        /// </summary>
        /// <remarks>This method removes the specified logger from the collection of active loggers. After
        /// calling this method, the specified logger will no longer receive log messages.</remarks>
        /// <param name="target">The logger instance to be unregistered. Must not be null.</param>
        public void UnregisterLogger(ILogging target) 
        {
            _loggers.ToList().Remove(target);
            Count--;
        }


        /// <summary>
        /// Logs a message with the specified log level to all registered loggers.
        /// </summary>
        /// <remarks>This method iterates through all configured loggers and forwards the message to each
        /// one. Ensure that at least one logger is configured to avoid the message being discarded.</remarks>
        /// <param name="message">The message to log. Cannot be null or empty.</param>
        /// <param name="level">The severity level of the log message. Defaults to <see cref="LogLevel.Debug"/> if not specified.</param>
        public void Log(string message, LogLevel level = LogLevel.Debug)
        {
            foreach (ILogging logger in _loggers)
            {
                logger.Log(message, level);
            }
        }


        /// <summary>
        /// Logs the specified exception at the given log level using all registered loggers.
        /// </summary>
        /// <param name="ex">The exception to log. Cannot be <see langword="null"/>.</param>
        /// <param name="level">The severity level of the log entry. Defaults to <see cref="LogLevel.Error"/>.</param>
        public void ExLog(Exception ex, LogLevel level = LogLevel.Error)
        {
            foreach (ILogging logger in _loggers)
            {
                logger.ExLog(ex, level);
            }
        }

     

    }
}
