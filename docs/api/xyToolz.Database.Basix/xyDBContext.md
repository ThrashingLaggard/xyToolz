# class xyDBContext

Namespace: `xyToolz.Database.Basix`  
Visibility: `public`  
Base/Interfaces:`DbContext`  
Source: `xyToolz\Database\xyDBContext.cs`

## Description:

///  A generic DbContext class providing common functionality for managing entities.
    ///  Can be used as a base class for specific database contexts.
    ///

## Constructors

- `xyDBContext(DbContextOptions options)` — `public`
  
  /// Initializes a new instance of the  class.
        ///

## Properties

- `DbSet<object> Entities{ get; set; }` — `public`
  
  (No XML‑Summary )
- `String Description{ get; set; }` — `public`
  
  (No XML‑Summary )
- `String? PathForDB{ get; set; }` — `public`
  
  (No XML‑Summary )

## Methods

- `DbSet<T> GetDbSet<T>()` — `public`
  
  /// Gets the DbSet for the specified type.
        ///
- `IQueryable<T> Query<T>()` — `public`
  
  /// Gets an IQueryable for the specified entity type.
        ///
- `String GetPathForLocalDbFromWithinApplicationFolder(String pathDB = @"\DB\xyLagerverwaltung.mdf")` — `public`
  
  /// Get the full path for the local db within application folder
        ///
- `Task<T?>? GetByID<T>(params object[] keyValues)` — `public async`
  
  /// Finds an entity with the given primary key values.
        ///
- `void AddEntity<T>(T entity)` — `public`
  
  /// Adds the given entity to the context.
        ///
- `void ApplyAllConfigurations(ModelBuilder modelBuilder)` — `private`
  
  /// Applies configurations for all entity types in the assembly.
        ///
- `void OnModelCreating(ModelBuilder modelBuilder)` — `protected override`
  
  /// Configures the model that was discovered by convention from the entity types exposed in DbSet properties on your derived context.
        ///
- `void OnModelCreatingPartial(ModelBuilder modelBuilder)` — `internal`
  
  (No XML‑Summary )
- `void RemoveEntity<T>(T entity)` — `public`
  
  /// Removes the given entity from the context.
        ///
- `void UpdateEntity<T>(T entity)` — `public`
  
  /// Updates the given entity in the context.
        ///

