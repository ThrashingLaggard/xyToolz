using Microsoft.Extensions.Logging;

namespace xyToolz.Logging.Interfaces
{
    public interface IEntityFormatter<T>
    {
        string UnpackAndFormatFromEntity(T entry_, string? callerName = null, LogLevel? level = null);
    }
}
