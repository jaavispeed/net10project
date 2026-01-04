using System.ComponentModel.DataAnnotations;

namespace PulseTrain.Domain.Entities;

public class Estado
{
    [Key]
    public int Id { get; set; }

    public string Status { get; set; } = string.Empty;
}
