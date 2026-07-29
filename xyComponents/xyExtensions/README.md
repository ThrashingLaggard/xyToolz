# xyExtensions

> Extracted component of [xyToolz](https://github.com/ThrashingLaggard/xyToolz) — source is maintained in the main xyToolz repository; this package exposes only this module standalone.

Extension methods for `string`, `byte[]`, `List<T>`, and `Dictionary<TKey,TValue>`.

## Install

```
dotnet add package xyExtensions
```

## Contents

- **`xyByteExtensions`**
  - `ToUtf8(this byte[])` — decode bytes to a UTF-8 string.
  - `ToBase(this byte[])` — encode bytes to Base64.

- **`xyStringExtensions`**
  - `Repeat(this string, ushort count)` — repeat a string.
  - `Reverse(this string)` / `ReverseUnicode(this string)` — reverse characters (Unicode-safe variant included).
  - `ToBytes(this string)` — UTF-8 encode to bytes.
  - `BaseToBytes(this string)` — decode a Base64 string to bytes.
  - `IsNullOrEmpty(this string)`, `IsNullOrWhitespace(this string)`.

- **`xyListExtensions`**
  - `FillWithNumbers(this IList<int>, int limit)` — fill with `0..limit`.
  - `AddEvenNumbers(this IList<int>, int limit)` — append even numbers up to `limit`.
  - `AddOddNumbers(this IList<int>, int limit)` — append odd numbers up to `limit`.

- **`xyDictionaryExtensions`**
  - `GetOrAdd<TKey,TValue>(this Dictionary<TKey,TValue>, TKey, TValue)` — single-lookup get-or-add via `CollectionsMarshal`.
  - `TryUpdate<TKey,TValue>(this Dictionary<TKey,TValue>, TKey, TValue)` — single-lookup update via `CollectionsMarshal`.

## Example

```csharp
string base64 = "hello".ToBytes().ToBase();
string doubled = "ab".Repeat(3);           // "ababab"
bool empty = someString.IsNullOrWhitespace();

var dict = new Dictionary<string, int>();
int value = dict.GetOrAdd("key", 42);
```

## Dependencies

- `xyLogger` (exception logging via `xyLog.ExLog`)

## Target Framework

.NET 8.0

## License

GPL-3.0-or-later
