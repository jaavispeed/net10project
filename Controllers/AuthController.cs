using System.ComponentModel.DataAnnotations;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PulseTrain.Features.Auth.Commands.Login;
using PulseTrain.Features.Auth.Commands.Register;

namespace PulseTrain.Controllers;

public class AuthController(ISender sender, IMapper mapper) : SharedController
{
    /// <summary>
    /// Registra un nuevo usuario en el sistema.
    /// </summary>
    [HttpPost("register")]
    [ProducesResponseType(typeof(RegisterResponse), StatusCodes.Status201Created)]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var command = new RegisterCommand(
            request.Email,
            request.Nombre,
            request.Apellido,
            request.Password
        );
        var result = await sender.Send(command, HttpContext.RequestAborted);
        return Ok(mapper.Map<RegisterResponse>(result));
    }

    /// <summary>
    /// Autentica un usuario y retorna un token JWT.
    /// </summary>
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status201Created)]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var command = new LoginCommand(request.Email, request.Password);
        var result = await sender.Send(command, HttpContext.RequestAborted);
        return Ok(mapper.Map<LoginResponse>(result));
    }
}

public record RegisterResponse(string Email, string Nombre, string Apellido, string Role);

public class RegisterRequest
{
    [Required(ErrorMessage = "El campo email es requerido")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "El campo nombre es requerido")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El campo apellido es requerido")]
    public string Apellido { get; set; } = string.Empty;

    [Required(ErrorMessage = "El campo password es requerido")]
    public string Password { get; set; } = string.Empty;
};

public record LoginResponse(string Email, string Role, string Token);

public class LoginRequest
{
    [Required(ErrorMessage = "El campo email es requerido")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "El campo password es requerido")]
    public string Password { get; set; } = string.Empty;
};
