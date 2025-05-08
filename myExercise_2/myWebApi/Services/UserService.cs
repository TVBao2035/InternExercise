using Microsoft.EntityFrameworkCore;
using myWebApi.Common;
using myWebApi.Enity;
using myWebApi.Repository;

namespace myWebApi.Services
{
    public class UserService
    {
        private IUserRepository _userRepository;

        public UserService(IUserRepository userRepository) {
            _userRepository = userRepository;
        }



        public async Task<List<User>> GetAll()
        {
            try
            {
                var data = await _userRepository.GetAll();
                return data;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<User> Delete(Guid id)
        {
            try
            {
                var user = await _userRepository.GetById(id);
                if (user == null) return null;
                await _userRepository.Delete(user);
                return user;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<User> Update(User user)
        {
            try
            {
                if (!Helpers.ValidateEmailAndPhone(user.Email, user.PhoneNumber))
                {
                    return null;
                }
                User userMain = await _userRepository.GetById(user.Id);
                if(userMain == null)
                {
                    return null;
                }
                userMain.BirthDate = user.BirthDate;
                userMain.Name = user.Name;
                userMain.Gender = user.Gender;
                userMain.SchoolName = user.SchoolName;

                await _userRepository.Update(userMain);

                return userMain;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<User> Create(User user)
        {
            try
            {
                if (!Helpers.ValidateEmailAndPhone(user.Email, user.PhoneNumber))
                {
                    return null;
                }
                var temp = await _userRepository
                    .FindBy(u =>
                    u.Email == user.Email ||
                    u.PhoneNumber == user.PhoneNumber)
                    .FirstOrDefaultAsync();
                if (temp != null)
                {
                    return null;
                }
                user.Id = Guid.NewGuid();

                await _userRepository.Create(user);
                return user;
            }
            catch (Exception ex)
            {
                return null;
               
            }
        }
    }
}
