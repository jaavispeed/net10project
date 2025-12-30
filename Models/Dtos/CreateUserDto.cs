using System.ComponentModel.DataAnnotations;

namespace PulseTrain.Models.Dtos;

public class CreateUserDto
{
  [Required(ErrorMessage = "El campo email es requerido")]
  public string Email { get; set; } = string.Empty;
  [Required(ErrorMessage = "El campo password es requerido")]
  public string? Password { get; set; }
  [Required(ErrorMessage = "El campo role es requerido")]
  public string? Role { get; set; }
}