using xyToolz.Security;
using Xunit;

namespace xyToolz.Tests.Security;

public class xyHasherTests
{
    [Fact]
    public void HashPbkdf2_VerifyPbkdf2_RoundTrip_Succeeds()
    {
        // Arrange
        string password = "Correct-Horse-Battery-Staple-42";

        // Act
        string hash = xyHasher.HashPbkdf2(password);
        bool isValid = xyHasher.VerifyPbkdf2(password, hash);

        // Assert
        Assert.True(isValid);
    }

    [Fact]
    public void VerifyPbkdf2_WrongPassword_IsRejected()
    {
        // Arrange
        string hash = xyHasher.HashPbkdf2("the-real-password");

        // Act
        bool isValid = xyHasher.VerifyPbkdf2("not-the-real-password", hash);

        // Assert
        Assert.False(isValid);
    }

    [Fact]
    public void BuildSaltedHash_VerifyPassword_RoundTrip_Succeeds()
    {
        // Arrange
        string password = "another-strong-password";

        // Act
        string saltedHash = xyHasher.BuildSaltedHash(System.Security.Cryptography.HashAlgorithmName.SHA256, password, out byte[] salt);
        bool isValid = xyHasher.VerifyPassword(System.Security.Cryptography.HashAlgorithmName.SHA256, password, saltedHash);

        // Assert
        Assert.NotEmpty(salt);
        Assert.True(isValid);
    }

    [Fact]
    public void BuildSaltedHash_VerifyPassword_WrongPassword_IsRejected()
    {
        // Arrange
        string saltedHash = xyHasher.BuildSaltedHash(System.Security.Cryptography.HashAlgorithmName.SHA256, "right-password", out _);

        // Act
        bool isValid = xyHasher.VerifyPassword(System.Security.Cryptography.HashAlgorithmName.SHA256, "wrong-password", saltedHash);

        // Assert
        Assert.False(isValid);
    }
}
