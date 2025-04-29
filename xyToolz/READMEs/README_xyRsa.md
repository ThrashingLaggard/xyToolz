# xyRsa

`xyRsa` is a reusable, fully static utility class designed for secure handling of **JWT (JSON Web Tokens)** using **RSA encryption** in .NET-based Web APIs or desktop applications.  
It enables easy generation, validation, and management of JWTs with **public/private key cryptography**, aligned with modern security practices.

---

## 🔐 Features

- ✅ **JWT Creation** using `RS256` and `SecurityTokenDescriptor`
- ✅ **JWT Validation** with configurable lifetime and signature checks
- 🔑 **Public/Private Key Loading** from PEM-formatted strings
- 📦 **Export Public Key** as PEM
- 🏷️ **Issuer & Audience configuration**
- 📜 Full logging of success, failure, and exceptions via `xyLog`

---

## 🧪 Example Usage

```csharp
await xyRsa.LoadKeysAsync(publicPem, privatePem);
await xyRsa.ConfigureAsync("MyApiIssuer", "MyAudience");

var token = await xyRsa.GenerateJwtAsync(new Dictionary<string, object>
{
    { "sub", "user123" },
    { "role", "admin" }
}, TimeSpan.FromHours(1));

var principal = await xyRsa.ValidateJwtAsync(token);
string pem = await xyRsa.GetPublicKeyAsPemAsync();
```

---

## 🛠️ Public API

### 🔑 LoadKeysAsync

```csharp
Task LoadKeysAsync(string publicKeyPem, string privateKeyPem)
```

Initializes the RSA key objects for signing (private key) and verification (public key).  
Requires PEM format (`-----BEGIN PUBLIC KEY-----` / `-----BEGIN PRIVATE KEY-----`).

---

### ⚙️ ConfigureAsync

```csharp
Task ConfigureAsync(string issuer, string audience)
```

Sets the default issuer and audience that will be used in both JWT creation and validation.

---

### 🪪 GenerateJwtAsync

```csharp
Task<string> GenerateJwtAsync(IDictionary<string, object> claims, TimeSpan validFor)
```

Generates a signed JWT with the specified claims and lifetime.  
Uses `RS256` (RSA SHA256) signature algorithm.

---

### ✅ ValidateJwtAsync

```csharp
Task<ClaimsPrincipal?> ValidateJwtAsync(string token, bool validateLifetime = true)
```

Validates a JWT against the configured `issuer`, `audience`, and public key.  
Returns `ClaimsPrincipal` if valid, otherwise `null`.

---

### 📦 GetPublicKeyAsPemAsync

```csharp
Task<string> GetPublicKeyAsPemAsync()
```

Returns the current public RSA key in PEM format (Base64).  
Can be published to clients for local JWT validation or external services.

---

## 🧩 Dependencies

- `System.IdentityModel.Tokens.Jwt`
- `Microsoft.IdentityModel.Tokens`
- `System.Security.Cryptography`
- `xyLog` (custom logging interface)
- `xyJson` (optional for serialization elsewhere)

---

## 🧱 Thread Safety

This class is fully thread-safe by design:
- All internal state is held in private `static` fields
- No shared mutable data outside initialization
- All methods are `static async`

---

## ❗ Notes & Limitations

- Keys must be provided in **PEM format**
- No automatic key rotation
- Refresh tokens not included – can be added via wrapper or external logic
- Intended for **stateless token handling**, not identity management

---

## 📎 Related Components

- `xyJson` – for object-serialization
- `xyHashHelper` – if you wish to hash payloads before JWT usage
- `xyDataProtection` – for encrypting/decrypting sensitive values

---

## ✅ Status

**✅ Fully reviewed**  
**✅ Logging integrated**  
**✅ Exception-safe**  
**🧪 Ready for production** (assuming secure key management)

---

## 👨‍🔧 Author’s Tip

If used in a Web API project, pair `xyRsa` with middleware or attribute-based token validation in controllers.  
You can inject the public key into `JwtBearerOptions.TokenValidationParameters` for server-side verification.

---