using System.Reflection;
using AutoMapper;
using Azure;
using Microsoft.EntityFrameworkCore;
using myWebApi.Common;
using myWebApi.Enity;
using myWebApi.Model.Response;
using myWebApi.Repository.Interface;
using myWebApi.Services.Interface;

namespace myWebApi.Services
{
    public class UserService:IUserService
    {
        private IUserRepository _userRepository;
        private IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper) {
            _userRepository = userRepository;
            _mapper = mapper;

        }

        public async Task<AppResponse<List<UserReponse>>> GetAll()
        {
            var response = new AppResponse<List<UserReponse>>();
            try
            {
                var data = await _userRepository.FindBy(u => u.Id != null).Select(u => _mapper.Map<UserReponse>(u)).ToListAsync();
                return response.Send(200, "Success", data);
            }
            catch (Exception ex)
            {
                return response.Send(400, "Error");
            }
        }

        public async Task<AppResponse<UserReponse>> Delete(Guid id)
        {
            var response = new AppResponse<UserReponse>();
            try
            {
                var user = await _userRepository.FindBy(u => u.Id == id).FirstOrDefaultAsync();
                if (user == null) return response.Send(404, "Not Found User");
                await _userRepository.Delete(user);
                UserReponse userReponse = _mapper.Map<UserReponse>(user);
                return response.Send(200, "Success", userReponse);
            }
            catch (Exception ex)
            {
                return response.Send(400, "Error");
            }
        }
        public async Task<AppResponse<UserReponse>> Update(User user)
        {
            var response = new AppResponse<UserReponse>();
            try
            {
                if (!Helpers.ValidateEmailAndPhone(user.Email, user.PhoneNumber))
                {
                    return response.Send(404, "Email OR Phone Number Invalid");
                }
                User userMain = await _userRepository.FindBy(u => u.Id == user.Id).FirstOrDefaultAsync();
                if (userMain == null)
                {
                    return response.Send(404, "Not Found User");
                }
                userMain.BirthDate = user.BirthDate;
                userMain.Name = user.Name;
                userMain.Gender = user.Gender;
                userMain.SchoolName = user.SchoolName;


                await _userRepository.Update(userMain);

                UserReponse userReponse = _mapper.Map<UserReponse>(userMain);
                return response.Send(200, "Success", userReponse);
            }
            catch (Exception ex)
            {

                return response.Send(400, "Error");
            }
        }

        public async Task<AppResponse<UserReponse>> Create(User user)
        {
            var response = new AppResponse<UserReponse>();
            try
            {
                if (!Helpers.ValidateEmailAndPhone(user.Email, user.PhoneNumber))
                {
                    return response.Send(404, "Email OR Phone Number Invalid");
                }
                var temp = await _userRepository
                    .FindBy(u =>
                    u.Email == user.Email ||
                    u.PhoneNumber == user.PhoneNumber)
                    .FirstOrDefaultAsync();
                if (temp != null)
                {
                    return response.Send(404, "Not Found User");
                }
                user.Id = Guid.NewGuid();

                await _userRepository.Create(user);
                UserReponse userReponse = _mapper.Map<UserReponse>(user);
                return response.Send(200, "Success", userReponse);
            }
            catch (Exception ex)
            {
                return response.Send(400, "Error");

            }
        }
    }
}
