using IdentityService.Data;
using IdentityService.Middleware;
using IdentityService.Repositories;
using IdentityService.Services;
using Microsoft.EntityFrameworkCore;
using Shared;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddDbContext<AppDbContext>
        (
            options =>
            {
                options.UseSqlServer
                (
                    builder.Configuration.GetConnectionString("DefaultConnection")
                );
            }
        );
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<ICompanyService, CompanyService>();

var encryptionKey = "12345678901234567890123456789012";//builder.Configuration["ENCRYPTION_KEY"];
var encryptionIv = "1234567890123456";// builder.Configuration["ENCRYPTION_IV"];

if (string.IsNullOrEmpty(encryptionKey) || string.IsNullOrEmpty(encryptionIv))
    throw new InvalidOperationException("Encryption keys are not set in user secrets.");

var encryptionHelper = new EncryptionHelper(encryptionKey, encryptionIv);

builder.Services.AddSingleton(encryptionHelper);

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

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
