using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserService.Models;
using UserService.Models.DTOs;
using UserService.Models.Enities;
using UserService.Repositories.Implements;
using UserService.Repositories.Interfaces;
using UserService.Services.Interfaces;

namespace UserService.Services.Implements
{
    public class UserServices : IUserService
    {
        private IUserRepository _userRepository;

        public UserServices(IUserRepository userRepository) {
            _userRepository = userRepository;
        }

        public async Task<AppReponse<User>> GetById(Guid Id)
        {
            var result = new AppReponse<User>();
            try
            {
                User user = await _userRepository.Query(u => u.Id == Id).FirstOrDefaultAsync();
                if(user == null)
                {
                    return result.SendReponse(404, "Not found user");
                }

                return result.SendReponse(200, "Success", user);
            }catch(Exception ex)
            {
                return result.SendReponse(404, ex.Message);
            }
        }
        public async Task<AppReponse<UserDTO>> Create(UserDTO request)
        {
            var result = new AppReponse<UserDTO>();
            try
            {
                User user = await _userRepository.Query(u => u.Email == request.Email).FirstOrDefaultAsync();
                if(user is not null)
                {
                    return result.SendReponse(404, "Email is exisiting");
                }
                user = new User();
                user.Id = Guid.NewGuid();
                user.Email = request.Email;
                user.Name = request.Name;
                _userRepository.Insert(user);
                return result.SendReponse(200, "Success", request);
            }
            catch (Exception ex)
            {
                return result.SendReponse(404, ex.Message);
            }
        }

        public async  Task<AppReponse<UserDTO>> Delete(Guid Id)
        {
            var result = new AppReponse<UserDTO>();
            try
            {
                User user = await _userRepository.Query(u => u.Id == Id).FirstOrDefaultAsync();
                if (user is  null)
                {
                    return result.SendReponse(404, "Not Found User");
                }
                _userRepository.Delete(user);
                return result.SendReponse(200, "Success", new UserDTO
                {
                    Email = user.Email,
                    Name = user.Name,
                });

            }
            catch (Exception ex)
            {
                return result.SendReponse(404, ex.Message);
            }
        }

        public async  Task<AppReponse<List<User>>> GetAll()
        {
            var result = new AppReponse<List<User>>();
            try
            {
                List<User> listUser = await _userRepository.Query().ToListAsync();
                return result.SendReponse(200, "Success", listUser);
            }
            catch (Exception ex)
            {
                return result.SendReponse(404, ex.Message);
            }
        }

        public async Task<AppReponse<UserDTO>> Update(User request)
        {
            var result = new AppReponse<UserDTO>();
            try
            {
                User user = await _userRepository.Query(u => u.Id == request.Id).FirstOrDefaultAsync();
                if (user is null) return result.SendReponse(404, "Not found user");
                user.Email = request.Email;
                user.Name = request.Name;
                _userRepository.Update(user);
                return result.SendReponse(200, "Success", new UserDTO
                {
                    Email = request.Email,
                    Name = request.Name,
                });
            }
            catch (Exception ex)
            {
                return result.SendReponse(404, ex.Message);
            }
        }
    }
}
