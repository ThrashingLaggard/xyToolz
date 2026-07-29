# xyQOL

> Extracted component of [xyToolz](https://github.com/ThrashingLaggard/xyToolz) — source is maintained in the main xyToolz repository; this package exposes only this module standalone.

Quality-of-life helpers: safe-execution wrappers, reflection-based property mapping, and collection-to-string joining.

## Install

```
dotnet add package xyQOL
```

## Contents

- **`xy`** — misc static helpers:
  - `TryCatch(...)` — sync/async overloads that wrap a delegate call in try/catch (0–2 params, `params object[]`, with/without return value).
  - `Repeat(char, int)`, `Print(this string)`, `Log(this string)`.
  - `Start(string processName)`, `Open(string fullPath)` — start/open a process or file.
  - `Editor()` / `Editor(string filePath)` — open a text editor.
  - `Piep()` — system beep. `Crash(UInt128)` — deliberate failure helper.

- **`xyPropertyHelper`** — reflection-based object/dictionary mapping:
  - `GetPropertyInfosForTarget<T>(T obj)` — public `PropertyInfo[]` of an object.
  - `GetPropertyValuesForTarget<TKey,TValue,T>(T obj)` — object's public properties as a dictionary.
  - `GetEntityFromDictionary<T,TKey,TValue>(Dictionary<TKey,TValue>)` — build an instance of `T` from a dictionary.
  - `PropertiesToString<TKey,TValue,T>(Dictionary<TKey,TValue>)`.

- **`xySpiller`** — extension methods to join collections into a single string:
  - `Spill<T>(this IEnumerable<T> / T[] / IList<T>, ...)`, `Spill<TKey,TValue>(this Dictionary<TKey,TValue>, ...)`.
  - `JoinDebug<T>(this IEnumerable<T>, ...)`.

## Example

```csharp
object result = xy.TryCatch(() => File.ReadAllText("config.json"));

var values = xyPropertyHelper.GetPropertyValuesForTarget<string, object, MyClass>(instance);

string joined = new[] { 1, 2, 3 }.Spill(); // "1, 2, 3"
```

## Dependencies

- `xyLogger`
- `xyMessageFactory`

## Target Framework

.NET 8.0

## License

GPL-3.0-or-later
