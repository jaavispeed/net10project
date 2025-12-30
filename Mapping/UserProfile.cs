using AutoMapper;
using PulseTrain.Models;
using PulseTrain.Models.Dtos;

namespace PulseTrain.Mapping;

public class UserProfile: Profile
{

    public UserProfile()
    {
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<User, CreateUserDto>().ReverseMap();
        CreateMap<User, UserLoginDto>().ReverseMap();
        CreateMap<User, UserLoginResponseDto>().ReverseMap();
    }
    
}