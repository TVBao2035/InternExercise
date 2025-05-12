using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using demonOnionArchitecture.Domain.Enities;
using demonOnionArchitecture.Domain.DTOs.Request;
using demonOnionArchitecture.Domain.DTOs.Response;
using demonOnionArchitecture.Service.Modals;
using demonOnionArchitecture.Service.Services.Interfaces;
using AutoMapper;
using demonOnionArchitecture.Infrastrue.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;



namespace demonOnionArchitecture.Service.Services.Implements
{
     public class UserService : IUserService
    {

        private IMapper _mapper;
        private IUserRepository _userRepository;

        public UserService(IMapper mapper, IUserRepository userRepository)
        {
           
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<User> CheckUserByEmail(string email)
        {
            try
            {
                return await _userRepository
                    .Query(u => u.Email == email).FirstOrDefaultAsync();
            }
            catch (Exception)
            {

                return null;
            }
        }
        
        public async Task<AppReponse<UserResponse>> Create(UserCreateRequest request)
        {
           var response = new AppReponse<UserResponse>();
            try
            {
                User user = await CheckUserByEmail(request.Email);
                if (user is not null)
                    return response.SendReponse(404, "User exist");
                user = new User();
                user = _mapper.Map<User>(request);
                user.Id = Guid.NewGuid();
                _userRepository.Add(user);
                UserResponse userResponse = _mapper.Map<UserResponse>(user);
                return response.SendReponse(200, "Success", userResponse);
            }
            catch (Exception ex)
            {
                return response.SendReponse(400, ex.Message);
            }
        }

        public async Task<AppReponse<UserResponse>> Delete(Guid Id)
        {
            var response = new AppReponse<UserResponse>();
            try
            {
                User user = await _userRepository.Query(u => u.Id == Id).FirstOrDefaultAsync();
                if (user is null) return response.SendReponse(404, "Not found ");

                _userRepository.Delete(user);
                return response.SendReponse(200, "Success");
            }
            catch (Exception ex)
            {
                return response.SendReponse(400, ex.Message);
            }
        }

        public async Task<AppReponse<List<UserResponse>>> GetAll()
        {
            var response = new AppReponse<List<UserResponse>> ();
            try
            {
                List<UserResponse> userList = await _userRepository.Query()
                    .Select(u => _mapper.Map<UserResponse>(u))
                    .ToListAsync();
                return response.SendReponse(200, "Success", userList);
            }
            catch (Exception ex)
            {
                return response.SendReponse(400, ex.Message);
            }
        }

        public async Task<AppReponse<UserResponse>> Update(UserUpdateRequest request)
        {
            var response = new AppReponse<UserResponse>();
            try
            {
                User user = await _userRepository.Query(u => u.Id == request.Id).FirstOrDefaultAsync();
                if (user is null) return response.SendReponse(404, "Not found ");
                user.Email = request.Email;
                user.Name = request.Name;
                _userRepository.Update(user);
                return response.SendReponse(200, "Success", _mapper.Map<UserResponse>(user));
            }
            catch (Exception ex)
            {
                return response.SendReponse(400, ex.Message);
            }
        }
    }
}
