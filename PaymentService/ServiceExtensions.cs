using Microsoft.EntityFrameworkCore;
using PaymentService.Data;
using PaymentService.Models;
using PaymentService.Repositories;
using PaymentService.Services;
using RabbitMQ.Client;
using Shared.Helpers;
using Shared.Services;
using System.Reflection;

namespace PaymentService;

public static class ServiceExtensions
{
    public static void AddCustomServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("PostgreSqlConnection"));
        });

        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<IPaymentService, Services.PaymentService>();

        services.AddHttpClient<IIdentityService, IdentityService>();
        services.AddScoped<ApiKeyAuthFilter>();
        services.AddHttpClient();

        var factory = new ConnectionFactory()
        {
            HostName = "rabbitmq",
            Port = 5672,
            UserName = "guest",
            Password = "guest"
        };

        var rabbitMqConnection = factory.CreateConnection();
        services.AddSingleton(rabbitMqConnection);
    }
}
