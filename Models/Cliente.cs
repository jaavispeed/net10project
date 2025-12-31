using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PulseTrain.Models;

public class Cliente
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    public int Edad { get; set; }

    [Required]
    public DateTime FechaCreacion { get; set; } = DateTime.Now;
    public DateTime? FechaActualizacion { get; set; }

    //Relaciones
    public int UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public required User User { get; set; }

    [Required]
    public int EstadoId { get; set; }

    [ForeignKey(nameof(EstadoId))]
    public required Estado Estado { get; set; }
}
