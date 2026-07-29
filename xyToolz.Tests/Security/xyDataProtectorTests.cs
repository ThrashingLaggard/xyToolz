using xyToolz.Security;
using Xunit;

namespace xyToolz.Tests.Security;

public class xyDataProtectorTests : IDisposable
{
    private readonly string _tempFile = Path.Combine(Path.GetTempPath(), $"xyDataProtectorTests_{Guid.NewGuid():N}.protected");

    public void Dispose()
    {
        if (File.Exists(_tempFile)) File.Delete(_tempFile);
    }

    [Fact]
    public async Task SaveProtectedToFileAsync_LoadProtectedFromFileAsync_RoundTrip_Succeeds()
    {
        // Arrange: SaveProtectedToFileAsync base64-encodes the DPAPI ciphertext and writes it as
        // text, but LoadProtectedFromFileAsync used to read the file's raw bytes directly without
        // base64-decoding first, so every load failed with CryptographicException even immediately
        // after a successful save on the same machine/user. This test guards that round-trip.
        string secret = "fake-test-key-not-real-oxb00-1234";

        // Act
        bool saved = await xyDataProtector.SaveProtectedToFileAsync(secret, _tempFile);
        string? loaded = await xyDataProtector.LoadProtectedFromFileAsync<string>(_tempFile);

        // Assert
        Assert.True(saved);
        Assert.Equal(secret, loaded);
    }
}
