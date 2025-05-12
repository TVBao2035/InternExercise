using System.Reflection;
// Quá đã new knowledge
namespace myWebApi.Config
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();
            foreach (var type in assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && !t.IsGenericType))
            {
                var interfaceType = type.GetInterfaces()
                    .FirstOrDefault(i => i.Name == $"I{type.Name}");

                if (interfaceType != null )
                {
                    services.AddScoped(interfaceType, type);
                }
            }
            return services;
        } 
    }
}
