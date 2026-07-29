using xyToolz.Filesystem;
using Xunit;

namespace xyToolz.Tests.Filesystem;

public class xyPathTests : IDisposable
{
    private readonly string _baseDir = Path.Combine(Path.GetTempPath(), $"xyPathTests_{Guid.NewGuid():N}");

    public xyPathTests()
    {
        Directory.CreateDirectory(_baseDir);
    }

    public void Dispose()
    {
        if (Directory.Exists(_baseDir)) Directory.Delete(_baseDir, recursive: true);
    }

    [Fact]
    public void EnsureNoTraversal_PathInsideBase_ReturnsResolvedPath()
    {
        // Act
        string result = xyPath.EnsureNoTraversal(_baseDir, "subfolder/file.txt");

        // Assert
        Assert.StartsWith(Path.GetFullPath(_baseDir), result);
    }

    [Fact]
    public void EnsureNoTraversal_PathEscapingBase_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => xyPath.EnsureNoTraversal(_baseDir, "../../escape.txt"));
    }
}
