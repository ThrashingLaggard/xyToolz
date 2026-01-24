using Microsoft.Extensions.Logging;
using xyToolz.Logging.Models;

namespace xyToolz.Logging.Interfaces
{
    /// <summary>
    /// Interface for Exception-Formatters
    /// </summary>
    public interface IExceptionEntityFormatter
    {
        /// <summary>
        /// Pack information into an entity
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="timestamp"></param>
        /// <param name="message"></param>
        /// <param name="id"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        xyExceptionEntry PackAndFormatIntoEntity(Exception exception, DateTime? timestamp = null, string? message = null, uint? id = null, string? description = null);

        /// <summary>
        /// Unpack information from entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="entry_"></param>
        /// <param name="callerName"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        string UnpackAndFormatFromEntity<T, TKey, TValue>(T entry_, string? callerName = null, LogLevel? level = LogLevel.Debug) where T : class where TKey : class;
    }
}
