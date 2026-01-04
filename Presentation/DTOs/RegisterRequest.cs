using System.ComponentModel.DataAnnotations;

namespace PulseTrain.Presentation.DTOs;

public class RegisterRequest
{
    [Required(ErrorMessage = "El campo email es requerido")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "El campo nombre es requerido")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El campo apellido es requerido")]
    public string Apellido { get; set; } = string.Empty;

    [Required(ErrorMessage = "El campo password es requerido")]
    public string Password { get; set; } = string.Empty;
}
