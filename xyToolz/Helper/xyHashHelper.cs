using System.Security.Cryptography;

namespace xyToolz.Helper
{
    /// <summary>
    /// Hash and check passwords based on [Rfc2898DeriveBytes]
    /// </summary>
    public static class xyHashHelper
    {
        #region "Member"

        /// <summary>
        /// Populate with environment variable when using, default is "Ahuhu"...
        /// </summary>
        public static String? Pepper { get; private set; }

        /// <summary>
        /// SHA256  =  32 Bytes   
        /// </summary>
        private const UInt16 KeyLength256 = 32;

        /// <summary>
        /// SHA512  =  64 Bytes    -->   Base64 => 86 characters (88 with padding)
        /// </summary>
        private const UInt16 KeyLength512 = 64;

        /// <summary>
        /// Number of iterations for the generator to choose the key from
        /// </summary>
        public static Int32 Iterations { get; private set; } = 100000;

        private static HashAlgorithmName Sha256 = HashAlgorithmName.SHA256;
        private static HashAlgorithmName Sha512 = HashAlgorithmName.SHA512;

        #endregion

        #region "Hashing"

        /// <summary>
        /// Hashes the given pasword in the specified algorithm
        /// </summary>
        /// <param name="hashAlgorithm"></param>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns>hashed password as base64 string</returns>
        public static String HashPassword(HashAlgorithmName hashAlgorithm, String password, Byte[] salt)
        {
            String hashedPasswordString = "";
            int length = SetKeySize(hashAlgorithm);
            String pepperedPassword = PepperPassword(password);
            Byte[] hash = new Byte[length];

            return HashToString(hashAlgorithm, password, salt);
        }

        /// <summary>
        /// Hash the given password in  the specified algorithm, without creating local variables 
        /// </summary>
        /// <param name="hashAlgorithm"></param>
        /// <param name="password"></param>
        /// <returns>
        /// Byte[] hashedPassword
        /// </returns>
        public static byte[] HashToBytes(HashAlgorithmName hashAlgorithm, String password, Byte[] salt)
        => (new Rfc2898DeriveBytes(PepperPassword(password), salt, Iterations, Sha256)).GetBytes(SetKeySize(hashAlgorithm));

        /// <summary>
        /// Hash the given password in  the specified algorithm, without creating local variables
        /// </summary>
        /// <param name="hashAlgorithm"></param>
        /// <param name="password"></param>
        /// <returns> base64 string     password</returns>
        public static String HashToString(HashAlgorithmName hashAlgorithm, String password, Byte[] salt)
        => Convert.ToBase64String((new Rfc2898DeriveBytes(PepperPassword(password), salt, Iterations, Sha256)).GetBytes(SetKeySize(hashAlgorithm)));

        /// <summary>
        /// Hashes the given pasword in the specified algorithm and returns hash and salt as base64 string
        /// </summary>
        /// <param name="hashAlgorithm"></param>
        /// <param name="password"></param>
        /// <returns>salt:hash</returns>
        public static String HashPasswordGetSaltAndHash(HashAlgorithmName hashAlgorithm, string password)
        {
            string saltNhash = "";
            int length = SetKeySize(hashAlgorithm);
            String pepperedPassword = PepperPassword(password);
            Byte[] salt = GetSalt((UInt16)(length / 2));
            Byte[] hash = new Byte[length];

            using (var pbkdf2 = new Rfc2898DeriveBytes(pepperedPassword, salt, Iterations, hashAlgorithm))
            {
                hash = pbkdf2.GetBytes(length);
                saltNhash = $"{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";
                return saltNhash;
            }
        }

        #endregion

        #region "Verification"
        /// <summary>
        /// Verify the password
        /// </summary>
        /// <param name="hashAlgorithm"></param>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static Boolean VerifyPassword(HashAlgorithmName hashAlgorithm, String password, Byte[] salt) => CryptographicOperations.FixedTimeEquals(Convert.FromBase64String(password), HashToBytes(hashAlgorithm, password, salt));

        /// <summary>
        /// This is the same function as the others but working step by step and using local variables
        /// </summary>
        /// <param name="hashAlgorithm"></param>
        /// <param name="password"></param>
        /// <param name="saltNhash"></param>
        /// <returns></returns>
        public static Boolean VerifyPassword(HashAlgorithmName hashAlgorithm, String password, String saltNhash)
        {
            String[] input = saltNhash.Split(':');
            if (input.Length != 2) return false;

            Byte[] salt = Convert.FromBase64String(input[0]);
            Byte[] hash = Convert.FromBase64String(input[1]);
            Byte[] hashToCheck = HashToBytes(hashAlgorithm, password, salt);

            return CryptographicOperations.FixedTimeEquals(hash, hashToCheck);
        }

        #endregion

        #region "Misc"
        /// <summary>
        /// Create an array of cryptographically strong random values
        /// </summary>
        /// <returns></returns>
        public static Byte[] GetSalt(UInt16 saltLength) => RandomNumberGenerator.GetBytes((int)saltLength);

        /// <summary>
        /// Checks the given algorithm and returns the according length in Bytes
        /// </summary>
        /// <param name="hashAlgorithm"></param>
        /// <returns></returns>
        private static Int32 SetKeySize(HashAlgorithmName hashAlgorithm)
        {
            if (hashAlgorithm == HashAlgorithmName.SHA256)
            {
                return KeyLength256;
            }
            if (hashAlgorithm == HashAlgorithmName.SHA512)
            {
                return KeyLength512;
            }
            else
            {
                xyLog.Log("No specific algorithm detected, default to secure alternative.");
                return 128;
            }
        }
        #endregion

        #region "Pepper"
        /// <summary>
        /// Get the value for a specific environment variable 
        /// </summary>
        /// <param name="namePepperEnvVar"></param>
        /// <returns>The value of the EnvVar</returns>
        private static String? GetValueByFromEnviromentVariable(string namePepperEnvVar)
        {
            string? environmentVariable = "";
            try
            {
                environmentVariable = Environment.GetEnvironmentVariable(namePepperEnvVar);
            }
            catch (Exception ex)
            {
                xyLog.ExLog(ex);
            }
            return environmentVariable;
        }

        /// <summary>
        /// Pepper the password with the value retrieved from the environment variable
        /// </summary>
        /// <param name="password"></param>
        /// <param name="namePepperEnvVar"></param>
        /// <returns></returns>
        private static String PepperPasswordWithEnvVar(string password, string namePepperEnvVar) => password + GetValueByFromEnviromentVariable(namePepperEnvVar);

        /// <summary>
        /// Set member "pepper" to the value of the targeted environment variable or if thats not availlable, the given string in parameter
        /// </summary>
        /// <param name="pepperVarName"></param>
        /// <param name="alternativeValueForMember"></param>
        private static void SetPepper(string pepperVarName, string alternativeValueForMember = "Ahuhu") => Pepper = GetValueByFromEnviromentVariable(pepperVarName) ?? alternativeValueForMember;

        /// <summary>
        /// Concat password and "Pepper" - member
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        private static String PepperPassword(string password) => password + Pepper;
        #endregion

        //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

        #region "Not in Use"
        /// <summary>
        /// Hashes the entered password based on [Rfc2898DeriveBytes(SHA512)]  
        /// working with its own local variables, if needed for debugging
        /// </summary>
        /// <param name="password"></param>
        /// <returns>hash</returns>
        public static String HashPassword512(String password)
        {
            UInt16 length = KeyLength512;
            Byte[] salt = RandomNumberGenerator.GetBytes(length / 2);
            Byte[] hash = new Byte[length];
            String pepperedPassword = PepperPassword(password);
            String hashedPassword = "";

            using (Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(pepperedPassword, salt, Iterations, Sha512))
            {
                hash = pbkdf2.GetBytes(KeyLength512);
                hashedPassword = Convert.ToBase64String(hash);
                return hashedPassword;
            }
        }

        /// <summary>
        /// Hashes the entered password based on [Rfc2898DeriveBytes(SHA256)]  
        /// working with its own local variables, if needed for debugging
        /// </summary>
        /// <param name="password"></param>
        /// <returns>hash</returns>
        public static String HashPassword256(String password)
        {
            UInt16 length = KeyLength256;
            Byte[] salt = RandomNumberGenerator.GetBytes(length / 2);
            Byte[] hash = new Byte[length];
            String pepperedPassword = PepperPassword(password);
            String hashedPassword = "";

            using (Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(pepperedPassword, salt, Iterations, Sha256))
            {
                hash = pbkdf2.GetBytes(KeyLength256);
                hashedPassword = Convert.ToBase64String(hash);
                return hashedPassword;
            }
        }

        /// <summary>
        /// Hashes the given pasword in the specified algorithm and returns hash and salt as base64 string without declaring additional variables
        /// </summary>
        /// <param name="hashAlgorithm"></param>
        /// <param name="password"></param>
        /// <returns>salt:hash</returns>
        public static String HashPwGetSaltHash(HashAlgorithmName hashAlgorithm, String password, Byte[] salt)
        => $"{Convert.ToBase64String(salt)}:{Convert.ToBase64String(new Rfc2898DeriveBytes(PepperPassword(password), GetSalt((UInt16)salt.Length), Iterations, hashAlgorithm).GetBytes(SetKeySize(hashAlgorithm)))}";

        /// <summary>
        /// Returns either the value of the target env_var      or      the value of the member "Pepper"
        /// </summary>
        /// <param name="pepperName"></param>
        /// <returns></returns>
        private static String GetPepper(string pepperName) => GetValueByFromEnviromentVariable(pepperName) ?? Pepper;

        /// <summary>
        /// Verifiy the password
        /// </summary>
        /// <param name="hashAlgorithm"></param>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static Boolean VerifyPassword(HashAlgorithmName hashAlgorithm, Byte[] password, Byte[] salt) => CryptographicOperations.FixedTimeEquals(password, HashToBytes(hashAlgorithm, Convert.ToBase64String(password), salt));

        /// <summary>
        /// Verify the password
        /// </summary>
        /// <param name="hashAlgorithm"></param>
        /// <param name="password"></param>
        /// <param name="saltNhash"></param>
        /// <param name="useless"></param>
        /// <returns></returns>
        public static Boolean VerifyPassword(HashAlgorithmName hashAlgorithm, String password, String saltNhash, string? useless = "")
        => CryptographicOperations.FixedTimeEquals(Convert.FromBase64String(saltNhash.Split(':').First()), HashToBytes(hashAlgorithm, password, Convert.FromBase64String(saltNhash.Split(':').Last())));

        #endregion

    }
}

