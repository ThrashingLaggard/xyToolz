# class xyCrudRepo<T>

Namespace: `xyToolz.Database.Repos`  
Visibility: `public`  
Base/Interfaces:`IExtendedCrud<T>`  
Source: `xyToolz\Database\Repos\xyCrudRepo.cs`

## Description:

/// Generic Repository for extended CRUD using EF Core
    ///

## Methoden

- `Task<bool> Create(T entity, [CallerMemberName] string? callerName = null)` — `public async`
  
  /// Create a new entry in the database using the given data
        ///
- `Task<bool> Delete(T entity, [CallerMemberName] string? callerName = null)` — `public async`
  
  /// Removing an entry from the database
        ///
- `Task<bool> Update(T entity, [CallerMemberName] string? callerName = null)` — `public async`
  
  /// Updating the target entry
        ///
- `Task<EntityEntry> GetEntryByID(int id, [CallerMemberName] string? callerName = null)` — `public async`
  
  /// Return a EntityEntry instance for the given id
        ///
- `Task<IEnumerable<T?>> GetAll([CallerMemberName] string? callerName = null)` — `public async`
  
  /// Return an IEnumerable filled with the content of the target table
        ///
- `Task<IEnumerable<T>> Pageineering(int page, int pageSize, [CallerMemberName] string? callerName = null)` — `public async`
  
  /// Select which and how many elements to return
        ///
- `Task<T?> Read(int id, [CallerMemberName] string? callerName = null)` — `public async`
  
  /// Get the corresponding instance for the given id
        ///

