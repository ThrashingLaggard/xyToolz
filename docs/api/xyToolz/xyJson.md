# class xyJson

Namespace: `xyToolz`  
Visibility: `public`  
Source: `xyToolz\xyJson.cs`

## Description:

/// Static utility class for handling various JSON operations used throughout the application.
    ///
    /// 
Available Features:

    /// 
    /// Serialize and deserialize objects and dictionaries to and from JSON files.
    /// Access individual keys and nested subkeys as objects, dictionaries, or byte arrays.
    /// Automatic root-tag wrapping and file structure validation for JSON content.
    /// Supports both System.Text.Json and Newtonsoft.Json querying models (JObject/JToken).
    /// Built-in structured logging via xyLog for errors and operations.
    /// 
    ///
    /// 
Thread Safety:

    /// This class is fully static and does not maintain any instance-level state.
    ///
    /// 
Limitations:

    /// - Only top-level and single-level subkey access supported explicitly.
    /// - Not optimized for complex nested arrays or mixed-content JSON structures.
    ///
    /// 
Performance:

    /// Performance depends on file size and structure; optimized for configuration-style JSON files (few KB).
    ///
    /// 
Configuration:

    /// Uses consistent serializer options for formatting and allows override via optional parameters.
    ///
    /// 
Example Usage:

    /// 
    /// string value = await xyJson.GetStringFromJsonFile("settings.json", "jwtPublicKey");
    /// var settings = await xyJson.DeserializeFromFile("appconfig.json");
    /// await xyJson.AddOrUpdateEntry("userprefs.json", "theme", "dark");
    /// 
    ///
    /// 
Related:

    /// 
    /// 
    /// 
    ///

## Methoden

- `Task AddOrUpdateEntry<T>(string path, string key, T value)` — `public static async`
  
  /// Adds or updates a key-value pair in a JSON file.
        /// If the key exists, its value is updated; otherwise, a new entry is added.
        ///
- `Task EnsureJsonRootTag(string filePath)` — `public static async`
  
  /// Ensures the JSON file begins with '{' and ends with '}'.
        /// Adds root structure if missing.
        ///
- `Task TestAddOrUpdateEntry(string path, string key, string value)` — `public static`
  
  (No XML‑Summary )
- `Task<(string First, string Last, string Full)> GetFirstAndLastLinesAsync(string filePath)` — `public static async`
  
  /// Returns the first line, last line, and full joined content from a text file.
        ///
- `Task<bool> SaveDataToJsonAsync<T>(T data, string fileName = "config.json", JsonSerializerOptions? options = null)` — `public static async`
  
  /// Save into file and overwrite it!
        ///
- `Task<bool> SerializeDictionary(string filePath, Dictionary<string, object> updatedDictionary)` — `public static async`
  
  ///  Serializes a dictionary to JSON (overwrites the file).
        ///
- `Task<byte[]?> DeserializeKeyToBytes(string filePath, string key)` — `public static async`
  
  /// Retrieves a base64-encoded value from the given key and decodes it into a byte array.
        ///
- `Task<byte[]?> DeserializeSubKeyToBytes(string filePath, string key, string subkey)` — `public static async`
  
  /// Retrieves a base64-encoded value from the given subkey of a JSON object and decodes it into a byte array.
        ///
- `Task<Dictionary<string, object>?> DeserializeFromFile(string filePath)` — `public static async`
  
  /// Reads the entire JSON file and deserializes it into a dictionary.
        ///
- `Task<Dictionary<string, object>?> DeserializeSubKeyToDictionary(string filePath, string key, string subkey)` — `public static`
  
  /// Deserializes the subkey into a dictionary.
        ///
- `Task<Dictionary<string, object>> DeserializeKeyIntoDictionary(string filePath, string key)` — `public static async`
  
  /// Reads and deserializes the object behind a key into a dictionary.
        ///
- `Task<JObject?> GetJObjectFromFile(string filePath)` — `public static async`
  
  /// Reads a JSON file and parses it into a JObject.
        ///
- `Task<JToken?> GetJTokenFromKey(string filePath, string key)` — `public static async`
  
  /// Extracts a raw JToken from the given key.
        ///
- `Task<object?> DeserializeFromKey(string filePath, string key)` — `public static async`
  
  /// Reads the value from a single key from the JSON file and returns it as an object.
        ///
- `Task<object?> DeserializeSubKey(string filePath, string key, string subkey)` — `public static`
  
  /// Reads a subkey from an object under a key and returns the raw object.
        ///
- `Task<string> GetStringFromJsonFile(string filePath, string key)` — `public static async`
  
  /// Returns the value of a key in the JSON file as string.
        ///
- `Task<string> TestGetStringFromJsonFile(string path, string key)` — `public static`
  
  (No XML‑Summary )
- `void OverrideForTests(IxyJson mocked)` — `public static`
  
  (No XML‑Summary )
- `void ResetOverride()` — `public static`
  
  (No XML‑Summary )

## Felder

- `JsonSerializerOptions defaultJsonOptions` — `internal static readonly`
  
  /// JSON serialization settings for consistent formatting and behavior.
        /// Used as default across all JSON-related methods in this class.
        ///

