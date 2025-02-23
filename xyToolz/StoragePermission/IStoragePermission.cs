using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xyToolz.StoragePermission
{
    public interface IStoragePermissionService
    {
        Task GetStoragePermissionsAsync();
    }
}
