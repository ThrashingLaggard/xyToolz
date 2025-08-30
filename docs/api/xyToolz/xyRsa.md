# class xyRsa

Namespace: `xyToolz`  
Visibility: `public static`  
Source: `xyToolz\xyRsa.cs`

## Description:

/// Utility class for handling JWT creation and validation using RSA public/private key encryption.
    ///
    /// 
Available Features:

    /// 
    ///   Asymmetric key loading from PEM (public/private).
    ///   JWT generation with arbitrary claims and expiration.
    ///   Token validation with issuer, audience, signature and lifetime checks.
    ///   Public key export in PEM format for client distribution.
    /// 
    ///
    /// 
Thread Safety:

    /// Not thread-safe due to internal static RSA references. Must not be used concurrently across multiple threads.
    ///
    /// 
Limitations:

    /// No support for rotating keys or refresh tokens out of the box.
    ///
    /// 
Example Usage:

    /// 
    /// await xyRsa.LoadKeysAsync(pubKey, privKey);
    /// await xyRsa.ConfigureAsync("https://myapi", "myAudience");
    /// string jwt = await xyRsa.GenerateJwtAsync(claims, TimeSpan.FromHours(1));
    /// var user = await xyRsa.ValidateJwtAsync(jwt);
    /// 
    ///

## Methoden

- `Task<bool> ConfigureAsync(string issuer, string audience)` — `public static async`
  
  /// Configures the issuer and audience values for JWT generation and validation.
        ///
- `Task<bool> LoadKeysAsync(string publicKeyPem, string privateKeyPem)` — `public static async`
  
  /// Loads RSA public and private Keys from PEM-formatted strings and initializes internal key containers.
        ///
- `Task<ClaimsPrincipal?> ValidateJwtAsync(string token, bool validateLifetime = true)` — `public static async`
  
  /// Validates a JWT using the configured RSA public key and returns the extracted ClaimsPrincipal if valid.
        ///
- `Task<string> GenerateJwtAsync(IDictionary<string, object> claims, TimeSpan validFor)` — `public static async`
  
  /// Generates a signed JSON Web Token (JWT) using the configured RSA private key and provided claims.
        ///
- `Task<string> GetPublicKeyAsPemAsync()` — `public static async`
  
  /// Exports the configured public RSA key as a PEM-formatted string.
        ///

