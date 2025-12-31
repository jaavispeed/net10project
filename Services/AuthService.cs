using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
}

public class AuthService(ApplicationDbContext dbContext, IConfiguration configuration)
    : IAuthService
{
    private const string RoleDefault = "User";
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

        //Aca se genera el token JWT
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
        return new LoginResultDto(user.Email, user.Role, handlerToken.WriteToken(token));
    }
}

public record LoginResultDto(string Email, string Role, string Token);
