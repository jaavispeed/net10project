using PulseTrain.Application.DTOs;

namespace PulseTrain.Presentation.DTOs;

public record LoginResponse(UserDto User, string Token) : LoginResult(User, Token);
