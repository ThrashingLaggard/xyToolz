using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xyToolz.Logging.Interfaces;

namespace xyToolz.Logging.Helper
{
    public class xyLoggerManager
    {
        private readonly List<ILogging> _loggers;

        public xyLoggerManager()
        {
            _loggers = new List<ILogging>();
        }

        public void RegisterLogger(ILogging logger)
        {
            _loggers.Add(logger);
        }

        public void Log(string message, LogLevel level = LogLevel.Debug)
        {
            foreach (ILogging logger in _loggers)
            {
                logger.Log(message, level);
            }
        }

        public void ExLog(Exception ex, LogLevel level = LogLevel.Error)
        {
            foreach (ILogging logger in _loggers)
            {
                logger.ExLog(ex, level);
            }
        }

        public void Unregister(ILogging target) 
        {
            _loggers.Remove(target);
        }

    }
}
