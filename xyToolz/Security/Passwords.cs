using System.Security.Cryptography;

namespace xyToolz.Security;

public static class Passwords
{
    // modern default
    public static string Hash(string password)
        => xyHasher.HashPbkdf2(password);

    public static bool Verify(string password, string hash)
        => xyHasher.VerifyPbkdf2(password, hash);

    // explicit / legacy
    public static string CreateSaltedHash(
        HashAlgorithmName algorithm,
        string password,
        out byte[] salt)
        => xyHasher.BuildSaltedHash(algorithm, password, out salt);

    public static bool VerifySalted(
        HashAlgorithmName algorithm,
        string password,
        string saltAndHash)
        => xyHasher.VerifyPassword(algorithm, password, saltAndHash);

    /// <summary>
    /// Basically legacy since modern methods do it themselves
    /// </summary>
    /// <param name="size"></param>
    /// <returns></returns>
    public static byte[] GenerateSalt(int size = 32)
        => xyHasher.GenerateSalt(size);
}
