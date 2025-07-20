using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace xyToolz.Logging.Interfaces
{
    public interface ILogging
    {
        void Log(string message, LogLevel level, [CallerMemberName] string? callerName = null);
        void ExLog(Exception ex, LogLevel level, [CallerMemberName] string? callerName = null);
    }
}