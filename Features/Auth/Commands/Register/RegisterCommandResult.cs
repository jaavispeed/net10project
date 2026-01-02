namespace PulseTrain.Features.Auth.Commands.Register;

public record RegisterResult(
    string Email,
    string Nombre,
    string Apellido,
    string Role,
    string Estado
);
