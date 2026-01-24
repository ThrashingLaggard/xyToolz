using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Runtime.CompilerServices;
using xyToolz.Database.Interfaces;
using xyToolz.Helper.Logging;
using xyToolz.StaticLogging;

namespace xyToolz.Database.Repos
{
    /// <summary>
    /// Generic Repository for extended CRUD using EF Core
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="context"></param>
    public class xyCrudRepo<T>(DbContext context) : IExtendedCrud<T> where T : class
    {
        /// <summary>
        /// Black magic to service incoming DB contexts
        /// </summary>
        private readonly dynamic _context = context;

        #region "CRUD"
        /// <summary>
        /// Create a new entry in the database using the given data
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="callerName"></param>
        /// <returns></returns>
        public async Task<bool> Create(T entity, [CallerMemberName] string? callerName = null)
        {
            bool isCreated = false;
            string invalidParam = "Please enter valid data for the creation!";
            string created = "Created and added the entry for the given data";

            if (entity == null || entity.Equals(""))
            {
                isCreated = false;
                await xyLog.AsxLog(invalidParam);
            }
            else
            {
                try
                {
                    if (_context.Add(entity) is EntityEntry entityEntry)
                    {
                        await _context.SaveChangesAsync();
                        await xyLog.AsxLog(created);
                        isCreated = true;
                    }
                }
                catch (Exception ex)
                {
                    await xyLog.AsxExLog(ex);
                }
            }
            return isCreated; 
        }

        /// <summary>
        /// Get the corresponding instance for the given id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="callerName"></param>
        /// <returns></returns>
        public async Task<T?> Read(int id, [CallerMemberName] string? callerName = null)
        {
            object[] param = { id };
            dynamic? target = default!;
            string nothingFound = $"Couldnt find the corresponding entries for the ID {id}";

            try
            {
                target = await _context.FindAsync<T>(param);
                if (target is null)
                {
                    await xyLog.AsxLog(nothingFound);
                }
            }
            catch (Exception ex)
            {
                xyLog.ExLog(ex);
            }
            return target!;
        }

        /// <summary>
        /// Updating the target entry
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="callerName"></param>
        /// <returns></returns>
        public async Task<bool> Update(T entity, [CallerMemberName] string? callerName = null)
        {
            bool isUpdated = false;

            if (entity is null)
            {
                return isUpdated;
            }
            try
            {
                _context.Update(entity);
                await _context.SaveChangesAsync();
                isUpdated = true;
            }
            catch (Exception ex)
            {
                xyLog.ExLog(ex);
            }
            return isUpdated;
        }

        /// <summary>
        /// Removing an entry from the database
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="callerName"></param>
        /// <returns></returns>
        public async Task<bool> Delete(T entity, [CallerMemberName] string? callerName = null)
        {
            string success = $"Entry with was removed";
            bool isDeleted = false;

            try
            {
                _context.Remove(entity);
                await _context.SaveChangesAsync();
                isDeleted = true;
            }
            catch (Exception ex)
            {
                xyLog.ExLog(ex);
            }
            await xyLog.AsxLog(success);
            return isDeleted;
        }

        #endregion

        #region "Extensions"
        /// <summary>
        /// Return an IEnumerable filled with the content of the target table
        /// </summary>
        /// <param name="callerName"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T?>> GetAll([CallerMemberName] string? callerName = null)
        {
            string noEntrys = "No elements in list, please inform experts immedeately!";
            IEnumerable<T> inventory = [];

            try
            {
                inventory = await _context.Set<T>().ToListAsync();

                if (!inventory.Any())
                {
                   await xyLog.AsxLog(noEntrys);
                }
            }
            catch (Exception ex)
            {
                await xyLog.AsxExLog(ex);
            }
            return inventory;
        }

        /// <summary>
        /// Select which and how many elements to return
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="callerName"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> Pageineering(int page, int pageSize, [CallerMemberName] string? callerName = null)
        { 
            IEnumerable<T> paginatedList = Enumerable.Empty<T>();
            string noEntries = "No entries found for the specified page.";
            string wrongParams = "Page and pageSize must be greater than 0.";


            if (page < 1 || pageSize < 1)
            {
                await xyLog.AsxExLog( new ArgumentException(wrongParams));
            }

            try
            {
                // Calculate the number of items to skip
                int skip = (page - 1) * pageSize;

                // Get the total count of items
                int totalCount = await _context.Set<T>().CountAsync();

                // Fetch the paginated data
                paginatedList = await _context.Set<T>().Skip(skip) .Take(pageSize).ToListAsync();
                    

                // If still empty
                if (!paginatedList.Any())
                {
                    await xyLog.AsxLog(noEntries);
                }
            }
            catch (Exception ex)
            {
                await xyLog.AsxExLog(ex);
            }
            return paginatedList;
        }

        #endregion


        #region "EF Core"
        /// <summary>
        /// Return a EntityEntry instance for the given id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="callerName"></param>
        /// <returns></returns>
        public async Task<EntityEntry> GetEntryByID(int id, [CallerMemberName] string? callerName = null)
        {
            EntityEntry entity = default!;
            dynamic? target = await  Read(id);

            try
            {
                entity = _context.Entry(target);
            }
            catch (Exception ex)
            {
                await xyLog.AsxExLog(ex);
            }
            return entity!;
        }

        #endregion


    }
}
