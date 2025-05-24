using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace xyToolz.Database.Interfaces
{
    public interface IBaseCrud<T> where T : class
    {
        Task<bool> Create(T entity, [CallerMemberName] string? callerName = null);
        Task<T?> Read(int id, [CallerMemberName] string? callerName = null);
        Task<bool> Update(T entity, [CallerMemberName] string? callerName = null);
        Task<bool> Delete(T entity, [CallerMemberName] string? callerName = null);
    }
}
