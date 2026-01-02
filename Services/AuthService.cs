using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Experimental;
using PulseTrain.Models;

namespace PulseTrain.Services;

public interface IAuthService
{
    Task<User> RegisterAsync(
        string email,
        string nombre,
        string apellido,
        string password,
        CancellationToken cancellationToken = default
    );
    Task<LoginResultDto> LoginAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default
    );
    Task<LoginResultDto> CheckAuthStatus(
        string token,
        CancellationToken cancellationToken = default
    );
}

public class AuthService(ApplicationDbContext dbContext, IConfiguration configuration)
    : IAuthService
{
    private const string RoleDefault = "User";
    private const int EstadoActivoDefault = 1;

    private readonly string? secretKey = configuration.GetValue<string>("ApiSettings:SecretKey");

    public async Task<User> RegisterAsync(
        string email,
        string nombre,
        string apellido,
        string password,
        CancellationToken cancellationToken = default
    )
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Email es requerido");
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException("La contraseña es requerida");
        }

        var existe = await dbContext.Users.AnyAsync(
            u => u.Email.ToLower().Trim() == email.ToLower().Trim(),
            cancellationToken
        );
        if (existe)
        {
            throw new InvalidOperationException("El usuario ya existe");
        }

        var encriptedPassword = BCrypt.Net.BCrypt.HashPassword(password);

        var user = new User
        {
            Email = email,
            Nombre = nombre,
            Apellido = apellido,
            Password = encriptedPassword,
            Role = RoleDefault,
            EstadoId = EstadoActivoDefault,
        };

        dbContext.Users.Add(user);

        await dbContext.SaveChangesAsync(cancellationToken);
        return user;
    }

    public async Task<LoginResultDto> LoginAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default
    )
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Email es requerido");
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException("La contraseña es requerida");
        }

        var user = await dbContext.Users.FirstOrDefaultAsync(
            u => u.Email.ToLower().Trim() == email.ToLower().Trim(),
            cancellationToken
        );

        if (user is null)
        {
            throw new InvalidOperationException("Usuario no encontrado");
        }

        var validarPassword = BCrypt.Net.BCrypt.Verify(password, user.Password);

        if (!validarPassword)
        {
            throw new InvalidOperationException("Contraseña incorrecta");
        }

        var token = GenerateJwtToken(user);

        return new LoginResultDto(
            new UserDto(user.Email, user.Nombre, user.Apellido, user.Role),
            token
        );
    }

    public async Task<LoginResultDto> CheckAuthStatus(
        string token,
        CancellationToken cancellationToken = default
    )
    {
        // 1️⃣ Validar token (firma + expiración)
        var principal = ValidateToken(token); // ❌ Lanza excepción si no es válido

        // 2️⃣ Extraer userId
        var userId = int.Parse(principal.FindFirst("id")!.Value);

        // 3️⃣ Confirmar usuario en DB
        var user = await dbContext.Users.FirstOrDefaultAsync(
            u => u.Id == userId,
            cancellationToken
        );

        if (user is null)
            throw new UnauthorizedAccessException("Usuario no existe");

        // 4️⃣ Token válido → generar nuevo
        var newToken = GenerateJwtToken(user);

        return new LoginResultDto(
            new UserDto(user.Email, user.Nombre, user.Apellido, user.Role),
            newToken
        );
    }

    private ClaimsPrincipal ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(secretKey!);

        try
        {
            return tokenHandler.ValidateToken(
                token,
                new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero,
                },
                out _
            );
        }
        catch
        {
            throw new UnauthorizedAccessException("Token inválido o expirado");
        }
    }

    private string GenerateJwtToken(User user)
    {
        var handlerToken = new JwtSecurityTokenHandler();

        if (string.IsNullOrEmpty(secretKey))
        {
            throw new InvalidOperationException("Secretkey no esta configurada.");
        }

        var key = Encoding.UTF8.GetBytes(secretKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([
                new Claim("id", user.Id.ToString()),
                new Claim("email", user.Email),
                new Claim(ClaimTypes.Role, user.Role),
            ]),
            Expires = DateTime.UtcNow.AddHours(2), //Dura 2 horas
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            ),
        };
        var token = handlerToken.CreateToken(tokenDescriptor);
        return handlerToken.WriteToken(token);
    }
}

public record UserDto(string Email, string Nombre, string Apellido, string Role);

public record LoginResultDto(UserDto User, string Token);
