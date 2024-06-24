using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.Repositories;
using OrderService.Services;
using Shared.Helpers;
using Shared.Services;
using System.Reflection;

namespace OrderService;

public static class ServiceExtensions
{
    public static void AddCustomServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("PostgreSqlConnection"));
        });

        services.AddHostedService<RabbitMqConsumer>(provider =>
        {
            var connectionString = configuration.GetConnectionString("RabbitMqConnection");
            var queueName = configuration.GetConnectionString("QueueName");
            var scopeFactory = provider.GetRequiredService<IServiceScopeFactory>();

            return new RabbitMqConsumer(connectionString, queueName, scopeFactory);
        });

        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IOrderService, OrderService.Services.OrderService>();
        services.AddHttpClient<IIdentityService, IdentityService>();
        services.AddScoped<ApiKeyAuthFilter>();

        services.AddRateLimiter(options =>
        {
            options.AddFixedWindowLimiter("Fixed", opt =>
            {
                opt.Window = TimeSpan.FromSeconds(3);
                opt.PermitLimit = 3;
            });
        });
    }
}
