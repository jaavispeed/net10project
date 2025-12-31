using MapsterMapper;
using MediatR;
using PulseTrain.Services;

namespace PulseTrain.Features.Auth.Commands.Login;

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
