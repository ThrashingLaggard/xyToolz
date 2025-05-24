using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace xyToolz.Database.Interfaces
{
    /// <summary>
    /// Providing EF Core extensions for IBaseCrud
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IEfCoreCrudExtensions<T> where T : class
    {
        Task<EntityEntry> GetEntryByID(int id);
    }
}
