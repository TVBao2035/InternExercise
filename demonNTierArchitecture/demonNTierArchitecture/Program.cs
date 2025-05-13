
using demonNTierArchitecture.Data.Data;
using demonNTierArchitecture.Data.Repository.Implements;
using demonNTierArchitecture.Data.Repository.Interfaces;
using demonNTierArchitecture.Service.Mapper;
using demonNTierArchitecture.Service.Services.Implements;
using demonNTierArchitecture.Service.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace demonNTierArchitecture
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<ApplicationDBContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("Server"));
            });
            builder.Services.AddAutoMapper(typeof(AutoMapperConfiguration));
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
