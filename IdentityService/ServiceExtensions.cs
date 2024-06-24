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

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("PostgreSqlConnection"));
        });

        services.AddScoped<ICompanyRepository, CompanyRepository>();
        services.AddScoped<ICompanyService, CompanyService>();

        var encryptionKey = "12345678901234567890123456789012";//configuration["ENCRYPTION_KEY"];
        var encryptionIv = "1234567890123456";// configuration["ENCRYPTION_IV"];

        if (string.IsNullOrEmpty(encryptionKey) || string.IsNullOrEmpty(encryptionIv))
            throw new InvalidOperationException("Encryption keys are not set in user secrets.");

        var encryptionHelper = new EncryptionHelper(encryptionKey, encryptionIv);

        services.AddSingleton(encryptionHelper);
    }
}
