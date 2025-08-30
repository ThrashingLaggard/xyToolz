# interface IBaseCrudExtensions<T>

Namespace: `xyToolz.Database.Interfaces`  
Visibility: `public`  
Source: `xyToolz\Database\Interfaces\IBaseCrudExtensions.cs`

## Description:

/// Extension for the IBaseCrud interface 
    /// providing :
    /// 
    /// GetAll()
    /// 
    /// Pageineering(x,y)
    /// 
    ///

## Methods

- `Task<IEnumerable<T?>> GetAll([CallerMemberName] string? callerName = null)`
  
  /// Get all instances
        ///
- `Task<IEnumerable<T>> Pageineering(int page, int pageSize, [CallerMemberName] string? callerName = null)`
  
  /// Filter the entries to be shown
        ///

