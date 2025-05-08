using System.Linq.Expressions;
using myWebApi.Enity;

namespace myWebApi.Repository
{
    public interface IUserRepository
    {
        Task Create(User user);
        Task<List<User>> GetAll();
        Task<User> GetById(Guid id);
        Task Update(User user);
        Task Delete(User user);
        IQueryable<User> FindBy(Expression<Func<User, bool>> predicate);
    }
}
