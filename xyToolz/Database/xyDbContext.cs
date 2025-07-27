using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using xyToolz.Helper.Logging;

namespace xyToolz.Database
{
    /// <summary>
    ///  A generic DbContext class providing common functionality for managing entities.
    ///  Can be used as a base class for specific database contexts.
    /// </summary>
    public class xyDBContext : DbContext
    {
        public String Description { get; set; } = "Generic DBcontext for easier interactions via EF-Core";

        public DbSet<object> Entities { get; set; }

        public String PathForDB { get; set; }

        private String _test = "";

        /// <summary>
        /// Initializes a new instance of the <see cref="xyDBContext"/> class.
        /// </summary>
        /// <param name="options">The options to be used by the DbContext.</param>
        public xyDBContext(DbContextOptions options) : base(options)
        {

        }

        /// <summary>
        /// Gets the DbSet for the specified type.
        /// </summary>
        /// <typeparam name="T">The type of entity.</typeparam>
        /// <returns>The DbSet for the specified entity type.</returns>
        public DbSet<T> GetDbSet<T>() where T : class
        {
            return Set<T>();
        }

        /// <summary>
        /// Finds an entity with the given primary key values.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="keyValues">The values of the primary key for the entity to be found.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the found entity or null.</returns>
        public async Task<T>? GetByID<T>(params object[] keyValues) where T : class
        {
            return await Set<T>().FindAsync(keyValues);
        }

        /// <summary>
        /// Adds the given entity to the context.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="entity">The entity to add.</param>
        public void AddEntity<T>(T entity) where T : class
        {
            Set<T>().Add(entity);
        }

        /// <summary>
        /// Updates the given entity in the context.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="entity">The entity to update.</param>
        public void UpdateEntity<T>(T entity) where T : class
        {
            Set<T>().Update(entity);
        }

        /// <summary>
        /// Removes the given entity from the context.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="entity">The entity to remove.</param>
        public void RemoveEntity<T>(T entity) where T : class
        {
            Set<T>().Remove(entity);
        }

        /// <summary>
        /// Gets an IQueryable for the specified entity type.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <returns>An IQueryable for the specified entity type.</returns>
        public IQueryable<T> Query<T>() where T : class
        {
            return Set<T>().AsQueryable();
        }

        /// <summary>
        /// Configures the model that was discovered by convention from the entity types exposed in DbSet properties on your derived context.
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            ApplyAllConfigurations(modelBuilder);
        }

        /// <summary>
        /// Applies configurations for all entity types in the assembly.
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
        private void ApplyAllConfigurations(ModelBuilder modelBuilder)
        {
            var applyGenericMethod = typeof(ModelBuilder).GetMethods().First(mb => mb.Name == "ApplyConfiguration" &&
            mb.GetParameters().First().ParameterType.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>));

            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                foreach (var iface in type.GetInterfaces())
                {
                    if (!iface.IsConstructedGenericType || iface.GetGenericTypeDefinition() != typeof(IEntityTypeConfiguration<>))
                        continue;

                    var entityType = iface.GenericTypeArguments[0];
                    var applyConcreteMethod = applyGenericMethod.MakeGenericMethod(entityType);
                    applyConcreteMethod.Invoke(modelBuilder, new[] { Activator.CreateInstance(type) });
                }
            }
        }







        internal void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {

            xyLog.Log("");
        }

        /// <summary>
        /// Get the full path for the local db within application folder
        /// </summary>
        /// <param name="pathDB"></param>
        /// <returns></returns>
        public String GetPathForLocalDbFromWithinApplicationFolder(String pathDB = @"\DB\xyLagerverwaltung.mdf")
        {
            try
            {
                String appFolder = xyDirUtils.GetInnerApplicationFolder();
                xyLog.Log(PathForDB = Path.Join(appFolder, pathDB));
            }
            catch (Exception ex)
            {
                xyLog.ExLog(ex);
            }
            return PathForDB;
        }









    }
}