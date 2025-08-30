# class xyQol

Namespace: `xyToolz.QOL`  
Visibility: `public`  
Source: `xyToolz\QOL\xyQol.cs`

## Description:

(No XML‑Summary )

## Constructors

- `xyQol(xyLoggerManager log_, xyMessageFactory fac_)` — `public`
  
  (No XML‑Summary )

## Methods

- `Dictionary<TKey, TValue> GetPropertyValuesForTarget<TKey, TValue, T>(T obj)` — `public`
  
  /// Read all values from all properties from the target
        ///
- `PropertyInfo[] GetPropertyInfosForTarget<T>(T obj)` — `public`
  
  /// Returns an array of PropertyInfo for the specified target
        ///
- `string PropertiesToString< TKey, TValue,T>(Dictionary<TKey, TValue> keyValuePairs)` — `public`
  
  /// Print all properties and their values into the console
        ///
- `T GetEntityFromDictionary<T, TKey, TValue>(Dictionary<TKey, TValue> keyValuePairs)` — `public`
  
  /// Create an instance of whatever the dictionary holds
        ///

## Fields

- `xyLoggerManager _log` — `private readonly`
  
  (No XML‑Summary )
- `xyMessageFactory _fac` — `private readonly`
  
  (No XML‑Summary )

