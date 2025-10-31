using System.Reflection;
using System.Text;
using xyToolz.Logging.Helper;
using xyToolz.Logging.Loggers;

namespace xyToolz.QOL
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Benennungsstile", Justification = "...")]
    public class xyQol
    {
        private readonly xyLoggerManager _log;
        private readonly xyMessageFactory _fac;

        public xyQol(xyLoggerManager log_, xyMessageFactory fac_)
        {
            _log = log_;
            _fac = fac_;

            _log.RegisterLogger(new xyConsoleLogger<string>(), new xyConsoleLogger<Exception>());
        }

        /// <summary>
        /// Returns an array of PropertyInfo for the specified target
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public PropertyInfo[] GetPropertyInfosForTarget<T>(T obj)
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

        /// <summary>
        /// Read all values from all properties from the target
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public Dictionary<TKey, TValue> GetPropertyValuesForTarget<TKey, TValue, T>(T obj) where T : class where TKey : class
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

        /// <summary>
        /// Create an instance of whatever the dictionary holds
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="keyValuePairs"></param>
        /// <returns></returns>
        public T GetEntityFromDictionary<T, TKey, TValue>(Dictionary<TKey, TValue> keyValuePairs) where T : class where TKey :class
        {
            // Creating a target-type instance
            T target = Activator.CreateInstance<T>();// CSI Gnarzraha, das habe ich gebraucht, lol!!1!!!!!!!!!!!1!!!!!111!!!!!!!  

            if (keyValuePairs is not null)
            {
                // Placeholder, needed for reflection
                string propertyName = "";

                // Getting all properties for the target type
                PropertyInfo[] propertyInfos = GetPropertyInfosForTarget(target);
                
                // Iterating trough'em all
                foreach (var keyValuePair in keyValuePairs)
                {
                    // Preparing string for search
                    propertyName = keyValuePair.Key!.ToString()!;

                    try
                    {
                        // Looking for the corresponding property
                        if (Array.Find(propertyInfos, x => x.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase)) is PropertyInfo info)
                        {
                            // Checking for writabillity
                            if (info.CanWrite)
                            {
                                // Convert the chosen value to the target type
                                object? value = Convert.ChangeType(keyValuePair.Value, info.PropertyType);

                                // Setting this value for the target
                                info.SetValue(target, value);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _log.ExLog(ex);
                    }
                }
            }
            return target;
        }

        /// <summary>
        /// Print all properties and their values into the console
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="keyValuePairs"></param>
        /// <returns></returns>
        public static string PropertiesToString< TKey, TValue,T>(Dictionary<TKey, TValue>  keyValuePairs) where T : class where TKey :class
        {
            StringBuilder stringBuilder = new();
            foreach (var keyValue in keyValuePairs)
            {
                stringBuilder.AppendLine($"{keyValue.Key.ToString()} : {keyValue.Value +""}");
            }

            return stringBuilder.ToString();
        }

    }
}
