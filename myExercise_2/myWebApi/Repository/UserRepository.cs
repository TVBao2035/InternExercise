using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using myWebApi.Data;
using myWebApi.Enity;
using myWebApi.Repository.GenericRepository;
using myWebApi.Repository.Interface;

namespace myWebApi.Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository 
    {
        public UserRepository(ApplicationDbContext context):base(context) { }
    }
}
