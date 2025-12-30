using PulseTrain.Models;
using PulseTrain.Models.Dtos;

namespace PulseTrain.Repository.IRepository;

public interface IUserRepository
{
    ICollection<User> GetUsers();
    User GetUser(int userId);
    bool IsUniqueUser(string email);
    Task<UserLoginResponseDto> Login(UserLoginDto userLoginDto);
    Task<User> Register(CreateUserDto createUserDto);
}