using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32.SafeHandles;

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
                              keyValuePairsFromJsonFile = await GetValuesFromJson(filePath);
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
                        await File.WriteAllTextAsync(filePath , updatedJsonContent);
                  }
                  catch (Exception ex)
                  {
                        xyLog.ExLog(ex);
                  }
            }
        /// <summary>
        /// Fügt einen neuen Schlüssel hinzu oder aktualisiert einen bestehenden Schlüssel in der JSON-Datei.
        /// </summary>
        /// <param name="filePath">Der Pfad zur JSON-Datei.</param>
        /// <param name="key">Der Schlüssel, der hinzugefügt oder aktualisiert werden soll.</param>
        /// <param name="value">Der Wert, der dem Schlüssel zugeordnet werden soll.</param>
        /// <param name="overwrite"></param>
        public static async Task AddOrUpdateRSAEntry( string filePath , string key , byte[] value , bool overwrite )
            {
                  try
                  {
                        if (overwrite)
                        {
                              await UpdateRsaEntry(filePath , key , value);
                              xyLog.Log(key + " overwritten");
                        }
                        else
                        {
                              await AddRsaEntry(filePath , key , value);
                              xyLog.Log(key + " added");
                        }
                  }
                  catch (Exception ex)
                  {
                        xyLog.ExLog(ex);
                  }
            }


            /// <summary>
            /// New RsaKey Entry in target file
            /// </summary>
            /// <param name="filePath"></param>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <returns></returns>
            public static async Task AddRsaEntry( string filePath , string key , byte[] value )
            {
                  (Dictionary<string , object>?, byte[]?) dictionary_SecretKeyBytes = await PrepareDictionaryAndKey(filePath , key , value);

                  if (await AddRsaKey(dictionary_SecretKeyBytes.Item1 , key , value) is not Dictionary<string , object> updatedDictionary)
                  {
                        xyLog.Log($"Cant add {key} to the {filePath} file");
                  }
                  else
                  {
                        await SerializeDictionary(filePath , updatedDictionary);
                  }
            }

            /// <summary>
            /// Update rsa key entry in json file
            /// </summary>
            /// <param name="filePath"></param>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <returns></returns>
            public static async Task UpdateRsaEntry( string filePath , string key , byte[] value )
            {
                  (Dictionary<string , object>?, byte[]?) dictionary_SecretKeyBytes = await PrepareDictionaryAndKey(filePath , key , value);

                  if (await UpdateRsaKey(dictionary_SecretKeyBytes.Item1 , key , value) is not Dictionary<string , object> updatedDictionary)
                  {
                        xyLog.Log($"Cant add {key} to the {filePath} file");
                  }
                  else
                  {
                        await SerializeDictionary(filePath , updatedDictionary);
                  }
            }




        /// <summary>
        ///                                                         ===v
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>A Dictionary and the entered byte[]</returns>
        private static async Task<(Dictionary<string , object>?, byte[]?)> PrepareDictionaryAndKey( string filePath , string key , byte[] value )
            {
                  string? updatedJsonContent = null;
                  Dictionary<string , object>? keyValuePairsFromJsonFile = await GetValuesFromJson(filePath);

                  if (value is not byte[] secretKeyFromRsaValue)
                  {
                        xyLog.Log("Unable to read key from parameter");
                        return (keyValuePairsFromJsonFile, null);
                  }
                  else
                  {
                        secretKeyFromRsaValue = value;
                        return (keyValuePairsFromJsonFile, secretKeyFromRsaValue);
                  }

            }

            /// <summary>
            /// Zielschlüssel überschreiben
            /// </summary>
            /// <param name="keyValuePairsFromJsonFile"></param>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <returns></returns>
            private static async Task<Dictionary<string , object>?> UpdateRsaKey( Dictionary<string , object> keyValuePairsFromJsonFile , string key , byte[] value )
            {
                  try
                  {
                        if (keyValuePairsFromJsonFile is not null)
                        {
                              if (keyValuePairsFromJsonFile.ContainsKey(key))
                              {
                                    keyValuePairsFromJsonFile[ key ] = value;
                                    xyLog.Log(key + " overwritten");
                                    return keyValuePairsFromJsonFile;
                              }
                        }
                  }
                  catch (Exception ex)
                  {
                        xyLog.ExLog(ex);
                  }
                  return null;
            }
            /// <summary>
            /// Fügt einen neuen Schlüssel in der JSON-Datei hinzu.
            /// </summary>
            /// <param name="keyValuePairsFromJsonFile"></param>
            /// <param name="key">Der Schlüssel, der hinzugefügt werden soll.</param>
            /// <param name="value">Der Wert, der dem Schlüssel zugeordnet werden soll.</param>
            private static async Task<Dictionary<string , object>?> AddRsaKey( Dictionary<string , object> keyValuePairsFromJsonFile , string key , byte[] value )
            {
                  try
                  {
                        if (keyValuePairsFromJsonFile is Dictionary<string , object> keyValuePairs)
                        {
                              if (!keyValuePairs.ContainsKey(key))
                              {
                                    keyValuePairs.Add(key , value);
                                    xyLog.Log(key + " added");
                              }
                              return keyValuePairs;
                        }
                  }
                  catch (Exception ex)
                  {
                        xyLog.ExLog(ex);
                  }
                  return null;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="filePath"></param>
            /// <param name="updatedDictionary"></param>
            /// <returns></returns>
            private static async Task<bool> SerializeDictionary( string filePath , Dictionary<string , object> updatedDictionary )
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








            //public static Object GetAppsettingsJson()
            //{
            //      var developmentConfig = new ConfigurationBuilder().AddJsonFile("appsettings.Development.json").Build();

            //}


            public static async Task<Dictionary<string , object>?> GetValuesFromJson( string filePath ) => await JsonSerializer.DeserializeAsync<Dictionary<string , object>>(new MemoryStream(Encoding.ASCII.GetBytes(await File.ReadAllTextAsync(filePath))));
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

            public static object? GetValuesFromJsonKey( string filePath , string key )
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


      }
}
