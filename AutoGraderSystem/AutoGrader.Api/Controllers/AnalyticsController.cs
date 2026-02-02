using AutoGrader.Api.DTOs;
using AutoGrader.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AutoGrader.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnalyticsController : ControllerBase
{
    private readonly IAnalyticsService _analyticsService;

    public AnalyticsController(IAnalyticsService analyticsService)
    {
        _analyticsService = analyticsService;
    }

    [HttpGet("student/{studentId}")]
    public async Task<ActionResult<List<ExamSummaryDto>>> GetStudentAnalytics(int studentId)
    {
        try
        {
            var summaries = await _analyticsService.GetStudentAnalyticsAsync(studentId);

            var dtos = summaries.Select(s => new ExamSummaryDto
            {
                ExamId = s.ExamId,
                StudentId = s.StudentId,
                TotalTasks = s.TotalTasks,
                CorrectTasks = s.CorrectTasks,
                ScorePercentage = s.ScorePercentage
            }).ToList();

            return Ok(dtos);
        }
        catch (Exception ex)
        {
            return NotFound($"Error retrieving student analytics: {ex.Message}");
        }
    }

    [HttpGet("exam/{examId}")]
    public async Task<ActionResult<ExamSummaryDto>> GetExamSummary(int examId)
    {
        try
        {
            var summary = await _analyticsService.GetExamSummaryAsync(examId);

            var dto = new ExamSummaryDto
            {
                ExamId = summary.ExamId,
                StudentId = summary.StudentId,
                TotalTasks = summary.TotalTasks,
                CorrectTasks = summary.CorrectTasks,
                ScorePercentage = summary.ScorePercentage
            };

            return Ok(dto);
        }
        catch (Exception ex)
        {
            return NotFound($"Error retrieving exam summary: {ex.Message}");
        }
    }

    [HttpGet("exam/{examId}/details")]
    public async Task<ActionResult<List<GradingResultDto>>> GetExamDetails(int examId)
    {
        try
        {
            var results = await _analyticsService.GetExamDetailsAsync(examId);

            var dtos = results.Select(r => new GradingResultDto
            {
                TaskId = r.TaskId,
                Expression = r.Task.Expression,
                IsCorrect = r.IsCorrect,
                CalculatedValue = r.CalculatedValue,
                ExpectedValue = r.ExpectedValue
            }).ToList();

            return Ok(dtos);
        }
        catch (Exception ex)
        {
            return NotFound($"Error retrieving exam details: {ex.Message}");
        }
    }
}
