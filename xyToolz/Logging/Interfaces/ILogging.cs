using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace xyToolz.Logging.Interfaces
{
    /// <summary>
    /// Interface for loggers
    /// </summary>
    public interface ILogging
    {
        /// <summary>
        /// Write a message 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="level"></param>
        /// <param name="callerName"></param>
        void Log(string message, LogLevel level, [CallerMemberName] string? callerName = null);
        /// <summary>
        /// Write an exception
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="level"></param>
        /// <param name="callerName"></param>
        void ExLog(Exception ex, LogLevel level, [CallerMemberName] string? callerName = null);
    }
}