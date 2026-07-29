# xyMaths

> Extracted component of [xyToolz](https://github.com/ThrashingLaggard/xyToolz) — source is maintained in the main xyToolz repository; this package exposes only this module standalone.

Digit-sum utilities and arbitrary-base number conversion for .NET.

## Install

```
dotnet add package xyMaths
```

## Contents

- **`SumOfDigits`** — digit-sum calculations across all integer/floating-point types and `Guid`:
  - `SumOfHexDigits(...)` — overloads for `byte`, `sbyte`, `short`, `ushort`, `int`, `uint`, `long`, `ulong`, `float`, `double`, `decimal`, `Guid`.
  - `SumOfDecimalDigits(...)` — same type coverage, decimal-digit sum.
  - `SumOfBinary(...)` — same type coverage, binary-digit sum.
  - `SumOfAnyDigits(ulong value, int baseSystem)`, `SumOfDigitsInBase(long/int value, int baseSystem)` — digit sum in an arbitrary base.

- **`xyConversion`** — number-system conversion helpers:
  - `X_to_X(int fromBase, string number, int toBase)` — convert a number string between arbitrary bases.
  - `DEC_to_X(int number, int toBase)`, `HEX_to_DEC`, `HEX_to_Oct`, `HEX_to_Bin`, `Bin_to_Dec`, `Bin_to_Hex`, `Bin_to_Oct`.
  - `Letterer(string)` / `DeLetterer(string)` — encode/decode numbers too large for standard bases using letters as extra digits.

## Example

```csharp
int digitSum = SumOfDigits.SumOfDecimalDigits(12345);       // 15
int hexSum   = SumOfDigits.SumOfHexDigits(0xFF);            // 30

string hex = xyConversion.DEC_to_X(255, toBaseOfTargetNumberSystem: 16); // "FF"
string bin = xyConversion.HEX_to_Bin("FF");
```

## Dependencies

- `xyExtensions` (project reference)
- `xyQOL` (project reference)

## Target Framework

.NET 8.0

## License

GPL-3.0-or-later
