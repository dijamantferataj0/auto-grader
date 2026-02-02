namespace AutoGrader.Api.DTOs;

public class ExamSummaryDto
{
    public int ExamId { get; set; }
    public int StudentId { get; set; }
    public int TotalTasks { get; set; }
    public int CorrectTasks { get; set; }
    public double ScorePercentage { get; set; }
}
