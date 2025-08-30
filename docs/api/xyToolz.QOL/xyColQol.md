# class xyColQol

Namespace: `xyToolz.QOL`  
Visibility: `public static`  
Source: `xyToolz\QOL\xyColQol.cs`

## Description:

/// Helpers for Enumerables:
      /// 
      /// Fill with integer
      ///             -> with even
      ///             -> with odd
      ///             
      /// Print whole thingy
      ///         --> also with Carriage return
      /// 
      /// Fill with a time for every quarter hour of the day 
      ///     
      ///

## Methods

- `IEnumerable<int> FillEvenList( int limit )` — `public static`
  
  (No XML‑Summary )
- `IEnumerable<int> FillOddList( int limit )` — `public static`
  
  (No XML‑Summary )
- `IEnumerable<int> FillTheList( int limit )` — `public static`
  
  (No XML‑Summary )
- `IEnumerable<string> GetQuarterHours()` — `private static`
  
  /// Returns a list filled with all the quarters of an hour in the day
            ///
- `string Spill( IEnumerable values )` — `public static`
  
  /// Print the targets intestines on your favourite console
        ///
- `string SpillDown( IEnumerable values )` — `public static`
  
  /// Print the targets intestines on your favourite console with comma and CARRIAGE RETURNS after every value
            ///
- `string SplitSpill( IEnumerable values )` — `public static`
  
  /// Print the targets intestines on your favourite console and splits them with a comma
            ///
- `Task<string> AsxSpill( IEnumerable values )` — `public static async`
  
  /// Print the targets intestines on your favourite console BUT ASYNC
            ///
- `Task<string> AsxSpillDown( IEnumerable values )` — `public static async`
  
  /// Print the targets intestines on your favourite console with comma and CARRIAGE RETURNS after every value
            ///

