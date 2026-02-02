using AutoGrader.Api.DTOs;
using AutoGrader.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AutoGrader.Api.Controllers;

[ApiController]
[Route("api/math")]
public class MathEvaluationController : ControllerBase
{
    private readonly IMathEvaluatorService _mathEvaluator;

    public MathEvaluationController(IMathEvaluatorService mathEvaluator)
    {
        _mathEvaluator = mathEvaluator;
    }

    [HttpPost("evaluate")]
    public ActionResult<MathEvaluationResponseDto> Evaluate([FromBody] MathEvaluationRequestDto request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Expression))
            {
                return BadRequest(new MathEvaluationResponseDto
                {
                    Result = 0,
                    IsValid = false,
                    ErrorMessage = "Expression cannot be empty"
                });
            }

            if (_mathEvaluator.TryEvaluate(request.Expression, out double result))
            {
                return Ok(new MathEvaluationResponseDto
                {
                    Result = result,
                    IsValid = true,
                    ErrorMessage = null
                });
            }
            else
            {
                return BadRequest(new MathEvaluationResponseDto
                {
                    Result = 0,
                    IsValid = false,
                    ErrorMessage = "Invalid expression"
                });
            }
        }
        catch (Exception ex)
        {
            return BadRequest(new MathEvaluationResponseDto
            {
                Result = 0,
                IsValid = false,
                ErrorMessage = ex.Message
            });
        }
    }
}
