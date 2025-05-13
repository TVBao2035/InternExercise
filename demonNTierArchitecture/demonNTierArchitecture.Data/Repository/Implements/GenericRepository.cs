using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using demonNTierArchitecture.Data.Data;
using demonNTierArchitecture.Data.Repository.Interfaces;

namespace demonNTierArchitecture.Data.Repository.Implements
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private ApplicationDBContext _context;

        public GenericRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
            _context.SaveChanges();
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            _context.SaveChanges();
        }

        public IQueryable<T> Query(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>().Where(expression).AsQueryable();
        }

        public IQueryable<T> Query()
        {
            return _context.Set<T>().AsQueryable();
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
            _context.SaveChanges();
        }
    }
}
