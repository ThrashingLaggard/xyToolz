using Microsoft.Extensions.Logging;
using xyToolz.Logging.Interfaces;
using xyToolz.Logging.Models;
using xyToolz.QOL;

namespace xyToolz.Logging.Helper.Formatters
{
    public class xyDefaultExceptionEntryFormatter : IExceptionEntityFormatter
    {
        public xyExceptionEntry PackAndFormatIntoEntity(Exception exception, DateTime? timestamp = null,string? message = null,  uint? id = null, string? description = null)
        {
            xyExceptionEntry entry = new(exception)
            {
                Exception = exception,
                Message = message ?? "",
                Timestamp = timestamp ?? DateTime.Now,
                Description = description ?? "",
            };
            return entry;
        }

      

        /// <inheritdoc/>
        public string UnpackAndFormatFromEntity<T, TKey, TValue>(T entry_, string? callerName = null, LogLevel? level = LogLevel.Debug)   where T : class where TKey : class
        {
            if (entry_ is xyExceptionEntry entry)
            {
                xyQol qol = new(new(),new());
                Dictionary<TKey,TValue> dictionary = qol.GetPropertyValuesForTarget<TKey, TValue, T>(entry_);
                string properties = qol.PropertiesToString<TKey,TValue,T>(dictionary);
                return properties;
            }
            return "";
        }

    }
}
