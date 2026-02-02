using AutoGrader.Domain.Models;
using AutoGrader.Persistence.Interfaces;
using AutoGrader.Services.Interfaces;

namespace AutoGrader.Services.Services;

public class GradingService : IGradingService
{
    private readonly IMathEvaluatorService _mathEvaluator;
    private readonly IRepository<GradingResult> _gradingResultRepository;

    public GradingService(IMathEvaluatorService mathEvaluator, IRepository<GradingResult> gradingResultRepository)
    {
        _mathEvaluator = mathEvaluator;
        _gradingResultRepository = gradingResultRepository;
    }

    public async Task<List<GradingResult>> GradeExamAsync(Exam exam)
    {
        var results = new List<GradingResult>();

        foreach (var task in exam.Tasks)
        {
            var result = GradeTask(task);
            results.Add(result);
            await _gradingResultRepository.AddAsync(result);
        }

        await _gradingResultRepository.SaveChangesAsync();
        return results;
    }

    public async Task<List<GradingResult>> GradeMultipleExamsAsync(List<Exam> exams)
    {
        var allResults = new List<GradingResult>();

        foreach (var exam in exams)
        {
            var results = await GradeExamAsync(exam);
            allResults.AddRange(results);
        }

        return allResults;
    }

    private GradingResult GradeTask(TaskItem task)
    {
        var parts = task.Expression.Split('=');
        if (parts.Length != 2)
        {
            return new GradingResult
            {
                TaskId = task.Id,
                Task = task,
                IsCorrect = false,
                CalculatedValue = 0,
                ExpectedValue = 0,
                GradedAt = DateTime.UtcNow
            };
        }

        var expressionPart = parts[0].Trim();
        var expectedPart = parts[1].Trim();

        double calculatedValue = 0;
        double expectedValue = 0;

        bool calculationSuccess = _mathEvaluator.TryEvaluate(expressionPart, out calculatedValue);
        bool expectedSuccess = double.TryParse(expectedPart, out expectedValue);

        if (!calculationSuccess || !expectedSuccess)
        {
            return new GradingResult
            {
                TaskId = task.Id,
                Task = task,
                IsCorrect = false,
                CalculatedValue = calculatedValue,
                ExpectedValue = expectedValue,
                GradedAt = DateTime.UtcNow
            };
        }

        bool isCorrect = Math.Abs(calculatedValue - expectedValue) < 0.0001;

        return new GradingResult
        {
            TaskId = task.Id,
            Task = task,
            IsCorrect = isCorrect,
            CalculatedValue = calculatedValue,
            ExpectedValue = expectedValue,
            GradedAt = DateTime.UtcNow
        };
    }
}
