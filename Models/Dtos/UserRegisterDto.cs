namespace PulseTrain.Models.Dtos;

public class UserRegisterDto
{ 
    public required string Email { get; set; } = string.Empty;
    public required string Password { get; set; } = string.Empty;
    public required string Role { get; set; } = "User";
}