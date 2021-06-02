using LibraryAPI.Models;
using AutoMapper;
using LibraryAPI.Dtos;

namespace LibraryAPI
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<CreateUserDto, User>();
        }
    }
}
