using Microsoft.EntityFrameworkCore;
using PaymentService.Data;
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
            options.UseNpgsql(configuration["POSTGRES_CONNECTION_STRING"]);
        });

        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<IPaymentService, Services.PaymentService>();

        services.AddHttpClient<IIdentityService, IdentityService>();
        services.AddScoped<ApiKeyAuthFilter>();
        services.AddHttpClient();

        var factory = new ConnectionFactory()
        {
            HostName = configuration["RABBITMQ_HOST"],
            Port = int.Parse(configuration["RABBITMQ_PORT"]),
            UserName = configuration["RABBITMQ_USER"],
            Password = configuration["RABBITMQ_PASS"]
        };

        var rabbitMqConnection = factory.CreateConnection();
        services.AddSingleton(rabbitMqConnection);
    }
}
