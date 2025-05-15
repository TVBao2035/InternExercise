using UserService.Models;
using UserService.Models.DTOs;
using UserService.Models.Enities;

namespace UserService.Services.Interfaces
{
    public interface IUserService
    {
        Task<AppReponse<UserDTO>> Create(UserDTO request);
        Task<AppReponse<UserDTO>> Update(User request);
        Task<AppReponse<UserDTO>> Delete(Guid Id);
        Task<AppReponse<List<User>>> GetAll();
        Task<AppReponse<User>> GetById(Guid Id);
    }
}
