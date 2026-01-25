using System.Globalization;
using System.Security.Cryptography;
using xyToolz.Helper.Logging;
using xyToolz.Misc;
using xyToolz.QOL;
using xyToolz.StaticLogging;

namespace xyToolz.Security
{

    internal static class xySecurityDefaults
    {
        public const int Pbkdf2Iterations = 150_000; // sicher & flott genug
        public const int SaltSizeBytes = 16;
        public const int KeySizeBytes = 32;
    }



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
    public static class xyHasher
    {
        #region "Configuration"

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
        private const string PepperEnvVarName = "PEPPER";

        /// <summary>
        /// The configured pepper value, loaded from environment variable or defaults to "Ahuhu".
        /// </summary>
        private static readonly string Pepper = string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(PepperEnvVarName)) ? "Ahuhu" : Environment.GetEnvironmentVariable(PepperEnvVarName)!;

        /// <summary>
        /// Number of iterations used in PBKDF2 password hashing.
        /// </summary>
        public static readonly int Iterations = 200_000;
        /// <summary>
        /// Key length in bytes for SHA256-based hashes.
        /// </summary>
        public static readonly int KeyLength256 = 32;
        /// <summary>
        /// Key length in bytes for SHA512-based hashes.
        /// </summary>
        public static readonly int KeyLength512 = 64;

        #endregion


        /// <summary>
        /// Derives a secure cryptographic key from a password and salt using PBKDF2 (RFC 2898).
        ///
        /// <para><b>Internals:</b></para>
        /// - Uses HMAC-SHA256 as PRF (Pseudorandom Function).
        /// - Internally applies the password, salt, and iteration count to generate the key.
        /// - Suitable for use with AES encryption or HMAC-based signing.
        ///
        /// <para><b>Thread Safety:</b></para>
        /// Thread-safe: stateless and does not cache results.
        ///
        /// <para><b>Limitations:</b></para>
        /// - Requires caller to provide a strong, unique salt.
        /// - Not suitable for hardware tokens or FIDO where key derivation must be deterministic.
        ///
        /// <para><b>Example Usage:</b></para>
        /// <code>
        /// byte[] salt = RandomNumberGenerator.GetBytes(16);
        /// byte[] key  = xyHashHelper.BuildKeyFromPassword("mySecret123", salt, 32);
        /// </code>
        /// </summary>
        /// <param name="password">The input password from which the key will be derived.</param>
        /// <param name="salt">A cryptographically secure random salt (minimum 8 bytes recommended).</param>
        /// <returns>A derived key as a byte array.</returns>
        /// <exception cref="ArgumentException">Thrown if password or salt is invalid, or key length is too short.</exception>
        public static byte[] BuildKeyFromPassword(string password, byte[] salt)
        {

            string errorSalt = "Salt must not be null or empty.";
            string errorPassword = "Password must not be null or empty.";
            string ok = "Seems to work";

            xyLog.Log(salt is null || salt.Length == 0 ? errorSalt : string.IsNullOrWhiteSpace(password) ? errorPassword : ok);

            using Rfc2898DeriveBytes pbkdf2 = new(xyEncoder.StringToBytes(password), salt!, Iterations, HashAlgorithmName.SHA256);

            return pbkdf2.GetBytes(KeyLength256);
        }




        #region Hashing Functions

        /// <summary>
        /// Returns a Base64-encoded hash string created with PBKDF2 from the provided password and salt.
        /// </summary>
        /// <param name="hashAlgorithm">The hash algorithm to use</param>
        /// <param name="password">The plaintext password.</param>
        /// <param name="salt">The cryptographic salt.</param>
        /// <returns>Base64-encoded hash string.</returns>
        public static string HashToString(HashAlgorithmName hashAlgorithm, string password, byte[] salt)
        {
            string result = "";

            if (HashToBytes(hashAlgorithm, password, salt) is byte[] bytes)
            {
                if (bytes.Length > 0)
                {
                    result = xyEncoder.BytesToBase(bytes);
                }
            }
            return result;
        }

        /// <summary>
        /// Returns a derived key (hash) as a byte array using PBKDF2 and the given hash algorithm.
        /// </summary>
        /// <param name="hashAlgorithm">The hash algorithm to use.</param>
        /// <param name="password">The plaintext password.</param>
        /// <param name="salt">The cryptographic salt.</param>
        /// <returns>Derived key as byte array.</returns>
        public static byte[] HashToBytes(HashAlgorithmName hashAlgorithm, string password, byte[] salt)
        {
            string log = $"Algorithm: {hashAlgorithm.Name}, SaltLength: {salt.Length}";
            string logSaltError = "Provided salt is null or empty.";
            string logPasswordError = "Provided password is null or empty.";


            if (salt is null || salt.Length == 0)
            {
                xyLog.Log(logSaltError);
                return Array.Empty<byte>();
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                xyLog.Log(logPasswordError);
                return Array.Empty<byte>();
            }


            xyLog.Log(log);

            string pepperedPassword = PepperPassword(password);
            int keySize = SetKeySize(hashAlgorithm);

            using Rfc2898DeriveBytes pbkdf2 = new(pepperedPassword, salt, Iterations, hashAlgorithm);
            return pbkdf2.GetBytes(keySize);
        }


        /// <summary>
        /// Creates a Base64 hash string by calling HashToString().
        /// </summary>
        /// <param name="hashAlgorithm">Hash algorithm to use.</param>
        /// <param name="password">The password to hash.</param>
        /// <param name="salt">The salt used in the hashing process.</param>
        /// <returns>Base64-encoded password hash.</returns>
        public static string HashPassword(HashAlgorithmName hashAlgorithm, string password, byte[] salt)
        {
            string saltError = "Salt is null or empty.";
            string pwError = "Password is null or empty.";
            string weird = "You should not be able to read this, please inform experts immediately!";

            bool isSaltValid = salt is not null && salt.Length > 0;
            bool isPwValid = !string.IsNullOrWhiteSpace(password);

            if (isSaltValid && isPwValid)
            {
                return HashToString(hashAlgorithm, password, salt!);
            }
            else
            {
                xyLog.Log(!isSaltValid ? saltError : !isPwValid ? pwError : weird);
                return string.Empty;
            }
        }

        public static string HashPbkdf2(string plaintext)
           => HashPbkdf2(plaintext, xySecurityDefaults.Pbkdf2Iterations, xySecurityDefaults.SaltSizeBytes, xySecurityDefaults.KeySizeBytes);

        public static string HashPbkdf2(string plaintext, int iterations, int saltSizeBytes, int keySizeBytes)
        {
            ArgumentNullException.ThrowIfNull(plaintext);
            var salt = RandomNumberGenerator.GetBytes(saltSizeBytes);
            var key = Rfc2898DeriveBytes.Pbkdf2(
                plaintext,
                salt,
                iterations,
                HashAlgorithmName.SHA256,
                keySizeBytes);

            // Format: pbkdf2-sha256$iter$saltBase64$keyBase64
            return $"pbkdf2-sha256${iterations}${Convert.ToBase64String(salt)}${Convert.ToBase64String(key)}";
        }

        #endregion

        #region Password Verification

        public static bool VerifyPbkdf2(string plaintext, string hash)
        {
            ArgumentNullException.ThrowIfNull(plaintext);
            ArgumentNullException.ThrowIfNull(hash);

            var parts = hash.Split('$');
            if (parts.Length != 4 || parts[0] != "pbkdf2-sha256") return false;

            var iterations = int.Parse(parts[1], CultureInfo.InvariantCulture);
            var salt = Convert.FromBase64String(parts[2]);
            var expected = Convert.FromBase64String(parts[3]);

            var actual = Rfc2898DeriveBytes.Pbkdf2(
                plaintext, salt, iterations, HashAlgorithmName.SHA256, expected.Length);

            return CryptographicOperations.FixedTimeEquals(actual, expected);
        }
        /// <summary>
        /// Verifies a plaintext password against a stored salted hash in the format "salt:hash".
        /// </summary>
        /// <param name="hashAlgorithm">The hash algorithm used originally to compute the hash (e.g., SHA256).</param>
        /// <param name="password">The plaintext password to verify.</param>
        /// <param name="saltNhash">The combined salt and hash string, separated by a colon ("salt:hash").</param>
        /// <returns>True if the password is valid; otherwise, false.</returns>
        public static bool VerifyPassword(HashAlgorithmName hashAlgorithm, string password, string saltNhash)
        {
            string bug = "This text is unreachable, please stay calm and call a supervisor";
            string logInvalidFormat = "Invalid format in saltNhash.";
            string logSaltNull = "Salt is null or empty.";
            string logPasswordNull = "Password is null or empty.";
            string[] input = [];

            byte[] hashToCheck = default!;
            byte[] hash = default!;
            byte[] salt = default!;
            bool isValid = false;
            bool isFormat = false;
            bool isSaltValid = false;
            bool isPwNull = string.IsNullOrWhiteSpace(password);
            bool isCheckNull = string.IsNullOrWhiteSpace(saltNhash);

            if (!isPwNull &&  !isCheckNull)
            {
                input = saltNhash.Split(Separator);
                isFormat = input.Length == 2;
                if (isFormat)
                {
                    salt = xyEncoder.BaseToBytes(input[0]);
                    hash = xyEncoder.BaseToBytes(input[1]);
                    isSaltValid = salt is not null && salt.Length > 0;
                    if (isSaltValid)
                    {
                        hashToCheck = HashToBytes(hashAlgorithm, password, salt!);
                        isValid = CryptographicOperations.FixedTimeEquals(hash, hashToCheck);
                    }
                }
            }     
            xyLog.Log(!isSaltValid? logSaltNull :!isFormat ? logInvalidFormat : isPwNull ? logPasswordNull : bug);
            return isValid;
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
            string nullHash1 = "First array is null or empty.";
            string nullHash2 = "Second hash is null or empty.";
            string lengthNotEqual = "Arrays have different length and cant be compared.";
            string bugMessage = "Unexpected level reached. Please investigate somewhere else.";

            bool isHash2Valid = hash2 is not null && hash2.Length > 0;
            bool isHash1Valid = hash1 is not null && hash1.Length > 0;
            bool isSameLength = isHash1Valid && isHash2Valid && hash1!.Length == hash2!.Length; 
            bool result = false;

            if (isHash1Valid && isHash2Valid)
            {
                if (isSameLength)
                {
                    result= CryptographicOperations.FixedTimeEquals(hash1, hash2);
                }
            }  
            xyLog.Log(!isHash1Valid ? nullHash1 : !isHash2Valid ? nullHash2 : !isSameLength? lengthNotEqual : bugMessage);
            return result;
        }

        #endregion

        #region Salt & Helper Functions

        /// <summary>
        /// Generates a secure random Salt, using a cryptographically strong RNG.
        /// </summary>
        /// <param name="length">The length of the salt in bytes: 16 or more.</param>
        /// <returns>A secure salt, or an empty array if the generation fails.</returns>
        public static byte[] GenerateSalt(int length = 32)
        {
            string tooShort = "WARNING: Salt length too short. Minimum requirement: 16 bytes.";

            if (length < 16)
            {
                xyLog.Log(tooShort);
                return Array.Empty<byte>();
            }

            try
            {
                byte[] salt = new byte[length];
                using RandomNumberGenerator rng = RandomNumberGenerator.Create();
                rng.GetBytes(salt);
                return salt;
            }
            catch (Exception ex)
            {
    
                xyLog.ExLog(ex);
                return Array.Empty<byte>();
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
            try
            {
                salt = GenerateSalt();
                string hash = HashPassword(hashAlgorithm, password, salt);
                string saltString = Convert.ToBase64String(salt);
                return string.Concat(saltString, Separator, hash);
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
            string unsupAlgoMsg = "Unsupported algorithm...";
            try
            {
                return algorithm switch                                                                                     // Super Sache!!!
                {
                    { } a when a == HashAlgorithmName.SHA256 => KeyLength256,                   
                    { } a when a == HashAlgorithmName.SHA512 => KeyLength512,                   // {} a when a ==           ist also ein gültiges Objekt => nicht null
                    _ => throw new ArgumentException(unsupAlgoMsg, nameof(algorithm))      // Für alles, was nicht abgedeckt wurde
                };

       
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
            string logPasswordMissing = "Password was null or empty. Using only pepper.";

            if (string.IsNullOrWhiteSpace(password))
            {
                xyLog.Log(logPasswordMissing);
                return Pepper ?? string.Empty;
            }

            return string.Concat(password, Pepper);
        }

        #endregion
    }
}
