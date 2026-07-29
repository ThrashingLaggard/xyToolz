using xyToolz.Serialization;
using Xunit;

namespace xyToolz.Tests.Serialization;

public class xyXmlTests
{
    public class Sample
    {
        public string Name { get; set; } = string.Empty;
        public int Value { get; set; }
    }

    [Fact]
    public void ToXML_FromXml_RoundTrip_Succeeds()
    {
        // Arrange
        var original = new Sample { Name = "widget", Value = 42 };

        // Act
        string xml = xyXml.ToXML(original);
        Sample loaded = xyXml.FromXml<Sample>(xml);

        // Assert
        Assert.Equal(original.Name, loaded.Name);
        Assert.Equal(original.Value, loaded.Value);
    }
}
