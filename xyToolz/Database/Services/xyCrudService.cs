using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using xyToolz.Database.Interfaces;
using xyToolz.Database.Repos;

namespace xyToolz.Database.Services
{
    /// <summary>
    /// The logic surrounding the CrudRepository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class xyCrudService<T>(xyCrudRepo<T> crudRepository) : IExtendedCrud<T> where T : class
    {

        /// <summary>
        /// Providing access to the target DB
        /// </summary>
        private readonly xyCrudRepo<T> _crudRepository = crudRepository;
       
        #region "CRUD"
        /// <summary>
        /// Gives a new entity to the CrudRepo 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Task<bool> Create(T entity, [CallerMemberName] string? callerName = null)
        {
            return _crudRepository.Create(entity);
        }

        /// <summary>
        /// Calls Read(id) from CRUD-Repo
        /// </summary>
        /// <param name="id"></param>
        /// <param name="callerName"></param>
        /// <returns></returns>
        public Task<T?> Read(int id, [CallerMemberName] string? callerName = null)
        {
            return _crudRepository.Read(id);
        }

        /// <summary>
        /// Gives new data for the target entity to the CrudRepo 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Task<bool> Update(T entity, [CallerMemberName] string? callerName = null)
        {
            return _crudRepository.Update(entity);
 
        }

        /// <summary>
        /// Give the target to the CrudRepo to delete it
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Task<bool> Delete(T entity, [CallerMemberName] string? callerName = null)
        {
            return _crudRepository.Delete(entity);
        }

        #endregion

        #region "Extensions"
        /// <summary>
        /// Calls GetAll() from the CrudRepo
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<T?>> GetAll([CallerMemberName] string? callerName = null)
        {
            return _crudRepository.GetAll();
        }

        /// <summary>
        /// Calls Pageineering from CrudRepo
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="callerName"></param>
        /// <returns></returns>
        public Task<IEnumerable<T>> Pageineering(int page, int pageSize, [CallerMemberName] string? callerName = null)
        {
            return _crudRepository.Pageineering(page, pageSize);
        }
        #endregion

        #region "EF Core"


        /// <summary>
        /// Get a EntityEntry for the given ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EntityEntry> GetEntryByID(int id, [CallerMemberName] string? callerName = null)
        { 
            
            return await _crudRepository.GetEntryByID(id);
        }

        #endregion
    }
}
