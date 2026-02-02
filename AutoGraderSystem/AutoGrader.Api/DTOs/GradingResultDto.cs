namespace AutoGrader.Api.DTOs;

public class GradingResultDto
{
    public int TaskId { get; set; }
    public string Expression { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
    public double CalculatedValue { get; set; }
    public double ExpectedValue { get; set; }
}
