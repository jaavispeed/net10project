using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

    [Required]
    public int EstadoId { get; set; }

    [ForeignKey(nameof(EstadoId))]
    public Estado? Estado { get; set; }
}
