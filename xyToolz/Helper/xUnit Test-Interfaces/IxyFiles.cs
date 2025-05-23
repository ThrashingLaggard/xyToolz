namespace xyToolz.Helper.Interfaces
{
    /// <summary>
    /// Interface for testing and mocking file operations in xyFiles.
    /// </summary>
    public interface IxyFiles
    {
        Task<string?> LoadFileAsync(string subfolder, string fileName);
        Task<string?> LoadFileAsync(string fullPath);
        
    }
}
