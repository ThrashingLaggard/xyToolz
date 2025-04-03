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
        /// <summary>
        /// Provide default options for the serializer, so he shutteth the fucketh up
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

        #region "Helper"
        public static async Task EnsureJsonRootTag(string filePath)
        {
            try
            {
                if (await GetFirstAndLastLines(filePath) is (string, string, string) first_last_content)
                {
                    if (CheckForJsonRootTag(first_last_content) is bool isTaggedAsJson)
                    {
                        if (isTaggedAsJson)
                        {
                            await xyLog.AsxLog("File is already rooted to Json");
                        }
                        else
                        {
                            string rooted = AddRootTag(first_last_content.Item3);
                            await File.WriteAllTextAsync(filePath, rooted);
                            await xyLog.AsxLog("Added a Json root tag to the file");
                        }
                    }
                }
                await xyLog.AsxLog("Cant ensure json root tag for target file");
            }
            catch (Exception ex)
            {
                await xyLog.AsxExLog(ex);
            }
        }


        internal static async Task<(string, string, string)> GetFirstAndLastLines(string filePath)
        {
            string lineError = "An error occured while trying to get the first and last line";
            string fileError = "Unreadable file?!";
            string fileHandlingError = "Unable to handle file";

            if (xyFiles.EnsurePathExists(filePath))
            {
                if (File.ReadLines(filePath) is IEnumerable<string> lines)
                {
                    if (lines.Count() == 0) return (null!, null!, null!);
                    else
                    {
                        if (lines.FirstOrDefault() is string firstLine && lines.LastOrDefault() is string lastLine)
                        {
                            return (firstLine, lastLine, xyColQol.Spill(lines));
                        }
                        await xyLog.AsxLog(lineError);
                    }
                }
                await xyLog.AsxLog(fileError);
            }
            await xyLog.AsxLog(fileHandlingError);
            return (null!, null!, null!);
        }

        /// <summary>
        /// Looks at the first char of the first and the last char from the last line and checks if they are curly braces {...}
        /// </summary>
        /// <param name="firstLine_LastLine"></param>
        /// <returns>
        /// yes -> true 
        /// no  -> false
        /// </returns>
        private static bool? CheckForJsonRootTag((string, string, string) firstLine_LastLine)
        {
            string firstLine = firstLine_LastLine.Item1;
            string lastLine = firstLine_LastLine.Item2;
            if (string.IsNullOrEmpty(firstLine) || string.IsNullOrEmpty(lastLine)) return false;

            return firstLine.StartsWith('{') && lastLine.EndsWith('}');
        }



        /// <summary>
        /// Encapsulates a string wich curly braces
        /// </summary>
        /// <param name="content"></param>
        /// <returns>
        /// {
        /// content
        /// }</returns>
        private static string AddRootTag(string content)
        {
            string rootTag = "{\n";
            string endTag = "\n}";

            string rootedContent = rootTag + content + endTag;
            return rootedContent;
        }


        #endregion


        #region "File I/O"
        /// <summary>
        /// Erstellt eine neue JSON-Datei, wenn sie nicht existiert.
        /// </summary>
        /// <param name="filePath">Der Pfad zur JSON-Datei.</param>
        private static void CreateNewJsonFile(string filePath)
        {
            var json = new { };
            var options = new JsonSerializerOptions { WriteIndented = true };
            var jsonContent = JsonSerializer.Serialize(json, options);
            File.WriteAllText(filePath, jsonContent);
        }

        /// <summary>
        /// Fügt einen neuen Schlüssel hinzu oder aktualisiert einen bestehenden Schlüssel in der JSON-Datei.
        /// </summary>
        /// <param name="filePath">Der Pfad zur JSON-Datei.</param>
        /// <param name="key">Der Schlüssel, der hinzugefügt oder aktualisiert werden soll.</param>
        /// <param name="value">Der Wert, der dem Schlüssel zugeordnet werden soll.</param>
        public static async Task AddOrUpdateEntry<T>(string filePath, string key, T value)
        {
            Dictionary<string, object>? keyValuePairsFromJsonFile = null;
            string? updatedJsonContent = null;
            try
            {
                if (xyFiles.EnsurePathExists(filePath))
                {
                    keyValuePairsFromJsonFile = await DeserializeFileIntoDictionary(filePath);
                    if (keyValuePairsFromJsonFile is not null)
                    {
                        if (keyValuePairsFromJsonFile.ContainsKey(key))
                        {
                            keyValuePairsFromJsonFile[key] = value;
                        }
                        else
                        {
                            keyValuePairsFromJsonFile.Add(key, value);
                        }
                    }
                }
                try
                {
                    updatedJsonContent = JsonSerializer.Serialize(keyValuePairsFromJsonFile, defaultJsonOptions);
                }
                catch (JsonException jEx)
                {
                    xyLog.ExLog(jEx);
                }
                await EnsureJsonRootTag(filePath);
                await File.WriteAllTextAsync(filePath, updatedJsonContent);

            }
            catch (Exception ex)
            {
                xyLog.ExLog(ex);
            }
        }
        #endregion


        #region "Serialization"


        /// <summary>
        /// Serialize data into a targetfile
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="subfolder"></param>
        /// <param name="fileName"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static async Task<bool> SaveDataToJsonAsync<T>(T data, string fileName = "config.json", JsonSerializerOptions? options = default)
        {

            try
            {
                if (xyFiles.EnsurePathExists(fileName))
                {
                    string jsonData = JsonSerializer.Serialize(data, options ?? defaultJsonOptions);

                    if (File.ReadLines(fileName) is IEnumerable<string> lines)
                    {
                        await File.AppendAllTextAsync(fileName, jsonData);
                    }
                    else
                    {
                        await File.WriteAllTextAsync(fileName, jsonData);
                    }
                    await xyLog.AsxLog($"{jsonData} was now added to {fileName}");
                    return true;
                }
            }
            catch (JsonException jEx)
            {
                xyLog.ExLog(jEx);
            }
            return false;
        }

        /// <summary>
        /// Write a dictionary into the target file OVERWRITING it completely
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="updatedDictionary"></param>
        /// <returns></returns>
        public static async Task<bool> SerializeDictionary(string filePath, Dictionary<string, object> updatedDictionary)
        {
            try
            {
                await SaveDataToJsonAsync(updatedDictionary, filePath);
                await xyLog.AsxLog($"{updatedDictionary.Count()} entrys are stored in the dictionary, it was now added to {filePath}");
                return true;
            }
            catch (JsonException jEx)
            {
                xyLog.ExLog(jEx);
            }
            return false;
        }


        // Diese beiden liefern die ganze Datei als ersten Key im Dictionary zurück... das muss noch agepasst und verfeinert werden
        public static async Task<Dictionary<string, object>?> DeserializeFromFile(string filePath) => await JsonSerializer.DeserializeAsync<Dictionary<string, object>>(await xyFiles.GetStreamFromFile(filePath));
        public static async Task<Dictionary<string, object>?> DeserializeFileIntoDictionary(string filePath)
        {
            Dictionary<string, object> jsonDic = [];
            try
            {
                if (await File.ReadAllTextAsync(filePath) is string jsonContent)
                {
                    MemoryStream mStream = new(xy.StringToBytes(jsonContent));
                    jsonDic = await JsonSerializer.DeserializeAsync<Dictionary<string, object>>(mStream);
                    if (jsonDic.Count < 1)
                    {
                        await xyLog.AsxLog("Unable to deserialize!");
                    }
                }
            }
            catch (Exception ex)
            {
                await xyLog.AsxExLog(ex);
            }
            return jsonDic!;
        }


        public static async Task<JObject?> GetJObjectFromFile(string filePath) => xyFiles.EnsurePathExists(filePath) ? (await File.ReadAllTextAsync(filePath) is string jsonFileContent) ? JObject.Parse(jsonFileContent) : (await xyLog.AsxLog("Cant read file content into JObject"), new JObject[0].FirstOrDefault()).Item2 : null;

        public static async Task<JToken> GetJTokenFromJsonFile(string filePath, string key)
        {
            string keyOk = $"Token for '{key}' was found in '{filePath}' ";
            string keyError = $"Key '{key}' wasnt found in '{filePath}' ";
            string pathError = $"Cant read data from the specified path: '{filePath}'";
            // Prüfe, ob die Datei existiert und lese sie als JObject
            JObject? jsonObject = await GetJObjectFromFile(filePath);
            if (jsonObject == null)
            {
                await xyLog.AsxLog(pathError);
                return null!;
            }
            if (jsonObject.TryGetValue(key, out JToken? token))
            {
                await xyLog.AsxLog(keyOk);
                return token;
            }
            else
            {
                await xyLog.AsxLog(keyError);
                return null!;
            }
        }

        public static async Task<string> GetStringFromJsonFile(string filePath, string key)
        {
            JToken? token = await GetJTokenFromJsonFile(filePath, key);
            if(token is not null)
            {
                return token.ToString();
            }
            return "";
        }

        public static async Task<object?> DeserializeFromKey(string filePath, string key)
        {
            string pathError = "Cant read data from the specified path: ";
            string serializingError = "Error in deserialization of the file ";
            string keyError = "No matching key for the given parameters!";

            if ((await File.ReadAllLinesAsync(filePath)).Count() == 0)
            {
                File.WriteAllText(filePath, AddRootTag(""));
            }

            if (await xyFiles.GetStreamFromFile(filePath) is MemoryStream memoryStream)
            {
                Dictionary<string, object>? jsonDic = await JsonSerializer.DeserializeAsync<Dictionary<string, object>>(memoryStream, defaultJsonOptions);

                if (jsonDic is null)
                {
                    xyLog.Log(serializingError + filePath);
                    return null;
                }
                else
                {
                    if (!jsonDic.ContainsKey(key))
                    {
                        xyLog.Log(keyError);
                        return null;
                    }
                    return jsonDic[key];
                }
            }
            xyLog.Log(pathError);
            return null;
        }
        public static async Task<Dictionary<string, object>> DeserializeKeyIntoDictionary(string fileName, string key)
        {
            Dictionary<string, object>? content = [];
            if (await GetJObjectFromFile(fileName) is JObject jsonObject)
            {
                if (jsonObject[key] is JObject targetValue)
                {
                    content = targetValue.ToObject<Dictionary<string, object>>();
                    return content;
                }
            }
            return content!;
        }


        public static async Task<object?> DeserializeSubKey(string filePath, string key, string subkey) => (await GetJObjectFromFile(filePath) is JObject jsonObject) ? (jsonObject[key][subkey] is object value) ? value : (async Task<object> () => { await xyLog.AsxLog("Cant read JObject into Object"); return null!; }) : null;
        public static async Task<Dictionary<string, object>?> DeserializeSubKeyToDictionary(string filePath, string key, string subkey) => (await GetJObjectFromFile(filePath) is JObject jsonObject) ? (jsonObject[key][subkey] is object value) ? value as Dictionary<string, object> : (await xyLog.AsxLog("Cant read JObject into Dictionary"), new Dictionary<string, object>()).Item2 : null;
        public static async Task<byte[]?> DeserializeSubKeyToBytes(string filePath, string key, string subkey) => (await GetJObjectFromFile(filePath) is JObject jsonObject) ? (xy.StringToBytes(jsonObject[key][subkey].ToString()) is byte[] value) ? value : (await xyLog.AsxLog("Cant read JObject into byte[]"), Array.Empty<byte>()).Item2 : null;


        public static async Task<byte[]?> DeserializeKeyToBytes(string filePath, string key)
        {
            try
            {
                IEnumerable<string> strings = await xyFiles.ReadIntoEnum(filePath);
                if (strings.Count() <= 4) return [];
                if (await GetJObjectFromFile(filePath) is JObject jsonObject)
                {
                    string keyString = jsonObject[key].ToString();
                    if (Convert.FromBase64String(keyString) is not byte[] value)
                    {
                        await xyLog.AsxLog($"Nothing to read for key{key}!");
                        return null!;
                    }
                    else
                    {
                        return value;
                    }
                }
            }
            catch (Exception ex)
            {
                await xyLog.AsxLog("Cant read JObject into byte[]");
            }
            return null!;
        }


        #endregion





        //public static Object GetAppsettingsJson()
        //{
        //      var developmentConfig = new ConfigurationBuilder().AddJsonFile("appsettings.Development.json").Build();

        //}

        //                            Deserialize from File!!!!!!!!!!!
        //{


    }
}
