using MediatR;
using PulseTrain.Application.DTOs;
using PulseTrain.Application.Queries.Auth;
using PulseTrain.Infrastructure.Services;

namespace PulseTrain.Application.Handlers.Auth;

public class CheckAuthStatusQueryHandler(IAuthService authService)
    : IRequestHandler<CheckAuthStatusQuery, LoginResult>
{
    public async Task<LoginResult> Handle(
        CheckAuthStatusQuery request,
        CancellationToken cancellationToken
    )
    {
        var result = await authService.CheckAuthStatus(request.Token, cancellationToken);
        return new LoginResult(
            new PulseTrain.Application.DTOs.UserDto(
                result.User.Email,
                result.User.Nombre,
                result.User.Apellido,
                result.User.Role,
                result.User.Estado
            ),
            result.Token
        );
    }
}
