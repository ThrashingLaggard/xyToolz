using Newtonsoft.Json.Linq;

namespace xyToolz.Serialization
{
    /// <summary>
    /// Key- and Subkey-based JSON access.
    /// Pure pass-through to xyJson.
    /// </summary>
    public static class JsonKeys
    {
        public static Task<object?> GetAsync(
            string filePath,
            string key)
            => xyJson.DeserializeFromKey(filePath, key);

        public static Task<Dictionary<string, object>>GetDictionaryAsync(string filePath, string key)
            => xyJson.DeserializeKeyIntoDictionary(filePath, key);

        public static Task<byte[]?>GetBytesAsync(string filePath, string key)
            => xyJson.DeserializeKeyToBytes(filePath, key);

        public static Task AddOrUpdateAsync<T>(
            string filePath,
            string key,
            T value)
            => xyJson.AddOrUpdateEntry(filePath, key, value);


        public static Task<object?> GetSubAsync(
            string filePath,
            string key,
            string subkey)
            => xyJson.DeserializeSubKey(filePath, key, subkey);

        public static Task<Dictionary<string, object>?>GetSubDictionaryAsync(
                string filePath,
                string key,
                string subkey)
            => xyJson.DeserializeSubKeyToDictionary(filePath, key, subkey);

        public static Task<byte[]?>GetSubBytesAsync(
                string filePath,
                string key,
                string subkey)
            => xyJson.DeserializeSubKeyToBytes(filePath, key, subkey);

    }
}
