using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using myWebApi.Data;
using myWebApi.Enity;

namespace myWebApi.Repository
{
    public class UserRepository : IUserRepository
    {
        private ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context) {
            _context = context;
        }
        public async Task Create(User user)
        {
            if(user is not  null)
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }
           
        }

        public async Task Delete(User user)
        {
            if(user is not null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
           
        }

        public async Task<List<User>> GetAll()
        {
            var userList = await _context.Users.ToListAsync();
            return userList;
        }

        public async Task<User> GetById(Guid id)
        {
            User user = await _context.Users.Where(u => u.Id == id).FirstOrDefaultAsync();
            return user;
        }

        public async Task Update(User user)
        {
           if(user is not null)
            {
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }
        }

        public IQueryable<User> FindBy(Expression<Func<User, bool>> predicate)
        {
            return _context.Users.Where(predicate).AsQueryable();
        }
    }
}
