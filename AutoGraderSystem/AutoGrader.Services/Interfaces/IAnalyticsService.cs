using AutoGrader.Domain.Models;

namespace AutoGrader.Services.Interfaces;

public interface IAnalyticsService
{
    Task<ExamSummary> GetExamSummaryAsync(int examId);
    Task<List<ExamSummary>> GetStudentAnalyticsAsync(int studentId);
    Task<List<GradingResult>> GetExamDetailsAsync(int examId);
}
