using System.ComponentModel.DataAnnotations;

namespace PulseTrain.Presentation.DTOs;

public class LoginRequest
{
    [Required(ErrorMessage = "El campo email es requerido")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "El campo password es requerido")]
    public string Password { get; set; } = string.Empty;
}
