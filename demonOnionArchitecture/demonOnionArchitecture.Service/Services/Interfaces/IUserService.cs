using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using demonOnionArchitecture.Domain.DTOs.Request;
using demonOnionArchitecture.Domain.DTOs.Response;
using demonOnionArchitecture.Service.Modals;

namespace demonOnionArchitecture.Service.Services.Interfaces
{
    public interface IUserService
    {
         Task<AppReponse<List<UserResponse>>> GetAll();
        Task<AppReponse<UserResponse>> Create(UserCreateRequest request);
        Task<AppReponse<UserResponse>> Update(UserUpdateRequest request);
        Task<AppReponse<UserResponse>> Delete(Guid Id);
    }
}
