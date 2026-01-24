using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace xyToolz.Logging.Interfaces
{
    /// <summary>
    /// Interface for my own loggers
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
        /// <param name="message"></param>
        /// <param name="callerName"></param>
        void ExLog(Exception ex, LogLevel level, string? message = null, [CallerMemberName] string? callerName = null);

        /// <summary>
        /// Set reference to null
        /// </summary>
        void Shutdown();
    }
}