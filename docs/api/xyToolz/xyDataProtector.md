# class xyDataProtector

Namespace: `xyToolz`  
Visibility: `public static`  
Source: `xyToolz\xyDataProtector.cs`

## Description:

/// Provides generic DPAPI-based encryption and decryption for local user scope.
    /// Only works on Windows systems.
    /// 
    /// 
Internals:

    /// - Uses Windows Data Protection API (DPAPI).
    /// - Scope: CurrentUser (user-specific encryption).
    /// - Only serializes and protects data locally.
    /// 
    /// 
Available Features:

    /// 
    /// Protect generic data objects using DPAPI.
    /// Unprotect to restore the original object.
    /// Supports string and file-based I/O.
    /// 
    ///

## Methoden

- `Task<bool> ProtectFileAsync<T>(string filePath)` — `public static async`
  
  /// Protect a file with windows DPAPI
        ///
- `Task<bool> SaveProtectedToFileAsync<T>(T obj, string filename = "secret.md")` — `public static async`
  
  /// Save protected data to specific path
        ///
- `Task<bool> SaveProtectedToFileAsync<T>(T obj, string subfolder = "HyperSecret", string filename = "secret.md")` — `public static async`
  
  /// Save protected data to specific path
        ///
- `Task<byte[]> ProtectAsync<T>(T obj)` — `public static async`
  
  /// Protects an object by serializing and encrypting it with DPAPI.
        ///
- `Task<byte[]> ProtectBytes(byte[] data)` — `public static async`
  
  /// Byte wrapper for generic method
        ///
- `Task<byte[]> ProtectString(string data)` — `public static async`
  
  /// String wrapper for generic method
        ///
- `Task<byte[]> UnprotectBytesAsync(byte[] protectedData)` — `public static async`
  
  /// Byte-Wrapper for generic method
        ///
- `Task<string> UnprotectStringAsync(byte[] protectedData)` — `public static async`
  
  /// String-Wrapper for generic method
        ///
- `Task<T?> LoadProtectedFromFileAsync<T>( string filename = "secret.bin")` — `public static async`
  
  /// Loads and decrypts an object from a protected file. 
        ///
- `Task<T?> LoadProtectedFromFileAsync<T>(string subfolder = "UltraSecret", string filename = "secret.bin")` — `public static async`
  
  /// Loads and decrypts an object from a protected file. 
        ///
- `Task<T?> UnprotectFromFileAsync<T>(string path, string key)` — `public static async`
  
  /// Unprotect and read the values from a key from a file
        ///
- `Task<T> UnprotectAsync<T>(byte[] encrypted)` — `public static async`
  
  /// Unprotects and deserializes a byte array into an object.
        ///
- `void OverrideForTests(IxyDataProtector testDouble)` — `public static`
  
  /// Replaces the default implementation with a mocked version .
        ///
- `void ResetOverride()` — `public static`
  
  /// Resets to original implementation after testing.
        ///

