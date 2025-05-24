using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xyToolz.Database.Interfaces;

namespace xyToolz.Database.Repos
{
    public class CrudRepo<T>(DbContext context) : IExtendedCrud<T> where T : class
    {
        public Task<bool> Create(T Entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(T Entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<EntityEntry> GetEntryByID(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> Pageineering(int page, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<T1> Read<T1>(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(T Entity)
        {
            throw new NotImplementedException();
        }
    }
}
