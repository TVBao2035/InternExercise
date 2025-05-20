using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using demonOnionArchitecture.Common.Interfaces;
using demonOnionArchitecture.Infrastructure.Logging;
using demonOnionArchitecture.Infrastrue.Context;
using demonOnionArchitecture.Infrastrue.Repository.Implements;
using demonOnionArchitecture.Infrastrue.Repository.Interfaces;
using demonOnionArchitecture.Service.Services.Implements;
using demonOnionArchitecture.Service.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace demonOnionArchitecture.Infrastrue.Configuration
{
     public static class RegisterServices
    {
        public static void RegisterDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("Server"));
            });
        }

        public static void RegisterDI(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped(typeof(IAppLogger<>), typeof(NLogger<>));
        }
    }
}
