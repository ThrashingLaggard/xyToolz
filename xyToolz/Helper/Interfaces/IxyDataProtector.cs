using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xyToolz.Helper.Interfaces
{
    /// <summary>
    /// Interface used to allow mocking the xyDataProtector static class during unit tests.
    /// </summary>
    public interface IxyDataProtector
    {
        Task<T?> UnprotectFromFileAsync<T>(string path, string key);
        Task<byte[]> ProtectString(string content);
        Task SaveProtectedToFileAsync(string content, string path);

    }
}
