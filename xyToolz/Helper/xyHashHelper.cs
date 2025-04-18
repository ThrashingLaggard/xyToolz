using System;
using System.Security.Cryptography;
using System.Text;

namespace xyToolz.Helper
{
    /// <summary>
    /// Provides secure password hashing and verification functionality using PBKDF2.
    /// Includes configurable peppering, salt generation, logging, and support for SHA256/SHA512 algorithms.
    /// This utility is intended for use in authentication flows where user passwords need to be securely hashed,
    /// stored, and verified in a tamper-resistant manner.
    ///
    /// <para><b>Available Features:</b></para>
    /// <list type="bullet">
    /// <item><description>Environment-based configurable peppering (default: "Ahuhu")</description></item>
    /// <item><description>Secure salt generation using RNGCryptoServiceProvider</description></item>
    /// <item><description>Password hashing via PBKDF2 (Rfc2898DeriveBytes) with configurable iterations</description></item>
    /// <item><description>Base64 encoding for storage and transport</description></item>
    /// <item><description>Constant-time hash comparison to prevent timing attacks</description></item>
    /// <item><description>Detailed logging with support for synchronous logging methods</description></item>
    /// </list>
    /// <para><b>Thread Safety:</b></para>
    /// This class is thread-safe, as it contains no instance fields and all operations are stateless.
    ///
    /// <para><b>Limitations:</b></para>
    /// Only SHA256 and SHA512 algorithms are supported. Use of any other algorithm will result in an exception.
    ///
    /// <para><b>Performance:</b></para>
    /// The cost of password hashing is directly proportional to the number of iterations configured. Use higher iteration counts for increased security,
    /// but be mindful of performance impacts in high-load environments or when running on limited hardware (e.g., embedded devices).
    ///
    /// <para><b>Configuration:</b></para>
    /// The pepper can be set using the environment variable "XYTOOLZ_PEPPER". If not found, the default value "Ahuhu" is used.
    /// <para><b>Example Usage:</b></para>
    /// <code>
    /// byte[] salt;
    /// string hash = xyHashHelper.BuildSaltedHash(HashAlgorithmName.SHA256, "myPassword123", out salt);
    /// bool isValid = xyHashHelper.VerifyPassword(HashAlgorithmName.SHA256, "myPassword123", hash);
    /// </code>
    /// </summary>
    public static class xyHashHelper
    {
        #region Configuration

        /// <summary>
        /// Separator used between salt and hash when combining to a single string.
        /// </summary>
        private const string Separator = ":";

        /// <summary>
        /// Returns the currently active pepper value (used for debugging or testing only).
        /// </summary>
        internal static string GetCurrentPepper() => Pepper;


        /// <summary>
        /// Name of the environment variable used to retrieve the pepper value.
        /// </summary>
        private const string PepperEnvVarName = "XYTOOLZ_PEPPER";

        /// <summary>
        /// The configured pepper value, loaded from environment variable or defaults to "Ahuhu".
        /// </summary>
        private static readonly string? Pepper =
            string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(PepperEnvVarName))
                ? "Ahuhu"
                : Environment.GetEnvironmentVariable(PepperEnvVarName);
        /// <summary>
        /// Number of iterations used in PBKDF2 password hashing.
        /// </summary>
        public static readonly int Iterations = 100_000;
        /// <summary>
        /// Key length in bytes for SHA256-based hashes.
        /// </summary>
        public static readonly int KeyLength256 = 32;
        /// <summary>
        /// Key length in bytes for SHA512-based hashes.
        /// </summary>
        public static readonly int KeyLength512 = 64;

        #endregion

        #region Hashing Functions

        /// <summary>
        /// Returns a Base64-encoded hash string created with PBKDF2 from the provided password and salt.
        /// </summary>
        /// <param name="hashAlgorithm">The hash algorithm to use (e.g., SHA256).</param>
        /// <param name="password">The plaintext password.</param>
        /// <param name="salt">The cryptographic salt.</param>
        /// <returns>Base64-encoded hash string.</returns>
        public static string HashToString(HashAlgorithmName hashAlgorithm, string password, byte[] salt)
        {
            const string logMessage = "HashToString was called.";
            xyLog.Log(logMessage);

            byte[] bytes = HashToBytes(hashAlgorithm, password, salt);
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Returns a derived key (hash) as a byte array using PBKDF2.
        /// </summary>
        /// <param name="hashAlgorithm">The hash algorithm to use.</param>
        /// <param name="password">The plaintext password.</param>
        /// <param name="salt">The cryptographic salt.</param>
        /// <returns>Derived hash as byte array.</returns>
        public static byte[] HashToBytes(HashAlgorithmName hashAlgorithm, string password, byte[] salt)
        {
            const string logMessage = "HashToBytes was called.";
            xyLog.Log(logMessage);

            string pepperedPassword = PepperPassword(password);
            int length = SetKeySize(hashAlgorithm);

            using var pbkdf2 = new Rfc2898DeriveBytes(pepperedPassword, salt, Iterations, hashAlgorithm);
            return pbkdf2.GetBytes(length);
        }

        /// <summary>
        /// Creates a Base64 hash string by delegating to HashToString().
        /// </summary>
        /// <param name="hashAlgorithm">Hash algorithm to use.</param>
        /// <param name="password">The password to hash.</param>
        /// <param name="salt">The salt used in the hashing process.</param>
        /// <returns>Base64-encoded password hash.</returns>
        public static string HashPassword(HashAlgorithmName hashAlgorithm, string password, byte[] salt)
        {
            const string logMessage = "HashPassword was called.";
            xyLog.Log(logMessage);

            return HashToString(hashAlgorithm, password, salt);
        }

        #endregion

        #region Password Verification

        /// <summary>
        /// Verifies a plaintext password against a stored salted hash in the format "salt:hash".
        /// </summary>
        /// <param name="hashAlgorithm">The hash algorithm used originally to compute the hash (e.g., SHA256).</param>
        /// <param name="password">The plaintext password to verify.</param>
        /// <param name="saltNhash">The combined salt and hash string, separated by a colon ("salt:hash").</param>
        /// <returns>True if the password is valid; otherwise, false.</returns>
        public static bool VerifyPassword(HashAlgorithmName hashAlgorithm, string password, string saltNhash)
        {
            string separator = Separator;
            const string logMessage = "VerifyPassword (string) was called.";
            const string logInvalidFormat = "Invalid format in saltNhash.";
            byte[] salt = default!;
            byte[] hash = default!;
            byte[] hashToCheck = default!;

            xyLog.Log(logMessage);

            string[] input = saltNhash.Split(separator);
            if (input.Length != 2)
            {
                xyLog.Log(logInvalidFormat);
                return false;
            }

            salt = Convert.FromBase64String(input[0]);
            hash = Convert.FromBase64String(input[1]);
            hashToCheck = HashToBytes(hashAlgorithm, password, salt);

            return CryptographicOperations.FixedTimeEquals(hash, hashToCheck);
        }

        /// <summary>
        /// Compares two hash byte arrays using a fixed-time comparison to prevent timing attacks.
        /// </summary>
        /// <param name="hashAlgorithm">The hash algorithm used (not actively used in comparison).</param>
        /// <param name="hash1">First byte array representing the computed hash.</param>
        /// <param name="hash2">Second byte array representing the stored hash.</param>
        /// <returns>True if the hashes are identical; otherwise, false.</returns>
        public static bool VerifyPassword(HashAlgorithmName hashAlgorithm, byte[] hash1, byte[] hash2)
        {
            const string logMessage = "VerifyPassword (byte[]) was called.";
            xyLog.Log(logMessage);

            return CryptographicOperations.FixedTimeEquals(hash1, hash2);
        }

        #endregion

        #region Salt & Helper Functions

        /// <summary>
        /// Generates a cryptographically secure random salt of the specified length.
        /// </summary>
        /// <param name="length">Length of the salt in bytes. Default is 32 bytes.</param>
        /// <returns>A byte array containing the random salt.</returns>
        public static byte[] GenerateSalt(int length = 32)
        {
            const string logMessage = "GenerateSalt was called.";
            xyLog.Log(logMessage);

            try
            {
                byte[] salt = new byte[length];
                using var rng = RandomNumberGenerator.Create();
                rng.GetBytes(salt);
                return salt;
            }
            catch (Exception ex)
            {
                xyLog.ExLog(ex);
                throw;
            }
        }

        /// <summary>
        /// Builds a combined salt and Base64-encoded password hash string in the format "salt:hash".
        /// </summary>
        /// <param name="hashAlgorithm">The hash algorithm to use for hashing the password.</param>
        /// <param name="password">The plaintext password to hash.</param>
        /// <param name="salt">The generated cryptographic salt used for hashing, returned via out parameter.</param>
        /// <returns>Combined string of Base64 salt and hash separated by a colon.</returns>
        public static string BuildSaltedHash(HashAlgorithmName hashAlgorithm, string password, out byte[] salt)
        {
            string separator = Separator;
            const string logMessage = "BuildSaltedHash was called.";
            xyLog.Log(logMessage);

            try
            {
                salt = GenerateSalt();
                string hash = HashPassword(hashAlgorithm, password, salt);
                string saltString = Convert.ToBase64String(salt);
                return string.Concat(saltString, separator, hash);
            }
            catch (Exception ex)
            {
                xyLog.ExLog(ex);
                salt = Array.Empty<byte>();
                return string.Empty;
            }
        }

        #endregion

        #region Internal: KeySize & Pepper

        /// <summary>
        /// Determines the key size in bytes based on the specified hash algorithm.
        /// </summary>
        /// <param name="algorithm">The hash algorithm (e.g., SHA256 or SHA512).</param>
        /// <returns>Corresponding key length in bytes.</returns>
        /// <exception cref="ArgumentException">Thrown when an unsupported algorithm is provided.</exception>
        private static int SetKeySize(HashAlgorithmName algorithm)
        {
            const string unsupportedAlgorithmMessage = "Unsupported algorithm";
            const string logMessage = "SetKeySize was called.";
            xyLog.Log(logMessage);

            try
            {
                if (algorithm == HashAlgorithmName.SHA256) return KeyLength256;
                if (algorithm == HashAlgorithmName.SHA512) return KeyLength512;
                throw new ArgumentException(unsupportedAlgorithmMessage, nameof(algorithm));
            }
            catch (Exception ex)
            {
                xyLog.ExLog(ex);
                throw;
            }
        }

        /// <summary>
        /// Appends the configured pepper to the password to increase entropy.
        /// </summary>
        /// <param name="password">The plaintext password.</param>
        /// <returns>The password combined with the pepper string.</returns>
        private static string PepperPassword(string password)
        {
            const string logMessage = "PepperPassword was called.";
            xyLog.Log(logMessage);

            return string.Concat(password, Pepper);
        }

        #endregion
    }
}
