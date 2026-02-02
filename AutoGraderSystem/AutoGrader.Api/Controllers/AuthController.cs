using AutoGrader.Api.DTOs;
using AutoGrader.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace AutoGrader.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly JwtTokenService _jwtTokenService;

    // Simple in-memory user store for demo purposes
    // In production, this would be in a database with hashed passwords
    private static readonly Dictionary<string, (string Password, string Role)> Users = new()
    {
        { "teacher1", ("teacher123", "Teacher") },
        { "teacher2", ("teacher123", "Teacher") },
        { "12345", ("student123", "Student") },  // Student ID from XML examples
        { "67890", ("student123", "Student") }
    };

    public AuthController(JwtTokenService jwtTokenService)
    {
        _jwtTokenService = jwtTokenService;
    }

    [HttpPost("login")]
    public ActionResult<LoginResponseDto> Login([FromBody] LoginRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.UserId) || string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest("UserId and Password are required");
        }

        if (!Users.TryGetValue(request.UserId, out var user))
        {
            return Unauthorized("Invalid credentials");
        }

        if (user.Password != request.Password)
        {
            return Unauthorized("Invalid credentials");
        }

        var token = _jwtTokenService.GenerateToken(request.UserId, user.Role);

        return Ok(new LoginResponseDto
        {
            Token = token,
            UserId = request.UserId,
            Role = user.Role
        });
    }

    [HttpGet("users")]
    public ActionResult<object> GetDemoUsers()
    {
        // Endpoint to show demo users for testing
        return Ok(new
        {
            Teachers = new[]
            {
                new { UserId = "teacher1", Password = "teacher123" },
                new { UserId = "teacher2", Password = "teacher123" }
            },
            Students = new[]
            {
                new { UserId = "12345", Password = "student123" },
                new { UserId = "67890", Password = "student123" }
            }
        });
    }
}
