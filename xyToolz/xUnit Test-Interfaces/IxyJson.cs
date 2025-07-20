using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xyToolz.Helper.Interfaces
{
    public interface IxyJson
    {
        Task<string?> GetStringFromJsonFile(string path, string key);
        Task AddOrUpdateEntry(string path, string key, string value);
        Task<Dictionary<string, object>?> DeserializeKeyIntoDictionary(string path, string key);

    }
}
