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

    }
}
