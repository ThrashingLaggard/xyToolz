using xyToolz.Serialization;
using Xunit;

namespace xyToolz.Tests.Serialization;

public class xyJsonTests : IDisposable
{
    private readonly string _tempFile = Path.Combine(Path.GetTempPath(), $"xyJsonTests_{Guid.NewGuid():N}.json");

    public void Dispose()
    {
        if (File.Exists(_tempFile)) File.Delete(_tempFile);
    }

    private sealed record Sample(string Name, int Value);

    [Fact]
    public async Task SaveDataToJsonAsync_DeserializeFromFile_RoundTrip_Succeeds()
    {
        // Arrange
        var original = new Sample("widget", 42);

        // Act
        bool saved = await xyJson.SaveDataToJsonAsync(original, _tempFile);
        Sample? loaded = await xyJson.DeserializeFromFile<Sample>(_tempFile);

        // Assert
        Assert.True(saved);
        Assert.NotNull(loaded);
        Assert.Equal(original.Name, loaded!.Name);
        Assert.Equal(original.Value, loaded.Value);
    }
}
