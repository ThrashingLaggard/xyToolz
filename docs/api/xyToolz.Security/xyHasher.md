# class xyHasher

Namespace: `xyToolz.Security`  
Visibility: `public static`  
Source: `xyToolz\Security\xyHasher.cs`

## Description:

/// Provides secure password hashing and verification functionality using PBKDF2.
    /// Includes configurable peppering, salt generation, logging, and support for SHA256/SHA512 algorithms.
    /// This utility is intended for use in authentication flows where user passwords need to be securely hashed,
    /// stored, and verified in a tamper-resistant manner.
    ///
    /// 
Available Features:

    /// 
    /// Environment-based configurable peppering (default: "Ahuhu")
    /// Secure salt generation using RNGCryptoServiceProvider
    /// Password hashing via PBKDF2 (Rfc2898DeriveBytes) with configurable iterations
    /// Base64 encoding for storage and transport
    /// Constant-time hash comparison to prevent timing attacks
    /// Detailed logging with support for synchronous logging methods
    /// 
    /// 
Thread Safety:

    /// This class is thread-safe, as it contains no instance fields and all operations are stateless.
    ///
    /// 
Limitations:

    /// Only SHA256 and SHA512 algorithms are supported. Use of any other algorithm will result in an exception.
    ///
    /// 
Performance:

    /// The cost of password hashing is directly proportional to the number of iterations configured. Use higher iteration counts for increased security,
    /// but be mindful of performance impacts in high-load environments or when running on limited hardware (e.g., embedded devices).
    ///
    /// 
Configuration:

    /// The pepper can be set using the environment variable "XYTOOLZ_PEPPER". If not found, the default value "Ahuhu" is used.
    /// 
Example Usage:

    /// 
    /// byte[] salt;
    /// string hash = xyHashHelper.BuildSaltedHash(HashAlgorithmName.SHA256, "myPassword123", out salt);
    /// bool isValid = xyHashHelper.VerifyPassword(HashAlgorithmName.SHA256, "myPassword123", hash);
    /// 
    ///

## Methods

- `bool VerifyPassword(HashAlgorithmName hashAlgorithm, byte[] hash1, byte[] hash2)` — `public static`
  
  /// Compares two hash byte arrays using a fixed-time comparison to prevent timing attacks.
        ///
- `bool VerifyPassword(HashAlgorithmName hashAlgorithm, string password, string saltNhash)` — `public static`
  
  /// Verifies a plaintext password against a stored salted hash in the format "salt:hash".
        ///
- `byte[] BuildKeyFromPassword(string password, byte[] salt)` — `public static`
  
  /// Derives a secure cryptographic key from a password and salt using PBKDF2 (RFC 2898).
        ///
        /// 
Internals:

        /// - Uses HMAC-SHA256 as PRF (Pseudorandom Function).
        /// - Internally applies the password, salt, and iteration count to generate the key.
        /// - Suitable for use with AES encryption or HMAC-based signing.
        ///
        /// 
Thread Safety:

        /// Thread-safe: stateless and does not cache results.
        ///
        /// 
Limitations:

        /// - Requires caller to provide a strong, unique salt.
        /// - Not suitable for hardware tokens or FIDO where key derivation must be deterministic.
        ///
        /// 
Example Usage:

        /// 
        /// byte[] salt = RandomNumberGenerator.GetBytes(16);
        /// byte[] key  = xyHashHelper.BuildKeyFromPassword("mySecret123", salt, 32);
        /// 
        ///
- `byte[] GenerateSalt(int length = 32)` — `public static`
  
  /// Generates a secure random Salt, using a cryptographically strong RNG.
        ///
- `byte[] HashToBytes(HashAlgorithmName hashAlgorithm, string password, byte[] salt)` — `public static`
  
  /// Returns a derived key (hash) as a byte array using PBKDF2 and the given hash algorithm.
        ///
- `int SetKeySize(HashAlgorithmName algorithm)` — `private static`
  
  /// Determines the key size in bytes based on the specified hash algorithm.
        ///
- `string BuildSaltedHash(HashAlgorithmName hashAlgorithm, string password, out byte[] salt)` — `public static`
  
  /// Builds a combined salt and Base64-encoded password hash string in the format "salt:hash".
        ///
- `string GetCurrentPepper()` — `internal static`
  
  /// Returns the currently active pepper value (used for debugging or testing only).
        ///
- `string HashPassword(HashAlgorithmName hashAlgorithm, string password, byte[] salt)` — `public static`
  
  /// Creates a Base64 hash string by calling HashToString().
        ///
- `string HashToString(HashAlgorithmName hashAlgorithm, string password, byte[] salt)` — `public static`
  
  /// Returns a Base64-encoded hash string created with PBKDF2 from the provided password and salt.
        ///
- `string PepperPassword(string password)` — `private static`
  
  /// Appends the configured pepper to the password to increase entropy.
        ///

## Fields

- `int Iterations` — `public static readonly`
  
  /// Number of iterations used in PBKDF2 password hashing.
        ///
- `int KeyLength256` — `public static readonly`
  
  /// Key length in bytes for SHA256-based hashes.
        ///
- `int KeyLength512` — `public static readonly`
  
  /// Key length in bytes for SHA512-based hashes.
        ///
- `string Pepper` — `private static readonly`
  
  /// The configured pepper value, loaded from environment variable or defaults to "Ahuhu".
        ///
- `string PepperEnvVarName` — `private const`
  
  /// Name of the environment variable used to retrieve the pepper value.
        ///
- `string Separator` — `private const`
  
  /// Separator used between salt and hash when combining to a single string.
        ///

