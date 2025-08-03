using Microsoft.Extensions.Logging;
using xyToolz.Logging.Models;

namespace xyToolz.Logging.Interfaces
{
    public interface IExceptionEntityFormatter
    {
        xyExceptionEntry PackAndFormatIntoEntity(Exception exception, DateTime? timestamp = null, string? message = null, uint? id = null, string? description = null);

        string UnpackAndFormatFromEntity<T, TKey, TValue>(T entry_, string? callerName = null, LogLevel? level = LogLevel.Debug) where T : class where TKey : class;
    }
}
