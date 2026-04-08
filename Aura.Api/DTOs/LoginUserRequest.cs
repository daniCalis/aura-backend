using System.ComponentModel.DataAnnotations;

namespace Aura.Api.DTOs;

public class LoginUserRequest
{
    //Data-annotations for validation
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}
