using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using myWebApi.Data;

namespace myWebApi.Repository.GenericRepository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private ApplicationDbContext _context;

        public GenericRepository(ApplicationDbContext context) {
            _context = context;
        }
        public async Task Create(T data)
        {
            _context.Set<T>().Add(data);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(T data)
        {
            _context.Set<T>().Remove(data); 
            await  _context.SaveChangesAsync();
        }

        public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate = null)
        {
            return _context.Set<T>().Where(predicate).AsQueryable();
        }

        public Task<List<T>> GetAll()
        {
            return _context.Set<T>().ToListAsync();
        }

        

        public async Task Update(T data)
        {
            _context.Set<T>().Update(data);
            await _context.SaveChangesAsync();
        }
    }
}
