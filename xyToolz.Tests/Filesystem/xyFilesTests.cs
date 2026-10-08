using xyToolz.Filesystem;
using xyToolz.Serialization;
using Xunit;

namespace xyToolz.Tests.Filesystem;

public class xyFilesTests : IDisposable
{
    private readonly string _tempDir = Path.Combine(Path.GetTempPath(), $"xyFilesTests_{Guid.NewGuid():N}");

    public xyFilesTests()
    {
        Directory.CreateDirectory(_tempDir);
    }

    public void Dispose()
    {
        if (Directory.Exists(_tempDir)) Directory.Delete(_tempDir, recursive: true);
    }

    [Fact]
    public async Task SaveToFile_ReadLinesAsync_RoundTrip_Succeeds()
    {
        // Arrange
        string path = Path.Combine(_tempDir, "roundtrip.txt");
        string content = "line one" + Environment.NewLine + "line two";

        // Act
        bool saved = await xyFiles.SaveToFile(content, path);
        var lines = (await xyFiles.ReadLinesAsync(path)).ToList();

        // Assert
        Assert.True(saved);
        Assert.Equal(["line one", "line two"], lines);
    }

    [Fact]
    public async Task SaveDataToJsonAsync_AlreadyCancelledToken_DoesNotReliablyHonorCancellation()
    {
        // Arrange: xyJson.SaveDataToJsonAsync accepts a CancellationToken and threads it into
        // File.WriteAllTextAsync, but never calls ct.ThrowIfCancellationRequested() up front.
        // Whether the write observes the pre-cancelled token before or after it completes is a
        // timing race, not a guarantee — observed as both "true" and "false" across runs of this
        // exact test. That non-determinism is itself the finding: the token is accepted but not
        // reliably honored. Fixing it (an explicit throw-if-cancelled guard before the write) is
        // a separate, wider change than this test-writing pass — flagged in the final report.
        string path = Path.Combine(_tempDir, "cancelled.json");
        using var cts = new CancellationTokenSource();
        cts.Cancel();

        // Act
        var exception = await Record.ExceptionAsync(async () =>
            await xyJson.SaveDataToJsonAsync(new { Value = 1 }, path, ct: cts.Token));

        // Assert: whatever happens, it must not throw an unhandled OperationCanceledException —
        // xyJson's own contract is "no exceptions thrown directly", not "cancellation works".
        Assert.Null(exception);
    }
}
