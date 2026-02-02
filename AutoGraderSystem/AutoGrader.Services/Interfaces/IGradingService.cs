using AutoGrader.Domain.Models;

namespace AutoGrader.Services.Interfaces;

public interface IGradingService
{
    Task<List<GradingResult>> GradeExamAsync(Exam exam);
    Task<List<GradingResult>> GradeMultipleExamsAsync(List<Exam> exams);
}
