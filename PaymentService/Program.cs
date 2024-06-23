using Microsoft.EntityFrameworkCore;
using PaymentService;
using PaymentService.Data;
using PaymentService.Models;
using PaymentService.Repositories;
using PaymentService.Services;
using RabbitMQ.Client;
using Shared.Helpers;
using Shared.Middleware;
using Shared.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.OperationFilter<CustomHeaderParameter>();
});

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

builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IPaymentService, PaymentService.Services.PaymentService>();
builder.Services.AddHttpClient<IIdentityService, IdentityService>();
builder.Services.AddScoped<ApiKeyAuthFilter>();
builder.Services.AddHttpClient();

var factory = new ConnectionFactory() { HostName = "localhost" }; // or your RabbitMQ host
var rabbitMqConnection = factory.CreateConnection();
builder.Services.AddSingleton<IConnection>(rabbitMqConnection);

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
