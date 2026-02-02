namespace AutoGrader.Api.DTOs;

public class MathEvaluationResponseDto
{
    public double Result { get; set; }
    public bool IsValid { get; set; }
    public string? ErrorMessage { get; set; }
}
