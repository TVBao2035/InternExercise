using myWebApi.Enity;
using myWebApi.Model.Response;

namespace myWebApi.Services.Interface
{
    public interface IUserService
    {
        Task<AppResponse<UserReponse>> Create(User user);
        Task<AppResponse<UserReponse>> Delete(Guid id);
        Task<AppResponse<List<UserReponse>>> GetAll();
        Task<AppResponse<UserReponse>> Update(User user);
    }
}
