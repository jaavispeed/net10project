using MediatR;
using PulseTrain.Application.DTOs;

namespace PulseTrain.Application.Commands.Auth.Login;

public record LoginCommand(string Email, string Password) : IRequest<LoginResult>;
