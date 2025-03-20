using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xyToolz
{
   public static class xyRsa
    {


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
                  Dictionary<string , object>? keyValuePairsFromJsonFile = await xyJson.DeserializeFromFile(filePath);

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
                        await xyJson.SerializeDictionary(filePath , updatedDictionary);
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
                        await xyJson.SerializeDictionary(filePath , updatedDictionary);
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


      }
}
