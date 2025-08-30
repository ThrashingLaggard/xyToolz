# interface IxyDataProtector

Namespace: `xyToolz.Helper.Interfaces`  
Visibility: `public`  
Source: `xyToolz\xUnit Test-Interfaces\IxyDataProtector.cs`

## Description:

/// Interface used to allow mocking the xyDataProtector static class during unit tests.
    ///

## Methods

- `Task SaveProtectedToFileAsync(string content, string path)`
  
  /// Test
        ///
- `Task<byte[]> ProtectString(string content)`
  
  /// Test
        ///
- `Task<T?> UnprotectFromFileAsync<T>(string path, string key)`
  
  /// Test
        ///

