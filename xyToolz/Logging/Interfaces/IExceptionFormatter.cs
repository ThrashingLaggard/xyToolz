using Microsoft.Extensions.Logging;

namespace xyToolz.Logging.Interfaces
{
    public interface IExceptionFormatter
    {
        string FormatExceptionDetails(Exception ex, LogLevel level, string? callerName = null);
    }
}
