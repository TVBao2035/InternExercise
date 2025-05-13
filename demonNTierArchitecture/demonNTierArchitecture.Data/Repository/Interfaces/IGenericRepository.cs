using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace demonNTierArchitecture.Data.Repository.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        IQueryable<T> Query(Expression<Func<T, bool>> expression);
        IQueryable<T> Query();
    }
}
