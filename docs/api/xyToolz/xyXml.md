# class xyXml

Namespace: `xyToolz`  
Visibility: `public static`  
Source: `xyToolz\Logging\Models\xyXml.cs`

## Description:

/// Helper class to (de)serialize objects from and to XML
    ///

## Methoden

- `string ToXML<T>(T target)` — `public static`
  
  /// Serialize the target into a xml string
        ///
- `T FromXml<T>(string xml, bool? outputTargetInConsole = false)` — `public static`
  
  /// Deserialize the target from xml and print it in the console if needed
        ///
- `T FromXml<T>(string xml)` — `public static`
  
  /// Deserialize the target from xml 
        ///

