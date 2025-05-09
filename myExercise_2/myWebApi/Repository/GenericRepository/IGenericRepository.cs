using myWebApi.Enity;
using System.Linq.Expressions;

namespace myWebApi.Repository.GenericRepository
{
    public interface IGenericRepository<T> where T : class
    {
        Task Create(T data);
        Task<List<T>> GetAll();
        Task Update(T data);
        Task Delete(T data);
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate= null);
    }
}
