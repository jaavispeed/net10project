using MapsterMapper;
using MediatR;
using PulseTrain.Application.DTOs;
using PulseTrain.Infrastructure.Services;

namespace PulseTrain.Application.Commands.Auth.Login;

public class LoginCommandHandler(IMapper mapper, IAuthService authService)
    : IRequestHandler<LoginCommand, LoginResult>
{
    public async Task<LoginResult> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var result = await authService.LoginAsync(
            request.Email,
            request.Password,
            cancellationToken
        );
        return mapper.Map<LoginResult>(result);
    }
}
