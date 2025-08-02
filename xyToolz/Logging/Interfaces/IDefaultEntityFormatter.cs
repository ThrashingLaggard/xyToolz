using Microsoft.Extensions.Logging;
using xyToolz.Logging.Models;

namespace xyToolz.Logging.Interfaces
{
    public interface IDefaultEntityFormatter<T>
    {
        string UnpackAndFormatFromEntity(T entry_, string? callerName = null, LogLevel? level = null);
        xyLogEntry PackAndFormatIntoEntity(string source, LogLevel level, string message, DateTime timestamp, uint? id = null, string? description = null, string? comment = null, Exception? exception = null);
    }
}
