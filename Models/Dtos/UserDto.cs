namespace PulseTrain.Models.Dtos
{
    public class UserDto
    {

    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Role { get; set; } = "User";
    }
}