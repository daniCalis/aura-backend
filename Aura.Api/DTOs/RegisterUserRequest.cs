using System.ComponentModel.DataAnnotations;

namespace Aura.Api.DTOs;

public class RegisterUserRequest
{
    //Data-annotations for validation
    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    [Required]
    [MinLength(6)]
    public required string Password { get; set; }
}
