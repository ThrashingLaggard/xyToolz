using Newtonsoft.Json.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using xyToolz.Helper.Interfaces;

namespace xyToolz
{

    /// <summary>
    /// Static utility class for handling various JSON operations used throughout the application.
    ///
    /// <para><b>Available Features:</b></para>
    /// <list type="bullet">
    /// <item><description>Serialize and deserialize objects and dictionaries to and from JSON files.</description></item>
    /// <item><description>Access individual keys and nested subkeys as objects, dictionaries, or byte arrays.</description></item>
    /// <item><description>Automatic root-tag wrapping and file structure validation for JSON content.</description></item>
    /// <item><description>Supports both System.Text.Json and Newtonsoft.Json querying models (JObject/JToken).</description></item>
    /// <item><description>Built-in structured logging via xyLog for errors and operations.</description></item>
    /// </list>
    ///
    /// <para><b>Thread Safety:</b></para>
    /// This class is fully static and does not maintain any instance-level state.
    ///
    /// <para><b>Limitations:</b></para>
    /// - Only top-level and single-level subkey access supported explicitly.
    /// - Not optimized for complex nested arrays or mixed-content JSON structures.
    ///
    /// <para><b>Performance:</b></para>
    /// Performance depends on file size and structure; optimized for configuration-style JSON files (few KB).
    ///
    /// <para><b>Configuration:</b></para>
    /// Uses consistent serializer options for formatting and allows override via optional parameters.
    ///
    /// <para><b>Example Usage:</b></para>
    /// <code>
    /// string value = await xyJson.GetStringFromJsonFile("settings.json", "jwtPublicKey");
    /// var settings = await xyJson.DeserializeFromFile("appconfig.json");
    /// await xyJson.AddOrUpdateEntry("userprefs.json", "theme", "dark");
    /// </code>
    ///
    /// <para><b>Related:</b></para>
    /// <see cref="System.Text.Json"/>
    /// <see cref="Newtonsoft.Json.Linq.JObject"/>
    /// <see cref="xyToolz.Logging.xyLog"/>
    /// </summary>
    public class xyJson
    {
        #region Json Configuration

        /// <summary>
        /// JSON serialization settings for consistent formatting and behavior.
        /// Used as default across all JSON-related methods in this class.
        /// </summary>
        internal static readonly JsonSerializerOptions defaultJsonOptions = new()
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            AllowOutOfOrderMetadataProperties = true,
            DefaultBufferSize = 4096,
            AllowTrailingCommas = false,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Default,
            UnknownTypeHandling = JsonUnknownTypeHandling.JsonElement,
        };

        #endregion

        #region File‑Utilities

       

        #endregion

        #region Serialization

        /// <summary>
        /// Save into file and overwrite it!
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="fileName"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static async Task<bool> SaveDataToJsonAsync<T>(T data, string fileName = "config.json", JsonSerializerOptions? options = null)
        {
            string errorMessage = $"Failed to serialize data to JSON file: '{fileName}'.";

            try
            {
                if ( await xyFiles.EnsurePathExistsAsync(fileName))
                {
                    string jsonData = JsonSerializer.Serialize(data, options ?? defaultJsonOptions);
                    await File.WriteAllTextAsync(fileName, jsonData);                  
                    return true;
                }
            }
            catch (Exception ex)
            {
                await xyLog.AsxExLog(ex);
                await xyLog.AsxLog(errorMessage);
            }
            return false;
        }

        /// <summary>
        /// Serializes a dictionary to JSON (overwrites the file).
        /// </summary>
        public static async Task<bool> SerializeDictionary(string filePath, Dictionary<string, object> updatedDictionary)
        {
            string successMessage = $"Successfully serialized {updatedDictionary.Count} entries to file: '{filePath}'.";
            string errorMessage = $"Failed to serialize dictionary to file: '{filePath}'.";

            bool isSaved = await SaveDataToJsonAsync(updatedDictionary, filePath);

            await xyLog.AsxLog(isSaved ? successMessage : errorMessage);
            return isSaved;
        }

        /// <summary>
        /// Adds or updates a key-value pair in a JSON file.
        /// If the key exists, its value is updated; otherwise, a new entry is added.
        /// </summary>
        /// <typeparam name="T">The type of the value to store.</typeparam>
        /// <param name="path">The path to the JSON file.</param>
        /// <param name="key">The key to add or update.</param>
        /// <param name="value">The value to assign to the key.</param>
        public static async Task AddOrUpdateEntry<T>(string path, string key, T value)
        {
            string updateMessage = $"Updated key '{key}' in '{path}'.";
            string addMessage = $"Added key '{key}' to '{path}'.";
            string errorMessage = $"Failed to update key '{key}' in file '{path}'.";

            if(_override is not null)
            {
                 await _override?.AddOrUpdateEntry(path, key, value as string);
                return;
            }
            try
            {
                Dictionary<string, object> data = await DeserializeFromFile(path) ?? new();

                if (data.ContainsKey(key))
                {
                    data[key] = value!;
                    await xyLog.AsxLog(updateMessage);
                }
                else
                {
                    data.Add(key, value!);
                    await xyLog.AsxLog(addMessage);
                }

                await SerializeDictionary(path, data);
            }
            catch (Exception ex)
            {
                await xyLog.AsxExLog(ex);
                await xyLog.AsxLog(errorMessage);
            }
        }

        #endregion

        #region "Deserialization – full file / top‑level key"

        /// <summary>
        /// Reads the entire JSON file and deserializes it into a dictionary.
        /// </summary>
        /// <param name="filePath">Path to the JSON file.</param>
        /// <returns>Deserialized dictionary or null if reading fails.</returns>
        public static async Task<Dictionary<string, object>?> DeserializeFromFile(string filePath)
        {
            try
            {
                dynamic stream = await xyFiles.GetStreamFromFileAsync(filePath);
                if (stream == null) return null;
                return await JsonSerializer.DeserializeAsync<Dictionary<string, object>>(stream, defaultJsonOptions);
            }
            catch (Exception ex)
            {
                await xyLog.AsxExLog(ex);
                return null;
            }
        }

        /// <summary>
        /// Reads a single key from the JSON file and returns it as an object.
        /// </summary>
        /// <param name="filePath">Path to the JSON file.</param>
        /// <param name="key">Top-level key to extract.</param>
        /// <returns>Value at the key or null if not found.</returns>
        public static async Task<object?> DeserializeFromKey(string filePath, string key) => await TryDeserializeKey<object>(filePath, key);

        /// <summary>
        /// Reads and deserializes the object behind a key into a dictionary.
        /// </summary>
        /// <param name="filePath">Path to the JSON file.</param>
        /// <param name="key">Top-level key pointing to an object.</param>
        /// <returns>Dictionary at the key or empty dictionary if missing.</returns>
        public static async Task<Dictionary<string, object>> DeserializeKeyIntoDictionary(string filePath, string key)  => await TryDeserializeKey<Dictionary<string, object>>(filePath, key) ?? new();

        /// <summary>
        /// Retrieves a base64-encoded value from the given key and decodes it into a byte array.
        /// </summary>
        /// <param name="filePath">The path to the JSON file.</param>
        /// <param name="key">The top-level key containing the base64 string.</param>
        /// <returns>Decoded byte array or null if conversion fails.</returns>
        public static async Task<byte[]?> DeserializeKeyToBytes(string filePath, string key)
        {
            string errorMessage = $"Key '{key}' could not be decoded to bytes from '{filePath}'.";
            string? base64 = await TryDeserializeKey<string>(filePath, key);
            byte[]? decoded = xy.BaseToBytes(base64);
            if (decoded == null)
            {
                await xyLog.AsxLog(errorMessage);
            }
            return decoded;
        }


        #endregion

        #region "Generic Helpers (top‑level & subkey)"

        /// <summary>
        /// Internal helper that attempts to extract a value of type <typeparamref name="T"/> from a top-level JSON key.
        /// </summary>
        /// <typeparam name="T">The expected return type.</typeparam>
        /// <param name="filePath">Path to the JSON file.</param>
        /// <param name="key">Top-level key.</param>
        /// <returns>The deserialized value or default if not found or failed.</returns>
        private static async Task<T?> TryDeserializeKey<T>(string filePath, string key)
        {
            try
            {
                JObject? obj = await GetJObjectFromFile(filePath);
                if (obj != null && obj.TryGetValue(key, out JToken? token))
                {
                    return token!.ToObject<T>();
                }
                else
                {
                    string missingKeyMessage = $"Key '{key}' not found in '{filePath}'.";
                    await xyLog.AsxLog(missingKeyMessage);
                    return default;
                }
            }
            catch (Exception ex)
            {
                await xyLog.AsxExLog(ex);
                return default;
            }
        }

        /// <summary>
        /// Internal helper that attempts to extract a value of type <typeparamref name="T"/> from a nested key within a JSON object.
        /// </summary>
        /// <typeparam name="T">The expected return type.</typeparam>
        /// <param name="filePath">Path to the JSON file.</param>
        /// <param name="key">Parent key containing the nested object.</param>
        /// <param name="subkey">Subkey to extract the value from.</param>
        /// <returns>The deserialized value or default if not found or failed.</returns>
        private static async Task<T?> TryDeserializeSubkey<T>(string filePath, string key, string subkey)
        {
            string parentKeyMessage = $"Parent key '{key}' not found or not an object in '{filePath}'.";
            string subkeyMessage = $"Subkey '{subkey}' not found under '{key}' in '{filePath}'.";
            try
            {
                JObject? obj = await GetJObjectFromFile(filePath);
                if (obj is null) return default;

                if (!obj.TryGetValue(key, out JToken? parent) || parent is not JObject nested)
                {

                    await xyLog.AsxLog(parentKeyMessage);
                    return default;
                }

                if (nested.TryGetValue(subkey, out JToken? subToken))
                {
                    return subToken!.ToObject<T>();
                }
                else
                {

                    await xyLog.AsxLog(subkeyMessage);
                    return default;
                }
            }
            catch (Exception ex)
            {
                await xyLog.AsxExLog(ex);
                return default;
            }
        }


        #endregion

        #region "Subkey helper wrapper"

        /// <summary>
        /// Reads a subkey from an object under a key and returns the raw object.
        /// </summary>
        /// <param name="filePath">Path to the JSON file.</param>
        /// <param name="key">Parent key.</param>
        /// <param name="subkey">Subkey within the parent object.</param>
        /// <returns>Object at the subkey or null.</returns>
        public static Task<object?> DeserializeSubKey(string filePath, string key, string subkey)=> TryDeserializeSubkey<object>(filePath, key, subkey);

        /// <summary>
        /// Deserializes the subkey into a dictionary.
        /// </summary>
        /// <param name="filePath">Path to the JSON file.</param>
        /// <param name="key">Parent key.</param>
        /// <param name="subkey">Subkey within the parent object.</param>
        /// <returns>Dictionary at the subkey or null.</returns>
        public static Task<Dictionary<string, object>?> DeserializeSubKeyToDictionary(string filePath, string key, string subkey) => TryDeserializeSubkey<Dictionary<string, object>>(filePath, key, subkey);

        /// <summary>
        /// Retrieves a base64-encoded value from the given subkey of a JSON object and decodes it into a byte array.
        /// </summary>
        /// <param name="filePath">The path to the JSON file.</param>
        /// <param name="key">The top-level key containing the sub-object.</param>
        /// <param name="subkey">The subkey whose value should be converted.</param>
        /// <returns>Decoded byte array or null if conversion fails.</returns>
        public static async Task<byte[]?> DeserializeSubKeyToBytes(string filePath, string key, string subkey)
        {
            string errorMessage = $"Subkey '{subkey}' under '{key}' could not be decoded to bytes from '{filePath}'.";
            string? b64 = await TryDeserializeSubkey<string>(filePath, key, subkey);
            byte[]? decoded = xy.BaseToBytes(b64);
            if (decoded == null)
            {
                await xyLog.AsxLog(errorMessage);
            }
            return decoded;
        }


        #endregion

        #region "JObject& JToken"

        /// <summary>
        /// Reads a JSON file and parses it into a JObject.
        /// </summary>
        /// <param name="filePath">Path to the JSON file.</param>
        /// <returns>Parsed JObject or null on failure.</returns>
        public static async Task<JObject?> GetJObjectFromFile(string filePath)
        {
            try
            {
                string json = await File.ReadAllTextAsync(filePath);
                return JObject.Parse(json);
            }
            catch (Exception ex)
            {
                await xyLog.AsxExLog(ex);
                return null;
            }
        }

        /// <summary>
        /// Extracts a raw JToken from the given key.
        /// </summary>
        /// <param name="filePath">Path to the file.</param>
        /// <param name="key">Key to extract.</param>
        /// <returns>The token or null.</returns>
        public static async Task<JToken?> GetJTokenFromKey(string filePath, string key)
        {
            string errorMessage = $"Key '{key}' not found in file '{filePath}'.";
            try
            {
                JObject? jsonObject = await GetJObjectFromFile(filePath);
                if (jsonObject != null && jsonObject.TryGetValue(key, out JToken? token))
                {
                    return token;
                }
                await xyLog.AsxLog(errorMessage);
                return null;
            }
            catch (Exception ex)
            {
                await xyLog.AsxExLog(ex);
                return null;
            }
        }

        /// <summary>
        /// Tries to access a nested subkey inside a top-level key and returns its value.
        /// </summary>
        /// <param name="filePath">Path to the JSON file.</param>
        /// <param name="key">Top-level key to look into.</param>
        /// <param name="subkey">Nested subkey whose value should be retrieved.</param>
        /// <returns>The corresponding JToken, or null if not found.</returns>
        private static async Task<JToken?> GetJTokenFromSubkey(string filePath, string key, string subkey)
        {
            string errorMessage = $"Unable to locate subkey '{subkey}' in key '{key}' from file '{filePath}'.";
            JObject? jsonObject = await GetJObjectFromFile(filePath);
            var token = jsonObject?[key]?[subkey];
            if (token is null)
            {
                await xyLog.AsxLog(errorMessage);
            }
            return token;
        }


        /// <summary>
        /// Returns the value of a key in the JSON file as string.
        /// </summary>
        /// <param name="filePath">Path to the JSON file.</param>
        /// <param name="key">Key whose value to retrieve.</param>
        /// <returns>Value of the key as string, or empty if not found.</returns>
        public static async Task<string> GetStringFromJsonFile(string filePath, string key)
        {
            JToken? token = await GetJTokenFromKey(filePath, key);
            if(token is not null)
            {
                return token.ToString();
            }
            return "";
        }

        #endregion

        #region "Root tag helper"

        /// <summary>
        /// Adds surrounding braces to content to ensure JSON root structure.
        /// </summary>
        /// <param name="content">The content to wrap.</param>
        /// <returns>Wrapped content as a JSON object.</returns>
        private static string AddRootTag(string content) => $"\n{content}\n";

        /// <summary>
        /// Ensures the JSON file begins with '{' and ends with '}'.
        /// Adds root structure if missing.
        /// </summary>
        public static async Task EnsureJsonRootTag(string filePath)
        {
            string addedRootMsg = $"Added JSON‑root braces to file '{filePath}'.";
            string alreadyRooted = $"File '{filePath}' already contains a valid root object.";

            try
            {
                string content = await File.ReadAllTextAsync(filePath);
                if (content.TrimStart().StartsWith('{') && content.TrimEnd().EndsWith('}'))
                {
                    await xyLog.AsxLog(alreadyRooted);
                    return;
                }

                string rooted = AddRootTag(content);
                await File.WriteAllTextAsync(filePath, rooted);
                await xyLog.AsxLog(addedRootMsg);
            }
            catch (Exception ex) { await xyLog.AsxExLog(ex); }
        }


        #endregion
        
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        /// <summary>
        /// Returns the first line, last line, and full joined content from a text file.
        /// </summary>
        public static async Task<(string First, string Last, string Full)> GetFirstAndLastLinesAsync(string filePath)
        {
            try
            {
                string[] lines = await File.ReadAllLinesAsync(filePath);
                if (lines.Length == 0) return (string.Empty, string.Empty, string.Empty);
                return (lines.First(), lines.Last(), string.Join('\n', lines));
            }
            catch (Exception ex)
            {
                await xyLog.AsxExLog(ex);
                return (string.Empty, string.Empty, string.Empty);
            }
        }


        private static IxyJson? _override;

        public static void OverrideForTests(IxyJson mocked) => _override = mocked;
        public static void ResetOverride() => _override = null;


        public static Task<string?> TestGetStringFromJsonFile(string path, string key) =>
            _override?.GetStringFromJsonFile(path, key) ?? GetStringFromJsonFile(path, key);

        public static Task TestAddOrUpdateEntry(string path, string key, string value) =>
            _override?.AddOrUpdateEntry(path, key, value) ?? AddOrUpdateEntry(path, key, value);

    }
}

