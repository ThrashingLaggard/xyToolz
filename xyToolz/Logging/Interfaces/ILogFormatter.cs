using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xyToolz.Logging.Interfaces
{
    public interface ILogFormatter
    {
        string FormatMessageForLogging(string message, string? callerName = null, LogLevel? level = null);
        string FormatExceptionDetails(Exception ex, LogLevel level, string? callerName = null);
    }
}
