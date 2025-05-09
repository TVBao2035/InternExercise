using System.Linq.Expressions;
using myWebApi.Enity;
using myWebApi.Repository.GenericRepository;

namespace myWebApi.Repository.Interface
{
    public interface IUserRepository:IGenericRepository<User>
    {
     
    }
}
