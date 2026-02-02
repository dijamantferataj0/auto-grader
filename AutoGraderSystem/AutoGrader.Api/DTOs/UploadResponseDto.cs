namespace AutoGrader.Api.DTOs;

public class UploadResponseDto
{
    public int TeacherId { get; set; }
    public int ProcessedStudents { get; set; }
    public int TotalExams { get; set; }
    public List<ExamSummaryDto> Summaries { get; set; } = new();
}
