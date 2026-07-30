using Aura.Api.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aura.Api.Controllers;

// Sta dicendo ad APS.NET Core: questa classe è un controller, quindi gestisce le richieste HTTP
// Inoltre attiva altre funzionalità, tra cui: validazione automatica dei DTO; conversione automatica JSON a oggetti C#; risposte autobatiche Bad Request se modello non è valido (senza entrare in IActionResult)
[ApiController]
// Definisce il percorso URL: register in UserController diventerà api/Users/register e quindi https://localhost:7000/api/Users/register
// Inoltre tramite dependency injection, ASP.NET Core genera un istanza di UsersController, non visibile all'esterno, e la distrugge dopo aver gestito la richiesta HTTP
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserService _userService;

    // ASP.NET Core utilizza la dependency injection per fornire automaticamente un'istanza di UserService al controller.
    // L'istruzione builder.Services.AddScoped<UserService>(); nel Program.cs garantisce la Dependecy Injection per questo controller
    // userService ha una dipendenza da AppDbContext
    public UsersController(UserService userService)
    {
        _userService = userService;
    }

    // Questi metodi dei controller vengono chiamati "action methods". Gestiscono le richieste HTTP. Il framework ASP.NET Core li chiama automaticamente quando riceve una richiesta HTTP che corrisponde al percorso (/api/Users/register)e al verbo HTTP (GET, POST, ecc.) specificato.
    // ASP.NET Core crea automaticamente l'istanza request di RegisterUserRequest (la classe DTO).
    // I controller non dovrebbero contenere logica, ma delegare la logica ai servizi.
    // Il controller deve solo ricevere la richiesta, la valida, chiama il servizio, e restituisce la risposta.
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
        return Ok(new { message = "GetMe was successfull" });
    }
}