using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace demonOnionArchitecture.Infrastrue.Repository.Interfaces
{
    public interface IGenericRepository< T> where T : class
    {
        public void Add( T entity );
        public void Update( T entity );
        public void Delete( T entity );
        public IQueryable<T> Query(Expression<Func<T, bool>> expression );
        public IQueryable<T> Query();
    }
}
