using PulseTrain.Application.Commands.Auth.Register;

namespace PulseTrain.Presentation.DTOs;

public record RegisterResponse(
    string Email,
    string Nombre,
    string Apellido,
    string Role,
    string Estado
) : RegisterResult(Email, Nombre, Apellido, Role, Estado);
