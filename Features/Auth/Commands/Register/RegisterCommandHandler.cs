using MapsterMapper;
using MediatR;
using PulseTrain.Services;

namespace PulseTrain.Features.Auth.Commands.Register;

public class RegisterCommandHandler(IMapper mapper, IAuthService authService)
    : IRequestHandler<RegisterCommand, RegisterResult>
{
    public async Task<RegisterResult> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken
    )
    {
        var result = await authService.RegisterAsync(
            request.Email,
            request.Password,
            cancellationToken
        );
        return mapper.Map<RegisterResult>(result);
    }
}
