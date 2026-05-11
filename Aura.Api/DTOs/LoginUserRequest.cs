using System.ComponentModel.DataAnnotations;

namespace Aura.Api.DTOs;

public class LoginUserRequest
{
    //Data-annotations for validation
    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    [Required]
    public required string Password { get; set; }
}
