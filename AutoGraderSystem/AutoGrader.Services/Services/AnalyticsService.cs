using AutoGrader.Domain.Models;
using AutoGrader.Persistence.Interfaces;
using AutoGrader.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AutoGrader.Services.Services;

public class AnalyticsService : IAnalyticsService
{
    private readonly IRepository<GradingResult> _gradingResultRepository;
    private readonly IRepository<ExamSummary> _examSummaryRepository;
    private readonly IRepository<Exam> _examRepository;

    public AnalyticsService(
        IRepository<GradingResult> gradingResultRepository,
        IRepository<ExamSummary> examSummaryRepository,
        IRepository<Exam> examRepository)
    {
        _gradingResultRepository = gradingResultRepository;
        _examSummaryRepository = examSummaryRepository;
        _examRepository = examRepository;
    }

    public async Task<ExamSummary> GetExamSummaryAsync(int examId)
    {
        var existingSummaries = await _examSummaryRepository.FindAsync(s => s.ExamId == examId);
        var existingSummary = existingSummaries.FirstOrDefault();

        if (existingSummary != null)
        {
            return existingSummary;
        }

        var exam = await _examRepository.GetByIdAsync(examId);
        if (exam == null)
        {
            throw new Exception($"Exam with ID {examId} not found");
        }

        var results = await _gradingResultRepository.FindAsync(r => r.Task.ExamId == examId);
        var resultsList = results.ToList();

        var summary = new ExamSummary
        {
            ExamId = examId,
            StudentId = exam.StudentId,
            TotalTasks = resultsList.Count,
            CorrectTasks = resultsList.Count(r => r.IsCorrect),
            ScorePercentage = resultsList.Count > 0
                ? (double)resultsList.Count(r => r.IsCorrect) / resultsList.Count * 100
                : 0,
            ProcessedAt = DateTime.UtcNow
        };

        await _examSummaryRepository.AddAsync(summary);
        await _examSummaryRepository.SaveChangesAsync();

        return summary;
    }

    public async Task<List<ExamSummary>> GetStudentAnalyticsAsync(int studentId)
    {
        var summaries = await _examSummaryRepository.FindAsync(s => s.StudentId == studentId);
        return summaries.ToList();
    }

    public async Task<List<GradingResult>> GetExamDetailsAsync(int examId)
    {
        var results = await _gradingResultRepository.FindWithIncludesAsync(
            r => r.Task.ExamId == examId,
            r => r.Task);
        return results.ToList();
    }
}
