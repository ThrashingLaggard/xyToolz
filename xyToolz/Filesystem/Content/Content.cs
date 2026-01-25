namespace xyToolz.Filesystem;

public static class FileContent
{
    public static Task<IEnumerable<string>> ReadLinesAsync(string filePath)
        => xyFiles.ReadLinesAsync(filePath);

    public static Task<Stream?> GetStreamFromFileAsync(string filePath)
        => xyFiles.GetStreamFromFileAsync(filePath);

    public static Task<bool> SaveToFile(string content, string filePath = "config.json")
        => xyFiles.SaveToFile(content, filePath);

    public static Task<bool> SaveBytesToFileAsync(byte[] data, string filePath = "config.json")
        => xyFiles.SaveBytesToFileAsync(data, filePath);

    public static Task<bool> SaveStringToFileAsync(string content, string subfolder = "AppData", string fileName = "config.json")
        => xyFiles.SaveStringToFileAsync(content, subfolder, fileName);

    public static Task<string?> LoadFileAsync(string subfolder = "AppData", string fileName = "config.json")
        => xyFiles.LoadFileAsync(subfolder, fileName);

    public static Task<string?> LoadFileAsync(string fileName)
        => xyFiles.LoadFileAsync(fileName);

    public static Task<byte[]?> LoadBytesFromFile(string fullPath)
        => xyFiles.LoadBytesFromFile(fullPath);

    public static Task<byte[]?> ReadBytes(string fullPath)
        => xyFiles.ReadBytes(fullPath);

    public static bool DeleteFile(string subfolder = "AppData", string fileName = "config.json")
        => xyFiles.DeleteFile(subfolder, fileName);
}
