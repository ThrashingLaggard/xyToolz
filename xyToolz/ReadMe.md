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
