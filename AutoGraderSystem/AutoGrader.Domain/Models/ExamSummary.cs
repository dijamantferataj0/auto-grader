namespace AutoGrader.Domain.Models;

public class ExamSummary
{
    public int Id { get; set; }
    public int ExamId { get; set; }
    public int StudentId { get; set; }
    public int TotalTasks { get; set; }
    public int CorrectTasks { get; set; }
    public double ScorePercentage { get; set; }
    public DateTime ProcessedAt { get; set; }
}
