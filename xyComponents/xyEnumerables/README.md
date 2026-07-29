# xyEnumerables

> Extracted component of [xyToolz](https://github.com/ThrashingLaggard/xyToolz) — source is maintained in the main xyToolz repository; this package exposes only this module standalone.

Stack-only, pooled-array-backed list implementation for .NET.

## Install

```
dotnet add package xyEnumerables
```

## Contents

- **`xyList<T>`** — `ref struct`, lives on the stack only (not usable in async/await, iterators, interfaces, or fields).
  Backed by an `ArrayPool<T>`-rented buffer with configurable expansion factor.
  - `Add(T)`, `RemoveAt(int)`, `RemoveAll()`, `IsEmpty()`, `ToArray()`, `Dispose()`
  - Indexer `this[int]` (get/set)
  - `Count`, `Capacity`, `Description`
  - `Remove(T)` is currently a no-op stub (not implemented).

## Example

```csharp
Span<int> buffer = stackalloc int[8];
var list = new xyList<int>(buffer, expansionFactor_: 2);

list.Add(1);
list.Add(2);
int first = list[0];

list.Dispose(); // returns the pooled buffer
```

## Dependencies

None.

## Target Framework

.NET 8.0

## License

GPL-3.0-or-later
