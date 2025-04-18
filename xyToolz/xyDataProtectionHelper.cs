using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using xyToolz.Helper;

namespace xyToolz;

/// <summary>
/// Provides encryption and decryption helpers using AES (Advanced Encryption Standard).
///
/// <para><b>Available Features:</b></para>
/// <list type="bullet">
///   <item><description>Generic AES encryption and decryption via CBC mode with PKCS7 padding.</description></item>
///   <item><description>Encrypt and decrypt generic objects with JSON serialization.</description></item>
///   <item><description>Utility functions for key-based encryption of strings.</description></item>
/// </list>
///
/// <para><b>Thread Safety:</b></para>
/// Fully threadsafe – all methods are static and use local variables only.
///
/// <para><b>Limitations:</b></para>
/// Caller is responsible for secure key and salt management.
///
/// <para><b>Performance:</b></para>
/// Optimized for small to medium payloads; uses memory streams and async I/O.
///
/// <para><b>Related:</b></para>
/// <see cref="xyHashHelper"/> for key derivation
/// <see cref="xyJson"/> for JSON configuration
/// </summary>
public static class xyDataProtectionHelper
{
    /// <summary>
    /// Asynchronously encrypts a generic object using AES encryption and returns the result as a byte array.
    /// </summary>
    public static async Task<byte[]?> EncryptAsync<T>(T obj, string password, byte[] salt)
    {
        const string logStart = "Starting AES encryption process...";
        const string logNullInput = "The object to encrypt is null.";
        const string logKeyFail = "Key derivation failed or returned invalid key.";
        const string logError = "An error occurred during encryption.";
        const string logSuccess = "Object was successfully encrypted.";

        await xyLog.AsxLog(logStart);

        if (obj is null)
        {
            await xyLog.AsxLog(logNullInput);
            return null;
        }

        try
        {
            string json = JsonSerializer.Serialize(obj, xyJson.defaultJsonOptions);
            byte[] jsonBytes = Encoding.UTF8.GetBytes(json);
            byte[] key = xyHashHelper.BuildKeyFromPassword(password, salt);

            if (key is null || key.Length != 32)
            {
                await xyLog.AsxLog(logKeyFail);
                return null;
            }

            using Aes aes = Aes.Create();
            aes.Key = key;
            aes.GenerateIV();
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using MemoryStream ms = new();
            using CryptoStream cs = new(ms, aes.CreateEncryptor(), CryptoStreamMode.Write);
            await cs.WriteAsync(jsonBytes);
            await cs.FlushAsync();
            cs.Close();

            byte[] encrypted = ms.ToArray();
            byte[] result = new byte[aes.IV.Length + encrypted.Length];
            Buffer.BlockCopy(aes.IV, 0, result, 0, aes.IV.Length);
            Buffer.BlockCopy(encrypted, 0, result, aes.IV.Length, encrypted.Length);

            await xyLog.AsxLog(logSuccess);
            return result;
        }
        catch (Exception ex)
        {
            await xyLog.AsxExLog(ex);
            await xyLog.AsxLog(logError);
            return null;
        }
    }

    /// <summary>
    /// Asynchronously decrypts AES-encrypted data (prefixed with IV) and deserializes it to a generic object of type T.
    /// </summary>
    public static async Task<T?> DecryptAsync<T>(byte[] encryptedData, string password, byte[] salt)
    {
        const string logStart = "Starting AES decryption process...";
        const string logInvalid = "Encrypted data is null or too short.";
        const string logKeyFail = "Key derivation failed or returned invalid key.";
        const string logError = "An error occurred during decryption.";
        const string logSuccess = "Decryption completed and data successfully deserialized.";

        await xyLog.AsxLog(logStart);

        if (encryptedData is null || encryptedData.Length < 17)
        {
            await xyLog.AsxLog(logInvalid);
            return default;
        }

        try
        {
            byte[] iv = encryptedData[..16];
            byte[] ciphertext = encryptedData[16..];
            byte[] key = xyHashHelper.BuildKeyFromPassword(password, salt);

            if (key is null || key.Length != 32)
            {
                await xyLog.AsxLog(logKeyFail);
                return default;
            }

            using Aes aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using MemoryStream msDecrypt = new(ciphertext);
            using CryptoStream cs = new(msDecrypt, aes.CreateDecryptor(), CryptoStreamMode.Read);
            using StreamReader sr = new(cs);
            string decryptedJson = await sr.ReadToEndAsync();

            await xyLog.AsxLog(logSuccess);
            return JsonSerializer.Deserialize<T>(decryptedJson, xyJson.defaultJsonOptions);
        }
        catch (Exception ex)
        {
            await xyLog.AsxExLog(ex);
            await xyLog.AsxLog(logError);
            return default;
        }
    }

    /// <summary>
    /// Encrypts a plain text string using a symmetric key and AES.
    /// </summary>
    public static async Task<byte[]> ProtectAsync(string plainText, byte[] key)
    {
        const string errorEmptyInput = "Either plain text or key was null or empty.";
        const string errorEncryption = "Encryption failed.";
        const string successMsg = "Text encrypted successfully.";

        if (string.IsNullOrWhiteSpace(plainText) || key is null || key.Length == 0)
        {
            await xyLog.AsxLog(errorEmptyInput);
            return Array.Empty<byte>();
        }

        try
        {
            using Aes aes = Aes.Create();
            aes.Key = key;
            aes.GenerateIV();

            await using MemoryStream ms = new();
            await ms.WriteAsync(aes.IV);
            await using CryptoStream cs = new(ms, aes.CreateEncryptor(), CryptoStreamMode.Write);
            await using StreamWriter sw = new(cs);
            await sw.WriteAsync(plainText);

            await xyLog.AsxLog(successMsg);
            return ms.ToArray();
        }
        catch (Exception ex)
        {
            await xyLog.AsxExLog(ex);
            await xyLog.AsxLog(errorEncryption);
            return Array.Empty<byte>();
        }
    }

    /// <summary>
    /// Decrypts a byte array (containing IV + cipher text) to a UTF8 string using the provided key.
    /// </summary>
    public static async Task<string> UnprotectAsync(byte[] cipherData, byte[] key)
    {
        const string errorEmptyInput = "Either cipher data or key was null or empty.";
        const string errorTooShort = "Encrypted data does not contain a valid IV.";
        const string errorDecryption = "Decryption failed.";
        const string successMsg = "Text decrypted successfully.";

        if (cipherData is null || cipherData.Length == 0 || key is null || key.Length == 0)
        {
            await xyLog.AsxLog(errorEmptyInput);
            return string.Empty;
        }

        try
        {
            using Aes aes = Aes.Create();
            aes.Key = key;

            int ivLength = aes.BlockSize / 8;
            if (cipherData.Length < ivLength)
            {
                await xyLog.AsxLog(errorTooShort);
                return string.Empty;
            }

            byte[] iv = cipherData[..ivLength];
            byte[] cipherText = cipherData[ivLength..];
            aes.IV = iv;

            await using MemoryStream ms = new(cipherText);
            await using CryptoStream cs = new(ms, aes.CreateDecryptor(), CryptoStreamMode.Read);
            using StreamReader sr = new(cs);
            string plainText = await sr.ReadToEndAsync();

            await xyLog.AsxLog(successMsg);
            return plainText;
        }
        catch (Exception ex)
        {
            await xyLog.AsxExLog(ex);
            await xyLog.AsxLog(errorDecryption);
            return string.Empty;
        }
    }
}
