
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using myWebApi.AutoMapper;
using myWebApi.Config;
using myWebApi.Data;
using myWebApi.Repository;
using myWebApi.Repository.GenericRepository;
using myWebApi.Repository.Interface;
using myWebApi.Services;
using myWebApi.Services.Interface;

namespace myWebApi
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


            //builder.Services.AddScoped<IUserRepository, UserRepository>();
            //builder.Services.AddScoped<IUserService, UserService>();
            //builder.Services.AddScoped<IProductRepository, ProductRepository>();
            //builder.Services.AddScoped<IProductService, ProductService>();
            //builder.Services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
            //builder.Services.AddScoped<IOrderDetailService, OrderDetailService>();
            //builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            //builder.Services.AddScoped<IOrderService, OrderService>();
          
            builder.Services.AddServices();
            builder.Services.AddAutoMapper(typeof(AutoMapperConfig));
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("Server"));
            });
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
