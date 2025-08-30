# class xyCrudService<T>

Namespace: `xyToolz.Database.Services`  
Visibility: `public`  
Base/Interfaces:`IExtendedCrud<T>`  
Source: `xyToolz\Database\Services\xyCrudService.cs`

## Description:

/// The logic surrounding the CrudRepository
    ///

## Methods

- `Task<bool> Create(T entity, [CallerMemberName] string? callerName = null)` — `public`
  
  /// Gives a new entity to the CrudRepo 
        ///
- `Task<bool> Delete(T entity, [CallerMemberName] string? callerName = null)` — `public`
  
  /// Give the target to the CrudRepo to delete it
        ///
- `Task<bool> Update(T entity, [CallerMemberName] string? callerName = null)` — `public`
  
  /// Gives new data for the target entity to the CrudRepo 
        ///
- `Task<EntityEntry> GetEntryByID(int id, [CallerMemberName] string? callerName = null)` — `public async`
  
  /// Get a EntityEntry for the given ID
        ///
- `Task<IEnumerable<T?>> GetAll([CallerMemberName] string? callerName = null)` — `public`
  
  /// Calls GetAll() from the CrudRepo
        ///
- `Task<IEnumerable<T>> Pageineering(int page, int pageSize, [CallerMemberName] string? callerName = null)` — `public`
  
  /// Calls Pageineering from CrudRepo
        ///
- `Task<T?> Read(int id, [CallerMemberName] string? callerName = null)` — `public`
  
  /// Calls Read(id) from CRUD-Repo
        ///

## Fields

- `xyCrudRepo<T> _crudRepository` — `private readonly`
  
  /// Providing access to the target DB
        ///

