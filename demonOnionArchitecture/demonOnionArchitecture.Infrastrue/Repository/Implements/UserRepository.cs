using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using demonOnionArchitecture.Domain.Enities;
using demonOnionArchitecture.Infrastrue.Context;
using demonOnionArchitecture.Infrastrue.Repository.Interfaces;

namespace demonOnionArchitecture.Infrastrue.Repository.Implements
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
