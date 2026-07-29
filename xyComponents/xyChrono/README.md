# xyChrono

> Extracted component of [xyToolz](https://github.com/ThrashingLaggard/xyToolz) — source is maintained in the main xyToolz repository; this package exposes only this module standalone.

Minimal date/time helper extensions for .NET.

## Install

```
dotnet add package xyChrono
```

## Contents

- **`xyDate`** — placeholder class, currently empty (reserved for future date helpers).
- **`xyTime`** — static helpers and extension methods for `DateTimeOffset`:
  - `HashNow()` — hash code of `DateTimeOffset.Now`.
  - `Now()` — shorthand for `DateTimeOffset.Now`.
  - `GetDateTime(this DateTimeOffset)` — returns the UTC `DateTime` component.
  - `GetOffset(this DateTimeOffset)` — returns the `TimeSpan` offset.
  - `GetTimeOfDay(this DateTimeOffset)` — returns the time-of-day component.

## Example

```csharp
DateTimeOffset now = xyTime.Now();
DateTime utc = now.GetDateTime();
TimeSpan offset = now.GetOffset();
```

## Dependencies

None.

## Target Framework

.NET 8.0

## License

GPL-3.0-or-later
