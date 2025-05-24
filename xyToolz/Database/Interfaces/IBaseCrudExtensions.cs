using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace xyToolz.Database.Interfaces
{
    /// <summary>
    /// Extension for the IBaseCrud interface 
    /// providing :
    /// 
    /// GetAll()
    /// 
    /// Pageineering(x,y)
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBaseCrudExtensions<T> where T : class
    {
        /// <summary>
        /// Get all instances
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<IEnumerable<T?>> GetAll([CallerMemberName] string? callerName = null);
        
        /// <summary>
        /// Filter the entries to be shown
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> Pageineering(int page, int pageSize, [CallerMemberName] string? callerName = null);
    }
}
