# class xyBaseModel

Namespace: `xyToolz.Database.Basix`  
Visibility: `public abstract`  
Source: `xyToolz\Database\xyBaseModel.cs`

## Description:

/// Providing basic properties and methods for models:
    /// 
    /// ID, Name, Description, Comment
    /// 
    /// ToString() => Name
    /// 
    /// 
    ///

## Properties

- `string? Comment{ get; set; }` — `public virtual`
  
  /// Comment or note for the object.
        ///
- `string? Description{ get; set; }` — `public virtual`
  
  /// Add usefull information for the target
        ///
- `string? Name{ get; set; }` — `public virtual`
  
  /// Name the target
        ///
- `uint? ID{ get; set; }` — `public virtual`
  
  /// Index
        ///

## Methods

- `string ToString()` — `public override`
  
  /// Returns the name property
        ///

