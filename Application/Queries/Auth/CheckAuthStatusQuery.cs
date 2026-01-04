using MediatR;
using PulseTrain.Application.DTOs;

namespace PulseTrain.Application.Queries.Auth;

public record CheckAuthStatusQuery(string Token) : IRequest<LoginResult>;
