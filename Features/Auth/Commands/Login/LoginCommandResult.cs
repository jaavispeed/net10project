namespace PulseTrain.Features.Auth.Commands.Login;

public record LoginResult(UserDto User, string Token);

public record UserDto(string Email, string Nombre, string Apellido, string Role, string Estado);
