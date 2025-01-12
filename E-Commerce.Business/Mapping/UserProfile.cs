using AutoMapper;
using E_Commerce.Business.DTOs.UserDtos;
using E_Commerce.Domain.Entities;

namespace E_Commerce.Business.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<CreateUserDto, User>().ReverseMap();
            CreateMap<GetUserDto, User>().ReverseMap();
            CreateMap<UpdateUserDto, User>().ReverseMap();
        }
    }
}
