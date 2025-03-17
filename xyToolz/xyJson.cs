using System.Text.Json;
using System.Text.Json.Serialization;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace xyToolz
{
     public class xyJson
    {
            internal static readonly JsonSerializerOptions defaultJsonOptions = new()
            {
                  WriteIndented = true ,
                  DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };



            /// <summary>
            /// Fügt einen neuen Schlüssel hinzu oder aktualisiert einen bestehenden Schlüssel in der JSON-Datei.
            /// </summary>
            /// <param name="filePath">Der Pfad zur JSON-Datei.</param>
            /// <param name="key">Der Schlüssel, der hinzugefügt oder aktualisiert werden soll.</param>
            /// <param name="value">Der Wert, der dem Schlüssel zugeordnet werden soll.</param>
            public static async void AddOrUpdateJsonEntry( string filePath , string key , object value )
            {
                  Dictionary<string, object>? keyValuePairsFromJsonFile = null;
                  try
                  {
                        if (xyFiles.EnsurePathExists(filePath))
                        {
                             keyValuePairsFromJsonFile = GetValuesFromJson(filePath);
                              if(keyValuePairsFromJsonFile is not null)
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
                        string? updatedJsonContent = JsonSerializer.Serialize(keyValuePairsFromJsonFile , defaultJsonOptions);
                       await File.WriteAllTextAsync(filePath,updatedJsonContent);
                  }
                  catch (Exception ex)
                  {
                        xyLog.ExLog(ex);
                  }
            }


            //public static Object GetAppsettingsJson()
            //{
            //      var developmentConfig = new ConfigurationBuilder().AddJsonFile("appsettings.Development.json").Build();
                  
            //}
            

            public static Dictionary<string , object>? GetValuesFromJson( string filePath )
            {
                  string? jsonContent = File.ReadAllText(filePath);
                  Dictionary<string , object>? jsonDic = JsonSerializer.Deserialize<Dictionary<string , object>>(jsonContent);
                  return jsonDic!;
            }

            public static object? GetValuesFromJsonKey( string filePath, string key)
            {
                  string pathError = "No target file at the specified path: ";
                  string serializingError = "Error in deserialization of the file ";
                  string keyError = "No matching key for the given parameters!";

                  string? jsonContent = File.ReadAllText(filePath);
                  if (jsonContent is null )
                  {
                        xyLog.Log( pathError+ filePath);
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
