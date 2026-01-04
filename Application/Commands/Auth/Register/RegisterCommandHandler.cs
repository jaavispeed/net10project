using MapsterMapper;
using MediatR;
using PulseTrain.Infrastructure.Services;

namespace PulseTrain.Application.Commands.Auth.Register;

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
            request.Nombre,
            request.Apellido,
            request.Password,
            cancellationToken
        );
        return mapper.Map<RegisterResult>(result);
    }
}
