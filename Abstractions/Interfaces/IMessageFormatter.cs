using Microsoft.Extensions.Logging;


namespace xyToolz.Logging.Interfaces
{
    /// <summary>
    /// Interface for Message-Formatters
    /// </summary>
    public interface IMessageFormatter
    {
        /// <summary>
        /// Format log message
        /// </summary>
        /// <param name="message"></param>
        /// <param name="callerName"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        string FormatMessageForLogging(string message, string? callerName = null, LogLevel? level = null);
    }
}
