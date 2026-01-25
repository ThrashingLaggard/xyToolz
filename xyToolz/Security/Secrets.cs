namespace xyToolz.Security;

public static class Secrets
{
    // Data

    public static Task<byte[]> ProtectAsync<T>(T value)
        => xyDataProtector.ProtectAsync(value);

    public static Task<T> UnprotectAsync<T>(byte[] data)
        => xyDataProtector.UnprotectAsync<T>(data);

    // Files 

    public static Task<bool> SaveAsync<T>(T value, string path)
        => xyDataProtector.SaveProtectedToFileAsync(value, path);

    public static Task<T?> LoadAsync<T>(string path)
        => xyDataProtector.LoadProtectedFromFileAsync<T>(path);

    public static Task<T?> LoadFromKeyAsync<T>(string path, string key)
        => xyDataProtector.UnprotectFromFileAsync<T>(path, key);

    public static Task<bool> ProtectFileAsync(string filePath)
        => xyDataProtector.ProtectFileAsync<object>(filePath);
}
