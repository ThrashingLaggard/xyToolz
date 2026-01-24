using Microsoft.Extensions.Logging;
using System.Reflection;
using System.Text;
using xyToolz.Logging.Helper;
using xyToolz.Logging.Interfaces;
using xyToolz.Logging.Models;

namespace xyToolz.Helper.Formatters
{
    public class xyDefaultExceptionEntryFormatter : IExceptionEntityFormatter
    {
        private readonly xyLoggerManager _log;
        private readonly xyMessageFactory _fac;

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
                Dictionary<TKey,TValue> dictionary = GetPropertyValuesForTarget<TKey, TValue, T>(entry_);
                string properties = PropertiesToString<TKey,TValue,T>(dictionary);
                return properties;
            }
            return "";
        }

        private PropertyInfo[] GetPropertyInfosForTarget<T>(T obj)
        {
            try
            {
                if (obj is null)
                {
                    _log.Log(_fac.ParameterNull(nameof(obj)));
                    return [];
                }
                else
                {
                    Type type = typeof(T);
                    if (type.GetProperties() is PropertyInfo[] propertyInfos && propertyInfos.Length > 0)
                    {
                        _log.Log($"Successfully read the property infos for {type}");
                        return propertyInfos;
                    }
                }
            }
            catch (Exception ex)
            {
                _log.ExLog(ex);
            }
            return [];
        }


        private Dictionary<TKey, TValue> GetPropertyValuesForTarget<TKey, TValue, T>(T obj) where T : class where TKey : class
        {
            PropertyInfo[] propertyInfos = GetPropertyInfosForTarget(obj);

            Dictionary<TKey, TValue> propertyDictionary = [];
            TKey key = default!;
            object? value = default;


            foreach (PropertyInfo info in propertyInfos)
            {
                value = info.GetValue(obj);
                if (value is null) continue;
                try
                {
                    key = (TKey)Convert.ChangeType(info.Name, typeof(TKey));
                    propertyDictionary.Add(key, (TValue)value!);
                }
                catch (Exception ex)
                {
                    _log.ExLog(ex);
                }
            }

            return propertyDictionary;
        }
        public static string PropertiesToString<TKey, TValue, T>(Dictionary<TKey, TValue> keyValuePairs) where T : class where TKey : class
        {
            StringBuilder stringBuilder = new();
            foreach (var keyValue in keyValuePairs)
            {
                stringBuilder.AppendLine($"{keyValue.Key.ToString()} : {keyValue.Value + ""}");
            }

            return stringBuilder.ToString();
        }

    }
}
