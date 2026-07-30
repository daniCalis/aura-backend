using System.ComponentModel.DataAnnotations;

namespace Aura.Api.DTOs;

public class LoginUserRequest
{
    //Data-annotations for validation
    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    [Required]
    // si potrebbe introdurre una Data Annotation anche per la lunghezza forse
    public required string Password { get; set; }
}
