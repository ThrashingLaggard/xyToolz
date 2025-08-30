# class xyILoggerAdapter<T>

Namespace: `xyToolz.Logging.Loggers`  
Visibility: `public`  
Base/Interfaces:`ILogger<T>`  
Source: `xyToolz\Logging\Loggers\xyILoggerAdapter.cs`

## Description:

/// This is a wrapper to make ILogger work with my own ILogging Framework
    ///

## Methods

- `bool IsEnabled(LogLevel logLevel)` — `public`
  
  /// Basically allways enabled (Lvl 6 disables it) 
        ///
- `IDisposable? BeginScope<TState>(TState state)` — `public`
  
  (No XML‑Summary )
- `void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)` — `public`
  
  (No XML‑Summary )

## Fields

- `ILogging _logger` — `private readonly`
  
  (No XML‑Summary )

