using System.Security.Cryptography;
using xyToolz.Security;

public static class Keys
{
    public static Task<string> GetPublicKeyAsync()
        => xyRsa.GetPublicKeyAsPemAsync();

    public static byte[] Encrypt(byte[] data, RSA rsa)
        => xyRsa.Encrypt(data, rsa);

    public static byte[] Decrypt(byte[] cipher, RSA rsa)
        => xyRsa.Decrypt(cipher, rsa);
}
