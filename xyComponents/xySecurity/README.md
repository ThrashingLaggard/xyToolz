# xySecurity

> Extracted component of [xyToolz](https://github.com/ThrashingLaggard/xyToolz) — source is maintained in the main xyToolz repository; this package exposes only this module standalone.

Password hashing, RSA/JWT, and data-protection helpers for .NET.

## Install

```
dotnet add package xySecurity
```

## Contents

- **`xyHasher`** — PBKDF2-based password hashing:
  - `HashPbkdf2(string)`, `VerifyPbkdf2(string, string)`
  - `BuildSaltedHash(HashAlgorithmName, string, out byte[] salt)`, `VerifyPassword(...)` overloads
  - `BuildKeyFromPassword`, `HashToBytes`, `HashToString`, `GenerateSalt(int)`

- **`xyRsa`** — RS256 JWT signing/validation:
  - `LoadKeysAsync(publicKeyPem, privateKeyPem)`, `ConfigureAsync(issuer, audience)`
  - `GenerateJwtAsync(IDictionary<string,object> claims, TimeSpan validFor)`
  - `ValidateJwtAsync(string token, bool validateLifetime = true)`
  - `GetPublicKeyAsPemAsync()`, `Encrypt(byte[], RSA)`, `Decrypt(byte[], RSA)`

- **`xyDataProtector`** — encrypt/decrypt objects, strings, bytes, and files:
  - `ProtectAsync<T>` / `UnprotectAsync<T>`, `ProtectString` / `UnprotectStringAsync`, `ProtectBytes` / `UnprotectBytesAsync`
  - `SaveProtectedToFileAsync<T>` / `LoadProtectedFromFileAsync<T>`, `ProtectFileAsync<T>`
  - `OverrideForTests` / `ResetOverride` — swap in an `IxyDataProtector` test double.

- **`IxyDataProtector`** — interface for mocking `xyDataProtector` in unit tests.

## Example

```csharp
string hash = xyHasher.BuildSaltedHash(HashAlgorithmName.SHA256, "Secure123!", out var salt);
bool valid = xyHasher.VerifyPassword(HashAlgorithmName.SHA256, "Secure123!", hash);

byte[] protectedBytes = await xyDataProtector.ProtectString("secret");
string plain = await xyDataProtector.UnprotectStringAsync(protectedBytes);
```

## Dependencies

- `Microsoft.IdentityModel.Tokens`
- `System.IdentityModel.Tokens.Jwt`
- `System.Security.Cryptography.ProtectedData`
- `xyLogger`
- `xyExtensions`, `xyQOL`, `xyFilesystem`, `xySerialization` (project references)

## Target Framework

.NET 8.0

## License

GPL-3.0-or-later
