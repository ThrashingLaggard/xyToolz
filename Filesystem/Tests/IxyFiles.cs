namespace xyToolz.Helper.Interfaces
{
    /// <summary>
    /// Interface for testing and mocking file operations in xyFiles.
    /// </summary>
    public interface IxyFiles
    {
        /// <summary>
        /// Test
        /// </summary>
        /// <param name="subfolder"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        Task<string?> LoadFileAsync(string subfolder, string fileName);
        /// <summary>
        /// Test
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        Task<string?> LoadFileAsync(string fullPath);
        
    }
}
