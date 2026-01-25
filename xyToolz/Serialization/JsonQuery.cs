using Newtonsoft.Json.Linq;

namespace xyToolz.Serialization
{
    /// <summary>
    /// Little Newtonsoft wrapper
    /// 
    /// 
    /// Facade for raw JSON inspection and querying.
    /// - JObject access
    /// - JToken access
    /// 
    /// Pure pass-through.
    /// No logic.
    /// No state.
    /// </summary>
    public static class JsonQuery
    {
        public static Task<JObject?> GetJObjectFromFile(string filePath)
            => xyJson.GetJObjectFromFile(filePath);

        public static Task<JToken?> GetJTokenFromKey(string filePath, string key)
            => xyJson.GetJTokenFromKey(filePath, key);
    }
}
