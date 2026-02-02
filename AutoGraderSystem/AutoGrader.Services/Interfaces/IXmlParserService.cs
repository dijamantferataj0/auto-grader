using AutoGrader.Domain.Models;

namespace AutoGrader.Services.Interfaces;

public interface IXmlParserService
{
    Task<Teacher> ParseXmlAsync(Stream xmlStream);
    Task<bool> ValidateXmlAsync(Stream xmlStream);
}
