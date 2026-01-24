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
        /// <summary>
        /// Test
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<T?> UnprotectFromFileAsync<T>(string path, string key);
        /// <summary>
        /// Test
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        Task<byte[]> ProtectString(string content);
        /// <summary>
        /// Test
        /// </summary>
        /// <param name="content"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        Task SaveProtectedToFileAsync(string content, string path);

    }
}
