using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using myWebApi.Enity;
using myWebApi.Model.Request;
using myWebApi.Model.Response;

namespace myWebApi.AutoMapper
{
    public class AutoMapperConfig:Profile
    {

        public AutoMapperConfig() { 
        
            CreateMap<User, UserReponse>().ReverseMap();
            CreateMap<Product, ProductCreateRequest>().ReverseMap();
        }
    }
}
