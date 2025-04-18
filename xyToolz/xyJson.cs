using System.Drawing.Imaging;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

namespace xyToolz
{

    /// <summary>
    /// JsonUtils:
    /// 
    /// New Entry in json file
    ///     
    /// Update Entry 
    /// 
    /// new or  update for rsa key
    /// 
    /// Read all contents from the given json file
    ///         --> read value from target key
    /// 
    /// new json file?!
    /// </summary>
    public class xyJson
    {
        #region Json Configuration

        /// <summary>
        /// Default <see cref="JsonSerializerOptions"/> used by all serialization / deserialization helpers.
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

        /// <inheritdoc cref="SaveDataToJsonAsync{T}(T,string,JsonSerializerOptions?)"/>
        public static async Task<bool> SaveDataToJsonAsync<T>(T data, string fileName = "config.json", JsonSerializerOptions? options = null)
        {
            string successMessage = $"Successfully saved JSON data to '{fileName}'.";
            string errorMessage = $"Failed to serialize data to JSON file: '{fileName}'.";

            try
            {
                if ( await xyFiles.EnsurePathExistsAsync(fileName))
                {
                    string jsonData = JsonSerializer.Serialize(data, options ?? defaultJsonOptions);
                    await File.WriteAllTextAsync(fileName, jsonData);
                    await xyLog.AsxLog(successMessage);
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

            try
            {
                bool saveSuccess = await SaveDataToJsonAsync(updatedDictionary, filePath);
                if (saveSuccess)
                {
                    await xyLog.AsxLog(successMessage);
                    return true;
                }
                await xyLog.AsxLog(errorMessage);
            }
            catch (Exception ex)
            {
                await xyLog.AsxExLog(ex);
                await xyLog.AsxLog(errorMessage);
            }
            return false;
        }

        /// <summary>
        /// Adds or updates a key/value pair in a JSON file.
        /// </summary>
        public static async Task AddOrUpdateEntry<T>(string filePath, string key, T value)
        {
            string updateMessage = $"Updated key '{key}' in '{filePath}'.";
            string addMessage = $"Added key '{key}' to '{filePath}'.";
            string errorMessage = $"Failed to update key '{key}' in file '{filePath}'.";

            try
            {
                Dictionary<string, object> data = await DeserializeFromFile(filePath) ?? new();

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

                await SerializeDictionary(filePath, data);
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
        public static async Task<JToken?> GetJTokenFromFile(string filePath, string key)
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
    }
}
