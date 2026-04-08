using System.ComponentModel.DataAnnotations;

namespace Aura.Api.DTOs;

public class RegisterUserRequest
{
    //Data-annotations for validation
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [MinLength(6)]
    public string Password { get; set; }
}
