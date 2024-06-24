using IdentityService.Data;
using IdentityService.Repositories;
using IdentityService.Services;
using Microsoft.EntityFrameworkCore;
using Shared.Helpers;
using System.Reflection;

namespace IdentityService;

public static class ServiceExtensions
{
    public static void AddCustomServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        var connectionString = configuration["POSTGRES_CONNECTION_STRING"];

        Console.WriteLine(connectionString);

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        services.AddScoped<ICompanyRepository, CompanyRepository>();
        services.AddScoped<ICompanyService, CompanyService>();

        var encryptionKey = configuration["ENCRYPTIONKEY"];//"12345678901234567890123456789012";
        var encryptionIv = configuration["ENCRYPTIONIV"];// "1234567890123456";

        Console.WriteLine($"{encryptionKey} | {encryptionIv}");

        if (string.IsNullOrEmpty(encryptionKey) || string.IsNullOrEmpty(encryptionIv))
            throw new InvalidOperationException("Encryption keys are not set in user secrets.");

        var encryptionHelper = new EncryptionHelper(encryptionKey, encryptionIv);

        services.AddSingleton(encryptionHelper);
    }
}
