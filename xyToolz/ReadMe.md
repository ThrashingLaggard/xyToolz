### Class: xyHashHelper

**Purpose:** Provides secure password hashing and verification functionality using PBKDF2.

# Features
- Environment-based configurable peppering (default: "Ahuhu")
- Secure salt generation using RNGCryptoServiceProvider
- PBKDF2 password hashing (configurable iterations)
- Base64 encoding
- Constant-time hash comparison
- Logging with xyLog (sync)

# Thread Safety
✅ Thread-safe — fully static, no instance state

# Limitations
❗ Only SHA256 and SHA512 supported

# Performance
⏱ Iterations affect CPU load — consider tuning for performance-sensitive applications

# Configuration
🔧 Set `PEPPER` env var to override default pepper ("Ahuhu")

# Example
```csharp
byte[] salt;
string hash = xyHashHelper.BuildSaltedHash(HashAlgorithmName.SHA256, "password", out salt);
bool ok = xyHashHelper.VerifyPassword(HashAlgorithmName.SHA256, "password", hash);


# ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


