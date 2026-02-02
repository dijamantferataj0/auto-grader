using AutoGrader.Domain.Models;
using AutoGrader.Services.Interfaces;
using AutoGrader.Services.Services;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace AutoGrader.Tests;

public class XmlParserServiceTests
{
    private readonly IXmlParserService _parserService;

    public XmlParserServiceTests()
    {
        _parserService = new XmlParserService();
    }

    [Fact]
    public async System.Threading.Tasks.Task ParseXmlAsync_ValidXml_ReturnsTeacher()
    {
        // Arrange
        var xml = @"<Teacher ID=""11111"">
  <Students>
    <Student ID=""12345"">
      <Exam Id=""1"">
        <Task id=""1"">2+3/6-4 = 74</Task>
        <Task id=""2"">6*2+3-4 = 22</Task>
      </Exam>
    </Student>
  </Students>
</Teacher>";
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(xml));

        // Act
        var result = await _parserService.ParseXmlAsync(stream);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(11111, result.Id);
        Assert.Single(result.Students);
        Assert.Equal(12345, result.Students[0].Id);
        Assert.Single(result.Students[0].Exams);
        Assert.Equal(1, result.Students[0].Exams[0].Id);
        Assert.Equal(2, result.Students[0].Exams[0].Tasks.Count);
        Assert.Equal("2+3/6-4 = 74", result.Students[0].Exams[0].Tasks[0].Expression);
        Assert.Equal("6*2+3-4 = 22", result.Students[0].Exams[0].Tasks[1].Expression);
    }

    [Fact]
    public async System.Threading.Tasks.Task ParseXmlAsync_MultipleStudents_ReturnsAllStudents()
    {
        // Arrange
        var xml = @"<Teacher ID=""11111"">
  <Students>
    <Student ID=""12345"">
      <Exam Id=""1"">
        <Task id=""1"">2+3 = 5</Task>
      </Exam>
    </Student>
    <Student ID=""67890"">
      <Exam Id=""1"">
        <Task id=""1"">3+4 = 7</Task>
      </Exam>
    </Student>
  </Students>
</Teacher>";
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(xml));

        // Act
        var result = await _parserService.ParseXmlAsync(stream);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Students.Count);
        Assert.Equal(12345, result.Students[0].Id);
        Assert.Equal(67890, result.Students[1].Id);
    }

    [Fact]
    public async System.Threading.Tasks.Task ParseXmlAsync_MalformedXml_ThrowsException()
    {
        // Arrange
        var xml = @"<Teacher ID=""11111"">
  <Students>
    <Student ID=""12345"">
      <Exam Id=""1"">
        <Task id=""1"">2+3 = 5</Task>
      </Exam>
    </Student>
  </Students>
</Teache"; // Missing closing tag
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(xml));

        // Act & Assert
        await Assert.ThrowsAsync<XmlException>(() => _parserService.ParseXmlAsync(stream));
    }

    [Fact]
    public async System.Threading.Tasks.Task ParseXmlAsync_InvalidStructure_ThrowsException()
    {
        // Arrange
        var xml = @"<InvalidRoot>
  <SomeElement>Test</SomeElement>
</InvalidRoot>";
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(xml));

        // Act & Assert
        await Assert.ThrowsAnyAsync<Exception>(() => _parserService.ParseXmlAsync(stream));
    }

    [Fact]
    public async System.Threading.Tasks.Task ParseXmlAsync_WhitespaceInExpression_PreservesExpression()
    {
        // Arrange
        var xml = @"<Teacher ID=""11111"">
  <Students>
    <Student ID=""12345"">
      <Exam Id=""1"">
        <Task id=""1"">  2 + 3 / 6 - 4  =  74  </Task>
      </Exam>
    </Student>
  </Students>
</Teacher>";
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(xml));

        // Act
        var result = await _parserService.ParseXmlAsync(stream);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("  2 + 3 / 6 - 4  =  74  ", result.Students[0].Exams[0].Tasks[0].Expression);
    }

    [Fact]
    public async System.Threading.Tasks.Task ValidateXmlAsync_ValidXml_ReturnsTrue()
    {
        // Arrange
        var xml = @"<Teacher ID=""11111"">
  <Students>
    <Student ID=""12345"">
      <Exam Id=""1"">
        <Task id=""1"">2+3 = 5</Task>
      </Exam>
    </Student>
  </Students>
</Teacher>";
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(xml));

        // Act
        var result = await _parserService.ValidateXmlAsync(stream);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async System.Threading.Tasks.Task ValidateXmlAsync_MissingRequiredAttribute_ThrowsException()
    {
        // Arrange
        var xml = @"<Teacher>
  <Students>
    <Student ID=""12345"">
      <Exam Id=""1"">
        <Task id=""1"">2+3 = 5</Task>
      </Exam>
    </Student>
  </Students>
</Teacher>";
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(xml));

        // Act & Assert
        await Assert.ThrowsAnyAsync<Exception>(() => _parserService.ValidateXmlAsync(stream));
    }
}
