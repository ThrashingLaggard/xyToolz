using System.Text.Json;

namespace xyToolz.Serialization
{
    /// <summary>
    /// JSON file persistence and structure handling.
    /// Pure pass-through to xyJson.
    /// </summary>
    public static class JsonFiles
    {
        public static Task<bool> SaveAsync<T>(
            T data,
            string fileName = "config.json",
            CancellationToken ct = default,
            JsonSerializerOptions? options = null)
            => xyJson.SaveDataToJsonAsync(data, fileName, ct, options);

        public static Task<bool> SaveDictionaryAsync(
            string filePath,
            Dictionary<string, object> dictionary)
            => xyJson.SerializeDictionary(filePath, dictionary);

        public static Task<Dictionary<string, object>?> LoadAsync(string filePath)
            => xyJson.DeserializeFromFile(filePath);

        /// <summary>
        /// 0 = Dictionary
        /// 1 = List
        /// 2 = Array
        /// </summary>
        public static Task<T?> LoadAsync<T>(string filePath, int outputFormat)
            => xyJson.DeserializeFromFile<T>(filePath, outputFormat);


        public static Task<(string First, string Last, string Full)>
            ReadLinesAsync(string filePath)
            => xyJson.GetFirstAndLastLinesAsync(filePath);

        public static Task<string>
            ReadValueAsync(string filePath, string key)
            => xyJson.GetStringFromJsonFile(filePath, key);

        public static Task<string>
            TestReadValueAsync(string filePath, string key)
            => xyJson.TestGetStringFromJsonFile(filePath, key);

        public static Task
            TestAddOrUpdateAsync(string path, string key, string value)
            => xyJson.TestAddOrUpdateEntry(path, key, value);
    }
}
