using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xyToolz.Logging.Interfaces;

namespace xyToolz.Logging.Loggers
{

    /// <summary>
    /// This is a wrapper to make ILogger work with my own ILogging Framework
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="logger_"></param>
    public class xyILoggerAdapter<T>(ILogging logger_) : ILogger<T>
    {
        private readonly ILogging _logger = logger_;

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            bool isEnabled = false;
            
            if((int)logLevel  < 6)
            {
                isEnabled = true;
            }
            return isEnabled;

        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            string caller = $"{eventId.Id}---{eventId.Name}";
            string message = formatter(state, exception);
            if (exception is not null)
            {
                _logger.ExLog(exception, logLevel, message,caller);
            }
            else
            {
                _logger.Log(message, logLevel, caller);
            }
        }
    }
}
