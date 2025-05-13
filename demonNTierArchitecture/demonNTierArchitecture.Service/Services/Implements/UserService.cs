using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using demonNTierArchitecture.Data.Repository.Implements;
using demonNTierArchitecture.Data.Repository.Interfaces;
using demonNTierArchitecture.Models.Enities;
using demonNTierArchitecture.Models.Models.Request;
using demonNTierArchitecture.Models.Models.Response;
using demonNTierArchitecture.Service.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace demonNTierArchitecture.Service.Services.Implements
{
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;
        private IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<AppResponse<UserResponse>> Create(UserCreateRequest request)
        {
            var result = new AppResponse<UserResponse>();
            try
            {
                User user = await _userRepository.Query(u => u.Email == request.Email).FirstOrDefaultAsync();
                if (user is not null)
                    return result.SendResponse(404, "User is exit");
                user = new User()
                {
                    Id = Guid.NewGuid(),
                    Email = request.Email,
                    Password = request.Password,
                    Name = request.Name
                };
                _userRepository.Add(user);
                UserResponse reponse = _mapper.Map<UserResponse>(user);
                return result.SendResponse(200, "Success", reponse);
            }
            catch (Exception ex)
            {
                return result.SendResponse(400, ex.Message);
            }
        }

        public async Task<AppResponse<UserResponse>> Delete(Guid Id)
        {
            var result = new AppResponse<UserResponse>();
            try
            {
                User user = await _userRepository.Query(u => u.Id == Id).FirstOrDefaultAsync();
                if(user is null)
                {
                    return result.SendResponse(404, "Not found user");
                }

                _userRepository.Delete(user);
                return result.SendResponse(200, "Delete Success");
            }
            catch (Exception ex)
            {
                return result.SendResponse(400, ex.Message);
            }
        }

        public async Task<AppResponse<List<UserResponse>>> GetAll()
        {
            var result = new AppResponse<List<UserResponse>>();
            try
            {
                var userList = await _userRepository.Query()
                    .Select(u => _mapper.Map<UserResponse>(u))
                    .ToListAsync();
                return result.SendResponse(200, "Success", userList);
            }
            catch (Exception ex)
            {
                return result.SendResponse(400, ex.Message);
            }
        }

        public async Task<AppResponse<UserResponse>> Update(UserUpdateRequest request)
        {
            var result = new AppResponse<UserResponse>();
            try
            {
                User user = await _userRepository.Query(u => u.Id == request.Id).FirstOrDefaultAsync();
                if (user is null) return result.SendResponse(404, "not found user");
                user.Email = request.Email;
                user.Password = request.Password;
                user.Name = request.Name;
                _userRepository.Update(user);
                UserResponse response = _mapper.Map<UserResponse>(user);
                return result.SendResponse(200, "Success", response);
            }
            catch (Exception ex)
            {
                return result.SendResponse(400, ex.Message);
            }
        }
    }
}
