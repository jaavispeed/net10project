namespace PulseTrain.Models.Dtos;

public class UserLoginResponseDto
{
    public UserRegisterDto? User { get; set; } 
    public string Token { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}