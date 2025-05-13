using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using demonNTierArchitecture.Models.Models.Request;
using demonNTierArchitecture.Models.Models.Response;

namespace demonNTierArchitecture.Service.Services.Interfaces
{
    public interface IUserService
    {
        Task<AppResponse<List<UserResponse>>> GetAll();
        Task<AppResponse<UserResponse>> Create(UserCreateRequest request);
        Task<AppResponse<UserResponse>> Update(UserUpdateRequest request);
        Task<AppResponse<UserResponse>> Delete(Guid Id);
    }
}
