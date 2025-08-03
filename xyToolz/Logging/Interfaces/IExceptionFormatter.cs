using Microsoft.Extensions.Logging;

namespace xyToolz.Logging.Interfaces
{
    /// <summary>
    /// Interface for Exception-Formatters
    /// </summary>
    public interface IExceptionFormatter
    {
        /// <summary>
        /// Format Exception details
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="level"></param>
        /// <param name="callerName"></param>
        /// <returns></returns>
        string FormatExceptionDetails(Exception ex, LogLevel level, string? callerName = null);
    }
}
