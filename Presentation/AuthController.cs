using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PulseTrain.Application.Commands.Auth.Login;
using PulseTrain.Application.Commands.Auth.Register;
using PulseTrain.Application.Queries.Auth;
using PulseTrain.Presentation.DTOs;

namespace PulseTrain.Presentation;

public class AuthController(ISender sender) : SharedController
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
        return Ok(result);
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
        return Ok(result);
    }

    [HttpGet("check-status")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status201Created)]
    public async Task<IActionResult> CheckAuthStatus()
    {
        var authHeader = HttpContext.Request.Headers.Authorization.ToString();

        if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Bearer "))
            return Unauthorized();

        var token = authHeader["Bearer ".Length..].Trim();
        var query = new CheckAuthStatusQuery(token);
        var result = await sender.Send(query, HttpContext.RequestAborted);
        return Ok(result);
    }
}
