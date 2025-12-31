using MediatR;

namespace PulseTrain.Features.Auth.Commands.Register;

public record RegisterCommand(string Email, string Password) : IRequest<RegisterResult>;
