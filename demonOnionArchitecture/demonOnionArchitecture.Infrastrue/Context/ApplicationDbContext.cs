using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using demonOnionArchitecture.Domain.Enities;
using Microsoft.EntityFrameworkCore;

namespace demonOnionArchitecture.Infrastrue.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

    }
}
