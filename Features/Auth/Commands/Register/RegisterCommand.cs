using MediatR;

namespace PulseTrain.Features.Auth.Commands.Register;

public record RegisterCommand(string Email, string Nombre, string Apellido, string Password)
    : IRequest<RegisterResult>;
