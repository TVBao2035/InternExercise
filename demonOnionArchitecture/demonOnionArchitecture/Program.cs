
using demonOnionArchitecture.Service.Mapper;
using demonOnionArchitecture.Infrastrue.Configuration;
using Microsoft.EntityFrameworkCore.Internal;
using NLog.Extensions.Logging;
using NLog.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;
using NLog;
namespace demonOnionArchitecture
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = LogManager.Setup()
                       .LoadConfigurationFromFile("NLog.config")
                       .GetCurrentClassLogger();
            var builder = WebApplication.CreateBuilder(args);
            logger.Info("App starting...");
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddAutoMapper(typeof(AutoMapperConfig));
            builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);
            builder.Logging.ClearProviders();
            builder.Host.UseNLog();


            builder.Services.RegisterDbContext(builder.Configuration);
            builder.Services.RegisterDI();
            builder.Services.AddSwaggerGen();
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
