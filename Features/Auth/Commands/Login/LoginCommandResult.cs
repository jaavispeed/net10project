namespace PulseTrain.Features.Auth.Commands.Login;

public record LoginResult(string Email, string Role, string Token);
