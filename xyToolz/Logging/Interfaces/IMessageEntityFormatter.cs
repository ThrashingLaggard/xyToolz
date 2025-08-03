using Microsoft.Extensions.Logging;
using xyToolz.Logging.Models;

namespace xyToolz.Logging.Interfaces
{
    public interface IMessageEntityFormatter<T>
    {
        string UnpackAndFormatFromEntity(T entry_, string? callerName = null, LogLevel? level = null);
        xyDefaultLogEntry PackAndFormatIntoEntity(string source, LogLevel level, string message, DateTime timestamp, uint? id = null, string? description = null, string? comment = null, Exception? exception = null);
    }
}
