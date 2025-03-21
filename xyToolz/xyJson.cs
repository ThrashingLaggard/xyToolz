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
            
            };

            #region "Root Tag"
            public static async Task AddJsonRootTag(string filePath)
            {
                  try
                  { 
                        (string, string,string) first_last_content = await GetFirstAndLastLines(filePath);
                        bool isTaggedAsJson = await CheckForJsonRootTag(first_last_content);
                  
                        if (isTaggedAsJson)
                        {
                              await xyLog.AsxLog("File is already rooted to Json");
                              return;
                        }
                        else
                        {
                              string rooted = AddRootTag(first_last_content.Item3);
                              await File.WriteAllTextAsync(filePath,rooted);
                              await xyLog.AsxLog("Added a Json root tag to the file");
                        }               
                  }
                  catch (Exception ex)
                  {
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
                              if (lines.Count() == 0)
                                    return (null!, null!, null!);
                              else

                              if (lines.FirstOrDefault() is string firstLine && lines.LastOrDefault() is string lastLine)
                              {
                                    return (firstLine, lastLine, xyColQol.Spill(lines));
                              }
                              await xyLog.AsxLog(lineError);
                        }
                        await xyLog.AsxLog(fileError);
                  }
                  await xyLog.AsxLog(fileHandlingError);
                  return (null!, null!, null!);
            }
                  public static async Task<bool> CheckForJsonRootTag( (string, string,string) firstLine_LastLine )
            {
                        string firstLine = firstLine_LastLine.Item1;
                        string lastLine = firstLine_LastLine.Item2;
                        return firstLine.StartsWith('{') && lastLine.EndsWith('}');
            }
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
            private static void CreateNewJsonFile( string filePath )
            {
                  var json = new { };
                  var options = new JsonSerializerOptions { WriteIndented = true };
                  var jsonContent = JsonSerializer.Serialize(json , options);
                  File.WriteAllText(filePath , jsonContent);
            }

            /// <summary>
            /// Fügt einen neuen Schlüssel hinzu oder aktualisiert einen bestehenden Schlüssel in der JSON-Datei.
            /// </summary>
            /// <param name="filePath">Der Pfad zur JSON-Datei.</param>
            /// <param name="key">Der Schlüssel, der hinzugefügt oder aktualisiert werden soll.</param>
            /// <param name="value">Der Wert, der dem Schlüssel zugeordnet werden soll.</param>
            public static async Task AddOrUpdateEntry( string filePath , string key , object value )
            {
                  Dictionary<string , object>? keyValuePairsFromJsonFile = null;
                  string? updatedJsonContent = null;
                  try
                  {
                        if (xyFiles.EnsurePathExists(filePath))
                        {
                              keyValuePairsFromJsonFile = await DeserializeFromFile(filePath);
                              if (keyValuePairsFromJsonFile is not null)
                              {
                                    if (keyValuePairsFromJsonFile.ContainsKey(key))
                                    {
                                          keyValuePairsFromJsonFile[ key ] = value;
                                    }
                                    else
                                    {
                                          keyValuePairsFromJsonFile.Add(key , value);
                                    }
                              }
                        }
                        try
                        {
                              updatedJsonContent = JsonSerializer.Serialize(keyValuePairsFromJsonFile , defaultJsonOptions);
                        }
                        catch (JsonException jEx)
                        {
                              xyLog.ExLog(jEx);
                        }
                        await AddJsonRootTag(filePath);
                        await File.WriteAllTextAsync(filePath , updatedJsonContent);
                  }
                  catch (Exception ex)
                  {
                        xyLog.ExLog(ex);
                  }
            }
            #endregion


            #region "Serialization"

            /// <summary>
            /// 
            /// </summary>
            /// <param name="filePath"></param>
            /// <param name="updatedDictionary"></param>
            /// <returns></returns>
            public static async Task<bool> SerializeDictionary( string filePath , Dictionary<string , object> updatedDictionary )
            {
                  try
                  {
                        if (xyFiles.EnsurePathExists(filePath))
                        {
                              string updatedJsonContent = JsonSerializer.Serialize(updatedDictionary , defaultJsonOptions);
                              await File.WriteAllTextAsync(filePath , updatedJsonContent);
                              await xyLog.AsxLog($"{updatedDictionary.Count()} entrys are stored in {filePath}");
                              return true;
                        }
                  }
                  catch (JsonException jEx)
                  {
                        xyLog.ExLog(jEx);
                  }
                  return false;
            }

            public static async Task<Dictionary<string , object>?> DeserializeFromFile( string filePath ) => await JsonSerializer.DeserializeAsync<Dictionary<string , object>>(new MemoryStream(Encoding.ASCII.GetBytes(await File.ReadAllTextAsync(filePath))));

            public static object? DeserializeFromKey( string filePath , string key )
            {
                  string pathError = "No target file at the specified path: ";
                  string serializingError = "Error in deserialization of the file ";
                  string keyError = "No matching key for the given parameters!";

                  string? jsonContent = File.ReadAllText(filePath);
                  if (jsonContent is null)
                  {
                        xyLog.Log(pathError + filePath);
                        return null;
                  }

                  Dictionary<string , object>? jsonDic = JsonSerializer.Deserialize<Dictionary<string , object>>(jsonContent);
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
                        return jsonDic[ key ];
                  }
            }
            public static async Task<JObject?> GetJObjectFromFile( string filePath ) => xyFiles.EnsurePathExists(filePath) ? ( await File.ReadAllTextAsync(filePath) is string jsonFileContent ) ? JObject.Parse(jsonFileContent) : (await xyLog.AsxLog("Cant read file content into JObject"), new JObject[0].FirstOrDefault()).Item2 : null;
        
            public static async Task<object> DeserializeSubKey( string filePath , string key , string subkey ) =>  (await GetJObjectFromFile(filePath) is    JObject jsonObject )? ( jsonObject[ key ][ subkey ] is object value ) ? value : (async Task<object>() => { await xyLog.AsxLog("Cant read JObject into Object"); return null!; } ) :null; 
                        
            public static async Task<byte[]> DeserializeSubKeyToBytes( string filePath , string key , string subkey ) => ( await DeserializeSubKey(filePath , key , subkey) is byte[] value ) ? value : ( await xyLog.AsxLog("Cant read JObject into byte[]"), Array.Empty<byte>()).Item2; 

               
            

                            

                        

            #endregion





            //public static Object GetAppsettingsJson()
            //{
            //      var developmentConfig = new ConfigurationBuilder().AddJsonFile("appsettings.Development.json").Build();

            //}

            //                            Deserialize from File!!!!!!!!!!!
            //{
            //      try
            //      {
            //            string? jsonContent = await File.ReadAllText(filePath);
            //            MemoryStream mStream = new(Encoding.ASCII.GetBytes(jsonContent));
            //            if(jsonContent is not null)
            //            {
            //                  Dictionary<string , object>? jsonDic = await JsonSerializer.DeserializeAsync<Dictionary<string , object>>(mStream);
            //                  return jsonDic!;
            //            }
            //      }
            //      catch(Exception ex)
            //      {
            //            xyLog.ExLog(ex);
            //      }
            //      return null;
            //}

      }
}
