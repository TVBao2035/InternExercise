using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using demonNTierArchitecture.Models.Enities;
using Microsoft.EntityFrameworkCore;

namespace demonNTierArchitecture.Data.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions options) : base(options)
        {
        }

        DbSet<User> Users { get; set; }

    }
}
