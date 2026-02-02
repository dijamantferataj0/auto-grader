namespace AutoGrader.Domain.Models;

public class GradingResult
{
    public int Id { get; set; }
    public int TaskId { get; set; }
    public TaskItem Task { get; set; } = null!;
    public bool IsCorrect { get; set; }
    public double CalculatedValue { get; set; }
    public double ExpectedValue { get; set; }
    public DateTime GradedAt { get; set; }
}
