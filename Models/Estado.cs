using System.ComponentModel.DataAnnotations;

namespace PulseTrain.Models;

public class Estado
{
    [Key]
    public int Id { get; set; }

    public string Status { get; set; } = string.Empty;
}
