using Aura.Api.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aura.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserService _userService;

    public UsersController(UserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserRequest request)
    {
        try
        {
            await _userService.RegisterAsync(request.Email, request.Password);

            return Created("", new { message = "User registered successfully" });
        }
        catch (ArgumentException ex)
        {
            // This could be due to invalid input, such as an improperly formatted email or a weak password.
            return BadRequest(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            // This could occur if the email is already in use by another user.
            return Conflict(new { error = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserRequest request)
    {
        try
        {
            var token = await _userService.LoginAsync(request.Email, request.Password);

            return Ok(new { token });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { error = ex.Message });
        }
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult GetMe()
    {
        // This endpoint is protected by the [Authorize] attribute, which means that only authenticated users can access it.
        return Ok("Sei autenticato 🎉");
    }
}