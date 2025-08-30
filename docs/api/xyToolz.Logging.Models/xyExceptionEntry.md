# class xyExceptionEntry

Namespace: `xyToolz.Logging.Models`  
Visibility: `public`  
Source: `xyToolz\Logging\Models\xyExceptionEntry.cs`

## Description:

(No XML‑Summary )

## Konstruktoren

- `xyExceptionEntry(Exception exception_)` — `public`
  
  (No XML‑Summary )

## Eigenschaften

- `DateTime Timestamp{ get; init; }` — `public`
  
  /// Time of occurance
        ///
- `Exception Exception{ get; init; }` — `public required`
  
  /// The target exception
        ///
- `Exception InnerException{ get; init; }` — `public`
  
  /// The inner exception
        ///
- `IDictionary CustomData{ get; set; }` — `public`
  
  /// Space for additional data
        ///
- `MethodBase TargetSite{ get; init; }` — `public`
  
  /// Where did it happen
        ///
- `string Description{ get; set; }` — `public`
  
  /// Add interesting information
        ///
- `string Message{ get; init; }` — `public`
  
  /// The message embedded in the exception
        ///
- `string Source{ get; init; }` — `public`
  
  /// The root of the problem
        ///
- `string StrackTrace{ get; init; }` — `public`
  
  /// Show the frames on the call stack
        ///
- `Type TypeOfException{ get; set; }` — `public`
  
  /// What kind of exception does this?
        ///
- `uint ID{ get; set; }` — `public`
  
  /// For easy administration
        ///

