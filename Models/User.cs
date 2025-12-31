using System.ComponentModel.DataAnnotations;

namespace PulseTrain.Models;

public class User
{
    [Key]
    public int Id { get; set; }

    public string Email { get; set; } = string.Empty;

    public string Nombre { get; set; } = string.Empty;

    public string Apellido { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string Role { get; set; } = "User";
}
