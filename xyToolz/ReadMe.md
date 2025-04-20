# xyHashHelper.cs

Der `xyHashHelper` bietet robuste und sichere Methoden zum Hashen, Vergleichen und Abspeichern von Passwörtern, Salts und kryptografischen Schlüsseln.

## 📌 Features

- 🔐 Salted Hashing via PBKDF2 (Rfc2898DeriveBytes)
- 🔑 Dynamische Salt-Generierung mit Secure RNG
- 🌶️ Optionale Pepper-Absicherung via Umgebungsvariable `PEPPER`
- 🧮 Unterstützt SHA256 und SHA512
- 🔁 Sichere Passwortverifikation mit `FixedTimeEquals`
- 🧪 Umfangreiches Logging
- 📦 Kompatibel mit AES-Key-Derivation (z. B. für Verschlüsselung)

## 🔧 Konfiguration

| Option           | Beschreibung                                  | Default     |
|------------------|-----------------------------------------------|-------------|
| `PEPPER`         | Umgebungsvariable für zusätzliche Entropie    | `"Ahuhu"`   |
| `Iterations`     | Wiederholungen in PBKDF2                      | `100_000`   |
| `KeyLength256`   | Keylänge für AES-256 / SHA256                 | `32 Bytes`  |
| `KeyLength512`   | Keylänge für SHA512                           | `64 Bytes`  |

## 📌 Methodenübersicht

| Methode                          | Zweck                                        |
|----------------------------------|----------------------------------------------|
| `BuildSaltedHash()`             | Erstellt neuen Salt + Hash im Format `salt:hash` |
| `VerifyPassword()`              | Vergleicht Password gegen Salt+Hash          |
| `BuildKeyFromPassword()`        | Generiert AES-Schlüssel aus Passwort + Salt  |
| `GenerateSalt()`                | Erstellt sicheren Salt in gewünschter Länge  |
| `HashToBytes()` / `HashToString()` | Raw-Hash oder Base64-Hash erzeugen        |
| `TryVerifyPassword()` *(neu)*   | Sicherer Passwort-Vergleich mit `out bool`   |

## 🧪 Beispiel

```csharp
// Hash erstellen
string password = "MeinSicheresPasswort123!";
string saltedHash = xyHashHelper.BuildSaltedHash(HashAlgorithmName.SHA256, password, out byte[] salt);

// Passwort prüfen
bool isCorrect = xyHashHelper.VerifyPassword(HashAlgorithmName.SHA256, password, saltedHash);






#~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
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
