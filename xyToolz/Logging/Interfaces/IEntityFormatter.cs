using Microsoft.Extensions.Logging;

namespace xyToolz.Logging.Interfaces
{
    public interface IEntityFormatter<T>
    {
        string FormatEntityForLogging(T entry_, string? callerName = null, LogLevel? level = null);
    }
}
