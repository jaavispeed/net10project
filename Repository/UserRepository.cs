using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PulseTrain.Models;
using PulseTrain.Models.Dtos;
using PulseTrain.Repository.IRepository;

namespace PulseTrain.Repository;

public class UserRepository : IUserRepository
{
    public readonly ApplicationDbContext _db;
    private string? secretKey;
    public UserRepository(ApplicationDbContext db, IConfiguration configuration)
    {
        _db = db;
        secretKey = configuration.GetValue<string>("ApiSettings:SecretKey");
    }
    public User GetUser(int userId)
    {
        return _db.Users.FirstOrDefault(u => u.Id == userId)!;
    }

    public ICollection<User> GetUsers()
    {
        return _db.Users.OrderBy(u => u.Email).ToList();
    }

    public bool IsUniqueUser(string email)
    {
        return !_db.Users.Any(u => u.Email.ToLower().Trim() == email.ToLower().Trim());
    }

    public async Task<UserLoginResponseDto> Login(UserLoginDto userLoginDto)
    {
        if(string.IsNullOrEmpty(userLoginDto.Email))
        {
            return new UserLoginResponseDto()
            {
                Token = "",
                User = null,
                Message = "Email es requerido"
            };
        }

        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email.ToLower().Trim() == userLoginDto.Email.ToLower().Trim());

        if(user == null)
        {
            return new UserLoginResponseDto()
            {
                Token = "",
                User = null,
                Message = "Usuario no encontrado"
            };
        }

        if(!BCrypt.Net.BCrypt.Verify(userLoginDto.Password, user.Password))
        {
            return new UserLoginResponseDto()
            {
                Token = "",
                User = null,
                Message = "Contraseña incorrecta"
            };
        }
        //Desde aca se genera el token JWT
        var handlerToken = new JwtSecurityTokenHandler();
        if(string.IsNullOrEmpty(secretKey))
        {
            throw new InvalidOperationException("Secretkey no esta configurada.");
        }
        var key = Encoding.UTF8.GetBytes(secretKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
                new Claim("id", user.Id.ToString()),
                new Claim("email", user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            ]),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token  = handlerToken.CreateToken(tokenDescriptor);
        return new UserLoginResponseDto()
        {
            Token = handlerToken.WriteToken(token),
            User = new UserRegisterDto()
            {
                Email = user.Email,
                Role = user.Role,
                Password = user.Password
            },
            Message = "Login exitoso"
        };
    }

    public async Task<User> Register(CreateUserDto createUserDto)
    {
        var encriptedPassword = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password);
        var user = new User()
        {
            Email = createUserDto.Email,
            Password = encriptedPassword,
            Role = "User"
        };
        _db.Users.Add(user);
        await _db.SaveChangesAsync();   
        return user;
    }
}