using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using demonNTierArchitecture.Data.Data;
using demonNTierArchitecture.Data.Repository.Interfaces;
using demonNTierArchitecture.Models.Enities;

namespace demonNTierArchitecture.Data.Repository.Implements
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDBContext context) : base(context)
        {
        }
    }
}
