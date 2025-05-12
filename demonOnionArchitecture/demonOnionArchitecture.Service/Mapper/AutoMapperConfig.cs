using AutoMapper;
using demonOnionArchitecture.Domain.Enities;
using demonOnionArchitecture.Domain.DTOs.Request;
using demonOnionArchitecture.Domain.DTOs.Response;

namespace demonOnionArchitecture.Service.Mapper;

public class AutoMapperConfig : Profile
{
    public  AutoMapperConfig()
    {
        CreateMap<UserResponse, User>().ReverseMap();
        CreateMap<UserCreateRequest, User>().ReverseMap();
        CreateMap<UserUpdateRequest, User>().ReverseMap();
    }
}
