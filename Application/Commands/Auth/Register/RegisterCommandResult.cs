namespace PulseTrain.Application.Commands.Auth.Register;

public record RegisterResult(
    string Email,
    string Nombre,
    string Apellido,
    string Role,
    string Estado
);
