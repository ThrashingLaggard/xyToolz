# class xy

Namespace: `xyToolz`  
Visibility: `public static`  
Attribute: `System.Diagnostics.CodeAnalysis.SuppressMessage`  
Source: `xyToolz\xy.cs`

## Description:

/// Little helpers:
    /// 
    /// Repeat  or  reverse   strings
    /// ---
    /// TryCatch for  async  and  sync  methods
    /// ---
    /// Print  on Console
    /// ---
    /// String to Bytes     
    /// ---
    /// Bytes to String
    /// ---
    /// Open Editor 
    ///         -> also with file
    ///         
    ///         ---------------------------
    ///         
    /// "Useless" experiments:
    /// 
    /// Biep
    /// ---
    /// Crash
    /// 
    ///

## Methoden

- `byte[] BaseToBytes(string base64)` — `public static`
  
  /// Converts a Base64-encoded string into a byte array.
        ///
- `byte[] StringToBytes(string input)` — `public static`
  
  /// Converts a UTF-8 string into a byte array.
        ///
- `object TryCatch(Func<object, object, object> dangerousMethod, object param1, object param2)` — `public static`
  
  (No XML‑Summary )
- `object TryCatch(Func<object, object> dangerousMethod, object param)` — `public static`
  
  (No XML‑Summary )
- `object TryCatch(Func<object[], object> method, params object[] args)` — `public static`
  
  /// Wraps a synchronous delegate with exception handling.
        ///
- `object TryCatch(Func<object> method)` — `public static`
  
  /// Wraps a parameterless synchronous delegate with exception handling.
        ///
- `string BytesToBase(byte[] bytes)` — `public static`
  
  /// Converts a byte array into a Base64-encoded string.
        ///
- `string BytesToString(byte[] bytes)` — `public static`
  
  /// Converts a byte array into a UTF-8 string.
        ///
- `string Crash(UInt128 high_number)` — `public static`
  
  /// A recursive method that tests edge-case control flow with arbitrary branching using goto.
        ///
- `string Repeat(string text, ushort count)` — `public static`
  
  /// Repeats a given string a specified number of times.
        ///
- `string Reverse(string input)` — `public static`
  
  /// Reverses the characters in a given string.
        ///
- `Task Editor()` — `public static async`
  
  /// Opens Notepad without any file.
        ///
- `Task Start(string processName)` — `public static async`
  
  (No XML‑Summary )
- `Task<bool> Open(string fullPath)` — `public static async`
  
  /// Asynchronously opens a file or folder in the system’s default file explorer.
        /// Cross‑platform (Windows, Linux, macOS) and fully logged.
        ///
- `Task<object> TryCatch(Func<object, object, Task<object>> dangerousMethod, object param1, object param2)` — `public static async`
  
  (No XML‑Summary )
- `Task<object> TryCatch(Func<object, Task<object>> dangerousMethod, object param)` — `public static async`
  
  (No XML‑Summary )
- `Task<object> TryCatch(Func<object[], Task<object>> method, params object[] args)` — `public static async`
  
  /// Wraps an asynchronous delegate with exception handling.
        ///
- `Task<object> TryCatch(Func<Task<object>> method)` — `public static async`
  
  /// Wraps a parameterless asynchronous delegate with exception handling.
        ///
- `void Editor(string filePath)` — `public static`
  
  /// Opens Notepad and loads a specific file.
        ///
- `void Piep()` — `public static`
  
  /// Triggers the console beep and logs it.
        ///
- `void Print(string message)` — `public static`
  
  /// Prints the given message to the console using xyLog.
        ///

