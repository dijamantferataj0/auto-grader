using AutoGrader.Api.DTOs;
using AutoGrader.Persistence.Interfaces;
using AutoGrader.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoGrader.Domain.Models;

namespace AutoGrader.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Teacher")]
public class GradingController : ControllerBase
{
    private readonly IXmlParserService _xmlParser;
    private readonly IGradingService _gradingService;
    private readonly IAnalyticsService _analyticsService;
    private readonly IRepository<Teacher> _teacherRepository;
    private readonly IRepository<ExamSummary> _examSummaryRepository;

    public GradingController(
        IXmlParserService xmlParser,
        IGradingService gradingService,
        IAnalyticsService analyticsService,
        IRepository<Teacher> teacherRepository,
        IRepository<ExamSummary> examSummaryRepository)
    {
        _xmlParser = xmlParser;
        _gradingService = gradingService;
        _analyticsService = analyticsService;
        _teacherRepository = teacherRepository;
        _examSummaryRepository = examSummaryRepository;
    }

    [HttpPost("upload")]
    public async Task<ActionResult<UploadResponseDto>> UploadXml(IFormFile file)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded");
            }

            if (!file.FileName.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest("Only XML files are accepted");
            }

            if (file.Length > 5 * 1024 * 1024) // 5MB limit
            {
                return BadRequest("File size exceeds 5MB limit");
            }

            using var stream = file.OpenReadStream();

            // Validate XML
            stream.Position = 0;
            var isValid = await _xmlParser.ValidateXmlAsync(stream);

            if (!isValid)
            {
                return BadRequest("Invalid XML structure");
            }

            // Parse XML
            stream.Position = 0;
            var teacher = await _xmlParser.ParseXmlAsync(stream);

            // Save teacher and related entities
            await _teacherRepository.AddAsync(teacher);
            await _teacherRepository.SaveChangesAsync();

            // Grade all exams
            var allExams = teacher.Students.SelectMany(s => s.Exams).ToList();
            var gradingResults = await _gradingService.GradeMultipleExamsAsync(allExams);

            // Generate and save summaries
            var summaries = new List<ExamSummaryDto>();
            foreach (var student in teacher.Students)
            {
                foreach (var exam in student.Exams)
                {
                    // Get results for this exam
                    var examResults = gradingResults.Where(r => r.Task.ExamId == exam.Id).ToList();
                    var correctCount = examResults.Count(r => r.IsCorrect);
                    var totalCount = examResults.Count;
                    var scorePercentage = totalCount > 0 ? (double)correctCount / totalCount * 100 : 0;

                    // Save ExamSummary to database (use XmlId as StudentId for lookup)
                    var examSummary = new ExamSummary
                    {
                        ExamId = exam.Id,
                        StudentId = student.XmlId,
                        TotalTasks = totalCount,
                        CorrectTasks = correctCount,
                        ScorePercentage = scorePercentage,
                        ProcessedAt = DateTime.UtcNow
                    };
                    await _examSummaryRepository.AddAsync(examSummary);

                    summaries.Add(new ExamSummaryDto
                    {
                        ExamId = exam.Id,
                        StudentId = student.XmlId,
                        TotalTasks = totalCount,
                        CorrectTasks = correctCount,
                        ScorePercentage = scorePercentage
                    });
                }
            }

            // Save all exam summaries
            await _examSummaryRepository.SaveChangesAsync();

            var response = new UploadResponseDto
            {
                TeacherId = teacher.XmlId,  // Use XmlId for display
                ProcessedStudents = teacher.Students.Count,
                TotalExams = allExams.Count,
                Summaries = summaries
            };

            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error processing XML: {ex.Message}");
        }
    }
}
