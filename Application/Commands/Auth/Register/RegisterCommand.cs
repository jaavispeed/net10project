using MediatR;

namespace PulseTrain.Application.Commands.Auth.Register;

public record RegisterCommand(string Email, string Nombre, string Apellido, string Password)
    : IRequest<RegisterResult>;
