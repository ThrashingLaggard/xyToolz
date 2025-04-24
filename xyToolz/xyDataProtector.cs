using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using xyToolz.Helper;
using xyToolz.Helper.Interfaces;
using static xyToolz.xyDataProtector;

namespace xyToolz
{


    /// <summary>
    /// Provides generic DPAPI-based encryption and decryption for local user scope.
    /// Only works on Windows systems.
    /// 
    /// <para><b>Internals:</b></para>
    /// - Uses Windows Data Protection API (DPAPI).
    /// - Scope: CurrentUser (user-specific encryption).
    /// - Only serializes and protects data locally.
    /// 
    /// <para><b>Available Features:</b></para>
    /// <list type="bullet">
    /// <item><description>Protect generic data objects using DPAPI.</description></item>
    /// <item><description>Unprotect to restore the original object.</description></item>
    /// <item><description>Supports string and file-based I/O.</description></item>
    /// </list>
    /// </summary>
    public static class xyDataProtector
    {

            private static IxyDataProtector? _override;
      
        /// <summary>
        /// Replaces the default implementation with a mocked version .
        /// </summary>
        public static void OverrideForTests(IxyDataProtector testDouble)
            => _override = testDouble;

        /// <summary>
        /// Resets to original implementation after testing.
        /// </summary>
        public static void ResetOverride()
            => _override = null;

        
        public static async Task<T?> UnprotectFromFileAsync<T>(string path, string key)
        {
            if (_override is not null)
            {
               return await  _override?.UnprotectFromFileAsync<T>(path, key);
            }
            else
            {
                if (await xyJson.DeserializeKeyToBytes(path, key) is byte[] encrypted)
                {
                    return await UnprotectAsync<T>(encrypted);
                }
            }
            return default;

        }




        /// <summary>
        /// Protects an object by serializing and encrypting it with DPAPI.
        /// </summary>
        public static async Task<byte[]> ProtectAsync<T>(T obj)
        {
            const string logNull = "Input object was null.";
            const string logSuccess = "Object was protected successfully.";
            const string logFail = "Failed to protect object.";

            if (obj == null)
            {
                await xyLog.AsxLog(logNull);
                return Array.Empty<byte>();
            }

            try
            {
                string json = JsonSerializer.Serialize(obj);
                byte[] raw = xy.StringToBytes(json);
                byte[] protectedData = ProtectedData.Protect(raw, null, DataProtectionScope.CurrentUser);

                await xyLog.AsxLog(logSuccess);
                return protectedData;
            }
            catch (Exception ex)
            {
                await xyLog.AsxExLog(ex);
                await xyLog.AsxLog(logFail);
                return Array.Empty<byte>();
            }
        }



        public static async Task<byte[]> ProtectString(string data) => await ProtectAsync(data);
        public static async Task<byte[]> ProtectBytes(byte[] data) => await ProtectAsync(data);



        /// <summary>
        /// Unprotects and deserializes a byte array into an object.
        /// </summary>
        public static async Task<T> UnprotectAsync<T>(byte[] encrypted)
        {
            string success = "Data successfully unprotected and deserialized.";
            string fail = "Unprotecting failed.";

            try
            {
                byte[] raw = ProtectedData.Unprotect(encrypted, null, DataProtectionScope.CurrentUser);
                string json = xy.BytesToString(raw);
                T? result = JsonSerializer.Deserialize<T>(json);

                await xyLog.AsxLog(success);
                return result;
            }
            catch (Exception ex)
            {
                await xyLog.AsxExLog(ex);
                await xyLog.AsxLog(fail);
                return default;
            }
        }

        public static async Task<string> UnprotectStringAsync(byte[] protectedData) => await UnprotectAsync<string>(protectedData);
        public static async Task<byte[]> UnprotectBytesAsync(byte[] protectedData) => await UnprotectAsync<byte[]>(protectedData);


        public static async Task<bool> SaveProtectedToFileAsync<T>(T obj, string filename = "secret.md")
        {
             string fail = "Cant save data to file.";
             string success = "Protected data has beeen written to the file.";

            try
            {
                byte[] encBytes = await ProtectAsync(obj);
                if (encBytes.Length == 0) return false;
                else
                {
                    string encString = xy.BytesToBase(encBytes);
                    await xyFiles.SaveToFile(encString, filename);
                    await xyLog.AsxLog(success);
                    return true;
                }
            }
            catch (Exception ex)
            {
                await xyLog.AsxExLog(ex);
                await xyLog.AsxLog(fail);
                return false;
            }
        }


        public static async Task<bool> SaveProtectedToFileAsync<T>(T obj, string subfolder = "HyperSecret", string filename = "secret.md")
        {
            if( xyPathHelper.Combine(subfolder, filename) is string fullPath && fullPath.Length > 3)    // Mucho intelligento
            {
                return await SaveProtectedToFileAsync(obj, fullPath);
            }
            return false;
        }

        /// <summary>
        /// Loads and decrypts an object from a protected file. 
        /// </summary>
        public static async Task<T?> LoadProtectedFromFileAsync<T>( string filename = "secret.bin")
        {
            string missing = "File not found or empty.";
            string fail = "Failed to load or decrypt protected file.";
            string success = "Protected data loaded and decrypted.";

            try
            {
                byte[]? encrypted = await xyFiles.LoadBytesFromFile(filename);

                if (encrypted == null || encrypted.Length == 0)
                {
                    await xyLog.AsxLog(missing);
                    return default;
                }

                T? obj = await UnprotectAsync<T>(encrypted);
                await xyLog.AsxLog(success);
                return obj;
            }
            catch (Exception ex)
            {
                await xyLog.AsxExLog(ex);
                await xyLog.AsxLog(fail);
                return default;
            }
        }
        public static async Task<T?> LoadProtectedFromFileAsync<T>(string subfolder = "UltraSecret", string filename = "secret.bin")
        {
                if (xyPathHelper.Combine(subfolder, filename)  is string path)
                {
                    return await LoadProtectedFromFileAsync<T>(path);   
                }
            return default;
        }


        public static async Task<bool> ProtectFileAsync<T>(string filePath)
        {
            IEnumerable<string> lines= await xyFiles.ReadLinesAsync(filePath);
            if (lines.Any())
            {
                string content = xyColQol.Spill(lines);
                byte[] data =  await ProtectString(content);
                return await xyFiles.SaveBytesToFileAsync(data, filePath);
            }
            return false;
        }



        
    }
}