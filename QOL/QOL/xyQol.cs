using System.Reflection;
using System.Text;
using xyToolz.Logging.Helper;
using xyToolz.Logging.Loggers;

namespace xyToolz.QOL
{
    /// <summary>
    /// Quality-of-life helper functions around reflection and object-dictionary conversion.
    /// </summary>
    /// <remarks>
    /// This class provides convenience methods to:
    /// <list type="bullet">
    ///   <item><description>Inspect the properties of a target type via reflection.</description></item>
    ///   <item><description>Extract a dictionary of property names and values from an instance.</description></item>
    ///   <item><description>Create and populate an instance of a type from a dictionary.</description></item>
    ///   <item><description>Render a dictionary as a multi-line string.</description></item>
    /// </list>
    /// Logging is performed via <see cref="xyLoggerManager"/>. All exceptions are caught and logged;
    /// methods generally return empty results instead of throwing.
    /// </remarks>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Benennungsstile", Justification = "...")]
    public class xyQol
    {
        private readonly xyLoggerManager _log;
        private readonly xyMessageFactory _fac;

        /// <summary>
        /// Initializes a new instance of <see cref="xyQol"/> with the provided logger manager and message factory.
        /// </summary>
        /// <param name="log_">Logger manager used to record informational messages and exceptions.</param>
        /// <param name="fac_">Factory used to create standardized log messages.</param>
        /// <remarks>
        /// The constructor also registers basic console loggers for <see cref="string"/> and <see cref="System.Exception"/> messages.
        /// </remarks>
        public xyQol(xyLoggerManager log_, xyMessageFactory fac_)
        {
            _log = log_;
            _fac = fac_;

            _log.RegisterLogger(new xyConsoleLogger<string>(), new xyConsoleLogger<Exception>());
        }

        /// <summary>
        /// Returns an array of <see cref="PropertyInfo"/> objects that describe the public properties
        /// of the generic type <typeparamref name="T"/> used by <paramref name="obj"/>.
        /// </summary>
        /// <typeparam name="T">The static type whose properties should be inspected.</typeparam>
        /// <param name="obj">An instance of <typeparamref name="T"/>. Used for null checking only.</param>
        /// <returns>
        /// An array of <see cref="PropertyInfo"/> for <typeparamref name="T"/>; an empty array if
        /// <paramref name="obj"/> is <see langword="null"/> or if no properties are found, or if an error occurs.
        /// </returns>
        /// <remarks>
        /// This method uses the compile-time type parameter <typeparamref name="T"/>—not the runtime type of
        /// <paramref name="obj"/>. If you pass a base type variable that actually holds a derived type,
        /// only the base type's properties are returned.
        /// </remarks>
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
        /// Reads all public property values from <paramref name="obj"/> and returns them as a dictionary.
        /// </summary>
        /// <typeparam name="TKey">The dictionary key type (typically <see cref="string"/>).</typeparam>
        /// <typeparam name="TValue">The dictionary value type (should be compatible with the property values).</typeparam>
        /// <typeparam name="T">The type of <paramref name="obj"/> to inspect.</typeparam>
        /// <param name="obj">The instance whose property values will be read.</param>
        /// <returns>
        /// A dictionary mapping converted property names (<typeparamref name="TKey"/>) to
        /// their values (<typeparamref name="TValue"/>). Properties with <see langword="null"/>
        /// values are skipped. Returns an empty dictionary when <paramref name="obj"/> is null,
        /// has no readable properties, or an error occurs.
        /// </returns>
        /// <remarks>
        /// <para>
        /// Keys are produced by converting the property name (<see cref="string"/>) to <typeparamref name="TKey"/>
        /// via <see cref="Convert.ChangeType(object?, System.Type)"/>. If <typeparamref name="TKey"/> is not
        /// convertible from <see cref="string"/>, the add operation for that property will fail and be logged.
        /// </para>
        /// <para>
        /// Values are cast to <typeparamref name="TValue"/>. If a property's value cannot be cast to
        /// <typeparamref name="TValue"/>, the exception is logged and the property is skipped.
        /// </para>
        /// </remarks>
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
        /// Creates a new instance of <typeparamref name="T"/> and sets its writable public properties
        /// from the provided dictionary of keys and values.
        /// </summary>
        /// <typeparam name="T">The type to instantiate and populate. Must have a parameterless constructor.</typeparam>
        /// <typeparam name="TKey">The dictionary key type (typically property names or name-like values).</typeparam>
        /// <typeparam name="TValue">The dictionary value type (should be convertible to each target property type).</typeparam>
        /// <param name="keyValuePairs">Source dictionary whose keys represent property names and values represent property values.</param>
        /// <returns>
        /// A new instance of <typeparamref name="T"/> with any matching, writable properties set to converted values.
        /// If a key does not correspond to a property (case-insensitive), or the value cannot be converted,
        /// that entry is skipped (and logged). If <paramref name="keyValuePairs"/> is <see langword="null"/>, a default
        /// instance of <typeparamref name="T"/> is returned.
        /// </returns>
        /// <remarks>
        /// <para>
        /// Property matching is case-insensitive. Only writable properties are set.
        /// </para>
        /// <para>
        /// Values are converted using <see cref="Convert.ChangeType(object?, System.Type)"/> which covers many common
        /// conversions but does not handle complex types or custom parsers. For those, consider custom converters.
        /// </para>
        /// </remarks>
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
        /// Renders the contents of a dictionary as a multi-line string in the form "Key : Value".
        /// </summary>
        /// <typeparam name="TKey">Dictionary key type.</typeparam>
        /// <typeparam name="TValue">Dictionary value type.</typeparam>
        /// <typeparam name="T">Unused generic parameter; present for call-site symmetry with other methods.</typeparam>
        /// <param name="keyValuePairs">The dictionary to render.</param>
        /// <returns>
        /// A string where each line contains "Key : Value" for one entry from <paramref name="keyValuePairs"/>.
        /// If the dictionary is empty, returns an empty string.
        /// </returns>
        /// <remarks>
        /// This method is <see langword="static"/> and purely formatting-oriented; it does not perform logging.
        /// </remarks>
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
