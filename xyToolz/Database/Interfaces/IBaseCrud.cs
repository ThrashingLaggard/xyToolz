using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xyToolz.Database.Interfaces
{
    public interface IBaseCrud<T> where T : class
    {
        Task<bool> Create(T Entity);
        Task<T?> Read<T>(int id);
        Task<bool> Update(T Entity);
        Task<bool> Delete(T Entity);
    }
}
