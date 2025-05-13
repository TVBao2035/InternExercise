using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using demonNTierArchitecture.Models.Enities;
using demonNTierArchitecture.Models.Models.Request;
using demonNTierArchitecture.Models.Models.Response;

namespace demonNTierArchitecture.Service.Mapper
{
    public class AutoMapperConfiguration : Profile
    {
        public AutoMapperConfiguration()
        {
            CreateMap<User, UserResponse>().ReverseMap();
            CreateMap<User, UserCreateRequest>().ReverseMap();
            CreateMap<User, UserUpdateRequest>().ReverseMap();
        }
    }
}
